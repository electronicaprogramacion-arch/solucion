using IndexedDB.Blazor;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CalibrationSaaS.Data.EntityFramework;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Infraestructure.EntityFramework.DataAccess;
using Helpers.Controls.ValueObjects;
using SqliteWasmHelper;

namespace CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite
{
    public class UOMServiceOffline<TContext> : IUOMService<CallContext>
        where TContext:DbContext, ICalibrationSaaSDBContextBase, ICalibrationSaaSDBContextBaseOff
    {
        private readonly ISqliteWasmDbContextFactory<TContext> DbFactory;

        public UOMServiceOffline(ISqliteWasmDbContextFactory<TContext> dbFactory)
        {
            this.DbFactory = dbFactory;
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


             IUOMRepository wod = new UOMRepositoryEF<TContext>(DbFactory);

            var a = await wod.GetAll();

            var r = new UOMResultSet();

            r.UnitOfMeasureList = a.ToList();

            return r;
        }

        public async ValueTask<UOMResultSet> GetAllEnabled(CallContext context)
        {


            IUOMRepository wod = new UOMRepositoryEF<TContext>(DbFactory);

            var a = await wod.GetAll();

            var r = new UOMResultSet();

            r.UnitOfMeasureList = a.ToList();

            return r;
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
            IUOMRepository wod = new UOMRepositoryEF<TContext>(DbFactory);


            var a = await wod.GetTypes();

            return a;


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