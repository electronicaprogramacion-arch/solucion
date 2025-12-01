using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public class QuoteItem : IGeneric
    {
        public QuoteItem()
        {
            Quantity = 1;
            IsParent = false;
            SortOrder = 0;
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
            ChildItems = new HashSet<QuoteItem>();
        }

        [Key]
        [Required]
        [DataMember(Order = 1)]
        public int QuoteItemID { get; set; }

        [DataMember(Order = 2)]
        [Required]
        public int QuoteID { get; set; }

        [DataMember(Order = 3)]
        public string PieceOfEquipmentID { get; set; }

        [DataMember(Order = 4)]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [DataMember(Order = 5)]
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Unit price must be greater than or equal to 0")]
        public decimal UnitPrice { get; set; }

        [DataMember(Order = 6)]
        [StringLength(500, ErrorMessage = "Item description is too long (500 character limit).")]
        public string ItemDescription { get; set; }

        [DataMember(Order = 7)]
        public int? ParentQuoteItemID { get; set; }

        [DataMember(Order = 8)]
        public bool IsParent { get; set; }

        [DataMember(Order = 9)]
        public int SortOrder { get; set; }

        [DataMember(Order = 10)]
        public DateTime CreatedDate { get; set; }

        [DataMember(Order = 11)]
        public DateTime ModifiedDate { get; set; }

        [DataMember(Order = 12)]
        [StringLength(200, ErrorMessage = "Equipment type display is too long (200 character limit).")]
        public string EquipmentTypeDisplay { get; set; }

        [DataMember(Order = 13)]
        public int? PriceTypeId { get; set; }

        // Navigation properties
        [IgnoreDataMember]
        public virtual Quote Quote { get; set; }

        [DataMember(Order = 14)]
        public virtual PieceOfEquipment PieceOfEquipment { get; set; }

        [DataMember(Order = 15)]
        public virtual QuoteItem ParentQuoteItem { get; set; }

        [DataMember(Order = 16)]
        public virtual PriceType PriceType { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<QuoteItem> ChildItems { get; set; }

        // Computed properties
        [NotMapped]
        [IgnoreDataMember]
        public decimal TotalPrice
        {
            get
            {
                return Quantity * UnitPrice;
            }
        }

        [NotMapped]
        [IgnoreDataMember]
        public string EquipmentDescription
        {
            get
            {
                if (PieceOfEquipment != null)
                {
                    var equipmentName = PieceOfEquipment.EquipmentTemplate?.Name ?? "Unknown Equipment";
                    return $"{equipmentName} - {PieceOfEquipment.SerialNumber}";
                }
                return "No Equipment Selected";
            }
        }

        // Helper property to check if equipment is selected
        [NotMapped]
        [IgnoreDataMember]
        public bool HasEquipment
        {
            get
            {
                return !string.IsNullOrEmpty(PieceOfEquipmentID) && PieceOfEquipment != null;
            }
        }

        // Property to get Equipment Type Group and Type display
        [NotMapped]
        [IgnoreDataMember]
        public string EquipmentTypeGroupAndType
        {
            get
            {
                if (PieceOfEquipment?.EquipmentTemplate?.EquipmentTypeObject != null)
                {
                    var equipmentType = PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject;
                    var equipmentTypeGroup = equipmentType.EquipmentTypeGroup;

                    if (equipmentTypeGroup != null)
                    {
                        return $"{equipmentTypeGroup.Name} - {equipmentType.Name}";
                    }
                    return $"Unknown Group - {equipmentType.Name}";
                }
                return EquipmentTypeDisplay ?? "Manual Entry Required";
            }
        }

        [NotMapped]
        public string Description { get; set; }
    }
}
