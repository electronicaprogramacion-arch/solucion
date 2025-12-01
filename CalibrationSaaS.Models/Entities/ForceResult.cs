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
    public class ForceResult: IResult2
    { 
        
        [Key]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int SequenceID { get; set; } //Consecutive of the Calibration Result

        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 2)]
        public int CalibrationSubTypeId { get; set; }

        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 3)]
        public int WorkOrderDetailId { get; set; }

        
        [DataMember(Order = 4)]
        public double FS { get; set; }

        
        [DataMember(Order = 5)]
        public int Equipmet { get; set; }

        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 6)]
        public double Nominal { get; set; }

        //asfound
        [DataMember(Order = 7)]
        public double RUN1 { get; set; }


        //adjusted
         [DataMember(Order = 8)]
        public double RUN2 { get; set; }


        //asleft
         [DataMember(Order = 9)]
        public double RUN3 { get; set; }

        //asleft
         [DataMember(Order = 10)]
        public double RUN4 { get; set; }


        
        [DataMember(Order = 11)]
        public double Error { get; set; }

        

        
          [DataMember(Order = 12)]
        public double ErrorPer { get; set; }

        
          [DataMember(Order = 13)]
        public double Uncertanty { get; set; }

        
         [DataMember(Order = 14)]
        public double RelativeIndicationError { get; set; }

        
         [DataMember(Order = 15)]
        public double RelativeRepeatabilityError { get; set; }

        
        [DataMember(Order = 16)]
        public string Class { get; set; }

        
        [DataMember(Order = 17)]
        public double Resolution { get; set; }

        
        [DataMember(Order = 18)]
        public int DecimalNumber { get; set; }

        
        //[DataMember(Order = 19)]
        //public double WeightApplied { get; set; }

        
        [DataMember(Order = 19)]
        public double IncludeASTM { get; set; }


        
        [DataMember(Order = 20)]
        public double ErrorRun1 { get; set; }

        
        [DataMember(Order = 21)]
        public double ErrorRun2 { get; set; }

        
        [DataMember(Order = 22)]
        public double ErrorRun3 { get; set; }

        
        [DataMember(Order = 23)]
        public double ErrorRun4 { get; set; }

        
        [DataMember(Order = 24)]
        public double RelativeIndicationErrorR1 { get; set; }

        
        [DataMember(Order = 25)]
        public double RelativeIndicationErrorR2 { get; set; }

        
        [DataMember(Order = 26)]
        public double RelativeIndicationErrorR3 { get; set; }

        
        [DataMember(Order = 27)]
        public double RelativeIndicationErrorR4 { get; set; }

        
        [DataMember(Order = 28)]
        public double ZeroReturn { get; set; }

        
        [DataMember(Order = 29)]
        public  double RelativeResolution { get; set; }


        [NotMapped]
        [DataMember(Order = 30)]
        public bool IsZeroReturn { get; set; }

        [NotMapped]
        [DataMember(Order = 31)]
        public double MaxErrorp { get; set; }

        [NotMapped]
        [DataMember(Order = 32)]
        public double ErrorpRun1 { get; set; }


        [NotMapped]
        [DataMember(Order = 33)]
        public string ClassRun1 { get; set; }

        [NotMapped]
        [DataMember(Order = 34)]
        public string InToleranceFound { get; set; }

        [NotMapped]
        [DataMember(Order = 35)]
        public string InToleranceLeft { get; set; }
        
        [NotMapped]
        [DataMember(Order = 36)]
        public double Repeatability { get; set; }

        [NotMapped]
        [DataMember(Order = 37)]
        public double RepeatabilityUncertainty { get; set; }

        [NotMapped]
        [DataMember(Order = 38)]
        public double MaxErrorNominal { get; set; }

        [NotMapped]
        [DataMember(Order = 39)]
        public double MaxErrorNominalASTM { get; set; }

        [NotMapped]
        [DataMember(Order = 40)]
        public double NominalTemp { get; set; }

        [JsonIgnore]
        [NotMapped]
        [IgnoreDataMember]
        public virtual Force Force { get; set; }

        [DataMember(Order = 41)]
        public int Position { get; set; }

        [NotMapped]
        [DataMember(Order = 42)]
        public double ErrorpRun2 { get; set; }
        [NotMapped]
        [DataMember(Order = 43)]
        public double ErrorpRun3 { get; set; }
        [NotMapped]
        [DataMember(Order = 44)]
        public double ErrorpRun4 { get; set; }
       
        [NotMapped]
        [DataMember(Order = 45)]
        public string StandardId { get; set; }

       

        [NotMapped]
        public string ComponentID { get ; set ; }

        [NotMapped]
        [DataMember(Order = 46)]
        public string InToleranceAdjusted { get; set; }

        [NotMapped]
        [DataMember(Order = 47)]
        public string ToastMessageForce { get; set; } = string.Empty;

        [NotMapped]
        [DataMember(Order = 48)]
        public double FsTemp{ get; set; }

        [DataMember(Order = 49)]
        public double? UncertaintyNew { get; set; }

        [NotMapped]
        [DataMember(Order = 50)]
        public string IntoleranceRun4 { get; set; }


        [DataMember(Order = 51)]
        public string Class1Temp{ get; set; }
  

        [DataMember(Order = 52)]
        public string ClassTemp { get; set; }

        [NotMapped]
        [DataMember(Order = 53)]
        public bool FirstCalculate
        {
            get; set;
        }

        [NotMapped]
        [DataMember(Order = 54)]
        public string MessageForceDifference { get; set; } = string.Empty;


        [NotMapped]
        [DataMember(Order = 55)]
        public string ToastMessageForceTotal { get; set; } = string.Empty;

        [NotMapped]
        [DataMember(Order = 56)]
        public string InToleranceLeftASTM { get; set; }

        [NotMapped]
        [DataMember(Order = 57)]
        public string ClassIso { get; set; }


        [NotMapped]
        [DataMember(Order = 58)]
        public double TUR { get; set; }

        [NotMapped]
        [DataMember(Order = 59)]
        public double TURAstm { get; set; }

        [NotMapped]
        [DataMember(Order = 60)]
        public double UpperToleranceTUR { get; set; }

        [NotMapped]
        [DataMember(Order = 61)]
        public double LowToleranceTUR { get; set; }
    }
}