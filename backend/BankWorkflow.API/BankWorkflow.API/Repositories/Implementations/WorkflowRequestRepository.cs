using BankWorkflow.API.Data;
using BankWorkflow.API.Models;
using BankWorkflow.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankWorkflow.API.Repositories.Implementations;

public class WorkflowRequestRepository : IWorkflowRequestRepository
{
    private readonly AppDbContext _context;

    public WorkflowRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<WorkflowRequest>> GetAllAsync()
    {
        return await _context.WorkflowRequests
            .Include(r => r.CreatedByUser)
            .Include(r => r.RequestType)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<WorkflowRequest>> GetByUserIdAsync(int userId)
    {
        return await _context.WorkflowRequests
            .Include(r => r.CreatedByUser)
            .Include(r => r.RequestType)
            .Where(r => r.CreatedByUserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<WorkflowRequest?> GetByIdAsync(int id)
    {
        return await _context.WorkflowRequests
            .Include(r => r.CreatedByUser)
            .Include(r => r.RequestType)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task AddAsync(WorkflowRequest workflowRequest)
    {
        await _context.WorkflowRequests.AddAsync(workflowRequest);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}