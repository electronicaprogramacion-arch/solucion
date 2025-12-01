using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public partial class CustomerAddress
    {
        //public CustomerAddress()
        //{
        //    WorkOrder = new HashSet<WorkOrder>();
        //}

        public int CustomerAddressId { get; set; }
       // public int TenantId { get; set; }
        public int CustomerId { get; set; }
        //public string CustomerName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }

        [DataMember(Order = 7)]
        public decimal? TravelExpense { get; set; }

        [JsonIgnore]
        public virtual Customer Customer { get; set; }

        [JsonIgnore]
        public virtual ICollection<WorkOrder> WorkOrder { get; set; }
    }
}
