using BankWorkflow.API.DTOs.WorkflowComment;
using BankWorkflow.API.Models;

namespace BankWorkflow.API.Common.Mappers;

public static class WorkflowCommentMapper
{
    public static WorkflowCommentDto ToDto(WorkflowComment comment)
    {
        return new WorkflowCommentDto
        {
            Id = comment.Id,
            Comment = comment.Comment,
            CreatedAt = comment.CreatedAt,
            UserName = $"{comment.User.FirstName} {comment.User.LastName}"
        };
    }
}