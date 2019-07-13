using IdentityServer4.Models;
using IdentityServer4.Validation;
using PowerPlanner.IdentityServer.Helpers;
using PowerPlanner.IdentityServer.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Identity
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            using (var conn = await MySqlConnection.CreateAsync())
            {
                long accountId = PasswordManager.IsValid(conn, context.UserName, context.Password);
                if (accountId == -1)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);
                }
                else
                {
                    context.Result = new GrantValidationResult(accountId.ToString(), "password");
                }
            }
        }
    }
}
