namespace IMS.Business.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendEmailWithAttachmentAsync(string to, string subject, string body, string filePath);
    Task SendEmailWithTemplateAsync<T>(string to, string subject, string templateName, T model);
    Task SendEmailWithTemplateAndAttachmentAsync<T>(string to, string subject, string templateName, T model, string filePath);


}
