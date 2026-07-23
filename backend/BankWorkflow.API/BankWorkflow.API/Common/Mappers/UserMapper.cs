using BankWorkflow.API.DTOs.User;
using BankWorkflow.API.Models;

namespace BankWorkflow.API.Common.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(User user)
    {
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
}