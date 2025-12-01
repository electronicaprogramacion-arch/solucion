using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    public class rules<T>
    {

        public T MyProperty { get; set; }

        public Address ad { get; set; }

    }

     [DataContract]
    public class FileInfo 
    {

         [DataMember(Order = 1)]
        public DateTime LastModified { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public long Size  { get; set; }

        [DataMember(Order = 4)]
        public string Type { get; set; }

         [DataMember(Order = 5)]
        public string RelativePath { get; set; }

       [DataMember(Order = 6)]
        public byte[] Data { get; set; }

        
    }


    [DataContract]
    public partial class Address: IGeneric
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        //[Range(1, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int AddressId { get; set; }
        // public int TenantId { get; set; }

        [Required(ErrorMessage = "Required")]
        [MinLength(1, ErrorMessage ="Very little char")]
        
        [DataMember(Order = 2)]
        public string StreetAddress1 { get; set; }
        
        [DataMember(Order = 3)]
        public string StreetAddress2 { get; set; }


        [DataMember(Order = 4)]
        public string StreetAddress3 { get; set; }


        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [DataMember(Order = 5)]
        public string CityID { get; set; }


       
        [DataMember(Order = 6)]
        public string City { get; set; }

        [Required(ErrorMessage = "Required", AllowEmptyStrings =false)]
        [DataMember(Order = 7)]
        public string StateID { get; set; }


       
        [DataMember(Order = 8)]
        public string State { get; set; }
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 9)]
        public string ZipCode { get; set; }


        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 10)]
        public string CountryID { get; set; }
        
        
        [DataMember(Order = 11)]
        public string Country { get; set; }

        [DataMember(Order = 12)]
        public string Description { get; set; }

        
        [DataMember(Order = 13)]
        public int AggregateID { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 14)]
        public bool IsDefault { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 15)]
        public bool IsEnable { get; set; }

        //[Required(ErrorMessage = "Required")]
        //[DataMember(Order = 16)]
        //public string Status { get; set; }

        [IgnoreDataMember]
        public string Name { get; set; }

        [NotMapped]
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<WorkOrder> WorkOrders { get; set; }

       
        [DataMember(Order = 16)]
        public string County { get; set; }

        [DataMember(Order = 17)]
        public bool IsDelete { get; set; }

        [DataMember(Order = 18)]
        public int? CustomerAggregateAggregateID { get; set; }

        [DataMember(Order = 19)]
        public decimal? TravelExpense { get; set; }

        [NotMapped]
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<WorkOrderDetail> WorkOrderDetails { get; set; }

        
    }
}
