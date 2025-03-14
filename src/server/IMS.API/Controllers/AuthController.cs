using IMS.Business.Handlers.Auth;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork unitOfWork;

    public AuthController(IMediator mediator, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        this.unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Login a user
    /// </summary>
    /// <param name="request"></param>
    /// <returns> LoginResponse with JWT token </returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello");
    }
}
