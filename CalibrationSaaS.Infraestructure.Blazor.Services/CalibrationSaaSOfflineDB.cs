using Blazor.IndexedDB.Framework;
using Helpers.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor
{
    public class CalibrationSaaSOfflineDB : IndexedDB.Blazor.IndexedDb
{
    public CalibrationSaaSOfflineDB(Microsoft.JSInterop.IJSRuntime jSRuntime, string name, int version) : base(jSRuntime, name, version) { }

    // These are like tables. Declare as many of them as you want.
    public IndexedSet<CalibrationSaaS.Domain.Aggregates.Entities.Customer> Customers { get; set; }

    public IndexedSet<CalibrationSaaS.Domain.Aggregates.Entities.WorkOrderDetail> WorkOrderDetail { get; set; }

    public IndexedSet<CalibrationSaaS.Domain.Aggregates.Entities.EquipmentType> EquipmentType { get; set; }

    public IndexedSet<CalibrationSaaS.Domain.Aggregates.Entities.Manufacturer> Manufacturer { get; set; }

    public IndexedSet<CalibrationSaaS.Domain.Aggregates.Entities.Status> Status { get; set; }
    public IndexedSet<CalibrationSaaS.Domain.Aggregates.Entities.User> User { get; set; }

    public IndexedSet<CalibrationSaaS.Domain.Aggregates.Entities.Certification> Certification { get; set; }


    public IndexedSet<CalibrationSaaS.Domain.Aggregates.Entities.CalibrationType> CalibrationType { get; set; }

    public IndexedSet<CalibrationSaaS.Domain.Aggregates.Entities.PieceOfEquipment> PieceOfEquipment { get; set; }



    public IndexedSet<CalibrationSaaS.Domain.Aggregates.Entities.Certificate> Certificate { get; set; }


    public IndexedSet<CalibrationSaaS.Domain.Aggregates.Entities.UnitOfMeasureType> UnitOfMeasureType { get; set; }


    public IndexedSet<CalibrationSaaS.Domain.Aggregates.Entities.UnitOfMeasure> UnitOfMeasure { get; set; }


    public IndexedSet<CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder> WorkOrder { get; set; }

    public IndexedSet<CalibrationSaaS.Domain.Aggregates.Entities.Address> Address { get; set; }

    public IndexedSet<CalibrationSaaS.Domain.Aggregates.Entities.CustomerAggregate> CustomerAggregate { get; set; }

    public IndexedSet<CalibrationSaaS.Domain.Aggregates.Entities.Customer> Customer { get; set; }

    public IndexedSet<CalibrationSaaS.Domain.Aggregates.Entities.EquipmentTemplate> EquipmentTemplate { get; set; }

        public IndexedSet<CalibrationSaaS.Domain.Aggregates.Entities.Rol> Roles { get; set; }

        public IndexedSet<CurrentUser> CurrentUser { get; set; }
    }
}
