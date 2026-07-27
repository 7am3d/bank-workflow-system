using BankWorkflow.API.Models;

namespace BankWorkflow.API.Repositories.Interfaces;

public interface IWorkflowRequestRepository
{
    Task<List<WorkflowRequest>> GetAllAsync();

    Task<List<WorkflowRequest>> GetByUserIdAsync(int userId);

    Task<WorkflowRequest?> GetByIdAsync(int id);

    Task AddAsync(WorkflowRequest workflowRequest);

    Task SaveChangesAsync();
}