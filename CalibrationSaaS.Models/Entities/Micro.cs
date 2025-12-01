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
    public partial class Micro : IGenericCalibrationSubType<MicroResult>
    {      

        public Micro()
        {
            //Weights = new List<WeightSet>();
        }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int SequenceID { get; set; } //TODO: Should be the TestPointNumber, and the primaty key should be a compound between this ID, Calibration Type and WorkOrderDetailId 

        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 2)]
        public int CalibrationSubTypeId { get; set; } = 4;

        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 3)]
        public int WorkOrderDetailId { get; set; }

        
        
        [DataMember(Order = 4)]
        public virtual ICollection<WeightSet> WeightSets { get; set; }

        
        
        [DataMember(Order = 5)]
        public virtual TestPoint TestPoint { get; set; }

        
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 6)]      
        public virtual MicroResult BasicCalibrationResult { get; set; } 

             

        
        [DataMember(Order = 8)]
        public int NumberOfSamples { get; set; } = 1;

        
        [DataMember(Order = 9)]
        public int? TestPointID { get; set; }

        

        [DataMember(Order = 11)]
        public int? UnitOfMeasureId { get; set; }

        [DataMember(Order = 12)]
        public int? CalibrationUncertaintyValueUnitOfMeasureId { get; set; } = null;

       

        [NotMapped]
        [IgnoreDataMember]
        public double CalculateWeightValue { get; set; }
       
       

        [DataMember(Order = 14)]
        public int? UncertaintyID { get;  set; }

        //[DataMember(Order = 15)]
        //public virtual ICollection<Uncertainty> UncertaintyList { get;  set; }

         [JsonIgnore]
        [DataMember(Order = 15)]
        public virtual ICollection<CalibrationSubType_Weight> CalibrationSubType_Weights { get; set; }

        [NotMapped]
        [DataMember(Order = 16)]
        public virtual ICollection<CalibrationResultContributor> CalibrationResultContributors { get; set; }


        
        [DataMember(Order = 17)]
        public virtual ICollection<CalibrationSubType_Standard> Standards { get; set; }
    }
}
