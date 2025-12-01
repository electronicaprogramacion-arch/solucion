using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;
using CalibrationSaaS.Domain.Aggregates.Interfaces;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public class Quote : IGeneric, INew<Quote>
    {
        public Quote()
        {
            QuoteItems = new HashSet<QuoteItem>();
            Status = "Draft";
            Priority = "Low";
            ServiceType = "Laboratory";
            IsActive = true;
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
            CreatedBy = "System";
            ModifiedBy = "System";
        }

        [Key]
        [Required]
        [DataMember(Order = 1)]
        public int QuoteID { get; set; }

        [DataMember(Order = 2)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Quote number is required")]
        [StringLength(50, ErrorMessage = "Quote number is too long (50 character limit).")]
        public string QuoteNumber { get; set; }

        [DataMember(Order = 3)]
        public int? CustomerID { get; set; }

        [DataMember(Order = 4)]
        public int? CustomerAddressId { get; set; }

        [DataMember(Order = 5)]
        [StringLength(200, ErrorMessage = "Customer name is too long (200 character limit).")]
        public string CustomerName { get; set; }

        [DataMember(Order = 6)]
        [Range(0, double.MaxValue, ErrorMessage = "Total cost must be greater than or equal to 0")]
        public decimal TotalCost { get; set; }

        [DataMember(Order = 7)]
        [Required]
        [StringLength(50, ErrorMessage = "Status is too long (50 character limit).")]
        public string Status { get; set; }

        [DataMember(Order = 8)]
        [Required]
        [StringLength(20, ErrorMessage = "Priority is too long (20 character limit).")]
        public string Priority { get; set; }

        [DataMember(Order = 9)]
        public DateTime? EstimatedDelivery { get; set; }

        [DataMember(Order = 10)]
        public DateTime CreatedDate { get; set; }

        [DataMember(Order = 11)]
        [Required]
        [StringLength(100, ErrorMessage = "Created by is too long (100 character limit).")]
        public string CreatedBy { get; set; }

        [DataMember(Order = 12)]
        public DateTime ModifiedDate { get; set; }

        [DataMember(Order = 13)]
        [Required]
        [StringLength(100, ErrorMessage = "Modified by is too long (100 character limit).")]
        public string ModifiedBy { get; set; }

        [DataMember(Order = 14)]
        public string Notes { get; set; }

        [DataMember(Order = 15)]
        [Required]
        [StringLength(20, ErrorMessage = "Service type is too long (20 character limit).")]
        public string ServiceType { get; set; } = "Laboratory";

        [DataMember(Order = 16)]
        public bool IsActive { get; set; }

        // Navigation properties
        [DataMember(Order = 17)]
        public virtual Customer Customer { get; set; }

        [DataMember(Order = 18)]
        public virtual ICollection<QuoteItem> QuoteItems { get; set; }

        // Computed properties
        [NotMapped]
        [IgnoreDataMember]
        public decimal CalculatedTotalCost
        {
            get
            {
                return QuoteItems?.Sum(qi => qi.TotalPrice) ?? 0;
            }
        }

        [NotMapped]
        [IgnoreDataMember]
        public int ItemCount
        {
            get
            {
                return QuoteItems?.Count ?? 0;
            }
        }

        [NotMapped]
        public string Description { get; set; }

        // Computed property to get effective customer name
        [NotMapped]
        [IgnoreDataMember]
        public string EffectiveCustomerName
        {
            get
            {
                if (!string.IsNullOrEmpty(CustomerName))
                {
                    return CustomerName;
                }
                return Customer?.Name ?? string.Empty;
            }
        }

        // Helper property to check if quote has an official customer
        [NotMapped]
        [IgnoreDataMember]
        public bool HasOfficialCustomer
        {
            get
            {
                return CustomerID.HasValue && CustomerID.Value > 0;
            }
        }

        // Implementation of INew<Quote>
        public static Quote New(string component)
        {
            return new Quote
            {
                QuoteNumber = "",
                CreatedBy = "System",
                ModifiedBy = "System",
                Status = "Draft",
                Priority = "Low",
                ServiceType = "Laboratory",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                TotalCost = 0
            };
        }
    }
}
