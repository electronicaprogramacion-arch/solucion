using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CalibrationSaaS.Domain.Aggregates.Interfaces;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public class PriceType : IGeneric, INew<PriceType>
    {
        public PriceType()
        {
            IsActive = true;
            IncludesTravel = false;
            RequiresTravel = false;
            SortOrder = 0;
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
            PriceTypePrices = new HashSet<PriceTypePrice>();
        }

        [Key]
        [Required]
        [DataMember(Order = 1)]
        public int PriceTypeId { get; set; }

        [DataMember(Order = 2)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name is too long (100 character limit).")]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        [StringLength(500)]
        public string Description { get; set; }

        [DataMember(Order = 4)]
        public bool IsActive { get; set; }

        [DataMember(Order = 5)]
        public bool RequiresTravel { get; set; }

        [DataMember(Order = 6)]
        public int SortOrder { get; set; }

        [DataMember(Order = 7)]
        public DateTime CreatedDate { get; set; }

        [DataMember(Order = 8)]
        public DateTime ModifiedDate { get; set; }

        [DataMember(Order = 9)]
        [StringLength(100)]
        public string CreatedBy { get; set; }

        [DataMember(Order = 10)]
        [StringLength(100)]
        public string ModifiedBy { get; set; }

        [DataMember(Order = 11)]
        public bool IncludesTravel { get; set; }

        // Navigation properties
        [IgnoreDataMember]
        public virtual ICollection<PriceTypePrice> PriceTypePrices { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<QuoteItem> QuoteItems { get; set; }

        // Implementation of INew<PriceType>
        public static PriceType New(string component)
        {
            return new PriceType
            {
                Name = "",
                IncludesTravel = false,
                RequiresTravel = false,
                IsActive = true,
                SortOrder = 0,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };
        }
    }
}
