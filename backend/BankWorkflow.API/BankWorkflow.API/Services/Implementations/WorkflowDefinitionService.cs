using BankWorkflow.API.Models;
using BankWorkflow.API.Repositories.Interfaces;
using BankWorkflow.API.Services.Interfaces;

namespace BankWorkflow.API.Services.Implementations;

public class WorkflowDefinitionService : IWorkflowDefinitionService
{
    private readonly IWorkflowDefinitionRepository _repository;

    public WorkflowDefinitionService(
        IWorkflowDefinitionRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorkflowDefinition?> GetActiveWorkflowAsync(
        int requestTypeId)
    {
        return await _repository
            .GetActiveByRequestTypeIdAsync(requestTypeId);
    }
}