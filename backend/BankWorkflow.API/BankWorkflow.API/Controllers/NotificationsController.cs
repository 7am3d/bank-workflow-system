using BankWorkflow.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankWorkflow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(
        INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyNotifications()
    {
        return Ok(
            await _notificationService.GetMyNotificationsAsync());
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount()
    {
        return Ok(
            await _notificationService.GetUnreadCountAsync());
    }

    [HttpPut("{id:int}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        await _notificationService.MarkAsReadAsync(id);

        return NoContent();
    }

    [HttpGet("unread")]
    public async Task<IActionResult> GetUnread()
    {
        var notifications = await _notificationService
            .GetUnreadNotificationsAsync();

        return Ok(notifications);
    }

    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        await _notificationService.MarkAllAsReadAsync();

        return NoContent();
    }
}