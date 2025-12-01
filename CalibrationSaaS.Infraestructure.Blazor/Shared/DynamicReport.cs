using CalibrationSaaS.Application.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using Grpc.Core;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Helpers.Controls.ValueObjects;
using Helpers.Controls;
using Microsoft.FluentUI.AspNetCore.Components;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Order;
using CalibrationSaaS.Infraestructure.Blazor.Shared;

using Reports.Domain.ReportViewModels;
using IdentityModel.Client;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.Extensions.Configuration;
using System.Web;
using Helpers;
using Microsoft.JSInterop;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;

namespace CalibrationSaaS.Infraestructure.Blazor
{
    public class DynamicReportBase
        : Base_Create<WorkOrderDetail, Func<dynamic, Application.Services.IWorkOrderDetailServices<CallContext>>, Domain.Aggregates.Shared.Basic.AppState>
    //, IPage<WorkOrderDetail, IWorkOrderDetailServices, Domain.Aggregates.Shared.AppStateCompany>
    {


        public int MyProperty { get; set; }
        public ICollection<GenericCalibrationResult2> RowsFilter { get; set; }

        public int DefaultDecimalNumber { get; set; }


        public dynamic _context;

        public Tolerance tolerance;

        [Parameter]
        public CalibrationType cmcValues { get; set; }

        public Blazed.Controls.MultiComponent.MultipleComponent MultipleComponent { get; set; }

        [Parameter]
        public IEnumerable<Bogus.DynamicProperty> DynamicPropertiesSchema { get; set; }


        [Parameter]
        public CalibrationSubType cst { get; set; }

        [Parameter]
        public bool? UseResult { get; set; }

        [Parameter]
        public bool IsRowView { get; set; }

        [Parameter]
        public string HideElement { get; set; } = "modalLinearity";

        public int Total { get; set; }

        public int Late { get; set; }

        public int DueToday { get; set; }

        public int NextDay { get; set; }

        public int TwoDay { get; set; }

        public int ThreeDay { get; set; }

        public int FourDay { get; set; }

        public string WoDId { get; set; }

        public bool refresh = false;

        public string SelectValue { get; set; } = "";

        //public List<CalibrationItemResult> tableListResult = null;

        public async Task TaskAfterSave(WorkOrderDetail item, string sav)
        {
           

        }
        public string SortField { get; set; }
        public WorkOrderDetail RowAfterRender(WorkOrderDetail lin)
        {

            //var srt = Grid.Items.Where(x => x.SequenceID == lin.SequenceID).ToList();
            
            //if (srt.Count > 1)
            //{


            //    lin.SequenceID = RT.Items.MaxBy(x => x.SequenceID).SequenceID + 1;
            //    lin.SequenceID = lin.SequenceID;
            //    int cont1 = 0;
            //    foreach (var itemn in RT.Items)
            //    {

            //        itemn.Position = cont1;
            //        cont1++;
            //    }


            //}


            

            //IniCalculated = 1;


            return lin;

        }
        public WorkOrderDetail RowChange(WorkOrderDetail lin)
        {

            return null;
        }

            public void RefreshGrid(string valor, int position)
        {


            //foreach (var item in tableListResult)
            //{
            //    item.KeyObject = null;
            //}

            //SelectRow = tableListResult.Where(x => x.Position == position).FirstOrDefault();

            //SelectValue = valor;
            //SelectPosition = position;

            //SelectRow.KeyObject = valor;




            StateHasChanged();



        }
        public async Task ChangeMultipleControl(ChangeEventArgs arg, dynamic lin, bool AddWeight = false)
        {



            //aTimer.Stop();
            //// new timer
            //aTimer.Start();
            //IniCalculated = 1;


        }
        public ResponsiveTable<WorkOrderDetail> Grid { get; set; }


        [Parameter]
        public ICollection<WorkOrderDetail> List { get; set; } = new List<WorkOrderDetail>();
        [Inject] public IConfiguration Configuration { get; set; }

        public string ReportUrl { get; set; }

        public string ShortUrl { get; set; }

        public string CreateUrlReport()
        {

            string query = "";
            if (report?.Parameters?.Count > 0)
            {


                foreach (var item in report?.Parameters)
                {
                    if (!string.IsNullOrEmpty(item.ColumnName))
                    {
                        query = query + "Column=" + item.ColumnName;
                    }

                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        query = query + "&Value=" + item.Value;
                    }

                    if (!string.IsNullOrEmpty(item.Operator) & item.Operator != "=")
                    {
                        query = query + "&Operator=" + item.Operator;
                    }
                    if (!string.IsNullOrEmpty(item.Name))
                    {
                        query = query + " &Name=" + item.Name;
                    }
                }
            }

            if (report?.Header.Parameters?.Count > 0)
            {


                foreach (var item in report?.Header?.Parameters)
                {
                    if (!string.IsNullOrEmpty(item.ColumnName))
                    {
                        query = query + "hColumn=" + item.ColumnName;
                    }

                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        query = query + "&hValue=" + item.Value;
                    }

                    if (!string.IsNullOrEmpty(item.Operator) & item.Operator != "=")
                    {
                        query = query + "&hOperator=" + item.Operator;
                    }
                    if (!string.IsNullOrEmpty(item.Name))
                    {
                        query = query + " &hName=" + item.Name;
                    }
                }
            }

            CurrentServer = NavigationManager.BaseUri.ToString();

            var url = Configuration.GetSection("Reports")["Url"];

            query = url + "/api/print/UrlPDF?url=" + CurrentServer + "dynamicReport/2?" + HttpUtility.UrlEncode(query);

            return query;
            //await JSRuntime.InvokeVoidAsync("eval", $"let _discard_ = open(`{url}`, `_blank`)");
        }

        public string CreateUrlReport2()
        {

            string query = "";
            if (report?.Parameters?.Count > 0)
            {


                foreach (var item in report?.Parameters)
                {
                   
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        query = query + "&Value=" + item.Value;
                    }

                   
                }
            }

            if (report?.Header.Parameters?.Count > 0)
            {


                foreach (var item in report?.Header?.Parameters)
                {
                   

                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        query = query + "&hValue=" + item.Value;
                    }

                  
                }
            }

            CurrentServer = NavigationManager.BaseUri.ToString();

            var url = Configuration.GetSection("Reports")["Url"];

            query = url + "/api/print/UrlPDF?url=" + CurrentServer + "dynamicReport/2?" + HttpUtility.UrlEncode(query);

            return query;
            //await JSRuntime.InvokeVoidAsync("eval", $"let _discard_ = open(`{url}`, `_blank`)");
        }
        public string CurrentServer { get; set; }
        public Search<WorkOrderDetail, IWorkOrderDetailServices<CallContext>, AppStateCompany> searchComponent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool First { get; set; }

        public bool Complete { get; set; }
        public override async Task<ResultSet<WorkOrderDetail>> LoadData(Pagination<WorkOrderDetail> pag)
        {


            WorkOrderDetailGrpc assets = new WorkOrderDetailGrpc(Client, DbFactory);
            //pag.JSonDefinitionResult = "xxccvvccfdcccc";
            var Eq = (await assets.GetWodsFromQuery(pag, new CallContext()));

            if (!First)
            {
                Total = Eq.Count;
                First = true;
            }
            LoadingWait = false;
            return Eq;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            if (firstRender)
            {

            }
            else if (!LoadingWait)
            {
                //Console.WriteLine("dynamic report-------------------------------");
            }

            await base.OnAfterRenderAsync(firstRender);
        }


        public async Task<bool> Delete(Domain.Aggregates.Entities.WorkOrderDetail DTO)
        {
            try
            {
                //await searchComponent.ShowModalAction();
                if (DTO.CurrentStatusID > 1)
                {
                    throw new Exception("Work Order Detail is being used, Only Work Order Detail in contract review status can be deleted ");
                }

                WorkOrderDetail w = new WorkOrderDetail();

                w.WorkOrderDetailID = DTO.WorkOrderDetailID;

                //var result = await Client.Delete(w);

                //WorkOrder_Create._listWorkOrderDetail.Remove(DTO);

                return true;
                // searchComponent.ShowResult();
            }
            catch (RpcException ex)
            {

                await ExceptionManager(ex);
                return false;

            }
            catch (Exception ex)
            {
                await ExceptionManager(ex);
                return false;

            }
        }

        [SupplyParameterFromQuery(Name = "Column")]
        private string[]? ColumnName { get; set; }

        [SupplyParameterFromQuery(Name = "Value")]
        private string[]? Value { get; set; }

        [SupplyParameterFromQuery(Name = "Operator")]
        private string[]? Operator { get; set; }

        [SupplyParameterFromQuery(Name = "Name")]
        private string[]? Name { get; set; }

        [SupplyParameterFromQuery(Name = "hColumn")]
        private string[]? hColumnName { get; set; }

        [SupplyParameterFromQuery(Name = "hValue")]
        private string[]? hValue { get; set; }

        [SupplyParameterFromQuery(Name = "hOperator")]
        private string[]? hOperator { get; set; }

        [SupplyParameterFromQuery(Name = "hName")]
        private string[]? hName { get; set; }

        public Task SelectModal(WorkOrderDetail DTO)
        {
            throw new NotImplementedException();
        }

        [Parameter]
        public List<Helpers.Controls.ValueObjects.ReportView.ParameterClass> Parameters { get; set; }

        [Parameter]
        public List<Helpers.Controls.ValueObjects.ReportView.ParameterClass> headerParameters { get; set; }


        [Parameter]
        public List<string> parameterValues { get; set; }

        [Parameter]
        public List<string> headerValues { get; set; }


        public ReportView report { get; set; }

        public bool PaginationView { get; set; } = true;

        public int PageSize { get; set; } = 20;


        protected override async Task OnInitializedAsync()
        {
            LoadingWait = true;

            if (EntityID == "2")
            {
                PaginationView = false;
                PageSize = 1000;
            }


            eq = new WorkOrderDetail();

            eq.PieceOfEquipment = new PieceOfEquipment();

            eq.PieceOfEquipment.EquipmentTemplate = new EquipmentTemplate();

            eq.CurrentStatus = new Domain.Aggregates.Entities.Status();

            eq.PieceOfEquipment.Customer = new Domain.Aggregates.Entities.Customer();

            eq.TestCode = new TestCode();

            eq.WorkOder = new Domain.Aggregates.Entities.WorkOrder();

            WorkOrderDetailGrpc client = new WorkOrderDetailGrpc(Client, DbFactory);
            
            //var res = await client.GetWOCountPerDay(new CallOptions());
            
            //if (res != null && res.Count() > 0)
            //{
            //    var tol = res.FirstOrDefault();

            //    int sdi = 0;
            //    //Total = tol.Value;
            //    //sumtemp = 0;
            //    //var late= res.Where(x => x.Key.Date < DateTime.Now.Date && tol.Key.Date != x.Key.Date).Sum(suma);

            //    var late = res.Where(x => x.Key.Date < DateTime.Now.Date.AddDays(sdi)).Sum(suma);

            //    //if(late != null)
            //    //{
            //    Late = late;
            //    //}
            //    sumtemp = 0;
            //    var today = res.Where(x => x.Key.Date == DateTime.Now.Date.AddDays(sdi)).Sum(suma);

            ////if (today != null)
            ////    {
            //        DueToday = today;
            //    // }
            //    sumtemp = 0;
            //    var next = res.Where(x => x.Key.Date == DateTime.Now.AddDays(1+ sdi).Date).Sum(suma);

            //    //if (next != null)
            //    //{
            //        NextDay = next;
            //    //}
            //    sumtemp = 0;
            //    var two = res.Where(x => x.Key.Date == DateTime.Now.AddDays(2 + sdi).Date).Sum(suma);

            //    //if (two != null)
            //    //{
            //        TwoDay = two;
            //    //}
            //    sumtemp = 0;
            //    var three = res.Where(x => x.Key.Date == DateTime.Now.AddDays(3 + sdi).Date).Sum(suma);

            //    //if (three != null)
            //    //{
            //    ThreeDay = three;
            //    //}
            //    sumtemp = 0;
            //    var four = res.Where(x => x.Key.Date == DateTime.Now.AddDays(4+ sdi).Date).Sum(suma);

            //    //if (four != null)
            //    //{
            //    FourDay = four;
            //    //}


            //}


            //eq.PieceOfEquipment

            report= new ReportView();
            

            report.Title = "LTI Calibration";

            report.ClassName = "workorderdetail";

            //report.Query = $"SELECT wod.*,poe.* FROM WorkOrderDetail wod inner join PieceOfEquipment poe on wod.PieceOfEquipmentId = poe.PieceOfEquipmentID ";//$"SELECT * FROM WorkOrderDetail ";

            report.Query = $"select distinct wod.[WorkOrderDetailID],wod.[WorkOderID],wod.[TenantId],wod.[PieceOfEquipmentId],wod.[IsAccredited],wod.[SelectedNewStatus],wod.[CertificateComment],wod.[Humidity],wod.[Temperature],wod.[Description],wod.[TemperatureUOMID],wod.[CalibrationIntervalID],wod.[CalibrationDate],wod.[CalibrationCustomDueDate],wod.[CalibrationNextDueDate],wod.[TechnicianComment],wod.[TestPointNumber],wod.[HumidityUOMID],wod.[TechnicianID],wod.[CurrentStatusID],wod.[ToleranceTypeID],wod.[AccuracyPercentage],wod.[DecimalNumber],wod.[Resolution],wod.[Environment],wod.[WorkOrderDetailHash],wod.[AddressID],wod.[CalibrationTypeID],wod.[Name],wod.[IsComercial],wod.[Multiplier],wod.[ClassHB44],wod.[OfflineID],wod.[OfflineStatus],wod.[StatusDate],wod.[HasBeenCompleted],wod.[TemperatureAfter],wod.[IncludeASTM],wod.[IsUniversal],wod.[CertificationID],wod.[TemperatureStandardId],wod.[TestCodeID],wod.[EndOfMonth],wod.[WorkOrderDetailUserID],wod.[CalibrationSubTypeID],wod.[Configuration],wod.[FullScale],wod.[TolerancePercentage],wod.[ToleranceValue],wod.[ToleranceFixedValue],wod.[JsonTolerance] from [WorkOrderDetail] wod inner join [CalibrationSubType_Weight] cw on cw.WorkOrderDetailID=wod.WorkOrderDetailID inner join WeightSet w on cw.WeightSetID=w.WeightSetID inner join PieceOfEquipment poe on poe.PieceOfEquipmentID=wod.PieceOfEquipmentId";

            report.Columns = new List<ReportView.Column>();
            var c1 = new ReportView.Column();

            c1.Field = "WorkOrderDetailID";
            c1.Title = "ID";

            report.Columns.Add(c1);

            var c2 = new ReportView.Column();

            c2.Title = "Work Order";

            c2.Field = "WorkOrderID";
            
            report.Columns.Add(c2);

            var c3 = new ReportView.Column();

            c3.Title = "POE";

            c3.Field = "PieceOfEquipmentId";

            report.Columns.Add(c3);

            var c4 = new ReportView.Column();

            c4.Title = "Serial";

            c4.Field = "PieceOfEquipment.SerialNumber";

            report.Columns.Add(c4);

            var c5 = new ReportView.Column();

            c5.Title = "Model";

            c5.Field = "PieceOfEquipment.EquipmentTemplate.Model";

            report.Columns.Add(c5);


            var c6 = new ReportView.Column();

            c6.Title = "ET #";

            c6.Field = "eq.PieceOfEquipment.EquipmentTemplate.EquipmentTemplateID";

            report.Columns.Add(c6);

            report.Header = new ReportView.HeaderClass();

            

            report.Header.PropertyName = "PieceOfEquipment";

            report.Header.ClassName = "CalibrationSaaS.Domain.Aggregates.Entities.PieceOfEquipment";

            report.Header.Query = $" select * from PieceOfEquipment ";

            report.Header.Includes = new List<string>();
            
            //eq.PieceOfEquipment.EquipmentTemplate.Manufacturer1.Name
            report.Header.Includes.Add("EquipmentTemplate");

            report.Header.Includes.Add("EquipmentTemplate.Manufacturer1");

            report.Header.Columns = new List<ReportView.Column>();

            var h1 = new ReportView.Column();

            h1.Title = "POE ID";

            h1.Field = "PieceOfEquipmentID";

            h1.Position = 0;

            report.Header.Columns.Add(h1);

            var h2 = new ReportView.Column();

            h2.Title = "SERIAL";

            h2.Field = "SerialNumber";

            h2.Position = 1;

            report.Header.Columns.Add(h2);

            //eq.PieceOfEquipment.EquipmentTemplate.Manufacturer1.Name
            var h3 = new ReportView.Column();

            h3.Title = "Model";

            h3.Field = "EquipmentTemplate.Model";

            h3.Position = 1;

            report.Header.Columns.Add(h3);

            var h4 = new ReportView.Column();

            h4.Title = "Manufacturer";

            h4.Field = "EquipmentTemplate.Manufacturer1.Name";

            h4.Position = 1;

            report.Header.Columns.Add(h4);


            //report.Parameters = new List<Helpers.Controls.ValueObjects.ReportView.ParameterClass>();

            //PieceOfEquipmentID
            var par1 = new Helpers.Controls.ValueObjects.ReportView.ParameterClass();

            par1.Name = "POE_No";            
            par1.Operator = "=";
            par1.ColumnName = "w.PieceOfEquipmentId";

            List<Helpers.Controls.ValueObjects.ReportView.ParameterClass> lstPar = new List<ReportView.ParameterClass>();
            lstPar.Add(par1);


            //Parameters = lstPar;


            var par2 = new Helpers.Controls.ValueObjects.ReportView.ParameterClass();

            par2.Name = "POE_No";
           
            par2.Operator = "=";
            par2.ColumnName = "PieceOfEquipmentID";

            List<Helpers.Controls.ValueObjects.ReportView.ParameterClass> lstPar2 = new List<ReportView.ParameterClass>();
            lstPar2.Add(par2);

            //headerParameters = lstPar2;


            report.BeginDate = DateTime.Now;
            report.EndDate = DateTime.Now;

            //report.Parameters.Add(par1);
            //report.Parameters.Add(par2);

            report.Includes = new List<string>();

            report.Includes.Add("PieceOfEquipment");
            report.Includes.Add("PieceOfEquipment.EquipmentTemplate");

            //EquipmentTemplate
            report.Parameters = Parameters;
            report.Header.Parameters = headerParameters;

            var json1 = Newtonsoft.Json.JsonConvert.SerializeObject(report);

            if (Value?.Count() > 0 & lstPar?.Count > 0)
            {
                int conta = 0;
                foreach (var item in lstPar)
                {
                    item.Value = Value.ElementAtOrDefault(conta);
                    conta++;
                }
                Parameters = lstPar;
            }

            if (hValue?.Count() > 0 & lstPar2?.Count > 0)
            {
                int conta = 0;
                foreach (var item in lstPar2)
                {
                    item.Value = hValue.ElementAtOrDefault(conta);
                    conta++;
                }
                headerParameters = lstPar2;
            }

            if (ColumnName?.Count() > 0)
            {
                Parameters = new List<ReportView.ParameterClass>();
                int conta = 0;
                foreach (var co in ColumnName)
                {
                    var parq = new Helpers.Controls.ValueObjects.ReportView.ParameterClass();

                    //par1.Name = "WOD #";
                    parq.Value = Value.ElementAtOrDefault(conta);
                    parq.Operator = Operator.ElementAtOrDefault(conta);
                    parq.ColumnName = co;
                    Parameters.Add(parq);
                    conta++;
                }
            }

            if (hColumnName?.Count() > 0)
            {
                headerParameters = new List<ReportView.ParameterClass>();
                int conta = 0;
                foreach (var co in hColumnName)
                {
                    var parq = new Helpers.Controls.ValueObjects.ReportView.ParameterClass();

                    //par1.Name = "WOD #";
                    parq.Value = hValue.ElementAtOrDefault(conta);
                    parq.Operator = hOperator.ElementAtOrDefault(conta);
                    parq.ColumnName = co;
                    headerParameters.Add(parq);
                    conta++;
                }
            }


            report.Parameters = Parameters;
            report.Header.Parameters = headerParameters;

            await base.OnInitializedAsync();

            Component.Group = Component.Group.Replace("tech.HasView","");

            Component.Group = Component.Group + "tech,job.HasView,job.HasEdit,job.HasSave";


            this.ReportUrl = CreateUrlReport();

            this.ShortUrl = CreateUrlReport2();

        }


        public async Task ShowModalReport()
        {
            await JSRuntime.InvokeVoidAsync("eval", $"let _discard_ = open(`{ShortUrl}`, `_blank`)");

        }

            int sumtemp = 0;
        int suma(KeyValueDate key)
        {
            sumtemp = sumtemp + key.Value;

            return sumtemp;
        }

        
        public IEnumerable<WorkOrderDetail> FilterList(string filter = "")
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

    }
    

   
}
