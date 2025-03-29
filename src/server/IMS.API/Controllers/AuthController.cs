using IMS.Business.Handlers.Auth;
using IMS.Business.ViewModels.Auth;
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

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
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

    /// <summary>
    /// Register a user (Mainly for testing)
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    /// <summary>
    /// API that allows users to receive an email for ForgotPassword Token
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("forgotPassword")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    /// <summary>
    /// API that allows users to reset their password based on token received from email
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("resetPassword")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    /// <summary>
    /// API that allows users to refresh their access token based on the previously received refresh token
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("refreshAccessToken")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshAccessTokenCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}
