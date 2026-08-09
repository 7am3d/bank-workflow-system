using BankWorkflow.API.DTOs.Dashboard;

namespace BankWorkflow.API.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardAsync();
}