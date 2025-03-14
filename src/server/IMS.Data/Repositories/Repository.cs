using System;
using IMS.Core.Constants;
using IMS.Models.Security;

namespace IMS.Data.Repositories;

public class Repository<T> : RepositoryBase<T, IMSDbContext> where T : class, IBaseEntity
{
    private readonly IUserIdentity _currentUser;

    public Repository(IMSDbContext dataContext, IUserIdentity currentUser)
        : base(dataContext)
    {
        _currentUser = currentUser;
    }

    protected override Guid CurrentUserId
    {
        get
        {
            if (_currentUser != null)
            {
                return _currentUser.UserId;
            }

            return CoreConstants.AdminId;
        }
    }

    protected override string CurrentUserName
    {
        get
        {
            if (_currentUser != null)
            {
                return _currentUser.UserName;
            }

            return "Anonymous";
        }
    }
}
