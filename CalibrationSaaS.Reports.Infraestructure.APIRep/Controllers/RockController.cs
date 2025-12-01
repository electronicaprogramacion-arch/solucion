 using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CalibrationSaaS.Reports.Infraestructure.APIRep.Extensions;
using CalibrationSaaS.Reports.Infraestructure.APIRep.Services.Meta;
using CalibrationSaaS.Reports.Infraestructure.APIRep.ViewModels;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Application.UseCases;
using Reports.Domain.ReportViewModels;
using CalibrationSaaS.Domain.Aggregates.Querys;
using QRCoder;

using System.Drawing;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Runtime.InteropServices;
//using Helpers;

namespace CalibrationSaaS.Reports.Infraestructure.APIRep.Controllers
{
    [Route("/api/print")]
    public class RockController : Controller
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



        public RockController(ITemplateService templateService, IWorkOrderDetailRepository workOrderDetailRepository,
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


         [HttpGet("LTIreportrock2")]
        public async Task<IActionResult> LTIPDFROCK(int id)
        {


            var request = HttpContext.Request;

            var host = request.Host.Value;


            if (request.IsHttps)
            {
                host ="https://" + host;
            }

            else
            {
                host ="http://" + host;
            }

            //var urlstr= request.
            //var wo = new WorkOrderDetail();
            //wo.WorkOrderDetailID = id;
            //var header = await WOD.GetWorkOrderDetailXIdRep2(wo);

            //return View("Templates/Certified",header);

            return await SeePdfView(host + "/api/print/LTIReportrock2?id=" + id,"");
        
        }



         [HttpGet("LTIReportRockwell")]
        public async Task<IActionResult> LTIReport(int id)     
        
        {
            
            WorkOrderDetail w = new WorkOrderDetail();

            
           w.WorkOrderDetailID = id;
            var wod = await _workOrderDetailRepository.GetByID(w);
            
            var isAccredited = wod.IsAccredited;

            var weights = await WOD.GetByID(w);//await _workOrderDetailRepository.GetWorkOrderDetailByID(id);
            var rock = weights.BalanceAndScaleCalibration.Rockwells;
          
            var _weigths = weights.WOD_Weights;

            var _poe = wod.PieceOfEquipment;   
            var _cus = _poe.Customer;

            var _address = _cus.Aggregates.FirstOrDefault().Addresses.FirstOrDefault();
            Boolean isISO = true;
            RockViewModel model = new RockViewModel(); 

            
          
            List<PieceOfEquipment> poes = new List<PieceOfEquipment>();
           
           
            //Get Technician
            var status = await _workOrderDetailRepository.GetStatus();

            var history = await _workOrderDetailRepository.GetHistory(wod);


            var laststatus = status.Where(x => x.IsLast == true).FirstOrDefault();

            var aproveduserid = history.Where(x => laststatus != null && x.StatusId == laststatus.StatusId).OrderByDescending(x => x.WorkDetailHistoryID).FirstOrDefault();

           
            string tecnameaproved = String.Empty;
            if (aproveduserid != null && aproveduserid.TechnicianID.HasValue)
            {
                var useraproved = await _basicsRepository.GetUserById2(new User() { UserID = aproveduserid.TechnicianID.Value });
                tecnameaproved = useraproved?.Name + " " + useraproved?.LastName;
            }
            if (tecnameaproved == "" || tecnameaproved == null )
                tecnameaproved = "Not set";

             var reviewuserid = history.Where(x => laststatus != null && x.StatusId == (laststatus.StatusId - 1)).OrderByDescending(x => x.WorkDetailHistoryID).FirstOrDefault();

             string tecnreview = String.Empty;
            if (reviewuserid != null && reviewuserid.TechnicianID.HasValue)
            {
                var tecnreview1 = await _basicsRepository.GetUserById2(new User() { UserID = reviewuserid.TechnicianID.Value });
                tecnreview = tecnreview1?.Name + " " + tecnreview1?.LastName;
            }

            if (tecnreview == "" || tecnreview == null )
                tecnreview = "Not set";

            string? html = "";
             List<StandardHeaderRock> listw = new List<StandardHeaderRock>();
            var standardsRock = await WOD.GetCalibrationSubType_StandardByWodI(wod.WorkOrderDetailID);


               if(standardsRock != null && standardsRock.Count() > 0)
            {
               

                foreach (var item in standardsRock)
                {
                    var poetmp = await _pieceOfEquipmentRepository.GetPieceOfEquipmentByIDHeader(item.PieceOfEquipmentID);

                    var _listCetificate = await Assets.GetCertificateXPoE(poetmp);


                    CertificatePoE cert = _listCetificate.OrderByDescending(x => x.Version).FirstOrDefault();

                    string certnumber = "";
                    if(cert != null)
                    {
                        certnumber = cert.CertificateNumber;
                    }

                    string uom1;
                    if (poetmp.UnitOfMeasure != null)
                    {
                        uom1 = poetmp.UnitOfMeasure.Abbreviation;
                    }
                    else
                    {
                        uom1 = "NA";
                    }
                    //.CalibrationDate.Value.ToString("MM/dd/yyyy"),
                    StandardHeaderRock ww = new StandardHeaderRock()
                    {
                         PoE=poetmp.PieceOfEquipmentID,
                         Serial=poetmp.SerialNumber,
                         Ref=certnumber,
                         CalibrationDueDate=poetmp.DueDate.ToString("MM/dd/yyyy"),
                         Note=poetmp.Notes,
                         CalibrationDate = poetmp.CalibrationDate.ToString("MM/dd/yyyy"),
                         EquipmentType = poetmp.EquipmentTemplate.EquipmentTypeObject.Name,
                          Capacity = poetmp.Capacity,
                          UnitOfMeasure = uom1

                    };
                    listw.Add(ww);
                }			


            }


            //yppp Add Temperature Standard

               PieceOfEquipment poeT = new PieceOfEquipment();
            poeT.PieceOfEquipmentID = wod.TemperatureStandardId;// wod.PieceOfEquipmentId;
            if (poeT.PieceOfEquipmentID != null)
            { 
            var tempStandard = await Poe.GetPieceOfEquipmentByID(poeT);

            var _listCetificate1 = await Assets.GetCertificateXPoE(tempStandard);


            CertificatePoE cert1 = _listCetificate1.OrderByDescending(x => x.Version).FirstOrDefault();

            string certnumber1 = "";
            if (cert1 != null)
            {
                certnumber1 = cert1.CertificateNumber;
            }

                string uom;
                if (tempStandard.UnitOfMeasure != null)
                {
                    uom = tempStandard.UnitOfMeasure.Name;
                }
                else
                {
                    uom = "NA";
                }

                StandardHeaderRock ww1 = new StandardHeaderRock()
            {
                PoE = tempStandard.PieceOfEquipmentID,
                Serial = tempStandard.SerialNumber,
                Ref = certnumber1,
                CalibrationDueDate = tempStandard.DueDate.ToString("MM/dd/yyyy"),
                Note = tempStandard.Notes,
                EquipmentType = tempStandard.EquipmentTemplate.EquipmentTypeObject.Name,
                Capacity = tempStandard.Capacity,
                UnitOfMeasure = uom,
            };
            listw.Add(ww1);

            }

            model.StandardHeaderRockList = listw;
            List<RockItemViewModel> listAsFound = new List<RockItemViewModel>();
            List<RockItemViewModel> listAsLeft = new List<RockItemViewModel>();

            if (rock != null )
            {
                var iso = wod.CertificationID;
                var includeASTM = wod.IncludeASTM;

             
                var rockAsfoundISO = rock.Where(x => x.CalibrationSubTypeId == 10);
                var rockAsleftISO = rock.Where(x => x.CalibrationSubTypeId == 11);
                var rockAsfoundASTM = rock.Where(x => x.CalibrationSubTypeId == 12);
                var rockAsleftASTM = rock.Where(x => x.CalibrationSubTypeId == 13);



                string _class;
                
                double adjTension = 0;
                double adjCompression = 0;
                double adjUniversal = 0;

                bool _showCompress = false;
                int contAsFound = 0;

                var  asFound = rockAsfoundISO.ToList();
                if (asFound.Count() == 0 && rockAsfoundASTM.Count() >0)
                {
                    asFound = rockAsfoundASTM.ToList();
                    isISO = false;
                }
                
                var asleft = rockAsleftISO.ToList();
                
               
                if (asleft.Count() == 0 && rockAsleftASTM.Count() >0)
                { 
                    asleft = rockAsleftASTM.ToList();
                    isISO = false;
                }
         
                DateTime dueDate;
                string due = "";
                DateTime calDate;
                string calibrationDate = "";
                if (wod.CalibrationCustomDueDate != null)
                {
                    dueDate = (DateTime)wod.CalibrationCustomDueDate;
                    due = dueDate.ToString("MM/dd/yyyy");
                }
                if (wod.CalibrationDate != null)
                {
                    calDate = (DateTime)wod.CalibrationDate;
                    calibrationDate = calDate.ToString("MM/dd/yyyy");
                }
                if (asFound.Count() > 0)
                {
                    foreach (var _item in asFound)
                    {
                        PieceOfEquipment standardSelect = new PieceOfEquipment();   
                        if (_item.Standards !=null && _item.Standards.Count()>0)
                        standardSelect = await _pieceOfEquipmentRepository.GetPieceOfEquipmentByIDHeader(_item.Standards.FirstOrDefault().PieceOfEquipmentID);
                        string standard = "0";
                        if (standardSelect !=null && standardSelect.PieceOfEquipmentID !=null)
                        { 
                         standard = standardSelect.Capacity + "+-" + wod.Resolution;
                        }
                        listAsFound.Add(
                        new RockItemViewModel(
                          _item.BasicCalibrationResult.ScaleRange,
                          standard,
                         Math.Round(_item.BasicCalibrationResult.Average, 3),
                          _item.BasicCalibrationResult.Test1,
                          _item.BasicCalibrationResult.Test2,
                          0,0,0,
                          (double)RoundFirstSignificantDigit(Convert.ToDecimal(_item.BasicCalibrationResult.Repeateability)) ,
                          _item.BasicCalibrationResult.Error,
                         (double)RoundFirstSignificantDigit(Convert.ToDecimal(_item.BasicCalibrationResult.Uncertanty))
                    ));
                    }
                }

                if (asleft.Count() > 0)
                {
                    foreach (var _item in asleft)
                    {
                        PieceOfEquipment standardSelect = new PieceOfEquipment();
                        if (_item.Standards != null && _item.Standards.Count() > 0)
                            standardSelect = await _pieceOfEquipmentRepository.GetPieceOfEquipmentByIDHeader(_item.Standards.FirstOrDefault().PieceOfEquipmentID);
                        string standard = "0";
                        if (standardSelect != null && standardSelect.PieceOfEquipmentID != null)
                        {
                            standard = standardSelect.Capacity + "+-" + wod.Resolution;
                        }
                        listAsLeft.Add
                      (
                        new RockItemViewModel(
                          _item.BasicCalibrationResult.ScaleRange,
                          standard,
                          Math.Round(_item.BasicCalibrationResult.Average,3),
                          _item.BasicCalibrationResult.Test1,
                          _item.BasicCalibrationResult.Test2,
                          _item.BasicCalibrationResult.Test3,
                          _item.BasicCalibrationResult.Test4,
                          _item.BasicCalibrationResult.Test5,
                         (double)RoundFirstSignificantDigit(Convert.ToDecimal(_item.BasicCalibrationResult.Repeateability)),
                          _item.BasicCalibrationResult.Error,
                           (double)RoundFirstSignificantDigit(Convert.ToDecimal(_item.BasicCalibrationResult.Uncertanty))

                       ));
                    }
                }


                //Notes 
                NoteViewModel noteViewModel = new NoteViewModel();
                noteViewModel = await GetNotes(_poe, wod);

                var eqCondition = wod.EquipmentCondition.ToList();
                string received = "";
                string returned = "";
                foreach (var item in eqCondition)
                {
                    if (item.IsAsFound)
                    {
                        if (item.Value)
                        {
                            received = "In Service";
                        }
                        else
                        {
                            received = "Out of Service";
                        }
                    }
                    else
                    {
                        if (item.Value)
                        {
                            returned = "In Service";
                        }
                        else
                        {
                            returned = "Out of Service";
                        }
                    }
                }
                model = new RockViewModel
                {

                    CreatedAt = calibrationDate,//DateTime.Now.ToString("dd-MM-yyyy"),
                    Due = due,//DateTime.Now.AddDays(10).ToString("dd-MM-yyyy"),
                    Id = wod.WorkOrderDetailID,
                    Address = "Test Address", // wod.Address.Description,
                    City = "Test City", // wod.Address.City,
                    Country = "Test Country",// wod.Address.Country,
                    Manufaturer = _poe.EquipmentTemplate.Manufacturer1.Name,
                    Model = wod.PieceOfEquipment.EquipmentTemplate.Model,
                    CompanyName = "Test Company",
                    Sign = tecnameaproved,
                    pieceOfEquipment = _poe,
                    customer = _cus,
                    address = _address,
                   // weights = _weigths,
                    StandardHeaderRockList = listw,
                    Accredited = (bool)isAccredited,
                    TemperatureInit = weights.Temperature.ToString(),
                     TemperatureEnd = weights.TemperatureAfter.ToString(),
                   AsFounds = listAsFound,
                   AsLefts = listAsLeft,
                   isISO = isISO,
                   NoteViewModel = noteViewModel,
                   CertificateComment = weights.CertificateComment,
                   Procedure = wod.TestCode.Procedure.Name,
                   Capacity = wod.PieceOfEquipment.Capacity,
                    UoM = wod.PieceOfEquipment.UnitOfMeasure.Abbreviation,
                    Resolution = wod.PieceOfEquipment.Resolution,
                    Unit = _poe.PieceOfEquipmentID,
                    CustomerTool = _poe.CustomerToolId,
                    InstallLocation = _poe.InstallLocation,
                    ReceivedCondition = received,
                    ReturnedCondition = returned

                };

                //if (isISO == true)
                //{
                    html = await _templateService.RenderAsync("Templates/RockwellReport", model);
                //}
                //else
                //{
                //    html = await _templateService.RenderAsync("Templates/ASTMLTITemplate", model);
                //}

            }
            else
            {
                html = await _templateService.RenderAsync("Templates/Error", model);

            }
            await using var browser = await Puppeteer.ConnectAsync(new ConnectOptions
            {
                BrowserWSEndpoint = "wss://chrome.browserless.io?token=cae21211-c2d1-4325-9f79-70fecc54826c"
            });

            //var browser =await Puppeteer.LaunchAsync(new LaunchOptions
            //{
            //    Headless = false,
            //    ExecutablePath = PuppeteerExtensions.ExecutablePath
            //});
            await using var page = await browser.NewPageAsync();
            await page.EmulateMediaTypeAsync(MediaType.Screen);
            await page.SetContentAsync(html);
            var pdfContent = await page.PdfStreamAsync(new PdfOptions
            {
                Format = PaperFormat.Letter,
                DisplayHeaderFooter = true,
                Landscape = false,
                PrintBackground = true
   
            });

            
            
            return File(pdfContent, "application/pdf");


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
                BrowserWSEndpoint = "wss://chrome.browserless.io?token=cae21211-c2d1-4325-9f79-70fecc54826c"
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