using BankWorkflow.API.DTOs.User;
using BankWorkflow.API.Models;
using BankWorkflow.API.Repositories.Interfaces;
using BankWorkflow.API.Services.Interfaces;
using System.Linq;

namespace BankWorkflow.API.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> CreateAsync(CreateUserDto request)
    {
        if (await _userRepository.EmailExistsAsync(request.Email))
        {
            throw new InvalidOperationException("Email already exists.");
        }

        var role = await _userRepository.GetRoleByIdAsync(request.RoleId);

        if (role is null)
        {
            throw new InvalidOperationException("Role not found.");
        }

        var department = await _userRepository.GetDepartmentByIdAsync(request.DepartmentId);

        if (department is null)
        {
            throw new InvalidOperationException("Department not found.");
        }

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            RoleId = request.RoleId,
            DepartmentId = request.DepartmentId,
            IsActive = true
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = role.Name,
            Department = department.Name,
            IsActive = user.IsActive
        };
    }

    public async Task<bool> DeactivateAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            return false;
        }

        user.IsActive = false;

        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();

        return true;
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(user => new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role.Name,
            Department = user.Department.Name,
            IsActive = user.IsActive
        }).ToList();
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            return null;

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role.Name,
            Department = user.Department.Name,
            IsActive = user.IsActive
        };
    }

    public async Task<UserDto?> UpdateAsync(int id, UpdateUserDto request)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            return null;
        }

        if (user.Email != request.Email &&
            await _userRepository.EmailExistsAsync(request.Email))
        {
            throw new InvalidOperationException("Email already exists.");
        }

        var role = await _userRepository.GetRoleByIdAsync(request.RoleId);

        if (role is null)
        {
            throw new InvalidOperationException("Role not found.");
        }

        var department = await _userRepository.GetDepartmentByIdAsync(request.DepartmentId);

        if (department is null)
        {
            throw new InvalidOperationException("Department not found.");
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.RoleId = request.RoleId;
        user.DepartmentId = request.DepartmentId;

        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = role.Name,
            Department = department.Name,
            IsActive = user.IsActive
        };
    }
}