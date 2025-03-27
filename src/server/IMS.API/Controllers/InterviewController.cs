using IMS.Business.Handlers;
using IMS.Business.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterviewController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InterviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _mediator.Send(new InterviewGetAllQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _mediator.Send(new InterviewGetByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(InterviewViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(InterviewCreateCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(InterviewViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, InterviewUpdateCommand command)
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

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new InterviewDeleteByIdCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("search")]
        [ProducesResponseType(typeof(IEnumerable<InterviewViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchAsync([FromBody] InterviewSearchQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelInterview(Guid id)
        {
            var result = await _mediator.Send(new UpdateInterviewStatusCommand { InterviewId = id });

            if (result)
            {
                return Ok(new { message = "Interview status updated to Cancelled." });
            }

            return BadRequest(new { message = "Interview status was not updated." });
        }

        [HttpPut("{id}/submit-result")]
        public async Task<IActionResult> SubmitResult(Guid id, [FromBody] InterviewResultCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID mismatch.");
            }

            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/send-reminder")]
        public async Task<IActionResult> SendReminder(Guid id)
        {
            var command = new InterviewSendReminderCommand { InterviewId = id };

            var result = await _mediator.Send(command);

            return result ? Ok(new { message = "Reminder sent successfully." }) : BadRequest(new { message = "Failed to send reminder." });
        }
    }
}
