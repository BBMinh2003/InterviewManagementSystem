using IMS.Business.Handlers.Skill;
using IMS.Business.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SkillController : ControllerBase
{
    private readonly IMediator _mediator;

    public SkillController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SkillViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _mediator.Send(new SkillGetAllQuery());
        return Ok(result);
    }
}
