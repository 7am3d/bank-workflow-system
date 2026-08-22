using BankWorkflow.API.Models;

namespace BankWorkflow.API.Services.Interfaces;

public interface IWorkflowDefinitionService
{
    Task<WorkflowDefinition?> GetActiveWorkflowAsync(int requestTypeId);
}