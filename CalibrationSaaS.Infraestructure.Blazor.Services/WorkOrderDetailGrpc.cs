using Blazor.IndexedDB.Framework;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Grpc.Core;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Services
{
    public class WorkOrderDetailGrpc : IDisposable, IWorkOrderDetailServices<CallContext>
    {

        private CallContext Context;

        private IWorkOrderDetailServices<CallContext> Service;


        private readonly dynamic DbFactory;

        

        public WorkOrderDetailGrpc(Func<dynamic, Application.Services.IWorkOrderDetailServices<CallContext>> _service,
            dynamic _DbFactory, CallOptions callOptions)
        {
            DbFactory = _DbFactory;

            Service = _service(DbFactory);
           
                Context = callOptions; // new CallContext();
            
           
        }


        public WorkOrderDetailGrpc(Func<dynamic, Application.Services.IWorkOrderDetailServices<CallContext>> _service,
            dynamic _DbFactory)
        {
            DbFactory = _DbFactory;

            Service = _service(DbFactory);
           

        }

        public WorkOrderDetailGrpc(IWorkOrderDetailServices<CallContext> _service)
        {
            Service = _service;

            Context = new CallContext();
        }

        //public ValueTask<WorkOrderDetail> CalculateValuesByID(WorkOrderDetail DTO)
        //{
        //    throw new NotImplementedException();
        //}

        public async ValueTask<WorkOrderDetail> Create(WorkOrderDetail DTO, CallContext context = default)
        {
            return await Service.Create(DTO, Context);
        }

        public async ValueTask<WorkOrderDetail> Reset(WorkOrderDetail DTO, CallContext context = default)
        {
            return await Service.Reset(DTO, Context);
        }

        public async ValueTask<WorkOrderDetail> Delete(WorkOrderDetail DTO, CallContext context = default)
        {
            return await Service.Delete(DTO, Context);
        }

        public void Dispose()
        {

        }

        public async ValueTask<WorkOrderDetailResultSet> GetAll(CallContext context = default)
        {
            return await Service.GetAll(Context);
        }

        public async ValueTask<WorkOrderDetailResultSet> GetAllEnabled(CallContext context)
        {
            return await Service.GetAllEnabled(Context);
        }

        public async ValueTask<WorkOrderDetail> GetByID(WorkOrderDetail DTO, CallContext context = default)
        {
            var a = await Service.GetByID(DTO, Context);


            return a;
        }

        public async ValueTask<WorkOrderDetail> GetByIDSync(WorkOrderDetail DTO, CallContext context = default)
        {
            var a = await Service.GetByID(DTO, Context);
            return a;
        }

        public async ValueTask<WorkOrderDetailHistoryResultSet> GetHistory(WorkOrderDetail DTO, CallContext context = default)
        {
            return await Service.GetHistory(DTO, Context);
        }

        public async ValueTask<IEnumerable<Domain.Aggregates.Entities.Status>> GetStatus(CallContext context = default)
        {
            return await Service.GetStatus(Context);
        }

        public async ValueTask<WorkOrderDetailResultSet> GetWorkOrderDetailXWorkOrder(WorkOrder DTO, CallContext context = default)
        {
            return await Service.GetWorkOrderDetailXWorkOrder(DTO, Context);
        }

        public async ValueTask<WorkOrderDetail> ChangeStatus(WorkOrderDetail DTO, CallContext context = default)
        {
            var result = await Service.ChangeStatus(DTO, Context);
            return result;

        }

        //YPPP
        public async ValueTask<WorkDetailHistory> ChangeStatusComplete(WorkDetailHistory DTO, CallContext context = default)
        {
            var result = await Service.ChangeStatusComplete(DTO, Context);
            return result;

        }

        public async ValueTask<Certificate> CreateCertificate(Certificate DTO, CallContext context = default)
        {
            return await Service.CreateCertificate(DTO, Context);
        }
        // END YPPP

        public async ValueTask<WorkOrderDetail> GetWorkOrderDetailByID(WorkOrderDetail DTO, CallContext context = default)
        {
            return await Service.GetWorkOrderDetailByID(DTO, Context);
        }

        public async ValueTask<WorkOrderDetail> SaveWod(WorkOrderDetail DTO, CallContext context = default)
        {
            return await Service.SaveWod(DTO, Context);
        }

        public async ValueTask<ICollection<WorkOrderDetail>> GetByTechnician(User DTO, CallContext context = default)
        {
            return await Service.GetByTechnician(DTO, Context);
        }

        public async ValueTask<ResultSet<WorkOrderDetail>> GetByTechnicianPag(Pagination<WorkOrderDetail> pagination, CallContext context = default)
        {
            return await Service.GetByTechnicianPag(pagination, Context);
        }

        public async ValueTask<WorkOrderDetail> GetConfiguredWeights(WorkOrderDetail DTO, CallContext context = default)
        {
            return await Service.GetConfiguredWeights(DTO, Context);
        }

        public async ValueTask<Certificate> GetCertificate(WorkOrderDetail DTO, CallContext context = default)
        {
            var result = await Service.GetCertificate(DTO, Context);
            return result;
        }

        public async Task<IEnumerable<int>> GetChartPie(CallContext context)
        {
            var result = await Service.GetChartPie(Context);
            return result;
        }

        public async Task<IEnumerable<int>> GetTotals(CallContext context)
        {
            var result = await Service.GetTotals(Context);
            return result;
        }

        public async Task<IEnumerable<KeyValueDate>> GetWODCountPerDay(CallContext context)
        {
            var result = await Service.GetWODCountPerDay(Context);
            return result;
        }

        public async Task<IEnumerable<KeyValueDate>> GetWOCountPerDay(CallContext context)
        {
            var result = await Service.GetWOCountPerDay(Context);
            return result;
        }


        // REMOVED: This method was causing performance issues by fetching ALL TestCodes
        // Use GetTestCodes(Pagination<TestCode> pagination) instead for proper pagination

        public async ValueTask<ResultSet<TestCode>> GetTestCodes(Pagination<TestCode> pagination, CallContext context=default)
        {
            var result = await Service.GetTestCodes(pagination, Context);
            return result;
        }

        public async ValueTask<TestCode> CreateTestCode(TestCode item, CallContext context=default )
        {
             var result = await Service.CreateTestCode(item, Context);
            return result;
        }

        public async ValueTask<TestCode> DeleteTestCode(TestCode item, CallContext context=default)
        {
             var result = await Service.DeleteTestCode(item, Context);
            return result;
        }

        public async ValueTask<TestCode> GetTestCodeByID(TestCode item, CallContext context=default)
        {
            var result = await Service.GetTestCodeByID(item, Context);
            return result;
        }


        public async Task<TestCode> GetTestCodeXName(TestCode item, CallContext context = default)
        {
            var result = await Service.GetTestCodeXName(item, Context);
            return result;
        }

        public async Task<List<CalibrationSubType>> GetCalibrationSubtype(CallContext context = default)
        {

            var result = await Service.GetCalibrationSubtype(Context);
            return result.ToList();
        }


        public async Task<InstrumentThread> GetInstrumentThread(WorkOrderDetail wod, CallContext context)
        {
            var result = await Service.GetInstrumentThread(wod, Context);
            return result;
        }

        public async Task<List<GenericCalibrationResult2>> GetResultsTable(WorkOrderDetail wod, CallContext context)
        {
            var result = await Service.GetResultsTable(wod, Context);
            return result;
        }

        public async Task<WOD_ParametersTable> GetWOD_Parameter(WOD_ParametersTable item, CallContext context)
        {
            var result = await Service.GetWOD_Parameter(item, Context);
            return result;
        }


       
        
          public async ValueTask<WorkOrderDetail> SaveWodOff(WorkOrderDetail DTO, CallContext context = default)
        {
            return await Service.SaveWodOff(DTO, Context);
        }

        public async ValueTask<ResultSet<WorkOrderDetail>> GetWods(Pagination<WorkOrderDetail> pag, CallContext context)
        {
            return await Service.GetWods(pag, Context);
        }

        public async ValueTask<ResultSet<WorkOrderDetail>> GetWodsFromQuery(Pagination<WorkOrderDetail> pag, CallContext context)
        {
            return await Service.GetWodsFromQuery(pag, Context);
        }

        public async ValueTask<WorkOrderDetail> GetByIDPreviousCalibration(WorkOrderDetail DTO, CallContext context = default)
        {
            var a = await Service.GetByIDPreviousCalibration(DTO, Context);

            return a;
        }

        public async ValueTask<IEnumerable<WorkOrderDetail>> GetWorkOrderDetailChildren(WorkOrderDetail workOrderDetail, CallContext context=default)
        {
            return await Service.GetWorkOrderDetailChildren(workOrderDetail, Context);
        }

        public async ValueTask<IEnumerable<ChildrenView>> GetChildrenView(WorkOrderDetail workOrderDetail, CallContext context = default)
        {
            return await Service.GetChildrenView(workOrderDetail, Context);
        }
    }
    
}