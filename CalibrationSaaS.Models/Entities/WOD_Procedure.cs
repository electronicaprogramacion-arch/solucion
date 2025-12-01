using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public class WOD_Procedure
    {
        [DataMember(Order = 1)]
        public int WorkOrderDetailID{ get; set; }

        [DataMember(Order = 2)]
        public int ProcedureID { get; set; }


        [DataMember(Order = 3)]
        public virtual Procedure Procedure { get; set; }

        [IgnoreDataMember]
        public virtual WorkOrderDetail WorkOrderDetail { get; set; }


      


    }
}
