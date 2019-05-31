using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Client.Models
{
    public class TokenModel
    {
        public string UserName { get; set; }

        public long Expirt { get; set; }
    }
}
