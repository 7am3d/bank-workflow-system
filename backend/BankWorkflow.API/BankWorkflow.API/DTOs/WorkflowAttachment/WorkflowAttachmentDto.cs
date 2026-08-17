namespace BankWorkflow.API.DTOs.WorkflowAttachment;

public class WorkflowAttachmentDto
{
    public int Id { get; set; }

    public string FileName { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public long FileSize { get; set; }

    public DateTime UploadedAt { get; set; }

    public string UploadedBy { get; set; } = null!;
}