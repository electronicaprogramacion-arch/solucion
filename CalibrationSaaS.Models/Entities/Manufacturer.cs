using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;


namespace CalibrationSaaS.Domain.Aggregates.Entities
{


    [DataContract]
    public class Manufacturer: IGeneric
    {
        public Manufacturer()
        {
            //PieceOfEquipment = new HashSet<PieceOfEquipment>();
        }

        [Key]
        //[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Required( ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int ManufacturerID { get; set; }

        [DataMember(Order = 2)]        
        [Required(AllowEmptyStrings =false,ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "Name is too long.")]
        public string Name { get; set; }

        [DataMember(Order = 3)]      
        [StringLength(500, ErrorMessage = "Name is too long.")]
        public string Description { get; set; }

        [DataMember(Order = 4)]       
        [StringLength(500, ErrorMessage = "Name is too long.")]
        public string ImageUrl { get; set; }

        [DataMember(Order = 5)]
        [StringLength(500, ErrorMessage = "Name is too long.")]
        public string Abbreviation { get; set; }


        [DataMember(Order = 6)]
        public bool IsEnabled { get; set; }

        [DataMember(Order = 7)]
        public bool IsDelete{ get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public virtual ICollection<PieceOfEquipment> PieceOfEquipment { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<EquipmentTemplate> EquipmentTemplates { get; set; }

        [NotMapped]
        [DataMember(Order = 8)]
        public bool IsOffline { get; set; }





    }
}
