using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Data.UnitOfWorks;
using IMS.Models.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IMS.Business.Handlers.Auth;

public class ResetPasswordCommandHandler : BaseHandler, IRequestHandler<ResetPasswordCommand, BaseResponse>
{
    private readonly UserManager<User> _userManager;

    public ResetPasswordCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager) : base(unitOfWork, mapper)
    {
        _userManager = userManager;
    }

    public async Task<BaseResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Email);
        if (user == null)
        {
            return new BaseResponse { Success = false, Message = "Invalid email address." };
        }

        var resetResult = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
        if (!resetResult.Succeeded)
        {
            var errors = string.Join("; ", resetResult.Errors.Select(e => e.Description));
            return new BaseResponse { Success = false, Message = $"Password reset failed: {errors}" };
        }

        return new BaseResponse { Message = "Your password has been reset successfully." };
    }

}