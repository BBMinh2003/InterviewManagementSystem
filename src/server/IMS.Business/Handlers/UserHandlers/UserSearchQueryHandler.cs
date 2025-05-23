using AutoMapper;
using IMS.Business.Services;
using IMS.Business.ViewModels.UserViews;
using IMS.Core.Extensions;
using IMS.Core.ViewModels;
using IMS.Data.Repositories;
using IMS.Data.UnitOfWorks;
using IMS.Models.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers.UserHandlers;

public class UserSearchQueryHandler : BaseUserHandler, IRequestHandler<UserSearchQuery, PaginatedResult<UserViewModel>>
{
    public UserSearchQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager, IUserIdentity currentUser, IEmailService emailService) : base(unitOfWork, mapper, userManager, roleManager, currentUser, emailService)
    {
    }

    public async Task<PaginatedResult<UserViewModel>> Handle(
        UserSearchQuery request,
        CancellationToken cancellationToken)
    {
        var query = _userManager.Users.AsQueryable()
        .Include(u => u.Department)
        .Include(u => u.CreatedBy)
        .Include(u => u.UpdatedBy)
        .Include(u => u.DeletedBy)
        .AsQueryable();

        if (!string.IsNullOrEmpty(request.Keyword))
        {
            query = query.Where(x => (x.UserName ?? "").Contains(request.Keyword)
                || x.Note!.Contains(request.Keyword)
                || x.Department!.Name.Contains(request.Keyword)
                || x.FullName!.Contains(request.Keyword)
                || x.Email!.Contains(request.Keyword)
                || x.PhoneNumber!.Contains(request.Keyword));
        }

        if (request.Role != null && request.Role != string.Empty)
        {
            var role = await _roleManager.FindByNameAsync(request.Role);
            if (role != null)
            {
                var users = await _userManager.GetUsersInRoleAsync(role.Name ?? "");
                query = query.Where(x => users.Contains(x));
            }
        }


        if (!string.IsNullOrEmpty(request.OrderBy))
        {
            query = query.OrderByExtension(request.OrderBy, request.OrderDirection.ToString());
        }
        else
        {
            query = query.OrderBy(x => x.CreatedAt);
        }

        var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
        query = query.Where(u => !adminUsers.Contains(u));

        var items = await query.Skip(request.PageSize * (request.PageNumber - 1))
            .Take(request.PageSize)
            .Include(x => x.Department)
            .ToListAsync(cancellationToken);


        var viewModels = _mapper.Map<IEnumerable<UserViewModel>>(items);
        var userCounts = items.Count;
        return new PaginatedResult<UserViewModel>(request.PageNumber, request.PageSize, userCounts, viewModels);
    }
}
