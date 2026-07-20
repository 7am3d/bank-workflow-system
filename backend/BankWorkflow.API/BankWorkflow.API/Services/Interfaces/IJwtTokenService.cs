using BankWorkflow.API.Models;

namespace BankWorkflow.API.Services.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}