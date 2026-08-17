using BankWorkflow.API.DTOs.Notification;

namespace BankWorkflow.API.Services.Interfaces;

public interface INotificationService
{
    Task<List<NotificationDto>> GetMyNotificationsAsync();

    Task<List<NotificationDto>> GetUnreadNotificationsAsync();

    Task<int> GetUnreadCountAsync();

    Task MarkAsReadAsync(int notificationId);

    Task MarkAllAsReadAsync();

    Task CreateAsync(
        int userId,
        string title,
        string message,
        string type,
        int? workflowRequestId = null);
}