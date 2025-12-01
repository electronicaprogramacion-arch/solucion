using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Application.UseCases;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Infraestructure.Grpc.Helpers;
using Grpc.Core;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using CalibrationSaaS.Infraestructure.EntityFramework.DataAccess;
namespace CalibrationSaaS.Infraestructure.GrpcServices.Services
{
    public class WorkOrderDetailServices : IWorkOrderDetailServices<CallContext>
    {


        private readonly WorkOrderDetailUseCase Logic;
        private readonly ValidatorHelper modelValidator;
        private readonly ILogger logger;
        private readonly PieceOfEquipmentUseCases Logicpoe;
        private readonly IConfiguration Configuration;
        public System.Net.Http.HttpClient Http { get; set; }

        public WorkOrderDetailServices(WorkOrderDetailUseCase _Logic, PieceOfEquipmentUseCases _Logicpoe, ILogger<WorkOrderDetailServices> _logger, ValidatorHelper _modelValidator, IConfiguration _Configuration)
        {
            Logic = _Logic;
            modelValidator = _modelValidator;
            logger = _logger;
            this.Logicpoe = _Logicpoe;
            Configuration = _Configuration;
        }


        //public WorkOrderDetailServices(IWorkOrderRepository wodRepository)
        //{
        //    this.wodRepository = wodRepository;
        //}


        public async ValueTask<WorkOrderDetailHistoryResultSet> GetHistory(WorkOrderDetail DTO, CallContext context=default)
        {

            var a = await Logic.GetHistory(DTO);

            var res = new WorkOrderDetailHistoryResultSet { WorkOrderDetailsHistory = a.ToList() };
            

            return res;

            List<WorkDetailHistory> list = new List<WorkDetailHistory>();

            WorkDetailHistory h1 = new WorkDetailHistory
            {
                Name = "Ready for Calibration",
                UserName = "Anne Sanders",
                Action = "Work Order Detail Created.",
                Date = DateTime.Now.AddDays(0),
                Description = "sdsdsdsdsd frsrsr"
            };

            list.Add(h1);


            for (int i=0; i< 10; i++) { 

            WorkDetailHistory h = new WorkDetailHistory
            {
                Name = "Ready for Calibration",
                UserName = "Anne Sanders",
                Action = "Work Order Detail Created.",
                Date =  DateTime.Now.AddDays(i) ,
                Description = "sdsdsdsdsd frsrsr"
            };

                list.Add(h);

            }

            var result = list.GroupBy(i => i.Date.ToString("yyyyMMdd"))
            .Select(i => new
            {
            
            Date = DateTime.ParseExact(i.Key, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None),
            Count = i.Count(),
            List=i.ToList()
            });

            Dictionary<Dictionary<string,string>, List<WorkDetailHistory>> dic = new Dictionary<Dictionary<string, string>, List<WorkDetailHistory>>();

           

            //    list.GroupBy(i => i.Date.ToString("yyyyMMdd"))
            //.Select(i => new
            //{
            //    Date = DateTime.ParseExact(i.Key, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None),
            //    Count = i.Count()
            //});
            var r = new WorkOrderDetailHistoryResultSet { WorkOrderDetailsHistory = list };
            return await Task.FromResult(r);
        }


        public async  ValueTask<WorkOrderDetail> Create(WorkOrderDetail DTO, CallContext context = default)
        {
            return DTO;
        }

        public async ValueTask<WorkOrderDetail> Reset(WorkOrderDetail DTO, CallContext context = default)
        {
            var result = await Logic.Delete(DTO,true);


            return Format(DTO); ;
        }

        public async ValueTask<WorkOrderDetail> Delete(WorkOrderDetail DTO, CallContext context = default)
        {
            var result = await Logic.Delete(DTO,false);


            return Format(DTO); ;
        }

        public async  ValueTask<WorkOrderDetailResultSet> GetAll( CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<WorkOrderDetailResultSet> GetByOrder(WorkOrderDetail DTO, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        //YPPP
        public async ValueTask<WorkOrderDetail> ChangeStatus(WorkOrderDetail DTO, CallContext context = default)
        {
            var user = context.ServerCallContext.GetHttpContext().User;

            var aa= user.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault();

            //if(aa== null)
            //{
            //    throw new Exception("Not Valid User");
            //}

            var a=   Format(await Logic.ChangeStatus(DTO,aa?.Value,GetComponent(context)));
           
           return a;


        }

        public async ValueTask<Certificate> CreateCertificate(Certificate DTO, CallContext context = default)
        {
            var result = await Logic.CreateCertificate(DTO);
            return result;
        }


        public async ValueTask<WorkDetailHistory> ChangeStatusComplete(WorkDetailHistory DTO, CallContext context = default)
        {

            var a = await Logic.ChangeStatusComplete(DTO);

            return a;

        

        }


        private WorkOrderDetail Format(WorkOrderDetail a)
        {

            if (a != null && a?.PieceOfEquipment != null)
            {
                a.PieceOfEquipment.WorOrderDetails = null;
            }

            if (a != null && a?.WorkOder != null)
            {
                a.WorkOder.WorkOrderDetails = null;
                a.WorkOder.Customer.WorkOrder = null;

                //if (a != null && a. != null)
                //{
                //    a.WorkOder.Customer. = null;
                //    a.WorkOder.Customer.WorkOrder = null;
                //}
            }


            if (a?.PieceOfEquipment != null)
            {
                a.PieceOfEquipment.EquipmentTemplate.PieceOfEquipments = null;

                //yp a.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject.PieceOfEquipment = null;

                //yp a.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject.EquipmentTemplates = null;
            }





            if (a?.WorkOder != null && a?.WorkOder.UserWorkOrders != null)
            {
                foreach (var item in a.WorkOder.UserWorkOrders)
                {
                    item.WorkOrder = null;

                    item.User.WorkOrderDetails = null;
                    item.User.UserWorkOrders = null;
                    item.User.UserRoles = null;

                }
            }

            if (a?.PieceOfEquipment != null && a?.PieceOfEquipment?.EquipmentTemplate != null)
            {

                if (a?.PieceOfEquipment?.EquipmentTemplate?.TestGroups != null)
                {
                    foreach (var item in a.PieceOfEquipment.EquipmentTemplate.TestGroups)
                    {
                        foreach (var item2 in item.TestPoints)
                        {
                            item2.TestPointGroup = null;

                        }

                    }
                }


            }


            if (a?.TestGroups != null)
            {

                if (a.TestGroups != null)
                {
                    foreach (var item in a.TestGroups)
                    {
                        foreach (var item2 in item.TestPoints)
                        {
                            item2.TestPointGroup = null;

                        }

                    }
                }


            }


            if (a?.TestGroups != null)
            {

                if (a.TestGroups != null)
                {
                    foreach (var item in a.TestGroups)
                    {
                        foreach (var item2 in item.TestPoints)
                        {
                            item2.TestPointGroup = null;

                        }

                    }
                }


            }


            if (a?.BalanceAndScaleCalibration != null)
            {
                a.BalanceAndScaleCalibration.WorkOrderDetail = null;
                if (a.BalanceAndScaleCalibration?.Repeatability?.BalanceAndScaleCalibration != null)
                {
                    a.BalanceAndScaleCalibration.Repeatability.BalanceAndScaleCalibration = null;
                }
                if (a.BalanceAndScaleCalibration?.Eccentricity?.BalanceAndScaleCalibration != null)
                {
                    a.BalanceAndScaleCalibration.Eccentricity.BalanceAndScaleCalibration = null;
                }


                if (a?.BalanceAndScaleCalibration?.Linearities != null)
                {
                    foreach (var item in a.BalanceAndScaleCalibration.Linearities)
                    {
                        item.BalanceAndScaleCalibration = null;
                    }
                }



            }
            if (a?.Technician != null)
            {
                a.Technician.WorkOrderDetails = null;
            }

            if (a?.Certificate != null)
            {
                a.Certificate.WorkOrderDetailSerialized = "";
            }


            if (a?.PieceOfEquipment?.Indicator != null)
            {
                a.PieceOfEquipment.Indicator = null;
            }

            if (a?.PieceOfEquipment != null)
            {
                a.PieceOfEquipment.POE_POE = null;
            }


            a.WorkDetailHistorys = null;

            return a;
        }


        public async ValueTask<WorkOrderDetail> SaveWod(WorkOrderDetail DTO, CallContext context)
        {
            var al = await Logic.SaveWod(DTO,GetComponent(context),false);

            var a = await Logic.GetByID(DTO);

            a = Format(a);

            return a;
        }


        public async ValueTask<ICollection<WorkOrderDetail>> GetByTechnician(User DTO, CallContext context = default)
        {
            var a = await Logic.GetByTechnician(DTO);

            return a;
        }


        public async ValueTask<ResultSet<WorkOrderDetail>> GetByTechnicianPag(Pagination<WorkOrderDetail> pagination, CallContext context = default)
        {
            var a = await Logic.GetByTechnicianPag(pagination);

            return a;
        }


        public async ValueTask<WorkOrderDetail> GetByID(WorkOrderDetail DTO, CallContext context = default)
           {


            using (var a = await Logic.GetByID(DTO))
            {
                if (a != null)
                {
                    var b = Format(a);
                    //a.dispose = true;

                    return b;
                }
                else
                {

                    return DTO;
                }
            }

              

        }
        //9503
        public async ValueTask<WorkOrderDetail> GetByIDPreviousCalibration(WorkOrderDetail DTO, CallContext context = default)
        {


            using (var a = await Logic.GetByIDPreviousCalibration(DTO))
            {
                if (a != null)
                {
                    var b = Format(a);
                    //a.dispose = true;

                    return b;
                }
                else
                {

                    return DTO;
                }
            }
        }

      public async ValueTask<WorkOrderDetailResultSet> GetWorkOrderDetailXWorkOrder(WorkOrder DTO, CallContext context = default)
        {

            var result = await Logic.GetWorkOrderDetailXWorkOrder(DTO.WorkOrderId);

            return new WorkOrderDetailResultSet { WorkOrderDetails = result.ToList()};

        }

       

        private void calculateCalibrationResultValues(Linearity linearity)
        {
            BasicCalibrationResult basicCalibrationResult = linearity.BasicCalibrationResult;

            var pieceOfEquipment = new PieceOfEquipment();

            var equipmentTemplate = new EquipmentTemplate();

            UnitOfMeasure testPointUnitOfMeasure = basicCalibrationResult.UnitOfMeasure;

            //Get the weights
            var calibrationResultWeights = linearity.WeightSets;

            double actualValue = 0;
            double nominalValue = 0;
            double calibrationUncertaintyValue = 0;
            int unitOfMeasureId = 0;
            int calibrationUncertaintyValueUnitOfMeasureId = 0;
            String calibrationUncertaintyType = "";
            String calibrationUncertaintyDistribution = "";
            double calibrationUncertaintyDivisor = 1;

            if (calibrationResultWeights != null && calibrationResultWeights.Count > 0)
            {
                foreach (var calibrationResultWeight in calibrationResultWeights)
                {

                    if (basicCalibrationResult.UnitOfMeasure != null && calibrationResultWeight.UnitOfMeasureID != basicCalibrationResult.UnitOfMeasureID)
                    {
                        actualValue += calibrationResultWeight.UnitOfMeasure.ConvertToUOM(calibrationResultWeight.WeightActualValue, basicCalibrationResult.UnitOfMeasure);
                    }
                    else
                    {
                        actualValue += calibrationResultWeight.WeightActualValue;
                    }

                    if (basicCalibrationResult.UnitOfMeasure != null && calibrationResultWeight.UnitOfMeasureID != basicCalibrationResult.UnitOfMeasureID)
                    {
                        nominalValue += calibrationResultWeight.UnitOfMeasure.ConvertToUOM(calibrationResultWeight.WeightNominalValue, basicCalibrationResult.UnitOfMeasure);
                    }
                    else
                    {
                        nominalValue += calibrationResultWeight.WeightNominalValue;
                    }


                    if (testPointUnitOfMeasure != null && testPointUnitOfMeasure.UncertaintyUnitOfMeasure != null)
                    {
                        if (calibrationResultWeight.UncertaintyUnitOfMeasure != null && calibrationResultWeight.UncertaintyUnitOfMeasureId != testPointUnitOfMeasure.UncertaintyUnitOfMeasureID)
                        {
                            calibrationUncertaintyValue += calibrationResultWeight.UncertaintyUnitOfMeasure.ConvertToUOM(calibrationResultWeight.CalibrationUncertValue, testPointUnitOfMeasure.UncertaintyUnitOfMeasure);
                            //Refresh the Calculated uncertainty value for that POEW, requires to convert to the actual common UOM
                            calibrationResultWeight.CalibrationUncertValue = calibrationResultWeight.UncertaintyUnitOfMeasure.ConvertToUOM(calibrationResultWeight.CalibrationUncertValue, testPointUnitOfMeasure.UncertaintyUnitOfMeasure);
                            calibrationResultWeight.UncertaintyUnitOfMeasureId = testPointUnitOfMeasure.UncertaintyUnitOfMeasureID;
                            calibrationResultWeight.UncertaintyUnitOfMeasure = testPointUnitOfMeasure.UncertaintyUnitOfMeasure;
                        }
                        else
                        {
                            calibrationUncertaintyValue += calibrationResultWeight.CalibrationUncertValue;
                            //Refresh the Calculated uncertainty value for that POEW, no changes from the uncertainty value
                            //calibrationResultWeight.set("calibrationUncertaintyResultValue", calibrationResultWeight.getDouble("calibrationUncertaintyValue"));
                            //calibrationResultWeight.set("calibrationUncertaintyResultValueUnitOfMeasureId", testPointUnitOfMeasure.getString("uncertaintyUnitOfMeasureId"));
                        }
                    }
                    else
                    {
                        //No uncertainty unit destination, converting to the same as the Testpoint                        
                        if (testPointUnitOfMeasure != null)
                        {
                            calibrationUncertaintyValue += calibrationResultWeight.UncertaintyUnitOfMeasure.ConvertToUOM(calibrationResultWeight.CalibrationUncertValue, testPointUnitOfMeasure);
                            calibrationResultWeight.CalibrationUncertValue = calibrationResultWeight.UncertaintyUnitOfMeasure.ConvertToUOM(calibrationResultWeight.CalibrationUncertValue, testPointUnitOfMeasure);
                            calibrationResultWeight.UncertaintyUnitOfMeasureId = testPointUnitOfMeasure.UnitOfMeasureID;
                            calibrationResultWeight.UncertaintyUnitOfMeasure = testPointUnitOfMeasure;
                        }
                        else
                        {
                            calibrationUncertaintyValue += calibrationResultWeight.CalibrationUncertValue;
                            //calibrationResultWeight.set("calibrationUncertaintyResultValue", calibrationResultWeight.getDouble("calibrationUncertaintyValue"));
                            //calibrationResultWeight.set("calibrationUncertaintyResultValueUnitOfMeasureId", calibrationResultWeight.getString("calibrationUncertaintyValueUnitOfMeasureId"));
                        }
                    }
                    //It is saved by EF
                    //delegator.store(calibrationResultWeight);

                    calibrationUncertaintyType = calibrationResultWeight.Type;

                    calibrationUncertaintyDistribution = calibrationResultWeight.Distribution;

                    calibrationUncertaintyDivisor = calibrationResultWeight.Divisor;

                    unitOfMeasureId = basicCalibrationResult.UnitOfMeasureID;

                    if (testPointUnitOfMeasure != null && testPointUnitOfMeasure.UncertaintyUnitOfMeasure != null)
                    {
                        calibrationUncertaintyValueUnitOfMeasureId = testPointUnitOfMeasure.UncertaintyUnitOfMeasureID.Value;
                    }
                    else
                    {
                        calibrationUncertaintyValueUnitOfMeasureId = basicCalibrationResult.UnitOfMeasure.UnitOfMeasureID;
                    }
                }
            }

            //Calculates Uncertainty:

            double quotient = calibrationUncertaintyValue / calibrationUncertaintyDivisor;
            double square = quotient * quotient;

            //Calculates the sum of squares
            double eccentricitySquare = 0;
            eccentricitySquare = linearity.BalanceAndScaleCalibration.Eccentricity.EccentricitySquare;


            double repeatabilitySquare = 0;
            repeatabilitySquare = linearity.BalanceAndScaleCalibration.Repeatability.RepeatabilitySquare;


            double resolutionSquare = 0;
            resolutionSquare = linearity.BalanceAndScaleCalibration.ResolutionSquare;


            double environmentalSquare = 0;
            environmentalSquare = linearity.BalanceAndScaleCalibration.EnvironmentalSquare;

            double sumOfSquares = square + eccentricitySquare + repeatabilitySquare + resolutionSquare + environmentalSquare;

            double totalUncertainty = Math.Sqrt(sumOfSquares);
            double expandedUncertainty = totalUncertainty * 2;

            //Debug.logInfo("Expanded Uncertainty: " + expandedUncertainty, module);

            basicCalibrationResult.FinalReadingStandard = nominalValue;
            linearity.TotalNominal = nominalValue;
            linearity.TotalActual = actualValue;
            linearity.SumUncertainty = calibrationUncertaintyValue;
            linearity.Quotient = quotient;
            linearity.Square = square;
            linearity.SumOfSquares = sumOfSquares;
            linearity.TotalUncertainty = totalUncertainty;
            linearity.ExpandedUncertainty = expandedUncertainty;
            linearity.CalibrationUncertaintyType = calibrationUncertaintyType;
            linearity.CalibrationUncertaintyDistribution = calibrationUncertaintyDistribution;
            linearity.CalibrationUncertaintyDivisor = calibrationUncertaintyDivisor;
            linearity.UnitOfMeasureId = unitOfMeasureId;
            linearity.CalibrationUncertaintyValueUnitOfMeasureId = calibrationUncertaintyValueUnitOfMeasureId;

            double tolerancePercentage = 0;
            double resolutionValue = 0;
            double minTolerance = nominalValue;
            double maxTolerance = nominalValue;
            //Percentage or Percentage + Resolution

            //TODO Define Tolerance Types
            if (equipmentTemplate.Tolerance.ToleranceTypeID == 1 || equipmentTemplate.Tolerance.ToleranceTypeID == 2)
            {
                tolerancePercentage = equipmentTemplate.Tolerance.AccuracyPercentage;
                minTolerance = nominalValue - (nominalValue * ((tolerancePercentage) / 100));
                maxTolerance = nominalValue + (nominalValue * ((tolerancePercentage) / 100));
            }

            if (equipmentTemplate.Tolerance.ToleranceTypeID == 2 || equipmentTemplate.Tolerance.ToleranceTypeID == 3)
            {
                /* TODO: Compound Resolution
                if (equipmentTemplate.getString("compoundResolution") != null && equipmentTemplate.getString("compoundResolution").equals("Y"))
                {
                    //Multiple Tolerances per range
                    List<EntityExpr> cmcRangesExprs1 = FastList.newInstance();
                    cmcRangesExprs1.add(EntityCondition.makeCondition("lowValue", EntityOperator.LESS_THAN_EQUAL_TO, nominalValue));
                    cmcRangesExprs1.add(EntityCondition.makeCondition("highValue", EntityOperator.GREATER_THAN_EQUAL_TO, nominalValue));
                    cmcRangesExprs1.add(EntityCondition.makeCondition("equipmentTemplateId", EntityOperator.EQUALS, equipmentTemplate.getString("equipmentTemplateId")));
                    //TODO: Pending conversion
                    //cmcRangesExprs1.add(EntityCondition.makeCondition("unitOfMeasureId", EntityOperator.EQUALS, unitOfMeasureTestPointId));

                    EntityConditionList<EntityExpr> cmcRangesAll = EntityCondition.makeCondition(cmcRangesExprs1, EntityOperator.AND);

                    List<GenericValue> equipmentTemplateTolerances = delegator.findList("EquipmentTemplateTolerance", cmcRangesAll, null, null, null, false);

                    if (equipmentTemplateTolerances != null && equipmentTemplateTolerances.size() > 0)
                    {
                        for (GenericValue equipmentTemplateTolerance : equipmentTemplateTolerances)
                        {
                            //Calculates High Value
                            resolutionValue = equipmentTemplateTolerance.getDouble("toleranceValue");
                        }
                    }
                    else
                    {
                        if (equipmentTemplate.getDouble("resolution") != null)
                        {
                            resolutionValue = equipmentTemplate.getDouble("resolution");
                        }
                    }
                }
                else
                {*/
                resolutionValue = equipmentTemplate.Resolution;
                //}
                minTolerance = minTolerance - resolutionValue;
                maxTolerance = maxTolerance + resolutionValue;
            }


            linearity.MinTolerance = minTolerance;
            //Calculates Lower Value
            linearity.MaxTolerance = maxTolerance;


            double asFound = basicCalibrationResult.AsFound;
            if (asFound >= minTolerance && asFound <= maxTolerance)
            {
                basicCalibrationResult.InToleranceFound = "Pass";
            }
            else
            {
                basicCalibrationResult.InToleranceFound = "Fail";
            }



            double asLeft = basicCalibrationResult.AsLeft;
            if (asLeft >= minTolerance && asLeft <= maxTolerance)
            {
                basicCalibrationResult.InToleranceLeft = "Pass";
            }
            else
            {
                basicCalibrationResult.InToleranceLeft = "Fail";
            }

            //Value stored complete in repository
            //delegator.store(basicCalibrationResult);
        }

        private static double Variance(double[] list)
        {
            double sum = 0.0;
            double num = 0.0;
            foreach (double i in list)
            {
                sum += i;
            }
            double mean = sum / list.Length;
            foreach (double i in list)
            {
                double numi = Math.Pow((double)i - mean, 2);
                num += numi;
            }
            return (num / list.Length);
        }

        private static double Stdev(double[] list)
        {
            double sum = 0.0;
            double num = 0.0;
            foreach (double i in list)
            {
                sum += i;
            }
            double mean = sum / list.Length;
            foreach (double i in list)
            {
                double numi = Math.Pow((double)i - mean, 2);
                num += numi;
            }
            return Math.Sqrt(num / list.Length);
        }

        public ValueTask<WorkOrderDetailResultSet> GetAllEnabled(CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<IEnumerable<Domain.Aggregates.Entities.Status>> GetStatus(CallContext context)
        {
            return await Logic.GetStatus();
        }

        public async ValueTask<WorkOrderDetail> GetWorkOrderDetailByID(WorkOrderDetail DTO, CallContext context)
        {
            var a= await Logic.GetWorkOrderDetailByID(DTO);

            if(a != null)
            {
                //a.BalanceAndScaleCalibration.
            }

            return Format(a);
        }

       

        //public async ValueTask<string> GetWorkOrderDetailXIdRep(WorkOrderDetail wo)
        //{
        //    //var workOrderDetail = await LogicSample.GetWorkOrderDetailXIdRep(wo.WorkOrderDetailID);
        //    //wo.WorkOrderDetailID = 2;

        //    var workOrderDetail1 = await Logic.GetByID(wo);
        //    var workOrderDetail = await Logic.GetWorkOrderDetailByID(wo);

        //    var workOrder = workOrderDetail1.WorkOder;
        //    var customer = workOrder.Customer;
        //    //var address = customer.Aggregates[0];
        //    var poe = workOrderDetail1.PieceOfEquipment;  // customer.PieceOfEquipment.Where(x => x.WorOrderDetailID == wo.WorkOrderDetailID).FirstOrDefault();
        //    var poes = await Logicpoe.GetAllWeightSets(poe);
        //    var eqTemp = poe.EquipmentTemplate;
        //    var weigthSets = poes.FirstOrDefault().WeightSets;
        //    //var manu = eqTemp.Manufacturer;
        //    string manufName;
        //    if (eqTemp.Manufacturer1 != null)
        //    {
        //        manufName = eqTemp.Manufacturer1.Name;
        //    }
        //    else
        //    {
        //        manufName = null;
        //    }

        //    var _linearity = workOrderDetail.BalanceAndScaleCalibration.Linearities;


        //    var _repeatability = workOrderDetail.BalanceAndScaleCalibration.Repeatability.BasicCalibrationResult.Where(x => x.CalibrationSubTypeId == 2);
        //    var _eccentricity = workOrderDetail.BalanceAndScaleCalibration.Eccentricity.BasicCalibrationResult.Where(x => x.CalibrationSubTypeId == 3);


        //    var asLeftL = _linearity.Where(x => x.BasicCalibrationResult.InToleranceLeft == "PASS".ToUpper());
        //    var asFoundL = _linearity.Where(x => x.BasicCalibrationResult.InToleranceFound == "PASS".ToUpper());
        //    string _AsLeftResult;
        //    string _AsFoundResult;

        //    if (asFoundL.Count() == _linearity.Count())
        //    {
        //        _AsFoundResult = "PASS";
        //    }
        //    else
        //    {
        //        _AsFoundResult = "FAIL";
        //    }

        //    if (asLeftL.Count() == _linearity.Count())
        //    {
        //        _AsLeftResult = "PASS";
        //    }
        //    else
        //    {
        //        _AsLeftResult = "FAIL";
        //    }
        //    var address = customer.Aggregates.FirstOrDefault().Addresses.FirstOrDefault();
        //    Reports.Domain.ReportViewModels.Header header = new Reports.Domain.ReportViewModels.Header()
        //    {
        //        Client = customer.Name,
        //        Address = address.ZipCode + ' ' + address.City + ' ' + address.County,
        //        Country = customer.Aggregates.FirstOrDefault().Addresses.FirstOrDefault().County,
        //        EquipmentLocation = poe.InstallLocation,
        //        EquipmentType = eqTemp.EquipmentType,
        //        NextCalDate = workOrderDetail.CalibrationNextDueDate.Value.ToString("MM/dd/yyyy"), //OJOO no está mapeado
        //        LastCalDate = workOrderDetail.CalibrationDate.Value.ToString("MM/dd/yyyy"), //OJOO no está mapeado
        //        ManufacturerInd = null, //OJOO no está mapeado
        //        ModelInd = null, //OJOO no está mapeado
        //        SerialInd = null, //OJOO no está mapeado
        //        CapInd = "Test",
        //        ManufacturerReceiv = manufName,
        //        ModelIndReceiv = eqTemp.Model,
        //        SerialIndReceiv = poe.SerialNumber,
        //        CapIndReceiv = null, //OJOO no está mapeado
        //        Class = poe.Class,
        //        Type = null, //OJOO no está mapeado
        //        PlatformSize = null, //OJOO no está mapeado
        //        ServiceLocation = null, //OJOO no está mapeado
        //        UnitNumber = null, //OJOO no está mapeado
        //        TestingMethod = null, //OJOO no está mapeado
        //        CalID = null, //OJOO no está mapeado
        //        Location = null, //OJOO no está mapeado
        //        AsLeftResult = _AsLeftResult, //OJOO no está mapeado
        //        AsFoundResult = _AsFoundResult, //OJOO no está mapeado
        //        CalibrtionDate = workOrderDetail.CalibrationDate.Value.ToString("MM/dd/yyyy"),
        //        Temperature = workOrderDetail.Temperature.ToString() + "/" + workOrderDetail.Humidity.ToString(),
        //        Enviroment = workOrderDetail.Environment

        //    };



        //    #region Data CalCertBlank
        //    List< Reports.Domain.ReportViewModels.AsFound > AsFoundList = new List<Reports.Domain.ReportViewModels.AsFound >();

        //    foreach (var af in _linearity)
        //    {
        //        var AsFoundLin = new Reports.Domain.ReportViewModels.AsFound
        //        {
        //            Standard = af.TestPoint.NominalTestPoit.ToString(),
        //            Tolerance = af.MinTolerance + "-" + af.MaxTolerance, //af.TestPoint.LowerTolerance,
        //            PassFail = af.BasicCalibrationResult.InToleranceFound,
        //            //Description = af.TestPoint.Description, 
        //            Indication = af.TestPoint.Description,
        //            Uncertainty = af.BasicCalibrationResult.AsFound.ToString(),//af.BasicCalibrationResult.Uncertainty,
        //            Range = 0 //OJOO no está mapeado
        //        };
        //        AsFoundList.Add(AsFoundLin);
        //    }

        //    List< Reports.Domain.ReportViewModels.AsLeft > AsLeftList = new List<Reports.Domain.ReportViewModels.AsLeft >();

        //    foreach (var af in _linearity)
        //    {
        //        var AsLeftLin = new Reports.Domain.ReportViewModels.AsLeft
        //        {
        //            Standard = af.TestPoint.NominalTestPoit.ToString(),
        //            Tolerance = af.MinTolerance + "-" + af.MaxTolerance,
        //            PassFail = af.BasicCalibrationResult.InToleranceLeft,
        //            //Description = af.TestPoint.Description, 
        //            Indication = af.TestPoint.Description,
        //            Uncertainty = af.BasicCalibrationResult.AsLeft.ToString(),//af.BasicCalibrationResult.Uncertainty,
        //            Range = 0  //OJOO no está mapeado
        //        };
        //        AsLeftList.Add(AsLeftLin);
        //    }

        //    List<Reports.Domain.ReportViewModels.Excentricity> eccList = new List<Reports.Domain.ReportViewModels.Excentricity>();

        //    foreach (var ex in _eccentricity)
        //    {
        //        //var um = (await LogicUOM.GetByID(af.TestPoint.UnitOfMeasurement)).Name;
        //        var ecc = new Reports.Domain.ReportViewModels.Excentricity
        //        {
        //            AsFound = ex.AsFound.ToString(),
        //            AsLeft = ex.AsLeft.ToString(), 
        //            Position = ex.Position
        //        };
        //        eccList.Add(ecc);
        //    };

        //    List<Reports.Domain.ReportViewModels.Repeteab> repetList = new List<Reports.Domain.ReportViewModels.Repeteab>();
        //    foreach (var re in _repeatability)
        //    {
        //        var rep = new Reports.Domain.ReportViewModels.Repeteab
        //        {
        //            AsFound = re.AsFound.ToString(),
        //            AsLeft = re.AsLeft.ToString(),
        //            Position = re.Position,
        //            Standard = "0", //OJOO no está mapeado
        //        };
        //        repetList.Add(rep);
        //    };



        //    List<Reports.Domain.ReportViewModels.ExcentricityDet> excentriDetList = new List<Reports.Domain.ReportViewModels.ExcentricityDet>();

        //    foreach (var re in weigthSets)
        //    {
        //        var exDet = new Reports.Domain.ReportViewModels.ExcentricityDet
        //        {
        //            Standard = re.WeightNominalValue.ToString(),
        //            Certificate = "TEST", //OJOO no está mapeado
        //            Description = re.Description, //OJOO no está mapeado
        //            CalDue = poe.CalibrationDate.ToString("MM/dd/YYYY"), //OJOO no está mapeado
        //        };
        //        excentriDetList.Add(exDet);
        //    };

        //    #endregion

        //    var jsonRepeatability = JsonConvert.SerializeObject(repetList);
        //    //var jsonPointDecNC = JsonConvert.SerializeObject(_pdNC);
        //    var jsonEccentricity = JsonConvert.SerializeObject(eccList);
        //    var jsonEccentricityDet = JsonConvert.SerializeObject(excentriDetList);
        //    var jsonAsFound = JsonConvert.SerializeObject(AsFoundList);
        //    var jsonAsLeft = JsonConvert.SerializeObject(AsLeftList);

        //    header.Eccentricity = jsonEccentricity;
        //    header.EccentricityDet = jsonEccentricityDet;
        //    header.AsFound = jsonAsFound;
        //    header.AsLeft = jsonAsLeft;
        //    header.Repeteability = jsonRepeatability;

        //    var jsonHeader = JsonConvert.SerializeObject(header);


        //    //Handle TLS protocols
        //    System.Net.ServicePointManager.SecurityProtocol =
        //        System.Net.SecurityProtocolType.Tls
        //        | System.Net.SecurityProtocolType.Tls11
        //        | System.Net.SecurityProtocolType.Tls12;

        //    System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };

        //    //var content = new StringContent(jsonCustomerReport, System.Text.Encoding.UTF8, "application/json");
        //    //  var content = new StringContent(jsonRepeatability, System.Text.Encoding.UTF8, "application/json");
        //    //var contentPNC = new StringContent(jsonPointDecNC, System.Text.Encoding.UTF8, "application/json");
        //    var contentHeader = new StringContent(jsonHeader, System.Text.Encoding.UTF8, "application/json");
        //    //  Console.WriteLine("contentPNC " + contentPNC);
        //    // Http.BaseAddress = new System.Uri("https://calsaasreport.azurewebsites.net");

        //    Http = new HttpClient();

        //    //Http.BaseAddress = new System.Uri("https://calsaasreport-staging.azurewebsites.net/"); 

        //    Http.BaseAddress = new System.Uri(Configuration.GetSection("Reports")["Url"]);
        //    //  Console.WriteLine("URL grpc " + Http.BaseAddress);
        //    Http.DefaultRequestHeaders.Accept.Clear();
        //    Http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //    HttpResponseMessage pdf = null;
        //    string psdInBase64 = "";
        //    try
        //    {
        //        // pdf = await Http.PostAsync("/GetCustomerCover", content);
        //        pdf = await Http.PostAsync("/GetPointDecresingNC", contentHeader);
        //        var contentReponse = await pdf.Content.ReadAsStringAsync();

        //        Console.WriteLine("contentReponse " + contentReponse);

        //        psdInBase64 = JsonConvert.DeserializeObject(contentReponse).ToString();
        //        //b64 = psdInBase64;

        //        Console.WriteLine("psdInBase  " + psdInBase64);
        //        byte[] bytes = Convert.FromBase64String(psdInBase64);

        //        UploadCertificate(header.SerialIndReceiv, bytes);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.InnerException);
        //    }



        //    return psdInBase64;
        //}

        public void UploadCertificate(string serial, Byte[] pdf)
        {
            var serialHash = GetSHA1Has(serial);

            var path = AppDomain.CurrentDomain.BaseDirectory + @"\Certificate\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (File.Exists(path + serialHash + ".pdf"))
            {
                File.Delete(path + serialHash + ".pdf");
            }
            System.IO.FileStream stream = new FileStream(path + serialHash + ".pdf", FileMode.CreateNew);
            System.IO.BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(pdf, 0, pdf.Length);
            writer.Close();

            String strorageconn = ConfigurationExtensions.GetConnectionString(this.Configuration, "fileShareConnectionString");
            CloudStorageAccount storageacc = CloudStorageAccount.Parse(strorageconn);

            //Create Reference to Azure Blob
            CloudBlobClient blobClient = storageacc.CreateCloudBlobClient();

            //The next 2 lines create if not exists a container named "democontainer"
            CloudBlobContainer container = blobClient.GetContainerReference("certificates");

            container.CreateIfNotExists();

            //CloudBlockBlob blockBlob = container.GetBlockBlobReference("DemoBlob");
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(serialHash + ".pdf");

            //**************************************************************************



            using (var filestream = System.IO.File.OpenRead(path + serialHash + ".pdf"))
            //System.IO.File.OpenRead(@"C :\Azure Storage Demo\test.txt"))
            {

                blockBlob.UploadFromStream(filestream);

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

        public async ValueTask<WorkOrderDetail> GetConfiguredWeights(WorkOrderDetail DTO, CallContext context)
        {
            return await Logic.GetConfiguredWeights(DTO);
        }

        public async ValueTask<Certificate> GetCertificate(WorkOrderDetail DTO, CallContext context)
        {
            var result = await Logic.GetCertificate(DTO);
            return result;
        }

        public async Task<IEnumerable<int>> GetChartPie(CallContext context)
        {
            var result = await Logic.GetChartPie();
            return result;
        }

        public async Task<IEnumerable<int>> GetTotals(CallContext context)
        {
            var result = await Logic.GetTotals();
            return result;
        }

        public async Task<IEnumerable<KeyValueDate>> GetWODCountPerDay(CallContext context)
        {
            var result = await Logic.GetWODCountPerDay();
            return result;
        }

        public async Task<IEnumerable<KeyValueDate>> GetWOCountPerDay(CallContext context)
        {
            var result = await Logic.GetWOCountPerDay();
            return result;
        }

        public async ValueTask<ResultSet<TestCode>> GetTestCodes(Pagination<TestCode> pagination, CallContext context)
        {
            var result = await Logic.GetTestCodes(pagination);
            return result;
        }

        public async ValueTask<TestCode> CreateTestCode(TestCode item, CallContext context)
        {
            var result = await Logic.CreateTestCode(item);
            return result;
        }

        public async ValueTask<TestCode> DeleteTestCode(TestCode item, CallContext context)
        {
            var result = await Logic.DeleteTestCode(item);
            return result;
        }

        public async ValueTask<TestCode> GetTestCodeByID(TestCode item, CallContext context)
        {
            var result = await Logic.GetTestCodeByID(item.TestCodeID);
            return result;
        }

        public async Task<TestCode> GetTestCodeXName(TestCode item, CallContext context)
        {
            var result = await Logic.GetTestCodeXName(item);
            return result;
        }
        public async Task<List<CalibrationSubType>> GetCalibrationSubtype(CallContext context)
        {
            var result = await Logic.GetCalibrationSubTypes();
            return result.ToList();
        }

        public string GetComponent(CallContext context)
        {
            var header = context.RequestHeaders.Where(x => x.Key.ToLower() == "component").FirstOrDefault();
            var user = context.ServerCallContext.GetHttpContext();

            string com = "";

            if (header != null)
            {
                com = header.Value;


            }

            return com;
        }

        public async Task<InstrumentThread> GetInstrumentThread(WorkOrderDetail wod, CallContext context)
        {
            var result = await Logic.GetInstrumentThread(wod);
            return result;
        }

        public async Task<List<GenericCalibrationResult2>> GetResultsTable(WorkOrderDetail wod, CallContext context)
        {
            var result = await Logic.GetResultsTable(wod);
            return result;
        }

        public async Task<WOD_ParametersTable> GetWOD_Parameter(WOD_ParametersTable DTO, CallContext context)
        {
            var result = await Logic.GetWOD_Parameter(DTO);
            return result;
        }

        //public async Task<WorkOrderDetail> CalculateValuesByID(WorkOrderDetail DTO, CallContext context)
        //{
        //    var result = await Logic.CalculateValuesByID(DTO);
         
        //    return result;
        //}

         public async ValueTask<WorkOrderDetail> SaveWodOff(WorkOrderDetail DTO, CallContext context)
        {


            var al = await Logic.SaveWod(DTO, GetComponent(context), true);

            var a = await Logic.GetByID(DTO);

            a = Format(a);

            return a;
        }

        public async ValueTask<ResultSet<WorkOrderDetail>> GetWods(Pagination<WorkOrderDetail> pag, CallContext context)
        {
            return await Logic.GetWods(pag);
        }
        public async ValueTask<ResultSet<WorkOrderDetail>> GetWodsFromQuery(Pagination<WorkOrderDetail> pag, CallContext context)
        {
            return await Logic.GetWodsFromQuery(pag);
        }

        public async ValueTask<IEnumerable<WorkOrderDetail>> GetWorkOrderDetailChildren(WorkOrderDetail workOrderDetail, CallContext context)
        {
            var result = await Logic.GetWorkOrderDetailChildren(workOrderDetail.WorkOrderDetailID);
            return result;
        }
        public async ValueTask<IEnumerable<ChildrenView>> GetChildrenView(WorkOrderDetail workOrderDetail, CallContext context)
        {
            var result = await Logic.GetChildrenView(workOrderDetail.WorkOrderDetailID);
            return result;
        }

        //public async Task<IEnumerable<Force>> GetWODCountPerDay(CallContext context)
        //{
        //    var result = await Logic.GetWODCountPerDay();
        //    return result;
        //}

    }
}
