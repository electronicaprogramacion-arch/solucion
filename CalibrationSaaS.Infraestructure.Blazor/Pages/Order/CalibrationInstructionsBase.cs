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
using CalibrationSaaS.Infraestructure.Blazor.Shared;
using Microsoft.Extensions.Configuration;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Order
{
    public class CalibrationInstructionsBase : CalibrationInstructionsBase_, ICalibrationInstructionsLTIBase
    {

        [Inject]
        public IConfiguration Configuration { get; set; }

        [Inject]
        public dynamic DbFactory { get; set; }

     

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task ChangeSwitch(ChangeEventArgs e)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {


        }


        public bool Enabled { get; set; } = true;
        public bool LabelDown { get; set; }

        public string Customer { get; set; }

        public bool IsAccreditedDisable { get; set; }


        public List<RangeTolerance> Ranges { get; set; } = new List<RangeTolerance>();
        public RangeTestPoint RangeComponent { get; set; }

        public RangeTestPoint RangeAccuracy  { get; set; }

        
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
            }
            catch (Exception ex)
            {

            }

            return "";
        }

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
                        if (eq?.WorkOder?.UserWorkOrders == null || eq?.WorkOder?.UserWorkOrders?.Count == 0)
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


        public List<CalibrationType> _calibrationTypeList { get; set; }= new List<CalibrationType>();

        [Parameter]
        public Domain.Aggregates.Entities.Status CurrentStatus { get; set; } = new Domain.Aggregates.Entities.Status();


       


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


        public async Task ChangeSwitchEndOfMonth(ChangeEventArgs e)
        {
            DateTime _dueDate = DateTime.Now;
            if (eq.CalibrationCustomDueDate != null)
                _dueDate = (DateTime)eq.CalibrationCustomDueDate;
            if (eq.EndOfMonth == true)
            {
                eq.CalibrationCustomDueDate = LastDayOfMonth(_dueDate);
            }
        }

        public async Task ChangeSwitchIsAccredited(ChangeEventArgs e)
        {
            
        }


        protected override async Task OnInitializedAsync()
        {


            Customer = Configuration.GetSection("Reports")["Customer"];
            CurrentStatus.StatusId = 1;


            editContext = new EditContext(eq);

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



        }


        string selectedAnswer = "";

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

        public DateTime LastDayOfMonth(DateTime inDate)
        {
            var daysInMonth = DateTime.DaysInMonth(inDate.Year, inDate.Month);
            return new DateTime(inDate.Year, inDate.Month, daysInMonth);
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

            var ct = eq.CalibrationIntervalID;


        }


        public bool Custom = false;

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

        public  Blazed.Controls.Modal ModalResolution { get; set; }
        public bool ResolutionShow { get; set; }
       
        public Func<Task> RefreshParent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<UnitOfMeasure> UnitofMeasureList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


         

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task ShowResolution()
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            ResolutionShow = true;


            ModalResolution.Toggle();


            StateHasChanged();

        }



        public Task ChangeCalibrationType(ChangeEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task Refresh()
        {
            throw new NotImplementedException();
        }

       
    }
}
