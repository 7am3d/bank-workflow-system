using BankWorkflow.API.Models;

namespace BankWorkflow.API.Repositories.Interfaces;

public interface INotificationRepository
{
    Task<List<Notification>> GetByUserIdAsync(int userId);

    Task<int> GetUnreadCountAsync(int userId);

    Task<Notification?> GetByIdAsync(int id);

    Task AddAsync(Notification notification);

    Task SaveChangesAsync();
}