using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Basics
{
    public class EquipmentType_DetailBase : ComponentBase
    {
        [Inject] Application.Services.IBasicsServices<CallContext> Client { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] AppState AppState { get; set; }


        public EquipmentType eq = new EquipmentType();
        protected string _Disable = "disabled";
#pragma warning disable CS0414 // El campo 'EquipmentType_DetailBase.formInvalid' está asignado pero su valor nunca se usa
        private bool formInvalid = true;
#pragma warning restore CS0414 // El campo 'EquipmentType_DetailBase.formInvalid' está asignado pero su valor nunca se usa
        protected string LastSubmitResult;


        [Parameter]
        public bool Enabled { get; set; }

        [Parameter]
        public string EntityID
        {

            get
            {
                return _Disable;
            }
            set
            {

                _Disable = value;


            }
        }


        public string NameValidationMessage { get; set; }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected override async Task OnParametersSetAsync()
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            NameValidationMessage = "valid";

            if (AppState.EquipmentTypes != null && AppState.EquipmentTypes.Count > 0)
            {

                eq = AppState.EquipmentTypes.Where(x => x.EquipmentTypeID == Convert.ToInt32(EntityID)).FirstOrDefault();

            }
            else
            {
                eq = new EquipmentType();

            }
            //editContext = new EditContext(eq);

        }



#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected override async Task OnInitializedAsync()
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {


            //CurrentEditContext.OnFieldChanged += HandleFieldChanged;


        }





        public void Dispose()
        {

        }

    }
}
