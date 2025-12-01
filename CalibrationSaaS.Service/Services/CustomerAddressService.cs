using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Application.UseCases;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
namespace CalibrationSaaS.Infraestructure.GrpcServices.Services
{
    public class CustomerAddressService : ICustomerAddressService
    {
        private readonly CustomerAddressUseCases customerAddressLogic;

        public CustomerAddressService(CustomerAddressUseCases customerAddressLogic)
        {
            this.customerAddressLogic = customerAddressLogic;
        }

        public async ValueTask<CustomerAddress> CreateCustomerAddress(CustomerAddress customerAddressDTO)
        {
            var result = await customerAddressLogic.CreateCustomerAddress(customerAddressDTO);
          
            return customerAddressDTO;
        }

        public async ValueTask<CustomerAddressResultSet> GetCustomerAddress(TenantDTO tenantID)
        {
            CustomerAddressResultSet result = new CustomerAddressResultSet 
            { CustomerAddress = (List<CustomerAddress>)await customerAddressLogic.GetCustomerAddress(tenantID) };
            return result;
        }
    }
}
