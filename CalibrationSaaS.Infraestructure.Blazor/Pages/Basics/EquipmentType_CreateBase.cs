
using Blazed.Controls;
using Blazed.Controls.Route.Services;
using Blazed.Controls.Toast;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using CalibrationSaaS.Infraestructure.Blazor.Shared;

using Grpc.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CalibrationSaaS.Domain.Aggregates.Querys.Querys;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Basics
{
    public class EquipmentType_CreateBase : Base_Create<EquipmentType, Application.Services.IBasicsServices<CallContext>, Domain.Aggregates.Shared.Basic.AppState>
     
    {
    
        [Inject] Application.Services.IWorkOrderDetailServices<CallContext> _wod { get; set; }
        protected ProgressBar eqtProgressBar;
        public List<CalibrationSubType> _calibrationSubtypeList = new List<CalibrationSubType>();


        public Note NoteRef { get; set; } = new Note();
        public List<Note> LIST1 { get; set; } = new List<Note>();
        public Note NewNote()
        { 
        
           return new Note();
        
        
        }
        public Note RowChange(Note lin)
        {



            if (RT.IsDragAndDrop)
            {
                int posi = 100;
                var tmpg = RT.Items;
                for (int i=0;i< RT.ItemList.Count;i++)
                {
                                                   

                    RT.ItemList[i].Position = posi;
                    posi++;
                    RT.ReplaceItemKey(RT.ItemList[i]);
                }

                return lin;
            }

            if (lin.Position >= 100)
            {
                return lin;
            }

            return lin;



        }

        public ResponsiveTable<Note> RT { get; set; } = new ResponsiveTable<Note>();

       

        protected async Task FormSubmitted(EditContext editContext)
        {
            bool formIsValid = CurrentEditContext.Validate();
            LastSubmitResult =
              formIsValid
              ? ""
              : "Failure - form was invalid";

            if (formIsValid)
            {
              
                await JSRuntime.InvokeVoidAsync("showProgressBar", "progressBar");

                LastSubmitResult = "OnValidSubmit was executed";

                //
                if(RT?.Items?.Count > 0)
                {
                    eq.Notes = RT.Items;
                }


                BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);
                var result = (await basics.CreateEquipmentType(eq, new CallOptions()));

                await CloseModal(result);

                await ShowToast("The information has been saved successfully.", ToastLevel.Success);

           
            }

        }
#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected async Task InvalidFormSubmitted(EditContext editContext)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            LastSubmitResult = "OnInvalidSubmit was executed";
        }



        /// <summary>
        /// Injected Navigation Manager
        /// </summary>
        [Inject]
        public NavigationManager NavManager { get; set; }

        /// <summary>
        /// Injected User Session Object
        /// </summary>
        [Inject]
        public RouterSessionService RouterSessionService { get; set; }
        public string PageUrl { get; set; }

        public bool IsClean { get; set; }
        public string RouteUrl { get; set; }
        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();

           
            NameValidationMessage = "valid";

            if (EntityID != "0")
            {
                BasicsServiceGRPC basic = new BasicsServiceGRPC(Client);

                eq.EquipmentTypeID = Convert.ToInt32(EntityID);

                eq = await basic.GetEquipmentTypeByID(eq);

                if(eq.Notes != null)
                {
                    LIST1 = eq.Notes.ToList();
                }
               

            }
            else
            {
                eq = new EquipmentType();
                eq.IsEnabled = true;
            }

            var dataGrids = await _wod.GetCalibrationSubtype(new CallContext()) ;


            _calibrationSubtypeList =  dataGrids;

            CurrentEditContext = new EditContext(eq);

            //CurrentEditContext.OnFieldChanged += HandleFieldChanged;
            this.RouteUrl = this.NavManager.Uri;
            
            this.IsClean = true;
            this.RouterSessionService.NavigationCancelled += OnNavigationCancelled;



        }

        private void OnNavigationCancelled(object sender, EventArgs e)
        {

        }

        protected void FunctionName(EventArgs args)
        {
            if (eq.Name.Length < 1)
            {
                NameValidationMessage = "invalid";
            }
            else
            {
                NameValidationMessage = "valid";
            }
            //Logger.LogDebug(this.eq.Name);
            StateHasChanged();

        }

        public async Task HandleReset()
        {
            eq = new EquipmentType();
            CurrentEditContext = new EditContext(eq);
            await JSRuntime.InvokeVoidAsync("removeValidClass");
            StateHasChanged();
        }


        public Task<IEnumerable<CalibrationSubType>> SearchCalibrationSubtype(string searchText)
        {
            var result = _calibrationSubtypeList.Where(x => x.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);


            return Task.FromResult<IEnumerable<CalibrationSubType>>(result);
        }

        public string ConvertCalibrationSubtype(CalibrationSubType ca)
        {

            var p = ca.Name;
            return p;
        }
#pragma warning disable CS0108 // 'EquipmentType_CreateBase.ShowToast(string, ToastLevel)' oculta el miembro heredado 'KavokuComponentBase<EquipmentTemplate>.ShowToast(string, ToastLevel)'. Use la palabra clave new si su intención era ocultarlo.
#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task ShowToast(string message, ToastLevel level)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
#pragma warning restore CS0108 // 'EquipmentType_CreateBase.ShowToast(string, ToastLevel)' oculta el miembro heredado 'KavokuComponentBase<EquipmentTemplate>.ShowToast(string, ToastLevel)'. Use la palabra clave new si su intención era ocultarlo.
        {

            Toast.ShowSuccess(message);

        }

#pragma warning disable CS0108 // 'EquipmentType_CreateBase.Dispose()' oculta el miembro heredado 'Base_Create<EquipmentTemplate, IBasicsServices<CallContext>, AppState>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
        public void Dispose()
#pragma warning restore CS0108 // 'EquipmentType_CreateBase.Dispose()' oculta el miembro heredado 'Base_Create<EquipmentTemplate, IBasicsServices<CallContext>, AppState>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
        {

        }

    }
}
