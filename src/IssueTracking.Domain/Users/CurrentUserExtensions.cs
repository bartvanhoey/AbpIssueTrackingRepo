using System;
using Volo.Abp.Users;

namespace IssueTracking.Domain.Users
{
    public static class CurrentUserExtensions
    {
        public static Guid GetId(this ICurrentUser user){
            return user.Id.Value;
        }
    }
}