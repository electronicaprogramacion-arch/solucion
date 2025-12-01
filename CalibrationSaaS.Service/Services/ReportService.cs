using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Application.UseCases;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Grpc.Helpers;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Reports.Domain.ReportViewModels;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using CalibrationSaaS.Domain.Repositories;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System.Text.Json;
using CalibrationSaaS.Domain.BusinessExceptions;

namespace CalibrationSaaS.Infraestructure.GrpcServices.Services
{
    public class ReportService : IReportService<CallContext>
    {
        private readonly SampleUseCases LogicSample;
        //   private readonly IWorkOrderDetailRepository wodRepository;

        private readonly UOMUseCases LogicUOM;
        private readonly WorkOrderDetailUseCase LogicWod;
        private readonly PieceOfEquipmentUseCases Logicpoe;
        private readonly ValidatorHelper modelValidator;
        private readonly ILogger logger;
        private readonly IConfiguration Configuration;
        private readonly BasicsUseCases Basics;
        public System.Net.Http.HttpClient Http { get; set; }

        public ReportService(UOMUseCases _LogicUOM,  SampleUseCases _LogicSample, WorkOrderDetailUseCase _LogicWod, PieceOfEquipmentUseCases _Logicpoe, ILogger<ReportService> _logger, ValidatorHelper _modelValidator, IConfiguration _Configuration,BasicsUseCases basics)
        {

           // Logic = _Logic;
            _LogicSample = LogicSample;
            logger = _logger;
            modelValidator = _modelValidator;
            this.LogicWod = _LogicWod;
            this.Logicpoe = _Logicpoe;
            this.LogicUOM = _LogicUOM;
            Configuration = _Configuration;
            Basics = basics;

        }



        //public async ValueTask<string> GetWorkOrderDetailXIdRep(WorkOrderDetail wo, CallContext context)
        //{
        //    var workOrderDetail = await LogicSample.GetWorkOrderDetailXIdRep(wo.WorkOrderDetailID);

        //    return "ok";
        //}

        public async ValueTask<string> GetSticker(WorkOrderDetail wo, CallContext context)
        {
            //var workOrderDetail = await LogicSample.GetWorkOrderDetailXIdRep(wo.WorkOrderDetailID);
            //wo.WorkOrderDetailID = 2;

            var workOrderDetail1 = await LogicWod.GetByID(wo);
            //var workOrderDetail = await LogicWod.GetWorkOrderDetailByID(wo);

            var workOrder = workOrderDetail1.WorkOder;
            var customer = workOrder.Customer;
            //var address = customer.Aggregates[0];
            var poe = workOrderDetail1.PieceOfEquipment;  // customer.PieceOfEquipment.Where(x => x.WorOrderDetailID == wo.WorkOrderDetailID).FirstOrDefault();
            var poes = await Logicpoe.GetAllWeightSets(poe);
            var eqTemp = poe.EquipmentTemplate;
            var weigthSets = poes.FirstOrDefault().WeightSets;
            //var manu = eqTemp.Manufacturer;
            string manufName;
            if (eqTemp.Manufacturer1 != null)
            {
                manufName = eqTemp.Manufacturer1.Name;
            }
            else
            {
                manufName = null;
            }


            Http = new HttpClient();
            //var url = "https://landeve.blob.core.windows.net/certificates/" + GetSHA1Has(poe.SerialNumber) + ".pdf?sv=2019-12-12&ss=bf&srt=so&sp=rwdlacx&se=2021-12-09T22:30:30Z&st=2020-12-09T14:30:30Z&spr=https,http&sig=NXHNSROwgMi%2BRepLzQJHVJIVCcBoQZCeW0F4216nugI%3D"; //Configuration.GetSection("Reports")["Url"];
            var url = Configuration.GetSection("Reports")["UrlCertificates"] + GetSHA1Has(poe.SerialNumber) + ".pdf?sv=" + Configuration.GetSection("Reports")["sv"];
            Http.BaseAddress = new System.Uri(Configuration.GetSection("Reports")["Url"]);

            var address = customer.Aggregates.FirstOrDefault().Addresses.FirstOrDefault();

            if(workOrderDetail1.CalibrationCustomDueDate == null)
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
                Url = url

            };

            var jsonSticker = JsonConvert.SerializeObject(sticker);

            //Handle TLS protocols
            System.Net.ServicePointManager.SecurityProtocol =
                System.Net.SecurityProtocolType.Tls
                | System.Net.SecurityProtocolType.Tls11
                | System.Net.SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };


            var content = new StringContent(jsonSticker, System.Text.Encoding.UTF8, "application/json");


            Http.DefaultRequestHeaders.Accept.Clear();
            Http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage pdf = null;


            // pdf = await Http.PostAsync("/GetCustomerCover", content);
            pdf = await Http.PostAsync("/GetSticker", content);


            var contentReponse = await pdf.Content.ReadAsStringAsync();

            //Console.WriteLine("contentReponse " + contentReponse);

            var psdInBase64 = JsonConvert.DeserializeObject(contentReponse).ToString();
            ////b64 = psdInBase64;

            //Console.WriteLine("psdInBase  " + psdInBase64);


            return psdInBase64;//psdInBase64;
        }


        public async ValueTask<ReportResultSet> GetWorkOrderDetailXIdRepWithSave(WorkOrderDetail wo)
        {


                var status=await LogicWod.GetStatus();

                var history = await LogicWod.GetHistory(wo);


               var laststatus = status.Where(x => x.IsLast == true).FirstOrDefault();

                var aproveduserid = history.Where(x => x.StatusId == laststatus.StatusId).OrderByDescending(x => x.WorkDetailHistoryID).FirstOrDefault();

            var reviewuserid = history.Where(x => x.StatusId == (laststatus.StatusId-1)).OrderByDescending(x => x.WorkDetailHistoryID).FirstOrDefault();   
            
           				string tecnameaproved=String.Empty;
            if(aproveduserid != null && aproveduserid.TechnicianID.HasValue)
            {
                 var useraproved = await Basics.GetUserById2(new User() { UserID=aproveduserid.TechnicianID.Value});
                  tecnameaproved = useraproved?.Name + " " + useraproved?.LastName;
            }
            string tecnreview=String.Empty;
            if(reviewuserid != null && reviewuserid.TechnicianID.HasValue)
            {
                  var userreview = await Basics.GetUserById2(new User() { UserID=reviewuserid.TechnicianID.Value});           

                   tecnreview = userreview?.Name + " " + userreview?.LastName;

            }
			
			
               var a = await GetWorkOrderDetailXIdRep2(wo,tecnameaproved,tecnreview);

             
                byte[] bytes = Convert.FromBase64String(a.PdfString);


                //if (wo.CurrentStatusID == 4)
                //{
                     UploadCertificate(a.Serial, bytes);
                //}

            return a;
        
        }
        public async ValueTask<ReportResultSet> GetWorkOrderDetailXIdRep2(WorkOrderDetail wo,
            string techAproved="",
            string techreview="")
        { 
          //var workOrderDetail = await LogicSample.GetWorkOrderDetailXIdRep(wo.WorkOrderDetailID);
            //wo.WorkOrderDetailID = 2;
            try { 
            

            
                
            var workOrderDetail1 = await LogicWod.GetByID(wo);
            var workOrderDetail = await LogicWod.GetWorkOrderDetailByID(wo);
           

            var workOrder = workOrderDetail1.WorkOder;
            var customer = workOrder.Customer;

            //var POE = await Logicpoe.GetPieceOfEquipmentByID(workOrderDetail1.PieceOfEquipment);

            //var address = customer.Aggregates[0];
            var poe = workOrderDetail1.PieceOfEquipment;  // customer.PieceOfEquipment.Where(x => x.WorOrderDetailID == wo.WorkOrderDetailID).FirstOrDefault();
            var poes = await Logicpoe.GetAllWeightSets(poe);
            var eqTemp = poe.EquipmentTemplate;
            var weigthSets = poes.FirstOrDefault().WeightSets;
            //var manu = eqTemp.Manufacturer;
            string manufName;
            if (eqTemp.Manufacturer1 != null)
            {
                manufName = eqTemp.Manufacturer1.Name;
            }
            else
            {
                manufName = null;
            }

            var _linearity = workOrderDetail.BalanceAndScaleCalibration.Linearities;
            IEnumerable<BasicCalibrationResult> _repeatability = null;
            IEnumerable<BasicCalibrationResult> _eccentricity = null;

            if (workOrderDetail.BalanceAndScaleCalibration.Repeatability != null)
            { 
             _repeatability = workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.Where(x => x.CalibrationSubTypeId == 2);
            }

            if (workOrderDetail.BalanceAndScaleCalibration.Eccentricity != null)
            {
                _eccentricity = workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.Where(x => x.CalibrationSubTypeId == 3);
            }

            var asLeftL = _linearity.Where(x => x.BasicCalibrationResult.InToleranceLeft.ToUpper() == "PASS".ToUpper());
            var asFoundL = _linearity.Where(x => x.BasicCalibrationResult.InToleranceFound.ToUpper() == "PASS".ToUpper());
            string _AsLeftResult;
            string _AsFoundResult;

            if (asFoundL.Count() == _linearity.Count())
            {
                _AsFoundResult = "PASS";
            }
            else
            {
                _AsFoundResult = "FAIL";
            }

            if (asLeftL.Count() == _linearity.Count())
            {
                _AsLeftResult = "PASS";
            }
            else
            {
                _AsLeftResult = "FAIL";
            }
            var address = customer.Aggregates.FirstOrDefault().Addresses.FirstOrDefault();
                var date = workOrderDetail1.StatusDate; // history.OrderByDescending(x => x.Date).FirstOrDefault().Date;
            string ManInd;
            string ModelInd;
            string SerialInd;
            if (poe.Indicator == null)
            {
                ManInd = manufName;
                ModelInd = eqTemp.Model;
                SerialInd = poe.SerialNumber;
            }
            else
            {
                ManInd = poe.Indicator != null && poe.Indicator.EquipmentTemplate != null && poe.Indicator.EquipmentTemplate.Manufacturer1 != null ? poe.Indicator.EquipmentTemplate.Manufacturer1.Name : string.Empty;
                ModelInd = poe.Indicator != null && poe.Indicator.EquipmentTemplate != null ? poe.Indicator.EquipmentTemplate.Model : string.Empty;
                SerialInd = poe.Indicator != null ? poe.Indicator.SerialNumber : string.Empty;
            }
                // Build proper address format: Street, City, State ZipCode
                string formattedAddress = "";
                if (!string.IsNullOrEmpty(address.StreetAddress1))
                {
                    formattedAddress = address.StreetAddress1;
                }
                if (!string.IsNullOrEmpty(address.City))
                {
                    formattedAddress += string.IsNullOrEmpty(formattedAddress) ? address.City : ", " + address.City;
                }
                if (!string.IsNullOrEmpty(address.State))
                {
                    formattedAddress += string.IsNullOrEmpty(formattedAddress) ? address.State : ", " + address.State;
                }
                if (!string.IsNullOrEmpty(address.ZipCode))
                {
                    formattedAddress += " " + address.ZipCode;
                }

                Header header = new Header()
                {
                    Client = customer.Name,
                    Address = formattedAddress,
                    Country = customer.Aggregates.FirstOrDefault().Addresses.FirstOrDefault().County,
                    EquipmentLocation = poe.InstallLocation,
                    EquipmentType = eqTemp.EquipmentType,
                    NextCalDate = workOrderDetail.CalibrationDate.Value.ToString("MM/dd/yyyy"), //OJOO no está mapeado
                    LastCalDate = date.Value.ToString("MM/dd/yyyy"), //OJOO no está mapeado
                    ManufacturerInd = ManInd,
                    ModelInd = ModelInd,
                    SerialInd = SerialInd,
                    CapInd = " ",
                    ManufacturerReceiv = manufName,
                    ModelIndReceiv = eqTemp.Model,
                    SerialIndReceiv = poe.SerialNumber,
                    CapIndReceiv = null, //OJOO no está mapeado
                    Class = poe.Class,
                    Type = poe.EquipmentTemplate.DeviceClass, //OJOO no está mapeado
                    PlatformSize = poe.EquipmentTemplate.PlatformSize,// null, //OJOO no está mapeado
                    ServiceLocation = null, //OJOO no está mapeado
                    UnitNumber = null, //OJOO no está mapeado
                    TestingMethod = null, //OJOO no está mapeado
                    CalID = poe.CustomerToolId, //OJOO no está mapeado
                    Location = poe.InstallLocation, //OJOO no está mapeado
                    AsLeftResult = _AsLeftResult, //OJOO no está mapeado
                    AsFoundResult = _AsFoundResult, //OJOO no está mapeado
                    CalibrtionDate = workOrderDetail.CalibrationDate.Value.ToString("MM/dd/yyyy"),
                    Temperature = workOrderDetail.Temperature.ToString() + "/" + workOrderDetail.Humidity.ToString(),
                    Enviroment = workOrderDetail.Environment,
                    Technician = techreview,//workOrderDetail1.Technician.Name + " " + workOrderDetail1.Technician.LastName
                    TechnicianAprove = techAproved,  
                    // Generate Certificate ID: job SO# + numeric designation + Rev-1 (can be used for ISO 17025 or other standards)
                    CertificateId = $"SO{workOrderDetail.WorkOder.WorkOrderId}-{workOrderDetail.WorkOrderDetailID}-Rev-1"
                };



            #region Data CalCertBlank
            List<AsFound> AsFoundList = new List<AsFound>();

            foreach (var af in _linearity)
            {
                var um = (await LogicUOM.GetByID(af.TestPoint.UnitOfMeasurement)).Abbreviation;
                var AsFoundLin = new Reports.Domain.ReportViewModels.AsFound
                {
                    Standard = af.TestPoint.NominalTestPoit + " " + um,

                    
                    Tolerance = af.TestPoint.LowerTolerance + "-" + af.TestPoint.UpperTolerance, //af.TestPoint.LowerTolerance,
                    PassFail = af.BasicCalibrationResult.InToleranceFound,
                    //Description = af.TestPoint.Description, 
                    Indication = af.TestPoint.Description,
                    Uncertainty = af.BasicCalibrationResult.AsFound.ToString() + " " + um, //af.BasicCalibrationResult.Uncertainty,
                    Range = 0 //OJOO no está mapeado

                };
                AsFoundList.Add(AsFoundLin);
            }

            List<AsLeft> AsLeftList = new List<AsLeft>();

            foreach (var af in _linearity)
            {
                var tolerance = "";
                if (workOrderDetail1.IsComercial)
                {
                    tolerance = af.MinToleranceAsLeft.ToString() + "-" + af.MaxToleranceAsLeft.ToString();
                }
                else
                {
                    tolerance = af.MinTolerance.ToString() + "-" + af.MinTolerance.ToString();
                }

                var um = (await LogicUOM.GetByID(af.TestPoint.UnitOfMeasurement)).Abbreviation;
                var AsLeftLin = new Reports.Domain.ReportViewModels.AsLeft
                {
                    Standard = af.TestPoint.NominalTestPoit.ToString() + " " + um,

                    Tolerance = tolerance,//af.TestPoint.LowerTolerance + "-" + af.TestPoint.UpperTolerance,
                    PassFail = af.BasicCalibrationResult.InToleranceLeft,
                    //Description = af.TestPoint.Description, 
                    Indication = af.TestPoint.Description,
                    Uncertainty = af.BasicCalibrationResult.AsLeft.ToString() + " " + um,//af.BasicCalibrationResult.Uncertainty,
                    Range = 0  //OJOO no está mapeado
                };
                AsLeftList.Add(AsLeftLin);
            }

            List<Reports.Domain.ReportViewModels.Excentricity> eccList = new List<Reports.Domain.ReportViewModels.Excentricity>();

            if (_eccentricity != null)
            { 
            foreach (var ex in _eccentricity)
            {
                    var um = (await LogicUOM.GetByID(ex.UnitOfMeasure)).Abbreviation;
                    // Get the nominal weight from the associated weight set instead of the applied weight
                    var nominalWeight = ex.WeightApplied; // Default to applied weight if no weight set found

                    // Access weight sets from the Eccentricity entity
                    var eccentricityEntity = workOrderDetail.BalanceAndScaleCalibration.Eccentricity;
                    if (eccentricityEntity?.WeightSets != null && eccentricityEntity.WeightSets.Any())
                    {
                        // Find the weight set that matches this test point's applied weight
                        var weightSet = eccentricityEntity.WeightSets.FirstOrDefault(ws =>
                            Math.Abs(ws.WeightNominalValue - ex.WeightApplied) < 0.001);

                        if (weightSet != null)
                        {
                            nominalWeight = weightSet.WeightNominalValue;
                        }
                        else
                        {
                            // If no exact match, use the first weight set as fallback
                            var firstWeightSet = eccentricityEntity.WeightSets.FirstOrDefault();
                            if (firstWeightSet != null)
                            {
                                nominalWeight = firstWeightSet.WeightNominalValue;
                            }
                        }
                    }

                    var ecc = new Reports.Domain.ReportViewModels.Excentricity
                {
                    AsFound =Convert.ToDouble(ex.AsFound.ToString()) + " " + um,
                    AsLeft = Convert.ToDouble(ex.AsLeft.ToString()) + " " + um ,
                    Position = ex.Position,
                    Weight = nominalWeight.ToString() + " " + um
                };
                eccList.Add(ecc);
            };
            }

            List<Reports.Domain.ReportViewModels.Repeteab> repetList = new List<Reports.Domain.ReportViewModels.Repeteab>();
            if (_repeatability != null)
            { 
                foreach (var re in _repeatability)
            {
                var rep = new Reports.Domain.ReportViewModels.Repeteab
                {
                    AsFound = re.AsFound.ToString(),
                    AsLeft = re.AsLeft.ToString(),
                    Position = re.Position,
                    Standard = re.WeightApplied.ToString(), //OJOO no está mapeado
                };
                repetList.Add(rep);
            };
            }


            List<Reports.Domain.ReportViewModels.ExcentricityDet> excentriDetList = new List<Reports.Domain.ReportViewModels.ExcentricityDet>();


            foreach (var re in weigthSets)
            {
                var exDet = new Reports.Domain.ReportViewModels.ExcentricityDet
                {
                    Standard = re.WeightNominalValue.ToString(),
                    Certificate = "TEST", //OJOO no está mapeado
                    Description = re.Description, //OJOO no está mapeado
                    CalDue = poe.CalibrationDate.ToString("MM/dd/yyyy"), //OJOO no está mapeado
                };
                excentriDetList.Add(exDet);
            };

            #endregion

            var jsonRepeatability = JsonConvert.SerializeObject(repetList);
            //var jsonPointDecNC = JsonConvert.SerializeObject(_pdNC);
            var jsonEccentricity = JsonConvert.SerializeObject(eccList);
            var jsonEccentricityDet = JsonConvert.SerializeObject(excentriDetList);
            var jsonAsFound = JsonConvert.SerializeObject(AsFoundList);
            var jsonAsLeft = JsonConvert.SerializeObject(AsLeftList);

            header.Eccentricity = jsonEccentricity;
            header.EccentricityDet = jsonEccentricityDet;
            header.AsFound = jsonAsFound;
            header.AsLeft = jsonAsLeft;
            header.Repeteability = jsonRepeatability;

            var jsonHeader = JsonConvert.SerializeObject(header);


            //var options = new JsonSerializerOptions
            //{
            //    IgnoreNullValues = true,
            //    WriteIndented = true
            //};

            ////using FileStream createStream = File.Create(fileName);
            //var serial = JsonConvert.SerializeObject(workOrderDetail1);

            //WorkOrderDetail wodcert = JsonConvert.DeserializeObject<WorkOrderDetail>(serial);


            //Handle TLS protocols
            System.Net.ServicePointManager.SecurityProtocol =
                System.Net.SecurityProtocolType.Tls
                | System.Net.SecurityProtocolType.Tls11
                | System.Net.SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };

          
            var contentHeader = new StringContent(jsonHeader, System.Text.Encoding.UTF8, "application/json");
          
            Http = new HttpClient();


            Http.BaseAddress = new System.Uri(Configuration.GetSection("Reports")["Url"]);
            //  Console.WriteLine("URL grpc " + Http.BaseAddress);
            Http.DefaultRequestHeaders.Accept.Clear();
            Http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage pdf = null;
            string psdInBase64 = "";

            // pdf = await Http.PostAsync("/GetCustomerCover", content);
            pdf = await Http.PostAsync("/GetPointDecresingNC", contentHeader);
            var contentReponse = await pdf.Content.ReadAsStringAsync();

//            Console.WriteLine("contentReponse " + contentReponse);

            psdInBase64 = JsonConvert.DeserializeObject(contentReponse).ToString();
            //b64 = psdInBase64;
            //byte[] bytes = Convert.FromBase64String(psdInBase64);


            //    if (wo.CurrentStatusID == 4)
            //    {
             //       UploadCertificate(header.SerialIndReceiv, bytes);
            //    }
           


            //03/29/2021
            
            

            var serialHash = GetSHA1Has(header.SerialIndReceiv);


            Certificate cert = new Certificate();
            cert.CertificateNumber = wo.WorkOrderDetailID.ToString() + "_" + wo.WorkOrderDetailHash;
                if (wo.CalibrationCustomDueDate.HasValue)
                {
                    cert.DueDate = wo.CalibrationCustomDueDate.Value;
                }
                if (wo.CalibrationDate.HasValue)
                {
                    cert.CalibrationDate = wo.CalibrationDate.Value;
                }
            //cert.PieceOfEquipmentId = 1;
            cert.Description = @"/Certificate/" + serialHash + ".pdf";
            cert.Version = Convert.ToInt32(wo.WorkOrderDetailHash);
            cert.WorkOrderDetailId = wo.WorkOrderDetailID;


            //var options = new JsonSerializerOptions
            //{
            //    IgnoreNullValues = true,
            //    WriteIndented = true
            //};

            ////using FileStream createStream = File.Create(fileName);
            //var serial = System.Text.Json.JsonSerializer.Serialize(workOrderDetail1, options);


            //var resultwhd =  LogicWod.CreateCertificate(cert);
            //var jsonHeader = JsonConvert.SerializeObject(header);


            var options = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                WriteIndented = true
            };
                string serial = "";
                //using FileStream createStream = File.Create(fileName);
                try {

                serial = JsonConvert.SerializeObject(workOrderDetail1);

                cert.WorkOrderDetailSerialized = serial;                   

                var resultwhd = await LogicWod.CreateCertificate(cert);

                   
                }
                catch(Exception ex)
                {

                }

                ReportResultSet res = new ReportResultSet();

                res.PdfString = psdInBase64;
                res.JsonObject = serial;
                res.Serial = header.SerialIndReceiv;
                return res;
            }
            catch (TimeoutException ex)
            {
                throw new TimeoutException("TimeOut");
            }

            catch (Exception ex)
            {
                throw new CertificationException("Error creating certificate");
            }
        
        }

        public async ValueTask<ReportResultSet> GetWorkOrderDetailXIdRep(WorkOrderDetail wo)
        {

            var a = await GetWorkOrderDetailXIdRep2(wo);

            return a;

        }

        //public async ValueTask<ReportResultSet> GetWorkOrderDetailXIdRepWithSave(WorkOrderDetail wo)
        //{

        //    var a = await GetWorkOrderDetailXIdRepWithSave(wo);

        //    return a;

        //}

        public async ValueTask<string> GetReportUncertaintyBudgetComp(WorkOrderDetail wo, CallContext context)
        {
            //var workOrderDetail = await LogicSample.GetWorkOrderDetailXIdRep(wo.WorkOrderDetailID);


            var workOrderDetail1 = await LogicWod.GetByID(wo);
            var workOrderDetail = await LogicWod.GetWorkOrderDetailByID(wo);

            var workOrder = workOrderDetail1.WorkOder;
            var customer = workOrder.Customer;
            //var address = customer.Aggregates[0];
            var poe = workOrderDetail1.PieceOfEquipment;  // customer.PieceOfEquipment.Where(x => x.WorOrderDetailID == wo.WorkOrderDetailID).FirstOrDefault();
            var poes = await Logicpoe.GetAllWeightSets(poe);
            var eqTemp = poe.EquipmentTemplate;
            var weigthSets = poes.FirstOrDefault().WeightSets;
            //var manu = eqTemp.Manufacturer;
            string manufName;
            if (eqTemp.Manufacturer1 != null)
            {
                manufName = eqTemp.Manufacturer1.Name;
            }
            else
            {
                manufName = null;
            }

            var _linearity = workOrderDetail.BalanceAndScaleCalibration.Linearities;
            
            CalibrationSaaS.Domain.Aggregates.Entities.Repeatability _repeatability = null;
            IEnumerable<BasicCalibrationResult> _repeatabilityBCR = null;
            CalibrationSaaS.Domain.Aggregates.Entities.Eccentricity _eccentricity = null;
            IEnumerable<BasicCalibrationResult> _eccentricityBCR = null;

            if (workOrderDetail.BalanceAndScaleCalibration.Repeatability != null)
            {
                 _repeatability = workOrderDetail.BalanceAndScaleCalibration.Repeatability;
                _repeatabilityBCR = workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.Where(x => x.CalibrationSubTypeId == 2);
            }
            
            
            
            var _balance = workOrderDetail.BalanceAndScaleCalibration;

            if (workOrderDetail.BalanceAndScaleCalibration.Eccentricity != null)
            {
                _eccentricityBCR = workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.Where(x => x.CalibrationSubTypeId == 3);
                _eccentricity = workOrderDetail.BalanceAndScaleCalibration.Eccentricity;//.BasicCalibrationResult.Where(x => x.CalibrationSubTypeId == 3);
            }
            

            Header header = new Header()
            {
                EquipmentType = eqTemp.EquipmentType,
                WorkOrderDetailId = workOrderDetail.WorkOrderDetailID,
                CalibrtionDate = workOrderDetail.CalibrationDate.Value.ToString("MM/dd/yyyy")

            };

            #region Data CalCertBlank
            List<UcertaintyUncert> UncertaintyList = new List<UcertaintyUncert>();

            foreach (var af in _linearity)
            {
                var Uncertainty = new Reports.Domain.ReportViewModels.UcertaintyUncert
                {
                    Weight = af.TestPoint.NominalTestPoit.ToString() + " " + af.TestPoint.UnitOfMeasurement.Abbreviation,
                    Distribution = af.CalibrationUncertaintyDistribution,
                    Divisor = af.CalibrationUncertaintyDivisor,
                    Magnitude = af.BasicCalibrationResult.AsLeft,
                    Qoutient = af.Quotient,
                    QoutSquare = af.Square,
                    Type = af.CalibrationUncertaintyType,
                    Units = af.TestPoint.UnitOfMeasurement.Abbreviation

                };
                UncertaintyList.Add(Uncertainty);
            }



            List<Reports.Domain.ReportViewModels.CornerloadUncertComp> eccList = new List<Reports.Domain.ReportViewModels.CornerloadUncertComp>();

            //foreach (var af in _eccentricity)
            //{
            if (_eccentricityBCR !=null)
            { 
                
                foreach (var x in _eccentricityBCR)
                {
                    var cornerload = new Reports.Domain.ReportViewModels.CornerloadUncertComp
                    {
                        Weight = x.WeightApplied.ToString() + " " + x.UnitOfMeasure.Abbreviation,
                        Distribution = x. Eccentricity.EccentricityUncertaintyDistribution,
                        Divisor = x.Eccentricity.EccentricityUncertaintyDivisor,
                        Magnitude = x.AsLeft,
                        Qoutient = x.Eccentricity.EccentricityQuotient,
                        QoutSquare = x.Eccentricity.EccentricitySquare,
                        Type = x.Eccentricity.EccentricityUncertaintyType,
                        Units = x.UnitOfMeasure.Abbreviation

                    };

                    eccList.Add(cornerload);

                };
            }
            List<Reports.Domain.ReportViewModels.RepeteabilityUncertComp> repetList = new List<Reports.Domain.ReportViewModels.RepeteabilityUncertComp>();

            if (_repeatabilityBCR != null)
            {
               
                foreach (var x in _repeatabilityBCR)
                {
                    var rep = new Reports.Domain.ReportViewModels.CornerloadUncertComp
                    {
                        Weight = x.WeightApplied.ToString() + " " + x.UnitOfMeasure.Abbreviation,
                        Distribution = x.Repeatability.RepeatabilityUncertaintyDistribution,
                        Divisor = x.Repeatability.RepeatabilityUncertaintyDivisor,
                        Magnitude = x.AsLeft,
                        Qoutient = x.Repeatability.RepeatabilityQuotient,
                        QoutSquare = x.Repeatability.RepeatabilitySquare,
                        Type = x.Repeatability.RepeatabilityUncertaintyType,
                        Units = x.UnitOfMeasure.Abbreviation

                    };

                    eccList.Add(rep);

                };
            }
           
            
            
            List<Reports.Domain.ReportViewModels.EnvironmentalUncertComp> envList = new List<Reports.Domain.ReportViewModels.EnvironmentalUncertComp>();
            List<Reports.Domain.ReportViewModels.ResolutionUncertComp> resList = new List<Reports.Domain.ReportViewModels.ResolutionUncertComp>();

            var env = new Reports.Domain.ReportViewModels.EnvironmentalUncertComp
            {
                Distribution = _balance.EnvironmentalUncertaintyDistribution,
                Divisor = _balance.EnvironmentalUncertaintyDivisor,
                //Magnitude = _balance.
                Qoutient = _balance.EnvironmentalQuotient,
                QoutSquare = _balance.EnvironmentalSquare,
                Type = _balance.EnvironmentalUncertaintyType,
                Units = ""

            };

            envList.Add(env);
          
                var res = new Reports.Domain.ReportViewModels.ResolutionUncertComp
            {
                Distribution = _balance.ResolutionUncertaintyDistribution,
                Divisor = _balance.ResolutionUncertaintyDivisor,
                Magnitude = 0.004,
                Qoutient = _balance.ResolutionQuotient,
                QoutSquare = _balance.ResolutionSquare,
                Type = _balance.ResolutionUncertaintyType,
                Units = ""

            };

            

            #endregion

            var jsonRepeatability = JsonConvert.SerializeObject(repetList);
            //var jsonPointDecNC = JsonConvert.SerializeObject(_pdNC);
            var jsonCornerload = JsonConvert.SerializeObject(eccList);
            var jsonLinearity = JsonConvert.SerializeObject(UncertaintyList);
            var jsonEnviroment = JsonConvert.SerializeObject(envList);
            var jsonResolution = JsonConvert.SerializeObject(resList);

            header.CornerloadBudgetComp = jsonCornerload;
            header.RepeteabBudgetComp = jsonRepeatability;
            header.UncertaintyBudgetComp = jsonLinearity;
            header.EnvironmetBudgetComp = jsonEnviroment;
            header.ResolutionBudgetComp = jsonResolution;

            var jsonHeader = JsonConvert.SerializeObject(header);

            //Handle TLS protocols
            System.Net.ServicePointManager.SecurityProtocol =
                System.Net.SecurityProtocolType.Tls
                | System.Net.SecurityProtocolType.Tls11
                | System.Net.SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };

            //var content = new StringContent(jsonCustomerReport, System.Text.Encoding.UTF8, "application/json");
            //  var content = new StringContent(jsonRepeatability, System.Text.Encoding.UTF8, "application/json");
            //var contentPNC = new StringContent(jsonPointDecNC, System.Text.Encoding.UTF8, "application/json");
            var contentHeader = new StringContent(jsonHeader, System.Text.Encoding.UTF8, "application/json");
            //  Console.WriteLine("contentPNC " + contentPNC);
            // Http.BaseAddress = new System.Uri("https://calsaasreport.azurewebsites.net");

            Http = new HttpClient();
            Http.BaseAddress = Http.BaseAddress = new System.Uri(Configuration.GetSection("Reports")["Url"]);//new System.Uri("https://localhost:44386/");
            //Console.WriteLine(">>>>>-- " + Configuration.GetSection("Reports")["URL"]);
            //Http.BaseAddress = new System.Uri(Configuration.GetSection("Reports")["URL"]);
//            Console.WriteLine("URL grpc " + Http.BaseAddress);
            Http.DefaultRequestHeaders.Accept.Clear();
            Http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage pdf = null;
            string psdInBase64 = "";

            // pdf = await Http.PostAsync("/GetCustomerCover", content);
            pdf = await Http.PostAsync("/GetReportUncertaintyBudgetComp", contentHeader);
            var contentReponse = await pdf.Content.ReadAsStringAsync();

//            Console.WriteLine("contentReponse " + contentReponse);

            psdInBase64 = JsonConvert.DeserializeObject(contentReponse).ToString();
            //b64 = psdInBase64;

//            Console.WriteLine("psdInBase  " + psdInBase64);


            return psdInBase64;

        }

        public async ValueTask<string> GetReportUncertaintyBudget(Linearity li, CallContext context)
        {
            //var workOrderDetail = await LogicSample.GetWorkOrderDetailXIdRep(wo.WorkOrderDetailID);
            WorkOrderDetail wo = new WorkOrderDetail();
            wo.WorkOrderDetailID = li.WorkOrderDetailId;

            var workOrderDetail1 = await LogicWod.GetByID(wo);
            var workOrderDetail = await LogicWod.GetWorkOrderDetailByID(wo);

            var workOrder = workOrderDetail1.WorkOder;
            var customer = workOrder.Customer;
            //var address = customer.Aggregates[0];
            var poe = workOrderDetail1.PieceOfEquipment;  // customer.PieceOfEquipment.Where(x => x.WorOrderDetailID == wo.WorkOrderDetailID).FirstOrDefault();
            var poes = await Logicpoe.GetAllWeightSets(poe);
            var eqTemp = poe.EquipmentTemplate;
            var weigthSets = poes.FirstOrDefault().WeightSets;
            //var manu = eqTemp.Manufacturer;
            string manufName;
            if (eqTemp.Manufacturer1 != null)
            {
                manufName = eqTemp.Manufacturer1.Name;
            }
            else
            {
                manufName = null;
            }

            var _linearity = workOrderDetail.BalanceAndScaleCalibration.Linearities;

            CalibrationSaaS.Domain.Aggregates.Entities.Repeatability _repeatability = null;
            IEnumerable<BasicCalibrationResult> _repeatabilityBCR = null;
            CalibrationSaaS.Domain.Aggregates.Entities.Eccentricity _eccentricity = null;
            IEnumerable<BasicCalibrationResult> _eccentricityBCR = null;

            if (workOrderDetail.BalanceAndScaleCalibration.Repeatability != null)
            {
                _repeatability = workOrderDetail.BalanceAndScaleCalibration.Repeatability;
                _repeatabilityBCR = workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.Where(x => x.CalibrationSubTypeId == 2);
            }



            var _balance = workOrderDetail.BalanceAndScaleCalibration;

            if (workOrderDetail.BalanceAndScaleCalibration.Eccentricity != null)
            {
                _eccentricityBCR = workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.Where(x => x.CalibrationSubTypeId == 3);
                _eccentricity = workOrderDetail.BalanceAndScaleCalibration.Eccentricity;//.BasicCalibrationResult.Where(x => x.CalibrationSubTypeId == 3);
            }


            Header header = new Header()
            {
                EquipmentType = eqTemp.EquipmentType,
                WorkOrderDetailId = workOrderDetail.WorkOrderDetailID,
                CalibrtionDate = workOrderDetail.CalibrationDate.Value.ToString("MM/dd/yyyy")

            };

            #region Data CalCertBlank
            List<UcertaintyUncert> UncertaintyList = new List<UcertaintyUncert>();

            foreach (var af in _linearity)
            {
                var Uncertainty = new Reports.Domain.ReportViewModels.UcertaintyUncert
                {
                    Weight = af.TestPoint.NominalTestPoit.ToString() + " " + af.TestPoint.UnitOfMeasurement.Abbreviation,
                    Distribution = af.CalibrationUncertaintyDistribution,
                    Divisor = af.CalibrationUncertaintyDivisor,
                    Magnitude = af.BasicCalibrationResult.AsLeft,
                    Qoutient = af.Quotient,
                    QoutSquare = af.Square,
                    Type = af.CalibrationUncertaintyType,
                    Units = af.TestPoint.UnitOfMeasurement.Abbreviation

                };
                UncertaintyList.Add(Uncertainty);
            }



            List<Reports.Domain.ReportViewModels.CornerloadUncertComp> eccList = new List<Reports.Domain.ReportViewModels.CornerloadUncertComp>();

            //foreach (var af in _eccentricity)
            //{
            if (_eccentricityBCR != null)
            {

                foreach (var x in _eccentricityBCR)
                {
                    var cornerload = new Reports.Domain.ReportViewModels.CornerloadUncertComp
                    {
                        Weight = x.WeightApplied.ToString() + x.UnitOfMeasure.Abbreviation,
                        Distribution = x.Eccentricity.EccentricityUncertaintyDistribution,
                        Divisor = x.Eccentricity.EccentricityUncertaintyDivisor,
                        Magnitude = x.AsLeft,
                        Qoutient = x.Eccentricity.EccentricityQuotient,
                        QoutSquare = x.Eccentricity.EccentricitySquare,
                        Type = x.Eccentricity.EccentricityUncertaintyType,
                        Units = x.UnitOfMeasure.Abbreviation

                    };

                    eccList.Add(cornerload);

                };
            }
            List<Reports.Domain.ReportViewModels.RepeteabilityUncertComp> repetList = new List<Reports.Domain.ReportViewModels.RepeteabilityUncertComp>();

            if (_repeatabilityBCR != null)
            {

                foreach (var x in _repeatabilityBCR)
                {
                    var rep = new Reports.Domain.ReportViewModels.CornerloadUncertComp
                    {
                        Weight = x.WeightApplied.ToString() + " " + x.UnitOfMeasure.Abbreviation,
                        Distribution = x.Repeatability.RepeatabilityUncertaintyDistribution,
                        Divisor = x.Repeatability.RepeatabilityUncertaintyDivisor,
                        Magnitude = x.AsLeft,
                        Qoutient = x.Repeatability.RepeatabilityQuotient,
                        QoutSquare = x.Repeatability.RepeatabilitySquare,
                        Type = x.Repeatability.RepeatabilityUncertaintyType,
                        Units = x.UnitOfMeasure.Name

                    };

                    eccList.Add(rep);

                };
            }



            List<Reports.Domain.ReportViewModels.EnvironmentalUncertComp> envList = new List<Reports.Domain.ReportViewModels.EnvironmentalUncertComp>();
            List<Reports.Domain.ReportViewModels.ResolutionUncertComp> resList = new List<Reports.Domain.ReportViewModels.ResolutionUncertComp>();

            var env = new Reports.Domain.ReportViewModels.EnvironmentalUncertComp
            {
                Distribution = _balance.EnvironmentalUncertaintyDistribution,
                Divisor = _balance.EnvironmentalUncertaintyDivisor,
                //Magnitude = _balance.
                Qoutient = _balance.EnvironmentalQuotient,
                QoutSquare = _balance.EnvironmentalSquare,
                Type = _balance.EnvironmentalUncertaintyType,
                Units = ""

            };

            envList.Add(env);

            var res = new Reports.Domain.ReportViewModels.ResolutionUncertComp
            {
                Distribution = _balance.ResolutionUncertaintyDistribution,
                Divisor = _balance.ResolutionUncertaintyDivisor,
                Magnitude = 0.004,
                Qoutient = _balance.ResolutionQuotient,
                QoutSquare = _balance.ResolutionSquare,
                Type = _balance.ResolutionUncertaintyType,
                Units = ""

            };



            #endregion

            var jsonRepeatability = JsonConvert.SerializeObject(repetList);
            //var jsonPointDecNC = JsonConvert.SerializeObject(_pdNC);
            var jsonCornerload = JsonConvert.SerializeObject(eccList);
            var jsonLinearity = JsonConvert.SerializeObject(UncertaintyList);
            var jsonEnviroment = JsonConvert.SerializeObject(envList);
            var jsonResolution = JsonConvert.SerializeObject(resList);

            header.CornerloadBudgetComp = jsonCornerload;
            header.RepeteabBudgetComp = jsonRepeatability;
            header.UncertaintyBudgetComp = jsonLinearity;
            header.EnvironmetBudgetComp = jsonEnviroment;
            header.ResolutionBudgetComp = jsonResolution;

            var jsonHeader = JsonConvert.SerializeObject(header);

            //Handle TLS protocols
            System.Net.ServicePointManager.SecurityProtocol =
                System.Net.SecurityProtocolType.Tls
                | System.Net.SecurityProtocolType.Tls11
                | System.Net.SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };

            //var content = new StringContent(jsonCustomerReport, System.Text.Encoding.UTF8, "application/json");
            //  var content = new StringContent(jsonRepeatability, System.Text.Encoding.UTF8, "application/json");
            //var contentPNC = new StringContent(jsonPointDecNC, System.Text.Encoding.UTF8, "application/json");
            var contentHeader = new StringContent(jsonHeader, System.Text.Encoding.UTF8, "application/json");
            //  Console.WriteLine("contentPNC " + contentPNC);
            // Http.BaseAddress = new System.Uri("https://calsaasreport.azurewebsites.net");

            Http = new HttpClient();
            Http.BaseAddress = Http.BaseAddress = new System.Uri(Configuration.GetSection("Reports")["Url"]);//new System.Uri("https://localhost:44386/");
            //Console.WriteLine(">>>>>-- " + Configuration.GetSection("Reports")["URL"]);
            //Http.BaseAddress = new System.Uri(Configuration.GetSection("Reports")["URL"]);
//            Console.WriteLine("URL grpc " + Http.BaseAddress);
            Http.DefaultRequestHeaders.Accept.Clear();
            Http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage pdf = null;
            string psdInBase64 = "";

            // pdf = await Http.PostAsync("/GetCustomerCover", content);
            pdf = await Http.PostAsync("/GetReportUncertaintyBudget", contentHeader);
            var contentReponse = await pdf.Content.ReadAsStringAsync();

//            Console.WriteLine("contentReponse " + contentReponse);

            psdInBase64 = JsonConvert.DeserializeObject(contentReponse).ToString();
            //b64 = psdInBase64;

//            Console.WriteLine("psdInBase  " + psdInBase64);


            return psdInBase64;

        }



        public void UploadCertificate(string serial, Byte[] pdf)
        {
            try
            {
                var serialHash = GetSHA1Has(serial);
                MemoryStream streams = new MemoryStream();
                var path = AppDomain.CurrentDomain.BaseDirectory ;
                
                //string[] spl = path.Split("\\");
                //bool f = true;
                //string path1 = "";
                //foreach (var x in spl)
                //{
                //    if (x == "CalibrationSaaS.Service")
                //    {
                //        f = false;
                //    }
                //    if (f)
                //    { 
                //    path1 = path1 + @"\" + x  ;
                //    }
                    
                //}

                //path = path1.Substring(1, path1.Length-1);

                path = path + @"wwwroot\Certificate";//@"\CalibrationSaaS.Infraestructure.Blazor\wwwroot\Certificate\";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    //if (File.Exists(path + serialHash + ".pdf"))
                    //{
                    //    File.Delete(path + serialHash + ".pdf");
                    //}
                }
                else
                {

                }

                //Directory.CreateDirectory(path);
                using (System.IO.FileStream stream = new FileStream(path + @"\" + serialHash + ".pdf", FileMode.OpenOrCreate))
                {
                     System.IO.BinaryWriter writer = new BinaryWriter(stream);
                     writer.Write(pdf, 0, pdf.Length);
                     writer.Close();

                }
                   

                String strorageconn = ConfigurationExtensions.GetConnectionString(this.Configuration, "fileShareConnectionString");
                CloudStorageAccount storageacc = CloudStorageAccount.Parse(strorageconn);

                //Create Reference to Azure Blob
                CloudBlobClient blobClient = storageacc.CreateCloudBlobClient();

                //The next 2 lines create if not exists a container named "democontainer"
                CloudBlobContainer container = blobClient.GetContainerReference("certificates");

                container.CreateIfNotExists();

                MemoryStream ms = new MemoryStream(pdf);

               



                //using (StreamWriter streamWriter = new StreamWriter(streams))
                //    {

                //            streamWriter.Write(pdf);
                //            streamWriter.Flush();


                //CloudBlockBlob blockBlob = container.GetBlockBlobReference("DemoBlob");
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(serialHash + ".pdf");

                //**************************************************************************



                // using (var filestream = System.IO.File.OpenRead(path + serialHash + ".pdf"))
                //
                //using (var filestream = System.IO.File.OpenRead(+ serialHash + ".pdf"))
                //////System.IO.File.OpenRead(@"C :\Azure Storage Demo\test.txt"))
                //{


                blockBlob.UploadFromStream(ms);



                //}

                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetSHA1Has(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] b = Encoding.ASCII.GetBytes(str);
            byte[] hash = md5.ComputeHash(b);

            StringBuilder sb = new StringBuilder();
            foreach (var a in hash)
                sb.Append(a.ToString("X2"));
            return sb.ToString();
        }

        public string GetUrlServer()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            return path;
        }
    }
}
