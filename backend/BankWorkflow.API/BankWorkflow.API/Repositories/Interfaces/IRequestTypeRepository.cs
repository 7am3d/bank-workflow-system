using BankWorkflow.API.Models;

namespace BankWorkflow.API.Repositories.Interfaces;

public interface IRequestTypeRepository
{
    Task<List<RequestType>> GetAllAsync();

    Task<RequestType?> GetByIdAsync(int id);

    Task<RequestType?> GetByNameAsync(string name);

    Task<bool> NameExistsAsync(string name);

    Task AddAsync(RequestType requestType);

    void Update(RequestType requestType);

    Task SaveChangesAsync();
}