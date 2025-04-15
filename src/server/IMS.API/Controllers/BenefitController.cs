using IMS.Business.Handlers.Benefit;
using IMS.Business.ViewModels.Benefit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BenefitController : ControllerBase
{
    private readonly IMediator _mediator;

    public BenefitController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BenefitViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _mediator.Send(new BenefitGetAllQuery());
        return Ok(result);
    }
}
