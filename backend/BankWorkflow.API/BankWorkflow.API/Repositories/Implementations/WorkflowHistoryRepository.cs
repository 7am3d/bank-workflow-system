using BankWorkflow.API.Data;
using BankWorkflow.API.Models;
using BankWorkflow.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankWorkflow.API.Repositories.Implementations;

public class WorkflowHistoryRepository : IWorkflowHistoryRepository
{
    private readonly AppDbContext _context;

    public WorkflowHistoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(WorkflowHistory history)
    {
        await _context.WorkflowHistory.AddAsync(history);
    }

    public async Task<List<WorkflowHistory>> GetByWorkflowRequestIdAsync(int workflowRequestId)
    {
        return await _context.WorkflowHistory
            .Include(h => h.User)
            .Where(h => h.WorkflowRequestId == workflowRequestId)
            .OrderBy(h => h.CreatedAt)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}