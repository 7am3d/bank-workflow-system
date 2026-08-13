using BankWorkflow.API.Common;

namespace BankWorkflow.API.DTOs.WorkflowRequest;

public class WorkflowRequestFilterDto
{
    public string? Search { get; set; }

    public RequestStatus? Status { get; set; }

    public PriorityLevel? Priority { get; set; }

    public int? RequestTypeId { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}