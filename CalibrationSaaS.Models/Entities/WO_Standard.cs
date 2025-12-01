using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public class WO_Standard 
    {
       
        [DataMember(Order = 1)]
        public int WorkOrderID { get; set; }

    
        [DataMember(Order = 2)]
        public string PieceOfEquipmentID { get; set; }


        [DataMember(Order = 3)]
        public virtual PieceOfEquipment Standard { get; set; }

        [IgnoreDataMember]
        public virtual WorkOrder WorkOrder { get; set; }


        [DataMember(Order = 4)]
        public string Option { get; set; }

    }
}
