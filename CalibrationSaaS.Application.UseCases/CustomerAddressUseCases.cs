using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.BusinessExceptions.Customer;
using CalibrationSaaS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.UseCases
{
    public class CustomerAddressUseCases
    {
        private readonly ICustomerAddressRepository customerAddressRepository;

        public CustomerAddressUseCases(ICustomerAddressRepository customerAddressRepository)
        {
            this.customerAddressRepository = customerAddressRepository;
        }

        //A DTO is also a Value Object, however a DTO has an integrity meaning you should not modify its properties while in a Value Object it is accepted.
        public async Task<int> CreateCustomerAddress(CustomerAddress customerAddressDTO)
        {
            //var doesCustomerAddressExist = await this.customerAddressRepository.GetCustomerAddressByName(customerAddressDTO.CustomerName, customerAddressDTO.TenantId);
            //if(doesCustomerAddressExist != null)
            //{
            //    throw new ExistingCustomerAddressException("This Customer Address is already in", null, doesCustomerAddressExist);
            //}
            customerAddressDTO.CustomerAddressId = 0;
            this.customerAddressRepository.InsertCustomerAddress(customerAddressDTO);
            await this.customerAddressRepository.Save();
            return customerAddressDTO.CustomerAddressId;
        }

        public async Task<IEnumerable<CustomerAddress>> GetCustomerAddress(TenantDTO tenantID)
        {
            return await customerAddressRepository.GetCustomersAddress(tenantID.TenantID);
        }
    }
}
