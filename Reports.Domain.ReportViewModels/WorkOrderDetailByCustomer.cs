using System;
using System.Collections.Generic;
using System.Text;

namespace Reports.Domain.ReportViewModels
{
    public class WorkOrderDetailByCustomer
    {
    
        public int WorkOrderIdDetailId { get; set; }
        public string Company { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string Address { get; set; }
        public string CityZipCodeState { get; set; }
        public string Manufacturer { get; set; }
        public string EquipmentType { get; set; }
        public string DueDate { get; set; }
       

    }
}
