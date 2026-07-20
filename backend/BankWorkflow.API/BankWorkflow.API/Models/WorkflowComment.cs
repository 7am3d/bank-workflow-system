namespace BankWorkflow.API.Models;

public class WorkflowComment
{
    public int Id { get; set; }

    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public int WorkflowRequestId { get; set; }

    public int UserId { get; set; }

    // Navigation Properties
    public WorkflowRequest WorkflowRequest { get; set; } = null!;

    public User User { get; set; } = null!;
}