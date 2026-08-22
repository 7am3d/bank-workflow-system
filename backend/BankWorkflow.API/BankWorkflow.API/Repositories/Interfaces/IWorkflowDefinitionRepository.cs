using BankWorkflow.API.Models;

namespace BankWorkflow.API.Repositories.Interfaces;

public interface IWorkflowDefinitionRepository
{
    Task<WorkflowDefinition?> GetActiveByRequestTypeIdAsync(int requestTypeId);
}