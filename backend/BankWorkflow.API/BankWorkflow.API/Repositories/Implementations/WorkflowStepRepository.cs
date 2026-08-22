using BankWorkflow.API.Common;
using BankWorkflow.API.Data;
using BankWorkflow.API.Models;
using BankWorkflow.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankWorkflow.API.Repositories.Implementations;

public class WorkflowStepRepository : IWorkflowStepRepository
{
    private readonly AppDbContext _context;

    public WorkflowStepRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<WorkflowStep>> GetByRequestIdAsync(int workflowRequestId)
    {
        return await _context.WorkflowSteps
            .Include(s => s.Role)
            .Include(s => s.ApproverUser)
            .Where(s => s.WorkflowRequestId == workflowRequestId)
            .OrderBy(s => s.Sequence)
            .ToListAsync();
    }

    public async Task<WorkflowStep?> GetCurrentStepAsync(int workflowRequestId, int sequence)
    {
        return await _context.WorkflowSteps
            .Include(s => s.Role)
            .Include(s => s.ApproverUser)
            .FirstOrDefaultAsync(s =>
                s.WorkflowRequestId == workflowRequestId &&
                s.Sequence == sequence);
    }

    public async Task AddRangeAsync(IEnumerable<WorkflowStep> steps)
    {
        await _context.WorkflowSteps.AddRangeAsync(steps);
    }

    public void Update(WorkflowStep workflowStep)
    {
        _context.WorkflowSteps.Update(workflowStep);
    }

    public async Task<WorkflowStep?> GetCurrentPendingStepAsync(int workflowRequestId)
    {
        return await _context.WorkflowSteps
            .Include(ws => ws.WorkflowRequest)
            .Include(ws => ws.Role)
            .FirstOrDefaultAsync(ws =>
                ws.WorkflowRequestId == workflowRequestId &&
                ws.Status == RequestStatus.Pending);
    }

    public async Task<WorkflowStep?> GetByIdAsync(int id)
    {
        return await _context.WorkflowSteps
            .Include(ws => ws.WorkflowRequest)
            .FirstOrDefaultAsync(ws => ws.Id == id);
    }

    public async Task<WorkflowStep?> GetNextStepAsync(
    int workflowRequestId,
    int currentSequence)
    {
        return await _context.WorkflowSteps
            .FirstOrDefaultAsync(ws =>
                ws.WorkflowRequestId == workflowRequestId &&
                ws.Sequence == currentSequence + 1);
    }

    public async Task<int> CountPendingApprovalsAsync(int userId)
    {
        return await _context.WorkflowSteps.CountAsync(step =>
            step.ApproverUserId == userId &&
            step.Status == RequestStatus.Pending &&
            step.Sequence == step.WorkflowRequest.CurrentStep &&
            step.WorkflowRequest.Status == RequestStatus.Pending);
    }

    public async Task<List<WorkflowStep>> GetPendingApprovalsAsync(
    int userId,
    int count)
    {
        return await _context.WorkflowSteps
            .Include(s => s.WorkflowRequest)
                .ThenInclude(r => r.CreatedByUser)
            .Where(s =>
                s.ApproverUserId == userId &&
                s.Status == RequestStatus.Pending &&
                s.Sequence == s.WorkflowRequest.CurrentStep &&
                s.WorkflowRequest.Status == RequestStatus.Pending)
            .OrderByDescending(s => s.WorkflowRequest.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<WorkflowStep>> GetAllPendingApprovalsAsync(int userId)
    {
        return await _context.WorkflowSteps
            .Include(s => s.WorkflowRequest)
                .ThenInclude(r => r.CreatedByUser)
            .Where(s =>
                s.ApproverUserId == userId &&
                s.Status == RequestStatus.Pending &&
                s.Sequence == s.WorkflowRequest.CurrentStep &&
                s.WorkflowRequest.Status == RequestStatus.Pending)
            .OrderByDescending(s => s.WorkflowRequest.CreatedAt)
            .ToListAsync();
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}