using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public partial class WorkOrderDetailJob:IGeneric
    {
        public WorkOrderDetailJob()
        {

            
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int WorkOrderDetailJobID { get; set; }

       

        
        [NotMapped]
        [DataMember(Order = 4)]
        public int PieceOfEquipmentId { get; set; }
       
        [DataMember(Order = 16)]
        public string Description { get ; set ; }

        [NotMapped]
        [DataMember(Order = 18)]
        public  PieceOfEquipment PieceOfEquipment { get; set; }

        [NotMapped]
        [DataMember(Order = 26)]
        public  WorkOrder WorkOder { get; set; }

       
        public string Name { get ; set ; }
        
    }
}
