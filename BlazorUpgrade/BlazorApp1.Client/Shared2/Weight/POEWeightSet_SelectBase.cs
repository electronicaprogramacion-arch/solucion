using Blazor.IndexedDB.Framework;
using Blazored.Modal.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalibrationSaaS.Domain.Aggregates.Security;

namespace BlazorApp1.Blazor.Blazor.GenericMethods
{
    public class POEWeightSet_SelectBase : Base_Create<PieceOfEquipment,
        Func<dynamic, CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext>>,
        CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany>
    //, IPage<PieceOfEquipment, Application.Services.IPieceOfEquipmentService<CallContext>, Domain.Aggregates.Shared.Basic.AppState>
    //IPage<EquipmentTemplate,
    //Application.Services.IBasicsServices, Domain.Aggregates.Shared.Basic.AppState>
    {
        public string MessageResult { get; set; }

        public ResponsiveTable<PieceOfEquipment> Grid { get; set; } = new ResponsiveTable<PieceOfEquipment>();
        //[CascadingParameter]public  BlazoredModalInstance _BlazoredModal { get; set; }
        [CascadingParameter] public IModalService Modal { get; set; }

        public PieceOfEquipmentGRPC _poeGrpc { get; set; }



        [Parameter]
        public int CalibrationTypeID { get; set; }

        [Parameter]
        public bool Accredited { get; set; }

        [Parameter]
        public bool Checkbox { get; set; }


        [Parameter]
        public CalibrationSaaS.Domain.Aggregates.Entities.Customer Customer { get; set; }


        [Parameter]
        public bool Indicator { get; set; }

        [Parameter]
        public string Parameter1 { get; set; }

        public CustomerGRPC _customerGrpc { get; set; }

   

        [Parameter]
        public List<PieceOfEquipment> pieceOfEquipmentsCh { get; set; }


        public List<PieceOfEquipment> _pieceOfEquipmentsFiltered { get; set; } = new List<PieceOfEquipment>();

        // protected List<EquipmentType> FilteredToDos => EquipmentTypeList.Where(i => i.Name.ToLower().Contains(SearchTerm.ToLower())).ToList();
        public List<PieceOfEquipment> _pieceOfEquipmentsDueDate = new List<PieceOfEquipment>();

        //private CalibrationSaaS.Pagination<PieceOfEquipment> pag = new CalibrationSaaS.Pagination<PieceOfEquipment>();
        protected override async Task OnInitializedAsync()
        {
            paginaSize = 5;

            if (pieceOfEquipmentsCh != null)
            {
                foreach (var uy in pieceOfEquipmentsCh)
                {
                    _pieceOfEquipmentsFiltered.Add(uy);
                }

                await base.OnInitializedAsync();
            }

        }


        public override async Task<ResultSet<PieceOfEquipment>> LoadData(Pagination<PieceOfEquipment> pag)
        {
            _poeGrpc = new PieceOfEquipmentGRPC(Client, DbFactory);

            PieceOfEquipment poe = new PieceOfEquipment();

            poe.IsWeigthSet = true;

            poe.IsForAccreditedCal = Accredited;
            //poe.Users. = Parameter1;
            poe.Customer = Customer;

            poe.EquipmentTemplate = new EquipmentTemplate();
            
            
            poe.EquipmentTemplate.EquipmentTypeObject = new EquipmentType();
            
            if (!string.IsNullOrEmpty(Parameter1))
            {
                //poe.EquipmentTemplate.EquipmentTypeObject.EquipmentTypeID = Parameter1;
                pag.EntityType = Parameter1;

            }
           
            
            poe.EquipmentTemplate.EquipmentTypeObject.CalibrationTypeID = CalibrationTypeID;

            pag.Entity = poe;



            var Eq = (await _poeGrpc.GetAllWeightSetsPag(pag));

            MessageResult = Eq.Message;


            return Eq;
        }



        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            { 
            if (!firstRender)
            {
                var uer = await CurrentUserName();

                //Component.User = uer;
                //Component.Permission = Helpers.Controls.Policies.HasFullAccess;
                //Component.Group = Component.Group + ",job.HasView,job.HasEdit,job.HasSave";
                //Component.Name = "SelectWODWeight";
                //TypeName = "SelectWODWeight";
                //return;
            }


        
               
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


#pragma warning disable CS0108 // 'POEWeightSet_SelectBase.Dispose()' oculta el miembro heredado 'Base_Create<PieceOfEquipment, Func<IIndexedDbFactory, IPieceOfEquipmentService<CallContext>>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intenci�n era ocultarlo.
        public void Dispose()
#pragma warning restore CS0108 // 'POEWeightSet_SelectBase.Dispose()' oculta el miembro heredado 'Base_Create<PieceOfEquipment, Func<IIndexedDbFactory, IPieceOfEquipmentService<CallContext>>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intenci�n era ocultarlo.
        {

        }


        public async Task<List<PieceOfEquipment>> ReturnSelect()
        {
            List<PieceOfEquipment> pieces = new List<PieceOfEquipment>();


            foreach (var item in Grid.CheckList)
            {
                await AddToList(item);
            }

            foreach (var item in _pieceOfEquipmentsFiltered)
            {
                var listPoe = Grid.CheckList.Where(x => x.PieceOfEquipmentID == item.PieceOfEquipmentID).FirstOrDefault();
                if (listPoe != null)
                {
                    pieces.Add(listPoe);

                }

            }


            return pieces;
        }


        public async Task ReturnModal()
        {
            


            await BlazoredModal.CloseAsync(ModalResult.Ok(await ReturnSelect()));
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
                    DueDate = listPoe.DueDate,
                    WeightSets= listPoe.WeightSets, 
                    CalibrationSubType_Standard= listPoe.CalibrationSubType_Standard,    
                };
                try
                {

                    Console.WriteLine(_pieceOfEquipmentsFiltered.Count);
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
                    Console.WriteLine("List Error");
                    Console.WriteLine(ex.Message);
                }

            }
           

        }



        public async Task AddToList(PieceOfEquipment Entity)
        {

            string ID = Entity.PieceOfEquipmentID;


            if (_pieceOfEquipmentsFiltered == null)
            {
                _pieceOfEquipmentsFiltered = new List<PieceOfEquipment>();
            }


            if (Grid.ItemsDataSource != null && _pieceOfEquipmentsFiltered != null)
            {

                var listPoe = Grid.ItemsDataSource.Where(x => x.PieceOfEquipmentID == ID).FirstOrDefault();
                PieceOfEquipment _poe = new PieceOfEquipment();

                if (listPoe == null) {
                    return;
                    throw new Exception("NO ITEM FOUND");
                
                }

                _poe = new PieceOfEquipment()
                {

                    PieceOfEquipmentID = ID,
                    SerialNumber = listPoe?.SerialNumber,
                    EquipmentTemplate = listPoe?.EquipmentTemplate,
                    Customer = listPoe?.Customer,
                    Capacity = listPoe.Capacity,
                    UnitOfMeasureID = listPoe?.UnitOfMeasureID,
                    Status = listPoe.Status,
                    DueDate = listPoe.DueDate,
                    WeightSets = listPoe.WeightSets,
                    CalibrationSubType_Standard = listPoe.CalibrationSubType_Standard,
                };
                try
                {

                    Console.WriteLine(_pieceOfEquipmentsFiltered.Count);
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
                    Console.WriteLine("List Error");
                    Console.WriteLine(ex.Message);
                }

            }


        }



        public async Task DownloadCertificate(string certNumber)
        {

        }

        public async Task<bool> Delete(PieceOfEquipment DTO)
        {
            return true;
        }




    }
}