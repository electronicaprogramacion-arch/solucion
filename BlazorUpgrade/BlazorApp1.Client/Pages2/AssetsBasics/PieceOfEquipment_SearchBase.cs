using Blazed.Controls;
using Blazored.Modal;
using Blazored.Modal.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp1.Blazor.Pages.AssetsBasics
{
    public class PieceOfEquipment_SearchBase : Base_Create<PieceOfEquipment, Func<dynamic, CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext>>,
        CalibrationSaaS.Domain.Aggregates.Shared.Basic.AppState>
    //, IPage<PieceOfEquipment, CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext>, Domain.Aggregates.Shared.Basic.AppState>
    //IPage<EquipmentTemplate,
    //Application.Services.IBasicsServices, Domain.Aggregates.Shared.Basic.AppState>
    {

        //[Inject]
        //public IAccessTokenProvider TokenProvider { get; set; }
        public ResponsiveTable<PieceOfEquipment> Grid { get; set; } = new ResponsiveTable<PieceOfEquipment>();
        [CascadingParameter] BlazoredModalInstance _BlazoredModal { get; set; }
        [CascadingParameter] public IModalService Modal { get; set; }

        [Inject] public Func<dynamic, CalibrationSaaS.Application.Services.IBasicsServices<CallContext>> _basicServices { get; set; }
        public PieceOfEquipmentGRPC _poeGrpc { get; set; }

       

        [Parameter]
        public bool Checkbox { get; set; }

        [Parameter]
        public CalibrationSaaS.Domain.Aggregates.Entities.Customer Customer { get; set; }
        [Parameter]
        public List<PieceOfEquipment> _pieceOfEquipmentsDueDate { get; set; }
        [Parameter]
        public bool IsForEt { get; set; }
        public CustomerGRPC _customerGrpc { get; set; }


        public string QueryType { get; set; }

        //public Search<PieceOfEquipment, IPieceOfEquipmentService<CallContext>, AppState> searchComponent 
        //{ get ; set ; }  = new Search<PieceOfEquipment, IPieceOfEquipmentService<CallContext>, AppState>();

        //public int paginaActual = 1;
        //public int paginasTotales;
        public string nombrePersonaFiltro = string.Empty;



        public List<PieceOfEquipment> _pieceOfEquipmentsFiltered = new List<PieceOfEquipment>();

        // protected List<EquipmentType> FilteredToDos => EquipmentTypeList.Where(i => i.Name.ToLower().Contains(SearchTerm.ToLower())).ToList();



        public void ClearList()
        {

            Grid.Clear();
            //StateHasChanged();

        }


        //private async Task<Metadata> GetHeaderAsync()
        //{
        //    var headers = new Metadata();
        //    var accessTokenResult = await TokenProvider.RequestAccessToken();
        //    var AccessToken = string.Empty;
        //    if (accessTokenResult.TryGetToken(out var token))
        //    {
        //        AccessToken = token.Value;
        //        headers.Add("Authorization", $"Bearer {AccessToken}");
        //    }

        //    return headers;
        //}


        public override async Task<ResultSet<PieceOfEquipment>> LoadData(Pagination<PieceOfEquipment> pag)
        {
            //await ShowProgress();

            ResultSet<PieceOfEquipment> result = new ResultSet<PieceOfEquipment>();
            if (IsForEt == false)

            {

                var g = new CallOptions(await GetHeaderAsync());


                _poeGrpc = new PieceOfEquipmentGRPC(Client,DbFactory,g);

                

                //pag.SaveCache = false;

                result = (await _poeGrpc.GetPiecesOfEquipmentXDueDate(pag));

            }
            
            //await CloseProgress();
            return result;
        }

        //protected override async Task OnInitializedAsync()
        //{
        //    await base.OnInitializedAsync();



        //}
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            //Component.Group = "admin,tech";

            //Grid.Filter = new Pagination<PieceOfEquipment>();
            //Grid.Filter.Entity = new PieceOfEquipment();
            eq.Customer = new CalibrationSaaS.Domain.Aggregates.Entities.Customer();
            eq.EquipmentTemplate = new EquipmentTemplate();

            BasicsServiceGRPC basic = new BasicsServiceGRPC(_basicServices,DbFactory);

            if (AppState?.EquipmentTypes?.Count == 0 && _basicServices != null)
            {

                var etypes = await basic.GetEquipmenTypes(new CallContext());

                foreach (var item in etypes.EquipmentTypes)
                {
                    AppState.AddEquipmentType(item);
                }
                //EquipmentTypes = etypes.EquipmentTypes;

            }


        }
        //protected override async Task OnInitializedAsync()
        //{
        //    await base.OnInitializedAsync();

        //}



#pragma warning disable CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task GetDetail(int ID)
#pragma warning restore CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            //searchComponent.Detail = searchComponent.List.Where(x => x.PieceOfEquipmentID == ID).FirstOrDefault();


            ////var messageForm = Modal.Show<Equipment_Create>("Passing Data");
            ////var result = await messageForm.Result;
            //await searchComponent.ShowModal();

            //StateHasChanged();

            ////await BlazoredModal.Close(ModalResult.Ok(searchComponent.Detail));

        }

#pragma warning disable CS0108 // 'PieceOfEquipment_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<PieceOfEquipment, IPieceOfEquipmentService<CallContext>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intenci�n era ocultarlo.
        public void Dispose()
#pragma warning restore CS0108 // 'PieceOfEquipment_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<PieceOfEquipment, IPieceOfEquipmentService<CallContext>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intenci�n era ocultarlo.
        {

        }


        public async Task SelectModal(PieceOfEquipment DTO)
        {
            await _BlazoredModal.CloseAsync(ModalResult.Ok(eq));
        }

        public async Task ReturnModal()
        {
            await _BlazoredModal.CloseAsync(ModalResult.Ok(_pieceOfEquipmentsFiltered));
        }


#pragma warning disable CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task addToListPoE(string ID)
#pragma warning restore CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
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

#pragma warning disable CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task DownloadCertificate(string certNumber)
#pragma warning restore CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

        }

        public async Task<bool> Delete(PieceOfEquipment DTO)
        {
            _poeGrpc = new PieceOfEquipmentGRPC(Client,DbFactory);

            await _poeGrpc.DeletePieceOfEquipment(DTO);

            return true;

        }
    }
}
