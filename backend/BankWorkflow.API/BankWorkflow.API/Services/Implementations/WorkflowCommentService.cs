using BankWorkflow.API.Common;
using BankWorkflow.API.Common.Mappers;
using BankWorkflow.API.DTOs.WorkflowComment;
using BankWorkflow.API.Models;
using BankWorkflow.API.Repositories.Interfaces;
using BankWorkflow.API.Services.Interfaces;

namespace BankWorkflow.API.Services.Implementations;

public class WorkflowCommentService : IWorkflowCommentService
{
    private readonly IWorkflowCommentRepository _commentRepository;
    private readonly IWorkflowRequestRepository _workflowRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IWorkflowHistoryService _historyService;

    public WorkflowCommentService(
        IWorkflowCommentRepository commentRepository,
        IWorkflowRequestRepository workflowRepository,
        ICurrentUserService currentUser,
        IWorkflowHistoryService historyService)
    {
        _commentRepository = commentRepository;
        _workflowRepository = workflowRepository;
        _currentUser = currentUser;
        _historyService = historyService;
    }

    public async Task<List<WorkflowCommentDto>> GetByWorkflowRequestIdAsync(
        int workflowRequestId)
    {
        var comments = await _commentRepository
            .GetByWorkflowRequestIdAsync(workflowRequestId);

        return comments
            .Select(WorkflowCommentMapper.ToDto)
            .ToList();
    }

    public async Task<WorkflowCommentDto> AddCommentAsync(
        int workflowRequestId,
        CreateWorkflowCommentDto dto)
    {
        var request = await _workflowRepository.GetByIdAsync(workflowRequestId);

        if (request is null)
            throw new InvalidOperationException("Workflow request not found.");

        var comment = new WorkflowComment
        {
            WorkflowRequestId = workflowRequestId,
            UserId = _currentUser.UserId,
            Comment = dto.Comment
        };

        await _commentRepository.AddAsync(comment);
        await _commentRepository.SaveChangesAsync();

        await _historyService.LogAsync(
            workflowRequestId,
            WorkflowAction.CommentAdded,
            request.Status,
            request.Status,
            dto.Comment);

        var comments = await _commentRepository
            .GetByWorkflowRequestIdAsync(workflowRequestId);

        return WorkflowCommentMapper.ToDto(comments.Last());
    }
}