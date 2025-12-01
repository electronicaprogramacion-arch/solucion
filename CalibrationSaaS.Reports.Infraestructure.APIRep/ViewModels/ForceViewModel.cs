using System;
using System.Collections.Generic;
using System.Linq;
using CalibrationSaaS.Domain.Aggregates.Entities;

namespace CalibrationSaaS.Reports.Infraestructure.APIRep.ViewModels
{
    public class ForceViewModel
    {
        public string? CreatedAt { get; set; }
        public string? Due { get; set; }
        public int Id { get; set; }
        public string? Address { get; set; }
        public string? Manufaturer { get; set; }
        public string? Model { get; set; }
        public string? CompanyName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string TemperatureInit { get; set; }
        public string TemperatureEnd { get; set; }
        public int CalibrationSubtypeId { get; set; }
        public ICollection<ForceItemViewModel> Items { get; set; }
        public ICollection<ForceItemViewModel> Tension { get; set; }
        public ICollection<ForceItemViewModel> Compression { get; set; }
        public ICollection<ForceItemViewModel> Universal { get; set; }
        public ICollection<PieceOfEquipment> standards{ get; set; }
        public Customer customer { get; set; }
        public PieceOfEquipment pieceOfEquipment { get; set; }
        public Address address { get; set; }
        public ICollection<WOD_Weight> weights { get; set; }
        public string Sign { get; set; }
        public string ShowUniversal { get; set; }
        public bool IncludeASTM { get; set; }
        public bool Accredited { get; set; }
        public List<StandardHeader> StandardHeaderList { get; set; }
        public bool ShowCompress { get; set; }
        public string CertificateComment { get; set; }
        public string Procedure { get; set; }
        public NoteViewModel NoteViewModel { get; set; }
        public double Capacity { get; set; }
        public string UoM { get; set; }
        public double Resolution { get; set; }
        public string Humidity { get; set; }
        public string Unit { get; set; }
        public string CustomerRef { get; set; }
        public string InstallLocation { get; set; }
        public string ReceivedCondition { get; set; }
        public string ReturnedCondition { get; set; }
        public string Serial { get; set; }
        public string CertificateNumber { get; set; }
        //public ICollection<NoteWOD> NotesWODList { get; set; }
        public string Sign2 { get; set; }
    }
      
    public class StandardHeader
    {
        public string PoE { get; set; }
        public string Serial { get; set; }
        public string Ref { get; set; }
        public string Value { get; set; }
        public string Type {get; set; }
        public string Note { get; set; }
        public string Distribution { get; set; }
        public string ActualValue { get; set; }
        public string Uncertainty { get; set; }
        public string CalibrationDueDate { get; set; }
        public string CalibrationDate { get; set; }
        public string EquipmentType { get; set; }
        public double Capacity { get; set; }
        public string UnitOfMeasure { get; set; }
        public string CalibrationProvider { get; set; }
        public string MaxRange { get; set; }
        public string MinRange { get; set; }
        public string Manufacturer { get; set; }
        public string Class { get; set; }

    }


    public class ForceItemViewModel
    {
        
        public double Nominal { get; set; }
        public string Run1 { get; set; }
        public string Run2 { get; set; }
        public string Run3 { get; set; }
        public string Run4 { get; set; }
        public double RelativeIndicationError { get; set; }
        public double RepeatabilitiIndicationError { get; set; }
        public double MaxError { get; set; }
        public string Class { get; set; }
        public string ClassRun1 { get; set; }
        public string ErrorRun1_ { get; set; }
        public string PassFail { get; set; }
        public string PassFailRun1 { get; set; }
        public double Uncertainty { get; set; }
        public double Adj { get; set; }
        public string ErrorRun2_ { get; set; }
        public string ErrorRun1 { get; set; }
        public string ErrorRun2 { get; set; }
        public string ErrorRun3 { get; set; }
        public string ErrorRun3_ { get; set; }
        public string ErrorRun4 { get; set; }
        public string ErrorRun4_ { get; set; }
        public double RepeatabilityASTM { get; set; }
        public double UncertaintyASTM { get; set; }
        public string StandardId { get; set; }
        public double TUR { get; set; }
        public double TURAstm { get; set; }


        public ForceItemViewModel(double nominal, string run1, string run2, string run3, string run4, 
            double relativeIndicationError, double repeatabilitiIndicationError, double maxError, string class_,
            string classRun1, string errorRun1_, string passFail, string passFailRun1, double uncertainty, double adj,
            string errorRun2_, string errorRun1, string errorRun2, string errorRun3, string errorRun3_, 
            string errorRun4, string errorRun4_, double repeatabilityASTM, double uncertaintyASTM, string standardId, double tur, double turAstm)
        {
            Nominal = nominal;
            Run1 = run1;
            Run2 = run2;
            Run3 = run3;
            Run4 = run4;
            RelativeIndicationError = relativeIndicationError;
            RepeatabilitiIndicationError = repeatabilitiIndicationError;
            MaxError = maxError;
            Class = class_;
            ClassRun1 = classRun1;
            ErrorRun1_ = errorRun1_;
            PassFail = passFail;
            PassFailRun1 = passFailRun1;
            Uncertainty = uncertainty;
            Adj = adj;
            ErrorRun2_ = errorRun2_;
            ErrorRun1 = errorRun1;
            ErrorRun2 = errorRun2;
            ErrorRun3 = errorRun3;
            ErrorRun3_ = errorRun3_;
            ErrorRun4 =  errorRun4;
            ErrorRun4_ = errorRun4_;
            RepeatabilityASTM = repeatabilityASTM;
            UncertaintyASTM = uncertaintyASTM;
            StandardId = standardId;
            TUR = tur;
            TURAstm = turAstm;

        }
    }
}