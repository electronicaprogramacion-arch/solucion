using CalibrationSaaS.Domain.Aggregates.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Domain.Repositories
{
    public interface ICustomerAddressRepository
    {
        void InsertCustomerAddress(CustomerAddress newCustomerAddress);

        Task<IEnumerable<CustomerAddress>> GetCustomersAddress(int tenantID);

        Task<CustomerAddress> GetCustomerAddressByID(int id, int tenantID);

//        Task<CustomerAddress> GetCustomerAddressByName(string name, int tenantID);

        Task<CustomerAddress> DeleteCustomerAddress(int id, int tenantID);

        void UpdateCustomerAddress(CustomerAddress newCustomerAddress);

        Task<bool> Save();
    }
}
