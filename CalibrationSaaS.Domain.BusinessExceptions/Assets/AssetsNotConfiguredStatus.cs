using System;

namespace CalibrationSaaS.Domain.BusinessExceptions.Assets
{
    [Serializable]
    public class AssetsNotConfiguredStatus : Exception
    {
        public int workOderId;
        public AssetsNotConfiguredStatus() { }
        public AssetsNotConfiguredStatus(string message) : base(message)
        {

        }
        public AssetsNotConfiguredStatus(string message, Exception inner) : base(message, inner)
        {

        }

        public AssetsNotConfiguredStatus(string message, Exception inner, CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder workOrder) : base(message, inner)
        {
            this.workOderId =   workOrder.WorkOrderId;
        }
    }
}
