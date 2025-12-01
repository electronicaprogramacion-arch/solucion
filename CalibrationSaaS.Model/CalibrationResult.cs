using System;
using System.Collections.Generic;

namespace CalibrationSaaS.Models
{
    public partial class CalibrationResult
    {
        public CalibrationResult()
        {
            Eccentricity = new HashSet<Eccentricity>();
            Linearity = new HashSet<Linearity>();
            Repeatability = new HashSet<Repeatability>();
        }

        public int CalibrationResultId { get; set; }
        public int WorkOrderDetailId { get; set; }
        public int TenantId { get; set; }
        public bool HasLinearity { get; set; }
        public bool HasEccentricity { get; set; }
        public bool HasRepeatability { get; set; }

        public virtual WorkOrderDetail WorkOrderDetail { get; set; }
        public virtual ICollection<Eccentricity> Eccentricity { get; set; }
        public virtual ICollection<Linearity> Linearity { get; set; }
        public virtual ICollection<Repeatability> Repeatability { get; set; }
    }
}
