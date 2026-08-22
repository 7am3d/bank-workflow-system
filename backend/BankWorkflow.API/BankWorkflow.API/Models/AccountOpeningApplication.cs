namespace BankWorkflow.API.Models;

public class AccountOpeningApplication
{
    public int Id { get; set; }

    public int WorkflowRequestId { get; set; }

    public string CustomerReference { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public string AccountType { get; set; } = string.Empty;

    // Navigation Property
    public WorkflowRequest WorkflowRequest { get; set; } = null!;
}