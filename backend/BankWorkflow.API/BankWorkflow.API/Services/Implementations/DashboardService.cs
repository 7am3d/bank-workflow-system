using BankWorkflow.API.Common;
using BankWorkflow.API.DTOs.Dashboard;
using BankWorkflow.API.Repositories.Interfaces;
using BankWorkflow.API.Services.Interfaces;

namespace BankWorkflow.API.Services.Implementations;

public class DashboardService : IDashboardService
{
    private readonly IWorkflowRequestRepository _workflowRepository;
    private readonly IWorkflowStepRepository _workflowStepRepository;
    private readonly ICurrentUserService _currentUser;

    public DashboardService(
        IWorkflowRequestRepository workflowRepository,
        IWorkflowStepRepository workflowStepRepository,
        ICurrentUserService currentUser)
    {
        _workflowRepository = workflowRepository;
        _workflowStepRepository = workflowStepRepository;
        _currentUser = currentUser;
    }

    public async Task<DashboardDto> GetDashboardAsync()
    {
        var dashboard = new DashboardDto
        {
            MyRequests = await _workflowRepository.CountByCreatorAsync(
                _currentUser.UserId),

            MyPendingRequests = await _workflowRepository
                .CountByCreatorAndStatusAsync(
                    _currentUser.UserId,
                    RequestStatus.Pending),

            MyApprovedRequests = await _workflowRepository
                .CountByCreatorAndStatusAsync(
                    _currentUser.UserId,
                    RequestStatus.Approved),

            MyRejectedRequests = await _workflowRepository
                .CountByCreatorAndStatusAsync(
                    _currentUser.UserId,
                    RequestStatus.Rejected),

            PendingApprovals = await _workflowStepRepository
                .CountPendingApprovalsAsync(
                    _currentUser.UserId),

            TotalRequests = await _workflowRepository.CountAsync(),

            TotalPending = await _workflowRepository
                .CountByStatusAsync(RequestStatus.Pending),

            TotalApproved = await _workflowRepository
                .CountByStatusAsync(RequestStatus.Approved),

            TotalRejected = await _workflowRepository
                .CountByStatusAsync(RequestStatus.Rejected)
        };

        // Recent requests created by the current user
        var recentRequests = await _workflowRepository
            .GetRecentByCreatorAsync(_currentUser.UserId, 5);

        dashboard.RecentRequests = recentRequests
            .Select(r => new RecentRequestDto
            {
                Id = r.Id,
                Title = r.Title,
                Status = r.Status.ToString(),
                Priority = r.Priority.ToString(),
                CreatedAt = r.CreatedAt
            })
            .ToList();

        // Requests waiting for the current user's approval
        var pendingApprovals = await _workflowStepRepository
            .GetPendingApprovalsAsync(_currentUser.UserId, 5);

        dashboard.PendingApprovalRequests = pendingApprovals
            .Select(s => new PendingApprovalDto
            {
                Id = s.WorkflowRequest.Id,
                Title = s.WorkflowRequest.Title,
                RequestedBy = $"{s.WorkflowRequest.CreatedByUser.FirstName} {s.WorkflowRequest.CreatedByUser.LastName}",
                Priority = s.WorkflowRequest.Priority.ToString(),
                CreatedAt = s.WorkflowRequest.CreatedAt
            })
            .ToList();

        return dashboard;
    }
}