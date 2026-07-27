using BankWorkflow.API.Common;
using System.ComponentModel.DataAnnotations;

namespace BankWorkflow.API.DTOs.WorkflowRequest;

public class CreateWorkflowRequestDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int RequestTypeId { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(2000, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public PriorityLevel Priority { get; set; }
}