using CalibrationSaaS.Domain.Aggregates.Entities;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.Services
{
    [ServiceContract]
    public interface IMetCalIntegrationService<TCallContext>
    {
        [OperationContract]
        ValueTask<MetCalImportResult> ImportElectricalCalibrationData(MetCalImportRequest request, TCallContext context);

        [OperationContract]
        ValueTask<MetCalConnectionResult> TestConnection(MetCalConnectionRequest request, TCallContext context);

        [OperationContract]
        ValueTask<MetCalProcedureListResult> GetAvailableProcedures(MetCalConnectionRequest request, TCallContext context);
    }

    [DataContract]
    public class MetCalImportRequest
    {
        [DataMember(Order = 1)]
        public int WorkOrderDetailId { get; set; }

        [DataMember(Order = 2)]
        public string ProcedureName { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public string MetCalServerUrl { get; set; } = string.Empty;

        [DataMember(Order = 4)]
        public string Username { get; set; } = string.Empty;

        [DataMember(Order = 5)]
        public string Password { get; set; } = string.Empty;

        [DataMember(Order = 6)]
        public bool OverwriteExisting { get; set; }
    }

    [DataContract]
    public class MetCalConnectionRequest
    {
        [DataMember(Order = 1)]
        public string MetCalServerUrl { get; set; } = string.Empty;

        [DataMember(Order = 2)]
        public string Username { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public string Password { get; set; } = string.Empty;
    }

    [DataContract]
    public class MetCalImportResult
    {
        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [DataMember(Order = 2)]
        public string ErrorMessage { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public int ImportedRecordsCount { get; set; }

        [DataMember(Order = 4)]
        public List<string> ImportedTestPoints { get; set; } = new();

        [DataMember(Order = 5)]
        public List<string> Warnings { get; set; } = new();

        [DataMember(Order = 6)]
        public string ImportSummary { get; set; } = string.Empty;
    }

    [DataContract]
    public class MetCalConnectionResult
    {
        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [DataMember(Order = 2)]
        public string ErrorMessage { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public string ServerVersion { get; set; } = string.Empty;

        [DataMember(Order = 4)]
        public bool IsElectricalCalibrationSupported { get; set; }
    }

    [DataContract]
    public class MetCalProcedureListResult
    {
        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [DataMember(Order = 2)]
        public string ErrorMessage { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public List<MetCalProcedure> Procedures { get; set; } = new();
    }

    [DataContract]
    public class MetCalProcedure
    {
        [DataMember(Order = 1)]
        public string Name { get; set; } = string.Empty;

        [DataMember(Order = 2)]
        public string Description { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public string Type { get; set; } = string.Empty;

        [DataMember(Order = 4)]
        public bool IsElectrical { get; set; }

        [DataMember(Order = 5)]
        public List<string> SupportedUnits { get; set; } = new();
    }

    [DataContract]
    public class MetCalTestPoint
    {
        [DataMember(Order = 1)]
        public string TestPointName { get; set; } = string.Empty;

        [DataMember(Order = 2)]
        public double NominalValue { get; set; }

        [DataMember(Order = 3)]
        public double AsFoundValue { get; set; }

        [DataMember(Order = 4)]
        public double AsLeftValue { get; set; }

        [DataMember(Order = 5)]
        public string Unit { get; set; } = string.Empty;

        [DataMember(Order = 6)]
        public double Uncertainty { get; set; }

        [DataMember(Order = 7)]
        public string Status { get; set; } = string.Empty;

        [DataMember(Order = 8)]
        public string Comments { get; set; } = string.Empty;

        [DataMember(Order = 9)]
        public string ReferenceStandard { get; set; } = string.Empty;

        [DataMember(Order = 10)]
        public double Temperature { get; set; }

        [DataMember(Order = 11)]
        public double Humidity { get; set; }

        [DataMember(Order = 12)]
        public string TestSequence { get; set; } = string.Empty;

        [DataMember(Order = 13)]
        public int TestPointNumber { get; set; }
    }

    [DataContract]
    public class MetCalApiResponse
    {
        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [DataMember(Order = 2)]
        public string ErrorMessage { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public string Data { get; set; } = string.Empty;

        [DataMember(Order = 4)]
        public int StatusCode { get; set; }
    }
}
