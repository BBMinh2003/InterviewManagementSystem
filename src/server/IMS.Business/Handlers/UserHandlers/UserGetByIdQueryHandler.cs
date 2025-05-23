using AutoMapper;
using IMS.Business.Services;
using IMS.Business.ViewModels.UserViews;
using IMS.Core.Exceptions;
using IMS.Data.Repositories;
using IMS.Data.UnitOfWorks;
using IMS.Models.Security;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers.UserHandlers;

public class UserGetByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager, IUserIdentity currentUser, IEmailService emailService) : BaseUserHandler(unitOfWork, mapper, userManager, roleManager, currentUser, emailService), IRequestHandler<UserGetByIdQuery, UserViewModel>
{
    public async Task<UserViewModel> Handle(UserGetByIdQuery request, CancellationToken cancellationToken)
    {
        User user = await _userManager.Users
            .AsQueryable()
            .Include(u => u.Department)
            .Include(u => u.CreatedBy)
            .Include(u => u.UpdatedBy)
            .Include(u => u.DeletedBy)
            .FirstOrDefaultAsync(u => u.Id == request.Id) ??
                throw new ResourceNotFoundException("User not found");

        return _mapper.Map<UserViewModel>(user);
    }

}