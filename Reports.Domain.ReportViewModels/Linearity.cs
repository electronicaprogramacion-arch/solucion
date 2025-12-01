using System;
using System.Collections.Generic;
using System.Text;

namespace Reports.Domain.ReportViewModels
{
    public class Lineartiy
    {
        public int WorkOrderDetailId { get; set; }
        public int CustomerId { get; set; }
        public int Position { get; set; }
        public string AsFound { get; set; }
        public string AsLeft { get; set; }
        public string PassFail { get; set; }
        public string Tolerance { get; set; }
        public string Weight { get; set; }
        public string PassFailAsFound { get; set; }
        public string PassFailAsLeft { get; set; }
        public string Uncertainty { get; set; }
        public string UOM { get; set; }
    }
}
