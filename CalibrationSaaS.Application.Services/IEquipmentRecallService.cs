using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Helpers.Controls.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace CalibrationSaaS.Application.Services
{
    [ServiceContract(Name = "CalibrationSaaS.Application.Services.EquipmentRecallService")]
    public interface IEquipmentRecallService<T>
    {
        /// <summary>
        /// Get equipment recalls based on filter criteria
        /// </summary>
        /// <param name="filter">Filter criteria including customer, date ranges</param>
        /// <param name="context">Call context</param>
        /// <returns>Collection of equipment recalls</returns>
        ValueTask<EquipmentRecallCollectionResult> GetEquipmentRecalls(EquipmentRecallFilter filter, T context);

        /// <summary>
        /// Get equipment recalls with pagination
        /// </summary>
        /// <param name="pagination">Pagination parameters with filter criteria</param>
        /// <param name="context">Call context</param>
        /// <returns>Paginated result set of equipment recalls</returns>
        ValueTask<ResultSet<EquipmentRecall>> GetEquipmentRecallsPaginated(Pagination<EquipmentRecall> pagination, T context);

        /// <summary>
        /// Export equipment recalls to Excel format
        /// </summary>
        /// <param name="filter">Filter criteria for export</param>
        /// <param name="context">Call context</param>
        /// <returns>Excel file as byte array wrapped in result object</returns>
        ValueTask<ExportResult> ExportEquipmentRecallsToExcel(EquipmentRecallFilter filter, T context);

        /// <summary>
        /// Get equipment recalls count for dashboard/summary purposes
        /// </summary>
        /// <param name="filter">Filter criteria</param>
        /// <param name="context">Call context</param>
        /// <returns>Count of equipment recalls matching criteria wrapped in result object</returns>
        ValueTask<CountResult> GetEquipmentRecallsCount(EquipmentRecallFilter filter, T context);

        /// <summary>
        /// Get overdue equipment count
        /// </summary>
        /// <param name="request">Customer filter request</param>
        /// <param name="context">Call context</param>
        /// <returns>Count of overdue equipment wrapped in result object</returns>
        ValueTask<CountResult> GetOverdueEquipmentCount(CustomerFilterRequest request, T context);
    }

    /// <summary>
    /// Result collection for equipment recalls
    /// </summary>
    [DataContract]
    public class EquipmentRecallCollectionResult
    {
        [DataMember(Order = 1)]
        public List<EquipmentRecall> EquipmentRecalls { get; set; } = new List<EquipmentRecall>();

        [DataMember(Order = 2)]
        public int TotalCount { get; set; }

        [DataMember(Order = 3)]
        public int OverdueCount { get; set; }

        [DataMember(Order = 4)]
        public bool Success { get; set; } = true;

        [DataMember(Order = 5)]
        public string Message { get; set; } = "";
    }

    /// <summary>
    /// Result wrapper for export operations
    /// </summary>
    [DataContract]
    public class ExportResult
    {
        [DataMember(Order = 1)]
        public byte[] Data { get; set; } = new byte[0];

        [DataMember(Order = 2)]
        public bool Success { get; set; } = true;

        [DataMember(Order = 3)]
        public string Message { get; set; } = "";

        [DataMember(Order = 4)]
        public string FileName { get; set; } = "";
    }

    /// <summary>
    /// Result wrapper for count operations
    /// </summary>
    [DataContract]
    public class CountResult
    {
        [DataMember(Order = 1)]
        public int Count { get; set; }

        [DataMember(Order = 2)]
        public bool Success { get; set; } = true;

        [DataMember(Order = 3)]
        public string Message { get; set; } = "";
    }

    /// <summary>
    /// Request wrapper for customer filter to avoid nullable primitive types in gRPC
    /// </summary>
    [DataContract]
    public class CustomerFilterRequest
    {
        [DataMember(Order = 1)]
        public int CustomerID { get; set; }

        [DataMember(Order = 2)]
        public bool HasCustomerFilter { get; set; }
    }
}
