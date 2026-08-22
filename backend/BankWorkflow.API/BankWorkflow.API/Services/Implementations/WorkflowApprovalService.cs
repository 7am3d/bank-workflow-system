using BankWorkflow.API.Common;
using BankWorkflow.API.DTOs.Dashboard;
using BankWorkflow.API.Models;
using BankWorkflow.API.Repositories.Interfaces;
using BankWorkflow.API.Services.Interfaces;

namespace BankWorkflow.API.Services.Implementations;

public class WorkflowApprovalService : IWorkflowApprovalService
{
    private readonly IWorkflowStepRepository _workflowStepRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IWorkflowHistoryService _workflowHistoryService;
    private readonly INotificationService _notificationService;
    private readonly IWorkflowDefinitionService _workflowDefinitionService;
    public WorkflowApprovalService(
    IWorkflowStepRepository workflowStepRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUser,
    IWorkflowHistoryService workflowHistoryService,
    INotificationService notificationService,
    IWorkflowDefinitionService workflowDefinitionService)
    {
        _workflowStepRepository = workflowStepRepository;
        _userRepository = userRepository;
        _currentUser = currentUser;
        _workflowHistoryService = workflowHistoryService;
        _notificationService = notificationService;
        _workflowDefinitionService = workflowDefinitionService;
    }

    public async Task InitializeWorkflowAsync(WorkflowRequest workflowRequest)
    {
        var workflowDefinition =
            await _workflowDefinitionService.GetActiveWorkflowAsync(
                workflowRequest.RequestTypeId);

        if (workflowDefinition is null)
        {
            throw new InvalidOperationException(
                $"No active workflow definition found for request type ID {workflowRequest.RequestTypeId}.");
        }

        if (workflowDefinition.Steps.Count == 0)
        {
            throw new InvalidOperationException(
                $"Workflow definition '{workflowDefinition.Name}' has no steps.");
        }

        var workflowSteps = new List<WorkflowStep>();

        foreach (var stepDefinition in workflowDefinition.Steps
            .OrderBy(s => s.Sequence))
        {
            if (stepDefinition.ApproverType == WorkflowApproverType.Role)
            {
                if (!stepDefinition.RoleId.HasValue)
                {
                    throw new InvalidOperationException(
                        $"Workflow step {stepDefinition.Id} requires a role.");
                }

                var approver = await _userRepository
                    .GetFirstByRoleAsync(stepDefinition.RoleId.Value);

                if (approver is null)
                {
                    throw new InvalidOperationException(
                        $"No active approver found for role ID {stepDefinition.RoleId.Value}.");
                }

                workflowSteps.Add(new WorkflowStep
                {
                    WorkflowRequestId = workflowRequest.Id,
                    Sequence = stepDefinition.Sequence,
                    RoleId = stepDefinition.RoleId.Value,
                    ApproverUserId = approver.Id,
                    Status = RequestStatus.Pending
                });
            }
            else
            {
                throw new InvalidOperationException(
                    $"Approver type '{stepDefinition.ApproverType}' is not implemented yet.");
            }
        }

        await _workflowStepRepository.AddRangeAsync(workflowSteps);
        await _workflowStepRepository.SaveChangesAsync();
    }

    public async Task ApproveAsync(int workflowRequestId)
    {
        var currentStep = await _workflowStepRepository
            .GetCurrentPendingStepAsync(workflowRequestId);

        if (currentStep == null)
            throw new InvalidOperationException("No pending workflow step found.");

        if (currentStep.ApproverUserId != _currentUser.UserId)
        {
            throw new UnauthorizedAccessException(
                "You are not assigned to approve this workflow.");
        }

        var approverRole = currentStep.Role?.Name ?? "Approver";

        currentStep.Status = RequestStatus.Approved;
        currentStep.CompletedAt = DateTime.UtcNow;

        var request = currentStep.WorkflowRequest;

        if (request.CreatedByUserId == _currentUser.UserId)
        {
            throw new UnauthorizedAccessException(
                "You cannot approve your own workflow request.");
        }

        var nextStep = await _workflowStepRepository.GetNextStepAsync(
            workflowRequestId,
            currentStep.Sequence);

        if (nextStep != null)
        {
            request.CurrentStep++;
            request.Status = RequestStatus.Pending;

            if (nextStep.ApproverUserId.HasValue)
            {
                await _notificationService.CreateAsync(
                    nextStep.ApproverUserId.Value,
                    "Approval Required",
                    $"Workflow request '{request.Title}' requires your approval.",
                    "ApprovalRequired",
                    workflowRequestId);
            }
        }
        else
        {
            request.Status = RequestStatus.Approved;

            await _notificationService.CreateAsync(
                request.CreatedByUserId,
                "Request Approved",
                $"Your workflow request '{request.Title}' has been fully approved.",
                "Approved",
                workflowRequestId);
        }

        var newStatus = nextStep != null
            ? RequestStatus.Pending
            : RequestStatus.Approved;

        await _workflowStepRepository.SaveChangesAsync();

        await _workflowHistoryService.LogAsync(
            workflowRequestId,
            WorkflowAction.Approved,
            RequestStatus.Pending,
            newStatus,
            $"Step {currentStep.Sequence} approved by {approverRole}.");


    }

    public async Task RejectAsync(int workflowRequestId, string reason)
    {
        var currentStep = await _workflowStepRepository
            .GetCurrentPendingStepAsync(workflowRequestId);

        if (currentStep == null)
            throw new InvalidOperationException("No pending workflow step found.");

        if (currentStep.ApproverUserId != _currentUser.UserId)
        {
            throw new UnauthorizedAccessException(
                "You are not assigned to reject this workflow.");
        }

        var approverRole = currentStep.Role?.Name ?? "Approver";

        currentStep.Status = RequestStatus.Rejected;
        currentStep.CompletedAt = DateTime.UtcNow;

        var request = currentStep.WorkflowRequest;

        if (request.CreatedByUserId == _currentUser.UserId)
        {
            throw new UnauthorizedAccessException(
                "You cannot reject your own workflow request.");
        }

        request.Status = RequestStatus.Rejected;

        await _notificationService.CreateAsync(
            request.CreatedByUserId,
            "Request Rejected",
            $"Your workflow request '{request.Title}' has been rejected.",
            "Rejected",
            workflowRequestId);

        await _workflowStepRepository.SaveChangesAsync();

        await _workflowHistoryService.LogAsync(
            workflowRequestId,
            WorkflowAction.Rejected,
            RequestStatus.Pending,
            RequestStatus.Rejected,
            $"Step {currentStep.Sequence} rejected by {approverRole}. Reason: {reason}");
    }

    public async Task<List<PendingApprovalDto>> GetPendingApprovalsAsync()
    {
        var pendingApprovals =
            await _workflowStepRepository.GetAllPendingApprovalsAsync(
                _currentUser.UserId);

        return pendingApprovals
            .Select(s => new PendingApprovalDto
            {
                Id = s.WorkflowRequest.Id,
                Title = s.WorkflowRequest.Title,
                RequestedBy =
                    $"{s.WorkflowRequest.CreatedByUser.FirstName} {s.WorkflowRequest.CreatedByUser.LastName}",
                Priority = s.WorkflowRequest.Priority.ToString(),
                CreatedAt = s.WorkflowRequest.CreatedAt
            })
            .ToList();
    }
}