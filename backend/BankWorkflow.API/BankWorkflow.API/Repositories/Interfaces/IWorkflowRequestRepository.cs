using BankWorkflow.API.Common;
using BankWorkflow.API.Models;

namespace BankWorkflow.API.Repositories.Interfaces;

public interface IWorkflowRequestRepository
{
    Task<List<WorkflowRequest>> GetAllAsync();

    Task<List<WorkflowRequest>> GetByUserIdAsync(int userId);

    Task<WorkflowRequest?> GetByIdAsync(int id);

    Task AddAsync(WorkflowRequest workflowRequest);

    Task<int> CountAsync();

    Task<int> CountByStatusAsync(RequestStatus status);

    Task<int> CountByCreatorAsync(int userId);

    Task<int> CountByCreatorAndStatusAsync(
        int userId,
        RequestStatus status);

    Task<List<WorkflowRequest>> GetRecentByCreatorAsync(
    int userId,
    int count);

    Task SaveChangesAsync();
}