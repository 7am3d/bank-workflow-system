using BankWorkflow.API.DTOs.WorkflowRequest;

namespace BankWorkflow.API.Services.Interfaces;

public interface IWorkflowRequestService
{
    Task<List<WorkflowRequestDto>> GetAllAsync();

    Task<List<WorkflowRequestDto>> GetMyRequestsAsync();

    Task<WorkflowRequestDto?> GetByIdAsync(int id);

    Task<WorkflowRequestDto> CreateAsync(CreateWorkflowRequestDto request);
}