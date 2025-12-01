using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Views;
using CalibrationSaaS.Domain.BusinessExceptions;
using Helpers.Controls;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using Audit.EntityFramework;

namespace CalibrationSaaS.Data.EntityFramework
{
    public class ApplicationUser : IdentityUser
    {

    }


    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }


    public partial class CalibrationSaaSDBContext : AuditDbContext, ICalibrationSaaSDBContextBase
    {

        //public virtual DbSet<DynamicReport> DynamicReport { get; set; }

        //public virtual DbSet<Report_SubType> Report_SubType { get; set; }


        //public virtual DbSet<SubType_DynamicProperty> SubType_DynamicProperty { get; set; }

        public DbSet<WOStatus> WOStatus { get; set; }

        public DbSet<CustomSequence> CustomSequence { get; set; }

        public virtual DbSet<BalanceAndScaleCalibration> BalanceAndScaleCalibration { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }

        public virtual DbSet<Eccentricity> Eccentricity { get; set; }
        public virtual DbSet<EquipmentCondition> EquipmentCondition { get; set; }
        public virtual DbSet<Linearity> Linearity { get; set; }
        public virtual DbSet<PieceOfEquipment> PieceOfEquipment { get; set; }
        public virtual DbSet<Repeatability> Repeatability { get; set; }
        public virtual DbSet<Tenant> Tenant { get; set; }
        public virtual DbSet<CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder> WorkOrder { get; set; }
        public virtual DbSet<WorkOrderDetail> WorkOrderDetail { get; set; }
        public virtual DbSet<WorkDetailHistory> WorkDetailHistory { get; set; }


        public virtual DbSet<UserInformation> UserInformation { get; set; }
        public virtual DbSet<User> User { get; set; }

        public virtual DbSet<EquipmentType> EquipmentType { get; set; }

        public virtual DbSet<EquipmentTemplate> EquipmentTemplate { get; set; }

        public virtual DbSet<Manufacturer> Manufacturer { get; set; }
        public virtual DbSet<Procedure> Procedure { get; set; }
        public virtual DbSet<ProcedureEquipment> ProcedureEquipment { get; set; }

        public virtual DbSet<CustomerAggregate> CustomerAggregates { get; set; }

        public virtual DbSet<Contact> Contact { get; set; }

        public virtual DbSet<Address> Address { get; set; }

        public virtual DbSet<EmailAddress> EmailAddress { get; set; }



        public virtual DbSet<TestPointGroup> TestPointGroup { get; set; }


        public virtual DbSet<TestPoint> TestPoint { get; set; }

        public virtual DbSet<RangeTolerance> RangeTolerance { get; set; }


        public virtual DbSet<UnitOfMeasure> UnitOfMeasure { get; set; }
        public virtual DbSet<Status> Status { get; set; }

        public virtual DbSet<Rol> Rol { get; set; }

        public virtual DbSet<UnitOfMeasureType> UnitOfMeasureType { get; set; }

        public virtual DbSet<User_Rol> User_Rol { get; set; }

        public virtual DbSet<PhoneNumber> PhoneNumber { get; set; }

        public DbSet<POE_WorkOrder> POE_WorkOrder { get; set; }

        public DbSet<User_WorkOrder> User_WorkOrder { get; set; }

        public DbSet<RepeatibilityCalibrationResult> RepeatibilityCalibrationResult { get; set; }

        public DbSet<EccentricityCalibrationResult> EccentricityCalibrationResult { get; set; }

        public DbSet<BasicCalibrationResult> BasicCalibrationResult { get; set; }


        public virtual DbSet<WeightSet> WeightSet { get; set; }


        public virtual DbSet<WOD_Weight> WOD_Weight { get; set; }
       
        public virtual DbSet<WO_Weight> WO_Weight { get; set; }

        public virtual DbSet<Micro> Micro { get; set; }

        public virtual DbSet<MicroResult> MicroResult { get; set; }

        public virtual DbSet<WOD_Standard> WOD_Standard { get; set; }

        public virtual DbSet<WO_Standard> WO_Standard { get; set; }

        public virtual DbSet<WOD_TestPoint> WOD_TestPoint { get; set; }

        public virtual DbSet<CalibrationSubType_Weight> CalibrationSubType_Weight { get; set; }


        public virtual DbSet<CalibrationSubType> CalibrationSubType { get; set; }


        public virtual DbSet<Certification> Certification { get; set; }

        public virtual DbSet<TechnicianCode> TechnicianCode { get; set; }
        public virtual DbSet<WorkOrderDetailByStatus> WorkOrderDetailByStatus { get; set; }
        public virtual DbSet<WorkOrderDetailByStatus> WorkOrderDetailByEquipment { get; set; }
        public virtual DbSet<WorkOrderDetailByCustomer> WorkOrderDetailByCustomer { get; set; }


        public virtual DbSet<CalibrationType> CalibrationType { get; set; }

        public virtual DbSet<Social> Social { get; set; }

        public virtual DbSet<POE_User> POE_User { get; set; }

        public virtual DbSet<Certificate> Certificate { get; set; }

        public virtual DbSet<CertificatePoE> CertificatePoE { get; set; }


        public virtual DbSet<POE_POE> POE_POE { get; set; }

        public virtual DbSet<Component> Component { get; set; }


        public virtual DbSet<Force> Force { get; set; }

        public virtual DbSet<ForceResult> ForceResult { get; set; }

        public virtual DbSet<Uncertainty> Uncertainty { get; set; }

        public virtual DbSet<CalibrationResultContributor> CalibrationResultContributor { get; set; }

        public virtual DbSet<TestCode> TestCode { get; set; }

        public virtual DbSet<ToleranceType> ToleranceType { get; set; }


        //public virtual DbSet<ToleranceType_EquipmentType> ToleranceType_EquipmentType { get; set; }


        public virtual DbSet<Rockwell> Rockwell { get; set; }

        public virtual DbSet<RockwellResult> RockwellResult { get; set; }

        public virtual DbSet<POE_Scale> POE_Scale { get; set; }

        public virtual DbSet<ExternalCondition> ExternalCondition { get; set; }

        public virtual DbSet<CalibrationSubType_Standard> CalibrationSubType_Standard { get; set; }


        public virtual DbSet<Note> Note { get; set; }

        public virtual DbSet<NoteWOD> NoteWOD { get; set; }

        public virtual DbSet<Bogus.DynamicProperty> DynamicProperty { get; set; }

        public virtual DbSet<Bogus.ViewPropertyBase> ViewPropertyBase { get; set; }

        public virtual DbSet<GenericCalibration> GenericCalibration { get; set; }

        public virtual DbSet<GenericCalibrationResult> GenericCalibrationResult { get; set; }

        //public virtual DbSet<CalibrationSubType_DynamicProperty> CalibrationSubType_DynamicProperty { get; set; }

        //public virtual DbSet<CalibrationSubType_ViewProperty> CalibrationSubType_ViewProperty { get; set; }

        public virtual DbSet<CalibrationSubTypeView> CalibrationSubTypeView { get; set; }


        public virtual DbSet<GenericCalibration2> GenericCalibration2 { get; set; }


        public virtual DbSet<GenericCalibrationResult2> GenericCalibrationResult2 { get; set; }


        public virtual DbSet<Mass> Mass { get; set; }

        public virtual DbSet<Lenght> Lenght { get; set; }


        public virtual DbSet<EquipmentTypeGroup> EquipmentTypeGroup { get; set; }

        public virtual DbSet<WOD_Procedure> WOD_Procedure { get; set; }

        public virtual DbSet<WOD_ParametersTable> WOD_ParametersTable { get; set; }

        public virtual DbSet<CMCValues> CMCValues { get; set; }

        // Audit Logging
        public virtual DbSet<AuditLog> AuditLogs { get; set; }

        // Quotes Module entities
        public virtual DbSet<Quote> Quote { get; set; }
        public virtual DbSet<QuoteItem> QuoteItem { get; set; }
        public virtual DbSet<PriceType> PriceType { get; set; }
        public virtual DbSet<PriceTypePrice> PriceTypePrice { get; set; }

        // Equipment Recalls
        public virtual DbSet<CustomerAddress> CustomerAddress { get; set; }

        private readonly IConfiguration _configuration;
        public CalibrationSaaSDBContext(DbContextOptions<CalibrationSaaSDBContext> options, IConfiguration configuration)
        :base(options)
        {
            _configuration = configuration;

            TenantName =  _configuration["Reports:Customer"];

            //Configuration.GetSection("Reports")["Customer"];

            // AUDIT FIX: Configure audit settings for this context instance
            ConfigureAuditSettings();
        }

        /// <summary>
        /// Configure audit settings for this DbContext instance
        /// </summary>
        private void ConfigureAuditSettings()
        {
            // Configure Entity Framework audit settings
            this.AuditDisabled = false;

            // Configure selective auditing for better performance
            this.IncludeEntityObjects = true; // Include entity objects for audit data
        }

        /// <summary>
        /// Check if audit logging is enabled in configuration
        /// </summary>
        private bool IsAuditLoggingEnabled()
        {
            try
            {
                if (_configuration != null)
                {
                    var auditSection = _configuration.GetSection("AuditSettings");
                    if (auditSection.Exists())
                    {
                        var enableValue = auditSection["EnableAuditLogging"];
                        return bool.TryParse(enableValue, out var result) && result;
                    }
                }

                // Fallback to environment variable
                var configValue = Environment.GetEnvironmentVariable("AUDIT_LOGGING_ENABLED");
                if (!string.IsNullOrEmpty(configValue))
                {
                    return bool.TryParse(configValue, out var result) && result;
                }

                // Default to false for performance (audit must be explicitly enabled)
                return false;
            }
            catch
            {
                return false; // Default to disabled if configuration check fails
            }
        }

        /// <summary>
        /// Capture current values for audit logging - simple approach that stores new values
        /// Users can compare with previous audit records to see changes
        /// </summary>
        private void CaptureCurrentValuesForAudit()
        {
            // Only audit these specific entities for performance
            var auditableEntities = new[]
            {
                "Manufacturer",
                "EquipmentTemplate",
                "PieceOfEquipment",
                "WorkOrder",
                "WorkOrderDetail"
            };

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Modified &&
                    auditableEntities.Contains(entry.Entity.GetType().Name))
                {
                    var entityTypeName = entry.Entity.GetType().Name;
                    var primaryKey = GetPrimaryKeyValue(entry);
                    var key = $"OriginalValues_{entityTypeName}_{primaryKey}";

                    try
                    {
                        // Store current values (the new values being saved)
                        var currentValues = new System.Collections.Generic.Dictionary<string, object>();

                        foreach (var property in entry.Properties.Where(p => p.IsModified))
                        {
                            currentValues[property.Metadata.Name] = property.CurrentValue;
//                            Console.WriteLine($"DBCONTEXT AUDIT: {entityTypeName}.{property.Metadata.Name} = '{property.CurrentValue}'");
                        }

                        if (currentValues.Any())
                        {
                            // Store in the audit storage
                            AuditOriginalValuesStorage.StoreOriginalValues(key, currentValues);
//                            Console.WriteLine($"DBCONTEXT AUDIT: Stored {currentValues.Count} current values for {entityTypeName} ID {primaryKey}");
                        }
                    }
                    catch (Exception ex)
                    {
//                        Console.WriteLine($"DBCONTEXT AUDIT ERROR: Failed to capture current values for {entityTypeName} ID {primaryKey}: {ex.Message}");
                    }
                }
            }
        }



        /// <summary>
        /// Get primary key value from entity entry
        /// </summary>
        private object GetPrimaryKeyValue(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry)
        {
            try
            {
                var keyProperties = entry.Metadata.FindPrimaryKey()?.Properties;
                if (keyProperties == null || !keyProperties.Any())
                    return "Unknown";

                if (keyProperties.Count == 1)
                {
                    return entry.Property(keyProperties.First().Name).CurrentValue ?? "Unknown";
                }

                var keyValues = keyProperties.Select(p => entry.Property(p.Name).CurrentValue?.ToString() ?? "null");
                return string.Join("_", keyValues);
            }
            catch (Exception ex)
            {
//                Console.WriteLine($"DBCONTEXT AUDIT ERROR: Failed to get primary key: {ex.Message}");
                return "Unknown";
            }
        }





        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class
        {
            var a = base.Add(entity);


            return a;

        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            try
            {
                // AUDIT: Store current values for audit logging (only if audit is enabled)
                if (IsAuditLoggingEnabled())
                {
                    CaptureCurrentValuesForAudit();
                }

                int i = await base.SaveChangesAsync();
                return i;

            }

            catch (Exception ex)
            {
                string msg = "";



                if (ex.InnerException != null)
                {
                    msg = ex.InnerException.Message;
                }

                if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                {
                    UpdateSoftDeleteStatuses();
                    try
                    {

                        return await base.SaveChangesAsync();

                    }
                    catch (Exception ex1)
                    {
                        base.ChangeTracker.Clear();

                        if (ex1.Source == "Microsoft.EntityFrameworkCore.Relational")
                        {
                            if (ex1.Source.Contains("FOREIGN KEY") || ex1.Source.Contains("constraint"))
                            {
                                throw new AlreadyInUseException("Already In Use record " + Environment.NewLine + " InnerException: " + msg);
                            }
                            else
                            {
                                throw new AlreadyInUseException(ex1.Message + Environment.NewLine + " InnerException: " + msg);
                            }

                        }
                    }

                }

                else
                if (ex.InnerException != null && ex.InnerException.Message.Contains("Invalid column name"))
                {
                    base.ChangeTracker.Clear();
                    throw new SchemaException("Problem with DB the schema");
                }

                base.ChangeTracker.Clear();



                throw ex;
            }

        }
        public override int SaveChanges()
        {
            // AUDIT: Store current values for audit logging (only if audit is enabled)
            if (IsAuditLoggingEnabled())
            {
                CaptureCurrentValuesForAudit();
            }

            UpdateSoftDeleteStatuses();
            return base.SaveChanges();
        }

        public bool PropertyExists(string path, Type t)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            var pp = path.Split('.');

            foreach (var prop in pp)
            {
                if (int.TryParse(prop, out var result))
                {

                    continue;
                }

                var propInfo = t.GetMember(prop)
                    .Where(p => p is PropertyInfo)
                    .Cast<PropertyInfo>()
                    .FirstOrDefault();

                if (propInfo != null)
                {
                    t = propInfo.PropertyType;

                    if (t.GetInterfaces().Contains(typeof(IEnumerable)) && t != typeof(string))
                    {
                        t = t.IsGenericType
                            ? t.GetGenericArguments()[0]
                            : t.GetElementType();

                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public bool TryDelete(int id)
        {
            try
            {
                return true;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547) return false;
                throw;
            }
        }

        private void UpdateSoftDeleteStatuses()
        {
            foreach (var entry in ChangeTracker.Entries())
            {

                if (PropertyExists("IsDelete", entry.Entity.GetType()))
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.CurrentValues["IsDelete"] = false;
                            break;
                        case EntityState.Deleted:

                            entry.State = EntityState.Modified;
                            entry.CurrentValues["IsDelete"] = true;
                            break;
                    }
                }
            }
        }



        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="optionsBuilder"></param>
        //public override EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class
        //{
        //    var a = base.Add(entity);


        //    return a;

        //}

        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{

        //    try
        //    {

        //        int i = await base.SaveChangesAsync();
        //        return i;

        //    }

        //    catch (Exception ex)
        //    {
        //        string msg = "";

        //        base.ChangeTracker.Clear();

        //        if (ex.InnerException != null)
        //        {
        //            msg = ex.InnerException.Message;
        //        }

        //        if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
        //        {
        //            UpdateSoftDeleteStatuses();
        //            try
        //            {
        //                return await base.SaveChangesAsync();
        //            }
        //            catch (Exception ex1)
        //            {
        //                if (ex1.Source == "Microsoft.EntityFrameworkCore.Relational")
        //                {
        //                    if (ex1.Source.Contains("FOREIGN KEY") || ex1.Source.Contains("constraint"))
        //                    {
        //                        throw new AlreadyInUseException("Already In Use record " + Environment.NewLine + " InnerException: " + msg);
        //                    }
        //                    else
        //                    {
        //                        throw new AlreadyInUseException(ex1.Message + Environment.NewLine + " InnerException: " + msg);
        //                    }

        //                }
        //            }

        //        }

        //        else
        //        if (ex.InnerException != null && ex.InnerException.Message.Contains("Invalid column name"))
        //        {
        //            throw new SchemaException("Problem with DB the schema");
        //        }





        //        throw ex;
        //    }

        //}
        //public override int SaveChanges()
        //{
        //    UpdateSoftDeleteStatuses();
        //    return base.SaveChanges();
        //}

        //public bool PropertyExists(string path, Type t)
        //{
        //    if (string.IsNullOrEmpty(path))
        //        return false;

        //    var pp = path.Split('.');

        //    foreach (var prop in pp)
        //    {
        //        if (int.TryParse(prop, out var result))
        //        {

        //            continue;
        //        }

        //        var propInfo = t.GetMember(prop)
        //            .Where(p => p is PropertyInfo)
        //            .Cast<PropertyInfo>()
        //            .FirstOrDefault();

        //        if (propInfo != null)
        //        {
        //            t = propInfo.PropertyType;

        //            if (t.GetInterfaces().Contains(typeof(IEnumerable)) && t != typeof(string))
        //            {
        //                t = t.IsGenericType
        //                    ? t.GetGenericArguments()[0]
        //                    : t.GetElementType();

        //            }
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        //public bool TryDelete(int id)
        //{
        //    try
        //    {
        //        return true;
        //    }
        //    catch (SqlException ex)
        //    {
        //        if (ex.Number == 547) return false;
        //        throw;
        //    }
        //}

        //private void UpdateSoftDeleteStatuses()
        //{
        //    foreach (var entry in ChangeTracker.Entries())
        //    {

        //        if (PropertyExists("IsDelete", entry.Entity.GetType()))
        //        {
        //            switch (entry.State)
        //            {
        //                case EntityState.Added:
        //                    entry.CurrentValues["IsDelete"] = false;
        //                    break;
        //                case EntityState.Deleted:

        //                    entry.State = EntityState.Modified;
        //                    entry.CurrentValues["IsDelete"] = true;


        //                    break;
        //            }
        //        }
        //    }
        //}



        string TenantName = "";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                   .EnableDetailedErrors()
                   .EnableSensitiveDataLogging(true)
                   .EnableServiceProviderCaching(true)
                   .EnableThreadSafetyChecks(true)
                   // Suppress all EF Core warnings to keep console clean
                   .ConfigureWarnings(warnings => warnings.Default(WarningBehavior.Ignore));


            


            if (!optionsBuilder.IsConfigured)
            {



                //var configuration = new ConfigurationBuilder();

                //   configuration.use(Directory.GetCurrentDirectory())
                //   .SetBasePath(Directory.GetCurrentDirectory())
                //   //.AddJsonFile("appsettings.json")
                //   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                //   .Build();
                //var connectionString = configuration.GetConnectionString("DefaultConnection");
                //optionsBuilder.UseSqlServer(@"Server=Server=DESKTOP-48PT01K\SQLEXPRESS;Database=CalibrationSaaSDB_Yenny1;Trusted_Connection=True;MultipleActiveResultSets=true");

          
                var a = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);


                //optionsBuilder.UseSqlServer(@"Server=DESKTOP-48PT01K;Database=CalibrationSaaSDB-Staging;User Id=cs;Password=cs;MultipleActiveResultSets=True");


                ////////for localdb
                //var currentRoute = Environment.CurrentDirectory;

                //optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + currentRoute + @"\LocalDb.mdf;Integrated Security=True");

            }

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {





            //modelBuilder.Entity<DynamicReport>().Ignore(e => e.CalibrationSubTypes);


            modelBuilder.Entity<UnitOfMeasure>().Ignore(e => e.Linearities);

            modelBuilder.Entity<UnitOfMeasure>().Ignore(e => e.BasicCalibrationResult);

            modelBuilder.Entity<UnitOfMeasure>().Ignore(e => e.UncertaintyUnitOfMeasure);

            modelBuilder.Entity<BalanceAndScaleCalibration>().Ignore(e => e.WorkOrderDetail);

            modelBuilder.Entity<Linearity>().Ignore(e => e.BalanceAndScaleCalibration);

            modelBuilder.Entity<Eccentricity>().Ignore(e => e.BalanceAndScaleCalibration);

            modelBuilder.Entity<Repeatability>().Ignore(e => e.BalanceAndScaleCalibration);

            modelBuilder.Entity<BasicCalibrationResult>().Ignore(e => e.Linearity);

            modelBuilder.Entity<BasicCalibrationResult>().Ignore(e => e.Eccentricity);

            modelBuilder.Entity<BasicCalibrationResult>().Ignore(e => e.Repeatability);

            modelBuilder.Entity<WorkOrderDetail>().Ignore(e => e.Tolerance);

            modelBuilder.Entity<EquipmentTemplate>().Ignore(e => e.Tolerance);

            modelBuilder.Entity<PieceOfEquipment>().Ignore(e => e.Tolerance);

            modelBuilder.Entity<WorkOrderDetail>().Ignore(e => e.IsModifiedOff);

            modelBuilder.Entity<WorkOrderDetail>().Ignore(e => e.IsOffline);



            modelBuilder.Entity<EquipmentCondition>().HasKey(f => f.EquipmentConditionId);
            modelBuilder.Entity<EquipmentCondition>().Property(f => f.EquipmentConditionId)
            .ValueGeneratedNever();

            modelBuilder.Entity<ExternalCondition>().HasKey(f => f.ExternalConditionId);
            modelBuilder.Entity<ExternalCondition>().Property(f => f.ExternalConditionId)
            .ValueGeneratedNever();


            modelBuilder.Entity<UnitOfMeasureType>().HasData(
            new UnitOfMeasureType { Value = 1, Name = "Temperature" });

            modelBuilder.Entity<UnitOfMeasureType>().HasData(
            new UnitOfMeasureType { Value = 2, Name = "Humidity" });

            modelBuilder.Entity<UnitOfMeasureType>().HasData(
            new UnitOfMeasureType { Value = 3, Name = "Weight" });


            modelBuilder.Entity<UnitOfMeasure>().HasData(
            new UnitOfMeasure { Name = "Pounds", Abbreviation = "Pd", UnitOfMeasureID = 1, TypeID = 3, IsEnabled = true, UncertaintyUnitOfMeasureID = 1 });

            modelBuilder.Entity<UnitOfMeasure>().HasData(
            new UnitOfMeasure { Name = "Kilogramo", Abbreviation = "Kg", UnitOfMeasureID = 2, TypeID = 3, IsEnabled = true, UncertaintyUnitOfMeasureID = 2 });


            //modelBuilder.Entity<WorkOrder>().Property(b => b.WorkOrderId).HasValueGenerator(typeof(UserGenerator));

            ////     modelBuilder.HasSequence<int>("OrderNumbers", schema: "shared")
            ////.StartsAt(1000)
            ////.IncrementsBy(5);

            //modelBuilder.Entity<WorkOrder>()
            //       .Property(b => b.WorkOrderId)
            //       .UseHiLo();

            modelBuilder.Entity<EquipmentTemplate>()
                .HasMany(e => e.PieceOfEquipments).WithOne(E => E.EquipmentTemplate).HasForeignKey(e => e.EquipmentTemplateId);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.PieceOfEquipment).WithOne(E => E.Customer).HasForeignKey(e => e.CustomerId);




            modelBuilder.Entity<Customer>()
                .HasMany(e => e.WorkOrder).WithOne(E => E.Customer).HasForeignKey(e => e.CustomerId);

            modelBuilder.Entity<Address>()
                .HasMany(e => e.WorkOrders).WithOne(E => E.Address).HasForeignKey(e => e.AddressId);

            modelBuilder.Entity<User>()
            .HasIndex(p => p.Email).IsUnique(true);

            modelBuilder.Entity<DynamicProperty>()
            .HasKey(bc => new { bc.DynamicPropertyID });

            modelBuilder.Entity<DynamicProperty>()
            .HasOne(e => e.ViewPropertyBase).WithOne(E => E.DynamicProperty).HasForeignKey<ViewPropertyBase>(x => x.DynamicPropertyID).OnDelete(DeleteBehavior.ClientNoAction);


            modelBuilder.Entity<ToleranceType>()
            .HasKey(bc => new { bc.Key, bc.CalibrationTypeId });





            modelBuilder.Entity<ViewPropertyBase>()
            .HasKey(bc => new { bc.ViewPropertyID });


            modelBuilder.Entity<ViewPropertyBase>()
            .Property(e => e.ViewPropertyID)
            .ValueGeneratedNever();



            modelBuilder.Entity<CalibrationSubType_DynamicProperty>()
            .HasKey(bc => new { bc.CalibrationSubTypeId, bc.DynamicPropertyID });

            modelBuilder.Entity<CalibrationSubType_DynamicProperty>()
            .HasOne(bc => bc.CalibrationSubType).WithMany(bc => bc.DynamicProperties).HasForeignKey(bc => bc.DynamicPropertyID);



            modelBuilder.Entity<CalibrationSubType_ViewProperty>()
            .HasKey(bc => new { bc.CalibrationSubTypeId, bc.ViewPropertyID });

            modelBuilder.Entity<CalibrationSubType_ViewProperty>()
            .HasOne(bc => bc.CalibrationSubType).WithMany(bc => bc.RenderProperties).HasForeignKey(bc => bc.ViewPropertyID);


            modelBuilder.Entity<CalibrationSubTypeView>()
            .HasKey(bc => new { bc.CalibrationSubTypeViewID });


            modelBuilder.Entity<CalibrationSubType>()
           .HasOne(e => e.CalibrationSubTypeView).WithOne(E => E.CalibrationSubType).HasForeignKey<CalibrationSubType>(x => x.CalibrationSubTypeViewID).OnDelete(DeleteBehavior.ClientNoAction); ;


            modelBuilder.Entity<ToleranceType>()
            .HasKey(bc => new { bc.Key });

            modelBuilder.Entity<CalibrationType>()
                .HasMany(bc => bc.ToleranceTypes)
                .WithOne(bc => bc.CalibrationType)
                .HasForeignKey(bc => bc.CalibrationTypeId);

            //   modelBuilder.Entity<ToleranceType_EquipmentType>()
            //.HasKey(bc => new { bc.Key, bc.EquipmentTypeID });
            //   modelBuilder.Entity<ToleranceType_EquipmentType>()
            //       .HasOne(bc => bc.ToleranceType)
            //       .WithMany(b => b.ToleranceType_EquipmentType)
            //       .HasForeignKey(bc => bc.Key);
            //   modelBuilder.Entity<ToleranceType_EquipmentType>()
            //       .HasOne(bc => bc.EquipmentType)
            //       .WithMany(c => c.ToleranceType_EquipmentType)
            //       .HasForeignKey(bc => bc.EquipmentTypeID);





            modelBuilder.Entity<User_Rol>()
            .HasKey(bc => new { bc.UserID, bc.RolID });
            modelBuilder.Entity<User_Rol>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.UserRoles)
                .HasForeignKey(bc => bc.UserID);
            modelBuilder.Entity<User_Rol>()
                .HasOne(bc => bc.Rol)
                .WithMany(c => c.UserRoles)
                .HasForeignKey(bc => bc.RolID);


            modelBuilder.Entity<User_WorkOrder>()
            .HasKey(bc => new { bc.UserID, bc.WorkOrderID });
            modelBuilder.Entity<User_WorkOrder>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.UserWorkOrders)
                .HasForeignKey(bc => bc.UserID);
            modelBuilder.Entity<User_WorkOrder>()
                .HasOne(bc => bc.WorkOrder)
                .WithMany(c => c.UserWorkOrders)
                .HasForeignKey(bc => bc.WorkOrderID);



            modelBuilder.Entity<TestPoint>(entity =>
            {
                entity.Property(e => e.TestPointID)
                    .HasColumnName("TestPointID");
                entity.Property(e => e.UnitOfMeasurementOutID).HasColumnName("UnitOfMeasureOutID");
                entity.Property(e => e.UnitOfMeasurementID).HasColumnName("UnitOfMeasurementID");
                entity.HasOne(d => d.UnitOfMeasurementOut)
                    .WithMany(p => p.TestPointsOut)
                    .HasForeignKey(d => d.UnitOfMeasurementOutID)
                    .OnDelete(DeleteBehavior.ClientNoAction)
                    .HasConstraintName("FK_TestPointsOut_UnitOfMeasureOut_UnitOfMeasureOutID");
                entity.HasOne(d => d.UnitOfMeasurement)
                    .WithMany(p => p.TestPointsIn)
                    .HasForeignKey(d => d.UnitOfMeasurementID)
                    .OnDelete(DeleteBehavior.ClientNoAction)
                    .HasConstraintName("FK_TestPointsin_UnitOfMeasureIn_UnitOfMeasureInID");
            });


            modelBuilder.Entity<EquipmentTemplate>(entity =>
            {
                entity.Property(e => e.EquipmentTemplateID)
                    .HasColumnName("EquipmentTemplateID");
                entity.Property(e => e.UnitofmeasurementID).HasColumnName("UnitofmeasurementID");
                entity.HasOne(d => d.UnitOfMeasure)
                   .WithMany(p => p.EquipmentTemplates)
                   .HasForeignKey(d => d.UnitofmeasurementID)
                   .OnDelete(DeleteBehavior.ClientNoAction)
                   .HasConstraintName("FK_EquipmentTemplate_UnitOfMeasureIn_UnitOfMeasureInID");

            });

            modelBuilder.Entity<EquipmentTemplate>(entity =>
            {
                entity.Property(e => e.EquipmentTemplateID)
                    .HasColumnName("EquipmentTemplateID");
                entity.Property(e => e.ManufacturerID).HasColumnName("ManufacturerID");
                entity.HasOne(d => d.Manufacturer1)
                   .WithMany(p => p.EquipmentTemplates)
                   .HasForeignKey(d => d.ManufacturerID)
                   .OnDelete(DeleteBehavior.ClientNoAction)
                   .HasConstraintName("FK_EquipmentTemplate_Manufacturer_Manufacturer1ID");
            });

            modelBuilder.Entity<EquipmentTemplate>(entity =>
            {
                entity.Property(e => e.EquipmentTemplateID)
                    .HasColumnName("EquipmentTemplateID");
                entity.Property(e => e.EquipmentTypeID).HasColumnName("EquipmentTypeID");
                entity.HasOne(d => d.EquipmentTypeObject)
                   .WithMany(p => p.EquipmentTemplates)
                   .HasForeignKey(d => d.EquipmentTypeID)
                   .OnDelete(DeleteBehavior.ClientNoAction)
                   .HasConstraintName("FK_EquipmentTemplate_EquipmentType_EquipmentType1ID");
            });


            modelBuilder.Entity<WorkOrderDetail>(entity =>
            {
                entity.Property(e => e.WorkOrderDetailID)
                    .HasColumnName("WorkOrderDetailID");
                entity.Property(e => e.WorkOrderID).HasColumnName("WorkOderID");
                entity.HasOne(d => d.WorkOder)
                   .WithMany(p => p.WorkOrderDetails)
                   .HasForeignKey(d => d.WorkOrderID)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_WorkOrderDetail_WorkOrderDetail_WorkOrderDetailID");
            });

            modelBuilder.Entity<WorkOrderDetail>()
            .HasOne(a => a.PieceOfEquipment)
            .WithMany(b => b.WorOrderDetails)
            .HasForeignKey(d => d.PieceOfEquipmentId)
             .OnDelete(DeleteBehavior.Cascade);




            modelBuilder.Entity<POE_WorkOrder>().HasKey(sc => new { sc.PieceOfEquipmentID, sc.WorkOrderID });

            modelBuilder.Entity<POE_WorkOrder>()
            .HasOne<PieceOfEquipment>(sc => sc.PieceOfEquipment)
            .WithMany(s => s.POE_WorkOrders)
            .HasForeignKey(sc => sc.PieceOfEquipmentID)
            .OnDelete(DeleteBehavior.ClientNoAction);



            modelBuilder.Entity<POE_WorkOrder>()
                .HasOne<WorkOrder>(sc => sc.WorkOrder)
                .WithMany(s => s.POE_WorkOrders)
                .HasForeignKey(sc => sc.WorkOrderID)
                .OnDelete(DeleteBehavior.ClientNoAction);



            modelBuilder.Entity<User>().HasMany(e => e.WorkOrderDetails)
                .WithOne(E => E.Technician)
                .HasForeignKey(t => t.TechnicianID).IsRequired(false)
                .OnDelete(DeleteBehavior.ClientNoAction);



            modelBuilder.Entity<WorkOrderDetail>()
           .HasOne(a => a.CurrentStatus)
           .WithMany(b => b.WorkOrderDetails)
           .HasForeignKey(d => d.CurrentStatusID)
            .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Linearity>()
           .HasOne(a => a.TestPoint)
           .WithMany(b => b.Linearities)
           .HasForeignKey(d => d.TestPointID)
            .OnDelete(DeleteBehavior.ClientNoAction);


            modelBuilder.Entity<Repeatability>()
          .HasOne(a => a.TestPoint)
          .WithMany(b => b.Repeatabilities)
          .HasForeignKey(d => d.TestPointID)
           .OnDelete(DeleteBehavior.ClientNoAction);


            modelBuilder.Entity<Eccentricity>()
            .HasOne(a => a.TestPoint)
            .WithMany(b => b.Eccentricities)
            .HasForeignKey(d => d.TestPointID)
            .OnDelete(DeleteBehavior.ClientNoAction);


            modelBuilder.Entity<Linearity>().HasKey(e => new { e.SequenceID, e.CalibrationSubTypeId, e.WorkOrderDetailId });


            modelBuilder.Entity<BasicCalibrationResult>().HasKey(e => new { e.SequenceID, e.CalibrationSubTypeId, e.WorkOrderDetailId });



            modelBuilder.Entity<WorkOrderDetail>().HasOne(e => e.BalanceAndScaleCalibration).WithOne(E => E.WorkOrderDetail).HasForeignKey<BalanceAndScaleCalibration>(t => t.WorkOrderDetailId).OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<BalanceAndScaleCalibration>().HasOne(e => e.Eccentricity).WithOne(E => E.BalanceAndScaleCalibration).HasForeignKey<Eccentricity>(t => t.WorkOrderDetailId).OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<BalanceAndScaleCalibration>().HasOne(e => e.Repeatability).WithOne(E => E.BalanceAndScaleCalibration).HasForeignKey<Repeatability>(t => t.WorkOrderDetailId).OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<BalanceAndScaleCalibration>().HasMany(e => e.Linearities).WithOne(E => E.BalanceAndScaleCalibration).HasForeignKey(t => t.WorkOrderDetailId).OnDelete(DeleteBehavior.ClientNoAction);


            modelBuilder.Entity<Linearity>().HasOne(e => e.BasicCalibrationResult).WithOne(E => E.Linearity)
                .HasForeignKey<Linearity>(e => new { e.SequenceID, e.CalibrationSubTypeId, e.WorkOrderDetailId }).OnDelete(DeleteBehavior.ClientNoAction);


            modelBuilder.Entity<Eccentricity>().HasKey(e => new { e.CalibrationSubTypeId, e.WorkOrderDetailId });

            modelBuilder.Entity<Eccentricity>().HasMany(e => e.TestPointResult).WithOne(E => E.Eccentricity)
                .HasForeignKey(t => new { t.CalibrationSubTypeId, t.WorkOrderDetailId }).OnDelete(DeleteBehavior.ClientNoAction);



            modelBuilder.Entity<Repeatability>().HasKey(e => new { e.CalibrationSubTypeId, e.WorkOrderDetailId });
            modelBuilder.Entity<Repeatability>().HasMany(e => e.TestPointResult).WithOne(E => E.Repeatability)
                .HasForeignKey(t => new { t.CalibrationSubTypeId, t.WorkOrderDetailId }).OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<UnitOfMeasure>().HasMany(e => e.BasicCalibrationResult).WithOne(E => E.UnitOfMeasure).HasForeignKey(t => t.UnitOfMeasureID).OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<UnitOfMeasure>().HasOne(e => e.UnitOfMeasureBase).WithOne(E => E.ParentUnitOfMeasureBase).HasForeignKey<UnitOfMeasure>(t => t.UnitOfMeasureBaseID).OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<UnitOfMeasure>().HasMany(e => e.WeightSets).WithOne(E => E.UnitOfMeasure).HasForeignKey(t => t.UnitOfMeasureID).OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<UnitOfMeasure>().HasMany(e => e.UncertaintyWeightSets).WithOne(E => E.UncertaintyUnitOfMeasure).HasForeignKey(t => t.UncertaintyUnitOfMeasureId).OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<UnitOfMeasure>().HasMany(e => e.Linearities).WithOne(E => E.UnitOfMeasure).HasForeignKey(t => t.UnitOfMeasureId).OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<UnitOfMeasure>().HasMany(e => e.UncertaintyLinearities).WithOne(E => E.CalibrationUncertaintyValueUncertaintyUnitOfMeasure).HasForeignKey(t => t.CalibrationUncertaintyValueUnitOfMeasureId).OnDelete(DeleteBehavior.ClientNoAction);


            modelBuilder.Entity<WOD_Weight>()
               .HasKey(bc => new { bc.WorkOrderDetailID, bc.WeightSetID });
            modelBuilder.Entity<WOD_Weight>()
                .HasOne(bc => bc.WeightSet)
                .WithMany(b => b.WOD_Weights)
                .HasForeignKey(bc => bc.WeightSetID);
            modelBuilder.Entity<WOD_Weight>()
                .HasOne(bc => bc.WorkOrderDetail)
                .WithMany(c => c.WOD_Weights)
                .HasForeignKey(bc => bc.WorkOrderDetailID);


            modelBuilder.Entity<WO_Weight>()
               .HasKey(bc => new { bc.WorkOrderID, bc.WeightSetID });
            modelBuilder.Entity<WO_Weight>()
                .HasOne(bc => bc.WeightSet)
                .WithMany(b => b.WO_Weights)
                .HasForeignKey(bc => bc.WeightSetID);
            modelBuilder.Entity<WO_Weight>()
                .HasOne(bc => bc.WorkOrder)
                .WithMany(c => c.WO_Weights)
                .HasForeignKey(bc => bc.WorkOrderID);

            modelBuilder.Entity<WOD_Standard>()
             .HasKey(bc => new { bc.WorkOrderDetailID, bc.PieceOfEquipmentID });
            modelBuilder.Entity<WOD_Standard>()
                .HasOne(bc => bc.Standard)
                .WithMany(b => b.WOD_Standard)
                .HasForeignKey(bc => bc.PieceOfEquipmentID);
            modelBuilder.Entity<WOD_Standard>()
                .HasOne(bc => bc.WorkOrderDetail)
                .WithMany(c => c.WOD_Standard)
                .HasForeignKey(bc => bc.WorkOrderDetailID);

            modelBuilder.Entity<WO_Standard>()
              .HasKey(bc => new { bc.WorkOrderID, bc.PieceOfEquipmentID });
            modelBuilder.Entity<WO_Standard>()
                .HasOne(bc => bc.Standard)
                .WithMany(b => b.WO_Standard)
                .HasForeignKey(bc => bc.PieceOfEquipmentID);
            modelBuilder.Entity<WO_Standard>()
                .HasOne(bc => bc.WorkOrder)
                .WithMany(c => c.WO_Standard)
                .HasForeignKey(bc => bc.WorkOrderID);



            modelBuilder.Entity<WOD_Procedure>()
          .HasKey(bc => new { bc.WorkOrderDetailID, bc.ProcedureID });
            modelBuilder.Entity<WOD_Procedure>()
                .HasOne(bc => bc.Procedure)
                .WithMany(b => b.WOD_Procedure)
                .HasForeignKey(bc => bc.ProcedureID);
            modelBuilder.Entity<WOD_Procedure>()
                .HasOne(bc => bc.WorkOrderDetail)
                .WithMany(c => c.WOD_Procedure)
                .HasForeignKey(bc => bc.WorkOrderDetailID);

            // ProcedureEquipment configuration - Fixed version
            modelBuilder.Entity<ProcedureEquipment>(entity =>
            {
                // Table and primary key
                entity.ToTable("ProcedureEquipment");
                entity.HasKey(pe => pe.Id);
                entity.Property(pe => pe.Id).ValueGeneratedOnAdd();

                // Properties configuration
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

                // Relationships
                entity.HasOne(pe => pe.Procedure)
                    .WithMany()
                    .HasForeignKey(pe => pe.ProcedureID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ProcedureEquipment_Procedure_ProcedureID");

                entity.HasOne(pe => pe.PieceOfEquipment)
                    .WithMany()
                    .HasForeignKey(pe => pe.PieceOfEquipmentID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ProcedureEquipment_PieceOfEquipment_PieceOfEquipmentID");

                // Indexes
                entity.HasIndex(pe => new { pe.ProcedureID, pe.PieceOfEquipmentID })
                    .IsUnique()
                    .HasDatabaseName("IX_ProcedureEquipment_Unique");

                entity.HasIndex(pe => pe.ProcedureID)
                    .HasDatabaseName("IX_ProcedureEquipment_ProcedureID");

                entity.HasIndex(pe => pe.PieceOfEquipmentID)
                    .HasDatabaseName("IX_ProcedureEquipment_PieceOfEquipmentID");

                // Ignore IGeneric properties
                entity.Ignore(pe => pe.IsNew);
                entity.Ignore(pe => pe.IsDeleted);
                entity.Ignore(pe => pe.IsModified);
                entity.Ignore(pe => pe.IsSelected);
                entity.Ignore(pe => pe.SearchFilter);
            });

            modelBuilder.Entity<WOD_ParametersTable>()
                .HasKey(bc => new { bc.WorkOrderDetailID });

            modelBuilder.Entity<CalibrationSubType>()
             .HasKey(bc => bc.CalibrationSubTypeId);

            modelBuilder.Entity<CalibrationSubType>()
            .Property(et => et.CalibrationSubTypeId)
            .ValueGeneratedNever();


            modelBuilder.Entity<WeightSet>()
            .HasKey(bc => bc.WeightSetID);
            modelBuilder.Entity<WeightSet>()
           .Property(et => et.WeightSetID)
           .ValueGeneratedNever();


            modelBuilder.Entity<CalibrationSubType_Weight>()
              .HasKey(bc => new { bc.ComponentID, bc.WeightSetID, bc.CalibrationSubTypeID, bc.SecuenceID });
            modelBuilder.Entity<CalibrationSubType_Weight>()
                .HasOne(bc => bc.WeightSet)
                .WithMany(b => b.CalibrationSubType_Weights)
                .HasForeignKey(bc => bc.WeightSetID);
            modelBuilder.Entity<CalibrationSubType_Weight>()
                .HasOne(bc => bc.CalibrationSubType)
                .WithMany(c => c.CalibrationSubType_Weights)
                .HasForeignKey(bc => bc.CalibrationSubTypeID);



            modelBuilder.Entity<WOD_TestPoint>()
            .HasKey(bc => new { bc.WorkOrderDetailID, bc.TestPointID, bc.CalibrationSubTypeID, bc.SecuenceID });
            modelBuilder.Entity<WOD_TestPoint>()
                .HasOne(bc => bc.TestPoint)
                .WithMany(b => b.WOD_TestPoints)
                .HasForeignKey(bc => bc.TestPointID);
            modelBuilder.Entity<WOD_TestPoint>()
                .HasOne(bc => bc.WorkOrderDetail)
                .WithMany(c => c.WOD_TestPoints)
                .HasForeignKey(bc => bc.WorkOrderDetailID);

            modelBuilder.Entity<WorkDetailHistory>()
            .HasOne(a => a.WorkOrderDetail)
            .WithMany(b => b.WorkDetailHistorys)
            .HasForeignKey(d => d.WorkOrderDetailID)
            .OnDelete(DeleteBehavior.ClientNoAction);


            modelBuilder.Entity<WorkOrderDetail>()
          .HasOne(a => a.Address)
          .WithMany(b => b.WorkOrderDetails)
          .HasForeignKey(d => d.AddressID)
          .OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<Note>()
         .HasOne(a => a.EquipmentType)
         .WithMany(b => b.Notes)
         .HasForeignKey(d => d.EquipmnetTypeId)
         .OnDelete(DeleteBehavior.ClientNoAction);


            modelBuilder.Entity<TechnicianCode>().HasKey(e => new { e.Code, e.StateID, e.CertificationID });





            modelBuilder.Entity<User>()
          .HasMany(a => a.TechnicianCodes)
          .WithOne(b => b.User)
          .HasForeignKey(d => d.UserID)
          .OnDelete(DeleteBehavior.ClientNoAction);



            modelBuilder.Entity<TechnicianCode>()
           .HasOne(a => a.Certification)
           .WithMany(b => b.TechnicianCodes)
           .HasForeignKey(d => d.CertificationID)
           .OnDelete(DeleteBehavior.ClientNoAction);


            modelBuilder.Entity<WorkOrderDetail>()
           .HasOne(a => a.CalibrationType)
           .WithMany(b => b.WorkOrderDetails)
           .HasForeignKey(d => d.CalibrationTypeID)
           .OnDelete(DeleteBehavior.ClientNoAction);


            modelBuilder.Entity<PieceOfEquipment>()
           .HasOne(a => a.UnitOfMeasure)
           .WithMany(b => b.PieceOfEquipments)
           .HasForeignKey(d => d.UnitOfMeasureID)
           .OnDelete(DeleteBehavior.ClientNoAction);


            modelBuilder.Entity<WorkOrderDetail>()
           .HasMany(a => a.EnviromentCondition)
           .WithOne(b => b.WorkOrderDetail)
           .HasForeignKey(d => d.WorkOrderDetailId)
           .OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder
           .Entity<PhoneNumber>()
           .Property(x => x.Name)
           .HasMaxLength(50)
           .IsUnicode(false);

            modelBuilder
          .Entity<PhoneNumber>()
          .Property(x => x.Number)
          .HasMaxLength(50)
         .IsUnicode(false);

            modelBuilder
       .Entity<PhoneNumber>()
       .Property(x => x.TypeID)
       .HasMaxLength(50)
      .IsUnicode(false);

            modelBuilder
            .Entity<PhoneNumber>()
            .Property(x => x.CountryID)
            .HasMaxLength(50)
            .IsUnicode(false);


            modelBuilder
          .Entity<Contact>()
          .Property(x => x.Name)
          .HasMaxLength(50)
          .IsUnicode(false);

            modelBuilder
     .Entity<Contact>()
     .Property(x => x.LastName)
     .HasMaxLength(50)
     .IsUnicode(false);

            modelBuilder
   .Entity<WorkOrderDetail>()
   .Property(x => x.TechnicianComment)
   .IsUnicode(false);

            modelBuilder
.Entity<WorkOrderDetail>()
.Property(x => x.CertificateComment)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
           .Entity<PieceOfEquipment>()
           .Property(x => x.PieceOfEquipmentID)
           .HasMaxLength(500)
           .IsUnicode(false);

            modelBuilder
.Entity<PieceOfEquipment>()
.Property(x => x.IndicatorPieceOfEquipmentID)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
.Entity<PieceOfEquipment>()
.Property(x => x.SerialNumber)
.HasMaxLength(500)
.IsUnicode(false);


            modelBuilder
.Entity<PieceOfEquipment>()
.Property(x => x.InstallLocation)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
.Entity<PieceOfEquipment>()
.Property(x => x.Class)
.HasMaxLength(5)
.IsUnicode(false);

            modelBuilder
.Entity<CertificatePoE>()
.Property(x => x.PieceOfEquipmentID)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
.Entity<CertificatePoE>()
.Property(x => x.Description)
.HasMaxLength(500)
.IsUnicode(false);
            modelBuilder
.Entity<CertificatePoE>()
.Property(x => x.Name)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder
.Entity<Customer>()
.Property(x => x.Name)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder
.Entity<Customer>().Property(x => x.Description)
.HasMaxLength(500)
.IsUnicode(false);


            modelBuilder
.Entity<UnitOfMeasure>().Property(x => x.Abbreviation)
.HasMaxLength(10)
.IsUnicode(false);

            modelBuilder
.Entity<UnitOfMeasure>().Property(x => x.Name)
.HasMaxLength(50)
.IsUnicode(false);

            modelBuilder
.Entity<UnitOfMeasure>().Property(x => x.Description)
.HasMaxLength(200)
.IsUnicode(false);

            modelBuilder
.Entity<PieceOfEquipment>().Property(x => x.OfflineID)
.HasMaxLength(100)
.IsUnicode(false);



            modelBuilder
           .Entity<WorkOrderDetail>().Property(x => x.OfflineID)
           .HasMaxLength(100)
           .IsUnicode(false);

            modelBuilder
.Entity<User_Rol>().Property(x => x.Permissions)
.HasMaxLength(200)
.IsUnicode(false);

            modelBuilder
.Entity<Rol>().Property(x => x.DefaultPermissions)
.HasMaxLength(200)
.IsUnicode(false);

            modelBuilder
.Entity<Rol>().Property(x => x.Description)
.HasMaxLength(200)
.IsUnicode(false);

            modelBuilder
           .Entity<Component>().Property(x => x.Route)
           .HasMaxLength(100)
           .IsUnicode(false);


            modelBuilder
.Entity<Component>().Property(x => x.Permission)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder
.Entity<Component>().Property(x => x.Name)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder
.Entity<Component>().Property(x => x.Group)
.HasMaxLength(200)
.IsUnicode(false);

            modelBuilder
           .Entity<PieceOfEquipment>().Property(x => x.CustomerToolId)
           .HasMaxLength(200)
           .IsUnicode(false);

            modelBuilder
.Entity<EquipmentTemplate>().Property(x => x.DeviceClass)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder.Entity<PieceOfEquipment>()
          .HasOne(a => a.UnitOfMeasure)
          .WithMany(b => b.PieceOfEquipments)
          .HasForeignKey(d => d.UnitOfMeasureID)
          .OnDelete(DeleteBehavior.ClientNoAction);


            modelBuilder.Entity<POE_User>()
        .HasKey(bc => new { bc.PieceOfEquipmentID, bc.UserID });
            modelBuilder.Entity<POE_User>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.POE_Users)
                .HasForeignKey(bc => bc.UserID);
            modelBuilder.Entity<POE_User>()
                .HasOne(bc => bc.POE)
                .WithMany(c => c.POE_Users)
                .HasForeignKey(bc => bc.PieceOfEquipmentID);



            modelBuilder.Entity<POE_POE>()
      .HasKey(bc => new { bc.PieceOfEquipmentID, bc.PieceOfEquipmentID2 });
            modelBuilder.Entity<POE_POE>()
                .HasOne(bc => bc.POE)
                .WithMany(b => b.POE_POE)
                .HasForeignKey(bc => bc.PieceOfEquipmentID);
            modelBuilder.Entity<POE_POE>()
                .HasOne(bc => bc.POE)
                .WithMany(c => c.POE_POE)
                .HasForeignKey(bc => bc.PieceOfEquipmentID2);

            modelBuilder.Entity<TechnicianCode>().Property(u => u.TechnicianCodeID).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);


            modelBuilder
    .Entity<WorkOrderDetailByStatus>()
    .ToView(nameof(WorkOrderDetailByStatus))
    .HasKey(t => t.WorkOrderDetailID);

            modelBuilder
.Entity<WorkOrderDetailByCustomer>()
.ToView(nameof(WorkOrderDetailByCustomer))
.HasKey(t => t.WorkOrderDetailID);

            modelBuilder.Entity<Force>().HasKey(e => new { e.SequenceID, e.CalibrationSubTypeId, e.WorkOrderDetailId });

            modelBuilder.Entity<ForceResult>().HasKey(e => new { e.SequenceID, e.CalibrationSubTypeId, e.WorkOrderDetailId });

            modelBuilder.Entity<Force>().HasOne(e => e.BasicCalibrationResult).WithOne(E => E.Force)
               .HasForeignKey<Force>(e => new { e.SequenceID, e.CalibrationSubTypeId, e.WorkOrderDetailId }).OnDelete(DeleteBehavior.ClientNoAction);


            modelBuilder.Entity<Rockwell>().HasKey(e => new { e.SequenceID, e.CalibrationSubTypeId, e.WorkOrderDetailId });

            modelBuilder.Entity<RockwellResult>().HasKey(e => new { e.SequenceID, e.CalibrationSubTypeId, e.WorkOrderDetailId });

            modelBuilder.Entity<Rockwell>().HasOne(e => e.BasicCalibrationResult).WithOne(E => E.Rockwell)
               .HasForeignKey<RockwellResult>(e => new { e.SequenceID, e.CalibrationSubTypeId, e.WorkOrderDetailId }).OnDelete(DeleteBehavior.ClientNoAction);


            modelBuilder.Entity<Micro>().HasKey(e => new { e.SequenceID, e.CalibrationSubTypeId, e.WorkOrderDetailId });

            modelBuilder.Entity<MicroResult>().HasKey(e => new { e.SequenceID, e.CalibrationSubTypeId, e.WorkOrderDetailId });

            modelBuilder.Entity<Micro>().HasOne(e => e.BasicCalibrationResult).WithOne(E => E.Micro)
               .HasForeignKey<MicroResult>(e => new { e.SequenceID, e.CalibrationSubTypeId, e.WorkOrderDetailId }).OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<GenericCalibration>().HasKey(e => new { e.SequenceID, e.CalibrationSubTypeId, e.WorkOrderDetailId });

            modelBuilder.Entity<GenericCalibrationResult>().HasKey(e => new { e.SequenceID, e.CalibrationSubTypeId, e.WorkOrderDetailId });

            modelBuilder.Entity<GenericCalibration>().HasOne(e => e.BasicCalibrationResult).WithOne(E => E.GenericCalibration)
               .HasForeignKey<GenericCalibrationResult>(e => new { e.SequenceID, e.CalibrationSubTypeId, e.WorkOrderDetailId }).OnDelete(DeleteBehavior.ClientNoAction);


            modelBuilder.Entity<GenericCalibration2>().HasKey(e => new { e.SequenceID, e.CalibrationSubTypeId, e.ComponentID });

            modelBuilder.Entity<GenericCalibrationResult2>().HasKey(e => new { e.SequenceID, e.CalibrationSubTypeId, e.ComponentID });

            modelBuilder.Entity<GenericCalibration2>().HasMany(e => e.TestPointResult).WithOne(E => E.GenericCalibration2).HasForeignKey(e => new { e.SequenceID, e.CalibrationSubTypeId, e.ComponentID }).OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<GenericCalibrationResult2Aggregate>().HasKey(e => new { e.SequenceID, e.CalibrationSubTypeId, e.ComponentID });

            modelBuilder.Entity<POE_Scale>()
       .HasKey(bc => new { bc.PieceOfEquipmentID, bc.Scale });
            modelBuilder.Entity<POE_Scale>()
                .HasOne(bc => bc.POE)
                .WithMany(b => b.POE_Scale)
                .HasForeignKey(bc => bc.PieceOfEquipmentID);



            modelBuilder
.Entity<POE_Scale>().Property(x => x.Scale)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder.Entity<POE_Scale>().Ignore(e => e.POE);

            modelBuilder
.Entity<ForceResult>().Property(x => x.ClassRun1)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder
.Entity<GenericCalibrationResult>().Property(x => x.Object)
.HasMaxLength(10000)
.IsUnicode(false);



            modelBuilder
.Entity<GenericCalibrationResult2>().Property(x => x.Object)
.HasMaxLength(10000)
.IsUnicode(false);



            modelBuilder
.Entity<GenericCalibrationResult2>().Property(x => x.ExtendedObject)
.HasMaxLength(10000)
.IsUnicode(false);

           
            modelBuilder
.Entity<GenericCalibrationResult2>().Property(x => x.UncertaintyJSON)
.HasMaxLength(5000)
.IsUnicode(false);


            modelBuilder.Entity<CalibrationSubType_Standard>()
             .HasKey(bc => new { bc.ComponentID, bc.PieceOfEquipmentID, bc.CalibrationSubTypeID, bc.SecuenceID });
            modelBuilder.Entity<CalibrationSubType_Standard>()
                .HasOne(bc => bc.Standard)
                .WithMany(b => b.CalibrationSubType_Standard)
                .HasForeignKey(bc => bc.PieceOfEquipmentID);
            modelBuilder.Entity<CalibrationSubType_Standard>()
                .HasOne(bc => bc.CalibrationSubType)
                .WithMany(c => c.CalibrationSubType_Standard)
                .HasForeignKey(bc => bc.CalibrationSubTypeID);







            modelBuilder
.Entity<CalibrationSubType_Standard>()
.Property(x => x.PieceOfEquipmentID)
.HasMaxLength(500)
.IsUnicode(false);


            modelBuilder
.Entity<DynamicProperty>()
.Property(x => x.DefaultValue)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
.Entity<DynamicProperty>()
.Property(x => x.DataType)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
.Entity<DynamicProperty>()
.Property(x => x.DefaultValue)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
.Entity<DynamicProperty>()
.Property(x => x.Name)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
.Entity<ViewPropertyBase>()
.Property(x => x.Comment)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
.Entity<ViewPropertyBase>()
.Property(x => x.ToastMessage)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
.Entity<ViewPropertyBase>()
.Property(x => x.ControlType)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder
.Entity<ViewPropertyBase>()
.Property(x => x.CSSClass)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder
.Entity<ViewPropertyBase>()
.Property(x => x.Display)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder
.Entity<ViewPropertyBase>()
.Property(x => x.ErrorMesage)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder
.Entity<ViewPropertyBase>()
.Property(x => x.LabelCSS)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder
.Entity<ViewPropertyBase>()
.Property(x => x.StepResol)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder
.Entity<ViewPropertyBase>()
.Property(x => x.ToolTipMessage)
.HasMaxLength(100)
.IsUnicode(false);


            //            modelBuilder
            //.Entity<ViewPropertyBase>()
            //.Property(x => x.ID)
            //.HasMaxLength(200)
            //.IsUnicode(false);

            modelBuilder
.Entity<ViewPropertyBase>()
.Property(x => x.CSSCol)
.HasMaxLength(200)
.IsUnicode(false);

            modelBuilder
.Entity<DynamicProperty>()
.Property(x => x.Formula)
.HasMaxLength(20000)
.IsUnicode(false);

            modelBuilder
.Entity<DynamicProperty>()
.Property(x => x.ValidationFormula)
.HasMaxLength(20000)
.IsUnicode(false);

            modelBuilder
.Entity<CalibrationSubType>()
.Property(x => x.CreateClass)
.HasMaxLength(500)
.IsUnicode(false);


            modelBuilder
.Entity<CalibrationSubType>()
.Property(x => x.GetClass)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
.Entity<CalibrationSubTypeView>()
.Property(x => x.CSSGrid)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
.Entity<CalibrationSubTypeView>()
.Property(x => x.CSSRow)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
.Entity<CalibrationSubTypeView>()
.Property(x => x.CSSRowSeparator)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
.Entity<CalibrationSubTypeView>()
.Property(x => x.ColActionCSS)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
.Entity<CalibrationSubTypeView>()
.Property(x => x.ColButtonActionCSS)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
.Entity<CalibrationSubTypeView>()
.Property(x => x.AlingActionCSS)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
.Entity<CalibrationSubTypeView>()
.Property(x => x.Key)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
.Entity<CalibrationSubTypeView>()
.Property(x => x.NoDataMessage)
.HasMaxLength(500)
.IsUnicode(false);


            modelBuilder
.Entity<ToleranceType>()
.Property(x => x.Key)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder
.Entity<ToleranceType>()
.Property(x => x.Value)
.HasMaxLength(100)
.IsUnicode(false);


            modelBuilder
.Entity<EquipmentTypeGroup>()
.Property(x => x.Name)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
.Entity<Procedure>()
.Property(x => x.DocumentUrl)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
.Entity<TestCode>()
.Property(x => x.Code)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder
.Entity<TestCode>()
.Property(x => x.Description)
.HasMaxLength(100)
.IsUnicode(false);




            modelBuilder.Entity<ViewPropertyBase>()
           .Property(b => b.ShowControl)
           .HasDefaultValue(false);

            modelBuilder.Entity<ViewPropertyBase>()
         .Property(b => b.ShowLabel)
         .HasDefaultValue(false);

            modelBuilder.Entity<Rol>()
        .HasIndex(x => x.Name).IsUnique();




            //            modelBuilder
            //.Entity<DynamicProperty>()
            //.Property(x => x.FormulaResult)
            //.HasMaxLength(5000)
            //.IsUnicode(false);

            modelBuilder
.Entity<GenericCalibrationResult>()
.Property(x => x.ExtendedObject)
.HasMaxLength(5000)
.IsUnicode(false);


            modelBuilder
.Entity<WeightSet>()
.Property(x => x.Note)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
.Entity<WeightSet>()
.Property(x => x.Name)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder
.Entity<WeightSet>()
.Property(x => x.PieceOfEquipmentID)
.HasMaxLength(450)
.IsUnicode(false);


            modelBuilder
.Entity<WeightSet>()
.Property(x => x.PieceOfEquipmentStatus)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder
.Entity<WeightSet>()
.Property(x => x.Type)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder
.Entity<WeightSet>()
.Property(x => x.Distribution)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder
.Entity<WeightSet>()
.Property(x => x.Reference)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder
.Entity<WeightSet>()
.Property(x => x.Serial)
.HasMaxLength(50)
.IsUnicode(false);


            modelBuilder
.Entity<WorkOrderDetail>()
.Property(x => x.TechnicianComment)
.IsUnicode(false);

            modelBuilder
.Entity<WorkOrderDetail>()
.Property(x => x.CertificateComment)
.HasMaxLength(500)
.IsUnicode(false);

            modelBuilder
         .Entity<WorkOrderDetail>().Property(x => x.PieceOfEquipmentId)
         .HasMaxLength(450)
         .IsUnicode(false);


            modelBuilder
      .Entity<WorkOrderDetail>().Property(x => x.CertificateComment)
      .HasMaxLength(450)
      .IsUnicode(false);

            modelBuilder
.Entity<WorkOrderDetail>().Property(x => x.Description)
.HasMaxLength(2000)
.IsUnicode(false);

            modelBuilder
.Entity<WorkOrderDetail>().Property(x => x.Environment)
.HasMaxLength(2000)
.IsUnicode(false);

            modelBuilder
.Entity<WorkOrderDetail>().Property(x => x.Name)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder
.Entity<WorkOrderDetail>().Property(x => x.WorkOrderDetailHash)
.HasMaxLength(2000)
.IsUnicode(false);


            modelBuilder.Entity<ViewPropertyBase>().Ignore(x => x.DynamicProperty);

            modelBuilder.Entity<Tolerance>().HasNoKey();

            // Configure AuditLog entity
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.AuditLogId);
                entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(e => e.Timestamp).HasDatabaseName("IX_AuditLogs_Timestamp");
                entity.HasIndex(e => e.EntityType).HasDatabaseName("IX_AuditLogs_EntityType");
                entity.HasIndex(e => e.EntityId).HasDatabaseName("IX_AuditLogs_EntityId");
                entity.HasIndex(e => e.UserName).HasDatabaseName("IX_AuditLogs_UserName");
                entity.HasIndex(e => e.ActionType).HasDatabaseName("IX_AuditLogs_ActionType");
            });

            // Configure Quotes Module entities
            ConfigureQuotesEntities(modelBuilder);

            base.OnModelCreating(modelBuilder);

            //Code added by LTI Request:

            modelBuilder.Entity<PieceOfEquipment>().ToTable(tb => tb.HasTrigger("tr_PoEToCSI "));
            modelBuilder.Entity<WorkOrderDetail>().ToTable(tb => tb.HasTrigger("tr_WODetailToCSI"));




            // Replace the problematic line with the following corrected code:

            if (TenantName == "LTI")
            {
                //modelBuilder.Entity<WorkOrder>().Property(o => o.WorkOrderId).ValueGeneratedOnAdd().HasValueGenerator<UserGenerator>();

                //modelBuilder.Entity<WorkOrder>()
                //.Property(e => e.WorkOrderId).ValueGeneratedOnAdd()
                //.HasValueGenerator((_, __) => new UserGenerator(_configuration));
            }
          



            OnModelCreatingPartial(modelBuilder);





        }
        public static int GenerateOrderID()
        {
            return 1;
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        private void ConfigureQuotesEntities(ModelBuilder modelBuilder)
        {
            // Configure Quote entity
            modelBuilder.Entity<Quote>(entity =>
            {
                entity.HasKey(e => e.QuoteID);
                entity.Property(e => e.QuoteID).ValueGeneratedOnAdd(); // Explicitly configure as identity column
                entity.Property(e => e.QuoteNumber).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.QuoteNumber).IsUnique();
            });

            // Configure QuoteItem entity
            modelBuilder.Entity<QuoteItem>(entity =>
            {
                entity.HasKey(e => e.QuoteItemID);
                entity.Property(e => e.QuoteItemID).ValueGeneratedOnAdd(); // Explicitly configure as identity column
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ItemDescription).HasMaxLength(500);

                entity.HasOne(e => e.Quote)
                    .WithMany(q => q.QuoteItems)
                    .HasForeignKey(e => e.QuoteID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.PieceOfEquipment)
                    .WithMany()
                    .HasForeignKey(e => e.PieceOfEquipmentID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ParentQuoteItem)
                    .WithMany(qi => qi.ChildItems)
                    .HasForeignKey(e => e.ParentQuoteItemID)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.PriceType)
                    .WithMany(pt => pt.QuoteItems)
                    .HasForeignKey(e => e.PriceTypeId)
                    .HasPrincipalKey(pt => pt.PriceTypeId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(e => e.QuoteID);
                entity.HasIndex(e => e.PieceOfEquipmentID);
                entity.HasIndex(e => e.PriceTypeId);
            });

            // Configure PriceType entity
            modelBuilder.Entity<PriceType>(entity =>
            {
                entity.HasKey(e => e.PriceTypeId);
                entity.Property(e => e.PriceTypeId).ValueGeneratedOnAdd();
            });

            // Configure PriceTypePrice entity
            modelBuilder.Entity<PriceTypePrice>(entity =>
            {
                entity.HasOne(e => e.PriceType)
                    .WithMany(pt => pt.PriceTypePrices)
                    .HasForeignKey(e => e.PriceTypeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.PriceTypeId);
                entity.HasIndex(e => new { e.PriceTypeId, e.EntityType, e.EntityId }).IsUnique();
            });

            // Configure CustomerAddress TravelExpense and WorkOrder relationship
            modelBuilder.Entity<CustomerAddress>(entity =>
            {
                entity.Property(e => e.TravelExpense).HasColumnType("decimal(18,2)");

                // Configure the relationship with WorkOrder using CustomerId
                entity.HasMany(ca => ca.WorkOrder)
                    .WithOne()
                    .HasForeignKey(wo => wo.CustomerId)
                    .HasPrincipalKey(ca => ca.CustomerId)
                    .OnDelete(DeleteBehavior.ClientNoAction);
            });
        }




    }


    public class UserGenerator : ValueGenerator<int>
    {
        private int _current;

        private IConfiguration _configuration;

        string RangeMinS = "1";

        int RangeMin = 1;
        public UserGenerator(IConfiguration configuration)
        {
            _configuration = configuration;

            //RangeMinS = _configuration["Reports:RangeMin"];


            //RangeMin = Convert.ToInt32(RangeMinS);

        }

        public override bool GeneratesTemporaryValues => false;

        public override int Next(EntityEntry entry)
        {
            var res=Interlocked.Increment(ref _current);
            return res;
        }
          


        //var userProvider = entry.Context.GetService<ITeamTunerUserProvider>();
    }
    public interface IUserProvider
    {
        string UserId { get; }
    }

    //public class DummyUserProvider : IUserProvider
    //{
    //    public string UserId { get; private set; } = 1;
    //}

    public class TestDbContext : DbContext
    {
        private readonly IServiceProvider serviceProvider;
        public TestDbContext(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInternalServiceProvider(serviceProvider);
        }
    }


    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer("server=.;database=myDb;trusted_connection=true;");
    //}



}

/// <summary>
/// Static storage for audit original values
/// </summary>
public static class AuditOriginalValuesStorage
{
    private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, object> _originalValues =
        new System.Collections.Concurrent.ConcurrentDictionary<string, object>();

    /// <summary>
    /// Store original values from DbContext (called by DbContext.SaveChanges)
    /// </summary>
    public static void StoreOriginalValues(string key, System.Collections.Generic.Dictionary<string, object> originalValues)
    {
        _originalValues[key] = originalValues;
//        Console.WriteLine($"STORAGE: Stored values for key {key}");
    }

    /// <summary>
    /// Get stored original values for a specific entity
    /// </summary>
    public static System.Collections.Generic.Dictionary<string, object> GetOriginalValues(string entityTypeName, object primaryKey)
    {
        var key = $"OriginalValues_{entityTypeName}_{primaryKey}";

        if (_originalValues.TryGetValue(key, out var values) && values is System.Collections.Generic.Dictionary<string, object> originalValues)
        {
//            Console.WriteLine($"STORAGE: Retrieved values for key {key}");
            return originalValues;
        }

//        Console.WriteLine($"STORAGE: No values found for key {key}");
        return null;
    }
}
