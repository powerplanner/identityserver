using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PowerPlanner.IdentityServer.Helpers
{
    public static class UserExtensions
    {
        public static long GetAccountId(this ClaimsPrincipal user)
        {
            string id = user.FindFirst(ClaimTypes.NameIdentifier).Value;
            return long.Parse(id);
        }
    }
}
