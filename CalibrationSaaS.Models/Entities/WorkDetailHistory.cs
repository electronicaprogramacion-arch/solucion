using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
   


    [DataContract]
    public  class WorkDetailHistory: IGeneric
    {
        //public CustomerAddress()
        //{
        //    WorkOrder = new HashSet<WorkOrder>();
        //}


        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int WorkDetailHistoryID { get; set; }
        // public int TenantId { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 2)]
        public int StatusId { get; set; }
        // public int TenantId { get; set; }
        [DataMember(Order = 3)]
        public string Name { get; set; }


        [DataMember(Order = 4)]
        public int? TechnicianID { get; set; }
        
        [DataMember(Order = 5)]
        public string TechnicianName { get; set; }


        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 6)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 7)]
        public string  Action  { get; set; }


        //[Required(ErrorMessage = "Required")]
        //[DataMember(Order = 8)]
        //public string Result { get; set; }


        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 8)]
        public bool IsEnable { get; set; }

        [DataMember(Order = 9)]
        public string Description { get ; set ; }


        [DataMember(Order = 10)]
        public int Version { get; set; }


        [DataMember(Order = 11)]
        public string  UserName { get; set; }

        [DataMember(Order = 12)]
        public string Data { get; set; }

        [DataMember(Order = 13)]
        public int  WorkOrderDetailID { get; set; }
        
       
        [IgnoreDataMember]
        [JsonIgnore]
        public virtual WorkOrderDetail WorkOrderDetail { get; set; }

        [DataMember(Order = 14)]
        public virtual User  Technician { get; set; }


    }

    [DataContract]
    public class WorkDetail_History 
    {

        [DataMember(Order = 1)]
        public int WorkOrderDetailID { get; set; }

        [DataMember(Order = 1)]
        public int WorkDetailHistoryID { get; set; }

        [DataMember(Order = 2)]
        public virtual WorkOrderDetail WorkOrderDetail { get; set; }

        [DataMember(Order = 3)]
        public virtual WorkDetailHistory WorkDetailHistory { get; set; }


    }



}
