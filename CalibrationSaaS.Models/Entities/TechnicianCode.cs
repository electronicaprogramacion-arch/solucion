using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public class TechnicianCode 
    {
       public TechnicianCode()
        {
           
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [DataMember(Order = 1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public int TechnicianCodeID { get; set; } 


       
        [StringLength(20, ErrorMessage = "Name is too long.")]
        [Required(ErrorMessage = "Required, Only numeric values​​")]
        [DataMember(Order = 2)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Only numeric values​​")]
        public string Code { get; set; } = "";

       
        [DataMember(Order = 3)]
        [StringLength(100, ErrorMessage = "Name is too long.")]

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public string StateID { get; set; } 

        [DataMember(Order = 4)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public int CertificationID { get; set; }

        [IgnoreDataMember] 
        public virtual User User { get; set; }


        [DataMember(Order = 5)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public int UserID { get; set; }

        [DataMember(Order = 6)]
        public virtual Certification Certification { get; set; }

        [DataMember(Order = 7)]
        public bool IsDelete { get; set; }
    }
}
