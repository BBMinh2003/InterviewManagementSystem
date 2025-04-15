using IMS.Business.Handlers;
using IMS.Business.Handlers.Job;
using IMS.Business.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Manager,Recruiter,Admin,Interviewer")]
public class JobController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<JobViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _mediator.Send(new JobGetAllQuery());
        return Ok(result);
    }

    [HttpGet("details/{id}")]
    public async Task<ActionResult<JobViewModel>> GetById(Guid id)
    {
        var result = await _mediator.Send(new JobGetByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost("search")]
    [ProducesResponseType(typeof(IEnumerable<JobViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SearchAsync([FromBody] JobSearchQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("create")]
    [Authorize(Roles = "Manager,Recruiter,Admin")]
    [ProducesResponseType(typeof(JobViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(JobCreateCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("update/{id}")]
    [Authorize(Roles = "Manager,Recruiter,Admin")]
    [ProducesResponseType(typeof(JobViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, JobUpdateCommand command)
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
    [Authorize(Roles = "Manager,Recruiter,Admin")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new JobDeleteByIdCommand { Id = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("import-excel")]
    // [Authorize(Roles = "Manager,Recruiter,Admin")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportJobsFromExcel([FromForm] JobImportExcelCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
