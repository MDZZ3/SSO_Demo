using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SSO.Client2.Helper
{
    public class CookieHelper
    {
        public static ClaimsPrincipal GetPrincipal(string UserName)
        {
            Claim[] claims = new Claim[]
            {
               new Claim(ClaimTypes.Name,UserName)
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims);
            return new ClaimsPrincipal(identity);
        }

    }
}
