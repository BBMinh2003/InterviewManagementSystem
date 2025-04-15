using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Microsoft.Extensions.Configuration;
using RazorLight;

namespace IMS.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly RazorLightEngine _razorEngine;
        private readonly string _smtpSenderEmail;
        private readonly int _smtpPort;
        private readonly string _smtpServer;
        private readonly string _smtpPassword;


        public EmailService(IConfiguration config)
        {
            _config = config;
            _razorEngine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(EmailService).Assembly)
                .UseMemoryCachingProvider()
                .Build();
            _smtpSenderEmail = _config["SmtpSettings:SenderEmail"] ?? "";
            _smtpPort = int.Parse(_config["SmtpSettings:Port"] ?? "587");
            _smtpServer = _config["SmtpSettings:Server"] ?? "";
            _smtpPassword = _config["SmtpSettings:Password"] ?? "";
        }

        private string LoadTemplate(string templateName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"IMS.Business.EmailTemplates.{templateName}.cshtml";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new FileNotFoundException($"Email template '{templateName}' not found.");
            }

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_smtpSenderEmail));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_smtpSenderEmail, _smtpPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendEmailWithAttachmentAsync(string to, string subject, string body, string filePath)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_smtpSenderEmail));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            var bodyPart = new TextPart(TextFormat.Html) { Text = body };
            MimeEntity attachmentPart;

            using (var stream = File.OpenRead(filePath))
            {
                attachmentPart = new MimePart()
                {
                    Content = new MimeContent(stream),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = Path.GetFileName(filePath)
                };

                var multipart = new Multipart("mixed") { bodyPart, attachmentPart };
                email.Body = multipart;

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_smtpSenderEmail, _smtpPassword);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
        }

        public async Task SendEmailWithTemplateAsync<T>(string to, string subject, string templateName, T model)
        {
            string templateContent = LoadTemplate(templateName);
            string emailBody = await _razorEngine.CompileRenderStringAsync(templateName, templateContent, model);
            await SendEmailAsync(to, subject, emailBody);
        }

        public async Task SendEmailWithTemplateAndAttachmentAsync<T>(string to, string subject, string templateName, T model, string filePath)
        {
            string templateContent = LoadTemplate(templateName);
            string emailBody = await _razorEngine.CompileRenderStringAsync(templateName, templateContent, model);
            await SendEmailWithAttachmentAsync(to, subject, emailBody, filePath);
        }
    }
}
