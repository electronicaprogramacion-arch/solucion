using Microsoft.EntityFrameworkCore;

namespace TestProcedureEquipmentDbSet
{
    /// <summary>
    /// Test DbContext to verify ProcedureEquipment DbSet functionality
    /// </summary>
    public class TestDbContext : DbContext
    {
        public TestDbContext()
        {
        }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        // DbSets for testing
        public DbSet<ProcedureEquipmentSimple> ProcedureEquipment { get; set; }
        public DbSet<ProcedureSimple> Procedure { get; set; }
        public DbSet<PieceOfEquipmentSimple> PieceOfEquipment { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Use a test connection string - replace with your actual connection string
                optionsBuilder.UseSqlServer("Server=localhost;Database=CalibrationSaaSTest;Trusted_Connection=true;TrustServerCertificate=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ProcedureEquipmentSimple
            modelBuilder.Entity<ProcedureEquipmentSimple>(entity =>
            {
                entity.ToTable("ProcedureEquipment");
                entity.HasKey(pe => pe.Id);
                entity.Property(pe => pe.Id).ValueGeneratedOnAdd();

                entity.Property(pe => pe.ProcedureID).IsRequired();
                entity.Property(pe => pe.PieceOfEquipmentID)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnType("varchar(500)");
                entity.Property(pe => pe.CreatedDate)
                    .IsRequired()
                    .HasColumnType("datetime2(7)")
                    .HasDefaultValueSql("GETUTCDATE()");
                entity.Property(pe => pe.CreatedBy)
                    .HasMaxLength(100);

                // Create unique index to prevent duplicate associations
                entity.HasIndex(pe => new { pe.ProcedureID, pe.PieceOfEquipmentID })
                    .IsUnique()
                    .HasDatabaseName("IX_ProcedureEquipment_Unique");
                
                entity.HasIndex(pe => pe.ProcedureID)
                    .HasDatabaseName("IX_ProcedureEquipment_ProcedureID");
                
                entity.HasIndex(pe => pe.PieceOfEquipmentID)
                    .HasDatabaseName("IX_ProcedureEquipment_PieceOfEquipmentID");
            });

            // Configure ProcedureSimple
            modelBuilder.Entity<ProcedureSimple>(entity =>
            {
                entity.ToTable("Procedure");
                entity.HasKey(p => p.ProcedureID);
                entity.Property(p => p.ProcedureID).ValueGeneratedOnAdd();
                entity.Property(p => p.ProcedureName).IsRequired().HasMaxLength(200);
            });

            // Configure PieceOfEquipmentSimple
            modelBuilder.Entity<PieceOfEquipmentSimple>(entity =>
            {
                entity.ToTable("PieceOfEquipment");
                entity.HasKey(e => e.PieceOfEquipmentID);
                entity.Property(e => e.PieceOfEquipmentID)
                    .HasMaxLength(500)
                    .HasColumnType("varchar(500)");
                entity.Property(e => e.EquipmentName).IsRequired().HasMaxLength(200);
            });
        }
    }
}
