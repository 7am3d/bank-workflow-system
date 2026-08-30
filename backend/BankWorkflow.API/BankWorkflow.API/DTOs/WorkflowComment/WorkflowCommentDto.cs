namespace BankWorkflow.API.DTOs.WorkflowComment;

public class WorkflowCommentDto
{
    public int Id { get; set; }

    public string Comment { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}