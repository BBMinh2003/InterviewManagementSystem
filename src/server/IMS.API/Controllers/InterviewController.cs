using IMS.Business.Handlers;
using IMS.Business.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InterviewController : ControllerBase
{
    private readonly IMediator _mediator;

    public InterviewController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _mediator.Send(new InterviewGetAllQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var result = await _mediator.Send(new InterviewGetByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(InterviewViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(InterviewCreateCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("update/{id}")]
    [ProducesResponseType(typeof(InterviewViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, InterviewUpdateCommand command)
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
        var command = new InterviewDeleteByIdCommand { Id = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("search")]
    [ProducesResponseType(typeof(IEnumerable<InterviewViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchAsync([FromBody] InterviewSearchQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("cancel")]
    public async Task<IActionResult> CancelInterview(InterviewUpdateStatusCommand command)
    {
        var result = await _mediator.Send(command);

        if (result)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPut("submit-result")]
    public async Task<IActionResult> SubmitResult([FromBody] InterviewSubmitResultCommand command)
    {
        var result = await _mediator.Send(command);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost("/send-reminder")]
    public async Task<IActionResult> SendReminder(InterviewSendReminderCommand command)
    {
        var result = await _mediator.Send(command);

        return result ? Ok() : BadRequest();
    }
}
