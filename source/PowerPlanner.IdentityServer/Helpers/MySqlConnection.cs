using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PowerPlanner.IdentityServer.Helpers
{
    public static class MySqlConnection
    {
        private static string _connectionString;
        private static Task _initializeTask;

        public static async Task<SqlConnection> CreateAsync()
        {
            await InitializeAsync();

            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }

        private static Task InitializeAsync()
        {
            if (_initializeTask != null)
            {
                if (_initializeTask.IsCompletedSuccessfully)
                {
                    return _initializeTask;
                }

                if (!_initializeTask.IsCanceled && !_initializeTask.IsFaulted)
                {
                    return _initializeTask;
                }
            }

            _initializeTask = InitializeHelperAsync();
            return _initializeTask;
        }

        private static async Task InitializeHelperAsync()
        {
            _connectionString = (await KeyVaultHelper.GetSecretAsync("dbConnectionString")).Value;
        }
    }
}
