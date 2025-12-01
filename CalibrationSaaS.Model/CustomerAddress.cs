using System;
using System.Collections.Generic;

namespace CalibrationSaaS.Models
{
    public partial class CustomerAddress
    {
        public CustomerAddress()
        {
            WorkOrder = new HashSet<WorkOrder>();
        }

        public int CustomerAddressId { get; set; }
        public int TenantId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<WorkOrder> WorkOrder { get; set; }
    }
}
