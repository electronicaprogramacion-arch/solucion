using Blazored.Modal.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
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

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Basics
{
    public class CalibrationType_DetailBase : Base_Create<CalibrationType, Application.Services.IBasicsServices<CallContext>,
        Domain.Aggregates.Shared.Basic.AppState>
  
    {


        [CascadingParameter] public IModalService Modal { get; set; }

        public ResponsiveTable<CalibrationType> Grid { get; set; } = new ResponsiveTable<CalibrationType>();


        public List<CalibrationType> ListCT = new List<CalibrationType>();


        public override async Task<ResultSet<CalibrationType>> LoadData(Pagination<CalibrationType> pag)
        {

            BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);

            //var Eq = (await basics.GetEquipmenTypesPag(pag));
            var Eq = await basics.GetCalibrationTypesPag(pag);

            
            if (Eq.List != null)
            {
                foreach (var item in Eq.List)
                {
                    var item2 = AppState.CalibrationTypes.Where(x => x.CalibrationTypeId == item.CalibrationTypeId).FirstOrDefault();

                   
                        AppState.AddCalibrationType(item);
                   

                }
            }


            return Eq; 

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }
            await base.OnParametersSetAsync();


            FormName = "Search Calibration Type";

            TypeName = "CalibrationTypeDetail";

            IsModal = IsModal;

            SelectOnly = SelectOnly;

        }



       

        public async Task<bool> Delete(CalibrationType DTO)
        {
            try
            {
               
                //BasicsServiceGRPC basic = new BasicsServiceGRPC(Client);

                //var result = await basic.DeleteEquipmentType(DTO);

                return true;
                
            }
            catch (RpcException ex)
            {

                await ExceptionManager(ex);
                return false;

            }
            catch (Exception ex)
            {
                await ExceptionManager(ex);
                return false;

            }
        }

      
        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();
           
           
           

        }

        public async Task SelectModal(CalibrationType DTO)
        {
            //await BlazoredModal.Close(ModalResult.Ok(searchComponent.Detail));
        }

    }
}
