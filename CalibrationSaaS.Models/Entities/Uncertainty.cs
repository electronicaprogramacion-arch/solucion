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
    public partial class Uncertainty: IGeneric
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required UncertaintyID")]
        [DataMember(Order = 1)]
        //[Range(1, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int UncertaintyID { get; set; }


        [DataMember(Order = 2)]
        public int? EquipmentTemplateID { get; set; }
        [DataMember(Order = 3)]
        public virtual EquipmentTemplate EquipmentTemplate { get; set; }

         //[Range(1, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        [DataMember(Order = 4)]
        public string PieceOfEquipmentID { get; set; }

        [DataMember(Order = 5)]
        public virtual PieceOfEquipment PieceOfEquipment { get; set; }

        //[DataMember(Order = 6)]
        //public int ControlNumber { get; set; }

        [DataMember(Order = 7)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Type")]
         [Range(1, Double.PositiveInfinity, ErrorMessage = "Type")]
        public int Type { get; set; }

         [DataMember(Order = 8)]
       // [Required(AllowEmptyStrings = false, ErrorMessage = "RangeMin")]
        public double? RangeMin { get; set; }

        [DataMember(Order = 9)]
       // [Required(AllowEmptyStrings = false, ErrorMessage = "RangeMax")]
        public double? RangeMax { get; set; }

         [DataMember(Order = 10)]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Value")]
        public double? Value { get; set; }

          [DataMember(Order = 11)]
        public string Description { get; set; }


         [DataMember(Order = 12)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "UnitOfMeasureID")]
         [Range(1, Double.PositiveInfinity, ErrorMessage = "Select UnitOfMeasure")]
        public int UnitOfMeasureID { get; set; }

         [DataMember(Order = 13)]

        public virtual UnitOfMeasure UnitOfMeasure { get; set; }


           [DataMember(Order = 14)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Divisor")]
        [Range(1, Double.PositiveInfinity, ErrorMessage = "Divisor")]
        public double Divisor { get; set; }

        [DataMember(Order = 15)]
        public double SensitiveCoefficient { get; set; } = 1;


             [DataMember(Order = 16)]
        public double ConfidenceLevel { get; set; }
        
        
               [DataMember(Order = 17)]
        public double Quotient { get; set; }

            [DataMember(Order = 18)]
        public double Square { get; set; }
       

          [DataMember(Order = 19)]
        public string Distribution { get; set; } 


         [DataMember(Order = 20)]
        public string Comment { get; set; }      


        //public virtual ICollection<Force> Forces { get; set; }

        //TODO problemas con el grpc se necesita el ID 
        [NotMapped]
        [DataMember(Order = 21)]
        public bool IsEmpty { get; set; }


        [DataMember(Order = 22)]
        public string UncertantyOperation { get; set; }


        [NotMapped] 
        [DataMember(Order = 23)]
        public string Source { get; set; }


        [NotMapped]
        [DataMember(Order = 24)]
        public string ContributorType { get; set; }


        
        [DataMember(Order = 25)]
        public string Contributors { get; set; }      

    }


    [DataContract]
    public class UncertantyOperation
    {
        [DataMember(Order = 1)] 
        public string DataSource { get; set; }

        [DataMember(Order = 2)]
        public string Operation { get; set; }


        [DataMember(Order = 3)]
        public string UnitOfMeasure { get; set; }



    }



}
