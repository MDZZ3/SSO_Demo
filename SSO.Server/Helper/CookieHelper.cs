using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SSO.Server.Helper
{
    public class CookieHelper
    {
        public static ClaimsPrincipal GetPrincipal(string Token)
        {
            Claim[] claims = new Claim[]
            {
               new Claim(ClaimTypes.Authentication,Token)
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims);
            return new ClaimsPrincipal(identity);
        }

    }
}
