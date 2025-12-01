using System;
using System.Collections.Generic;
using System.Text;

namespace Reports.Domain.ReportViewModels
{
    public class Repeteab
    {
        public int WorkOrderDetailId { get; set; }
        public int CustomerId { get; set; }
        public int Position { get; set; }
        public string Standard { get; set; }
        public string AsFound { get; set; }
        public string AsLeft { get; set; }
        public bool ViewGrid { get; set; } = false;
        public double StandarDerivation { get; set; }
        public string PassFail { get; set; }
        public string Tolerance { get; set; }
        public string PassFailAsFound { get; set; }
        public string PassFailAsLeft { get; set; }
       
    }

}
