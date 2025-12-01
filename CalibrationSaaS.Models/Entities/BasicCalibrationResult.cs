using CalibrationSaaS.Domain.Aggregates.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public class BasicCalibrationResult : IResult2
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int SequenceID { get; set; } //Consecutive of the Calibration Result

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 2)]
        public int CalibrationSubTypeId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 3)]
        public int WorkOrderDetailId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 4)]
        public double AsFound { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 5)]
        public double AsLeft { get; set; }      
       
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 6)]
        public double WeightApplied { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 7)]
        public double ReadingStandard { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 8)]
        public double FinalReadingStandard { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 9)]
        public int TestResultID { get; set; }

        [DataMember(Order = 10)]
        public double? Uncertainty { get; set; }


        private int _Position;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 11)]
        public int Position { 
            get {

                return _Position;


            } 
            set { _Position = value; } 
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 12)]
        public int UnitOfMeasureID { get; set; }


        [JsonIgnore]
        [NotMapped]
        [IgnoreDataMember]
        public virtual Linearity Linearity { get; set; }

        [JsonIgnore]
        [NotMapped]
        [IgnoreDataMember]
        public virtual Repeatability Repeatability { get; set; }

        [JsonIgnore]
        [NotMapped]
        [IgnoreDataMember]
        public virtual Eccentricity Eccentricity { get; set; }

       
        [NotMapped]
        [DataMember(Order = 13)]
        public virtual UnitOfMeasure UnitOfMeasure { get; set; }

        [DataMember(Order = 14)]
        public string InToleranceFound { get; set; }

        [DataMember(Order = 15)]
        public string InToleranceLeft { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        //[DataMember(Order = 5)]
        //public int WeightSetID { get; set; }

        //[DataMember(Order = 16)]
        //public double ToleranceAsLeft { get; set; }
        //[DataMember(Order = 17)]
        //public double ToleranceAsFound { get; set; }
        [DataMember(Order = 16)]
        public double Resolution { get; set; }

        [DataMember(Order = 17)]
        public string Description { get; set; }

        [DataMember(Order = 18)]
        public double? UncertaintyNew { get; set; }

        [NotMapped]
        public double LowerTolerance { get; set; }

        [NotMapped]
        public double UpperTolerance { get; set; }

        [NotMapped]
        public int DecimalNumber { get ; set; }

        [NotMapped]
        public string ComponentID { get ; set ; }
    }

    [DataContract]
    public class TensionResult 
    { 
       [NotMapped]
        [Key]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int SequenceID { get; set; } //Consecutive of the Calibration Result

        [NotMapped]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 2)]
        public int CalibrationSubTypeId { get; set; }

        [NotMapped]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 3)]
        public int WorkOrderDetailId { get; set; }

        [NotMapped]
        [DataMember(Order = 4)]
        public double FS { get; set; }

        [NotMapped]
        [DataMember(Order = 5)]
        public int Equipmet { get; set; }

        [NotMapped]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 6)]
        public double Nominal { get; set; }

       [NotMapped]
        [DataMember(Order = 7)]
        public double RUN1 { get; set; }

        [NotMapped]
        [DataMember(Order = 8)]
        public double RUN2 { get; set; }


        [NotMapped]
         [DataMember(Order = 9)]
        public double RUN3 { get; set; }

        [NotMapped]
        [DataMember(Order = 10)]
        public double Error { get; set; }

        [NotMapped]
          [DataMember(Order = 11)]
        public double ErrorPer { get; set; }

        [NotMapped]
          [DataMember(Order = 12)]
        public double Uncertanty { get; set; }

        [NotMapped]
         [DataMember(Order = 13)]
        public double RelativeIndicationError { get; set; }

        [NotMapped]
         [DataMember(Order = 14)]
        public double RelativeRepeatabilityError { get; set; }

        [NotMapped]
        [DataMember(Order = 15)]
        public double Class { get; set; }

        [NotMapped]
        [DataMember(Order = 16)]
        public double Resolution { get; set; }

        [NotMapped]
        [DataMember(Order = 17)]
        public int DecimalNumber { get; set; }

        [NotMapped]
        [DataMember(Order = 18)]
        public double WeightApplied { get; set; }
    
    }

    [DataContract]
    public class CompresionResult 
    { 
        [NotMapped]
        [Key]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int SequenceID { get; set; } //Consecutive of the Calibration Result

        [NotMapped]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 2)]
        public int CalibrationSubTypeId { get; set; }

        [NotMapped]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 3)]
        public int WorkOrderDetailId { get; set; }

        [NotMapped]
        [DataMember(Order = 4)]
        public double FS { get; set; }

        [NotMapped]
        [DataMember(Order = 5)]
        public int Equipmet { get; set; }

        [NotMapped]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 6)]
        public double Nominal { get; set; }

        [NotMapped]//asfound
        [DataMember(Order = 7)]
        public double RUN1 { get; set; }


        [NotMapped]//adjusted
         [DataMember(Order = 8)]
        public double RUN2 { get; set; }


        [NotMapped]//asleft
         [DataMember(Order = 9)]
        public double RUN3 { get; set; }

        [NotMapped]//asleft
         [DataMember(Order = 9)]
        public double RUN4 { get; set; }


        [NotMapped]
        [DataMember(Order = 10)]
        public double Error { get; set; }

        [NotMapped]
          [DataMember(Order = 11)]
        public double ErrorPer { get; set; }

        [NotMapped]
          [DataMember(Order = 12)]
        public double Uncertanty { get; set; }

        [NotMapped]
         [DataMember(Order = 13)]
        public double RelativeIndicationError { get; set; }

        [NotMapped]
         [DataMember(Order = 14)]
        public double RelativeRepeatabilityError { get; set; }

        [NotMapped]
        [DataMember(Order = 15)]
        public double Class { get; set; }

        [NotMapped]
        [DataMember(Order = 16)]
        public double Resolution { get; set; }

        [NotMapped]
        [DataMember(Order = 17)]
        public int DecimalNumber { get; set; }

        [NotMapped]
        [DataMember(Order = 18)]
        public double WeightApplied { get; set; }

        [NotMapped]
        [DataMember(Order = 19)]
        public double IncludeASTM { get; set; }
    
    }

    [DataContract]
    public class RepeatibilityCalibrationResult : BasicCalibrationResult
    {

    }

    [DataContract]
    public class EccentricityCalibrationResult : BasicCalibrationResult
    {

    }




}
