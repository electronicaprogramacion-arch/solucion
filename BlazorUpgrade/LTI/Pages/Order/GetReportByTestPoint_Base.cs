using Blazored.Modal.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Tools1
{
    public class GetReportByTestPoint_Base : ComponentBase
    {

        [Parameter]
        public string TestPointID { get; set; }
        [Parameter]
        public string workOrderDetailID { get; set; }
        [CascadingParameter] public IModalService Modal { get; set; }
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
            Linearity linearity = new Linearity();

            linearity.TestPointID = Convert.ToInt32(TestPointID);
            linearity.WorkOrderDetailId = Convert.ToInt32(workOrderDetailID);

            //CalibrationSaaS.Infraestructure.Blazor.Services.UOMServiceGRPC _serviceGRPC =     new CalibrationSaaS.Infraestructure.Blazor.Services.UOMServiceGRPC(_reportService);
            //   CalibrationSaaS.Infraestructure.Blazor.Services.ReportServiceGRPC _serviceGRPC =  new CalibrationSaaS.Infraestructure.Blazor.Services.ReportServiceGRPC(;

            var result = await _reportService.GetReportUncertaintyBudget(linearity, new CallContext()); //_wodService.GetHistory(wod);

            //**************************************
            await JSRuntime.InvokeVoidAsync("PrintPDF", result);


        }




    }
}
