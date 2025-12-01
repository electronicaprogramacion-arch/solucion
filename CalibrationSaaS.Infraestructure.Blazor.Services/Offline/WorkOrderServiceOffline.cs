using IndexedDB.Blazor;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using ProtoBuf.Grpc;
using System;
//using Blazored.LocalStorage;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Services.Offline
{
    public class WorkOrderServiceOffline : Application.Services.IWorkOrderService<CallContext>
    {
        //private readonly ILocalStorageService localStorageService;

        //public SampleServiceOffline(ILocalStorageService localStorageService)

        private readonly IIndexedDbFactory DbFactory;

        public WorkOrderServiceOffline(IIndexedDbFactory dbFactory)
        {
            //this.localStorageService = localStorageService;
            this.DbFactory = dbFactory;
        }

        public ValueTask<WorkOrder> CreateWorkOrder(WorkOrder workOrderDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<WorkOrder> DeleteWorkOrder(WorkOrder workOrder, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<WorkOrder> GetWorkOrderByID(WorkOrder workOrder, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<WorkOrderResultSet> GetWorkOrders(CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<WorkOrder> UpdateWorkOrder(WorkOrder DTO, CallContext context)
        {
            throw new NotImplementedException();
        }
    }
}
