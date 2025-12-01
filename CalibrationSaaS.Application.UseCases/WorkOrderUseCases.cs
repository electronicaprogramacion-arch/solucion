using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.BusinessExceptions.Customer;
using CalibrationSaaS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.UseCases
{
    public class WorkOrderUseCases
    {
        private readonly IWorkOrderRepository workOrderRepository;

        public WorkOrderUseCases(IWorkOrderRepository workOrderRepository)
        {
            this.workOrderRepository = workOrderRepository;
        }

        //A DTO is also a Value Object, however a DTO has an integrity meaning you should not modify its properties while in a Value Object it is accepted.
       

        public async Task<IEnumerable<WorkOrder>> GeWorkOrder()
        {
            return await workOrderRepository.GetWorkOrder();
        }


       


        public async Task<WorkOrder> DeleteWorkOrder(int workOrderId)
        {
            var result = await workOrderRepository.DeleteWorkOrder(workOrderId);
            return result;
        }

        public async Task<WorkOrder> UpdateWorkOrder(WorkOrder DTO)
        {
            var result = await this.workOrderRepository.UpdateWorkOrder(DTO);
            await this.workOrderRepository.Save();

            return result;
        }

        public async Task<WorkOrder> CreateWorkOrder(WorkOrder DTO)
        {
            //no se debe generar WOD sin POE
            if(DTO.WorkOrderDetails == null || DTO.WorkOrderDetails.Count == 0)
            {
                DTO.WorkOrderDetails = null;
               
            }
          

            var result = await this.workOrderRepository.InsertWokOrder(DTO);
            await this.workOrderRepository.Save();

            return result;
        }

        public async Task<WorkOrder> GetWorkOrderByID(int workOrderId)
        {
            var result = await workOrderRepository.GetWorkOrderByID(workOrderId);
            return result;
        }

        public async Task<IEnumerable<WorkOrderDetail>> GetWorkOrderDetailXWorkOrder(int id)
        {
            return await workOrderRepository.GetWorkOrderDetailXWorkOrder(id);
        }


     

    }
}
