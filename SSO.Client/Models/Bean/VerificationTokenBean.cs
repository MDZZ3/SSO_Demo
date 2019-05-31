using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Client.Models.Bean
{
    public class VerificationTokenBean
    {
        public string code { get; set; }

        public UserInfo info { get; set; }

        public string Message { get; set; }
    }
}
