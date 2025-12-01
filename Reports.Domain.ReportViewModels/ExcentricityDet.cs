using System;
using System.Collections.Generic;
using System.Text;

namespace Reports.Domain.ReportViewModels
{
    public class ExcentricityDet
    {
        public int WorkOrderDetailId { get; set; }
        public string Standard { get; set; }
        public string Description { get; set; }
        public string Certificate { get; set; }
        public string CalDue { get; set; }

    }
}
