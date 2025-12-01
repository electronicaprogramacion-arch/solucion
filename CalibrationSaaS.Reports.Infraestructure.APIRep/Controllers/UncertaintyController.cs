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
using Reports.Domain.ReportViewModels;
using System.Drawing;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Helpers;
using Newtonsoft.Json.Linq;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using static CalibrationSaaS.Domain.Aggregates.Querys.Querys;
using Newtonsoft.Json;
using UncertaintyViewModel = Reports.Domain.ReportViewModels.UncertaintyViewModel;
using Totales = Reports.Domain.ReportViewModels.Totales;
using LinqKit;
using static SQLite.SQLite3;
using CalibrationSaaS.Infraestructure.EntityFramework.DataAccess;
using Helpers.Controls.ValueObjects;
using Microsoft.IdentityModel.Tokens;

namespace CalibrationSaaS.Reports.Infraestructure.APIRep.Controllers
{
    [Route("/api/print")]
    public class UncertaintyController : Controller
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
        private readonly UOMUseCases Uom;
        private readonly  IUOMRepository uoMRepository;


        public UncertaintyController(ITemplateService templateService, IWorkOrderDetailRepository workOrderDetailRepository,
            IBasicsRepository basicsRepository,
            IConfiguration _Configuration,
            WorkOrderDetailUseCase _WOD,
            IPieceOfEquipmentRepository pieceOfEquipmentRepository,
            IAssetsRepository _Assets,
            PieceOfEquipmentUseCases _Poe,
            UOMUseCases _uom,
            IUOMRepository _UoMRepository
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
            Uom = _uom;
            uoMRepository = _UoMRepository;
        }

        [HttpGet("LTIUncertaintyDynamic")]
        public async Task<IActionResult> LTIUncertaintyDynamic(int sequence, int calSubtyeId, int wodid)
        {
            WorkOrderDetail wo = new WorkOrderDetail();
            wo.WorkOrderDetailID = wodid;
            var wod = await _workOrderDetailRepository.GetByID(wo);
            var wod1 = await _workOrderDetailRepository.GetWorkOrderDetailByID(wodid);
            var genericObject = wod?.BalanceAndScaleCalibration?.TestPointResult?.FirstOrDefault(x => x.SequenceID == sequence && x.CalibrationSubTypeId == calSubtyeId);

            CalibrationType calType = new CalibrationType()
            {
                CalibrationTypeId = (int)wod.CalibrationTypeID
            };
            calType = await _basicsRepository.GetCalibrationTypeByID(calType);
            List<Uncertainty> uncertainties = null;
            if (wod?.WOD_Weights?.Count() > 0)
            {
                foreach (var standard in wod.WOD_Weights)
                {
                    
                    var precontributorPoe = await _pieceOfEquipmentRepository.GetUncertaintyByPoe(standard.WeightSet?.PieceOfEquipmentID);
                    standard.WeightSet.PieceOfEquipment = await _pieceOfEquipmentRepository.GetPieceOfEquipmentByID(standard.WeightSet.PieceOfEquipmentID);
                    standard.WeightSet.PieceOfEquipment.Uncertainty = precontributorPoe?.ToList();
                    
                }
            }
          
            


            //Contributors by testpoint


            //foreach (var item in genericObject?.GenericCalibration2?.WeightSets?.ToList())
            //{
            //    List<Uncertainty> uncertaintybyPoes = new List<Uncertainty>();
            //    var uncertaintybyPoe = await _pieceOfEquipmentRepository.GetUncertaintyByPoe(item.PieceOfEquipmentID);
            //    var poe = await _pieceOfEquipmentRepository.GetPieceOfEquipmentByID(item.PieceOfEquipmentID);
            //    uncertaintybyPoes = uncertaintybyPoe.ToList();

            //    item.PieceOfEquipment = poe;
            //    item.PieceOfEquipment.Uncertainty ??= new List<Uncertainty>();
                

            //    foreach (var item2 in uncertaintybyPoes)
            //    {
            //        item.PieceOfEquipment.Uncertainty.Add(item2);


            //        UnitOfMeasure uom = new UnitOfMeasure
            //        {
            //            UnitOfMeasureID = item2.UnitOfMeasureID
            //        };

            //        var uom_ = await  uoMRepository.GetByID(uom);

            //        item2.UnitOfMeasure = uom_;
            //    }
            //}

            var uncertObject = await new UncertaintyLogicDynamic().FormulaMethodToReportDynamic(genericObject, calType?.JsonUncertaintyConfiguration, wod, calType);
            string? html = "";

            UncertaintyViewModel model = new UncertaintyViewModel();
            var listContributors = new List<ContributorDynamic>();
            listContributors = uncertObject as List<ContributorDynamic>;

            var sumOfSquares = listContributors
                                .Where(x => !string.IsNullOrEmpty(x.Square.ToString()))
                                .Sum(x => double.TryParse(x.Square.ToString(), out var val) ? val : 0);
            var totalUncertainty = Math.Sqrt(sumOfSquares);
            var tot = new Totales()
            {
                SumOfSquares = Math.Round(sumOfSquares, 3).ToString(),
                TotalUncerainty = Math.Round(totalUncertainty, 3),
                ExpandedUncertainty = Math.Round(totalUncertainty * 2, 3)

            };
            model = new UncertaintyViewModel
            {
                Totales = tot,
                ContributorsDynamic = listContributors
            };

            await using var browser = await Puppeteer.ConnectAsync(new ConnectOptions
            {
                BrowserWSEndpoint = "wss://chrome.browserless.io?token=cae21211-c2d1-4325-9f79-70fecc54826c"
            });

            await using var page = await browser.NewPageAsync();
            await page.EmulateMediaTypeAsync(MediaType.Screen);
            html = await _templateService.RenderAsync("Templates/UncertaintyBudgetDynamic", model);

            await page.SetContentAsync(html);
            
            var pdfContent = await page.PdfStreamAsync(new PdfOptions
            {
                Format = PaperFormat.Letter,
                DisplayHeaderFooter = true,
                Landscape = true,
                PrintBackground = true
            });

            return File(pdfContent, "application/pdf");
        }

        [HttpGet("LTIUncertaintyDynamicTest")]
        public async Task<IActionResult> LTIUncertaintyDynamicTest(int wodid)
        {
            WorkOrderDetail wo = new WorkOrderDetail();
            wo.WorkOrderDetailID = wodid;
            var wod = await _workOrderDetailRepository.GetByID(wo);
            var wod1 = await _workOrderDetailRepository.GetWorkOrderDetailByID(wodid);
   

            var uncertObject = await new UncertaintyLogicDynamic().GetContributtors(wod);
            string? html = "";

            UncertaintyViewModel model = new UncertaintyViewModel();
            var listContributors = new List<ContributorDynamic>();
            listContributors = uncertObject as List<ContributorDynamic>;

            var sumOfSquares = listContributors
                                .Where(x => !string.IsNullOrEmpty(x.Square.ToString()))
                                .Sum(x => double.TryParse(x.Square.ToString(), out var val) ? val : 0);
            var totalUncertainty = Math.Sqrt(sumOfSquares);
            var tot = new Totales()
            {
                SumOfSquares = Math.Round(sumOfSquares, 3).ToString(),
                TotalUncerainty = Math.Round(totalUncertainty, 3),
                ExpandedUncertainty = Math.Round(totalUncertainty * 2, 3)

            };
            model = new UncertaintyViewModel
            {
                Totales = tot,
                ContributorsDynamic = listContributors
            };

            await using var browser = await Puppeteer.ConnectAsync(new ConnectOptions
            {
                BrowserWSEndpoint = "wss://chrome.browserless.io?token=cae21211-c2d1-4325-9f79-70fecc54826c"
            });

            await using var page = await browser.NewPageAsync();
            await page.EmulateMediaTypeAsync(MediaType.Screen);
            html = await _templateService.RenderAsync("Templates/UncertaintyBudgetDynamic", model);

            await page.SetContentAsync(html);

            var pdfContent = await page.PdfStreamAsync(new PdfOptions
            {
                Format = PaperFormat.Letter,
                DisplayHeaderFooter = true,
                Landscape = true,
                PrintBackground = true
            });

            return File(pdfContent, "application/pdf");
        }
        [HttpGet("LTIUncertaintyMicro")]
        public async Task<IActionResult> LTIReportUncertMicro(int sequence, int calSubtyeId, int wodid)

        {
            // Demo mode is enabled when Customer is "Demo"
            var customer = Configuration.GetSection("Reports")["Customer"];
            ViewBag.isdemo = (customer == "Demo") ? "true" : "false";
            WorkOrderDetail wo = new WorkOrderDetail();
            wo.WorkOrderDetailID = wodid;
            ViewModels.UncertaintyViewModel model = new ViewModels.UncertaintyViewModel();
            var wod = await _workOrderDetailRepository.GetByID(wo);
            var wod1 = await _workOrderDetailRepository.GetWorkOrderDetailByID(wodid);
            var genericObject = wod1.BalanceAndScaleCalibration.GenericCalibration.FirstOrDefault(x => x.SequenceID == sequence && x.CalibrationSubTypeId == calSubtyeId && x.WorkOrderDetailId == wodid);

            CalibrationType calType = new CalibrationType()
            {
                CalibrationTypeId = (int)wod.CalibrationTypeID
            };
            calType = await _basicsRepository.GetCalibrationTypeByID(calType);
            PieceOfEquipment poe = new PieceOfEquipment();
            poe = wod.PieceOfEquipment;
            var uncertObject = await new UncertaintyLogic2().FormulaMethod(genericObject.BasicCalibrationResult, poe, calType);
            string? html = "";
            UnitOfMeasure uomEntity = new UnitOfMeasure();
            uomEntity.UnitOfMeasureID = (int)wod.PieceOfEquipment.UnitOfMeasureID;
            var uom1 = await Uom.GetByID(uomEntity);
            List<APIRep.ViewModels.Contributor> listContributors = new List<APIRep.ViewModels.Contributor>();
            

            if (uncertObject != null)
            {
               
                model = JsonConvert.DeserializeObject<ViewModels.UncertaintyViewModel>(uncertObject.ToString());
                model.pieceOfEquipment = wod.PieceOfEquipment;
                model.WorkOrder = wod.WorkOrderID.ToString();
                model.Due = DateTime.Now.AddDays(10).ToString("dd-MM-yyyy");
                model.Id = wod.WorkOrderDetailID;
                model.Address = "Test Address"; // wod.Address.Description,
                model.City = "Test City"; // wod.Address.City,
                model.Country = "Test Country";// wod.Address.Country,
                model.Model = wod.PieceOfEquipment.EquipmentTemplate.Model;
                model.CompanyName = "Test Company";
                model.WorkOrderDetailId = wod.WorkOrderDetailID.ToString();
                model.FS = 0;
                model.Nominal = 0;
                model.UnitOfMeasure = uom1.Name;
                model.Title = "UNCERTAINTY MICRO";
          
                
                           
            html = await _templateService.RenderAsync("Templates/UncertaintyBudget", model);

            }

            await using var browser = await Puppeteer.ConnectAsync(new ConnectOptions
            {
                BrowserWSEndpoint = "wss://chrome.browserless.io?token=cae21211-c2d1-4325-9f79-70fecc54826c"
            });


                await using var page = await browser.NewPageAsync();
                await page.EmulateMediaTypeAsync(MediaType.Screen);
                await page.SetContentAsync(html);
                var pdfContent = await page.PdfStreamAsync(new PdfOptions
                {
                    Format = PaperFormat.Letter,
                    DisplayHeaderFooter = true,
                    Landscape = true,
                    PrintBackground = true

                });

                return File(pdfContent, "application/pdf");



        }

        [HttpGet("LTIUncertaintyBrinell")]
        public async Task<IActionResult> LTIReportUncertBrinell(int sequence, int calSubtyeId, int wodid)
        {
            WorkOrderDetail wo = new WorkOrderDetail();
            wo.WorkOrderDetailID = wodid;
            ViewModels.UncertaintyViewModel model = new ViewModels.UncertaintyViewModel();
            var wod = await _workOrderDetailRepository.GetByID(wo);
            var wod1 = await _workOrderDetailRepository.GetWorkOrderDetailByID(wodid);
            var genericObject = wod1.BalanceAndScaleCalibration.TestPointResult.FirstOrDefault(x => x.SequenceID == sequence && x.CalibrationSubTypeId == calSubtyeId && x.WorkOrderDetailId == wodid);

            CalibrationType calType = new CalibrationType()
            {
                CalibrationTypeId = (int)wod.CalibrationTypeID
            };
            calType = await _basicsRepository.GetCalibrationTypeByID(calType);

            var uncertObject = await new UncertaintyLogicBrinell().FormulaMethodToReport(genericObject, wod, calType);
            string? html = "";
            UnitOfMeasure uomEntity = new UnitOfMeasure();
            uomEntity.UnitOfMeasureID = (int)wod.PieceOfEquipment.UnitOfMeasureID;
            var uom1 = await Uom.GetByID(uomEntity);
            List<APIRep.ViewModels.Contributor> listContributors = new List<APIRep.ViewModels.Contributor>();


            if (uncertObject != null)
            {

                model = JsonConvert.DeserializeObject<ViewModels.UncertaintyViewModel>(uncertObject.ToString());
                
                model.pieceOfEquipment = wod.PieceOfEquipment;
                model.WorkOrder = wod.WorkOrderID.ToString();
                model.Due = DateTime.Now.AddDays(10).ToString("dd-MM-yyyy");
                model.Id = wod.WorkOrderDetailID;
                model.Address = "Test Address"; // wod.Address.Description,
                model.City = "Test City"; // wod.Address.City,
                model.Country = "Test Country";// wod.Address.Country,
                model.Model = wod.PieceOfEquipment.EquipmentTemplate.Model;
                model.CompanyName = "Test Company";
                model.WorkOrderDetailId = wod.WorkOrderDetailID.ToString();
                model.FS = 0;
                model.Nominal = 0;
                model.UnitOfMeasure = uom1.Name;
                model.Title = "UNCERTAINTY BRINELL";
                html = await _templateService.RenderAsync("Templates/UncertaintyBudget", model);

            }

            await using var browser = await Puppeteer.ConnectAsync(new ConnectOptions
            {
                BrowserWSEndpoint = "wss://chrome.browserless.io?token=cae21211-c2d1-4325-9f79-70fecc54826c"
            });


            await using var page = await browser.NewPageAsync();
            await page.EmulateMediaTypeAsync(MediaType.Screen);
            await page.SetContentAsync(html);
            var pdfContent = await page.PdfStreamAsync(new PdfOptions
            {
                Format = PaperFormat.Letter,
                DisplayHeaderFooter = true,
                Landscape = true,
                PrintBackground = true

            });

            return File(pdfContent, "application/pdf");



        }

        [HttpGet("LTIUncertaintyLeeb")]
        public async Task<IActionResult> LTIReportUncertLeeb(int sequence, int calSubtyeId, int wodid)

        {
            WorkOrderDetail wo = new WorkOrderDetail();
            wo.WorkOrderDetailID = wodid;
            ViewModels.UncertaintyViewModel model = new ViewModels.UncertaintyViewModel();
            var wod = await _workOrderDetailRepository.GetByID(wo);
            var wod1 = await _workOrderDetailRepository.GetWorkOrderDetailByID(wodid);
            var genericObject = wod.BalanceAndScaleCalibration?.TestPointResult?.FirstOrDefault(x => x.SequenceID == sequence && x.CalibrationSubTypeId == calSubtyeId && x.ComponentID == wodid.ToString());
            CalibrationType calType = new CalibrationType()
            {
                CalibrationTypeId = (int)wod.CalibrationTypeID
            };
            calType = await _basicsRepository.GetCalibrationTypeByID(calType);
           var uncertObject = await new UncertaintyLogicLeeb().FormulaMethodToReport(genericObject, wod, calType);

            string? html = "";
            UnitOfMeasure uomEntity = new UnitOfMeasure();
            uomEntity.UnitOfMeasureID = (int)wod.PieceOfEquipment.UnitOfMeasureID;
            var uom1 = await Uom.GetByID(uomEntity);
            List<APIRep.ViewModels.Contributor> listContributors = new List<APIRep.ViewModels.Contributor>();


            if (uncertObject != null)
            {

                model = JsonConvert.DeserializeObject<ViewModels.UncertaintyViewModel>(uncertObject.ToString());
                model.pieceOfEquipment = wod.PieceOfEquipment;
                model.WorkOrder = wod.WorkOrderID.ToString();
                model.Due = DateTime.Now.AddDays(10).ToString("dd-MM-yyyy");
                model.Id = wod.WorkOrderDetailID;
                model.Address = "Test Address"; // wod.Address.Description,
                model.City = "Test City"; // wod.Address.City,
                model.Country = "Test Country";// wod.Address.Country,
                model.Model = wod.PieceOfEquipment.EquipmentTemplate.Model;
                model.CompanyName = "Test Company";
                model.WorkOrderDetailId = wod.WorkOrderDetailID.ToString();
                model.FS = 0;
                model.Nominal = 0;
                model.UnitOfMeasure = uom1.Name;
                model.Title = "UNCERTAINTY BUDGET";
                html = await _templateService.RenderAsync("Templates/UncertaintyBudget", model);

            }

            await using var browser = await Puppeteer.ConnectAsync(new ConnectOptions
            {
                BrowserWSEndpoint = "wss://chrome.browserless.io?token=cae21211-c2d1-4325-9f79-70fecc54826c"
            });


            await using var page = await browser.NewPageAsync();
            await page.EmulateMediaTypeAsync(MediaType.Screen);
            await page.SetContentAsync(html);
            var pdfContent = await page.PdfStreamAsync(new PdfOptions
            {
                Format = PaperFormat.Letter,
                DisplayHeaderFooter = true,
                Landscape = true,
                PrintBackground = true

            });

            return File(pdfContent, "application/pdf");



        }
        [HttpGet("LTIUncertaintyForceGage")]
        public async Task<IActionResult> LTIReportUncertForceGage(int sequence, int calSubtyeId, int wodid)

        {
            WorkOrderDetail wo = new WorkOrderDetail();
            wo.WorkOrderDetailID = wodid;
            ViewModels.UncertaintyViewModel model = new ViewModels.UncertaintyViewModel();
            var wod = await _workOrderDetailRepository.GetByID(wo);
            var wod1 = await _workOrderDetailRepository.GetWorkOrderDetailByID(wodid);
            var genericObject = wod1.BalanceAndScaleCalibration.GenericCalibration.FirstOrDefault(x => x.SequenceID == sequence && x.CalibrationSubTypeId == calSubtyeId && x.WorkOrderDetailId == wodid);

            var uncertObject = await new UncertaintyForceGage().FormulaMethodToReport(genericObject);
            string? html = "";
            UnitOfMeasure uomEntity = new UnitOfMeasure();
            uomEntity.UnitOfMeasureID = (int)wod.PieceOfEquipment.UnitOfMeasureID;
            var uom1 = await Uom.GetByID(uomEntity);
            List<APIRep.ViewModels.Contributor> listContributors = new List<APIRep.ViewModels.Contributor>();


            if (uncertObject != null)
            {

                model = JsonConvert.DeserializeObject<ViewModels.UncertaintyViewModel>(uncertObject.ToString());
                model.pieceOfEquipment = wod.PieceOfEquipment;
                model.WorkOrder = wod.WorkOrderID.ToString();
                model.Due = DateTime.Now.AddDays(10).ToString("dd-MM-yyyy");
                model.Id = wod.WorkOrderDetailID;
                model.Address = "Test Address"; // wod.Address.Description,
                model.City = "Test City"; // wod.Address.City,
                model.Country = "Test Country";// wod.Address.Country,
                model.Model = wod.PieceOfEquipment.EquipmentTemplate.Model;
                model.CompanyName = "Test Company";
                model.WorkOrderDetailId = wod.WorkOrderDetailID.ToString();
                model.FS = 0;
                model.Nominal = 0;
                model.UnitOfMeasure = uom1.Name;
                model.Title = "UNCERTAINTY FORCE GAGE";
                html = await _templateService.RenderAsync("Templates/UncertaintyBudget", model);

            }

            await using var browser = await Puppeteer.ConnectAsync(new ConnectOptions
            {
                BrowserWSEndpoint = "wss://chrome.browserless.io?token=cae21211-c2d1-4325-9f79-70fecc54826c"
            });


            await using var page = await browser.NewPageAsync();
            await page.EmulateMediaTypeAsync(MediaType.Screen);
            await page.SetContentAsync(html);
            var pdfContent = await page.PdfStreamAsync(new PdfOptions
            {
                Format = PaperFormat.Letter,
                DisplayHeaderFooter = true,
                Landscape = true,
                PrintBackground = true

            });

            return File(pdfContent, "application/pdf");



        }
        [HttpGet("LTIUncertaintyBudgetScale")]
        public async Task<IActionResult> LTIReportScale(int id, int seq)
        {

            WorkOrderDetail w = new WorkOrderDetail();


            w.WorkOrderDetailID = id;
            var wod = await _workOrderDetailRepository.GetByID(w);
            var isAccredited = wod.IsAccredited;

            var weights = await WOD.GetByID(w);

            var _weigths = weights.WOD_Weights;

            var _poe = wod.PieceOfEquipment;
            var _cus = _poe.Customer;

            var _address = _cus.Aggregates.FirstOrDefault().Addresses.FirstOrDefault();
            Boolean isISO = true;
           UncertaintyViewModel model = new UncertaintyViewModel();
            List<UncertaintyOfRerence> uncertaintyOfRerences = new List<UncertaintyOfRerence>();
            List<RepeatabilityOfBalance> repeatabilityOfBalances = new List<RepeatabilityOfBalance>();
            List<CornerloadEccentricity> cornerloadEccentricities = new List<CornerloadEccentricity>();
            List<EnvironmentalFactors> environmentalFactors = new List<EnvironmentalFactors>();
            List<Resolution> resolutions = new List<Resolution>();
            Totales totales = new Totales();

            Linearity linearity = new Linearity();
            linearity.WorkOrderDetailId = id;
            linearity.TestPointID = seq;
            string? html = "";

            
            var result = await Poe.GetReportUncertaintyBudget(linearity, seq, wod,null, null, null);
            var resultUncert = result.UncertaintyOfRerences;
            var resultRep = result.RepeatabilityOfBalances;
            var resultEcce = result.CornerloadEccentricities;
            var resultEnv = result.EnvironmentalFactors;
            var resultRes = result.Resolutions;
            var resultMpe = result.UncertaintyMPEs;
            var resultAir = result.AirBuoyancies;
            var resultDrift = result.DriftStabilities;
            var resultTem = result.TemperatureEffects;
            totales = result.Totales;

            if (result != null)
            {

                model = new UncertaintyViewModel
                {

                    CalibrationDate = DateTime.Now.AddDays(10).ToString("dd-MM-yyyy"),//wod.CalibrationCustomDueDate,
                    Id = wod.WorkOrderDetailID,
                    UncertaintyOfRerences = resultUncert,
                    RepeatabilityOfBalances = resultRep,
                    CornerloadEccentricities = resultEcce,
                    EnvironmentalFactors = resultEnv,
                    Resolutions = resultRes,
                    Totales = totales,
                    cmcValuesReplace = result.cmcValuesReplace,
                    UncertaintyMPEs = resultMpe,
                    AirBuoyancies = resultAir,
                    DriftStabilities = resultDrift,
                    TemperatureEffects = resultTem

                };
                try
                {
                    if (wod.CertificationID == 2)
                    {
                        html = await _templateService.RenderAsync("Templates/UncertaintyBudgetScaleAnab", model);
                    }
                    else
                    {
                        html = await _templateService.RenderAsync("Templates/UncertaintyBudgetScale", model);
                    }
                }
                catch (Exception ex)
                {

                }
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
                Landscape = true,
                PrintBackground = true

            });

            return File(pdfContent, "application/pdf");


        }


        [HttpGet("LTIUncertaintyBudget")]
        public async Task<IActionResult> LTIReport(int id, int seq, int CalibSubtyId)
        {
            WorkOrderDetail w = new WorkOrderDetail();
            w.WorkOrderDetailID = id;
            var weights = await WOD.GetByID(w);//await _workOrderDetailRepository.GetWorkOrderDetailByID(id);

            //var wod = await _workOrderDetailRepository.GetByID(w);

            var isAccredited = weights.IsAccredited;


            ICollection<Force> force = null;
            if (weights.BalanceAndScaleCalibration.Forces != null)
            {
                force = weights.BalanceAndScaleCalibration.Forces.Where(x => x.BasicCalibrationResult.CalibrationSubTypeId == CalibSubtyId).ToList();
            }


            bool includeAstm = weights.IncludeASTM;

            var _weigths = weights.WOD_Weights;

            var _poe = weights.PieceOfEquipment;
            var _cus = _poe.Customer;

            var _address = _cus.Aggregates.FirstOrDefault().Addresses.FirstOrDefault();
            Boolean isISO = true;
            int? iso = 0;

            if (weights.CertificationID != null)
                iso = weights.CertificationID;

            APIRep.ViewModels.UncertaintyViewModel model = new APIRep.ViewModels.UncertaintyViewModel();
            List<APIRep.ViewModels.Contributor> listContributors = new List<APIRep.ViewModels.Contributor>();
            List<APIRep.ViewModels.Contributor> listContributorsRep = new List<APIRep.ViewModels.Contributor>();
            List<APIRep.ViewModels.Contributor> listContributorsASTM = new List<APIRep.ViewModels.Contributor>();
            APIRep.ViewModels.Totales totales = new APIRep.ViewModels.Totales();
            APIRep.ViewModels.Totales totalesAstm = new APIRep.ViewModels.Totales();

            string? html = "";

            var dto = new ISOandASTM();
            dto.Forces = force.ToList();
            dto.WorkOrderDetail = weights;


            if (force != null)
            {
                var forces = await Poe.GetCalculatesForISOandASTM(dto);
                forces = await Poe.CalculateUncertainty(forces.ToList(), (int)iso, weights.Tolerance);

                var forcesNew = new List<Force>();
                foreach (var item in forces)
                {
                    var clonedJson = JsonConvert.SerializeObject(item);
                    var resultCloned = JsonConvert.DeserializeObject<Force>(clonedJson);

                    resultCloned.CalibrationResultContributors = resultCloned.CalibrationResultContributors.Where(x => x.TypeContributor == "Standard" || x.TypeContributor == "EquipmentTemplate").ToList();

                    forcesNew.Add(resultCloned);
                }
                var forcesASTM = await Poe.CalculateUncertASTM(forcesNew.ToList(), (int)iso, null);
                var forcesSeqASTM = forcesASTM.Where(x => x.BasicCalibrationResult.Position == seq).FirstOrDefault();

                var forceSeq = forces.Where(x => x.BasicCalibrationResult.Position == seq).FirstOrDefault();

                UnitOfMeasure uomEntity = new UnitOfMeasure();

                if (forceSeq != null)
                    uomEntity.UnitOfMeasureID = (int)forceSeq.UnitOfMeasureId;
                else
                    uomEntity.UnitOfMeasureID = (int)forcesSeqASTM.UnitOfMeasureId;
                var uom1 = await Uom.GetByID(uomEntity);

                string cmcReplace = null;


                if (iso == 1 && forceSeq != null && forceSeq.CalibrationResultContributors != null && forceSeq.CalibrationResultContributors.Count() > 0)
                {
                    double sumSquares = 0;
                    double sumSquaresRep = 0;
                    double totalUncert = 0;
                    double totalUncertRep = 0;
                    double expandedUncert = 0;
                    double expandedUncertRep = 0;


                    foreach (var _item in forceSeq.CalibrationResultContributors)
                    {
                        APIRep.ViewModels.Contributor contributor = new APIRep.ViewModels.Contributor();
                        if (_item.PieceOfEquipment != null)
                        {
                            contributor.SerialNumber = _item.PieceOfEquipment.SerialNumber;
                            contributor.ControlNumber = _item.PieceOfEquipment.PieceOfEquipmentID;
                        }
                        else
                        {
                            contributor.SerialNumber = _poe.SerialNumber;
                            contributor.ControlNumber = _poe.PieceOfEquipmentID;
                        }
                        if (_item.Description != null && _item.Description.Contains("Repeatability"))
                            contributor.Type = "A";
                        else
                            contributor.Type = "B";



                        contributor.CalibrationRole = _item.TypeContributor;
                        contributor.Contributors = _item.Description;
                        contributor.Magnitude = Math.Round(_item.Magnitude, 3).ToString();
                        contributor.Units = uom1.Abbreviation;


                        contributor.Distribution = _item.Distribution;
                        contributor.Divisor = Math.Round(_item.Divisor, 3).ToString();
                        contributor.Quotient = Math.Round(_item.Quotient, 8).ToString();
                        contributor.Square = Math.Round(_item.Square, 3).ToString();

                        contributor.Comment = _item.Comment;

                        sumSquares = (sumSquares + _item.Square);
                        totalUncert = Math.Sqrt(sumSquares);
                        expandedUncert = totalUncert * 2;
                        totales.SumOfSquares = Math.Round(sumSquares, 2).ToString();
                        totales.TotalUncerainty = Math.Round(totalUncert, 2).ToString();
                        totales.ExpandedUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(expandedUncert))).ToString();

                        if (forceSeq.BasicCalibrationResult.UncertaintyNew != null && totalUncert < forceSeq.BasicCalibrationResult.UncertaintyNew)
                        {
                            cmcReplace = "The original value for this uncertainty was: " +
                                        totalUncert + "-" + uom1.Abbreviation +
                                        " but it was replaced by the CMC value of " + forceSeq.BasicCalibrationResult.UncertaintyNew +
                                        " according to the range limit of " + forceSeq.MinRange + " " + uom1.Abbreviation + "-" + forceSeq.MaxRange + " " + uom1.Abbreviation;
                            totales.ExpandedUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(forceSeq.BasicCalibrationResult.UncertaintyNew))).ToString() + " * ";
                        }
                        listContributors.Add(contributor);

                    }
                }


                if ((iso == 2 || includeAstm == true) && forcesSeqASTM != null && forcesSeqASTM.CalibrationResultContributors != null && forcesSeqASTM.CalibrationResultContributors.Count() > 0)
                {


                    double sumSquares = 0;
                    double sumSquaresRep = 0;
                    double totalUncert = 0;
                    double totalUncertRep = 0;
                    double expandedUncert = 0;
                    double expandedUncertRep = 0;
                    includeAstm = true;

                    foreach (var _item in forcesSeqASTM.CalibrationResultContributors) //forcesSeqASTM.CalibrationResultContributors)
                    {
                        APIRep.ViewModels.Contributor contributor = new APIRep.ViewModels.Contributor();
                        if (_item.PieceOfEquipment != null)
                        {
                            contributor.SerialNumber = _item.PieceOfEquipment.SerialNumber;
                            contributor.ControlNumber = _item.PieceOfEquipment.PieceOfEquipmentID;
                        }
                        else
                        {
                            contributor.SerialNumber = _poe.SerialNumber;
                            contributor.ControlNumber = _poe.PieceOfEquipmentID;
                        }

                        if (_item.Description != null && _item.Description.Contains("Repeatability"))
                            contributor.Type = "A";
                        else
                            contributor.Type = "B";
                        contributor.CalibrationRole = _item.TypeContributor;
                        contributor.Contributors = _item.Description;
                        contributor.Magnitude = Math.Round(_item.Magnitude, 3).ToString();
                        contributor.Units = uom1.Abbreviation;
                        contributor.Distribution = _item.Distribution;
                        contributor.Divisor = Math.Round(_item.Divisor, 3).ToString();
                        contributor.Quotient = Math.Round(_item.Quotient, 8).ToString();
                        contributor.Square = Math.Round(_item.Square, 3).ToString();

                        contributor.Comment = _item.Comment;

                        sumSquares = (sumSquares + _item.Square);
                        totalUncert = Math.Sqrt(sumSquares);
                        expandedUncert = totalUncert * 2;
                        totalesAstm.SumOfSquares = Math.Round(sumSquares, 2).ToString();
                        totalesAstm.TotalUncerainty = Math.Round(totalUncert, 2).ToString();
                        totalesAstm.ExpandedUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(expandedUncert))).ToString() + " * ";
                        if (forcesSeqASTM.BasicCalibrationResult.UncertaintyNew != null && totalUncert < forcesSeqASTM.BasicCalibrationResult.UncertaintyNew)
                        {
                            totalesAstm.cmcValuesReplace = "The original value for this uncertainty was: " +
                                        totalUncert + "-" + uom1.Abbreviation +
                                        " but it was replaced by the CMC value of " + forcesSeqASTM.BasicCalibrationResult.UncertaintyNew +
                                        " according to the range limit of " + forceSeq.MinRange + " " + uom1.Abbreviation + "-" + forcesSeqASTM.MaxRange + " " + uom1.Abbreviation;
                            totalesAstm.ExpandedUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(forcesSeqASTM.BasicCalibrationResult.UncertaintyNew))).ToString() + " * ";
                        }
                            listContributorsASTM.Add(contributor);

                        }
                    }


                    model = new APIRep.ViewModels.UncertaintyViewModel
                    {

                        Due = DateTime.Now.AddDays(10).ToString("dd-MM-yyyy"),//wod.CalibrationCustomDueDate,
                        Id = weights.WorkOrderDetailID,
                        Address = "Test Address", // wod.Address.Description,
                        City = "Test City", // wod.Address.City,
                        Country = "Test Country",// wod.Address.Country,
                        Model = weights.PieceOfEquipment.EquipmentTemplate.Model,
                        CompanyName = "Test Company",
                        pieceOfEquipment = _poe,
                        Contributors = listContributors,
                        ContributorsRep = listContributorsRep,
                        totales = totales,
                        totalesAstm = totalesAstm,
                        WorkOrderDetailId = weights.WorkOrderDetailUserID.ToString(),
                        FS = forceSeq.BasicCalibrationResult.FS,
                        Nominal = forceSeq.BasicCalibrationResult.Nominal,
                        UnitOfMeasure = uom1.Abbreviation,
                        WorkOrder = weights.WorkOrderID.ToString(),
                        IncludeASTM = includeAstm,
                        ContributorsASTM = listContributorsASTM,
                        CalibrationSubtypeId = CalibSubtyId,
                        cmcValuesReplace = cmcReplace,


                    };

                    html = await _templateService.RenderAsync("Templates/UncertaintyBudget", model);
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
                    Landscape = true,
                    PrintBackground = true

                });

                return File(pdfContent, "application/pdf");


            }
        
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

                while (Math.Abs(val) < 1)
                {
                    val *= 10;
                    precision++;
                }

                var hh = (long)numberInit;
                var mm = Math.Log10(hh);
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
                aux = Math.Round(Convert.ToDecimal(numberInit), precision - 2);
                auxchar = aux.ToString();

                numberEnd = auxchar.ToString().Substring(0, precision);
                var ssub = auxchar.ToString().Substring(0, precision);//substring(@auxchar, 1, @precision)
                return Convert.ToDecimal(numberEnd);
            }
            catch (Exception ex)
            {
//                Console.WriteLine(ex);
                return Math.Round(numberInit, 2);
            }
        }



    }
}