using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public class Certification : IGeneric
    {
       public Certification()
        {
           
        }

        [Key]
        [DataMember(Order = 1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public int CertificationID { get; set; }

        [DataMember(Order = 2)]
        [StringLength(100, ErrorMessage = "Name is too long.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public string Name { get; set; } = "";

        [DataMember(Order = 3)]
        public string Description { get; set; } = "";

        [DataMember(Order = 4)]
        public virtual ICollection<TechnicianCode> TechnicianCodes { get; set; }


      
        [DataMember(Order = 5)]
       
        public int CalibrationTypeID { get; set; }

        [DataMember(Order = 6)]
       
        public int ID { get; set; }





    }
}
