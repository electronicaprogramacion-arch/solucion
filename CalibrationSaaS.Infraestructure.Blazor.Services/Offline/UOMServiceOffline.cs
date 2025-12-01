using IndexedDB.Blazor;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helpers.Controls.ValueObjects;




namespace CalibrationSaaS.Infraestructure.Blazor.Services.Offline
{
    internal class UOMServiceOffline : IUOMService<CallContext>
    {
        private readonly IIndexedDbFactory DbFactory;

        public UOMServiceOffline(IIndexedDbFactory DbFactory)
        {
            this.DbFactory = DbFactory;
        }

        //public ValueTask<UnitOfMeasure> Conversion(UnitOfMeasure _Source, UnitOfMeasure _Target, UnitOfMeasure _Base, UnitOfMeasure uncertain, CallContext context)
        //{
        //    throw new System.NotImplementedException();
        //}

        public ValueTask<UnitOfMeasure> Create(UnitOfMeasure DTO, CallContext context)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask<UnitOfMeasureType> CreateType(UnitOfMeasureType DTO, CallContext context)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask<UnitOfMeasure> Delete(UnitOfMeasure DTO, CallContext context)
        {
            throw new System.NotImplementedException();
        }

        public async ValueTask<UOMResultSet> GetAll(CallContext context)
        {


            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                return await Task.FromResult(new UOMResultSet { UnitOfMeasureList = db.UnitOfMeasure.ToList() });

            }
        }

        public async ValueTask<UOMResultSet> GetAllEnabled(CallContext context)
        {


            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                return await Task.FromResult(new UOMResultSet { UnitOfMeasureList = db.UnitOfMeasure.Where(x => x.IsEnabled == true).ToList() });

            }
        }

        public ValueTask<ResultSet<UnitOfMeasure>> GetAllPag(Pagination<UnitOfMeasure> Pagination, CallContext context)
        {
            throw new System.NotImplementedException();
        }

     

        public ValueTask<UnitOfMeasure> GetByID(UnitOfMeasure DTO, CallContext context)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask<UOMResultSet> GetByType(UnitOfMeasureType Type, CallContext context)
        {
            throw new System.NotImplementedException();
        }

        public async ValueTask<ICollection<UnitOfMeasureType>> GetTypes(CallContext context)
        {


            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                return await Task.FromResult(db.UnitOfMeasureType.ToList());

            }
        }

        public ValueTask<UnitOfMeasure> Update(UnitOfMeasure DTO, CallContext context)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask<UnitOfMeasureType> UpdateType(UnitOfMeasureType DTO, CallContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}