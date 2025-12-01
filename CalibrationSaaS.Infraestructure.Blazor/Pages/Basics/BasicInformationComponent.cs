
using Blazed.Controls.Toast;
using Blazored.Modal;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ProtoBuf.Grpc;
using System;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Basics
{
    public class BasicInformation_CreateBase :
        Base_Create<EquipmentTemplate, Application.Services.IBasicsServices<CallContext>, Domain.Aggregates.Shared.Basic.AppState>
    {

        [Inject] public Application.Services.IAssetsServices<CallContext> _assetServices { get; set; }
        public string selectedAnswer = "";

        public bool IsPercentage = false;

        public ResolutionComponent ResolutionComponent;
     

        protected override async Task OnAfterRenderAsync(bool firstRender)

        {

            if ( ResolutionComponent != null && eq != null)
            {

                var eqq = eq;

                ResolutionComponent.Show(eqq);

            }
           
        }


        protected override async Task OnParametersSetAsync()

        {


        }

        protected async Task FormSubmitted(EditContext editContext)
        {
            bool re = await ContextValidation(true);
            var msg = ValidationMessages;
            
            if (re)
            {

                try
                {

                    BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);


                    Result = (await basics.CreateEquipment(eq));


                    await ShowToast("The information has been saved successfully.", ToastLevel.Success);

                    await CloseModal(Result);

                }

                catch (RpcException ex)
                {

                    await ExceptionManager(ex);

                }
                catch (Exception ex)
                {
                    await ExceptionManager(ex);

                }




            }
            else
            {

                await CloseProgress();

            }
        }



        protected override async Task OnInitializedAsync()
        {

            BasicsServiceGRPC Client2 = new BasicsServiceGRPC(Client);

            await base.OnInitializedAsync();

            TypeName = "Equipment";

            if (AppState?.EquipmentTypes?.Count == 0)
            {
                var etypes = await Client2.GetEquipmenTypes(new CallOptions());

                foreach (var item in etypes.EquipmentTypes)
                {
                    AppState.AddEquipmentType(item);
                }
          

            }
            else
            {


            }

            if (AppState.Manufacturers.Count == 0)
            {
                var eManufactures = await Client2.GetAllManufacturers(new CallOptions());

                foreach (var item in eManufactures.Manufacturers)
                {
                    AppState.AddManufacturer(item);
                }

               
            }

            if (AppState.CalibrationTypes.Count == 0)
            {
                var eCalibrationType = await _assetServices.GetCalibrationType();

                foreach (var item in eCalibrationType.CalibrationTypes)
                {
                    AppState.AddCalibrationType(item);
                }

               
            }
           
            NameValidationMessage = "valid";


            ModalParameters.Add("Enabled2", false);


        }


        protected void FunctionName(EventArgs args)
        {
          

            if (eq.Name.Length < 1)
            {
                NameValidationMessage = "invalid";
            }
            else
            {
                NameValidationMessage = "valid";
            }
        
            StateHasChanged();

        }



        public new void Dispose()
        {

        }

    }
}
