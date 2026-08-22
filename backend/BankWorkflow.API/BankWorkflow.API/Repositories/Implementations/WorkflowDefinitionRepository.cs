using BankWorkflow.API.Data;
using BankWorkflow.API.Models;
using BankWorkflow.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankWorkflow.API.Repositories.Implementations;

public class WorkflowDefinitionRepository : IWorkflowDefinitionRepository
{
    private readonly AppDbContext _context;

    public WorkflowDefinitionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<WorkflowDefinition?> GetActiveByRequestTypeIdAsync(
        int requestTypeId)
    {
        return await _context.WorkflowDefinitions
            .Include(w => w.Steps)
                .ThenInclude(s => s.Role)
            .FirstOrDefaultAsync(w =>
                w.RequestTypeId == requestTypeId &&
                w.IsActive);
    }
}