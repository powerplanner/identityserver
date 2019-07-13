using Konscious.Security.Cryptography;
using PowerPlanner.IdentityServer.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlanner.IdentityServer.Identity
{
    public static class PasswordManager
    {
        private class DbCredentials
        {
            public long AccountId { get; set; }
            public string Username { get; set; }
            public string SaltedPassword { get; set; }
            public string Salt { get; set; }
        }

        public static long IsValid(SqlConnection conn, string username, string password)
        {
            var credentials = GetCredentials(conn, username);

            if (credentials == null)
            {
                return -1;
            }

            if (credentials.Salt == null)
            {
                if (password != credentials.SaltedPassword)
                {
                    return -1;
                }

                UpgradePassword(conn, username, password);
                return credentials.AccountId;
            }

            string incomingSaltedPassword = SaltPassword(password, credentials.Salt);
            if (incomingSaltedPassword == credentials.SaltedPassword)
            {
                return credentials.AccountId;
            }
            return -1;
        }

        private static void UpgradePassword(SqlConnection conn, string username, string password)
        {
            long accountId;
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "select top 1 AccountId from Account where Username = @username";
                cmd.Parameters.AddWithValue("username", username);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        accountId = reader.GetInt64(0);
                    }
                    else
                    {
                        return;
                    }
                }
            }

            UpdatePassword(conn, accountId, password);
        }

        public static void UpdatePassword(SqlConnection conn, long accountId, string newPassword)
        {
            string newSalt = CreateSalt();
            string newSaltedPassword = SaltPassword(newPassword, newSalt);

            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "update Account set Password = @password, PasswordSalt = @passwordSalt where AccountId = @accountId";
                cmd.Parameters.AddWithValue("accountId", accountId);
                cmd.Parameters.AddWithValue("password", newSaltedPassword);
                cmd.Parameters.AddWithValue("passwordSalt", newSalt);

                cmd.ExecuteNonQuery();
            }
        }

        public static bool CreateAccount(SqlConnection conn, string username, string password, string email)
        {
            string salt = CreateSalt();
            string saltedPassword = SaltPassword(password, salt);

            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "insert into Account (Username, Password, PasswordSalt, Email, WeekOneStartsOn) values (@username, @password, @passwordSalt, @email, @weekOneStartsOn) select cast(scope_identity() as bigint)";

                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", saltedPassword);
                cmd.Parameters.AddWithValue("passwordSalt", salt);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("weekOneStartsOn", DateTools.Last(DayOfWeek.Sunday));

                try
                {
                    object answer = cmd.ExecuteScalar();

                    if (answer is long)
                        return true; // To get the account ID, app should request to login
                    else
                        return false;
                }

                catch
                {
                    //likely means there was a duplicate username
                    return false;
                }
            }
        }

        private static string SaltPassword(string password, string salt)
        {
            var argon2 = new Argon2i(Encoding.UTF8.GetBytes(password))
            {
                Salt = Encoding.UTF8.GetBytes(salt),
                DegreeOfParallelism = 16,
                MemorySize = 1024,
                Iterations = 40
            };

            return Convert.ToBase64String(argon2.GetBytes(128));
        }

        private static RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();
        private static string CreateSalt()
        {
            var bytes = new byte[32];
            _rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private static DbCredentials GetCredentials(SqlConnection conn, string username)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "select top 1 AccountId, Password, PasswordSalt from Account where Username = @username";
                cmd.Parameters.AddWithValue("username", username);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new DbCredentials()
                        {
                            AccountId = reader.GetInt64(0),
                            Username = username,
                            SaltedPassword = reader.GetString(1),
                            Salt = reader.IsDBNull(2) ? null : reader.GetString(2)
                        };
                    }
                }
            }

            return null;
        }
    }
}
