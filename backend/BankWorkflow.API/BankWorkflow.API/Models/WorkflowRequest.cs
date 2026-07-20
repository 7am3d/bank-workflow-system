using BankWorkflow.API.Common;

namespace BankWorkflow.API.Models;

public class WorkflowRequest
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public RequestStatus Status { get; set; } = RequestStatus.Pending;

    public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;

    public int CurrentStep { get; set; } = 1;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Foreign Keys
    public int CreatedByUserId { get; set; }

    public int RequestTypeId { get; set; }

    // Navigation Properties
    public User CreatedByUser { get; set; } = null!;

    public RequestType RequestType { get; set; } = null!;

    public ICollection<WorkflowStep> WorkflowSteps { get; set; } = new List<WorkflowStep>();

    public ICollection<WorkflowComment> Comments { get; set; } = new List<WorkflowComment>();

    public ICollection<WorkflowHistory> HistoryEntries { get; set; } = new List<WorkflowHistory>();
}