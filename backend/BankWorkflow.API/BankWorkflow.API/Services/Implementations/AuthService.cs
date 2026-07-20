using BankWorkflow.API.DTOs.Auth;
using BankWorkflow.API.DTOs.User;
using BankWorkflow.API.Repositories.Interfaces;
using BankWorkflow.API.Services.Interfaces;

namespace BankWorkflow.API.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        IUserRepository userRepository,
        IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        // Find the user by email
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null)
            return null;

        // Check if the account is active
        if (!user.IsActive)
            return null;

        // Verify the password
        var passwordValid = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.PasswordHash);

        if (!passwordValid)
            return null;

        // Generate JWT token
        var token = _jwtTokenService.GenerateToken(user);

        // Return login response
        return new LoginResponseDto
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(8),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.Name
        };
    }

    public async Task<UserProfileDto?> GetCurrentUserAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
            return null;

        return new UserProfileDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role.Name,
            Department = user.Department.Name
        };
    }
}