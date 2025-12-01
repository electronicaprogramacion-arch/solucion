using System;
using System.Collections.Generic;
using System.Text;

namespace Reports.Domain.ReportViewModels
{

    public class TruckWeigths
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
    }

        public class TrucKHeader
    {
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

        public string Capacity { get; set; }
        public string Resolution { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Humidity { get; set; }
        public string UoM { get; set; }
       

        public string Temperature { get; set; }
        public string? Due { get; set; }
        public string Url { get; set; }
        public string key { get; set; }
        public string Technician { get; set; }
        public string TechnicianAprove { get; set; }
        public string Unit { get; set; }
        public string CustomerTool { get; set; }
        public string InstallLocation { get; set; }
        public string ReceivedCondition { get; set; }
        public string ReturnedCondition { get; set; }

       
        public string CertficateComment { get; set; }


        public List<AsFoundLinearity> AsFoundLinearityList { get; set; }
        public AsFoundSectionsHeader AsFoundSectionsHeader { get; set; }
        public List<AsFoundSections> AsFoundSectionsList { get; set; }
        public AsFoundShiftHeader AsFoundShiftHeader { get; set; }
        public List<AsFoundShift> AsFoundShiftList { get; set; }
        public AsFoundStrain AsFoundStrain { get; set; }

       
        public bool IsAccredited { get; set; }
        public NoteScale NoteScale { get; set; }

        public Enviroment Environment { get; set; }
        
    }

    public class AsFoundLinearity
    {
        public string Standard { get; set; }
        public string Indication { get; set; }
        public string ToleranceRange { get; set; }
        public string PassFail { get; set; }

    }
    public class AsFoundSectionsHeader
    {
        public string Standard { get; set; }
        public string Pass1 { get; set; }
        public string Pass2 { get; set; }
       

    }
    public class AsFoundSections
    {
        public string Section { get; set; }
        public string Pass1 { get; set; }
        public string Pass2 { get; set; }


    }

    public class AsFoundShiftHeader
    {
        public string Standard { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }


    }
    public class AsFoundShift
    {
        public string Section { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }


    }

    public class AsFoundStrain
    {
        public string EmptyTruck { get; set; }
        public string TestWeightAdded { get; set; }
        public string TotalWeight { get; set; }
        public string IndicatedWeight { get; set; }
        public string Error1 { get; set; }
        public string TotalRange { get; set; }
        public string Deceasing { get; set; }
        public string Error2 { get; set; }
        public string ToleranceRange { get; set; }

    }



}


