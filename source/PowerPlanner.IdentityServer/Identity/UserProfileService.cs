using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Identity
{
    public class UserProfileService : IProfileService
    {
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            // TODO: Get the account info and return the data
            if (context.RequestedClaimTypes.Any())
            {
                //List<Claim> claims = new List<Claim>();

                //if (context.RequestedClaimTypes.Contains("openid"))
                //{
                //    //claims.Add(new Claim("openid", context.))
                //}

                //context.AddRequestedClaims(claims);
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            // Determines if user should be allowed to complete login
            // TODO: Should check that the account exists (it could have been deleted)
            
            context.IsActive = true;
        }
    }
}
