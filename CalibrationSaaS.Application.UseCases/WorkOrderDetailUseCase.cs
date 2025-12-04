using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using CalibrationSaaS.Domain.BusinessExceptions.WorkOrderDetail;
using CalibrationSaaS.Domain.Aggregates.Querys;
using System.Linq.Expressions;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Helpers.Controls.ValueObjects;
using Helpers.Controls;
using Reports.Domain.ReportViewModels;
using Newtonsoft.Json;
using Helpers;
using static CalibrationSaaS.Domain.Aggregates.Querys.Querys;
using Bogus.DataSets;
using Newtonsoft.Json.Linq;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using System.Net;
using System.ComponentModel;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json.Nodes;
using System.Dynamic;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Domain.Aggregates.Shared;
using System.Net.NetworkInformation;

using Microsoft.Extensions.Primitives;
using System.Reflection;
using System.Collections;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.Metrics;
using static CalibrationSaaS.Domain.Aggregates.Querys.Querys.WOD;
using System.Runtime.ConstrainedExecution;
using LinqKit;
using Address = CalibrationSaaS.Domain.Aggregates.Entities.Address;
using Microsoft.Extensions.Configuration;
using Bogus;
using System.Runtime.InteropServices.JavaScript;

using CalibrationSaaS.Domain.Aggregates;
using System.Transactions;


using static System.Collections.Specialized.BitVector32;
using System.Xml.Linq;
using Microsoft.AspNetCore.Builder;
using System.Reflection.Emit;
using System.Security.Cryptography.Xml;
using Jint;

namespace CalibrationSaaS.Application.UseCases
{
    public class WorkOrderDetailUseCase
    {
        private readonly IWorkOrderDetailRepository Repository;
        private readonly IIdentityRepository IdentityRepository;
        private readonly IPieceOfEquipmentRepository PieceOfEquipmentRepository;
        private readonly IAssetsRepository Assets;
        private readonly IUOMRepository UoMRepository;
        private readonly IBasicsRepository Basics;
        //private readonly IWorkOrderRepository WorkOrderRepository;
        private readonly HttpClient _httpClient;
        private readonly PieceOfEquipmentUseCases Poe;
        private readonly Microsoft.Extensions.Configuration.IConfiguration Configuration;
       // private readonly ICustomerRepository customerRepository;
        public WorkOrderDetailUseCase(IWorkOrderDetailRepository _Repository, IIdentityRepository _IdentityRepository,IPieceOfEquipmentRepository _PieceOfEquipmentRepository, IAssetsRepository _assets, 
            IUOMRepository _UoMRepository, IBasicsRepository _Basics, 
            IHttpClientFactory httpClientFactory, PieceOfEquipmentUseCases poe,
            Microsoft.Extensions.Configuration.IConfiguration _Configuration)
        {

            _httpClient = httpClientFactory.CreateClient();
            Repository = _Repository;
            this.IdentityRepository = _IdentityRepository;
            PieceOfEquipmentRepository = _PieceOfEquipmentRepository;
            Assets = _assets;
            UoMRepository = _UoMRepository;
            Basics = _Basics;
            Poe = poe;
            Configuration = _Configuration;
            //WorkOrderRepository = _WorkOrderRepository;
        }

        public async Task SaveCustomStatus()
        {

            Status first = new Status()
            {
                Name = "PoE Review",
                IsDefault = true,
                IsEnable = true,
                Description= "",
                Possibilities="2",
                IsLast=false
            };

            await Repository.SaveStatus(first);

            Status second = new Status()
            {
                Name = "Ready for Calibration",
                IsDefault = false,
                IsEnable = true,
                Description = "",
                Possibilities = "1;3"
            };

            await Repository.SaveStatus(second);

            Status third = new Status()
            {
                Name = "Technical Review",
                IsDefault = false,
                IsEnable = true,
                Description = "",
                Possibilities = "1;2;4"
            };
            await Repository.SaveStatus(third);
            //Status four = new Status()
            //{
            //    Name = "Calibrated",
            //    IsDefault = false,
            //    IsEnable = true,
            //    Description = "",
            //    Possibilities = "3;5"
            //};
            //await Repository.SaveStatus(four);
            //Status Five = new Status()
            //{
            //    Name = "Quality Review",
            //    IsDefault = false,
            //    IsEnable = true,
            //    Description = "",
            //    Possibilities = "3;4;6"
            //};
            //await Repository.SaveStatus(Five);

            Status six = new Status()
            {
                Name = "Completed",
                IsDefault = false,
                IsEnable = true,
                Description = "",
                IsLast = true,
                //Possibilities = "3;4;6"
            };
            await Repository.SaveStatus(six);

        }

        public async  Task<IEnumerable<Status>> GetStatus()
        {
           List<Status> a= (List<Status>)await  Repository.GetStatus();

            if(a == null || a.Count ==0)
            {
               await  SaveCustomStatus();

            }

            a = (List<Status>)await Repository.GetStatus();

            return a;

        }

        public Task<IEnumerable<WorkOrderDetail>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<WorkOrderDetail> Create(WorkOrderDetail DTO)
        {
            throw new NotImplementedException();
        }

        public async Task<WorkOrderDetail> Delete(WorkOrderDetail DTO,bool Reset)
        {

            //if (DTO.CurrentStatus.StatusId > 1)
            //{
            //    return null;
            //}
            //else
            //{
            //    var result = await Repository.Delete(DTO);
            //    return DTO;
            //}

            var result = await Repository.Delete(DTO,Reset);
            return DTO;


        }


        public async Task<ResultSet<WorkOrderDetail>> GetByTechnicianPag(Pagination<WorkOrderDetail> pagination)
        {
            var a = await Repository.GetByTechnicianPag(pagination);


            return a;

        }

        public async Task<List<CalibrationSubType>> GetCalibrationSubTypes() 
        {
             var calibrationSubTypes = await Repository.GetCalibrationSubType();
            return calibrationSubTypes;
        }
        public async Task<ICollection<WorkOrderDetail>> GetByTechnician(User DTO)
        {
            var a = await Repository.GetByTechnician(DTO);


            return a;

        }

        public async Task<WorkOrderDetail> GetByID(WorkOrderDetail DTO)
        {
            //se inicializan valores por defecto
            var a = await Repository.GetByID(DTO);

            if (a == null)
            {
                throw new Exception("Order Detail not Found");
            }

         
            return a;
        }
        //9503
        public async Task<WorkOrderDetail> GetByIDPreviousCalibration(WorkOrderDetail DTO)
        {
            //se inicializan valores por defecto
            var a = await Repository.GetByIDPreviousCalibration(DTO);

            if (a == null)
            {
                throw new Exception("Order Detail not Found");
            }


            return a;
        }

        public async Task<IEnumerable<WorkDetailHistory>> GetHistory(WorkOrderDetail DTO)
        {
            return await Repository.GetHistory(DTO);
        }

        public async Task<WorkOrderDetail> GetHeaderById(WorkOrderDetail DTO)
        {
            return await Repository.GetHeaderById(DTO);
        }


        private string ValidateWOD( Status NextStatus,ref WorkOrderDetail DTO, bool Calculate =false)
        {

           
            var validation = Querys.WOD.ValidateWODList(NextStatus, ref DTO, Calculate);
            
            var a= string.Join(Environment.NewLine, validation.ToArray());

            if (!string.IsNullOrEmpty(a))
            {
                return a;
            }

            if(NextStatus.StatusId >= 3) 
            { 

            try
            {
                var bc = DTO.BalanceAndScaleCalibration;

                var bc2=  Repository.GetConfiguredWeights(DTO.WorkOrderDetailID,  bc).GetAwaiter().GetResult(); ;

                DTO.BalanceAndScaleCalibration = bc2;

                //var result = Querys.WOD.CalculateValuesByID(DTO).GetAwaiter().GetResult();

                //DTO.BalanceAndScaleCalibration = result.BalanceAndScaleCalibration;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            }


            return "";

        }


        public async Task<WorkOrderDetail> GetConfiguredWeights(WorkOrderDetail DTO)
        {

            var bc = DTO.BalanceAndScaleCalibration;

            var bc2 = await Repository.GetConfiguredWeights(DTO.WorkOrderDetailID, bc);

            DTO.BalanceAndScaleCalibration = bc2;

            return DTO;

        }





        public async Task<WorkOrderDetail> ChangeStatus(WorkOrderDetail DTO, string userName=null, string Component = null)
        {
            //bussiness rules to validation change status

            


            var s = await GetStatus();
            Status nextStatus = Querys.WOD.GetNextStatus(DTO,s); //new Status();
           

            string validationMessage = null;
            if (!nextStatus.IsLast)
            { 
                validationMessage = ValidateWOD( nextStatus, ref DTO);
            }
            else
            {
                DTO.HasBeenCompleted = true;
            }
            if (!string.IsNullOrEmpty(validationMessage))
            {

                throw new ChangeStatusException(validationMessage);
            }

            DTO.CurrentStatus = nextStatus;
            DTO.CurrentStatusID = nextStatus.StatusId;
            DTO.SelectedNewStatus = -1;

            DTO.StatusDate = DateTime.Now;


            if (!string.IsNullOrEmpty(DTO.TechnicianComment))
            {
                
                var blog = DTO.TechnicianComment;

                

            }

            var result = await Repository.ChangeStatus(DTO, Component);

            if (nextStatus.IsLast)
            { 
            
            }

            DTO = await GetByID(DTO);

            var hi = await SaveHistory(DTO,1,"Change Status " + DTO.CurrentStatusID.GetStatus(s),userName);

            if (!hi)
            {
                throw new ChangeStatusException("Trace not save");
            }

          


            return DTO;

        }


       
        public async Task<WorkDetailHistory> ChangeStatusComplete(WorkDetailHistory DTO)
        {
            var result = await Repository.ChangeStatusComplete(DTO);
            return result;
        }

        public async Task<Certificate> CreateCertificate(Certificate DTO)
        {
            var result = await Repository.CreateCertificate(DTO);
            return result;
        }
 
            public async Task<bool> SaveHistory(WorkOrderDetail DTO, int version, string Action= "Change Status",string UserName=null)
        {

            try
            {
                if (!string.IsNullOrEmpty(UserName))
                {
                    var r2 = await IdentityRepository.ShowRecords(UserName);

                    if (r2 != null)
                    {
                        UserName = r2.Name;
                    }
                }
               
            }
            catch (Exception ex)
            {

            }
           

            if (DTO.CurrentStatusID > 10)
            {
                var options = new JsonSerializerOptions
                {
                    IgnoreNullValues = true,
                    WriteIndented = true
                };

             
                var serial  = System.Text.Json.JsonSerializer.Serialize(DTO, options);
            }

            

            WorkDetailHistory wh = new WorkDetailHistory();

            wh.WorkOrderDetailID = DTO.WorkOrderDetailID;


            if (DTO.TechnicianID.HasValue)
            {
                wh.TechnicianID = DTO.TechnicianID.Value;
            }          

            if (DTO.Technician != null)
            {
                wh.Name = DTO.Technician.Name;
            }

            wh.StatusId = DTO.CurrentStatusID;

            wh.Version = version;

            wh.UserName = UserName;

            wh.Action = Action;

            wh.Date = DateTime.Now;

           bool r= await Repository.CreateHistory(wh);

           return r;
        }

        public async ValueTask<GenericHeader> GetGenericHeader(WorkOrderDetail workOrderDetail1)
        {

            
            var workOrderDetail = workOrderDetail1;


         
            var workOrder = workOrderDetail1.WorkOder;
            var customer = workOrder.Customer;

            
            var poe = workOrderDetail1.PieceOfEquipment;  

            List<WeightSetHeader> listw = new List<WeightSetHeader>();

            var poes = await PieceOfEquipmentRepository.GetAllWeightSets(poe);

            if (workOrderDetail.WOD_Weights != null)
            {
                var testp = workOrderDetail.WOD_Weights.DistinctBy(x => x.WeightSet.PieceOfEquipmentID);

                foreach (var item in testp)
                {
                    var poetmp = await PieceOfEquipmentRepository.GetPieceOfEquipmentByIDHeader(item.WeightSet.PieceOfEquipmentID);

                    var _listCetificate = await Assets.GetCertificateXPoE(poetmp);


                    CertificatePoE cert = _listCetificate.OrderByDescending(x => x.Version).FirstOrDefault();

                    string certnumber = "";
                    if (cert != null)
                    {
                        certnumber = cert.CertificateNumber;
                    }

                    
                    WeightSetHeader ww = new WeightSetHeader()
                    {

                        PoE = poetmp.PieceOfEquipmentID,
                        Serial = poetmp.SerialNumber,
                        Ref = certnumber,
                        CalibrationDueDate = poetmp.DueDate.ToString("yyyy-MM-dd"),
                        Note = poetmp.Notes

                    };
                    listw.Add(ww);
                }
            }

            string _AsLeftResult = "";
            string _AsFoundResult = "";


            var eqTemp = poe.EquipmentTemplate;
            var weigthSets = poes.FirstOrDefault().WeightSets;
           
            string manufName;
            if (eqTemp.Manufacturer1 != null)
            {
                manufName = eqTemp.Manufacturer1.Name;
            }
            else
            {
                manufName = null;
            }


            string customerOrder = "" ;
            string customerPO = "";
            string customerAddress1 = "";
            string customerAddress2 = "";
            string customerAddressCity = "";
            string customerAddressCountry = "";
            string customerAddressState = "";
            string shipTo = "";
            string shipVia = "";
            var address = customer.Aggregates.FirstOrDefault().Addresses.FirstOrDefault();
            if (!string.IsNullOrEmpty(poe.PieceOfEquipmentID))
            { 
                customerOrder = poe.PieceOfEquipmentID;
                customerPO = poe.PieceOfEquipmentID;
            }
            if (!string.IsNullOrEmpty(address.StreetAddress1))
                customerAddress1 = address.StreetAddress1;

            if (!string.IsNullOrEmpty(address.StreetAddress2))
                customerAddress2 = address.StreetAddress2;                                                                                                                                                                                                                         ;
            
            if (!string.IsNullOrEmpty(address.StreetAddress3))
                customerAddressCity = address.StreetAddress3;
            
            if (!string.IsNullOrEmpty(address.Country))
                customerAddressCountry = address.Country;
           
            if (!string.IsNullOrEmpty(address.State))
                customerAddressState = address.State;

            var techApproved = await GetTechnician(workOrderDetail1, "aproved");
            var techReview = await GetTechnician(workOrderDetail1, "review");


            GenericHeader header = new GenericHeader()
            {
                
                CustomerOrder= customerOrder,
                CustomerPO = customerPO,
                CustomerAddress1 = customerAddress1,
                CustomerAddress2 = customerAddress2,
                CustomerAddressCity = customerAddressCity,
                CustomerAddressCountry = customerAddressCountry,
                CustomerAddressState = customerAddressState,
                ShipTo = "",
                ShipVia = "123",
                TechnicianApproved = techApproved,
                TechnicianReview = techReview,
                CalibrationDate = workOrderDetail1.CalibrationDate.HasValue ? workOrderDetail1.CalibrationDate.Value.ToString("MM/dd/yyyy") : string.Empty,
                CalibrationDueDate = workOrderDetail1.CalibrationCustomDueDate.HasValue ? workOrderDetail1.CalibrationCustomDueDate.Value.ToString("MM/dd/yyyy") : string.Empty,
            };


            
            string headerJson = System.Text.Json.JsonSerializer.Serialize(header);

      

            string Title = "Header";
            var titledJson = $"{{\"{Title}\":{headerJson}}}"; 
            
            
            return header;

        }
        public async Task<Reports.Domain.ReportViewModels.HeaderMaxPro> GetHeaderMaxPro(WorkOrderDetail wo)
        {


            //1 get GenericCalibrationResult2
            List<GenericCalibrationResult2> testpoints = new List<GenericCalibrationResult2>();
            List<string> extendedObjectList = null;
            HeaderMaxPro headerMaxPro = new HeaderMaxPro();

            try
            {
                if (wo.BalanceAndScaleCalibration != null && wo.BalanceAndScaleCalibration.TestPointResult != null && wo.BalanceAndScaleCalibration.TestPointResult.Count() > 0)
                {

                    JsonObject jsonObjectShiping = new JsonObject();
                    JsonObject jsonObjectServiceManagerComments = new JsonObject();
                    JsonObject jsonObjectCalibrationAdditional = new JsonObject();
                    JsonObject jsonObjectServiceDetails = new JsonObject();
                    JsonObject jsonObjectContactInformation = new JsonObject();
                    JsonObject jsonObjectTechComments = new JsonObject();
                    JsonObject jsonObjectCertificateInformation = new JsonObject();
                    JsonObject jsonObjectCalibrationOnSite = new JsonObject();

                    var calibrationSubtype = await GetCalibrationSubTypes();
                    calibrationSubtype = calibrationSubtype?.Where(x => x.CalibrationTypeId == wo.CalibrationTypeID && x.CalibrationSubTypeView.AccordionType != null && x.CalibrationSubTypeView.AccordionType.ToUpper() == "section".ToUpper()).ToList();

                    var csShippingInformation = calibrationSubtype.Where(x => x.Name.ToUpper() == "ShippingInformation".ToUpper()).Select(x => x.CalibrationSubTypeId).FirstOrDefault();
                    var csServiceManagerComments = calibrationSubtype.Where(x => x.Name.ToUpper() == "ServiceManagerComments".ToUpper()).Select(x => x.CalibrationSubTypeId).FirstOrDefault();
                    var csCalibrationAdditionalInformation = calibrationSubtype.Where(x => x.Name.ToUpper() == "CalibrationAdditionalInformation".ToUpper()).Select(x => x.CalibrationSubTypeId).FirstOrDefault();
                    var csServiceDetails = calibrationSubtype.Where(x => x.Name.ToUpper() == "ServiceDetails".ToUpper()).Select(x => x.CalibrationSubTypeId).FirstOrDefault();
                    var csContactInformation = calibrationSubtype.Where(x => x.Name.ToUpper() == "ContactInformation".ToUpper()).Select(x => x.CalibrationSubTypeId).FirstOrDefault();
                    var csTechComments = calibrationSubtype.Where(x => x.Name.ToUpper() == "TechComments".ToUpper()).Select(x => x.CalibrationSubTypeId).FirstOrDefault();
                    var csCertificateInformation = calibrationSubtype.Where(x => x.Name.ToUpper() == "CertificateInformation".ToUpper()).Select(x => x.CalibrationSubTypeId).FirstOrDefault();
                    var csCalibrationOnSite = calibrationSubtype.Where(x => x.Name.ToUpper() == "CalibrationOnSite".ToUpper()).Select(x => x.CalibrationSubTypeId).FirstOrDefault();

                    string shipingInformation = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == csShippingInformation).Select(x => x.ExtendedObject).FirstOrDefault();
                    string serviceManagerComments = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == csServiceManagerComments).Select(x => x.ExtendedObject).FirstOrDefault();
                    string calibrationAdditionalInformation = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == csCalibrationAdditionalInformation).Select(x => x.ExtendedObject).FirstOrDefault();
                    string serviceDetails = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == csServiceDetails).Select(x => x.ExtendedObject).FirstOrDefault();
                    string contactInformation = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == csContactInformation).Select(x => x.ExtendedObject).FirstOrDefault();
                    string techComments = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == csTechComments).Select(x => x.ExtendedObject).FirstOrDefault();
                    string certificateInformation = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == csCertificateInformation).Select(x => x.ExtendedObject).FirstOrDefault();
                    string calibrationonsite = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == csCalibrationOnSite).Select(x => x.ExtendedObject).FirstOrDefault();
                   
                    if (!string.IsNullOrEmpty(shipingInformation))
                    {
                        jsonObjectShiping = JsonNode.Parse(shipingInformation)?.AsObject();

                    }
                    if (!string.IsNullOrEmpty(serviceManagerComments))
                    {
                        jsonObjectServiceManagerComments = JsonNode.Parse(serviceManagerComments)?.AsObject();

                    }
                    if (!string.IsNullOrEmpty(calibrationAdditionalInformation))
                    {
                        jsonObjectCalibrationAdditional = JsonNode.Parse(calibrationAdditionalInformation)?.AsObject();

                    }
                    if (!string.IsNullOrEmpty(serviceDetails))
                    {
                        jsonObjectServiceDetails = JsonNode.Parse(serviceDetails)?.AsObject();

                    }
                    if (!string.IsNullOrEmpty(contactInformation))
                    {
                        jsonObjectContactInformation = JsonNode.Parse(contactInformation)?.AsObject();

                    }
                    if (!string.IsNullOrEmpty(techComments))
                    {
                        jsonObjectTechComments = JsonNode.Parse(techComments)?.AsObject();

                    }

                    if (!string.IsNullOrEmpty(certificateInformation))
                    {
                        jsonObjectCertificateInformation = JsonNode.Parse(certificateInformation)?.AsObject();

                    }

                    if (!string.IsNullOrEmpty(calibrationonsite))
                    {
                        jsonObjectCalibrationOnSite = JsonNode.Parse(calibrationonsite)?.AsObject();

                    }

                    int uomid = (int) wo.PieceOfEquipment?.EquipmentTemplate?.UnitofmeasurementID;
                    UnitOfMeasure uom = new UnitOfMeasure
                    {
                        UnitOfMeasureID = uomid
                    };

                    var uom_ = await UoMRepository.GetByID(uom);
                    string abreviatureuom;
                    if (uom_ != null)
                    {
                        abreviatureuom = uom_.Abbreviation;
                    }
                    else
                    {

                        abreviatureuom = "NA";
                    }
                    string method = "NA";
                    if (wo.CertificationID != null && wo.CertificationID != 0 )
                    {
                        var result = await Basics.GetCertifications();
                        method = result.Where(x => x.CalibrationTypeID == wo.CalibrationTypeID).FirstOrDefault().Name;
                    }

                    var techApproved = await GetTechnician(wo, "aproved");
                    var techReview = await GetTechnician(wo, "review");
                    var toleranceObject = System.Text.Json.JsonSerializer.Deserialize<Tolerance>(wo.JsonTolerance);
                    double ToleranceValue = toleranceObject?.ToleranceValue ?? 0.0;
                    double ToleranceTypeId = toleranceObject?.ToleranceTypeID ?? 0.0;
                    var fullscale = toleranceObject?.FullScale ?? false;
                    string tolerance = "";
                  

                    // Acces to ToleranceListDynamic y and filter by 

                    var appState = new AppState();
                    var filteredToleranceList = appState.ToleranceListDynamic?
                                                 .FirstOrDefault(t => t.Key == ToleranceTypeId.ToString())
                                                 ?? appState.ToleranceListNoDynamic?
                                                 .FirstOrDefault(t => t.Key == ToleranceTypeId.ToString());

                    if (fullscale)
                    {
                        tolerance = filteredToleranceList.Value + ": " + ToleranceValue + " FullScale";

                    }
                    else
                    {
                        tolerance = filteredToleranceList.Value + ": " + ToleranceValue;
                    }

                    var cust = wo.WorkOder?.Customer?.Name;

                
                    var IsAcalibrationOnSite = bool.TryParse(jsonObjectCalibrationOnSite?["IsAcalibrationOnSite"]?[2]?.ToString(), out var res) && res;

                    string CompanyName = "";
                    string Address1 = "";
                    string Address2 = "";
                    string City = "";
                    string ZipCode = "";
                    string State = "";

                    string CompanyNameFor = "";
                    string Address1For = "";
                    string Address2For = "";
                    string CityFor = "";
                    string ZipCodeFor = "";
                    string StateFor = "";

                    if (IsAcalibrationOnSite)
                    {
                       if (jsonObjectCalibrationOnSite?["CalibrationOnSiteAddress"]?[2] != null) 
                        {
                            var input = jsonObjectCalibrationOnSite?["CalibrationOnSiteAddress"]?[2].ToString();
                            var parts = input.Split('-', 2)[1]
                                        .Split(';')
                                        .Select(x => x.Trim())
                        .ToArray();
                         
                            CompanyName = parts.ElementAtOrDefault(0) ?? "";
                            Address1  = parts.ElementAtOrDefault(1) ?? "";
                            Address2  = parts.ElementAtOrDefault(2) ?? "";
                            City = parts.ElementAtOrDefault(3) ?? "";
                            ZipCode = parts.ElementAtOrDefault(4) ?? "";
                            State = parts.ElementAtOrDefault(5) ?? "";

                        }
                       else if (jsonObjectCalibrationOnSite?["CompanyName"]?[2] != null )
                        {

                            CompanyName = jsonObjectCalibrationOnSite?["CompanyName"]?[2]?.ToString() ?? "";
                            Address1 = jsonObjectCalibrationOnSite?["Address1"]?[2]?.ToString() ?? "";
                            Address2 = jsonObjectCalibrationOnSite?["Address2"]?[2]?.ToString() ?? "";
                            City = jsonObjectCalibrationOnSite?["City"]?[2]?.ToString() ?? "";
                            ZipCode = jsonObjectCalibrationOnSite?["ZipCode"]?[2]?.ToString() ?? "";
                            State = jsonObjectCalibrationOnSite?["State"]?[2]?.ToString() ?? "";
                        }
                      else
                        {

                            CompanyName = "Maxpro Corporation";
                            Address1 = "427 Sargon Way";
                            Address2 = "Unit D";
                            City = "Horsham, PA 19044";
                            ZipCode = "215-293-0800";
                        //CalibratedFor1 = "Mortenson Construction",
                        //CalibratedFor2 = "103 Enterprise Drive",
                        //CalibratedFor3 = "",
                        //CalibratedFor4 = "Northwood IA 50459",
                        //CalibratedFor5 = "",

                        }
                    }
                    else
                    {
                        CompanyName = "Maxpro Corporation";
                        Address1 = "427 Sargon Way";
                        Address2 = "Unit D";
                        City = "Horsham, PA 19044";
                        ZipCode = "215-293-0800";

                    }

                    //Calibrated For

                    var addressWo = await Assets.GetAddressByCustomerId(wo?.WorkOder?.CustomerId ?? 0);

                    if (jsonObjectCertificateInformation?["CertificateAddress"]?[2] != null)
                    {

                        var input = jsonObjectCertificateInformation?["CertificateAddress"]?[2].ToString();
                        var parts = (input?.Contains('-') == true
                                     ? input.Split('-', 2)[1].Split(';').Select(x => x.Trim()).ToArray()
                                     : Array.Empty<string>());

                        CompanyNameFor = parts.ElementAtOrDefault(0) ?? "";
                        Address1For = parts.ElementAtOrDefault(1) ?? "";
                        Address2For = parts.ElementAtOrDefault(2) ?? "";
                        CityFor = parts.ElementAtOrDefault(3) ?? "";
                        ZipCodeFor = parts.ElementAtOrDefault(4) ?? "";
                        StateFor = parts.ElementAtOrDefault(5) ?? "";

                    }
                    else
                    {
                        if (addressWo != null)
                        {
                            var address = addressWo.Where(x => x.AddressId == wo?.WorkOder?.AddressId).FirstOrDefault();
                        CompanyNameFor = address?.Name ?? "";
                        Address1For = address?.StreetAddress1 ?? "";
                            Address2For = address?.StreetAddress2 ?? "";
                            CityFor = address?.City ?? "";
                            ZipCodeFor = address?.ZipCode ?? "";
                            StateFor = address?.State ?? "";

                        }

                    }


                    headerMaxPro = new HeaderMaxPro
                    {

                        CalibrationLocation1 = CompanyName,
                        CalibrationLocation2 = Address1,
                        CalibrationLocation3 = Address2,
                        CalibrationLocation4 = City + " " + State,
                        CalibrationLocation5 = ZipCode,
                        State = State,
                        CalibratedFor1 = CompanyNameFor,
                        CalibratedFor2 = Address1For,
                        CalibratedFor3 = Address2For,
                        CalibratedFor4 = CityFor + " " + StateFor,
                        CalibratedFor5 = ZipCodeFor,
                        // jsonObjectCertificateInformation?["CertificateAddress"]?[2]?.ToString(),
                        //CalibratedFor = strCalibratedFor.ToString(),
                        CalibrationCertificate = wo.WorkOrderDetailID.ToString() + "-1",
                        CalibrationDate = wo.CalibrationDate.HasValue ? wo.CalibrationDate.Value.ToString("MM/dd/yyyy") : string.Empty,
                        CalibrationDueDate = wo.CalibrationCustomDueDate.HasValue ? wo.CalibrationCustomDueDate.Value.ToString("MM/dd/yyyy") : string.Empty,
                        UoM = abreviatureuom,
                        Range = wo.PieceOfEquipment?.EquipmentTemplate?.Capacity + " " + wo.PieceOfEquipment?.EquipmentTemplate.Resolution,
                        Accuracy = "NA",
                        Method = method,
                        Temperature = wo.Temperature.ToString() + "F",
                        Humidity = wo.Humidity.ToString() + "% RH",
                        MaxproControl = wo.PieceOfEquipmentId.ToString(),
                        CustomerPO = jsonObjectCertificateInformation?["CertificatePO"]?[2]?.ToString(),
                        CustomerToolId = wo.PieceOfEquipment?.CustomerToolId,
                        EquipmentType = wo.PieceOfEquipment?.EquipmentTemplate?.EquipmentTypeObject.Name,
                        Manufacturer = wo.PieceOfEquipment?.EquipmentTemplate.Manufacturer1?.Name,
                        Model = wo.PieceOfEquipment?.EquipmentTemplate?.Model,
                        Serial = wo.PieceOfEquipment?.SerialNumber,
                        ReceivedCondition = jsonObjectTechComments?["ReceivedEquipmentCondition"]?[2]?.ToString(),
                        ReturnedCondition = jsonObjectTechComments?["ReturnedEquipmentCondition"]?[2]?.ToString(),
                        WorkOrderDetailId = wo.WorkOrderDetailID.ToString(),
                        Description = wo.PieceOfEquipment?.EquipmentTemplate?.EquipmentTypeObject.Name,
                        TechnicianApproved = techApproved,
                        TechnicianReview = techReview,
                        Tolerance = tolerance,
                        Customer = cust

                    };


                }
            }
            catch (Exception ex)
            {
                throw;
            }
           
            return headerMaxPro;
        }

        public async ValueTask<JObject> SplitProperties(string jobject)
        { 
       
            JObject oldObject = JObject.Parse(jobject);
            JObject newObject = new JObject();

            foreach (var property in oldObject.Properties())
            {
                string newKey = Regex.Replace(property.Name, "(\\B[A-Z])", " $1");
                JObject innerObject = new JObject();

                foreach (var subProperty in property.Value)
                {
                    string newSubKey = Regex.Replace(subProperty.ToObject<JProperty>().Name, "(\\B[A-Z])", " $1").ToLower();
                    innerObject.Add(newSubKey, subProperty.ToObject<JProperty>().Value);
                }

                newObject.Add(newKey, innerObject);
            }
         
            return newObject;

        }

        public async ValueTask<List<GenericStandard>> GetGenericStandard(WorkOrderDetail wo)
        {

            string? html = "";
            List<GenericStandard> listw = new List<GenericStandard>();
            var wod = wo;

            //YP
            var equipmentTypeObject = await Basics.GetAllEquipmentType();
            var eqGroup = equipmentTypeObject.AsQueryable().Where(x => x.EquipmentTypeGroupID == wod.PieceOfEquipment.EquipmentTemplate.EquipmentTypeGroupID && x.HasStandard == false).FirstOrDefault();

            var wodStandar = await GetWorkOrderDetailByID(wo,eqGroup.DynamicConfiguration);//wod.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject.DynamicConfiguration);

            if (wodStandar.WOD_Standard != null)
            {
                var standards = wodStandar.WOD_Standard.ToList();

                

                var st = await PieceOfEquipmentRepository.GetPieceOfEquipmentByIDHeader(wodStandar.PieceOfEquipmentId);

                if (standards != null && standards.Count() > 0)
                {


                    foreach (var item in standards)
                    {
                        var poetmp = await PieceOfEquipmentRepository.GetPieceOfEquipmentByIDHeader(item.PieceOfEquipmentID);

                        var _listCetificate = await Assets.GetCertificateXPoE(poetmp);



                        CertificatePoE cert = _listCetificate.OrderByDescending(x => x.Version).FirstOrDefault();

                        string certnumber = "";
                        string calibrationProvider = "";
                        if (cert != null)
                        {
                 
                            certnumber = cert.CertificateNumber;
                            calibrationProvider = cert.CalibrationProvider;
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

                        
                        //var calibrationProvider = cert.CalibrationProvider;
                        string description = "";
                        if (poetmp.EquipmentTemplate.EquipmentTypeGroupID != 134)   //(poetmp.EquipmentTemplate.EquipmentTypeObject.EquipmentTypeID != 8)
                            description = poetmp.EquipmentTemplate.EquipmentTypeGroup.Name + " - " + poetmp.Capacity + " - " + uom1 + " - " + calibrationProvider;
                        else
                            description = poetmp.EquipmentTemplate.EquipmentTypeGroup.Name;

                        GenericStandard ww = new GenericStandard()
                        {
                            StandardId = poetmp.PieceOfEquipmentID,
                            Description = description,
                            DueDate = poetmp.DueDate.ToString("yyyy-MM-dd"),
                            CalibrationDate = poetmp.CalibrationDate.ToString("yyyy-MM-dd")

                        };
                        listw.Add(ww);
                    }


                }
                
            }
            if (wodStandar.WOD_Weights != null && wodStandar.WOD_Weights.Count() > 0)
            {
                var weights = wodStandar.WOD_Weights.DistinctBy(x => x.WeightSet?.PieceOfEquipmentID).ToList();
                List<PieceOfEquipment> standards = new List<PieceOfEquipment>();
                foreach (var w in weights)
                {
                    var weigth = await PieceOfEquipmentRepository.GetWeigthSetById(w.WeightSetID);
                    if (weigth != null)
                    {
                        var standard = await PieceOfEquipmentRepository.GetPieceOfEquipmentByIDHeader(weigth.PieceOfEquipmentID);
                        standards.Add(standard);
                    }
                    
                }

              
                if (standards != null && standards.Count() > 0)
                {


                    foreach (var item in standards)
                    {
                        var poetmp = await PieceOfEquipmentRepository.GetPieceOfEquipmentByIDHeader(item.PieceOfEquipmentID);

                        var _listCetificate = await Assets.GetCertificateXPoE(poetmp);


                        CertificatePoE cert = _listCetificate.OrderByDescending(x => x.Version).FirstOrDefault();

                        string certnumber = "";
                        string calibrationProvider = "";
                        if (cert != null)
                        {
                            certnumber = cert.CertificateNumber;
                            calibrationProvider = cert.CalibrationProvider;
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

                    
                        string description = "";
                       
                        if (poetmp.EquipmentTemplate.EquipmentTypeGroupID != 134)   //(poetmp.EquipmentTemplate.EquipmentTypeObject.EquipmentTypeID != 8)
                            description = poetmp.EquipmentTemplate.EquipmentTypeGroup.Name + " - " + poetmp.Capacity + " - " + uom1 + " - " + calibrationProvider;
                        else
                            description = poetmp.EquipmentTemplate.EquipmentTypeGroup.Name;
                        GenericStandard ww = new GenericStandard()
                        {
                            StandardId = poetmp.PieceOfEquipmentID,
                            Description = description,
                            DueDate = poetmp.DueDate.ToString("yyyy-MM-dd"),
                            CalibrationDate = poetmp.CalibrationDate.ToString("yyyy-MM-dd")

                        };
                        listw.Add(ww);
                    }


                }
            }
            else
            {
                if (wodStandar.CalibrationSubType_Standards != null)
                {
                    var standardsRock = wodStandar.CalibrationSubType_Standards.ToList();
                    if (standardsRock != null && standardsRock.Count() > 0)
                    {
                        foreach (var item in standardsRock)
                        {
                            var poetmp = await PieceOfEquipmentRepository.GetPieceOfEquipmentByIDHeader(item.PieceOfEquipmentID);

                            var _listCetificate = await Assets.GetCertificateXPoE(poetmp);
                            string uom;
                            if (item.Standard.UnitOfMeasure != null)
                            {
                                uom = item.Standard.UnitOfMeasure.Name;
                            }
                            else
                            {
                                uom = "NA";
                            }
                            CertificatePoE cert = _listCetificate.OrderByDescending(x => x.Version).FirstOrDefault();
                            string certnumber = "";
                            string calibrationProvider = "";
                            if (cert != null)
                            {
                                certnumber = cert.CertificateNumber;
                                calibrationProvider = cert.CalibrationProvider;
                            }

                            string description = "";
                            if (poetmp.EquipmentTemplate.EquipmentTypeGroupID != 134)   //(poetmp.EquipmentTemplate.EquipmentTypeObject.EquipmentTypeID != 8)
                                description = poetmp.EquipmentTemplate.EquipmentTypeGroup.Name + " - " + poetmp.Capacity + " - " + uom + " - " + calibrationProvider;
                            else
                                description = poetmp.EquipmentTemplate.EquipmentTypeGroup.Name;


                            GenericStandard www = new GenericStandard()
                            {
                                StandardId = item.PieceOfEquipmentID.ToString(),
                                Description = description,
                                DueDate = item.Standard.DueDate.ToString("yyyy-MM-dd"),
                                CalibrationDate = item.Standard.CalibrationDate.ToString("yyyy-MM-dd")

                            };
                            listw.Add(www);
                        }


                    }
                }
            }


            PieceOfEquipment poeT = new PieceOfEquipment();
            poeT.PieceOfEquipmentID = wod.TemperatureStandardId;
            if (poeT.PieceOfEquipmentID != null)
            {
                var tempStandard = await PieceOfEquipmentRepository.GetPieceOfEquipmentByID(poeT.PieceOfEquipmentID,"", "WorkOrderItem");

                var _listCetificate1 = await Assets.GetCertificateXPoE(tempStandard);


                CertificatePoE cert1 = _listCetificate1.OrderByDescending(x => x.Version).FirstOrDefault();

                string certnumber1 = "";
                string calibrationProvider = "";
                if (cert1 != null)
                {
                    certnumber1 = cert1.CertificateNumber;
                    calibrationProvider = cert1.CalibrationProvider;
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
                string description = "";
                if (tempStandard?.EquipmentTemplate?.EquipmentTypeGroupID != 134)
                    description = tempStandard.EquipmentTemplate.EquipmentTypeGroup.Name + " - " + tempStandard.Capacity + " - " + uom + uom + " - " + calibrationProvider; 
                else
                    description = tempStandard?.EquipmentTemplate?.EquipmentTypeGroup?.Name;

                
                GenericStandard ww1 = new GenericStandard()
                {
                    StandardId = tempStandard.PieceOfEquipmentID,
                    Description = description,
                    DueDate = tempStandard.DueDate.ToString("yyyy-MM-dd"),
                    CalibrationDate = tempStandard.CalibrationDate.ToString("yyyy-MM-dd")

                };
               
                listw.Add(ww1);

            }

            return listw;

        }

        public async ValueTask<string> GetGenericGrid(WorkOrderDetail wo)
        
        {
            try
            {
                var wod = await GetWorkOrderDetailByID(wo,true);
                var wod1 = wo;
                //var genericCalibration = wod.BalanceAndScaleCalibration.GenericCalibration;
                var genericCalibration = wod1.BalanceAndScaleCalibration.GenericCalibration;
                string jsonAgrupado = "";
                List<string> keysToRemove = new List<string>();
                var isAccredited = wod.IsAccredited;
                JObject newObjOrder = new JObject();
                List<JObject> listJson = new List<JObject>();
                if (genericCalibration != null)
                {
                    foreach (var item in genericCalibration)
                    {
                        Dictionary<string, JToken> newObj1 = new Dictionary<string, JToken>();
                        string titledJson = "";
                        if (item.BasicCalibrationResult.ExtendedObject != null && item.BasicCalibrationResult.Updated != null)
                        {
                            var extObj = item.BasicCalibrationResult.ExtendedObject;

                            var obj = JsonConvert.DeserializeObject(extObj);

                            var _extendedObject = JsonConvert.SerializeObject(obj);

                            JObject obj1 = JObject.Parse(_extendedObject);



                            foreach (var prop in obj1)
                            {


                                JArray arr = (JArray)prop.Value;



                                if (arr.Count >= 4 && Convert.ToInt32(Convert.ToDecimal(arr[3])) == 0)
                                {
                                    arr[2] = arr[2].ToString() + " *";
                                    arr.RemoveAt(3);
                                }
                                else if (arr.Count >= 4)
                                {
                                    arr.RemoveAt(3);
                                }

                                var key = arr[0].ToString();
                                var value = prop.Value;

                                newObj1[key] = value;


                                arr.RemoveAt(0);

                                arr[0] = arr[0].ToString();

                            }

                            JObject newObject = new JObject();
                            foreach (var entry in newObj1)
                            {
                                newObject[entry.Key] = entry.Value;
                            }

                           
                            obj1 = newObject;


                            Dictionary<string, int> openWith = new Dictionary<string, int>();
                            foreach (var prop in obj1)
                            {
                                JArray arr = (JArray)prop.Value;


                                openWith.Add(prop.Key, Convert.ToInt16(arr[0]));
                            }

                            List<JProperty> orderedProperties = new List<JProperty>();
                            var orderedPropertiesTrue = obj1.Properties()
                           .Where(p => {
                               JArray arr = (JArray)p.Value;
                               return arr.Count > 2 && (arr[2].Type == JTokenType.Boolean && arr[2].Value<bool>() == true);
                           })
                           .OrderBy(p => openWith.ContainsKey(p.Name) ? openWith[p.Name] : int.MaxValue)
                           .ToList();


                            var orderedPropertiesFalse = obj1.Properties()
                            .Where(p => {
                                JArray arr = (JArray)p.Value;
                                return arr.Count > 2 && (arr[2].Type != JTokenType.Boolean || arr[2].Value<bool>() != true);
                            })
                            .OrderBy(p => openWith.ContainsKey(p.Name) ? openWith[p.Name] : int.MaxValue)
                            .ToList();

                            ///OnlyAcredited


                            if (orderedPropertiesTrue?.Count() > 0 || orderedPropertiesFalse?.Count() > 0)
                            {

                                if (isAccredited == true && orderedPropertiesTrue?.Count > 0)
                                {
                                    orderedProperties = orderedPropertiesFalse
                                   .Concat(orderedPropertiesTrue)
                                   .ToList();
                                }
                                else
                                {
                                    orderedProperties = orderedPropertiesFalse;
                                }
                            }
                            else
                            {
                                orderedProperties = obj1.Properties()
                                   .OrderBy(p => openWith.ContainsKey(p.Name) ? openWith[p.Name] : int.MaxValue)
                                   .ToList();

                            }
                            ///
                            JObject newJsonObject = new JObject();
                            
                            foreach (var prop in orderedProperties)
                            {
                                JArray arr = (JArray)prop.Value;
                                arr.RemoveAt(0);



                                if (arr[0].Type != JTokenType.String)
                                {
                                    string numericValue = arr.ToString().Replace("[", "").Replace("]", "");
                                    numericValue = numericValue.ToString().Replace("\r\n", "").Trim();
                                    string stringValue = numericValue;
                                    arr.First().Replace(stringValue.ToString());
                                }

                                newJsonObject.Add(prop.Name, prop.Value);
          
                            }



                            var jsonActualizado = newJsonObject.ToString().Replace("[", "").Replace("]", "");
                            
                            var Title = wod1.GetCalibrationType().CalibrationSubTypes.FirstOrDefault(x => x.CalibrationSubTypeId == item.BasicCalibrationResult.CalibrationSubTypeId).NameToShow;
                             titledJson = $"{{\"{Title}\":[{jsonActualizado}]}}";

                            dynamic myJson = JsonConvert.DeserializeObject<ExpandoObject>(titledJson);
                            myJson.position = wod1.GetCalibrationType().CalibrationSubTypes.FirstOrDefault(x => x.CalibrationSubTypeId == item.BasicCalibrationResult.CalibrationSubTypeId).Position;

                            //   listJson.Add(JObject.Parse(titledJson));
                            string updatedJson = JsonConvert.SerializeObject(myJson);
                            listJson.Add(JObject.Parse(updatedJson));
                        }
                    }

                    var order = listJson.OrderBy(j => (double)j.Property("position") );

                    var grupos = order.GroupBy(
                        j => (string)j.Properties().First().Name,
                        j => j[(string)j.Properties().First().Name]
                    );

                   
                    var dicc = grupos.ToDictionary(
                        g => g.Key,
                        g => g.SelectMany(j => (JArray)j).ToList()
                    );

                    //// Add Notes if Exist for each grid
                    //// - Buscar las notas
                    //var notes = await GetGenericStatement(wod1);
                    //Dictionary<string, string> newElement = new Dictionary<string, string>();

                    //foreach (var dicc1 in dicc)
                    //{
                    //   var notesforgrid = notes.Where(x=>x.DataGrid == dicc1.Key).ToList();

                    //    if (notesforgrid != null)
                    //    {
                    //        foreach (var note in notesforgrid)
                    //        {
                    //                newElement = new Dictionary<string, string>
                    //            {
                    //                { "Statement: ", note.Statement.ToString()}
                    //                };

                    //            dicc1.Value.Add(JToken.FromObject(newElement));
                    //            //List<JToken> tokenList = newElement.Values.Select(value => JToken.Parse(value)).ToList();
                    //            //dicc.TryAdd(newElement.Keys.First(), tokenList);
                    //        }
                            
                    //    }
                        
                    //}

                    jsonAgrupado = JsonConvert.SerializeObject(dicc);
                }
            return jsonAgrupado;

            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public async ValueTask<string> GetGenericGrid2(WorkOrderDetail wod)

        {
            try
            {
                
               var wod1 = await GetByID(wod);
                var calibrationSubtypes = await Repository.GetCalibrationSubType();
                var subtypeList = calibrationSubtypes.Where(x => x.CalibrationSubTypeViewID != null && x.CalibrationSubTypeView.AccordionType != "Section");
                var subtypeIds = subtypeList.Select(x => x.CalibrationSubTypeId).ToHashSet();
                var isAccredited = wod.IsAccredited;
                //var genericCalibration = wod.BalanceAndScaleCalibration.TestPointResult.Where(x=>x.Updated != null).OrderBy(x=>x.Position).ToList();
                var genericCalibration = wod.BalanceAndScaleCalibration.TestPointResult
                             .Where(x => x.Updated != null
                               && subtypeIds.Contains(x.CalibrationSubTypeId)
                               && !string.IsNullOrWhiteSpace(x.ExtendedObject)
                                   && x.ExtendedObject.Trim() != "{}")
                              .OrderBy(x => x.Position)
                              .ToList();


                string jsonAgrupado = "";
                List<string> keysToRemove = new List<string>();
               
                JObject newObjOrder = new JObject();
                List<JObject> listJson = new List<JObject>();
                
                string ComponentId = wod1.PieceOfEquipmentId;
                var calibrationPoe = await PieceOfEquipmentRepository.GetPieceOfEquipmentByID(ComponentId, "WorkOrderItem");

                List<GenericCalibrationResult2> genericCalibrationResult2Poe = new List<GenericCalibrationResult2>();
                if (calibrationPoe != null )
                {
                     genericCalibrationResult2Poe = calibrationPoe.TestPointResult.Where(x => x.CalibrationSubTypeId == 39).ToList();

                }
                

                if (genericCalibration != null)
                {
                    if (genericCalibrationResult2Poe.Count() > 0)
                    {
                        int pos = genericCalibration.Count();
                        foreach (var gcr2 in genericCalibrationResult2Poe)
                        {

                            genericCalibration.Add(gcr2);
                        }
                    }



                    foreach (var item in genericCalibration)
                    {
                        Dictionary<string, JToken> newObj1 = new Dictionary<string, JToken>();
                        string titledJson = "";
                        if (item.ExtendedObject != null)
                        {
                            var extObj = item.ExtendedObject;

                            var obj = JsonConvert.DeserializeObject(extObj);

                            var _extendedObject = JsonConvert.SerializeObject(obj);

                            JObject obj1 = JObject.Parse(_extendedObject);



                            foreach (var prop in obj1)
                            {


                                JArray arr = (JArray)prop.Value;



                                if (arr.Count >= 4 && Convert.ToInt32(Convert.ToDecimal(arr[3])) == 0 )
                                {
                                    arr[2] = arr[2].ToString() + " *";
                                    arr.RemoveAt(3);

                                }
                                else if (arr.Count >= 4)
                                {
                                    arr.RemoveAt(3);
                                }

                                
                                //else if (arr.Count == 5 && Convert.ToInt32(Convert.ToDecimal(arr[4]))==0)
                                //{
                                //    arr[2] = arr[2].ToString() + " *";
                                //    arr.RemoveAt(3);
                                //    arr.RemoveAt(3);

                                //}
                                //else if (arr.Count == 5)
                                //{
                                    
                                //    arr.RemoveAt(3);
                                //    arr.RemoveAt(3);

                                //}

                                var key = arr[0].ToString();
                                var value = prop.Value;

                                newObj1[key] = value;


                                arr.RemoveAt(0);

                                arr[0] = arr[0].ToString();

                            }

                            JObject newObject = new JObject();
                            foreach (var entry in newObj1)
                            {
                                newObject[entry.Key] = entry.Value;
                            }


                            obj1 = newObject;


                            Dictionary<string, int> openWith = new Dictionary<string, int>();
                            foreach (var prop in obj1)
                            {
                                JArray arr = (JArray)prop.Value;


                                openWith.Add(prop.Key, Convert.ToInt16(arr[0]));
                            }

                            List<JProperty> orderedProperties = new List<JProperty>();


                            //var orderedProperties = obj1.Properties()
                            //    .OrderBy(p => openWith.ContainsKey(p.Name) ? openWith[p.Name] : int.MaxValue)
                            //    .ToList();

                            

                            var orderedPropertiesTrue = obj1.Properties()
                             .Where(p => {
                                 JArray arr = (JArray)p.Value;
                                 return arr.Count > 2 && (arr[2].Type == JTokenType.Boolean && arr[2].Value<bool>() == true);
                             })
                             .OrderBy(p => openWith.ContainsKey(p.Name) ? openWith[p.Name] : int.MaxValue)
                             .ToList();


                            var orderedPropertiesFalse = obj1.Properties()
                            .Where(p => {
                                JArray arr = (JArray)p.Value;
                                return arr.Count > 2 && (arr[2].Type != JTokenType.Boolean || arr[2].Value<bool>() != true);
                            })
                            .OrderBy(p => openWith.ContainsKey(p.Name) ? openWith[p.Name] : int.MaxValue)
                            .ToList();

                            ////OnlyAccredited

                            if (orderedPropertiesTrue?.Count() > 0 || orderedPropertiesFalse?.Count() > 0)
                            {

                                if (isAccredited == true && orderedPropertiesTrue?.Count > 0)
                                {
                                    orderedProperties = orderedPropertiesFalse
                                   .Concat(orderedPropertiesTrue)
                                   .ToList();
                                }
                                else
                                {
                                    orderedProperties = orderedPropertiesFalse;
                                }
                            }
                            else
                            {
                                 orderedProperties = obj1.Properties()
                                    .OrderBy(p => openWith.ContainsKey(p.Name) ? openWith[p.Name] : int.MaxValue)
                                    .ToList();

                            }
                            ///
                            JObject newJsonObject = new JObject();

                            foreach (var prop in orderedProperties)
                            {
                                JArray arr = (JArray)prop.Value;
                                arr.RemoveAt(0);

                                
                                if (arr.Count >= 2)
                                {
                                    arr.RemoveAt(1);
                                }

                                if (arr[0].Type != JTokenType.String)
                                {
                                    string numericValue = arr.ToString().Replace("[", "").Replace("]", "");
                                    numericValue = numericValue.ToString().Replace("\r\n", "").Trim();
                                    string stringValue = numericValue;
                                    arr.First().Replace(stringValue.ToString());
                                }

                                newJsonObject.Add(prop.Name, prop.Value);

                            }



                            var jsonActualizado = newJsonObject.ToString().Replace("[", "").Replace("]", "");
                            string Title = "";
                            CalibrationType ct = new CalibrationType();
                            ct.CalibrationTypeId = (int)wod1.CalibrationTypeID;
                            ct = await Basics.GetCalibrationTypeByID(ct);
                            if (item.Position == -1 )
                            {
                                Title = "Data " + ct.CalibrationSubTypes.FirstOrDefault(x => x.CalibrationSubTypeId == item.CalibrationSubTypeId).NameToShow; 
                            }
                            else
                            {
                                Title = ct.CalibrationSubTypes.FirstOrDefault(x => x.CalibrationSubTypeId == item.CalibrationSubTypeId).NameToShow;
                            }
                                titledJson = $"{{\"{Title}\":[{jsonActualizado}]}}";

                            // clean invalid values in Json
                            titledJson = System.Text.RegularExpressions.Regex.Replace(titledJson, @",\s*null\s*(?=[,\}])", "");

                          
                            dynamic myJson = JsonConvert.DeserializeObject<ExpandoObject>(titledJson);
                            myJson.position = ct.CalibrationSubTypes.FirstOrDefault(x => x.CalibrationSubTypeId == item.CalibrationSubTypeId).Position;
                            myJson.groupName = item.GroupName ?? "Default";

                            //   listJson.Add(JObject.Parse(titledJson));
                            string updatedJson = JsonConvert.SerializeObject(myJson);
                            listJson.Add(JObject.Parse(updatedJson));
                        }
                    }

                    var order = listJson.OrderBy(j => (double)j.Property("position"));

                    // First group by calibration type (CalibrationSubType) as before
                    var grupos = order.GroupBy(
                        j => (string)j.Properties().First().Name,
                        j => new {
                            Data = j[(string)j.Properties().First().Name],
                            GroupName = j.Property("groupName")?.Value?.ToString() ?? "Default"
                        }
                    );

                    var finalDict = new Dictionary<string, object>();

                    foreach (var grupo in grupos)
                    {
                        var calibrationType = grupo.Key;
                        var allItems = grupo.SelectMany(x => (JArray)x.Data).ToList();

                        // Check if there are multiple GroupNames in this calibration type
                        var groupNames = grupo.Select(x => x.GroupName).Distinct().ToList();

                        if (groupNames.Count > 1 || (groupNames.Count == 1 && groupNames[0] != "Default"))
                        {
                            // There is GroupName grouping, create sub-groups
                            var subGroups = new Dictionary<string, List<JToken>>();

                            foreach (var item in grupo)
                            {
                                var groupName = item.GroupName;
                                var groupKey = groupName == "Default" ? "General" : groupName;

                                if (!subGroups.ContainsKey(groupKey))
                                {
                                    subGroups[groupKey] = new List<JToken>();
                                }

                                if (item.Data is JArray array)
                                {
                                    subGroups[groupKey].AddRange(array);
                                }
                            }

                            // Add each sub-group as a separate entry
                            foreach (var subGroup in subGroups.OrderBy(x => x.Key))
                            {
                                var subGroupKey = $"{calibrationType} - {subGroup.Key}";
                                finalDict[subGroupKey] = subGroup.Value;
                            }
                        }
                        else
                        {
                            // No GroupName grouping or only "Default", use original behavior
                            finalDict[calibrationType] = allItems;
                        }
                    }

                    jsonAgrupado = JsonConvert.SerializeObject(finalDict);
                }
                return jsonAgrupado;

            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public async ValueTask<string> GetGenericGridForce(WorkOrderDetail wo)

        {
            try
            {
                var wod = await GetWorkOrderDetailByID(wo, true);
                var wod1 = wo;
                //var genericCalibration = wod.BalanceAndScaleCalibration.GenericCalibration;
                var genericCalibration = wod1.BalanceAndScaleCalibration.Forces;
                string jsonAgrupado = "";
                List<string> keysToRemove = new List<string>();

                UnitOfMeasure umm = new UnitOfMeasure();
                umm.UnitOfMeasureID = genericCalibration.FirstOrDefault().UnitOfMeasureId.Value;
                var um1 = (await UoMRepository.GetByID(umm)).Abbreviation;

                var CalSubtypes = await GetCalibrationSubTypes();
                JObject newObjOrder = new JObject();
                List<JObject> listJson = new List<JObject>();
                if (genericCalibration != null)
                {
                    var jsonResults = new List<Dictionary<string, object[]>>();
                    int index = 1;
                    Tolerance tolerance = wod1.Tolerance;
                    foreach (var item in genericCalibration)
                    {
                        if (tolerance != null)
                        {
                            
                            var lowTolerance = ValidTolerance(item.BasicCalibrationResult.Nominal, (int)tolerance.ToleranceTypeID, tolerance.Resolution, tolerance.AccuracyPercentage, "low");
                            var maxTolerance = ValidTolerance(item.BasicCalibrationResult.Nominal, (int)tolerance.ToleranceTypeID, tolerance.Resolution, tolerance.AccuracyPercentage, "max");
                            var toleranceDifference = maxTolerance - lowTolerance;
                            var uncertx2 = (item.BasicCalibrationResult.Uncertanty * 2);
                            if (uncertx2 == 0)
                            {
                                uncertx2 = 1;
                            }

                            var tur = (toleranceDifference / uncertx2);
                            item.BasicCalibrationResult.TUR = tur;
                        }
                        Dictionary<string, JToken> newObj1 = new Dictionary<string, JToken>();
                        var forceResult = new ForceResult()
                        {
                            
                            FS = item.BasicCalibrationResult.FS,
                            Nominal = item.BasicCalibrationResult.Nominal,
                            RUN1 = item.BasicCalibrationResult.RUN1,
                            RUN2 = item.BasicCalibrationResult.RUN2,
                            RUN3 = item.BasicCalibrationResult.RUN3,
                            RUN4 = item.BasicCalibrationResult.RUN4,
                            Error = item.BasicCalibrationResult.Error,
                            ClassRun1 = item.BasicCalibrationResult.ClassRun1,
                            Resolution = item.BasicCalibrationResult.Resolution,
                            MaxErrorNominal = item.BasicCalibrationResult.MaxErrorNominal,
                            Uncertanty = item.BasicCalibrationResult.Uncertanty,
                            Class = item.BasicCalibrationResult.Class,
                            TUR = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(item.BasicCalibrationResult.TUR)))
                        };

                        var forceJson = new Dictionary<string, object[]>
                        {
                            
                            ["FS"] = new object[] { "FS", index++, forceResult.FS.ToString(), true },
                            ["Nominal"] = new object[] { "Nominal (" + um1 + ")" , index++, forceResult.Nominal.ToString(), true },
                            ["RUN1"] = new object[] { "RUN1asFound (" + um1 + ")", index++, forceResult.RUN1.ToString(), true },
                            ["Error"] = new object[] { "Error", index++, forceResult.Error.ToString(), true },
                            ["ClassRun1"] = new object[] { "ClassRun1", index++, forceResult.ClassRun1, true },
                            ["RUN2"] = new object[] { "RUN1asAdjusted (" + um1 + ")", index++, forceResult.RUN2.ToString(), true },
                            ["RUN3"] = new object[] { "RUN2 (" + um1 + ")", index++, forceResult.RUN3.ToString(), true },
                            ["RUN4"] = new object[] { "RUN3 (" + um1 + ")", index++, forceResult.RUN4.ToString(), true },
                            ["Resolution"] = new object[] { "Resolution", index++, forceResult.Resolution, true },
                            ["MaxErrorNominal"] = new object[] { "Max Error", index++, forceResult.MaxErrorNominal.ToString(), true },
                            ["Uncertanty"] = new object[] { "Uncertainty (" + um1 + ")", index++, forceResult.Uncertanty.ToString(), true },
                            ["Class"] = new object[] { "Class", index++, forceResult.Class, true },
                            ["TUR"] = new object[] { "TUR", index++, forceResult.TUR, true }
                        };

                        var _extendedObject = JsonConvert.SerializeObject(forceJson);

                        JObject obj1 = JObject.Parse(_extendedObject);
                        string title = CalSubtypes.FirstOrDefault(x => x.CalibrationSubTypeId == item.BasicCalibrationResult.CalibrationSubTypeId).NameToShow; //wod1.GetCalibrationType().CalibrationSubTypes.FirstOrDefault(x => x.CalibrationSubTypeId == item.BasicCalibrationResult.CalibrationSubTypeId).NameToShow;
                        listJson = await GetJsonGroup(obj1, listJson, title, 1);


                    }

                    if (wod.IncludeASTM)
                    {
                        foreach (var item in genericCalibration)
                        {
                            Dictionary<string, JToken> newObj1 = new Dictionary<string, JToken>();
                            var forceResult = new ForceResult()
                            {
                                
                                FS = item.BasicCalibrationResult.FS,
                                Nominal = item.BasicCalibrationResult.Nominal,
                                RUN1 = item.BasicCalibrationResult.RUN1,
                                ErrorRun1 = item.BasicCalibrationResult.ErrorRun1,
                                ErrorpRun1 = item.BasicCalibrationResult.ErrorpRun1,
                                RUN2 = item.BasicCalibrationResult.RUN2,
                                ErrorRun2 = item.BasicCalibrationResult.ErrorRun2,
                                ErrorpRun2 = item.BasicCalibrationResult.ErrorpRun2,
                                Repeatability = item.BasicCalibrationResult.Repeatability,
                                MaxErrorNominal = item.BasicCalibrationResult.MaxErrorNominal,
                                Uncertanty = item.BasicCalibrationResult.Uncertanty,
                                TUR = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(item.BasicCalibrationResult.TURAstm)))

                            };

                            var forceJson = new Dictionary<string, object[]>
                            {
                                
                                ["FS"] = new object[] { "FS", index++, forceResult.FS.ToString(), true },
                                ["Nominal"] = new object[] { "Nominal (" + um1 + ")", index++, forceResult.Nominal.ToString(), true },
                                ["RUN1"] = new object[] { "RUN1asFound (" + um1 + ")", index++, forceResult.RUN1.ToString(), true },
                                ["ErrorRun1"] = new object[] { "Error Run1", index++, forceResult.ErrorRun1.ToString(), true },
                                ["ErrorpRun1"] = new object[] { "Error Run1 %", index++, forceResult.ErrorpRun1, true },
                                ["RUN2"] = new object[] { "Run2 (" + um1 + ")", index++, forceResult.RUN2.ToString(), true },
                                ["ErrorRun2"] = new object[] { "Error Run2", index++, forceResult.ErrorRun2.ToString(), true },
                                ["ErrorpRun2"] = new object[] { "Error Run 2 %", index++, forceResult.ErrorpRun2.ToString(), true },
                                ["Repeatability"] = new object[] { "Repeatability", index++, forceResult.Repeatability, true },
                                ["MaxErrorNominal"] = new object[] { "Max Error %", index++, forceResult.MaxErrorNominal.ToString(), true },
                                ["Uncertanty"] = new object[] { "Uncertainty (" + um1 + ")", index++, forceResult.Uncertanty.ToString(), true },
                                ["TUR"] = new object[] { "TUR", index++, forceResult.TURAstm.ToString(), true },

                            };



                            var _extendedObject = JsonConvert.SerializeObject(forceJson);

                            JObject obj1 = JObject.Parse(_extendedObject);
                            string title = CalSubtypes.FirstOrDefault(x => x.CalibrationSubTypeId == item.BasicCalibrationResult.CalibrationSubTypeId).NameToShow;
                            listJson = await GetJsonGroup(obj1, listJson, "ASTM", 1);

                        }

                    }
                    var order = listJson.OrderBy(j => (double)j.Property("position"));

                    var grupos = order.GroupBy(
                        j => (string)j.Properties().First().Name,
                        j => j[(string)j.Properties().First().Name]
                    );


                    var dicc = grupos.ToDictionary(
                        g => g.Key,
                        g => g.SelectMany(j => (JArray)j).ToList()
                    );



                    jsonAgrupado = JsonConvert.SerializeObject(dicc);
                }
                return jsonAgrupado;

            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public async ValueTask<string> GetGenericGridBalance(WorkOrderDetail wo)

        {
            try
            {
                var wod = wo;//await GetWorkOrderDetailByID(wo, true);
                var wod1 = wo;
                //var genericCalibration = wod.BalanceAndScaleCalibration.GenericCalibration;
                var genericCalibration = wod1.BalanceAndScaleCalibration;
                string jsonAgrupado = "";
                List<string> keysToRemove = new List<string>();
                List<Linearity> linearities = new List<Linearity>();
                List<Domain.Aggregates.Entities.Repeatability> repeatabilities = new List<Domain.Aggregates.Entities.Repeatability>();
                List<Eccentricity> eccentricities = new List<Eccentricity>();

                List<JObject> listJson = new List<JObject>();
                string customer = Configuration.GetSection("Reports")["Customer"];
                var balanceReport = await GetWorkOrderDetailXIdRepWithSave(wo, customer, false);
                List<List<JObject>> listOfListsJson = new List<List<JObject>>();
                if (balanceReport != null)
                {
                    Dictionary<string, object[]> balanceJson = new Dictionary<string, object[]>();

                    foreach (var item in balanceReport.AsFoundList)
                    {

                        if (balanceReport.AsFoundList != null)
                        {

                            balanceJson = new Dictionary<string, object[]>
                            {

                                ["Standard"] = new object[] { "Standard", 1, item.Standard, true },
                                ["Indication"] = new object[] { "Indication", 2, item.Indication, true },
                                ["Tolerance"] = new object[] { "Tolerance", 3, item.Tolerance, true },
                                ["Range"] = new object[] { "Range", 4, item.Range.ToString(), true },
                                ["PassFail"] = new object[] { "Pass/Fail", 5, item.PassFail, true },
                                ["Uncertainty"] = new object[] { "Uncertainty", 6, item.Uncertainty, true },
                                ["Position"] = new object[] { "Position", 7, item.Position.ToString(), true },
                                ["Description"] = new object[] { "Description", 8, item.Description, true },
                                ["UOM"] = new object[] { "Unit of Measure", 9, item.UOM, true },
                                ["Value"] = new object[] { "Value", 10, item.Value, true }
                            };


                        }
                        var _extendedObject = JsonConvert.SerializeObject(balanceJson);
                        JObject obj1 = JObject.Parse(_extendedObject);

                        
                        listJson = await GetJsonGroup(obj1, listJson, "Linearity As Found", 1);
                        

                    }
                    foreach (var item in balanceReport.AsLeftList)
                    {

                        if (balanceReport.AsLeftList != null )
                        {

                            balanceJson = new Dictionary<string, object[]>
                            {

                                ["Standard"] = new object[] { "Standard", 1, item.Standard, true },
                                ["Indication"] = new object[] { "Indication", 2, item.Indication, true },
                                ["Tolerance"] = new object[] { "Tolerance", 3, item.Tolerance, true },
                                ["Range"] = new object[] { "Range", 4, item.Range.ToString(), true },
                                ["PassFail"] = new object[] { "Pass/Fail", 5, item.PassFail, true },
                                ["Uncertainty"] = new object[] { "Uncertainty", 6, item.Uncertainty, true },
                                ["Position"] = new object[] { "Position", 7, item.Position.ToString(), true },
                                ["Description"] = new object[] { "Description", 8, item.Description, true },
                                ["UOM"] = new object[] { "Unit of Measure", 9, item.UOM, true },
                                ["Value"] = new object[] { "Value", 10, item.Value, true }
                            };


                        }
                        var _extendedObject = JsonConvert.SerializeObject(balanceJson);
                        JObject obj1 = JObject.Parse(_extendedObject);

                        listJson = await GetJsonGroup(obj1, listJson, "Linearity As Left", 2);

                    }
                    
                    foreach (var item in balanceReport.eccList)
                    {

                        if (balanceReport.eccList != null)  // && balanceReport.Eccentricity != null
                        {

                            balanceJson = new Dictionary<string, object[]>
                            {
                                ["Position"] = new object[] { "Position", 1, item.Position.ToString(), true },
                                ["AsFound"] = new object[] { "As Found", 2, item.AsFound, true },
                                ["AsLeft"] = new object[] { "As Left", 2, item.AsLeft, true },
                                ["PassFail"] = new object[] { "Pass/Fail", 5, item.PassFail, true },
                                ["Tolerance"] = new object[] { "Tolerance", 3, item.Tolerance, true },
                                ["Weight"] = new object[] { "Weight", 4, item.Weight.ToString(), true },
                                ["PassFailAsFound"] = new object[] { "Pass Fail As Found", 5, item.PassFailAsFound, true },
                                ["PassFailAsLeft"] = new object[] { "Pass Fail As Left", 5, item.PassFailAsLeft, true },
                            };


                        }
                        var _extendedObject = JsonConvert.SerializeObject(balanceJson);
                        JObject obj1 = JObject.Parse(_extendedObject);

                        listJson = await GetJsonGroup(obj1, listJson, "Repeatability", 3);

                    }

                    foreach (var item in balanceReport.RepeteabilityList) 
                    {

                        if (balanceReport.RepeteabilityList != null) // balanceReport.Repeteability != null
                        {

                            balanceJson = new Dictionary<string, object[]>
                            {
                                ["Position"] = new object[] { "Position", 1, item.Position.ToString(), true },
                                ["AsFound"] = new object[] { "As Found", 2, item.AsFound, true },
                                ["AsLeft"] = new object[] { "As Left", 2, item.AsLeft, true },
                                ["PassFail"] = new object[] { "Pass/Fail", 5, item.PassFail, true },
                                ["Tolerance"] = new object[] { "Tolerance", 3, item.Tolerance, true },
                                ["StandarDerivation"] = new object[] { "Standar Derivation", 4, item.StandarDerivation.ToString(), true },
                                ["PassFailAsFound"] = new object[] { "Pass Fail As Found", 5, item.PassFailAsFound, true },
                                ["PassFailAsLeft"] = new object[] { "Pass Fail As Left", 5, item.PassFailAsLeft, true },
                            };

                        }
                        var _extendedObject = JsonConvert.SerializeObject(balanceJson);
                        JObject obj1 = JObject.Parse(_extendedObject);

                        listJson = await GetJsonGroup(obj1, listJson, "Eccentricity", 4);

                    }

                    var order = listJson.OrderBy(j => (double)j.Property("position"));

                        var grupos = order.GroupBy(
                            j => (string)j.Properties().First().Name,
                            j => j[(string)j.Properties().First().Name]
                        );


                        var dicc = grupos.ToDictionary(
                            g => g.Key,
                            g => g.SelectMany(j => (JArray)j).ToList()
                        );



                        jsonAgrupado = JsonConvert.SerializeObject(dicc);
                    
                    
                }
                return jsonAgrupado;
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public async Task<List<JObject>> GetJsonGroup(JObject obj1, List<JObject> listJson, string title, int position)

        {
            Dictionary<string, JToken> newObj1 = new Dictionary<string, JToken>();
            
            foreach (var prop in obj1)
            {


                JArray arr = (JArray)prop.Value;



                if (arr.Count >= 4 && Convert.ToInt32(Convert.ToDecimal(arr[3])) == 0)
                {
                    arr[2] = arr[2].ToString() + " *";
                    arr.RemoveAt(3);
                }
                else if (arr.Count >= 4)
                {
                    arr.RemoveAt(3);
                }

                var key = arr[0].ToString();
                var value = prop.Value;

                newObj1[key] = value;


                arr.RemoveAt(0);

                arr[0] = arr[0].ToString();

            }

            JObject newObject = new JObject();
            foreach (var entry in newObj1)
            {
                newObject[entry.Key] = entry.Value;
            }


            obj1 = newObject;


            Dictionary<string, int> openWith = new Dictionary<string, int>();
            foreach (var prop in obj1)
            {
                JArray arr = (JArray)prop.Value;


                openWith.Add(prop.Key, Convert.ToInt16(arr[0]));
            }

            var orderedProperties = obj1.Properties()
                .OrderBy(p => openWith.ContainsKey(p.Name) ? openWith[p.Name] : int.MaxValue)
                .ToList();


            JObject newJsonObject = new JObject();

            foreach (var prop in orderedProperties)
            {
                JArray arr = (JArray)prop.Value;
                arr.RemoveAt(0);

                if (arr[0].Type != JTokenType.String)
                {
                    string numericValue = arr.ToString().Replace("[", "").Replace("]", "");
                    numericValue = numericValue.ToString().Replace("\r\n", "").Trim();
                    string stringValue = numericValue;
                    arr.First().Replace(stringValue.ToString());
                }

                newJsonObject.Add(prop.Name, prop.Value);

            }



            var jsonActualizado = newJsonObject.ToString().Replace("[", "").Replace("]", "");

            var Title = title;// wod1.GetCalibrationType().CalibrationSubTypes.FirstOrDefault(x => x.CalibrationSubTypeId ==  "" item.BasicCalibrationResult.CalibrationSubTypeId).NameToShow;
            var titledJson = $"{{\"{Title}\":[{jsonActualizado}]}}";

            dynamic myJson = JsonConvert.DeserializeObject<ExpandoObject>(titledJson);
            myJson.position = position;// wod1.GetCalibrationType().CalibrationSubTypes.FirstOrDefault(x => x.CalibrationSubTypeId == item.BasicCalibrationResult.CalibrationSubTypeId).Position;

            
            string updatedJson = JsonConvert.SerializeObject(myJson);
            listJson.Add(JObject.Parse(updatedJson));

            return listJson;
        
        }


    public async ValueTask<GenericCustomerInstrument> GetGenericInstrument(WorkOrderDetail wo)
        {
            var workOrderDetail1 = wo;//await GetByID(wo);
             var workOrderDetail = workOrderDetail1;//await GetWorkOrderDetailByID(wo);

            var workOrder = workOrderDetail1.WorkOder;
            var customer = workOrder.Customer;

            //var POE = await Logicpoe.GetPieceOfEquipmentByID(workOrderDetail1.PieceOfEquipment);

            //var address = customer.Aggregates[0];
            var poe = workOrderDetail1.PieceOfEquipment;  // customer.PieceOfEquipment.Where(x => x.WorOrderDetailID == wo.WorkOrderDetailID).FirstOrDefault();

            List<WeightSetHeader> listw = new List<WeightSetHeader>();

            var poes = await PieceOfEquipmentRepository.GetAllWeightSets(poe);

            var eqTemp = poe.EquipmentTemplate;
           

            GenericCustomerInstrument model = new GenericCustomerInstrument();
            string calibrationDate = "";
            DateTime dueDate;
            string due = "";
            DateTime calDate;

            
            string accuracy = "";
            string accuracy1 = "";
            string certificatecomments = "";
            string customerref = "";
            string humidity = "";
            string manufacturer = "";
            string procedure = "";
            string temperature = "";
            string temperatureAfter = "";

            if (!string.IsNullOrEmpty(poe.Tolerance.AccuracyPercentage.ToString()) && workOrderDetail.CertificationID != null)
            {
                var _accuracy = await Basics.GetCertifications();
                accuracy = _accuracy.Where(x => x.ID == workOrderDetail.CertificationID && x.CalibrationTypeID == workOrderDetail.CalibrationTypeID ).FirstOrDefault().Name;
            }
            else
            {
                accuracy = "Customer";
            }

            if (!string.IsNullOrEmpty(workOrderDetail.CertificateComment))
            {
                certificatecomments = workOrderDetail.CertificateComment; 
            }

            if (!string.IsNullOrEmpty(poe.Customer.CustomerID.ToString()))
            {
                customerref = poe.Customer.CustomerID.ToString(); 
            }

            if (!string.IsNullOrEmpty(workOrderDetail.Humidity.ToString()))
            {
                humidity = workOrderDetail.Humidity.ToString("0.0");
            }

            if (poe.EquipmentTemplate.Manufacturer1 != null)
            {
                manufacturer = poe.EquipmentTemplate.Manufacturer1.Name;
            }

            if (!string.IsNullOrEmpty(workOrderDetail?.TestCode?.Procedure?.Name))
            {
                procedure = workOrderDetail.TestCode.Procedure.Name;
            }
            if (!string.IsNullOrEmpty(workOrderDetail.Temperature.ToString()))
            {
                temperature = workOrderDetail.Temperature.ToString("0.0");
            }
            if (!string.IsNullOrEmpty(workOrderDetail.TemperatureAfter.ToString()))
            {
                temperatureAfter = workOrderDetail.TemperatureAfter.ToString("0.0");
            }

            double ToleranceValue = workOrderDetail?.Tolerance?.ToleranceValue ?? 0.0;
            var fullscale = workOrderDetail?.Tolerance?.FullScale ?? false;

            // Acces to ToleranceListDynamic y and filter by 
            double ToleranceTypeId = workOrderDetail?.Tolerance?.ToleranceTypeID ?? 0;
            var appState = new AppState();
            var filteredToleranceList = appState.ToleranceListDynamic?
      .FirstOrDefault(t => t.Key == ToleranceTypeId.ToString())
      ?? appState.ToleranceListNoDynamic?
      .FirstOrDefault(t => t.Key == ToleranceTypeId.ToString());

            if (fullscale)
            {
                accuracy1 = filteredToleranceList.Value + ": " + ToleranceValue + " FullScale";

            }
            else
            {
                accuracy1 = filteredToleranceList.Value + ": " + ToleranceValue;
            }

            if (workOrderDetail.CalibrationCustomDueDate != null)
            {
                dueDate = (DateTime)workOrderDetail.CalibrationCustomDueDate;
                due = dueDate.ToString("yyyy-MM-dd");
            }
            if (workOrderDetail.CalibrationDate != null)
            {
                calDate = (DateTime)workOrderDetail.CalibrationDate;
                calibrationDate = calDate.ToString("yyyy-MM-dd");
            }
            UnitOfMeasure umm = new UnitOfMeasure();
            umm.UnitOfMeasureID = poe.UnitOfMeasureID.Value;
            var um1 = (await UoMRepository.GetByID(umm)).Abbreviation;

            

            string umTemp = "";
            if (workOrderDetail.TemperatureUOMID != 0)
            {
                umm.UnitOfMeasureID = workOrderDetail.TemperatureUOMID;
                umTemp = (await UoMRepository.GetByID(umm)).Abbreviation;
            }
            else
                umTemp = "C";

            var eqCondition = workOrderDetail.EquipmentCondition.ToList();
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
            model = new GenericCustomerInstrument()
            {

                Unit = poe.PieceOfEquipmentID,
                Accuracy = accuracy1,
                CalibrationDate = calibrationDate,
                CalibrationDueDate = due,
                CertificateComments = certificatecomments,
                CustomerRef = poe.CustomerToolId ,
                Description = poe.InstallLocation,
                Humidity = humidity + " %RH",
                Manufacturer = manufacturer,
                Model = poe.EquipmentTemplate.Model,
                Procedure = procedure ,
                RangeRes = poe.Capacity.ToString() + " " + um1 + "/" + poe.Resolution.ToString(),
                ReceivedCondition = received,
                Temperature = "Before Cal: " + temperature + " " + umTemp + "/ After Cal: " + temperatureAfter + " " + umTemp,
                AccuracySpecification = accuracy,
                ReturnedCondition = returned,
                CertificateNumber = wo.WorkOrderDetailID.ToString() + "_" + wo.WorkOrderDetailHash,
                InstallLocation = poe.InstallLocation,
                Serial = poe.SerialNumber,
                UoM = um1


            };


            string headerJson = System.Text.Json.JsonSerializer.Serialize(model);
            var newObject = SplitProperties(headerJson);
            string Title = "Customer Instrument";
            var titledJson = $"{{\"{Title}\":{newObject}}}";

          
            return model;
        }


        public async ValueTask<List<GenericStatement>> GetGenericStatement(WorkOrderDetail wo)
        {
            try
            {
                var workOrderDetail1 = wo;
                var workOrderDetail = workOrderDetail1;

                List<GenericStatement> genericStatements = new List<GenericStatement>();
                genericStatements = await GetNotes(workOrderDetail.PieceOfEquipment, workOrderDetail);

                if (genericStatements != null && genericStatements.Count() > 0)
                {

                    string statementJson = System.Text.Json.JsonSerializer.Serialize(genericStatements);



                    string Title = "Statements";
                    var titledJson = $"{{\"{Title}\":{statementJson}}}";


                    JObject obj = JObject.Parse(titledJson);


                    JArray statements = (JArray)obj["Statements"];


                    foreach (JObject statement in statements)
                    {
                        statement.Remove("Statement");
                    }

                    return genericStatements;
                }
                else if (wo.BalanceAndScaleCalibration.Forces != null && wo.BalanceAndScaleCalibration.Forces.Count() > 0)
                {

                    //var path = Path.Combine(AppContext.BaseDirectory, "Json", "ForceStatements.json");

                    //using FileStream openStream = File.OpenRead(path);
                    string jsonForce = @"[{ ""Statement"": ""Calibration services were performed under a controlled Quality System Program Manual, Rev. 21 dated 5/1/2019 which incorporates the requirements of ISO/IEC 17025:2017, ISO 10012-1, ANSI/INCSL Z640-1 and MIL-STD-45682A."",
    ""DataGrid"": ""Force""
  },
  {
    ""Statement"": ""Calibration services were performed under a controlled Quality System Program Manual, Rev. 21 dated 5/1/2019 which incorporates the requirements of ISO/IEC 17025:2017, ISO 10012-1, ANSI/INCSL Z640-1 and MIL-STD-45682A."",
    ""DataGrid"": ""Force""
  },
  {
    ""Statement"": ""Calibration Data contained in this report obtained from the subject instrument used standards which are traceable to the International System of Units (SI) through National Metrological Institutes (such as,NIST) Intrinsic Standards or other accepted means."",
    ""DataGrid"": ""Force""
  },
  {
    ""Statement"": ""The result herein relates only to the item calibrated and are reflective of conditions at the time of the calibration/test."",
    ""DataGrid"": ""Force""
  },
  {
    ""Statement"": ""This report shall not be reproduced, except in full, without the written permission of Laboratory Testing Inc."",
    ""DataGrid"": ""Force""
  },
  {
    ""Statement"": ""The services provided on this certificate have been performed in conformance with the purchase order requirements (PO number referenced above)."",
    ""DataGrid"": ""Force""
  },
  {
    ""Statement"": ""The recording of false, fictious or fraudulent statements or entries on this document may be punishable as a felony under Federal Statutes."",
    ""DataGrid"": ""Force""
  }
]";

                    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonForce));
                    var jsonStatements = await System.Text.Json.JsonSerializer.DeserializeAsync<List<GenericStatement>>(stream);

                    string Title = "Statements";
                    string titledJson = $"{{\"{Title}\":{System.Text.Json.JsonSerializer.Serialize(jsonStatements)}}}";

                    JObject obj = JObject.Parse(titledJson);
                    JArray statements = (JArray)obj[Title];


                    foreach (JObject statement in statements)
                    {
                        statement.Remove("Statement");
                    }

                    return jsonStatements;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public async Task<List<GenericStatement>> GetNotes(PieceOfEquipment? _poe, WorkOrderDetail? wod)
        {
            List<Note> NotesEqTc = new List<Note>();
            List<Note> notesEqType = new List<Note>();
            List<Note> notesCalibrationType = new List<Note>();

            if (_poe != null)
            {
                // Use the correct EquipmentTypeID for Balance & Scale equipment
                // EquipmentTypeGroupID 57 corresponds to Balance & Scale which should use EquipmentTypeID = 3
                if (_poe.EquipmentTemplate.EquipmentTypeGroupID.HasValue && _poe.EquipmentTemplate.EquipmentTypeGroupID.Value == 57)
                {
                    int correctEquipmentTypeId = 3; // Balance & Scale equipment type
                    notesEqType = await Basics.GetNotes(correctEquipmentTypeId, 1);
                }
                else
                {
                    // Fallback to original method if EquipmentTypeGroupID is not available
                    var eqType = await Basics.GetEquipmentTypeXId(_poe.EquipmentTemplate.EquipmentTypeID);
                    if (eqType != null)
                    {
                        notesEqType = eqType.Notes.OrderBy(x => x.EquipmnetTypeId != null).ThenBy(z => z.Position).ToList();
                    }
                }
            }


            List<Note> notesTestCode = new List<Note>();
            var testCode = await GetTestCodeByID((int)wod.TestCodeID);
            if (testCode != null && testCode.Notes != null)
            {
                notesTestCode = testCode.Notes.OrderBy(y => y.TestCodeID != null).ToList();
            }
            NotesEqTc = notesEqType.Concat(notesTestCode).ToList();
            List<CalibrationSubType> calibrationSubTypes= new List<CalibrationSubType>();
            calibrationSubTypes = await Repository.GetCalibrationSubType();
            List<GenericStatement> genericStatements = new List<GenericStatement>();
            List<Note> notesCondition = new List<Note>();
            foreach (var note in NotesEqTc)
            {

                GenericStatement statement = new GenericStatement();
                statement.Statement = note.Text;
                
                if (note.CalibrationSubtypeId != 0)
                {
                    statement.DataGrid = calibrationSubTypes.FirstOrDefault(x => x.CalibrationSubTypeId == note.CalibrationSubtypeId).NameToShow.ToString();
                }
                else
                {
                    statement.DataGrid = "";
                }

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
                    statement.Statement = noteText;
                    
                }

                ////
                if (note.Condition == 1 && validation)
                {
                    genericStatements.Add(statement);
                }
                else if (wod.IsAccredited == true && note.Condition == 4)
                {
                    genericStatements.Add(statement);
                }
                else if (wod.IsAccredited == false && note.Condition == 5)
                {
                    genericStatements.Add(statement);
                }


            }

            List<NoteWOD> notesWOD = new List<NoteWOD>();
            var notesWOD1 = await Repository.GetNotes(wod.WorkOrderDetailID, 1);
            notesWOD = notesWOD1.OrderBy(x => x.Position).ToList();

            foreach (var note in notesWOD1)
            {
                
                GenericStatement statement = new GenericStatement();
                statement.Statement = note.Text;
                if (note.CalibrationSubtypeId != null && note.CalibrationSubtypeId != 0)
                {
                    statement.DataGrid = calibrationSubTypes.FirstOrDefault(x => x.CalibrationSubTypeId == note.CalibrationSubtypeId).NameToShow.ToString();
                }
                else
                {
                    statement.DataGrid = "";
                }

                genericStatements.Add(statement);

            }

            return genericStatements;
        }

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
                    }



                }

                return res;


            }
            catch (Exception ex)
            {

                return false;

            }


        }
        //public async Task<string> DynamicTextNote(WorkOrderDetail Model, Note note)
        //{
        //    string res = "";
        //    var engine = new Engine();

        //    try
        //    {
        //        string inputString = note.Text;
        //        string script = $"`{inputString}`";

        //        engine.SetValue("model", Model);

        //        var result = engine.Execute(script);
        //       // var x = result.get
        //        string outputString = result.ToString(); // Convertir a cadena

        //        return outputString;
        //    }
        //    catch (Exception ex)
        //    {
        //        return "";
        //    }
        //}
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
                string propertyValue = "";
                if (placeholder == "TemperatureUOMID" || placeholder == "HumidityUOMID")
                {
                    var val = GetPropertyValue(Model, placeholder);

                    UnitOfMeasure umm = new UnitOfMeasure();
                    umm.UnitOfMeasureID = Convert.ToInt16(val);
                    var prop = await UoMRepository.GetByID(umm);
                    if (prop != null)
                    {
                        propertyValue = (await UoMRepository.GetByID(umm)).Abbreviation;  
                    }
                    else 
                    {
                        propertyValue = "";
                    }

                }
                else
                {
                     propertyValue = GetPropertyValue(Model, placeholder); // Obtener el valor de la propiedad
                }
                inputString = inputString.Replace(match.Value, propertyValue);
            }

            return inputString;
        }

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
        public async ValueTask<CalibratedBy> GetCalibratedBy(WorkOrderDetail wo)
        {
            var workOrderDetail1 = wo;
            var workOrderDetail = workOrderDetail1;//await GetWorkOrderDetailByID(wo);

            var workOrder = workOrderDetail1.WorkOder;
            var customer = workOrder.Customer;

            

            CalibratedBy model = new CalibratedBy();

            if (workOrderDetail.Technician != null)
            {
                model = new CalibratedBy()
                {
                    User = "User",
                    Name = workOrderDetail.Technician.Name,
                    Id = workOrderDetail.Technician.UserID.ToString(),
                    CertificationNumber = "",
                    State = ""

                };


                string headerJson = System.Text.Json.JsonSerializer.Serialize(model);
                //var newObject = SplitProperties(headerJson);
                string Title = "Calibrated By";
                var titledJson = $"{{\"{Title}\":{headerJson}}}";


               
            }
            
                return model;
        }
        public async ValueTask<ApprovedBy> GetApprovedBy(WorkOrderDetail wo)
        {
            var workOrderDetail1 = wo; 
            var workOrderDetail = workOrderDetail1;

            var workOrder = workOrderDetail1.WorkOder;
            var customer = workOrder.Customer;

            var poe = workOrderDetail1.PieceOfEquipment;  

            List<WeightSetHeader> listw = new List<WeightSetHeader>();

            var poes = await PieceOfEquipmentRepository.GetAllWeightSets(poe);

            var eqTemp = poe.EquipmentTemplate;


            ApprovedBy model = new ApprovedBy();
            string calibrationDate = "";
            DateTime dueDate;
            string due = "";
            DateTime calDate;
            if (workOrderDetail.CalibrationCustomDueDate != null)
            {
                dueDate = (DateTime)workOrderDetail.CalibrationCustomDueDate;
                due = dueDate.ToString("yyyy-MM-dd");
            }
            if (workOrderDetail.CalibrationDate != null)
            {
                calDate = (DateTime)workOrderDetail.CalibrationDate;
                calibrationDate = calDate.ToString("yyyy-MM-dd");
            }
            if (workOrderDetail.Technician != null)
            {
                model = new ApprovedBy()
                {
                    User = "User",
                    Name = workOrderDetail.Technician.Name,
                    Id = workOrderDetail.Technician.UserID.ToString(),
                    CertificationNumber = "",
                    State = ""

                };


                string headerJson = System.Text.Json.JsonSerializer.Serialize(model);
                var newObject = SplitProperties(headerJson);
                string Title = "Calibrated By";
                var titledJson = $"{{\"{Title}\":{newObject}}}";

            }
            return model;
        }

        public async ValueTask<GenericReport> GetJsonGeneric(WorkOrderDetail wo, string? cust, bool Save = false)
        {
            try
            {
                GenericReport genericReport = new GenericReport();
                var wod = await GetWorkOrderDetailByID(wo, true);
                var wodById = await GetByID(wo);
                
                if(cust == "Maxpro")
                {
                    genericReport.HeaderMaxPro = await GetHeaderMaxPro(wodById);
                }

                var header = await GetGenericHeader(wodById);
                genericReport.Header = header;
                var customerIntrument = await GetGenericInstrument(wodById);
                genericReport.CustomerInstrument = customerIntrument;
                 
                string grid = "";
                if (wodById.BalanceAndScaleCalibration != null && wodById.BalanceAndScaleCalibration.TestPointResult != null && wodById.BalanceAndScaleCalibration.TestPointResult.Count() > 0)
                     grid = await GetGenericGrid2(wodById);
                else if (wodById.BalanceAndScaleCalibration != null && wodById.BalanceAndScaleCalibration.GenericCalibration != null && wodById.BalanceAndScaleCalibration.GenericCalibration.Count() > 0)
                    grid = await GetGenericGrid(wodById);
                else if (wodById.BalanceAndScaleCalibration != null && wodById.BalanceAndScaleCalibration.Forces != null && wodById.BalanceAndScaleCalibration.Forces.Count() > 0)
                    grid = await GetGenericGridForce(wodById);
                else if (wodById.BalanceAndScaleCalibration.Linearities != null || wodById.BalanceAndScaleCalibration.Eccentricity != null || wodById.BalanceAndScaleCalibration.Repeatability != null)
                    grid = await GetGenericGridBalance(wodById);
                if (grid != "")
                genericReport.DataGrids = JObject.Parse(grid);
                var standard = await GetGenericStandard(wodById);
                genericReport.Standards = standard;
                var statements = await GetGenericStatement(wodById);
               // genericReport.Statements = statements;
                genericReport.Statements = statements;
                
                var by = await GetCalibratedBy(wodById);
                genericReport.CalibratedBy = by;
                var approved = await GetApprovedBy(wodById);
                genericReport.ApprovedBy = approved;
                
                
                return genericReport; 

            }
            catch (Exception ex)
            {
                GenericReport genericReport = new GenericReport();
                return genericReport;
            }



        }

        public async Task<string> GetTechnician(WorkOrderDetail wo, string typeTech)
        {

            var status = await GetStatus();

            var history = await GetHistory(wo);


            var laststatus = status.Where(x => x.IsLast == true).FirstOrDefault();

            var aproveduserid = history.Where(x => x.StatusId == laststatus.StatusId).OrderByDescending(x => x.WorkDetailHistoryID).FirstOrDefault();

            var reviewuserid = history.Where(x => x.StatusId == (laststatus.StatusId - 1)).OrderByDescending(x => x.WorkDetailHistoryID).FirstOrDefault();

            //TODO NEW
            string tecnameaproved = string.Empty;
            User useraproved = null;
            if (aproveduserid != null && aproveduserid.TechnicianID.HasValue)
            {
                useraproved = await Basics.GetUserById2(new User() { UserID = aproveduserid.TechnicianID.Value });
                tecnameaproved = useraproved?.Name + " " + useraproved?.LastName;
            }
            string tecnreview = string.Empty;
            if (reviewuserid != null && reviewuserid.TechnicianID.HasValue)
            {
                var userreview = await Basics.GetUserById2(new User() { UserID = reviewuserid.TechnicianID.Value });

                tecnreview = userreview?.Name + " " + userreview?.LastName;

            }

            if (typeTech == "aprove")
            {
                return useraproved?.Name;

            }
            else
            {
                return tecnreview;
            }

        }

        /// <summary>
        /// Constructs the certificate ID in the format: {WorkOrderId}-{Sequence}-Ver{Version}
        /// </summary>
        /// <param name="workOrderDetail">The work order detail</param>
        /// <returns>The constructed certificate ID</returns>
        private async Task<string> ConstructCertificateId(WorkOrderDetail workOrderDetail)
        {
            try
            {
                // Get WorkOrderId from the work order
                int workOrderId = workOrderDetail.WorkOrderID;

                // Extract sequence from WorkOrderDetailUserID (format: "WorkOrderId-Sequence")
                string sequence = "1"; // Default sequence
                if (!string.IsNullOrEmpty(workOrderDetail.WorkOrderDetailUserID))
                {
                    var parts = workOrderDetail.WorkOrderDetailUserID.Split('-');
                    if (parts.Length >= 2)
                    {
                        sequence = parts[1]; // Get the sequence part
                    }
                }

                // Get the latest version from WorkOrderDetailHistory for current status
                int version = 1; // Default version
                var history = await GetHistory(workOrderDetail);
                if (history != null && history.Any())
                {
                    // Get the latest history record for the current status
                    var latestHistoryForCurrentStatus = history
                        .Where(h => h.StatusId == workOrderDetail.CurrentStatusID)
                        .OrderByDescending(h => h.Date)
                        .FirstOrDefault();

                    if (latestHistoryForCurrentStatus != null)
                    {
                        version = latestHistoryForCurrentStatus.Version;
                    }
                }

                // Construct the certificate ID: {WorkOrderId}-{Sequence}-Ver{Version}
                return $"{workOrderId}-{sequence}-Ver{version}";
            }
            catch (Exception ex)
            {
                // Log the exception if needed and return a fallback format
                return $"{workOrderDetail.WorkOrderID}-1-Ver1";
            }
        }

        public async ValueTask<Header> GetWorkOrderDetailXIdRepWithSave(WorkOrderDetail wo, string customer, bool Save = false)
        
        {


            var status = await GetStatus();

            var history = await GetHistory(wo);


            var laststatus = status.Where(x => x.IsLast == true).FirstOrDefault();

            var aproveduserid = history.Where(x => x.StatusId == laststatus.StatusId).OrderByDescending(x => x.WorkDetailHistoryID).FirstOrDefault();

            var reviewuserid = history.Where(x => x.StatusId == (laststatus.StatusId - 1)).OrderByDescending(x => x.WorkDetailHistoryID).FirstOrDefault();

            //TODO NEW
            string tecnameaproved = string.Empty;
            User useraproved = null;
            if (aproveduserid != null && aproveduserid.TechnicianID.HasValue)
            {
                useraproved = await Basics.GetUserById2(new User() { UserID = aproveduserid.TechnicianID.Value });
                tecnameaproved = useraproved?.Name + " " + useraproved?.LastName;
            }
            string tecnreview = string.Empty;
            if (reviewuserid != null && reviewuserid.TechnicianID.HasValue)
            {
                var userreview = await Basics.GetUserById2(new User() { UserID = reviewuserid.TechnicianID.Value });

                tecnreview = userreview?.Name + " " + userreview?.LastName;

            }

            tecnameaproved = "123456";

            Header a = new Header();

            if (customer == "LTI" || customer == "ThermoTempBalance")
            {
                a = await GetWorkOrderDetailXIdRep2(wo, useraproved, tecnreview, Save);
            }
            else if (customer == "Bitterman" || customer == "Advance")
            {
                a = await GetWorkOrderDetailXIdRep2Bitterman(wo, useraproved, tecnreview, Save, customer);
            }
            else if (customer == "ThermoTemp")
            {
                a = await GetWorkOrderDetailXIdRepThermoTemp(wo, useraproved, tecnreview, Save);
            }
           

            return a;

        }
      
        public async ValueTask<Header> GetWorkOrderDetailXIdRep2(WorkOrderDetail wo,
         User useraproved,
         string techreview = "", bool Save = false)
        {
            
            try
            {


                var ToleranceList = new Dictionary<string, string>();

                ToleranceList.Add(1.ToString(), "Resolution");
                ToleranceList.Add(2.ToString(), "Percentage + Resolution");
                ToleranceList.Add(3.ToString(), "HB44");

                var workOrderDetail1 = await GetByID(wo);
                var workOrderDetail = workOrderDetail1;

                var toleranceTUR = workOrderDetail1.Tolerance;

                var workOrder = workOrderDetail1.WorkOder;
                var customer = workOrder.Customer;

             
                var poe = workOrderDetail1.PieceOfEquipment; 

                List<WeightSetHeader> listw = new List<WeightSetHeader>();

                var poes = await PieceOfEquipmentRepository.GetAllWeightSets(poe);

                if (workOrderDetail.WOD_Weights != null)
                {
                    var testp = workOrderDetail.WOD_Weights.DistinctBy(x => x.WeightSet.PieceOfEquipmentID);

                    foreach (var item in testp)
                    {
                        var poetmp = await PieceOfEquipmentRepository.GetPieceOfEquipmentByIDHeader(item.WeightSet.PieceOfEquipmentID);

                        var _listCetificate = await Assets.GetCertificateXPoE(poetmp);


                        CertificatePoE cert = _listCetificate.OrderByDescending(x => x.Version).FirstOrDefault();

                        string certnumber = "";
                        string calibrationProvider = "";
                        if (cert != null)
                        {
                            ///
                            calibrationProvider = cert.CalibrationProvider;
                            ///
                            certnumber = cert.CertificateNumber;
                        }


                        WeightSetHeader ww = new WeightSetHeader()
                        {

                            PoE = poetmp.PieceOfEquipmentID,
                            Serial = poetmp.SerialNumber,
                            Ref = certnumber + " - " + calibrationProvider,
                            CalibrationDueDate = poetmp.DueDate.ToString("MM/dd/yyyy"),
                            Note = poetmp.Notes

                        };
                        listw.Add(ww);
                    }
                }

                string _AsLeftResult = "";
                string _AsFoundResult = "";


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


                IEnumerable<BasicCalibrationResult> _repeatability = null;
                IEnumerable<BasicCalibrationResult> _eccentricity = null;

                if (workOrderDetail.BalanceAndScaleCalibration != null && workOrderDetail.BalanceAndScaleCalibration.Repeatability != null)
                {
                    _repeatability = workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.Where(x => x.CalibrationSubTypeId == 2);
                }

                if (workOrderDetail.BalanceAndScaleCalibration != null && workOrderDetail.BalanceAndScaleCalibration.Eccentricity != null)
                {
                    _eccentricity = workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.Where(x => x.CalibrationSubTypeId == 3);
                }

                List<Linearity> _linearity = new List<Linearity>();

                if (workOrderDetail.BalanceAndScaleCalibration != null && workOrderDetail.BalanceAndScaleCalibration.Linearities != null)
                {
                    _linearity = OrderAlgoritm(workOrderDetail.BalanceAndScaleCalibration.Linearities.ToList());
                    var asLeftL = _linearity.Where(x => x.BasicCalibrationResult.InToleranceLeft.ToUpper() == "PASS".ToUpper()).OrderBy(x=>x.TestPoint.Position);
                    var asFoundL = _linearity.Where(x => x.BasicCalibrationResult.InToleranceFound.ToUpper() == "PASS".ToUpper()).OrderBy(x => x.TestPoint.Position);


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

                }
                var address = customer.Aggregates.FirstOrDefault().Addresses.FirstOrDefault();


                string techAproved = "";
                string tecertiied = "";
                if (useraproved != null)
                {
                    techAproved = useraproved?.Name + " " + useraproved?.LastName;

                    if (useraproved.TechnicianCodes != null && useraproved.TechnicianCodes.Count > 0)
                    {

                        var tecertiiedo = useraproved.TechnicianCodes.Where(z => z.StateID == address.StateID).FirstOrDefault();

                        if (tecertiiedo != null)
                        {
                            tecertiied = tecertiiedo.Certification.Name;

                        }
                    }

                }



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

                UnitOfMeasure umm = new UnitOfMeasure();
                umm.UnitOfMeasureID = poe.UnitOfMeasureID.Value;
                var um1 = (await UoMRepository.GetByID(umm)).Abbreviation;

                UnitOfMeasure ummTemp = new UnitOfMeasure();
                ummTemp.UnitOfMeasureID = workOrderDetail.TemperatureUOMID;
                var temperature = await UoMRepository.GetByID(ummTemp);
                string umTemperatute = "";
                if (temperature != null)
                    umTemperatute = temperature.Abbreviation;


                // Notes 
                //Notes Equipment Type
                List<Note> notesEqType = new List<Note>();

                EquipmentType eqType = null;
                  if (poe.EquipmentTemplate.EquipmentTypeID != null)
                {
                    eqType = await Basics.GetEquipmentTypeXId(poe.EquipmentTemplate.EquipmentTypeID);
                }
                if (eqType != null)
                {
                    notesEqType = eqType.Notes.OrderBy(x => x.EquipmnetTypeId != null).ThenBy(y => y.TestCodeID != null).ThenBy(z => z.Position).ToList();

                }
                //Notes TestCode
                List<Note> notesTestCode = new List<Note>();
                if (workOrderDetail.TestCode != null && workOrderDetail.TestCode.TestCodeID != null)
                {

                    var testCode = await Repository.GetTestCodeByID((int)workOrderDetail.TestCodeID);
                    if (testCode != null && testCode.Notes != null)
                    {
                        notesTestCode = testCode.Notes.OrderBy(y => y.TestCodeID != null).ToList();

                    }
                }
                List<Note> NotesEqTc = new List<Note>();
                NotesEqTc = notesEqType.Concat(notesTestCode).ToList();

                //Validate Notes Condition
                List<Note> notesCondition = new List<Note>();
                foreach (var note in NotesEqTc)
                {
                    if (note.Condition == 1)
                        notesCondition.Add(note);
                    else if (workOrderDetail.IsAccredited == true && note.Condition == 4)
                        notesCondition.Add(note);
                    else if (workOrderDetail.IsAccredited == false && note.Condition == 5)
                        notesCondition.Add(note);

                }

                //Notes WOD
                List<NoteWOD> notesWOD = new List<NoteWOD>();
                var notesWOD1 = await Repository.GetNotes(wo.WorkOrderDetailID, 1);
                notesWOD = notesWOD1.OrderBy(x => x.Position).ToList();
                NoteScale noteViewModel = new NoteScale();
                noteViewModel.NotesEqScaleList = new List<NoteEqScale>();
                noteViewModel.NoteWODScaleList = new List<NoteWODScale>();

                try
                {
                   
                    if (notesCondition != null && notesCondition.Count() > 0)
                    {
                        foreach (var itemet in notesCondition)
                        {
                            var noteEq = new NoteEqScale();
                            noteEq.Text = itemet.Text;
                            noteViewModel.NotesEqScaleList.Add(noteEq);


                        }
                    }

                    if (notesWOD != null && notesWOD.Count() > 0)
                    {
                        foreach (var itemwod in notesWOD)
                        {
                            var notewod = new NoteWODScale();
                            notewod.Text = itemwod.Text;
                            noteViewModel.NoteWODScaleList.Add(notewod);


                        }

                    }
                }
                catch (Exception ex)
                {

                }

                //var eqCondition =  workOrderDetail.EquipmentCondition.ToList();
                string received = await GetEquipmentCondition(workOrderDetail, 1);
                string returned = await GetEquipmentCondition(workOrderDetail, 2);
                //foreach (var item in eqCondition)
                //{
                //    if (item.IsAsFound)
                //    {
                //        if (item.Value)
                //        {
                //            received = "In Service";
                //        }
                //        else
                //        {
                //            received = "Out of Service";
                //        }
                //    }
                //    else
                //    {
                //        if (item.Value)
                //        {
                //            returned = "In Service";
                //        }
                //        else
                //        {
                //            returned = "Out of Service";
                //        }
                //    }
                //}

                DateTime dueDate;
                string due = "";
                DateTime calDate;
                string calibrationDate = "";
                if (workOrderDetail.CalibrationCustomDueDate != null)
                {
                    dueDate = (DateTime)workOrderDetail.CalibrationCustomDueDate;
                    due = dueDate.ToString("MM/dd/yyyy");
                }
                if (workOrderDetail.CalibrationDate != null)
                {
                    calDate = (DateTime)workOrderDetail.CalibrationDate;
                    calibrationDate = calDate.ToString("MM/dd/yyyy");
                }

                double totLin = 0;
                double totEcc = 0;
                double totRep = 0;


                if (_linearity?.Count > 0)
                {
                    totLin = _linearity.Sum(x => x.BasicCalibrationResult.AsLeft);
                }



                if (_eccentricity?.Count() > 0)
                {
                    totEcc = _eccentricity.Sum(x => x.AsLeft);
                }




                //Standard Deviation Repeatability
                double stdDevAsFoundRep = 0;
                double stdDevAsLeftRep = 0;
                string accuracy1 = "";
                if (_repeatability?.Count() > 0)
                {
                    totRep = _repeatability.Sum(x => x.AsLeft);
                    stdDevAsFoundRep = Math.Round(CalculateStandardDeviation(_repeatability.Select(r => r.AsFound).ToList()), 5);
                    stdDevAsLeftRep = Math.Round(CalculateStandardDeviation(_repeatability.Select(r => r.AsLeft).ToList()), 5);
                }

                if (!string.IsNullOrEmpty(workOrderDetail?.Tolerance?.AccuracyPercentage.ToString()))
                {
                    accuracy1 = workOrderDetail.Tolerance?.AccuracyPercentage.ToString();
                }

                //FullScale
                double ToleranceTypeId = workOrderDetail?.Tolerance?.ToleranceTypeID ?? 0.0;
                var appState = new AppState();

                double ToleranceValue = workOrderDetail?.Tolerance?.Resolution ?? 0.0;   
                var fullscale = workOrderDetail?.Tolerance?.FullScale ?? false;
                    string accuracy = "";


                // Acces to ToleranceListDynamic y and filter by 


                var filteredToleranceList = appState.ToleranceListDynamic?
     .FirstOrDefault(t => t.Key == ToleranceTypeId.ToString())
     ?? appState.ToleranceListNoDynamic?
     .FirstOrDefault(t => t.Key == ToleranceTypeId.ToString());
                string toleranceTypeValue = "";

                if (filteredToleranceList != null)
                {
                    toleranceTypeValue = filteredToleranceList.Value;
                }
                else
                {
                    switch (ToleranceTypeId)
                    {
                        case 2:
                            toleranceTypeValue = "Percentage + Resolution";
                            break;
                        case 3:
                            toleranceTypeValue = "HB44";
                            break;
                        case 4:
                            toleranceTypeValue = "Percentage";
                            break;
                    
                    }
                }

                    if (fullscale)
                    {
                    accuracy = toleranceTypeValue + ": " + ToleranceValue + " FullScale";

                    }
                    else
                    {
                    accuracy = toleranceTypeValue + ": " + ToleranceValue;
                    }


                ///
                /// Address
                /// 
                string customerOrder = "";
                string customerPO = "";
                string customerAddress1 = "";
                string customerAddress2 = "";
                string customerAddressCity = "";
                string customerAddressCountry = "";
                string customerAddressState = "";
                string shipTo = "";
                string shipVia = "";
               
                
                if (!string.IsNullOrEmpty(poe.PieceOfEquipmentID))
                {
                    customerOrder = poe.PieceOfEquipmentID;
                    customerPO = poe.PieceOfEquipmentID;
                }
                if (!string.IsNullOrEmpty(address.StreetAddress1))
                    customerAddress1 = address.StreetAddress1;

                if (!string.IsNullOrEmpty(address.StreetAddress2))
                    customerAddress2 = address.StreetAddress2; ;

                if (!string.IsNullOrEmpty(address.StreetAddress3))
                    customerAddressCity = address.StreetAddress3;

                if (!string.IsNullOrEmpty(address.Country))
                    customerAddressCountry = address.Country;

                if (!string.IsNullOrEmpty(address.State))
                    customerAddressState = address.State;

                received =  received.Equals("In Service", StringComparison.OrdinalIgnoreCase) ? "In Tolerance"
                  : received.Equals("Out of Service", StringComparison.OrdinalIgnoreCase) ? "Out of Tolerance"
                  : received;

                returned =  returned.Equals("In Service", StringComparison.OrdinalIgnoreCase) ? "In Tolerance"
                  : returned.Equals("Out of Service", StringComparison.OrdinalIgnoreCase) ? "Out of Tolerance"
                  : returned;

                Header header = new Header()
                {
                    Accuracy = accuracy,
                    Client = customer.Name,
                    Address = address.StreetAddress1, // + " " + ' ' + address.City + ' ' + address.County + " " + address.Country + " " + address.ZipCode,
                    Country = customer.Aggregates.FirstOrDefault().Addresses.FirstOrDefault().County,
                    City = address.City,
                    State = address.State,
                    ZipCode = address.ZipCode,
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
                    CapIndReceiv = poe.Capacity.ToString() + " " + um1, //OJOO no está mapeado
                    Class = poe.Class,
                    Type = poe.EquipmentTemplate.DeviceClass, //OJOO no está mapeado
                    PlatformSize = poe.EquipmentTemplate.PlatformSize,// null, //OJOO no está mapeado
                    CalibrtionDate = calibrationDate,
                    Temperature = "Before Cal: " + workOrderDetail.Temperature + " " + umTemperatute + "/ After Cal: " + workOrderDetail.TemperatureAfter + " " + umTemperatute,
                    Enviroment = workOrderDetail.Environment,
                    Technician = techreview,//workOrderDetail1.Technician.Name + " " + workOrderDetail1.Technician.LastName
                    TechnicianAprove = tecertiied,
                    UnitOfMeasure = um1,
                    Tolerance = ToleranceList.GetValueOrDefault(workOrderDetail.Tolerance.ToleranceTypeID.ToString()),
                    Resolution = workOrderDetail.Resolution.ToString(),
                    PieceOfEquipmentID = poe.PieceOfEquipmentID.ToString(),
                    NoteScale = noteViewModel,
                    Unit = poe.PieceOfEquipmentID,
                    CustomerTool = poe.CustomerToolId,
                    InstallLocation = poe.InstallLocation,
                    ReceivedCondition = received,
                    ReturnedCondition = returned,
                    UoM = workOrderDetail.PieceOfEquipment.UnitOfMeasure.Abbreviation,
                    Due = due,
                    Humidity = workOrderDetail.Humidity.ToString() + " %RH",
                    StDevRepeatAsFound = stdDevAsFoundRep.ToString("F5"),
                    StDevRepeatAsLeft = stdDevAsLeftRep.ToString("F5"),
                    CertficateComment = workOrderDetail?.CertificateComment,
                    CustomerAddress1 = customerAddress1,
                    CustomerAddress2 = customerAddress2,
                    CustomerAddressState = customerAddressCity,
                    CustomerAddressCity = customerAddressCity,
                    CustomerAddressCountry = customerAddressCountry,
                    CertificateNumber = workOrderDetail.WorkOrderDetailID.ToString() + "_" + workOrderDetail.WorkOrderDetailHash,
                    CustomerRef = poe.CustomerToolId,
                    CalibrationDueDate = workOrderDetail.CalibrationCustomDueDate.HasValue ? workOrderDetail.CalibrationCustomDueDate.Value.ToString("MM/dd/yyyy") : string.Empty,
                    Manufacturer = poe?.EquipmentTemplate?.Manufacturer1?.Name,
                    Model = poe?.EquipmentTemplate?.Model,
                    RangeRes = poe.Capacity.ToString() + " " + um1 + "/" + poe.Resolution.ToString(),
                    Procedure = workOrderDetail?.TestCode?.Procedure?.Name,
                    CertificateComments = workOrderDetail.CertificateComment,


                };

                if (workOrderDetail.IsAccredited.HasValue)
                {
                    header.IsAccredited = workOrderDetail.IsAccredited.Value;
                }

                #region Data CalCertBlank
                List<AsFound> AsFoundList = new List<AsFound>();
                double res = 0;
                foreach (var af in _linearity)
                {

                    var um = (await UoMRepository.GetByID(af.TestPoint.UnitOfMeasurement)).Abbreviation;

                  
                    //if (af.BasicCalibrationResult.Resolution == 0)
                    //{
                        res = workOrderDetail.Resolution;

                    //}
                    

                    IEnumerable<WeightSet> weightSets = new List<WeightSet>();
                    weightSets = await PieceOfEquipmentRepository.GetWeigthSets();

                    var totalUncertainty = await Poe.GetReportUncertaintyBudget(af, af.SequenceID, workOrderDetail1, workOrderDetail, poes.ToList(), weightSets.ToList());
                   
                    ////
                    ///Uncertainty CMC
                    ///
                    if (af.BasicCalibrationResult.UncertaintyNew != null && af.BasicCalibrationResult.UncertaintyNew != 0)
                    {
                        totalUncertainty.Totales.ExpandedUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(af.BasicCalibrationResult.UncertaintyNew)));
                        af.TotalUncertainty = (double)af.BasicCalibrationResult.UncertaintyNew;

                    }

                    ///TUR AsFound
                    ///
                    double tur = 0;
                    double xpandedUncert = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(totalUncertainty.Totales.ExpandedUncertainty))); 
                    {
                        var lowTolerance = ValidTolerance(af.TestPoint.NominalTestPoit, (int)toleranceTUR.ToleranceTypeID, toleranceTUR.Resolution, toleranceTUR.AccuracyPercentage, "low");
                        var maxTolerance = ValidTolerance(af.TestPoint.NominalTestPoit, (int)toleranceTUR.ToleranceTypeID, toleranceTUR.Resolution, toleranceTUR.AccuracyPercentage, "max");
                        var toleranceDifference = maxTolerance - lowTolerance;
                        var uncertx2 = (xpandedUncert * 2);
                        if (uncertx2 == 0)
                        {
                            uncertx2 = 1;
                        }

                        tur = (toleranceDifference / uncertx2);  
                    }

                    var value = QueryableExtensions2.Completezero(af.BasicCalibrationResult.AsFound.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))).ToString();

                    var AsFoundLin = new Reports.Domain.ReportViewModels.AsFound
                    {
                        Standard = QueryableExtensions2.Completezero(af?.TestPoint?.NominalTestPoit.ToString()==null?"0":af?.TestPoint?.NominalTestPoit.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))).ToString(),
                         Tolerance = QueryableExtensions2.Completezero(af?.TestPoint?.LowerTolerance.ToString() == null ? "0" : af?.TestPoint?.LowerTolerance.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))).ToString() + "-" + QueryableExtensions2.Completezero(af?.TestPoint?.UpperTolerance.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))).ToString(), //af.TestPoint.LowerTolerance,
                        PassFail = af.BasicCalibrationResult.InToleranceFound,
                        //Description = af.TestPoint.Description, 
                        Indication = af.TestPoint.Description,
                        Uncertainty = QueryableExtensions2.Completezero(((double)RoundFirstSignificantDigit(Convert.ToDecimal(totalUncertainty.Totales.ExpandedUncertainty))).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))).ToString(), //af.BasicCalibrationResult.Uncertainty,
                        Range = 0, //OJOO no está mapeado
                        Value = value,
                        TUR = RoundFirstSignificantDigit(Convert.ToDecimal(tur)).ToString(),
                        WeigthActual = QueryableExtensions2.Completezero(af.WeightSets?.FirstOrDefault()?.WeightActualValue.ToString()==null?"0": af.WeightSets?.FirstOrDefault()?.WeightActualValue.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))).ToString()  


                    };
                    AsFoundList.Add(AsFoundLin);
                }

                List<AsLeft> AsLeftList = new List<AsLeft>();

                if (_linearity != null && _linearity.Count > 0)
                {
                    foreach (var af in _linearity.OrderBy(x => x.TestPoint.Position).ToList())
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


                        var totalUncertainty = await Poe.GetReportUncertaintyBudget(af, af.SequenceID, workOrderDetail1, workOrderDetail, poes.ToList(), null);

                        ///TUR  As Left
                        ///
                        double tur = 0; 
                        if (tolerance != null)
                        {
                            double xpandedUncert = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(totalUncertainty.Totales.ExpandedUncertainty)));
                            var lowTolerance = ValidTolerance(af.TestPoint.NominalTestPoit, (int)toleranceTUR.ToleranceTypeID, toleranceTUR.Resolution, toleranceTUR.AccuracyPercentage, "low");
                            var maxTolerance = ValidTolerance(af.TestPoint.NominalTestPoit, (int)toleranceTUR.ToleranceTypeID, toleranceTUR.Resolution, toleranceTUR.AccuracyPercentage, "max");
                            var toleranceDifference = maxTolerance - lowTolerance;
                            var uncertx2 = (xpandedUncert * 2);
                            if (uncertx2 == 0)
                            {
                                uncertx2 = 1;
                            }

                            tur = (toleranceDifference / uncertx2);
                            
                        }
                        var um = (await UoMRepository.GetByID(af.TestPoint.UnitOfMeasurement)).Abbreviation;
                        var value = QueryableExtensions2.Completezero(af.BasicCalibrationResult.AsLeft.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res)));//af.BasicCalibrationResult.Uncertainty,


                        var AsLeftLin = new Reports.Domain.ReportViewModels.AsLeft
                        {
                            Standard = QueryableExtensions2.Completezero(af?.TestPoint?.NominalTestPoit.ToString() == null ? "0" : af?.TestPoint?.NominalTestPoit.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))).ToString(),
                            Tolerance = QueryableExtensions2.Completezero(af?.TestPoint?.LowerTolerance.ToString() == null ? "0" : af?.TestPoint?.LowerTolerance.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))).ToString() + "-" + QueryableExtensions2.Completezero(af?.TestPoint?.UpperTolerance.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(af.TestPoint.Resolution))).ToString(), //af.TestPoint.LowerTolerance,
                            PassFail = af.BasicCalibrationResult.InToleranceLeft,
                            //Description = af.TestPoint.Description, 
                            Indication = af.TestPoint.Description,
                            Value = value,
                            Uncertainty = QueryableExtensions2.Completezero(((double)RoundFirstSignificantDigit(Convert.ToDecimal(totalUncertainty.Totales.ExpandedUncertainty))).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(af.TestPoint.Resolution))).ToString(), //af.BasicCalibrationResult.Uncertainty,
                            TUR = RoundFirstSignificantDigit(Convert.ToDecimal(tur)).ToString(),
                            WeigthActual = QueryableExtensions2.Completezero(af.WeightSets?.FirstOrDefault()?.WeightActualValue.ToString() == null ? "0" : af.WeightSets?.FirstOrDefault()?.WeightActualValue.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(af.TestPoint.Resolution))).ToString()

                        };
                        AsLeftList.Add(AsLeftLin);
                    }
                }
                List<Reports.Domain.ReportViewModels.Lineartiy> linList = new List<Reports.Domain.ReportViewModels.Lineartiy>();

                if (_linearity != null)
                { 
                   foreach (var lin in _linearity.OrderBy(x=>x.SequenceID))
                    {
                        string um = "N/A";
                        if (lin?.UnitOfMeasure != null)
                        {
                             um = (await UoMRepository.GetByID(lin.UnitOfMeasure)).Abbreviation;
                        }
                                               

                        var linearity = new Reports.Domain.ReportViewModels.Lineartiy
                        {
                            AsFound = QueryableExtensions2.Completezero(lin.BasicCalibrationResult.AsFound.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))),
                            AsLeft = QueryableExtensions2.Completezero(lin.BasicCalibrationResult.AsLeft.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))),
                            Position = lin.BasicCalibrationResult.Position,
                            Weight = QueryableExtensions2.Completezero(lin.BasicCalibrationResult.WeightApplied.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))),
                            PassFail = lin.BasicCalibrationResult.InToleranceLeft,
                        };
                        linList.Add(linearity);
                    }
                }


                List<Reports.Domain.ReportViewModels.Excentricity> eccList = new List<Reports.Domain.ReportViewModels.Excentricity>();
                List<double> valueseccAsFound = new List<double>();
                List<double> valueseccAsLeft = new List<double>();
              if (_eccentricity != null)
                {
                    foreach (var ex in _eccentricity.OrderBy(x => x.SequenceID))
                    {
                        string um = "N/A";
                        if (ex?.UnitOfMeasure != null)
                        {
                            um = (await UoMRepository.GetByID(ex.UnitOfMeasure)).Abbreviation;
                        }
                        
                        //if (ex.Resolution == 0)
                        //{
                            res = workOrderDetail.Resolution;

                        //}
                   
                        var valueAF = Convert.ToDouble(QueryableExtensions2.Completezero(ex.AsFound.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))));
                            valueseccAsFound.Add(valueAF);

                         var valueAL = Convert.ToDouble(QueryableExtensions2.Completezero(ex.AsLeft.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))));
                         valueseccAsLeft.Add(valueAL);



                        var ecc = new Reports.Domain.ReportViewModels.Excentricity
                        {
                            AsFound = Convert.ToDouble(QueryableExtensions2.Completezero(ex.AsFound.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res)))) + " " + um,
                            AsLeft =  Convert.ToDouble(QueryableExtensions2.Completezero(ex.AsLeft.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res)))) + " " + um,//ex.AsLeft.ToString() + " " + um ,
                            Position = ex.Position,
                            Weight = QueryableExtensions2.Completezero(ex.WeightApplied.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))) + " " + um,// ex.WeightApplied.ToString() + " " + um
                            PassFail = ex.InToleranceLeft,
                        };
                        eccList.Add(ecc);
                    };
                }

                List<Reports.Domain.ReportViewModels.Repeteab> repetList = new List<Reports.Domain.ReportViewModels.Repeteab>();
                bool _viewGrid = false;

                if (_repeatability != null && _repeatability.Count() > 0)
                {
                    _viewGrid = true;
                    string um = "N/A";
                    if (_repeatability?.FirstOrDefault().UnitOfMeasure != null)
                    {
                         um = (await UoMRepository.GetByID(_repeatability.FirstOrDefault().UnitOfMeasure)).Abbreviation;
                    }

                   
                    foreach (var re in _repeatability)
                    {
                      
                        //if (re.Resolution == 0)
                        //{
                            res = workOrderDetail.Resolution;

                        //}
                        
                        var rep = new Reports.Domain.ReportViewModels.Repeteab
                        {
                            AsFound = QueryableExtensions2.Completezero(re.AsFound.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))) + " " + um,// re.AsFound + " " + um,
                            AsLeft = QueryableExtensions2.Completezero(re.AsLeft.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))) + " " + um,//re.AsLeft + " " + um,
                            Position = re.Position,
                            Standard = QueryableExtensions2.Completezero(re.WeightApplied.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))) + " " + um,// re.WeightApplied + " " + um, //OJOO no está mapeado//
                            ViewGrid = _viewGrid,
                            PassFail = re.InToleranceLeft

                        };
                        repetList.Add(rep);
                    };

                }

                List<Reports.Domain.ReportViewModels.ExcentricityDet> excentriDetList = new List<Reports.Domain.ReportViewModels.ExcentricityDet>();


                if (weigthSets != null)
                {
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
                }
                #endregion

               

                header.RepeteabilityList = repetList;
                header.eccList = eccList;
                header.excentriDetList = excentriDetList;
                header.AsFoundList = AsFoundList;
                header.AsLeftList = AsLeftList;
                header.WeightSetList = listw;

                var asFoundValues = valueseccAsFound;
                var asLeftValues = valueseccAsLeft;

                var maxAsFoundEcc = asFoundValues?.Count > 0 ? asFoundValues.Max() : 0;
                var minAsFoundEcc = asFoundValues?.Count > 0 ? asFoundValues.Min() : 0;

                var maxAsLeftEcc = asLeftValues?.Count > 0 ? asLeftValues.Max() : 0;
                var minAsLeftEcc = asLeftValues?.Count > 0 ? asLeftValues.Min() : 0;


                header.MaxMinAsFoundtEcc = QueryableExtensions2.Completezero((minAsFoundEcc - maxAsFoundEcc).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))).ToString();
                header.MaxMinAsLeftEcc = QueryableExtensions2.Completezero((minAsLeftEcc - maxAsLeftEcc).ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))).ToString();
                if (Save)
                {
                    var serialHash = header.SerialIndReceiv.GetSHA1Has();


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
                    var serial = JsonConvert.SerializeObject(workOrderDetail1);

                    cert.WorkOrderDetailSerialized = serial;
                    var resultwhd = CreateCertificate(cert);
                }



                return header;


            }
            catch (TimeoutException ex)
            {
                throw new TimeoutException("TimeOut");
            }

        }

        public async Task<string> GetEquipmentCondition (WorkOrderDetail wo, int option)
        {
            var eqCondition = wo.EquipmentCondition.ToList();
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
            if (option == 1)
            {
                return received;
            }
            else
            { 
                return returned; 
            }
        }
    public async ValueTask<Header> GetWorkOrderDetailXIdRepThermoTemp(WorkOrderDetail wo, User useraproved, string techreview = "", bool Save = false)
        {
            //var workOrderDetail = await LogicSample.GetWorkOrderDetailXIdRep(wo.WorkOrderDetailID);
            //wo.WorkOrderDetailID = 2;

            var ToleranceList = new Dictionary<string, string>();

            ToleranceList.Add(1.ToString(), "Resolution");
            ToleranceList.Add(2.ToString(), "Percentage + Resolution");
            ToleranceList.Add(3.ToString(), "HB44");

            var CalibrationInterval = new Dictionary<int, string>();

            CalibrationInterval.Add(1, "Custom");
            CalibrationInterval.Add(2, "1 months");
            CalibrationInterval.Add(3, "2 months");
            CalibrationInterval.Add(4, "3 months");
            CalibrationInterval.Add(5, "6 months");
            CalibrationInterval.Add(6, "12 months");

            var workOrderDetail1 = await GetByID(wo);
            var workOrderDetail = workOrderDetail1;//await GetWorkOrderDetailByID(wo);

            var workOrder = workOrderDetail1.WorkOder;
            var customer = workOrder.Customer;

            string received = await GetEquipmentCondition(workOrderDetail, 1);
            string returned = await GetEquipmentCondition(workOrderDetail, 2);
            //var POE = await Logicpoe.GetPieceOfEquipmentByID(workOrderDetail1.PieceOfEquipment);

            //var address = customer.Aggregates[0];
            var poe = workOrderDetail1.PieceOfEquipment;  // customer.PieceOfEquipment.Where(x => x.WorOrderDetailID == wo.WorkOrderDetailID).FirstOrDefault();

            List<WeightSetHeader> listw = new List<WeightSetHeader>();

            List<PieceOfEquipment> poes = new List<PieceOfEquipment>();
            //var poes = await PieceOfEquipmentRepository.GetAllWeightSets(poe);

            if (workOrderDetail.WOD_Weights != null)
            {
                var testp = workOrderDetail.WOD_Weights.DistinctBy(x => x.WeightSet.PieceOfEquipmentID);

                foreach (var item in testp)
                {
                    PieceOfEquipment poetmp = new PieceOfEquipment();
                    if (item.WeightSet.PieceOfEquipmentID != null)
                    {
                        poetmp = await PieceOfEquipmentRepository.GetPieceOfEquipmentByIDHeader(item.WeightSet.PieceOfEquipmentID);
                    }
                    else if (item.WeightSet.Serial != null)
                    {
                        poetmp = await PieceOfEquipmentRepository.GetPieceOfEquipmentBySerial1(item.WeightSet.Serial);

                    }
                    var _listCetificate = await Assets.GetCertificateXPoE(poetmp);


                    CertificatePoE cert = _listCetificate.OrderByDescending(x => x.Version).FirstOrDefault();

                    string certnumber = "";
                    if (cert != null)
                    {
                        certnumber = cert.CertificateNumber;
                    }

                    //.CalibrationDate.Value.ToString("MM/dd/yyyy"),
                    WeightSetHeader ww = new WeightSetHeader()
                    {
                        
                        PoE = poetmp.PieceOfEquipmentID,
                        Serial = poetmp.SerialNumber,
                        Ref = certnumber,
                        CalibrationDueDate = poetmp.DueDate.ToString("MM/dd/yyyy"),
                        Note = poetmp.Notes,
                        CalibrationDate = poetmp.CalibrationDate.ToString("MM/dd/yyyy"),
                        Description = poetmp.Description,
                        Manufacturer = poetmp?.EquipmentTemplate?.Manufacturer1?.Name,
                        Model = poetmp?.EquipmentTemplate?.Model
                        

                    };
                    listw.Add(ww);
                }
            }

            string _AsLeftResult = "";
            string _AsFoundResult = "";


            var eqTemp = poe.EquipmentTemplate;

            ICollection<WeightSet> weigthSets = null;

            if (poes?.Count() > 0)
            {
                weigthSets = poes.FirstOrDefault().WeightSets;
            }

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


            IEnumerable<BasicCalibrationResult> _repeatability = null;
            IEnumerable<BasicCalibrationResult> _eccentricity = null;


            var address1 = customer.Aggregates.FirstOrDefault().Addresses.ToList();

            Address address = new Address();
            string techAproved = "";
            string tecertiied = "";
            address = address1.FirstOrDefault();
            if (useraproved != null)
            {
                techAproved = useraproved?.Name + " " + useraproved?.LastName;

                if (useraproved.TechnicianCodes != null && useraproved.TechnicianCodes.Count > 0)
                {
                    var tecertiiedo = useraproved.TechnicianCodes
                    .Where(tc => address1.Any(addr => addr.StateID == tc.StateID))
                    .FirstOrDefault();


                    //  var tecertiiedo = useraproved.TechnicianCodes.Where(z => z.StateID == address.StateID).FirstOrDefault();
                    address = address1.Where(x => x.StateID == tecertiiedo.StateID).FirstOrDefault();
                    if (tecertiiedo != null && tecertiiedo.Certification != null)
                    {
                        tecertiied = tecertiiedo.Certification.Name;

                    }
                }

            }

            string lastCaldate = "";
            if (workOrderDetail1.StatusDate != null)
                lastCaldate = workOrderDetail1.StatusDate.Value.ToString("MM/dd/yyyy");

            string ManInd;
            string ModelInd;
            string SerialInd;
            string Manufact;
            string Model;
            
            Manufact = poe.EquipmentTemplate.Manufacturer1.Name;
            Model = poe.EquipmentTemplate.Model;
            UnitOfMeasure umm = new UnitOfMeasure();
            umm.UnitOfMeasureID = poe.UnitOfMeasureID.Value;
            var um1 = (await UoMRepository.GetByID(umm)).Abbreviation;

            UnitOfMeasure ummTemp = new UnitOfMeasure();
            ummTemp.UnitOfMeasureID = workOrderDetail.TemperatureUOMID;
            var temperature = await UoMRepository.GetByID(ummTemp);
            string umTemperatute = "";
            if (temperature != null)
                umTemperatute = temperature.Abbreviation;

            double totLin = 0;
            double totEcc = 0;
            double totRep = 0;


            var county = "";
            if (address.County != null)
                county = address.County;

            var addres = address.StreetAddress1 + " " + ' ' + address.City + ' ' + county + " " + address.Country + " " + address.ZipCode;

            string Class = "";
            if (poe != null && poe.Class != null)
                Class = poe.Class;

            string platSize = "";
            if (poe.EquipmentTemplate != null && poe.EquipmentTemplate.PlatformSize != null)
                platSize = poe.EquipmentTemplate.PlatformSize;

            CalibrationType calType = new CalibrationType();
            calType.CalibrationTypeId = (int) workOrderDetail?.PieceOfEquipment?.EquipmentTemplate?.EquipmentTypeObject?.CalibrationTypeID;
            calType = await Basics.GetCalibrationTypeByID(calType);

            Header header = new Header()
            {
                Client = customer.Name,
                Address = address.StreetAddress1 + " " + ' ' + address.City + ' ' + county + " " + address.Country + " " + address.ZipCode,
                Country = customer.Aggregates.FirstOrDefault().Addresses.FirstOrDefault().Country,
                EquipmentLocation = poe.InstallLocation,
                EquipmentType = eqTemp.EquipmentTypeObject.Name,
                NextCalDate = workOrderDetail.CalibrationCustomDueDate.Value.ToString("MM/dd/yyyy"),
                LastCalDate = lastCaldate, //OJOO no está mapeado
                ManufacturerInd = Manufact,
                ModelInd = Model,
                CapInd = eqTemp.Capacity.ToString(),
                ManufacturerReceiv = manufName,
                ModelIndReceiv = eqTemp.Model,
                CapIndReceiv = poe.Capacity.ToString() + " " + um1, //OJOO no está mapeado
                Class = Class,
                Type = poe.EquipmentTemplate.DeviceClass, //OJOO no está mapeado
                PlatformSize = platSize,// null, //OJOO no está mapeado
                ServiceLocation = "", //OJOO no está mapeado
                UnitNumber = "", //OJOO no está mapeado
                TestingMethod = "", //OJOO no está mapeado
                CalID = poe.CustomerToolId, //OJOO no está mapeado
                Location = poe.InstallLocation, //OJOO no está mapeado
                AsLeftResult = _AsLeftResult, //OJOO no está mapeado
                AsFoundResult = _AsFoundResult, //OJOO no está mapeado
                CalibrtionDate = workOrderDetail.CalibrationDate.Value.ToString("MM/dd/yyyy"),
                Temperature = workOrderDetail.Temperature.ToString() + " " + umTemperatute,
                Enviroment = workOrderDetail.Environment,
                Technician = techreview,//workOrderDetail1.Technician.Name + " " + workOrderDetail1.Technician.LastName
                TechnicianAprove = tecertiied,
                UnitOfMeasure = um1,
                Tolerance = ToleranceList.GetValueOrDefault(workOrderDetail.Tolerance.ToleranceTypeID.ToString()),
                Resolution = workOrderDetail.Resolution.ToString(),
                EquipmentID = eqTemp.EquipmentTemplateID.ToString(),
                Capacity = eqTemp.Capacity.ToString(),
                CertficateComment = workOrderDetail.CertificateComment,
                Serial = poe.SerialNumber,
                Procedure = calType?.Name,
                //CompanyNo = workOrderDetail.PieceOfEquipment.EquipmentTemplate.com
                CalDueDate = workOrderDetail.PieceOfEquipment.DueDate.ToString("MM/dd/yyyy"),
                ReportNumber = workOrderDetail.WorkOrderDetailUserID,
                PONumber = workOrder.PurchaseOrder != null ? workOrder.PurchaseOrder.ToString() : "",
                CalibrationInterval = CalibrationInterval.GetValueOrDefault(workOrderDetail.CalibrationIntervalID),
                ReceivedCondition = received,
                ReturnedCondition = returned,
                Humidity = workOrderDetail.Humidity.ToString() + " %RH"
                
            };

            if (workOrderDetail.IsAccredited.HasValue)
            {
                header.IsAccredited = workOrderDetail.IsAccredited.Value;
            }

           if (listw != null && listw.Count() == 0)
                header.WeightSetList = null;
           else
               header.WeightSetList = listw;

            //Get Statements

            var statements = await GetGenericStatement(workOrderDetail);
            List<GenericStatementThermo> statementList = new List<GenericStatementThermo>();
            if (statements !=null && statements.Count()>0)
            {
                
                foreach (var item in statements)
                {
                    GenericStatementThermo statement = new GenericStatementThermo()
                    {
                        Statement = item.Statement,
                        DataGrid = "Thermo Temp"
                    };
                    statementList.Add(statement);
                    
                }
            }

            header.GenericStatementsThermo = statementList;
            header = await GetReportThermoTemp(workOrderDetail, header);
            List<ParentAndChildren> parentAndChildren = new List<ParentAndChildren>();
            bool isParent = false;



         
                
            List<ChildrenView> childrenView = (await GetChildrenView(workOrderDetail.WorkOrderDetailID) ?? Enumerable.Empty<ChildrenView>()).ToList();
            
            if (childrenView != null && childrenView.Count() > 0)
            {
                if (childrenView != null && childrenView.Count()> 0)
                {

                    foreach (var item in childrenView)
                    {
                        ParentAndChildren parentAndChildrenOne = new ParentAndChildren();
                        if (!string.IsNullOrEmpty(item.ID) && item.ID.Contains("-"))
                        {
                            var parts = item.ID.Split('-');
                            parentAndChildrenOne.ID = parts[0]; // Primera parte
                            parentAndChildrenOne.SerialNumber = parts[1]; // Segunda parte
                        }
                        else
                        {
                            // Manejo de casos donde item.ID no tiene el formato esperado
                            parentAndChildrenOne.ID = item.ID;
                            parentAndChildrenOne.SerialNumber = null; // O algún valor predeterminado
                        }

                       
                        parentAndChildrenOne.TestPointsThermoTemps = await GetReportThermoTempChildren(item.TestPointResult);
                        parentAndChildren.Add(parentAndChildrenOne);
                    }
                    
                }

            }

            header.ParentAndChildren = parentAndChildren;
            return header;


        }

       


        public async ValueTask<Header> GetWorkOrderDetailXIdRep2Bitterman(WorkOrderDetail wo, User useraproved, string techreview = "", bool Save = false, string? cust = "")
        {
            //var workOrderDetail = await LogicSample.GetWorkOrderDetailXIdRep(wo.WorkOrderDetailID);
            //wo.WorkOrderDetailID = 2;

            var ToleranceList = new Dictionary<string, string>();

            ToleranceList.Add(1.ToString(), "Resolution");
            ToleranceList.Add(2.ToString(), "Percentage + Resolution");
            ToleranceList.Add(3.ToString(), "HB44");

            var workOrderDetail1 = await GetByID(wo);
            var workOrderDetail = workOrderDetail1;//await GetWorkOrderDetailByID(wo);

            var toleranceTUR = workOrderDetail1.Tolerance;

            var workOrder = workOrderDetail1.WorkOder;
            var customer = workOrder.Customer;

            //var POE = await Logicpoe.GetPieceOfEquipmentByID(workOrderDetail1.PieceOfEquipment);

            //var address = customer.Aggregates[0];
            var poe = workOrderDetail1.PieceOfEquipment;  // customer.PieceOfEquipment.Where(x => x.WorOrderDetailID == wo.WorkOrderDetailID).FirstOrDefault();
            var resol = (double)workOrderDetail1?.PieceOfEquipment?.EquipmentTemplate?.Resolution;
            List<WeightSetHeader> listw = new List<WeightSetHeader>();

            List<PieceOfEquipment> poes = new List<PieceOfEquipment>();
            //var poes = await PieceOfEquipmentRepository.GetAllWeightSets(poe);

            if (workOrderDetail.WOD_Weights != null)
            {
                var testp = workOrderDetail.WOD_Weights.DistinctBy(x => x.WeightSet.PieceOfEquipmentID);

                foreach (var item in testp)
                {
                    PieceOfEquipment poetmp = new PieceOfEquipment();
                    if (item.WeightSet.PieceOfEquipmentID != null)
                    {
                        poetmp = await PieceOfEquipmentRepository.GetPieceOfEquipmentByIDHeader(item.WeightSet.PieceOfEquipmentID);
                    }
                    else if (item.WeightSet.Serial != null)
                    {
                        poetmp = await PieceOfEquipmentRepository.GetPieceOfEquipmentBySerial1(item.WeightSet.Serial);

                    }
                    var _listCetificate = await Assets.GetCertificateXPoE(poetmp);


                    CertificatePoE cert = _listCetificate.OrderByDescending(x => x.Version).FirstOrDefault();

                    string certnumber = "";
                    if (cert != null)
                    {
                        certnumber = cert.CertificateNumber;
                    }

                    //.CalibrationDate.Value.ToString("MM/dd/yyyy"),
                    WeightSetHeader ww = new WeightSetHeader()
                    {
                        //Distribution = item.WeightSet.Distribution,
                        //Value = item.WeightSet.WeightNominalValue.ToString(),
                        //ActualValue = item.WeightSet.WeightActualValue.ToString(),
                        // PoE=item.WeightSet.PieceOfEquipmentID,
                        // Ref=item.WeightSet.Reference,
                        // Note=item.WeightSet.Note,
                        // Type=item.WeightSet.Type,
                        //Uncertainty=item.WeightSet.CalibrationUncertValue.ToString(),
                        PoE = poetmp.PieceOfEquipmentID,
                        Serial = poetmp.SerialNumber,
                        Ref = certnumber,
                        CalibrationDueDate = poetmp.DueDate.ToString("MM/dd/yyyy"),
                        Note = poetmp.Notes

                    };
                    listw.Add(ww);
                }
            }

            string _AsLeftResult = "";
            string _AsFoundResult = "";

            // Initialize statements list for GenericStatementsWOD
            var statements = new List<GenericStatementWOD>();

            var eqTemp = poe.EquipmentTemplate;

            ICollection<WeightSet> weigthSets = null;

            if (poes?.Count() > 0)
            {
                weigthSets = poes.FirstOrDefault().WeightSets;
            }

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


            IEnumerable<BasicCalibrationResult> _repeatability = null;
            IEnumerable<BasicCalibrationResult> _eccentricity = null;
            List<Linearity> _linearity = new List<Linearity>();
            IOrderedEnumerable<Linearity> asLeftL = null;
            IOrderedEnumerable<Linearity> asFoundL = null;
            IEnumerable<BasicCalibrationResult> asLeftR = null;
            IEnumerable<BasicCalibrationResult> asFoundR = null;
            IEnumerable<BasicCalibrationResult> asLeftE = null;
            IEnumerable<BasicCalibrationResult> asFoundE = null;

            if (workOrderDetail?.BalanceAndScaleCalibration != null && workOrderDetail?.BalanceAndScaleCalibration?.Repeatability != null)
            {
                _repeatability = workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.Where(x => x.CalibrationSubTypeId == 2);
                asLeftR = _repeatability.Where(x => x.InToleranceLeft.ToUpper() == "PASS".ToUpper()).OrderBy(x => x.Position);
                asFoundR = _repeatability.Where(x => x.InToleranceFound.ToUpper() == "PASS".ToUpper()).OrderBy(x => x.Position);

            }

            if (workOrderDetail?.BalanceAndScaleCalibration != null && workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity != null)
            {
                _eccentricity = workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.Where(x => x.CalibrationSubTypeId == 3);
                asLeftL = _linearity.Where(x => x.BasicCalibrationResult.InToleranceLeft.ToUpper() == "PASS".ToUpper()).OrderBy(x => x.TestPoint.Position);
                asFoundL = _linearity.Where(x => x.BasicCalibrationResult.InToleranceFound.ToUpper() == "PASS".ToUpper()).OrderBy(x => x.TestPoint.Position);

            }


            if (workOrderDetail?.BalanceAndScaleCalibration != null && workOrderDetail?.BalanceAndScaleCalibration?.Linearities != null)
            {
                _linearity = workOrderDetail.BalanceAndScaleCalibration.Linearities.ToList().OrderBy(x => x.TestPoint.Position).ToList();  //OrderAlgoritm(workOrderDetail.BalanceAndScaleCalibration.Linearities.ToList());
                asLeftL = _linearity.Where(x => x.BasicCalibrationResult.InToleranceLeft.ToUpper() == "PASS".ToUpper()).OrderBy(x => x.TestPoint.Position);
                asFoundL = _linearity.Where(x => x.BasicCalibrationResult.InToleranceFound.ToUpper() == "PASS".ToUpper()).OrderBy(x => x.TestPoint.Position);

            }

            
            var address1 = customer.Aggregates.FirstOrDefault().Addresses.ToList();

            Address address = new Address();
            string techAproved = "";
            string tecertiied = "";
            address = address1.FirstOrDefault();
            if (useraproved != null)
            {
                techAproved = useraproved?.Name + " " + useraproved?.LastName;

                if (useraproved.TechnicianCodes != null && useraproved.TechnicianCodes.Count > 0)
                {
                    var tecertiiedo = useraproved.TechnicianCodes
                    .Where(tc => address1.Any(addr => addr.StateID == tc.StateID))
                    .FirstOrDefault();
                    
                    if (tecertiiedo != null && tecertiiedo.Certification != null)
                    {
                        //  var tecertiiedo = useraproved.TechnicianCodes.Where(z => z.StateID == address.StateID).FirstOrDefault();
                        address = address1.Where(x => x.StateID == tecertiiedo.StateID).FirstOrDefault();
                    
                        tecertiied = tecertiiedo.Code;
                    }
                }

            }

            string lastCaldate = "";
            if (workOrderDetail1.StatusDate != null)
                lastCaldate = workOrderDetail1.StatusDate.Value.ToString("MM/dd/yyyy");

            string ManInd;
            string ModelInd;
            string SerialInd;


            var children = await PieceOfEquipmentRepository.GetPieceOfEquipmentChildrenAll(poe);

            var childrenIndicator = children.Where(x => x.EquipmentTemplate?.EquipmentTypeObject?.HasIndicator == true);
          


            if (poe.Indicator == null && (children == null || children?.Count() == 0 ) ) 
            {
                ManInd = manufName;
                ModelInd = eqTemp.Model;
                SerialInd = poe.SerialNumber;
            }
            else if(childrenIndicator != null && childrenIndicator?.Count() > 0)
            {
                var poechildren = childrenIndicator.FirstOrDefault();
                ManInd = poechildren.EquipmentTemplate != null && poechildren.EquipmentTemplate.Manufacturer1 != null ? poechildren.EquipmentTemplate.Manufacturer1.Name : string.Empty;
                ModelInd = poechildren.EquipmentTemplate != null ? poechildren.EquipmentTemplate.Model : string.Empty;
                SerialInd = poechildren != null ? poechildren.SerialNumber : string.Empty;
            }
            else
            {
                ManInd = poe.Indicator != null && poe.Indicator.EquipmentTemplate != null && poe.Indicator.EquipmentTemplate.Manufacturer1 != null ? poe.Indicator.EquipmentTemplate.Manufacturer1.Name : string.Empty;
                ModelInd = poe.Indicator != null && poe.Indicator.EquipmentTemplate != null ? poe.Indicator.EquipmentTemplate.Model : string.Empty;
                SerialInd = poe.Indicator != null ? poe.Indicator.SerialNumber : string.Empty;
            }

            UnitOfMeasure umm = new UnitOfMeasure();
            umm.UnitOfMeasureID = poe.UnitOfMeasureID.Value;
            var um1 = (await UoMRepository.GetByID(umm)).Abbreviation;

            double totLin = 0;
            double totEcc = 0;
            double totRepAsFound = 0;
            double totRepAsLeft = 0;

            if (_linearity?.Count > 0)
            {
                totLin = _linearity.Sum(x => x.BasicCalibrationResult.AsLeft);
            }



            if (_eccentricity?.Count() > 0)
            {
                totEcc = _eccentricity.Sum(x => x.AsLeft);
            }


            //Standard Deviation Repeatability
            double stdDevAsFoundRep = 0;
            double stdDevAsLeftRep = 0;
            if (_repeatability?.Count() > 0)
            {
                totRepAsLeft = _repeatability.Sum(x => x.AsLeft);
                totRepAsFound = _repeatability.Sum(x => x.AsFound);
                stdDevAsFoundRep = CalculateStandardDeviation(_repeatability.Select(r => r.AsFound).ToList());
                stdDevAsLeftRep = CalculateStandardDeviation(_repeatability.Select(r => r.AsLeft).ToList());
            }

            var county = "";
            if (address.County != null)
                county = address.County;

            var addres = address.StreetAddress1 + ", " + address.CityID + ", " + address.StateID + " " + address.ZipCode;

            string Class = "";
            if (poe != null && poe.Class != null)
                Class = poe.Class;

            string platSize = "";
            if (poe.EquipmentTemplate != null && poe.EquipmentTemplate.PlatformSize != null)
                platSize = poe.EquipmentTemplate.PlatformSize;


            // Construct Certificate ID in format: {WorkOrderId}-{Sequence}-Ver{Version}
            string certificateId = await ConstructCertificateId(workOrderDetail);

            // Get comprehensive statements using the proper method that retrieves all note types
            // This includes Equipment Type notes, Test Code notes, and Work Order Detail notes
            var genericStatements = await GetGenericStatement(workOrderDetail);

            if (genericStatements != null && genericStatements.Count > 0)
            {
                foreach (var genericStatement in genericStatements)
                {
                    var statement = new GenericStatementWOD();
                    statement.Statement = genericStatement.Statement;
                    statement.DataGrid = genericStatement.DataGrid;
                    statements.Add(statement);
                }
            }

            //Calibrated For
            string CompanyName = "";
            string Address1 = "";
            string Address2 = "";
            string City = "";
            string ZipCode = "";
            string State = "";

            string CompanyNameFor = "";
            string Address1For = "";
            string Address2For = "";
            string CityFor = "";
            string ZipCodeFor = "";
            string StateFor = "";


            if (address1 != null)
            {
               
                Address1 = address1?.FirstOrDefault().StreetAddress1;
                Address2 = address1?.FirstOrDefault().StreetAddress2;
                City = address1?.FirstOrDefault().City;
                State = address1?.FirstOrDefault().State;
                ZipCode = address1?.FirstOrDefault().ZipCode;

                CompanyNameFor = address?.Name ?? "";
                    Address1For = address?.StreetAddress1 ?? "";
                    Address2For = address?.StreetAddress2 ?? "";
                    CityFor = address?.City ?? "";
                    ZipCodeFor = address?.ZipCode ?? "";
                    StateFor = address?.State ?? "";

                }

            string received = await GetEquipmentCondition(workOrderDetail, 1);
            string returned = await GetEquipmentCondition(workOrderDetail, 2);
            //Calculate Pass/Fail AsFound AsLeft Header

            if (asFoundL?.Count() == _linearity?.Count() && asFoundR?.Count() == _repeatability?.Count() && asFoundE?.Count() == _eccentricity?.Count())
            {
                _AsFoundResult = "Pass";
            }
            else
            {
                _AsFoundResult = "Fail";
            }

            if (asLeftL?.Count() == _linearity?.Count() && asLeftR?.Count() == _repeatability?.Count() && asLeftE?.Count() == _eccentricity?.Count())
            {
                _AsLeftResult = "Pass";
            }
            else
            {
                _AsLeftResult = "Fail";
            }

            Header header = new Header()
            {
                CalibrationLocation1 = poe.Customer.Name,
                CalibrationLocation2 = Address1,
                CalibrationLocation3 = Address2,
                CalibrationLocation4 = City + " " + State,
                CalibrationLocation5 = ZipCode,
                CalibratedFor1 = poe.Customer.Name,
                CalibratedFor2 = Address1For,
                CalibratedFor3 = Address2For,
                CalibratedFor4 = CityFor + " " + StateFor,
                CalibratedFor5 = ZipCodeFor,
                Client = customer.Name,
                Address = addres,
                City = address.City,
                State = address.State,
                ZipCode = address.ZipCode,
                Country = customer.Aggregates.FirstOrDefault().Addresses.FirstOrDefault().Country,
                EquipmentLocation = poe.InstallLocation,
                EquipmentType = eqTemp.EquipmentTypeObject.Name,
                UoM = um1,
                NextCalDate = workOrderDetail.CalibrationCustomDueDate.Value.ToString("MM/dd/yyyy"),
                LastCalDate = lastCaldate, //OJOO no está mapeado
                ManufacturerInd = ManInd,
                ModelInd = ModelInd,
                SerialInd = SerialInd,
                CapInd = eqTemp.Capacity.ToString(),
                ManufacturerReceiv = manufName,
                ModelIndReceiv = eqTemp.Model,
                SerialIndReceiv = poe.SerialNumber,
                CapIndReceiv = poe.Capacity.ToString() + " " + um1, //OJOO no está mapeado
                Class = Class,
                Type = poe.EquipmentTemplate.DeviceClass, //OJOO no está mapeado
                PlatformSize = platSize,// null, //OJOO no está mapeado
                ServiceLocation = "", //OJOO no está mapeado
                UnitNumber = "", //OJOO no está mapeado
                TestingMethod = "", //OJOO no está mapeado
                CalID = poe.CustomerToolId, //OJOO no está mapeado
                Location = poe.InstallLocation, //OJOO no está mapeado
                AsLeftResult = _AsLeftResult, //OJOO no está mapeado
                AsFoundResult = _AsFoundResult, //OJOO no está mapeado
                CalibrtionDate = workOrderDetail.CalibrationDate.Value.ToString("MM/dd/yyyy"),
                Temperature = workOrderDetail.Temperature.ToString(),
                Humidity =  workOrderDetail.Humidity.ToString(),
                Enviroment = workOrderDetail.Environment,
                Technician = techreview,//workOrderDetail1.Technician.Name + " " + workOrderDetail1.Technician.LastName
                TechnicianAprove = tecertiied,
                UnitOfMeasure = um1,
                Tolerance = ToleranceList.GetValueOrDefault(workOrderDetail.Tolerance.ToleranceTypeID.ToString()),
                Resolution = workOrderDetail.Resolution.ToString(),
                EquipmentID = poe.PieceOfEquipmentID.ToString(),
                Capacity = eqTemp.Capacity.ToString(),
                TotalAsFoundEcc = totEcc,
                TotalAsFoundLin = totLin,
                TotalAsFoundRep = totRepAsFound,
                TotalAsLeftRep = totRepAsLeft,
                CertficateComment = workOrderDetail.CertificateComment,
                ReportNumber = workOrderDetail.WorkOrderDetailUserID,
                CertificateId = certificateId,
                StDevRepeatAsFound = stdDevAsFoundRep.ToString("F5"),
                StDevRepeatAsLeft = stdDevAsLeftRep.ToString("F5"),
                GenericStatementsWOD = statements,
                CustomerTool = poe.CustomerToolId,
                CompanyName = poe.Customer.Name,
                ReceivedCondition = received,
                ReturnedCondition = returned,
                CalibrationDueDate = poe.DueDate.ToString("MM/dd/yyyy"),
                CustomerId = customer.CustomerID,
                CalibrationCertificate = workOrderDetail.WorkOrderDetailUserID,
                InstallLocation = poe.InstallLocation,
            };

            if (workOrderDetail.IsAccredited.HasValue)
            {
                header.IsAccredited = workOrderDetail.IsAccredited.Value;
            }
            
            //
            //Calculate StandardDeviation to Repeatability

            double maxValueUsP = 0;
            Reports.Domain.ReportViewModels.USP41 uSP_41 = new Reports.Domain.ReportViewModels.USP41();
            TotalsRepeatabilityAdvance totalsRepeatabilityAdvance = new TotalsRepeatabilityAdvance();
                //Calculate  RepeatabilityPassCriteria
                if (_repeatability != null && _repeatability.Count() > 0)
                {


                    double repeatabilityCriteriaPercentage = 0;

                    double repeatabilityStdDeviationAsLeft = CalculateStandardDeviation(_repeatability.Select(r => r.AsLeft).ToList());
                    double repeReadingStandard = (double)_repeatability.FirstOrDefault().WeightApplied;


                    repeatabilityCriteriaPercentage = Math.Round((repeatabilityStdDeviationAsLeft * 2) / repeReadingStandard, 7);

                    string repeatabilityCriteriaResult = "";

                    if (repeatabilityCriteriaPercentage > 0.1)
                    {
                        repeatabilityCriteriaResult = "Fail";
                    }
                    else
                    {
                        repeatabilityCriteriaResult = "Pass";
                    }


                    var totalRep = Math.Round(repeatabilityCriteriaPercentage, QueryableExtensions2.CalculateResolution(Convert.ToDecimal(resol))).ToString() + "% - " + repeatabilityCriteriaResult.ToString();
                    uSP_41.RepeatabilityPassCriteria = totalRep.ToString();

                    /// Calculate As Left Standard Deviation:
                    double asLeftStandardDeviation = CalculateStandardDeviation(_repeatability.Select(r => r.AsLeft).ToList());
                    uSP_41.AsLeftStandardDeviation = Math.Round(asLeftStandardDeviation, QueryableExtensions2.CalculateResolution(Convert.ToDecimal(resol))).ToString();
                   double asFoundStandardDeviation = CalculateStandardDeviation(_repeatability.Select(r => r.AsFound).ToList());

                //Calculate Resolution x 0.41
                double resolution = (double)workOrderDetail1?.PieceOfEquipment?.EquipmentTemplate?.Resolution * 0.41;
                    uSP_41.Resolution = resolution.ToString();

                    //Max Value
                    double maxValue = 0;
                    if (resolution > repeatabilityStdDeviationAsLeft)
                    {
                        maxValueUsP = resolution;
                    }
                    else
                    {
                        maxValueUsP = Math.Round(repeatabilityStdDeviationAsLeft, QueryableExtensions2.CalculateResolution(Convert.ToDecimal(resol)));
                    }

                    totalsRepeatabilityAdvance.StdevAsFound = asFoundStandardDeviation.ToString();
                    totalsRepeatabilityAdvance.StdevAsLeft = asLeftStandardDeviation.ToString();
                    }

            header.totalsRepeatabilityAdvance = totalsRepeatabilityAdvance;
            #region "USP41"
            if (workOrderDetail1.IsUSP41 == true)
            {

                uSP_41.MaxValue = maxValueUsP.ToString();

                //Minimum Weight or SNW (Max Value x 2000)
                var maxValuex2000 = Math.Round(maxValueUsP * 2000, 7);
                uSP_41.MinimumWeight = maxValuex2000.ToString() + " " + um1;
            }
            else
            {
                uSP_41 = null;
            }
            #endregion
            if ((workOrderDetail.CalibrationTypeID != 1000 && cust != "Advance") || (workOrderDetail.CalibrationTypeID == 102 && cust == "Advance"))
            {
                #region Data CalCertBlank
                List<AsFound> AsFoundList = new List<AsFound>();
                double tur = 0;

                foreach (var af in _linearity)
                {
                    var um = (await UoMRepository.GetByID(af.TestPoint.UnitOfMeasurement)).Abbreviation;

                    double res = 0;
                    if (af.BasicCalibrationResult.Resolution == 0)
                    {
                        res = workOrderDetail.Resolution;

                    }

                    IEnumerable<WeightSet> weightSets = new List<WeightSet>();
                    weightSets = await PieceOfEquipmentRepository.GetWeigthSets();

                    var totalUncertainty = await Poe.GetReportUncertaintyBudget(af, af.SequenceID, workOrderDetail1, workOrderDetail, poes.ToList(), weightSets.ToList());
                    ////
                    ///Uncertainty CMC
                    ///
                    if (af.BasicCalibrationResult.UncertaintyNew != null && af.BasicCalibrationResult.UncertaintyNew != 0)
                    {
                        af.TotalUncertainty = (double)af.BasicCalibrationResult.UncertaintyNew;

                    }
                    ///TUR AsFound
                    ///

                    double xpandedUncert = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(totalUncertainty.Totales.ExpandedUncertainty)));
                    if (toleranceTUR != null)
                    {
                        var lowTolerance = ValidTolerance(af.TestPoint.NominalTestPoit, (int)toleranceTUR.ToleranceTypeID, toleranceTUR.Resolution, toleranceTUR.AccuracyPercentage, "low");
                        var maxTolerance = ValidTolerance(af.TestPoint.NominalTestPoit, (int)toleranceTUR.ToleranceTypeID, toleranceTUR.Resolution, toleranceTUR.AccuracyPercentage, "max");
                        var toleranceDifference = maxTolerance - lowTolerance;
                        var uncertx2 = (xpandedUncert * 2);
                        if (uncertx2 == 0)
                        {
                            uncertx2 = 1;
                        }

                        tur = (toleranceDifference / uncertx2);
                    }
                    var value = QueryableExtensions2.Completezero(af.BasicCalibrationResult.AsFound.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(af.TestPoint.Resolution)));

                    var AsFoundLin = new Reports.Domain.ReportViewModels.AsFound
                    {
                        Description = af?.TestPoint?.Description,
                        Standard = QueryableExtensions2.Completezero(af?.TestPoint?.NominalTestPoit.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(af.TestPoint.Resolution))).ToString(),
                        //af?.TestPoint?.NominalTestPoit.ToString() + " " + um,// af.TestPoint.NominalTestPoit.ToString() + " " + um,
                        Tolerance = af.TestPoint.LowerTolerance + "-" + af.TestPoint.UpperTolerance, //af.TestPoint.LowerTolerance,
                        PassFail = char.ToUpper(af.BasicCalibrationResult.InToleranceFound[0]) + af.BasicCalibrationResult.InToleranceFound.Substring(1),
                        Indication = value.ToString(),
                        Uncertainty = af.TotalUncertainty.ToString(), //af.BasicCalibrationResult.Uncertainty,
                        Range = 0, //OJOO no está mapeado
                        Value = value,
                        TUR = RoundFirstSignificantDigit(Convert.ToDecimal(tur)).ToString(),
                        ToleranceMin = Math.Round(Convert.ToDouble(af?.TestPoint?.LowerTolerance), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(af.TestPoint.Resolution))).ToString(),
                        ToleranceMax = Math.Round(Convert.ToDouble(af?.TestPoint?.UpperTolerance), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(af.TestPoint.Resolution))).ToString(),

                    };
                    AsFoundList.Add(AsFoundLin);

                    af.TotalUncertainty = totalUncertainty.Totales.ExpandedUncertainty;
                }

                List<AsLeft> AsLeftList = new List<AsLeft>();

                if (_linearity != null && _linearity.Count > 0)
                {
                    foreach (var af in _linearity.OrderBy(x => x.TestPoint.Position).ToList())
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

                        var um = (await UoMRepository.GetByID(af.TestPoint.UnitOfMeasurement)).Abbreviation;
                        var AsLeftLin = new Reports.Domain.ReportViewModels.AsLeft
                        {
                            Description = af?.TestPoint?.Description,
                            Standard = QueryableExtensions2.Completezero(af?.TestPoint?.NominalTestPoit.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(af.TestPoint.Resolution))).ToString(),
                            Tolerance = tolerance,//af.TestPoint.LowerTolerance + "-" + af.TestPoint.UpperTolerance,
                            PassFail = char.ToUpper(af.BasicCalibrationResult.InToleranceLeft[0]) + af.BasicCalibrationResult.InToleranceLeft.Substring(1),
                            //Description = af.TestPoint.Description, 
                            Indication = QueryableExtensions2.Completezero(af.BasicCalibrationResult.AsLeft.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(af.TestPoint.Resolution))),
                            Value = QueryableExtensions2.Completezero(af.BasicCalibrationResult.AsLeft.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(af.TestPoint.Resolution))),
                            Range = 0,  //OJOO no está mapeado
                            Uncertainty = af.TotalUncertainty.ToString(),
                            TUR = RoundFirstSignificantDigit(Convert.ToDecimal(tur)).ToString(),
                            ToleranceMin = QueryableExtensions2.Completezero(af?.TestPoint?.LowerTolerance.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(af.TestPoint.Resolution))).ToString(),
                            ToleranceMax = QueryableExtensions2.Completezero(af?.TestPoint?.UpperTolerance.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(af.TestPoint.Resolution))).ToString(),

                        };
                        AsLeftList.Add(AsLeftLin);
                    }
                }

                List<Reports.Domain.ReportViewModels.Lineartiy> linList = new List<Reports.Domain.ReportViewModels.Lineartiy>();

                if (_linearity != null)
                {
                    foreach (var lin in _linearity.OrderBy(x => x.SequenceID))
                    {

                        var um = (await UoMRepository.GetByID(lin.BasicCalibrationResult.UnitOfMeasure)).Abbreviation;

                        double res = 0;


                        var linearity = new Reports.Domain.ReportViewModels.Lineartiy
                        {
                            AsFound = QueryableExtensions2.Completezero(lin.BasicCalibrationResult.AsFound.ToString(), NumericExtensions.CalculateDecimalNumber(Convert.ToDecimal(res))),
                            AsLeft = QueryableExtensions2.Completezero(lin.BasicCalibrationResult.AsLeft.ToString(), NumericExtensions.CalculateDecimalNumber(Convert.ToDecimal(res))),
                            Position = lin.BasicCalibrationResult.Position,
                            Weight = QueryableExtensions2.Completezero(lin.BasicCalibrationResult.WeightApplied.ToString(), NumericExtensions.CalculateDecimalNumber(Convert.ToDecimal(res))),
                            PassFail = lin.BasicCalibrationResult.InToleranceLeft,
                            Uncertainty = lin.TotalUncertainty.ToString(),
                            UOM = um,
                            Tolerance = Math.Round(lin.MinTolerance, NumericExtensions.CalculateDecimalNumber(Convert.ToDecimal(res))).ToString() + "-" + Math.Round(lin.MaxTolerance, NumericExtensions.CalculateDecimalNumber(Convert.ToDecimal(res))).ToString()
                        };
                        linList.Add(linearity);
                    }
                }



                List<Reports.Domain.ReportViewModels.Excentricity> eccList = new List<Reports.Domain.ReportViewModels.Excentricity>();
                TestPoint toleranceEcc = new TestPoint();
                if (workOrderDetail.TestGroups != null)
                {
                    toleranceEcc = workOrderDetail.TestGroups.FirstOrDefault().TestPoints.Where(x => x.CalibrationType == "Eccentricity").FirstOrDefault();
                }
                List<double> valueseccAsFound = new List<double>();
                List<double> valueseccAsLeft = new List<double>();


                if (_eccentricity != null && _eccentricity?.Count() > 0)
                {


                    foreach (var ex in _eccentricity.OrderBy(x => x.SequenceID))
                    {
                        var um = (await UoMRepository.GetByID(ex.UnitOfMeasure)).Abbreviation;


                        double res = ex.Resolution;
                        if (ex.Resolution == 0)
                        {
                            res = workOrderDetail.Resolution;

                        }

                        string AsFound = "";

                        string AsLeft = "";

                        int Position = 0;

                        string Weight = "";

                        string PassFailAsFound = "";

                        string PassFailAsLeft = "";

                        string Tolerance = "";

                        if (ex != null)
                        {

                            AsFound = QueryableExtensions2.Completezero(ex.AsFound.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))); // + " " + um;
                            var valueAF = Convert.ToDouble(QueryableExtensions2.Completezero(ex.AsFound.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))));
                            valueseccAsFound.Add(valueAF);

                            AsLeft = QueryableExtensions2.Completezero(ex.AsLeft.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))); // + " " + um;
                            var valueAL = Convert.ToDouble(QueryableExtensions2.Completezero(ex.AsLeft.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))));
                            valueseccAsLeft.Add(valueAL);
                            Position = ex.Position;
                            Weight = QueryableExtensions2.Completezero(ex.WeightApplied.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))) + " " + um;// ex.WeightApplied.ToString() + " " + um
                            PassFailAsFound = char.ToUpper(ex.InToleranceFound[0]) + ex.InToleranceFound.Substring(1).ToLower();
                            PassFailAsLeft = char.ToUpper(ex.InToleranceLeft[0]) + ex.InToleranceLeft.Substring(1).ToLower();

                           if (toleranceEcc != null)
                            {
                                Tolerance = Math.Round(Convert.ToDecimal(toleranceEcc.LowerTolerance), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))).ToString() + " - " + Math.Round(Convert.ToDecimal(toleranceEcc.UpperTolerance), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))).ToString();
                            }

                        }

                        ///IsAcreddited Wod
                        if (cust == "Bitterman")
                        {
                            double? fiveTestPointAsFound = _eccentricity.Count() > 4 ? _eccentricity.ElementAt(4).AsFound : null;
                            double fiveValueAsFound = fiveTestPointAsFound.GetValueOrDefault();

                            double? fiveTestPointAsLeft = _eccentricity.Count() > 4 ? _eccentricity.ElementAt(4).AsLeft : null;
                            double fiveValueAsLeft = fiveTestPointAsLeft.GetValueOrDefault();
                            if (workOrderDetail1.IsAccredited == true && ex.Position < 5)
                            {
                                AsFound = Math.Round((Convert.ToDouble(AsFound) - fiveValueAsFound), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))).ToString();
                                AsLeft = Math.Round((Convert.ToDouble(AsLeft) - fiveValueAsLeft), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))).ToString();

                            }
                        }

                        var ecc = new Reports.Domain.ReportViewModels.Excentricity
                        {
                            AsFound = QueryableExtensions2.Completezero(AsFound.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))) + " " + um,
                            AsLeft = QueryableExtensions2.Completezero(AsLeft.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))) + " " + um,
                            Position = Position,
                            Weight = Weight,
                            PassFailAsFound = PassFailAsFound,
                            PassFailAsLeft = PassFailAsLeft,
                            Tolerance = Tolerance,

                        };
                        eccList.Add(ecc);
                    };
                }

                List<Reports.Domain.ReportViewModels.Repeteab> repetList = new List<Reports.Domain.ReportViewModels.Repeteab>();
                bool _viewGrid = false;

                if (_repeatability != null && _repeatability.Count() > 0)
                {
                    _viewGrid = true;
                    var um = (await UoMRepository.GetByID(_repeatability.FirstOrDefault().UnitOfMeasure)).Abbreviation;

                    foreach (var re in _repeatability)
                    {

                        string passFailAsFound = "";
                        string passFailAsLeft = "";

                        if (re.InToleranceFound != null)
                            passFailAsFound = char.ToUpper(re.InToleranceFound[0]) + re.InToleranceFound.Substring(1).ToLower();

                        if (re.InToleranceLeft != null)
                            passFailAsLeft = char.ToUpper(re.InToleranceLeft[0]) + re.InToleranceLeft.Substring(1).ToLower();


                        double res = re.Resolution;
                        if (re.Resolution == 0)
                        {
                            res = workOrderDetail.Resolution;

                        }
                        var lowerTolerance = re.WeightApplied - re.Resolution;
                        var upperTolerance = re.WeightApplied + re.Resolution;
                        var rep = new Reports.Domain.ReportViewModels.Repeteab
                        {
                            AsFound = Math.Round(re.AsFound, QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))) + " " + um,// re.AsFound + " " + um,
                            AsLeft = Math.Round (re.AsLeft, QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))) + " " + um,//re.AsLeft + " " + um,
                            Position = re.Position,
                            Standard = QueryableExtensions2.Completezero(re.WeightApplied.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(res))) + " " + um,// re.WeightApplied + " " + um, //OJOO no está mapeado//
                            ViewGrid = _viewGrid,
                            Tolerance = RoundFirstSignificantDigit(Convert.ToDecimal(lowerTolerance)).ToString() + "-" + RoundFirstSignificantDigit(Convert.ToDecimal(upperTolerance)).ToString(), //af.TestPoint.LowerTolerance,
                            PassFailAsFound = passFailAsFound,
                            PassFailAsLeft = passFailAsLeft,
                        };
                        repetList.Add(rep);
                    };

                }

                List<Reports.Domain.ReportViewModels.ExcentricityDet> excentriDetList = new List<Reports.Domain.ReportViewModels.ExcentricityDet>();



                if (weigthSets != null)
                {
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
                }



                var conditions = new List<Reports.Domain.ReportViewModels.Condition>();

                if (workOrderDetail.EquipmentCondition != null && workOrderDetail?.EquipmentCondition?.Count() > 0)
                {
                    foreach (var item in workOrderDetail.EquipmentCondition)
                    {
                        var condition = new Reports.Domain.ReportViewModels.Condition()
                        {
                            Name = item.Name,
                            Value = item.Value.ToString(),
                            AsFound = item.IsAsFound.ToString(),
                            Label = item.Label

                        };
                        conditions.Add(condition);
                    }
                }

                UnitOfMeasure umTemp = new UnitOfMeasure();
                umTemp.UnitOfMeasureID = workOrderDetail.TemperatureUOMID;
                var x = await UoMRepository.GetByID(umTemp);

                string umTemperature = "";

                if (x != null && workOrderDetail.Temperature != 0)
                {
                    umTemperature = workOrderDetail.TemperatureAfter.ToString() + " " + x.Abbreviation;
                }

                UnitOfMeasure umHumid = new UnitOfMeasure();
                umHumid.UnitOfMeasureID = workOrderDetail.HumidityUOMID;

                var y = await UoMRepository.GetByID(umHumid);
                string umHumidity = "";

                if (y != null && workOrderDetail.Humidity != 0)
                {
                    umHumidity = workOrderDetail.Humidity.ToString() + " " + y.Abbreviation;
                }
                var env = new Reports.Domain.ReportViewModels.Enviroment()
                {
                    Temperature = umTemperature,
                    Humidity = umHumidity,
                    Conditions = conditions



                };

                #endregion


                header.RepeteabilityList = repetList;
                header.eccList = eccList;
                header.excentriDetList = excentriDetList;
                header.AsFoundList = AsFoundList;
                header.AsLeftList = AsLeftList;
                header.WeightSetList = listw;
                header.Environment = env;
                header.linList = linList;
                header.USP41 = uSP_41;

                var asFoundValues = valueseccAsFound;
                var maxAsFoundEcc = asFoundValues?.Count > 0 ? asFoundValues.Max() : 0;
                var minAsFoundEcc = asFoundValues?.Count > 0 ? asFoundValues.Min() : 0;

                var asLeftValues = valueseccAsLeft;
                var maxAsLeftEcc = asLeftValues?.Count > 0 ? asLeftValues.Max() : 0;
                var minAsLeftEcc = asLeftValues?.Count > 0 ? asLeftValues.Min() : 0;


                header.MaxMinAsFoundtEcc = QueryableExtensions2.Completezero(minAsFoundEcc.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(workOrderDetail1.Resolution))).ToString() + " - " + QueryableExtensions2.Completezero(maxAsFoundEcc.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(workOrderDetail1.Resolution))).ToString();
                header.MaxMinAsLeftEcc = QueryableExtensions2.Completezero(minAsLeftEcc.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(workOrderDetail1.Resolution))).ToString() + " - " + QueryableExtensions2.Completezero(maxAsLeftEcc.ToString(), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(workOrderDetail1.Resolution))).ToString();

                if (Save)
                {
                    var serialHash = header.SerialIndReceiv.GetSHA1Has();


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

                    cert.Description = @"/Certificate/" + serialHash + ".pdf";
                    cert.Version = Convert.ToInt32(wo.WorkOrderDetailHash);
                    cert.WorkOrderDetailId = wo.WorkOrderDetailID;
                    var serial = JsonConvert.SerializeObject(workOrderDetail1);

                    cert.WorkOrderDetailSerialized = serial;
                    var resultwhd = CreateCertificate(cert);
                }

            }
            else if (workOrderDetail.CalibrationTypeID == 1000 && cust == "Bitterman")
            {
                header.WeightSetList = listw;
                header = await GetReportTruck(workOrderDetail, header);
            }
            else if (cust == "Advance")
            {
                header.WeightSetList = listw;
                header = await GetReportTruckAdvance(workOrderDetail, header,useraproved,techreview);
            }
            if (totalsRepeatabilityAdvance != null)
            {
                totalsRepeatabilityAdvance.StdevAsFound = Math.Round(Convert.ToDouble(totalsRepeatabilityAdvance.StdevAsFound ?? "0"), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(resol))).ToString();
                totalsRepeatabilityAdvance.StdevAsLeft = Math.Round(Convert.ToDouble(totalsRepeatabilityAdvance.StdevAsLeft ?? "0"), QueryableExtensions2.CalculateResolution(Convert.ToDecimal(resol))).ToString();
                header.totalsRepeatabilityAdvance = new TotalsRepeatabilityAdvance();
                header.totalsRepeatabilityAdvance = totalsRepeatabilityAdvance;
            }
            return header;


        }


        public async ValueTask<Header> GetWorkOrderDetailXIdRepGenericAdvance(Domain.Aggregates.Entities.WorkOrder wo)
        {
            var workOrder = await Assets.GetWorkOrderByID(wo.WorkOrderId);
            List<WorkOrderDetail> workOrderDetails = new List<WorkOrderDetail>();

            CalibrationType calibrationType = new CalibrationType();

            

            foreach (var item in workOrder.WorkOrderDetails)
            {
                calibrationType.CalibrationTypeId = (int)item.CalibrationTypeID;
                calibrationType = await Basics.GetCalibrationTypeByID(calibrationType);

                WorkOrderDetail workOrderDetail = new WorkOrderDetail();
                workOrderDetail = await GetByID(item);
                workOrderDetail.calibrationType = calibrationType;
                workOrderDetails.Add(workOrderDetail);
            }
            
            var customer = workOrder.Customer;
            string ManInd;
            string ModelInd;
            string SerialInd;
            string Manufact;
            string Model;

            List<TestPointsAdvance> TestPointsTAdvance_ = new List<TestPointsAdvance>();
            List<Advance> advances = new List<Advance>();
            var statements = new List<GenericStatementWOD>();
            int level = 0;
            string ety = "";
            foreach (var item in workOrderDetails)
            {
                UnitOfMeasure umm = new UnitOfMeasure();
                umm.UnitOfMeasureID = item.PieceOfEquipment.UnitOfMeasureID.Value;
                var um1 = (await UoMRepository.GetByID(umm)).Abbreviation;
                var act = "";
                TestPointsAdvance testPoint = new TestPointsAdvance();
                TestPointsTAdvance_ = new List<TestPointsAdvance>();

                if (item.calibrationType.IsNivel1 == true && item.BalanceAndScaleCalibration != null && item.BalanceAndScaleCalibration.TestPointResult != null && item.BalanceAndScaleCalibration.TestPointResult.Count() > 0)
                {
                    int countTestPoints = item.BalanceAndScaleCalibration.TestPointResult.Where(x => x.ComponentID == item.WorkOrderDetailID.ToString() && x.Position != -1).Select(x => x.ExtendedObject).ToList().Count();
                    if (countTestPoints == 1)
                    {
                        ///TODO YPPP
                        string advance1 = item.BalanceAndScaleCalibration.TestPointResult.Where(x => x.ComponentID == item.WorkOrderDetailID.ToString() && x.Position != -1).Select(x => x.ExtendedObject).FirstOrDefault();
                        act = item.BalanceAndScaleCalibration.TestPointResult.Where(x => x.ComponentID == item.WorkOrderDetailID.ToString() && x.Position == -1).Select(x => x.Object).FirstOrDefault();

                        ///Level1

                        var jsonObjectAct = JsonNode.Parse(act)?.AsObject();
                        var jsonObject = JsonNode.Parse(advance1)?.AsObject();

                        testPoint = new TestPointsAdvance
                        {
                            WeightApplied = jsonObject?["WeightApplied"]?[2]?.ToString(),
                            AsFound = jsonObject?["AsFound"]?[2]?.ToString(),
                            AsLeft = jsonObject?["AsLeft"]?[2]?.ToString(),
                            Act1 = jsonObjectAct?["Act1"].ToString(),
                            Act2 = jsonObjectAct?["Act2"].ToString(),
                            Act3 = jsonObjectAct?["Act3"].ToString()

                        };

                        level = 1;
                        ety = "Level 1";
                    }
                    else if (countTestPoints > 1)
                    {
                        ///Level2
                        ///
                        List<GenericCalibrationResult2> advanceListLevel2 = item.BalanceAndScaleCalibration.TestPointResult.Where(x => x.ComponentID == item.WorkOrderDetailID.ToString() && x.Position != -1).ToList();
                        var actlevel2 = item.BalanceAndScaleCalibration.TestPointResult.Where(x => x.ComponentID == item.WorkOrderDetailID.ToString() && x.Position == -1).Select(x => x.Object).FirstOrDefault();

                        var jsonObjectActLevel2 = JsonNode.Parse(actlevel2)?.AsObject();
                        TestPointsAdvance testPointLevel2 = new TestPointsAdvance();

                        foreach (var item1 in advanceListLevel2)
                        {

                            string advance1evel2 = item1.ExtendedObject;



                            var jsonObjectLevel2 = JsonNode.Parse(advance1evel2)?.AsObject();

                            testPoint = new TestPointsAdvance
                            {
                                WeightApplied = jsonObjectLevel2?["WeightApplied"]?[2]?.ToString(),
                                AsFound = jsonObjectLevel2?["Asfound"]?[2]?.ToString(),
                                AsLeft = jsonObjectLevel2?["AsLeft"]?[2]?.ToString(),
                                Act1 = jsonObjectActLevel2?["Act1"].ToString(),
                                Act2 = jsonObjectActLevel2?["Act2"].ToString(),
                                Act3 = jsonObjectActLevel2?["Act3"].ToString()

                            };

                            TestPointsTAdvance_.Add(testPoint);

                        }
                        level = 2;
                        ety = "Level 2";
                    }
                }

                Advance advance = new Advance()
                {
                    Scale = item.PieceOfEquipment.CustomerToolId,
                    Location = item.PieceOfEquipment.InstallLocation,
                    Make = item.PieceOfEquipment.EquipmentTemplate.Manufacturer1.Name,
                    Model = item.PieceOfEquipment.EquipmentTemplate.Model,
                    SerialNumber = item.PieceOfEquipment.SerialNumber,
                    EquipmentType = ety,
                    UOM = um1,
                    AppliedWT = testPoint.WeightApplied,
                    AsFound = testPoint.AsFound,
                    AsLeft = testPoint.AsLeft,
                    Act = testPoint.Act1 + "," + testPoint.Act2 + "," + testPoint.Act3,
                    TestPointsAdvances = TestPointsTAdvance_

                };

                advances.Add(advance);

                if (item.CertificateComment != null)
                {
                var statement = new GenericStatementWOD();
                statement.Statement = item.CertificateComment;
                statement.DataGrid = item.PieceOfEquipment.CustomerToolId;
                statements.Add(statement);
                }
            }

            List<WeightSetHeader> listw = new List<WeightSetHeader>();

            List<PieceOfEquipment> poes = new List<PieceOfEquipment>();
            //var poes = await PieceOfEquipmentRepository.GetAllWeightSets(poe);



            var address1 = customer.Aggregates.FirstOrDefault().Addresses.ToList();

            Address address = new Address();
            string techAproved = "";
            string tecertiied = "";
            address = address1.FirstOrDefault();

            //  var techApproved = await GetTechnician(workOrderDetail1, "aproved");
            //  var techReview = await GetTechnician(workOrderDetail1, "review");

            var techReview = workOrder?.UserWorkOrders?.FirstOrDefault().User?.Name;
            var county = "";
            if (address.County != null)
                county = address.County;

            var addres = address.StreetAddress1 + " " + ' ' + address.City + ' ' + county + " " + address.Country + " " + address.ZipCode;


            Header header = new Header()
            {
               
                WorkOrderId = workOrder.WorkOrderId.ToString(),
                Client = customer.Name,
                Address = address.StreetAddress1 + " ", //+ ' ' + address.City + ' ' + county + " " + address.Country + " " + address.ZipCode,
                Country = "Not Found",//customer.Aggregates.FirstOrDefault().Addresses.FirstOrDefault().Country,
                CalibrtionDate = workOrder.CalibrationDate.HasValue ? workOrder.CalibrationDate.Value.ToString("MM/dd/yyyy") : "NA",
                CalDueDate = workOrder.NextDueDate.HasValue ? workOrder.NextDueDate.Value.ToString("MM/dd/yyyy") : "NA",
                // Location = poe.InstallLocation, //OJOO no está mapeado
                //Technician = workOrder.
                TechnicianAprove = tecertiied,
                Technician = techReview,
                AdvanceWods = advances,
                GenericStatementsWOD = statements,
                level = level,
                CalibratedFor = workOrder.Customer.Name

            };

           
            /////
            ///Standards
            ///
            if (workOrder.WO_Standard != null)
            {
                var testp = workOrder.WO_Standard.DistinctBy(x => x.PieceOfEquipmentID);

                foreach (var item in testp)
                {
                    PieceOfEquipment poetmp = new PieceOfEquipment();
                    if (item.PieceOfEquipmentID != null)
                    {
                        poetmp = await PieceOfEquipmentRepository.GetPieceOfEquipmentByIDHeader(item.PieceOfEquipmentID);
                    }
                    
                    var _listCetificate = await Assets.GetCertificateXPoE(poetmp);


                    CertificatePoE cert = _listCetificate.OrderByDescending(x => x.Version).FirstOrDefault();

                    string certnumber = "";
                    if (cert != null)
                    {
                        certnumber = cert.CertificateNumber;
                    }

                    //.CalibrationDate.Value.ToString("MM/dd/yyyy"),
                    WeightSetHeader ww = new WeightSetHeader()
                    {

                        PoE = poetmp.PieceOfEquipmentID,
                        Serial = poetmp.SerialNumber,
                        Ref = certnumber,
                        CalibrationDueDate = poetmp.DueDate.ToString("MM/dd/yyyy"),
                        Note = poetmp.Notes,
                        CalibrationDate = poetmp.CalibrationDate.ToString("MM/dd/yyyy"),
                        Description = poetmp.Description,
                        

                    };
                    listw.Add(ww);
                }
            }


            if (listw != null && listw.Count() == 0)
                header.WeightSetList = null;
            else
                header.WeightSetList = listw;




            return header;


        }

        //public async Task<Reports.Domain.ReportViewModels.Header> GetReportAdvance(WorkOrderDetail wo, Header header)
        //{


        //    //1 get GenericCalibrationResult2
        //    List<GenericCalibrationResult2> testpoints = new List<GenericCalibrationResult2>();
        //    List<string> extendedObjectList = null;


        //    List<Advance> advances = new List<Advance>();

        //    try
        //    {

        //        if (wo.BalanceAndScaleCalibration != null && wo.BalanceAndScaleCalibration.TestPointResult != null && wo.BalanceAndScaleCalibration.TestPointResult.Count() > 0)
        //        {

                   

        //            List<string> advanceList = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == 1).Select(x => x.ExtendedObject).ToList();


        //            foreach (var item in advanceList)
        //            {
        //                var jsonObject = JsonNode.Parse(item)?.AsObject();
        //                var testPointList = new Advance
        //                {
        //                    AppliedWT = jsonObject?["AsFound"]?[2]?.ToString(),
        //                    AsFound = jsonObject?["AsFound"]?[2]?.ToString(),
        //                    AsLeft = jsonObject?["AsLeft"]?[2]?.ToString()
        //                    //Act1 = jsonObject?["Act1"]?[2]?.ToString(),
        //                    //Act2 = jsonObject?["Act2"]?[2]?.ToString(),
        //                    //Act3 = jsonObject?["Act3"]?[2]?.ToString(),

        //                };

        //                TestPointsTAdvance_.Add(testPointList);

        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    header.TestPointsAdvances = TestPointsTAdvance_.ToList();

        //    return header;
        //}


        public async Task<Reports.Domain.ReportViewModels.Header> GetReportThermoTemp(WorkOrderDetail wo, Header header)
        {


            //1 get GenericCalibrationResult2
            List<GenericCalibrationResult2> testpoints = new List<GenericCalibrationResult2>();
            List<string> extendedObjectList = null;


            List<TestPointsThermoTemp> TestPointsThermoTemp_ = new List<TestPointsThermoTemp>();
          
            try
            {
                if (wo.BalanceAndScaleCalibration != null && wo.BalanceAndScaleCalibration.TestPointResult != null && wo.BalanceAndScaleCalibration.TestPointResult.Count() > 0)
                {


                    //List<string> termoThempList = wo.BalanceAndScaleCalibration.TestPointResult.Select(x => x.ExtendedObject).ToList();
                    var termoThempList = wo.BalanceAndScaleCalibration.TestPointResult
                    .Select(x => new
                    {
                        x.ExtendedObject,
                        x.GroupName
                    })
                    .ToList();

                    foreach (var item in termoThempList)
                    {
                        var jsonObject = JsonNode.Parse(item.ExtendedObject)?.AsObject();
                        var testPointList = new TestPointsThermoTemp
                        {
                            FunctionTested = jsonObject?["FunctionTested"]?[2]?.ToString(),
                            Nominal = jsonObject?["Nominal"]?[2]?.ToString(),
                            AsFound = jsonObject?["AsFound"]?[2]?.ToString(),
                            OutOfTol1 = jsonObject?["OutofTol1"]?[2]?.ToString(),
                            AsLeft = jsonObject?["AsLeft"]?[2]?.ToString(),
                            OutOfTol2 = jsonObject?["OutofTol2"]?[2]?.ToString(),
                            CalibrationTolerance = jsonObject?["CalibrationTolerance"]?[2]?.ToString(),
                            //Uncertainties = jsonObject?["AsLeft"]?[2]?.ToString()
                           UoM = jsonObject?["UoM"]?[2]?.ToString(),
                           GroupName = item.GroupName
                        };

                        TestPointsThermoTemp_.Add(testPointList);

                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            header.TestPointsThermoTemps = TestPointsThermoTemp_;
           
            return header;
        }




        public async Task<List<TestPointsThermoTemp>> GetReportThermoTempChildren(List<GenericCalibrationResult2> tps)
        {


            //1 get GenericCalibrationResult2
            List<GenericCalibrationResult2> testpoints = tps;
            List<string> extendedObjectList = null;


            List<TestPointsThermoTemp> TestPointsThermoTemp_ = new List<TestPointsThermoTemp>();

            try
            {
                if (testpoints != null)
                {


                    List<string> termoThempList = testpoints.Select(x => x.ExtendedObject).ToList();


                    foreach (var item in termoThempList)
                    {
                        var jsonObject = JsonNode.Parse(item)?.AsObject();
                        var testPointList = new TestPointsThermoTemp
                        {
                            FunctionTested = jsonObject?["FunctionTested"]?[2]?.ToString(),
                            Nominal = jsonObject?["Nominal"]?[2]?.ToString(),
                            AsFound = jsonObject?["AsFound"]?[2]?.ToString(),
                            OutOfTol1 = jsonObject?["OutofTol1"]?[2]?.ToString(),
                            AsLeft = jsonObject?["AsLeft"]?[2]?.ToString(),
                            OutOfTol2 = jsonObject?["OutofTol2"]?[2]?.ToString(),
                            CalibrationTolerance = jsonObject?["CalibrationTolerance"]?[2]?.ToString(),
                            //Uncertainties = jsonObject?["AsLeft"]?[2]?.ToString()
                            UoM = jsonObject?["UoM"]?[2]?.ToString(),
                            
                        };

                        TestPointsThermoTemp_.Add(testPointList);

                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }


            return TestPointsThermoTemp_;
        }

     

        public async Task<Reports.Domain.ReportViewModels.Header> GetReportTruck(WorkOrderDetail wo, Header header)
        {
        
      
            //1 get GenericCalibrationResult2
            List<GenericCalibrationResult2> testpoints = new List<GenericCalibrationResult2>();
           List<string> extendedObjectList = null;
            List<LinearityTruck> LinearityList_ = new List<LinearityTruck>();
            AsFoundSectionHeader AsFoundSectionsHeader_ = new AsFoundSectionHeader();
           List<AsFoundSection> AsFoundSectionsList_ = new List<AsFoundSection>();
           
            Strain Strain_ = new Strain();
         
           AsLeftSectionsHeader AsLeftSectionsHeader_ = new AsLeftSectionsHeader();
            List<AsLeftSections> AsLeftSectionsList_ = new List<AsLeftSections>();
           
           CommentAsFound CommentAsFound_ = new CommentAsFound();
           CommentAsLeft CommentAsLeft_ = new CommentAsLeft();

            TotalsAsFound TotalsAsFound_ = new TotalsAsFound();
            TotalsAsLeft TotalAsLeft_  = new TotalsAsLeft();
            try
            {
                if (wo.BalanceAndScaleCalibration != null && wo.BalanceAndScaleCalibration.TestPointResult != null && wo.BalanceAndScaleCalibration.TestPointResult.Count() > 0)
                {


                    List<string> asfoundLinearityList = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == 1002).Select(x => x.ExtendedObject).ToList();

                    
                    foreach (var item in asfoundLinearityList)
                    {
                        var jsonObject = JsonNode.Parse(item)?.AsObject();
                        var asFoundLinearityList = new LinearityTruck
                        {
                            Description = jsonObject?["Description"]?[2]?.ToString(),
                            Weight = jsonObject?["Weight"]?[2]?.ToString(),
                            WeightApplied = jsonObject?["WeightApplied"]?[2]?.ToString(),
                            StepResol = jsonObject?["StepResol"]?[2]?.ToString(),
                            UoM = jsonObject?["UoM"]?[2]?.ToString(),
                            AsFound = jsonObject?["AsFound"]?[2]?.ToString(),
                            ResultAsFound = jsonObject?["ResultAsFound"]?[2]?.ToString(),
                            AsLeft = jsonObject?["AsLeft"]?[2]?.ToString(),
                            ResultAsLeft = jsonObject?["ResultAsLeft"]?[2]?.ToString(),
                            ToleranceMin = jsonObject?["ToleranceMin"]?[2]?.ToString(),
                            ToleranceMax = jsonObject?["ToleranceMax"]?[2]?.ToString(),
                        };
                        
                        LinearityList_.Add(asFoundLinearityList);

                    }

                    List<string> asFoundSectionsList = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == 1003).Select(x => x.ExtendedObject).ToList();
                    foreach (var item in asFoundSectionsList)
                    {
                        var jsonObject = JsonNode.Parse(item)?.AsObject();
                        var asFoundSectionList = new AsFoundSection
                        {
                            Section = jsonObject?["Section"]?[2]?.ToString(),
                            Pass1 = jsonObject?["Pass1"]?[2]?.ToString(),
                            Pass2 = jsonObject?["Pass2"]?[2]?.ToString(),
                            Left = jsonObject?["Left"]?[2]?.ToString(),
                            Right = jsonObject?["Right"]?[2]?.ToString(),
                          
                        };
                       
                        AsFoundSectionsList_.Add(asFoundSectionList);

                    }

                    string strain = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == 1005).Select(x => x.ExtendedObject).FirstOrDefault();
                    JsonObject jsonObjectStrain = new JsonObject();
                    if (strain != null)
                    {
                        jsonObjectStrain = JsonNode.Parse(strain)?.AsObject();

                        Strain_ = new Strain
                        {
                            UoM = jsonObjectStrain?["UoM"]?[2]?.ToString(),
                            EmptyTruck = jsonObjectStrain?["EmptyTruck"]?[2]?.ToString(),
                            TestWeightAdded = jsonObjectStrain?["TestWeightAdded"]?[2]?.ToString(),
                            TotalWeight = jsonObjectStrain?["TotalWeight"]?[2]?.ToString(),
                            IndicatedWeight = jsonObjectStrain?["IndicatedWeight"]?[2]?.ToString(),
                            Error1 = jsonObjectStrain?["Error1"]?[2]?.ToString(),
                            ToleranceRange = jsonObjectStrain?["ToleranceRange"]?[2]?.ToString(),
                            Decreasing = jsonObjectStrain?["Decreasing"]?[2]?.ToString(),
                            Error2 = jsonObjectStrain?["Error2"]?[2]?.ToString(),
                            ToleranceRange2 = jsonObjectStrain?["ToleranceRange2"]?[2]?.ToString(),


                        };
                    }
                    string commentAsFound = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == 1007).Select(x => x.ExtendedObject).FirstOrDefault();

                    var jsonObjectCommentAsFound = JsonNode.Parse(commentAsFound)?.AsObject();
                    CommentAsFound_ = new CommentAsFound
                    {
                        Comment = jsonObjectCommentAsFound?["Comment"]?[2]?.ToString()
                      
                    };

                    ///AsLeft
                    ///
                    List<string> asLeftSectionsList = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == 1004).Select(x => x.ExtendedObject).ToList();
                    foreach (var item in asLeftSectionsList)
                    {
                        var jsonObject = JsonNode.Parse(item)?.AsObject();
                        var asLeftSections = new AsLeftSections
                        {
                            Section = jsonObject?["Section"]?[2]?.ToString(),
                            Pass1 = jsonObject?["Pass1"]?[2]?.ToString(),
                            Pass2 = jsonObject?["Pass2"]?[2]?.ToString(),
                            Left = jsonObject?["Left"]?[2]?.ToString(),
                            Right = jsonObject?["Right"]?[2]?.ToString(),
                        };
                        AsLeftSectionsList_.Add(asLeftSections);

                    }
                    string commentAsLeft = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == 1006).Select(x => x.ExtendedObject).FirstOrDefault();

                    var jsonObjectCommentAsLeft = JsonNode.Parse(commentAsLeft)?.AsObject();
                    CommentAsLeft_ = new CommentAsLeft
                    {
                        Comment = jsonObjectCommentAsLeft?["Comment"]?[2]?.ToString()

                    };

                    string sectionsAsFoundHeader = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == 1003 && x.Position == -1).Select(x => x.ExtendedObject).FirstOrDefault();

                    var jsonObjectAFHeader = JsonNode.Parse(sectionsAsFoundHeader)?.AsObject();
                    AsFoundSectionsHeader_ = new AsFoundSectionHeader
                    {
                        Pass1AsFoundHeader = jsonObjectAFHeader?["Pass1AsFoundHeader"]?[2]?.ToString(),
                        Pass2AsFoundHeader = jsonObjectAFHeader?["Pass2AsFoundHeader"]?[2]?.ToString(),
                        LeftAsFoundHeader = jsonObjectAFHeader?["LeftAsFoundHeader"]?[2]?.ToString(),
                        RightAsFoundHeader = jsonObjectAFHeader?["RightAsFoundHeader"]?[2]?.ToString(),
                        TolerancePass1AsFound = jsonObjectAFHeader?["TolerancePass1AsFound"]?[2]?.ToString(),
                        TolerancePass2AsFound = jsonObjectAFHeader?["TolerancePass2AsFound"]?[2]?.ToString(),
                        ToleranceRightAsFound = jsonObjectAFHeader?["ToleranceRightAsFound"]?[2]?.ToString(),
                        ToleranceLeftAsFound = jsonObjectAFHeader?["ToleranceLeftAsFound"]?[2]?.ToString(),
                    };

                    string sectionsAsLeftHeader = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == 1004 && x.Position == -1).Select(x => x.ExtendedObject).FirstOrDefault();

                    if (sectionsAsLeftHeader != null)
                    {
                        var jsonObjectALHeader = JsonNode.Parse(sectionsAsLeftHeader)?.AsObject();
                        AsLeftSectionsHeader_ = new AsLeftSectionsHeader
                        {
                            Pass1AsLeftHeader = jsonObjectALHeader?["Pass1AsLeftHeader"]?[2]?.ToString(),
                            Pass2AsLeftHeader = jsonObjectALHeader?["Pass2AsLeftHeader"]?[2]?.ToString(),
                            LeftAsLeftHeader = jsonObjectALHeader?["LeftAsLeftHeader"]?[2]?.ToString(),
                            RightAsLeftHeader = jsonObjectALHeader?["RightAsLeftHeader"]?[2]?.ToString(),
                            TolerancePass1AsLeft = jsonObjectALHeader?["TolerancePass1AsLeft"]?[2]?.ToString(),
                            TolerancePass2AsLeft = jsonObjectALHeader?["TolerancePass2AsLeft"]?[2]?.ToString(),
                            ToleranceRightAsLeft = jsonObjectALHeader?["ToleranceRightAsLeft"]?[2]?.ToString(),
                            ToleranceLeftAsLeft = jsonObjectALHeader?["ToleranceLeftAsLeft"]?[2]?.ToString(),
                        };
                    }

                    if (AsFoundSectionsList_ != null && AsFoundSectionsList_.Count() > 0)
                    {
                        TotalsAsFound_ = new TotalsAsFound
                        {
                            MinPass1 = AsFoundSectionsList_.Min(x => x.Pass1),
                            MinPass2 = AsFoundSectionsList_.Min(x => x.Pass2),
                            MinLeft = AsFoundSectionsList_.Min(x => x.Left),
                            MinRight = AsFoundSectionsList_.Min(x => x.Right),
                            MaxPass1 = AsFoundSectionsList_.Max(x => x.Pass1),
                            MaxPass2 = AsFoundSectionsList_.Max(x => x.Pass2),
                            MaxLeft = AsFoundSectionsList_.Max(x => x.Left),
                            MaxRight = AsFoundSectionsList_.Max(x => x.Right)
                        };
                    }
                    if (AsLeftSectionsList_ != null && AsLeftSectionsList_.Count() > 0)
                    {
                        TotalAsLeft_ = new TotalsAsLeft
                        {
                            MinPass1 = AsLeftSectionsList_.Min(x => x.Pass1),
                            MinPass2 = AsLeftSectionsList_.Min(x => x.Pass2),
                            MinLeft = AsLeftSectionsList_.Min(x => x.Left),
                            MinRight = AsLeftSectionsList_.Min(x => x.Right),
                            MaxPass1 = AsLeftSectionsList_.Max(x => x.Pass1),
                            MaxPass2 = AsLeftSectionsList_.Max(x => x.Pass2),
                            MaxLeft = AsLeftSectionsList_.Max(x => x.Left),
                            MaxRight = AsLeftSectionsList_.Max(x => x.Right)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            header.LinearityTruckList = LinearityList_;
            header.AsFoundSectionsList = AsFoundSectionsList_?.Where(x => x.Section != null).ToList();
            header.Strain = Strain_;
            header.AsLeftSectionsList = AsLeftSectionsList_?.Where(x => x.Section != null).ToList(); ;
            header.CommentAsFound = CommentAsFound_;
            header.CommentAsLeft = CommentAsLeft_;
            header.AsFoundSectionsHeader = AsFoundSectionsHeader_;
            header.AsLeftSectionsHeader = AsLeftSectionsHeader_;
            header.TotalAsFound = TotalsAsFound_;
            header.TotalAsLeft = TotalAsLeft_;
            return header;
        }


        public async Task<Reports.Domain.ReportViewModels.Header> GetReportTruckAdvance(WorkOrderDetail wo, Header header, User useraproved, string techreview)
        {

             //1 get GenericCalibrationResult2
            List<GenericCalibrationResult2> testpoints = new List<GenericCalibrationResult2>();
            List<string> extendedObjectList = null;
            List<SectionTruckAdvance> SectionsTruckAdvanceList = new List<SectionTruckAdvance>();
            List<CertifiedWeight> CertifiedWeightsList = new List<CertifiedWeight>();
            List<SideToSideTest> SideToSideTestsList = new List<SideToSideTest>();
            List<StrainTest> StrainTestLis = new List<StrainTest>();
            List<TestValue> TestValuesList = new List<TestValue>();
            HeaderTruckScaleAdvance HeaderTruckScaleAdvance_ = new HeaderTruckScaleAdvance();
            try
            {
                if (wo.BalanceAndScaleCalibration != null && wo.BalanceAndScaleCalibration.TestPointResult != null && wo.BalanceAndScaleCalibration.TestPointResult.Count() > 0)
                {


                    List<string> sectionsTruckAdvance = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == 6).Select(x => x.ExtendedObject).ToList();


                    foreach (var item in sectionsTruckAdvance)
                    {

                        if (string.IsNullOrWhiteSpace(item)) continue;

                        JsonObject? jsonObject = JsonNode.Parse(item)?.AsObject();
                        if (jsonObject == null) continue;

                        var sectionArray = jsonObject["Section"] as JsonArray;
                        var testWeightArray = jsonObject["TestWeight"] as JsonArray;
                        var asFoundArray = jsonObject["AsFound"] as JsonArray;
                        var asLeftArray = jsonObject["AsLeft"] as JsonArray;

                        var sectionTruckAdvance = new SectionTruckAdvance
                        {
                            Section = sectionArray?.ElementAtOrDefault(2)?.ToString(),
                            TestWeight = testWeightArray?.ElementAtOrDefault(2)?.ToString(),
                            AsFound = asFoundArray?.ElementAtOrDefault(2)?.ToString(),
                            AsLeft = asLeftArray?.ElementAtOrDefault(2)?.ToString(),
                        };

                        SectionsTruckAdvanceList.Add(sectionTruckAdvance);

                    }

                    List<string> certifiedWeightsList = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == 8).Select(x => x.ExtendedObject).ToList();
                    foreach (var item in certifiedWeightsList)
                    {
                        if (string.IsNullOrWhiteSpace(item)) continue;

                        JsonObject? jsonObject = JsonNode.Parse(item)?.AsObject();
                        if (jsonObject == null) continue;

                        var sectionArray = jsonObject["Section"] as JsonArray;
                        var testWeightArray = jsonObject["TestWeight"] as JsonArray;
                        var asFoundArray = jsonObject["AsFound"] as JsonArray;
                        var asLeftArray = jsonObject["AsLeft"] as JsonArray;

                        var certifiedWeight = new CertifiedWeight
                        {
                            Section = sectionArray?.ElementAtOrDefault(2)?.ToString(),
                            TestWeight = testWeightArray?.ElementAtOrDefault(2)?.ToString(),
                            AsFound = asFoundArray?.ElementAtOrDefault(2)?.ToString(),
                            AsLeft = asLeftArray?.ElementAtOrDefault(2)?.ToString(),
                        };

                        CertifiedWeightsList.Add(certifiedWeight);

                    }

                     List<string> sideToSideTestList = wo.BalanceAndScaleCalibration.TestPointResult
                     .Where(x => x.CalibrationSubTypeId == 7)
                     .Select(x => x.ExtendedObject)
                     .ToList();

                    foreach (var item in sideToSideTestList)
                    {
                        if (string.IsNullOrWhiteSpace(item)) continue;

                        JsonObject? jsonObject = JsonNode.Parse(item)?.AsObject();
                        if (jsonObject == null) continue;

                        var sectionArray = jsonObject["Section"] as JsonArray;
                        var testWeightArray = jsonObject["TestWeight"] as JsonArray;
                        var asFoundArray = jsonObject["AsFound"] as JsonArray;
                        var asLeftArray = jsonObject["AsLeft"] as JsonArray;

                        var sideToSideTest = new SideToSideTest
                        {
                            Section = sectionArray?.ElementAtOrDefault(2)?.ToString(),
                            TestWeight = testWeightArray?.ElementAtOrDefault(2)?.ToString(),
                            AsFound = asFoundArray?.ElementAtOrDefault(2)?.ToString(),
                            AsLeft = asLeftArray?.ElementAtOrDefault(2)?.ToString(),
                        };

                        SideToSideTestsList.Add(sideToSideTest);
                    }

                    List<string> strainTestList = wo.BalanceAndScaleCalibration.TestPointResult
                     .Where(x => x.CalibrationSubTypeId == 9)
                     .Select(x => x.ExtendedObject)
                     .ToList();

                    foreach (var item in strainTestList)
                    {
                        if (string.IsNullOrWhiteSpace(item)) continue;

                        JsonObject? jsonObjectStrain = JsonNode.Parse(item)?.AsObject();
                        if (jsonObjectStrain == null) continue;

                        var nameArray = jsonObjectStrain["Name"] as JsonArray;
                        var valueArray = jsonObjectStrain["Value"] as JsonArray;

                        var strainTest = new StrainTest
                        {
                            Name = nameArray?.ElementAtOrDefault(2)?.ToString(),
                            Value = valueArray?.ElementAtOrDefault(2)?.ToString(),
                        };

                        StrainTestLis.Add(strainTest);
                    }



                    List<string> testValuesList = wo.BalanceAndScaleCalibration.TestPointResult
                        .Where(x => x.CalibrationSubTypeId == 10)
                        .Select(x => x.ExtendedObject)
                        .ToList();

                    foreach (var item in testValuesList)
                    {
                        if (string.IsNullOrWhiteSpace(item)) continue;

                        JsonObject? jsonObject = JsonNode.Parse(item)?.AsObject();
                        if (jsonObject == null) continue;

                        var asFoundArray = jsonObject["AsFound"] as JsonArray;
                        var asLeftArray = jsonObject["AsLeft"] as JsonArray;

                        var testValue = new TestValue
                        {
                            AsFound = asFoundArray?.ElementAtOrDefault(2)?.ToString(),
                            AsLeft = asLeftArray?.ElementAtOrDefault(2)?.ToString(),
                        };

                        TestValuesList.Add(testValue);
                    }

                    string headerTruckScaleAdvanceTech = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == 11).Select(x => x.ExtendedObject).FirstOrDefault();
                    var jsonObjectTruckScaleAdvanceTech = JsonNode.Parse(headerTruckScaleAdvanceTech)?.AsObject();

                    string headerTruckScaleAdvanceLead = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == 12).Select(x => x.ExtendedObject).FirstOrDefault();
                    var jsonObjectTruckScaleAdvanceTLead = JsonNode.Parse(headerTruckScaleAdvanceLead)?.AsObject();
                    
                    string headerTruckScaleAdvanceIndicator = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == 13).Select(x => x.ExtendedObject).FirstOrDefault();
                    var jsonObjectTruckScaleAdvanceIndicator = JsonNode.Parse(headerTruckScaleAdvanceIndicator)?.AsObject();

                    string headerTruckScaleAdvanceTruck= wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == 14).Select(x => x.ExtendedObject).FirstOrDefault();
                    var jsonObjectTruckScaleAdvanceTruck = JsonNode.Parse(headerTruckScaleAdvanceTruck)?.AsObject();

                    string headerTruckScaleAdvanceAccesory = wo.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == 15).Select(x => x.ExtendedObject).FirstOrDefault();
                    var jsonObjectTruckScaleAdvanceAccesory = JsonNode.Parse(headerTruckScaleAdvanceAccesory)?.AsObject();


                    ///
                    var workOrder = wo.WorkOder;
                    var customer = workOrder.Customer;
                   

                    var address1 = customer.Aggregates.FirstOrDefault().Addresses.ToList();

                    Address address = new Address();
                    string techAproved = "";
                    string tecertiied = "";
                    address = address1.FirstOrDefault();
                    if (useraproved != null)
                    {
                        techAproved = useraproved?.Name + " " + useraproved?.LastName;

                        if (useraproved.TechnicianCodes != null && useraproved.TechnicianCodes.Count > 0)
                        {
                            var tecertiiedo = useraproved.TechnicianCodes
                            .Where(tc => address1.Any(addr => addr.StateID == tc.StateID))
                            .FirstOrDefault();

                            if (tecertiiedo != null && tecertiiedo.Certification != null)
                            {
                                //  var tecertiiedo = useraproved.TechnicianCodes.Where(z => z.StateID == address.StateID).FirstOrDefault();
                                address = address1.Where(x => x.StateID == tecertiiedo.StateID).FirstOrDefault();

                                tecertiied = tecertiiedo.Code;
                            }
                        }

                    }

                    ///
                    //
                    string sectiosPoe = wo?.PieceOfEquipment?.TestPointResult?.Select(x => x.ExtendedObject).FirstOrDefault();
                    string numberSections = null;
                    if (!string.IsNullOrWhiteSpace(sectiosPoe))
                    {

                        JsonObject? jsonObject = JsonNode.Parse(sectiosPoe)?.AsObject();
                        if (jsonObject != null)
                        {

                            var sectionArray = jsonObject["NumberSections"] as JsonArray;
                            numberSections = sectionArray?.ElementAtOrDefault(2)?.ToString();
                        }
                    }




                    HeaderTruckScaleAdvance_ = new HeaderTruckScaleAdvance
                    {
                        CalibrationLocation = address1.FirstOrDefault().StreetAddress1,
                        CalibrationCertificate = wo.WorkOrderDetailID.ToString() + "-1",
                        CalibrationDate = wo.CalibrationDate.HasValue ? wo.CalibrationDate.Value.ToString("MM/dd/yyyy") : string.Empty,
                        PlatformManufacturer = jsonObjectTruckScaleAdvanceTech?["PlatformManufacturer"]?[2]?.ToString(),
                        PlatformModel = jsonObjectTruckScaleAdvanceTech?["PlatformModel"]?[2]?.ToString(),
                        PlatformSerial = jsonObjectTruckScaleAdvanceTech?["PlatformSerial"]?[2]?.ToString(),
                        Capacity = jsonObjectTruckScaleAdvanceTech?["Capacity"]?[2]?.ToString(),
                        TypeOfDeck = jsonObjectTruckScaleAdvanceTech?["TypeOfDeck"]?[2]?.ToString(),
                        CLC = jsonObjectTruckScaleAdvanceTruck?["CLC "]?[2]?.ToString(),
                        CalibratedFor = customer?.Name,
                        CustomerID = customer.CustomID,
                        CalibrationDueDate = wo.CalibrationCustomDueDate.HasValue ? wo.CalibrationCustomDueDate.Value.ToString("MM/dd/yyyy") : string.Empty,
                        IndicatorManufacturer = jsonObjectTruckScaleAdvanceIndicator?["Manufacturer"]?[2]?.ToString(),
                        IndicatorModel = jsonObjectTruckScaleAdvanceIndicator?["Model"]?[2]?.ToString(),
                        IndicatorSerial = jsonObjectTruckScaleAdvanceIndicator?["SerialNumber"]?[2]?.ToString(),
                        Division = jsonObjectTruckScaleAdvanceTruck?["Divisions"]?[2]?.ToString(),
                        NumberSections = numberSections,
                        PlatformSize = jsonObjectTruckScaleAdvanceTruck?["PlatformSize"]?[2]?.ToString(),
                        LeadWireSeal = "As Found: " + jsonObjectTruckScaleAdvanceTLead?["AsFound"]?[2]?.ToString() + " / As Left: " + jsonObjectTruckScaleAdvanceTLead?["AsLeft"]?[2]?.ToString(),
                        ScaleDescription = jsonObjectTruckScaleAdvanceTech?["ScaleDescription"]?[2]?.ToString(),
                        ScoreBoard = jsonObjectTruckScaleAdvanceAccesory?["ScoreBoard"]?[2]?.ToString(),
                        LocalRemote = jsonObjectTruckScaleAdvanceAccesory?["LocalRemote"]?[2]?.ToString(),
                        Printer = jsonObjectTruckScaleAdvanceAccesory?["Printer"]?[2]?.ToString(),
                        Other = jsonObjectTruckScaleAdvanceAccesory?["Other"]?[2]?.ToString(),
                        Technician = techreview,
                        TechnicianAprove = tecertiied,

                    };

                    
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            header.SectionsTruckAdvance = SectionsTruckAdvanceList;
            header.CertifiedWeights = CertifiedWeightsList;
            header.SideToSidesTest = SideToSideTestsList;
            header.StrainTests = StrainTestLis;
            header.TestValues = TestValuesList;
            header.HeaderTruckScaleAdvance = HeaderTruckScaleAdvance_;
            return header;
        }


        public List<Linearity> OrderAlgoritm(List<Linearity> Items)
        {
           
            var rr = new List<TestPoint>();
            int cont = 100;
            var aarrl = new List<Linearity>();
            aarrl = Items.Where(x => x.TestPoint.CalibrationType.ToLower() == "linearity" && x.TestPoint.IsDescendant == false)
                .OrderBy(x => x.TestPoint.NominalTestPoit).ToList();

            aarrl.ForEach(item22 =>
            {

                item22.TestPoint.Position = cont;
                cont++;

            });


            var aarr2 = Items.Where(x => x.TestPoint.CalibrationType.ToLower() == "linearity" && x.TestPoint.IsDescendant == true)
                .OrderByDescending(x => x.TestPoint.NominalTestPoit).ToList();

            aarr2.ForEach(item22 =>
            {

                item22.TestPoint.Position = cont;
                cont++;

            });

            foreach (var item in aarr2)
            {
                aarrl.Add(item);
            }


            var arr3 = Items.Where(x => x.TestPoint.CalibrationType.ToLower() == "eccentricity").OrderBy(x => x.TestPoint.NominalTestPoit).OrderBy(x => x.TestPoint.IsDescendant).ToList();
            arr3.ForEach(x =>
            {
                x.TestPoint.Position = cont;
                cont++;
            });

            var arr4 = Items.Where(x => x.TestPoint.CalibrationType.ToLower() == "repeatability").OrderBy(x => x.TestPoint.NominalTestPoit).OrderBy(x => x.TestPoint.IsDescendant).ToList();
            arr4.ForEach(y =>
            {
                y.TestPoint.Position = cont;
                cont++;
            });



            foreach (var it in arr3)
            {
                aarrl.Add(it);
            }

            foreach (var it in arr4)
            {
                aarrl.Add(it);
            }



            return aarrl;

        }
        public Task<IEnumerable<WorkOrderDetail>> GetWorkOrderDetailXWorkOrder(CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder DTO)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WorkOrderDetail>> GetAllEnabled()
        {
            throw new NotImplementedException();
        }

        public Task<Status> SaveStatus(Status sta)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<WorkOrderDetail>> GetWorkOrderDetailXWorkOrder(int ID)
        {

            var result = await Repository.GetWorkOrderDetailByWorkOrderID(ID);

            return result;

        }

        public async Task<WorkOrderDetail> GetWorkOrderDetailByID(WorkOrderDetail DTO,bool IsGeneric=false)
        {
            var workOrderDetail = await Repository.GetWorkOrderDetailByID(DTO.WorkOrderDetailID, IsGeneric);

            return workOrderDetail;

        }

        public void UpdateWorkOrderDetail(WorkOrderDetail workOrderDetail)
        {
            Repository.UpdateWorkOrderDetail(workOrderDetail);
        }


        public async Task<ResultSet<WorkOrderDetail>> GetWods(Pagination<WorkOrderDetail> pag)
        {
            return await Repository.GetWods(pag);
        }

        public async Task<ResultSet<WorkOrderDetail>> GetWodsFromQuery(Pagination<WorkOrderDetail> pag)
        {
            return await Repository.GetWodsFromQuery(pag);
        }

        public async Task<WorkOrderDetail> SaveWod(WorkOrderDetail DTO,string Component, bool offline)
        {
            //try
            //{

            //}
            if (DTO?.BalanceAndScaleCalibration != null && DTO?.BalanceAndScaleCalibration?.Forces?.Count > 0)
            {
                var forces = DTO.BalanceAndScaleCalibration.Forces.ToList();
                var iso = DTO.BalanceAndScaleCalibration.Forces.First().ISO;
                int isISO;
                if (iso)
                {
                    isISO = 1;
                }
                else
                {
                    isISO = 0;
                }

                var resUncertainty = await Poe.CalculateUncertainty(DTO.BalanceAndScaleCalibration.Forces.ToList(), isISO, null);

                ////
                ///Uncertainty CMC
                ///
                CalibrationType calibrationType = new CalibrationType()
                {
                    CalibrationTypeId = (int)DTO.CalibrationTypeID
                };

                calibrationType = await Basics.GetCalibrationTypeByID(calibrationType);
                var forces1 = resUncertainty;
                if (calibrationType != null && calibrationType.CMCValues != null && calibrationType.CMCValues.Count() > 0)
                {
                    foreach (var lin in forces1)
                    {
                        bool replace = false;
                        var nominalValue = lin.BasicCalibrationResult.Nominal;

                        foreach (var item in calibrationType.CMCValues)
                        {

                            if (item.IncludeEqualsMin && item.IncludeEqualsMax && nominalValue >= item.MinRange && nominalValue <= item.MaxRange)
                            {
                                replace = true;
                            }
                            else if (item.IncludeEqualsMin && !item.IncludeEqualsMax && nominalValue >= item.MinRange && nominalValue < item.MaxRange)
                            {
                                replace = true;
                            }
                            else if (!item.IncludeEqualsMin && item.IncludeEqualsMax && nominalValue > item.MinRange && nominalValue <= item.MaxRange)
                            {
                                replace = true;
                            }
                            else if (!item.IncludeEqualsMin && !item.IncludeEqualsMax && nominalValue > item.MinRange && nominalValue < item.MaxRange)
                            {
                                replace = true;
                            }

                            if (replace && lin.Uncertainty < item.CMC)
                            {
                                var uom = lin.UnitOfMeasureId.ToString();
                                lin.BasicCalibrationResult.UncertaintyNew = item.CMC;
                                lin.MinRange = item.MinRange;
                                lin.MaxRange = item.MaxRange;
                                break;
                            }

                        }
                        if (replace == false)
                        {
                            lin.BasicCalibrationResult.UncertaintyNew = null;
                        }
                    }


                }

            }




            if (offline && DTO?.PieceOfEquipment?.EquipmentTemplate != null
               && DTO.PieceOfEquipment?.EquipmentTemplate?.Manufacturer1 != null)
            {
                var man = await Basics.GetManufacturers();

                var res = man.Where(x => x.Name.ToLower() ==
                DTO.PieceOfEquipment.EquipmentTemplate.Manufacturer1.Name.ToLower()).FirstOrDefault();

                if (res == null)
                {
                    res = await Basics.CreateManufacturer(DTO.PieceOfEquipment.EquipmentTemplate.Manufacturer1);
                    DTO.PieceOfEquipment.EquipmentTemplate.ManufacturerID = res.ManufacturerID;
                    DTO.PieceOfEquipment.EquipmentTemplate.Manufacturer1 = null;
                }
            }


            if (offline && DTO?.PieceOfEquipment?.EquipmentTemplate != null)

            {
                var model = DTO.PieceOfEquipment.EquipmentTemplate.Model;
                var etype = DTO.PieceOfEquipment.EquipmentTemplate.EquipmentTypeGroupID;
                Pagination<EquipmentTemplate> pag = new Pagination<EquipmentTemplate>();
                pag.Show = 20;
                pag.Entity = new EquipmentTemplate();
                //pag.Entity.EquipmentTypeGroupID = etype;
                pag.Entity.Model = model;
                //pag.Entity.ManufacturerID = DTO.PieceOfEquipment.EquipmentTemplate.ManufacturerID;

                var man = await Basics.GetValidEquipment(model,etype, DTO.PieceOfEquipment.EquipmentTemplate.ManufacturerID);
                var res = man; // man.List.Where(x => x.Model.ToLower() == model.ToLower() && x.EquipmentTypeGroupID.HasValue && x.EquipmentTypeGroupID==etype && x.ManufacturerID == DTO.PieceOfEquipment.EquipmentTemplate.ManufacturerID).FirstOrDefault();

                if (res == null)
                {
                    res = await Basics.CreateEquipment(DTO.PieceOfEquipment.EquipmentTemplate);
                    if (res != null)
                    {
                        DTO.PieceOfEquipment.EquipmentTemplateId = res.EquipmentTemplateID;
                        DTO.PieceOfEquipment.EquipmentTemplate = null;
                    }
                 
                }


            }

            if (offline && DTO?.PieceOfEquipment != null)

            {
                var model = DTO.PieceOfEquipment.PieceOfEquipmentID;
                //var etype = DTO.PieceOfEquipment ;
                Pagination<PieceOfEquipment> pag = new Pagination<PieceOfEquipment>();
                pag.Show = 3;

                var man = await PieceOfEquipmentRepository.GetPieceOfEquipmentByIDHeader(model);

                var res = man;//man.List.Where(x => x.PieceOfEquipmentID.ToLower() == model.ToLower()).FirstOrDefault();

                if (res == null)
                {
                    res = await PieceOfEquipmentRepository.InsertPieceOfEquipment(DTO.PieceOfEquipment);
                    DTO.PieceOfEquipment.PieceOfEquipmentID = res.PieceOfEquipmentID;
                    DTO.PieceOfEquipment = null;                    
                }
                else
                {
                    res = await PieceOfEquipmentRepository.UpdatePieceOfEquipment(DTO.PieceOfEquipment);
                    DTO.PieceOfEquipment.PieceOfEquipmentID = res.PieceOfEquipmentID;
                    DTO.PieceOfEquipment = null;
                }


                var he = await Repository.GetHeaderById(DTO);

                if (he == null)
                {
                    var dto1 = (WorkOrderDetail)DTO.CloneObject();

                    var result2 = await Repository.ChangeStatus(dto1);
                }

              

            }



            var result = await Repository.ChangeStatus(DTO);


            if (DTO.NotesWOD != null && DTO.NotesWOD.Count > 0)
            {
                foreach (var item2 in DTO.NotesWOD)
                {
                    await Repository.SaveNotes(item2);
                }
            }

            var wo = await Assets.GetWorkOrderByIDHeader(DTO.WorkOrderID,0);


            wo.StatusID = 2;

            await Assets.UpdateWorkOrder(wo);



            await Repository.RemoveNotes<WorkOrderDetail>(DTO.WorkOrderDetailID, DTO); 
           return result;
        }

        public async Task<Certificate> GetCertificate(WorkOrderDetail DTO)
        {
            var result = await Repository.GetCertificate(DTO);

            return result;

            //DTO.PieceOfEquipment.UnitOfMeasure.Abbreviation
        }



        public void Save()
        {
            Repository.Save();
        }

        public async Task<IEnumerable<int>> GetChartPie()
        {
            var result = await Repository.GetChartPie();

            return result;
        }

        public async Task<IEnumerable<int>> GetTotals()
        {
            var result = await Repository.GetTotals();

            return result;
        }

        public async Task<IEnumerable<KeyValueDate>> GetWODCountPerDay()
        {
            var result = await Repository.GetWODCountPerDay();

            return result;
        }

        public async Task<IEnumerable<KeyValueDate>> GetWOCountPerDay()
        {
            var result = await Repository.GetWOCountPerDay();

            return result;
        }


        public async  Task<ResultSet<TestCode>> GetTestCodes(Pagination<TestCode> pagination)
        {
           var result = await Repository.GetTestCodes(pagination);

            return result;
        }

        public async Task<TestCode> CreateTestCode(TestCode item)
        {
            var result = await Repository.CreateTestCode(item);


            if (item.Notes != null && item.Notes.Count > 0)
            {
                foreach (var item2 in item.Notes)
                {
                    await Basics.SaveNotes(item2);
                }



            }

            await Basics.RemoveNotes<TestCode>(item.TestCodeID, item, 2);
            return result;
        }

          public async Task<TestCode> GetTestCodeByID(int item)
        {
            var result = await Repository.GetTestCodeByID(item);

            if(result != null)
            {
                result.Notes = await Basics.GetNotes(result.TestCodeID,2);
            }



            return result;
        }

        public async Task<TestCode> GetTestCodeXName(TestCode DTO)
        {
           
            var result = await this.Repository.GetTestCodeXName(DTO);

            ///
            ////
            if ((result != null && result.TestCodeID == DTO.TestCodeID && result.Code == DTO.Code) || result == null)
            {
                return new TestCode();
            }
            else
            {
                return result;
            }
            //

        }

        public async Task<TestCode> DeleteTestCode(TestCode item)
        {
            var result = await Repository.DeleteTestCode(item);

            return result;
        }

        public async Task<ICollection<CalibrationSubType_Standard>> GetCalibrationSubType_StandardByWodI(int wodid)
        {
            var result = await Repository.GetCalibrationSubType_StandardByWodI(wodid);

            return result;
        }

        

        public async Task<InstrumentThread> GetInstrumentThread(WorkOrderDetail wod)
        {
            bool res = false;
            EquipmentTemplate et = new EquipmentTemplate();
            et.EquipmentTemplateID = (int)wod.PieceOfEquipment.EquipmentTemplate.EquipmentTemplateParent;
            var equipmentTemplateParent = await Basics.GetEquipmentByID(et);
            var testPoints = equipmentTemplateParent.TestPointResult.ToList();
            //var calibrationSubtypeChild= wod.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject.CalibrationType.CalibrationSubTypes.FirstOrDefault();
            //int CalibrationSubTypeIdChild = 0;
            //if (calibrationSubtypeChild != null)
            //CalibrationSubTypeIdChild = calibrationSubtypeChild.CalibrationSubTypeId;

            string type = null;
            string series = null;
            string class_ = null;
            string Gage_Function = null;
            string Pitch_Diam_Size = null;
            string Size_Range = null;
            string Description = null;
            string Calibrate_With_Type = null;
            string NoGo_Pitch_Diam = null;
            string fileName = GetFileName(wod.TestCode.CalibrationType.CalibrationSubTypes.FirstOrDefault().CalibrationSubTypeId);//GetFileName(wod.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject.CalibrationType.CalibrationSubTypes.FirstOrDefault().CalibrationSubTypeId); 
            var typeObject = testPoints.ToList().Where(x => x.CalibrationSubTypeId == 505 && x.KeyObject !=null).FirstOrDefault();
            if (typeObject != null)
            { 
                var type_ = typeObject.ExtendedObject;
                JObject obj = JObject.Parse(type_);

                type = obj["ItemThreadType"][2].ToString();
                // Extraer el primer elemento del array
                
            }
            //series
            var seriesObject = testPoints.ToList().Where(x => x.CalibrationSubTypeId == 506 && x.KeyObject != null).FirstOrDefault();
            if (seriesObject != null)
            {
                var obj_ = seriesObject.ExtendedObject;
                JObject obj = JObject.Parse(obj_);

                series = obj["ItemThreadSeries"][2].ToString();
                // Extraer el primer elemento del array

            }

            //class
            var classObject = testPoints.ToList().Where(x => x.CalibrationSubTypeId == 500 && x.KeyObject != null).FirstOrDefault();
            if (classObject != null)
            {
                var obj_ = classObject.ExtendedObject;
                JObject obj = JObject.Parse(obj_);

                class_ = obj["ItemThreadClass"][2].ToString();
                // Extraer el primer elemento del array

            }

            //Gage_Function
            var Gage_FunctionObject = testPoints.ToList().Where(x => x.CalibrationSubTypeId == 501 && x.KeyObject != null).FirstOrDefault();
            if (Gage_FunctionObject != null)
            {
                var obj_ = Gage_FunctionObject.ExtendedObject;
                JObject obj = JObject.Parse(obj_);

                Gage_Function = obj["ItemThreadGageFunction"][2].ToString();
                // Extraer el primer elemento del array

            }

            //Pitch_Diam_Size
            var Pitch_Diam_SizeObject = testPoints.ToList().Where(x => x.CalibrationSubTypeId == 502 && x.KeyObject != null).FirstOrDefault();
            if (Pitch_Diam_SizeObject != null)
            {
                var obj_ = Pitch_Diam_SizeObject.ExtendedObject;
                JObject obj = JObject.Parse(obj_);

                Pitch_Diam_Size = obj["ItemPitchDiameter"][2].ToString();
                // Extraer el primer elemento del array

            }


            //Size Range
            var Size_RangeObject = testPoints.ToList().Where(x => x.CalibrationSubTypeId == 504).FirstOrDefault();
            if (Size_RangeObject != null)
            {
                var obj_ = Size_RangeObject.ExtendedObject;
                JObject obj = JObject.Parse(obj_);

                Size_Range = obj["Size"][2].ToString();
                // Extraer el primer elemento del array

            }


            //Description
            var DescriptionObject = testPoints.ToList().Where(x => x.CalibrationSubTypeId == 503).FirstOrDefault();
            if (DescriptionObject != null)
            {
                var obj_ = DescriptionObject.ExtendedObject;
                JObject obj = JObject.Parse(obj_);

                Size_Range = obj["Description"][2].ToString();
                // Extraer el primer elemento del array

            }


            InstrumentThread instrument = new InstrumentThread
            { 
                Id ="1",
                InstrumentNumber = wod.PieceOfEquipment.PieceOfEquipmentID.ToString(),
                CustomerNumber = wod.PieceOfEquipment.CustomerId.ToString(),
                DefaultTestCode = wod.PieceOfEquipment.TestCodeID.ToString(),
                Department = null,
                InstrumentType = "",
                InstrumentDescription = "",
                Manufacturer = equipmentTemplateParent.Manufacturer,
                Model = equipmentTemplateParent.Model,
                SizeOrRange = "",
                Accuracy = "0",
                SerialNumber = wod.PieceOfEquipment.SerialNumber,
                CustomerReference = wod.PieceOfEquipment.CustomerToolId,
                CalibrationFrequency = "60",
                CalibrationDate = wod.PieceOfEquipment.CalibrationDate.ToString(),
                CalibrationDueDate = wod.PieceOfEquipment.DueDate.ToString(),
                CalibratedBy = wod.TechnicianID.ToString(),
                ProcedureName = "",
                ProcedureRevision = "",
                ProcedureAddenda = "",
                UncertaintyCalibration = "0",
                UncertaintyKFactor = "0",
                Notes = "",
                LTI_Active = "",
                LTI_InServiceDate = "",
                LTI_RetiredDate = "",
                LTI_Department = "",
                LTI_WorkCenter = "",
                LTI_NIST = "",
                LTI_TYPE = "",
                LTI_SetPlugPitchDiam = "",
                type = type,
                series = series,
                class_ = class_,
                Gage_Function = Gage_Function,
                Pitch_Diam_Size = Pitch_Diam_Size,
                Size_Range = Size_Range,
                Description = Description,
                Calibrate_With_Type = Calibrate_With_Type,
                NoGo_Pitch_Diam = NoGo_Pitch_Diam,
                TestCode = "",
                Cal_With_Instrument_Number = "",
                Cal_With_Cal_Due_Date = "",
                Cal_With_Set_Plug_Pitch_Diameter = "",
                FileName = fileName, 
                WodId = wod.WorkOrderDetailID.ToString(),
                 
            };

            var json = System.Text.Json.JsonSerializer.Serialize(new[] { instrument });
            // URL Power Automate
            var urlFlujo = "https://prod-55.westus.logic.azure.com:443/workflows/2a9e97a766c1466fab675bdd56c3e4c4/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=SLFVQEyUSUF59355UYX47OwaVtl8gd5OzIczMoyXlvE";


            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            //HTTP POST whit json data
            var response = await _httpClient.PostAsync(urlFlujo, content);
          
            //
            if (response.IsSuccessStatusCode)
            {
                
                var responseContent = await response.Content.ReadAsStringAsync();
                instrument.UrlWebFile = responseContent.ToString();
                //save liknweb in WOD_Parameter
                WOD_ParametersTable wodParameter = new WOD_ParametersTable()
                {
                    WorkOrderDetailID = wod.WorkOrderDetailID,
                    urlWebOneDrive = instrument.UrlWebFile
                };

                await Repository.SaveWOD_Parameter(wodParameter);

            }
            else
            {

            }
          
            //
            return instrument;

        }

        public static decimal RoundFirstSignificantDigit(decimal numberInit)
        {
            if (numberInit == 0)
                return 0;


            Int64 integerPart = (Int64)Math.Truncate(numberInit);
            decimal decimalPart = numberInit - integerPart;


            if (decimalPart == 0)
                return numberInit;


            int scale = (int)Math.Floor(Math.Log10((double)Math.Abs(decimalPart))) + 1;
            decimal factor = (decimal)Math.Pow(10, scale - 2);


            decimal roundedDecimalPart = Math.Round(decimalPart / factor, MidpointRounding.AwayFromZero) * factor;


            return integerPart + roundedDecimalPart;
        }


        public async Task<List<GenericCalibrationResult2>> GetResultsTable(WorkOrderDetail wod)
        {
            ResultsTable resultsTable = new ResultsTable();
            int calibrationSubtypeId = wod.TestCode.CalibrationType.CalibrationSubTypes.FirstOrDefault().CalibrationSubTypeId; //wod.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject.CalibrationType.CalibrationSubTypes.FirstOrDefault().CalibrationSubTypeId;
            resultsTable.FileName = GetFileName(calibrationSubtypeId);
           
            resultsTable.WodId = wod.WorkOrderDetailID.ToString();

            List<string> processedData;

            var json = System.Text.Json.JsonSerializer.Serialize(new[] { resultsTable });
            // URL del flujo de Power Automate
            var urlFlujo = "https://prod-03.westus.logic.azure.com:443/workflows/29eb060ff16d42bdafd42ab53f278737/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=1jDsbKelCTEVzvbkOplK45j32bZjl8f1-Yr3j5QaZfg";


            // Crear un contenido HTTP con el JSON
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // Realizar la solicitud HTTP POST con el contenido JSON
            var response = await _httpClient.PostAsync(urlFlujo, content);
            string jsonResultsTable = ""; 
            // Verificar el estado de la respuesta
            if (response.IsSuccessStatusCode)
            {
                // Manejar la respuesta del flujo si es necesario
                var responseContent = await response.Content.ReadAsStringAsync();
                //resultsTable.JsonResults = responseContent.ToString();
                jsonResultsTable = responseContent.ToString();
            }
            else
            {
                // Manejar el caso de una respuesta no exitosa
            }

            ////
            ///
            var jsonData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, object>>>>(jsonResultsTable);
            var body = jsonData["body"];

            processedData = new List<string>();
            
            
            
            int seq = 0;
            var testPointResults = new List<GenericCalibrationResult2>();

            foreach (var item in body)
            {
                seq++;
                
               
                var itemJson = System.Text.Json.JsonSerializer.Serialize(item);
                
                GenericCalibrationResult2 genericCalibrationResult2 = new GenericCalibrationResult2()
                {
                    SequenceID = seq,
                    CalibrationSubTypeId = calibrationSubtypeId,
                    ComponentID = wod.WorkOrderDetailID.ToString(),
                    Position = seq,
                    Object = itemJson,
                    Component = "WorkOrderItem",
                    Updated = 1

                };
                testPointResults.Add(genericCalibrationResult2);
                

                
            }
            wod.TestPointResult = testPointResults;

            await Repository.ChangeStatus(wod, "WorkOrderItem");
           
            return testPointResults;


        }

        public string GetFileName(int CalibrationSubTypeId )
        {
            if ( CalibrationSubTypeId == 507 )
            { return "AcmeThreadRing"; }

            if (CalibrationSubTypeId == 508 )
            { return "NPTL1RingGage"; }

            if (CalibrationSubTypeId == 509)
            { return "StandardThreadRing"; }

            if (CalibrationSubTypeId == 510)
            { return "NPTL1Plug"; }

            if (CalibrationSubTypeId == 511)
            { return "ANPT3StepPlug"; } 

            if (CalibrationSubTypeId == 512)
            { return "DoubleStartWorkPlug"; }

            if (CalibrationSubTypeId == 513)
            { return "AcmeSettingPlug"; }
            
            if (CalibrationSubTypeId == 516)
            { return "ThreadsetPlug"; }

            if (CalibrationSubTypeId == 517)
            { return "ThreadWorkPlug"; }

            if (CalibrationSubTypeId == 518)
            { return "ThreadWorkPlug-STI"; }

            return "";
        }

        public async Task<WOD_ParametersTable> GetWOD_Parameter(WOD_ParametersTable item)
        {
            var result = await Repository.GetWOD_Parameter(item);
            return result;

        }

        //public async Task<WorkOrderDetail> CalculateValuesByID(WorkOrderDetail DTO)
        // {

        //    int linenumber = 0;

        //     WorkOrderDetail workGetById = await Repository.GetByID(DTO);
        //    WorkOrderDetail workGetWorkOrderItem = await Repository.GetWorkOrderDetailByID(DTO.WorkOrderDetailID);

        //    if (workGetWorkOrderItem.BalanceAndScaleCalibration == null)
        //    {
        //        return DTO;
        //    }

        //    try
        //    {
        //        var workOrderDetail = workGetById;//DTO;

        //         workOrderDetail.BalanceAndScaleCalibration= workGetWorkOrderItem.BalanceAndScaleCalibration;//DTO;



        //        double ecceALMax = 0;
        //        double ecceALMin = 0;
        //        double ecceAFMax = 0;
        //        double ecceAFMin = 0;

        //        List<double> ecceAFValues = new List<double>();
        //        List<double> ecceALValues = new List<double>();
        //        List<double> repeAFValues = new List<double>();
        //        List<double> repeALValues = new List<double>();

        //        int ecceCounter = 0;
        //        int repeCounter = 0;
        //        linenumber = 1;


        //        if (workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity?.TestPointResult != null)
        //        {
        //            foreach (BasicCalibrationResult basicCalibrationResult in workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult)
        //            {
        //                ecceAFValues.Add(basicCalibrationResult.AsFound);

        //                ecceALValues.Add(basicCalibrationResult.AsLeft);
        //                if (ecceAFMin == 0)
        //                {
        //                    ecceAFMin = ecceAFValues[ecceCounter];

        //                }
        //                if (ecceALMin == 0)
        //                {
        //                    ecceALMin = ecceALValues[ecceCounter];

        //                }
        //                if (ecceAFValues[ecceCounter] < ecceAFMin && ecceAFValues[ecceCounter] > 0)
        //                {
        //                    ecceAFMin = ecceAFValues[ecceCounter];
        //                }
        //                if (ecceAFValues[ecceCounter] >= ecceAFMax)
        //                {
        //                    ecceAFMax = ecceAFValues[ecceCounter];
        //                }
        //                if (ecceALValues[ecceCounter] < ecceALMin && ecceALValues[ecceCounter] > 0)
        //                {
        //                    ecceALMin = ecceALValues[ecceCounter];
        //                }
        //                if (ecceALValues[ecceCounter] >= ecceALMax)
        //                {
        //                    ecceALMax = ecceALValues[ecceCounter];
        //                }
        //                ecceCounter++;
        //            }



        //        }

        //        if (workOrderDetail?.BalanceAndScaleCalibration?.Repeatability?.TestPointResult != null)
        //        {
        //            foreach (BasicCalibrationResult basicCalibrationResult in workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult)
        //            {
        //                repeAFValues.Add(basicCalibrationResult.AsFound);
        //                repeALValues.Add(basicCalibrationResult.AsLeft);
        //                repeCounter++;
        //            }
        //        }
        //        linenumber = 2;


        //        linenumber = 3;

               

        //        EquipmentTemplate et = workOrderDetail.PieceOfEquipment.EquipmentTemplate;
        //        PieceOfEquipment POE = workOrderDetail.PieceOfEquipment;
        //        var poes = await PieceOfEquipmentRepository.GetAllWeightSets(POE);
                
        //        if (workOrderDetail?.BalanceAndScaleCalibration?.Linearities != null)
        //        {
        //            foreach (Linearity linearity in workOrderDetail.BalanceAndScaleCalibration.Linearities)
        //            {
        //                linearity.BalanceAndScaleCalibration = DTO.BalanceAndScaleCalibration;
        //               await calculateCalibrationResultValues(linearity, et, poes.ToList(), workGetById, workOrderDetail);
        //            }
        //        }

        //        linenumber = 4;
        //        if (workOrderDetail?.BalanceAndScaleCalibration?.Repeatability?.TestPointResult != null
        //              && workOrderDetail?.BalanceAndScaleCalibration?.Repeatability?.TestPointResult?.Count > 0)
        //        {
        //            workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityStdDeviationAsLeft = Stdev(repeALValues.ToArray());
        //            workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityStdDeviationAsFound = Stdev(repeAFValues.ToArray());
        //        }
        //        linenumber = 5;
        //        if (workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity != null
        //                && workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity?.TestPointResult != null
        //                && workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity?.TestPointResult?.Count > 0)
        //        {
        //            linenumber = 51;
        //            workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityVarianceAsLeft = ecceALMax - ecceALMin;
        //            workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityVarianceAsFound = ecceAFMax - ecceAFMin;
        //            workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityMax = ecceALMax;
        //            workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityMin = ecceALMin;
        //            linenumber = 52;

        //            workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityUncertaintyValueUOMID = workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.FirstOrDefault().UnitOfMeasureID;
        //            workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityUncertaintyType = "A";
        //            workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityUncertaintyDistribution = "Rectangular";
        //            workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityUncertaintyDivisor = 1.73;
        //            linenumber = 53;
        //            double eccentricityDeltaFinalUOM = 0;
        //            if (workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity?.TestPointResult?.Count > 0
        //                    && workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity?.TestPointResult?.FirstOrDefault().UnitOfMeasure != null
        //                    && workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.FirstOrDefault().UnitOfMeasure.UncertaintyUnitOfMeasureID.requiresUncertantyConversion(workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.FirstOrDefault().UnitOfMeasure.UnitOfMeasureID))
        //            {
        //                linenumber = 54;
        //                eccentricityDeltaFinalUOM = workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.FirstOrDefault().UnitOfMeasure.ConversionValue.ConvertToUOM(workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityDelta, workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.FirstOrDefault().UnitOfMeasure.UncertaintyUnitOfMeasure);
        //            }
        //            else
        //            {

        //                eccentricityDeltaFinalUOM = workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityDelta;
        //                linenumber = 55;
        //            }
        //            workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityQuotient = eccentricityDeltaFinalUOM / workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityUncertaintyDivisor;
        //            linenumber = 6;

        //            await calculateEccentricityCalibrationResultValues(workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity, et, POE, workOrderDetail);
        //            linenumber = 7;
        //        }




        //        linenumber = 8;

        //        if (workOrderDetail?.BalanceAndScaleCalibration?.Repeatability != null && workOrderDetail?.BalanceAndScaleCalibration?.Repeatability?.TestPointResult != null && workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.FirstOrDefault()?.UnitOfMeasure?.UncertaintyUnitOfMeasure != null)
        //        {
        //            workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityUncertaintyValueUOMID = workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.FirstOrDefault().UnitOfMeasure.UnitOfMeasureID; ;
        //            workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityUncertaintyType = "A";
        //            workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityUncertaintyDistribution = "Normal";
        //            workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityUncertaintyDivisor = 1;

        //            double repeatabilityStdDeviationAsLeftFinalUOM = 0;

        //            if (workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.FirstOrDefault().UnitOfMeasure.UncertaintyUnitOfMeasureID.requiresUncertantyConversion(workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.FirstOrDefault().UnitOfMeasure.UnitOfMeasureID))
        //            {
        //                repeatabilityStdDeviationAsLeftFinalUOM = workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.FirstOrDefault().UnitOfMeasure.ConversionValue.ConvertToUOM(workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityStdDeviationAsLeft, workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.FirstOrDefault().UnitOfMeasure.UncertaintyUnitOfMeasure);
        //            }
        //            else
        //            {
        //                repeatabilityStdDeviationAsLeftFinalUOM = workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityStdDeviationAsLeft;
        //            }
        //            workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityQuotient = repeatabilityStdDeviationAsLeftFinalUOM / workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityUncertaintyDivisor;


        //            await calculateRepeatCalibrationResultValues(workOrderDetail?.BalanceAndScaleCalibration?.Repeatability, et, POE, workOrderDetail);
        //        }

        //        linenumber = 9;

        //        if (workOrderDetail?.BalanceAndScaleCalibration?.Linearities != null && workOrderDetail?.BalanceAndScaleCalibration?.Linearities?.FirstOrDefault()?.BasicCalibrationResult != null)
        //        {
        //            workOrderDetail.BalanceAndScaleCalibration.ResolutionUncertaintyValueUOMID = workOrderDetail.BalanceAndScaleCalibration.Linearities.FirstOrDefault().BasicCalibrationResult.UnitOfMeasureID;
        //        }

        //        linenumber = 10;
        //        if (workOrderDetail?.BalanceAndScaleCalibration != null)
        //        {
        //            workOrderDetail.BalanceAndScaleCalibration.ResolutionUncertaintyType = "B";
        //            workOrderDetail.BalanceAndScaleCalibration.ResolutionUncertaintyDistribution = "Resolution";
        //            workOrderDetail.BalanceAndScaleCalibration.ResolutionUncertaintyDivisor = 3.46;

        //            workOrderDetail.BalanceAndScaleCalibration.ResolutionFormatted = 1 / (Math.Pow(10, 3));
        //        }

        //        linenumber = 11;

        //        if (workOrderDetail?.BalanceAndScaleCalibration?.Linearities != null && workOrderDetail?.BalanceAndScaleCalibration?.Linearities?.FirstOrDefault()?.BasicCalibrationResult != null)
        //            workOrderDetail.BalanceAndScaleCalibration.EnvironmentalUncertaintyValueUOMID = workOrderDetail.BalanceAndScaleCalibration.Linearities.FirstOrDefault().BasicCalibrationResult.UnitOfMeasureID;
        //        workOrderDetail.BalanceAndScaleCalibration.EnvironmentalUncertaintyType = "B";
        //        workOrderDetail.BalanceAndScaleCalibration.EnvironmentalUncertaintyDistribution = "Rectangular";
        //        workOrderDetail.BalanceAndScaleCalibration.EnvironmentalUncertaintyDivisor = 1.73;

        //        workOrderDetail.BalanceAndScaleCalibration.EnvironmentalFactor = 0.004;


        //        linenumber = 12;
        //        //TODO
        //        if(workOrderDetail?.PieceOfEquipment?.EquipmentTemplate?.PieceOfEquipments != null)
        //        {
        //            workOrderDetail.PieceOfEquipment.EquipmentTemplate.PieceOfEquipments = null;
        //        }
        //        if (workOrderDetail?.PieceOfEquipment?.WorOrderDetails != null)
        //        {
        //            workOrderDetail.PieceOfEquipment.WorOrderDetails = null;
        //        }
        //        if (workOrderDetail.WOD_Weights != null)
        //        {
        //            workOrderDetail.WOD_Weights = null;
        //        }
        //        if (workOrderDetail?.BalanceAndScaleCalibration?.WorkOrderDetail != null)
        //        {
        //            workOrderDetail.BalanceAndScaleCalibration.WorkOrderDetail = null;
        //        }
               

        //        var selectedData = new WorkOrderDetail
        //        {
        //            WorkOrderDetailID = workOrderDetail.WorkOrderDetailID,
        //            //PieceOfEquipment = workOrderDetail.PieceOfEquipment
        //            BalanceAndScaleCalibration = workOrderDetail.BalanceAndScaleCalibration,
        //            WOD_Weights = workOrderDetail.WOD_Weights,
        //            PieceOfEquipment = workOrderDetail.PieceOfEquipment,
                   
        //            // Añade otras propiedades necesarias
        //        };


        //        return selectedData;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message + "__ " + linenumber);
        //    }



        //}

        private async Task calculateCalibrationResultValues(Linearity linearity, EquipmentTemplate equipmentTemplate, List<PieceOfEquipment>? poes_, WorkOrderDetail wodGetByID, WorkOrderDetail workOrderDetail)
        {
            BasicCalibrationResult basicCalibrationResult = linearity.BasicCalibrationResult;

            if (basicCalibrationResult == null)
            {
                return;
            }


            UnitOfMeasure testPointUnitOfMeasure = basicCalibrationResult.UnitOfMeasure;


            var calibrationResultWeights = linearity.WeightSets;

            double actualValue = 0;
            double nominalTestPointValue = 0;
            var test = nominalTestPointValue + 5;
            double calibrationUncertaintyValue = 0;
            int? unitOfMeasureId = null;
            int? calibrationUncertaintyValueUnitOfMeasureId = null;
            string calibrationUncertaintyType = "";
            string calibrationUncertaintyDistribution = "";
            double calibrationUncertaintyDivisor = 1;

            double manualNominal = linearity.BasicCalibrationResult.WeightApplied;


            ///Uncertainty Calculate
            ///Linearity li, int seq, WorkOrderDetail wod, List<PieceOfEquipment>? poes
            var uncertainty = await Poe.GetReportUncertaintyBudget(linearity, linearity.SequenceID, wodGetByID, workOrderDetail, poes_, null);  //new Reports.Domain.ReportViewModels.UncertaintyViewModel();//
            double totalUncertainty = uncertainty.Totales.TotalUncerainty; // Math.Sqrt(sumOfSquares);
            double expandedUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(uncertainty.Totales.ExpandedUncertainty)));//totalUncertainty * 2;



            basicCalibrationResult.FinalReadingStandard = nominalTestPointValue;
            linearity.TotalNominal = nominalTestPointValue;
            linearity.TotalActual = actualValue;
            linearity.SumUncertainty = calibrationUncertaintyValue;
            //linearity.Quotient =0 ;
            //linearity.Square = square;
            linearity.SumOfSquares = Convert.ToDouble(uncertainty.Totales.SumOfSquares);
            linearity.TotalUncertainty = totalUncertainty;
            linearity.ExpandedUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(expandedUncertainty))); ;
            linearity.CalibrationUncertaintyType = calibrationUncertaintyType;
            linearity.CalibrationUncertaintyDistribution = calibrationUncertaintyDistribution;
            linearity.CalibrationUncertaintyDivisor = calibrationUncertaintyDivisor;
            linearity.UnitOfMeasureId = unitOfMeasureId;
            linearity.CalibrationUncertaintyValueUnitOfMeasureId = calibrationUncertaintyValueUnitOfMeasureId;
            linearity.BasicCalibrationResult.Uncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(expandedUncertainty)));
                
            double tolerancePercentage = 0;
            double resolutionValue = 0;

            if (linearity.TestPoint != null)
                nominalTestPointValue = linearity.TestPoint.NominalTestPoit;
            double minTolerance = nominalTestPointValue;
            double maxTolerance = nominalTestPointValue;


            if (workOrderDetail.Tolerance.ToleranceTypeID == 0)
            {
                throw new Exception("Tolerance Type Not Found");
            }

            if (workOrderDetail.Ranges == null)
            {
                workOrderDetail.Ranges = new List<RangeTolerance>();
            }


            if (workOrderDetail.Tolerance.ToleranceTypeID == 2)
            {

                tolerancePercentage = workOrderDetail.Tolerance.AccuracyPercentage;
                resolutionValue = workOrderDetail.Resolution;

                var tolerance = workOrderDetail.Ranges.Where(x => x.ToleranceTypeID == 2 && x.MinValue <= nominalTestPointValue && nominalTestPointValue >= x.MaxValue).FirstOrDefault();

                if (tolerance != null)
                {
                    tolerancePercentage = tolerance.Percent;
                }

                var resolution = workOrderDetail.Ranges.Where(x => x.ToleranceTypeID == 1 && x.MinValue <= nominalTestPointValue && nominalTestPointValue >= x.MaxValue).FirstOrDefault();

                if (resolution != null)
                {
                    resolutionValue = resolution.Resolution;
                }

                if ((nominalTestPointValue * ((tolerancePercentage) / 100)) > resolutionValue)
                {
                    minTolerance = nominalTestPointValue - (nominalTestPointValue * ((tolerancePercentage) / 100));
                    maxTolerance = nominalTestPointValue + (nominalTestPointValue * ((tolerancePercentage) / 100));
                }
                else
                {
                    minTolerance = nominalTestPointValue - resolutionValue;
                    maxTolerance = nominalTestPointValue + resolutionValue;
                }

                linearity.MinTolerance = minTolerance;

                linearity.MaxTolerance = maxTolerance;
            }

            if (workOrderDetail.Tolerance.ToleranceTypeID == 4)
            {

                tolerancePercentage = workOrderDetail.Tolerance.AccuracyPercentage;
                resolutionValue = workOrderDetail.Resolution;

                var tolerance = workOrderDetail.Ranges.Where(x => x.ToleranceTypeID == 4 && x.MinValue <= nominalTestPointValue && nominalTestPointValue >= x.MaxValue).FirstOrDefault();

                if (tolerance != null)
                {
                    tolerancePercentage = tolerance.Percent;
                }

                minTolerance = nominalTestPointValue - (nominalTestPointValue * ((tolerancePercentage) / 100));
                maxTolerance = nominalTestPointValue + (nominalTestPointValue * ((tolerancePercentage) / 100));


                linearity.MinTolerance = minTolerance;

                linearity.MaxTolerance = maxTolerance;
            }



            if (workOrderDetail.Tolerance.ToleranceTypeID == 1)
            {
                resolutionValue = workOrderDetail.Resolution;

                var resolution = workOrderDetail.Ranges.Where(x => x.ToleranceTypeID == 1 && x.MinValue <= nominalTestPointValue && nominalTestPointValue >= x.MaxValue).FirstOrDefault();

                if (resolution != null)
                {
                    resolutionValue = resolution.Resolution;
                }

                minTolerance = minTolerance - resolutionValue;
                maxTolerance = maxTolerance + resolutionValue;

                linearity.MinTolerance = minTolerance;

                linearity.MaxTolerance = maxTolerance;
            }


            if (workOrderDetail.Tolerance.ToleranceTypeID == 3)
            {
                double grads = nominalTestPointValue / workOrderDetail.Resolution;
                int toleranceGrads = grads.GetGradsHB44(workOrderDetail.ClassHB44);
                double toleranceValue = (double)toleranceGrads * workOrderDetail.Resolution;
                if (workOrderDetail.IsComercial)
                {
                    if (toleranceValue > workOrderDetail.Resolution)
                    {

                        linearity.MinTolerance = minTolerance - toleranceValue;

                        linearity.MaxTolerance = maxTolerance + toleranceValue;
                    }
                    else
                    {
                        linearity.MinTolerance = minTolerance - workOrderDetail.Resolution;
                        linearity.MaxTolerance = maxTolerance + workOrderDetail.Resolution;
                    }


                    int acceptanceToleranceGrads = toleranceGrads / 2;
                    double toleranceAcceptanceValue = (double)acceptanceToleranceGrads * workOrderDetail.Resolution;

                    if (toleranceAcceptanceValue > workOrderDetail.Resolution)
                    {
                        
                        linearity.MinToleranceAsLeft = minTolerance - toleranceAcceptanceValue;
                        linearity.MaxToleranceAsLeft = maxTolerance + toleranceAcceptanceValue;
                    }
                    else
                    {
                        linearity.MinToleranceAsLeft = minTolerance - workOrderDetail.Resolution;
                        linearity.MaxToleranceAsLeft = maxTolerance + workOrderDetail.Resolution;
                    }
                }
                else
                {
                    if (workOrderDetail.Multiplier == 0)
                    {
                        workOrderDetail.Multiplier = 1;
                    }
                    if ((toleranceValue * workOrderDetail.Multiplier) > workOrderDetail.Resolution)
                    {

                        linearity.MinTolerance = minTolerance - (toleranceValue * workOrderDetail.Multiplier);

                        linearity.MaxTolerance = maxTolerance + (toleranceValue * workOrderDetail.Multiplier);
                    }
                    else
                    {
                        linearity.MinTolerance = minTolerance - workOrderDetail.Resolution;
                        linearity.MaxTolerance = maxTolerance + workOrderDetail.Resolution;
                    }

                    ;
                }
            }

            double asFound = basicCalibrationResult.AsFound;
            if (asFound >= linearity.MinTolerance && asFound <= linearity.MaxTolerance)
            {
                basicCalibrationResult.InToleranceFound = "Pass";
            }
            else
            {
                basicCalibrationResult.InToleranceFound = "Fail";
            }

            double asLeft = basicCalibrationResult.AsLeft;
            if (workOrderDetail.Tolerance.ToleranceTypeID == 3 && workOrderDetail.IsComercial)
            {
                if (asLeft >= linearity.MinToleranceAsLeft && asLeft <= linearity.MaxToleranceAsLeft)
                {
                    basicCalibrationResult.InToleranceLeft = "Pass";
                }
                else
                {
                    basicCalibrationResult.InToleranceLeft = "Fail";
                }
            }
            else
            {
                if (asLeft >= linearity.MinTolerance && asLeft <= linearity.MaxTolerance)
                {
                    basicCalibrationResult.InToleranceLeft = "Pass";
                }
                else
                {
                    basicCalibrationResult.InToleranceLeft = "Fail";
                }
            }

            //TODO
            basicCalibrationResult.Linearity = null;
            linearity.BalanceAndScaleCalibration = null;
            linearity.BasicCalibrationResult.UnitOfMeasure.BasicCalibrationResult = null;

        }

        public double ValidTolerance(double Nominal, int ToleranceTypeID, double Resolution, double AccuracyPercentage, string lownmax)
        {
            //if (run1 == -1)
            //{ 
            //    var message = "In";
            //}

            double nominal = Nominal;

            int toleranceType = ToleranceTypeID;
            double toleranceLow = 0;
            double toleranceMax = 0;


            if (toleranceType == 1)
            {
                toleranceLow = ResolutionTolerance(Resolution, 1, nominal);
                toleranceMax = ResolutionTolerance(Resolution, 2, nominal);
            }
            else if (toleranceType == 2)
            {
                toleranceLow = PercentageResolutionTolerance(AccuracyPercentage, Resolution, 1, nominal);
                toleranceMax = PercentageResolutionTolerance(AccuracyPercentage, Resolution, 2, nominal);
            }
            else if (toleranceType == 4)
            {
                toleranceLow = Percentage(AccuracyPercentage, 1, nominal);
                toleranceMax = Percentage(AccuracyPercentage, 2, nominal);
            }

            if (lownmax == "low")
            {
                return toleranceLow;
            }
            else
            {
                return toleranceMax;
            }


        }

        private double Percentage(double tolerance, int ToleranceRange, double testpoint)
        {
            double Accuracy = tolerance;
            double toleranceLow = 0;

            double TestPoint = testpoint;
            if (ToleranceRange == 1)
                toleranceLow = TestPoint - (TestPoint * Accuracy) / 100;
            else
                toleranceLow = TestPoint + (TestPoint * Accuracy) / 100;




            return toleranceLow;
        }
        private double PercentageResolutionTolerance(double tolerance, double resol, int ToleranceRange, double testpoint)
        {
            double toleranceLow;
            double Accuracy = tolerance;
            double resolution = resol;
            double TestPoint;


            TestPoint = testpoint;


            if (ToleranceRange == 1)
                toleranceLow = TestPoint - (((TestPoint * Accuracy) / 100) + resolution);
            else
                toleranceLow = TestPoint + (((TestPoint * Accuracy) / 100) + resolution);

            return toleranceLow;
        }
        //Resolution
        private double ResolutionTolerance(double tolerance, int ToleranceRange, double testpoint)
        {
            double resolution = tolerance;
            double toleranceLow;


            double TestPoint = testpoint;
            if (ToleranceRange == 1)
                toleranceLow = TestPoint - resolution;
            else
                toleranceLow = TestPoint + resolution;
            return toleranceLow;
        }

        public async Task<IEnumerable<WorkOrderDetail>> GetWorkOrderDetailChildren(int workOrderId)
        {


            var result = await Repository.GetWorkOrderDetailChildren(workOrderId);
            return result;


        }
        public async Task<IEnumerable<ChildrenView>> GetChildrenView(int workOrderId)
        {
            var result = await Repository.GetChildrenView(workOrderId);
            return result;
        }

        public static double CalculateStandardDeviation(List<double> values)
        {
            if (values == null || values.Count == 0)
                return 0;

            double average = values.Average();
            double sumOfSquaresOfDifferences = values.Select(val => (val - average) * (val - average)).Sum();
            double stdDev = Math.Sqrt(sumOfSquaresOfDifferences / values.Count);
            return stdDev;
        }




    }
}
