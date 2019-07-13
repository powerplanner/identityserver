using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerPlanner.IdentityServer.Helpers;
using PowerPlanner.IdentityServer.Identity;

namespace PowerPlanner.IdentityServer.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        // POST: api/Account/updatepassword
        [HttpPost("updatepassword")]
        [Authorize]
        public async Task UpdatePasswordAsync([FromForm]string password)
        {
            using (var conn = await MySqlConnection.CreateAsync())
            {
                PasswordManager.UpdatePassword(conn, User.GetAccountId(), password);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateAsync([FromForm]string username, [FromForm]string password, [FromForm]string email)
        {
            email = email.Trim().ToLower();

            string error = MyErrorHandler.GetUsernameError(username);
            if (error != null)
            {
                return BadRequest(error);
            }

            error = MyErrorHandler.GetPasswordError(password);
            if (error != null)
            {
                return BadRequest(error);
            }

            error = MyErrorHandler.GetEmailError(email);
            if (error != null)
            {
                return BadRequest(error);
            }

            using (var conn = await MySqlConnection.CreateAsync())
            {
                if (PasswordManager.CreateAccount(conn, username, password, email))
                {
                    return Ok();
                }
                else
                {
                    // Most likely username is taken
                    return Conflict("Username is already taken.");
                }
            }
        }
    }
}
