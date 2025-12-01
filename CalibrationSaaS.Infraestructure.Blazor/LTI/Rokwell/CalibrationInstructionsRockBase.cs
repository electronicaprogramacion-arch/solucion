using Blazor.IndexedDB.Framework;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics.TestPoints;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazed.Controls.Toast;
using System.Linq;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Order;
using Modal = Blazed.Controls.Modal;
using CalibrationSaaS.Infraestructure.Blazor.Shared;
using Microsoft.Extensions.Configuration;

namespace CalibrationSaaS.Infraestructure.Blazor
{
    public class CalibrationInstructionsRockBase : CalibrationInstructionsBase_, ICalibrationInstructionsLTIBase
    {


   
        [Inject]
        public dynamic DbFactory { get; set; }
       
        [Inject]
       public IConfiguration Configuration { get; set; }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task ChangeSwitch(ChangeEventArgs e)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {





        }


        public bool Enabled { get; set; } = true;
        public bool LabelDown { get; set; }

        public string Customer { get; set; }

        public int? ToleranceTypeID { get; set; }
        public EquipmentType EquipmentTypeObject { get; set; }

        [Parameter]
        public WorkOrderDetail eq { get; set; }
#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.


        public List<RangeTolerance> Ranges = new List<RangeTolerance>();

        public RangeTestPoint RangeComponent { get; set; }

        public RangeTestPoint RangeAccuracy { get; set; }

        public bool IsAccreditedDisable { get; set; }
        public string BoundValue(WorkOrderDetail eq)
        {
            if (eq != null)
            { 
            ToleranceTypeID = WorkOrderItemCreate.eq.Tolerance.ToleranceTypeID;
                EquipmentTypeObject = eq.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject;

            }
            try
            {
                Custom = false;
                
                if (!eq.CalibrationDate.HasValue)
                {
                    return "";
                }


                if (eq.CalibrationIntervalID == 1)
                {
                    Custom = true;
                }

                if (eq.CalibrationIntervalID == 2)
                {
                    var cd = eq.CalibrationDate;

                    cd = cd.Value.AddMonths(1);

                    eq.CalibrationCustomDueDate = cd;
                }
                if (eq.CalibrationIntervalID == 3)
                {
                    var cd = eq.CalibrationDate;

                    cd = cd.Value.AddMonths(2);

                    eq.CalibrationCustomDueDate = cd;
                }

                if (eq.CalibrationIntervalID == 2 + 2)
                {
                    var cd = eq.CalibrationDate;

                    cd = cd.Value.AddMonths(3);

                    eq.CalibrationCustomDueDate = cd;
                }

                if (eq.CalibrationIntervalID == 3 + 2)
                {
                    var cd = eq.CalibrationDate;

                    cd = cd.Value.AddMonths(6);

                    eq.CalibrationCustomDueDate = cd;
                }
                if (eq.CalibrationIntervalID == 4 + 2)
                {
                    var cd = eq.CalibrationDate;

                    cd = cd.Value.AddMonths(12);

                    eq.CalibrationCustomDueDate = cd;
                }

                if (eq.EndOfMonth == true)
                {
                    DateTime _dueDate = DateTime.Now;
                    if (eq.CalibrationCustomDueDate != null)
                        _dueDate = (DateTime)eq.CalibrationCustomDueDate;
                    
                        eq.CalibrationCustomDueDate = LastDayOfMonth(_dueDate);
                   
                }

                
                
            }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            {

            }

            return "";
        }

        //[Inject] public Application.Services.IBasicsServices<CallContext> Service { get; set; }
        [Inject]
        public Func<dynamic, Application.Services.IBasicsServices<CallContext>> Service { get; set; }

        public ICollection<Certification> Certifications { get; set; }


       

        public bool ShowTech { get; set; }
        //[Inject] public IJSRuntime JSRuntime { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {

                //await ShowProgress();
                if (firstRender)
                {
                    
                    if (AppState != null)
                    {

                    }

                    //var um = AppState.UnitofMeasureList.Where(x => x.TypeID == 2).ToList();
                    //if (um != null && um.Count > 0)
                    //{
                    //    HumidityUnitofMeasureList = um;
                    //}


                    //var umt = AppState.UnitofMeasureList.Where(x => x.TypeID == 1).ToList();
                    //if (umt != null && umt.Count > 0)
                    //{
                    //    TemperatureUnitofMeasureList = umt;
                    //}
                    if (Client != null)
                    {
                        AssetsServiceGRPC assets = new AssetsServiceGRPC(Client, DbFactory);

                        var _calibType = await assets.GetCalibrationType();
                        _calibrationTypeList = _calibType.CalibrationTypes;
                    }



                    if (firstRender)
                    {
                        await JSRuntime.InvokeVoidAsync("removeValidClass");
                    }
                    else
                    {

                    }





                }


                if (!firstRender && ShowTech)
                {
                    try
                    {
                        if (WorkOrderItemCreate.eq?.WorkOder?.UserWorkOrders == null || WorkOrderItemCreate.eq?.WorkOder?.UserWorkOrders?.Count == 0)
                        {
                            await ShowToast("No technicians assigned to the Work Order to be selected in the Work Order Detail", ToastLevel.Warning);
                            //No technicians assigned to the Work Order to be selected in the Work Order Detail
                        }
                    }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                    catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                    {

                    }
                }

                await base.OnAfterRenderAsync(firstRender);
            }

#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            {

            }
            finally
            {
                //await CloseProgress();
            }
        }




        //[Inject]
        //public Application.Services.IAssetsServices<CallContext> Client { get; set; }

        [Inject]
        public Func<dynamic, Application.Services.IAssetsServices<CallContext>> Client { get; set; }


        public List<CalibrationType> _calibrationTypeList { get; set; } = new List<CalibrationType>();

        //[Parameter]
        //public Domain.Aggregates.Entities.Status CurrentStatus { get; set; } = new Domain.Aggregates.Entities.Status();

       


        [Inject] public AppStateCompany AppState { get; set; }

        [Inject] public AppState AppStateBasics { get; set; }

        //[Inject] IAssetsServices<CallContext> TechniciansService { get; set; }

        //public WorkOrderDetail WorkOrderDetail { get; set; } = new WorkOrderDetail();

        public PieceOfEquipment PieceOfEquipment { get; set; } = new PieceOfEquipment();

        public EquipmentTemplate EquipmentTemplate { get; set; } = new EquipmentTemplate();

        public List<User> Technicians { get; set; }


        //public List<UnitOfMeasure> HumidityUnitofMeasureList { get; set; }

        //public List<UnitOfMeasure> TemperatureUnitofMeasureList { get; set; }

        [Parameter]
        public string UsersAccess { get; set; }


        public List<UnitOfMeasure> UnitofMeasureList { get; set; }





        protected override async Task OnInitializedAsync()
        {



            Customer = Configuration.GetSection("Reports")["Customer"];

            await base.OnInitializedAsync();



            Certifications = AppSecurity.CertificationList.Where(x=>x.CalibrationTypeID==WorkOrderItemCreate.eq.CalibrationTypeID).ToList(); // result as ICollection<Certification>;

          

            UsersAccess = WorkOrderItemCreate.UsersAccess;

            if (WorkOrderItemCreate.eq != null)
            {
                ToleranceTypeID = WorkOrderItemCreate.eq.Tolerance.ToleranceTypeID;

                if (WorkOrderItemCreate.eq.CurrentStatusID <= 2)
                {
                    IsAccreditedDisable = false;

                }
                else
                {
                    IsAccreditedDisable = true;
                }
            }
            StateHasChanged();

            }

#pragma warning disable CS0414 // El campo 'CalibrationInstructionsBase.selectedAnswer' está asignado pero su valor nunca se usa
            string selectedAnswer = "";
#pragma warning restore CS0414 // El campo 'CalibrationInstructionsBase.selectedAnswer' está asignado pero su valor nunca se usa
#pragma warning disable CS0414 // El campo 'CalibrationInstructionsBase.IsPercentage' está asignado pero su valor nunca se usa
        //bool IsPercentage = false;
#pragma warning restore CS0414 // El campo 'CalibrationInstructionsBase.IsPercentage' está asignado pero su valor nunca se usa


        public string ToleranceTypeChanged(WorkOrderDetail args)
        {
            var res = ((WorkOrderDetail)editContext.Model).Tolerance.ToleranceTypeID.ToString();



            if ((string)res == "1")
            {
                IsPercentage = true;
            }
            else
            {
                IsPercentage = false;
            }

            return "";
        }






        public void SelectionChanged(ChangeEventArgs args)
        {

            var res = args.Value;



            if ((string)res == "2")
            {
                IsPercentage = true;
            }
            else
            {
                IsPercentage = false;
            }

            // StateHasChanged();
        }


        public void ChangeDate(ChangeEventArgs args)
        {

            var ct = WorkOrderItemCreate.eq.CalibrationIntervalID;


        }


        public bool Custom = false;


        public async Task ChangeCalibrationType(ChangeEventArgs args)
        {
            //if (RefreshParent != null)
            //{
            //    await RefreshParent();
            //}

            //await WorkOrderItemCreate.Refresh();
            if (JSRuntime != null)
            {
                await JSRuntime.InvokeVoidAsync("Collapse", "ScaleCalibration");
            }
            //Console.WriteLine(WorkOrderItemCreate.eq.CalibrationTypeID);
        }

        public async Task ChangeEndOfMonth(ChangeEventArgs args)
        {
            DateTime _dueDate = DateTime.Now;
            if (eq.CalibrationCustomDueDate != null)
                _dueDate = (DateTime)eq.CalibrationCustomDueDate;
            if (eq.EndOfMonth == true)
            {
                eq.CalibrationCustomDueDate = LastDayOfMonth(_dueDate);
            }
            if (JSRuntime != null)
            {
                await JSRuntime.InvokeVoidAsync("Collapse", "ScaleCalibration");
            }
        
        }
        public DateTime LastDayOfMonth(DateTime inDate)
        {
            var daysInMonth = DateTime.DaysInMonth(inDate.Year, inDate.Month);
            return new DateTime(inDate.Year, inDate.Month, daysInMonth);
        }

        public void ChangeList(ChangeEventArgs args)
        {

            var res = args.Value;
            if (res.ToString() == "1")
            {
                Custom = true;
            }
            else
            {
                Custom = false;
            }
            //StateHasChanged();
        }

        public Modal ModalResolution { get; set; }
        public bool ResolutionShow { get; set; }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task ShowResolution()
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            ResolutionShow = true;


            ModalResolution.Toggle();


            StateHasChanged();

        }

//        public void LoadConditionData()
//        {
//            LoadConditionData(WorkOrderItemCreate.eq);
//        }


//        public void LoadConditionData(WorkOrderDetail eq)
//        {
//            if (eq != null && (eq?.CalibrationTypeID == null || eq?.CalibrationTypeID == 0) && eq?.WorkOder?.CalibrationType > 0)
//            {
//                eq.CalibrationTypeID = eq.WorkOder.CalibrationType;
//            }



//            var equipmentTemplate = new EquipmentTemplate();
//            if (eq.PieceOfEquipment != null && eq.PieceOfEquipment.IsToleranceImport)
//            {
//                var q = eq.PieceOfEquipment;
//                equipmentTemplate.Tolerance.ToleranceTypeID = q.Tolerance.ToleranceTypeID;
//                equipmentTemplate.Resolution = q.Resolution;
//                equipmentTemplate.Tolerance.AccuracyPercentage = q.Tolerance.AccuracyPercentage;
//                equipmentTemplate.DecimalNumber = q.DecimalNumber;
//                equipmentTemplate.ClassHB44 = q.ClassHB44;
//                equipmentTemplate.Ranges = q.Ranges;
//                equipmentTemplate.Tolerance.ToleranceFixedValue = q.Tolerance.ToleranceFixedValue;
//            }
//            else if (!eq.PieceOfEquipment.IsToleranceImport && eq?.PieceOfEquipment?.EquipmentTemplate != null)
//            {
//                equipmentTemplate = eq.PieceOfEquipment.EquipmentTemplate;
//            }
//            //= eq?.PieceOfEquipment?.EquipmentTemplate;
//            if (eq != null && eq.Tolerance.ToleranceTypeID == 0 && equipmentTemplate?.Tolerance?.ToleranceTypeID > 0)
//            {
//                eq.Tolerance.ToleranceTypeID = equipmentTemplate.Tolerance.ToleranceTypeID.Value;
//            }
//            if (eq != null && eq.Tolerance.AccuracyPercentage == 0 && equipmentTemplate?.Tolerance?.AccuracyPercentage > 0)
//            {
//                eq.Tolerance.AccuracyPercentage = equipmentTemplate.Tolerance.AccuracyPercentage;
//            }
//            if (eq != null && eq.Resolution == 0 && equipmentTemplate?.Resolution > 0)
//            {
//                eq.Resolution = equipmentTemplate.Resolution;
//            }
//            if (eq != null && eq.ClassHB44 == 0 && equipmentTemplate?.ClassHB44 > 0)
//            {
//                eq.ClassHB44 = equipmentTemplate.ClassHB44;
//            }
//            if (eq != null && eq.DecimalNumber == 0 && equipmentTemplate?.DecimalNumber > 0)
//            {
//                eq.DecimalNumber = equipmentTemplate.DecimalNumber;
//            }
//            if (eq != null && (eq?.Tolerance?.ToleranceFixedValue == 0 || eq.Tolerance.ToleranceFixedValue==null) && equipmentTemplate?.Tolerance?.ToleranceFixedValue > 0)
//            {
//                eq.Tolerance.ToleranceFixedValue = equipmentTemplate.Tolerance.ToleranceFixedValue;
//            }
//            //TODO falata codigo para ranges
//            if (equipmentTemplate != null && equipmentTemplate?.Ranges?.Count > 0)
//            {
//                eq.Ranges = equipmentTemplate?.Ranges;
//            }
//            //eq.IsComercial = eq.PieceOfEquipment.EquipmentTemplate.IsComercial;
//            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//            if (eq?.Tolerance?.ToleranceTypeID == 2)
//            {
//                IsPercentage = true;
//            }
//            if (eq == null || !eq.CalibrationDate.HasValue || eq.CalibrationDate.Value.Year == 1)
//            {
//                eq.CalibrationDate = DateTime.Now;
//            }
//            if (eq == null || !eq.CalibrationDate.HasValue || eq.CalibrationDate.Value.Year == 1)
//            {
//                eq.CalibrationNextDueDate = DateTime.Now;
//            }
//            //ypp
//            if (ResolutionComponent != null)
//            {
//                EquipmentTemplate et = new EquipmentTemplate();
//                et.Tolerance.ToleranceTypeID = eq.Tolerance.ToleranceTypeID;
//                et.Tolerance.AccuracyPercentage = eq.Tolerance.AccuracyPercentage;
//                et.Resolution = eq.Resolution;
//                et.DecimalNumber = eq.DecimalNumber;
//                et.Ranges = eq.Ranges;
//                et.ClassHB44 = eq.ClassHB44;
//                et.Tolerance.ToleranceFixedValue = eq.Tolerance.ToleranceFixedValue;
//#pragma warning disable CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
//                ResolutionComponent.Show(et);
//#pragma warning restore CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
//            }
//        }


        [Parameter]
        public Func<Task> RefreshParent { get; set; }


        public async Task Refresh()
        {



        }

    }
}
