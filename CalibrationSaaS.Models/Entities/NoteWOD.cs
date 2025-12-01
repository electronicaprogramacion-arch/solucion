using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
     [DataContract]
    public class NoteWOD
    {


        [DataMember(Order = 1)]
        public int NoteWODId { get; set; }

        [DataMember(Order = 2)]
        public string Text { get; set; }

      
        [DataMember(Order = 3)]
        public int? WorkOrderDetailId { get; set; }

        public virtual WorkOrderDetail WorkOrderDetail { get; set; }

        [DataMember(Order = 4)]
        public int Position { get; set; }

        [DataMember(Order = 5)]
        public int? CalibrationSubtypeId { get; set; }


    }
}
