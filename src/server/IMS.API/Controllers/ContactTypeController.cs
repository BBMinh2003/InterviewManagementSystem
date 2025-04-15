using IMS.Business.Handlers;
using IMS.Business.ViewModels.ContactType;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactTypeController: ControllerBase
{
    private readonly IMediator _mediator;

    public ContactTypeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ContactTypeViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _mediator.Send(new ContactTypeGetAllQuery());
        return Ok(result);
    }
}

