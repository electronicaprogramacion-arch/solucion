using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public class POE_User 
    {
        [DataMember(Order = 1)]
        public int UserID { get; set; }

        [DataMember(Order = 2)]
        public string PieceOfEquipmentID { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public virtual User User { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public virtual PieceOfEquipment POE { get; set; }



    }
}
