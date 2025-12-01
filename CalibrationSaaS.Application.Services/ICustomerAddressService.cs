using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.Services
{
    [ServiceContract(Name = "CalibrationSaaS.Application.Services.CustomerAddress")]
    public interface ICustomerAddressService
    {       
        ValueTask<CustomerAddress> CreateCustomerAddress(CustomerAddress customerAddressDTO);

        ValueTask<CustomerAddressResultSet> GetCustomerAddress(TenantDTO tenantID);


    }
}
