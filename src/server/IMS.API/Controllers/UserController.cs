using IMS.Business.Handlers.UserHandlers;
using IMS.Business.ViewModels.UserViews;
using IMS.Core.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;
/// <summary>
/// Controller for user management actions
/// </summary>
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>View models of the retrieved users</returns>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(IEnumerable<UserViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _mediator.Send(new UserGetAllQuery());
        return Ok(result);
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns>View model of the retrieved user</returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var result = await _mediator.Send(new UserGetByIdQuery { Id = id });
        return Ok(result);
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="command"></param>
    /// <returns>View model of the created user</returns>
    [HttpPost("create")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] UserCreateCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command);
        return CreatedAtAction("Create", result);
    }

    /// <summary>
    /// Update user information
    /// </summary>
    /// <param name="command"></param>
    /// <returns>View model of the updated user</returns>
    [HttpPut("update")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(UserUpdateCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Switch the status of a user (active/inactive)
    /// </summary>
    /// <param name="command"></param>
    /// <returns>View model of the updated user</returns>
    [HttpPut("switch-status")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> switchStatus(UserSwitchStatusCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Search for users based on the provided query
    /// </summary>
    /// <param name="query"></param>
    /// <returns>A paginated result of the searched users</returns>
    [HttpPost("search")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PaginatedResult<UserViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SearchAsync([FromBody] UserSearchQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get the create/update view model for user management
    /// </summary>
    /// <returns>Roles and Departments needed for user create/update actions</returns>
    [HttpGet("createUpdate")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(IEnumerable<UserCreateUpdateViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCreateUpdateAsync()
    {
        var result = await _mediator.Send(new UserCreateUpdateQuery());
        return Ok(result);
    }

    /// <summary>
    /// Get the current user's information
    /// </summary>
    /// <returns>View model of the current user</returns>
    [HttpGet("info")]
    [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> getCurrentUserInfo()
    {
        var result = await _mediator.Send(new UserProfileQuery());
        return Ok(result);
    }

    /// <summary>
    /// Update the current user's information
    /// </summary>
    /// <returns>View model of the current user</returns>
    [HttpPut("info-update")]
    [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> updateCurrentUser([FromBody] UserProfileUpdateCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
