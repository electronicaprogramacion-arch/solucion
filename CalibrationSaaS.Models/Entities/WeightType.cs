using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;


namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public class WeightType
    {
        public WeightType()
        {
           
        }

        [Required]       
        [DataMember(Order = 1)]
        public int WeightTypeID { get; set; }
        
        [DataMember(Order = 2)]        
        [Required(AllowEmptyStrings =false,ErrorMessage = "Required")]
        [StringLength(500, ErrorMessage = "Name is too long.")]
        public string Name { get; set; }

     
    }
}
