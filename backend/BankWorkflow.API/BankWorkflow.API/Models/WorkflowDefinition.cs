namespace BankWorkflow.API.Models;

public class WorkflowDefinition
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public int RequestTypeId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public RequestType RequestType { get; set; } = null!;

    public ICollection<WorkflowStepDefinition> Steps { get; set; }
        = new List<WorkflowStepDefinition>();
}