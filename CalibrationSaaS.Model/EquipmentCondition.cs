using System;
using System.Collections.Generic;

namespace CalibrationSaaS.Models
{
    public partial class EquipmentCondition
    {
        public int EquipmentConditionId { get; set; }
        public int WorkOrderDetailId { get; set; }
        public bool IsAsFound { get; set; }
        public bool Value { get; set; }
        public string Label { get; set; }

        public virtual WorkOrderDetail WorkOrderDetail { get; set; }
    }
}
