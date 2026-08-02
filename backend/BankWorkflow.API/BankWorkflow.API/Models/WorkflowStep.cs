using BankWorkflow.API.Common;

namespace BankWorkflow.API.Models;

public class WorkflowStep
{
    public int Id { get; set; }

    public int Sequence { get; set; }

    public RequestStatus Status { get; set; } = RequestStatus.Pending;

    public DateTime? CompletedAt { get; set; }

    // Foreign Keys
    public int WorkflowRequestId { get; set; }

    public int RoleId { get; set; }

    public int? ApproverUserId { get; set; }

    // Navigation Properties
    public WorkflowRequest WorkflowRequest { get; set; } = null!;

    public Role Role { get; set; } = null!;

    public User? ApproverUser { get; set; }
}