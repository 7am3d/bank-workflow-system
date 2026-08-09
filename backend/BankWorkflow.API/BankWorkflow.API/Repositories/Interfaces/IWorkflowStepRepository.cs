using BankWorkflow.API.Models;

namespace BankWorkflow.API.Repositories.Interfaces;

public interface IWorkflowStepRepository
{
    Task<List<WorkflowStep>> GetByRequestIdAsync(int workflowRequestId);

    Task<WorkflowStep?> GetCurrentStepAsync(int workflowRequestId, int sequence);

    Task AddRangeAsync(IEnumerable<WorkflowStep> steps);

    void Update(WorkflowStep workflowStep);

    Task<WorkflowStep?> GetCurrentPendingStepAsync(int workflowRequestId);

    Task<WorkflowStep?> GetNextStepAsync(int workflowRequestId, int currentSequence);

    Task<WorkflowStep?> GetByIdAsync(int id);

    Task<int> CountPendingApprovalsAsync(int userId);

    Task<List<WorkflowStep>> GetPendingApprovalsAsync(
    int userId,
    int count);

    Task SaveChangesAsync();
}