using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Helpers.Controls.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Domain.Repositories
{
   public  interface IUOMRepository
    {
        Task<IEnumerable<UnitOfMeasure>> GetHeaderAll();
              Task<bool> Exists(UnitOfMeasure DTO);

             Task<UnitOfMeasure> Update(UnitOfMeasure DTO);

            Task<IEnumerable<UnitOfMeasure>> GetAll();

             Task<ResultSet<UnitOfMeasure>> GetAllPag(Pagination<UnitOfMeasure> pagination);

        Task<UnitOfMeasure> Create(UnitOfMeasure DTO);

            Task<UnitOfMeasure> Delete(UnitOfMeasure DTO);

            Task<UnitOfMeasure> GetByID(UnitOfMeasure DTO);

            Task<IEnumerable<UnitOfMeasure>> GetByType(UnitOfMeasureType Type);

        //ValueTask<UnitOfMeasure> Conversion(UnitOfMeasure _Source, UnitOfMeasure _Target,
        //    UnitOfMeasure _Base, UnitOfMeasure uncertain, T context);

            Task<bool> Save();


            Task<ICollection<UnitOfMeasureType>> GetTypes();

           Task<UnitOfMeasureType> UpdateType(UnitOfMeasureType DTO);

        

            Task<UnitOfMeasureType> CreateType(UnitOfMeasureType DTO);




    }
}
