using AutoMapper;
using IMS.Business.Services;
using IMS.Business.ViewModels.Auth;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Models.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IMS.Business.Handlers.Auth;

public class RefreshAccessTokenCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager,
    ITokenService tokenService,
    IConfiguration configuration) : BaseHandler(unitOfWork, mapper), IRequestHandler<RefreshAccessTokenCommand, LoginResponse>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IConfiguration _configuration = configuration;

    public async Task<LoginResponse> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenEntry = await _unitOfWork.RefreshTokenRepository.GetQuery()
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token.Equals(request.RefreshToken));

        if (tokenEntry == null || tokenEntry.ExpiryDate < DateTime.UtcNow || tokenEntry.IsRevoked)
        {
            throw new TokenInvalidException("Invalid or expired refresh token.");
        }
        var user = tokenEntry.User;

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("Your account is deactivated. Please contact an administrator.");
        }

        var userRoles = await _userManager.GetRolesAsync(user);

        var accessToken = await _tokenService.GenerateTokenAsync(user, userRoles);

        await _tokenService.RevokeUserRefreshTokensAsync(user.Id, "User logged in again");

        var refreshTokenEntity = await _tokenService.GenerateRefreshTokenAsync(user.Id);

        if (!int.TryParse(_configuration["JWT:AccessTokenExpiryMinutes"], out var expiryMinutes))
        {
            expiryMinutes = 15;
        }

        return new LoginResponse
        {
            AccessToken = accessToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes),
            RefreshToken = refreshTokenEntity.Token
        };
    }
}
