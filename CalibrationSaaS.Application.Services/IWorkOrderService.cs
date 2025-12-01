using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.Services
{
    [ServiceContract(Name = "CalibrationSaaS.Application.Services.WorkOrder")]
    public interface IWorkOrderService<T>
    {       
        ValueTask<WorkOrder> CreateWorkOrder(WorkOrder workOrderDTO, T context);

        ValueTask<WorkOrderResultSet> GetWorkOrders(T context);

        ValueTask<WorkOrder> DeleteWorkOrder(WorkOrder workOrder, T context);
        ValueTask<WorkOrder> UpdateWorkOrder(WorkOrder DTO, T context);
        ValueTask<WorkOrder> GetWorkOrderByID(WorkOrder workOrder, T context);




    }
}
