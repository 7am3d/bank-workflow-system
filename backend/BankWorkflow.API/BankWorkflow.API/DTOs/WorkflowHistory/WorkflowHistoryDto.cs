using BankWorkflow.API.Common;

namespace BankWorkflow.API.DTOs.WorkflowHistory;

public class WorkflowHistoryDto
{
    public int Id { get; set; }

    public WorkflowAction Action { get; set; }

    public RequestStatus PreviousStatus { get; set; }

    public RequestStatus NewStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public string PerformedBy { get; set; } = string.Empty;
}