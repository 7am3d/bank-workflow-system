using BankWorkflow.API.DTOs.WorkflowAttachment;
using BankWorkflow.API.Models;

namespace BankWorkflow.API.Common.Mappers;

public static class WorkflowAttachmentMapper
{
    public static WorkflowAttachmentDto ToDto(
        WorkflowAttachment attachment)
    {
        return new WorkflowAttachmentDto
        {
            Id = attachment.Id,
            FileName = attachment.FileName,
            ContentType = attachment.ContentType,
            FileSize = attachment.FileSize,
            UploadedAt = attachment.UploadedAt,
            UploadedBy =
                $"{attachment.UploadedByUser.FirstName} " +
                $"{attachment.UploadedByUser.LastName}"
        };
    }
}