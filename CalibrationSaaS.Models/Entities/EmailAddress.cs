using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public partial class EmailAddress: IGeneric
    {
        //public CustomerAddress()
        //{
        //    WorkOrder = new HashSet<WorkOrder>();
        //}
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int EmailAddressID { get; set; }
        // public int TenantId { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 2)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 3)]
        public string TypeID { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 4)]
        public string Type { get; set; }


        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 5)]
        public bool IsEnabled { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 6)]
        public string AggregateID { get; set; }


        public string Name { get ; set ; }
        public string Description { get ; set ; }
    }
}
