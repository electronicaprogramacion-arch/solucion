using System;

namespace CalibrationSaaS.Domain.BusinessExceptions.Customer
{
    [Serializable]
    public class ExistingCustomerException : Exception
    {
        public string CustomerName;
        public ExistingCustomerException() { }
        public ExistingCustomerException(string message) : base(message)
        {

        }
        public ExistingCustomerException(string message, Exception inner) : base(message, inner)
        {

        }

        public ExistingCustomerException(string message, Exception inner, CalibrationSaaS.Domain.Aggregates.Entities.Customer customer) : base(message, inner)
        {
            this.CustomerName = customer.Name;
        }
    }
}
