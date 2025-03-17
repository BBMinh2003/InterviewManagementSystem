using System;
using AutoMapper;
using IMS.Business.Handlers;
using IMS.Business.Services;
using IMS.Data.UnitOfWorks;
using IMS.Models.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IMS.Business.ViewModels.Auth;

public class ForgotPasswordCommand : IRequest<BaseResponse>
{
    public required string Email { get; set; }
}


public class ForgotPasswordCommandHandler : BaseHandler, IRequestHandler<ForgotPasswordCommand, BaseResponse>
{
    private readonly IEmailService _emailService;
    private readonly UserManager<User> _userManager;
    public ForgotPasswordCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, UserManager<User> userManager) : base(unitOfWork, mapper)
    {
        _emailService = emailService;
        _userManager = userManager;
    }

    public async Task<BaseResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email address");
        }

        string resetLink = $"http://localhost:5155//reset-password?email={request.Email}&token=generated-token";

        string emailBody = string.Format(@"
        <p>We have just received a password reset request for <strong>{0}</strong>.</p>
        <p>Please click <a href='{1}'>here</a> to reset your password.</p>
        <p>For your security, the link will expire in 30 minutes or immediately after you reset your password.</p>
        <br>
        <p>Thanks & Regards!<br>IMS Team.</p>", request.Email, resetLink);

        await _emailService.SendEmailAsync(request.Email, "Password Reset", emailBody);

        return new BaseResponse { Message = "We've sent an email with the link to reset your password." };
    }
}