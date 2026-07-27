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

    public WorkflowRequestsController(IWorkflowRequestService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
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
}