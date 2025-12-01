using CalibrationSaaS.Domain.Aggregates.Entities;
using System;

namespace CalibrationSaaS.Domain.BusinessExceptions
{
    [Serializable]
    public class CertificationException : Exception
    {
       

        public string IdentityException { get; set; }

      
        public CertificationException() {

           
        }
        public CertificationException(string message) : base(message)
        {

        }
        public CertificationException(string message, Exception inner) : base(message, inner)
        {
            
        }

      
    }
}
