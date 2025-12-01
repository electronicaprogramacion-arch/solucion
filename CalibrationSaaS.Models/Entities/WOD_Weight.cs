using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public class WOD_Weight 
    {
        [DataMember(Order = 1)]
        public int WorkOrderDetailID{ get; set; }

        [DataMember(Order = 2)]
        public int WeightSetID { get; set; }

        [DataMember(Order = 3)]
        public virtual WeightSet WeightSet { get; set; }

        [IgnoreDataMember]
        public virtual WorkOrderDetail WorkOrderDetail { get; set; }

        [DataMember(Order = 4)]
        public string Option { get; set; }




    }
}
