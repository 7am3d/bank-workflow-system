using BankWorkflow.API.Common;
using BankWorkflow.API.DTOs.WorkflowHistory;

namespace BankWorkflow.API.Services.Interfaces;

public interface IWorkflowHistoryService
{
    Task LogAsync(
        int workflowRequestId,
        WorkflowAction action,
        RequestStatus previousStatus,
        RequestStatus newStatus,
        string? details = null);

    Task<List<WorkflowHistoryDto>> GetHistoryAsync(int workflowRequestId);
}