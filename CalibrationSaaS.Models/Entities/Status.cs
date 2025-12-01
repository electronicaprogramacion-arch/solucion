using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
   


    [DataContract]
    public  class Status: IGeneric
    {
        //public CustomerAddress()
        //{
        //    WorkOrder = new HashSet<WorkOrder>();
        //}
        [Key]
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int StatusId { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public string Description { get; set; }
     
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 4)]
        public bool IsDefault { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 5)]
        public bool IsEnable { get; set; }


        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 6)]
        public string Possibilities { get; set; }

       
        [IgnoreDataMember]
        public virtual ICollection<WorkOrderDetail> WorkOrderDetails { get; set; }

        [DataMember(Order = 7)]
        public bool IsLast { get; set; }

        [DataMember(Order = 8)]
        public int? Position { get; set; }
    }


    [DataContract]
    public class WOStatus : IGeneric
    {
        //public CustomerAddress()
        //{
        //    WorkOrder = new HashSet<WorkOrder>();
        //}
        [Key]
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int WOStatusId { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }


        [DataMember(Order = 3)]
        public bool Enabled { get; set; }




    }


    }
