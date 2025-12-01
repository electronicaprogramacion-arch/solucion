using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public class WOD_TestPoint
    {
        [DataMember(Order = 1)]
        public int WorkOrderDetailID{ get; set; }

        [DataMember(Order = 2)]
        public int TestPointID { get; set; }


        [DataMember(Order = 3)]
        public int? SecuenceID { get; set; }


        [DataMember(Order = 4)]
        public int CalibrationSubTypeID { get; set; }


        [DataMember(Order = 5)]
        public virtual WorkOrderDetail WorkOrderDetail { get; set; }

        [DataMember(Order = 6)]
        public virtual TestPoint TestPoint { get; set; }



    }
}
