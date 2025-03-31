using System;
using IMS.Business.Handlers;
using IMS.Business.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OfferController : ControllerBase
{
    private readonly IMediator _mediator;

    public OfferController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _mediator.Send(new OfferGetAllQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var result = await _mediator.Send(new OfferGetByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(OfferViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(OfferCreateCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("update/{id}")]
    [ProducesResponseType(typeof(OfferViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, OfferUpdateCommand command)
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

    [HttpPut("update-status")]
    public async Task<IActionResult> UpdateOfferStatus(OfferUpdateStatusCommand command)
    {
        var result = await _mediator.Send(command);

        if (result)
        {
            return Ok("Offer status updated successfully");
        }

        return BadRequest("No changes were made to the offer");
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new OfferDeleteByIdCommand { Id = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("search")]
    [ProducesResponseType(typeof(IEnumerable<OfferViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchAsync([FromBody] OfferSearchQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
