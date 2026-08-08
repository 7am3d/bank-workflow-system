using BankWorkflow.API.Common;
using BankWorkflow.API.Models;
using BankWorkflow.API.Repositories.Interfaces;
using BankWorkflow.API.Services.Interfaces;
using BankWorkflow.API.Common.Mappers;
using BankWorkflow.API.DTOs.WorkflowHistory;

namespace BankWorkflow.API.Services.Implementations;

public class WorkflowHistoryService : IWorkflowHistoryService
{
    private readonly IWorkflowHistoryRepository _historyRepository;
    private readonly ICurrentUserService _currentUser;

    public WorkflowHistoryService(
        IWorkflowHistoryRepository historyRepository,
        ICurrentUserService currentUser)
    {
        _historyRepository = historyRepository;
        _currentUser = currentUser;
    }

    public async Task LogAsync(
        int workflowRequestId,
        WorkflowAction action,
        RequestStatus previousStatus,
        RequestStatus newStatus,
        string? details = null)
    {
        var history = new WorkflowHistory
        {
            WorkflowRequestId = workflowRequestId,
            UserId = _currentUser.UserId,
            Action = action,
            PreviousStatus = previousStatus,
            NewStatus = newStatus,
            Details = details,
            CreatedAt = DateTime.UtcNow
        };

        await _historyRepository.AddAsync(history);
        await _historyRepository.SaveChangesAsync();
    }

    public async Task<List<WorkflowHistoryDto>> GetHistoryAsync(int workflowRequestId)
    {
        var history = await _historyRepository
            .GetByWorkflowRequestIdAsync(workflowRequestId);

        return history
            .Select(WorkflowHistoryMapper.ToDto)
            .ToList();
    }
}