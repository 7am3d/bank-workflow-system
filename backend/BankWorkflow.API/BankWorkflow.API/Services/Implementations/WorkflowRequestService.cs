using BankWorkflow.API.Common;
using BankWorkflow.API.Common.Mappers;
using BankWorkflow.API.DTOs.WorkflowRequest;
using BankWorkflow.API.Models;
using BankWorkflow.API.Repositories.Interfaces;
using BankWorkflow.API.Services.Interfaces;

namespace BankWorkflow.API.Services.Implementations;

public class WorkflowRequestService : IWorkflowRequestService
{
    private readonly IWorkflowRequestRepository _workflowRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRequestTypeRepository _requestTypeRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IWorkflowApprovalService _workflowApprovalService;
    private readonly IWorkflowHistoryService _workflowHistoryService;
    private readonly IWorkflowStepRepository _workflowStepRepository;

    public WorkflowRequestService(
        IWorkflowRequestRepository workflowRepository,
        IUserRepository userRepository,
        IRequestTypeRepository requestTypeRepository,
        ICurrentUserService currentUser,
        IWorkflowApprovalService workflowApprovalService,
        IWorkflowHistoryService workflowHistoryService,
        IWorkflowStepRepository workflowStepRepository)
    {
        _workflowRepository = workflowRepository;
        _userRepository = userRepository;
        _requestTypeRepository = requestTypeRepository;
        _currentUser = currentUser;
        _workflowApprovalService = workflowApprovalService;
        _workflowHistoryService = workflowHistoryService;
        _workflowStepRepository = workflowStepRepository;
    }

    public async Task<List<WorkflowRequestDto>> GetAllAsync(
        WorkflowRequestFilterDto filter)
    {
        var requests = await _workflowRepository
            .GetFilteredAsync(filter);

        return requests
            .Select(WorkflowRequestMapper.ToDto)
            .ToList();
    }

    public async Task<List<WorkflowRequestDto>> GetMyRequestsAsync()
    {
        var requests = await _workflowRepository
            .GetByUserIdAsync(_currentUser.UserId);

        return requests
            .Select(WorkflowRequestMapper.ToDto)
            .ToList();
    }

    public async Task<WorkflowRequestDto?> GetByIdAsync(int id)
    {
        var request = await _workflowRepository.GetByIdAsync(id);

        if (request is null)
            return null;

        var dto = WorkflowRequestMapper.ToDto(request);

        var workflowSteps =
            await _workflowStepRepository.GetByRequestIdAsync(id);

        dto.WorkflowSteps = workflowSteps
            .Select(step => new WorkflowStepDto
            {
                Sequence = step.Sequence,
                Status = step.Status.ToString(),
                ApproverRole = step.Role?.Name ?? "Employee",
                ApproverName = step.ApproverUser is null
                    ? null
                    : $"{step.ApproverUser.FirstName} {step.ApproverUser.LastName}"
            })
            .ToList();

        if (request.Status == RequestStatus.Pending)
        {
            var currentStep =
                await _workflowStepRepository
                    .GetCurrentPendingStepAsync(id);

            dto.CanCurrentUserAct =
                request.CreatedByUserId != _currentUser.UserId &&
                currentStep?.ApproverUserId == _currentUser.UserId;
        }

        return dto;
    }

    public async Task<WorkflowRequestDto> CreateAsync(
        CreateWorkflowRequestDto dto)
    {
        var requestType =
            await _requestTypeRepository.GetByIdAsync(dto.RequestTypeId);

        if (requestType is null)
            throw new InvalidOperationException(
                "Request type not found.");

        var createdBy =
            await _userRepository.GetByIdAsync(_currentUser.UserId);

        if (createdBy is null)
            throw new InvalidOperationException(
                "User not found.");

        var request = new WorkflowRequest
        {
            Title = dto.Title,
            Description = dto.Description,
            RequestTypeId = dto.RequestTypeId,

            CreatedByUserId = _currentUser.UserId,

            Status = RequestStatus.Pending,
            Priority = dto.Priority,
            CurrentStep = 1,

            CreatedAt = DateTime.UtcNow,

            RequestType = requestType,
            CreatedByUser = createdBy
        };

        await _workflowRepository.AddAsync(request);
        await _workflowRepository.SaveChangesAsync();

        await _workflowApprovalService.InitializeWorkflowAsync(request);

        await _workflowHistoryService.LogAsync(
            request.Id,
            WorkflowAction.Created,
            null,
            RequestStatus.Pending);

        return WorkflowRequestMapper.ToDto(request);
    }
}