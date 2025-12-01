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
using Helpers.Controls.ValueObjects;
using CalibrationSaaS.Application.Services;

namespace CalibrationSaaS.Infraestructure.Blazor.Services.Offline
{
    public class POEServiceOffline : Application.Services.IPieceOfEquipmentService<CallContext>
    {
        //private readonly ILocalStorageService localStorageService;

        //public SampleServiceOffline(ILocalStorageService localStorageService)

        private readonly IIndexedDbFactory DbFactory;

        public POEServiceOffline(IIndexedDbFactory dbFactory)
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

        public ValueTask<PieceOfEquipment> DeletePieceOfEquipment(PieceOfEquipment PieceOfEquipmentDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<CalibrationSubType> EnableConfiguration(CalibrationSubType DTO, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResultSet<PieceOfEquipment>> GetAllPeripheralsPag(Pagination<PieceOfEquipment> pagination, CallContext context = default)
        {
            throw new NotImplementedException();
        }


        public ValueTask<PieceOfEquipmentResultSet> GetAllWeightSets(PieceOfEquipment DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultSet<PieceOfEquipment>> GetAllWeightSetsPag(Pagination<PieceOfEquipment> pagination, CallContext context = default)
        {
            //TODO
            var DTO = pagination.Entity;

            var filterQuery = Querys.FilterWeightSets(pagination);

            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                var queriable = db.PieceOfEquipment.OrderBy(c => c.DueDate).AsQueryable();
                //context.PieceOfEquipment.AsNoTracking().Include(x => x.WeightSets).ThenInclude(x => x.UnitOfMeasure)
                //.Include(x => x.UnitOfMeasure).OrderBy(c => c.DueDate).AsQueryable();

                var simplequery = db.PieceOfEquipment.AsQueryable(); ;

                //var result = await QueryableExtensions.PaginationAndFilterQuery<PieceOfEquipment>(exprTree, pagination, context.PieceOfEquipment, queriable);
                var result = await queriable.PaginationAndFilterQueryOff<PieceOfEquipment>(pagination, simplequery, filterQuery);

                return result;
            }




            //return null;
        }

      

        public ValueTask<ICollection<Force>> GetCalculatesForISOandASTM(ISOandASTM ISOandASTM, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<CalibrationType> GetDynamicConfiguration(CalibrationType CalibrationType, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<PieceOfEquipmentResultSet> GetPieceOfEquipment(Pagination<PieceOfEquipment> pagination, CallContext context)
        {
            throw new NotImplementedException();
        }

       

        public ValueTask<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentBalanceByIndicator(Pagination<PieceOfEquipment> DTO, CallContext context = default)
        {
            throw new NotImplementedException();
        }

    

        public ValueTask<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentBalanceByPer(Pagination<PieceOfEquipment> DTO, CallContext context = default)
        {
            throw new NotImplementedException();
        }

    
        public async ValueTask<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentByCustomer(Pagination<PieceOfEquipment> Pagination, CallContext context)
        {


            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                //var a = await context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).Where(x=> x.CustomerId== DTO.CustomerID).OrderBy(x => x.Customer.Name).ToListAsync();


                var b = db.PieceOfEquipment.Where(POE => POE.CustomerId == Pagination.Entity.Customer.CustomerID).ToList();

                var a = (from POE in db.PieceOfEquipment
                         where POE.EquipmentTemplate.EquipmentTypeID >= 3 && POE.CustomerId == Pagination.Entity.Customer.CustomerID
                         select POE).AsQueryable();


                //return a;

                var filterQuery = Querys.POEWOFilter(Pagination.Filter);

                var queriable = a; // context.WorkOrder.Include(x => x.Customer);//context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();

                var simplequery = a;

                //var result = await QueryableExtensions.PaginationAndFilterQuery<PieceOfEquipment>(exprTree, pagination, context.PieceOfEquipment, queriable);
                var result = await queriable.PaginationAndFilterQueryOff<PieceOfEquipment>(Pagination, simplequery, filterQuery);

                return result;
            }



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

        public ValueTask<WorkOrderDetailResultSet> GetPieceOfEquipmentHistory(PieceOfEquipment DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentIndicator(Pagination<PieceOfEquipment> DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

     

        public async ValueTask<PieceOfEquipment> GetPieceOfEquipmentXId(PieceOfEquipment PieceOfEquipmentDTO, CallContext context)
        {

            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                return await Task.FromResult(db.PieceOfEquipment.Where(x => x.PieceOfEquipmentID == PieceOfEquipmentDTO.PieceOfEquipmentID).FirstOrDefault());

            }



        }

        public async ValueTask<ResultSet<PieceOfEquipment>> GetPiecesOfEquipmentXDueDate(Pagination<PieceOfEquipment> pagination, CallContext context)
        {
            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                var filterQuery = Querys.POEFilterNew(pagination);

               

                var queriable = db.PieceOfEquipment.AsQueryable(); //.Include(x => x.EquipmentTemplate).ThenInclude(x => x.EquipmentTypeObject).Include(d => d.Customer).AsQueryable();

                var simplequery = db.PieceOfEquipment.AsQueryable();
                //if (pagination.Filter == null)
                //{
                //    pagination.Filter = "Delete";
                //}
                //var result = await QueryableExtensions.PaginationAndFilterQuery<PieceOfEquipment>(exprTree, pagination, context.PieceOfEquipment, queriable);
                var result = await queriable.PaginationAndFilterQueryOff<PieceOfEquipment>(pagination, simplequery, filterQuery);

                if(result !=null && result?.List?.Count > 0)
                {
                     foreach (var item in result.List)
                {
                        var r = db.EquipmentTemplate.Where(x => x.EquipmentTemplateID == item.EquipmentTemplateId).FirstOrDefault();

                        item.EquipmentTemplate = r;
                }
                }

                 if(result !=null && result?.List?.Count > 0)
                {
                     foreach (var item in result.List)
                {
                        var r = db.Customer.Where(x => x.CustomerID == item.CustomerId).FirstOrDefault();

                        item.Customer = r;
                }
                }
               

                return result;
            }
        }

      

        public ValueTask<ResultSet<PieceOfEquipment>> GetPOEByTestCodePag(Pagination<PieceOfEquipment> pagination, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResultSet<POE_Scale>> GetPOEScale(Pagination<POE_Scale> pagination, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<PieceOfEquipment>> GetResolutionByLenght(IEnumerable<PieceOfEquipment> DTO, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<PieceOfEquipment>> GetResolutionByMass(IEnumerable<PieceOfEquipment> DTO, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ICollection<PieceOfEquipment>> GetTemperatureStandard(CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResultSet<Uncertainty>> GetUncertainty(Pagination<Uncertainty> Uncertainty, CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<PieceOfEquipment> PieceOfEquipmentCreate(PieceOfEquipment item, CallContext context)
        {

            try
            {
                using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {

                    if (item != null)
                    {
                        var c = db.PieceOfEquipment.Where(x => x.PieceOfEquipmentID == item.PieceOfEquipmentID).FirstOrDefault();

                        item.OfflineID = Guid.NewGuid().ToString();
                        if (c == null)
                        {
                            db.PieceOfEquipment.Add(item);
                        }
                        else
                        {
                            db.PieceOfEquipment.Remove(item);


                            db.PieceOfEquipment.Add(item);
                        }
                    }

                    await db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return item;



        }

        public Task<ResultSet<WeightSet>> SaveWeights(ICollection<WeightSet> W, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<PieceOfEquipment> UpdateIndicator(PieceOfEquipment PieceOfEquipmentDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<PieceOfEquipment> UpdatePieceOfEquipment(PieceOfEquipment PieceOfEquipmentDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        Task<ResultSet<WeightSet>> IPieceOfEquipmentService<CallContext>.SaveWeights(ICollection<WeightSet> W, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ICollection<Force>> CalculateUncertainty(List<Force> forces, int iso, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResultSet<PieceOfEquipment>> GetPiecesOfEquipmentChildren(Pagination<PieceOfEquipment> pagination, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResultSet<PieceOfEquipment>> GetSelectPOEChildren(Pagination<PieceOfEquipment> pagination, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<PieceOfEquipment> UpdateChildPieceOfEquipment(PieceOfEquipment pieceOfEquipmentDTO, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ICollection<PieceOfEquipment>> GetPieceOfEquipmentChildrenAll(PieceOfEquipment PieceOfEquipmentDTO, CallContext context = default)
        {
            throw new NotImplementedException();
        }
    }
}
