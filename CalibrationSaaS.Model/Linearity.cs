using System;
using System.Collections.Generic;

namespace CalibrationSaaS.Models
{
    public partial class Linearity
    {
        public int LinearityId { get; set; }
        public int CalibrationResultId { get; set; }
        public int TenantId { get; set; }
        public string Label { get; set; }
        public decimal Weight { get; set; }
        public string UnitsOfMeasure { get; set; }
        public decimal AsFound { get; set; }
        public decimal? AsLeft { get; set; }
        public decimal Tolerance { get; set; }
        public bool IsDirectionUp { get; set; }
        public decimal? Resolution { get; set; }
        public decimal? Uncertainty { get; set; }

        public virtual CalibrationResult CalibrationResult { get; set; }
    }
}
