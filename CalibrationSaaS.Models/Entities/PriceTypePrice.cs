using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CalibrationSaaS.Domain.Aggregates.Interfaces;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    public enum EntityType
    {
        PieceOfEquipment = 1,
        EquipmentTemplate = 2
    }

    [DataContract]
    public class PriceTypePrice : IGeneric, INew<PriceTypePrice>
    {
        public PriceTypePrice()
        {
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
        }

        [Key]
        [Required]
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        [Required]
        public int PriceTypeId { get; set; }

        [DataMember(Order = 3)]
        [Required]
        public EntityType EntityType { get; set; }

        [DataMember(Order = 4)]
        [Required]
        public int EntityId { get; set; }

        [DataMember(Order = 5)]
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal Price { get; set; }

        [DataMember(Order = 6)]
        public DateTime CreatedDate { get; set; }

        [DataMember(Order = 7)]
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        [DataMember(Order = 8)]
        public virtual PriceType PriceType { get; set; }

        // Helper properties for entity identification
        [NotMapped]
        [IgnoreDataMember]
        public bool IsPieceOfEquipmentPrice
        {
            get { return EntityType == EntityType.PieceOfEquipment; }
        }

        [NotMapped]
        [IgnoreDataMember]
        public bool IsEquipmentTemplatePrice
        {
            get { return EntityType == EntityType.EquipmentTemplate; }
        }

        // Implementation of INew<PriceTypePrice>
        public static PriceTypePrice New(string component)
        {
            return new PriceTypePrice
            {
                Price = 0,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };
        }
    }
}
