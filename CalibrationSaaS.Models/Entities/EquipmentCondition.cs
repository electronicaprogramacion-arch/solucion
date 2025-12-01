using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public  class EquipmentCondition:IGeneric
    {


        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int EquipmentConditionId { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 2)]
        public int WorkOrderDetailId { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 3)]
        public bool IsAsFound { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 4)]
        public bool Value { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 5)]
        public string Label { get; set; }

        public virtual WorkOrderDetail WorkOrderDetail { get; set; }


       
        [DataMember(Order = 6)]
        public string Name { get ; set; }

        [NotMapped]
        [DataMember(Order = 7)]
        public string Description { get; set ; }

        //[DataMember(Order = 7)]
        //public bool InService { get; set; }


        //[DataMember(Order = 8)]
        //public bool OutofService { get; set; }


    }

    [DataContract]
    public class ExternalCondition : IGeneric
    {

        [Key]
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int ExternalConditionId { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 2)]
        public int WorkOrderDetailId { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 3)]
        public double Temperature { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 4)]
        public double Humidity  { get; set; }

        [DataMember(Order = 5)]
        public double HRC { get; set; }


        [DataMember(Order = 6)]
        public double HRA { get; set; }


        [DataMember(Order = 7)]
        public double HRN { get; set; }


        [DataMember(Order = 8)]
        public bool Pass1 { get; set; }

        [DataMember(Order = 9)]
        public bool Pass2 { get; set; }


        [DataMember(Order = 10)]
        public bool Pass3 { get; set; }


        [DataMember(Order = 11)]
        public double Onesixteenth { get; set; }

        [DataMember(Order = 12)]
        public double AnEighth { get; set; }

       
        [DataMember(Order = 13)]
        public double Quarter { get; set; }

            
        [DataMember(Order = 14)]
        public double Medium { get; set; }

        [DataMember(Order = 15)]
        public bool PassHRA { get; set; }


        [DataMember(Order = 16)]
        public bool PassAnEighth { get; set; }

        [DataMember(Order = 17)]
        public bool PassHRN { get; set; }

        
        [DataMember(Order = 18)]
        public bool PassQuarter { get; set; }


        
        [DataMember(Order = 19)]
        public string  Serial { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 20)]
        public bool IsAsFound { get; set; }



        public virtual WorkOrderDetail WorkOrderDetail { get; set; }



        [DataMember(Order = 21)]
        public string Name { get; set; }

        [NotMapped]
        [DataMember(Order = 22)]
        public string Description { get; set; }

        //[DataMember(Order = 7)]
        //public bool InService { get; set; }


        //[DataMember(Order = 8)]
        //public bool OutofService { get; set; }


    }
}
