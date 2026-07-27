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

    public WorkflowRequestService(
        IWorkflowRequestRepository workflowRepository,
        IUserRepository userRepository,
        IRequestTypeRepository requestTypeRepository,
        ICurrentUserService currentUser)
    {
        _workflowRepository = workflowRepository;
        _userRepository = userRepository;
        _requestTypeRepository = requestTypeRepository;
        _currentUser = currentUser;
    }

    public async Task<List<WorkflowRequestDto>> GetAllAsync()
    {
        var requests = await _workflowRepository.GetAllAsync();

        return requests
            .Select(WorkflowRequestMapper.ToDto)
            .ToList();
    }

    public async Task<List<WorkflowRequestDto>> GetMyRequestsAsync()
    {
        var requests = await _workflowRepository.GetByUserIdAsync(_currentUser.UserId);

        return requests
            .Select(WorkflowRequestMapper.ToDto)
            .ToList();
    }

    public async Task<WorkflowRequestDto?> GetByIdAsync(int id)
    {
        var request = await _workflowRepository.GetByIdAsync(id);

        if (request is null)
            return null;

        return WorkflowRequestMapper.ToDto(request);
    }

    public async Task<WorkflowRequestDto> CreateAsync(CreateWorkflowRequestDto dto)
    {
        var requestType = await _requestTypeRepository.GetByIdAsync(dto.RequestTypeId);

        if (requestType is null)
            throw new InvalidOperationException("Request type not found.");

        var createdBy = await _userRepository.GetByIdAsync(_currentUser.UserId);

        if (createdBy is null)
            throw new InvalidOperationException("User not found.");

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

        return WorkflowRequestMapper.ToDto(request);
    }
}