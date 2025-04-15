using IMS.Business.Handlers;
using IMS.Business.Services;
using IMS.Business.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Manager,Recruiter,Admin,Interviewer")]
public class CandidateController : ControllerBase
{
    private readonly IMediator _mediator;

    public CandidateController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all candidates.
    /// </summary>
    /// <returns>Candidate list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CandidateViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _mediator.Send(new CandidateGetAllQuery());
        return Ok(result);
    }

    /// <summary>
    /// Get candidate by ID.
    /// </summary>
    /// <param name="id">The candidate ID.</param>
    /// <returns>The candidate with the given ID.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CandidateViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var result = await _mediator.Send(new CandidateGetByIdQuery { Id = id });
        return Ok(result);
    }

    /// <summary>
    /// Search candidates based on the provided query.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <returns>A list of candidates matching the search criteria.</returns>
    [HttpPost("search")]
    [ProducesResponseType(typeof(IEnumerable<CandidateViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SearchAsync([FromBody] CandidateSearchQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Create a new candidate.
    /// </summary>
    /// <param name="command">The candidate create command.</param>
    /// <returns>The created candidate.</returns>
    [HttpPost("create")]
    [Authorize(Roles = "Manager,Recruiter,Admin")]
    [ProducesResponseType(typeof(CandidateViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromForm] CandidateCreateCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Test Azure connection.
    /// </summary>
    /// <param name="storageService">The Azure storage service.</param>
    [HttpGet("test-azure-connection")]
    public async Task<IActionResult> TestAzureConnection([FromServices] AzureStorageService storageService)
    {
        bool isConnected = await storageService.CheckConnectionAsync();
        return Ok(new { Connected = isConnected });
    }

    /// <summary>
    /// Download a file from Azure storage.
    /// </summary>
    /// <param name="fileUrl">The URL of the file to download.</param>
    /// <param name="storageService">The Azure storage service.</param>
    /// <returns>The file stream.</returns>
    [HttpGet("download")]
    public async Task<IActionResult> DownloadFile(string fileUrl, [FromServices] AzureStorageService storageService)
    {
        try
        {
            var fileStream = await storageService.DownloadFileFromUrlAsync(fileUrl);
            return File(fileStream, "application/octet-stream", Path.GetFileName(fileUrl));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Update an existing candidate.
    /// </summary>
    /// <param name="id">The candidate ID.</param>
    /// <param name="command">The candidate update command.</param>
    /// <returns>The updated candidate.</returns>
    [HttpPut("update/{id}")]
    [Authorize(Roles = "Manager,Recruiter,Admin")]
    [ProducesResponseType(typeof(CandidateViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id,[FromForm] CandidateUpdateCommand command)
    {
        command.Id = id;

        if (!ModelState.IsValid)
        {
            return BadRequest(id);
        }

        var result = await _mediator.Send(command);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Delete a candidate by ID.
    /// </summary>
    /// <param name="id">The candidate ID.</param>
    /// <returns>True if the candidate was deleted, false otherwise.</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Manager,Recruiter,Admin")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new CandidateDeleteByIdCommand { Id = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    /// <summary>
    /// Ban a candidate by ID.
    /// </summary>
    /// <param name="id">The candidate ID.</param>
    /// <returns>True if the candidate was banned, false otherwise.</returns>
    [HttpPut("ban/{id}")]
    [Authorize(Roles = "Manager,Recruiter,Admin")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BanCandidate(Guid id)
    {
        bool success = await _mediator.Send(new CandidateBanByIdCommand { Id = id });

        return Ok(success);
    }

    /// <summary>
    /// List all files in Azure storage.
    /// </summary>
    /// <param name="storageService">The Azure storage service.</param>
    /// <returns>A list of file names.</returns>
    [HttpGet("list-files")]
    public async Task<IActionResult> ListFiles([FromServices] AzureStorageService storageService)
    {
        try
        {
            var fileNames = await storageService.GetAllFilesAsync();
            return Ok(fileNames);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving files: {ex.Message}");
        }
    }

    /// <summary>
    /// Upload a file to Azure storage.
    /// </summary>
    /// <param name="file">The file to upload.</param>
    /// <param name="storageService">The Azure storage service.</param>
    /// <returns>The URL of the uploaded file.</returns>
    [HttpPost("upload")]
    [Authorize(Roles = "Manager,Recruiter,Admin")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadFileAsync(IFormFile file, [FromServices] AzureStorageService storageService)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        try
        {
            // Mở stream của file để upload
            using (var stream = file.OpenReadStream())
            {
                // Gọi service để upload file lên Azure
                string fileUrl = await storageService.UploadFileAsync(stream, file.FileName);

                // Trả về URL của file đã được upload
                return Ok(new { FileUrl = fileUrl });
            }
        }
        catch (Exception ex)
        {
            // Xử lý lỗi khi upload
            return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
        }
    }
}
