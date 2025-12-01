using CalibrationSaaS.Domain.Aggregates.Entities;
using Grpc.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Tools
{
    public class GetSticker_Base : CalibrationSaaS.Infraestructure.Blazor.KavokuComponentBase<int>
    {
       
        [Parameter]
        public bool Enabled { get; set; }

        // [Inject] public Application.Services.IUOMService<CallContext> _reportService { get; set; }
        //[Inject] public Application.Services.IWorkOrderDetailServices _wodService { get; set; }
        [Inject] public Application.Services.IReportService<CallContext> _reportService { get; set; }

        // [Inject] public Application.Services.IReportService<CallContext> _reportService { get; set; }
#pragma warning disable CS0108 // 'GetSticker_Base.JSRuntime' oculta el miembro heredado 'KavokuComponentBase<int>.JSRuntime'. Use la palabra clave new si su intención era ocultarlo.
        [Inject] IJSRuntime JSRuntime { get; set; }
#pragma warning restore CS0108 // 'GetSticker_Base.JSRuntime' oculta el miembro heredado 'KavokuComponentBase<int>.JSRuntime'. Use la palabra clave new si su intención era ocultarlo.
        [Inject] System.Net.Http.HttpClient Http { get; set; }
        [Inject] Application.Services.ICustomerService<CallContext> Client { get; set; }
        [Inject] IConfiguration Configuration { get; set; }

        public string b64 { get; set; }
        public string url { get; set; }
        private IEnumerable<Domain.Aggregates.Entities.Customer> Customers { get; set; } = new List<Domain.Aggregates.Entities.Customer>();

        protected override async Task OnInitializedAsync()
        {

            ///---***********************************************************

            WorkOrderDetail wod = new WorkOrderDetail();
            wod.WorkOrderDetailID = Convert.ToInt32(EntityID);


            //CalibrationSaaS.Infraestructure.Blazor.Services.UOMServiceGRPC _serviceGRPC = new CalibrationSaaS.Infraestructure.Blazor.Services.UOMServiceGRPC(_reportService);
            // CalibrationSaaS.Infraestructure.Blazor.Services.ReportServiceGRPC _serviceGRPC = new CalibrationSaaS.Infraestructure.Blazor.Services.ReportServiceGRPC(_reportService);
#pragma warning disable CS0219 // La variable 'sticker' está asignada pero su valor nunca se usa
            object sticker = null;
#pragma warning restore CS0219 // La variable 'sticker' está asignada pero su valor nunca se usa


            //var result = await _serviceGRPC.GetSticker(wod, new CallContext()); //_wodService.GetHistory(wod);
            try
            {
                var result = await _reportService.GetSticker(wod, new CallContext());
                b64 = result;

                await JSRuntime.InvokeVoidAsync("PrintPDF", result, 4);

                await JSRuntime.InvokeVoidAsync("SetReport", b64);




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




    }
}
