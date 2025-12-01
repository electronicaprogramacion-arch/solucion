using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public class POE_POE 
    {
        [DataMember(Order = 1)]
        public string PieceOfEquipmentID { get; set; }

        [DataMember(Order = 2)]
        public string PieceOfEquipmentID2 { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public virtual PieceOfEquipment POE { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public virtual PieceOfEquipment POE2 { get; set; }

    }


    [DataContract]
    public class POE_Scale
    {
        [DataMember(Order = 1)]
        public string PieceOfEquipmentID { get; set; }

        [DataMember(Order = 2)]
        public string Scale { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public virtual PieceOfEquipment POE { get; set; }




    }
}
