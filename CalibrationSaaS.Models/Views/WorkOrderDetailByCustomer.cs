using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using CalibrationSaaS.Domain.Aggregates.Entities;

namespace CalibrationSaaS.Domain.Aggregates.Views
{
    [DataContract]
    public partial class WorkOrderDetailByCustomer:IGeneric
    {
      
        [Key]
       
        [DataMember(Order = 1)]
        public int WorkOrderDetailID { get; set; }

     
        [DataMember(Order = 2)]
        public string Company { get; set; }

        [DataMember(Order = 3)]
        public string Model { get; set; }

        [DataMember(Order = 4)]
        public string SerialNumber { get; set; }

        [DataMember(Order = 5)]
        public string EquipmentType { get; set; }

        [DataMember(Order = 6)]
        public int WorkOrderId { get; set; }

        [DataMember(Order = 7)]
        public DateTime? WorkOrderReceiveDate { get; set; }

        [DataMember(Order = 8)]
        public string Status { get; set; }

        [DataMember(Order = 9)]
        public string Name { get; set; }

        [DataMember(Order = 10)]
        public string Description { get; set; }


        [DataMember(Order = 11)]
        public int EquipmentTypeID { get; set; }


        [DataMember(Order = 12)]
        public int StatusId { get; set; }


        [DataMember(Order = 13)]
        public DateTime? StatusDate { get; set; }

        [DataMember(Order = 14)]
        public string Address { get; set; }

        [DataMember(Order = 15)]
        public string City { get; set; }


        [DataMember(Order = 16)]
        public string ZipCode { get; set; }

        [DataMember(Order = 17)]
        public string State { get; set; }

        [DataMember(Order = 18)]
        public DateTime? DueDate { get; set; }

        [DataMember(Order = 19)]
        public string Manufacturer { get; set; }

        [DataMember(Order = 20)]
        public DateTime? DateEnd { get; set; }

        [DataMember(Order = 21)]
        public string County { get; set; }

    }
}
