using IMS.Business.Handlers.UserHandlers;
using IMS.Business.ViewModels.UserViews;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GetUserController : ControllerBase
{
    private readonly IMediator _mediator;

    public GetUserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves a list of interviewers.
    /// </summary>
    /// <returns>An IActionResult containing a list of interviewers.</returns>
    [HttpGet("interviewers")]
    [ProducesResponseType(typeof(IEnumerable<GetUserViewModel>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetInterviewers()
    {
        var users = await _mediator.Send(new UserGetInterviewQuery());
        return Ok(users);
    }

    /// <summary>
    /// Retrieves a list of recruiters and managers.
    /// </summary>
    /// <returns>An IActionResult containing a list of recruiters and managers.</returns>
    [HttpGet("recruiters")]
    [ProducesResponseType(typeof(IEnumerable<GetUserViewModel>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetRecruitersAndManagers()
    {
        var users = await _mediator.Send(new UserGetRecruiterQuery());
        return Ok(users);
    }


    /// <summary>
    /// Retrieves a list of admins.
    /// </summary>
    /// <returns>An IActionResult containing a list of admins.</returns>
    [HttpGet("admin")]
    [ProducesResponseType(typeof(IEnumerable<GetUserViewModel>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetAdmin()
    {
        var users = await _mediator.Send(new UserGetAdminQuery());
        return Ok(users);
    }
}
