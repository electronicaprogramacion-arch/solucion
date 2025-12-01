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
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using CalibrationSaaS.Application.Services;
using Blazored.Modal;
using Blazored.Modal.Services;
using CalibrationSaaS.Infraestructure.Blazor.Shared.Component;
using ProtoBuf.Grpc;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Order
{
    public class Certified_SearchBase :ComponentBase,IPage<Certificate, Application.Services.IAssetsServices<CallContext>, AppState>
    {

        [Parameter]
        public PieceOfEquipment POE { get; set; }

        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; }
        [CascadingParameter] public IModalService Modal { get; set; }


        [Parameter]
        public bool SelectOnly { get; set; }

        [Parameter]
        public bool IsModal { get; set; }


        public Search<Certificate, IAssetsServices<CallContext>, AppState> searchComponent
        { get; set; }





        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }


            searchComponent.BaseCreateUrl = "CertificateCreate";

            searchComponent.BaseDetailUrl = "CertificateDetail";

            searchComponent.FormName = "Search Certificate";

            searchComponent.IsModal = IsModal;

            searchComponent.SelectOnly = SelectOnly;

            if (searchComponent.Client != null)
            {
                //PieceOfEquipment POE = new PieceOfEquipment();

                var Eq = (await searchComponent.Client.GetCertificateXPoE(POE));

                if (Eq != null)
                {
                    searchComponent.List = Eq.ToList();

                   
                    searchComponent.FilteredToDos = Eq;

                    StateHasChanged();


                }
            }

                   

           
        }

        public IEnumerable<Certificate> FilterList(string filter)
        {

              return searchComponent.List.Where(i => i.Name.ToLower().Contains(filter.ToLower())).ToList();

        }

        public async Task GetDetail(Certificate DTO)
        {
            searchComponent.Detail = DTO; // searchComponent.FilteredToDos.Where(x => x.UserID == ID).FirstOrDefault();

            await ModalDetail.ShowModal(searchComponent.Detail);
            //await ShowModal();

            //StateHasChanged();
        }

        public DetailView<Certificate> ModalDetail = new DetailView<Certificate>();

        public async Task SelectModal(Certificate DTO)
        {
            await BlazoredModal.CloseAsync(ModalResult.Ok(searchComponent.Detail));
        }

        public async Task Delete(Certificate DTO)
        {
            //await searchComponent.ShowModalAction();

            //searchComponent.Detail = await searchComponent.Client.DeleteManufacturer(DTO);

            //searchComponent.ShowResult();
        }

        public void Dispose()
        {

        }

    }
}
