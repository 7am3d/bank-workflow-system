namespace BankWorkflow.API.DTOs.WorkflowRequest;

public class WorkflowStepDto
{
    public int Sequence { get; set; }

    public string Status { get; set; } = string.Empty;

    public string ApproverRole { get; set; } = string.Empty;

    public string? ApproverName { get; set; }
}