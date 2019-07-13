using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Identity
{
    public static class Scopes
    {
        public const string AccountRead = "account.read";
        public const string AccountWrite = "account.write";
        public const string FullApp = "fullapp";
        public const string ParentsRead = "parents.read";
        public const string ParentsWrite = "parents.write";
    }
}
