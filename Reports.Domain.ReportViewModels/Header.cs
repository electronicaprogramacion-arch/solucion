using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Reports.Domain.ReportViewModels
{

    public class WeightSetHeader
    {
        public string PoE { get; set; }
        public string Serial { get; set; }
        public string Ref { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public string Note { get; set; }
        public string Distribution { get; set; }

        public string ActualValue { get; set; }

        public string Uncertainty { get; set; }

        public string CalibrationDueDate { get; set; }
        public string CalibrationDate { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        
    }

        public class Header
    {
        public string Accuracy { get; set; }
        public int CustomerId { get; set; }

        public int WorkOrderDetailId { get; set; }
        public string Client { get; set; }
        public string PieceOfEquipmentID { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string UnitNumber { get; set; }
        public string ServiceLocation { get; set; }
        public string EquipmentID { get; set; }
        public string EquipmentLocation { get; set; }
        public string EquipmentType { get; set; }
        public string TestingMethod { get; set; }

        public string CalID { get; set; }
        public string Location { get; set; }
        public string LastCalDate { get; set; }
        public string NextCalDate { get; set; }
        public string AsFoundResult { get; set; }
        public string AsLeftResult { get; set; }
        public string ManufacturerInd { get; set; }
        public string ModelInd { get; set; }
        public string SerialInd { get; set; }
        public string CapInd { get; set; }
        public string ManufacturerReceiv { get; set; }
        public string ModelIndReceiv { get; set; }
        public string SerialIndReceiv { get; set; }
        public string CapIndReceiv { get; set; }
        public string Class { get; set; }
        public string Type { get; set; }
        public string PlatformSize { get; set; }
        public string CalibrtionDate { get; set; }
        public string Enviroment { get; set; }
        public string CalibratedFor { get; set; }
        //Same Force       
        public string CertificateNumber { get; set; }
        public string CustomerAddress1 { get; set; }
        public string CustomerAddress2 { get; set; }
        public string CustomerAddressState { get; set; }
        public string CustomerAddressCity { get; set; }
        public string CustomerAddressCountry { get; set; }
        public string CustomerRef { get; set; }
        public string CalibrationDueDate { get; set; }
        public string Model { get; set; }
        public string RangeRes { get; set; }
        public string Manufacturer { get; set; }
        public string CertificateComments { get; set; }

        //ThermoTemp
        public string CompanyNo { get; set; }
        public string Description { get; set; }
        public string Serial { get; set; }
        public string Procedure { get; set; }
        public string CalDueDate { get; set; }
        public string ReportNumber { get; set; }
        public string PONumber { get; set; }
        public string CalibrationInterval { get; set; }
        public List<GenericStatementThermo> GenericStatementsThermo { get; set; }
        public string PhoneTechnicianNumber { get; set; }

       
        ///
        public string Capacity { get; set; }    
        public string Resolution { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Humidity { get; set; }
        public string UoM { get; set; }
        public string Repeteability { get; set; }
        public string Linearity { get; set; }
        public string Eccentricity { get; set; }
        public string EccentricityDet { get; set; }

        public string AsFound { get; set; }
        public string AsLeft { get; set; }

        public string Temperature { get; set; }
        public string UncertaintyBudget { get; set; }
        public string RepeteabBudget { get; set; }
        public string CornerloadBudget { get; set; }
        public string EnvironmetBudget { get; set; }
        public string ResolutionBudget { get; set; }
        public string? Due { get; set; }
        public string UncertaintyBudgetComp { get; set; }
        public string RepeteabBudgetComp { get; set; }
        public string CornerloadBudgetComp { get; set; }
        public string EnvironmetBudgetComp { get; set; }
        public string ResolutionBudgetComp { get; set; }
        public string Url { get; set; }
        public string key { get; set; }
        public string Technician { get; set; }
        public string TechnicianAprove { get; set; }
        public string Unit { get; set; }
        public string CustomerTool { get; set; }
        public string InstallLocation { get; set; }
        public string ReceivedCondition { get; set; }
        public string ReturnedCondition { get; set; }

        public double TotalAsFoundLin { get; set; }
        public double TotalAsFoundEcc { get; set; }
        public double TotalAsFoundRep { get; set; }
        public double TotalAsLeftRep { get; set; }
        public string CertficateComment { get; set; }
        public string CertificateId { get; set; }

        public List<Reports.Domain.ReportViewModels.Repeteab> RepeteabilityList { get; set; }

        public List<Reports.Domain.ReportViewModels.Excentricity> eccList { get; set; }

        public List<Reports.Domain.ReportViewModels.Lineartiy> linList { get; set; }

        public List<Reports.Domain.ReportViewModels.ExcentricityDet> excentriDetList { get; set; }

        public List<AsFound> AsFoundList { get; set; }

        public List<AsLeft> AsLeftList { get; set; }

        public string Tolerance { get; set; }

        public List<WeightSetHeader> WeightSetList { get; set; }


        public bool IsAccredited { get; set; }
        public NoteScale NoteScale { get; set; }

        public Enviroment Environment { get; set; }

        //New ReportBitterman
        public string StDevRepeatAsFound { get; set; }
        public string StDevRepeatAsLeft { get; set; }
        public string MaxMinAsFoundtEcc { get; set; }
        public string ToleranceAsFoundtEcc { get; set; }
        public string MaxMinAsLeftEcc { get; set; }
        public string ToleranceAsLeftEcc { get; set; }
        
        /// <summary>
        /// Truc report
        /// </summary>

        public List<LinearityTruck> LinearityTruckList { get; set; }
        public AsFoundSectionHeader AsFoundSectionsHeader { get; set; }
        public List<AsFoundSection> AsFoundSectionsList { get; set; }
       
        public Strain Strain { get; set; }

        public AsLeftSectionsHeader AsLeftSectionsHeader { get; set; }
        public List<AsLeftSections> AsLeftSectionsList { get; set; }

        

        public CommentAsFound  CommentAsFound { get; set; }
        public CommentAsLeft CommentAsLeft { get; set; }
        public TotalsAsFound TotalAsFound { get; set; }
        public TotalsAsLeft TotalAsLeft { get; set; }


        ///ThermoTemp
        ///
        public List<TestPointsThermoTemp> TestPointsThermoTemps { get; set; }
        public List<ParentAndChildren> ParentAndChildren { get; set; }

        ///Advance
        /// <summary>
        /// Advance
        /// </summary>
        /// 
        public string CalibrationLocation1 { get; set; }
        public string CalibrationLocation2 { get; set; }
        public string CalibrationLocation3 { get; set; }
        public string CalibrationLocation4 { get; set; }
        public string CalibrationLocation5 { get; set; }
        public string CalibratedFor1 { get; set; }
        public string CalibratedFor2 { get; set; }
        public string CalibratedFor3 { get; set; }
        public string CalibratedFor4 { get; set; }
        public string CalibratedFor5 { get; set; }
        public string CompanyName { get; set; }
        public string CalibrationCertificate { get; set; }
        public int level { get; set; }
        public string WorkOrderId { get; set; }
        public List<Advance> AdvanceWods { get; set; }
        public List<TestPointsAdvance> TestPointsAdvances { get; set; }
        public List<GenericStatementWOD> GenericStatementsWOD { get; set; }
        public TotalsRepeatabilityAdvance totalsRepeatabilityAdvance { get; set; }

        ///Truck Scale Advance <summary>
        /// Truck Scale Advance
        /// </summary>
        /// 
        public HeaderTruckScaleAdvance HeaderTruckScaleAdvance { get; set; }
        public List<SectionTruckAdvance> SectionsTruckAdvance { get; set; } //2
        public List<CertifiedWeight> CertifiedWeights { get; set; }  //4 Sections and SectionsAndShiftAsLeft
        public List<SideToSideTest> SideToSidesTest { get; set; }  //3
        public List<StrainTest> StrainTests { get; set; } //5
        public List<TestValue> TestValues { get; set; }  //6
       

        ///USP41
        ///
        public USP41 USP41 { get; set; }
    }
    
    #region TruckScaleAdvance

    /// <summary>
    /// 
    /// </summary>
    public class HeaderTruckScaleAdvance
    {
        public string CalibrationLocation { get; set; }
        public string CalibrationCertificate { get; set; }
        public string CalibrationDate { get; set; }
        public string PlatformManufacturer { get; set; }
        public string PlatformModel { get; set; }
        public string PlatformSerial { get; set; }
        public string Capacity { get; set; }
        public string TypeOfDeck { get; set; }
        public string CLC { get; set; }
        public string CalibratedFor { get; set; }
        public string CustomerID { get; set; }
        public string CalibrationDueDate { get; set; }
        public string IndicatorManufacturer { get; set; }
        public string IndicatorModel { get; set; }
        public string IndicatorSerial { get; set; }
        public string Division { get; set; }
        public string NumberSections { get; set; }
        public string PlatformSize { get; set; }
        public string LeadWireSeal { get; set; }
        public string ScaleDescription { get; set; }
        public string ScoreBoard { get; set; }
        public string LocalRemote { get; set; }
        public string Printer { get; set; }
        public string Other { get; set; }
        public string Technician { get; set; }
        public string TechnicianAprove { get; set; }

    }

    public class SectionTruckAdvance
    {
        public string Section { get; set; }
        public string TestWeight { get; set; }
        public string AsFound { get; set; }
        public string AsLeft { get; set; }
    }


    public class CertifiedWeight
    {
        public string Section { get; set; }
        public string TestWeight { get; set; }
        public string AsFound { get; set; }
        public string AsLeft { get; set; }
    }
    public class SideToSideTest
    {
        public string Section { get; set; }
        public string TestWeight { get; set; }
        public string AsFound { get; set; }
        public string AsLeft { get; set; }
    }
    public class StrainTest
    {
        public string Name  { get; set; }
        public string Value { get; set; }
             
    }

    public class TestValue
    {
        public string AsFound { get; set; }
        public string AsLeft { get; set; }
       
    }

    #endregion TruckScaleAdvance

    public class GenericStatementThermo
    {

        public string Statement { get; set; }
        public string DataGrid { get; set; }


    }
    public class GenericStatementWOD
    {

        public string Statement { get; set; }
        public string DataGrid { get; set; }


    }
    public class LinearityTruck
    {
        public string Description { get; set; }
        public string Weight { get; set; }
        public string WeightApplied { get; set; }
        public string StepResol { get; set; }
        public string UoM { get; set; }
        public string AsFound { get; set; }
        public string ResultAsFound { get; set; }
        public string AsLeft { get; set; }
        public string ResultAsLeft { get; set; }
        public string ToleranceMin { get; set; }
        public string ToleranceMax { get; set; }

    }
    public class AsFoundSectionHeader
    {
        public string Pass1AsFoundHeader     { get; set; }
        public string Pass2AsFoundHeader { get; set; }
        public string LeftAsFoundHeader { get; set; }
        public string RightAsFoundHeader { get; set; }
        public string TolerancePass1AsFound { get; set; }
        public string TolerancePass2AsFound { get; set; }
        public string ToleranceLeftAsFound { get; set; }
        public string ToleranceRightAsFound { get; set; }

    }
    public class AsFoundSection
    {
        public string Section { get; set; }
        public string Pass1 { get; set; }
        public string Pass2 { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }

    }

    public class Strain
    {
        public string UoM { get; set; }
        public string EmptyTruck { get; set; }
        public string TestWeightAdded { get; set; }
        public string TotalWeight { get; set; }
        public string IndicatedWeight { get; set; }
        public string Error1 { get; set; }
        public string TotalRange { get; set; }
        public string Decreasing { get; set; }
        public string Error2 { get; set; }
        public string ToleranceRange { get; set; }
        public string ToleranceRange2 { get; set; }

    }


    public class AsLeftSectionsHeader
    {
        public string Pass1AsLeftHeader { get; set; }
        public string Pass2AsLeftHeader { get; set; }
        public string LeftAsLeftHeader { get; set; }
        public string RightAsLeftHeader { get; set; }
        public string TolerancePass1AsLeft { get; set; }
        public string TolerancePass2AsLeft { get; set; }
        public string ToleranceLeftAsLeft { get; set; }
        public string ToleranceRightAsLeft { get; set; }

    }
    public class AsLeftSections
    {
        public string Section { get; set; }
        public string Pass1 { get; set; }
        public string Pass2 { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }

    }

    
    public class CommentAsFound
    {
        public string Comment { get; set; }
        


    }
    public class CommentAsLeft
    {
        public string Comment{ get; set; }

    }

    public class TestPointsThermoTemp
    {
        public string FunctionTested { get; set; }
        public string Nominal { get; set; }
        public string AsFound { get; set; }
        public string OutOfTol1 { get; set; }
        public string AsLeft { get; set; }
        public string OutOfTol2 { get; set; }
        public string CalibrationTolerance { get; set; }
        public string Uncertainties { get; set; }
        public string UoM { get; set; }
        public string GroupName { get; set; }
    }

    public class TotalsRepeatabilityAdvance
    {
        public string StdevAsFound { get; set; }
        public string StdevAsLeft { get; set; }
     
    }

    public class TotalsAsFound
    {
        public string MinPass1 { get; set; }
        public string MinPass2 { get; set; }
        public string MinLeft { get; set; }
        public string MinRight { get; set; }
        public string MaxPass1 { get; set; }
        public string MaxPass2 { get; set; }
        public string MaxLeft { get; set; }
        public string MaxRight { get; set; }
    }
    public class TotalsAsLeft
    {
        public string MinPass1 { get; set; }
        public string MinPass2 { get; set; }
        public string MinLeft { get; set; }
        public string MinRight { get; set; }
        public string MaxPass1 { get; set; }
        public string MaxPass2 { get; set; }
        public string MaxLeft { get; set; }
        public string MaxRight { get; set; }
    }

    public class Advance
    {
        public string Scale { get; set; }
        public string Location { get; set; }
        public string Make { get; set; } 
        public string Model { get; set; }    
        public string SerialNumber { get; set; }
        public string EquipmentType { get; set; }
        public string UOM { get; set; }
        public string AppliedWT { get; set; }
        public string AsFound { get; set; }
        public string AsLeft { get; set; }
        public string Act { get; set; }
        public List<TestPointsAdvance> TestPointsAdvances { get; set; }

    }

    public class TestPointsAdvance
    {
        public string WeightApplied { get; set; }
        public string AsFound { get; set; }
        public string AsLeft { get; set; }
        public string Act1 { get; set; }
        public string Act2 { get; set; }
        public string Act3 { get; set; }
    }


    ///Child ThermoTemp
    ///
    public class ParentAndChildren
    {
        public string ID { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public List<TestPointsThermoTemp> TestPointsThermoTemps { get; set; }

    }

    public class USP41
    {
        public string RepeatabilityPassCriteria { get; set; }
        public string AsLeftStandardDeviation { get; set; }
        public string Resolution { get; set; }
        public string MaxValue { get; set; }
        public string MinimumWeight { get; set; }

    }

}


