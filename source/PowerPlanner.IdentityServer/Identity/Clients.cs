using IdentityServer4;
using IdentityServer4.Models;
using PowerPlanner.IdentityServer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Identity
{
    /// <summary>
    /// Clients are applications, like a mobile app and a web app
    /// </summary>
    public static class Clients
    {
        public static Client[] GetClients()
        {
            var legacyServerClientSecretTask = KeyVaultHelper.GetSecretAsync("LegacyServerClientSecret");
            legacyServerClientSecretTask.Wait();
            if (legacyServerClientSecretTask.Exception != null)
            {
                throw legacyServerClientSecretTask.Exception;
            }

            return new Client[]
            {
                new Client()
                {
                    ClientId = "PowerPlannerAndroid",
                    ClientName = "Power Planner Android app",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowOfflineAccess = true, // Offline access needed to receive refresh token
                    AllowedScopes = new string[]
                    {
                        Scopes.FullApp
                    },
                    RequireClientSecret = false,
                    RequireConsent = false
                },

                new Client()
                {
                    ClientId = "PowerPlannerUWP",
                    ClientName = "Power Planner UWP app",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowOfflineAccess = true, // Offline access needed to receive refresh token
                    AllowedScopes = new string[]
                    {
                        Scopes.FullApp
                    },
                    RequireClientSecret = false,
                    RequireConsent = false
                },

                new Client()
                {
                    ClientId = "LegacyServerClient",
                    ClientName = "Legacy server client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowOfflineAccess = false,
                    AllowedScopes = new string[]
                    {
                        Scopes.AccountRead,
                        Scopes.AccountWrite,
                        Scopes.FullApp
                    },
                    RequireClientSecret = true,
                    RequireConsent = false,
                    ClientSecrets = new Secret[]
                    {
                        new Secret(legacyServerClientSecretTask.Result.Value)
                    }
                }
            };
        }
    }
}
