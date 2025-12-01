using Blazored.Modal;
using Blazored.Modal.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Shared.Component
{
    public class Indicator_Base : Base_Create<PieceOfEquipment, Application.Services.IPieceOfEquipmentService<CallContext>, Domain.Aggregates.Shared.Basic.AppState>
    {
        [CascadingParameter] public IModalService Modal { get; set; }
       
        [Inject] IJSRuntime JSRuntime { get; set; }
#pragma warning restore CS0108 // 'Indicator_Base.JSRuntime' oculta el miembro heredado 'KavokuComponentBase<PieceOfEquipment>.JSRuntime'. Use la palabra clave new si su intención era ocultarlo.

        [Parameter]
        public string CustomerId { get; set; }
        [Parameter]
        public string CustomerName { get; set; }
        [Parameter]
        public int AddressId { get; set; }

        [Parameter]
        public PieceOfEquipment _listPoEIndicator { get; set; }
        [Parameter]
        public string Manufacturer { get; set; }
        [Parameter]
        public string Model { get; set; }

        public PieceOfEquipment _listPoE = new PieceOfEquipment();
        public PieceOfEquipmentGRPC _poeGrpc { get; set; }
        public List<PieceOfEquipment> _pieceOfEquipmentsDueDate = new List<PieceOfEquipment>();
        public ResponsiveTable<PieceOfEquipment> Grid { get; set; } = new ResponsiveTable<PieceOfEquipment>();

        [CascadingParameter] BlazoredModalInstance _BlazoredModal { get; set; }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected override async Task OnAfterRenderAsync(bool firstRender)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            if (!firstRender)
            {
                return;
            }



            //eq = _listPoEIndicator;
            //_listPoE = _listPoEIndicator;

            //Console.WriteLine("Manufacturer ****" + Manufacturer);
            //Console.WriteLine("Model ****" + Model);

            //StateHasChanged();
        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public override async Task<ResultSet<PieceOfEquipment>> LoadData(Pagination<PieceOfEquipment> pag)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            //_poeGrpc = new PieceOfEquipmentGRPC(Client);

            ResultSet<PieceOfEquipment> rsPoE = new ResultSet<PieceOfEquipment>();

            //PieceOfEquipment poe = new PieceOfEquipment();
            //poe.PieceOfEquipmentID = _listPoEIndicator.PieceOfEquipmentID;


            //  var eq = (await _poeGrpc.GetPieceOfEquipmentXId(poe));

            //eq = _listPoEIndicator;
            _listPoE = _listPoEIndicator;
            rsPoE.List.Add(_listPoE);
            eq = _listPoE;
            return rsPoE;
        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected override async Task OnParametersSetAsync()
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            eq = _listPoEIndicator;
            _listPoE = _listPoEIndicator;
            Console.WriteLine("Manufacturer ****" + Manufacturer);
            Console.WriteLine("Model ****" + Model);

            StateHasChanged();
        }


        public EquipmentTemplate ET;


#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected async Task FormSubmitted(EditContext editContext)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {


        }


#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected override async Task OnInitializedAsync()
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            // await base.OnInitializedAsync();


            eq = _listPoEIndicator;
            _listPoE = _listPoEIndicator;
            Console.WriteLine("Manufacturer ****" + Manufacturer);
            Console.WriteLine("Model ****" + Model);

        }

        public new void Dispose()
        {

        }


    }
}
