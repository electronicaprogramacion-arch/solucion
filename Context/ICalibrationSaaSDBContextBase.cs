using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Views;
using Helpers.Controls;
using Microsoft.EntityFrameworkCore;

namespace CalibrationSaaS.Data.EntityFramework
{
    public interface ICalibrationSaaSDBContextBase
    {
        //DbSet<Address> Address { get; set; }
        //DbSet<BalanceAndScaleCalibration> BalanceAndScaleCalibration { get; set; }
        //DbSet<BasicCalibrationResult> BasicCalibrationResult { get; set; }
        //DbSet<CalibrationSubType> CalibrationSubType { get; set; }
        //DbSet<CalibrationSubType_Weight> CalibrationSubType_Weight { get; set; }
        //DbSet<CalibrationType> CalibrationType { get; set; }
        //DbSet<Certificate> Certificate { get; set; }
        //DbSet<CertificatePoE> CertificatePoE { get; set; }
        //DbSet<Certification> Certification { get; set; }
        //DbSet<Component> Component { get; set; }
        //DbSet<Contact> Contact { get; set; }
        //DbSet<Customer> Customer { get; set; }
        //DbSet<CustomerAggregate> CustomerAggregates { get; set; }
        //DbSet<Eccentricity> Eccentricity { get; set; }
        //DbSet<EccentricityCalibrationResult> EccentricityCalibrationResult { get; set; }
        //DbSet<EmailAddress> EmailAddress { get; set; }
        //DbSet<EquipmentCondition> EquipmentCondition { get; set; }
        //DbSet<EquipmentTemplate> EquipmentTemplate { get; set; }
        //DbSet<EquipmentType> EquipmentType { get; set; }
        //DbSet<Linearity> Linearity { get; set; }
        //DbSet<Manufacturer> Manufacturer { get; set; }
        //DbSet<PhoneNumber> PhoneNumber { get; set; }
        //DbSet<PieceOfEquipment> PieceOfEquipment { get; set; }
        //DbSet<POE_POE> POE_POE { get; set; }
        //DbSet<POE_User> POE_User { get; set; }
        //DbSet<POE_WorkOrder> POE_WorkOrder { get; set; }
        //DbSet<RangeTolerance> RangeTolerance { get; set; }
        //DbSet<Repeatability> Repeatability { get; set; }
        //DbSet<RepeatibilityCalibrationResult> RepeatibilityCalibrationResult { get; set; }
        //DbSet<Rol> Rol { get; set; }
        //DbSet<Social> Social { get; set; }
        //DbSet<Status> Status { get; set; }
        //DbSet<TechnicianCode> TechnicianCode { get; set; }
        //DbSet<Tenant> Tenant { get; set; }
        //DbSet<TestPoint> TestPoint { get; set; }
        //DbSet<TestPointGroup> TestPointGroup { get; set; }
        //DbSet<UnitOfMeasure> UnitOfMeasure { get; set; }
        //DbSet<UnitOfMeasureType> UnitOfMeasureType { get; set; }
        //DbSet<User> User { get; set; }
        //DbSet<User_Rol> User_Rol { get; set; }
        //DbSet<User_WorkOrder> User_WorkOrder { get; set; }
        //DbSet<UserInformation> UserInformation { get; set; }
        //DbSet<WeightSet> WeightSet { get; set; }
        //DbSet<WOD_TestPoint> WOD_TestPoint { get; set; }
        //DbSet<WOD_Weight> WOD_Weight { get; set; }
        //DbSet<WorkDetailHistory> WorkDetailHistory { get; set; }
        //DbSet<WorkOrder> WorkOrder { get; set; }
        //DbSet<WorkOrderDetail> WorkOrderDetail { get; set; }
        //DbSet<WorkOrderDetailByCustomer> WorkOrderDetailByCustomer { get; set; }
        //DbSet<WorkOrderDetailByStatus> WorkOrderDetailByEquipment { get; set; }
        //DbSet<WorkOrderDetailByStatus> WorkOrderDetailByStatus { get; set; }

        // DbSet<Note> Note { get; set; }

        // DbSet<NoteWOD> NoteWOD { get; set; }

        // DbSet<Bogus.DynamicProperty> DynamicProperty { get; set; }

        // DbSet<Bogus.ViewPropertyBase> ViewPropertyBase { get; set; }

        // DbSet<GenericCalibration> GenericCalibration { get; set; }

        // DbSet<GenericCalibrationResult> GenericCalibrationResult { get; set; }

        // DbSet<CalibrationSubType_DynamicProperty> CalibrationSubType_DynamicProperty { get; set; }

        // DbSet<CalibrationSubType_ViewProperty> CalibrationSubType_ViewProperty { get; set; }

        // DbSet<CalibrationSubTypeView> CalibrationSubTypeView { get; set; }


        // DbSet<GenericCalibration2> GenericCalibration2 { get; set; }


        // DbSet<GenericCalibrationResult2> GenericCalibrationResult2 { get; set; }


        // DbSet<Mass> Mass { get; set; }

        // DbSet<Lenght> Lenght { get; set; }

        //DbSet<TestCode> TestCode { get; set; }

        //DbSet<Procedure> Procedure { get; set; }

        //DbSet<ExternalCondition> ExternalCondition { get; set; }

        //DbSet<Micro> Micro { get; set; }

        //DbSet<MicroResult> MicroResult { get; set; }

        //DbSet<WOD_Standard> WOD_Standard { get; set; }

        //DbSet<ForceResult> ForceResult { get; set; }

        //DbSet<Force> Force { get; set; }

        //DbSet<CalibrationSubType_Standard> CalibrationSubType_Standard { get; set; }

        //DbSet<POE_Scale> POE_Scale { get; set; }

        //DbSet<CalibrationResultContributor> CalibrationResultContributor { get; set; }

        //DbSet<Uncertainty> Uncertainty { get; set; }


        // DbSet<ToleranceType> ToleranceType { get; set; }
        

        DbSet<WOStatus> WOStatus { get; set; }

        DbSet<BalanceAndScaleCalibration> BalanceAndScaleCalibration { get; set; }
        DbSet<Customer> Customer { get; set; }

        DbSet<Eccentricity> Eccentricity { get; set; }
        DbSet<EquipmentCondition> EquipmentCondition { get; set; }
        DbSet<Linearity> Linearity { get; set; }
        DbSet<PieceOfEquipment> PieceOfEquipment { get; set; }
        DbSet<Repeatability> Repeatability { get; set; }
        DbSet<Tenant> Tenant { get; set; }
        DbSet<WorkOrder> WorkOrder { get; set; }
        DbSet<WorkOrderDetail> WorkOrderDetail { get; set; }
        DbSet<WorkDetailHistory> WorkDetailHistory { get; set; }


        DbSet<UserInformation> UserInformation { get; set; }
        DbSet<User> User { get; set; }

        DbSet<EquipmentType> EquipmentType { get; set; }

        DbSet<EquipmentTemplate> EquipmentTemplate { get; set; }

        DbSet<Manufacturer> Manufacturer { get; set; }
        DbSet<Procedure> Procedure { get; set; }
        DbSet<ProcedureEquipment> ProcedureEquipment { get; set; }

        DbSet<CustomerAggregate> CustomerAggregates { get; set; }

        DbSet<Contact> Contact { get; set; }

        DbSet<Address> Address { get; set; }

        DbSet<EmailAddress> EmailAddress { get; set; }



        DbSet<TestPointGroup> TestPointGroup { get; set; }


        DbSet<TestPoint> TestPoint { get; set; }

        DbSet<RangeTolerance> RangeTolerance { get; set; }


        DbSet<UnitOfMeasure> UnitOfMeasure { get; set; }
        DbSet<Status> Status { get; set; }

        DbSet<Rol> Rol { get; set; }

        DbSet<UnitOfMeasureType> UnitOfMeasureType { get; set; }

        DbSet<User_Rol> User_Rol { get; set; }

        DbSet<PhoneNumber> PhoneNumber { get; set; }

        public DbSet<POE_WorkOrder> POE_WorkOrder { get; set; }

        public DbSet<User_WorkOrder> User_WorkOrder { get; set; }

        public DbSet<RepeatibilityCalibrationResult> RepeatibilityCalibrationResult { get; set; }

        public DbSet<EccentricityCalibrationResult> EccentricityCalibrationResult { get; set; }

        public DbSet<BasicCalibrationResult> BasicCalibrationResult { get; set; }


        DbSet<WeightSet> WeightSet { get; set; }


        DbSet<WOD_Weight> WOD_Weight { get; set; }

   

        DbSet<WO_Weight> WO_Weight { get; set; }

        DbSet<Micro> Micro { get; set; }

        DbSet<MicroResult> MicroResult { get; set; }

        DbSet<WOD_Standard> WOD_Standard { get; set; }
        DbSet<WO_Standard> WO_Standard { get; set; }

        DbSet<WOD_TestPoint> WOD_TestPoint { get; set; }

        DbSet<CalibrationSubType_Weight> CalibrationSubType_Weight { get; set; }


        DbSet<CalibrationSubType> CalibrationSubType { get; set; }


        DbSet<Certification> Certification { get; set; }

        DbSet<TechnicianCode> TechnicianCode { get; set; }
        DbSet<WorkOrderDetailByStatus> WorkOrderDetailByStatus { get; set; }
        DbSet<WorkOrderDetailByStatus> WorkOrderDetailByEquipment { get; set; }
        DbSet<WorkOrderDetailByCustomer> WorkOrderDetailByCustomer { get; set; }


        DbSet<CalibrationType> CalibrationType { get; set; }

        DbSet<Social> Social { get; set; }

        DbSet<POE_User> POE_User { get; set; }

        DbSet<Certificate> Certificate { get; set; }

        DbSet<CertificatePoE> CertificatePoE { get; set; }


        DbSet<POE_POE> POE_POE { get; set; }

        DbSet<Component> Component { get; set; }


        DbSet<Force> Force { get; set; }

        DbSet<ForceResult> ForceResult { get; set; }

        DbSet<Uncertainty> Uncertainty { get; set; }

        DbSet<CalibrationResultContributor> CalibrationResultContributor { get; set; }

        DbSet<TestCode> TestCode { get; set; }

        DbSet<ToleranceType> ToleranceType { get; set; }


        //DbSet<ToleranceType_EquipmentType> ToleranceType_EquipmentType { get; set; }


        DbSet<Rockwell> Rockwell { get; set; }

        // Audit Logging
        DbSet<AuditLog> AuditLogs { get; set; }

        DbSet<RockwellResult> RockwellResult { get; set; }

        DbSet<POE_Scale> POE_Scale { get; set; }

        DbSet<ExternalCondition> ExternalCondition { get; set; }

        DbSet<CalibrationSubType_Standard> CalibrationSubType_Standard { get; set; }


        DbSet<Note> Note { get; set; }

        DbSet<NoteWOD> NoteWOD { get; set; }

        DbSet<Bogus.DynamicProperty> DynamicProperty { get; set; }

        DbSet<Bogus.ViewPropertyBase> ViewPropertyBase { get; set; }

        DbSet<GenericCalibration> GenericCalibration { get; set; }

        DbSet<GenericCalibrationResult> GenericCalibrationResult { get; set; }

        //DbSet<CalibrationSubType_DynamicProperty> CalibrationSubType_DynamicProperty { get; set; }

        //DbSet<CalibrationSubType_ViewProperty> CalibrationSubType_ViewProperty { get; set; }

        DbSet<CalibrationSubTypeView> CalibrationSubTypeView { get; set; }


        DbSet<GenericCalibration2> GenericCalibration2 { get; set; }


        DbSet<GenericCalibrationResult2> GenericCalibrationResult2 { get; set; }


        DbSet<Mass> Mass { get; set; }

        DbSet<Lenght> Lenght { get; set; }


        DbSet<EquipmentTypeGroup> EquipmentTypeGroup { get; set; }

        DbSet<WOD_Procedure> WOD_Procedure { get; set; }

        DbSet<WOD_ParametersTable> WOD_ParametersTable { get; set; }

        DbSet<CMCValues> CMCValues { get; set; }

        DbSet<CustomSequence> CustomSequence { get; set; }

        // Quotes Module entities
        DbSet<Quote> Quote { get; set; }
        DbSet<QuoteItem> QuoteItem { get; set; }
        DbSet<PriceType> PriceType { get; set; }
        DbSet<PriceTypePrice> PriceTypePrice { get; set; }

        // Equipment Recalls
        DbSet<CustomerAddress> CustomerAddress { get; set; }

    }
}