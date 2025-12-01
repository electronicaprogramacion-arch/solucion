using Blazed.Controls;
using Blazed.Controls.MultiComponent;
using Blazed.Controls.Toast;
using Blazor.IndexedDB.Framework;
using BlazorApp1.Blazor.Blazor;
using BlazorApp1.Blazor.Blazor.Pages.Order;
using BlazorApp1.Blazor.Shared;

using Blazored.Modal;
using Blazored.Modal.Services;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Castle.Components.DictionaryAdapter.Xml;
using Grpc.Core;
using Helpers;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using Helpers.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using static CalibrationSaaS.Domain.Aggregates.Querys.Querys;

//using WeightSetComponent = CalibrationSaaS.Infraestructure.Blazor.Shared.WeightSetComponent;

namespace BlazorApp1.Blazor.Pages.Order
{
    public class WorkOrderItemCreateBase :
        Base_Create<CalibrationSaaS.Domain.Aggregates.Entities.WorkOrderDetail,
            Func<dynamic, CalibrationSaaS.Application.Services.IWorkOrderDetailServices<CallContext>>, CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany>, BlazorApp1.Blazor.Blazor.Pages.Order.IWorkOrderItemCreate
    {

        public ChangeEventArgs StandardsToAssing { get; set; } = new ChangeEventArgs();


        public BlazorApp1.Blazor.Pages.Order.WorkOrderDetailChildren_Search WorkOrderDetailSearch
            = new  BlazorApp1.Blazor.Pages.Order.WorkOrderDetailChildren_Search();


        public List<WorkOrderDetail> listWorkOrderDetailchildren { get; set; } = new List<WorkOrderDetail>();
        public EquipmentType EquipmentTypeObject { get; set; }

        public List<PieceOfEquipment> _listPoEDueDate = new List<PieceOfEquipment>();
        public string Customer { get; set; }
        public List<dynamic> weigset { get; set; } = new();
        [Inject] public Func<dynamic,CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext>> _poeServices { get; set; }
        public dynamic WeightSetComponent
        

        {
            
            get
            {

                return weigset; 
            }

            set
            {
                

                bool inc = false;
                dynamic itemtmp = null;
                foreach (var item in weigset)
                {
                    if (item.ID == value.ID)
                    {
                        inc = true;
                        itemtmp = item;
                    }
                }
                if (itemtmp != null)
                {
                    weigset.Remove(itemtmp);
                }
                weigset.Add(value);

            }




        }


        public CalibrationTypeComponent<WorkOrderDetail, GenericCalibration2, GenericCalibrationResult2> CalibrationTypeComponent { get; set; }

        public int OptionID { get; set; }


         [Parameter]
        public string StandardSectionTitle { get; set; } = "Standards";

        [Parameter]
        public string AddWeightSets { get; set; } = "Add Standard";

         
        public List<CalibrationSubType> _calibrationSubtypeList = new List<CalibrationSubType>();
        public CalibrationSubType datagrid = new CalibrationSubType();

        [Parameter]
        public string ClearWeightSets { get; set; } = "Clear All";
        [Inject] public CalibrationSaaS.Domain.Aggregates.Shared.Basic.AppState AppStateBasics { get; set; }
        [Inject] public Func<dynamic, CalibrationSaaS.Application.Services.IUOMService<CallContext>> UOM { get; set; }
        [Inject] public CalibrationSaaS.Application.Services.IReportService<CallContext> _reportService { get; set; }

#pragma warning disable CS0108 
        [Inject] ILogger<WorkOrderItemCreateBase> Logger { get; set; }
#pragma warning restore CS0108 

        public ICollection<Certificate> _listCetificate = new List<Certificate>();

        [Inject]
        public Func<dynamic, CalibrationSaaS.Application.Services.IAssetsServices<CallContext>> _assetsServices { get; set; }

        
        //[Inject]
        //public CalibrationSaaS.Application.Services.IBasicsServices<CallContext> _basicsServices { get; set; }


        [Inject] public Func<dynamic, CalibrationSaaS.Application.Services.IBasicsServices<CallContext>> _basicsServices { get; set; }


        public List<CalibrationSaaS.Domain.Aggregates.Entities.Status> StatusList { get; set; }


        public List<WeightSet> WeightSetList2 { get; set; }

        //public dynamic WeightSetComponent { get; set; } 


        public dynamic EccentricityComponent { get; set; }

        public dynamic RepeabilityComponent { get; set; }
       
       

        public dynamic EquipmentConditionComponent { get; set; }


        public dynamic BasicInfo { get; set; }

        public BlazorApp1.Blazor.Blazor.ICalibrationInstructionsLTIBase CalibrationInstructions { get; set; }


        public dynamic Linearity { get; set; }

        public dynamic Compresion { get; set; }


        public List<dynamic> GridComponent{ get; set; }

        public List<WorkOrderDetail> LIST { get; set; } = new List<WorkOrderDetail>();

        public EditContext editContext { get; set; }

        public PieceOfEquipment PieceOfEquipment { get; set; } = new PieceOfEquipment();

        [CascadingParameter] public IModalService Modal { get; set; }


        [Parameter]
        public string CustomerId { get; set; }
        [Parameter]
        public string CustomerName { get; set; }
        [Parameter]
        public int AddressId { get; set; }

        [Parameter]
        public string AddressStreet { get; set; }

        public string _CustomerValue { get; set; }

        public dynamic CertificateCreate { get; set; }

        public string _message;

        public string _Model { get; set; }
        public string _Manufacturer { get; set; }
        public string _Name { get; set; }

        [Parameter]
        public bool ChangeResolution { get; set; }

        [Parameter]
        public ICollection<WorkOrderDetail> listWods { get; set; }
        public List<int> compositesIds { get; set; }

        public string _EquipmentTemplateID { get; set; }


        public ModalParameters ModalParametersReport = new ModalParameters();
        public List<KeyValueOption> compositeWods = new List<KeyValueOption>();
        public async Task<string> GetUrl(WorkOrderDetail wod)
        {
            var result = await _reportService.GetSticker(wod, new CallContext());

            await JSRuntime.InvokeVoidAsync("OpenWindow", result);

            return result;

        }

        public NoteWOD NoteRef { get; set; } = new NoteWOD();
        public List<NoteWOD> LIST1 { get; set; } = new List<NoteWOD>();
        public NoteWOD NewNote()
        {

            return new NoteWOD();


        }


        public ResponsiveTable<NoteWOD> RT1 { get; set; } = new ResponsiveTable<NoteWOD>();




#pragma warning disable CS0109 


        public List<UnitOfMeasure> HumidityUnitofMeasureList { get; set; }

        public List<UnitOfMeasure> TemperatureUnitofMeasureList { get; set; }

        public ModalParameters parameters { get; set; } = new ModalParameters();

        [Inject] Microsoft.Extensions.Configuration.IConfiguration Configuration { get; set; }
        
        public string url { get; set; }

        public string urlWebExcel { get; set; } 
        public bool isThread {  get; set; }
        public string urlAzure { get; set; }
        public bool subscription { get; set; }

        public CalibrationType cmcValuesCalType { get; set; }
        public async Task NewCalibration(ICalibrationType result,int CurrentCalibrationSubTypes)
        {


            if(result == null)
            {
                eq.Configuration= CurrentCalibrationSubTypes.ToString();

                return;
            }
            var ct = result as CalibrationType;

            eq.Configuration = CurrentCalibrationSubTypes.ToString();

            //yp eq.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject.CalibrationType = ct;

            await Task.Delay(100);

            StateHasChanged();

        }

            public async Task<List<GenericCalibrationResult2>> DynamicResult(List<GenericCalibrationResult2> result,string CalibrationSubType)
        {


            foreach (var item in result)
            {
                if (item?.GenericCalibration2 != null)
                {

                    item.GenericCalibration2.CalibrationSubTypeId = item.CalibrationSubTypeId;
                    item.GenericCalibration2.SequenceID = item.SequenceID;
                    item.GenericCalibration2.Component = item.Component;
                    item.GenericCalibration2.ComponentID = item.ComponentID;
                    item.GenericCalibration2.TestPointResult = null;

                }
            
            
            }
            if (eq?.BalanceAndScaleCalibration==null)
            {
                eq.BalanceAndScaleCalibration = new BalanceAndScaleCalibration();
            }

            if(string.IsNullOrEmpty(CalibrationSubType))
            {
                eq.BalanceAndScaleCalibration.TestPointResult = result;
            }
            else
            {
                if(eq.BalanceAndScaleCalibration.TestPointResult != null)
                {
                    eq.BalanceAndScaleCalibration.TestPointResult.RemoveAll(x => x.CalibrationSubTypeId == Convert.ToInt32(CalibrationSubType));

                    eq.BalanceAndScaleCalibration.TestPointResult.AddRange(result);
                }
                else
                {
                    eq.BalanceAndScaleCalibration.TestPointResult = result;
                }
               

            }


            if (eq.CalibrationTypeID != null && eq.CalibrationTypeID > 0 && eq.CalibrationTypeID >= 502 && eq.CalibrationTypeID <= 512)
                isThread = true;
            else
                isThread = false;



                //StateHasChanged();  
                return eq.BalanceAndScaleCalibration.TestPointResult; // result;

        }

        WorkOrderDetailGrpc wod;
        protected override async Task OnInitializedAsync()
        {

            Customer = Configuration.GetSection("Reports")["Customer"];
            url = Configuration.GetSection("Reports")["URL"];

            urlAzure = Configuration.GetSection("Reports")["AzurePath"];

            subscription = Convert.ToBoolean(Configuration.GetSection("Reports")["EnableSubscription"]);


            await base.OnInitializedAsync();

           
        }

        
        public async Task LoadEviroment()
        {

            if (HumidityUnitofMeasureList == null)
            {
                var um = AppState.UnitofMeasureList.Where(x => x.TypeID == 2).ToList();

                if (um != null && um.Count > 0)
                {
                    HumidityUnitofMeasureList = um.DistinctBy(x => x.UnitOfMeasureID).ToList();
                }
            }


            if (TemperatureUnitofMeasureList == null)
            {
                var umt = AppState.UnitofMeasureList.Where(x => x.TypeID == 1).ToList();
                if (umt != null && umt.Count > 0)
                {
                    TemperatureUnitofMeasureList = umt.DistinctBy(x=>x.UnitOfMeasureID).ToList();
                }

            }


            if (StatusList == null)
            {

                //WorkOrderDetailGrpc wod = new WorkOrderDetailGrpc(Client, DbFactory,Header);

                var a = await wod.GetStatus();

                StatusList = a.ToList();

                TypeName = "Customer";

                NameValidationMessage = "valid";

            }

        }




        public async Task LoadMeasurament()
        {
            UOMServiceGRPC uom = new UOMServiceGRPC(UOM, DbFactory);

            if (AppState.UnitofMeasureList == null || AppState.UnitofMeasureList.Count == 0)
            {
                AppState.UnitofMeasureTypeList = new List<UnitOfMeasureType>();

                AppState.UnitofMeasureList = new List<UnitOfMeasure>();

                var a = await uom.GetTypes(new Grpc.Core.CallOptions());

                AppState.UnitofMeasureTypeList = a.ToList();

                var b = await uom.GetAllEnabled(new Grpc.Core.CallOptions());

                if (b != null && b.UnitOfMeasureList != null && b.UnitOfMeasureList.Count > 0)
                {

                    AppState.UnitofMeasureList = b.UnitOfMeasureList.ToList();
                }
            }

            if (AppStateBasics.UnitofMeasureList == null || AppStateBasics.UnitofMeasureList.Count == 0)
            {
                AppStateBasics.UnitofMeasureTypeList = new List<UnitOfMeasureType>();

                AppStateBasics.UnitofMeasureList = new List<UnitOfMeasure>();

                var a = await uom.GetTypes(new Grpc.Core.CallOptions());

                AppStateBasics.UnitofMeasureTypeList = a.ToList();

                var b = await uom.GetAll(new Grpc.Core.CallOptions());
                if (b != null && b.UnitOfMeasureList != null && b.UnitOfMeasureList.Count > 0)
                {
                    AppStateBasics.UnitofMeasureList = b.UnitOfMeasureList.ToList();
                }

            }
        }

        public async Task LoadUrlWebThread()
        {

            //WorkOrderDetailGrpc wod = new WorkOrderDetailGrpc(Client, DbFactory, Header);
            WOD_ParametersTable item = new WOD_ParametersTable()
            {
                WorkOrderDetailID = eq.WorkOrderDetailID
            };

            var result = await wod.GetWOD_Parameter(item, new CallContext());

            urlWebExcel = result.urlWebOneDrive;

        }
        protected override async Task OnParametersSetAsync()
        {

            await base.OnParametersSetAsync();

            NameValidationMessage = "valid";

        }


        public WorkOrderDetail Map(WorkOrderDetail we, CalibrationSaaS.Domain.Aggregates.Entities.Status status = null)
        {
            BalanceAndScaleCalibration bc = new BalanceAndScaleCalibration();


            if(BasicInfo != null)
            {
                we = BasicInfo.eq;
            }
            
 
            if (RepeabilityComponent != null && RepeabilityComponent?.Entity != null)
            {
                bc.Repeatability = RepeabilityComponent.Entity;
            }


            if (RepeabilityComponent != null && RepeabilityComponent?.RT != null & RepeabilityComponent?.RT?.Items?.Count > 0)
            {
                bc.Repeatability.TestPointResult = RepeabilityComponent.RT.Items;
            }
            if (EccentricityComponent != null && EccentricityComponent?.Entity != null)
            {
                bc.Eccentricity = EccentricityComponent.Entity;
            }


            if (EccentricityComponent != null && EccentricityComponent?.RT != null & EccentricityComponent?.RT?.Items?.Count > 0)
            {
                bc.Eccentricity.TestPointResult = EccentricityComponent.RT.Items;
            }



            if (eq?.WorkOder?.UserWorkOrders != null)
            {
                var w = eq.WorkOder.UserWorkOrders.ToList();

                var wr = w.Where(x => we.TechnicianID.HasValue && x.UserID == we.TechnicianID.Value).FirstOrDefault();

                if (wr != null)
                {
                    we.Technician = wr.User;

                }

            }




            if (status != null)
            {
                var next = StatusList.Where(x =>
                x.StatusId == Convert.ToInt32(status.StatusId)).FirstOrDefault();
                if (next != null)
                 we.SelectedNewStatus = (short)next.StatusId;
            }
            else
            {
                we.SelectedNewStatus = -1;
            }
            if (we.PreviusStatus == null)
            {
                we.PreviusStatus = new List<CalibrationSaaS.Domain.Aggregates.Entities.Status>();
            }

            we.PreviusStatus.Add(eq.CurrentStatus);




            if (eq?.BalanceAndScaleCalibration?.Linearities != null)
            {
                bc.Linearities = eq.BalanceAndScaleCalibration.Linearities;
            }

             if (eq?.BalanceAndScaleCalibration?.Forces != null)
            {
                bc.Forces = eq.BalanceAndScaleCalibration.Forces;
            }

            if (eq?.BalanceAndScaleCalibration?.Rockwells != null)
            {
                bc.Rockwells = eq.BalanceAndScaleCalibration.Rockwells;
            }

            if (eq?.BalanceAndScaleCalibration?.GenericCalibration != null)
            {
                bc.GenericCalibration = eq.BalanceAndScaleCalibration.GenericCalibration;
            }


            if (eq?.BalanceAndScaleCalibration?.TestPointResult != null)
            {
                bc.TestPointResult = eq.BalanceAndScaleCalibration.TestPointResult;
            }

            if (eq?.EnviromentCondition != null)
            {
                we.EnviromentCondition = eq.EnviromentCondition;
                Console.WriteLine("Map 367 Enviroment Condition=" + eq.EnviromentCondition);
            }

            bc.WorkOrderDetailId = eq.WorkOrderDetailID;


            bc.WorkOrderDetail = null;
 
            if (bc?.Linearities != null && bc?.Linearities.Count > 0)
            {


                if (we?.TestGroups == null || we?.TestGroups?.Count == 0 || (we?.TestGroups?.Count == 1
                 && we?.TestGroups?.ElementAtOrDefault(0)?.TestPoints == null)
                 || (we?.TestGroups?.Count == 1 && we?.TestGroups?.ElementAtOrDefault(0)?.TestPoints?.Count == 0))
                {

                    List<TestPointGroup> TestGroups = new List<TestPointGroup>();
                    if (we.TestGroups == null || we?.TestGroups?.Count == 0)
                    {
                        we.TestGroups = new List<TestPointGroup>();
                        var tpg2 = new TestPointGroup();
                        //if (!IsOnline || 1 == 1)
                        //{
                            tpg2.TestPoitGroupID = NumericExtensions.GetUniqueID(tpg2.TestPoitGroupID);
                        //}
                        we.TestGroups.Add(new TestPointGroup());
                    }

                    TestPointGroup TP = we.TestGroups.ElementAtOrDefault(0);

                    TP.TypeID = "WOD";

                    TP.WorkOrderDetailID = we.WorkOrderDetailID;

                    TP.UnitOfMeasurementOut = null;


                    List<TestPoint> ts = new List<TestPoint>();

                    bc.Linearities.ToList().ForEach(item =>
                    {
                        if (item.TestPoint != null && (item.TestPoint.TestPointTarget == 1 || item.TestPoint.TestPointID == 0))
                        {
                            var ity = item.TestPoint;
                            ity.TestPointTarget = 1;
                            item.TestPoint.UnitOfMeasurement = null;
                            item.TestPoint.UnitOfMeasurementOut = null;
                            ity.WOD_TestPoints = null;

                            TP.OutUnitOfMeasurementID = item.TestPoint.UnitOfMeasurementID;
                            ts.Add(ity);
                        }


                    });

                    TP.TestPoints = ts;
                    TestGroups.Add(TP);

                    we.TestGroups = TestGroups;
                }




                bc.HasLinearity = true;
                int cont = 0;
                bc?.Linearities?.ToList()?.ForEach(item =>
                {
  
                    if (item?.TestPoint != null && item?.TestPoint?.TestPointID > 0)
                    {
                        item.TestPointID = item.TestPoint.TestPointID;

                    }
                    if (item?.TestPoint != null)
                    {
                        item.TestPoint.UnitOfMeasurement = null;
                        item.TestPoint.UnitOfMeasurementOut = null;
                    }


                    if (item.WeightSets != null)
                    {
                        foreach (var w in item.WeightSets)
                        {

                        }
                    }
                    cont = cont + 1;
                }
              );

            }

            if (bc?.Repeatability != null && bc?.Repeatability?.TestPointResult?.Count > 0)
            {
                bc.HasRepeatability = true;

                if (bc?.Repeatability?.TestPoint != null)
                {
                    bc.Repeatability.TestPoint.UnitOfMeasurementOut = null;
                    bc.Repeatability.TestPoint.UnitOfMeasurement = null;
                }
                bc.Repeatability.WorkOrderDetailId = eq.WorkOrderDetailID;

                if (bc.Repeatability.TestPoint != null)
                {
                    bc.Repeatability.TestPointID = bc.Repeatability.TestPoint.TestPointID;
                    bc.Repeatability.TestPoint.UnitOfMeasurementOut = null;
                    bc.Repeatability.TestPoint.UnitOfMeasurement = null;
                }






                bc?.Repeatability?.TestPointResult.ToList().ForEach(item =>
                {
                    if (item.UnitOfMeasure == null)
                    {
                        item.UnitOfMeasure = Helpers.NumericExtensions.Conversion<UnitOfMeasure>(item.UnitOfMeasure,
                        item.UnitOfMeasureID.ToString(), AppState.UnitofMeasureList, nameof(item.UnitOfMeasureID));
                    }
                    item.WorkOrderDetailId = we.WorkOrderDetailID;
                }
              );
            }
            if (bc?.Eccentricity != null && bc?.Eccentricity?.TestPointResult?.Count > 0)
            {
                bc.HasEccentricity = true;
                if (bc?.Eccentricity?.TestPoint != null)
                {
                    bc.Eccentricity.TestPoint.UnitOfMeasurementOut = null;
                    bc.Eccentricity.TestPoint.UnitOfMeasurement = null;
                }


                bc.Eccentricity?.TestPointResult.ToList().ForEach(item =>
                {
                    item.WorkOrderDetailId = we.WorkOrderDetailID;
                    if (item.UnitOfMeasure == null)
                    {
                        item.UnitOfMeasure = Helpers.NumericExtensions.Conversion<UnitOfMeasure>(item.UnitOfMeasure,
                        item.UnitOfMeasureID.ToString(), AppState.UnitofMeasureList, nameof(item.UnitOfMeasureID));
                    }


                }
              );
            }


            we.BalanceAndScaleCalibration = bc;

            if (we?.PieceOfEquipment?.EquipmentTemplate?.UnitOfMeasure != null)
            {
                we.PieceOfEquipment.EquipmentTemplate.UnitOfMeasure = null;
            }

            if (we?.EquipmentCondition == null)
            {
                we.EquipmentCondition = new List<EquipmentCondition>();
            }
            we.EquipmentCondition.ToList().ForEach(item =>

            {
                if (item.EquipmentConditionId == 0 && !IsOnline)
                {
                    item.EquipmentConditionId = NumericExtensions.GetUniqueID(item.EquipmentConditionId);
                }
                item.WorkOrderDetailId = eq.WorkOrderDetailID;

            });
            return we;

        }

        public List<WOD_TestPoint> lstwt { get; set; } = new List<WOD_TestPoint>();

        public List<WOD_Weight> lstWOD_Weight { get; set; } = new List<WOD_Weight>();
        public void WT(WorkOrderDetail we)
        {

            if (we?.BalanceAndScaleCalibration==null) 
            {
                return;
            }

            BalanceAndScaleCalibration bc = we.BalanceAndScaleCalibration;

            lstwt = new List<WOD_TestPoint>();
            lstWOD_Weight = new List<WOD_Weight>();

            if (bc?.Linearities != null && bc?.Linearities?.Count() > 0)
            {

                bc?.Linearities?.ToList()?.ForEach(item =>
                {
                    WOD_TestPoint wt = new WOD_TestPoint();
                    if (item.TestPointID.HasValue)
                    {
                        wt.TestPointID = item.TestPointID.Value;
                    }

                    wt.WorkOrderDetailID = we.WorkOrderDetailID;
                    wt.CalibrationSubTypeID = item.CalibrationSubTypeId;
                    wt.SecuenceID = item.SequenceID;
                    lstwt.Add(wt);

                    if (item.WeightSets != null && item.WeightSets.Count > 0)
                    {
                        List<CalibrationSubType_Weight> csw = new List<CalibrationSubType_Weight>();

                        foreach (var w in item.WeightSets)
                        {
                            CalibrationSubType_Weight ww = new CalibrationSubType_Weight();

                            ww.CalibrationSubTypeID = item.CalibrationSubTypeId;
                            ww.SecuenceID = item.SequenceID;
                            ww.WorkOrderDetailID = we.WorkOrderDetailID;
                            ww.WeightSetID = w.WeightSetID;
                            csw.Add(ww);
                        }

                        item.CalibrationSubType_Weights = csw;
                        //item.WeightSets = null;
                    }


                });






            }

             if (bc?.Repeatability != null && bc?.Repeatability?.TestPointResult != null
                && bc?.Repeatability.TestPointResult?.Count() > 0)
            {
                var item = bc.Repeatability;
                WOD_TestPoint wt1 = new WOD_TestPoint();
                if (item.TestPointID.HasValue)
                {
                    wt1.TestPointID = item.TestPointID.Value;
                }

                wt1.WorkOrderDetailID = we.WorkOrderDetailID;
                wt1.CalibrationSubTypeID = item.CalibrationSubTypeId;
                lstwt.Add(wt1);
                if (item.WeightSets != null && item.WeightSets.Count > 0)
                {
                    List<CalibrationSubType_Weight> csw = new List<CalibrationSubType_Weight>();
                    foreach (var w in item.WeightSets)
                    {
                        CalibrationSubType_Weight ww = new CalibrationSubType_Weight();
                        ww.CalibrationSubTypeID = item.CalibrationSubTypeId;
                        ww.SecuenceID = 1;
                        ww.WorkOrderDetailID = we.WorkOrderDetailID;
                        ww.WeightSetID = w.WeightSetID;
                        csw.Add(ww);
                    }

                    item.CalibrationSubType_Weights = csw;
                    //item.WeightSets = null;
                }


            }


            if (bc?.Eccentricity?.TestPointResult?.Count() > 0)
            {
                var item = bc.Eccentricity;
                WOD_TestPoint wt1 = new WOD_TestPoint();
                if (item.TestPointID.HasValue)
                {
                    wt1.TestPointID = item.TestPointID.Value;
                }
                if (item.TestPoint != null)
                {

                }

                wt1.TestPoint = null;
                wt1.WorkOrderDetailID = we.WorkOrderDetailID;
                wt1.CalibrationSubTypeID = item.CalibrationSubTypeId;
                lstwt.Add(wt1);

                if (item.WeightSets != null && item.WeightSets.Count > 0)
                {
                    List<CalibrationSubType_Weight> csw = new List<CalibrationSubType_Weight>();
                    foreach (var w in item.WeightSets)
                    {
                        CalibrationSubType_Weight ww = new CalibrationSubType_Weight();
                        ww.CalibrationSubTypeID = item.CalibrationSubTypeId;
                        ww.SecuenceID = 1;
                        ww.WorkOrderDetailID = we.WorkOrderDetailID;
                        ww.WeightSetID = w.WeightSetID;
                        csw.Add(ww);

                    }
                    item.CalibrationSubType_Weights = csw;
                    //item.WeightSets = null;
                }

            }

            if (bc?.Forces != null && bc?.Forces.Count() > 0)
            {

                bc?.Forces?.ToList()?.ForEach(item =>
                {
                    WOD_TestPoint wt = new WOD_TestPoint();
                    if (item.TestPointID.HasValue)
                    {
                        wt.TestPointID = item.TestPointID.Value;
                    }

                    wt.WorkOrderDetailID = we.WorkOrderDetailID;
                    wt.CalibrationSubTypeID = item.CalibrationSubTypeId;
                    wt.SecuenceID = item.SequenceID;
                    lstwt.Add(wt);

                    if (item.WeightSets != null && item.WeightSets.Count > 0)
                    {
                        List<CalibrationSubType_Weight> csw = new List<CalibrationSubType_Weight>();

                        foreach (var w in item.WeightSets)
                        {
                            CalibrationSubType_Weight ww = new CalibrationSubType_Weight();

                            ww.CalibrationSubTypeID = item.CalibrationSubTypeId;
                            ww.SecuenceID = item.SequenceID;
                            ww.WorkOrderDetailID = we.WorkOrderDetailID;
                            ww.WeightSetID = w.WeightSetID;
                            csw.Add(ww);
                        }

                        item.CalibrationSubType_Weights = csw;
                        //item.WeightSets = null;
                    }


                });

            }



            


        }

        public WorkOrderDetail LoadET(WorkOrderDetail we, EquipmentTemplate reset, bool? IsAcredited)
        {
            we.Resolution = reset.Resolution;
            we.Tolerance.AccuracyPercentage = reset.Tolerance.AccuracyPercentage;
            we.DecimalNumber = reset.DecimalNumber;
            if (reset.Tolerance.ToleranceTypeID.HasValue)
            {
                we.Tolerance.ToleranceTypeID = reset.Tolerance.ToleranceTypeID.Value;
            }
         
            we.IsAccredited = IsAcredited;
            we.IsComercial = reset.IsComercial;
            we.ClassHB44 = reset.ClassHB44;
            if (reset.Tolerance.ToleranceFixedValue > 0)
            {
                we.Tolerance.ToleranceFixedValue = reset.Tolerance.ToleranceFixedValue;
            }

            

            return we;
        }


        public async Task Save()

        {

            await SetCurrentStatus(eq.CurrentStatus, true);


        }

       


            public async Task SetCurrentStatus(CalibrationSaaS.Domain.Aggregates.Entities.Status status, bool OnlySave = false)

        {

            Saving = true;

            WorkOrderDetail we = null;

            try
            {
               
                if (RT1?.Items?.Count > 0)
                {
                    eq.NotesWOD = RT1.Items;
                }

               

                await ShowProgress();

                int version = 0;

                 we = Map(eq, status);

               

                WT(we);

                

                //if (eq?.BalanceAndScaleCalibration?.GenericCalibration2 != null)
                //{
                //    var result = eq.BalanceAndScaleCalibration.GenericCalibration2;

                //    foreach (var item in result)
                //    {

                //        if (item?.GenericCalibration2 != null)
                //        {

                //            item.GenericCalibration2.CalibrationSubTypeId = item.CalibrationSubTypeId;
                //            item.GenericCalibration2.SequenceID = item.SequenceID;
                //            item.GenericCalibration2.Component = item.Component;
                //            item.GenericCalibration2.ComponentID = item.ComponentID;
                //            item.GenericCalibration2.BasicCalibrationResult = null;
                //        }
                      

                //    }

                //    eq.BalanceAndScaleCalibration.GenericCalibration2 = result;


                //    we.BalanceAndScaleCalibration.GenericCalibration2 = result;
                //}

                if (CalibrationTypeComponent != null && CalibrationTypeComponent._refs != null && CalibrationTypeComponent._refs.Count > 0)
                {
                    if (string.IsNullOrEmpty(we.Configuration)|| we.Configuration=="0")
                    {
                        we.Configuration = CalibrationTypeComponent._refs.Count.ToString();
                    }
                   

                    await CalibrationTypeComponent.CloseGenericWindow();

                }


                we.BalanceAndScaleCalibration.TestPointResult = eq.BalanceAndScaleCalibration.TestPointResult;


                //var con = await CallOptions(Component.Name);


                //WorkOrderDetailGrpc wod = new WorkOrderDetailGrpc(Client, DbFactory, con.CallOptions);

                WorkOrderDetail all = new WorkOrderDetail();
                if(eq.Technician != null)
                {
                      we.Technician.Name = eq.Technician.Name;
                we.Technician.LastName = eq.Technician.LastName;
                }
              

                if (CalibrationInstructions != null && CalibrationInstructions?.ResolutionComponent != null)
                {
                    var reset = CalibrationInstructions.ResolutionComponent.eq;
 
                    we =  LoadET(we, reset, eq.IsAccredited);
                     we.Ranges = new List<RangeTolerance>();

                    if (CalibrationInstructions != null && CalibrationInstructions?.ResolutionComponent?.RangeComponent?.RT?.Items?.Count > 0)
                    {
                        foreach (var iyy in CalibrationInstructions.ResolutionComponent.RangeComponent.RT.Items)
                        {
                            iyy.WorkOrderDetailID = we.WorkOrderDetailID;
                            we.Ranges.Add(iyy);
                        }

                    }
                    if (CalibrationInstructions?.ResolutionComponent?.RangeAccuracy?.RT?.Items?.Count > 0)
                    {
                        foreach (var iyy in CalibrationInstructions.ResolutionComponent.RangeAccuracy.RT.Items)
                        {
                            iyy.WorkOrderDetailID = we.WorkOrderDetailID;
                            we.Ranges.Add(iyy);

                        }

                    }
                    
                }

                if (we.BalanceAndScaleCalibration.Eccentricity != null && we.BalanceAndScaleCalibration.Eccentricity.TestPointResult != null && we.BalanceAndScaleCalibration.Eccentricity.TestPointResult.Count() > 0 )
                {
                    TestPoint testPoint = new TestPoint();
                    if (we.BalanceAndScaleCalibration.Eccentricity.TestPoint == null)
                    { 
                    testPoint.UnitOfMeasurementOutID = (int)we.PieceOfEquipment.UnitOfMeasureID;
                    testPoint.UnitOfMeasurementID = (int)we.PieceOfEquipment.UnitOfMeasureID;
                    testPoint.LowerTolerance = we.BalanceAndScaleCalibration.Eccentricity.TestPointResult.FirstOrDefault().LowerTolerance;
                    testPoint.UpperTolerance = we.BalanceAndScaleCalibration.Eccentricity.TestPointResult.FirstOrDefault().UpperTolerance;
                    testPoint.CalibrationType = "Eccentricity";
                    we.BalanceAndScaleCalibration.Eccentricity.TestPoint = testPoint;

                    }
                    else
                    {
                        we.BalanceAndScaleCalibration.Eccentricity.TestPoint.LowerTolerance = we.BalanceAndScaleCalibration.Eccentricity.TestPointResult.FirstOrDefault().LowerTolerance;
                        we.BalanceAndScaleCalibration.Eccentricity.TestPoint.UpperTolerance = we.BalanceAndScaleCalibration.Eccentricity.TestPointResult.FirstOrDefault().UpperTolerance;
                        we.BalanceAndScaleCalibration.Eccentricity.TestPoint.CalibrationType = "Eccentricity";

                    }

                }


                if (!OnlySave)
                {

                   var status2 = Querys.WOD.GetNextStatus(we, StatusList); 
                    if (status != null && status.StatusId == 4 || status == null && (status2 != null && status2.IsLast))
                    {

                        ModalParametersReport = new ModalParameters();
                        ModalParametersReport.Add("WOD", we);
                        WorkOrderDetail wodd = new WorkOrderDetail();
                        wodd.WorkOrderDetailID = eq.WorkOrderDetailID;
                        wodd.WorkOrderDetailHash = version.ToString();
                        wodd.CalibrationCustomDueDate = eq.CalibrationCustomDueDate;
                        wodd.CalibrationDate = eq.CalibrationDate;
                        wodd.TechnicianID = eq.TechnicianID;
                        //ReportServiceGRPC basic = new ReportServiceGRPC(_reportService);
                        var dd = wodd.CurrentStatus;
                        var currsid = wodd.CurrentStatusID;
 
                    }

                    if (status == null)
                    {
                        status = status2;
                    }

                    List<string> validation = Querys.WOD.ValidateWODList(status, ref we, false);

                    if (validation.Count() > 0)
                    {
                        foreach (var item in validation)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                await ShowToast(item, ToastLevel.Error);

                            }

                        }

                        throw new Exception("Validation Error");

                    }


                    if (we?.PieceOfEquipment?.EquipmentTemplate?.TestGroups != null)
                    {
                        if (we?.PieceOfEquipment?.EquipmentTemplate?.TestGroups?.Count > 0)
                        {
                            var linne = we.PieceOfEquipment.EquipmentTemplate.TestGroups.ElementAtOrDefault(0).TestPoints.Where(Querys.WOD.GetLinearityTestPoint().Compile()).ToList();

                            var ecce = we.PieceOfEquipment.EquipmentTemplate.TestGroups.ElementAtOrDefault(0).TestPoints.
                               Where(Querys.WOD.GetEccentricityTestPoint().Compile()).ToList();

                            var repea = we.PieceOfEquipment.EquipmentTemplate.TestGroups.ElementAtOrDefault(0).TestPoints.
                              Where(Querys.WOD.GetRepeatibilityTestPoint().Compile()).ToList();

                           
                        }
                       
                    }
                    
                    if (!IsOnline && !we.TechnicianID.HasValue)
                    {
                        await ShowError("Please assign Technichian");
                        return;
                    }
                    
                    WorkOrderDetail wodReplaced = new WorkOrderDetail();
                    if (CalibrationInstructions?.ResolutionComponent.eq.Tolerance != null && CalibrationInstructions.ResolutionComponent.eq.Tolerance.UsePreviousCalibrationData == true && we.BalanceAndScaleCalibration != null && we.BalanceAndScaleCalibration.TestPointResult == null)
                    {
                        wodReplaced = await wod.GetByIDPreviousCalibration(we);

                    }


                    if (wodReplaced != null && wodReplaced.WorkOrderDetailID !=null && wodReplaced.WorkOrderDetailID != 0)
                    {
                        all = await wod.ChangeStatus(wodReplaced);
                        await ShowToast("Values successfully imported from " + wodReplaced.WorkOrderDetailIdPrevious , ToastLevel.Success);
                                                
                        eq = all;
                        Saving = false;
                    }
                    else
                    {
                       
                        
                        
                        all = await wod.ChangeStatus(we);
                        Saving = false;
                        if (all.CurrentStatusID == 2)
                        {
                            List<WorkOrderDetail> lsttemp = new List<WorkOrderDetail>();

                            foreach (var item in listWorkOrderDetailchildren)
                            {
                                item.CalibrationDate = we.CalibrationDate;
                                item.CalibrationIntervalID = we.CalibrationIntervalID;
                                item.CalibrationNextDueDate = we.CalibrationNextDueDate;
                                item.ClassHB44 = we.ClassHB44;
                                item.CurrentStatus = all.CurrentStatus;
                                item.CurrentStatusID = all.CurrentStatusID;
                                item.DecimalNumber = we.DecimalNumber;
                                item.Description = we.Description;
                                item.EndOfMonth = we.EndOfMonth;
                                item.EnviromentCondition = we.EnviromentCondition;
                                item.Environment = we.Environment;

                                item.EquipmentCondition = we.EquipmentCondition;

                                foreach (var iec in item.EquipmentCondition)
                                {
                                    iec.EquipmentConditionId = NumericExtensions.GetUniqueID(0);
                                }


                                item.HasBeenCompleted = all.HasBeenCompleted;
                                item.Humidity = we.Humidity;
                                item.HumidityUOM = we.HumidityUOM;
                                item.HumidityUOMID = we.HumidityUOMID;
                                item.IncludeASTM = we.IncludeASTM;
                                item.IsAccredited = we.IsAccredited;
                                item.IsComercial = we.IsComercial;
                                item.IsModifiedOff = we.IsModifiedOff;
                                item.IsOffline = all.IsOffline;
                                item.IsSync = all.IsSync;
                                item.IsUniversal = we.IsUniversal;
                                item.JsonTolerance = we.JsonTolerance;
                                item.ModeID = we.ModeID;
                                item.Multiplier = we.Multiplier;
                                item.StatusDate = we.StatusDate;
                                item.Technician = we.Technician;
                                item.TechnicianID = we.TechnicianID;
                                item.Temperature = we.Temperature;
                                item.TemperatureAfter = we.TemperatureAfter;
                                item.TemperatureStandardId = we.TemperatureStandardId;
                                item.TemperatureUOM = we.TemperatureUOM;
                                item.TestCode = we.TestCode;
                                item.TestCodeID = we.TestCodeID;
                                item.Tolerance = we.Tolerance;
                                item.ToleranceTypeID = we.ToleranceTypeID;
                                item.Uncertainty = we.Uncertainty;

                                if(item.Uncertainty != null)
                                { 

                                    foreach (var itemun in item.Uncertainty)
                                    {
                                        itemun.UncertaintyID = 0;
                                    }
                                }

                                var child1 = await wod.SaveWod(item);

                                lsttemp.Add(child1);
                            }

                            listWorkOrderDetailchildren = lsttemp;
                        }




                        if (CalibrationInstructions.ResolutionComponent.eq.Tolerance != null && CalibrationInstructions.ResolutionComponent.eq.Tolerance.UsePreviousCalibrationData == true)
                        {
                            await ShowToast("No information found to be imported", ToastLevel.Info);
                        }

                    }
                    

                    await ShowToast("Status changed successfully", ToastLevel.Success);

                    //ChangeResolution = false;

                    if (all != null)
                    {

                        eq = all;

                        ModalParametersReport = new ModalParameters();
                        ModalParametersReport.Add("WOD", eq);

                    }
                }
                else
                {
                    

                    if (!IsOnline && we.TechnicianID.HasValue==false && AppSecurity.CurrentUserID.HasValue)
                    {

                        we.TechnicianID = AppSecurity.CurrentUserID;
                    }



                    if (!IsOnline && !we.TechnicianID.HasValue)
                    {
                        await ShowError("Please assign Technichian");
                        return;
                    }


                    all = await wod.SaveWod(we);
                    Saving = false;
                    //var calibrationType = eq.CalibrationType;
                    //all.CalibrationType = calibrationType;
                    //EquipmentTypeObject.CalibrationType = eq.CalibrationType;
                    eq = all;
                    List<WorkOrderDetail> lsttemp = new List<WorkOrderDetail>();
                     
                    await ShowToast("The information has been saved successfully.", ToastLevel.Success);

                    ChangeResolution = false;

                }

                if (eq.CalibrationTypeID != null && eq.CalibrationType== null)
                {
                    var con = await CallOptions(Component.Name);
                    PieceOfEquipmentGRPC poegrpc = new PieceOfEquipmentGRPC(_poeServices, DbFactory, con.CallOptions);
                    var result = await poegrpc.GetDynamicConfiguration(new CalibrationType() { CalibrationTypeId = (int)eq.CalibrationTypeID });
                    eq.CalibrationType = result;
                    
                }

                

                if (eq.NotesWOD != null)
                {
                    LIST1 = eq.NotesWOD.ToList();
                }

            }
            catch (RpcException ex)
            {
                

                eq = we;
                await ExceptionManager(ex, "RPC -- SetCurrentStatus ");
            }
            catch (Exception ex)
            {
                eq = we;
                await ExceptionManager(ex, "SetCurrentStatus ");
            }
            finally
            {
                //eq = lsttemp2.ElementAtOrDefault(0);
                Saving = false;
                await CloseProgress();

                //StateHasChanged();
            }
 
        }
       
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            int line = 0;

            await base.OnAfterRenderAsync(firstRender);

            try
            {
                

                //if (firstRender)
                //{
                //    await ShowProgress();
                //}

                line = 1;

                if (weigset != null && WeightSetList2 != null)
                {

                    foreach (var wei in WeightSetComponent)
                    {
                        await wei.Show(WeightSetList2);
                    }                  

                }


                line = 2;
                 if (eq?.CurrentStatusID == 0)
                {
                    eq.CurrentStatusID = 1;
                }
                if (firstRender)
                {

                    var uer = await CurrentUserName();

                    Component.User = uer;

                    var con = await CallOptions(Component.Name);

                    wod = new WorkOrderDetailGrpc(Client, DbFactory, con.CallOptions);

                    await LoadMeasurament();

                    await LoadEviroment();

                    if (IsOnline)
                    {
                        await LoadUrlWebThread();
                    }

                    line = 3;
                    await LoadWOD2();


 
                }

                AssetsServiceGRPC assts = new AssetsServiceGRPC(_assetsServices, DbFactory);
                line = 4;
                if (eq?.WOD_Weights != null)
                {

                    foreach (var itenx in eq.WOD_Weights)
                    {
                        if(itenx?.WeightSet?.PieceOfEquipment != null)
                        {
                            itenx.WeightSet.PieceOfEquipment.EquipmentTemplate = null;
                            itenx.WeightSet.PieceOfEquipment.WeightSets = null;
                            itenx.WeightSet.WOD_Weights = null;
                        }
                       
                    }
                }

                line = 5;

                WorkOrderDetail wdtmp= new  WorkOrderDetail();

                if(eq != null)
                {
                    wdtmp.WorkOrderDetailID = eq.WorkOrderDetailID;
                }
               


                _listCetificate = await assts.GetCertificateXWod(wdtmp);

                line = 6;

                if (_listCetificate != null && _listCetificate?.Count() > 0 && CertificateCreate != null)
                {
                    line = 7;
                    CertificateCreate.Show(_listCetificate.ToList());

                }

                line = 8;



                if (CalibrationInstructions != null && CalibrationInstructions?.ResolutionComponent?.RangeComponent?.RT?.Items?.Count > 0)
                {
                    line = 9;
                    foreach (var iyy in CalibrationInstructions.ResolutionComponent.RangeComponent.RT.Items)
                    {
                        iyy.WorkOrderDetailID = eq.WorkOrderDetailID;
                        eq.Ranges.Add(iyy);
                    }

                }
                line = 10;
                if (CalibrationInstructions != null && CalibrationInstructions?.ResolutionComponent?.RangeAccuracy?.RT?.Items?.Count > 0)
                {
                    line = 11;
                    foreach (var iyy in CalibrationInstructions.ResolutionComponent.RangeAccuracy.RT.Items)
                    {
                        iyy.WorkOrderDetailID = eq.WorkOrderDetailID;
                        eq.Ranges.Add(iyy);

                    }

                }


                if (EquipmentTypeObject != null && EquipmentTypeObject?.CalibrationTypeID > 0 && EquipmentTypeObject?.CalibrationTypeID >= 502 && EquipmentTypeObject?.CalibrationTypeID <= 512)
                    isThread = true;
                else
                    isThread = false;



            }
            catch (RpcException ex)
            {

                await ExceptionManager(ex, "OnAfterRenderAsync " + line.ToString());

            }

            catch (Exception ex)
            {
                await ExceptionManager(ex, "OnAfterRenderAsync Line : " + line.ToString());
            }
            finally
           {
                
                

                if (firstRender)
                {
                    //await CloseProgress();
                    
                    line = 12;
                    LoadingWait = true;
                    StateHasChanged();
                    
                }
                //await ScrollPosition();
            }

        }

      

        public async Task LoadWOD2()
        {

            int line = 0;
 

            if (EntityID == "0")
            {
                line = 2;
                eq = new CalibrationSaaS.Domain.Aggregates.Entities.WorkOrderDetail();

                //eq.PieceOfEquipment.TestPointResult

            }
            else
            {
                eq.WorkOrderDetailID = Convert.ToInt32(EntityID);


                //WorkOrderDetailGrpc wod = null;
                //   if(IsOnline)
                //{
                //    wod= new WorkOrderDetailGrpc(Client, DbFactory, Header);
                //}
                //else
                //{
                //    wod = new WorkOrderDetailGrpc(Client, DbFactory);
                //}
                   

                line = 3;

                eq = await wod.GetByID(eq);

                var lch = await wod.GetWorkOrderDetailChildren(eq);

                if(lch != null && lch.Count() > 0)
                {
                    listWorkOrderDetailchildren = lch.ToList();
                }
               

                EquipmentTypeObject = eq.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject;

                if (EquipmentTypeObject?.CalibrationType == null 
                    || (EquipmentTypeObject?.CalibrationType?.CalibrationSubTypes == null|| EquipmentTypeObject?.CalibrationType?.CalibrationSubTypes.Count==0))
                {
                    var con = await CallOptions(Component.Name);
                    PieceOfEquipmentGRPC poegrpc = new PieceOfEquipmentGRPC(_poeServices, DbFactory, con.CallOptions);
                    var result = await poegrpc.GetDynamicConfiguration(new CalibrationType() { CalibrationTypeId = EquipmentTypeObject.CalibrationTypeID});

                    //Show columns If OnlyAccreditted and Wod is Accredited
                    if (eq?.IsAccredited.HasValue == false || eq?.IsAccredited == false)
                    {
                        foreach (var item in result?.CalibrationSubTypes)
                        {
                            if(item.DynamicPropertiesSchema != null)
                            {
                                foreach (var item1 in item?.DynamicPropertiesSchema)
                                {
                                    if (item1?.ViewPropertyBase?.OnlyAccredited == true)
                                    {
                                        item1.ViewPropertyBase.IsVisible = false;
                                    }
                                }
                            }
                           
                        }
                    }







                    EquipmentTypeObject.CalibrationType = result;
                }
                

                if (EquipmentTypeObject?.CalibrationTypeID != null && EquipmentTypeObject?.CalibrationType?.CalibrationSubTypes != null)
                {
                    _calibrationSubtypeList = EquipmentTypeObject.CalibrationType.CalibrationSubTypes.DistinctBy(x=>x.CalibrationSubTypeId).ToList();//dataGrids.Where(x=>x.CalibrationTypeId == eq.CalibrationTypeID && x.CalibrationTypeId != 1 && eq.CalibrationTypeID != 2).ToList();

                    EquipmentTypeObject.CalibrationType.CalibrationSubTypes = _calibrationSubtypeList;

                    if (eq.CalibrationType.CalibrationSubTypes == null)
                    {
                        eq.CalibrationType.CalibrationSubTypes = _calibrationSubtypeList;
                    }
                   

                }
                else if(eq?.PieceOfEquipment?.EquipmentTemplate?.EquipmentTypeObject?.DynamicConfiguration2==true 
                    || eq?.PieceOfEquipment?.EquipmentTemplate?.EquipmentTypeObject?.DynamicConfiguration==true)
                {
                    var dataGrids = await wod.GetCalibrationSubtype(new CallContext());

                    _calibrationSubtypeList = dataGrids.DistinctBy(x => x.CalibrationSubTypeId).ToList();

                }

                Console.WriteLine("eq LoadWOD2====> " + eq.WorkOrderDetailID);

                //CMCValues

                if (eq?.CalibrationType?.CalibrationTypeId  > 0)
                {
                    BasicsServiceGRPC basics = new BasicsServiceGRPC(_basicsServices,DbFactory);
                    CalibrationType calibrationType = new CalibrationType()
                    {
                        CalibrationTypeId = (int)eq.CalibrationType.CalibrationTypeId
                    };
                    var calTypeCMCValues = await basics.GetCalibrationTypeById(calibrationType);
                    cmcValuesCalType = calTypeCMCValues;
                }



                //if(eq?.BalanceAndScaleCalibration?.GenericCalibration != null 
                //    && eq?.BalanceAndScaleCalibration?.GenericCalibration?.Count > 0)
                //{
                //    foreach (var item in eq.BalanceAndScaleCalibration.GenericCalibration)
                //    {
                //        item.BasicCalibrationResult = item.BasicCalibrationResult.Object.ToDynamic();
                //    }

                //}


                line = 4;
                if (eq?.WorkOder != null && eq?.WorkOder?.Customer
                    != null && eq?.WorkOder?.Customer?.Aggregates != null && eq?.WorkOder?.Customer?.Aggregates?.Count > 0)
                {
                    line = 5;
                    Addresses = eq.WorkOder.Customer.Aggregates.ElementAtOrDefault(0).Addresses.ToList();
                    line = 6;

                }
                else
                {


                }
                line = 7;
                if (eq?.CalibrationTypeID == 0 && eq?.WorkOder?.CalibrationType > 0)
                {
                    //eq.CalibrationTypeID = eq.WorkOder.CalibrationType;
                }
                line = 8;
                if (!eq?.IsAccredited.HasValue == true  && eq?.WorkOder?.IsAccredited==true)
                {
                    eq.IsAccredited = eq.WorkOder.IsAccredited;
                }

                line = 9;
                if (eq?.AddressID.HasValue == false && eq?.WorkOder != null && eq?.WorkOder?.AddressId > 0)
                {
                    eq.AddressID = eq?.WorkOder?.AddressId;
                }
                line = 10;
                if (eq?.WorkOder != null && eq?.WorkOder.Customer
                     != null && eq.WorkOder.Customer.Aggregates != null && eq.WorkOder.Customer.Aggregates.Count > 0)
                {

                    Addresses = eq.WorkOder.Customer.Aggregates.ElementAtOrDefault(0).Addresses.ToList();


                }
                else
                {


                }
   
                line = 11;
                Logger.LogDebug(eq.CurrentStatus.Name);

                PieceOfEquipment = eq.PieceOfEquipment;

                if (PieceOfEquipment == null)
                {
                    PieceOfEquipment = new PieceOfEquipment();
                }
                editContext = new EditContext(PieceOfEquipment);

                line = 12;

                editContext.OnFieldChanged += EditContext_OnFieldChanged;

                line = 13;
                LIST.Add(eq);

                if (BasicInfo != null)
                {
                    BasicInfo.eq = eq;
                }
                line = 15;
               
                WeightSetList2 = new List<WeightSet>();
                line = 16;
                if (eq.WOD_Weights != null)
                {
                    line = 17;
                    PieceOfEquipment POETemp = new PieceOfEquipment();
                    foreach (var wi in eq.WOD_Weights)
                    {
                        if (wi.WeightSet != null && wi.WeightSet.WeightSetID > 0)
                        {
                            line = 18;
                            var Uom = wi.WeightSet.UnitOfMeasureID.GetUoM(AppState.UnitofMeasureList);
                            wi.WeightSet.UnitOfMeasure = Uom;
                            WeightSetList2.Add(wi.WeightSet);


                        }

                    }

                    POETemp.WeightSets = WeightSetList2;

                }
                else
                {

                }

                if (eq.NotesWOD != null)
                {
                    LIST1 = eq.NotesWOD.ToList();
                }
                ModalParametersReport = new ModalParameters();
                ModalParametersReport.Add("WOD", eq);

            }


            ///YP List Composite
            try
            {

                if (EntityID != null && int.TryParse(EntityID, out int currentWodId))
                {

                    await LoadFilteredWorkOrderDetailList(currentWodId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading filtered WorkOrderDetail list: {ex.Message}");
            }

            parameters.Add("WorkOrderDetail", eq);

            line = 19;
        }

        /// <summary>
        /// Loads and filters the WorkOrderDetail list for composite WODs
        /// Excludes: 1) Current WorkOrderDetail, 2) WorkOrderDetails with same CalibrationTypeId as current
        /// </summary>
        private async Task LoadFilteredWorkOrderDetailList(int currentWorkOrderDetailId)
        {
            try
            {
                // Get the current WorkOrderDetail to access its WorkOrder
                if (eq?.WorkOder?.WorkOrderId > 0)
                {
                    // Get all WorkOrderDetails for this WorkOrder
                    var allWorkOrderDetails = await GetWorkOrderDetailsForWorkOrder(eq.WorkOder.WorkOrderId);


                    var customerComposite = eq?.PieceOfEquipment?.EquipmentTemplate?.EquipmentTypeObject?.CalibrationType?.CustomerComposite;
                    List<int> customerIds = null;


                    if (customerComposite != null)
                    {

                       var items = customerComposite.Split(',');
                        customerIds = items.Select(int.Parse).ToList();

                    }


                    if (allWorkOrderDetails != null && allWorkOrderDetails.Count > 0)
                    {
                        // Find the current WorkOrderDetail to get its CalibrationTypeId
                        var currentWod = allWorkOrderDetails.FirstOrDefault(x => x.WorkOrderDetailID == currentWorkOrderDetailId);

                        if (currentWod != null)
                        {
                            // Filter the list: exclude current WorkOrderDetail AND those with same CalibrationTypeId
                            var filteredList = allWorkOrderDetails.
                                Where(x => x.WorkOrderDetailID != currentWorkOrderDetailId
                                  /*&&  x.CalibrationTypeID != currentWod.CalibrationTypeID*/)
                                .ToList();

                            listWods = filteredList.Where(x=>x.PieceOfEquipment?.EquipmentTemplate?.EquipmentTypeObject?.EquipmentTypeGroupID == currentWod.PieceOfEquipment?.EquipmentTemplate?.EquipmentTypeObject?.EquipmentTypeGroupID).ToList();

                            // Fill compositesIds with WorkOrderDetailID from listWods
                            if (listWods.Count > 0)
                            {
                                compositesIds = listWods.Select(x => x.WorkOrderDetailID).ToList();

                            }

                            foreach (var item in listWods)
                            {
                                // Find the corresponding WorkOrderDetail for more descriptive text
                                var value = item?.PieceOfEquipment?.PieceOfEquipmentID + "-" + item?.WorkOrderDetailID;
                                KeyValueOption key = new KeyValueOption()
                                {
                                    Key = "WOD-" + item.WorkOrderDetailID.ToString(),
                                    Value = value
                                };
                                compositeWods.Add(key);
                            }

                            if (compositesIds != null)
                            {
                                var con = await CallOptions(Component.Name);
                                foreach (var item in customerIds)
                                {
                                    CalibrationSaaS.Domain.Aggregates.Entities.Customer customer = new CalibrationSaaS.Domain.Aggregates.Entities.Customer();
                                    customer.CustomerID = item;
                                    PieceOfEquipmentGRPC poegrpc = new PieceOfEquipmentGRPC(_poeServices, DbFactory, con.CallOptions);
                                    var poesByCustomer = await poegrpc.GetPieceOfEquipmentByCustomerId(customer);

                                        foreach (var poe in poesByCustomer.PieceOfEquipments.Where(x => x.EquipmentTemplate?.EquipmentTypeGroupID == currentWod.PieceOfEquipment?.EquipmentTemplate?.EquipmentTypeObject?.EquipmentTypeGroupID).ToList())
                                        {


                                       var value = poe?.PieceOfEquipmentID + "-" + poe?.CustomerId;
                                       KeyValueOption key = new KeyValueOption()
                                       {
                                           Key = "POE-" + (poe?.PieceOfEquipmentID.ToString() ?? "0"),
                                           Value = value
                                       };
                                        compositeWods.Add(key);

                                    }
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error in LoadFilteredWorkOrderDetailList: {ex.Message}");
                listWods = new List<WorkOrderDetail>();
                compositesIds = new List<int>();
            }
        }

        /// <summary>
        /// Gets all WorkOrderDetails for a specific WorkOrder
        /// </summary>
        private async Task<List<WorkOrderDetail>> GetWorkOrderDetailsForWorkOrder(int workOrderId)
        {
            try
            {
                // Use the existing WorkOrderDetailGrpc service
                var wodService = new WorkOrderDetailGrpc(Client, DbFactory, (await CallOptions(Component.Name)).CallOptions);
                var workOrder = new WorkOrder { WorkOrderId = workOrderId };

                var result = await wodService.GetWorkOrderDetailXWorkOrder(workOrder);
                return result?.WorkOrderDetails ?? new List<WorkOrderDetail>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting WorkOrderDetails for WorkOrder {workOrderId}: {ex.Message}");
                return new List<WorkOrderDetail>();
            }
        }
        public List<KeyValueOption> GetStandardComponentArray()
        {


            if (!string.IsNullOrEmpty(EquipmentTypeObject.StandardComponent)) 
            {
                
                var nvc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KeyValueOption>>(EquipmentTypeObject.StandardComponent);
                
                return nvc;

            }

            return new List<KeyValueOption>(); 
        }

        public List<KeyValueOption> GetJSONConfigurationArray()
        {


            if (!string.IsNullOrEmpty(EquipmentTypeObject?.JSONConfiguration))
            {
               
                var nvc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KeyValueOption>>(EquipmentTypeObject.JSONConfiguration);
                return nvc;

            }

            return null;
        }



        public IEnumerable<Address> Addresses { get; set; }

        [Parameter]
        public EventCallback<ChangeEventArgs> OnChangeDescription { get; set; }

        public string Description { get; set; }



        public async Task ClearWeightSet(ChangeEventArgs arg)
        {


            var poq = new PieceOfEquipment();

            WeightSetList2 = new List<WeightSet>();

            poq.WeightSets = new List<WeightSet>();

            eq.WOD_Weights = new List<WOD_Weight>();

            await SetWeights(poq);


            //WeightSet WeightSetdelete;
            //if (arg.Value != null)
            //{
            //    WeightSetdelete= (WeightSet)arg.Value;

            //    var poq = new PieceOfEquipment();

            //    WeightSetList2.Remove(WeightSetdelete);

            //    poq.WeightSets = new List<WeightSet>();

            //    eq.WOD_Weights = new List<WOD_Weight>();

            //    await SetWeights(poq);

            //}
            //else
            //{
               
            //}

          




        }


        public async Task ChangeDescripcion(ChangeEventArgs arg)
        {

            var poq = ((PieceOfEquipment)arg.Value);
            await SetWeights(poq);

            //WeightSetComponent.
            StateHasChanged();

        }



        private async Task SetWeights(PieceOfEquipment poq)
        {

            Console.WriteLine("SetWeights..................................................");


            List<WOD_Weight> lsww = new List<WOD_Weight>();
            if (poq?.WeightSets == null || poq?.WeightSets?.Count == 0)
            {
                return;
            }
            foreach (var item in poq.WeightSets)
            {
                WOD_Weight wod = new WOD_Weight();

                wod.WorkOrderDetailID = eq.WorkOrderDetailID;
                wod.WeightSetID = item.WeightSetID;
                wod.WeightSet = item;
                //wod.WeightSet.PieceOfEquipment = poq;
                lsww.Add(wod);
            }

            WeightSetList2 = poq.WeightSets.ToList();

            eq.WOD_Weights = lsww;

            Description += ((PieceOfEquipment)poq).PieceOfEquipmentID + " | ";

            ChangeEventArgs arg = new ChangeEventArgs();

            arg.Value = poq;
            if(weigset != null && weigset.Count > 0)
            {
                await weigset.ElementAtOrDefault(0).CancelItem();
            }
            

            await OnChangeDescription.InvokeAsync(arg);

            if (Linearity != null && Linearity?.WeightSetComponent != null ) //&& Linearity.WeightSetComponent.ChangeDescription.HasDelegate)
            {
                //await Linearity.WeightSetComponent.ChangeDescription.InvokeAsync(arg);
                await Linearity.WeightSetComponent.ChangeAdd(arg);
            }




            if (Compresion != null && Compresion?.WeightSetComponent != null ) //&& Compresion.WeightSetComponent.ChangeDescription.HasDelegate)
            {
                //await Compresion.WeightSetComponent.ChangeDescription.InvokeAsync(arg);

                await Compresion.WeightSetComponent.ChangeAdd(arg);
            }




            if (EccentricityComponent != null && EccentricityComponent.WeightSetComponent != null)// && EccentricityComponent.WeightSetComponent.ChangeDescription.HasDelegate)
            {
                //await EccentricityComponent.WeightSetComponent.ChangeDescription.InvokeAsync(arg);
                await EccentricityComponent.WeightSetComponent.ChangeAdd(arg);
            }


            if (RepeabilityComponent != null && RepeabilityComponent.WeightSetComponent != null) // && RepeabilityComponent.WeightSetComponent.ChangeDescription.HasDelegate)
            {
                // await RepeabilityComponent.WeightSetComponent.ChangeDescription.InvokeAsync(arg);
                await RepeabilityComponent.WeightSetComponent.ChangeAdd(arg);
            }




        }



        public string UsersAccess { get; set; } = "HasAccess";

        public List<string> Possibilities = new List<string>();

        public bool LinearityShow { get; set; }
        public bool RepeatibilityShow { get; set; }
        public bool EccentricityShow { get; set; }
      
        public string visibleModal { get; set; } = "invisible";
 
        public async Task ShowTimeLine()
        {

            var parameters = new ModalParameters();
            parameters.Add("WorkOrderDetail", eq);

            var messageForm = Modal.Show<BlazorApp1.Blazor.Blazor.Pages.Order.WorkOrderDetailHist_Search>("TimeLine", parameters);
            var result = await messageForm.Result;

            if (!result.Cancelled)
                _message = result.Data?.ToString() ?? string.Empty;
              PieceOfEquipment PieceOfEquipment = (PieceOfEquipment)result.Data;


        }



        public async Task ShowCertified()
        {

            var parameters = new ModalParameters();
            parameters.Add("SelectOnly", true);
            parameters.Add("IsModal", true);
            parameters.Add("POE", eq.PieceOfEquipment);
            var messageForm = Modal.Show<BlazorApp1.Blazor.Pages.Order.Certified_Search>("Certifies", parameters);
            var result = await messageForm.Result;

            if (!result.Cancelled)
                _message = result.Data?.ToString() ?? string.Empty;


        }


        public string convert(string item)
        {

            return null;
        }

        public void CloseSetTestPoint(ChangeEventArgs arg)
        {

            if (arg.Value != null)
            {

                ModalResult result = (ModalResult)arg.Value;

                if (result.Data != null)
                {

                    EquipmentTemplate e = (EquipmentTemplate)result.Data;

                    eq.PieceOfEquipment.EquipmentTemplate = e;

                }

            }
        }

        public async Task Refresh()
        {
            await InvokeAsync(StateHasChanged);
            
        }


        public Modal ModalLinearity { get; set; }
    public Modal ModalLRepeatibility { get; set; }
    public Modal ModalLEccentricity { get; set; }
    

        public async Task ShowTests()
    {
 
        LinearityShow = true;
        RepeatibilityShow = true;
        EccentricityShow = true;
     
            await ScrollPosition();
        await ScrollPosition();
    }


    public async Task ShowLinearity()
    {


        if (CalibrationInstructions.changeTolerance && CalibrationInstructions?.ResolutionComponent != null)
        {


            eq.Resolution = CalibrationInstructions.ResolutionComponent.eq.Resolution;
            eq.Tolerance.AccuracyPercentage = CalibrationInstructions.ResolutionComponent.eq.Tolerance.AccuracyPercentage;
            eq.DecimalNumber = CalibrationInstructions.ResolutionComponent.eq.DecimalNumber;
            eq.Tolerance.ToleranceTypeID = CalibrationInstructions.ResolutionComponent.eq.Tolerance.ToleranceTypeID.Value;
                eq.Tolerance.Resolution = CalibrationInstructions.ResolutionComponent.eq.Resolution;
                if (CalibrationInstructions.eq != null && CalibrationInstructions.eq.IsAccredited.HasValue)
                {
                    eq.IsAccredited = CalibrationInstructions.eq.IsAccredited.Value;
                }
                if (CalibrationInstructions?.ResolutionComponent?.eq.IsComercial==true)
                {
                    eq.IsComercial = CalibrationInstructions.ResolutionComponent.eq.IsComercial=true;
                }
                else if(CalibrationInstructions?.ResolutionComponent?.eq.IsComercial == false)
                {
                    eq.IsComercial = CalibrationInstructions.ResolutionComponent.eq.IsComercial = false;
                }
            eq.ClassHB44 = CalibrationInstructions.ResolutionComponent.eq.ClassHB44;
            eq.Tolerance.ToleranceFixedValue = CalibrationInstructions.ResolutionComponent.eq.Tolerance.ToleranceFixedValue;
            eq.Tolerance.ToleranceValue = CalibrationInstructions.ResolutionComponent.eq.Tolerance.ToleranceValue;
               
                ChangeResolution = true;
        }
        else
        {
            ChangeResolution = false;
        }
         eq.Ranges = new List<RangeTolerance>();



        visibleModal = "";
        LinearityShow = true;
        RepeatibilityShow = false;
        EccentricityShow = false;
        ModalLEccentricity.Hide();

        ModalLRepeatibility.Hide();
 
        if (ModalLinearity != null)
        {
            ModalLinearity.Toggle();
        }
   
            StateHasChanged();

    }

    public async Task ShowRepeatibility()
    {

        LinearityShow = false;
        RepeatibilityShow = true;
        EccentricityShow = false;
           
            ModalLinearity.Hide();

            ModalLEccentricity.Hide();

            ModalLRepeatibility.Toggle();
           
            StateHasChanged();

    }

       

        public async Task ShowEccentricity()
    {

        LinearityShow = false;
        RepeatibilityShow = false;
        EccentricityShow = true;
            
            ModalLinearity.Hide();
           
        ModalLEccentricity.Toggle();

        ModalLRepeatibility.Hide();
        StateHasChanged();

    }


        public async Task CloseLinearity()
    {


            Console.WriteLine("closelinerity " + Linearity.RT.Items.Count);
        var linearities = Linearity.RT.Items;


        if (eq?.BalanceAndScaleCalibration == null)
        {
            eq.BalanceAndScaleCalibration = new BalanceAndScaleCalibration();
        }

        eq.BalanceAndScaleCalibration.Linearities = linearities;

    }
	
	
	  public async Task CloseRepeatibility()
    {

        //var linearities = RepeabilityComponent.Entity;

        //linearities.BasicCalibrationResult = RepeabilityComponent.RT.Items;
        //if (eq?.BalanceAndScaleCalibration == null)
        //{
        //    eq.BalanceAndScaleCalibration = new BalanceAndScaleCalibration();
        //}

        ////eq.BalanceAndScaleCalibration.Repeatability = linearities;

            ///////////
            ///
            var linearities = RepeabilityComponent.Entity;

            if (RepeabilityComponent?.RT?.Items?.Count >= 0)
            {
                Console.WriteLine("CloseRepeatibility" + RepeabilityComponent.RT.Items.Count);
                linearities.TestPointResult = RepeabilityComponent.RT.Items;
            }
            else
            {
                linearities.TestPointResult = null;
            }




            if (eq?.BalanceAndScaleCalibration == null)
            {
                eq.BalanceAndScaleCalibration = new BalanceAndScaleCalibration();
            }

            eq.BalanceAndScaleCalibration.Repeatability = linearities;

        }



        public async Task SaveEccentricity()
        {

            try
            {

                await ShowProgress();

                await CloseEccentricity();


                await SetCurrentStatus(eq.CurrentStatus, true);


            }
            catch (Exception ex)
            {

                await ExceptionManager(ex);


            }
            finally
            {
                await CloseProgress();

            }

        }


        public async Task SaveAndCloseEccentricity()
        {

            try
            {

                await ShowProgress();

                await CloseEccentricity();


                await SetCurrentStatus(eq.CurrentStatus, true);


                if(ModalLEccentricity != null)
                {
                    ModalLEccentricity.Hide();

                }

                if (ModalLinearity != null)
                {
                    ModalLinearity.Hide();

                }


                if (ModalLRepeatibility != null)
                {
                    ModalLRepeatibility.Hide();

                }



            }
            catch (Exception ex)
            {

                await ExceptionManager(ex);


            }
            finally
            {
                await CloseProgress();

            }

        }


        public async Task SaveRepeateability()
        {

            try
            {

                await ShowProgress();

                await CloseRepeatibility();


                await SetCurrentStatus(eq.CurrentStatus, true);


            }
            catch (Exception ex)
            {

                await ExceptionManager(ex);


            }
            finally
            {
                await CloseProgress();

            }

        }


        public async Task SaveAndCloseRepeateability()
        {

            try
            {

                await ShowProgress();

                await CloseRepeatibility();


                await SetCurrentStatus(eq.CurrentStatus, true);


                if (ModalLEccentricity != null)
                {
                    ModalLEccentricity.Hide();

                }

                if (ModalLinearity != null)
                {
                    ModalLinearity.Hide();

                }


                if (ModalLRepeatibility != null)
                {
                    ModalLRepeatibility.Hide();

                }

            }
            catch (Exception ex)
            {

                await ExceptionManager(ex);


            }
            finally
            {
                await CloseProgress();

            }

        }



        public async Task SaveLinearity()
        {

            try
            {

                await ShowProgress();

                await CloseLinearity();


                await SetCurrentStatus(eq.CurrentStatus, true);


            }
            catch (Exception ex)
            {

                await ExceptionManager(ex);


            }
            finally
            {
                await CloseProgress();

            }

        }


        public async Task SaveAndCloseLinearity()
         {

            try
            {

                await ShowProgress();

                await CloseLinearity();


                await SetCurrentStatus(eq.CurrentStatus, true);


                if (ModalLEccentricity != null)
                {
                    ModalLEccentricity.Hide();

                }

                if (ModalLinearity != null)
                {
                    ModalLinearity.Hide();

                }


                if (ModalLRepeatibility != null)
                {
                    ModalLRepeatibility.Hide();

                }
   
            }
            catch (Exception ex)
            {

                await ExceptionManager(ex);


            }
            finally
            {
                await CloseProgress();

            }

        }




        public async Task CloseEccentricity()
    {

        EccentricityShow = false;

        var linearities = EccentricityComponent.Entity;

        if (EccentricityComponent?.RT?.Items?.Count >= 0)
        {
            Console.WriteLine("CloseEccentricity" + EccentricityComponent.RT.Items.Count);
            linearities.TestPointResult = EccentricityComponent.RT.Items;
        }
        else
        {
            linearities.TestPointResult = null;
        }




        if (eq?.BalanceAndScaleCalibration == null)
        {
            eq.BalanceAndScaleCalibration = new BalanceAndScaleCalibration();
        }

        eq.BalanceAndScaleCalibration.Eccentricity = linearities;

    }

        public NoteWOD RowChange(NoteWOD lin)
        {
            if (RT1.IsDragAndDrop)
            {
                int posi = 100;
                
                for (int i = 0; i < RT1.ItemList.Count; i++)
                {
                    RT1.ItemList[i].Position = posi;
                    posi++;
                    RT1.ReplaceItemKey(RT1.ItemList[i]);
                }

                return lin;
            }

            if (lin.Position >= 100)
            {
                return lin;
            }

            return lin;
        }


        public async Task<CalibrationSubType> CalibrationConfig(CreateModel CalibrationType)
        {
            var parameters = new ModalParameters();


            parameters.Add("NewPerson", CalibrationType);
            //parameters.Add("IsModal", true);

            ModalOptions op = new ModalOptions();
            op.ContentScrollable = true;
            op.Class = "blazored-modal " + ModalSize.MediumWindow;

            var messageForm = Modal.Show<CreateModelForm>("Config", parameters, op);
            var result = await messageForm.Result;

            if (!result.Cancelled)
            {

            }

            return null;
        }


        public string GetCalibrationInstruccionsType(WorkOrderDetail eq)
        {

            if (!string.IsNullOrEmpty(eq?.PieceOfEquipment?.EquipmentTemplate?.GetEquipmentType(eq)?.CalibrationInstrucctions))//EquipmentTypeObject?.CalibrationInstrucctions))
            {
                var res = eq?.PieceOfEquipment?.EquipmentTemplate?.GetEquipmentType(eq).CalibrationInstrucctions?.ToLower();
                return res;
            }

            return string.Empty;
            
        }

  
      
            [Inject]
        public HttpClient HttpClient { get; set; }
        //public ICollection<WorkOrderDetail> listWods { get ; set ; }

        public async Task CancelWOD()
        {
            var res = await ConfirmAsync("Please confirm that you want to cancel this WOD, data will not be deleted but it will be considered completed as cancelled");

            //
            if (res)
            {
                eq.OnlyChngeStatus = true;
                eq.CurrentStatusID = 5;

                var all = await wod.ChangeStatus(eq);


                await ShowToast("Cancel is success",ToastLevel.Success);


            }

          

        }

        public async Task OpenWOD()
        {
            var res = await ConfirmAsync("Do you want to re open this Work Order Detail?");

            //
            if (res)
            {
                eq.OnlyChngeStatus = true;
                eq.CurrentStatusID = 1;

                var all = await wod.ChangeStatus(eq);

                await ShowToast("Open is success", ToastLevel.Success);

            }





        }

        public async Task UploadtoExcel()
        {
            try
            {
                await ShowProgress();
                eq.TestPointResult = null;
                eq.BalanceAndScaleCalibration.TestPointResult = null;               

                var result = await wod.GetInstrumentThread(eq, new CallContext());

                if (result != null)
                {

                    urlWebExcel = result.UrlWebFile;
                    _message = "File " + result.FileName + "_" + result.WodId + ".xlsx, updated";
                }
                else
                {
                    _message = "Error ";
                }


            }
            catch (Exception ex)
            {
                await ExceptionManager(ex);
            }
            finally
            {
                await CloseProgress();
            }
        }

        public async Task ResetWOD()
        {
            try
            {

                await ShowProgress();

                var DTO = eq;
                //await searchComponent.ShowModalAction();

                if (DTO.CurrentStatusID !=2)
                {
                    //throw new Exception("Work Order Detail is being used, Only Work Order Detail in contract review status can be deleted ");

                    return;
                
                }

                bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "This will delete all the information entered in 'Ready for Calibration' status and will reset the Work Order Detail to the 'Contract Review' status. Are you sure that you want to continue. This action cannot be undone");
                if (!confirmed)
                {
                    return;
                }

                    WorkOrderDetail w = new WorkOrderDetail();

                w.WorkOrderDetailID = DTO.WorkOrderDetailID;

                WorkOrderDetailGrpc cl = new WorkOrderDetailGrpc(Client, DbFactory);


                var result = await cl.Reset(w, new CallContext());



                if (result != null)
                {
                    

                    await ShowToast("Succesfully", ToastLevel.Success);

                    NavigationManager.NavigateTo(NavigationManager.Uri, true);



                }

                return ;
                // searchComponent.ShowResult();
            }
            catch (RpcException ex)
            {

                await ExceptionManager(ex);
                //return false;

            }
            catch (Exception ex)
            {
                await ExceptionManager(ex);
                //return false;

            }
            finally
            {
                await CloseProgress();
            }

        }

                public async Task DownloadResults()
        {
            try
            {

                await ShowProgress();
                eq.TestPointResult = null;
                eq.BalanceAndScaleCalibration.TestPointResult = null;


                WorkOrderDetailGrpc wod = new WorkOrderDetailGrpc(Client, DbFactory, (await CallOptions(Component.Name)).CallOptions);
                var result = await wod.GetResultsTable(eq, new CallContext());

                if (result != null)
                {
                    eq.BalanceAndScaleCalibration.TestPointResult = result; 
                }
                else
                {
                    eq.BalanceAndScaleCalibration.TestPointResult = null;
                }


            }
            catch (Exception ex)
            {
                await ExceptionManager(ex);
            }
            finally
            {
                await CloseProgress();
            }
        }
    }
    }
