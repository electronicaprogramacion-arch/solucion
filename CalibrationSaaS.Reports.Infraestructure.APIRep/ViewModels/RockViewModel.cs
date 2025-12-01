using System;
using System.Collections.Generic;
using System.Linq;
using CalibrationSaaS.Domain.Aggregates.Entities;

namespace CalibrationSaaS.Reports.Infraestructure.APIRep.ViewModels
{
    public class RockViewModel
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
        public ICollection<RockItemViewModel> AsFounds { get; set; }
        public ICollection<RockItemViewModel> AsLefts { get; set; }
        public NoteViewModel NoteViewModel { get; set; }
        public ICollection<PieceOfEquipment> standards { get; set; }
        public List<StandardHeaderRock> StandardHeaderRockList { get; set; }
        public bool Accredited { get; set; }
        public PieceOfEquipment pieceOfEquipment { get; set; }
        public Address address { get; set; }
        public Customer customer { get; set; }
        public string Sign { get; set; }
        public bool isISO { get; set; }
        public string CertificateComment { get; set; }
        public string Procedure { get; set; }
        public double Capacity { get; set; }
        public string UoM { get; set; }
        public double Resolution { get; set; }
        public string Unit { get; set; }
        public string CustomerTool { get; set; }
        public string InstallLocation { get; set; }
        public string ReceivedCondition { get; set; }
        public string ReturnedCondition { get; set; }

    }
      
   
	
    public class RockItemViewModel
    {

        public string ScaleRange { get; set; }
        public string Standard { get; set; }
        public double Average { get; set; }
        public double Test1 { get; set; }
        public double Test2 { get; set; }
        public double Test3 { get; set; }
        public double Test4 { get; set; }
        public double Test5 { get; set; }
        public double Repeatability { get; set; }
        public double Error { get; set; }
        public double Uncertainty { get; set; }

        public RockItemViewModel(string scaleRange, string standard, double average, double test1, double test2,
                                 double test3, double test4, double test5, double repeatability, double error, double uncertainty)

        {
            ScaleRange = scaleRange;
            Standard = standard;
            Average = average;
            Test1 = test1;
            Test2 = test2;
            Test3 = test3;
            Test4 = test4;
            Test5 = test5;
            Repeatability = repeatability;
            Error = error;
            Uncertainty = uncertainty;  
        
        }
    }

    public class StandardHeaderRock
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
        public string EquipmentType { get; set; }
        public double Capacity { get; set; }
        public string UnitOfMeasure { get; set; }


    }

}