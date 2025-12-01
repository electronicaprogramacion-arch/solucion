

using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;

using CalibrationSaaS.Domain.Aggregates.Shared.Basic;


using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ProtoBuf.Grpc;



namespace BlazorApp1.Blazor.Blazor
{
    public interface ICalibrationInstructionsLTIBase
    {
        List<CalibrationType> _calibrationTypeList { get; set; }
        
        AppState AppStateBasics { get; set; }
        ICollection<Certification> Certifications { get; set; }
         bool changeTolerance { get; set; }
        Func<dynamic, IAssetsServices<CallContext>> Client { get; set; }
        dynamic DbFactory { get; set; }
        EditContext editContext { get; set; }
        bool Enabled { get; set; }
        WorkOrderDetail eq { get; set; }
        EquipmentTemplate EquipmentTemplate { get; set; }
        bool LabelDown { get; set; }
        Blazed.Controls.Modal ModalResolution { get; set; }
        Dictionary<int, string> Modes { get; set; }
        PieceOfEquipment PieceOfEquipment { get; set; }
        BlazorApp1.Blazor.Pages.Basics.TestPoints.RangeTestPoint RangeAccuracy { get; set; }
        BlazorApp1.Blazor.Pages.Basics.TestPoints.RangeTestPoint RangeComponent { get; set; }
        Func<Task> RefreshParent { get; set; }
        BlazorApp1.Blazor.Blazor.Pages.Basics.ResolutionComponent ResolutionComponent { get; set; }
        bool ResolutionShow { get; set; }
        Func<dynamic, IBasicsServices<CallContext>> Service { get; set; }
        bool ShowTech { get; set; }
        List<User> Technicians { get; set; }
        List<UnitOfMeasure> UnitofMeasureList { get; set; }
        string UsersAccess { get; set; }
        BlazorApp1.Blazor.Blazor.Pages.Order.IWorkOrderItemCreate WorkOrderItemCreate { get; set; }

        string BoundValue(WorkOrderDetail eq);
        Task ChangeCalibrationType(ChangeEventArgs args);
        void ChangeDate(ChangeEventArgs args);
        void ChangeList(ChangeEventArgs args);
        Task ChangeSwitch(ChangeEventArgs e);
        Task CloseWindow();
        void LoadConditionData();
        void LoadConditionData(WorkOrderDetail eq);
        Task Refresh();
        void SelectionChanged(ChangeEventArgs args);
        Task ShowResolution();
        string ToleranceTypeChanged(WorkOrderDetail args);
    }
}