using System;
using System.Collections.Generic;
using System.Text;

namespace Reports.Domain.ReportViewModels
{
    public class AsFound
    {
        public int WorkOrderDetailId { get; set; }
        public int CustomerId { get; set; }
        
        public string Standard { get; set; }
        public string Indication { get; set; }
        public string Tolerance { get; set; }
        public double Range { get; set; }
        public string PassFail { get; set; }
        public string Uncertainty { get; set; }
        
        public int Position { get; set; }
        public string Description { get; set; }

        public string UOM { get; set; }
        public string Value { get; set; }
        public string TUR { get; set; }
        public string WeigthAppliedRep { get; set; }
        public string WeigthAppliedEcc { get; set; }
        public string WeigthActual { get; set; }
        public string ToleranceMin { get; set; }
        public string ToleranceMax { get; set; }

    }
}
