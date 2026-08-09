namespace BankWorkflow.API.DTOs.Dashboard;

public class DashboardDto
{
    public int MyRequests { get; set; }

    public int MyPendingRequests { get; set; }

    public int MyApprovedRequests { get; set; }

    public int MyRejectedRequests { get; set; }

    public int PendingApprovals { get; set; }

    public int TotalRequests { get; set; }

    public int TotalPending { get; set; }

    public int TotalApproved { get; set; }

    public int TotalRejected { get; set; }

    public List<RecentRequestDto> RecentRequests { get; set; } = [];

    public List<PendingApprovalDto> PendingApprovalRequests { get; set; } = new();
}