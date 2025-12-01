using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.Services
{

   
    public interface IBaseServices<T,R>
    {
        ValueTask<T> Create(T DTO);
        ValueTask<R> GetAll();

        ValueTask<T> Delete(T DTO);

        ValueTask<T> GetByID(T DTO);


    }
}
