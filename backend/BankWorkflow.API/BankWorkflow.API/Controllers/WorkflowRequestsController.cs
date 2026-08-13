using BankWorkflow.API.DTOs.WorkflowRequest;
using BankWorkflow.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankWorkflow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkflowRequestsController : ControllerBase
{
    private readonly IWorkflowRequestService _service;
    private readonly IWorkflowApprovalService _approvalService;
    private readonly IWorkflowHistoryService _historyService;

    public WorkflowRequestsController(
        IWorkflowRequestService service,
        IWorkflowApprovalService approvalService,
        IWorkflowHistoryService historyService)
    {
        _service = service;
        _approvalService = approvalService;
        _historyService = historyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
    [FromQuery] WorkflowRequestFilterDto filter)
    {
        return Ok(await _service.GetAllAsync(filter));
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyRequests()
    {
        return Ok(await _service.GetMyRequestsAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var request = await _service.GetByIdAsync(id);

        if (request is null)
            return NotFound();

        return Ok(request);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorkflowRequestDto dto)
    {
        var request = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = request.Id },
            request);
    }

    [HttpPost("{id:int}/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        await _approvalService.ApproveAsync(id);

        return NoContent();
    }

    [HttpPost("{id:int}/reject")]
    public async Task<IActionResult> Reject(
    int id,
    [FromBody] RejectWorkflowRequestDto dto)
    {
        await _approvalService.RejectAsync(id, dto.Reason);

        return NoContent();
    }

    [HttpGet("{id:int}/history")]
    public async Task<IActionResult> GetHistory(int id)
    {
        var history = await _historyService.GetHistoryAsync(id);

        return Ok(history);
    }
}