using BankWorkflow.API.DTOs.WorkflowComment;

namespace BankWorkflow.API.Services.Interfaces;

public interface IWorkflowCommentService
{
    Task<List<WorkflowCommentDto>> GetByWorkflowRequestIdAsync(int workflowRequestId);

    Task<WorkflowCommentDto> AddCommentAsync(
        int workflowRequestId,
        CreateWorkflowCommentDto dto);
}