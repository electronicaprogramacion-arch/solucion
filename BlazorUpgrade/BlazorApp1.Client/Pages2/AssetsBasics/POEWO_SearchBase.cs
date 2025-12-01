using Blazor.IndexedDB.Framework;
using Blazored.Modal.Services;
using Blazed.Controls;
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
using BlazorApp1.Blazor.Blazor.Shared;
using System.Drawing.Printing;

namespace BlazorApp1.Blazor.Pages.AssetsBasics
{
    public class POEWO_SearchBase : Base_Create<PieceOfEquipment, Func<dynamic, CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext>>, CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany>
    //, IPage<PieceOfEquipment, CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext>, Domain.Aggregates.Shared.Basic.AppState>
    //IPage<EquipmentTemplate,
    //Application.Services.IBasicsServices, Domain.Aggregates.Shared.Basic.AppState>
    {


        [Parameter]
        public int EquipmentTypeID { get; set; }


        [Parameter]
        public int CalibrationTypeID { get; set; }

         [Parameter]
        public int TestCodeID { get; set; }
        

        public ResponsiveTable<PieceOfEquipment> Grid { get; set; } = new ResponsiveTable<PieceOfEquipment>();
        //[CascadingParameter]public  BlazoredModalInstance _BlazoredModal { get; set; }
        [CascadingParameter] public IModalService Modal { get; set; }

        public PieceOfEquipmentGRPC _poeGrpc { get; set; }

        

        [Parameter]
        public bool Accredited { get; set; }

        [Parameter]
        public bool Checkbox { get; set; }


        [Parameter]
        public CalibrationSaaS.Domain.Aggregates.Entities.Customer Customer { get; set; }


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
        public List<PieceOfEquipment> _pieceOfEquipmentsDueDate = new List<PieceOfEquipment>();


        [Parameter]
        public ICollection<WorkOrderDetail> Parameter1 { get; set; }

        //private CalibrationSaaS.Pagination<PieceOfEquipment> pag = new CalibrationSaaS.Pagination<PieceOfEquipment>();
        protected override async Task OnInitializedAsync()
        {
            paginaSize =5;
           
            foreach (var uy in pieceOfEquipmentsCh)
            {
                _pieceOfEquipmentsFiltered.Add(uy);
            }


            await base.OnInitializedAsync();


        }


        

        public override async Task<ResultSet<PieceOfEquipment>> LoadData(Pagination<PieceOfEquipment> pag)
        {
            _poeGrpc = new PieceOfEquipmentGRPC(Client, DbFactory);

            PieceOfEquipment poe = new PieceOfEquipment();
            poe.IsWeigthSet = true;
            poe.IsForAccreditedCal = Accredited;
            poe.CustomerId = CustomerId;
            poe.Customer = Customer;
            pag.Entity = poe;
            pag.Entity.EquipmentTemplate = new EquipmentTemplate();
            //yp pag.Entity.EquipmentTemplate.EquipmentTypeObject = new EquipmentType();

            //yp pag.Entity.EquipmentTemplate.EquipmentTypeObject.CalibrationTypeID = CalibrationTypeID;

            //yp pag.Entity.EquipmentTemplate.EquipmentTypeObject.EquipmentTypeID = EquipmentTypeID;


            TestCode t = new TestCode();

            t.TestCodeID = TestCodeID;
            pag.Other = TestCodeID;
            //var Eq = (await _poeGrpc.GetPieceOfEquipmentByCustomer(pag));

            var Eq = (await _poeGrpc.GetPOEByTestCodePag(pag));


            
            return Eq;
        }

        public async Task<bool> Delete(PieceOfEquipment DTO)
        {
            //await searchComponent.ShowModalAction();
            PieceOfEquipmentGRPC poe = new PieceOfEquipmentGRPC(Client, DbFactory);

            await poe.DeletePieceOfEquipment(DTO, new CallOptions());


            return true;

            //searchComponent.ShowResult();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }


            try
            {
                //  if(_pieceOfEquipmentsFiltered != null)
                //{
                //    foreach(var ite in _pieceOfEquipmentsFiltered)
                //    {

                //    }


                //}


                FormName = "Search Piece of Equipment";
                TypeName = "PieceOfEquipment";
                IsModal = IsModal;
                SelectOnly = SelectOnly;




            }
            catch (Exception ex)
            {
                await ExceptionManager(ex);
            }

        }



#pragma warning disable CS0108 // 'POEWO_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<PieceOfEquipment, Func<IIndexedDbFactory, IPieceOfEquipmentService<CallContext>>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intenci�n era ocultarlo.
        public void Dispose()
#pragma warning restore CS0108 // 'POEWO_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<PieceOfEquipment, Func<IIndexedDbFactory, IPieceOfEquipmentService<CallContext>>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intenci�n era ocultarlo.
        {

        }

        public async Task<List<PieceOfEquipment>> ReturnSelect()
        {
            //await BlazoredModal.CloseAsync(ModalResult.Ok(Grid.CheckList));

            foreach (var item in Grid.CheckList)
            {
                await AddToList(item);
            }



            return _pieceOfEquipmentsFiltered;


        }

        public async Task ReturnModal()
        {
          


            await BlazoredModal.CloseAsync(ModalResult.Ok(await ReturnSelect()));


        }
        public string btnSelectAllText = "Deselect All";

        public bool selectState { get; set; } 

        public bool? selectAllButton { get; set; }
        public async Task SelectAll()
        {
            selectAllButton = true;
            if (selectState && selectState == true)
            {
                btnSelectAllText = "Select All";
                _pieceOfEquipmentsFiltered.Clear();
                selectState = false;
            }
            else
            {
                btnSelectAllText = "Deselect All";

                //foreach (var item in Grid.Items)
                //{
                //   await  addToListPoE(item.WorkOrderDetailID,false);
                //}
                _pieceOfEquipmentsFiltered = Grid.Items.ToList();

                selectState = true;
            }



            //WODOnlineList.Clear();

            StateHasChanged();

            selectAllButton = false;


        }

        public async Task addToListPoE(string ID)

        {
              
            if (_pieceOfEquipmentsFiltered == null)
            {
                _pieceOfEquipmentsFiltered = new List<PieceOfEquipment>();
            }


            if (Grid.Items != null && _pieceOfEquipmentsFiltered != null)
            {

                var listPoe = Grid.Items.Where(x => x.PieceOfEquipmentID == ID).FirstOrDefault();
                PieceOfEquipment _poe = new PieceOfEquipment();
                _poe = new PieceOfEquipment()
                {
                    PieceOfEquipmentID = ID,
                    SerialNumber = listPoe.SerialNumber,
                    EquipmentTemplate = listPoe.EquipmentTemplate,
                    Customer = listPoe.Customer,
                    Capacity = listPoe.Capacity,
                    UnitOfMeasureID = listPoe.UnitOfMeasureID,
                    Status = listPoe.Status,
                    DueDate = listPoe.DueDate

                };
                try
                {

                    Console.WriteLine(_pieceOfEquipmentsFiltered.Count);
                    paginasTotales = ((int)(_pieceOfEquipmentsFiltered.Count / paginaSize)) + 1;

         
                    var xx = _pieceOfEquipmentsFiltered.Where(x => x.PieceOfEquipmentID == ID).ToArray();
                    if (xx != null && xx?.Count() > 0)
                    {
                        var res = _pieceOfEquipmentsFiltered.Remove(xx[0]);
                    }
                    else
                    {
                        _pieceOfEquipmentsFiltered.Add(_poe);
                    }
                    Console.WriteLine(_pieceOfEquipmentsFiltered.Count);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            //await BlazoredModal.Close(ModalResult.Ok(searchComponent.Detail));

        }



        public async Task AddToList(PieceOfEquipment Entity)
        {
            
            
            string ID = Entity.PieceOfEquipmentID;


            if (_pieceOfEquipmentsFiltered == null)
            {
                _pieceOfEquipmentsFiltered = new List<PieceOfEquipment>();
            }


            if (Grid.Items != null && _pieceOfEquipmentsFiltered != null)
            {

                var listPoe = Grid.Items.Where(x => x.PieceOfEquipmentID == ID).FirstOrDefault();
                PieceOfEquipment _poe = new PieceOfEquipment();
                _poe = new PieceOfEquipment()
                {
                    PieceOfEquipmentID = ID,
                    SerialNumber = listPoe.SerialNumber,
                    EquipmentTemplate = listPoe.EquipmentTemplate,
                    Customer = listPoe.Customer,
                    Capacity = listPoe.Capacity,
                    UnitOfMeasureID = listPoe.UnitOfMeasureID,
                    Status = listPoe.Status,
                    DueDate = listPoe.DueDate

                };
                try
                {

                    Console.WriteLine(_pieceOfEquipmentsFiltered.Count);
                    paginasTotales = ((int)(_pieceOfEquipmentsFiltered.Count / paginaSize)) + 1;


                    var xx = _pieceOfEquipmentsFiltered.Where(x => x.PieceOfEquipmentID == ID).ToArray();
                    if (xx != null && xx?.Count() > 0)
                    {
                        var res = _pieceOfEquipmentsFiltered.Remove(xx[0]);
                    }
                    else
                    {
                        _pieceOfEquipmentsFiltered.Add(_poe);
                    }
                    Console.WriteLine(_pieceOfEquipmentsFiltered.Count);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            //await BlazoredModal.Close(ModalResult.Ok(searchComponent.Detail));

        }



#pragma warning disable CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task DownloadCertificate(string certNumber)
#pragma warning restore CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

        }






    }
}
