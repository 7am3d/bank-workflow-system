namespace BankWorkflow.API.Models;

public class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Foreign Keys
    public int RoleId { get; set; }

    public int DepartmentId { get; set; }

    // Navigation Properties
    public Role Role { get; set; } = null!;

    public Department Department { get; set; } = null!;

    public ICollection<WorkflowRequest> CreatedRequests { get; set; } = new List<WorkflowRequest>();

    public ICollection<WorkflowComment> Comments { get; set; } = new List<WorkflowComment>();

    public ICollection<WorkflowHistory> HistoryEntries { get; set; } = new List<WorkflowHistory>();

    public ICollection<WorkflowStep> AssignedWorkflowSteps { get; set; } = new List<WorkflowStep>();
}