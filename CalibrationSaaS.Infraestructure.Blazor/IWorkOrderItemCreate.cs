using Blazed.Controls;
using Blazor.IndexedDB.Framework;
using Blazored.Modal;
using Blazored.Modal.Services;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.Pages.AssetsBasics;
using Helpers.Controls;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Component = Helpers.Controls.Component;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Order
{
    public interface IWorkOrderItemCreate
    {

        ChangeEventArgs StandardsToAssing { get; set; } 

        int OptionID { get; set; }

        Func<dynamic, IAssetsServices<CallContext>> _assetsServices { get; set; }
        string _CustomerValue { get; set; }
        string _EquipmentTemplateID { get; set; }
        string _Manufacturer { get; set; }
        string _Model { get; set; }
        string _Name { get; set; }
        IReportService<CallContext> _reportService { get; set; }
        IEnumerable<Address> Addresses { get; set; }
        int AddressId { get; set; }
        string AddressStreet { get; set; }
        AppState AppStateBasics { get; set; }
        dynamic BasicInfo { get; set; }
        ICalibrationInstructionsLTIBase CalibrationInstructions { get; set; }
        dynamic CertificateCreate { get; set; }
        bool ChangeResolution { get; set; }
        dynamic Compresion { get; set; }
        string CustomerId { get; set; }
        string CustomerName { get; set; }
        string Description { get; set; }
        //EccentricityComponent EccentricityComponent { get; set; }
        bool EccentricityShow { get; set; }
        EditContext editContext { get; set; }
        dynamic EquipmentConditionComponent { get; set; }
        List<UnitOfMeasure> HumidityUnitofMeasureList { get; set; }
        dynamic Linearity { get; set; }

        List<dynamic> GridComponent{ get; set; }
        bool LinearityShow { get; set; }
        List<WorkOrderDetail> LIST { get; set; }
        List<WOD_Weight> lstWOD_Weight { get; set; }
        List<WOD_TestPoint> lstwt { get; set; }
        IModalService Modal { get; set; }
        //Modal ModalLEccentricity { get; set; }
        //Modal ModalLinearity { get; set; }
        //Modal ModalLRepeatibility { get; set; }
        EventCallback<ChangeEventArgs> OnChangeDescription { get; set; }
        ModalParameters parameters { get; set; }
        PieceOfEquipment PieceOfEquipment { get; set; }
        dynamic RepeabilityComponent { get; set; }
        bool RepeatibilityShow { get; set; }
        List<Status> StatusList { get; set; }
        List<UnitOfMeasure> TemperatureUnitofMeasureList { get; set; }
        Func<dynamic, IUOMService<CallContext>> UOM { get; set; }
        string UsersAccess { get; set; }
        string visibleModal { get; set; }
        dynamic WeightSetComponent { get; set; }
        List<WeightSet> WeightSetList2 { get; set; }

        Task ChangeDescripcion(ChangeEventArgs arg);
        Task ClearWeightSet(ChangeEventArgs arg);
        //Task CloseEccentricity();
        //Task CloseLinearity();
        //Task CloseRepeatibility();
        void CloseSetTestPoint(ChangeEventArgs arg);
        string convert(string item);
        Task<string> GetUrl(WorkOrderDetail wod);
        WorkOrderDetail LoadET(WorkOrderDetail we, EquipmentTemplate reset, bool? IsAcredited);
        Task LoadEviroment();
        Task LoadMeasurament();
        Task LoadWOD2();
        WorkOrderDetail Map(WorkOrderDetail we, Status status = null);
       
        Task SetCurrentStatus(Status status, bool OnlySave = false);
        Task ShowCertified();
        

        //Task ShowRepeatibility();
        //Task ShowEccentricity();
        //Task ShowLinearity();
        
        //Task ShowTests(string Param="");
        Task ShowTimeLine();
        void WT(WorkOrderDetail we);

        WorkOrderDetail eq { get; set; }

        ICollection<WorkOrderDetail> listWods { get; set; }

        bool Enabled { get; set; }


        //Task ShowCalibration(string Param="");


        Task Refresh();


        Component Component { get; set; }
    }
}