using CalibrationSaaS.Domain.Aggregates.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Tools
{
    public class GetReportUncertBudget_Base : ComponentBase
    {
        [Parameter]
        public int WorkOrderDetailID { get; set; }

        //[Inject] public Application.Services.IUOMService<CallContext> _reportService { get; set; }
        //[Inject] public Application.Services.IWorkOrderDetailServices _wodService { get; set; }
        [Inject] public Application.Services.IReportService<CallContext> _reportService { get; set; }

        // [Inject] public Application.Services.IReportService<CallContext> _reportService { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] System.Net.Http.HttpClient Http { get; set; }
        [Inject] Application.Services.ICustomerService<CallContext> Client { get; set; }
        [Inject] IConfiguration Configuration { get; set; }

        public string b64 { get; set; }
        private IEnumerable<Domain.Aggregates.Entities.Customer> Customers { get; set; } = new List<Domain.Aggregates.Entities.Customer>();


        protected override async Task OnInitializedAsync()
        {
            WorkOrderDetail wod = new WorkOrderDetail();
            wod.WorkOrderDetailID = WorkOrderDetailID;

            //CalibrationSaaS.Infraestructure.Blazor.Services.UOMServiceGRPC _serviceGRPC =     new CalibrationSaaS.Infraestructure.Blazor.Services.UOMServiceGRPC(_reportService);
            //   CalibrationSaaS.Infraestructure.Blazor.Services.ReportServiceGRPC _serviceGRPC =  new CalibrationSaaS.Infraestructure.Blazor.Services.ReportServiceGRPC(;

            var result = await _reportService.GetReportUncertaintyBudgetComp(wod, new CallContext()); //_wodService.GetHistory(wod);
            Console.WriteLine("dfdsafsdf");
            //**************************************
            await JSRuntime.InvokeVoidAsync("PrintPDF", result);


        }




    }
}
