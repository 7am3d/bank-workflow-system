using BankWorkflow.API.Data;
using BankWorkflow.API.Models;
using BankWorkflow.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankWorkflow.API.Repositories.Implementations;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _context;

    public NotificationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Notification>> GetByUserIdAsync(int userId)
    {
        return await _context.Notifications
            .Include(n => n.WorkflowRequest)
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(int userId)
    {
        return await _context.Notifications
            .CountAsync(n =>
                n.UserId == userId &&
                !n.IsRead);
    }

    public async Task<Notification?> GetByIdAsync(int id)
    {
        return await _context.Notifications
            .Include(n => n.WorkflowRequest)
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task AddAsync(Notification notification)
    {
        await _context.Notifications.AddAsync(notification);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}