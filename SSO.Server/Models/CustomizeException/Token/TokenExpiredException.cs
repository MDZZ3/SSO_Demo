using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Server.Models.CustomizeException.Token
{
   public class TokenExpiredException:TokenException
    {
        public TokenExpiredException()
        {

        }
        public TokenExpiredException(string message) : base(message)
        {

        }

        public TokenExpiredException(string message, TokenException innerException) : base(message, innerException)
        {

        }
    }
}
