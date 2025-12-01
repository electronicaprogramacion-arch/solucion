using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
     [DataContract]
    public class CustomerReplaced
    {


        [DataMember(Order = 1)]
        public ICollection<PieceOfEquipment> ListPieceOfEquipment { get; set; }

        [DataMember(Order = 2)]
        public Customer CustomerOld { get; set; }

        [DataMember(Order = 3)]
        public Customer CustomerNew { get; set; }


    }
}
