using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Server.Models.CustomizeException.Token
{
    public class TokenSignatureException: TokenException
    {
        public TokenSignatureException()
        {

        }
        public TokenSignatureException(string message) : base(message)
        {

        }

        public TokenSignatureException(string message, TokenException innerException) : base(message, innerException)
        {

        }

    }
}
