
using System;

namespace CalibrationSaaS.Domain.BusinessExceptions
{
    [Serializable]
    public class LoginException : Exception
    {
        

        public string IdentityException { get; set; }

       
        public LoginException() { }
        public LoginException(string message) : base(message)
        {

        }
        public LoginException(string message, Exception inner) : base(message, inner)
        {

        }

       
    }
}
