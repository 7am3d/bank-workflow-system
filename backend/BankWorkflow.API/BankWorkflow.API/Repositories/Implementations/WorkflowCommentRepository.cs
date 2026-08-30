using BankWorkflow.API.Data;
using BankWorkflow.API.Models;
using BankWorkflow.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankWorkflow.API.Repositories.Implementations;

public class WorkflowCommentRepository : IWorkflowCommentRepository
{
    private readonly AppDbContext _context;

    public WorkflowCommentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<WorkflowComment>> GetByWorkflowRequestIdAsync(
        int workflowRequestId)
    {
        return await _context.WorkflowComments
            .Include(c => c.User)
            .Where(c => c.WorkflowRequestId == workflowRequestId)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(WorkflowComment comment)
    {
        await _context.WorkflowComments.AddAsync(comment);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}