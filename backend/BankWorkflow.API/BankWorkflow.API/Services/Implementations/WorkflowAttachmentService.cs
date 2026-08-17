using BankWorkflow.API.Common.Mappers;
using BankWorkflow.API.DTOs.WorkflowAttachment;
using BankWorkflow.API.Repositories.Interfaces;
using BankWorkflow.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BankWorkflow.API.Services.Implementations;

public class WorkflowAttachmentService : IWorkflowAttachmentService
{
    private readonly IWorkflowAttachmentRepository _attachmentRepository;
    private readonly IWorkflowRequestRepository _workflowRequestRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IWebHostEnvironment _environment;

    private static readonly string[] AllowedExtensions =
    {
        ".pdf",
        ".doc",
        ".docx",
        ".png",
        ".jpg",
        ".jpeg"
    };

    private const long MaxFileSize = 10 * 1024 * 1024;

    public WorkflowAttachmentService(
        IWorkflowAttachmentRepository attachmentRepository,
        IWorkflowRequestRepository workflowRequestRepository,
        ICurrentUserService currentUser,
        IWebHostEnvironment environment)
    {
        _attachmentRepository = attachmentRepository;
        _workflowRequestRepository = workflowRequestRepository;
        _currentUser = currentUser;
        _environment = environment;
    }

    public async Task<WorkflowAttachmentDto> UploadAsync(
        int workflowRequestId,
        IFormFile file)
    {
        var request = await _workflowRequestRepository
            .GetByIdAsync(workflowRequestId);

        if (request is null)
            throw new InvalidOperationException(
                "Workflow request not found.");

        if (file is null || file.Length == 0)
            throw new InvalidOperationException(
                "A file is required.");

        if (file.Length > MaxFileSize)
            throw new InvalidOperationException(
                "File size cannot exceed 10 MB.");

        var extension = Path.GetExtension(file.FileName)
            .ToLowerInvariant();

        if (!AllowedExtensions.Contains(extension))
            throw new InvalidOperationException(
                "File type is not supported.");

        var uploadDirectory = Path.Combine(
            _environment.ContentRootPath,
            "uploads",
            "workflow-attachments");

        Directory.CreateDirectory(uploadDirectory);

        var storedFileName =
            $"{Guid.NewGuid()}{extension}";

        var filePath = Path.Combine(
            uploadDirectory,
            storedFileName);

        await using (var stream = new FileStream(
            filePath,
            FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var attachment = new Models.WorkflowAttachment
        {
            FileName = file.FileName,
            StoredFileName = storedFileName,
            ContentType = file.ContentType,
            FileSize = file.Length,
            FilePath = filePath,
            UploadedAt = DateTime.UtcNow,
            WorkflowRequestId = workflowRequestId,
            UploadedByUserId = _currentUser.UserId
        };

        await _attachmentRepository.AddAsync(attachment);
        await _attachmentRepository.SaveChangesAsync();

        var savedAttachment =
            await _attachmentRepository.GetByIdAsync(
                attachment.Id);

        return WorkflowAttachmentMapper.ToDto(
            savedAttachment!);
    }

    public async Task<List<WorkflowAttachmentDto>>
        GetByWorkflowRequestIdAsync(int workflowRequestId)
    {
        var request = await _workflowRequestRepository
            .GetByIdAsync(workflowRequestId);

        if (request is null)
            throw new InvalidOperationException(
                "Workflow request not found.");

        var attachments = await _attachmentRepository
            .GetByWorkflowRequestIdAsync(workflowRequestId);

        return attachments
            .Select(WorkflowAttachmentMapper.ToDto)
            .ToList();
    }

    public async Task<(byte[] FileBytes, string ContentType, string FileName)>
        DownloadAsync(int attachmentId)
    {
        var attachment = await _attachmentRepository
            .GetByIdAsync(attachmentId);

        if (attachment is null)
            throw new InvalidOperationException(
                "Attachment not found.");

        if (!File.Exists(attachment.FilePath))
            throw new InvalidOperationException(
                "Attachment file could not be found.");

        var fileBytes = await File.ReadAllBytesAsync(
            attachment.FilePath);

        return (
            fileBytes,
            attachment.ContentType,
            attachment.FileName);
    }
}