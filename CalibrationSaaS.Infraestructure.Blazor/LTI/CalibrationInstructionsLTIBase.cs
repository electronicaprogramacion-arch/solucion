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

namespace CalibrationSaaS.Infraestructure.Blazor
{
    public class CalibrationInstructionsLTIBase : CalibrationInstructionsBase_, ICalibrationInstructionsLTIBase
    {

       




        [Inject]
        public dynamic DbFactory { get; set; }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task ChangeSwitch(ChangeEventArgs e)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {





        }


        public bool Enabled { get; set; } = true;
        public bool LabelDown { get; set; }



        

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
//        public async Task CloseWindow()
//#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
//        {

//            Console.WriteLine("CloseWindow CALINStruc");

//            Console.WriteLine(WorkOrderItemCreate.eq.Resolution);

//            Console.WriteLine(ResolutionComponent.eq.Resolution);

//            Console.WriteLine(ResolutionComponent.ResolutionChanged);


            

//            if (WorkOrderItemCreate.eq.IsComercial != ResolutionComponent.eq.IsComercial)
//            {
//                changeTolerance = true;
//                WorkOrderItemCreate.eq.IsComercial = ResolutionComponent.eq.IsComercial;
//            }
//            if (WorkOrderItemCreate.eq.Tolerance.ToleranceTypeID != ResolutionComponent.Tolerance.ToleranceTypeID.Value)
//            {
//                changeTolerance = true;
//                WorkOrderItemCreate.eq.Tolerance.ToleranceTypeID = ResolutionComponent.Tolerance.ToleranceTypeID.Value;
//            }

//            if (WorkOrderItemCreate.eq.Resolution != ResolutionComponent.eq.Resolution)
//            {
//                changeTolerance = true;
//                WorkOrderItemCreate.eq.Resolution = ResolutionComponent.eq.Resolution;
//            }

//            if (WorkOrderItemCreate.eq.DecimalNumber != ResolutionComponent.eq.DecimalNumber)
//            {
//                changeTolerance = true;
//                WorkOrderItemCreate.eq.DecimalNumber = ResolutionComponent.eq.DecimalNumber;
//            }

//            if (WorkOrderItemCreate.eq.ClassHB44 != ResolutionComponent.eq.ClassHB44)
//            {
//                changeTolerance = true;
//                WorkOrderItemCreate.eq.ClassHB44 = ResolutionComponent.eq.ClassHB44;
//            }

//            //eq.IsComercial = ResolutionComponent.eq.IsComercial;

//            //eq.Resolution= ResolutionComponent.eq.Resolution;

//            WorkOrderItemCreate.ChangeResolution = changeTolerance | ResolutionComponent.ResolutionChanged;

//            if (changeTolerance)
//            {
//                WorkOrderItemCreate.eq.Tolerance = ResolutionComponent.Tolerance;
//            }
           

//            Console.WriteLine("closewindow" + WorkOrderItemCreate.ChangeResolution );

//              if (JSRuntime != null)
//            {
//                await JSRuntime.InvokeVoidAsync("Collapse", "ScaleCalibration");
//            }
//            Console.WriteLine(WorkOrderItemCreate.eq.GetCalibrationTypeID());

//            StateHasChanged();

//        }

        public List<RangeTolerance> Ranges = new List<RangeTolerance>();

        public RangeTestPoint RangeComponent { get; set; }

        public RangeTestPoint RangeAccuracy { get; set; }

        
        public string BoundValue(WorkOrderDetail eq)
        {
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

        public bool IsAccreditedDisable { get; set; }


        public bool ShowTech { get; set; }
        //[Inject] public IJSRuntime JSRuntime { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {

                //await ShowProgress();
                if (firstRender)
                {

                   
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

        //9503
        //[CascadingParameter(Name ="CascadeParam1")]
        //public  IWorkOrderItemCreate WorkOrderItemCreate { get; set; }


        public List<UnitOfMeasure> UnitofMeasureList { get; set; }




        protected override async Task OnInitializedAsync()
        {

            if (WorkOrderItemCreate.eq.CurrentStatusID <= 2)
            {
                IsAccreditedDisable = false;

            }
            else
            {
                IsAccreditedDisable = true;
            }

            await base.OnInitializedAsync();



            Certifications = AppSecurity.CertificationList; // result as ICollection<Certification>;

            var umt = AppStateBasics.UnitofMeasureList.Where(x => x.TypeID == 3).ToList();
            if (umt != null && umt.Count > 0)
            {
                UnitofMeasureList = umt;
            }


            UsersAccess = WorkOrderItemCreate.UsersAccess;

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
            //Console.WriteLine(WorkOrderItemCreate.eq.GetCalibrationTypeID() );
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

       
        [Parameter]
        public Func<Task> RefreshParent { get; set; }


        public async Task Refresh()
        {



        }

    }
}
