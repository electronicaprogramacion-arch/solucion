using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Test
{
    public class CustomerServiceDummy
    {
        private List<Customer> Customers { get; set; }

        public CustomerServiceDummy()
        {
            Customers = new List<Customer>();
            Customers.Add(new Customer() { CustomerID = 1, Name = "Engineering Inc" });
            Customers.Add(new Customer() { CustomerID = 2, Name = "Wind Energy Inc" });
            Customers.Add(new Customer() { CustomerID = 3, Name = "SolarLight Inc" });
            Customers.Add(new Customer() { CustomerID = 4, Name = "GoodbyeOil Inc" });
            Customers.Add(new Customer() { CustomerID = 5, Name = "Quantium Inc" });
        }

        public async ValueTask<Customer> CreateCustomer(Customer customerDTO)
        {
            customerDTO.CustomerID = this.Customers.Count + 1;
            await Task.Run(() => this.Customers.Add(customerDTO));
            return customerDTO;
        }

        public async ValueTask<CustomerResultSet> GetCustomers(TenantDTO tenantID)
        {
            List<Customer> _inustomers = null;
            await Task.Run(() => _inustomers = this.Customers);
            return new CustomerResultSet { Customers = _inustomers };
        }

        public async  ValueTask<Customer> GetCustomersByID(Customer tenantID)
        {
           return new Customer() { CustomerID = 1, Name = "Engineering Inc" };
        }

        public ValueTask<CustomerResultSet> GetCustomers()
        {
            throw new NotImplementedException();
        }

        public ValueTask<Customer> DeleteCustomer(Customer customerDTO)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCustomer(Customer customerDTO)
        {
            throw new NotImplementedException();
        }
    }
}
