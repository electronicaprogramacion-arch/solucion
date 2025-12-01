using CalibrationSaaS.Domain.Aggregates.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Domain.Repositories
{
    public interface IWorkOrderRepository
    {


       

        Task<WorkOrder> InsertWokOrder(WorkOrder newWorkOrder);

        Task<IEnumerable<WorkOrder>> GetWorkOrder();

        Task<WorkOrder> GetWorkOrderByID(int newWorkOrder);

        //Task<WorkOrder> GetCustomerAddressByName(string name, int tenantID);

        Task<WorkOrder> DeleteWorkOrder(int newWorkOrder);

        Task<WorkOrder> UpdateWorkOrder(WorkOrder newWorkOrder);

        Task<bool> Save();

        Task<IEnumerable<WorkOrderDetail>> GetWorkOrderDetailXWorkOrder(int id);

    }
     
}
