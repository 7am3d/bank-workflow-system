using System.ComponentModel.DataAnnotations;

namespace BankWorkflow.API.DTOs.WorkflowRequest;

public class RejectWorkflowRequestDto
{
    [Required]
    [StringLength(1000)]
    public string Reason { get; set; } = string.Empty;
}