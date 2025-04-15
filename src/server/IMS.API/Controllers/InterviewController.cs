using IMS.Business.Handlers;
using IMS.Business.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Manager,Recruiter,Admin,Interviewer")]

public class InterviewController : ControllerBase
{
    private readonly IMediator _mediator;

    public InterviewController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all interviews.
    /// </summary>
    /// <returns>A list of interviews.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _mediator.Send(new InterviewGetAllQuery());
        return Ok(result);
    }


    /// <summary>
    /// Get interview by ID.
    /// </summary>
    /// <param name="id">The interview ID.</param>
    /// <returns>The interview with the given ID.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(InterviewViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var result = await _mediator.Send(new InterviewGetByIdQuery { Id = id });
        return Ok(result);
    }


    /// <summary>
    /// Create a new interview.
    /// </summary>
    /// <param name="command">The interview create command.</param>
    /// <returns>The created interview.</returns>
    [HttpPost("create")]
    [Authorize(Roles = "Manager,Recruiter,Admin")]
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


    /// <summary>
    /// Update an interview by ID.
    /// </summary>
    /// <param name="id">The interview ID.</param>
    /// <param name="command">The interview update command.</param>
    /// <returns>The updated interview.</returns>
    [HttpPut("update/{id}")]
    [Authorize(Roles = "Manager,Recruiter,Admin")]
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


    /// <summary>
    /// Delete an interview by ID.
    /// </summary>
    /// <param name="id">The interview ID.</param>
    /// <returns>A boolean indicating whether the deletion was successful.</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Manager,Recruiter,Admin")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new InterviewDeleteByIdCommand { Id = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    /// <summary>
    /// Search for interviews based on query parameters.
    /// </summary>
    /// <param name="query">The search query parameters.</param>
    /// <returns>A list of interviews matching the search criteria.</returns>
    [HttpPost("search")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<InterviewViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchAsync([FromBody] InterviewSearchQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }


    /// <summary>
    /// Cancel an interview by ID.
    /// </summary>
    /// <param name="command">The command containing interview status update to "cancel".</param>
    /// <returns>A boolean indicating whether the cancel operation was successful.</returns>
    [HttpPut("cancel")]
    [Authorize(Roles = "Manager,Recruiter,Admin")]
    public async Task<IActionResult> CancelInterview(InterviewUpdateStatusCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }


    /// <summary>
    /// Submit results for an interview.
    /// </summary>
    /// <param name="command">The command containing interview result data.</param>
    /// <returns>The submitted interview result.</returns
    [HttpPut("submit-result")]
    [Authorize(Roles = "Interviewer")]
    public async Task<IActionResult> SubmitResult([FromBody] InterviewSubmitResultCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }


    /// <summary>
    /// Send a reminder for an interview.
    /// </summary>
    /// <param name="command">The command to send the interview reminder.</param>
    /// <returns>Response status indicating whether the reminder was sent successfully.</returns>
    [HttpPost("send-reminder")]
    [Authorize(Roles = "Manager,Recruiter,Admin")]

    public async Task<IActionResult> SendReminder(InterviewSendReminderCommand command)
    {
        var result = await _mediator.Send(command);

        return result ? Ok() : BadRequest();
    }
}
