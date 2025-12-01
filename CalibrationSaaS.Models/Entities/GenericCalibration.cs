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
    public partial class GenericCalibration : IGenericCalibrationSubType<GenericCalibrationResult> , IGenericCalibrationStandard
    {      

        public GenericCalibration()
        {
            //Weights = new List<WeightSet>();
        }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int SequenceID { get; set; } //TODO: Should be the TestPointNumber, and the primaty key should be a compound between this ID, Calibration Type and WorkOrderDetailId 

        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 2)]
        public int CalibrationSubTypeId { get; set; } = 0;

        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 3)]
        public int WorkOrderDetailId { get; set; }

        
        
        [DataMember(Order = 4)]
        public virtual ICollection<WeightSet> WeightSets { get; set; }

        
        
        [DataMember(Order = 5)]
        public virtual TestPoint TestPoint { get; set; }

        
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 6)]      
        public virtual GenericCalibrationResult  BasicCalibrationResult { get; set; } 

             

        
        [DataMember(Order = 7)]
        public int NumberOfSamples { get; set; } = 1;

        
        [DataMember(Order = 8)]
        public int? TestPointID { get; set; }

        

        [DataMember(Order = 10)]
        public int? UnitOfMeasureId { get; set; }

      

        [NotMapped]
        [IgnoreDataMember]
        public double CalculateWeightValue { get; set; }



        [NotMapped]
        [JsonIgnore]
        [DataMember(Order = 11)]
        public virtual ICollection<CalibrationSubType_Weight> CalibrationSubType_Weights { get; set; }

        [NotMapped]
        [DataMember(Order = 12)]
        public virtual ICollection<CalibrationResultContributor> CalibrationResultContributors { get; set; }


        [NotMapped]
        [DataMember(Order = 13)]
        public virtual ICollection<CalibrationSubType_Standard> Standards { get; set; }

        [NotMapped]
        
        public virtual dynamic BasicCalibrationResult2 { get; set; }


        [NotMapped]

        public  string Test { get; set; }


        [DataMember(Order = 14)]
        public string ComponentID { get; set; }


    }
}
