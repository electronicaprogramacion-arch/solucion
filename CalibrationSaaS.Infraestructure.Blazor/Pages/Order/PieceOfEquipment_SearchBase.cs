using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using CalibrationSaaS.Infraestructure.Blazor.Shared;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using CalibrationSaaS.Domain.Aggregates.Entities;
using Microsoft.AspNetCore.Components.Forms;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Web;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using Blazored.Modal;
using Blazored.Modal.Services;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using System.Runtime.CompilerServices;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Shared.Component;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Order
{
    public class PieceOfEquipment_SearchBase : Base_Create<PieceOfEquipment, Application.Services.IPieceOfEquipmentService<, Domain.Aggregates.Shared.AppStateCompany>, IPage<PieceOfEquipment, Application.Services.IPieceOfEquipmentService, Domain.Aggregates.Shared.Basic.AppState>
        //IPage<EquipmentTemplate,
        //Application.Services.IBasicsServices, Domain.Aggregates.Shared.Basic.AppState>
    {
        [CascadingParameter] BlazoredModalInstance _BlazoredModal { get; set; }
        [CascadingParameter] public IModalService Modal { get; set; }


        [Parameter]
        public bool SelectOnly { get; set; }

        [Parameter]
        public bool IsModal { get; set; }

        [Parameter]
        public bool Checkbox { get; set; }

      
        public Search<PieceOfEquipment, IPieceOfEquipmentService, AppState> searchComponent 
        { get ; set ; }

       


        public List<PieceOfEquipment> _pieceOfEquipmentsFiltered = new List<PieceOfEquipment>();
       // protected List<EquipmentType> FilteredToDos => EquipmentTypeList.Where(i => i.Name.ToLower().Contains(SearchTerm.ToLower())).ToList();
        public List<PieceOfEquipment> _pieceOfEquipmentsDueDate = new List<PieceOfEquipment>();
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
           

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }

           

          
        //searchComponent = new Search<EquipmentTemplate, IBasicsServices, AppState>();

        searchComponent.BaseCreateUrl = "PieceOfEquipmentCreate";

            searchComponent.BaseDetailUrl = "EquipmentDetail";

            searchComponent.FormName = "Search Piece of Equipment";

            searchComponent.TypeName = "PieceOfEquipment";

            searchComponent.IsModal = IsModal;

            searchComponent.SelectOnly = SelectOnly;
            

                //var Eq = (await searchComponent.Client.GetEquipment());

                //searchComponent.List = Eq.EquipmentTemplates;
                TenantDTO tenant = new TenantDTO();
                if (searchComponent.Client != null)
                {
                    

                    var Eq = (await searchComponent.Client.GetPieceOfEquipmentWeightSet(tenant));
               
                    if (Eq != null)
                    {
                   
                        searchComponent.List = Eq.PieceOfEquipments;

                        searchComponent.FilteredToDos = Eq.PieceOfEquipments;

                        StateHasChanged();
                    }
                }
                else
                {
                    //Console.WriteLine("null data");
                }
            
        }

       
      
        public async Task GetDetail(int ID)
        {
            searchComponent.Detail = searchComponent.List.Where(x => x.PieceOfEquipmentId == ID).FirstOrDefault();


            //var messageForm = Modal.Show<Equipment_Create>("Passing Data");
            //var result = await messageForm.Result;
            await searchComponent.ShowModal();

            StateHasChanged();

            //await BlazoredModal.Close(ModalResult.Ok(searchComponent.Detail));

        }

        public void Dispose()
        {

        }

        public IEnumerable<PieceOfEquipment> FilterList(string filter)
        {
            try
            {

                List<PieceOfEquipment> lequ = searchComponent.List.Where(x => (x.Name != null && x.Name.ToLower().StartsWith(filter.ToLower()))
                ).ToList();


                if (lequ == null)
                {
                    return new List<PieceOfEquipment>();
                }

                return lequ;

            }
            catch (Exception ex)
            {
                return new List<PieceOfEquipment>();

            }
        }

       // public async Task SelectModal(PieceOfEquipment DTO)
        public async Task SelectModal(PieceOfEquipment DTO)
        {
            await _BlazoredModal.Close(ModalResult.Ok(searchComponent.Detail));
        }

        public async Task ReturnModal()
        {
            await _BlazoredModal.Close(ModalResult.Ok(_pieceOfEquipmentsFiltered));
        }


        public async Task addToListPoE(int ID)
        {
            PieceOfEquipment _poe = new PieceOfEquipment();
         
           var listPoe = _pieceOfEquipmentsDueDate.Where(x=>x.PieceOfEquipmentId==ID).FirstOrDefault();

            _poe = new PieceOfEquipment() {
                PieceOfEquipmentId = ID,
                SerialNumber = listPoe.SerialNumber
            };

           
            _pieceOfEquipmentsFiltered.Add(_poe);

            //await BlazoredModal.Close(ModalResult.Ok(searchComponent.Detail));

        }

        public async Task DownloadCertificate(string certNumber)
        { 
           
        }

        public async  Task Delete(PieceOfEquipment DTO)
        {
           await searchComponent.ShowModalAction();
            
            //searchComponent.Detail =  await searchComponent.Client.de(DTO);
            
            searchComponent.ShowResult();
        }



      
    }
}
