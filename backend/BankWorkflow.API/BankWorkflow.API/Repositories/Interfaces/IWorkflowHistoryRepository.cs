using BankWorkflow.API.Models;

namespace BankWorkflow.API.Repositories.Interfaces;

public interface IWorkflowHistoryRepository
{
    Task AddAsync(WorkflowHistory history);

    Task<List<WorkflowHistory>> GetByWorkflowRequestIdAsync(int workflowRequestId);

    Task SaveChangesAsync();
}