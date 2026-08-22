using BankWorkflow.API.Common;

namespace BankWorkflow.API.Models;

public class WorkflowStepDefinition
{
    public int Id { get; set; }

    public int WorkflowDefinitionId { get; set; }

    public int Sequence { get; set; }

    public WorkflowApproverType ApproverType { get; set; }

    public int? RoleId { get; set; }

    public bool IsRequired { get; set; } = true;

    // Navigation Properties
    public WorkflowDefinition WorkflowDefinition { get; set; } = null!;

    public Role? Role { get; set; }
}