using BankWorkflow.API.DTOs.Dashboard;
using BankWorkflow.API.Models;

namespace BankWorkflow.API.Services.Interfaces;

public interface IWorkflowApprovalService
{
    Task InitializeWorkflowAsync(WorkflowRequest workflowRequest);

    Task ApproveAsync(int workflowRequestId);

    Task RejectAsync(int workflowRequestId, string reason);

    Task<List<PendingApprovalDto>> GetPendingApprovalsAsync();
}