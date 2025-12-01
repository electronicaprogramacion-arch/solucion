using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Data.EntityFramework;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Infraestructure.EntityFramework.DataAccess;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using IndexedDB.Blazor;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;
using Reports.Domain.ReportViewModels;
using SqliteWasmHelper;
using System;
using System.Collections.Generic;
using System.Linq;
//using Blazored.LocalStorage;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite
{
    public class WODServiceOffline<TContext> : Application.Services.IWorkOrderDetailServices<CallContext>
        where TContext:DbContext, ICalibrationSaaSDBContextBase, ICalibrationSaaSDBContextBaseOff
    {
        //private readonly ILocalStorageService localStorageService;

        //public SampleServiceOffline(ILocalStorageService localStorageService)

        private ISqliteWasmDbContextFactory<TContext> DbFactory;

        public WODServiceOffline(ISqliteWasmDbContextFactory<TContext> dbFactory)
        {
            //this.localStorageService = localStorageService;
            this.DbFactory = dbFactory;
        }

        //public ValueTask<WorkOrderDetail> CalculateValuesByID(WorkOrderDetail DTO)
        //{
        //    throw new NotImplementedException();
        //}

       

        public async ValueTask<WorkOrderDetail> ChangeStatus(WorkOrderDetail item, CallContext context)
        {
            item.IsModifiedOff = true;
            item.CurrentStatusID = 2;

            var a = await SaveWod(item,context);

            return a;

        }

        public ValueTask<WorkDetailHistory> ChangeStatusComplete(WorkDetailHistory dto, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<WorkOrderDetail> Create(WorkOrderDetail DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Certificate> CreateCertificate(Certificate DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<TestCode> CreateTestCode(TestCode item, CallContext context)
        {
            IWorkOrderDetailRepository wod = new WODRepositoryEF<TContext>(DbFactory);

            return  await wod.CreateTestCode(item);
        }
        public async ValueTask<WorkOrderDetail> Reset(WorkOrderDetail DTO, CallContext context)
        {
            IWorkOrderDetailRepository wod = new WODRepositoryEF<TContext>(DbFactory);

            var a = await wod.Delete(DTO,true);

            return a;
        }
        public async ValueTask<WorkOrderDetail> Delete(WorkOrderDetail DTO, CallContext context)
        {
            IWorkOrderDetailRepository wod = new WODRepositoryEF<TContext>(DbFactory);

            var a= await wod.Delete(DTO);

            return a;
        }

        public ValueTask<TestCode> DeleteTestCode(TestCode item, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<WorkOrderDetailResultSet> GetAll(CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<WorkOrderDetailResultSet> GetAllEnabled(CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<WorkOrderDetail> GetByID(WorkOrderDetail DTO, CallContext context)
        {
            //using (var db = await this.DbFactory.CreateDbContextAsync())
            //{

            //    return await Task.FromResult(db.WorkOrderDetail.Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).FirstOrDefault());
            //}
            IWorkOrderDetailRepository wod = new WODRepositoryEF<TContext>(DbFactory);

            var a= await wod.GetByID(DTO);

            return a;

        }

        public ValueTask<ICollection<WorkOrderDetail>> GetByTechnician(User DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<ResultSet<WorkOrderDetail>> GetByTechnicianPag(Pagination<WorkOrderDetail> pagination, CallContext context)
        {
            //throw new NotImplementedException();
            IWorkOrderDetailRepository wod = new WODRepositoryEF<TContext>(DbFactory);

            var a= await wod.GetByTechnicianPag(pagination);

            return a;


        }


        public Task<List<CalibrationSubType>> GetCalibrationSubtype(CallContext context)
        {
            IWorkOrderDetailRepository Repository = new WODRepositoryEF<TContext>(DbFactory);

            return Repository.GetCalibrationSubType();
        }

        public ValueTask<Certificate> GetCertificate(WorkOrderDetail DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<int>> GetChartPie(CallContext context)
        {
            IWorkOrderDetailRepository Repository = new WODRepositoryEF<TContext>(DbFactory);

            return Repository.GetChartPie();

        }

        public async ValueTask<WorkOrderDetail> GetConfiguredWeights(WorkOrderDetail DTO, CallContext context)
        {

            IWorkOrderDetailRepository Repository = new WODRepositoryEF<TContext>(DbFactory);

            var bc = DTO.BalanceAndScaleCalibration;

            var bc2 = await Repository.GetConfiguredWeights(DTO.WorkOrderDetailID, bc);

            DTO.BalanceAndScaleCalibration = bc2;

            return DTO;
        }

        public ValueTask<WorkOrderDetailHistoryResultSet> GetHistory(WorkOrderDetail DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public Task<InstrumentThread> GetInstrumentThread(WorkOrderDetail DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public Task<List<GenericCalibrationResult2>> GetResultsTable(WorkOrderDetail DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<IEnumerable<Status>> GetStatus(CallContext context)
        {

           IWorkOrderDetailRepository wod = new WODRepositoryEF<TContext>(DbFactory);

            return await wod.GetStatus();


        }

        public async ValueTask<TestCode> GetTestCodeByID(TestCode item, CallContext context)
        {
            IWorkOrderDetailRepository wod = new WODRepositoryEF<TContext>(DbFactory);

            return await wod.GetTestCodeByID(item.TestCodeID);
        }

        public async ValueTask<ResultSet<TestCode>> GetTestCodes(Pagination<TestCode> pagination, CallContext context)
        {
            IWorkOrderDetailRepository wod = new WODRepositoryEF<TContext>(DbFactory);

            return await wod.GetTestCodes(pagination);
        }

        public Task<TestCode> GetTestCodeXName(TestCode DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<int>> GetTotals(CallContext context)
        {
            IWorkOrderDetailRepository Repository = new WODRepositoryEF<TContext>(DbFactory);

            return Repository.GetTotals();
        }

        public Task<IEnumerable<KeyValueDate>> GetWOCountPerDay(CallContext context)
        {
            IWorkOrderDetailRepository Repository = new WODRepositoryEF<TContext>(DbFactory);

            return Repository.GetWOCountPerDay();
        }

        public Task<IEnumerable<KeyValueDate>> GetWODCountPerDay(CallContext context)
        {
            IWorkOrderDetailRepository Repository = new WODRepositoryEF<TContext>(DbFactory);

            return Repository.GetWODCountPerDay();
        }

        public async ValueTask<ResultSet<WorkOrderDetail>> GetWods(Pagination<WorkOrderDetail> pag, CallContext context)
        {
            IWorkOrderDetailRepository wod = new WODRepositoryEF<TContext>(DbFactory);

            return await wod.GetWods(pag);
        }

        public async ValueTask<ResultSet<WorkOrderDetail>> GetWodsFromQuery(Pagination<WorkOrderDetail> pag, CallContext context)
        {
            IWorkOrderDetailRepository wod = new WODRepositoryEF<TContext>(DbFactory);

            return await wod.GetWodsFromQuery(pag);
        }

        public async Task<WOD_ParametersTable> GetWOD_Parameter(WOD_ParametersTable DTO, CallContext context)
        {
            IWorkOrderDetailRepository wod = new WODRepositoryEF<TContext>(DbFactory);

            return await wod.GetWOD_Parameter(DTO);
        }

        public async ValueTask<WorkOrderDetail> GetWorkOrderDetailByID(WorkOrderDetail DTO, CallContext context)
        {
            IWorkOrderDetailRepository wod = new WODRepositoryEF<TContext>(DbFactory);

            return await wod.GetWorkOrderDetailByID(DTO.WorkOrderDetailID);
        }

        public ValueTask<WorkOrderDetailResultSet> GetWorkOrderDetailXWorkOrder(CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<WorkOrderDetail> SaveWod(WorkOrderDetail item, CallContext context)
        {
            try
            {
                IWorkOrderDetailRepository wod = new WODRepositoryEF<TContext>(DbFactory);


                item.IsModifiedOff = true;

                var a= await wod.ChangeStatus(item);

                var b = await wod.GetByID(item);

                a = null;

                return b;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async ValueTask<WorkOrderDetail> SaveWodOff(WorkOrderDetail DTO, CallContext context)
        {
             try
            {
                IWorkOrderDetailRepository wod = new WODRepositoryEF<TContext>(DbFactory);
                
                

                var a = await wod.ChangeStatus(DTO);

                return a;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public ValueTask<WorkOrderDetail> GetByIDPreviousCalibration(WorkOrderDetail DTO, CallContext context)
        {
            throw new NotImplementedException();
        }
        public async ValueTask<IEnumerable<WorkOrderDetail>> GetWorkOrderDetailChildren(WorkOrderDetail workOrderDetail, CallContext context)
        {
            IWorkOrderDetailRepository wod = new WODRepositoryEF<TContext>(DbFactory);

            var result = await wod.GetWorkOrderDetailChildren(workOrderDetail.WorkOrderDetailID);

            return result;
        }

        public ValueTask<IEnumerable<ChildrenView>> GetChildrenView(int workOrderId)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<ChildrenView>> GetChildrenView(WorkOrderDetail workOrderDetail, CallContext context)
        {
            throw new NotImplementedException();
        }
    }
}
