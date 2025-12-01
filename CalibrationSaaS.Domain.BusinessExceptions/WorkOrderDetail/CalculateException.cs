using System;

namespace CalibrationSaaS.Domain.BusinessExceptions.WorkOrderDetail
{
    [Serializable]
    public class CalculateException : Exception
    {
        public int Id;
        public CalculateException() { }
        public CalculateException(string message) : base(message)
        {

        }
        public CalculateException(string message, Exception inner) : base(message, inner)
        {

        }

        public CalculateException(string message, Exception inner, CalibrationSaaS.Domain.Aggregates.Entities.WorkOrderDetail DTO) : base(message, inner)
        {
            //this.workOderId =   workOrder.WorkOrderId;

            Id = DTO.WorkOrderDetailID;

        }
    }
}
