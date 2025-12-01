using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public class RangeTolerance : IGeneric
    {
       public RangeTolerance()
        {
           
        }
        [Key]
        [DataMember(Order = 1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public int RangeToleranceID { get; set; }

        [DataMember(Order = 2)]
        [Range(0, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        public double MinValue { get; set; }

        [Range(0, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        [DataMember(Order = 3)]      
        public double MaxValue { get; set; }

        [DataMember(Order = 4)]
        [Range(0, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {0}.")]
        public double Percent { get; set; }

        [Range(0, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {0}.")]
        [DataMember(Order = 5)]
        public double Resolution { get; set; }


        [Range(1, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {0}.")]
        [DataMember(Order = 6)]
        public int ToleranceTypeID { get; set; }
        

        //[DataMember(Order = 7)]
        //public int SourceID { get; set; }

        [DataMember(Order = 7)]
        public int? EquipmentTemplateID { get; set; }

        [DataMember(Order = 8)]
        public string PieceOfEquipmentID { get; set; }


        [DataMember(Order = 9)]
        public int? WorkOrderDetailID { get; set; }



        //[IgnoreDataMember]
        //public string Name { get; set; }

        //[IgnoreDataMember]
        //public string Description { get; set; }
    }
}
