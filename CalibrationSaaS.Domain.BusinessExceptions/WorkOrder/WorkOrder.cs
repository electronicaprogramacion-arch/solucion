using System;

namespace CalibrationSaaS.Domain.BusinessExceptions.WorkOrder
{
    [Serializable]
    public class ExistingWorkOrderException : Exception
    {
        public int workOrderId;
        public ExistingWorkOrderException() { }
        public ExistingWorkOrderException(string message) : base(message)
        {

        }
        public ExistingWorkOrderException(string message, Exception inner) : base(message, inner)
        {

        }

        public ExistingWorkOrderException(string message, Exception inner, CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder workOrder) : base(message, inner)
        {
            this.workOrderId =   workOrder.WorkOrderId;
        }
    }
}
