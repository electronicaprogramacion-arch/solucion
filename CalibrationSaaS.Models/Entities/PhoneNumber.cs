using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public partial class PhoneNumber: IGeneric
    {
        //public CustomerAddress()
        //{
        //    WorkOrder = new HashSet<WorkOrder>();
        //}
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int PhoneNumberID { get; set; }
        // public int TenantId { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 2)]
        [StringLength(13, ErrorMessage = "Number is too long. (Max 8 digits")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 3)]
        public string TypeID { get; set; }
       
        //[DataMember(Order = 4)]
        //public string Type { get; set; }

        //[Required(ErrorMessage = "Required")]
        [DataMember(Order = 4)]
        public string CountryID { get; set; }

        //[DataMember(Order = 6)]
        //public string Country { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 5)]
        public bool IsEnabled { get; set; }

        [IgnoreDataMember]
        public string Name { get ; set ; }
        [IgnoreDataMember]
        public string Description { get ; set; }

        [DataMember(Order = 6)]
        public int? CustomerAggregateAggregateID { get; set; }
    }
}
