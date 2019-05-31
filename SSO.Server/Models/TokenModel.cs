using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Server.Models
{
    public class TokenModel
    {
        public string UserName { get; set; }

        public long Expire { get; set; }
    }
}
