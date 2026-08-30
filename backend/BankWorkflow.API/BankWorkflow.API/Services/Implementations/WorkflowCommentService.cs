using BankWorkflow.API.DTOs.WorkflowComment;
using BankWorkflow.API.Models;
using BankWorkflow.API.Repositories.Interfaces;
using BankWorkflow.API.Services.Interfaces;

namespace BankWorkflow.API.Services.Implementations;

public class WorkflowCommentService : IWorkflowCommentService
{
    private readonly IWorkflowCommentRepository _commentRepository;
    private readonly IWorkflowRequestRepository _workflowRequestRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUser;

    public WorkflowCommentService(
        IWorkflowCommentRepository commentRepository,
        IWorkflowRequestRepository workflowRequestRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUser)
    {
        _commentRepository = commentRepository;
        _workflowRequestRepository = workflowRequestRepository;
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public async Task<List<WorkflowCommentDto>>
        GetByWorkflowRequestIdAsync(int workflowRequestId)
    {
        var request = await _workflowRequestRepository
            .GetByIdAsync(workflowRequestId);

        if (request is null)
        {
            throw new InvalidOperationException(
                "Workflow request not found.");
        }

        var comments = await _commentRepository
            .GetByWorkflowRequestIdAsync(workflowRequestId);

        return comments
            .Select(c => new WorkflowCommentDto
            {
                Id = c.Id,
                Comment = c.Comment,
                UserName = $"{c.User.FirstName} {c.User.LastName}",
                CreatedAt = c.CreatedAt
            })
            .ToList();
    }

    public async Task<WorkflowCommentDto> AddCommentAsync(
        int workflowRequestId,
        CreateWorkflowCommentDto dto)
    {
        var request = await _workflowRequestRepository
            .GetByIdAsync(workflowRequestId);

        if (request is null)
        {
            throw new InvalidOperationException(
                "Workflow request not found.");
        }

        if (string.IsNullOrWhiteSpace(dto.Comment))
        {
            throw new InvalidOperationException(
                "Comment cannot be empty.");
        }

        var user = await _userRepository
            .GetByIdAsync(_currentUser.UserId);

        if (user is null)
        {
            throw new InvalidOperationException(
                "User not found.");
        }

        var comment = new WorkflowComment
        {
            WorkflowRequestId = workflowRequestId,
            UserId = _currentUser.UserId,
            Comment = dto.Comment.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        await _commentRepository.AddAsync(comment);
        await _commentRepository.SaveChangesAsync();

        return new WorkflowCommentDto
        {
            Id = comment.Id,
            Comment = comment.Comment,
            UserName = $"{user.FirstName} {user.LastName}",
            CreatedAt = comment.CreatedAt
        };
    }
}