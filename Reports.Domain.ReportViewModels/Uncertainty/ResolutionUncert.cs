using System;
using System.Collections.Generic;
using System.Text;

namespace Reports.Domain.ReportViewModels
{
    public class ResolutionUncert
    {
        
        public int WorkOrderDetailID { get; set; }
        public double Weight { get; set; }
        public double Magnitude { get; set; }
        public string Units {get; set; }
        public string Type { get; set; }
        public string Distribution { get; set; }
        public double Divisor { get; set; }
        public double Qoutient { get; set; }
        public double QoutSquare { get; set; }
    }
}
