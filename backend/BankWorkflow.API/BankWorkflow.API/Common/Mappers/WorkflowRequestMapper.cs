using BankWorkflow.API.DTOs.WorkflowRequest;
using BankWorkflow.API.Models;

namespace BankWorkflow.API.Common.Mappers;

public static class WorkflowRequestMapper
{
    public static WorkflowRequestDto ToDto(WorkflowRequest request)
    {
        return new WorkflowRequestDto
        {
            Id = request.Id,
            Title = request.Title,
            Description = request.Description,
            RequestType = request.RequestType.Name,
            CreatedBy = $"{request.CreatedByUser.FirstName} {request.CreatedByUser.LastName}",
            Status = request.Status,
            Priority = request.Priority,
            CurrentStep = request.CurrentStep,
            CreatedAt = request.CreatedAt
        };
    }
}