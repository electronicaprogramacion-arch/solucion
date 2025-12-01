using CalibrationSaaS.Domain.Aggregates.Entities;
using System;

namespace CalibrationSaaS.Domain.BusinessExceptions
{
    [Serializable]
    public class ExistingException : Exception
    {
       

        public string IdentityException { get; set; }

      
        public ExistingException() {

           
        }
        public ExistingException(string message) : base(message)
        {

        }
        public ExistingException(string message, Exception inner) : base(message, inner)
        {
            
        }

      
    }
}
