using System;
using IMS.Business.Handlers;
using IMS.Business.Services;
using IMS.Business.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Manager,Recruiter,Admin")]
/// <summary>
/// Manages operations related to offers
/// </summary>
public class OfferController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IOfferReminderService _offerReminderService;
    private readonly ILogger<OfferController> _logger;

    private readonly IExportExcelFileService _offerExportService;

    public OfferController(IMediator mediator, IOfferReminderService offerReminderService,
            ILogger<OfferController> logger, IExportExcelFileService offerExportService)
    {
        _mediator = mediator;
        _offerReminderService = offerReminderService;
        _offerExportService = offerExportService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all available offers
    /// </summary>
    /// <returns>
    /// List of OfferViewModel entries.
    /// Returns 204 No Content if no offers exist
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OfferViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _mediator.Send(new OfferGetAllQuery());
        return result.Any() ? Ok(result) : NoContent();
    }

    /// <summary>
    /// Gets a specific offer by unique identifier
    /// </summary>
    /// <param name="id">Unique GUID identifier of the offer</param>
    /// <returns>
    /// Single OfferViewModel details.
    /// Returns 404 Not Found if offer doesn't exist
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OfferViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var result = await _mediator.Send(new OfferGetByIdQuery { Id = id });
        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Creates a new offer entry
    /// </summary>
    /// <param name="command">Data required for offer creation</param>
    /// <returns>
    /// Newly created OfferViewModel with Location header.
    /// Returns 400 Bad Request for invalid input
    /// </returns>
    [HttpPost("create")]
    [ProducesResponseType(typeof(OfferViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(OfferCreateCommand command)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing offer
    /// </summary>
    /// <param name="id">GUID of the offer to update</param>
    /// <param name="command">Updated offer data</param>
    /// <returns>
    /// Modified OfferViewModel.
    /// Returns 404 Not Found if offer doesn't exist,
    /// 400 Bad Request for invalid data
    /// </returns>
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

    /// <summary>
    /// Updates the status of an offer
    /// </summary>
    /// <param name="command">Status update parameters</param>
    /// <returns>
    /// Success message if updated successfully.
    /// Returns 400 Bad Request if no changes were applied
    /// </returns>
    [HttpPut("update-status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateOfferStatus(OfferUpdateStatusCommand command)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _mediator.Send(command);
        return result ? Ok("Offer status updated successfully") : BadRequest("No changes were made");
    }

    /// <summary>
    /// Deletes an offer by identifier
    /// </summary>
    /// <param name="id">GUID of the offer to delete</param>
    /// <returns>
    /// Boolean confirmation of deletion.
    /// Returns 404 Not Found if offer doesn't exist
    /// </returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new OfferDeleteByIdCommand { Id = id });
        return result ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Searches offers based on criteria
    /// </summary>
    /// <param name="query">Search filters and parameters</param>
    /// <returns>
    /// Filtered list of OfferViewModel entries.
    /// Returns 400 Bad Request for invalid filters
    /// </returns>
    [HttpPost("search")]
    [ProducesResponseType(typeof(IEnumerable<OfferViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync([FromBody] OfferSearchQuery query)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Manually triggers offer approval reminders
    /// </summary>
    /// <returns>
    /// Operation status message.
    /// Returns 500 Internal Server Error for system failures
    /// </returns>
    /// <remarks>
    /// Primarily used for debugging and manual operations
    /// </remarks>
    [HttpPost("trigger-offer-reminders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> TriggerOfferReminders()
    {
        try
        {
            _logger.LogInformation("Triggering offer reminders manually...");
            await _offerReminderService.CheckAndSendApprovalRemindersAsync();
            return Ok(new { Message = "Reminder job triggered successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error triggering reminders");
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpGet("export")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExportOffers([FromQuery] OfferExportQuery request)
    {
        try
        {
            if (request.FromDate > request.ToDate)
                return BadRequest("FromDate must be less than or equal to ToDate.");

            var fileBytes = await _offerExportService.ExportOffersAsync(request.FromDate, request.ToDate);

            var fileName = $"Offerlist-{request.FromDate:yyyy-MM-dd}_{request.ToDate:yyyy-MM-dd}.xlsx";

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
