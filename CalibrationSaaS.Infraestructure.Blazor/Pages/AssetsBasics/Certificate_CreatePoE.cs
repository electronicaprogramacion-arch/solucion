using Blazed.Controls;
using Blazor.Extensions.Logging;
using Blazored.Modal;
using Blazored.Modal.Services;
using BlazorInputFile;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using Helpers;
using Helpers.Controls;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Component = Helpers.Controls.Component;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.AssetsBasics
{
    public partial class Certificate_CreatePoE: ComponentBase
    {


    bool Enabled = true;
    public ResponsiveTable<Certificate> Grid { get; set; }
    [Inject] public Application.Services.IReportService<CallContext> _reportService { get; set; }
    [Inject] public CalibrationSaaS.Application.Services.IFileUpload fileUpload { get; set; }
        private string fileName = "";
        public IEnumerable<Certificate> FilterList(string filter = "")
    {

        if (Grid != null && Grid.ItemsDataSource != null)
        {
            var templist = Grid.ItemsDataSource;

          
            return templist.Where(i => i.Name.ToLower().Contains(filter.ToLower()

                )).ToArray();
        }
        else
        {
            return null;
        }


    }



        protected async Task Download(string fileName)
        {

            var base64 =  fileUpload.DownloadAsync(fileName);
            await JSRuntime.InvokeVoidAsync("DownloadPdf", fileName, base64);

        }


        public string pathS;
    [Inject] Application.Services.IAssetsServices<CallContext> _assetsServices { get; set; }
    [CascadingParameter] public IModalService Modal { get; set; }
    public int _CustomerId { get; set; }
    public string _CustomerValue { get; set; }
        
        

            string _message;
    int _equipmentTemplate;
    bool isVisible = false;
        public IFileListEntry file;
       
        public List<CertificatePoE> LIST1 { get; set; } = new List<CertificatePoE>();

    [Inject] public AppState AppState { get; set; }

        [Inject] public IConfiguration Configuration { get; set; }
        [Parameter]
    public Component Component { get; set; }

    [CascadingParameter(Name = "CascadeParam1")]
    public PieceOfEquipment PieceOfEquipment { get; set; }

        public string url { get; set; }
        public bool subscription { get; set; }
        public ResponsiveTable<CertificatePoE> RT { get; set; } = new ResponsiveTable<CertificatePoE>();

    public void NewItem()
    {

    }

    public void Show(List<CertificatePoE> group)
    {

        LIST1 = group;
     
        
        StateHasChanged();
     
    }


    protected override async Task OnInitializedAsync()
    {

            url = Configuration.GetSection("Reports")["AzurePath"];

            subscription = Convert.ToBoolean(Configuration.GetSection("Reports")["EnableSubscription"]);
            if(PieceOfEquipment.CertificatePoEs !=null && PieceOfEquipment.CertificatePoEs.Count() > 0 )
                    { 
            LIST1 = PieceOfEquipment.CertificatePoEs.ToList();
            }
        }

    protected override async Task OnParametersSetAsync()
    {
        try
        {
            var result = _reportService.GetUrlServer();
            pathS = result;
        }
        catch (Exception ex)
        {

        }
    }

    public async Task ShowModalCustomer()
    {
        Dictionary<string, object> EmptyValidationDictionary = new Dictionary<string, object>();

        var parameters = new ModalParameters();
        parameters.Add("SelectOnly", true);
        parameters.Add("IsModal", true);

        var messageForm = Modal.Show<CalibrationSaaS.Infraestructure.Blazor.Pages.Customer.Customer_Search>("Select Customer", parameters);
        var result = await messageForm.Result;

        if (!result.Cancelled)
            

            EmptyValidationDictionary = result.Data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                  .ToDictionary(prop => prop.Name, prop => prop.GetValue(result.Data, null));

        _CustomerId = Convert.ToInt32(EmptyValidationDictionary["CustomerID"].ToString());
        _CustomerValue = EmptyValidationDictionary["Name"].ToString();


    }

       


        public List<Domain.Aggregates.Entities.FileInfo> fileinfo = new List<Domain.Aggregates.Entities.FileInfo>();
    string status = "";
    const int MaxFileSize = 5 * 1024 * 1024;
        async Task HandleFileSelected(IFileListEntry[] files)
        {


            var file = files.FirstOrDefault();
            if (file == null)
            {
                return;
            }
            fileName = file.Name;
            if (file.Size > MaxFileSize)
            {
                status = $"That's too big. Max size: {MaxFileSize} bytes.";
            }
            else
            {


                if (file != null)
                {
                    Domain.Aggregates.Entities.FileInfo f = new Domain.Aggregates.Entities.FileInfo();
                 
                    f.Name = file.Name;
                    f.Size = file.Size;
                    f.RelativePath = file.RelativePath;
                    f.LastModified = file.LastModified;
                    f.Type = file.Type;
                

                    MemoryStream ms = new MemoryStream();
                    await file.Data.CopyToAsync(ms);
                    f.Data = ms.ToArray();
                    fileinfo.Add(f);
                 
                    RT.currentEdit.Name = file.Name;
                    RT.currentEdit.Description = file.Name;
                }

            }

        }
        public CertificatePoE Newcertificate()
        {
            fileName = "";
            CertificatePoE c = new CertificatePoE();

            c.DueDate = DateTime.Now;

            c.CalibrationDate = DateTime.Now;
            if (PieceOfEquipment != null)
            {
                c.PieceOfEquipmentID = PieceOfEquipment.PieceOfEquipmentID;
            }
            else
            {
                c.PieceOfEquipmentID = "temp";
            }

            return c;

        }

        protected async Task Submitted(EventArgs args)
        {

            var x = args.GetFields("CertificateNumber");
            var y = RT.Items;
        }

        public async Task<bool> Delete(CertificatePoE dto)
        {
            RT.Items.Remove(dto);
            LIST1.Remove(dto);
            StateHasChanged();

            return true;


        }


    }
}
