using BankWorkflow.API.Data;
using BankWorkflow.API.Models;
using BankWorkflow.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankWorkflow.API.Repositories.Implementations;

public class RequestTypeRepository : IRequestTypeRepository
{
    private readonly AppDbContext _context;

    public RequestTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<RequestType>> GetAllAsync()
    {
        return await _context.RequestTypes
            .Where(r => r.IsActive)
            .OrderBy(r => r.Name)
            .ToListAsync();
    }

    public async Task<RequestType?> GetByIdAsync(int id)
    {
        return await _context.RequestTypes
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<RequestType?> GetByNameAsync(string name)
    {
        return await _context.RequestTypes
            .FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task<bool> NameExistsAsync(string name)
    {
        return await _context.RequestTypes
            .AnyAsync(r => EF.Functions.ILike(r.Name, name));
    }
    public async Task AddAsync(RequestType requestType)
    {
        await _context.RequestTypes.AddAsync(requestType);
    }

    public void Update(RequestType requestType)
    {
        _context.RequestTypes.Update(requestType);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}