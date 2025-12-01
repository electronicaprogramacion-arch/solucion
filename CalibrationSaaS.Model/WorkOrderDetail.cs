using System;
using System.Collections.Generic;

namespace CalibrationSaaS.Models
{
    public partial class WorkOrderDetail
    {
        public WorkOrderDetail()
        {
            CalibrationResult = new HashSet<CalibrationResult>();
            EquipmentCondition = new HashSet<EquipmentCondition>();
        }

        public int WorkOrderDetailId { get; set; }
        public int WorkOderId { get; set; }
        public int TenantId { get; set; }
        public int PieceOfEquipmentId { get; set; }
        public bool? IsAccredited { get; set; }
        public bool? IsService { get; set; }
        public bool? IsRepair { get; set; }
        public short Status { get; set; }
        public string Comments { get; set; }
        public string TechnicianName { get; set; }
        public string StandardDescription { get; set; }
        public decimal Humidity { get; set; }
        public decimal Temperature { get; set; }

        public virtual PieceOfEquipment PieceOfEquipment { get; set; }
        public virtual WorkOrder WorkOder { get; set; }
        public virtual ICollection<CalibrationResult> CalibrationResult { get; set; }
        public virtual ICollection<EquipmentCondition> EquipmentCondition { get; set; }
    }
}
