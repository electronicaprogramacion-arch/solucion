using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CalibrationSaaS.Domain.Aggregates.Entities;

namespace CalibrationSaaS.Data.EntityFramework.Configuration
{
    /// <summary>
    /// Entity Framework configuration for ProcedureEquipment entity
    /// This fixes the DbSet creation issues
    /// </summary>
    public class ProcedureEquipmentConfiguration : IEntityTypeConfiguration<ProcedureEquipment>
    {
        public void Configure(EntityTypeBuilder<ProcedureEquipment> builder)
        {
            // Table configuration
            builder.ToTable("ProcedureEquipment");
            
            // Primary key
            builder.HasKey(pe => pe.Id);
            builder.Property(pe => pe.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            // ProcedureID configuration
            builder.Property(pe => pe.ProcedureID)
                .IsRequired()
                .HasColumnName("ProcedureID");

            // PieceOfEquipmentID configuration
            builder.Property(pe => pe.PieceOfEquipmentID)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("varchar(500)")
                .HasColumnName("PieceOfEquipmentID");

            // CreatedDate configuration
            builder.Property(pe => pe.CreatedDate)
                .IsRequired()
                .HasColumnType("datetime2(7)")
                .HasDefaultValueSql("GETUTCDATE()");

            // CreatedBy configuration
            builder.Property(pe => pe.CreatedBy)
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            // Relationship with Procedure
            builder.HasOne(pe => pe.Procedure)
                .WithMany() // Procedure doesn't have a collection of ProcedureEquipment
                .HasForeignKey(pe => pe.ProcedureID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ProcedureEquipment_Procedure_ProcedureID");

            // Relationship with PieceOfEquipment
            builder.HasOne(pe => pe.PieceOfEquipment)
                .WithMany() // PieceOfEquipment doesn't have a collection of ProcedureEquipment
                .HasForeignKey(pe => pe.PieceOfEquipmentID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ProcedureEquipment_PieceOfEquipment_PieceOfEquipmentID");

            // Unique index to prevent duplicate associations
            builder.HasIndex(pe => new { pe.ProcedureID, pe.PieceOfEquipmentID })
                .IsUnique()
                .HasDatabaseName("IX_ProcedureEquipment_Unique");

            // Additional indexes for performance
            builder.HasIndex(pe => pe.ProcedureID)
                .HasDatabaseName("IX_ProcedureEquipment_ProcedureID");

            builder.HasIndex(pe => pe.PieceOfEquipmentID)
                .HasDatabaseName("IX_ProcedureEquipment_PieceOfEquipmentID");

            // Ignore IGeneric properties that shouldn't be mapped
            builder.Ignore(pe => pe.IsNew);
            builder.Ignore(pe => pe.IsDeleted);
            builder.Ignore(pe => pe.IsModified);
            builder.Ignore(pe => pe.IsSelected);
            builder.Ignore(pe => pe.SearchFilter);
        }
    }
}
