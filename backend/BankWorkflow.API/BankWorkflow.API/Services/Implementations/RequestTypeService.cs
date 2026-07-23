using BankWorkflow.API.Common.Mappers;
using BankWorkflow.API.DTOs.RequestType;
using BankWorkflow.API.Models;
using BankWorkflow.API.Repositories.Interfaces;
using BankWorkflow.API.Services.Interfaces;

namespace BankWorkflow.API.Services.Implementations;

public class RequestTypeService : IRequestTypeService
{
    private readonly IRequestTypeRepository _repository;

    public RequestTypeService(IRequestTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<RequestTypeDto>> GetAllAsync()
    {
        var requestTypes = await _repository.GetAllAsync();

        return requestTypes
            .Select(RequestTypeMapper.ToDto)
            .ToList();
    }

    public async Task<RequestTypeDto?> GetByIdAsync(int id)
    {
        var requestType = await _repository.GetByIdAsync(id);

        if (requestType is null)
            return null;

        return RequestTypeMapper.ToDto(requestType);
    }

    public async Task<RequestTypeDto> CreateAsync(CreateRequestTypeDto request)
    {
        if (await _repository.NameExistsAsync(request.Name))
        {
            throw new InvalidOperationException("Request type already exists.");
        }

        var requestType = new RequestType
        {
            Name = request.Name,
            Description = request.Description,
            IsActive = true
        };

        await _repository.AddAsync(requestType);
        await _repository.SaveChangesAsync();

        return RequestTypeMapper.ToDto(requestType);
    }

    public async Task<RequestTypeDto?> UpdateAsync(int id, UpdateRequestTypeDto request)
    {
        var requestType = await _repository.GetByIdAsync(id);

        if (requestType is null)
            return null;

        if (!string.Equals(requestType.Name, request.Name, StringComparison.OrdinalIgnoreCase) &&
            await _repository.NameExistsAsync(request.Name))
        {
            throw new InvalidOperationException("Request type already exists.");
        }

        requestType.Name = request.Name;
        requestType.Description = request.Description;
        requestType.IsActive = request.IsActive;

        _repository.Update(requestType);
        await _repository.SaveChangesAsync();

        return RequestTypeMapper.ToDto(requestType);
    }

    public async Task<bool> DeactivateAsync(int id)
    {
        var requestType = await _repository.GetByIdAsync(id);

        if (requestType is null)
            return false;

        requestType.IsActive = false;

        _repository.Update(requestType);
        await _repository.SaveChangesAsync();

        return true;
    }
}