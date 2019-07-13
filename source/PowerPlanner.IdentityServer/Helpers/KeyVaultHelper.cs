using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PowerPlanner.IdentityServer.Helpers
{
    public static class KeyVaultHelper
    {
        private static KeyVaultClient _keyVaultClient;
        private const string _vaultBaseUrl = "https://powerplannervault.vault.azure.net";

        static KeyVaultHelper()
        {
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            _keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
        }

        public static Task<SecretBundle> GetSecretAsync(string secretName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _keyVaultClient.GetSecretAsync(_vaultBaseUrl, secretName, cancellationToken);
        }
    }
}
