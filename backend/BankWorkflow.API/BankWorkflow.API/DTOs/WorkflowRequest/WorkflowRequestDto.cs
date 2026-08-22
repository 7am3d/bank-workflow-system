using BankWorkflow.API.Common;

namespace BankWorkflow.API.DTOs.WorkflowRequest;

public class WorkflowRequestDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string RequestType { get; set; } = string.Empty;

    public string CreatedBy { get; set; } = string.Empty;

    public RequestStatus Status { get; set; }

    public PriorityLevel Priority { get; set; }

    public int CurrentStep { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool CanCurrentUserAct { get; set; }
}