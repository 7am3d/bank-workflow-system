using BankWorkflow.API.Models;

namespace BankWorkflow.API.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();

    Task<User?> GetByIdAsync(int id);

    Task<User?> GetByEmailAsync(string email);

    Task<bool> EmailExistsAsync(string email);

    Task<Role?> GetRoleByIdAsync(int id);

    Task<Department?> GetDepartmentByIdAsync(int id);

    Task AddAsync(User user);

    void Update(User user);

    Task SaveChangesAsync();

    Task<User?> GetFirstByRoleAsync(
    int roleId,
    int? excludeUserId = null);

    Task<User?> GetFirstEmployeeCheckerAsync(int excludedUserId);

    Task<Role?> GetRoleByNameAsync(string name);
}