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
using Microsoft.EntityFrameworkCore;
using CalibrationSaaS.Infraestructure.EntityFramework.Helpers;
using CalibrationSaaS.Data.EntityFramework;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Infraestructure.EntityFramework.DataAccess;
using Helpers.Controls.ValueObjects;
using SqliteWasmHelper;


namespace CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite
{
    public class AssetsServiceOffline<TContext> : Application.Services.IAssetsServices<CallContext> 
        where TContext:DbContext, ICalibrationSaaSDBContextBase, ICalibrationSaaSDBContextBaseOff
    {
        //private readonly ILocalStorageService localStorageService;

        //public SampleServiceOffline(ILocalStorageService localStorageService)

        private readonly ISqliteWasmDbContextFactory<TContext> DbFactory;

        public AssetsServiceOffline(ISqliteWasmDbContextFactory<TContext> dbFactory)
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


            IAssetsRepository ass = new AssetsRepositoryEF<TContext>(DbFactory);

           


            var workOrder = await ass.InsertWokOrder(DTO); 


            return workOrder;
        }

        public ValueTask<WeightSet> DeleteWeightSet(WeightSet DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<WorkOrder> DeleteWorkOrder(WorkOrder workOrder, CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<AddressResultSet> GetAddressByCustomerId(Customer DTO)
        {

             IAssetsRepository ass = new AssetsRepositoryEF<TContext>(DbFactory);


            var a= await ass.GetAddressByCustomerId(DTO.CustomerID);

            AddressResultSet cr = new AddressResultSet();

            cr.Addresses = (List<Address>)a;

            return cr; 

           


        }

        public async ValueTask<CalibrationTypeResultSet> GetCalibrationType()
        {

            IAssetsRepository ass = new AssetsRepositoryEF<TContext>(DbFactory);

            var a = await ass.GetCalibrationTypes();

            CalibrationTypeResultSet cr = new CalibrationTypeResultSet();

            cr.CalibrationTypes = a.ToList();

            return cr;

           


        }

        public async Task<ICollection<CertificatePoE>> GetCertificateXPoE(PieceOfEquipment DTO)
        {
            IAssetsRepository ass = new AssetsRepositoryEF<TContext>(DbFactory);


            var a = await ass.GetCertificateXPoE(DTO);

           

            return a;
        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task<ICollection<Certificate>> GetCertificateXWod(WorkOrderDetail DTO)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {


            IAssetsRepository ass = new AssetsRepositoryEF<TContext>(DbFactory);


            var a = await ass.GetCertificateByWod(DTO.WorkOrderDetailID);



            return a;


        }

        public async ValueTask<ContactResultSet> GetContactsByCustomerId(Customer DTO)
        {
                  IAssetsRepository ass = new AssetsRepositoryEF<TContext>(DbFactory);


            var a= await ass.GetContactsByCustomerId(DTO.CustomerID);

            ContactResultSet cr = new ContactResultSet();

            cr.Contacts = (List<Contact>)a;

            return cr; 
        }

        public ValueTask<PieceOfEquipmentResultSet> GetPieceOfEquipmentByCustomerId(Customer customerId)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<IEnumerable<WOStatus>> GetStatus(CallContext context = default)
        {
            IAssetsRepository ass = new AssetsRepositoryEF<TContext>(DbFactory);


            var a = await ass.GetStatus();



            return a;

        }

        public async ValueTask<UserResultSet> GetUsers()
        {
            using (var db = await this.DbFactory.CreateDbContextAsync())
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
            //using (var db = await this.DbFactory.CreateDbContextAsync())
            //{
            //    return await Task.FromResult(db.WorkOrder.Where(x => x.WorkOrderId == DTO.WorkOrderId).FirstOrDefault());

            //}
             IAssetsRepository ass = new AssetsRepositoryEF<TContext>(DbFactory);


            var a= await ass.GetWorkOrderByID(DTO.WorkOrderId);

            return a;

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



             IAssetsRepository ass = new AssetsRepositoryEF<TContext>(DbFactory);


            var a= await ass.GetWorkOrder(pagination);

            return a;



            //TODO
            //var DTO = pagination.Entity;

            //var filterQuery = Querys.WorkOrderFilter(pagination.Filter);

            //using (var db = await this.DbFactory.CreateDbContextAsync())
            //{
            //    var queriable = db.WorkOrder.AsQueryable();
                

            //    var simplequery = db.WorkOrder.AsQueryable(); ;

                
            //    var result = await queriable.PaginationAndFilterQuery<WorkOrder>(pagination, simplequery, filterQuery);

            //    return result;
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
