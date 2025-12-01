using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Application.UseCases;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using ProtoBuf.Grpc;
using CalibrationSaaS.Infraestructure.Grpc.Helpers;
using Microsoft.Extensions.Logging;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using Reports.Domain.ReportViewModels;

namespace CalibrationSaaS.Infraestructure.GrpcServices.Services
{
    public class WorkOrderService : IWorkOrderService<CallContext>
    {
        private readonly WorkOrderUseCases workOrderLogic;
        
        private readonly ValidatorHelper modelValidator;
        private readonly ILogger _logger;
        public WorkOrderService(WorkOrderUseCases workOrderLogic)
        {
            this.workOrderLogic = workOrderLogic;
        }

        public async ValueTask<Domain.Aggregates.Entities.WorkOrder> CreateWorkOrder(Domain.Aggregates.Entities.WorkOrder workOrderDTO, CallContext context)
        {
            var result = await workOrderLogic.CreateWorkOrder(workOrderDTO);
          
            return workOrderDTO;
        }

        public async ValueTask<WorkOrderResultSet> GetWorkOrders(CallContext context)
        {

            Domain.Aggregates.Entities.WorkOrder wo = new Domain.Aggregates.Entities.WorkOrder
            {
                WorkOrderId = 1,
                CustomerId = 1,
                Customer = new Domain.Aggregates.Entities.Customer { 
                 Name="TEST COMPANY"
                },
            };



            WorkOrderResultSet result = new WorkOrderResultSet { WorkOrders = new List<Domain.Aggregates.Entities.WorkOrder>() };//await workOrderLogic.GeWorkOrder() };

            result.WorkOrders.Add(wo);

            return result;
        }

        public async ValueTask<Domain.Aggregates.Entities.WorkOrder> DeleteWorkOrder(Domain.Aggregates.Entities.WorkOrder workOrder, CallContext context)
        {
            var result = await workOrderLogic.DeleteWorkOrder(workOrder.WorkOrderId);
            return result;
        }

        public async ValueTask<Domain.Aggregates.Entities.WorkOrder> UpdateWorkOrder(Domain.Aggregates.Entities.WorkOrder DTO, CallContext context)
        {
            var result = await this.workOrderLogic.UpdateWorkOrder(DTO);
          
            return result;
        }

        public async ValueTask<Domain.Aggregates.Entities.WorkOrder> GetWorkOrderByID(Domain.Aggregates.Entities.WorkOrder workOrder, CallContext context)
        {
            var result = await workOrderLogic.GetWorkOrderByID(workOrder.WorkOrderId);
            return result;
        }

     


    }
}
