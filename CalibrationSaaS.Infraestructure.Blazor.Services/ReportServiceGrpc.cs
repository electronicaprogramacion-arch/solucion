using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Grpc.Core;
using ProtoBuf.Grpc;
using System;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Services
{
    public class ReportServiceGRPC : IDisposable, IReportService<CallContext>
    {

        private CallContext context;

        private readonly IReportService<CallContext> service;

        public ReportServiceGRPC(IReportService<CallContext> _service)
        {
            service = _service;
            context = new CallOptions();
        }

        public ReportServiceGRPC(IReportService<CallContext> _service, CallContext _context)
        {
            service = _service;
            context = _context;
        }

        public void Dispose()
        {

        }

        public async ValueTask<ReportResultSet> GetWorkOrderDetailXIdRep(WorkOrderDetail wo)
        {
            var workOrderDetail = await service.GetWorkOrderDetailXIdRep(wo);

            return workOrderDetail;

        }

        public async ValueTask<ReportResultSet> GetWorkOrderDetailXIdRepWithSave(WorkOrderDetail wo)
        {
            var workOrderDetail = await service.GetWorkOrderDetailXIdRepWithSave(wo);

            return workOrderDetail;

        }


        public async ValueTask<string> GetReportUncertaintyBudgetComp(WorkOrderDetail wo, CallContext context)
        {
            var result = await service.GetReportUncertaintyBudgetComp(wo, context);

            return result;
        }

        public async ValueTask<string> GetReportUncertaintyBudget(Linearity li, CallContext context)
        {
            var result = await service.GetReportUncertaintyBudget(li, context);

            return result;
        }

        public async ValueTask<string> GetSticker(WorkOrderDetail wo, CallContext context)
        {
            var result = await service.GetSticker(wo, context);

            return result;
        }

        public string GetUrlServer()
        {
            var result = service.GetUrlServer();
            return result;
        }
    }
}
