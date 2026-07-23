using BankWorkflow.API.DTOs.RequestType;
using BankWorkflow.API.Models;

namespace BankWorkflow.API.Common.Mappers;

public static class RequestTypeMapper
{
    public static RequestTypeDto ToDto(RequestType requestType)
    {
        return new RequestTypeDto
        {
            Id = requestType.Id,
            Name = requestType.Name,
            Description = requestType.Description,
            IsActive = requestType.IsActive
        };
    }
}