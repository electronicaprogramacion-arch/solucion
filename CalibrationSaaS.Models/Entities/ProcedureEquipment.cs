using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    /// <summary>
    /// Represents the many-to-many relationship between Procedures and PieceOfEquipment
    /// This entity tracks which procedures are associated with which equipment
    /// </summary>
    [DataContract]
    [Table("ProcedureEquipment")]
    public class ProcedureEquipment : IGeneric
    {
        public ProcedureEquipment()
        {
            CreatedDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Primary key for the ProcedureEquipment association
        /// </summary>
        [Key]
        [Required]
        [DataMember(Order = 1)]
        [Column("Id")]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the Procedure table
        /// </summary>
        [DataMember(Order = 2)]
        [Required]
        [Column("ProcedureID")]
        public int ProcedureID { get; set; }

        /// <summary>
        /// Foreign key to the PieceOfEquipment table
        /// Note: PieceOfEquipmentID is a string (VARCHAR(500)) not an int
        /// </summary>
        [DataMember(Order = 3)]
        [Required]
        [StringLength(500, ErrorMessage = "PieceOfEquipmentID is too long (500 character limit).")]
        [Column("PieceOfEquipmentID", TypeName = "varchar(500)")]
        public string PieceOfEquipmentID { get; set; }

        /// <summary>
        /// When this association was created
        /// </summary>
        [DataMember(Order = 4)]
        [Required]
        [Column("CreatedDate", TypeName = "datetime2(7)")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Who created this association
        /// </summary>
        [DataMember(Order = 5)]
        [StringLength(100, ErrorMessage = "CreatedBy is too long (100 character limit).")]
        [Column("CreatedBy")]
        public string? CreatedBy { get; set; }

        // Navigation properties
        /// <summary>
        /// Navigation property to the associated Procedure
        /// </summary>
        [DataMember(Order = 6)]
        [ForeignKey("ProcedureID")]
        public virtual Procedure Procedure { get; set; }

        /// <summary>
        /// Navigation property to the associated PieceOfEquipment
        /// </summary>
        [DataMember(Order = 7)]
        [ForeignKey("PieceOfEquipmentID")]
        public virtual PieceOfEquipment PieceOfEquipment { get; set; }

        // IGeneric implementation
        [NotMapped]
        [IgnoreDataMember]
        public bool IsNew { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public bool IsDeleted { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public bool IsModified { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public bool IsSelected { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public string SearchFilter { get; set; }
    }
}
