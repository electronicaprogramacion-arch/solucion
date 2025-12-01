
using Bogus;
using CalibrationSaaS.Data.EntityFramework;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.Views;
using Helpers.Controls;
using IndexedDB.Blazor;
using LinqKit;
using Microsoft.EntityFrameworkCore;
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

namespace CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite
{
  

//    public class CalibrationSaaSDBContextOff33 : DbContext, ICalibrationSaaSDBContextBase, ICalibrationSaaSDBContextBaseOff
//    {
//        public IJSRuntime IJSRuntime { get; set; }

//        //public DbSet<Contribution> Contributions { get; set; }
//        //public DbSet<Speaker> Speakers { get; set; }
//        //public DbSet<ContributionSpeaker> ContributionSpeakers { get; set; }


//        //public DbSet<CalibrationSaaS.Domain.Aggregates.Entities.WorkOrderDetail> WorkOrderDetail { get; set; }

//        //public DbSet<CalibrationSaaS.Domain.Aggregates.Entities.EquipmentType> EquipmentType { get; set; }

//        //public DbSet<CalibrationSaaS.Domain.Aggregates.Entities.Manufacturer> Manufacturer { get; set; }

//        //public DbSet<CalibrationSaaS.Domain.Aggregates.Entities.Status> Status { get; set; }
//        //public DbSet<CalibrationSaaS.Domain.Aggregates.Entities.User> User { get; set; }

//        //public DbSet<CalibrationSaaS.Domain.Aggregates.Entities.Certification> Certification { get; set; }


//        //public DbSet<CalibrationSaaS.Domain.Aggregates.Entities.CalibrationType> CalibrationType { get; set; }

//        //public DbSet<CalibrationSaaS.Domain.Aggregates.Entities.PieceOfEquipment> PieceOfEquipment { get; set; }



//        //public DbSet<CalibrationSaaS.Domain.Aggregates.Entities.Certificate> Certificate { get; set; }


//        //public DbSet<CalibrationSaaS.Domain.Aggregates.Entities.UnitOfMeasureType> UnitOfMeasureType { get; set; }


//        //public DbSet<CalibrationSaaS.Domain.Aggregates.Entities.UnitOfMeasure> UnitOfMeasure { get; set; }


//        //public DbSet<CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder> WorkOrder { get; set; }

//        //public DbSet<CalibrationSaaS.Domain.Aggregates.Entities.Address> Address { get; set; }

//        //public DbSet<CalibrationSaaS.Domain.Aggregates.Entities.CustomerAggregate> CustomerAggregate { get; set; }

//        //public DbSet<CalibrationSaaS.Domain.Aggregates.Entities.Customer> Customer { get; set; }

//        //public DbSet<CalibrationSaaS.Domain.Aggregates.Entities.EquipmentTemplate> EquipmentTemplate { get; set; }

//        //public DbSet<CalibrationSaaS.Domain.Aggregates.Entities.Rol> Roles { get; set; }




//        public DbSet<CurrentUser> CurrentUser { get; set; }

//        public DbSet<CustomClaim> CustomClaim { get; set; }


//        public virtual DbSet<BalanceAndScaleCalibration> BalanceAndScaleCalibration { get; set; }
//        public virtual DbSet<Customer> Customer { get; set; }
//        //public virtual DbSet<CustomerAddress> CustomerAddress { get; set; }

//        public virtual DbSet<Eccentricity> Eccentricity { get; set; }
//        public virtual DbSet<EquipmentCondition> EquipmentCondition { get; set; }
//        public virtual DbSet<Linearity> Linearity { get; set; }
//        public virtual DbSet<PieceOfEquipment> PieceOfEquipment { get; set; }
//        public virtual DbSet<Repeatability> Repeatability { get; set; }
//        public virtual DbSet<Tenant> Tenant { get; set; }
//        public virtual DbSet<WorkOrder> WorkOrder { get; set; }
//        public virtual DbSet<WorkOrderDetail> WorkOrderDetail { get; set; }
//        public virtual DbSet<WorkDetailHistory> WorkDetailHistory { get; set; }


//        public virtual DbSet<UserInformation> UserInformation { get; set; }
//        public virtual DbSet<User> User { get; set; }

//        public virtual DbSet<EquipmentType> EquipmentType { get; set; }

//        public virtual DbSet<EquipmentTemplate> EquipmentTemplate { get; set; }

//        public virtual DbSet<Manufacturer> Manufacturer { get; set; }

//        public virtual DbSet<CustomerAggregate> CustomerAggregates { get; set; }

//        public virtual DbSet<Contact> Contact { get; set; }

//        public virtual DbSet<Address> Address { get; set; }

//        public virtual DbSet<EmailAddress> EmailAddress { get; set; }



//        public virtual DbSet<TestPointGroup> TestPointGroup { get; set; }


//        public virtual DbSet<TestPoint> TestPoint { get; set; }

//        public virtual DbSet<RangeTolerance> RangeTolerance { get; set; }


//        public virtual DbSet<UnitOfMeasure> UnitOfMeasure { get; set; }
//        public virtual DbSet<Status> Status { get; set; }

//        public virtual DbSet<Rol> Rol { get; set; }

//        public virtual DbSet<UnitOfMeasureType> UnitOfMeasureType { get; set; }

//        public virtual DbSet<User_Rol> User_Rol { get; set; }

//        public virtual DbSet<PhoneNumber> PhoneNumber { get; set; }

//        public DbSet<POE_WorkOrder> POE_WorkOrder { get; set; }

//        public DbSet<User_WorkOrder> User_WorkOrder { get; set; }

//        public DbSet<RepeatibilityCalibrationResult> RepeatibilityCalibrationResult { get; set; }

//        public DbSet<EccentricityCalibrationResult> EccentricityCalibrationResult { get; set; }

//        public DbSet<BasicCalibrationResult> BasicCalibrationResult { get; set; }


//        public virtual DbSet<WeightSet> WeightSet { get; set; }


//        public virtual DbSet<WOD_Weight> WOD_Weight { get; set; }

//        public virtual DbSet<WOD_TestPoint> WOD_TestPoint { get; set; }

//        public virtual DbSet<CalibrationSubType_Weight> CalibrationSubType_Weight { get; set; }


//        public virtual DbSet<CalibrationSubType> CalibrationSubType { get; set; }


//        public virtual DbSet<Certification> Certification { get; set; }

//        public virtual DbSet<TechnicianCode> TechnicianCode { get; set; }
//        public virtual DbSet<WorkOrderDetailByStatus> WorkOrderDetailByStatus { get; set; }
//        public virtual DbSet<WorkOrderDetailByStatus> WorkOrderDetailByEquipment { get; set; }
//        public virtual DbSet<WorkOrderDetailByCustomer> WorkOrderDetailByCustomer { get; set; }


//        public virtual DbSet<CalibrationType> CalibrationType { get; set; }

//        public virtual DbSet<Social> Social { get; set; }

//        public virtual DbSet<POE_User> POE_User { get; set; }

//        public virtual DbSet<Certificate> Certificate { get; set; }

//        public virtual DbSet<CertificatePoE> CertificatePoE { get; set; }


//        public virtual DbSet<POE_POE> POE_POE { get; set; }

//        public virtual DbSet<Helpers.Controls.Component> Component { get; set; }

//        public virtual DbSet<UnitOfMeasure2> UnitOfMeasure2 { get; set; }
//        public DbSet<Procedure> Procedure { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<Micro> Micro { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<MicroResult> MicroResult { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<WOD_Standard> WOD_Standard { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<Force> Force { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<ForceResult> ForceResult { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<Uncertainty> Uncertainty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<CalibrationResultContributor> CalibrationResultContributor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<TestCode> TestCode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<ToleranceType> ToleranceType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
       
//        public DbSet<Rockwell> Rockwell { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<RockwellResult> RockwellResult { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<POE_Scale> POE_Scale { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<ExternalCondition> ExternalCondition { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<CalibrationSubType_Standard> CalibrationSubType_Standard { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<Note> Note { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<NoteWOD> NoteWOD { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<DynamicProperty> DynamicProperty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<ViewPropertyBase> ViewPropertyBase { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<GenericCalibration> GenericCalibration { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        //public DbSet<GenericCalibrationResult> GenericCalibrationResult { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        //public DbSet<CalibrationSubType_DynamicProperty> CalibrationSubType_DynamicProperty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        
//        //public DbSet<CalibrationSubType_ViewProperty> CalibrationSubType_ViewProperty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<CalibrationSubTypeView> CalibrationSubTypeView { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<GenericCalibration2> GenericCalibration2 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<GenericCalibrationResult2> GenericCalibrationResult2 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<Mass> Mass { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public DbSet<Lenght> Lenght { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

//        public DbSet<EquipmentTypeGroup> EquipmentTypeGroup { get; set; }

//        public DbSet<WOD_Procedure> WOD_Procedure { get; set; }

//        public DbSet<WOD_ParametersTable> WOD_ParametersTable { get; set; }
//        public DbSet<GenericCalibrationResult> GenericCalibrationResult { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

//        private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

//        private readonly ISqliteWasmDbContextFactory<CalibrationSaaSDBContextOff33> Factory;
//        public CalibrationSaaSDBContextOff33(DbContextOptions<CalibrationSaaSDBContextOff33> options
//            , IJSRuntime jsRuntime, ISqliteWasmDbContextFactory<CalibrationSaaSDBContextOff33> _Factory)
//        : base(options)
//        {
//            // REVIEW: Nicht im Konstruktor machen, auslagern
//            _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
//               "import", "./js/file5.js").AsTask());

//            IJSRuntime = jsRuntime;
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {

//            //modelBuilder.Entity<UnitOfMeasureType>().HasData(
//            //new UnitOfMeasureType { Value=1,Name= "Temperature" });

//            //modelBuilder.Entity<UnitOfMeasureType>().HasData(
//            //new UnitOfMeasureType { Value = 2, Name = "Humidity" });

//            //modelBuilder.Entity<UnitOfMeasureType>().HasData(
//            //new UnitOfMeasureType { Value = 3, Name = "Weight" });


//            //modelBuilder.Entity<UnitOfMeasure>().HasData(
//            //new UnitOfMeasure { Name = "Pounds", Abbreviation = "Pd", UnitOfMeasureID = 1, TypeID = 3, IsEnabled = true, UncertaintyUnitOfMeasureID = 1 });

//            //modelBuilder.Entity<UnitOfMeasure>().HasData(
//            //new UnitOfMeasure { Name = "Kilogramo", Abbreviation = "Kg", UnitOfMeasureID = 2, TypeID = 3, IsEnabled = true, UncertaintyUnitOfMeasureID = 2 });

//            //modelBuilder.Entity<UnitOfMeasureType>().Property(p => p.Value).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

//            modelBuilder.Entity<UnitOfMeasureType>().HasKey(f => f.Value);
//            modelBuilder.Entity<UnitOfMeasureType>().Property(f => f.Value)
//           .ValueGeneratedNever();

//            modelBuilder.Entity<UnitOfMeasure>().HasKey(f => f.UnitOfMeasureID);
//            modelBuilder.Entity<UnitOfMeasure>().Property(f => f.UnitOfMeasureID)
//           .ValueGeneratedNever();

//            modelBuilder.Entity<CertificatePoE>().HasKey(f => f.CertificateNumber);
//            //modelBuilder.Entity<CertificatePoE>().Property(f => f.CertificatePoEId)
//            //.ValueGeneratedNever();

//            modelBuilder.Entity<UnitOfMeasure2>().HasKey(f => f.UnitOfMeasureID);
//            modelBuilder.Entity<UnitOfMeasure2>().Property(f => f.UnitOfMeasureID)
//           .ValueGeneratedNever();

//            modelBuilder.Entity<UnitOfMeasure>().Property(f => f.ConversionValue).HasConversion<decimal>();


//            //modelBuilder.Entity<UnitOfMeasureType>()
//            //    .HasMany(e => e.UnitOfMeasure2).WithOne(E => E.Type).HasForeignKey(e => e.TypeID);
//               modelBuilder.Entity<CustomerAggregate>().HasKey(f => f.AggregateID);
//            modelBuilder.Entity<CustomerAggregate>().Property(f => f.AggregateID)
//            .ValueGeneratedNever();


//             modelBuilder.Entity<Contact>().HasKey(f => f.ContactID);
//            modelBuilder.Entity<Contact>().Property(f => f.ContactID)
//            .ValueGeneratedNever();

//              modelBuilder.Entity<Address>().HasKey(f => f.AddressId);
//            modelBuilder.Entity<Address>().Property(f => f.AddressId)
//            .ValueGeneratedNever();

//                modelBuilder.Entity<PhoneNumber>().HasKey(f => f.PhoneNumberID);
//            modelBuilder.Entity<PhoneNumber>().Property(f => f.PhoneNumberID)
//            .ValueGeneratedNever();

//                modelBuilder.Entity<Social>().HasKey(f => f.SocialID);
//            modelBuilder.Entity<Social>().Property(f => f.SocialID)
//            .ValueGeneratedNever();

//             modelBuilder.Entity<PieceOfEquipment>().HasKey(f => f.PieceOfEquipmentID);
//            modelBuilder.Entity<PieceOfEquipment>().Property(f => f.PieceOfEquipmentID)
//            .ValueGeneratedNever();

//            //   modelBuilder.Entity<CustomerAggregate>()
//            //    .HasMany(e => e.Addresses).WithOne(E => E.CustomerAggregateAggregate).HasForeignKey(e => e.CustomerAggregateAggregateID).IsRequired(false);

//            //modelBuilder.Entity<CustomerAggregate>()
//            //    .HasMany(e => e.Contacts).WithOne(E => E.CustomerAggregateAggregate).HasForeignKey(e => e.CustomerAggregateAggregateID).IsRequired(false);


//            // modelBuilder.Entity<CustomerAggregate>()
//            //    .HasMany(e => e.Socials).WithOne(E => E.CustomerAggregateAggregate).HasForeignKey(e => e.CustomerAggregateAggregateID).IsRequired(false);

//            //modelBuilder.Entity<CustomerAggregate>()
//            //    .HasMany(e => e.PhoneNumbers).WithOne(E => E.CustomerAggregateAggregate).HasForeignKey(e => e.CustomerAggregateAggregateID).IsRequired(false);
//            //  modelBuilder.Entity<Address>()
//            //    .HasOne(e => e.CustomerAggregateAggregate).WithMany(E => E.Addresses).HasForeignKey(e => e.CustomerAggregateAggregateID).IsRequired(false);

//            //modelBuilder.Entity<Contact>()
//            //    .HasOne(e => e.CustomerAggregateAggregate).WithMany(E => E.Contacts).HasForeignKey(e => e.CustomerAggregateAggregateID).IsRequired(false);


            
//            //modelBuilder.Entity<Social>()
//            //    .HasOne(e => e.CustomerAggregateAggregate).WithMany(E => E.Socials).HasForeignKey(e => e.CustomerAggregateAggregateID).IsRequired(false);

//            //modelBuilder.Entity<PhoneNumber>()
//            //    .HasOne(e => e.CustomerAggregateAggregate).WithMany(E => E.PhoneNumbers).HasForeignKey(e => e.CustomerAggregateAggregateID).IsRequired(false);




//            modelBuilder.Entity<Manufacturer>().HasKey(f => f.ManufacturerID);
//            modelBuilder.Entity<Manufacturer>().Property(f => f.ManufacturerID)
//           .ValueGeneratedNever();


//            modelBuilder.Entity<Status>().HasKey(f => f.StatusId);
//            modelBuilder.Entity<Status>().Property(f => f.StatusId)
//           .ValueGeneratedNever();

//            modelBuilder.Entity<EquipmentTemplate>().HasKey(f => f.EquipmentTemplateID);
//            modelBuilder.Entity<EquipmentTemplate>().Property(f => f.EquipmentTemplateID)
//            .ValueGeneratedNever();


//               modelBuilder.Entity<TestPointGroup>().HasKey(f => f.TestPoitGroupID);
//            modelBuilder.Entity<TestPointGroup>().Property(f => f.TestPoitGroupID)
//            .ValueGeneratedNever();

//              modelBuilder.Entity<TestPoint>().HasKey(f => f.TestPointID);
//            modelBuilder.Entity<TestPoint>().Property(f => f.TestPointID)
//            .ValueGeneratedNever();

//             modelBuilder.Entity<Certificate>().HasKey(f => f.CertificateID);
//            modelBuilder.Entity<Certificate>().Property(f => f.CertificateID)
//            .ValueGeneratedNever();

          

//            modelBuilder.Entity<EquipmentCondition>().HasKey(f => f.EquipmentConditionId);
//            modelBuilder.Entity<EquipmentCondition>().Property(f => f.EquipmentConditionId)
//            .ValueGeneratedNever();

//              modelBuilder.Entity<WorkOrderDetail>().HasKey(f => f.WorkOrderDetailID);
//            modelBuilder.Entity<WorkOrderDetail>().Property(f => f.WorkOrderDetailID)
//            .ValueGeneratedNever();


//            //modelBuilder.Entity<EquipmentTemplate>().Property(f => f.AccuracyPercentage).HasConversion<decimal>();

//            modelBuilder.Entity<EquipmentTemplate>().Property(f => f.Resolution).HasConversion<decimal>();

//            modelBuilder.Entity<EquipmentTemplate>().Property(f => f.Capacity).HasConversion<decimal>();



//            modelBuilder.Entity<TestPoint>().Property(f => f.NominalTestPoit).HasConversion<decimal>();

//            modelBuilder.Entity<TestPoint>().Property(f => f.LowerTolerance).HasConversion<decimal>();

//            modelBuilder.Entity<TestPoint>().Property(f => f.UpperTolerance).HasConversion<decimal>();

//            modelBuilder.Entity<TestPoint>().Property(f => f.Resolution).HasConversion<decimal>();



//            modelBuilder.Entity<PieceOfEquipment>().Property(f => f.Resolution).HasConversion<decimal>();

//            modelBuilder.Entity<PieceOfEquipment>().Property(f => f.Capacity).HasConversion<decimal>();

//            //modelBuilder.Entity<PieceOfEquipment>().Property(f => f.AccuracyPercentage).HasConversion<decimal>();

//            modelBuilder.Entity<PieceOfEquipment>().Property(f => f.Resolution).HasConversion<decimal>();

            
            
            
            
//            modelBuilder.Entity<Linearity>().Property(f => f.TotalNominal).HasConversion<decimal>();

//            modelBuilder.Entity<Linearity>().Property(f => f.TotalActual).HasConversion<decimal>();

//            modelBuilder.Entity<Linearity>().Property(f => f.SumUncertainty).HasConversion<decimal>();

//            modelBuilder.Entity<Linearity>().Property(f => f.Quotient).HasConversion<decimal>();

//            modelBuilder.Entity<Linearity>().Property(f => f.Square).HasConversion<decimal>();

//            modelBuilder.Entity<Linearity>().Property(f => f.SumOfSquares).HasConversion<decimal>();

//            modelBuilder.Entity<Linearity>().Property(f => f.TotalUncertainty).HasConversion<decimal>();

//            modelBuilder.Entity<Linearity>().Property(f => f.ExpandedUncertainty).HasConversion<decimal>();

//            modelBuilder.Entity<Linearity>().Property(f => f.CalibrationUncertaintyDivisor).HasConversion<decimal>();

//            modelBuilder.Entity<Linearity>().Property(f => f.MinTolerance).HasConversion<decimal>();

//            modelBuilder.Entity<Linearity>().Property(f => f.MaxTolerance).HasConversion<decimal>();

//            modelBuilder.Entity<Linearity>().Property(f => f.Tolerance).HasConversion<decimal>();

//             modelBuilder.Entity<Linearity>().Property(f => f.CalculateWeightValue).HasConversion<decimal>();

//             modelBuilder.Entity<Linearity>().Property(f => f.MinToleranceAsLeft).HasConversion<decimal>();

//            modelBuilder.Entity<Linearity>().Property(f => f.MaxToleranceAsLeft).HasConversion<decimal>();



            



//            modelBuilder.Entity<BasicCalibrationResult>().Property(f => f.AsFound).HasConversion<decimal>();

//            modelBuilder.Entity<BasicCalibrationResult>().Property(f => f.AsLeft).HasConversion<decimal>();

//            modelBuilder.Entity<BasicCalibrationResult>().Property(f => f.WeightApplied).HasConversion<decimal>();

//            modelBuilder.Entity<BasicCalibrationResult>().Property(f => f.ReadingStandard).HasConversion<decimal>();


//            modelBuilder.Entity<BasicCalibrationResult>().Property(f => f.FinalReadingStandard).HasConversion<decimal>();

//            modelBuilder.Entity<BasicCalibrationResult>().Property(f => f.Uncertainty).HasConversion<decimal>();

//            modelBuilder.Entity<BasicCalibrationResult>().Property(f => f.Resolution).HasConversion<decimal>();


//            modelBuilder.Entity<BalanceAndScaleCalibration>().Property(f => f.EnvironmentalFactor).HasConversion<decimal>();

//            modelBuilder.Entity<BalanceAndScaleCalibration>().Property(f => f.EnvironmentalUncertaintyDivisor).HasConversion<decimal>();

//            //modelBuilder.Entity<BalanceAndScaleCalibration>().Property(f => f.EnvironmentalQuotient).HasConversion<decimal>();

//            //modelBuilder.Entity<BalanceAndScaleCalibration>().Property(f => f.EnvironmentalSquare).HasConversion<decimal>();

//            modelBuilder.Entity<BalanceAndScaleCalibration>().Property(f => f.ResolutionUncertaintyDivisor).HasConversion<decimal>();

//            modelBuilder.Entity<BalanceAndScaleCalibration>().Property(f => f.Resolution).HasConversion<decimal>();

//            modelBuilder.Entity<BalanceAndScaleCalibration>().Property(f => f.ResolutionFormatted).HasConversion<decimal>();

//            modelBuilder.Entity<BalanceAndScaleCalibration>().Property(f => f.ResolutionNumber).HasConversion<decimal>();






//        modelBuilder.Entity<Eccentricity>().Property(f => f.EccentricityMax).HasConversion<decimal>();


//            modelBuilder.Entity<Eccentricity>().Property(f => f.EccentricityMin).HasConversion<decimal>();


//            modelBuilder.Entity<Eccentricity>().Property(f => f.EccentricityVarianceAsLeft).HasConversion<decimal>();

//            modelBuilder.Entity<Eccentricity>().Property(f => f.EccentricityVarianceAsFound).HasConversion<decimal>();

//            modelBuilder.Entity<Eccentricity>().Property(f => f.EccentricityUncertaintyDivisor).HasConversion<decimal>();

//            modelBuilder.Entity<Eccentricity>().Property(f => f.EccentricityQuotient).HasConversion<decimal>();



//            modelBuilder.Entity<Repeatability>().Property(f => f.RepeatabilityStdDeviationAsLeft).HasConversion<decimal>();

//            modelBuilder.Entity<Repeatability>().Property(f => f.RepeatabilityStdDeviationAsFound).HasConversion<decimal>();


//            modelBuilder.Entity<Repeatability>().Property(f => f.RepeatabilityUncertaintyDivisor).HasConversion<decimal>();

//            modelBuilder.Entity<Repeatability>().Property(f => f.RepeatabilityQuotient).HasConversion<decimal>();









//            modelBuilder.Entity<WeightSet>().Property(f => f.WeightNominalValue).HasConversion<decimal>();


//            modelBuilder.Entity<WeightSet>().Property(f => f.WeightActualValue).HasConversion<decimal>();


//            modelBuilder.Entity<WeightSet>().Property(f => f.CalibrationUncertValue).HasConversion<decimal>();


//            modelBuilder.Entity<WeightSet>().Property(f => f.Divisor).HasConversion<decimal>();




//            modelBuilder.Entity<WorkOrderDetail>().Property(f => f.Humidity).HasConversion<decimal>();


//            modelBuilder.Entity<WorkOrderDetail>().Property(f => f.Temperature).HasConversion<decimal>();


//            //modelBuilder.Entity<WorkOrderDetail>().Property(f => f.AccuracyPercentage).HasConversion<decimal>();


//            modelBuilder.Entity<WorkOrderDetail>().Property(f => f.Resolution).HasConversion<decimal>();


//            modelBuilder.Entity<WorkOrderDetail>().Property(f => f.TemperatureAfter).HasConversion<decimal>();



//            modelBuilder.Entity<RangeTolerance>().Property(f => f.MinValue).HasConversion<decimal>();



//            modelBuilder.Entity<RangeTolerance>().Property(f => f.MaxValue).HasConversion<decimal>();



//            modelBuilder.Entity<RangeTolerance>().Property(f => f.Percent).HasConversion<decimal>();


//            modelBuilder.Entity<RangeTolerance>().Property(f => f.Resolution).HasConversion<decimal>();




//            modelBuilder.Entity<User>().HasKey(f => f.UserID);
//            modelBuilder.Entity<User>().Property(f => f.UserID)
//            .ValueGeneratedNever();

//            modelBuilder.Entity<Customer>().HasKey(f => f.CustomerID);
//            modelBuilder.Entity<Customer>().Property(f => f.CustomerID)
//            .ValueGeneratedNever();


//            modelBuilder.Entity<EquipmentTemplate>()
//                .HasMany(e => e.PieceOfEquipments).WithOne(E => E.EquipmentTemplate).HasForeignKey(e => e.EquipmentTemplateId);

//            modelBuilder.Entity<Customer>()
//                .HasMany(e => e.PieceOfEquipment).WithOne(E => E.Customer).HasForeignKey(e => e.CustomerId);




//            modelBuilder.Entity<Customer>()
//                .HasMany(e => e.WorkOrder).WithOne(E => E.Customer).HasForeignKey(e => e.CustomerId);

//            modelBuilder.Entity<Address>()
//                .HasMany(e => e.WorkOrders).WithOne(E => E.Address).HasForeignKey(e => e.AddressId);

//            modelBuilder.Entity<User>()
//            .HasIndex(p => p.Email).IsUnique(true);


//            modelBuilder.Entity<User_Rol>()
//         .HasKey(bc => new { bc.UserID, bc.RolID });
//            modelBuilder.Entity<User_Rol>()
//                .HasOne(bc => bc.User)
//                .WithMany(b => b.UserRoles)
//                .HasForeignKey(bc => bc.UserID);
//            modelBuilder.Entity<User_Rol>()
//                .HasOne(bc => bc.Rol)
//                .WithMany(c => c.UserRoles)
//                .HasForeignKey(bc => bc.RolID);


//            modelBuilder.Entity<User_WorkOrder>()
//      .HasKey(bc => new { bc.UserID, bc.WorkOrderID });
//            modelBuilder.Entity<User_WorkOrder>()
//                .HasOne(bc => bc.User)
//                .WithMany(b => b.UserWorkOrders)
//                .HasForeignKey(bc => bc.UserID);
//            modelBuilder.Entity<User_WorkOrder>()
//                .HasOne(bc => bc.WorkOrder)
//                .WithMany(c => c.UserWorkOrders)
//                .HasForeignKey(bc => bc.WorkOrderID);



//            modelBuilder.Entity<TestPoint>(entity =>
//            {
//                entity.Property(e => e.TestPointID)
//                    .HasColumnName("TestPointID");
//                entity.Property(e => e.UnitOfMeasurementOutID).HasColumnName("UnitOfMeasureOutID");
//                entity.Property(e => e.UnitOfMeasurementID).HasColumnName("UnitOfMeasurementID");
//                entity.HasOne(d => d.UnitOfMeasurementOut)
//                    .WithMany(p => p.TestPointsOut)
//                    .HasForeignKey(d => d.UnitOfMeasurementOutID)
//                    .OnDelete(DeleteBehavior.ClientNoAction)
//                    .HasConstraintName("FK_TestPointsOut_UnitOfMeasureOut_UnitOfMeasureOutID");
//                entity.HasOne(d => d.UnitOfMeasurement)
//                    .WithMany(p => p.TestPointsIn)
//                    .HasForeignKey(d => d.UnitOfMeasurementID)
//                    .OnDelete(DeleteBehavior.ClientNoAction)
//                    .HasConstraintName("FK_TestPointsin_UnitOfMeasureIn_UnitOfMeasureInID");
//            });



//            modelBuilder.Entity<EquipmentTemplate>(entity =>
//            {
//                entity.Property(e => e.EquipmentTemplateID)
//                    .HasColumnName("EquipmentTemplateID");
//                entity.Property(e => e.UnitofmeasurementID).HasColumnName("UnitofmeasurementID");
//                entity.HasOne(d => d.UnitOfMeasure)
//                   .WithMany(p => p.EquipmentTemplates)
//                   .HasForeignKey(d => d.UnitofmeasurementID)
//                   .OnDelete(DeleteBehavior.ClientNoAction)
//                   .HasConstraintName("FK_EquipmentTemplate_UnitOfMeasureIn_UnitOfMeasureInID");

//            });

//            modelBuilder.Entity<EquipmentTemplate>(entity =>
//            {
//                entity.Property(e => e.EquipmentTemplateID)
//                    .HasColumnName("EquipmentTemplateID");
//                entity.Property(e => e.ManufacturerID).HasColumnName("ManufacturerID");
//                entity.HasOne(d => d.Manufacturer1)
//                   .WithMany(p => p.EquipmentTemplates)
//                   .HasForeignKey(d => d.ManufacturerID)
//                   .OnDelete(DeleteBehavior.ClientNoAction)
//                   .HasConstraintName("FK_EquipmentTemplate_Manufacturer_Manufacturer1ID");
//            });

//            modelBuilder.Entity<EquipmentTemplate>(entity =>
//            {
//                entity.Property(e => e.EquipmentTemplateID)
//                    .HasColumnName("EquipmentTemplateID");
//                entity.Property(e => e.EquipmentTypeID).HasColumnName("EquipmentTypeID");
//                entity.HasOne(d => d.EquipmentTypeObject)
//                   .WithMany(p => p.EquipmentTemplates)
//                   .HasForeignKey(d => d.EquipmentTypeID)
//                   .OnDelete(DeleteBehavior.ClientNoAction)
//                   .HasConstraintName("FK_EquipmentTemplate_EquipmentType_EquipmentType1ID");
//            });


//            modelBuilder.Entity<WorkOrderDetail>(entity =>
//            {
//                entity.Property(e => e.WorkOrderDetailID)
//                    .HasColumnName("WorkOrderDetailID");
//                entity.Property(e => e.WorkOrderID).HasColumnName("WorkOderID");
//                entity.HasOne(d => d.WorkOder)
//                   .WithMany(p => p.WorkOrderDetails)
//                   .HasForeignKey(d => d.WorkOrderID)
//                   .OnDelete(DeleteBehavior.Cascade)
//                   .HasConstraintName("FK_WorkOrderDetail_WorkOrderDetail_WorkOrderDetailID");
//            });




//            modelBuilder.Entity<WorkOrderDetail>()
//            .HasOne(a => a.PieceOfEquipment)
//            .WithMany(b => b.WorOrderDetails)
//            .HasForeignKey(d => d.PieceOfEquipmentId)
//             .OnDelete(DeleteBehavior.Cascade);




//            modelBuilder.Entity<POE_WorkOrder>().HasKey(sc => new { sc.PieceOfEquipmentID, sc.WorkOrderID });

//            modelBuilder.Entity<POE_WorkOrder>()
//            .HasOne<PieceOfEquipment>(sc => sc.PieceOfEquipment)
//            .WithMany(s => s.POE_WorkOrders)
//            .HasForeignKey(sc => sc.PieceOfEquipmentID)
//            .OnDelete(DeleteBehavior.ClientNoAction);



//            modelBuilder.Entity<POE_WorkOrder>()
//                .HasOne<WorkOrder>(sc => sc.WorkOrder)
//                .WithMany(s => s.POE_WorkOrders)
//                .HasForeignKey(sc => sc.WorkOrderID)
//                .OnDelete(DeleteBehavior.ClientNoAction);



//            modelBuilder.Entity<User>().HasMany(e => e.WorkOrderDetails)
//                .WithOne(E => E.Technician)
//                .HasForeignKey(t => t.TechnicianID).IsRequired(false)
//                .OnDelete(DeleteBehavior.ClientNoAction);



//            modelBuilder.Entity<WorkOrderDetail>()
//           .HasOne(a => a.CurrentStatus)
//           .WithMany(b => b.WorkOrderDetails)
//           .HasForeignKey(d => d.CurrentStatusID)
//            .OnDelete(DeleteBehavior.Cascade);


//            modelBuilder.Entity<Linearity>()
//           .HasOne(a => a.TestPoint)
//           .WithMany(b => b.Linearities)
//           .HasForeignKey(d => d.TestPointID)
//            .OnDelete(DeleteBehavior.ClientNoAction);


//            modelBuilder.Entity<Repeatability>()
//          .HasOne(a => a.TestPoint)
//          .WithMany(b => b.Repeatabilities)
//          .HasForeignKey(d => d.TestPointID)
//           .OnDelete(DeleteBehavior.ClientNoAction);


//            modelBuilder.Entity<Eccentricity>()
//       .HasOne(a => a.TestPoint)
//       .WithMany(b => b.Eccentricities)
//       .HasForeignKey(d => d.TestPointID)
//        .OnDelete(DeleteBehavior.ClientNoAction);




//            modelBuilder.Entity<Linearity>().HasKey(e => new { e.SequenceID, e.CalibrationSubTypeId, e.WorkOrderDetailId });


//            modelBuilder.Entity<BasicCalibrationResult>().HasKey(e => new { e.SequenceID, e.CalibrationSubTypeId, e.WorkOrderDetailId });



//            modelBuilder.Entity<WorkOrderDetail>().HasOne(e => e.BalanceAndScaleCalibration).WithOne(E => E.WorkOrderDetail).HasForeignKey<BalanceAndScaleCalibration>(t => t.WorkOrderDetailId).OnDelete(DeleteBehavior.ClientNoAction);

//            modelBuilder.Entity<BalanceAndScaleCalibration>().HasOne(e => e.Eccentricity).WithOne(E => E.BalanceAndScaleCalibration).HasForeignKey<Eccentricity>(t => t.WorkOrderDetailId).OnDelete(DeleteBehavior.ClientNoAction);

//            modelBuilder.Entity<BalanceAndScaleCalibration>().HasOne(e => e.Repeatability).WithOne(E => E.BalanceAndScaleCalibration).HasForeignKey<Repeatability>(t => t.WorkOrderDetailId).OnDelete(DeleteBehavior.ClientNoAction);

//            modelBuilder.Entity<BalanceAndScaleCalibration>().HasMany(e => e.Linearities).WithOne(E => E.BalanceAndScaleCalibration).HasForeignKey(t => t.WorkOrderDetailId).OnDelete(DeleteBehavior.ClientNoAction);


//            modelBuilder.Entity<Linearity>().HasOne(e => e.BasicCalibrationResult).WithOne(E => E.Linearity)
//                .HasForeignKey<Linearity>(e => new { e.SequenceID, e.CalibrationSubTypeId, e.WorkOrderDetailId }).OnDelete(DeleteBehavior.ClientNoAction);


//            modelBuilder.Entity<Eccentricity>().HasKey(e => new { e.CalibrationSubTypeId, e.WorkOrderDetailId });
           
//            modelBuilder.Entity<Eccentricity>().HasMany(e => e.TestPointResult).WithOne(E => E.Eccentricity)
//                .HasForeignKey(t => new { t.CalibrationSubTypeId, t.WorkOrderDetailId }).OnDelete(DeleteBehavior.ClientNoAction);



//            modelBuilder.Entity<Repeatability>().HasKey(e => new { e.CalibrationSubTypeId, e.WorkOrderDetailId });
//            modelBuilder.Entity<Repeatability>().HasMany(e => e.TestPointResult).WithOne(E => E.Repeatability)
//                .HasForeignKey(t => new { t.CalibrationSubTypeId, t.WorkOrderDetailId }).OnDelete(DeleteBehavior.ClientNoAction);

            

//            //modelBuilder.Entity<UnitOfMeasure>().HasMany(e => e.TestPointResult).WithOne(E => E.UnitOfMeasure).HasForeignKey(t => t.UnitOfMeasureID).OnDelete(DeleteBehavior.ClientNoAction);



//            modelBuilder.Entity<UnitOfMeasure>().HasOne(e => e.UnitOfMeasureBase).WithOne(E => E.ParentUnitOfMeasureBase).HasForeignKey<UnitOfMeasure>(t => t.UnitOfMeasureBaseID).OnDelete(DeleteBehavior.ClientNoAction);

//            modelBuilder.Entity<UnitOfMeasure>().HasMany(e => e.WeightSets).WithOne(E => E.UnitOfMeasure).HasForeignKey(t => t.UnitOfMeasureID).OnDelete(DeleteBehavior.ClientNoAction);

//            modelBuilder.Entity<UnitOfMeasure>().HasMany(e => e.UncertaintyWeightSets).WithOne(E => E.UncertaintyUnitOfMeasure).HasForeignKey(t => t.UncertaintyUnitOfMeasureId).OnDelete(DeleteBehavior.ClientNoAction);

//            modelBuilder.Entity<UnitOfMeasure>().HasMany(e => e.Linearities).WithOne(E => E.UnitOfMeasure).HasForeignKey(t => t.UnitOfMeasureId).OnDelete(DeleteBehavior.ClientNoAction);

//            modelBuilder.Entity<UnitOfMeasure>().HasMany(e => e.UncertaintyLinearities).WithOne(E => E.CalibrationUncertaintyValueUncertaintyUnitOfMeasure).HasForeignKey(t => t.CalibrationUncertaintyValueUnitOfMeasureId).OnDelete(DeleteBehavior.ClientNoAction);


//            modelBuilder.Entity<WOD_Weight>()
//               .HasKey(bc => new { bc.WorkOrderDetailID, bc.WeightSetID });
//            modelBuilder.Entity<WOD_Weight>()
//                .HasOne(bc => bc.WeightSet)
//                .WithMany(b => b.WOD_Weights)
//                .HasForeignKey(bc => bc.WeightSetID);
//            modelBuilder.Entity<WOD_Weight>()
//                .HasOne(bc => bc.WorkOrderDetail)
//                .WithMany(c => c.WOD_Weights)
//                .HasForeignKey(bc => bc.WorkOrderDetailID);



//            modelBuilder.Entity<CalibrationSubType>()
//            .Property(et => et.CalibrationSubTypeId)
//            .ValueGeneratedNever();


//            modelBuilder.Entity<CalibrationSubType_Weight>()
//              .HasKey(bc => new { bc.WorkOrderDetailID, bc.WeightSetID, bc.CalibrationSubTypeID, bc.SecuenceID });
//            modelBuilder.Entity<CalibrationSubType_Weight>()
//                .HasOne(bc => bc.WeightSet)
//                .WithMany(b => b.CalibrationSubType_Weights)
//                .HasForeignKey(bc => bc.WeightSetID);
//            modelBuilder.Entity<CalibrationSubType_Weight>()
//                .HasOne(bc => bc.CalibrationSubType)
//                .WithMany(c => c.CalibrationSubType_Weights)
//                .HasForeignKey(bc => bc.CalibrationSubTypeID);



//            modelBuilder.Entity<WOD_TestPoint>()
//            .HasKey(bc => new { bc.WorkOrderDetailID, bc.TestPointID, bc.CalibrationSubTypeID, bc.SecuenceID });
//            modelBuilder.Entity<WOD_TestPoint>()
//                .HasOne(bc => bc.TestPoint)
//                .WithMany(b => b.WOD_TestPoints)
//                .HasForeignKey(bc => bc.TestPointID);
//            modelBuilder.Entity<WOD_TestPoint>()
//                .HasOne(bc => bc.WorkOrderDetail)
//                .WithMany(c => c.WOD_TestPoints)
//                .HasForeignKey(bc => bc.WorkOrderDetailID);



//            modelBuilder.Entity<WorkDetailHistory>()
//            .HasOne(a => a.WorkOrderDetail)
//            .WithMany(b => b.WorkDetailHistorys)
//            .HasForeignKey(d => d.WorkOrderDetailID)
//            .OnDelete(DeleteBehavior.ClientNoAction);


//            modelBuilder.Entity<WorkOrderDetail>()
//          .HasOne(a => a.Address)
//          .WithMany(b => b.WorkOrderDetails)
//          .HasForeignKey(d => d.AddressID)
//          .OnDelete(DeleteBehavior.ClientNoAction);


//            modelBuilder.Entity<TechnicianCode>().HasKey(e => new { e.Code, e.StateID, e.CertificationID });


//            //  modelBuilder.Entity<TechnicianCode>()
//            //.HasOne(a => a.User)
//            //.WithMany(b => b.TechnicianCodes)
//            //.HasForeignKey(d => d.TechnicianCodeID)
//            //.OnDelete(DeleteBehavior.ClientNoAction);



//            modelBuilder.Entity<User>()
//          .HasMany(a => a.TechnicianCodes)
//          .WithOne(b => b.User)
//          .HasForeignKey(d => d.UserID)
//          .OnDelete(DeleteBehavior.ClientNoAction);



//            // modelBuilder.Entity<TechnicianCode>()
//            //.HasOne(a => a.Certification)
//            //.WithMany(b => b.TechnicianCodes)
//            //.HasForeignKey(d => d.CertificationID)
//            //.OnDelete(DeleteBehavior.ClientNoAction);


//            modelBuilder.Entity<WorkOrderDetail>()
//           .HasOne(a => a.CalibrationType)
//           .WithMany(b => b.WorkOrderDetails)
//           .HasForeignKey(d => d.CalibrationTypeID)
//           .OnDelete(DeleteBehavior.ClientNoAction);


//            modelBuilder.Entity<PieceOfEquipment>()
//           .HasOne(a => a.UnitOfMeasure)
//           .WithMany(b => b.PieceOfEquipments)
//           .HasForeignKey(d => d.UnitOfMeasureID)
//           .OnDelete(DeleteBehavior.ClientNoAction);

//            modelBuilder
//           .Entity<PhoneNumber>()
//           .Property(x => x.Name)
//           .HasMaxLength(50)
//           .IsUnicode(false);

//            modelBuilder
//          .Entity<PhoneNumber>()
//          .Property(x => x.Number)
//          .HasMaxLength(50)
//         .IsUnicode(false);

//            modelBuilder
//       .Entity<PhoneNumber>()
//       .Property(x => x.TypeID)
//       .HasMaxLength(50)
//      .IsUnicode(false);

//            modelBuilder
//            .Entity<PhoneNumber>()
//            .Property(x => x.CountryID)
//            .HasMaxLength(50)
//            .IsUnicode(false);


//            modelBuilder
//          .Entity<Contact>()
//          .Property(x => x.Name)
//          .HasMaxLength(50)
//          .IsUnicode(false);

//            modelBuilder
//     .Entity<Contact>()
//     .Property(x => x.LastName)
//     .HasMaxLength(50)
//     .IsUnicode(false);

//            modelBuilder
//   .Entity<WorkOrderDetail>()
//   .Property(x => x.TechnicianComment)
//   .IsUnicode(false);

//            modelBuilder
//.Entity<WorkOrderDetail>()
//.Property(x => x.CertificateComment)
//.HasMaxLength(500)
//.IsUnicode(false);

//            modelBuilder
//           .Entity<PieceOfEquipment>()
//           .Property(x => x.PieceOfEquipmentID)
//           .HasMaxLength(500)
//           .IsUnicode(false);






//            modelBuilder
//.Entity<PieceOfEquipment>()
//.Property(x => x.IndicatorPieceOfEquipmentID)
//.HasMaxLength(500)
//.IsUnicode(false);

//            modelBuilder
//.Entity<PieceOfEquipment>()
//.Property(x => x.SerialNumber)
//.HasMaxLength(500)
//.IsUnicode(false);


//            modelBuilder
//.Entity<PieceOfEquipment>()
//.Property(x => x.InstallLocation)
//.HasMaxLength(500)
//.IsUnicode(false);

//            modelBuilder
//.Entity<PieceOfEquipment>()
//.Property(x => x.Class)
//.HasMaxLength(5)
//.IsUnicode(false);

//            modelBuilder
//.Entity<CertificatePoE>()
//.Property(x => x.PieceOfEquipmentID)
//.HasMaxLength(500)
//.IsUnicode(false);

//            modelBuilder
//.Entity<CertificatePoE>()
//.Property(x => x.Description)
//.HasMaxLength(500)
//.IsUnicode(false);
//            modelBuilder
//.Entity<CertificatePoE>()
//.Property(x => x.Name)
//.HasMaxLength(100)
//.IsUnicode(false);




//            modelBuilder
//.Entity<Customer>()
//.Property(x => x.Name)
//.HasMaxLength(100)
//.IsUnicode(false);

//            modelBuilder
//.Entity<Customer>().Property(x => x.Description)
//.HasMaxLength(500)
//.IsUnicode(false);


//            modelBuilder
//.Entity<UnitOfMeasure>().Property(x => x.Abbreviation)
//.HasMaxLength(10)
//.IsUnicode(false);

//            modelBuilder
//.Entity<UnitOfMeasure>().Property(x => x.Name)
//.HasMaxLength(50)
//.IsUnicode(false);

//            modelBuilder
//.Entity<UnitOfMeasure>().Property(x => x.Description)
//.HasMaxLength(200)
//.IsUnicode(false);

//            modelBuilder
//.Entity<PieceOfEquipment>().Property(x => x.OfflineID)
//.HasMaxLength(100)
//.IsUnicode(false);



//            modelBuilder
//           .Entity<WorkOrderDetail>().Property(x => x.OfflineID)
//           .HasMaxLength(100)
//           .IsUnicode(false);

//            modelBuilder
//.Entity<User_Rol>().Property(x => x.Permissions)
//.HasMaxLength(200)
//.IsUnicode(false);

//            modelBuilder
//.Entity<Rol>().Property(x => x.DefaultPermissions)
//.HasMaxLength(200)
//.IsUnicode(false);

//            modelBuilder
//.Entity<Rol>().Property(x => x.Description)
//.HasMaxLength(200)
//.IsUnicode(false);

//            modelBuilder
//           .Entity<Helpers.Controls.Component>().Property(x => x.Route)
//           .HasMaxLength(100)
//           .IsUnicode(false);


//            modelBuilder
//.Entity<Helpers.Controls.Component>().Property(x => x.Permission)
//.HasMaxLength(100)
//.IsUnicode(false);

//            modelBuilder
//.Entity<Helpers.Controls.Component>().Property(x => x.Name)
//.HasMaxLength(100)
//.IsUnicode(false);

//            modelBuilder
//.Entity<Helpers.Controls.Component>().Property(x => x.Group)
//.HasMaxLength(200)
//.IsUnicode(false);

//            modelBuilder
//           .Entity<PieceOfEquipment>().Property(x => x.CustomerToolId)
//           .HasMaxLength(200)
//           .IsUnicode(false);

//            modelBuilder
//.Entity<EquipmentTemplate>().Property(x => x.DeviceClass)
//.HasMaxLength(100)
//.IsUnicode(false);




//            modelBuilder.Entity<PieceOfEquipment>()
//          .HasOne(a => a.UnitOfMeasure)
//          .WithMany(b => b.PieceOfEquipments)
//          .HasForeignKey(d => d.UnitOfMeasureID)
//          .OnDelete(DeleteBehavior.ClientNoAction);


//            modelBuilder.Entity<POE_User>()
//        .HasKey(bc => new { bc.PieceOfEquipmentID, bc.UserID });
//            modelBuilder.Entity<POE_User>()
//                .HasOne(bc => bc.User)
//                .WithMany(b => b.POE_Users)
//                .HasForeignKey(bc => bc.UserID);
//            modelBuilder.Entity<POE_User>()
//                .HasOne(bc => bc.POE)
//                .WithMany(c => c.POE_Users)
//                .HasForeignKey(bc => bc.PieceOfEquipmentID);



//            modelBuilder.Entity<POE_POE>()
//      .HasKey(bc => new { bc.PieceOfEquipmentID, bc.PieceOfEquipmentID2 });
//            modelBuilder.Entity<POE_POE>()
//                .HasOne(bc => bc.POE)
//                .WithMany(b => b.POE_POE)
//                .HasForeignKey(bc => bc.PieceOfEquipmentID);
//            modelBuilder.Entity<POE_POE>()
//                .HasOne(bc => bc.POE)
//                .WithMany(c => c.POE_POE)
//                .HasForeignKey(bc => bc.PieceOfEquipmentID2);

//            modelBuilder.Entity<TechnicianCode>().Property(u => u.TechnicianCodeID).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);


//            modelBuilder
//    .Entity<WorkOrderDetailByStatus>()
//    .ToView(nameof(WorkOrderDetailByStatus))
//    .HasKey(t => t.WorkOrderDetailID);

//            modelBuilder
//.Entity<WorkOrderDetailByCustomer>()
//.ToView(nameof(WorkOrderDetailByCustomer))
//.HasKey(t => t.WorkOrderDetailID);


//            //modelBuilder.Entity<CustomClaim>().HasNoKey();


//            modelBuilder.Entity<CurrentUser>()
//            .HasMany(a => a.Claims)
//            .WithOne(b => b.User)
//            .HasForeignKey(d => d.CurrentUserID)
//             .OnDelete(DeleteBehavior.Cascade);


//            modelBuilder.Entity<PieceOfEquipment>().Ignore(e => e.Indicator);

//            modelBuilder.Entity<WorkOrder>().Ignore(e => e.Address);

//            modelBuilder.Entity<WorkOrderDetail>().Ignore(e => e.Address);


//            //modelBuilder.Entity<CustomerAggregate>().Ignore(e => e.Customer);

            
//            // modelBuilder.Entity<CustomerAggregate>().Ignore(e => e.Customer);
//            //modelBuilder.Entity<Address>().Ignore(e => e.CustomerAggregateAggregate);
//            // modelBuilder.Entity<Contact>().Ignore(e => e.CustomerAggregateAggregate);
//            // modelBuilder.Entity<PhoneNumber>().Ignore(e => e.CustomerAggregateAggregate);
//            // modelBuilder.Entity<Social>().Ignore(e => e.CustomerAggregateAggregate);

//             modelBuilder.Entity<TestPoint>().Ignore(e => e.TestPointGroup);

//             modelBuilder.Entity<BasicCalibrationResult>().Ignore(e => e.Eccentricity);

//             modelBuilder.Entity<BasicCalibrationResult>().Ignore(e => e.Repeatability);

//             modelBuilder.Entity<BasicCalibrationResult>().Ignore(e => e.Linearity);

//             modelBuilder.Entity<Eccentricity>().Ignore(e => e.BalanceAndScaleCalibration);

//             modelBuilder.Entity<Repeatability>().Ignore(e => e.BalanceAndScaleCalibration);

//             modelBuilder.Entity<Linearity>().Ignore(e => e.BalanceAndScaleCalibration);

//            modelBuilder.Entity<EquipmentCondition>().Ignore(e => e.WorkOrderDetail);

//            modelBuilder.Entity<EquipmentType>().Ignore(e => e.EquipmentTemplates);

//            modelBuilder.Entity<EquipmentType>().Ignore(e => e.PieceOfEquipment);

//            modelBuilder.Entity<WeightSet>().Ignore(e => e.WOD_Weights);

//            //modelBuilder.Entity<CustomerAggregate>().Ignore(e => e.Customer);

//            modelBuilder.Entity<User>().Ignore(e => e.WorkOrderDetails);

//            modelBuilder.Entity<User>().Ignore(e => e.WorkDetailHistories);

//            modelBuilder.Entity<CalibrationType>().Ignore(e => e.WorkOrderDetails);


//            base.OnModelCreating(modelBuilder);
//        }

//        protected override void OnConfiguring(DbContextOptionsBuilder options)
//        {
//            options.LogTo(Console.WriteLine, LogLevel.Error)
                    
//                   .EnableDetailedErrors()
//                   .EnableSensitiveDataLogging(true)
//                   .EnableServiceProviderCaching(true)
//                   .EnableThreadSafetyChecks(true).UseSqlite("Data Source=/database/app.db;Foreign Keys=False");
//        }

//        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
//        {
//            try
//            {
//                var result = await base.SaveChangesAsync(cancellationToken);
//                //#if RELEASE
//                await PersistDatabaseAsync(cancellationToken);
//                //#endif
//                return result;
//            }
//            catch (Exception ex)
//            {
//                if (ex.InnerException != null)
//                {
//                    Console.WriteLine("Error database " + ex.InnerException.Message);
//                }
//                Console.WriteLine("Error database " + ex.Message);

//                throw ex;

//            }
//            return 0;
//        }

//        private async Task PersistDatabaseAsync(CancellationToken cancellationToken = default)
//        {
//            Console.WriteLine("Start saving database");
//            var module = await _moduleTask.Value;



//            //var objRef = DotNetObjectReference.Create(options);
//            //_moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
//            //  "import", "./js/file.js").AsTask());





//            try
//            {
//                //var popperWrapper = await IJSRuntime.InvokeAsync<IJSInProcessObjectReference>("import", "./js/file3.js");
//                if (IJSRuntime != null)
//                {
//                    var obj = await module.InvokeAsync<dynamic>("syncDatabase", false);
//                }

//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }

//            //await module.InvokeVoidAsync("syncDatabase", false, cancellationToken);
//            Console.WriteLine("Finish save database");
//        }
//    }
}
