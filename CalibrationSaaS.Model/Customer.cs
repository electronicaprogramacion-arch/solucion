using System;
using System.Collections.Generic;

namespace CalibrationSaaS.Models
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerAddress = new HashSet<CustomerAddress>();
            PieceOfEquipment = new HashSet<PieceOfEquipment>();
        }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int TenantId { get; set; }

        public virtual ICollection<CustomerAddress> CustomerAddress { get; set; }
        public virtual ICollection<PieceOfEquipment> PieceOfEquipment { get; set; }
    }
}
