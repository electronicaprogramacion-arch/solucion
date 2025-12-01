using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;


namespace CalibrationSaaS.Domain.Aggregates.Entities
{


    [DataContract]
    public class Procedure: IGeneric
    {
        public Procedure()
        {
            //PieceOfEquipment = new HashSet<PieceOfEquipment>();
        }

        
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [Required( ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int ProcedureID { get; set; }

        [DataMember(Order = 2)]        
        //[Required(AllowEmptyStrings =false,ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "Name is too long.")]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        [RegularExpression(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$")]
        public string DocumentUrl { get; set; }



        [NotMapped]
        public virtual ICollection<WOD_Procedure> WOD_Procedure { get; set; }


    }

   
}
