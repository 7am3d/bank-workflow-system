using Microsoft.AspNetCore.Http;

namespace BankWorkflow.API.DTOs.WorkflowAttachment;

public class CreateWorkflowAttachmentDto
{
    public IFormFile File { get; set; } = null!;
}