using System.ComponentModel.DataAnnotations;

namespace BankWorkflow.API.DTOs.WorkflowComment;

public class CreateWorkflowCommentDto
{
    [Required]
    [MaxLength(2000)]
    public string Comment { get; set; } = string.Empty;
}