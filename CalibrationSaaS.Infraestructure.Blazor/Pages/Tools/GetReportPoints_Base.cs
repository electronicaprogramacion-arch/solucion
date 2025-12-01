using CalibrationSaaS.Domain.Aggregates.Entities;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using Grpc.Core;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Tools
{
    public class GetReportPoints_Base : KavokuComponentBase<WorkOrderDetail>
    {
      


        [Parameter]
        public bool Enabled { get; set; }


        //[Inject] public Application.Services.IUOMService<CallContext> _reportService { get; set; }
        //[Inject] public Application.Services.IWorkOrderDetailServices _wodService { get; set; }
        [Inject] public Application.Services.IReportService<CallContext> _reportService { get; set; }

        // [Inject] public Application.Services.IReportService<CallContext> _reportService { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] System.Net.Http.HttpClient Http { get; set; }
        [Inject] Application.Services.ICustomerService<CallContext> Client { get; set; }
        [Inject] IConfiguration Configuration { get; set; }

        public string b64 { get; set; }
        public string url { get; set; }
        private IEnumerable<Domain.Aggregates.Entities.Customer> Customers { get; set; } = new List<Domain.Aggregates.Entities.Customer>();

        [Parameter]
        public WorkOrderDetail WorkOrderDetail { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try



            {

                await ShowProgress();

            WorkOrderDetail wod = new WorkOrderDetail();
            wod.WorkOrderDetailID = Convert.ToInt32(EntityID);

            // CalibrationSaaS.Infraestructure.Blazor.Services.UOMServiceGRPC _serviceGRPC = new CalibrationSaaS.Infraestructure.Blazor.Services.UOMServiceGRPC(_reportService);
            CalibrationSaaS.Infraestructure.Blazor.Services.ReportServiceGRPC _serviceGRPC = 
                    new CalibrationSaaS.Infraestructure.Blazor.Services.ReportServiceGRPC(_reportService);
            
            url = Configuration.GetSection("Reports")["URL"];
            //Console.WriteLine("url ---" + url);
            var result = await _serviceGRPC.GetWorkOrderDetailXIdRep(WorkOrderDetail); //_wodService.GetHistory(wod);

            b64 = result.PdfString;
            //Console.WriteLine("b64 --" + b64);
            //**************************************
            await JSRuntime.InvokeVoidAsync("PrintPDF", b64);

            await JSRuntime.InvokeVoidAsync("SetReport", b64);
            }

             catch(RpcException ex)
            {
               await  ExceptionManager(ex);
            }

            
            catch(Exception ex)
            {
               await  ExceptionManager(ex);
            }
            finally
            {
                await CloseProgress();
            }
         

        }




    }
}
