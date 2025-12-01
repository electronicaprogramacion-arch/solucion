using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    /// <summary>
    /// DTO for Equipment Recalls reporting module
    /// This represents equipment due for calibration with all necessary information for display and Excel export
    /// </summary>
    [DataContract]
    public class EquipmentRecall
    {
        [DataMember(Order = 1)]
        public string PieceOfEquipmentID { get; set; }

        [DataMember(Order = 2)]
        public string SerialNumber { get; set; }

        [DataMember(Order = 3)]
        public DateTime CalibrationDate { get; set; }

        [DataMember(Order = 4)]
        public DateTime DueDate { get; set; }

        [DataMember(Order = 5)]
        public string CustomerName { get; set; }

        [DataMember(Order = 6)]
        public int CustomerID { get; set; }

        [DataMember(Order = 7)]
        public string Description { get; set; } // Equipment Template Name

        // Excel-only columns (empty for grid display)
        [DataMember(Order = 8)]
        public string Technician { get; set; } = "";

        [DataMember(Order = 9)]
        public string ReportNumber { get; set; } = "";

        [DataMember(Order = 10)]
        public string WorkOrderNumber { get; set; } = "";

        [DataMember(Order = 11)]
        public string PONumber { get; set; } = "";

        [DataMember(Order = 12)]
        public string QuoteNumber { get; set; } = "";

        [DataMember(Order = 13)]
        public string PTNumber { get; set; } = "";

        // Additional properties for filtering and display (computed properties - not serialized)
        public bool IsOverdue => DueDate < DateTime.Today;

        public int DaysUntilDue => (DueDate - DateTime.Today).Days;
    }

    /// <summary>
    /// Filter criteria for Equipment Recalls search
    /// </summary>
    [DataContract]
    public class EquipmentRecallFilter
    {
        [DataMember(Order = 1)]
        public int? CustomerID { get; set; }

        [DataMember(Order = 2)]
        public DateTime? InitialDueDate { get; set; }

        [DataMember(Order = 3)]
        public DateTime? FinalDueDate { get; set; }

        [DataMember(Order = 4)]
        public bool IncludeOverdue { get; set; } = true;

        [DataMember(Order = 5)]
        public string CustomerName { get; set; }

        [DataMember(Order = 6)]
        public int PageSize { get; set; } = 100;

        [DataMember(Order = 7)]
        public int PageNumber { get; set; } = 1;

        [DataMember(Order = 8)]
        public string SelectedEquipmentIdsString { get; set; } = "";

        // Helper property to work with List<string> in code but serialize as string
        public List<string> SelectedEquipmentIds
        {
            get => string.IsNullOrEmpty(SelectedEquipmentIdsString)
                ? new List<string>()
                : SelectedEquipmentIdsString.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            set => SelectedEquipmentIdsString = value != null ? string.Join(",", value) : "";
        }
    }
}
