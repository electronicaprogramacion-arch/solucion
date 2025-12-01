
using Bogus;
using CalibrationSaaS.Data.EntityFramework;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.Views;

using Helpers.Controls;
using SQLite;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;


using SqliteWasmHelper;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

using CustomClaim = Helpers.Controls.CustomClaim;
using Customer = CalibrationSaaS.Domain.Aggregates.Entities.Customer;
using Repeatability = CalibrationSaaS.Domain.Aggregates.Entities.Repeatability;
using WorkOrder = CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder;
using WorkOrderDetailByCustomer = CalibrationSaaS.Domain.Aggregates.Views.WorkOrderDetailByCustomer;

namespace CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite
{
    public interface ICalibrationSaaSDBContextBaseOff

    {


        DbSet<CurrentUser> CurrentUser { get; set; }

        DbSet<CustomClaim> CustomClaim { get; set; }

        DbSet<UnitOfMeasure2> UnitOfMeasure2 { get; set; }

    }
    //public interface ICalibrationSaaSDBContextBaseOff

    //{


    //    DbSet<CurrentUser> CurrentUser { get; set; }

    //    DbSet<CustomClaim> CustomClaim { get; set; }

    //    DbSet<UnitOfMeasure2> UnitOfMeasure2 { get; set; }

    //}

    public class CalibrationSaaSDBContextOff2 : DbContext, ICalibrationSaaSDBContextBase, ICalibrationSaaSDBContextBaseOff
    {


        //public virtual DbSet<DynamicReport> DynamicReport { get; set; }

        //public virtual DbSet<Report_SubType> Report_SubType { get; set; }


        //public virtual DbSet<SubType_DynamicProperty> SubType_DynamicProperty { get; set; }


        public DbSet<WOStatus> WOStatus { get; set; }


        public DbSet<CustomSequence> CustomSequence { get; set; }

        public DbSet<CurrentUser> CurrentUser { get; set; }

        public DbSet<CustomClaim> CustomClaim { get; set; }


        public virtual DbSet<BalanceAndScaleCalibration> BalanceAndScaleCalibration { get; set; }
        public virtual DbSet<Domain.Aggregates.Entities.Customer> Customer { get; set; }
        //public virtual DbSet<CustomerAddress> CustomerAddress { get; set; }

        public virtual DbSet<Eccentricity> Eccentricity { get; set; }
        public virtual DbSet<EquipmentCondition> EquipmentCondition { get; set; }
        public virtual DbSet<Linearity> Linearity { get; set; }
        public virtual DbSet<PieceOfEquipment> PieceOfEquipment { get; set; }
        public virtual DbSet<Repeatability> Repeatability { get; set; }
        public virtual DbSet<Tenant> Tenant { get; set; }
        public virtual DbSet<WorkOrder> WorkOrder { get; set; }
        public virtual DbSet<WorkOrderDetail> WorkOrderDetail { get; set; }
        public virtual DbSet<WorkDetailHistory> WorkDetailHistory { get; set; }


        public virtual DbSet<UserInformation> UserInformation { get; set; }
        public virtual DbSet<User> User { get; set; }

        public virtual DbSet<EquipmentType> EquipmentType { get; set; }

        public virtual DbSet<EquipmentTemplate> EquipmentTemplate { get; set; }

        public virtual DbSet<Manufacturer> Manufacturer { get; set; }

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

        public virtual DbSet<Helpers.Controls.Component> Component { get; set; }

        public virtual DbSet<UnitOfMeasure2> UnitOfMeasure2 { get; set; }
        public DbSet<Procedure> Procedure { get ; set ; }
        public DbSet<ProcedureEquipment> ProcedureEquipment { get; set; }

        [Ignore]
        public DbSet<Micro> Micro { get ; set ; }

        [Ignore]
        public DbSet<MicroResult> MicroResult { get ; set ; }


        public DbSet<WOD_Standard> WOD_Standard { get ; set ; }

        [Ignore]
        public DbSet<Force> Force { get; set; }

        [Ignore]
        public DbSet<ForceResult> ForceResult { get; set; }

        public DbSet<Uncertainty> Uncertainty { get ; set ; }
        public DbSet<CalibrationResultContributor> CalibrationResultContributor { get ; set ; }
        public DbSet<TestCode> TestCode { get ; set ; }
        public DbSet<ToleranceType> ToleranceType { get ; set ; }
        //public DbSet<ToleranceType_EquipmentType> ToleranceType_EquipmentType { get ; set ; }

        [NotMapped]
        [Ignore]
        public DbSet<Rockwell> Rockwell { get ; set ; }

        [NotMapped]
        [Ignore]
        public DbSet<RockwellResult> RockwellResult { get ; set ; }
        public DbSet<POE_Scale> POE_Scale { get ; set ; }
        public DbSet<ExternalCondition> ExternalCondition { get ; set ; }
        public DbSet<CalibrationSubType_Standard> CalibrationSubType_Standard { get ; set ; }
        public DbSet<Note> Note { get ; set ; }
        public DbSet<NoteWOD> NoteWOD { get ; set ; }
        public DbSet<DynamicProperty> DynamicProperty { get ; set ; }
        public DbSet<ViewPropertyBase> ViewPropertyBase { get ; set ; }

        [NotMapped]
        [Ignore]
        public DbSet<GenericCalibration> GenericCalibration { get; set; }

        [NotMapped]
        [Ignore]
        public DbSet<GenericCalibrationResult> GenericCalibrationResult { get; set; }




        //public DbSet<CalibrationSubType_DynamicProperty> CalibrationSubType_DynamicProperty { get ; set ; }
        //public DbSet<CalibrationSubType_ViewProperty> CalibrationSubType_ViewProperty { get ; set ; }
        public DbSet<CalibrationSubTypeView> CalibrationSubTypeView { get ; set ; }

        //[NotMapped]
        //[Ignore]
        public DbSet<GenericCalibration2> GenericCalibration2 { get ; set ; }
        //[NotMapped]
        //[Ignore]
        public DbSet<GenericCalibrationResult2> GenericCalibrationResult2 { get ; set ; }
        public DbSet<Mass> Mass { get ; set ; }
        public DbSet<Lenght> Lenght { get ; set ; }

        public DbSet<EquipmentTypeGroup> EquipmentTypeGroup { get; set; }

        public DbSet<WOD_Procedure> WOD_Procedure { get; set; }

        public DbSet<WOD_ParametersTable> WOD_ParametersTable { get; set; }

        public DbSet<CMCValues> CMCValues { get; set; }

        // Audit Logging (offline context - not used but required for interface compatibility)
        public virtual DbSet<AuditLog> AuditLogs { get; set; }

        // Quotes Module entities
        public virtual DbSet<Quote> Quote { get; set; }
        public virtual DbSet<QuoteItem> QuoteItem { get; set; }
        public virtual DbSet<PriceType> PriceType { get; set; }
        public virtual DbSet<PriceTypePrice> PriceTypePrice { get; set; }

        // Equipment Recalls
        public virtual DbSet<CustomerAddress> CustomerAddress { get; set; }

        public CalibrationSaaSDBContextOff2(DbContextOptions<CalibrationSaaSDBContextOff2> options)
        : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<DynamicReport>().Ignore(e => e.CalibrationSubTypes);

            modelBuilder.Ignore<GenericCalibration>();

            modelBuilder.Ignore<GenericCalibrationResult>();

            //modelBuilder.Ignore<GenericCalibration2>();

            //modelBuilder.Ignore<GenericCalibrationResult2>();

            modelBuilder.Ignore<Force>();

            modelBuilder.Ignore<ForceResult>();

            modelBuilder.Ignore<Micro>();

            modelBuilder.Ignore<MicroResult>();

            modelBuilder.Ignore<Rockwell>();

            modelBuilder.Ignore<RockwellResult>();


            modelBuilder.Entity<WorkOrderDetail>().Ignore(e => e.WOD_Procedure);

            modelBuilder.Entity<TestCode>().Ignore(e => e.CalibrationType);

            modelBuilder.Entity<TestCode>().Ignore(e => e.CalibrationTypeID);

            modelBuilder.Entity<TestCode>().Ignore(e => e.Procedure);

            modelBuilder.Entity<TestCode>().Ignore(e => e.Notes);



            modelBuilder.Entity<Procedure>().Ignore(e => e.WOD_Procedure);

            modelBuilder.Entity<WOD_Procedure>().Ignore(e => e.Procedure); 



            modelBuilder.Entity<WOD_Procedure>().Ignore(e => e.WorkOrderDetail);

            //modelBuilder.Entity<WOD_ParametersTable>().Ignore(e => e.WorkOrderDetail);
            modelBuilder.Entity<UnitOfMeasure>().Ignore(e => e.Linearities);

            modelBuilder.Entity<UnitOfMeasure>().Ignore(e => e.BasicCalibrationResult);

            //modelBuilder.Entity<UnitOfMeasure>().Ignore(e => e.Type);

            modelBuilder.Entity<EquipmentTemplate>().Ignore(e => e.EquipmentTypeObject);

            modelBuilder.Entity<BalanceAndScaleCalibration>().Ignore(e=>e.WorkOrderDetail);

            modelBuilder.Entity<Linearity>().Ignore(e => e.BalanceAndScaleCalibration);

            modelBuilder.Entity<Eccentricity>().Ignore(e => e.BalanceAndScaleCalibration);

            modelBuilder.Entity<Repeatability>().Ignore(e => e.BalanceAndScaleCalibration);

            modelBuilder.Entity<BasicCalibrationResult>().Ignore(e => e.Linearity);

            modelBuilder.Entity<BasicCalibrationResult>().Ignore(e => e.Eccentricity);

            modelBuilder.Entity<BasicCalibrationResult>().Ignore(e => e.Repeatability);

            modelBuilder.Entity<WorkOrderDetail>().Ignore(e => e.Tolerance);

            modelBuilder.Entity<EquipmentTemplate>().Ignore(e => e.Tolerance);

            modelBuilder.Entity<PieceOfEquipment>().Ignore(e => e.Tolerance);

            modelBuilder.Entity<EquipmentType>().Ignore(e => e.EquipmentTypeGroup);

           

            modelBuilder.Entity<WeightSet>()
           .HasKey(bc => bc.WeightSetID);
            modelBuilder.Entity<WeightSet>()
           .Property(et => et.WeightSetID)
           .ValueGeneratedNever();

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





            modelBuilder.Entity<WOD_ParametersTable>()
                .HasKey(bc => new { bc.WorkOrderDetailID });


            //modelBuilder.Entity<UnitOfMeasureType>().HasData(
            //new UnitOfMeasureType { Value=1,Name= "Temperature" });

            //modelBuilder.Entity<UnitOfMeasureType>().HasData(
            //new UnitOfMeasureType { Value = 2, Name = "Humidity" });

            //modelBuilder.Entity<UnitOfMeasureType>().HasData(
            //new UnitOfMeasureType { Value = 3, Name = "Weight" });


            //modelBuilder.Entity<UnitOfMeasure>().HasData(
            //new UnitOfMeasure { Name = "Pounds", Abbreviation = "Pd", UnitOfMeasureID = 1, TypeID = 3, IsEnabled = true, UncertaintyUnitOfMeasureID = 1 });

            //modelBuilder.Entity<UnitOfMeasure>().HasData(
            //new UnitOfMeasure { Name = "Kilogramo", Abbreviation = "Kg", UnitOfMeasureID = 2, TypeID = 3, IsEnabled = true, UncertaintyUnitOfMeasureID = 2 });

            //modelBuilder.Entity<UnitOfMeasureType>().Property(p => p.Value).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<UnitOfMeasureType>().HasKey(f => f.Value);
            modelBuilder.Entity<UnitOfMeasureType>().Property(f => f.Value)
           .ValueGeneratedNever();

            modelBuilder.Entity<UnitOfMeasure>().HasKey(f => f.UnitOfMeasureID);
            modelBuilder.Entity<UnitOfMeasure>().Property(f => f.UnitOfMeasureID)
           .ValueGeneratedNever();

            modelBuilder.Entity<CertificatePoE>().HasKey(f => f.CertificateNumber);
            //modelBuilder.Entity<CertificatePoE>().Property(f => f.CertificatePoEId)
            //.ValueGeneratedNever();

            modelBuilder.Entity<UnitOfMeasure2>().HasKey(f => f.UnitOfMeasureID);
            modelBuilder.Entity<UnitOfMeasure2>().Property(f => f.UnitOfMeasureID)
           .ValueGeneratedNever();

            modelBuilder.Entity<UnitOfMeasure>().Property(f => f.ConversionValue).HasConversion<decimal>();


            //modelBuilder.Entity<UnitOfMeasureType>()
            //    .HasMany(e => e.UnitOfMeasure2).WithOne(E => E.Type).HasForeignKey(e => e.TypeID);
               modelBuilder.Entity<CustomerAggregate>().HasKey(f => f.AggregateID);
            modelBuilder.Entity<CustomerAggregate>().Property(f => f.AggregateID)
            .ValueGeneratedNever();


             modelBuilder.Entity<Contact>().HasKey(f => f.ContactID);
            modelBuilder.Entity<Contact>().Property(f => f.ContactID)
            .ValueGeneratedNever();

              modelBuilder.Entity<Address>().HasKey(f => f.AddressId);
            modelBuilder.Entity<Address>().Property(f => f.AddressId)
            .ValueGeneratedNever();

                modelBuilder.Entity<PhoneNumber>().HasKey(f => f.PhoneNumberID);
            modelBuilder.Entity<PhoneNumber>().Property(f => f.PhoneNumberID)
            .ValueGeneratedNever();

                modelBuilder.Entity<Social>().HasKey(f => f.SocialID);
            modelBuilder.Entity<Social>().Property(f => f.SocialID)
            .ValueGeneratedNever();

             modelBuilder.Entity<PieceOfEquipment>().HasKey(f => f.PieceOfEquipmentID);
            modelBuilder.Entity<PieceOfEquipment>().Property(f => f.PieceOfEquipmentID)
            .ValueGeneratedNever();

          


            modelBuilder.Entity<Manufacturer>().HasKey(f => f.ManufacturerID);
            modelBuilder.Entity<Manufacturer>().Property(f => f.ManufacturerID)
           .ValueGeneratedNever();


            modelBuilder.Entity<Status>().HasKey(f => f.StatusId);
            modelBuilder.Entity<Status>().Property(f => f.StatusId)
           .ValueGeneratedNever();

            modelBuilder.Entity<EquipmentTemplate>().HasKey(f => f.EquipmentTemplateID);
            modelBuilder.Entity<EquipmentTemplate>().Property(f => f.EquipmentTemplateID)
            .ValueGeneratedNever();


               modelBuilder.Entity<TestPointGroup>().HasKey(f => f.TestPoitGroupID);
            modelBuilder.Entity<TestPointGroup>().Property(f => f.TestPoitGroupID)
            .ValueGeneratedNever();

              modelBuilder.Entity<TestPoint>().HasKey(f => f.TestPointID);
            modelBuilder.Entity<TestPoint>().Property(f => f.TestPointID)
            .ValueGeneratedNever();

             modelBuilder.Entity<Certificate>().HasKey(f => f.CertificateID);
            modelBuilder.Entity<Certificate>().Property(f => f.CertificateID)
            .ValueGeneratedNever();

          

            modelBuilder.Entity<EquipmentCondition>().HasKey(f => f.EquipmentConditionId);
            modelBuilder.Entity<EquipmentCondition>().Property(f => f.EquipmentConditionId)
            .ValueGeneratedNever();

              modelBuilder.Entity<WorkOrderDetail>().HasKey(f => f.WorkOrderDetailID);
            modelBuilder.Entity<WorkOrderDetail>().Property(f => f.WorkOrderDetailID)
            .ValueGeneratedNever();

            modelBuilder.Entity<ExternalCondition>().HasKey(f => f.ExternalConditionId);
            modelBuilder.Entity<ExternalCondition>().Property(f => f.ExternalConditionId)
            .ValueGeneratedNever();


            //modelBuilder.Entity<EquipmentTemplate>().Property(f => f.AccuracyPercentage).HasConversion<decimal>();

            modelBuilder.Entity<EquipmentTemplate>().Property(f => f.Resolution).HasConversion<decimal>();

            modelBuilder.Entity<EquipmentTemplate>().Property(f => f.Capacity).HasConversion<decimal>();



            modelBuilder.Entity<TestPoint>().Property(f => f.NominalTestPoit).HasConversion<decimal>();

            modelBuilder.Entity<TestPoint>().Property(f => f.LowerTolerance).HasConversion<decimal>();

            modelBuilder.Entity<TestPoint>().Property(f => f.UpperTolerance).HasConversion<decimal>();

            modelBuilder.Entity<TestPoint>().Property(f => f.Resolution).HasConversion<decimal>();



            modelBuilder.Entity<PieceOfEquipment>().Property(f => f.Resolution).HasConversion<decimal>();

            modelBuilder.Entity<PieceOfEquipment>().Property(f => f.Capacity).HasConversion<decimal>();

            //modelBuilder.Entity<PieceOfEquipment>().Property(f => f.AccuracyPercentage).HasConversion<decimal>();

            modelBuilder.Entity<PieceOfEquipment>().Property(f => f.Resolution).HasConversion<decimal>();

            
            
            
            
            modelBuilder.Entity<Linearity>().Property(f => f.TotalNominal).HasConversion<decimal>();

            modelBuilder.Entity<Linearity>().Property(f => f.TotalActual).HasConversion<decimal>();

            modelBuilder.Entity<Linearity>().Property(f => f.SumUncertainty).HasConversion<decimal>();

            modelBuilder.Entity<Linearity>().Property(f => f.Quotient).HasConversion<decimal>();

            modelBuilder.Entity<Linearity>().Property(f => f.Square).HasConversion<decimal>();

            modelBuilder.Entity<Linearity>().Property(f => f.SumOfSquares).HasConversion<decimal>();

            modelBuilder.Entity<Linearity>().Property(f => f.TotalUncertainty).HasConversion<decimal>();

            modelBuilder.Entity<Linearity>().Property(f => f.ExpandedUncertainty).HasConversion<decimal>();

            modelBuilder.Entity<Linearity>().Property(f => f.CalibrationUncertaintyDivisor).HasConversion<decimal>();

            modelBuilder.Entity<Linearity>().Property(f => f.MinTolerance).HasConversion<decimal>();

            modelBuilder.Entity<Linearity>().Property(f => f.MaxTolerance).HasConversion<decimal>();

            modelBuilder.Entity<Linearity>().Property(f => f.Tolerance).HasConversion<decimal>();

             modelBuilder.Entity<Linearity>().Property(f => f.CalculateWeightValue).HasConversion<decimal>();

             modelBuilder.Entity<Linearity>().Property(f => f.MinToleranceAsLeft).HasConversion<decimal>();

            modelBuilder.Entity<Linearity>().Property(f => f.MaxToleranceAsLeft).HasConversion<decimal>();



            



            modelBuilder.Entity<BasicCalibrationResult>().Property(f => f.AsFound).HasConversion<decimal>();

            modelBuilder.Entity<BasicCalibrationResult>().Property(f => f.AsLeft).HasConversion<decimal>();

            modelBuilder.Entity<BasicCalibrationResult>().Property(f => f.WeightApplied).HasConversion<decimal>();

            modelBuilder.Entity<BasicCalibrationResult>().Property(f => f.ReadingStandard).HasConversion<decimal>();


            modelBuilder.Entity<BasicCalibrationResult>().Property(f => f.FinalReadingStandard).HasConversion<decimal>();

            modelBuilder.Entity<BasicCalibrationResult>().Property(f => f.Uncertainty).HasConversion<decimal>();

            modelBuilder.Entity<BasicCalibrationResult>().Property(f => f.Resolution).HasConversion<decimal>();


            modelBuilder.Entity<BalanceAndScaleCalibration>().Property(f => f.EnvironmentalFactor).HasConversion<decimal>();

            modelBuilder.Entity<BalanceAndScaleCalibration>().Property(f => f.EnvironmentalUncertaintyDivisor).HasConversion<decimal>();

            //modelBuilder.Entity<BalanceAndScaleCalibration>().Property(f => f.EnvironmentalQuotient).HasConversion<decimal>();

            //modelBuilder.Entity<BalanceAndScaleCalibration>().Property(f => f.EnvironmentalSquare).HasConversion<decimal>();

            modelBuilder.Entity<BalanceAndScaleCalibration>().Property(f => f.ResolutionUncertaintyDivisor).HasConversion<decimal>();

            modelBuilder.Entity<BalanceAndScaleCalibration>().Property(f => f.Resolution).HasConversion<decimal>();

            modelBuilder.Entity<BalanceAndScaleCalibration>().Property(f => f.ResolutionFormatted).HasConversion<decimal>();

            modelBuilder.Entity<BalanceAndScaleCalibration>().Property(f => f.ResolutionNumber).HasConversion<decimal>();






        modelBuilder.Entity<Eccentricity>().Property(f => f.EccentricityMax).HasConversion<decimal>();


            modelBuilder.Entity<Eccentricity>().Property(f => f.EccentricityMin).HasConversion<decimal>();


            modelBuilder.Entity<Eccentricity>().Property(f => f.EccentricityVarianceAsLeft).HasConversion<decimal>();

            modelBuilder.Entity<Eccentricity>().Property(f => f.EccentricityVarianceAsFound).HasConversion<decimal>();

            modelBuilder.Entity<Eccentricity>().Property(f => f.EccentricityUncertaintyDivisor).HasConversion<decimal>();

            modelBuilder.Entity<Eccentricity>().Property(f => f.EccentricityQuotient).HasConversion<decimal>();



            modelBuilder.Entity<Repeatability>().Property(f => f.RepeatabilityStdDeviationAsLeft).HasConversion<decimal>();

            modelBuilder.Entity<Repeatability>().Property(f => f.RepeatabilityStdDeviationAsFound).HasConversion<decimal>();


            modelBuilder.Entity<Repeatability>().Property(f => f.RepeatabilityUncertaintyDivisor).HasConversion<decimal>();

            modelBuilder.Entity<Repeatability>().Property(f => f.RepeatabilityQuotient).HasConversion<decimal>();









            modelBuilder.Entity<WeightSet>().Property(f => f.WeightNominalValue).HasConversion<decimal>();


            modelBuilder.Entity<WeightSet>().Property(f => f.WeightActualValue).HasConversion<decimal>();


            modelBuilder.Entity<WeightSet>().Property(f => f.CalibrationUncertValue).HasConversion<decimal>();


            modelBuilder.Entity<WeightSet>().Property(f => f.Divisor).HasConversion<decimal>();




            modelBuilder.Entity<WorkOrderDetail>().Property(f => f.Humidity).HasConversion<decimal>();


            modelBuilder.Entity<WorkOrderDetail>().Property(f => f.Temperature).HasConversion<decimal>();


            //modelBuilder.Entity<WorkOrderDetail>().Property(f => f.AccuracyPercentage).HasConversion<decimal>();


            modelBuilder.Entity<WorkOrderDetail>().Property(f => f.Resolution).HasConversion<decimal>();


            modelBuilder.Entity<WorkOrderDetail>().Property(f => f.TemperatureAfter).HasConversion<decimal>();



            modelBuilder.Entity<RangeTolerance>().Property(f => f.MinValue).HasConversion<decimal>();



            modelBuilder.Entity<RangeTolerance>().Property(f => f.MaxValue).HasConversion<decimal>();



            modelBuilder.Entity<RangeTolerance>().Property(f => f.Percent).HasConversion<decimal>();


            modelBuilder.Entity<RangeTolerance>().Property(f => f.Resolution).HasConversion<decimal>();

            


            modelBuilder.Entity<User>().HasKey(f => f.UserID);
            modelBuilder.Entity<User>().Property(f => f.UserID)
            .ValueGeneratedNever();

            modelBuilder.Entity<Customer>().HasKey(f => f.CustomerID);
            modelBuilder.Entity<Customer>().Property(f => f.CustomerID)
            .ValueGeneratedNever();


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

            

            //modelBuilder.Entity<UnitOfMeasure>().HasMany(e => e.TestPointResult).WithOne(E => E.UnitOfMeasure).HasForeignKey(t => t.UnitOfMeasureID).OnDelete(DeleteBehavior.ClientNoAction);



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

            modelBuilder.Entity<CalibrationSubType>()
            .Property(et => et.CalibrationSubTypeId)
            .ValueGeneratedNever();


            modelBuilder.Entity<CalibrationSubType_Weight>()
              .HasKey(bc => new { bc.WorkOrderDetailID, bc.WeightSetID, bc.CalibrationSubTypeID, bc.SecuenceID });
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


            modelBuilder.Entity<TechnicianCode>().HasKey(e => new { e.Code, e.StateID, e.CertificationID });


            //  modelBuilder.Entity<TechnicianCode>()
            //.HasOne(a => a.User)
            //.WithMany(b => b.TechnicianCodes)
            //.HasForeignKey(d => d.TechnicianCodeID)
            //.OnDelete(DeleteBehavior.ClientNoAction);



            modelBuilder.Entity<User>()
          .HasMany(a => a.TechnicianCodes)
          .WithOne(b => b.User)
          .HasForeignKey(d => d.UserID)
          .OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<ToleranceType>()
     .HasKey(bc => new { bc.Key, bc.CalibrationTypeId });

            // modelBuilder.Entity<TechnicianCode>()
            //.HasOne(a => a.Certification)
            //.WithMany(b => b.TechnicianCodes)
            //.HasForeignKey(d => d.CertificationID)
            //.OnDelete(DeleteBehavior.ClientNoAction);


           // modelBuilder.Entity<WorkOrderDetail>()
           //.HasOne(a => a.CalibrationType)
           //.WithMany(b => b.WorkOrderDetails)
           //.HasForeignKey(d => d.CalibrationTypeID)
           //.OnDelete(DeleteBehavior.ClientNoAction);

           

            modelBuilder.Entity<PieceOfEquipment>()
           .HasOne(a => a.UnitOfMeasure)
           .WithMany(b => b.PieceOfEquipments)
           .HasForeignKey(d => d.UnitOfMeasureID)
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
           .Entity<Helpers.Controls.Component>().Property(x => x.Route)
           .HasMaxLength(100)
           .IsUnicode(false);


            modelBuilder
.Entity<Helpers.Controls.Component>().Property(x => x.Permission)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder
.Entity<Helpers.Controls.Component>().Property(x => x.Name)
.HasMaxLength(100)
.IsUnicode(false);

            modelBuilder
.Entity<Helpers.Controls.Component>().Property(x => x.Group)
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

            //modelBuilder.Entity<CalibrationType>().Property(u => u.CalibrationTypeId).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);


            modelBuilder
    .Entity<WorkOrderDetailByStatus>()
    .ToView(nameof(WorkOrderDetailByStatus))
    .HasKey(t => t.WorkOrderDetailID);

            modelBuilder
.Entity<WorkOrderDetailByCustomer>()
.ToView(nameof(WorkOrderDetailByCustomer))
.HasKey(t => t.WorkOrderDetailID);


            //modelBuilder.Entity<CustomClaim>().HasNoKey();


            modelBuilder.Entity<CurrentUser>()
            .HasMany(a => a.Claims)
            .WithOne(b => b.User)
            .HasForeignKey(d => d.CurrentUserID)
             .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<PieceOfEquipment>().Ignore(e => e.Indicator);

            modelBuilder.Entity<WorkOrder>().Ignore(e => e.Address);

            modelBuilder.Entity<WorkOrderDetail>().Ignore(e => e.Address);


            //modelBuilder.Entity<CustomerAggregate>().Ignore(e => e.Customer);

            
            // modelBuilder.Entity<CustomerAggregate>().Ignore(e => e.Customer);
            //modelBuilder.Entity<Address>().Ignore(e => e.CustomerAggregateAggregate);
            // modelBuilder.Entity<Contact>().Ignore(e => e.CustomerAggregateAggregate);
            // modelBuilder.Entity<PhoneNumber>().Ignore(e => e.CustomerAggregateAggregate);
            // modelBuilder.Entity<Social>().Ignore(e => e.CustomerAggregateAggregate);

             modelBuilder.Entity<TestPoint>().Ignore(e => e.TestPointGroup);

             modelBuilder.Entity<BasicCalibrationResult>().Ignore(e => e.Eccentricity);

             modelBuilder.Entity<BasicCalibrationResult>().Ignore(e => e.Repeatability);

             modelBuilder.Entity<BasicCalibrationResult>().Ignore(e => e.Linearity);

             modelBuilder.Entity<Eccentricity>().Ignore(e => e.BalanceAndScaleCalibration);

             modelBuilder.Entity<Repeatability>().Ignore(e => e.BalanceAndScaleCalibration);

             modelBuilder.Entity<Linearity>().Ignore(e => e.BalanceAndScaleCalibration);

            modelBuilder.Entity<EquipmentCondition>().Ignore(e => e.WorkOrderDetail);

            modelBuilder.Entity<EquipmentType>().Ignore(e => e.EquipmentTemplates);

            modelBuilder.Entity<EquipmentType>().Ignore(e => e.PieceOfEquipment);

            modelBuilder.Entity<WeightSet>().Ignore(e => e.WOD_Weights);

            //modelBuilder.Entity<CustomerAggregate>().Ignore(e => e.Customer);

            modelBuilder.Entity<User>().Ignore(e => e.WorkOrderDetails);

            modelBuilder.Entity<User>().Ignore(e => e.WorkDetailHistories);

            modelBuilder.Entity<CalibrationType>().Ignore(e => e.WorkOrderDetails);


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            modelBuilder.Entity<CalibrationSubTypeView>()
      .HasKey(bc => new { bc.CalibrationSubTypeViewID });


            modelBuilder.Entity<CalibrationSubType>()
           .HasOne(e => e.CalibrationSubTypeView).WithOne(E => E.CalibrationSubType).HasForeignKey<CalibrationSubType>(x => x.CalibrationSubTypeViewID).OnDelete(DeleteBehavior.ClientNoAction); ;





            modelBuilder.Entity<CalibrationType>().HasKey(f => f.CalibrationTypeId);
            modelBuilder.Entity<CalibrationType>().Property(f => f.CalibrationTypeId)
           .ValueGeneratedNever();


            //modelBuilder.Entity<CalibrationType>()
            //    .HasMany(bc => bc.ToleranceTypes)
            //    .WithOne(bc => bc.CalibrationType)
            //    .HasForeignKey(bc => bc.CalibrationTypeId);

            ////////////////////////////////////////////////////////////////

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
.Entity<EquipmentTypeGroup>()
.Property(x => x.Name)
.HasMaxLength(500)
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


            modelBuilder.Entity<Note>()
         .HasOne(a => a.EquipmentType)
         .WithMany(b => b.Notes)
         .HasForeignKey(d => d.EquipmnetTypeId)
         .OnDelete(DeleteBehavior.ClientNoAction);


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

            modelBuilder.Entity<ToleranceType>()
        .HasKey(bc => new { bc.Key });




            // Configure Quotes Module entities
            ConfigureQuotesEntities(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureQuotesEntities(ModelBuilder modelBuilder)
        {
            // Configure Quote entity
            modelBuilder.Entity<Quote>(entity =>
            {
                entity.HasKey(e => e.QuoteID);
                entity.Property(e => e.QuoteNumber).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.QuoteNumber).IsUnique();
            });

            // Configure QuoteItem entity
            modelBuilder.Entity<QuoteItem>(entity =>
            {
                entity.HasKey(e => e.QuoteItemID);

                entity.HasOne(e => e.Quote)
                    .WithMany(q => q.QuoteItems)
                    .HasForeignKey(e => e.QuoteID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ParentQuoteItem)
                    .WithMany()
                    .HasForeignKey(e => e.ParentQuoteItemID)
                    .OnDelete(DeleteBehavior.Restrict);

                // Temporarily remove PriceType relationship to fix QuoteItemID1 error
                // entity.HasOne(e => e.PriceType)
                //     .WithMany(pt => pt.QuoteItems)
                //     .HasForeignKey(e => e.PriceTypeId)
                //     .HasPrincipalKey(pt => pt.PriceTypeId)
                //     .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.QuoteID);
                entity.HasIndex(e => e.PriceTypeId);
            });

            // Configure PriceType entity
            modelBuilder.Entity<PriceType>(entity =>
            {
                entity.HasKey(e => e.PriceTypeId);
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

            // Configure CustomerAddress TravelExpense
            modelBuilder.Entity<CustomerAddress>(entity =>
            {
                entity.Property(e => e.TravelExpense).HasColumnType("decimal(18,2)");
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {

            string path = $"Data Source={DatabaseService2<CalibrationSaaSDBContextOff2>.FileName};Foreign Keys=False";
            options
                   .EnableDetailedErrors()
                   .EnableSensitiveDataLogging(true)
                   .EnableServiceProviderCaching(true)
                   .EnableThreadSafetyChecks(true)
                   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                   // Suppress all EF Core warnings to keep console clean
                   .ConfigureWarnings(warnings => warnings.Default(WarningBehavior.Ignore))
                   //.UseLazyLoadingProxies()
                   //.ConfigureWarnings(warn => warn.Ignore(CoreEventId.LazyLoadOnDisposedContextWarning))
                   //.ConfigureWarnings(warn => warn.Ignore(CoreEventId.DetachedLazyLoadingWarning))
                   //.ConfigureWarnings(warn => warn.Ignore(CoreEventId.NavigationLazyLoading))
                   .UseSqlite(path);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            int result = 0;
            var tries = 0;
            var saved = false;
            while (!saved )
            {
                try
                {
                    // Attempt to save changes to the database
                    await  base.SaveChangesAsync(cancellationToken);
                    saved = true;
                    result = 1;
                }
                catch (DbUpdateConcurrencyException ex1)
                {
//                    Console.WriteLine("--------------------ERROR SAVE DbUpdateConcurrencyException____________________________________");
//                    Console.WriteLine(ex1.Message);
//                    Console.WriteLine(ex1?.InnerException?.Message);
//                    Console.WriteLine(ex1.StackTrace);

                    foreach (var entry in ex1.Entries)
                    {
                        if (1 == 1)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();


                            if(proposedValues != null)
                            {
                                foreach (var property in proposedValues.Properties)
                                {
                                    var proposedValue = proposedValues[property];

                                    if (databaseValues != null)
                                    {
                                        var databaseValue = databaseValues[property];
                                    }

                                    // TODO: decide which value should be written to database
                                    proposedValues[property] = proposedValue;
                                }
                            }
                         
                            if(databaseValues != null)
                            {
                                // Refresh original values to bypass next concurrency check
                                entry.OriginalValues.SetValues(databaseValues);
                            }
                           
                        }
                        else
                        {
                            throw new NotSupportedException(
                                "Don't know how to handle concurrency conflicts for "
                                + entry.Metadata.Name);
                        }
                    }
                    if (tries > 4)
                    {
                        base.ChangeTracker.Clear();
                        throw ex1;
                    }
                    tries++;
                }
                catch (DbUpdateException ex1)//Microsoft.EntityFrameworkCore.Relational
                {
//                    Console.WriteLine("--------------------ERROR SAVE____________________________________");
//                    Console.WriteLine(ex1.Message);
//                    Console.WriteLine(ex1?.InnerException?.Message);
//                    Console.WriteLine(ex1.StackTrace);
                    if (ex1.Source == "Microsoft.EntityFrameworkCore.Relational")
                    {
                        if (ex1.InnerException != null && (ex1.InnerException.Message.Contains("FOREIGN KEY")==true 
                            || ex1.InnerException.Message?.Contains("constraint")==true))
                        {
                           
                            throw new DbUpdateException(ex1.Message + Environment.NewLine + " InnerException: " + ex1.Message, ex1);
                        }
                        else
                        {
                            throw new DbUpdateException(ex1.Message + Environment.NewLine + " InnerException: " + ex1.Message,ex1);
                        }

                    }
                    
                    if(tries > 4)
                    {
                        base.ChangeTracker.Clear();
                        throw ex1;
                    }
                    tries++;
                }
               
            }
            return result;
        }

       
    }
}
