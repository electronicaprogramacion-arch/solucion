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
    public class MicroResult:IResult2
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

        
       

        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 6)]
        public double NominalValue { get; set; }


       
         [DataMember(Order = 7)]
        public double Test1X { get; set; }

        [DataMember(Order = 8)]
        public double Test1Y { get; set; }

        [DataMember(Order = 9)]
        public double Test2X { get; set; }

        [DataMember(Order = 10)]
        public double Test2Y { get; set; }

        [DataMember(Order = 11)]
        public double Test3X { get; set; }

        [DataMember(Order = 12)]
        public double Test3Y { get; set; }

        [DataMember(Order = 13)]
        public double Test4X { get; set; }

        [DataMember(Order = 13)]
        public double Test4Y { get; set; }

        [DataMember(Order = 14)]
        public double Test5X { get; set; }

        [DataMember(Order = 15)]
        public double Test5Y { get; set; }


        [DataMember(Order = 16)]
        public double Error { get; set; }

        [DataMember(Order = 17)]
        public double ErrorPer { get; set; }




        [DataMember(Order = 16)]
        public double Repeateability { get; set; }

        [DataMember(Order = 16)]
        public double RepeateabilityPer { get; set; }


        [DataMember(Order = 17)]
        public double Uncertanty { get; set; }

        [DataMember(Order = 18)]
        public double Standard { get; set; }

        [DataMember(Order = 19)]
        public double UnBias { get; set; }

        [DataMember(Order = 20)]
        public double Average { get; set; }

        [DataMember(Order = 21)]
        public double AveragePer { get; set; }





        [DataMember(Order = 22)]
        public double Resolution { get; set; }

        
        [DataMember(Order = 23)]
        public int DecimalNumber { get; set; }

        
        //[DataMember(Order = 19)]
        //public double WeightApplied { get; set; }

        
        

        [JsonIgnore]
        [NotMapped]
        [IgnoreDataMember]
        public virtual Micro Micro { get; set; }

        [DataMember(Order = 24)]
        public int Position { get; set; }

        
        [DataMember(Order = 25)]
        public double Max { get; set; }

        [DataMember(Order = 26)]
       
        public double Min { get; set; }

        [NotMapped]
        public string ComponentID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string GroupName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}