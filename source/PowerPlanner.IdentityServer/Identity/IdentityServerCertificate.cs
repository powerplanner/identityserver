using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;
using PowerPlanner.IdentityServer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Identity
{
    public static class IdentityServerCertificate
    {
        public static async Task<X509Certificate2> GetAsync()
        {
            int retries = 0;
            const int maxRetries = 4;
            const string certName = "JwtCert";
            while (true)
            {
                try
                {
                    /* The next four lines of code show you how to use AppAuthentication library to fetch secrets from your key vault */
                    // Note that for this to work on developer machine, you must install Azure CLI and log in
                    SecretBundle secretBundle = await KeyVaultHelper.GetSecretAsync(certName);
                    return new X509Certificate2(Convert.FromBase64String(secretBundle.Value));
                }

                /* If you have throttling errors see this tutorial https://docs.microsoft.com/azure/key-vault/tutorial-net-create-vault-azure-web-app */
                /// <exception cref="KeyVaultErrorException">
                /// Thrown when the operation returned an invalid status code
                /// </exception>
                catch (KeyVaultErrorException keyVaultException)
                {
                    if (retries < maxRetries)
                    {
                        retries++;
                        await Task.Delay(getWaitTime(retries));
                    }
                    else
                    {
                        throw keyVaultException;
                    }
                }
            }
        }

        // This method implements exponential backoff if there are 429 errors from Azure Key Vault
        private static int getWaitTime(int retryCount)
        {
            int waitTime = (int)(Math.Pow(2, retryCount) * 100L);
            return waitTime;
        }
    }
}
