using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.Services
{

    [ServiceContract(Name = "CalibrationSaaS.Application.Services.AddressServices")]
    public interface IAddressServices 
    {


        ValueTask<PhoneNumbersResultSet> GetPhoneByCustomer(Customer DTO);

        ValueTask<AddressResultSet> GetAll();

        ValueTask<Address> Create(Address DTO);

        ValueTask<Address> Delete(Address DTO);

        ValueTask<Address> GetByID(Address DTO);

        ValueTask<AddressResultSet> GetByType();
      
        ValueTask<Address> GetByCustomer(Customer DTO);

        ValueTask<AddressResultSet> GetAllByCustomer(Customer DTO);


    }
}
