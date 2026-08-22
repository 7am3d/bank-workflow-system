using BankWorkflow.API.DTOs.WorkflowHistory;
using BankWorkflow.API.Models;

namespace BankWorkflow.API.Common.Mappers;

public static class WorkflowHistoryMapper
{
    public static WorkflowHistoryDto ToDto(WorkflowHistory history)
    {
        return new WorkflowHistoryDto
        {
            Id = history.Id,
            Action = history.Action.ToString(),
            PreviousStatus = history.PreviousStatus?.ToString(),
            NewStatus = history.NewStatus.ToString(),
            Details = history.Details,
            CreatedAt = history.CreatedAt,
            PerformedBy =
                $"{history.User.FirstName} {history.User.LastName}"
        };
    }
}