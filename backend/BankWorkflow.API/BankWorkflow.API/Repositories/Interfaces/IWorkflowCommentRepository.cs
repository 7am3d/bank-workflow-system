using BankWorkflow.API.Models;

namespace BankWorkflow.API.Repositories.Interfaces;

public interface IWorkflowCommentRepository
{
    Task<List<WorkflowComment>> GetByWorkflowRequestIdAsync(int workflowRequestId);

    Task AddAsync(WorkflowComment comment);

    Task SaveChangesAsync();
}