using IMS.Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[ApiController]
[Route("api/email")]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
    {
        await _emailService.SendEmailAsync(request.To, request.Subject, request.Body);
        return Ok(new { Message = "Email sent successfully!" });
    }
}

public class EmailRequest
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}