namespace BankWorkflow.API.DTOs.Dashboard;

public class PendingApprovalDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string RequestedBy { get; set; } = string.Empty;

    public string Priority { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}