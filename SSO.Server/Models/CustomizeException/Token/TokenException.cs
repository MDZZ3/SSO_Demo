using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Server.Models.CustomizeException.Token
{
    public class TokenException:Exception
    {
        public TokenException()
        {

        }
        public TokenException(string message) : base(message)
        {

        }

        public TokenException(string message, TokenException innerException) : base(message, innerException)
        {

        }
    }
}
