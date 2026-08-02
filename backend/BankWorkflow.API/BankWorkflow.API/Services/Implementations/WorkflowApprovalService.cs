using BankWorkflow.API.Common;
using BankWorkflow.API.Models;
using BankWorkflow.API.Repositories.Interfaces;
using BankWorkflow.API.Services.Interfaces;

namespace BankWorkflow.API.Services.Implementations;

public class WorkflowApprovalService : IWorkflowApprovalService
{
    private readonly IWorkflowStepRepository _workflowStepRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUser;
    public WorkflowApprovalService(
    IWorkflowStepRepository workflowStepRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUser)
    {
        _workflowStepRepository = workflowStepRepository;
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public async Task InitializeWorkflowAsync(WorkflowRequest workflowRequest)
    {
        var approvalChain = new[]
        {
            new { Sequence = 1, RoleId = RoleIds.Supervisor },
            new { Sequence = 2, RoleId = RoleIds.Manager },
            new { Sequence = 3, RoleId = RoleIds.Director }
        };

        var workflowSteps = new List<WorkflowStep>();

        foreach (var step in approvalChain)
        {
            var approver = await _userRepository.GetFirstByRoleAsync(step.RoleId);

            if (approver is null)
            {
                throw new InvalidOperationException(
                    $"No active approver found for role ID {step.RoleId}.");
            }

            workflowSteps.Add(new WorkflowStep
            {
                WorkflowRequestId = workflowRequest.Id,
                Sequence = step.Sequence,
                RoleId = step.RoleId,
                ApproverUserId = approver.Id,
                Status = RequestStatus.Pending
            });
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

        currentStep.Status = RequestStatus.Approved;
        currentStep.CompletedAt = DateTime.UtcNow;

        var request = currentStep.WorkflowRequest;

        var nextStep = await _workflowStepRepository.GetNextStepAsync(
            workflowRequestId,
            currentStep.Sequence);

        if (nextStep != null)
        {
            request.CurrentStep++;
            request.Status = RequestStatus.Pending;
        }
        else
        {
            request.Status = RequestStatus.Approved;
        }

        await _workflowStepRepository.SaveChangesAsync();
    }

    public Task RejectAsync(int workflowRequestId)
    {
        throw new NotImplementedException();
    }
}