using CalibrationSaaS.Application.UseCases;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Reports.Infraestructure.APIRep.Extensions;
using CalibrationSaaS.Reports.Infraestructure.APIRep.Services.Meta;
using CalibrationSaaS.Reports.Infraestructure.APIRep.ViewModels;
using Helpers;
using Jint;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using Reports.Domain.ReportViewModels;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.RegularExpressions;

namespace CalibrationSaaS.Reports.Infraestructure.APIRep.Controllers
{
    [Route("/api/print")]
    public class PrintController : Controller
    {
        private readonly ITemplateService _templateService;
        private readonly IWorkOrderDetailRepository _workOrderDetailRepository;
        private readonly IPieceOfEquipmentRepository _pieceOfEquipmentRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBasicsRepository _basicsRepository;
        private readonly IQuoteRepository _quoteRepository;
        private readonly IConfiguration Configuration;
        private readonly IAssetsRepository Assets;

        private readonly BasicsUseCases Basics;
        private readonly WorkOrderDetailUseCase WOD;
        private readonly PieceOfEquipmentUseCases Poe;
        private readonly AssetsUseCases assets;
        private readonly QuoteUseCases Quote;


        public PrintController(ITemplateService templateService, IWorkOrderDetailRepository workOrderDetailRepository,
            IBasicsRepository basicsRepository,
            IConfiguration _Configuration,
            WorkOrderDetailUseCase _WOD,
            IPieceOfEquipmentRepository pieceOfEquipmentRepository,
            IAssetsRepository _Assets,
            PieceOfEquipmentUseCases _Poe,
            IQuoteRepository quoteRepository,
            QuoteUseCases quoteUseCases
            )

        {

            _templateService = templateService ?? throw new ArgumentNullException(nameof(templateService));
            _workOrderDetailRepository = workOrderDetailRepository;
            _basicsRepository = basicsRepository;
            Configuration = _Configuration;
            _pieceOfEquipmentRepository = pieceOfEquipmentRepository;
            WOD = _WOD;
            Assets = _Assets;
            Poe = _Poe;
            _quoteRepository = quoteRepository;
            Quote = quoteUseCases;
        }

        [HttpGet("certified2")]
        public async Task<IActionResult> certified2(int id)
        {

           
            var wo = new WorkOrderDetail();
            wo.WorkOrderDetailID = id;

            string customer = Configuration.GetSection("Reports")["Customer"];
            
            if (customer == "ThermoTemp")
            {
                customer = "ThermoTempBalance";
            }

                var header = await WOD.GetWorkOrderDetailXIdRepWithSave(wo, customer, false);

            if (customer == "Bitterman")
            {

                //return await GeneratePDF(header, "CertifiedBitterman", PaperFormat.Letter);
                return await GeneratePDF(header, "CertifiedBittermanNew", PaperFormat.Letter);
            }
            else if (customer == "ThermoTemp" || customer == "ThermoTempBalance")
            {
             
                return await GeneratePDF(header, "CertifiedBalanceThermotemp", PaperFormat.Letter);

            }
            else
            {
                return await GeneratePDF(header, "Certified", PaperFormat.Letter);
            }




        }

        [HttpGet("certifiedTruck")]
        public async Task<IActionResult> certifiedTruck(int id)
        {

            try
            {
                var wo = new WorkOrderDetail();
                wo.WorkOrderDetailID = id;

                string customer = Configuration.GetSection("Reports")["Customer"];
                var header = await WOD.GetWorkOrderDetailXIdRepWithSave(wo, customer, false);

                var asLeftL = header?.LinearityTruckList?.Where(x => (x.ResultAsLeft.ToUpper() ?? "") == "PASS".ToUpper());
                var asFoundL = header?.LinearityTruckList?.Where(x => (x.ResultAsFound.ToUpper() ?? "") == "PASS".ToUpper());



                if (asFoundL != null && asFoundL.Count() > 0 && asFoundL.Count() == header.LinearityTruckList.Count())
                {
                    header.AsFoundResult = "Pass";
                }
                else
                {
                    header.AsFoundResult = "Fail";
                }

                if (asLeftL != null && asLeftL.Count() > 0 && asLeftL.Count() == header.LinearityTruckList.Count())
                {
                    header.AsLeftResult = "Pass";
                }
                else
                {
                    header.AsLeftResult = "Fail";
                }

                if (customer == "Advance" )
                {
                    return await GeneratePDF(header, "CertifiedAdvanceTruckScale", PaperFormat.Letter);
                }
                else 
                {
                    return await GeneratePDF(header, "CertifiedBittermanTruckScale", PaperFormat.Letter);
                }
               
            }
            catch(Exception ex)
            {
                return null;
            }

        }

        [HttpGet("certifiedThermoTemp")]
        public async Task<IActionResult> certifiedThermoTemp(int id)
        {

            try
            {
                var wo = new WorkOrderDetail();
                wo.WorkOrderDetailID = id;

                string customer = Configuration.GetSection("Reports")["Customer"];
                var header = await WOD.GetWorkOrderDetailXIdRepWithSave(wo, customer, false);

                Header head = new Header();
                head = header;
                head.TestPointsThermoTemps = header.TestPointsThermoTemps;
                head.WeightSetList = header.WeightSetList;

               
                return await GeneratePDF(head, "CertifiedThermoTemp", PaperFormat.Letter);
            }
            catch (Exception ex)
            {
                return null;
            }

        }



        [HttpGet("certifiedGenericAdvance")]
        public async Task<IActionResult> certifiedGenericAdvance(int id)
        {

            try
            {
                var wo = new Domain.Aggregates.Entities.WorkOrder();
                wo.WorkOrderId = id;

                string customer = Configuration.GetSection("Reports")["Customer"];
                var header = await WOD.GetWorkOrderDetailXIdRepGenericAdvance(wo);

                Header head = new Header();
                head = header;
                //head.TestPointsThermoTemps = header.TestPointsThermoTemps;
                //head.WeightSetList = header.WeightSetList;

                if (header.level == 1)
                    return await GeneratePDF(head, "CertifiedGenericAdvanced", PaperFormat.Letter);
                else if (header.level == 2)
                    return await GeneratePDF(head, "CertifiedGenericAdvancedLevel2", PaperFormat.Letter);
                else
                    return null;
               
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [HttpGet("certifiedAdvanceLevel3")]
        public async Task<IActionResult> certifiedAdvanceLevel3(int id)
        {

            var wo = new WorkOrderDetail();
            wo.WorkOrderDetailID = id;

            string customer = Configuration.GetSection("Reports")["Customer"];

            var header = await WOD.GetWorkOrderDetailXIdRepWithSave(wo, customer, false);

            return await GeneratePDF(header, "CertifiedAdvanceLevel3", PaperFormat.Letter);
  
        }


        //[HttpGet("certifiedMaxpro")]
        //public async Task<IActionResult> certifiedThermoTemp(int id)
        //{

        //    try
        //    {
        //        var wo = new WorkOrderDetail();
        //        wo.WorkOrderDetailID = id;

        //        string customer = Configuration.GetSection("Reports")["Customer"];
        //        var header = await WOD.GetWorkOrderDetailXIdRepWithSave(wo, customer, false);

        //        Header head = new Header();
        //        head = header;
        //        head.TestPointsThermoTemps = header.TestPointsThermoTemps;
        //        head.WeightSetList = header.WeightSetList;


        //        return await GeneratePDF(head, "CertifiedThermoTemp", PaperFormat.Letter);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}


        [HttpGet("certifiedsave2")]
        public async Task<IActionResult> certifiedsave2(int id)
        {

            var wo = new WorkOrderDetail();
            wo.WorkOrderDetailID = id;
            var header = new Header();




            return View("Templates/Certified", header);


        }

        [HttpGet("certified")]
        public async Task<IActionResult> certified(int id)
        {

            var wo = new WorkOrderDetail();
            wo.WorkOrderDetailID = id;
            string customer = Configuration.GetSection("Reports")["Customer"];
            var header = await WOD.GetWorkOrderDetailXIdRepWithSave(wo, customer, false);

            return View("Templates/Certified", header);


        }

        [HttpGet("certifiedPDF")]
        public async Task<IActionResult> certifiedPDF(int id)
        {


            var request = HttpContext.Request;

            var host = request.Host.Value;


            if (request.IsHttps)
            {
                host = "https://" + host;
            }
            else
            {
                host = "http://" + host;
            }


            return await SeePdfView(host + "/api/print/certified?id=" + id, "");

        }

        [HttpGet("UrlPDF")]
        public async Task<IActionResult> SeeURLPDF(string url)
        {


            var request = HttpContext.Request;

            var host = request.Host.Value;


            if (request.IsHttps)
            {
                host = "https://" + host;
            }
            else
            {
                host = "http://" + host;
            }


            return await SeePdfView2(url, "");

        }

        [HttpGet("certifiedStr")]
        public async Task<string> certifiedStr(int id)
        {


            var request = HttpContext.Request;

            var host = request.Host.Value;


            if (request.IsHttps)
            {
                host = "https://" + host;
            }
            else
            {
                host = "http://" + host;
            }

            var a = await SeePdfStr(host + "/api/print/certified?id=" + id, "");
            return a;

        }


        [HttpGet("stickerstr")]
        public async Task<string> stickerStr(int id)
        {


            var request = HttpContext.Request;

            var host = request.Host.Value;


            if (request.IsHttps)
            {
                host = "https://" + host;
            }
            else
            {
                host = "http://" + host;
            }


            var a = await SeePdfStr(host + "/api/print/stiker?id=" + id, "");
            return a;

        }


        [HttpGet("certifiedsave")]
        public async Task<string> certifiedSave(int id, string serial)
        {


            var request = HttpContext.Request;

            var host = request.Host.Value;


            if (request.IsHttps)
            {
                host = "https://" + host;
            }
            else
            {
                host = "http://" + host;
            }


            var a = await SeePdf(host + "/api/print/certified2?id=" + id);


            await UploadCertificate(serial, a);


            return ConvertToBase64(a);

        }


        [HttpGet("LTIreport2")]
        public async Task<IActionResult> LTIPDF(int id)
        {


            var request = HttpContext.Request;

            var host = request.Host.Value;


            if (request.IsHttps)
            {
                host = "https://" + host;
            }

            else
            {
                host = "http://" + host;
            }

            return await SeePdfView(host + "/api/print/LTIReport2?id=" + id, "");

        }



        [HttpGet("LTIReport")]
        public async Task<IActionResult> LTIReport(int id)

        {
            try
            {
                WorkOrderDetail w = new WorkOrderDetail();


                w.WorkOrderDetailID = id;
                var wod = await _workOrderDetailRepository.GetByID(w);

                var isAccredited = wod.IsAccredited;

                var weights = await WOD.GetByID(w);
                var force = weights?.BalanceAndScaleCalibration?.Forces;
                var _weigths = weights?.WOD_Weights;

                var _poe = wod?.PieceOfEquipment;
                var _cus = _poe?.Customer;

                var _address = _cus.Aggregates.FirstOrDefault().Addresses.FirstOrDefault();
                Boolean isISO = true;
                ForceViewModel model = new ForceViewModel();

                List<ForceItemViewModel> listItems = new List<ForceItemViewModel>();
                List<ForceItemViewModel> listTensionItems = new List<ForceItemViewModel>();
                List<ForceItemViewModel> listCompressionItems = new List<ForceItemViewModel>();
                List<ForceItemViewModel> listUniversaltems = new List<ForceItemViewModel>();
                List<PieceOfEquipment> poes = new List<PieceOfEquipment>();
                string showUniversal = "";


                var status = await _workOrderDetailRepository.GetStatus();

                var history = await _workOrderDetailRepository.GetHistory(wod);


                var laststatus = status.Where(x => x.IsLast == true).FirstOrDefault();

                var aproveduserid = history.Where(x => laststatus != null && x.StatusId == laststatus.StatusId).OrderByDescending(x => x.WorkDetailHistoryID).FirstOrDefault();


                string tecnameaproved = String.Empty;
                if (aproveduserid != null && aproveduserid.TechnicianID.HasValue && aproveduserid?.UserName != null)
                {
                    // var useraproved = await _basicsRepository.GetUserById2(new User() { UserID = aproveduserid.TechnicianID.Value });
                    //tecnameaproved = useraproved?.Name + " " + useraproved?.LastName;
                    tecnameaproved = aproveduserid?.UserName;
                }
                if (tecnameaproved == "" || tecnameaproved == null)
                    tecnameaproved = "Not set";

                var reviewuserid = history.Where(x => laststatus != null && x.StatusId == (laststatus.StatusId - 1)).OrderByDescending(x => x.WorkDetailHistoryID).FirstOrDefault();

                string tecnreview = String.Empty;
                if (reviewuserid != null && reviewuserid.TechnicianID.HasValue)
                {
                    var tecnreview1 = await _basicsRepository.GetUserById2(new User() { UserID = reviewuserid.TechnicianID.Value });
                    tecnreview =  tecnreview1?.Name + " " + tecnreview1?.LastName;
                }

                if (tecnreview == "" || tecnreview == null)
                    tecnreview = "Not set";

                string? html = "";
                List<StandardHeader> listw = new List<StandardHeader>();



                if (_weigths != null)
                {
                    var testp = _weigths.DistinctBy(x => x.WeightSet.PieceOfEquipmentID);


                    foreach (var item in testp)

                    {
                        double rangeMin = 0;
                        double rangeMax = 0;

                        if (item.WeightSet != null && item.WeightSet.PieceOfEquipmentID != null)
                        {
                            PieceOfEquipment poeSt = new PieceOfEquipment()
                            {
                                PieceOfEquipmentID = item.WeightSet.PieceOfEquipmentID
                            };
                            var range = await Poe.GetPieceOfEquipmentByID(poeSt);
                            if (range.WeightSets != null && range.WeightSets.Count() > 0)
                            {
                                rangeMin = range.WeightSets.Min(x => x.WeightNominalValue);
                                rangeMax = range.WeightSets.Max(x => x.WeightNominalValue2);
                            }

                        }
                        var poetmp = await _pieceOfEquipmentRepository.GetPieceOfEquipmentByIDHeader(item.WeightSet.PieceOfEquipmentID);
                        string uom1;
                        if (poetmp.UnitOfMeasure != null)
                        {
                            uom1 = poetmp.UnitOfMeasure.Abbreviation;
                        }
                        else
                        {
                            uom1 = "NA";
                        }

                        var _listCetificate = await Assets.GetCertificateXPoE(poetmp);


                        CertificatePoE cert = _listCetificate.OrderByDescending(x => x.Version).FirstOrDefault();

                        string certnumber = "";
                        string calibrationProvider = "";
                        if (cert != null)
                        {
                            certnumber = cert.CertificateNumber;
                            calibrationProvider = cert.CalibrationProvider;
                        }

                        var equipmentType = await _basicsRepository.GetEquipmentTypeXId(poetmp.EquipmentTemplate.EquipmentTypeID);
                        if (equipmentType == null && wod.TestCode != null)
                        {
                            equipmentType = await _basicsRepository.GetEquipmentTypeXId(wod.TestCode.CalibrationTypeID);
                            
                        }
                        StandardHeader ww = new StandardHeader()
                        {
                            PoE = poetmp.PieceOfEquipmentID,
                            Serial = poetmp.SerialNumber,
                            Ref = certnumber,
                            CalibrationDueDate = poetmp.DueDate.ToString("MM/dd/yyyy"),
                            Note = poetmp.Notes,
                            CalibrationDate = poetmp.CalibrationDate.ToString("MM/dd/yyyy"),
                            EquipmentType = equipmentType.Name,
                            Capacity = poetmp.Capacity,
                            UnitOfMeasure = uom1,
                            CalibrationProvider = calibrationProvider,
                            MaxRange = rangeMax.ToString(),
                            MinRange = rangeMin.ToString(),
                            Class = poetmp.Class,
                            Manufacturer = poetmp?.EquipmentTemplate?.Manufacturer1?.Name

                        };
                        listw.Add(ww);
                    }


                }

                PieceOfEquipment poeT = new PieceOfEquipment();
                poeT.PieceOfEquipmentID = wod.TemperatureStandardId;
                CertificatePoE cert1 = new CertificatePoE();
                PieceOfEquipment tempStandard = new PieceOfEquipment();
                if (poeT != null && poeT.PieceOfEquipmentID != null)
                {
                    tempStandard = await Poe.GetPieceOfEquipmentByID(poeT);
                    var _listCetificate1 = await Assets.GetCertificateXPoE(tempStandard);
                    cert1 = _listCetificate1.OrderByDescending(x => x.Version).FirstOrDefault();
                }

                string certnumber1 = "";
                string calibrationProvider1 = "";
                if (cert1 != null && cert1.CertificateNumber != null)
                {
                    certnumber1 = cert1.CertificateNumber;
                    calibrationProvider1 = cert1.CalibrationProvider;


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
                if (tempStandard.PieceOfEquipmentID != null)
                {
                    StandardHeader ww1 = new StandardHeader()
                    {
                        PoE = tempStandard.PieceOfEquipmentID,
                        Serial = tempStandard.SerialNumber,
                        Ref = certnumber1,
                        CalibrationDueDate = tempStandard.DueDate.ToString("MM/dd/yyyy"),
                        Note = tempStandard.Notes,
                        EquipmentType = tempStandard.EquipmentTemplate.EquipmentTypeObject.Name,
                        Capacity = tempStandard.Capacity,
                        UnitOfMeasure = uom,
                        CalibrationDate = tempStandard.CalibrationDate.ToString("MM/dd/yyyy"),
                        CalibrationProvider = calibrationProvider1,
                        //MaxRange = rangeMax.ToString(),
                        //MinRange = rangeMin.ToString(),
                        //Class = tempStandard?.Class,
                        //Manufacturer = tempStandard?.EquipmentTemplate?.Manufacturer1?.Name

                    };

                    listw.Add(ww1);

                }

                model.StandardHeaderList = listw;


                if (force != null)
                {
                    var iso = wod.CertificationID;
                    var includeASTM = wod.IncludeASTM;

                    var dto = new ISOandASTM();
                    dto.Forces = force.ToList();
                    dto.WorkOrderDetail = wod;

                    //var forcespre = await Poe.GetCalculatesForISOandASTM(dto);

                    ///////************************************************************
                    ///

                    var gruposForces = dto.Forces.GroupBy(f => f.CalibrationSubTypeId);

                    var results = new List<ICollection<Force>>();

                    // Procesar cada grupo individualmente
                    foreach (var grupo in gruposForces)
                    {
                        var grupoDto = new ISOandASTM
                        {
                            Forces = grupo.ToList(),
                            WorkOrderDetail = dto.WorkOrderDetail
                        };


                        var resultadoGrupo = await Poe.GetCalculatesForISOandASTM(grupoDto);
                         resultadoGrupo = await Poe.CalculateUncertainty(resultadoGrupo.ToList(), (int)iso, wod.Tolerance);

                        results.Add(resultadoGrupo);
                    }


                    var forcespre = results.SelectMany(r => r).ToList();
                    ////// ******************************





                    var forcesNew = new List<Force>();

                    foreach (var item in forcespre)
                    {
                        var clonedJson = JsonConvert.SerializeObject(item);
                        var resultCloned = JsonConvert.DeserializeObject<Force>(clonedJson);

                        if (resultCloned.CalibrationResultContributors != null)
                            resultCloned.CalibrationResultContributors = resultCloned.CalibrationResultContributors.Where(x => x.TypeContributor == "Standard" || x.TypeContributor == "EquipmentTemplate").ToList();

                        forcesNew.Add(resultCloned);
                    }
                    var forcesASTM = await Poe.CalculateUncertASTM(forcesNew.ToList(), (int)iso, wod.Tolerance);

                    var forces = forcesASTM;

                    var forcesTension = forces.Where(x => (x.CalibrationSubTypeId == 4 && iso == 1)).OrderBy(y => y.BasicCalibrationResult.Position);
                    var forcesTensionASTM = forces.Where(x => x.CalibrationSubTypeId == 6).OrderBy(y => y.BasicCalibrationResult.Position);
                    var forcesCompression = forces.Where(x => (x.CalibrationSubTypeId == 5 && iso == 1)).OrderBy(y => y.BasicCalibrationResult.Position);
                    var forcesCompressionASTM = forces.Where(x => x.CalibrationSubTypeId == 7).OrderBy(y => y.BasicCalibrationResult.Position);
                    var forcesUniversal = forces.Where(x => (x.CalibrationSubTypeId == 8 && iso == 1)).OrderBy(y => y.BasicCalibrationResult.Position);
                    var forcesUniversalASTM = forces.Where(x => x.CalibrationSubTypeId == 9).OrderBy(y => y.BasicCalibrationResult.Position);
                    string _class;

                    double adjTension = 0;
                    double adjCompression = 0;
                    double adjUniversal = 0;

                    bool _showCompress = false;
                    int contAsFound = 0;

                    IOrderedEnumerable<Force> tension = forcesTension;
                    if (forcesTension.Count() == 0 && forcesTensionASTM.Count() > 0)
                    {
                        tension = forcesTensionASTM;
                        isISO = false;
                    }
                    adjTension = tension.Sum(x => x.BasicCalibrationResult.RUN2);
                    var compression = forcesCompression;


                    if (forcesCompression.Count() == 0 && forcesTensionASTM.Count() > 0)
                    {

                        compression = forcesCompressionASTM;

                        isISO = false;
                    }
                    adjCompression = compression.Sum(x => x.BasicCalibrationResult.RUN2);
                    var universal = forcesUniversal;


                    if (forcesUniversal.Count() == 0 && forcesUniversalASTM.Count() > 0)
                    {
                        universal = forcesUniversalASTM;

                        isISO = false;
                    }

                    adjUniversal = universal.Sum(x => x.BasicCalibrationResult.RUN2);

                    if (wod.IsUniversal == false)
                    {
                        showUniversal = "hidden";
                        if (tension.Count() > 0)
                        {
                            foreach (var _item in tension)
                            {
                                if (_item.BasicCalibrationResult.Class.ToString() == "0" || _item.BasicCalibrationResult.Class.ToString() == "4")
                                    _class = "NA";
                                else
                                    _class = _item.BasicCalibrationResult.Class.ToString();

                                if (isISO == false)
                                {
                                    if (_item.BasicCalibrationResult.Class.ToString() == "0.5")
                                        _class = "A";
                                    else if (_item.BasicCalibrationResult.Class.ToString() == "1")
                                        _class = "B";
                                    else if (_item.BasicCalibrationResult.Class.ToString() == "2")
                                        _class = "C";
                                    else if (_item.BasicCalibrationResult.Class.ToString() == "3")
                                        _class = "D";
                                    else
                                        _class = "NA";
                                }

                                var errorRun1Per = Math.Round((_item.BasicCalibrationResult.ErrorRun1 / _item.BasicCalibrationResult.Nominal) * 100, 3);

                                if (double.IsNaN(errorRun1Per) || double.IsInfinity(errorRun1Per))
                                    errorRun1Per = 0;

                                var errorRun2Per = Math.Round((_item.BasicCalibrationResult.ErrorRun2 / _item.BasicCalibrationResult.Nominal) * 100, 3);

                                if (double.IsNaN(errorRun2Per) || double.IsInfinity(errorRun2Per))
                                    errorRun2Per = 0;

                                var errorRun3Per = Math.Round((_item.BasicCalibrationResult.ErrorRun3 / _item.BasicCalibrationResult.Nominal) * 100, 3);
                                if (double.IsNaN(errorRun3Per) || double.IsInfinity(errorRun3Per))
                                    errorRun3Per = 0;

                                var errorRun4Per = Math.Round((_item.BasicCalibrationResult.ErrorRun4 / _item.BasicCalibrationResult.Nominal) * 100, 3);
                                if (double.IsNaN(errorRun4Per) || double.IsInfinity(errorRun4Per))
                                    errorRun4Per = 0;

                                double repeatabilityASTM = 0;

                                if (adjTension != 0)
                                {
                                    repeatabilityASTM = Math.Round(errorRun3Per - errorRun2Per, 3);
                                }
                                else
                                {
                                    repeatabilityASTM = Math.Round(errorRun3Per - errorRun1Per, 3);
                                }

                                var passfailASTM = "Pass";
                                if (Math.Abs(repeatabilityASTM) > 1)
                                    passfailASTM = "Fail";
                                else
                                    passfailASTM = _item.BasicCalibrationResult.InToleranceLeftASTM;



                                var uncert = Convert.ToDecimal(_item.Uncertainty);
                                double uncertASTM = 0;
                                if (!double.IsNaN(_item.UncertaintyASTM) && !double.IsInfinity(_item.UncertaintyASTM))
                                    uncertASTM = (double)RoundFirstSignificantDigit(Convert.ToDecimal(_item.UncertaintyASTM));

                                string standardId = null;
                                if (_item.WeightSets != null && _item.WeightSets.Count()>0)
                                {
                                    standardId = _item.WeightSets.FirstOrDefault().PieceOfEquipmentID.ToString();
                                }
                                ////YP RUN with resolution
                                ///

                                var run1 = QueryableExtensions2.Completezero(_item.BasicCalibrationResult.RUN1.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                var run2 = QueryableExtensions2.Completezero(_item.BasicCalibrationResult.RUN2.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                var run3 = QueryableExtensions2.Completezero(_item.BasicCalibrationResult.RUN3.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                var run4 = QueryableExtensions2.Completezero(_item.BasicCalibrationResult.RUN4.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));

                                string ErrorpRun1 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorpRun1).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                string ErrorpRun2 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorpRun2).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                string ErrorpRun3 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorpRun3).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                string ErrorpRun4 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorpRun4).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));

                                string ErrorRun1 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorRun1).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                string ErrorRun2 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorRun2).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                string ErrorRun3 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorRun3).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                string ErrorRun4 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorRun4).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));


                                listTensionItems.Add(
                                        new ForceItemViewModel(_item.BasicCalibrationResult.Nominal,
                                        run1,
                                        run2,
                                        run3,
                                        run4,
                                        _item.BasicCalibrationResult.RelativeIndicationError,
                                       _item.BasicCalibrationResult.RelativeRepeatabilityError,
                                        Math.Abs(_item.BasicCalibrationResult.MaxErrorp),
                                        _class,
                                        _item.BasicCalibrationResult.ClassRun1,
                                        ErrorpRun1,
                                        passfailASTM, 
                                        _item.BasicCalibrationResult.InToleranceFound,
                                        (double)RoundFirstSignificantDigit(uncert),
                                        adjTension,
                                        ErrorpRun2,
                                        ErrorRun1,
                                        ErrorRun2,
                                        ErrorRun3,
                                        ErrorpRun3,
                                        ErrorRun4,
                                        ErrorpRun4,
                                        Math.Abs(repeatabilityASTM),
                                        uncertASTM,
                                        standardId,
                                        (double)RoundFirstSignificantDigit((decimal)_item.BasicCalibrationResult.TUR),
                                        (double)RoundFirstSignificantDigit((decimal)_item.BasicCalibrationResult.TURAstm)
                        
                                ));

                            }
                        }

                        if (compression.Count() > 0)
                        {
                            foreach (var _item in compression)
                            {
                                if (_item.BasicCalibrationResult.RUN1 != 0)
                                {
                                    contAsFound = contAsFound + 1;
                                }
                                if (_item.BasicCalibrationResult.Class.ToString() == "0")
                                    _class = "NA";
                                else
                                    _class = _item.BasicCalibrationResult.Class.ToString();

                                if (isISO == false)
                                {
                                    if (_item.BasicCalibrationResult.Class.ToString() == "0.5")
                                        _class = "A";
                                    else if (_item.BasicCalibrationResult.Class.ToString() == "1")
                                        _class = "B";
                                    else if (_item.BasicCalibrationResult.Class.ToString() == "2")
                                        _class = "C";
                                    else if (_item.BasicCalibrationResult.Class.ToString() == "3")
                                        _class = "D";
                                }

                                var errorRun1Per = Math.Round((_item.BasicCalibrationResult.ErrorRun1 / _item.BasicCalibrationResult.Nominal) * 100, 3);

                                if (double.IsNaN(errorRun1Per) || double.IsInfinity(errorRun1Per))
                                    errorRun1Per = 0;

                                var errorRun2Per = Math.Round((_item.BasicCalibrationResult.ErrorRun2 / _item.BasicCalibrationResult.Nominal) * 100, 3);

                                if (double.IsNaN(errorRun2Per) || double.IsInfinity(errorRun2Per))
                                    errorRun2Per = 0;

                                var errorRun3Per = Math.Round((_item.BasicCalibrationResult.ErrorRun3 / _item.BasicCalibrationResult.Nominal) * 100, 3);
                                if (double.IsNaN(errorRun3Per) || double.IsInfinity(errorRun3Per))
                                    errorRun3Per = 0;

                                var errorRun4Per = Math.Round((_item.BasicCalibrationResult.ErrorRun4 / _item.BasicCalibrationResult.Nominal) * 100, 3);
                                if (double.IsNaN(errorRun4Per) || double.IsInfinity(errorRun4Per))
                                    errorRun4Per = 0;
                                double repeatabilityASTM = 0;
                                if (adjCompression != 0)
                                {
                                    repeatabilityASTM = Math.Round(errorRun3Per - errorRun2Per, 3);
                                }
                                else
                                {
                                    repeatabilityASTM = Math.Round(errorRun3Per - errorRun1Per, 3);
                                }

                                var passfailASTM = "Pass";
                                if (Math.Abs(repeatabilityASTM) > 1)
                                    passfailASTM = "Fail";
                                else
                                    passfailASTM = _item.BasicCalibrationResult.InToleranceLeftASTM;

                                var uncert = Convert.ToDecimal(_item.Uncertainty);
                                double uncertASTM = 0;
                                if (!double.IsNaN(_item.UncertaintyASTM) && !double.IsInfinity(_item.UncertaintyASTM))
                                    uncertASTM = (double)RoundFirstSignificantDigit(Convert.ToDecimal(_item.UncertaintyASTM));

                                string standardId = null;
                                if (_item.WeightSets != null && _item.WeightSets.Count() > 0)
                                {
                                    standardId = _item.WeightSets.FirstOrDefault().PieceOfEquipmentID.ToString();
                                }

                                var run1 = QueryableExtensions2.Completezero(_item.BasicCalibrationResult.RUN1.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                var run2 = QueryableExtensions2.Completezero(_item.BasicCalibrationResult.RUN2.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                var run3 = QueryableExtensions2.Completezero(_item.BasicCalibrationResult.RUN3.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                var run4 = QueryableExtensions2.Completezero(_item.BasicCalibrationResult.RUN4.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));

                                string ErrorpRun1 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorpRun1).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                string ErrorpRun2 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorpRun2).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                string ErrorpRun3 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorpRun3).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                string ErrorpRun4 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorpRun4).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));

                                string ErrorRun1 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorRun1).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                string ErrorRun2 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorRun2).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                string ErrorRun3 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorRun3).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                string ErrorRun4 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorRun4).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));

                                listCompressionItems.Add(
                                        new ForceItemViewModel(_item.BasicCalibrationResult.Nominal,
                                         run1,
                                        run2,
                                        run3,
                                        run4,
                                        _item.BasicCalibrationResult.RelativeIndicationError,
                                        _item.BasicCalibrationResult.RelativeRepeatabilityError,
                                        Math.Abs(_item.BasicCalibrationResult.MaxErrorp),
                                        _class,
                                        _item.BasicCalibrationResult.ClassRun1,
                                        ErrorpRun1,
                                        passfailASTM, //_item.BasicCalibrationResult.InToleranceLeft,
                                        _item.BasicCalibrationResult.InToleranceFound,
                                         (double)RoundFirstSignificantDigit(uncert),
                                        adjCompression,
                                        ErrorpRun2,
                                        ErrorRun1,
                                        ErrorRun2,
                                        ErrorRun3,
                                        ErrorpRun3,
                                        ErrorRun4,
                                       ErrorpRun4,
                                      Math.Abs(repeatabilityASTM),
                                       uncertASTM,
                                       standardId,
                                      (double)RoundFirstSignificantDigit((decimal)_item.BasicCalibrationResult.TUR),
                                      (double)RoundFirstSignificantDigit((decimal)_item.BasicCalibrationResult.TURAstm)
                                    ));

                            }
                        }
                        if (contAsFound > 0)
                            _showCompress = true;
                    }
                    else
                    {

                        if (universal.Count() > 0)
                        {

                            foreach (var _item in universal)
                            {
                                if (_item.BasicCalibrationResult.Class.ToString() == "0")
                                    _class = "NA";
                                else
                                    _class = _item.BasicCalibrationResult.Class.ToString();

                                if (isISO == false)
                                {
                                    if (_item.BasicCalibrationResult.Class.ToString() == "0.5")
                                        _class = "A";
                                    else if (_item.BasicCalibrationResult.Class.ToString() == "1")
                                        _class = "B";
                                    else if (_item.BasicCalibrationResult.Class.ToString() == "2")
                                        _class = "C";
                                    else if (_item.BasicCalibrationResult.Class.ToString() == "3")
                                        _class = "D";
                                }

                                var errorRun1Per = Math.Round((_item.BasicCalibrationResult.ErrorRun1 / _item.BasicCalibrationResult.Nominal) * 100, 3);

                                if (double.IsNaN(errorRun1Per) || double.IsInfinity(errorRun1Per))
                                    errorRun1Per = 0;

                                var errorRun2Per = Math.Round((_item.BasicCalibrationResult.ErrorRun2 / _item.BasicCalibrationResult.Nominal) * 100, 3);

                                if (double.IsNaN(errorRun2Per) || double.IsInfinity(errorRun2Per))
                                    errorRun2Per = 0;

                                var errorRun3Per = Math.Round((_item.BasicCalibrationResult.ErrorRun3 / _item.BasicCalibrationResult.Nominal) * 100, 3);
                                if (double.IsNaN(errorRun3Per) || double.IsInfinity(errorRun3Per))
                                    errorRun3Per = 0;

                                var errorRun4Per = Math.Round((_item.BasicCalibrationResult.ErrorRun4 / _item.BasicCalibrationResult.Nominal) * 100, 3);
                                if (double.IsNaN(errorRun4Per) || double.IsInfinity(errorRun4Per))
                                    errorRun4Per = 0;

                                double repeatabilityASTM = 0;
                                if (adjUniversal != 0)
                                {
                                    repeatabilityASTM = Math.Round(errorRun3Per - errorRun2Per, 3);
                                }
                                else
                                {
                                    repeatabilityASTM = Math.Round(errorRun3Per - errorRun1Per, 3);
                                }

                                var passfailASTM = "Pass";
                                if (Math.Abs(repeatabilityASTM) > 1)
                                    passfailASTM = "Fail";
                                else
                                    passfailASTM = _item.BasicCalibrationResult.InToleranceLeftASTM;
                                var uncert = Convert.ToDecimal(_item.Uncertainty);
                                double uncertASTM = 0;
                                if (!double.IsNaN(_item.UncertaintyASTM) && !double.IsInfinity(_item.UncertaintyASTM))
                                    uncertASTM = (double)RoundFirstSignificantDigit(Convert.ToDecimal(_item.UncertaintyASTM));

                                string standardId = null;
                                if (_item.WeightSets != null && _item.WeightSets.Count() > 0)
                                {
                                    standardId = _item.WeightSets.FirstOrDefault().PieceOfEquipmentID.ToString();
                                }

                                var run1 = QueryableExtensions2.Completezero(_item.BasicCalibrationResult.RUN1.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                var run2 = QueryableExtensions2.Completezero(_item.BasicCalibrationResult.RUN2.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                var run3 = QueryableExtensions2.Completezero(_item.BasicCalibrationResult.RUN3.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                var run4 = QueryableExtensions2.Completezero(_item.BasicCalibrationResult.RUN4.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));

                                string ErrorpRun1 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorpRun1).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                string ErrorpRun2 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorpRun2).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                string ErrorpRun3 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorpRun3).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                string ErrorpRun4 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorpRun4).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));

                                string ErrorRun1 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorRun1).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                string ErrorRun2 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorRun2).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                string ErrorRun3 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorRun3).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));
                                string ErrorRun4 = QueryableExtensions2.Completezero(Math.Abs(_item.BasicCalibrationResult.ErrorRun4).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(wod.Resolution)));


                                listUniversaltems.Add(
                                    new ForceItemViewModel(_item.BasicCalibrationResult.Nominal,
                                    run1,
                                    run2,
                                    run3,
                                    run4,
                                    _item.BasicCalibrationResult.RelativeIndicationError,
                                    _item.BasicCalibrationResult.RelativeRepeatabilityError,
                                    Math.Abs(_item.BasicCalibrationResult.MaxErrorp),
                                    _class,
                                    _item.BasicCalibrationResult.ClassRun1,
                                     ErrorpRun1,
                                     passfailASTM,// _item.BasicCalibrationResult.InToleranceLeft,
                                    _item.BasicCalibrationResult.InToleranceFound,
                                     (double)RoundFirstSignificantDigit(uncert),
                                    adjUniversal,
                                    ErrorpRun2,
                                    ErrorRun1,
                                    ErrorRun2,
                                    ErrorRun3,
                                    ErrorpRun3,
                                    ErrorRun4,
                                    ErrorpRun4,
                                    Math.Abs(repeatabilityASTM),
                                    uncertASTM,
                                    standardId,
                                    (double)RoundFirstSignificantDigit((decimal)_item.BasicCalibrationResult.TUR),
                                    (double)RoundFirstSignificantDigit((decimal)_item.BasicCalibrationResult.TURAstm)
                               ));

                            }

                        }
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

                    model = new ForceViewModel
                    {

                        CreatedAt = calibrationDate,
                        Due = due,
                        Id = wod.WorkOrderDetailID,
                        Address = "Test Address",
                        City = "Test City",
                        Country = "Test Country",
                        Serial = wod.PieceOfEquipment.SerialNumber,
                        Manufaturer = wod.PieceOfEquipment.EquipmentTemplate.Manufacturer1.Name,
                        Model = wod.PieceOfEquipment.EquipmentTemplate.Model,
                        CompanyName = "Test Company",
                        Sign = tecnameaproved.ToUpper(),
                        Items = listItems,
                        Tension = listTensionItems,
                        Compression = listCompressionItems,
                        Universal = listUniversaltems,
                        pieceOfEquipment = _poe,
                        customer = _cus,
                        address = _address,
                        weights = _weigths,
                        ShowUniversal = showUniversal,
                        StandardHeaderList = listw,
                        IncludeASTM = includeASTM,
                        Accredited = (bool)isAccredited,
                        TemperatureInit = weights.Temperature.ToString(),
                        TemperatureEnd = weights.TemperatureAfter.ToString(),
                        ShowCompress = _showCompress,
                        CertificateComment = weights.CertificateComment,
                        Procedure = wod.TestCode.Procedure != null ? wod.TestCode.Procedure.Name : "NA",
                        NoteViewModel = noteViewModel,
                        Capacity = wod.PieceOfEquipment.Capacity,
                        UoM = wod.PieceOfEquipment.UnitOfMeasure.Abbreviation,
                        Resolution = wod.Resolution,
                        Humidity = weights.Humidity.ToString() + " %RH",
                        Unit = _poe.PieceOfEquipmentID,
                        CustomerRef = _poe.CustomerToolId,
                        InstallLocation = _poe.InstallLocation,
                        ReceivedCondition = received,
                        ReturnedCondition = returned,
                        CertificateNumber = wod.WorkOrderID + " - " + _poe.SerialNumber + "- 1" ,
                        Sign2 = tecnreview.ToUpper()

                    };


                    if (isISO == true)
                    {
                        html = await _templateService.RenderAsync("Templates/ISOLTITemplate", model);
                    }
                    else
                    {
                        html = await _templateService.RenderAsync("Templates/ASTMLTITemplate", model);
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

          
                await using var page = await browser.NewPageAsync();

                await page.EmulateMediaTypeAsync(MediaType.Screen);
                await page.SetContentAsync(html);

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
                        Top = "65px",
                        Bottom = "65px"
                    }
                });



                return File(pdfContent, "application/pdf");

            }
            catch (Exception ex)
            {

                return null;
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<NoteViewModel> GetNotes(PieceOfEquipment? _poe, WorkOrderDetail? wod)
        {

            List<Note> NotesEqTc = new List<Note>();
            List<Note> notesEqType = new List<Note>();

            var eqType = await _basicsRepository.GetEquipmentTypeXId(_poe.EquipmentTemplate.EquipmentTypeID);
            if (eqType != null)
            {
                notesEqType = eqType.Notes.OrderBy(x => x.EquipmnetTypeId != null).ThenBy(z => z.Position).ToList();
            }


            List<Note> notesTestCode = new List<Note>();
            var testCode = await _workOrderDetailRepository.GetTestCodeByID((int)wod.TestCodeID);
            if (testCode != null && testCode.Notes != null)
            {
                notesTestCode = testCode.Notes.OrderBy(y => y.TestCodeID != null).ToList();

            }
            NotesEqTc = notesEqType.Concat(notesTestCode).ToList();


            List<Note> notesCondition = new List<Note>();
            foreach (var note in NotesEqTc)
            {
                //// Dynamic Statements
                string noteText = "";
                bool validation = true;
                if (note.Validation != null)
                {
                    validation = await DynamicValidationNote(wod, note);
                }

                if (note.Text.Contains("{"))
                {
                    noteText = await DynamicTextNote(wod, note);
                }

                if (validation && noteText != null && noteText != "")
                {
                    note.Text = noteText;

                }

                ////

                if (note.Condition == 1)
                    notesCondition.Add(note);
                else if (wod.IsAccredited == true && note.Condition == 4)
                    notesCondition.Add(note);
                else if (wod.IsAccredited == false && note.Condition == 5)
                    notesCondition.Add(note);

            }


            List<NoteWOD> notesWOD = new List<NoteWOD>();
            var notesWOD1 = await _workOrderDetailRepository.GetNotes(wod.WorkOrderDetailID, 1);
            notesWOD = notesWOD1.OrderBy(x => x.Position).ToList();

            NoteViewModel noteViewModel = new NoteViewModel();
            noteViewModel.NotesList = notesCondition;
            noteViewModel.NotesWODList = notesWOD;

            return noteViewModel;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<string> DynamicTextNote(WorkOrderDetail Model, Note note)
        {
            string inputString = note.Text;

            // Utilizar expresión regular para buscar los marcadores de posición entre {}
            var regex = new Regex("{(.*?)}");
            var matches = regex.Matches(inputString);

            // Reemplazar los marcadores de posición con los valores correspondientes
            foreach (Match match in matches)
            {
                string placeholder = match.Groups[1].Value;
                string propertyValue = GetPropertyValue(Model, placeholder); // Obtener el valor de la propiedad
                inputString = inputString.Replace(match.Value, propertyValue);
            }

            return inputString;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<bool> DynamicValidationNote(WorkOrderDetail Model, Note note)
        {
            string field = "";
            bool res = false;

            try
            {
                var engine2 = new Engine();
                engine2.SetValue("model", Model);



                if (!string.IsNullOrEmpty(note.Validation))
                {
                    field = field + "  error in validationformula  ";

                    var obj = engine2.Evaluate(note.Validation).ToObject();

                    note.ValidationResult = obj;

                    try
                    {
                        res = Convert.ToBoolean(obj);


                    }
                    catch (Exception ex)
                    {
//                        Console.WriteLine("no boolean");
                    }



                }

                return res;


            }
            catch (Exception ex)
            {

                return false;

            }


        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private string GetPropertyValue(WorkOrderDetail Model, string propertyName)
        {
            var property = Model.GetType().GetProperty(propertyName);
            if (property != null)
            {
                var value = property.GetValue(Model);
                if (value != null)
                {
                    return value.ToString();
                }
            }
            return "";
        }

        [HttpGet("Print")]
        public async Task<IActionResult> Print(int id)
        {



            var wo = new WorkOrderDetail();
            wo.WorkOrderDetailID = id;
            var workOrderDetail1 = await _workOrderDetailRepository.GetByID(wo);

            var workOrder = workOrderDetail1.WorkOder;
            var customer = workOrder.Customer;

            var poe = workOrderDetail1.PieceOfEquipment;

            var eqTemp = poe.EquipmentTemplate;

            string manufName;
            if (eqTemp.Manufacturer1 != null)
            {
                manufName = eqTemp.Manufacturer1.Name;
            }
            else
            {
                manufName = null;
            }



            var url = Configuration.GetSection("Reports")["UrlCertificates"] + poe.SerialNumber.GetSHA1Has() + ".pdf?sv=" + Configuration.GetSection("Reports")["sv"];

            var urlbase = Configuration.GetSection("Reports")["Url"];

            var address = customer?.Aggregates?.FirstOrDefault().Addresses.FirstOrDefault();

            if (workOrderDetail1.CalibrationCustomDueDate == null)
            {
                throw new Exception("Failed Precondition: Due Date");
            }

            Sticker sticker = new Sticker()
            {
                Model = eqTemp.Model,
                Serial = poe.SerialNumber,
                CalibrationDate = workOrderDetail1.CalibrationDate.Value.ToString("MM/dd/yyyy"),
                CalibrationDue = workOrderDetail1.CalibrationCustomDueDate.Value.ToString("MM/dd/yyyy"),
                Tech = workOrderDetail1.Technician.Name + " " + workOrderDetail1.Technician.LastName,
                Url = url,
                ImageStr = GenerateQR(url)

            };


            //return View("Templates/Sticker",sticker);

            return await GeneratePDF(sticker, "Sticker", PaperFormat.A6);


        }

        [HttpGet("Sticker")]
        public async Task<IActionResult> Sticker(int id)
        {
            var wo = new WorkOrderDetail();
            wo.WorkOrderDetailID = id;
            var workOrderDetail1 = await _workOrderDetailRepository.GetByID(wo);


            var workOrder = workOrderDetail1.WorkOder;
            var customer = workOrder.Customer;

            var poe = workOrderDetail1.PieceOfEquipment;
            var eqTemp = poe.EquipmentTemplate;


            string manufName;
            if (eqTemp.Manufacturer1 != null)
            {
                manufName = eqTemp.Manufacturer1.Name;
            }
            else
            {
                manufName = null;
            }



            var url = Configuration.GetSection("Reports")["UrlCertificates"] + poe.SerialNumber.GetSHA1Has() + ".pdf?sv=" + Configuration.GetSection("Reports")["sv"];

            var urlbase = Configuration.GetSection("Reports")["Url"];

            var address = customer?.Aggregates?.FirstOrDefault().Addresses.FirstOrDefault();

            if (workOrderDetail1.CalibrationCustomDueDate == null)
            {
                throw new Exception("Failed Precondition: Due Date");
            }

            Sticker sticker = new Sticker()
            {
                Model = eqTemp.Model,
                Serial = poe.SerialNumber,
                CalibrationDate = workOrderDetail1.CalibrationDate.Value.ToString("MM/dd/yyyy"),
                CalibrationDue = workOrderDetail1.CalibrationCustomDueDate.Value.ToString("MM/dd/yyyy"),
                Tech = workOrderDetail1.Technician.Name + " " + workOrderDetail1.Technician.LastName,
                Url = url,
                ImageStr = GenerateQR(url)

            };


            return View("Templates/Sticker", sticker);




        }

        [HttpGet("quote")]
        public async Task<IActionResult> QuoteReport(int id)
        {
            try
            {
//                Console.WriteLine($"QuoteReport: Starting report generation for quote ID {id}");

                var quote = new Domain.Aggregates.Entities.Quote { QuoteID = id };
                var quoteData = await Quote.GetQuoteByID(quote);

                if (quoteData == null)
                {
//                    Console.WriteLine($"QuoteReport: Quote with ID {id} not found");
                    return NotFound($"Quote with ID {id} not found");
                }

//                Console.WriteLine($"QuoteReport: Quote found - {quoteData.QuoteNumber}, Customer: {quoteData.Customer?.Name}, Items: {quoteData.QuoteItems?.Count ?? 0}");

                var viewModel = BuildQuoteReportViewModel(quoteData);

                // Set the base URL for image references
                var request = HttpContext.Request;
                var baseUrl = $"{request.Scheme}://{request.Host}";
                viewModel.BaseUrl = baseUrl;

//                Console.WriteLine($"QuoteReport: ViewModel built successfully with BaseUrl: {baseUrl}");

                return View("~/Views/Templates/ServiceQuotation.cshtml", viewModel);
            }
            catch (Exception ex)
            {
//                Console.WriteLine($"QuoteReport Error: {ex.Message}");
//                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return StatusCode(500, $"Error generating quote report: {ex.Message}");
            }
        }

        [HttpGet("quotePDF")]
        public async Task<IActionResult> QuoteReportPDF(int id)
        {
            try
            {
                var request = HttpContext.Request;
                var host = request.Host.Value;

                if (request.IsHttps)
                {
                    host = "https://" + host;
                }
                else
                {
                    host = "http://" + host;
                }

                return await SeePdfView(host + "/api/print/quote?id=" + id, "");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generating quote PDF: {ex.Message}");
            }
        }

        [HttpGet("quoteStr")]
        public async Task<string> QuoteReportStr(int id)
        {
            try
            {
                var request = HttpContext.Request;
                var host = request.Host.Value;

                if (request.IsHttps)
                {
                    host = "https://" + host;
                }
                else
                {
                    host = "http://" + host;
                }

                var result = await SeePdfStr(host + "/api/print/quote?id=" + id, "");

                // Validate that the result is a valid Base64 string
                if (string.IsNullOrEmpty(result))
                {
                    throw new Exception("PDF generation returned empty result");
                }

                // Try to validate Base64 format
                try
                {
                    Convert.FromBase64String(result);
                }
                catch (FormatException)
                {
                    throw new Exception($"PDF generation failed - invalid Base64 format. Result: {result.Substring(0, Math.Min(100, result.Length))}...");
                }

                return result;
            }
            catch (Exception ex)
            {
                // Log the full error for debugging
//                Console.WriteLine($"QuoteReportStr Error: {ex.Message}");
//                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                // Return a proper error response instead of invalid Base64
                throw new Exception($"Error generating quote report: {ex.Message}");
            }
        }

        private ViewModels.QuoteReportViewModel BuildQuoteReportViewModel(Domain.Aggregates.Entities.Quote quote)
        {
            var viewModel = new ViewModels.QuoteReportViewModel
            {
                QuoteNumber = quote.QuoteNumber,
                QuoteDate = quote.CreatedDate,
                Subject = "Service Request"
            };

            // Get customer information
            if (quote.Customer != null)
            {
                viewModel.Customer.Name = quote.Customer.Name;

                // Get customer contact information from aggregates
                var customerAggregate = quote.Customer.Aggregates?.FirstOrDefault();
                if (customerAggregate != null)
                {
                    // Get contact person
                    var contact = customerAggregate.Contacts?.FirstOrDefault();
                    if (contact != null)
                    {
                        viewModel.Customer.ContactPerson = $"{contact.Name} {contact.LastName}".Trim();
                    }
                    else
                    {
                        viewModel.Customer.ContactPerson = quote.Customer.Name;
                    }

                    // Get address
                    if (customerAggregate.Addresses?.Any() == true)
                    {
                        var address = customerAggregate.Addresses.First();
                        viewModel.Customer.Address = address.StreetAddress1;
                        viewModel.Customer.City = address.City;
                        viewModel.Customer.State = address.State;
                        viewModel.Customer.ZipCode = address.ZipCode;
                    }

                    // Get phone number
                    var phoneNumber = customerAggregate.PhoneNumbers?.FirstOrDefault();
                    if (phoneNumber != null)
                    {
                        viewModel.Customer.Phone = phoneNumber.Number;
                    }

                    // Get email address
                    var emailAddress = customerAggregate.EmailAddresses?.FirstOrDefault();
                    if (emailAddress != null)
                    {
                        viewModel.Customer.Email = emailAddress.Address;
                    }
                }
                else
                {
                    viewModel.Customer.ContactPerson = quote.Customer.Name;
                }
            }

            // Get quote items
            if (quote.QuoteItems?.Any() == true)
            {
                foreach (var item in quote.QuoteItems)
                {
                    var quoteItemInfo = new ViewModels.QuoteItemInfo
                    {
                        Quantity = item.Quantity,
                        Description = BuildItemDescription(item),
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice
                    };

                    if (item.PieceOfEquipment != null)
                    {
                        quoteItemInfo.SerialNumber = item.PieceOfEquipment.SerialNumber;
                        quoteItemInfo.EquipmentType = item.PieceOfEquipment.EquipmentTemplate?.Name;
                    }

                    viewModel.Items.Add(quoteItemInfo);
                }
            }

            // Calculate totals
            viewModel.SubTotal = viewModel.Items.Sum(i => i.TotalPrice);
            viewModel.TotalAmount = viewModel.SubTotal; // Add tax calculation if needed
            viewModel.TaxAmount = 0; // Implement tax calculation if needed

            return viewModel;
        }

        private string BuildItemDescription(Domain.Aggregates.Entities.QuoteItem item)
        {
            var description = new List<string>();

            // Use ItemDescription if available
            if (!string.IsNullOrEmpty(item.ItemDescription))
            {
                return item.ItemDescription;
            }

            // Build description from equipment template
            if (item.PieceOfEquipment?.EquipmentTemplate != null)
            {
                description.Add($"Calibrate for {item.PieceOfEquipment.EquipmentTemplate.Name}");
            }

            if (description.Count == 0)
            {
                description.Add("Service/Calibration");
            }

            return string.Join(" ", description);
        }


        public async Task<IActionResult> GeneratePDF<T>(T model, string Template, PaperFormat format)
        {
            string? html = "";

            html = await _templateService.RenderAsync("Print/Templates/" + Template, model);

           
            await using var browser = await Puppeteer.ConnectAsync(new ConnectOptions
            {
                BrowserWSEndpoint = "wss://chrome.browserless.io?token=cae21211-c2d1-4325-9f79-70fecc54826c"
            });
            await using var page = await browser.NewPageAsync();

            await page.EmulateMediaTypeAsync(MediaType.Screen);
            await page.SetContentAsync(html);

            //var pdfContent = await page.PdfStreamAsync(new PdfOptions
            //{
            //    Format = PaperFormat.Letter,
            //    DisplayHeaderFooter = true,
            //    Landscape = false,
            //    PrintBackground = true,



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
                    Top = "10px",
                    Bottom = "20px"
                }
            });

            return File(pdfContent, "application/pdf");

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string GenerateQR(string stickerDS)
        {



            QRCoder.QRCodeGenerator QR = new QRCoder.QRCodeGenerator();
            ASCIIEncoding ASSCII = new ASCIIEncoding();
            var z = QR.CreateQrCode(ASSCII.GetBytes(stickerDS), QRCoder.QRCodeGenerator.ECCLevel.Q);
            QRCoder.PngByteQRCode png = new QRCoder.PngByteQRCode();
            png.SetQRCodeData(z);
            var arr = png.GetGraphic(5);
            MemoryStream ms = new MemoryStream();
            ms.Write(arr, 0, arr.Length);

            var s = Convert.ToBase64String(arr);

            return "data:image/png;base64, " + s;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task SavePdf(string url, string Name)
        {


            var pdfcontent = await SeePdf(url);

            await UploadCertificate(Name, pdfcontent);



        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> SeePdfView2(string url, string Name)
        {

            var pdfcontent = await SeePdf2(url);

            return File(pdfcontent, "application/pdf");


        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> SeePdfView(string url, string Name)
        {

            var pdfcontent = await SeePdf(url);

            return File(pdfcontent, "application/pdf");


        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<string> SeePdfStr(string url, string Name)
        {

            var pdfcontent = await SeePdf(url);

            return ConvertToBase64(pdfcontent);


        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<Stream> SeePdf(string url)
        {
            try
            {

                //string path = @"C:\Program Files\Google\Chrome\Application\chrome.exe";

                var p = PuppeteerExtensions.ExecutablePath;

                using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions()
                {
                    Headless = true,
                    ExecutablePath = p//PuppeteerExtensions.ExecutablePath
                }))
                {
                    using (var page = await browser.NewPageAsync())
                    {


                        List<WaitUntilNavigation> wai = new List<WaitUntilNavigation>();
                        wai.Add(WaitUntilNavigation.DOMContentLoaded);
                        NavigationOptions OP = new NavigationOptions();
                        OP.WaitUntil = wai.ToArray();
                        OP.Timeout = 59000000;


                        await page.GoToAsync(url, OP);

                        WaitForSelectorOptions wf = new WaitForSelectorOptions();

                        wf.Timeout = 9000000;

                       

                        await page.EmulateMediaTypeAsync(MediaType.Screen);

                        await page.SetRequestInterceptionAsync(true);

                        Dictionary<string, string> dic = new Dictionary<string, string>();

                        dic.Add("Accept-Language", "en-US,en;q=0.9");

                        await page.SetExtraHttpHeadersAsync(dic);

                        await page.SetUserAgentAsync("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36");


                        //await page.WaitForNavigationAsync();//page.WaitForTimeoutAsync(1500);


                        var pdfcontent = await page.PdfStreamAsync(new PdfOptions()
                        {
                            PrintBackground = true,

                            Landscape = true,

                            Format = PuppeteerSharp.Media.PaperFormat.Legal,

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
        public async Task<Stream> SeePdf2(string url)
        {
            try
            {

                string path = PuppeteerExtensions.ExecutablePath; 
                try
                {
                    if (!string.IsNullOrEmpty(Configuration.GetSection("Reports")["Path"]))
                    {
                        path = Configuration.GetSection("Reports")["Path"];
                    }
                }
                catch (Exception ex)
                {

                }
               
                //string path = @"C:\Program Files\Google\Chrome\Application\chrome.exe";

               

                using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions()
                {
                    Headless = true,
                    ExecutablePath = path//PuppeteerExtensions.ExecutablePath
                }))
                {
                    using (var page = await browser.NewPageAsync())
                    {


                        List<WaitUntilNavigation> wai = new List<WaitUntilNavigation>();
                        wai.Add(WaitUntilNavigation.DOMContentLoaded);
                        NavigationOptions OP = new NavigationOptions();
                        OP.WaitUntil = wai.ToArray();
                        OP.Timeout = 59000000;


                        await page.GoToAsync(url, OP);

                        WaitForSelectorOptions wf = new WaitForSelectorOptions();

                        wf.Timeout = 9000000;

                        var a0 = await page.WaitForSelectorAsync("#wrapper");                        

                        var a1 = await page.WaitForSelectorAsync(".container-fluid", wf);

                        var a2 = await page.WaitForSelectorAsync("div.FullContainer", wf);

                        await page.WaitForXPathAsync("/html/body/app/div/div[1]", wf);

                        await page.WaitForXPathAsync("/html/body/app/div/div[1]/div", wf);

                        var a3 = await page.WaitForSelectorAsync("#paginationTotal", wf);

                        wf.Hidden = true;

                        await page.WaitForSelectorAsync("#progressBar", wf);



                        await page.EmulateMediaTypeAsync(MediaType.Screen);

                        await page.SetRequestInterceptionAsync(true);

                        Dictionary<string, string> dic = new Dictionary<string, string>();

                        dic.Add("Accept-Language", "en-US,en;q=0.9");

                        await page.SetExtraHttpHeadersAsync(dic);

                        await page.SetUserAgentAsync("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36");


                        //await page.WaitForNavigationAsync();//page.WaitForTimeoutAsync(1500);


                        var pdfcontent = await page.PdfStreamAsync(new PdfOptions()
                        {
                            PrintBackground = true,

                            Landscape = true,

                            Format = PuppeteerSharp.Media.PaperFormat.Legal,

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
        public async Task UploadCertificate(string serial, Stream pdf)
        {
            try
            {
                var serialHash = serial.GetSHA1Has();
                MemoryStream streams = new MemoryStream();
                var path = AppDomain.CurrentDomain.BaseDirectory;



                path = path + @"wwwroot\Certificate";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);

                }
                else
                {

                }


                using (System.IO.FileStream stream = new FileStream(path + @"\" + serialHash + ".pdf", FileMode.OpenOrCreate))
                {
                    System.IO.BinaryWriter writer = new BinaryWriter(stream);

                    MemoryStream ms1 = new MemoryStream();
                    pdf.CopyTo(ms1);
                    var byt = ms1.ToArray();



                    writer.Write(byt, 0, byt.Length);
                    writer.Close();

                }


                String strorageconn = ConfigurationExtensions.GetConnectionString(this.Configuration, "fileShareConnectionString");
                CloudStorageAccount storageacc = CloudStorageAccount.Parse(strorageconn);


                CloudBlobClient blobClient = storageacc.CreateCloudBlobClient();


                CloudBlobContainer container = blobClient.GetContainerReference("certificates");

                await container.CreateIfNotExistsAsync();






                CloudBlockBlob blockBlob = container.GetBlockBlobReference(serialHash + ".pdf");



                await blockBlob.UploadFromStreamAsync(pdf);




            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<Stream> GeneratePDF2<T>(T model, string Template, PaperFormat format)
        {
            string? html = "";

            html = await _templateService.RenderAsync("Print/Templates/" + Template, model);




            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,

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
            return pdfContent;

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string ConvertToBase64(Stream stream)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            string base64 = Convert.ToBase64String(bytes);
            return base64;

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public static decimal RoundFirstSignificantDigit(decimal numberInit)
        {
            try
            {

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
                var kk = Math.Floor(mm + 1);
                ints = (long)Math.Abs(numberInit);
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
                var ssub = auxchar.ToString().Substring(0, precision);
                return Convert.ToDecimal(numberEnd);
            }
            catch (Exception ex)
            {
                return Math.Round(numberInit, 2);
            }
        }
    }
}