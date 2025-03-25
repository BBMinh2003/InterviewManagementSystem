using IMS.Business.Handlers;
using IMS.Business.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CandidateController : ControllerBase
{
    private readonly IMediator _mediator;

    public CandidateController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _mediator.Send(new CandidateGetAllQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var result = await _mediator.Send(new CandidateGetByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost("search")]
    [ProducesResponseType(typeof(IEnumerable<CandidateViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchAsync([FromBody] CategorySearchQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(CandidateViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CandidateCreateCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("update/{id}")]
    [ProducesResponseType(typeof(CandidateViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, CandidateUpdateCommand command)
    {
        command.Id = id;

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new CandidateDeleteByIdCommand { Id = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("ban/{id}")]
    public async Task<IActionResult> BanCandidate(Guid id)
    {
        bool success = await _mediator.Send(new CandidateBanByIdCommand{Id = id});

        if (!success)
        {
            return BadRequest(new { message = "Candidate is already banned or update failed" });
        }

        return Ok(new { message = "Candidate banned successfully" });
    }
}
