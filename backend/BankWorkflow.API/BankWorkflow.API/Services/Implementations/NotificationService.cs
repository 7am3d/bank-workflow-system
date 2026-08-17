using BankWorkflow.API.DTOs.Notification;
using BankWorkflow.API.Models;
using BankWorkflow.API.Repositories.Interfaces;
using BankWorkflow.API.Services.Interfaces;

namespace BankWorkflow.API.Services.Implementations;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentUserService _currentUser;

    public NotificationService(
        INotificationRepository notificationRepository,
        ICurrentUserService currentUser)
    {
        _notificationRepository = notificationRepository;
        _currentUser = currentUser;
    }

    public async Task<List<NotificationDto>> GetMyNotificationsAsync()
    {
        var notifications = await _notificationRepository
            .GetByUserIdAsync(_currentUser.UserId);

        return notifications
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                Type = n.Type,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt,
                WorkflowRequestId = n.WorkflowRequestId
            })
            .ToList();
    }

    public async Task<List<NotificationDto>> GetUnreadNotificationsAsync()
    {
        var notifications = await _notificationRepository
            .GetByUserIdAsync(_currentUser.UserId);

        return notifications
            .Where(n => !n.IsRead)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                Type = n.Type,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt,
                WorkflowRequestId = n.WorkflowRequestId
            })
            .ToList();
    }

    public async Task<int> GetUnreadCountAsync()
    {
        return await _notificationRepository
            .GetUnreadCountAsync(_currentUser.UserId);
    }

    public async Task MarkAsReadAsync(int notificationId)
    {
        var notification = await _notificationRepository
            .GetByIdAsync(notificationId);

        if (notification is null)
            throw new InvalidOperationException(
                "Notification not found.");

        if (notification.UserId != _currentUser.UserId)
            throw new UnauthorizedAccessException(
                "You cannot access this notification.");

        if (notification.IsRead)
            return;

        notification.IsRead = true;

        await _notificationRepository.SaveChangesAsync();
    }

    public async Task MarkAllAsReadAsync()
    {
        var notifications = await _notificationRepository
            .GetByUserIdAsync(_currentUser.UserId);

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
        }

        await _notificationRepository.SaveChangesAsync();
    }

    public async Task CreateAsync(
    int userId,
    string title,
    string message,
    string type,
    int? workflowRequestId = null)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            Type = type,
            WorkflowRequestId = workflowRequestId,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        await _notificationRepository.SaveChangesAsync();
    }
}