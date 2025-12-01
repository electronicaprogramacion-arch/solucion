using Blazor.IndexedDB.Framework;
using Blazored.Modal;
using Blazored.Modal.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Infraestructure.Blazor;

using CalibrationSaaS.Infraestructure.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorApp1.Client.Pages.AssetsBasics;
using BlazorApp1.Blazor.Pages.AssetsBasics;



namespace BlazorApp1.Blazor.Pages.Order
{
    public class BasicInformationBase : KavokuComponentBase<WorkOrderDetail>
    {
        [Inject]
#pragma warning disable CS0108 // 'BasicInformationBase.NavigationManager' oculta el miembro heredado 'KavokuComponentBase<WorkOrderDetail>.NavigationManager'. Use la palabra clave new si su intención era ocultarlo.
        public NavigationManager NavigationManager { get; set; }
#pragma warning restore CS0108 // 'BasicInformationBase.NavigationManager' oculta el miembro heredado 'KavokuComponentBase<WorkOrderDetail>.NavigationManager'. Use la palabra clave new si su intención era ocultarlo.

        [Inject]
        public Reports.Domain.ReportViewModels.Repeatability repeatability { get; set; }



        [Parameter]
        public string UsersAccess { get; set; }

        //[Inject] public Application.Services.IPieceOfEquipmentService<CallContext> _poeServices { get; set; }

        [Inject]
        public Func<dynamic, CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext>> _poeServices { get; set; }


        [CascadingParameter] public IModalService Modal { get; set; }

        [Parameter]
        public CalibrationSaaS.Domain.Aggregates.Entities.Status CurrentStatus { get; set; }

        public EditContext editContext { get; set; }

        [Parameter]
        public WorkOrderDetail eq { get; set; } = new WorkOrderDetail();

        public string SerialNumber { get; set; } = "";

        public string EquipmentType { get; set; } = "";

        public string EquipmentTemplate { get; set; } = "";

        [Parameter]
        public IEnumerable<Address> _addressFiltered { get; set; }



        public string Customer { get; set; }

        public string TextToShow { get; set; }

        //YPPP
        public int _CustomerId { get; set; }
        public PieceOfEquipment _listPoEIndicator = new PieceOfEquipment();

        public async Task ShowSerial()
        {
            var parameters = new ModalParameters();
            parameters.Add("SelectOnly", true);
            parameters.Add("IsModal", true);
            ModalOptions op = new ModalOptions();
            op.ContentScrollable = true;
            op.Class = "blazored-modal " + ModalSize.MediumWindow;

            var messageForm = Modal.Show<PieceOfEquipment_Search>("Select Piece Of Equipment", parameters, op);
            var result = await messageForm.Result;

            if (!result.Cancelled)
                _message = result.Data?.ToString() ?? string.Empty;

            //EmptyValidationDictionary = result.Data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
            //      .ToDictionary(prop => prop.Name, prop => prop.GetValue(result.Data, null));

            if (result.Data != null)
            {
                eq.PieceOfEquipment = (PieceOfEquipment)result.Data;

                eq.PieceOfEquipmentId = eq.PieceOfEquipment.PieceOfEquipmentID;
            }

        }

        public async Task ShowSerial2()
        {
            var parameters = new ModalParameters();
            parameters.Add("SelectOnly", true);
            parameters.Add("IsModal", true);
            parameters.Add("EntityID", eq.PieceOfEquipmentId);

            ModalOptions op = new ModalOptions();
            op.ContentScrollable = true;
            op.Class = "blazored-modal " + ModalSize.MediumWindow;

            var messageForm = Modal.Show<PieceOfEquipment_Create>("Piece Of Equipment", parameters, op);
            var result = await messageForm.Result;

            if (!result.Cancelled)
                _message = result.Data?.ToString() ?? string.Empty;

            //EmptyValidationDictionary = result.Data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
            //      .ToDictionary(prop => prop.Name, prop => prop.GetValue(result.Data, null));

            if (result.Data != null)
            {
                eq.PieceOfEquipment = (PieceOfEquipment)result.Data;

                eq.PieceOfEquipmentId = eq.PieceOfEquipment.PieceOfEquipmentID;
            }

        }

        async Task ShowEquipment()
        {
            //Dictionary<string, object> EmptyValidationDictionary = new Dictionary<string, object>();
            //List<EquipmentTemplate> listEquipment;

            var parameters = new ModalParameters();
            parameters.Add("Enabled", false);
            //parameters.Add("IsModal", true);

            var messageForm = Modal.Show<BlazorApp1.Blazor.Blazor.Pages.Basics.Equipment_Create>("Equipment Template", parameters);
            var result = await messageForm.Result;

            if (!result.Cancelled)
                _message = result.Data?.ToString() ?? string.Empty;

            //EmptyValidationDictionary = result.Data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
            //      .ToDictionary(prop => prop.Name, prop => prop.GetValue(result.Data, null));

            EquipmentTemplate Template = (EquipmentTemplate)result.Data;

            eq.PieceOfEquipment.EquipmentTemplate = Template;
            //eq.PieceOfEquipment.EquipmentTemplateId = Template.EquipmentTemplateID;
            //_EquipmentTemplateID = EmptyValidationDictionary["EquipmentTemplateID"].ToString();
            //_Model = EmptyValidationDictionary["Model"].ToString();
            //_Name = EmptyValidationDictionary["Name"].ToString();
            //_Manufacturer = EmptyValidationDictionary["Manufacturer"].ToString();


            //eq.EquipmentTemplateId = Convert.ToInt32(EmptyValidationDictionary["EquipmentTemplateID"].ToString());
            //_message = "";

        }

        public string _message { get; set; }
        async Task ShowEquipmentType()
        {
            //Dictionary<string, object> EmptyValidationDictionary = new Dictionary<string, object>();
            //List<EquipmentTemplate> listEquipment;

            var parameters = new ModalParameters();
            parameters.Add("SelectOnly", true);
            parameters.Add("IsModal", true);

            var messageForm = Modal.Show<BlazorApp1.Blazor.Pages.Basics.Equipment_Search>("Select Equipment Template", parameters);
            var result = await messageForm.Result;

            if (!result.Cancelled)
                _message = result.Data?.ToString() ?? string.Empty;

            //EmptyValidationDictionary = result.Data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
            //      .ToDictionary(prop => prop.Name, prop => prop.GetValue(result.Data, null));

            EquipmentTemplate Template = (EquipmentTemplate)result.Data;

            eq.PieceOfEquipment.EquipmentTemplate = Template;
            //eq.PieceOfEquipment.EquipmentTemplateId = Template.EquipmentTemplateID;
            //_EquipmentTemplateID = EmptyValidationDictionary["EquipmentTemplateID"].ToString();
            //_Model = EmptyValidationDictionary["Model"].ToString();
            //_Name = EmptyValidationDictionary["Name"].ToString();
            //_Manufacturer = EmptyValidationDictionary["Manufacturer"].ToString();


            //eq.EquipmentTemplateId = Convert.ToInt32(EmptyValidationDictionary["EquipmentTemplateID"].ToString());
            //_message = "";

        }



        async Task ShowCustomer()
        {


            var parameters = new ModalParameters();
            parameters.Add("SelectOnly", true);
            parameters.Add("IsModal", true);
            ModalOptions op = new ModalOptions();
            op.ContentScrollable = true;
            op.Class = "blazored-modal " + ModalSize.MediumWindow;


            var messageForm = Modal.Show<BlazorApp1.Blazor.Pages.Customer.Customer_Search>("Select a Customer", parameters, op);
            var result = await messageForm.Result;

            if (!result.Cancelled)
                _message = result.Data?.ToString() ?? string.Empty;

            //EmptyValidationDictionary = result.Data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
            //      .ToDictionary(prop => prop.Name, prop => prop.GetValue(result.Data, null));

            eq.PieceOfEquipment = (PieceOfEquipment)result.Data;

            eq.PieceOfEquipmentId = eq.PieceOfEquipment.PieceOfEquipmentID;

        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {


            try
            {

                await base.OnAfterRenderAsync(firstRender);
                //await ShowProgress();
                if (firstRender)
                {
                    try
                    {

                        if (eq != null && eq?.PieceOfEquipment?.EquipmentTemplate != null)
                        {

                            EquipmentType = eq.PieceOfEquipment.EquipmentTemplate.Name;

                            SerialNumber = eq.PieceOfEquipment.SerialNumber;

                            //Manufacturer = eq.PieceOfEquipment.EquipmentTemplate.Manufacturer;
                            EquipmentTemplate = eq.PieceOfEquipment.EquipmentTemplate.Model;
                        }
                    }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                    catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                    {


                    }

                    if (firstRender)
                    {
                        await JSRuntime.InvokeVoidAsync("removeValidClass");

                    }
                    if (eq != null && eq?.PieceOfEquipment?.EquipmentTemplate != null)
                    {
                        editContext = new EditContext(eq.PieceOfEquipment.EquipmentTemplate);

                    }
                    else
                    {
                        editContext = new EditContext(new EquipmentTemplate());
                    }

                }
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
            //eq.PieceOfEquipment.EquipmentTemplate


            
        }



#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected override async Task OnInitializedAsync()
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            //if (eq != null && eq?.PieceOfEquipment?.EquipmentTemplate != null)
            //{
            //    editContext = new EditContext(eq.PieceOfEquipment.EquipmentTemplate);

            //}
            //else
            //{
            //    editContext = new EditContext(new EquipmentTemplate());
            //}




            await base.OnInitializedAsync();


        }

        public void Submit(EditContext e)
        {
            var result = e.Validate();



        }

        public void ChangeList(ChangeEventArgs arg)
        {
            eq.PieceOfEquipment.AddressId = (int)arg.Value;

        }
        [Inject]
        public dynamic DbFactory { get; set; }
        public async Task ShowModalPoEIndicator()
        {
            _CustomerId = eq.WorkOder.CustomerId;
            Dictionary<string, object> EmptyValidationDictionary = new Dictionary<string, object>();
            //_listPoEDueDate = new List<PieceOfEquipment>();
            var parameters = new ModalParameters();
            parameters.Add("SelectOnly", true);
            parameters.Add("IsModal", true);
            parameters.Add("Checkbox", false);
            parameters.Add("Indicator", true);
            parameters.Add("CustomerId", _CustomerId);

            ModalOptions op = new ModalOptions();
            op.ContentScrollable = true;
            op.Class = "blazored-modal " + ModalSize.MediumWindow;

            //PieceOfEquipmentDueDate_Search
            var messageForm = Modal.Show<POEIndicator_Search>("Piece Of Equipment Indicator", parameters, op);
            var result = await messageForm.Result;

            if (result != null)
            {

                var res = (PieceOfEquipment)result.Data;
                PieceOfEquipment _poe = new PieceOfEquipment();
                _poe.PieceOfEquipmentID = res.PieceOfEquipmentID;

                Tenant tenant = new Tenant();

                var con = await CallOptions(Component.Name);

                PieceOfEquipmentGRPC poeGrpc = new PieceOfEquipmentGRPC(_poeServices, DbFactory,con.CallOptions);

                var listPoe = await poeGrpc.GetPieceOfEquipmentXId(_poe);

                _listPoEIndicator = listPoe; //  res.FirstOrDefault();

            }



            if (!result.Cancelled)
                _message = result.Data?.ToString() ?? string.Empty;


            //EmptyValidationDictionary = result.Data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
            //      .ToDictionary(prop => prop.Name, prop => prop.GetValue(result.Data, null));

        }


    }
}
