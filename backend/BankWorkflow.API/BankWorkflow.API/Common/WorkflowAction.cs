namespace BankWorkflow.API.Common;

public enum WorkflowAction
{
    Created = 1,
    Submitted = 2,
    Approved = 3,
    Rejected = 4,
    Returned = 5,
    Cancelled = 6,
    CommentAdded = 7,
    Reassigned = 8
}