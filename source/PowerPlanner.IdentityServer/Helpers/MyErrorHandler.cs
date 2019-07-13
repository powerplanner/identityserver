using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerPlanner.IdentityServer.Helpers
{
    public static class MyErrorHandler
    {
        public static string GetEmailError(string email)
        {
            if (email == null)
                return "Email was null.";

            if (email.Length > 150)
                return "Email was too long. It must be at most 150 characters.";

            if (string.IsNullOrWhiteSpace(email))
                return "Email was empty.";

            return null;
        }

        public static string GetUsernameError(string username)
        {
            if (username == null)
                return "Username was null.";

            if (username.Length < 1)
                return "You must enter a username!";

            if (username.Length > 50)
                return "Username was too long. It has to be 50 or less characters long.";

            //if (username.Length < 4)
            //    return "Username was too short. It must be at least 4 characters.";

            if (!areUsernameCharactersValid(username))
                return "Usernames can only contain A-Z, a-z, 0-9, and " + string.Join(", ", VALID_SPECIAL_USERNAME_SYMBOLS) + " characters.";

            return null;
        }

        private static readonly char[] VALID_SPECIAL_USERNAME_SYMBOLS = { '$', '-', '_', '.', '+', '!', '\'', '(', ')', ',' };

        private static bool areUsernameCharactersValid(string username)
        {
            for (int i = 0; i < username.Length; i++)
                if (!isUsernameCharacterValid(username[i]))
                    return false;

            return true;
        }

        private static bool isUsernameCharacterValid(char c)
        {
            if ((c >= 'A' && c <= 'Z') ||
                (c >= 'a' && c <= 'z') ||
                (c >= '0' && c <= '9') ||
                VALID_SPECIAL_USERNAME_SYMBOLS.Contains(c))
                return true;

            return false;
        }

        public static string GetPasswordError(string password)
        {
            if (password == null)
                return "Password was null.";

            if (password.Length != 64)
                return "Password was not correct length.";

            for (int i = 0; i < password.Length; i++)
                if (!isHex(password[i]))
                    return "Password had invalid characters.";

            return null;
        }

        private static bool isHex(char c)
        {
            if (c >= '0' && c <= '9')
                return true;

            if (c >= 'a' && c <= 'f')
                return true;

            return false;
        }

    }
}