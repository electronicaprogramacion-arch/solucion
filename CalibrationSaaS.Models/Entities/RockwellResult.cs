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
    public class RockwellResult:IResult2,IUpdated
    {

        //[Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int SequenceID { get; set; } //Consecutive of the Calibration Result

        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 2)]
        public int CalibrationSubTypeId { get; set; }

        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 3)]
        public int WorkOrderDetailId { get; set; }

        
        //[DataMember(Order = 4)]
        //public double FS { get; set; }

        
        //[DataMember(Order = 5)]
        //public int Equipmet { get; set; }

        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 6)]
        public double Nominal { get; set; }

        //asfound
        [DataMember(Order = 7)]
        public string ScaleRange { get; set; }


        //adjusted
         [DataMember(Order = 8)]
        public double Standard { get; set; }


        //asleft
         [DataMember(Order = 9)]
        public double Average { get; set; }

        //asleft
         [DataMember(Order = 10)]
        public double Test1 { get; set; }

        [DataMember(Order = 11)]
        public double Test2 { get; set; }

        [DataMember(Order = 12)]
        public double Test3 { get; set; }

        [DataMember(Order = 13)]
        public double Test4 { get; set; }

        [DataMember(Order = 14)]
        public double Test5 { get; set; }


        [DataMember(Order = 15)]
        public double Error { get; set; }

        

        
          [DataMember(Order = 16)]
        public double Repeateability { get; set; }

        
          [DataMember(Order = 17)]
        public double Uncertanty { get; set; }

        
        

        
        [DataMember(Order = 18)]
        public double Resolution { get; set; }

        
        [DataMember(Order = 19)]
        public int DecimalNumber { get; set; }

        
        //[DataMember(Order = 19)]
        //public double WeightApplied { get; set; }

        
        

        [JsonIgnore]
        [NotMapped]
        [IgnoreDataMember]
        public virtual Rockwell Rockwell { get; set; }

        [DataMember(Order = 20)]
        public int Position { get; set; }

        
        [DataMember(Order = 21)]
        public double Max { get; set; }

        [DataMember(Order = 22)]
       
        public double Min { get; set; }

        [NotMapped]
        public string ComponentID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        [NotMapped]
        [DataMember(Order = 23)]
        public long? Updated { get ; set ; }
    }
}