using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public class User_WorkOrder
    {
        [DataMember(Order = 1)]
        public int UserID { get; set; }

        [DataMember(Order = 2)]
        public int WorkOrderID { get; set; }

        [NotMapped]
        [DataMember(Order = 3)]
        public virtual User User { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public virtual WorkOrder WorkOrder { get; set; }



    }
}
