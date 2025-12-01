using IndexedDB.Blazor;

using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
//using Blazored.LocalStorage;
using System.Threading.Tasks;
using CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite;
using Microsoft.EntityFrameworkCore;
using CalibrationSaaS.Data.EntityFramework;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Infraestructure.EntityFramework.DataAccess;
using Helpers.Controls.ValueObjects;
using CalibrationSaaS.Application.Services;
using static SQLite.SQLite3;
using SqliteWasmHelper;

namespace CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite
{
    public class POEServiceOffline<TContext> : Application.Services.IPieceOfEquipmentService<CallContext>
        where TContext:DbContext, ICalibrationSaaSDBContextBase, ICalibrationSaaSDBContextBaseOff
    {
        //private readonly ILocalStorageService localStorageService;

        //public SampleServiceOffline(ILocalStorageService localStorageService)

        private readonly ISqliteWasmDbContextFactory<TContext> DbFactory;

        public POEServiceOffline(ISqliteWasmDbContextFactory<TContext> dbFactory)
        {
            //this.localStorageService = localStorageService;
            this.DbFactory = dbFactory;
        }

        public ValueTask<CalibrationType> CreateConfiguration(CalibrationType DTO, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Uncertainty> CreateUncertainty(TableChanges<Uncertainty> PieceOfEquipmentDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<CalibrationSubType> DeleteConfiguration(CalibrationSubType DTO, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<PieceOfEquipment> DeletePieceOfEquipment(PieceOfEquipment PieceOfEquipmentDTO, CallContext context)
        {
             IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            var a= await wod.DeletePieceOfEquipment(PieceOfEquipmentDTO);

            return a;
        }

        public ValueTask<CalibrationSubType> EnableConfiguration(CalibrationSubType DTO, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResultSet<PieceOfEquipment>> GetAllPeripheralsPag(Pagination<PieceOfEquipment> pagination, CallContext context = default)
        {
            throw new NotImplementedException();
        }


        public async ValueTask<PieceOfEquipmentResultSet> GetAllWeightSets(PieceOfEquipment DTO, CallContext context)
        {
            IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            var result = await wod.GetAllWeightSets(DTO);

            if (result != null)
            {
                return new PieceOfEquipmentResultSet { PieceOfEquipments = result.ToList() };
            }

            return null;

        }

        public async Task<ResultSet<PieceOfEquipment>> GetAllWeightSetsPag(Pagination<PieceOfEquipment> pagination, CallContext context = default)
        {
            

            
             IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            var a= await wod.GetAllWeightSetsPag(pagination);

            return a;

        }


        public ValueTask<ICollection<Force>> GetCalculatesForISOandASTM(ISOandASTM ISOandASTM, CallContext context)
        {
            throw new NotImplementedException();
        }
        public string GetComponent(CallContext context)
        {

            if(context.RequestHeaders == null)
            {
                return null;
            }

            var header = context.RequestHeaders.Where(x => x.Key.ToLower() == "component").FirstOrDefault();
            //var user = context.ServerCallContext.GetHttpContext();

            string com = null;

            if (header != null)
            {
                com = header.Value;


            }

            return com;
        }

        public async ValueTask<CalibrationType> GetDynamicConfiguration(CalibrationType CalibrationType, CallContext context = default)
        {
            IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            var c = GetComponent(context);

            var a = await wod.GetDynamicConfiguration(CalibrationType.CalibrationTypeId,c);

            return a;
        }

        public ValueTask<PieceOfEquipmentResultSet> GetPieceOfEquipment(Pagination<PieceOfEquipment> pagination, CallContext context)
        {
            throw new NotImplementedException();
        }

 
        public async ValueTask<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentBalanceByIndicator(Pagination<PieceOfEquipment> DTO, CallContext context = default)
        {
            IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            var a = await wod.GetPieceOfEquipmentBalanceByIndicator(DTO);

            return a;
        }

      

        public ValueTask<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentBalanceByPer(Pagination<PieceOfEquipment> DTO, CallContext context = default)
        {
            throw new NotImplementedException();
        }

 

        public async ValueTask<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentByCustomer(Pagination<PieceOfEquipment> Pagination, CallContext context)
        {


             IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            var a= await wod.GetPieceOfEquipmentByCustomer(Pagination);

            return a;

            //using (var db = await this.DbFactory.CreateDbContextAsync())
            //{
            //    //var a = await context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).Where(x=> x.CustomerId== DTO.CustomerID).OrderBy(x => x.Customer.Name).ToListAsync();


            //    var b = db.PieceOfEquipment.Where(POE => POE.CustomerId == Pagination.Entity.Customer.CustomerID).ToList();

            //    var a = (from POE in db.PieceOfEquipment
            //             where POE.EquipmentTemplate.EquipmentTypeID >= 3 && POE.CustomerId == Pagination.Entity.Customer.CustomerID
            //             select POE).AsQueryable();


            //    //return a;

            //    var filterQuery = Querys.POEWOFilter(Pagination.Filter);

            //    var queriable = a; // context.WorkOrder.Include(x => x.Customer);//context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();

            //    var simplequery = a;

            //    //var result = await QueryableExtensions.PaginationAndFilterQuery<PieceOfEquipment>(exprTree, pagination, context.PieceOfEquipment, queriable);
            //    var result = await queriable.PaginationAndFilterQueryOff<PieceOfEquipment>(Pagination, simplequery, filterQuery);

            //    return result;
            //}



        }

       

        public ValueTask<PieceOfEquipmentResultSet> GetPieceOfEquipmentByCustomerId(Customer DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<PieceOfEquipmentResultSet> GetPieceOfEquipmentByET(EquipmentTemplate DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<PieceOfEquipment>> GetPieceOfEquipmentByScale(string id, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<WorkOrderDetailResultSet> GetPieceOfEquipmentHistory(PieceOfEquipment DTO, CallContext context)
        {
               IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            var a= await wod.GetPieceOfEquipmentHistory(DTO.PieceOfEquipmentID);

            WorkOrderDetailResultSet res = new WorkOrderDetailResultSet();

            res.WorkOrderDetails = a.ToList();
            return res;
        }

        public ValueTask<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentIndicator(Pagination<PieceOfEquipment> DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

     

        public async ValueTask<PieceOfEquipment> GetPieceOfEquipmentXId(PieceOfEquipment DTO, CallContext context)
        {

            //using (var db = await this.DbFactory.CreateDbContextAsync())
            //{
            //    return await Task.FromResult(db.PieceOfEquipment.Where(x => x.PieceOfEquipmentID == PieceOfEquipmentDTO.PieceOfEquipmentID).FirstOrDefault());

            //}
            IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            var a= await wod.GetPieceOfEquipmentByID(DTO.PieceOfEquipmentID,"",GetComponent(context));

            return a;


        }

        public async ValueTask<ResultSet<PieceOfEquipment>> GetPiecesOfEquipmentXDueDate(Pagination<PieceOfEquipment> pagination, CallContext context)
        {
            
            
            IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            var a= await wod.GetPieceOfEquipment(pagination);

            return a;
            
            
            
            //using (var db = await this.DbFactory.CreateDbContextAsync())
            //{
            //    var filterQuery = Querys.POEFilterNew(pagination);

               

            //    var queriable = db.PieceOfEquipment.AsQueryable(); //.Include(x => x.EquipmentTemplate).ThenInclude(x => x.EquipmentTypeObject).Include(d => d.Customer).AsQueryable();

            //    var simplequery = db.PieceOfEquipment.AsQueryable();
            //    //if (pagination.Filter == null)
            //    //{
            //    //    pagination.Filter = "Delete";
            //    //}
            //    //var result = await QueryableExtensions.PaginationAndFilterQuery<PieceOfEquipment>(exprTree, pagination, context.PieceOfEquipment, queriable);
            //    var result = await queriable.PaginationAndFilterQueryOff<PieceOfEquipment>(pagination, simplequery, filterQuery);

            //    if(result !=null && result?.List?.Count > 0)
            //    {
            //         foreach (var item in result.List)
            //    {
            //            var r = db.EquipmentTemplate.Where(x => x.EquipmentTemplateID == item.EquipmentTemplateId).FirstOrDefault();

            //            item.EquipmentTemplate = r;
            //    }
            //    }

            //     if(result !=null && result?.List?.Count > 0)
            //    {
            //         foreach (var item in result.List)
            //    {
            //            var r = db.Customer.Where(x => x.CustomerID == item.CustomerId).FirstOrDefault();

            //            item.Customer = r;
            //    }
            //    }
               

            //    return result;
            }

      

        public async ValueTask<ResultSet<PieceOfEquipment>> GetPOEByTestCodePag(Pagination<PieceOfEquipment> pagination, CallContext context = default)
        {
            IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            return await wod.GetPOEByTestCodePag(pagination);
        }

        public async ValueTask<ResultSet<POE_Scale>> GetPOEScale(Pagination<POE_Scale> pagination, CallContext context = default)
        {
            IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            return await wod.GetPOEScale(pagination);
        }

        public async ValueTask<IEnumerable<PieceOfEquipment>> GetResolutionByLenght(IEnumerable<PieceOfEquipment> DTO, CallContext context = default)
        {
            IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            return await wod.GetResolutionByLenght(DTO);
        }

        public async ValueTask<IEnumerable<PieceOfEquipment>> GetResolutionByMass(IEnumerable<PieceOfEquipment> DTO, CallContext context = default)
        {
            IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            return await wod.GetResolutionByMass(DTO);
        }

        public async ValueTask<ICollection<PieceOfEquipment>> GetTemperatureStandard(CallContext context = default)
        {
            IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            var result = await wod.GetTemperatureStandard();
            if (result != null)
            {
                return result.ToList(); //new PieceOfEquipmentResultSet { PieceOfEquipments = result.ToList() };
            }
            return null;
        }

        public async ValueTask<ResultSet<Uncertainty>> GetUncertainty(Pagination<Uncertainty> Uncertainty, CallContext context)
        {
            IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            return await wod.GetUncertainty(Uncertainty);


        }

        public async ValueTask<PieceOfEquipment> PieceOfEquipmentCreate(PieceOfEquipment item, CallContext context)
        {

            IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);
            
            item.IsOffline = true;
            
            var a= await wod.InsertPieceOfEquipment(item,GetComponent(context));

            return a;

            

        }

        public async Task<ResultSet<WeightSet>> SaveWeights(ICollection<WeightSet> W, CallContext context = default)
        {
            IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            return await wod.SaveWeights(W);
        }

        public async ValueTask<PieceOfEquipment> UpdateIndicator(PieceOfEquipment PieceOfEquipmentDTO, CallContext context)
        {
            IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            return await wod.UpdateIndicator(PieceOfEquipmentDTO);
        }

        public async ValueTask<PieceOfEquipment> UpdatePieceOfEquipment(PieceOfEquipment PieceOfEquipmentDTO, CallContext context)
        {
            IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            return await wod.UpdatePieceOfEquipment(PieceOfEquipmentDTO,GetComponent(context));
        }

        public async ValueTask<ICollection<Force>> CalculateUncertainty(List<Force> forces, int iso, CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<ResultSet<PieceOfEquipment>> GetPiecesOfEquipmentChildren(Pagination<PieceOfEquipment> pagination, CallContext context)
        {
            IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            return await wod.GetPieceOfEquipmentChildren(pagination);
        }

        public ValueTask<ICollection<PieceOfEquipment>> GetPieceOfEquipmentChildrenAll(PieceOfEquipment PieceOfEquipmentDTO, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<ResultSet<PieceOfEquipment>> GetSelectPOEChildren(Pagination<PieceOfEquipment> pagination, CallContext context = default)
        {
            IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            return await wod.GetSelectPOEChildren(pagination);
        }

        public async ValueTask<PieceOfEquipment> UpdateChildPieceOfEquipment(PieceOfEquipment pieceOfEquipmentDTO, CallContext context = default)
        {
            IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            return await wod.UpdateChildPieceOfEquipment(pieceOfEquipmentDTO);
        }
    }
}
