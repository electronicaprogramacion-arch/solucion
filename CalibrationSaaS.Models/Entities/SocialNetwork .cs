using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public partial class Social: IGeneric
    {
        //public CustomerAddress()
        //{
        //    WorkOrder = new HashSet<WorkOrder>();
        //}
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int SocialID { get; set; }
        // public int TenantId { get; set; }

        [Url]
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 2)]
        public string Link { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 3)]
        public string SocialTypeID { get; set; }

       
        [DataMember(Order = 4)]
        public string SocialType { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 5)]
        public bool IsEnabled { get; set; } = true;

        [DataMember(Order = 6)]
        public string Description { get; set; }


        //[DataMember(Order = 7)]
        //public int AggregateID { get; set; }

        [IgnoreDataMember]
        public string Name { get; set; }

        [DataMember(Order = 7)]
        public int? CustomerAggregateAggregateID { get; set; }
    }
}
