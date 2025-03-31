using AutoMapper;
using IMS.Business.Handlers;
using IMS.Business.Services;
using IMS.Core.Models;
using IMS.Data.UnitOfWorks;
using IMS.Models.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace IMS.Business.ViewModels.Auth;

public class ForgotPasswordCommandHandler : BaseHandler, IRequestHandler<ForgotPasswordCommand, BaseResponse>
{
    private readonly IEmailService _emailService;
    private readonly IConfiguration _config;
    private readonly UserManager<User> _userManager;
    public ForgotPasswordCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, UserManager<User> userManager, IConfiguration config) : base(unitOfWork, mapper)
    {
        _emailService = emailService;
        _userManager = userManager;
        _config = config;
    }

    public async Task<BaseResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email address");
        }
        request.ClientURI = request.ClientURI.TrimEnd('/');
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        string resetLink = $"{request.ClientURI}/reset-password?email={request.Email}&token={token}";

        await _emailService.SendEmailWithTemplateAsync(
                request.Email,
                "Password Reset",
                "ResetPasswordEmail",
                new ResetPasswordEmailModel 
                    { Email = request.Email, ResetLink = resetLink, ExpiryMinutes = int.Parse(_config["SmtpSettings:ResetPasswordTokenExpiryMinutes"]) });

        return new BaseResponse { Message = "We've sent an email with the link to reset your password." };
    }
}