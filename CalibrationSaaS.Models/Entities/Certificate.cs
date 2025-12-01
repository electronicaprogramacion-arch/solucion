using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public class Certificate : IGeneric
    {
       public Certificate()
        {
           
        }

        [Key]
        [DataMember(Order = 1)]      
        public int CertificateID { get; set; }

       
        [DataMember(Order = 2)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]        
        [StringLength(50, ErrorMessage = "Name is too long.")]
        public string CertificateNumber { get; set; }
        
        [DataMember(Order = 3)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public DateTime DueDate { get; set; }


        [DataMember(Order = 4)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public DateTime CalibrationDate { get; set; }

        [DataMember(Order = 5)]
        public string Company { get; set; } //fk WeigthSet
        //[DataMember(Order = 4)]
        //public int CustomerId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 6)]
        public bool AffectDueDate { get; set; }

        [DataMember(Order = 7)]
        [StringLength(100, ErrorMessage = "Name is too long.")]
        public string Name { get; set; } = "";

        [DataMember(Order = 8)]
       
        [StringLength(100, ErrorMessage = "Name is too long.")]
        public string Description { get; set; } = "";

        //[DataMember(Order = 9)]
        //public int PieceOfEquipmentId { get; set; } //fk WeigthSet


        [DataMember(Order = 9)]
        public int Version { get; set; }

        [DataMember(Order = 10)]
        public int WorkOrderDetailId { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public WorkOrderDetail WorkOrderDetail { get; set; }

        [DataMember(Order = 11)]
        public string  WorkOrderDetailSerialized { get; set; }

    }
}
