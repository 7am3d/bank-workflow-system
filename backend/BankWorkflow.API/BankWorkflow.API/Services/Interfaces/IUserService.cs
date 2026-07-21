using BankWorkflow.API.DTOs.User;

namespace BankWorkflow.API.Services.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();

    Task<UserDto?> GetByIdAsync(int id);

    Task<UserDto> CreateAsync(CreateUserDto request);

    Task<UserDto?> UpdateAsync(int id, UpdateUserDto request);

    Task<bool> DeactivateAsync(int id);
}