using System;

namespace CalibrationSaaS.Domain.BusinessExceptions.Customer
{
    [Serializable]
    public class ExistingContactException : Exception
    {
        public string CustomerAddressName;
        public ExistingContactException() { }
        public ExistingContactException(string message) : base(message)
        {

        }
        public ExistingContactException(string message, Exception inner) : base(message, inner)
        {

        }

      
    }
}
