using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestProcedureEquipmentDbSet
{
    /// <summary>
    /// Simplified version of ProcedureEquipment entity for testing
    /// </summary>
    [Table("ProcedureEquipment")]
    public class ProcedureEquipmentSimple
    {
        public ProcedureEquipmentSimple()
        {
            CreatedDate = DateTime.UtcNow;
        }

        [Key]
        [Required]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("ProcedureID")]
        public int ProcedureID { get; set; }

        [Required]
        [StringLength(500)]
        [Column("PieceOfEquipmentID", TypeName = "varchar(500)")]
        public string PieceOfEquipmentID { get; set; } = string.Empty;

        [Required]
        [Column("CreatedDate", TypeName = "datetime2(7)")]
        public DateTime CreatedDate { get; set; }

        [StringLength(100)]
        [Column("CreatedBy")]
        public string? CreatedBy { get; set; }
    }

    /// <summary>
    /// Simplified Procedure entity for testing relationships
    /// </summary>
    [Table("Procedure")]
    public class ProcedureSimple
    {
        [Key]
        [Column("ProcedureID")]
        public int ProcedureID { get; set; }

        [Required]
        [StringLength(200)]
        public string ProcedureName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Simplified PieceOfEquipment entity for testing relationships
    /// </summary>
    [Table("PieceOfEquipment")]
    public class PieceOfEquipmentSimple
    {
        [Key]
        [StringLength(500)]
        [Column("PieceOfEquipmentID", TypeName = "varchar(500)")]
        public string PieceOfEquipmentID { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string EquipmentName { get; set; } = string.Empty;
    }
}
