using IndexedDB.Blazor;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
//using Blazored.LocalStorage;
using System.Threading.Tasks;
using Helpers.Controls.ValueObjects;
using Helpers.Controls;

namespace CalibrationSaaS.Infraestructure.Blazor.Services.Offline
{
    public class WODServiceOffline : Application.Services.IWorkOrderDetailServices<CallContext>
    {
        //private readonly ILocalStorageService localStorageService;

        //public SampleServiceOffline(ILocalStorageService localStorageService)

        private readonly IIndexedDbFactory DbFactory;

        public WODServiceOffline(IIndexedDbFactory dbFactory)
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
            try
            {
                using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {

                    if (item != null)
                    {
                        var c = db.WorkOrderDetail.Where(x => x.WorkOrderDetailID == item.WorkOrderDetailID).FirstOrDefault();

                        item.OfflineID = Guid.NewGuid().ToString();
                        if (c == null)
                        {
                            db.WorkOrderDetail.Add(item);
                        }
                        else
                        {
                            db.WorkOrderDetail.Remove(item);


                            db.WorkOrderDetail.Add(item);
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

        public ValueTask<TestCode> CreateTestCode(TestCode item, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<WorkOrderDetail> Delete(WorkOrderDetail DTO, CallContext context)
        {
            throw new NotImplementedException();
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
            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {

                return await Task.FromResult(db.WorkOrderDetail.Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).FirstOrDefault());
            }
        }

        public ValueTask<WorkOrderDetail> GetByIDPreviousCalibration(WorkOrderDetail DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ICollection<WorkOrderDetail>> GetByTechnician(User DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResultSet<WorkOrderDetail>> GetByTechnicianPag(Pagination<WorkOrderDetail> pagination, CallContext context)
        {
            throw new NotImplementedException();
        }

      

        public Task<List<CalibrationSubType>> GetCalibrationSubtype(CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Certificate> GetCertificate(WorkOrderDetail DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<int>> GetChartPie(CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<ChildrenView>> GetChildrenView(int workOrderId)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<ChildrenView>> GetChildrenView(WorkOrderDetail workOrderDetail, CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<WorkOrderDetail> GetConfiguredWeights(WorkOrderDetail DTO, CallContext context)
        {
            //using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            //{
            //    return await Task.FromResult(db.WorkOrderDetail.Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).FirstOrDefault());

            //}
            return null;
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

            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                return await Task.FromResult(db.Status.ToList());

            }


        }

        public ValueTask<TestCode> GetTestCodeByID(TestCode item, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResultSet<TestCode>> GetTestCodes(Pagination<TestCode> pagination, CallContext context)
        {
            throw new NotImplementedException();
        }

        public Task<TestCode> GetTestCodeXName(TestCode DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<int>> GetTotals(CallContext context)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<KeyValueDate>> GetWOCountPerDay(CallContext context)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<int>> GetWODCountPerDay(CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResultSet<WorkOrderDetail>> GetWods(Pagination<WorkOrderDetail> pag, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResultSet<WorkOrderDetail>> GetWodsFromQuery(Pagination<WorkOrderDetail> pag, CallContext context)
        {
            throw new NotImplementedException();
        }

        public Task<WOD_ParametersTable> GetWOD_Parameter(WOD_ParametersTable DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<WorkOrderDetail> GetWorkOrderDetailByID(WorkOrderDetail DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<WorkOrderDetail>> GetWorkOrderDetailChildren(WorkOrderDetail workOrderDetail, CallContext context) 
        {
            return new ValueTask<IEnumerable<WorkOrderDetail>>();
        }

        public ValueTask<WorkOrderDetailResultSet> GetWorkOrderDetailXWorkOrder(WorkOrder DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<WorkOrderDetail> Reset(WorkOrderDetail DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<WorkOrderDetail> SaveWod(WorkOrderDetail item, CallContext context)
        {
            try
            {
                using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {

                    if (item != null)
                    {
                        var c = db.WorkOrderDetail.Where(x => x.WorkOrderDetailID == item.WorkOrderDetailID).FirstOrDefault();

                        item.OfflineID = Guid.NewGuid().ToString();
                        if (c == null)
                        {
                            db.WorkOrderDetail.Add(item);
                        }
                        else
                        {
                            db.WorkOrderDetail.Remove(item);


                            db.WorkOrderDetail.Add(item);
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

        public ValueTask<WorkOrderDetail> SaveWodOff(WorkOrderDetail DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<KeyValueDate>> IWorkOrderDetailServices<CallContext>.GetWODCountPerDay(CallContext context)
        {
            throw new NotImplementedException();
        }

       
    }
}
