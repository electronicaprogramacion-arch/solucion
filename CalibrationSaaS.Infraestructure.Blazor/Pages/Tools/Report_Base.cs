using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Tools
{
    public class Report_Base : CalibrationSaaS.Infraestructure.Blazor.KavokuComponentBase<int>
    {


        [Parameter]
        public WorkOrderDetail WOD { get; set; }

         [Parameter]
        public PieceOfEquipment POE { get; set; }

        [Parameter]
        public bool Enabled { get; set; }

        
        [Inject] Reports.Domain.ReportViewModels.Repeatability repeatability { get; set; } //YPPP
        [Inject] Reports.Domain.ReportViewModels.PointDecresingNoCorner pointDecresingNoCorner { get; set; } //YPPP
        [Inject] Reports.Domain.ReportViewModels.Header header { get; set; } //YPPP
#pragma warning disable CS0108 // 'Report_Base.JSRuntime' oculta el miembro heredado 'KavokuComponentBase<int>.JSRuntime'. Use la palabra clave new si su intención era ocultarlo.
        [Inject] IJSRuntime JSRuntime { get; set; }
#pragma warning restore CS0108 // 'Report_Base.JSRuntime' oculta el miembro heredado 'KavokuComponentBase<int>.JSRuntime'. Use la palabra clave new si su intención era ocultarlo.
        [Inject] System.Net.Http.HttpClient Http { get; set; }
        [Inject] Application.Services.IPieceOfEquipmentService<CallContext> Client { get; set; }
        [Inject] IConfiguration Configuration { get; set; }

        public string b64 { get; set; }
        private IEnumerable<Domain.Aggregates.Entities.Customer> Customers { get; set; } = new List<Domain.Aggregates.Entities.Customer>();


        protected override async Task OnInitializedAsync()
        {
            Tenant tenant = new Tenant();


             PieceOfEquipmentGRPC poegrpc = new PieceOfEquipmentGRPC(Client);

             POE = await poegrpc.GetPieceOfEquipmentXId(WOD.PieceOfEquipmentId);

            var reportCutomer = new Reports.Domain.ReportViewModels.Customer();
            var reportPoints = new Reports.Domain.ReportViewModels.PointDecresingNoCorner();
            var reportHeader = new Reports.Domain.ReportViewModels.Header();

            //https://docs.microsoft.com/en-us/aspnet/core/blazor/call-web-api?view=aspnetcore-3.1#:~:text=Blazor%20WebAssembly%20apps%20call%20web,to%20the%20server%20of%20origin.


            repeatability.CustomerName = POE.Customer.Name; // "Test Customer";

            pointDecresingNoCorner.CustomerId = POE.CustomerId; // 777;
            header.Client = "Bitterman Scales";
            header.Address = "2445C Old Philadelphia Pike";
            header.Country = "Lancaster";
            header.EquipmentLocation = POE?.InstallLocation;// "Location Test";
            header.EquipmentType = POE?.EquipmentTemplate?.EquipmentTypeObject.Name;// "Type Test";
            header.NextCalDate = WOD.CalibrationCustomDueDate.Value.ToShortDateString();//DateTime.Now.ToString();
            header.LastCalDate = DateTime.Now.ToString();
            header.ManufacturerInd = POE.Indicator.EquipmentTemplate.Manufacturer1.Name;// "Rice Lake";
            header.ManufacturerReceiv = POE.EquipmentTemplate.Manufacturer1.Name; //"LOCOSO";
            header.ModelInd = POE.Indicator.EquipmentTemplate.Model; // "120 Plus";
            header.SerialInd = POE.Indicator.SerialNumber; // "J30019";
            header.CapInd = POE?.Indicator?.Capacity.ToString(); // "1000 x 0.2 lb";
            header.ModelIndReceiv = POE?.EquipmentTemplate.Model;// "LP7620";
            header.SerialIndReceiv = POE?.SerialNumber; // "64643";
            header.CapIndReceiv =POE?.Capacity.ToString();// "1000 lb";
            header.Class = POE?.Class;//"III";
            header.Type = POE?.EquipmentTemplate.DeviceClass;// "Electrónic";
            header.PlatformSize = POE?.EquipmentTemplate?.PlatformSize; // "24 x 24";
            header.ServiceLocation = POE.InstallLocation; ;// "Location Test";


            var jsonRepeatability = JsonConvert.SerializeObject(repeatability);
            var jsonPointDecNC = JsonConvert.SerializeObject(pointDecresingNoCorner);
            var jsonHeader = JsonConvert.SerializeObject(header);



            //Console.WriteLine("jsonPointDecNC", jsonPointDecNC);
            //Console.WriteLine("Header ", jsonHeader);


            System.Net.ServicePointManager.SecurityProtocol =
                System.Net.SecurityProtocolType.Tls
                | System.Net.SecurityProtocolType.Tls11
                | System.Net.SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };


            var content = new StringContent(jsonRepeatability, System.Text.Encoding.UTF8, "application/json");
            var contentPNC = new StringContent(jsonPointDecNC, System.Text.Encoding.UTF8, "application/json");
            var contentHeader = new StringContent(jsonHeader, System.Text.Encoding.UTF8, "application/json");
            //Console.WriteLine("contentPNC " + contentPNC);

            //Http.BaseAddress = new System.Uri("https://localhost:44388/");
            Http.BaseAddress = Http.BaseAddress = new System.Uri(Configuration.GetSection("Reports")["Url"]);
            // new System.Uri(Configuration.GetSection("Reports")["URL"]);
            //Console.WriteLine("URL  " + Http.BaseAddress);
            Http.DefaultRequestHeaders.Accept.Clear();
            Http.DefaultRequestHeaders.Accept.Add(new
              MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage pdf = null;

            try
            {

                pdf = await Http.PostAsync("/GetCustomerCover", content);
            }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            {
                await ShowError("Connection error");
                return;
            }
            if (pdf == null)
            {
                await ShowError("Customer Header not found");
                return;
            }
            var contentReponse = await pdf.Content.ReadAsStringAsync();

            //Logger.LogDebug("contentReponse " + contentReponse);

            var psdInBase64 = JsonConvert.DeserializeObject(contentReponse).ToString();
            b64 = psdInBase64;

            //Console.WriteLine("psdInBase  " + psdInBase64);

            await JSRuntime.InvokeVoidAsync("PrintPDF", psdInBase64);


        }




    }
}
