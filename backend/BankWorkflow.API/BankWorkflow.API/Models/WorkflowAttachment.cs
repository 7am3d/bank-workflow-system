namespace BankWorkflow.API.Models;

public class WorkflowAttachment
{
    public int Id { get; set; }

    public string FileName { get; set; } = null!;

    public string StoredFileName { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public long FileSize { get; set; }

    public string FilePath { get; set; } = null!;

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public int WorkflowRequestId { get; set; }

    public int UploadedByUserId { get; set; }

    // Navigation Properties
    public WorkflowRequest WorkflowRequest { get; set; } = null!;

    public User UploadedByUser { get; set; } = null!;
}