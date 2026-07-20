using BankWorkflow.API.DTOs.Auth;
using BankWorkflow.API.DTOs.User;

namespace BankWorkflow.API.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);

    Task<UserProfileDto?> GetCurrentUserAsync(int userId);
}