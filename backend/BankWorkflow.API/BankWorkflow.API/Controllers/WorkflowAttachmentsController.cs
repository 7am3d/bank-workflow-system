using BankWorkflow.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankWorkflow.API.Controllers;

[ApiController]
[Authorize]
public class WorkflowAttachmentsController : ControllerBase
{
    private readonly IWorkflowAttachmentService _attachmentService;

    public WorkflowAttachmentsController(
        IWorkflowAttachmentService attachmentService)
    {
        _attachmentService = attachmentService;
    }

    [HttpPost("api/WorkflowRequests/{workflowRequestId}/attachments")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(
        int workflowRequestId,
        IFormFile file)
    {
        var attachment = await _attachmentService.UploadAsync(
            workflowRequestId,
            file);

        return Ok(attachment);
    }

    [HttpGet("api/WorkflowRequests/{workflowRequestId}/attachments")]
    public async Task<IActionResult> GetByWorkflowRequestId(
        int workflowRequestId)
    {
        var attachments = await _attachmentService
            .GetByWorkflowRequestIdAsync(workflowRequestId);

        return Ok(attachments);
    }

    [HttpGet("api/WorkflowAttachments/{attachmentId}/download")]
    public async Task<IActionResult> Download(int attachmentId)
    {
        var result = await _attachmentService
            .DownloadAsync(attachmentId);

        return File(
            result.FileBytes,
            result.ContentType,
            result.FileName);
    }
}