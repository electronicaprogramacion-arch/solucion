using System;
using System.Collections.Generic;

namespace CalibrationSaaS.Models
{
    public partial class Repeatability
    {
        public int RepeatabilityId { get; set; }
        public int CalibrationResultId { get; set; }
        public int TenantId { get; set; }
        public decimal Weight1 { get; set; }
        public decimal Weight2 { get; set; }
        public decimal Weight3 { get; set; }
        public decimal Weight4 { get; set; }
        public decimal Weight5 { get; set; }
        public decimal AsFound1 { get; set; }
        public decimal AsFound2 { get; set; }
        public decimal AsFound3 { get; set; }
        public decimal AsFound4 { get; set; }
        public decimal? AsFound10 { get; set; }
        public decimal AsFound9 { get; set; }
        public decimal AsFound8 { get; set; }
        public decimal AsFound7 { get; set; }
        public decimal AsFound6 { get; set; }
        public decimal AsFound5 { get; set; }
        public decimal? AsLeft1 { get; set; }
        public decimal? AsLeft2 { get; set; }
        public decimal? AsLeft3 { get; set; }
        public decimal? AsLeft4 { get; set; }
        public decimal? AsLeft5 { get; set; }
        public decimal? AsLeft10 { get; set; }
        public decimal? AsLeft9 { get; set; }
        public decimal? AsLeft8 { get; set; }
        public decimal? AsLeft7 { get; set; }
        public decimal? AsLeft6 { get; set; }

        public virtual CalibrationResult CalibrationResult { get; set; }
    }
}
