using System;

namespace CalibrationSaaS.Domain.BusinessExceptions.Customer
{
    [Serializable]
    public class ExistingCustomerAddressException : Exception
    {
        public string CustomerAddressName;
        public ExistingCustomerAddressException() { }
        public ExistingCustomerAddressException(string message) : base(message)
        {

        }
        public ExistingCustomerAddressException(string message, Exception inner) : base(message, inner)
        {

        }

      
    }
}
