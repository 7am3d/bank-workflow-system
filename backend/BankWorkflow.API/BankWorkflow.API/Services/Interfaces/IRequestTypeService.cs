using BankWorkflow.API.DTOs.RequestType;

namespace BankWorkflow.API.Services.Interfaces;

public interface IRequestTypeService
{
    Task<List<RequestTypeDto>> GetAllAsync();

    Task<RequestTypeDto?> GetByIdAsync(int id);

    Task<RequestTypeDto> CreateAsync(CreateRequestTypeDto request);

    Task<RequestTypeDto?> UpdateAsync(int id, UpdateRequestTypeDto request);

    Task<bool> DeactivateAsync(int id);
}