using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Reports.Domain.ReportViewModels
{

    public class GenericReport
    {
        public GenericHeader Header { get; set; }
        [JsonProperty("Customer Instrument")]
        public GenericCustomerInstrument CustomerInstrument { get; set; }
        [JsonProperty("Data Grids")]
        public JObject DataGrids { get; set; }
        public JObject DataGridsAsFound { get; set; }
        public List<GenericStandard> Standards { get; set; }
        public List<GenericStatement> Statements { get; set; }
        [JsonProperty("Calibrated By")]
        public CalibratedBy CalibratedBy { get; set; }
        [JsonProperty("Approved By")]
        public ApprovedBy ApprovedBy { get; set; }
        public string Statementstring { get; set; }
        public string Gridstring { get; set; }
        public string GridstringAsFound { get; set; }
        public List<GenericStatementNoGrid> StatementsNoGrid { get; set; }
        public string StatementstringNoGrid { get; set; }
        public HeaderMaxPro HeaderMaxPro { get; set; }
       

    }

    public class gridNotes
    {
        public string Grid { get; set; }
        public string Statement { get; set; }
    }
    public class ReportGeneric
    {
        public GenericStandard[] StandardObjects { get; set; }
    }

    public class StandardObject
    {
        public GenericStandard[] StandardObjects { get; set; }
    }

    public class CalibrationResult
    {
        public Dictionary<string, object> ExtendedObject { get; set; }
    }

    public class GenericJson
    {
        
        public string Title { get; set; }
        public string Data { get; set; }
       
    }

    public class GenericHeader
    {
        [JsonProperty("Customer Order")]
        public string CustomerOrder { get; set; }
        [JsonProperty("Customer PO")]
        public string CustomerPO { get; set; }
        [JsonProperty("Ship Via")]
        public string ShipVia { get; set; }
        [JsonProperty("Customer Address 1")]
        public string CustomerAddress1 { get; set; }
        [JsonProperty("Customer Address 2")]
        public string CustomerAddress2 { get; set; }
        [JsonProperty("Customer Address State")]
        public string CustomerAddressState { get; set; }
        [JsonProperty("Customer Address City")]
        public string CustomerAddressCity { get; set; }
        [JsonProperty("Customer Address Country")]
        public string CustomerAddressCountry { get; set; }
        [JsonProperty("Ship To")]
        public string ShipTo { get; set; }

        ///Generic
        public string PieceOfEquipmentID { get; set; }
        public string TechnicianApproved { get; set; }
        public string TechnicianReview { get; set; }
        public string CalibrationDate { get; set; }
        public string CalibrationDueDate { get; set; }
    }



    public class GenericCustomerInstrument
    {

        public string Unit { get; set; }
        public string Accuracy { get; set; }
        [JsonProperty("Customer Ref")]
        public string CustomerRef { get; set; }
        [JsonProperty("Calibration Date")]
        public string CalibrationDate { get; set; }
        public string Description { get; set; }
        [JsonProperty("Calibration Due Date")]
        public string CalibrationDueDate { get; set; }
        public string Manufacturer { get; set; }
        public string Temperature { get; set; }
        public string Model { get; set; }
        public string Humidity { get; set; }
        public string RangeRes { get; set; }
        [JsonProperty("Received Condition")]
        public string ReceivedCondition { get; set; }
        public string Procedure { get; set; }
        [JsonProperty("Certificate Comments")]
        public string CertificateComments { get; set; }
        [JsonProperty("Accuracy Specification")]
        public string AccuracySpecification { get; set; }
        public string ReturnedCondition { get; set; }

        /// <summary>
        /// Generic 
        /// </summary>
        public string CertificateNumber { get; set; }
        public string InstallLocation { get; set; }
        public string Serial { get; set; }
        public string UoM { get; set; }
     
    }

    public class GenericStandard
    {
        [JsonProperty("Standard Id")]
        public string StandardId { get; set; }
        public string Description { get; set; }
        [JsonProperty("Calibration Date")]
        public string CalibrationDate { get; set; }
        [JsonProperty("Due Date")]
        public string DueDate { get; set; }
    }
    public class GenericStatement
    {
        
        public string Statement { get; set; }
        public string DataGrid { get; set; }
       

    }

    public class GenericStatementNoGrid
    {

        public string Statement { get; set; }
   
    }
    public class CalibratedBy
    {
        public string User { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        [JsonProperty("Certification Number")]
        public string CertificationNumber { get; set; }
        public string State { get; set; }

    }
    public class ApprovedBy
    {
        public string User { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        [JsonProperty("Certification Number")]
        public string CertificationNumber { get; set; }
        public string State { get; set; }

    }

    public class HeaderMaxPro
    {
        public string CalibrationLocation1 { get; set; }
        public string CalibrationLocation2 { get; set; }
        public string CalibrationLocation3 { get; set; }
        public string CalibrationLocation4 { get; set; }
        public string CalibrationLocation5 { get; set; }
        public string State { get; set; }
        public string CalibratedFor1 { get; set; }
        public string CalibratedFor2 { get; set; }
        public string CalibratedFor3 { get; set; }
        public string CalibratedFor4 { get; set; }
        public string CalibratedFor5 { get; set; }
        public string CalibrationCertificate { get; set; }
        public string CalibrationDate { get; set; }
        public string CalibrationDueDate { get; set; }
        public string UoM { get; set; }
        public string Range { get; set; }
        public string Accuracy { get; set; }
        public string Method { get; set; }
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string CalibratedFor { get; set; }
        public string MaxproControl { get; set; }
        public string CustomerPO { get; set; }
        public string CustomerToolId { get; set; }
        public string EquipmentType { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Serial { get; set; }
        public string ReceivedCondition { get; set; }
        public string ReturnedCondition { get; set; }
        public string Tolerance { get; set; }
        public string TechnicianApproved { get; set; }
        public string TechnicianReview { get; set; }
        public string WorkOrderDetailId { get; set; }
        public string Customer { get; set; }
        public string Description { get; set; }

    }
}


