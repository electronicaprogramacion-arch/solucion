
using AKSoftware.Blazor.Utilities;
using Blazed.Controls.Toast;
using Blazored.Modal;
using Blazored.Modal.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;

using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;

using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp1.Blazor.Pages.Order
{
    public class EnviromentLTIComponentBase :
        Base_Create<PieceOfEquipment, CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext>, CalibrationSaaS.Domain.Aggregates.Shared.Basic.AppState>
    {

        public List<PieceOfEquipment> TemperatureStandardList { get; set; }


        [CascadingParameter(Name = "CascadeParam1")]
        public BlazorApp1.Blazor.Pages.Order.WorkOrderItemCreate WorkOrderItemCreate { get; set; }

        public CalibrationType calibrationTypeEnviroment { get; set; }
        public string messageHumidity { get; set; }
        public string messageTempBefore { get; set; }
        public string messageTempAfter { get; set; }
        private bool hasRendered = false;
        private double? previousHumidity;
        private double? previousTemperature;
        private double? previousTemperatureAfter;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            bool hasChanges = false;

                if (WorkOrderItemCreate.PieceOfEquipment != null &&
                WorkOrderItemCreate.PieceOfEquipment.EquipmentTemplate != null &&
                WorkOrderItemCreate.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject != null &&
                WorkOrderItemCreate.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject.CalibrationType != null)
            {
                calibrationTypeEnviroment = WorkOrderItemCreate.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject.CalibrationType;

                // Verificar cambios en Humidity
                if (calibrationTypeEnviroment.MinHumidity != null && calibrationTypeEnviroment.MaxHumidity != null)
                {
                    double currentHumidity = WorkOrderItemCreate.eq.Humidity;
                    if (previousHumidity != currentHumidity)
                    {
                        previousHumidity = currentHumidity;
                        hasChanges = true;

                        messageHumidity = (currentHumidity < calibrationTypeEnviroment.MinHumidity ||
                                           currentHumidity > calibrationTypeEnviroment.MaxHumidity)
                                          ? "Out of Tolerance" : null;
                    }
                }

                // Verificar cambios en Temperature
                if (calibrationTypeEnviroment.MinTemperature != null && calibrationTypeEnviroment.MaxTemperature != null)
                {
                    double currentTemperature = WorkOrderItemCreate.eq.Temperature;
                    if (previousTemperature != currentTemperature)
                    {
                        previousTemperature = currentTemperature;
                        hasChanges = true;

                        messageTempBefore = (currentTemperature < calibrationTypeEnviroment.MinTemperature || currentTemperature > calibrationTypeEnviroment.MaxTemperature) ? "Out of Tolerance" : null;
                    }

                    // Verificar cambios en TemperatureAfter
                    double currentTemperatureAfter = WorkOrderItemCreate.eq.TemperatureAfter;
                    if (previousTemperatureAfter != currentTemperatureAfter)
                    {
                        previousTemperatureAfter = currentTemperatureAfter;
                        hasChanges = true;

                        messageTempAfter = (currentTemperatureAfter < calibrationTypeEnviroment.MinTemperature ||
                                            currentTemperatureAfter > calibrationTypeEnviroment.MaxTemperature)
                                           ? "Out of Tolerance" : null;
                    }
                }
            }

            if (hasChanges)
            {
                StateHasChanged();
            }

        }

        protected override async Task OnInitializedAsync()
        {

            //await base.OnParametersSetAsync();
            await base.OnInitializedAsync();

          

                if (TemperatureStandardList == null)
                {
                    var eTemperatureStandard = await Client.GetTemperatureStandard(new CallOptions());


                    TemperatureStandardList = eTemperatureStandard.ToList();

                }
         

            

            //Manufacturers = eManufactures.Manufacturers;
            //}

            //StatusList = AppState.StatusList;

            //if (StatusList == null)
            //{

            //    Logger.LogDebug("es null");
            //}
            //Logger.LogDebug(StatusList.Count);

            //CurrentEditContext.OnFieldChanged += HandleFieldChanged;
            //StateHasChanged();
            NameValidationMessage = "valid";


            //  MessagingCenter.Subscribe<CalibrationSaaS.Infraestructure.Blazor.LTI.UncertaintyComponent, Uncertainty>(this, "item_added", (sender, args) =>
            //{
            //    // Recaulcate the items in the cart 
            //    //_cartItems = GetCartListFromCart();

            //    // Notify the UI about the change
            //    StateHasChanged(); 
            //});



        }




    }
}
