using Blazored.Modal;
using Blazored.Modal.Services;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.AssetsBasics
{
    public class POEWeigtSets_SearchBase : Base_Create<PieceOfEquipment, Application.Services.IPieceOfEquipmentService<CallContext>, Domain.Aggregates.Shared.AppStateCompany>, IPage<PieceOfEquipment, Application.Services.IPieceOfEquipmentService<CallContext>, Domain.Aggregates.Shared.Basic.AppState>
    //IPage<EquipmentTemplate,
    //Application.Services.IBasicsServices, Domain.Aggregates.Shared.Basic.AppState>
    {
        [CascadingParameter] BlazoredModalInstance _BlazoredModal { get; set; }
        [CascadingParameter] public IModalService Modal { get; set; }

        public PieceOfEquipmentGRPC _poeGrpc { get; set; }

        

        [Parameter]
        public bool Accredited { get; set; }

        [Parameter]
        public bool Checkbox { get; set; }

        public CustomerGRPC _customerGrpc { get; set; }

        public Search<PieceOfEquipment, IPieceOfEquipmentService<CallContext>, AppState> searchComponent
        { get; set; }




        public List<PieceOfEquipment> _pieceOfEquipmentsFiltered = new List<PieceOfEquipment>();
        // protected List<EquipmentType> FilteredToDos => EquipmentTypeList.Where(i => i.Name.ToLower().Contains(SearchTerm.ToLower())).ToList();
        public List<PieceOfEquipment> _pieceOfEquipmentsDueDate = new List<PieceOfEquipment>();
#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected override async Task OnInitializedAsync()
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            //await base.OnInitializedAsync();
            //Tenant tenant = new Tenant();

            //_poeGrpc = new PieceOfEquipmentGRPC(Client);


            //Console.WriteLine("OnInitializedAsync");
            //Console.WriteLine(Accredited);

            ////var Eq = (await Client.GetPiecesOfEquipmentXDueDate(tenant, new CallOptions()));
            //var Eq = (await _poeGrpc.GetPiecesOfEquipmentXDueDate(new CallOptions()));

            //_pieceOfEquipmentsDueDate = Eq.PieceOfEquipments;

            //Console.Write("OnInitializedAsync");

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }
            if (searchComponent != null)
            {
                searchComponent.BaseCreateUrl = "PieceOfEquipmentCreate";

                searchComponent.BaseDetailUrl = "EquipmentDetail";

                searchComponent.FormName = "Search Piece of Equipment";

                searchComponent.TypeName = "PieceOfEquipment";

                searchComponent.IsModal = IsModal;

                searchComponent.SelectOnly = SelectOnly;


                //var Eq = (await searchComponent.Client.GetEquipment());

                //searchComponent.List = Eq.EquipmentTemplates;
                Tenant tenant = new Tenant();
                if (searchComponent.Client != null)
                {

                    _poeGrpc = new PieceOfEquipmentGRPC(Client);

                    PieceOfEquipment poe = new PieceOfEquipment();
                    poe.IsWeigthSet = true;
                    poe.IsForAccreditedCal = Accredited;

                    var Eq = (await _poeGrpc.GetAllWeightSets(poe));

                    if (Eq != null)
                    {

                        searchComponent.List = Eq.PieceOfEquipments;

                        searchComponent.FilteredToDos = Eq.PieceOfEquipments;

                        StateHasChanged();
                    }
                }
                else
                {
                    //Logger.LogDebug("null data");
                }
            }
        }



        public async Task GetDetail(string ID)
        {
            searchComponent.Detail = searchComponent.List.Where(x => x.PieceOfEquipmentID == ID).FirstOrDefault();


            //var messageForm = Modal.Show<Equipment_Create>("Passing Data");
            //var result = await messageForm.Result;
            await searchComponent.ShowModal();

            StateHasChanged();

            //await BlazoredModal.Close(ModalResult.Ok(searchComponent.Detail));

        }

#pragma warning disable CS0108 // 'POEWeigtSets_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<PieceOfEquipment, IPieceOfEquipmentService<CallContext>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
        public void Dispose()
#pragma warning restore CS0108 // 'POEWeigtSets_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<PieceOfEquipment, IPieceOfEquipmentService<CallContext>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
        {

        }

        public IEnumerable<PieceOfEquipment> FilterList(string filter)
        {
            try
            {

                List<PieceOfEquipment> lequ = searchComponent.List.ToList();


                if (lequ == null)
                {
                    return new List<PieceOfEquipment>();
                }

                return lequ;

            }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            {
                return new List<PieceOfEquipment>();

            }
        }

        // public async Task SelectModal(PieceOfEquipment DTO)
        public async Task SelectModal(PieceOfEquipment DTO)
        {
            await _BlazoredModal.CloseAsync(ModalResult.Ok(searchComponent.Detail));
        }

        public async Task ReturnModal()
        {
            await _BlazoredModal.CloseAsync(ModalResult.Ok(_pieceOfEquipmentsFiltered));
        }


#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task addToListPoE(string ID)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            PieceOfEquipment _poe = new PieceOfEquipment();
            if (_pieceOfEquipmentsDueDate != null)
            {
                var listPoe = _pieceOfEquipmentsDueDate.Where(x => x.PieceOfEquipmentID == ID).FirstOrDefault();

                _poe = new PieceOfEquipment()
                {
                    PieceOfEquipmentID = ID,
                    SerialNumber = listPoe.SerialNumber
                };


                _pieceOfEquipmentsFiltered.Add(_poe);
            }
            //await BlazoredModal.Close(ModalResult.Ok(searchComponent.Detail));

        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task DownloadCertificate(string certNumber)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task Delete(PieceOfEquipment DTO)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            //await searchComponent.ShowModalAction();

            //searchComponent.Detail =  await searchComponent.Client.de(DTO);

            //searchComponent.ShowResult();
        }




    }
}
