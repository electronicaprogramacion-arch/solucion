using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
     [DataContract]
    public class Note
    {

        [Key]
        [DataMember(Order = 1)]
        public int NoteId { get; set; }



        [Required(AllowEmptyStrings = false, ErrorMessage = "Data Required")]
        [DataMember(Order = 2)]
        public string Text { get; set; }

        [DataMember(Order = 3)]
        public int? Commnet { get; set; }



        [Required(AllowEmptyStrings = false, ErrorMessage = "Condition Required")]
        [DataMember(Order = 4)]
        public int Condition { get; set; }

        [DataMember(Order = 5)]
        public int? EquipmnetTypeId { get; set; }

        [NotMapped]
        public virtual EquipmentType EquipmentType { get; set; }


        [DataMember(Order = 6)]
        public int? TestCodeID { get; set; }

       


        [DataMember(Order = 7)]
        public int Position { get; set; }


        [DataMember(Order = 8)]
        public int CalibrationSubtypeId { get; set; }

        [DataMember(Order = 9)]
        public string Validation { get; set; }

      
       
        
        [NotMapped]
        //[DataMember(Order = 12)]
        public object ValidationResult { get; set; }

    }
}
