using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;

using Blazored.Modal.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using BlazorInputFile;
using Blazor.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Blazored.Modal;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using Helpers.Controls;
using System.Reflection;
using Grpc.Core;
using Component = Helpers.Controls.Component;

namespace BlazorApp1.Blazor.Pages.AssetsBasics
{
    public partial class Certificate_Create : ComponentBase
    {
          public string pathS;
    [Inject] CalibrationSaaS.Application.Services.IAssetsServices<CallContext> _assetsServices { get; set; }
    [CascadingParameter] public IModalService Modal { get; set; }
    public int _CustomerId { get; set; }
    public string _CustomerValue { get; set; }


    string _message;
    int _equipmentTemplate;
    bool isVisible = false;
    IFileListEntry file;

    public List<Certificate> LIST1 { get; set; }

    [Inject] public AppState AppState { get; set; }

    [Parameter]
    public Component Component { get; set; }
        const int MaxFileSize = 5 * 1024 * 1024; // 5MB
        const string DefaultStatus = "Drop a csv file here to view it, or click to choose a CSV file";
        public string status = DefaultStatus;
        public ResponsiveTable<Certificate> RT { get; set; } = new ResponsiveTable<Certificate>();

        public string fileName = "";
        public string fileTextContents;

        async Task HandleFileSelected(IFileListEntry[] files)
        {
            file = files.FirstOrDefault();
            
        }



        public void NewItem()
    {

    }


    public void Show(List<Certificate> group)
    {

            LIST1 = group;
            //RT.ItemsDataSource = LIST1;
            Logger.LogDebug("Certificate on Show Wc" + LIST1.Count());
            StateHasChanged();
            //Logger.LogDebug(RT.ItemsDataSource.Count());
        }

        [Inject]
    IConfiguration Configuration { get; set; }
    string Url { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Url = Configuration["Kestrel:Endpoints:Http2:Url"];


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


    bool Enabled = true;
    public ResponsiveTable<Certificate> Grid { get; set; }
    [Inject] public CalibrationSaaS.Application.Services.IReportService<CallContext> _reportService { get; set; }

    public IEnumerable<Certificate> FilterList(string filter = "")
    {

            if (Grid != null && Grid.ItemsDataSource != null)
            {
                var templist = Grid.ItemsDataSource;

                //return null;

                return templist.Where(i => i.Name.ToLower().Contains(filter.ToLower()

                    )).ToArray();
            }
            else
            {
                return null;
            }



        }

        public Certificate Newcertificate()
    {
            Certificate c = new Certificate();

            c.DueDate = DateTime.Now;

            c.CalibrationDate = DateTime.Now;

            return c;

        }

    public async Task Download()
    {

    }

}

    }

