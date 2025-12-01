 using System;
using Microsoft.AspNetCore.Mvc;
using CalibrationSaaS.Reports.Infraestructure.APIRep.Extensions;
using CalibrationSaaS.Reports.Infraestructure.APIRep.Services.Meta;
using CalibrationSaaS.Reports.Infraestructure.APIRep.ViewModels;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Application.UseCases;
using Reports.Domain.ReportViewModels;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Text;
using System.Web;
using System.Reflection;
using System.Collections.Generic;
using Esprima;
using static QRCoder.PayloadGenerator;

//using Helpers;

namespace CalibrationSaaS.Reports.Infraestructure.APIRep.Controllers
{
    [Route("/api/print")]
    public class GeneralController : Controller
    {
        private readonly ITemplateService _templateService;
        private readonly IWorkOrderDetailRepository _workOrderDetailRepository;
        private readonly IPieceOfEquipmentRepository _pieceOfEquipmentRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBasicsRepository _basicsRepository;
        private readonly IConfiguration Configuration;
        private readonly IAssetsRepository Assets;

        private readonly BasicsUseCases Basics;
        private readonly WorkOrderDetailUseCase WOD;
        private readonly PieceOfEquipmentUseCases Poe;


        
        public GeneralController(ITemplateService templateService, IWorkOrderDetailRepository workOrderDetailRepository,
            IBasicsRepository basicsRepository,
            IConfiguration _Configuration,
            WorkOrderDetailUseCase _WOD,
            IPieceOfEquipmentRepository pieceOfEquipmentRepository,
            IAssetsRepository _Assets,
            PieceOfEquipmentUseCases _Poe
            )
           
        {

            _templateService = templateService ?? throw new ArgumentNullException(nameof(templateService));
            _workOrderDetailRepository = workOrderDetailRepository;
            _basicsRepository = basicsRepository;
            Configuration=_Configuration;
            _pieceOfEquipmentRepository = pieceOfEquipmentRepository;
            WOD = _WOD;
            Assets = _Assets;
             Poe = _Poe;
        }

         [HttpGet("LTIReportJson")]
        public async Task<string> LTIReport(int id)


        {
            string customer = Configuration.GetSection("Reports")["Customer"];


            GenericReport model = new GenericReport();
            var wo = new WorkOrderDetail();
            wo.WorkOrderDetailID = id;
            var genericReport = await WOD.GetJsonGeneric(wo, customer, false);
            Type type = typeof(GenericReport);
            genericReport.Statementstring = "";
            genericReport.Gridstring = "";
            PropertyInfo statementStringProperty = type.GetProperty("Statementstring");
            PropertyInfo gridStringProperty = type.GetProperty("Gridstring");
            List<PropertyInfo> propertiesToRemove = new List<PropertyInfo> { statementStringProperty, gridStringProperty };
            foreach (var property in propertiesToRemove)
            {
                type.GetRuntimeProperties().ToList().Remove(property);
            }

     
            

            JObject jsonObj = JObject.FromObject(genericReport);
            string jsonResult = jsonObj.ToString();

            return jsonResult;


        }

        [HttpGet("LTIReportJsonOld")]
        public async Task<string> LTIReportOld(int id)
        {
            var customer = "NA";
            GenericReport model = new GenericReport();
            var wo = new WorkOrderDetail();
            wo.WorkOrderDetailID = id;
            var genericReport = await WOD.GetJsonGeneric(wo, customer, false);
            Type type = typeof(GenericReport);
            genericReport.Statementstring = "";
            genericReport.Gridstring = "";
            PropertyInfo statementStringProperty = type.GetProperty("Statementstring");
            PropertyInfo gridStringProperty = type.GetProperty("Gridstring");
            List<PropertyInfo> propertiesToRemove = new List<PropertyInfo> { statementStringProperty, gridStringProperty };
            foreach (var property in propertiesToRemove)      
            {
                type.GetRuntimeProperties().ToList().Remove(property);
            }




            JObject jsonObj = JObject.FromObject(genericReport);
            string jsonResult = jsonObj.ToString();

            return jsonResult;


        }

        [HttpPost("createHeader")]
        public async Task<IActionResult> Create([FromBody] Domain.Aggregates.Entities.WorkOrder wod)
        {



            return Ok(wod);


        }


        [HttpPost("createCalibrationInstrucctions")]
        public async Task<IActionResult> CalibrationInstrucctions([FromBody] Domain.Aggregates.Entities.WorkOrderDetail wod)
        {



            return Ok(wod);


        }



        [HttpGet("LTIReportGeneral")]
        public async Task<IActionResult> GenericReport(int id)
        {
            try
            {
                string customer = Configuration.GetSection("Reports")["Customer"];

                GenericReport model = new GenericReport();
                // Demo mode is enabled when Customer is "Demo"
                ViewBag.isdemo = (customer == "Demo") ? "true" : "false";
                var wo = new WorkOrderDetail();
                wo.WorkOrderDetailID = id;
                var report = await WOD.GetJsonGeneric(wo, customer, false);
                JObject jsonObj = JObject.FromObject(report.Header);
                // JObject jsonGrid = JObject.FromObject(report.DataGrids);

                string jsonResult = jsonObj.ToString();
                var headerJson = JsonConvert.DeserializeObject<GenericHeader>(jsonResult);
                string grids = "";
                string gridsAsFound = "";
                var statemetsNoGrid = report?.Statements?.Where(x => x.DataGrid == "");
                List<GenericStatementNoGrid> stNogridLis = new List<GenericStatementNoGrid>();
                if (statemetsNoGrid != null && statemetsNoGrid.Count() > 0)
                {
                    foreach (var c in statemetsNoGrid)
                    {
                        GenericStatementNoGrid stNoGrid = new GenericStatementNoGrid()
                        {
                            Statement = c.Statement

                        };

                        stNogridLis.Add(stNoGrid);
                    }
                }

                var statements = JsonToHtmlTable(stNogridLis);


                //JObject dictionaryGrids = JObject.Parse(report.DataGrids.ToString());
                //
                KeyValuePair<string, List<JToken>> dictionaryGrids = new KeyValuePair<string, List<JToken>>();

                List<KeyValuePair<string, JToken>> gridsnotes = new List<KeyValuePair<string, JToken>>();
                List<KeyValuePair<string, JToken>> grnotes = new List<KeyValuePair<string, JToken>>();
                string titledJson = "";
                string Title = "Statements";

                List<Object> gridsnotesObject = new List<Object>();
                List<KeyValuePair<string, Dictionary<string, JToken>>> list = new List<KeyValuePair<string, Dictionary<string, JToken>>>();
                Dictionary<string, Object> newObj2 = new Dictionary<string, Object>();
                if (report.DataGrids != null)
                {
                    foreach (var grid in report.DataGrids)
                    {

                        gridsnotes.Add(grid);
                        //gridsnotesObject.Add(grid);
                        var notes = report.Statements?.Where(x => x.DataGrid == grid.Key);
                        Dictionary<string, JToken> newObj1 = new Dictionary<string, JToken>();

                        //newObj2.Add("grids", JObject.FromObject(grid));


                        if (notes != null && notes.Count() > 0)
                        {

                            KeyValuePair<string, JToken> notesg = new KeyValuePair<string, JToken>();
                            List<JToken> values = new List<JToken>();
                            Dictionary<string, List<JToken>> keyValuePairs = new Dictionary<string, List<JToken>>();

                            //string key = "Statements";
                            int cont = 0;
                            foreach (var note in notes)
                            {
                                values.Add(new JValue(note.Statement.ToString()));
                                cont = cont + 1;
                                var key = "Statement " + cont;
                                var value = note.Statement.ToString();

                                newObj1[key] = value;

                            }
                            //keyValuePairs.Add(key, values);
                            ////////new
                            //JObject groupedObject = new JObject();
                            //foreach (KeyValuePair<string, List<JToken>> pair in keyValuePairs)
                            //{
                            //    JArray jArray = new JArray(pair.Value);
                            //    groupedObject[pair.Key] = jArray;
                            //}
                            //string jsonActualizado = groupedObject.ToString();

                            /////end new


                            JObject jsonObject2 = new JObject();

                            foreach (KeyValuePair<string, JToken> pair in newObj1)
                            {
                                jsonObject2[pair.Key] = pair.Value;
                            }


                            var title = "Statements for " + grid.Key;



                            titledJson = $"{{\"{title}\":[{jsonObject2}]}}"; // $"{{\"{title}\":[{jsonActualizado}]}}";
                            JObject jsonObjectmod = JObject.Parse(titledJson);

                            // Obtener el par clave-valor del JObject
                            jsonObjectmod["Key"] = new JValue("Value");

                            KeyValuePair<string, JToken> keyValuePair = jsonObjectmod
                                .Properties()
                                .Select(prop => new KeyValuePair<string, JToken>(prop.Name, prop.Value))
                                .FirstOrDefault();

                            gridsnotes.Add(keyValuePair);

                        }
                    }

                    string? gridJson = "";
                    string? gridJsonAsFound = "";

                    JObject jsonObject = new JObject();

                    foreach (var kvp in gridsnotes)
                    {
                        jsonObject.Add(kvp.Key, kvp.Value);
                    }


                    report.DataGrids = jsonObject;

                    // Filter jsonObject to exclude fields with "left" (case insensitive)
                    JObject filteredJsonObject = new JObject();
                    foreach (var kvp in jsonObject)
                    {
                        if (kvp.Value is JArray array)
                        {
                            
                            JArray filteredArray = new JArray();
                            foreach (JObject item in array)
                            {
                                JObject filteredItem = new JObject();
                                foreach (var prop in item)
                                {
                                    if (!prop.Key.ToLower().Contains("left"))
                                    {
                                        filteredItem.Add(prop.Key, prop.Value);
                                    }
                                }
                                filteredArray.Add(filteredItem);
                            }
                            filteredJsonObject.Add(kvp.Key, filteredArray);
                        }
                        else
                        {
                            
                            if (!kvp.Key.ToLower().Contains("left"))
                            {
                                filteredJsonObject.Add(kvp.Key, kvp.Value);
                            }
                        }
                    }
                    report.DataGridsAsFound = filteredJsonObject;

                    gridJson = jsonObject.ToString(); //report.DataGrids.ToString();
                    gridJsonAsFound = filteredJsonObject.ToString();

                    grids = JsonToHtmlTableJson<dynamic>(gridJson);
                    gridsAsFound = JsonToHtmlTableJson<dynamic>(gridJsonAsFound);
                    //string grids1 = JsonToHtmlTableJson<dynamic>(gridJson1);
                }
                model = report;
                model.Statementstring = statements;
                model.Gridstring = grids;
                model.GridstringAsFound = gridsAsFound;

                string? html = "";
                if (customer == "Maxpro")
                {
                    html = await _templateService.RenderAsync("Templates/GenericReportMaxPro", model);
                }
                else if(customer == "ThermoTemp")
                {
                    html = await _templateService.RenderAsync("Templates/GenericReportThermoTemp", model);
                }
                else
                { 
                html = await _templateService.RenderAsync("Templates/GenericReport", model);
                }
                //*****************************************************************************
                //using (var httpClient = new HttpClient())
                //{
                //    var response = await httpClient.GetAsync("http://localhost:7246/api/PuppeteerHttpFunction");
                //    var content = await response.Content.ReadAsStringAsync();

                //    Console.WriteLine(content);
                //}

                //********************************************************************************

                await using var browser = await Puppeteer.ConnectAsync(new ConnectOptions
                {
                    BrowserWSEndpoint = "wss://chrome.browserless.io?token=cae21211-c2d1-4325-9f79-70fecc54826c"
                });


                // await new BrowserFetcher().DownloadAsync();
                //var chromiumExecutablePath = Environment.GetEnvironmentVariable("ChromiumExecutablePath");

                //string functionTempDirectory = Path.GetTempPath();
                //string userDataDir = Path.Combine(functionTempDirectory, "TempPuppeter");

                //var launchOptions = new LaunchOptions
                //{
                //    ExecutablePath = chromiumExecutablePath,
                //    Headless = true,
                //    Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" },
                //    UserDataDir = userDataDir
                //};
                                
                //var browser = await Puppeteer.LaunchAsync(launchOptions);

                await using var page = await browser.NewPageAsync();
                await page.EmulateMediaTypeAsync(MediaType.Screen);
                await page.SetContentAsync(html);
                //var pdfContent = await page.PdfStreamAsync(new PdfOptions
                //{
                //    Format = PaperFormat.Letter,
                //    DisplayHeaderFooter = true,
                //    Landscape = true,
                //    PrintBackground = true

                //});
                var pdfContent = await page.PdfStreamAsync(new PdfOptions
                {
                    Format = PaperFormat.Letter,
                    DisplayHeaderFooter = true,
                    Landscape = false,
                    PrintBackground = true,
                    HeaderTemplate = "<div style='width: 100%; text-align: center; font-size: 10px;'></div>",
                    FooterTemplate = "<div style='width: 100%; text-align: center; font-size: 10px;'>Page <span class='pageNumber'></span> of <span class='totalPages'></span></div>",
                    MarginOptions = new MarginOptions
                    {
                        Top = "30px",
                        Bottom = "65px"
                    }
                });


                return File(pdfContent, "application/pdf");
            }
            catch (Exception ex)
            {

                return Ok(null);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public static string JsonToHtmlTableJson<T>(string json)
        {

            // Convert JSON string to JObject
            JObject jsonObj = JObject.Parse(json);

            // Create StringBuilder to build HTML table
            StringBuilder tableHtml = new StringBuilder();

            // Create table header with JSON properties

            foreach (var property in jsonObj.Properties())
            {
                //tableHtml.AppendFormat("<th>{0}</th>", HttpUtility.HtmlEncode(property.Name));
                int cont = 0;
                var keyName = property.Name;

                // Get list of objects for this table
                var tableData = jsonObj[keyName] as JArray;
                if (tableData == null || tableData.Count == 0)
                    continue;

                // Identify columns that have all values equal to 0
                var columnsToExclude = GetColumnsWithAllZeros(tableData);

                // If all columns are excluded, skip this table entirely
                if (tableData.Count > 0)
                {
                    var firstObject = tableData[0] as JObject;
                    if (firstObject != null && firstObject.Properties().Count() == columnsToExclude.Count)
                        continue;
                }

                tableHtml.Append("<table Style=\"page-break-inside:avoid\" border=\"1\"><tr class=\"heading\">");

                foreach (JObject obj in tableData)
                {


                    if (cont == 0)
                    {
                        // Count only columns that will not be excluded
                        int contCols = obj.Properties().Count() - columnsToExclude.Count;
                        tableHtml.AppendFormat("<tr class=\"heading\">");
                        tableHtml.AppendFormat("<td colspan={0}>{1}</td>", contCols, HttpUtility.HtmlEncode(property.Name.ToString()));
                        tableHtml.Append("</tr>");
                        tableHtml.AppendFormat("<tr class=\"heading\">");
                        foreach (JProperty prop in obj.Properties())
                        {
                            // Skip columns that should be excluded
                            if (columnsToExclude.Contains(prop.Name))
                                continue;

                            tableHtml.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(prop.Name.ToString()));

                            //tableHtml.AppendFormat("<td>{0}</td>", "\"" + HttpUtility.HtmlEncode(prop.Value.ToString()) + "\"");
                        }
                    }
                    tableHtml.Append("</tr>");
                    foreach (JProperty prop in obj.Properties())
                    {
                        // Skip columns that should be excluded
                        if (columnsToExclude.Contains(prop.Name))
                            continue;

                        tableHtml.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(prop.Value.ToString()));

                        //tableHtml.AppendFormat("<td>{0}</td>", "\"" + HttpUtility.HtmlEncode(prop.Value.ToString()) + "\"");
                    }
                    cont++;

                }
                tableHtml.Append("</tr>");
                tableHtml.Append("</table>");

            }




            // Close table

            return tableHtml.ToString();
        }

        /// <summary>
        /// Identifies columns that have all values equal to 0
        /// </summary>
        private static HashSet<string> GetColumnsWithAllZeros(JArray tableData)
        {
            var columnsWithAllZeros = new HashSet<string>();

            if (tableData.Count == 0)
                return columnsWithAllZeros;

            // Get column names from first object
            var firstObject = tableData[0] as JObject;
            if (firstObject == null)
                return columnsWithAllZeros;

            // For each column, verify if all values are 0
            foreach (var property in firstObject.Properties())
            {
                bool allZeros = true;
                foreach (JObject obj in tableData)
                {
                    var value = obj[property.Name]?.ToString();
                    if (value != null && value != "0")
                    {
                        allZeros = false;
                        break;
                    }
                }

                if (allZeros)
                {
                    columnsWithAllZeros.Add(property.Name);
                }
            }

            return columnsWithAllZeros;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public static string JsonToHtmlTable<T>(IEnumerable<T> items)
        {
            //var jsonString = JsonConvert.SerializeObject(obj);
            //var json = JObject.Parse(jsonString);

            var sb = new StringBuilder();
            sb.Append("<table>");

            var properties = typeof(T).GetProperties();
            sb.Append("<tr>");
            foreach (var property in properties)
            {
                sb.Append($"<th>{property.Name}</th>");
            }
            sb.Append("</tr>");

            foreach (var item in items)
            {
                sb.Append("<tr>");
                foreach (var property in properties)
                {
                    sb.Append($"<td style=\"text-align:justify; line-height:100%\">{"- " + property.GetValue(item)}</td>");
                }
                sb.Append("</tr>");
            }

            sb.Append("</table>");
            return sb.ToString();
            
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<NoteViewModel> GetNotes(PieceOfEquipment? _poe, WorkOrderDetail? wod)
        {
            //Notes Equipment Type
            List<Note> NotesEqTc = new List<Note>();
            List<Note> notesEqType = new List<Note>();

            var eqType = await _basicsRepository.GetEquipmentTypeXId(_poe.EquipmentTemplate.EquipmentTypeID);
            notesEqType = eqType.Notes.OrderBy(x => x.EquipmnetTypeId != null).ThenBy(z => z.Position).ToList();


            //Notes TestCode
            List<Note> notesTestCode = new List<Note>();
            var testCode = await _workOrderDetailRepository.GetTestCodeByID((int)wod.TestCodeID);
            if (testCode != null && testCode.Notes != null)
            {
                notesTestCode = testCode.Notes.OrderBy(y => y.TestCodeID != null).ToList();

            }
            NotesEqTc = notesEqType.Concat(notesTestCode).ToList();

            //Validate Notes Condition
            List<Note> notesCondition = new List<Note>();
            foreach (var note in NotesEqTc)
            {
                if (note.Condition == 1)
                    notesCondition.Add(note);
                else if (wod.IsAccredited == true && note.Condition == 4)
                    notesCondition.Add(note);
                else if (wod.IsAccredited == false && note.Condition == 5)
                    notesCondition.Add(note);

            }

            //Notes WOD
            List<NoteWOD> notesWOD = new List<NoteWOD>();
            var notesWOD1 = await _workOrderDetailRepository.GetNotes(wod.WorkOrderDetailID, 1);
            notesWOD = notesWOD1.OrderBy(x => x.Position).ToList();

            NoteViewModel noteViewModel = new NoteViewModel();
            noteViewModel.NotesList = notesCondition;
            noteViewModel.NotesWODList = notesWOD;

            return noteViewModel;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GeneratePDF<T>(T model,string Template,PaperFormat format)
        {
            string? html = "";

            html = await _templateService.RenderAsync("Print/Templates/" + Template, model);



            await using var browser = await Puppeteer.ConnectAsync(new ConnectOptions
            {
                BrowserWSEndpoint = "wss://chrome.browserless.io?token=9f60a03f-7ad1-4f8a-bd49-f6893d704ba7"
            });

            //var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            //{
            //    Headless = true,
            //    ExecutablePath = PuppeteerExtensions.ExecutablePath
            //});


            await using var page = await browser.NewPageAsync();
            
            await page.EmulateMediaTypeAsync(MediaType.Screen);
            
            await page.SetRequestInterceptionAsync(true);

            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic.Add("Accept-Language","en-US,en;q=0.9");

            await page.SetExtraHttpHeadersAsync(dic);
            
            await page.SetUserAgentAsync("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36");



            await page.SetContentAsync(html);
            var pdfContent = await page.PdfStreamAsync(new PdfOptions
            {
                Format = format,
                DisplayHeaderFooter = true,
                Landscape = false,
                PrintBackground = true

            });
            return File(pdfContent, "application/pdf");

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> SeePdfView(string url, string Name)
        {

            var pdfcontent = await SeePdf(url, Name);
        
            return File(pdfcontent, "application/pdf");
        
        
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<string> SeePdfStr(string url, string Name)
        {

            var pdfcontent = await SeePdf(url, Name);
        
            return ConvertToBase64(pdfcontent);
        
        
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<Stream> SeePdf(string url,string Name)
        {
            try
            {
                
                //  await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
                using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions()
                {
                    Headless = true, // You can set false Watch the web page show the results ( Note ： Not Headless You can't remit PDF)
                    ExecutablePath = PuppeteerExtensions.ExecutablePath
                }))
                {
                    using (var page = await browser.NewPageAsync())
                    {
                        //http://score.xxxx.com/search/print?clkid=209&xuehao=5706180009  Some fonts display abnormally 
                        //http://score.xxxxx.com/search/print?clkid=195&xuehao=1366180216  Image overflow 

                        List<WaitUntilNavigation> wai = new List<WaitUntilNavigation>();
                        wai.Add(WaitUntilNavigation.Networkidle0);
                        NavigationOptions OP = new NavigationOptions();
                        OP.WaitUntil = wai.ToArray();
                        OP.Timeout = 3000000;


                        await page.GoToAsync(url,OP);


                         await page.EmulateMediaTypeAsync(MediaType.Screen);
            
            await page.SetRequestInterceptionAsync(true);

            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic.Add("Accept-Language","en-US,en;q=0.9");

            await page.SetExtraHttpHeadersAsync(dic);
            
            await page.SetUserAgentAsync("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36");



                        // Through SetViewport Control the window size to determine the size of the snapshot 
                        //await page.SetViewportAsync(new ViewPortOptions
                        //{
                        //    Width = 960,
                        //    Height = 1000,
                        //    IsMobile = false,
                        //    IsLandscape = false,
                        //});

                        await page.WaitForNavigationAsync();//await page.WaitForTimeoutAsync(1500);//25min
                        //await page.EvaluateExpressionAsync("$(\"span\").css({\"font-family\":\"\"});");// Clear font format 
                        //await page.PdfAsync(Environment.CurrentDirectory + "" $"D:\\FreewayTraffic\\Snapshot.pdf", new PdfOptions()
                     var pdfcontent= await page.PdfStreamAsync( new PdfOptions()
                        {
                            PrintBackground = true,
                            //MarginOptions = new PuppeteerSharp.Media.MarginOptions()
                            //{
                            //    Left = "20px",
                            //    Right = "20px",
                            //    Bottom = "45px",
                            //    Top = "10px",
                            //},
                            Landscape=true,
                            //DisplayHeaderFooter = true,
                            Format = PuppeteerSharp.Media.PaperFormat.Legal,
                            //FooterTemplate = "<div style='font-size:12px;text-align:center;width:100%;border:0;color:#000;margin:0;padding:0;'><span style='vertical-align:bottom;color:#000;font-size:12px' class='pageNumber'></span>/<span style='vertical-align:bottom;' class='totalPages'></span></div>",
                        });
                        return pdfcontent;
                    }
                }

                

            }
            catch (Exception ex)
            {
//                Console.WriteLine(ex.ToString());
                throw;
            }
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<Stream> GeneratePDF2<T>(T model, string Template, PaperFormat format)
        {
            string? html = "";

            html = await _templateService.RenderAsync("Print/Templates/" + Template, model);



            //await using var browser = await Puppeteer.ConnectAsync(new ConnectOptions
            //{
            //    BrowserWSEndpoint = "wss://chrome.browserless.io?token=9f60a03f-7ad1-4f8a-bd49-f6893d704ba7"
            //});


            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                //ExecutablePath = PuppeteerExtensions.ExecutablePath
                ExecutablePath = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe"
            });


            await using var page = await browser.NewPageAsync();

            await page.EmulateMediaTypeAsync(MediaType.Screen);

            await page.SetRequestInterceptionAsync(true);

            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic.Add("Accept-Language", "en-US,en;q=0.9");

            await page.SetExtraHttpHeadersAsync(dic);

            await page.SetUserAgentAsync("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36");


            await page.SetContentAsync(html);
            var pdfContent = await page.PdfStreamAsync(new PdfOptions
            {
                Format = format,
                DisplayHeaderFooter = true,
                Landscape = false,
                PrintBackground = true

            });
            return pdfContent;//File(pdfContent, "application/pdf");

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string ConvertToBase64( Stream stream)
    {
        byte[] bytes;
        using (var memoryStream = new MemoryStream())
        {
            stream.CopyTo(memoryStream);
            bytes = memoryStream.ToArray();
        }

        string base64 = Convert.ToBase64String(bytes);
            return base64;
        //return new MemoryStream(Encoding.UTF8.GetBytes(base64));
    }

        [ApiExplorerSettings(IgnoreApi = true)]
        public static decimal RoundFirstSignificantDigit(decimal numberInit)
        {
            try
            {
                ///
                if (numberInit == 0)
                    return 0;
                int precision = 0;

                long ints = 0;
                string numberEnd;

                decimal val = numberInit;
                decimal aux;
                string auxchar;

                //numberDigit = 2;
                while (Math.Abs(val) < 1)
                {
                    val *= 10;
                    precision++;
                }

                var hh = (long)numberInit;
                var mm = Math.Log10(hh);
                var kk = Math.Floor(mm + 1);
                ints = (long)Math.Abs(numberInit); //(int)Math.Floor(Math.Log10(cedula) + 1);// len(abs(cast(@numberInit as bigint)))
                ints = ints.ToString().Length;

                if (precision < 1)
                {
                    if ((2 - ints) < 0)

                        precision = (int)ints;
                    else
                        precision = (int)(2 + (2 - ints));

                }

                else
                    precision = precision + 3;
                aux = Math.Round(Convert.ToDecimal(numberInit), precision - 2);  //    Round(Convert(decimal(25, 20), @numberInit), @precision - 2);
                auxchar = aux.ToString(); // CONVERT(nvarchar(40), @aux);

                numberEnd = auxchar.ToString().Substring(0, precision); 
                var ssub = auxchar.ToString().Substring(0, precision);//substring(@auxchar, 1, @precision)
                return Convert.ToDecimal(numberEnd);
            }
            catch (Exception ex)
            {
                return Math.Round(numberInit, 2);
            }
        }
    }
}