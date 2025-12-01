using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.Services
{

    [ServiceContract(Name = "CalibrationSaaS.Application.Services.CompanyServices")]
    public interface ICompanyServices
    {
         ValueTask<UserResultSet>  GetAll();

          ValueTask<User> Create(User DTO);
       

        ValueTask<User> Delete(User DTO);

        ValueTask<User> GetByID(User DTO);

        ValueTask<UserResultSet> GetUsersByType();

       

        ValueTask<UserResultSet> GetUsersByCustomer(Customer CustomerDTO);

    }
}
