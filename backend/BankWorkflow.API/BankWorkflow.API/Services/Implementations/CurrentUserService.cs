using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BankWorkflow.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BankWorkflow.API.Services.Implementations;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User is not authenticated.");

            return int.Parse(userId);
        }
    }

    public string Email =>
        _httpContextAccessor.HttpContext?
            .User
            .FindFirst(JwtRegisteredClaimNames.Email)?
            .Value
        ?? throw new UnauthorizedAccessException("User is not authenticated.");

    public string Role =>
        _httpContextAccessor.HttpContext?
            .User
            .FindFirst(ClaimTypes.Role)?
            .Value
        ?? throw new UnauthorizedAccessException("User is not authenticated.");
}