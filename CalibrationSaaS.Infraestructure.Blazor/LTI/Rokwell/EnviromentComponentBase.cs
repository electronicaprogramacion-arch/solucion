
using AKSoftware.Blazor.Utilities;
using Blazed.Controls.Toast;
using Blazored.Modal;
using Blazored.Modal.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.Helper;
using CalibrationSaaS.Infraestructure.Blazor.Pages.AssetsBasics;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics.TestPoints;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Order;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using Reports.Domain.ReportViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.Rockwell
{
    public class EnviromentComponentBase :
        Base_Create<PieceOfEquipment, Application.Services.IPieceOfEquipmentService<CallContext>, Domain.Aggregates.Shared.Basic.AppState>
    {
        [Parameter]
        public bool IsAsFound { get; set; }

        public AppState AppState { get; set; }



        [CascadingParameter(Name = "CascadeParam1")]
        public IWorkOrderItemCreate WorkOrderItemCreate { get; set; }

        public string UsersAccess { get; set; } = "HasAccess";


        public ExternalCondition EnviromentCondition { get; set; } = new ExternalCondition(); 


        public List<PieceOfEquipment> TemperatureStandardList { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            EnviromentCondition.IsAsFound = IsAsFound;
            if (WorkOrderItemCreate.eq.EnviromentCondition == null)
            {
                WorkOrderItemCreate.eq.EnviromentCondition = new List<ExternalCondition>();
            }
                if (firstRender && EnviromentCondition != null && WorkOrderItemCreate.eq.EnviromentCondition != null)
            {
               
                  var envCondition = WorkOrderItemCreate.eq.EnviromentCondition.Where(x => x.IsAsFound == IsAsFound).FirstOrDefault();

                    if (envCondition != null) 
                    {
                        EnviromentCondition = envCondition;
                    }
                //}
                //else
                //{

                //    EnviromentCondition = new ExternalCondition();
                //    EnviromentCondition.IsAsFound = IsAsFound;
                //}




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
         
         
            NameValidationMessage = "valid";

            EnviromentCondition.IsAsFound = IsAsFound;

        }




    }
}
