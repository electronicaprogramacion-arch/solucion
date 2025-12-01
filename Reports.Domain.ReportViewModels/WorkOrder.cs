using System;
using System.Collections.Generic;
using System.Text;

namespace Reports.Domain.ReportViewModels
{
    public class WorkOrder
    {
    
        public int WorkOrderId { get; set; }
        public string CustomerAddressID { get; set; }
        public string CustomerName { get; set; }
    }
}
