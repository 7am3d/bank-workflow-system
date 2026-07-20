using BankWorkflow.API.Common;

namespace BankWorkflow.API.Models;

public class WorkflowHistory
{
    public int Id { get; set; }

    public WorkflowAction Action { get; set; }

    public RequestStatus PreviousStatus { get; set; }

    public RequestStatus NewStatus { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public int WorkflowRequestId { get; set; }

    public int UserId { get; set; }

    // Navigation Properties
    public WorkflowRequest WorkflowRequest { get; set; } = null!;

    public User User { get; set; } = null!;
}