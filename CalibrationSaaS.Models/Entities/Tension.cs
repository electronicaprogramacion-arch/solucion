using CalibrationSaaS.Domain.Aggregates.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public partial class Tension : ICalibrationSubType
    {      

        public Tension()
        {
            //Weights = new List<WeightSet>();
        }

        [Key]
        
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int SequenceID { get; set; } //TODO: Should be the TestPointNumber, and the primaty key should be a compound between this ID, Calibration Type and WorkOrderDetailId 

        [NotMapped]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 2)]
        public int CalibrationSubTypeId { get; set; } = 1;

        [NotMapped]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 3)]
        public int WorkOrderDetailId { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 4)]
        public virtual ICollection<WeightSet> WeightSets { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 5)]
        public TestPoint TestPoint { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 6)]
       
        public virtual TensionResult BasicCalibrationResult { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 7)]
        public double Tolerance { get; set; }

        [NotMapped]
        [DataMember(Order = 8)]
        public int NumberOfSamples { get; set; } = 1;

        [NotMapped]
        [DataMember(Order = 9)]
        public int? TestPointID { get; set; }

        [NotMapped]
          [DataMember(Order = 10)]
        public double Uncertainty { get; set; }
        

        //[JsonIgnore]
        //[IgnoreDataMember]
        //public virtual BalanceAndScaleCalibration BalanceAndScaleCalibration { get; set; }

        //[JsonIgnore]
        //[DataMember(Order = 10)]
        //public virtual ICollection<CalibrationSubType_Weight> CalibrationSubType_Weights { get; set; }
       

        public double TotalNominal { get; set; }
        public double TotalActual { get; set; }
        public double SumUncertainty { get; set; }
        public double Quotient { get; set; }
        public double Square { get; set; }
        public double SumOfSquares { get; set; }
        public double TotalUncertainty { get; set; }
        public double ExpandedUncertainty { get; set; }
        public string CalibrationUncertaintyType { get; set; }
        public string CalibrationUncertaintyDistribution { get; set; }
        public double CalibrationUncertaintyDivisor { get; set; }

        [DataMember(Order = 11)]
        public int? UnitOfMeasureId { get; set; }

        [DataMember(Order = 12)]
        public int? CalibrationUncertaintyValueUnitOfMeasureId { get; set; } = null;

        //public virtual UnitOfMeasure UnitOfMeasure { get; set; }

        //public virtual UnitOfMeasure CalibrationUncertaintyValueUncertaintyUnitOfMeasure { get; set; }
        public double MinTolerance { get; set; }
        public double MaxTolerance { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public double CalculateWeightValue { get; set; }
        public double MinToleranceAsLeft { get; internal set; }
        public double MaxToleranceAsLeft { get; internal set; }
    }
}
