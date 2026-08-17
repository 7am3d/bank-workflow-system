using BankWorkflow.API.Models;

namespace BankWorkflow.API.Repositories.Interfaces;

public interface IWorkflowAttachmentRepository
{
    Task AddAsync(WorkflowAttachment attachment);

    Task<WorkflowAttachment?> GetByIdAsync(int id);

    Task<List<WorkflowAttachment>> GetByWorkflowRequestIdAsync(
        int workflowRequestId);

    Task DeleteAsync(WorkflowAttachment attachment);

    Task SaveChangesAsync();
}