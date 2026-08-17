using BankWorkflow.API.DTOs.WorkflowAttachment;
using Microsoft.AspNetCore.Http;

namespace BankWorkflow.API.Services.Interfaces;

public interface IWorkflowAttachmentService
{
    Task<WorkflowAttachmentDto> UploadAsync(
        int workflowRequestId,
        IFormFile file);

    Task<List<WorkflowAttachmentDto>> GetByWorkflowRequestIdAsync(
        int workflowRequestId);

    Task<(byte[] FileBytes, string ContentType, string FileName)> DownloadAsync(
        int attachmentId);
}