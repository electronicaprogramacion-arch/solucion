using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public class CertificatePoE : IGeneric
    {
       public CertificatePoE()
        {
           
        }

        [Key]
        [DataMember(Order = 1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required CertificateNumber")]
        
        [StringLength(50, ErrorMessage = "Name is too long.")]
        public string CertificateNumber { get; set; }
        
        [DataMember(Order = 2)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required DueDate")]
        public DateTime DueDate { get; set; }


        [DataMember(Order = 3)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required CalibrationDate")]
        public DateTime CalibrationDate { get; set; }

        [DataMember(Order = 4)]
        public string Company { get; set; } //fk WeigthSet
        //[DataMember(Order = 4)]
        //public int CustomerId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required AffectDueDate")]
        [DataMember(Order = 5)]
        public bool AffectDueDate { get; set; }

        [DataMember(Order = 6)]
        [StringLength(100, ErrorMessage = "Name is too long.")]
        public string Name { get; set; } = "";

        [DataMember(Order = 7)]
       
        [StringLength(100, ErrorMessage = "Name is too long.")]
        public string Description { get; set; } = "";


         [Required(AllowEmptyStrings = false, ErrorMessage = "Required PieceOfEquipmentID")]
        [DataMember(Order = 8)]
        public string PieceOfEquipmentID { get; set; } //fk WeigthSet


        [DataMember(Order = 9)]
        public int Version { get; set; }

      
        public virtual PieceOfEquipment PieceOfEquipment { get; set; }

        [DataMember(Order = 10)]
        public string CalibrationProvider { get; set; }


    }
}
