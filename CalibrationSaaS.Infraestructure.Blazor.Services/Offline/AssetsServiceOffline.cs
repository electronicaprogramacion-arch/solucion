using IndexedDB.Blazor;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.Aggregates.Views;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
//using Blazored.LocalStorage;
using System.Threading.Tasks;
using Helpers.Controls.ValueObjects;

namespace CalibrationSaaS.Infraestructure.Blazor.Services.Offline
{
    public class AssetsServiceOffline : Application.Services.IAssetsServices<CallContext>
    {
        //private readonly ILocalStorageService localStorageService;

        //public SampleServiceOffline(ILocalStorageService localStorageService)

        private readonly IIndexedDbFactory DbFactory;

        public AssetsServiceOffline(IIndexedDbFactory dbFactory)
        {
            //this.localStorageService = localStorageService;
            this.DbFactory = dbFactory;
        }

        //public ValueTask<Certificate> CreateCertificate(Certificate DTO)
        //{
        //    throw new NotImplementedException();
        //}

        public ValueTask<WeightSet> CreateWeightSet(WeightSet DTO)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<WorkOrder> CreateWorkOrder(WorkOrder DTO, CallContext context)
        {
            //Console.WriteLine("GetCustomers Offline");
            WorkOrderDetailResultSet resultSet = new WorkOrderDetailResultSet();
            try
            {
                using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {


                    var c = db.WorkOrder.Where(x => x.WorkOrderId == DTO.WorkOrderId).FirstOrDefault();
                    if (c == null)
                    {
                        db.WorkOrder.Add(DTO);
                    }
                    else
                    {
                        db.WorkOrder.Remove(DTO);
                        await db.SaveChanges();

                        db.WorkOrder.Add(DTO);
                    }

                    await db.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }



            return DTO;
        }

        public ValueTask<WeightSet> DeleteWeightSet(WeightSet DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<WorkOrder> DeleteWorkOrder(WorkOrder workOrder, CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<AddressResultSet> GetAddressByCustomerId(Customer customerId)
        {



            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                var cust = db.Customer.Where(x => x.CustomerID == customerId.CustomerID).FirstOrDefault();
                var a = cust.Aggregates;
                if (cust.Aggregates != null && cust.Aggregates.Count > 0)
                {
                    return await Task.FromResult(new AddressResultSet { Addresses = db.Address.Where(x => x.AggregateID == cust.Aggregates.ElementAtOrDefault(0).AggregateID).ToList() });
                }
                else
                {
                    return null;
                }



            }


        }

        public async ValueTask<CalibrationTypeResultSet> GetCalibrationType()
        {

            CalibrationTypeResultSet resultSet = new CalibrationTypeResultSet();

            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                return await Task.FromResult(new CalibrationTypeResultSet { CalibrationTypes = db.CalibrationType.ToList() });

            }


        }

        public Task<ICollection<CertificatePoE>> GetCertificateXPoE(PieceOfEquipment DTO)
        {
            throw new NotImplementedException();
        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task<ICollection<Certificate>> GetCertificateXWod(WorkOrderDetail DTO)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {


            //using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            //{
            //    return await Task.FromResult( db.Certificate.Where(x=>x.WorkOrderDetailId==DTO.WorkOrderDetailID ).ToList());

            //}

            return null;


        }

        public async ValueTask<ContactResultSet> GetContactsByCustomerId(Customer customerId)
        {
            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                var cust = db.Customer.Where(x => x.CustomerID == customerId.CustomerID).FirstOrDefault();
                var a = cust.Aggregates;
                if (cust.Aggregates != null && cust.Aggregates.Count > 0)
                {
                    List<Contact> cont = new List<Contact>();

                    foreach (var item in cust.Aggregates)
                    {
                        foreach (var item2 in item.Contacts)
                        {
                            cont.Add(item2);
                        }
                    }

                    return await Task.FromResult(new ContactResultSet { Contacts = cont });
                }
                else
                {
                    return null;
                }



            }
        }

        public ValueTask<PieceOfEquipmentResultSet> GetPieceOfEquipmentByCustomerId(Customer customerId)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<WOStatus>> GetStatus(CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<UserResultSet> GetUsers()
        {
            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                return await Task.FromResult(new UserResultSet { Users = db.User.ToList() });

            }

        }

        public ValueTask<UserResultSet> GetUsersByCustomerId(Customer customerId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<WeightSet>> GetWeightSetXPoE(PieceOfEquipment DTO)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<WorkOrder> GetWorkOrderByID(WorkOrder DTO, CallContext context)
        {
            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                return await Task.FromResult(db.WorkOrder.Where(x => x.WorkOrderId == DTO.WorkOrderId).FirstOrDefault());

            }
        }

        public ValueTask<ResultSet<WorkOrderDetailByCustomer>> GetWorkOrderDetailByCustomer(Pagination<WorkOrderDetailByCustomer> pagination, CallContext context)
        {
            throw new NotImplementedException();
        }

      

        public ValueTask<ResultSet<WorkOrderDetailByStatus>> GetWorkOrderDetailByEquipment(Pagination<WorkOrderDetailByStatus> pagination, CallContext context)
        {
            throw new NotImplementedException();
        }

     
        public ValueTask<ResultSet<WorkOrderDetailByStatus>> GetWorkOrderDetailByStatus(Pagination<WorkOrderDetailByStatus> pagination, CallContext context)
        {
            throw new NotImplementedException();
        }

    
        public async ValueTask<ResultSet<WorkOrder>> GetWorkOrders(Pagination<WorkOrder> pagination, CallContext context)
        {


            //TODO
            var DTO = pagination.Entity;

            var filterQuery = Querys.WorkOrderFilter(pagination.Filter);

            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                var queriable = db.WorkOrder.AsQueryable();
                //context.PieceOfEquipment.AsNoTracking().Include(x => x.WeightSets).ThenInclude(x => x.UnitOfMeasure)
                //.Include(x => x.UnitOfMeasure).OrderBy(c => c.DueDate).AsQueryable();

                var simplequery = db.WorkOrder.AsQueryable(); ;

                //var result = await QueryableExtensions.PaginationAndFilterQuery<PieceOfEquipment>(exprTree, pagination, context.PieceOfEquipment, queriable);
                var result = await queriable.PaginationAndFilterQueryOff<WorkOrder>(pagination, simplequery, filterQuery);

                return result;
            }



        }

     

        public ValueTask<ResultSet<WorkOrder>> GetWorkOrdersOff(Pagination<WorkOrder> pagination, CallContext context)
        {
            throw new NotImplementedException();
        }

   

        public ValueTask<WorkOrder> UpdateWorkOrder(WorkOrder DTO, CallContext context)
        {
            throw new NotImplementedException();
        }
    }
}
