using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
      [DataContract]
    public partial class CalibrationResultContributor: IGeneric
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        //[Range(1, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        public string CalibrationResultContributorID { get; set; }


        [DataMember(Order = 2)]
        public int? EquipmentTemplateID { get; set; }

         [DataMember(Order = 3)]
        public virtual EquipmentTemplate EquipmentTemplate { get; set; }

         //[Range(1, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        [DataMember(Order = 4)]
        public string PieceOfEquipmentID { get; set; }

        [DataMember(Order = 5)]
        public virtual PieceOfEquipment PieceOfEquipment { get; set; }

        [DataMember(Order = 6)]
        public int Type { get; set; }

        
         [DataMember(Order = 7)]
        public int UnitOfMeasureID { get; set; }

         [DataMember(Order = 8)]
        public virtual UnitOfMeasure UnitOfMeasure { get; set; }


           [DataMember(Order = 9)]
        public double Divisor { get; set; }

        [DataMember(Order = 10)]
        public double SensitiveCoefficient { get; set; } = 1;


             [DataMember(Order = 11)]
        public double ConfidenceLevel { get; set; }
        
        
               [DataMember(Order =12)]
        public double Quotient { get; set; }

            [DataMember(Order = 13)]
        public double Square { get; set; }
       

          [DataMember(Order = 14)]
        public string Distribution { get; set; } 
         [DataMember(Order = 15)]
        public string Comment { get; set; }

        [DataMember(Order = 16)]
        public string TypeContributor { get; set; }

        [DataMember(Order = 17)]
        public double Magnitude { get; set; }

        [DataMember(Order = 18)]
        public string Description { get; set; }

        [NotMapped]
        [DataMember(Order = 19)]
        public bool IsEmpty { get; set; }
        


    }
}
