using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public partial class Customer:IGeneric
    {
        public Customer()
        {
            //CustomerAddress = new HashSet<CustomerAddress>();
            //WorkOrder = new HashSet<WorkOrder>();
            //PieceOfEquipment = new HashSet<PieceOfEquipment>();
            //Aggregates = new HashSet<CustomerAggregate>();
        }


        [DataMember(Order = 1)]     
        [Key]
        public int CustomerID { get; set; }
       
        [DataMember(Order =2)]
        [Required(ErrorMessage = "Name Required")]
        [StringLength(100, ErrorMessage = "CustomerName too long (100 character limit).")]
        public string Name { get; set; }
       
        [DataMember(Order = 3)]
        public int TenantId { get; set; }

        [DataMember(Order = 4)]
        public string Description { get; set; }

       
        [DataMember(Order = 5)]
        public virtual ICollection<CustomerAggregate> Aggregates { get; set; }

        [DataMember(Order = 6)]
        public bool IsDelete { get; set; }

        [DataMember(Order = 7)]
        [Range(0.01, 100.0, ErrorMessage = "Quote Multiplier must be between 0.01 and 100.0")]
        public decimal? QuoteMultiplier { get; set; }


        //[DataMember(Order = 5)]

        //public virtual ICollection<Contact> Contacts { get; set; }
        //// [IgnoreDataMember]
        //[DataMember(Order = 6)]
        //public virtual ICollection<Address> CustomerAddress { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<PieceOfEquipment> PieceOfEquipment { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<WorkOrder> WorkOrder { get; set; }

        [DataMember(Order = 8)]
        public string CustomID { get; set; }

        [NotMapped]
        [DataMember(Order = 9)]
        public int AddressId { get; set; }
    }
}
