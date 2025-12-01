using Blazored.Modal.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Helpers.Controls;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages
{
    public class Full_SearchBase : Base_Create<KeyValueSearch, Application.Services.IPieceOfEquipmentService<CallContext>, Domain.Aggregates.Shared.AppStateCompany>
    //, IPage<PieceOfEquipment, Application.Services.IPieceOfEquipmentService<CallContext>, Domain.Aggregates.Shared.Basic.AppState>
    //IPage<EquipmentTemplate,
    //Application.Services.IBasicsServices, Domain.Aggregates.Shared.Basic.AppState>
    {


        public ResponsiveTable<KeyValueSearch> Grid { get; set; } = new ResponsiveTable<KeyValueSearch>();
        //[CascadingParameter]public  BlazoredModalInstance _BlazoredModal { get; set; }
        [CascadingParameter] public IModalService Modal { get; set; }

        public PieceOfEquipmentGRPC _poeGrpc { get; set; }

        

        [Parameter]
        public bool Accredited { get; set; }

        [Parameter]
        public bool Checkbox { get; set; }


        [Parameter]
        public Domain.Aggregates.Entities.Customer Customer { get; set; }


        [Parameter]
        public bool Indicator { get; set; }

        [Parameter]
        public int CustomerId { get; set; }

        public CustomerGRPC _customerGrpc { get; set; }

        //public Search<PieceOfEquipment, IPieceOfEquipmentService<CallContext>, AppState> searchComponent 
        //{ get ; set ; }

        [Parameter]
        public List<PieceOfEquipment> pieceOfEquipmentsCh { get; set; }


        public List<PieceOfEquipment> _pieceOfEquipmentsFiltered { get; set; } = new List<PieceOfEquipment>();

        // protected List<EquipmentType> FilteredToDos => EquipmentTypeList.Where(i => i.Name.ToLower().Contains(SearchTerm.ToLower())).ToList();
        [Parameter]
        public List<KeyValueSearch> _pieceOfEquipmentsDueDate { get; set; } = new List<KeyValueSearch>();

        //private CalibrationSaaS.Pagination<PieceOfEquipment> pag = new CalibrationSaaS.Pagination<PieceOfEquipment>();
        protected override async Task OnInitializedAsync()
        {
            paginaSize = 5;

            //foreach(var uy in pieceOfEquipmentsCh)
            //{
            //    _pieceOfEquipmentsFiltered.Add(uy);
            //}


            await base.OnInitializedAsync();
            


        }


        //public override async Task<ResultSet<PieceOfEquipment>> LoadData(Pagination<PieceOfEquipment> pag)
        //{
        //    //_poeGrpc = new PieceOfEquipmentGRPC(Client);

        //    //PieceOfEquipment poe = new PieceOfEquipment();
        //    //poe.IsWeigthSet = true;
        //    //poe.IsForAccreditedCal = Accredited;
        //    //poe.CustomerId = CustomerId;
        //    //poe.Customer = Customer;
        //    //pag.Entity = poe;



        //    //ResultSet<PieceOfEquipment> Eq = new ResultSet<PieceOfEquipment>();

        //    //Eq = (await _poeGrpc.GetPieceOfEquipmentIndicator(pag ));



        //    //return Eq;

        //    //return Eq;

        //}


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //if (!firstRender)
            //{
            //    return;
            //}


            try
            {

                FormName = "Search Piece of Equipment";
                TypeName = "PieceOfEquipment";
                IsModal = IsModal;
                SelectOnly = SelectOnly;




            }
            catch (Exception ex)
            {
                await ShowError(ex.Message);
            }
            finally
            {
                await CloseProgress();
            }

        }



        //public async Task GetDetail(int ID)
        //{
        //        searchComponent.Detail = searchComponent.List.Where(x => x.PieceOfEquipmentID == ID).FirstOrDefault();


        //    //var messageForm = Modal.Show<Equipment_Create>("Passing Data");
        //    //var result = await messageForm.Result;
        //    await searchComponent.ShowModal();

        //    StateHasChanged();

        //    //await BlazoredModal.Close(ModalResult.Ok(searchComponent.Detail));

        //}

#pragma warning disable CS0108 // 'Full_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<KeyValueSearch, IPieceOfEquipmentService<CallContext>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
        public void Dispose()
#pragma warning restore CS0108 // 'Full_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<KeyValueSearch, IPieceOfEquipmentService<CallContext>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
        {

        }

        //public IEnumerable<PieceOfEquipment> FilterList(string filter)
        //{
        //    try
        //    {

        //        List<PieceOfEquipment> lequ = Grid.Items
        //            .Where(x => (x.Name != null && x.Name.ToLower().StartsWith(filter.ToLower()))
        //            || (x.SerialNumber != null && x.SerialNumber.ToLower().StartsWith(filter.ToLower()))
        //            || (x.Description != null && x.Description.ToLower().StartsWith(filter.ToLower()))
        //             || (x.EquipmentTemplate != null && x.EquipmentTemplate.Model != null && x.EquipmentTemplate.Model.ToLower().StartsWith(filter.ToLower()))
        //            || (x.PieceOfEquipmentID != null && x.PieceOfEquipmentID.ToString().ToLower().StartsWith(filter.ToLower()))).ToList();



        //        if (lequ == null)
        //        {
        //            return new List<PieceOfEquipment>();
        //        }

        //        return lequ;

        //    }
        //    catch (Exception ex)
        //    {
        //        return new List<PieceOfEquipment>();

        //    }
        //}

        //// public async Task SelectModal(PieceOfEquipment DTO)
        // public async Task SelectModal(PieceOfEquipment DTO)
        // {
        //     await BlazoredModal.Close(ModalResult.Ok(searchComponent.Detail));
        // }

        public async Task ReturnModal()
        {
            await BlazoredModal.CloseAsync(ModalResult.Ok(_pieceOfEquipmentsFiltered));
        }


        //public async Task addToListPoE(int ID)
        //{

        //    if(_pieceOfEquipmentsFiltered == null)
        //    {
        //        _pieceOfEquipmentsFiltered = new List<PieceOfEquipment>();
        //    }


        // if(Grid.Items !=null && _pieceOfEquipmentsFiltered != null)
        //    { 

        //        var listPoe = Grid.Items.Where(x=>x.PieceOfEquipmentID==ID).FirstOrDefault();
        //        PieceOfEquipment _poe = new PieceOfEquipment();
        //        _poe = new PieceOfEquipment()
        //        {
        //            PieceOfEquipmentID = ID,
        //            SerialNumber = listPoe.SerialNumber
        //        };
        //        try {

        //            Console.WriteLine(_pieceOfEquipmentsFiltered.Count);
        //        var xx = _pieceOfEquipmentsFiltered.Where(x => x.PieceOfEquipmentID == ID).ToArray();
        //            if (xx != null && xx?.Count()> 0)
        //            {
        //              var res= _pieceOfEquipmentsFiltered.Remove(xx[0]);
        //            }
        //            else
        //            {
        //                _pieceOfEquipmentsFiltered.Add(_poe);
        //            }
        //            Console.WriteLine(_pieceOfEquipmentsFiltered.Count);
        //        }
        //        catch(Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }

        //    }
        //    //await BlazoredModal.Close(ModalResult.Ok(searchComponent.Detail));

        //}

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task DownloadCertificate(string certNumber)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task<bool> Delete(KeyValueSearch DTO)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            return true;
        }




    }
}
