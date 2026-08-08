using BankWorkflow.API.DTOs.WorkflowComment;
using BankWorkflow.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankWorkflow.API.Controllers;

[ApiController]
[Authorize]
[Route("api/workflowrequests/{workflowRequestId:int}/comments")]
public class WorkflowCommentsController : ControllerBase
{
    private readonly IWorkflowCommentService _commentService;

    public WorkflowCommentsController(
        IWorkflowCommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetComments(int workflowRequestId)
    {
        var comments = await _commentService
            .GetByWorkflowRequestIdAsync(workflowRequestId);

        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(
        int workflowRequestId,
        [FromBody] CreateWorkflowCommentDto dto)
    {
        var comment = await _commentService
            .AddCommentAsync(workflowRequestId, dto);

        return Ok(comment);
    }
}