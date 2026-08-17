using BankWorkflow.API.Data;
using BankWorkflow.API.Models;
using BankWorkflow.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankWorkflow.API.Repositories.Implementations;

public class WorkflowAttachmentRepository
    : IWorkflowAttachmentRepository
{
    private readonly AppDbContext _context;

    public WorkflowAttachmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(WorkflowAttachment attachment)
    {
        await _context.WorkflowAttachments.AddAsync(attachment);
    }

    public async Task<WorkflowAttachment?> GetByIdAsync(int id)
    {
        return await _context.WorkflowAttachments
            .Include(a => a.UploadedByUser)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<WorkflowAttachment>>
        GetByWorkflowRequestIdAsync(int workflowRequestId)
    {
        return await _context.WorkflowAttachments
            .Include(a => a.UploadedByUser)
            .Where(a => a.WorkflowRequestId == workflowRequestId)
            .OrderByDescending(a => a.UploadedAt)
            .ToListAsync();
    }

    public Task DeleteAsync(WorkflowAttachment attachment)
    {
        _context.WorkflowAttachments.Remove(attachment);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}