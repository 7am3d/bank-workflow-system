using BankWorkflow.API.DTOs.RequestType;
using BankWorkflow.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankWorkflow.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RequestTypesController : ControllerBase
{
    private readonly IRequestTypeService _service;

    public RequestTypesController(IRequestTypeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<RequestTypeDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RequestTypeDto>> GetById(int id)
    {
        var requestType = await _service.GetByIdAsync(id);

        if (requestType is null)
            return NotFound();

        return Ok(requestType);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<RequestTypeDto>> Create(CreateRequestTypeDto request)
    {
        var requestType = await _service.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = requestType.Id },
            requestType);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<RequestTypeDto>> Update(
        int id,
        UpdateRequestTypeDto request)
    {
        var requestType = await _service.UpdateAsync(id, request);

        if (requestType is null)
            return NotFound();

        return Ok(requestType);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Deactivate(int id)
    {
        var success = await _service.DeactivateAsync(id);

        if (!success)
            return NotFound();

        return NoContent();
    }
}