using BankWorkflow.API.Common;

namespace BankWorkflow.API.DTOs.WorkflowHistory;

public class WorkflowHistoryDto
{
    public int Id { get; set; }

    public string Action { get; set; } = string.Empty;

    public string PreviousStatus { get; set; } = string.Empty;

    public string NewStatus { get; set; } = string.Empty;

    public string? Details { get; set; }

    public DateTime CreatedAt { get; set; }

    public string PerformedBy { get; set; } = string.Empty;
}