using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Helpers.Controls.ValueObjects;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.Services
{



    [ServiceContract(Name = "CalibrationSaaS.Application.Services.UOMService")]
    public interface IUOMService<T>
    {

        ValueTask<UOMResultSet> GetAll(T context);

        ValueTask<ResultSet<UnitOfMeasure>> GetAllPag(Pagination<UnitOfMeasure> Pagination, T context);

        ValueTask<UnitOfMeasure> Create(UnitOfMeasure DTO,  T context);

        ValueTask<UnitOfMeasure> Update(UnitOfMeasure DTO, T context);

        ValueTask<UnitOfMeasure> Delete(UnitOfMeasure DTO, T context);

        ValueTask<UnitOfMeasure> GetByID(UnitOfMeasure DTO, T context);

        ValueTask<UOMResultSet> GetByType(UnitOfMeasureType Type, T context);

        //ValueTask<UnitOfMeasure> Conversion(UnitOfMeasure _Source, UnitOfMeasure _Target, 
        //    UnitOfMeasure _Base, UnitOfMeasure uncertain, T context);

        ValueTask<ICollection<UnitOfMeasureType>> GetTypes( T context);

       

        ValueTask<UnitOfMeasureType> UpdateType(UnitOfMeasureType DTO, T context);



        ValueTask<UnitOfMeasureType> CreateType(UnitOfMeasureType DTO, T context);


        ValueTask<UOMResultSet> GetAllEnabled(T context);


       

    }
}
