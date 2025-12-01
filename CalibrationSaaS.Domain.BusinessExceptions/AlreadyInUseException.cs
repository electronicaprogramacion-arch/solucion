using CalibrationSaaS.Domain.Aggregates.Entities;
using System;

namespace CalibrationSaaS.Domain.BusinessExceptions
{
    [Serializable]
    public class AlreadyInUseException : Exception
    {
       

        public string IdentityException { get; set; }

      
        public AlreadyInUseException() {

           
        }
        public AlreadyInUseException(string message) : base(message)
        {

        }
        public AlreadyInUseException(string message, Exception inner) : base(message, inner)
        {
            
        }

      
    }

     [Serializable]
    public class SchemaException : Exception
    {
       

        public string IdentityException { get; set; }

      
        public SchemaException() {

           
        }
        public SchemaException(string message) : base(message)
        {

        }
        public SchemaException(string message, Exception inner) : base(message, inner)
        {
            
        }

      
    }

}
