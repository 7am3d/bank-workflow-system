namespace BankWorkflow.API.DTOs.WorkflowComment;

public class WorkflowCommentDto
{
    public int Id { get; set; }

    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public string User { get; set; } = string.Empty;
}