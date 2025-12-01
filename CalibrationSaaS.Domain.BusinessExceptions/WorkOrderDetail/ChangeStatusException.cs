using System;

namespace CalibrationSaaS.Domain.BusinessExceptions.WorkOrderDetail
{
    [Serializable]
    public class ChangeStatusException : Exception
    {
        public int Id;
        public ChangeStatusException() { }
        public ChangeStatusException(string message) : base(message)
        {

        }
        public ChangeStatusException(string message, Exception inner) : base(message, inner)
        {

        }

        public ChangeStatusException(string message, Exception inner, CalibrationSaaS.Domain.Aggregates.Entities.WorkOrderDetail DTO) : base(message, inner)
        {
            //this.workOderId =   workOrder.WorkOrderId;

            Id = DTO.WorkOrderDetailID;

        }
    }
}
