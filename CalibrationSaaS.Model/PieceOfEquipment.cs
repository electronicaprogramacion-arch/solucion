using System;
using System.Collections.Generic;

namespace CalibrationSaaS.Models
{
    public partial class PieceOfEquipment
    {
        public PieceOfEquipment()
        {
            WorkOrderDetail = new HashSet<WorkOrderDetail>();
        }

        public int PieceOfEquipmentId { get; set; }
        public string SerialNumber { get; set; }
        public string ClientId { get; set; }
        public int CustomerId { get; set; }
        public int TenantId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<WorkOrderDetail> WorkOrderDetail { get; set; }
    }
}
