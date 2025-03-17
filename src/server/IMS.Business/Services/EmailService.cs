using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Microsoft.Extensions.Configuration;

namespace IMS.Business.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_config["SmtpSettings:SenderEmail"]));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_config["SmtpSettings:Server"], int.Parse(_config["SmtpSettings:Port"]), SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_config["SmtpSettings:SenderEmail"], _config["SmtpSettings:Password"]);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}