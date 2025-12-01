using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Views;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bogus;
using Helpers;
using static System.Net.WebRequestMethods;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using Reports.Domain.ReportViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CalibrationSaaS.Domain.Aggregates.Querys
{
   public partial class Querys
    {


        //public CalibrationType GetConfiguration(EquipmentType EquipmentType)
        //{
        //    if (EquipmentType.DynamicConfiguration)
        //    {

        //        return EquipmentType.CalibrationType;

        //    }

        //    return null;
        //}


        public static IEnumerable<EquipmentCondition> DefaultEquipmentConditionLTI()
        {
            List<EquipmentCondition> lsteqc = new List<EquipmentCondition>();

            EquipmentCondition eqc = new EquipmentCondition
            {

                IsAsFound = true,
                Label = "Out of Service /In Service",
                Value = false,
                Name = "Status"

            };

            EquipmentCondition eqc2 = new EquipmentCondition
            {

                IsAsFound = false,
                Label = "Out of Service /In Service",
                Value = false,
                Name = "Status"


            };

            //EquipmentCondition eqc3 = new EquipmentCondition
            //{

            //    IsAsFound = false,
            //    Label = "Out of Service /In Service",
            //    Value = false,
            //    Name = "Status"

            //};
            //EquipmentCondition eqc4 = new EquipmentCondition
            //{

            //    IsAsFound = false,
            //    Label = "Out of Service /In Service",
            //    Value = false,
            //    Name = "Status"

            //};

            EquipmentCondition ceqc = new EquipmentCondition
            {

                IsAsFound = false,
                Label = "Out of Service /In Service",
                Value = false,
                Name = "In Working Order"

            };

            EquipmentCondition ceqc2 = new EquipmentCondition
            {

                IsAsFound = false,
                Label = "No / Yes",
                Value = false,
                Name = "In Working Order"

            };

            //EquipmentCondition ceqc3 = new EquipmentCondition
            //{

            //    IsAsFound = false,
            //    Label = "No / Yes",
            //    Value = false,
            //    Name = "In Working Order"

            //};
            //EquipmentCondition ceqc4 = new EquipmentCondition
            //{

            //    IsAsFound = false,
            //    Label = "No / Yes",
            //    Value = false,
            //    Name = "In Working Order"

            //};

            lsteqc.Add(eqc);
            lsteqc.Add(eqc2);
            //lsteqc.Add(ceqc);
            //lsteqc.Add(ceqc2);
          
            return lsteqc;
        }

        public static IEnumerable<EquipmentCondition> DefaultEquipmentConditionForBalance()
        {
            List<EquipmentCondition> lsteqc = new List<EquipmentCondition>();

            EquipmentCondition eqc = new EquipmentCondition
            {

                IsAsFound = true,
                Label = "Out of Service /In Service",
                Value = false,
                Name = "Status"

            };

            EquipmentCondition eqc2 = new EquipmentCondition
            {

                IsAsFound = false,
                Label = "Out of Service /In Service",
                Value = false,
                Name = "Status"


            };

            EquipmentCondition eqc3 = new EquipmentCondition
            {

                IsAsFound = false,
                Label = "Out of Service /In Service",
                Value = false,
                Name = "Status"

            };
            EquipmentCondition eqc4 = new EquipmentCondition
            {

                IsAsFound = false,
                Label = "Out of Service /In Service",
                Value = false,
                Name = "Status"

            };

            EquipmentCondition ceqc = new EquipmentCondition
            {

                IsAsFound = false,
                Label = "No / Yes",
                Value = false,
                Name = "Clean"

            };

            EquipmentCondition ceqc2 = new EquipmentCondition
            {

                IsAsFound = false,
                Label = "No / Yes",
                Value = false,
                Name = "Clean"

            };

            EquipmentCondition ceqc3 = new EquipmentCondition
            {

                IsAsFound = false,
                Label = "No / Yes",
                Value = false,
                Name = "Clean"

            };
            EquipmentCondition ceqc4 = new EquipmentCondition
            {

                IsAsFound = false,
                Label = "No / Yes",
                Value = false,
                Name = "Clean"

            };
        
            EquipmentCondition leqc = new EquipmentCondition
            {

                IsAsFound = false,
                Label = "No / Yes",
                Value = false,
                Name = "Level"

            };

            EquipmentCondition leqc2 = new EquipmentCondition
            {

                IsAsFound = false,
                Label = "No / Yes",
                Value = false,
                Name = "Level"

            };

            EquipmentCondition leqc3 = new EquipmentCondition
            {

                IsAsFound = false,
                Label = "No / Yes",
                Value = false,
                Name = "Level"

            };
            EquipmentCondition leqc4 = new EquipmentCondition
            {

                IsAsFound = false,
                Label = "No / Yes",
                Value = false,
                Name = "Level"

            };

           
            EquipmentCondition feqc = new EquipmentCondition
            {

                IsAsFound = false,
                Label = "No / Yes",
                Value = false,
                Name = "Functioning"

            };

            EquipmentCondition feqc2 = new EquipmentCondition
            {

                IsAsFound = false,
                Label = "No / Yes",
                Value = false,
                Name = "Functioning"

            };

            EquipmentCondition feqc3 = new EquipmentCondition
            {

                IsAsFound = false,
                Label = "No / Yes",
                Value = false,
                Name = "Functioning"

            };
            EquipmentCondition feqc4 = new EquipmentCondition
            {

                IsAsFound = false,
                Label = "No / Yes",
                Value = false,
                Name = "Functioning"

            };




            lsteqc.Add(eqc);
            lsteqc.Add(eqc2);
          
            //lsteqc.Add(ceqc);
            //lsteqc.Add(ceqc2);
         

            //lsteqc.Add(leqc);
            //lsteqc.Add(leqc2);
     
            //lsteqc.Add(feqc);
            //lsteqc.Add(feqc2);
     
            return lsteqc;
        }



        public class WOD
        {
                    



            public static Expression<Func<TestPoint, bool>> GetLinearityTestPoint()
            {
                
                Expression<Func<TestPoint, bool>> exprTree = x => x.CalibrationType != null && x.CalibrationType.ToLower() == "linearity";
                return exprTree;
            }

            public static Expression<Func<TestPoint, bool>> GetEccentricityTestPoint()
            {
                Expression<Func<TestPoint, bool>> exprTree = x => x.CalibrationType != null && x.CalibrationType.ToLower() == "eccentricity";
                return exprTree;
            }

            public static Expression<Func<TestPoint, bool>> GetRepeatibilityTestPoint()
            {
                Expression<Func<TestPoint, bool>> exprTree = x => x.CalibrationType != null && x.CalibrationType.ToLower().Trim() == "repeatability";
                return exprTree;
            }

            public static Domain.Aggregates.Entities.Eccentricity GetEccentricityList(WorkOrderDetail eq)
            {
//                Console.WriteLine("GetEccentricityList");
                List<Domain.Aggregates.Entities.Eccentricity> list = new List<Eccentricity>();

                var r= eq.BalanceAndScaleCalibration.Eccentricity;

                return r;


            }
            public static Domain.Aggregates.Entities.Repeatability GetRepeatabilityList(WorkOrderDetail eq)
            {

                List<Domain.Aggregates.Entities.Repeatability> list = new List<Domain.Aggregates.Entities.Repeatability>();

                var r = eq.BalanceAndScaleCalibration.Repeatability;

                return r;


            }


            public static Domain.Aggregates.Entities.Eccentricity CreateEccentricityList(WorkOrderDetail eq, bool GenerateId)
            {

                List<Domain.Aggregates.Entities.Eccentricity> list = new List<Domain.Aggregates.Entities.Eccentricity>();

                List<TestPoint> tp = null;

                if(eq?.PieceOfEquipment?.IsTestPointImport  == false &&  
                    eq?.PieceOfEquipment?.EquipmentTemplate?.TestGroups != null && eq?.PieceOfEquipment?.EquipmentTemplate?.TestGroups?.ElementAtOrDefault(0)?.TestPoints?.Count > 0)
                {
                    tp = eq.PieceOfEquipment.EquipmentTemplate.TestGroups.ElementAtOrDefault(0).TestPoints.Where(Querys.WOD.GetEccentricityTestPoint().Compile()).ToList();
                }

                if ( eq?.PieceOfEquipment?.IsTestPointImport == true && eq?.PieceOfEquipment?.TestGroups != null && eq?.PieceOfEquipment?.TestGroups.Count > 0 && eq?.PieceOfEquipment?.TestGroups?.ElementAtOrDefault(0)?.TestPoints?.Count > 0)
                {
                    tp = eq.PieceOfEquipment.TestGroups.ElementAtOrDefault(0).TestPoints.Where(Querys.WOD.GetEccentricityTestPoint().Compile()).ToList();
                }


                if (tp == null || tp?.Count == 0)
                {
                    Eccentricity e = new Eccentricity();
                    e.WorkOrderDetailId = eq.WorkOrderDetailID;
              
                    return e;
                }
                int cont = 1;
                foreach (var item in tp)
                {
                    Domain.Aggregates.Entities.Eccentricity ln = new Domain.Aggregates.Entities.Eccentricity();

                    if (GenerateId || 1 == 1)
                    {
                        item.TestPointID = NumericExtensions.GetUniqueID(0);
                    }
                    else
                    {
                        item.TestPointID = 0;
                    }
                    ln.TestPoint = item; 
                    ln.TestPointID = item.TestPointID;
                    ln.TestPointResult = new List<BasicCalibrationResult>();
                    ln.CalibrationSubTypeId = new Eccentricity().CalibrationSubTypeId;
                    double wa = 0;
                    if (eq?.PieceOfEquipment?.Capacity != null && eq.PieceOfEquipment?.Capacity != 0)
                    {
                        wa = eq.PieceOfEquipment.Capacity / 3;
                    }
                    for (int i = 0; i < ln.NumberOfSamples; i++)
                    {
                       
                        ln.WorkOrderDetailId = eq.WorkOrderDetailID;
                        BasicCalibrationResult bc = new BasicCalibrationResult()
                        {
                            CalibrationSubTypeId = ln.CalibrationSubTypeId,
                            WorkOrderDetailId = eq.WorkOrderDetailID,
                            SequenceID = i + 1,
                            UnitOfMeasure = item.UnitOfMeasurement,
                            UnitOfMeasureID = item.UnitOfMeasurementID,
                            WeightApplied = wa,
                            Position = i + 1,
                          

                        };
                        ln.TestPointResult.Add(bc);
                    }

                    ln.WorkOrderDetailId = eq.WorkOrderDetailID;
                    ln.WeightSets = new List<WeightSet>();
                    list.Add(ln);
                    cont = cont + 1;
                }

                return list[0];

            }

            public static Domain.Aggregates.Entities.Repeatability CreateRepeatabilityList(WorkOrderDetail eq, bool GenerateID)
            {

                List<Domain.Aggregates.Entities.Repeatability> list = new List<Domain.Aggregates.Entities.Repeatability>();

                List<TestPoint> tp = null;
                
                if(eq?.PieceOfEquipment?.IsTestPointImport == false && eq?.PieceOfEquipment?.EquipmentTemplate?.TestGroups != null 
                    &&  eq?.PieceOfEquipment?.EquipmentTemplate?.TestGroups?.ElementAtOrDefault(0)?.TestPoints?.Count > 0)
                {
                    var test = eq.PieceOfEquipment.EquipmentTemplate.TestGroups.ElementAtOrDefault(0).TestPoints.ToList();

                    tp = test.Where(Querys.WOD.GetRepeatibilityTestPoint().Compile()).ToList();
                }

                if (eq?.PieceOfEquipment?.IsTestPointImport == true && eq?.PieceOfEquipment?.TestGroups != null 
                    && eq?.PieceOfEquipment?.TestGroups.Count > 0 
                    && eq?.PieceOfEquipment?.TestGroups?.ElementAtOrDefault(0)?.TestPoints?.Count > 0)
                {
                    tp = eq.PieceOfEquipment.TestGroups.ElementAtOrDefault(0).TestPoints.Where(Querys.WOD.GetRepeatibilityTestPoint().Compile()).ToList();
                }



                if (tp == null || tp?.Count == 0)
                {
                    Entities.Repeatability e = new Entities.Repeatability();
                    e.WorkOrderDetailId = eq.WorkOrderDetailID;
                    e.CalibrationSubTypeId = 2;

                    return e;
                }
                int cont = 1;
                foreach (var item in tp)
                {
                    Domain.Aggregates.Entities.Repeatability ln = new Domain.Aggregates.Entities.Repeatability();

                    if (GenerateID || 1 == 1)
                    {
                        item.TestPointID = NumericExtensions.GetUniqueID(0);
                    }
                    else
                    {
                        item.TestPointID = 0;
                    }

                    item.TestPointTarget = 3;
                   
                    ln.TestPoint = item;
                    ln.TestPointID = item.TestPointID;
                    ln.TestPointResult = new List<BasicCalibrationResult>();
                    ln.CalibrationSubTypeId = 2;
                    ln.WorkOrderDetailId=eq.WorkOrderDetailID;

                    double wa = 0;
                    if (eq?.PieceOfEquipment?.Capacity != null && eq.PieceOfEquipment?.Capacity != 0)
                    {
                        wa = eq.PieceOfEquipment.Capacity / 3;
                    }

                    for (int i = 0; i < ln.NumberOfSamples; i++)
                    {
                     

                        ln.WorkOrderDetailId = eq.WorkOrderDetailID;
                        BasicCalibrationResult bc = new BasicCalibrationResult()
                        {
                            CalibrationSubTypeId = ln.CalibrationSubTypeId,
                            WorkOrderDetailId = eq.WorkOrderDetailID,
                            SequenceID = i + 1,
                            UnitOfMeasure = item.UnitOfMeasurement,
                            UnitOfMeasureID = item.UnitOfMeasurementID,
                            WeightApplied = wa /*item.NominalTestPoit*/,
                            Position = i + 1

                        };
                        ln.TestPointResult.Add(bc);
                    }


                    ln.WeightSets = new List<WeightSet>();
                    list.Add(ln);
                    cont = cont + 1;
                }

                return list[0];

            }


          



         

         

            public static List<Domain.Aggregates.Entities.Linearity> GetLinearityList(WorkOrderDetail eq)
            {
               
                List<Domain.Aggregates.Entities.Linearity> listlienarity = new List<Domain.Aggregates.Entities.Linearity>();

                listlienarity= eq?.BalanceAndScaleCalibration?.Linearities.OrderBy(x => x.TestPoint.Position).ToList();

                return listlienarity;


            }

                public static List<Domain.Aggregates.Entities.Linearity> CreateLinearityList(WorkOrderDetail eq, bool GenerateID)
            {
                List<Domain.Aggregates.Entities.Linearity> listlienarity = new List<Domain.Aggregates.Entities.Linearity>();

                List<TestPoint> tp = null;


                if (eq?.PieceOfEquipment?.IsTestPointImport == false && eq?.PieceOfEquipment?.EquipmentTemplate?.TestGroups != null   && eq?.PieceOfEquipment?.EquipmentTemplate?.TestGroups?.ElementAtOrDefault(0)?.TestPoints.Count > 0)
                {
                   tp = eq.PieceOfEquipment.EquipmentTemplate.TestGroups.ElementAtOrDefault(0).TestPoints.Where(Querys.WOD.GetLinearityTestPoint().Compile()).ToList();
                }
                else

                if (eq?.PieceOfEquipment?.IsTestPointImport == true && eq?.PieceOfEquipment?.TestGroups != null && eq?.PieceOfEquipment?.TestGroups.Count > 0  
                    && eq?.PieceOfEquipment?.TestGroups?.ElementAtOrDefault(0)?.TestPoints?.Count > 0)
                {
                    tp= eq.PieceOfEquipment.TestGroups.ElementAtOrDefault(0).TestPoints.Where(Querys.WOD.GetLinearityTestPoint().Compile()).ToList();
                }
                

                if (tp == null || tp?.Count == 0)
                {
                    return null;
                    Linearity e = new Linearity();
                    e.WorkOrderDetailId = eq.WorkOrderDetailID;
                    e.CalibrationSubTypeId = 1;
                    listlienarity.Add(e);
                    return listlienarity;
                }


                tp = tp.OrderBy(x => x.NominalTestPoit).OrderBy(x => x.IsDescendant).ToList();
                

           

                int cont = 1;
                foreach (var item in tp)
                {
                    Domain.Aggregates.Entities.Linearity ln = new Domain.Aggregates.Entities.Linearity();

                    if (GenerateID || 1 == 1)
                    {
                        item.TestPointID = NumericExtensions.GetUniqueID(0);
                    }
                    else
                    {
                        item.TestPointID = 0;
                    }

                    item.TestPointGroupTestPoitGroupID = null;
                    item.TestPointTarget = 3;
                    ln.TestPointID =  item.TestPointID;
                    ln.WorkOrderDetailId = eq.WorkOrderDetailID;
                
                    ln.SequenceID = cont;
                    


                    ln.TestPoint = item;
                    ln.UnitOfMeasure = item.UnitOfMeasurement;

                    ln.UnitOfMeasureId = item.UnitOfMeasurementID;
                    ln.BasicCalibrationResult = new BasicCalibrationResult();
                    ln.BasicCalibrationResult.CalibrationSubTypeId = 1;
                    ln.BasicCalibrationResult.SequenceID = cont;
                    ln.BasicCalibrationResult.WorkOrderDetailId = eq.WorkOrderDetailID;
                    ln.BasicCalibrationResult.UnitOfMeasure = ln.UnitOfMeasure;
                    ln.BasicCalibrationResult.UnitOfMeasureID = item.UnitOfMeasurementID;
                    ln.MaxTolerance = item.UpperTolerance;
                    ln.MinTolerance = item.LowerTolerance;
                    ln.WeightSets = new List<WeightSet>();
                    
                    listlienarity.Add(ln);
                    cont = cont + 1;
                }
                if(listlienarity.Count > 0 && listlienarity[0].TestPoint.NominalTestPoit==0)
                {
                    listlienarity[0].MinTolerance = 0;
                    listlienarity[0].MaxTolerance = 0;
                }

                return listlienarity;

            }

            public static async Task<WorkOrderDetail> CalculateValuesByID(WorkOrderDetail DTO,IEnumerable<UnitOfMeasure> UoMs,  bool IsLinearity=true, CalibrationType calibrationType = null)
            {

                int linenumber = 0;

                //WorkOrderDetail workGetById = await Repository.GetByID(DTO);
                //WorkOrderDetail workGetWorkOrderItem = await Repository.GetWorkOrderDetailByID(DTO.WorkOrderDetailID);

                if (DTO.BalanceAndScaleCalibration == null)
                {
                    return DTO;
                }

                try
                {
                    var workOrderDetail = DTO;//DTO;

                    //workOrderDetail.BalanceAndScaleCalibration = workGetWorkOrderItem.BalanceAndScaleCalibration;//DTO;



                    double ecceALMax = 0;
                    double ecceALMin = 0;
                    double ecceAFMax = 0;
                    double ecceAFMin = 0;

                    List<double> ecceAFValues = new List<double>();
                    List<double> ecceALValues = new List<double>();
                    List<double> repeAFValues = new List<double>();
                    List<double> repeALValues = new List<double>();

                    int ecceCounter = 0;
                    int repeCounter = 0;
                    linenumber = 1;


                    if (workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity?.TestPointResult != null)
                    {
                        foreach (BasicCalibrationResult basicCalibrationResult in workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult)
                        {
                            ecceAFValues.Add(basicCalibrationResult.AsFound);

                            ecceALValues.Add(basicCalibrationResult.AsLeft);
                            if (ecceAFMin == 0)
                            {
                                ecceAFMin = ecceAFValues[ecceCounter];

                            }
                            if (ecceALMin == 0)
                            {
                                ecceALMin = ecceALValues[ecceCounter];

                            }
                            if (ecceAFValues[ecceCounter] < ecceAFMin && ecceAFValues[ecceCounter] > 0)
                            {
                                ecceAFMin = ecceAFValues[ecceCounter];
                            }
                            if (ecceAFValues[ecceCounter] >= ecceAFMax)
                            {
                                ecceAFMax = ecceAFValues[ecceCounter];
                            }
                            if (ecceALValues[ecceCounter] < ecceALMin && ecceALValues[ecceCounter] > 0)
                            {
                                ecceALMin = ecceALValues[ecceCounter];
                            }
                            if (ecceALValues[ecceCounter] >= ecceALMax)
                            {
                                ecceALMax = ecceALValues[ecceCounter];
                            }
                            ecceCounter++;
                        }



                    }

                    if (workOrderDetail?.BalanceAndScaleCalibration?.Repeatability?.TestPointResult != null)
                    {
                        foreach (BasicCalibrationResult basicCalibrationResult in workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult)
                        {
                            repeAFValues.Add(basicCalibrationResult.AsFound);
                            repeALValues.Add(basicCalibrationResult.AsLeft);
                            repeCounter++;
                        }
                    }
                    linenumber = 2;


                    linenumber = 3;



                    EquipmentTemplate et = workOrderDetail.PieceOfEquipment.EquipmentTemplate;
                    PieceOfEquipment POE = workOrderDetail.PieceOfEquipment;
                    //var poes = await PieceOfEquipmentRepository.GetAllWeightSets(POE);
                    var linearities = workOrderDetail.BalanceAndScaleCalibration.Linearities.OrderBy(x=>x.TestPoint.Position).ToList();

                    if (workOrderDetail?.BalanceAndScaleCalibration?.Linearities != null && IsLinearity)
                    {

                        for (int i = 0; i < linearities.Count; i++)
                        {

                            var linearity = linearities[i];
                            linearity.BalanceAndScaleCalibration = DTO.BalanceAndScaleCalibration;

                            // FEATURE DISABLED: "Copy WeightApplied from previous/next test point"
                            // This feature was confusing because it would automatically copy WeightApplied values
                            // from adjacent test points when the current test point had WeightApplied = 0.
                            // This caused issues when users intentionally cleared weights for a test point.
                            // To re-enable this feature, uncomment the code block below.

                            /*
                            // OLD BEHAVIOR: Copy WeightApplied from previous or next test point if current is 0
                            if (linearity.BasicCalibrationResult.WeightApplied == 0 && linearity.TestPoint.NominalTestPoit != 0)
                            {
                                double? previous = i > 0 ? linearities[i - 1].BasicCalibrationResult.WeightApplied : (double?)null;
                                double? next = i < linearities.Count - 1 ? linearities[i + 1].BasicCalibrationResult.WeightApplied : (double?)null;

                                if (previous.HasValue && previous.Value != 0)
                                {
                                    linearity.BasicCalibrationResult.WeightApplied = previous.Value;
                                }
                                else if (next.HasValue && next.Value != 0)
                                {
                                    linearity.BasicCalibrationResult.WeightApplied = next.Value;
                                }
                            }
                            */

                            await calculateCalibrationResultValues(linearity,workOrderDetail,UoMs);


                            if (calibrationType != null && calibrationType.CMCValues != null && calibrationType.CMCValues.Count() > 0)
                            {
                                var nominalValue = linearity.TestPoint.NominalTestPoit;
                                
                                foreach (var item in calibrationType.CMCValues)
                                {
                                    bool replace = false;
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

                                    if (replace && linearity.TotalUncertainty < item.CMC)
                                    {
                                        linearity.BasicCalibrationResult.UncertaintyNew = item.CMC;
                                        break;
                                    }
                                }

                            }
                        }
                    }

                    linenumber = 4;
                    if (workOrderDetail?.BalanceAndScaleCalibration?.Repeatability?.TestPointResult != null
                          && workOrderDetail?.BalanceAndScaleCalibration?.Repeatability?.TestPointResult?.Count > 0)
                    {
                        workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityStdDeviationAsLeft = Stdev(repeALValues.ToArray());
                        workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityStdDeviationAsFound = Stdev(repeAFValues.ToArray());
                    }
                    linenumber = 5;
                    if (workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity != null
                            && workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity?.TestPointResult != null
                            && workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity?.TestPointResult?.Count > 0)
                    {
                        linenumber = 51;
                        workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityVarianceAsLeft = ecceALMax - ecceALMin;
                        workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityVarianceAsFound = ecceAFMax - ecceAFMin;
                        workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityMax = ecceALMax;
                        workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityMin = ecceALMin;
                        linenumber = 52;

                        workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityUncertaintyValueUOMID = workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.FirstOrDefault().UnitOfMeasureID;
                        workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityUncertaintyType = "A";
                        workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityUncertaintyDistribution = "Rectangular";
                        workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityUncertaintyDivisor = 1.73;
                        linenumber = 53;
                        double eccentricityDeltaFinalUOM = 0;
                        if (workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity?.TestPointResult?.Count > 0
                                && workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity?.TestPointResult?.FirstOrDefault().UnitOfMeasure != null
                                && workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.FirstOrDefault().UnitOfMeasure.UncertaintyUnitOfMeasureID.requiresUncertantyConversion(workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.FirstOrDefault().UnitOfMeasure.UnitOfMeasureID))
                        {
                            linenumber = 54;
                            eccentricityDeltaFinalUOM = workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.FirstOrDefault().UnitOfMeasure.ConversionValue.ConvertToUOM(workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityDelta, workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.FirstOrDefault().UnitOfMeasure.UncertaintyUnitOfMeasure);
                        }
                        else
                        {

                            eccentricityDeltaFinalUOM = workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityDelta;
                            linenumber = 55;
                        }
                        workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityQuotient = eccentricityDeltaFinalUOM / workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityUncertaintyDivisor;
                        linenumber = 6;

                        await calculateEccentricityCalibrationResultValues(workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity, et, POE, workOrderDetail);
                        linenumber = 7;
                    }




                    linenumber = 8;

                    if (workOrderDetail?.BalanceAndScaleCalibration?.Repeatability != null && workOrderDetail?.BalanceAndScaleCalibration?.Repeatability?.TestPointResult != null && workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.FirstOrDefault()?.UnitOfMeasure?.UncertaintyUnitOfMeasure != null)
                    {
                        workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityUncertaintyValueUOMID = workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.FirstOrDefault().UnitOfMeasure.UnitOfMeasureID; ;
                        workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityUncertaintyType = "A";
                        workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityUncertaintyDistribution = "Normal";
                        workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityUncertaintyDivisor = 1;

                        double repeatabilityStdDeviationAsLeftFinalUOM = 0;

                        if (workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.FirstOrDefault().UnitOfMeasure.UncertaintyUnitOfMeasureID.requiresUncertantyConversion(workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.FirstOrDefault().UnitOfMeasure.UnitOfMeasureID))
                        {
                            repeatabilityStdDeviationAsLeftFinalUOM = workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.FirstOrDefault().UnitOfMeasure.ConversionValue.ConvertToUOM(workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityStdDeviationAsLeft, workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.FirstOrDefault().UnitOfMeasure.UncertaintyUnitOfMeasure);
                        }
                        else
                        {
                            repeatabilityStdDeviationAsLeftFinalUOM = workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityStdDeviationAsLeft;
                        }
                        workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityQuotient = repeatabilityStdDeviationAsLeftFinalUOM / workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityUncertaintyDivisor;


                        await calculateRepeatCalibrationResultValues(workOrderDetail?.BalanceAndScaleCalibration?.Repeatability, et, POE, workOrderDetail);
                    }

                    linenumber = 9;

                    if (workOrderDetail?.BalanceAndScaleCalibration?.Linearities != null && workOrderDetail?.BalanceAndScaleCalibration?.Linearities?.FirstOrDefault()?.BasicCalibrationResult != null)
                    {
                        workOrderDetail.BalanceAndScaleCalibration.ResolutionUncertaintyValueUOMID = workOrderDetail.BalanceAndScaleCalibration.Linearities.FirstOrDefault().BasicCalibrationResult.UnitOfMeasureID;
                    }

                    linenumber = 10;
                    if (workOrderDetail?.BalanceAndScaleCalibration != null)
                    {
                        workOrderDetail.BalanceAndScaleCalibration.ResolutionUncertaintyType = "B";
                        workOrderDetail.BalanceAndScaleCalibration.ResolutionUncertaintyDistribution = "Resolution";
                        workOrderDetail.BalanceAndScaleCalibration.ResolutionUncertaintyDivisor = 3.46;

                        workOrderDetail.BalanceAndScaleCalibration.ResolutionFormatted = 1 / (Math.Pow(10, 3));
                    }

                    linenumber = 11;

                    if (workOrderDetail?.BalanceAndScaleCalibration?.Linearities != null && workOrderDetail?.BalanceAndScaleCalibration?.Linearities?.FirstOrDefault()?.BasicCalibrationResult != null)
                        workOrderDetail.BalanceAndScaleCalibration.EnvironmentalUncertaintyValueUOMID = workOrderDetail.BalanceAndScaleCalibration.Linearities.FirstOrDefault().BasicCalibrationResult.UnitOfMeasureID;
                    workOrderDetail.BalanceAndScaleCalibration.EnvironmentalUncertaintyType = "B";
                    workOrderDetail.BalanceAndScaleCalibration.EnvironmentalUncertaintyDistribution = "Rectangular";
                    workOrderDetail.BalanceAndScaleCalibration.EnvironmentalUncertaintyDivisor = 1.73;

                    workOrderDetail.BalanceAndScaleCalibration.EnvironmentalFactor = 0.004;


                    linenumber = 12;
                    //TODO
                    if (workOrderDetail?.PieceOfEquipment?.EquipmentTemplate?.PieceOfEquipments != null)
                    {
                        workOrderDetail.PieceOfEquipment.EquipmentTemplate.PieceOfEquipments = null;
                    }
                    if (workOrderDetail?.PieceOfEquipment?.WorOrderDetails != null)
                    {
                        workOrderDetail.PieceOfEquipment.WorOrderDetails = null;
                    }
                    //if (workOrderDetail.WOD_Weights != null)
                    //{
                    //    workOrderDetail.WOD_Weights = null;
                    //}
                    if (workOrderDetail?.BalanceAndScaleCalibration?.WorkOrderDetail != null)
                    {
                        workOrderDetail.BalanceAndScaleCalibration.WorkOrderDetail = null;
                    }


                    var selectedData = new WorkOrderDetail
                    {
                        WorkOrderDetailID = workOrderDetail.WorkOrderDetailID,
                        //PieceOfEquipment = workOrderDetail.PieceOfEquipment
                        BalanceAndScaleCalibration = workOrderDetail.BalanceAndScaleCalibration,
                        WOD_Weights = workOrderDetail.WOD_Weights,
                        PieceOfEquipment = workOrderDetail.PieceOfEquipment,

                        // Añade otras propiedades necesarias
                    };


                    return selectedData;
                }
                catch (Exception ex)
                {
//                    Console.WriteLine(ex.Message);
                    if(ex.InnerException != null)
                    {
//                        Console.WriteLine(ex.InnerException.Message);
                    }
                   
//                    Console.WriteLine(ex.StackTrace);

                    throw new Exception(ex.Message + "__ " + linenumber);
                }




            }


            //     public static async System.Threading.Tasks.Task<WorkOrderDetail> CalculateValuesByID(WorkOrderDetail DTO)
            //     {

            //         int linenumber = 0;

            //         try
            //         {
            //                 var workOrderDetail = DTO;

            //         double ecceALMax = 0;
            //         double ecceALMin = 0;
            //         double ecceAFMax = 0;
            //         double ecceAFMin = 0;

            //         List<double> ecceAFValues = new List<double>();
            //         List<double> ecceALValues = new  List<double>();
            //         List<double> repeAFValues = new List<double>();
            //         List<double> repeALValues = new List<double>();

            //         int ecceCounter = 0;
            //         int repeCounter = 0;
            //             linenumber = 1;


            //         if (workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity?.TestPointResult != null)
            //         {
            //             foreach (BasicCalibrationResult basicCalibrationResult in workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult)
            //             {
            //                    ecceAFValues.Add(basicCalibrationResult.AsFound);

            //ecceALValues.Add(basicCalibrationResult.AsLeft);
            //                 if (ecceAFMin == 0)
            //                 {
            //                     ecceAFMin = ecceAFValues[ecceCounter];

            //                    }
            //                 if (ecceALMin == 0)
            //                 {
            //                     ecceALMin = ecceALValues[ecceCounter];

            //                 }
            //                 if (ecceAFValues[ecceCounter] < ecceAFMin && ecceAFValues[ecceCounter] > 0)
            //                 {
            //                     ecceAFMin = ecceAFValues[ecceCounter];
            //                 }
            //                 if (ecceAFValues[ecceCounter] >= ecceAFMax)
            //                 {
            //                     ecceAFMax = ecceAFValues[ecceCounter];
            //                 }
            //                 if (ecceALValues[ecceCounter] < ecceALMin && ecceALValues[ecceCounter] > 0)
            //                 {
            //                     ecceALMin = ecceALValues[ecceCounter];
            //                 }
            //                 if (ecceALValues[ecceCounter] >= ecceALMax)
            //                 {
            //                     ecceALMax = ecceALValues[ecceCounter];
            //                 }
            //                 ecceCounter++;
            //             }



            //          }

            //             if (workOrderDetail?.BalanceAndScaleCalibration?.Repeatability?.TestPointResult != null)
            //             {
            //                 foreach (BasicCalibrationResult basicCalibrationResult in workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult)
            //                 {
            //   repeAFValues.Add(basicCalibrationResult.AsFound);
            //  repeALValues.Add(basicCalibrationResult.AsLeft);
            //                     repeCounter++;
            //                 }
            //             }
            //            linenumber = 2;


            //              linenumber = 3;
            //          EquipmentTemplate et = workOrderDetail.PieceOfEquipment.EquipmentTemplate;
            //         PieceOfEquipment POE = workOrderDetail.PieceOfEquipment;
            //         if (workOrderDetail?.BalanceAndScaleCalibration?.Linearities != null)
            //         { 
            //             foreach (Linearity linearity in workOrderDetail.BalanceAndScaleCalibration.Linearities)
            //             {
            //                 linearity.BalanceAndScaleCalibration = DTO.BalanceAndScaleCalibration;
            //                 calculateCalibrationResultValues(linearity, et, POE, workOrderDetail);
            //             }
            //         }

            //          linenumber = 4;
            //           if ( workOrderDetail?.BalanceAndScaleCalibration?.Repeatability?.TestPointResult != null  
            //                 && workOrderDetail?.BalanceAndScaleCalibration?.Repeatability?.TestPointResult?.Count > 0 )
            //         {
            //             workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityStdDeviationAsLeft = NumericExtensions.standardDeviation(repeALValues.ToArray());
            //             workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityStdDeviationAsFound = NumericExtensions.standardDeviation(repeAFValues.ToArray());
            //         }
            //          linenumber = 5;
            //         if (workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity != null  
            //                 && workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity?.TestPointResult != null 
            //                 && workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity?.TestPointResult?.Count > 0)
            //         {   linenumber = 51;
            //             workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityVarianceAsLeft = ecceALMax - ecceALMin;
            //             workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityVarianceAsFound = ecceAFMax - ecceAFMin;
            //             workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityMax = ecceALMax;
            //             workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityMin = ecceALMin;
            //                 linenumber = 52;

            //             workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityUncertaintyValueUOMID = workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.FirstOrDefault().UnitOfMeasureID;
            //             workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityUncertaintyType = "A";
            //             workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityUncertaintyDistribution = "Rectangular";
            //             workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityUncertaintyDivisor = 1.73;
            //             linenumber = 53;
            //                 double eccentricityDeltaFinalUOM = 0;
            //             if (workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity?.TestPointResult?.Count > 0 
            //                     && workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity?.TestPointResult?.FirstOrDefault().UnitOfMeasure != null 
            //                     && workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.FirstOrDefault().UnitOfMeasure.UncertaintyUnitOfMeasureID.requiresUncertantyConversion(workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.FirstOrDefault().UnitOfMeasure.UnitOfMeasureID))
            //             {
            //                     linenumber = 54;
            //                     eccentricityDeltaFinalUOM = workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.FirstOrDefault().UnitOfMeasure.ConversionValue.ConvertToUOM(workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityDelta, workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.FirstOrDefault().UnitOfMeasure.UncertaintyUnitOfMeasure);
            //             }
            //             else
            //             {

            //                 eccentricityDeltaFinalUOM = workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityDelta;
            //                     linenumber = 55;
            //             }
            //             workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityQuotient = eccentricityDeltaFinalUOM / workOrderDetail.BalanceAndScaleCalibration.Eccentricity.EccentricityUncertaintyDivisor;
            //                 linenumber = 6;

            //             calculateEccentricityCalibrationResultValues(workOrderDetail?.BalanceAndScaleCalibration?.Eccentricity, et, POE, workOrderDetail);
            //                     linenumber = 7;
            //         }




            //         linenumber = 8;

            //         if (workOrderDetail?.BalanceAndScaleCalibration?.Repeatability != null  && workOrderDetail?.BalanceAndScaleCalibration?.Repeatability?.TestPointResult != null && workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.FirstOrDefault()?.UnitOfMeasure?.UncertaintyUnitOfMeasure !=null)
            //         {
            //             workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityUncertaintyValueUOMID = workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.FirstOrDefault().UnitOfMeasure.UnitOfMeasureID; ;
            //             workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityUncertaintyType = "A";
            //             workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityUncertaintyDistribution = "Normal";
            //             workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityUncertaintyDivisor = 1;

            //             double repeatabilityStdDeviationAsLeftFinalUOM = 0;

            //             if (workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.FirstOrDefault().UnitOfMeasure.UncertaintyUnitOfMeasureID.requiresUncertantyConversion(workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.FirstOrDefault().UnitOfMeasure.UnitOfMeasureID))
            //             {
            //                 repeatabilityStdDeviationAsLeftFinalUOM = workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.FirstOrDefault().UnitOfMeasure.ConversionValue.ConvertToUOM(workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityStdDeviationAsLeft, workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.FirstOrDefault().UnitOfMeasure.UncertaintyUnitOfMeasure);
            //             }
            //             else
            //             {
            //                 repeatabilityStdDeviationAsLeftFinalUOM = workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityStdDeviationAsLeft;
            //             }
            //             workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityQuotient = repeatabilityStdDeviationAsLeftFinalUOM / workOrderDetail.BalanceAndScaleCalibration.Repeatability.RepeatabilityUncertaintyDivisor;


            //             calculateRepeatCalibrationResultValues(workOrderDetail?.BalanceAndScaleCalibration?.Repeatability, et, POE, workOrderDetail);
            //         }

            //         linenumber = 9;

            //         if (workOrderDetail?.BalanceAndScaleCalibration?.Linearities != null && workOrderDetail?.BalanceAndScaleCalibration?.Linearities?.FirstOrDefault()?.BasicCalibrationResult != null)
            //         {
            //             workOrderDetail.BalanceAndScaleCalibration.ResolutionUncertaintyValueUOMID = workOrderDetail.BalanceAndScaleCalibration.Linearities.FirstOrDefault().BasicCalibrationResult.UnitOfMeasureID;
            //         }

            //         linenumber = 10;
            //         if(workOrderDetail?.BalanceAndScaleCalibration != null)
            //         {
            //             workOrderDetail.BalanceAndScaleCalibration.ResolutionUncertaintyType = "B";
            //             workOrderDetail.BalanceAndScaleCalibration.ResolutionUncertaintyDistribution = "Resolution";
            //             workOrderDetail.BalanceAndScaleCalibration.ResolutionUncertaintyDivisor = 3.46;

            //             workOrderDetail.BalanceAndScaleCalibration.ResolutionFormatted = 1 / (Math.Pow(10, 3));
            //         }

            //         linenumber = 11;

            //         if (workOrderDetail?.BalanceAndScaleCalibration?.Linearities != null  && workOrderDetail?.BalanceAndScaleCalibration?.Linearities?.FirstOrDefault()?.BasicCalibrationResult != null)
            //             workOrderDetail.BalanceAndScaleCalibration.EnvironmentalUncertaintyValueUOMID = workOrderDetail.BalanceAndScaleCalibration.Linearities.FirstOrDefault().BasicCalibrationResult.UnitOfMeasureID;
            //         workOrderDetail.BalanceAndScaleCalibration.EnvironmentalUncertaintyType = "B";
            //         workOrderDetail.BalanceAndScaleCalibration.EnvironmentalUncertaintyDistribution = "Rectangular";
            //         workOrderDetail.BalanceAndScaleCalibration.EnvironmentalUncertaintyDivisor = 1.73;

            //         workOrderDetail.BalanceAndScaleCalibration.EnvironmentalFactor = 0.004;


            //         linenumber = 12;
            //         return workOrderDetail;
            //         }
            //         catch(Exception ex)
            //         {
            //             throw new Exception(ex.Message + "__ " + linenumber);
            //         }



            //     }


            public static async Task calculateEccentricityCalibrationResultValues(Eccentricity eccentricity, EquipmentTemplate equipmentTemplate, PieceOfEquipment pieceOfEquipment, WorkOrderDetail workOrderDetail)
            {
                var bc =eccentricity.TestPointResult;
                double nominalTestPointValue = 0;
               
                
                    var resolutionValue = workOrderDetail.Resolution;
                    foreach (var item in bc)
                    {
              
                        nominalTestPointValue = item.WeightApplied;
                 
                   

                    var minTolerance = nominalTestPointValue - resolutionValue;
                        var maxTolerance = nominalTestPointValue + resolutionValue;

                        item.LowerTolerance = minTolerance;
                        item.UpperTolerance = maxTolerance;

                        item.InToleranceFound = "PASS";

                        if (item.AsFound > item.UpperTolerance)
                        {
                            item.InToleranceFound = "FAIL";
                        }

                        if (item.AsFound < item.LowerTolerance)
                        {
                            item.InToleranceFound = "FAIL";
                        }

                        item.InToleranceLeft = "PASS";

                        if (item.AsLeft > item.UpperTolerance)
                        {
                            item.InToleranceLeft = "FAIL";
                        }

                        if (item.AsLeft < item.LowerTolerance)
                        {
                            item.InToleranceLeft = "FAIL";
                        }

                    
                    if (item?.Eccentricity != null)
                    {
                        item.Eccentricity = null;

                    }


                    if(item?.UnitOfMeasure?.BasicCalibrationResult != null)
                    {
                        item.UnitOfMeasure.BasicCalibrationResult = null;
                    }
                   

                    
                }


            }

            public static async Task calculateRepeatCalibrationResultValues(Entities.Repeatability repeatability,
                EquipmentTemplate equipmentTemplate, 
                PieceOfEquipment pieceOfEquipment, 
                WorkOrderDetail workOrderDetail)
            {
                var bc = repeatability.TestPointResult;
                double nominalTestPointValue = 0;


                var resolutionValue = workOrderDetail.Resolution;
                foreach (var item in bc)
                {
                   
                    nominalTestPointValue = item.WeightApplied;
                


                    var minTolerance = nominalTestPointValue - resolutionValue;
                    var maxTolerance = nominalTestPointValue + resolutionValue;

                    item.LowerTolerance = minTolerance;
                    item.UpperTolerance = maxTolerance;

                    item.InToleranceFound = "PASS";

                    if (item.AsFound > item.UpperTolerance)
                    {
                        item.InToleranceFound = "FAIL";
                    }

                    if (item.AsFound < item.LowerTolerance)
                    {
                        item.InToleranceFound = "FAIL";
                    }

                    item.InToleranceLeft = "PASS";

                    if (item.AsLeft > item.UpperTolerance)
                    {
                        item.InToleranceLeft = "FAIL";
                    }

                    if (item.AsLeft < item.LowerTolerance)
                    {
                        item.InToleranceLeft = "FAIL";
                    }

                    if(item?.Repeatability != null)
                    {
                        item.Repeatability = null;
                    }
                   if(item?.UnitOfMeasure?.BasicCalibrationResult != null)
                    {
                        item.UnitOfMeasure.BasicCalibrationResult = null;
                    }
                    

                    //foreach (var rep in item.Repeatability.TestPointResult)
                    //{

                    //    rep.Repeatability = null;
                    //}

                }

            }

            private static async Task calculateCalibrationResultValues(Linearity linearity, WorkOrderDetail workOrderDetail,IEnumerable<UnitOfMeasure> UoMs)
            {
                BasicCalibrationResult basicCalibrationResult = linearity.BasicCalibrationResult;

                if (basicCalibrationResult == null)
                {
                    return;
                }

                if(basicCalibrationResult.UnitOfMeasure== null && basicCalibrationResult.UnitOfMeasureID > 0)
                {
                    basicCalibrationResult.UnitOfMeasure = basicCalibrationResult.UnitOfMeasureID.GetUoM(UoMs);
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
                var uncertainty =  await GetReportUncertaintyBudget(linearity, linearity.SequenceID, workOrderDetail, workOrderDetail, null,UoMs);  //new Reports.Domain.ReportViewModels.UncertaintyViewModel();//
                double totalUncertainty = uncertainty.Totales.TotalUncerainty; // Math.Sqrt(sumOfSquares);
                double expandedUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(uncertainty.Totales.ExpandedUncertainty))); 



                basicCalibrationResult.FinalReadingStandard = nominalTestPointValue;
                linearity.TotalNominal = nominalTestPointValue;
                linearity.TotalActual = actualValue;
                linearity.SumUncertainty = calibrationUncertaintyValue;
                //linearity.Quotient =0 ;
                //linearity.Square = square;
                linearity.SumOfSquares = Convert.ToDouble(uncertainty.Totales.SumOfSquares);
                linearity.TotalUncertainty = totalUncertainty;
                linearity.ExpandedUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(uncertainty.Totales.ExpandedUncertainty))); 
                linearity.CalibrationUncertaintyType = calibrationUncertaintyType;
                linearity.CalibrationUncertaintyDistribution = calibrationUncertaintyDistribution;
                linearity.CalibrationUncertaintyDivisor = calibrationUncertaintyDivisor;
                linearity.UnitOfMeasureId = unitOfMeasureId;
                linearity.CalibrationUncertaintyValueUnitOfMeasureId = calibrationUncertaintyValueUnitOfMeasureId;
                linearity.BasicCalibrationResult.Uncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(uncertainty.Totales.ExpandedUncertainty))); ;
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
                /////////////Calculate resolution
                if(linearity?.TestPoint?.Resolution > 0)
                {
                    resolutionValue = linearity.TestPoint.Resolution;
                }
                else if (linearity?.TestPoint?.Resolution==0 && workOrderDetail.Tolerance != null && workOrderDetail.Tolerance.Resolution > 0)
                {
                    resolutionValue = workOrderDetail.Tolerance.Resolution;
                }
                else if (linearity?.TestPoint?.Resolution == 0 && workOrderDetail.Resolution > 0)
                {
                    resolutionValue = workOrderDetail.Resolution;
                }
                //  
                linearity.TestPoint.Resolution = resolutionValue;

                //itemm.TestPoint.Resolution = WorkOrderItemCreate.eq.Resolution;
                var resultdec = NumericExtensions.CalculateDecimalNumber(Convert.ToDecimal(resolutionValue));              

                linearity.TestPoint.DecimalNumber = resultdec;
                linearity.BasicCalibrationResult.Resolution = resolutionValue;


                ////////////////////////////////////////////////////////////////////////////////////////

                if (workOrderDetail.Tolerance.ToleranceTypeID == 2)
                {

                    tolerancePercentage = workOrderDetail.Tolerance.AccuracyPercentage;
                    //resolutionValue = linearity.BasicCalibrationResult.Resolution; //  workOrderDetail.Resolution;

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
                    //resolutionValue = workOrderDetail.Resolution;

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
                    //resolutionValue = workOrderDetail.Resolution;

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
                    double grads = nominalTestPointValue / resolutionValue;
                    int toleranceGrads = grads.GetGradsHB44(workOrderDetail.ClassHB44);
                    double toleranceValue = (double)toleranceGrads * resolutionValue;
                    if (workOrderDetail.IsComercial)
                    {
                        if (toleranceValue > resolutionValue)
                        {

                            linearity.MinTolerance = minTolerance - toleranceValue;

                            linearity.MaxTolerance = maxTolerance + toleranceValue;
                        }
                        else
                        {
                            linearity.MinTolerance = minTolerance - resolutionValue;
                            linearity.MaxTolerance = maxTolerance + resolutionValue;
                        }




                        int acceptanceToleranceGrads = toleranceGrads / 2;
                        double toleranceAcceptanceValue = (double)acceptanceToleranceGrads * resolutionValue;

                        if (toleranceAcceptanceValue > resolutionValue)
                        {

                            linearity.MinToleranceAsLeft = minTolerance - toleranceAcceptanceValue;
                            linearity.MaxToleranceAsLeft = maxTolerance + toleranceAcceptanceValue;
                        }
                        else
                        {
                            linearity.MinToleranceAsLeft = minTolerance - resolutionValue;
                            linearity.MaxToleranceAsLeft = maxTolerance + resolutionValue;
                        }
                    }
                    else
                    {
                        if (workOrderDetail.Multiplier == 0)
                        {
                            workOrderDetail.Multiplier = 1;
                        }
                        if ((toleranceValue * workOrderDetail.Multiplier) > resolutionValue)
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
                if (linearity?.TestPoint != null)
                {
                    linearity.TestPoint.LowerTolerance = linearity.MinTolerance;
                    linearity.TestPoint.UpperTolerance = linearity.MaxTolerance;
                }

            }


            public static async Task<Reports.Domain.ReportViewModels.UncertaintyViewModel> 
                GetReportUncertaintyBudget(Linearity li
                , int seq, WorkOrderDetail wodById, WorkOrderDetail workOrderDetail_
                //, List<PieceOfEquipment>? poes_
                , List<WeightSet>? weightSets_
                ,IEnumerable<UnitOfMeasure> UoMs)
            {

                WorkOrderDetail wo = new WorkOrderDetail();
                wo.WorkOrderDetailID = li.WorkOrderDetailId;
                //WorkOrderDetail workOrderDetail1 = new WorkOrderDetail();
                IEnumerable<PieceOfEquipment> poes = new List<PieceOfEquipment>();
                WorkOrderDetail workOrderDetail = new WorkOrderDetail();

                workOrderDetail = wodById;

                if (workOrderDetail?.BalanceAndScaleCalibration == null && workOrderDetail_ != null && workOrderDetail_.BalanceAndScaleCalibration != null)
                {
                    workOrderDetail.BalanceAndScaleCalibration = workOrderDetail_.BalanceAndScaleCalibration;
                }

                //if (workOrderDetail != null && workOrderDetail.BalanceAndScaleCalibration == null)
                //{
                //    // workOrderDetail = workOrderDetail_;
                //    workOrderDetail1 = await wodRepository.GetWorkOrderDetailByID(wo.WorkOrderDetailID);
                //    workOrderDetail.BalanceAndScaleCalibration = workOrderDetail1.BalanceAndScaleCalibration;
                //}


                var workOrder = workOrderDetail.WorkOder;


                var poe = workOrderDetail.PieceOfEquipment;

                //if (poes_ == null)
                //    //poes = await pieceOfEquipmentRepository.GetAllWeightSets(poe);
                //throw new Exception("method: GetReportUncertaintyBudget -- Weighset is null");
                //else
                //    poes = poes_;

                var eqTemp = poe.EquipmentTemplate;
                //List<WeightSet> weigthSets = new List<WeightSet>();

                //if (poes != null)
                //    weigthSets = poes.FirstOrDefault().WeightSets.ToList();

                string manufName;
                if (eqTemp.Manufacturer1 != null)
                {
                    manufName = eqTemp.Manufacturer1.Name;
                }
                else
                {
                    manufName = null;
                }

                ICollection<Linearity> _linearity = null;
                if (workOrderDetail.BalanceAndScaleCalibration != null && workOrderDetail.BalanceAndScaleCalibration.Linearities != null)
                    _linearity = workOrderDetail.BalanceAndScaleCalibration.Linearities;


                IEnumerable<BasicCalibrationResult> _repeatabilityBCR = null;
                IEnumerable<BasicCalibrationResult> _eccentricityBCR = null;

                if (workOrderDetail.BalanceAndScaleCalibration != null && workOrderDetail.BalanceAndScaleCalibration.Repeatability != null)
                {

                    _repeatabilityBCR = workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.Where(x => x.CalibrationSubTypeId == 2);
                }



                var _balance = workOrderDetail.BalanceAndScaleCalibration;

                if (workOrderDetail.BalanceAndScaleCalibration != null && workOrderDetail.BalanceAndScaleCalibration.Eccentricity != null)
                {
                    _eccentricityBCR = workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.Where(x => x.CalibrationSubTypeId == 3);

                }


                Reports.Domain.ReportViewModels.UncertaintyViewModel uncert = new Reports.Domain.ReportViewModels.UncertaintyViewModel()
                {
                    EquipmentTypeId = eqTemp.EquipmentTypeID.ToString(),
                    WorkOrderDetail = workOrderDetail.WorkOrderDetailID.ToString(),
                    CalibrationDate = workOrderDetail.CalibrationDate.Value.ToString("MM/dd/yyyy")
                };





                List<UncertaintyOfRerence> uncertaintyOfRerence = new List<UncertaintyOfRerence>();
                List<CornerloadEccentricity> cornerloadEccentricities = new List<CornerloadEccentricity>();
                List<RepeatabilityOfBalance> repeatabilityOfBalances = new List<RepeatabilityOfBalance>();
                EnvironmentalFactors environmentalFactors = new EnvironmentalFactors();
                DriftStability driftStability = new DriftStability();
                AirBuoyancy airBuoyancy = new AirBuoyancy();
                TemperatureEffect temperatureEffect = new TemperatureEffect();
                Resolution resolutions = new Resolution();
                List<UncertaintyMPE> uncertaintyMPE = new List<UncertaintyMPE>();
                Reports.Domain.ReportViewModels.Totales totales = new Reports.Domain.ReportViewModels.Totales();

                double totalSquare = 0;
                if (_linearity != null && _linearity?.Count > 0)
                {
                    var af = _linearity.Where(x => x.SequenceID == seq).FirstOrDefault();

                    //var wodWeigths = workOrderDetail.WOD_Weights.ToList();

                    if (af != null && af.WeightSets != null && af.WeightSets.Count() > 0)
                    {
                        var wodWeigths = af?.WeightSets?.Where(x => x.MPE == 0 || x.MPE == null).ToList();

                        ///YP // sum Variance
                        //var variances = wodWeigths
                        //.Select(wodWeight =>
                        //{

                        //    double magnitude = (double)RoundFirstSignificantDigit(Convert.ToDecimal(wodWeight.CalibrationUncertValue));
                        //    double divisor = wodWeight.Divisor;
                        //    double quotient = divisor != 0 ? magnitude / divisor : 0;
                        //    double square = Math.Pow(quotient, 2);
                        //    return square;
                        //})
                        //.ToList();

                        //double totalVariance = variances.Sum();

                        foreach (var wodWeight in wodWeigths)
                        {
                            if (wodWeight == null)
                            {
                                throw new Exception("method: GetReportUncertaintyBudget -- Weightset is null-- 1558");
                            }

                            WeightSet weightSet = wodWeight;

                            var aff = af.WeightSets.FirstOrDefault();

                            double magnitude = 0;
                            string weigth = "0";
                            double divisor = 0;
                            if (af.WeightSets != null && af.WeightSets.Count() > 0)
                            {
                                magnitude = (double)RoundFirstSignificantDigit(Convert.ToDecimal(weightSet.CalibrationUncertValue)); //Math.Round(weightSet.CalibrationUncertValue, 5);
                                UnitOfMeasure uom = new UnitOfMeasure();
                                uom.UnitOfMeasureID = weightSet.UnitOfMeasureID;
                                string Abbreviation = "";
                                if (uom.UnitOfMeasureID != 0 && string.IsNullOrEmpty(uom.Abbreviation))
                                {
                                    var uom11 = uom.UnitOfMeasureID.GetUoM(UoMs);// await uomRepository.GetByID(uom);
                                    if (uom11 != null)
                                    {
                                        uom = uom11;
                                        Abbreviation = uom11.Abbreviation;
                                    }

                                }
                                if (af?.TestPoint?.UnitOfMeasurementID > 0
                                    && (af?.TestPoint?.UnitOfMeasurement == null || string.IsNullOrEmpty(af?.TestPoint?.UnitOfMeasurement?.Abbreviation)))
                                {
                                    var uomaf = af.TestPoint.UnitOfMeasurementID.GetUoM(UoMs);// await uomRepository.GetByID(uom);

                                    if (uomaf != null)
                                    {
                                        af.TestPoint.UnitOfMeasurement = uomaf;
                                    }

                                }


                                if (string.IsNullOrEmpty(uom.Abbreviation))
                                {
                                    throw new Exception("method: GetReportUncertaintyBudget -- UoM data is not complete 1681 UoM ID:" + uom.UnitOfMeasureID);
                                }

                                weigth = weightSet.WeightActualValue.ToString() + " " + Abbreviation;
                                divisor = weightSet.Divisor;

                            }

                            double quotient = 0;
                            if (divisor != 0)
                                quotient = (double)RoundFirstSignificantDigit(Convert.ToDecimal(magnitude / divisor));
                            AppState appState = new AppState();
                            Dictionary<int, string> distributionUncertList = appState.DistributionUncertList;
                            List<WeightType> typeUncertList = appState.WeightTypeList;
                            int t = Convert.ToInt16(weightSet.Type);
                            var type_ = typeUncertList.Where(x => x.WeightTypeID == t).FirstOrDefault();
                            string type = "";
                            if (type_ != null)
                                type = type_.Name;

                            var distr = weightSet.Distribution.GetDistribution(distributionUncertList);

                            var Uncertainty = new UncertaintyOfRerence
                            {
                                Weigth = weigth,
                                Distribution = distr,
                                Divisor = divisor,
                                Magnitude = RoundFirstSignificantDigit(Convert.ToDecimal(magnitude)).ToString(),
                                Quotient = quotient,
                                Square = RoundFirstSignificantDigit(Convert.ToDecimal(Math.Pow(quotient, 2))).ToString(),
                                Type = type,
                                Units = af.TestPoint.UnitOfMeasurement.Abbreviation,
                                Contributor = "Calibration Uncertainty of Weights"

                            };
                            totalSquare = (double)RoundFirstSignificantDigit(Convert.ToDecimal(totalSquare + Convert.ToDouble(Uncertainty.Square)));

                            uncertaintyOfRerence.Add(Uncertainty);


                            Uncertainty.Variance = Uncertainty.Square.ToString();
                            Uncertainty.Degrees = "100"; //TODO
                            Uncertainty.df = RoundFirstSignificantDigit(Convert.ToDecimal(Math.Pow(Uncertainty.Quotient.GetValueOrDefault(), 4) / Convert.ToDouble(Uncertainty.Degrees))).ToString();
                           // Uncertainty.PercentContribution = RoundFirstSignificantDigit(Convert.ToDecimal(Convert.ToDouble(Uncertainty.Variance) / totalVariance)).ToString();
                            Uncertainty.Comments = wodWeight.Note;
                        }

                    }
                    // Anab 10049
                    //
                    if (workOrderDetail.CertificationID == 2 && af != null && af.WeightSets != null && af.WeightSets.Count() > 0)
                    {
                        
                        var wodWeigthsMpe = af?.WeightSets?.Where(x => x.MPE != null && x.MPE != 0).ToList();
                        foreach (var item in wodWeigthsMpe)
                        {

                            if (item == null)
                            {
                                throw new Exception("method: GetReportUncertaintyBudget -- Weightset is null-- 1558");
                            }

                            WeightSet weightSet = item;

                            var aff = af.WeightSets.FirstOrDefault();

                            double magnitude = 0;
                            string weigth = "0";
                            double divisor = 0;
                            if (af.WeightSets != null && af.WeightSets.Count() > 0)
                            {
                                magnitude = (double)RoundFirstSignificantDigit(Convert.ToDecimal(weightSet.MPE));
                                UnitOfMeasure uom = new UnitOfMeasure();
                                uom.UnitOfMeasureID = weightSet.UnitOfMeasureID;
                                string Abbreviation = "";
                                if (uom.UnitOfMeasureID != 0 && string.IsNullOrEmpty(uom.Abbreviation))
                                {
                                    var uom11 = uom.UnitOfMeasureID.GetUoM(UoMs);
                                    if (uom11 != null)
                                    {
                                        uom = uom11;
                                        Abbreviation = uom11.Abbreviation;
                                    }

                                }
                                if (af?.TestPoint?.UnitOfMeasurementID > 0
                                    && (af?.TestPoint?.UnitOfMeasurement == null || string.IsNullOrEmpty(af?.TestPoint?.UnitOfMeasurement?.Abbreviation)))
                                {
                                    var uomaf = af.TestPoint.UnitOfMeasurementID.GetUoM(UoMs);

                                    if (uomaf != null)
                                    {
                                        af.TestPoint.UnitOfMeasurement = uomaf;
                                    }

                                }


                                if (string.IsNullOrEmpty(uom.Abbreviation))
                                {
                                    throw new Exception("method: GetReportUncertaintyBudget -- UoM data is not complete 1681 UoM ID:" + uom.UnitOfMeasureID);
                                }

                                weigth = weightSet.WeightActualValue.ToString() + " " + Abbreviation;
                                divisor = weightSet.Divisor;

                            }

                            double quotient = 0;
                            if (divisor != 0)
                                quotient = (double)RoundFirstSignificantDigit(Convert.ToDecimal(magnitude / divisor));
                            AppState appState = new AppState();
                            Dictionary<int, string> distributionUncertList = appState.DistributionUncertList;
                            List<WeightType> typeUncertList = appState.WeightTypeList;
                            int t = Convert.ToInt16(weightSet.Type);
                            var type_ = typeUncertList.Where(x => x.WeightTypeID == t).FirstOrDefault();
                            string type = "";
                            if (type_ != null)
                                type = type_.Name;

                            var distr = weightSet.Distribution.GetDistribution(distributionUncertList);

                            var Uncertainty = new UncertaintyMPE
                            {
                                Weigth = weigth,
                                Distribution = distr,
                                Divisor = divisor,
                                Magnitude = RoundFirstSignificantDigit(Convert.ToDecimal(magnitude)).ToString(),
                                Quotient = quotient,
                                Square = RoundFirstSignificantDigit(Convert.ToDecimal(Math.Pow(quotient, 2))).ToString(),
                                Type = type,
                                Units = af.TestPoint.UnitOfMeasurement.Abbreviation,
                                Contributor = "Max Permissible Error of Weights"

                            };
                            totalSquare = (double)RoundFirstSignificantDigit(Convert.ToDecimal(totalSquare + Convert.ToDouble(Uncertainty.Square)));

                            uncertaintyMPE.Add(Uncertainty);



                        }

                        #region DriftStability
                        var magnitudeDrift = "0.0000";
                        var divisorDrift = 1.73;
                        var quotientDrift = (double)RoundFirstSignificantDigit(Convert.ToDecimal(Convert.ToDouble(magnitudeDrift) / divisorDrift));
                        var Drift = new DriftStability
                        {

                            Distribution = "Rectangular",
                            Divisor = divisorDrift,
                            Magnitude = magnitudeDrift,
                            Quotient = quotientDrift,
                            Square = RoundFirstSignificantDigit(Convert.ToDecimal(Math.Pow(quotientDrift, 2))).ToString(),
                            Type = "B",
                            Units = "lb",
                            Contributor = "Drift/Stability of Weights"
                        };
                        totalSquare = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(totalSquare + Convert.ToDouble(Drift.Square))));
                        driftStability = Drift;
                        #endregion

                        #region AirBuoyancy
                        var magnitudeAir = "0.0127";
                        var divisorAir = 1.73;
                        var quotientAir = (double)RoundFirstSignificantDigit(Convert.ToDecimal(Convert.ToDouble(magnitudeAir) / divisorAir));
                        var Air = new AirBuoyancy
                        {

                            Distribution = "Rectangular",
                            Divisor = divisorAir,
                            Magnitude = magnitudeAir,
                            Quotient = quotientAir,
                            Square = RoundFirstSignificantDigit(Convert.ToDecimal(Math.Pow(quotientAir, 2))).ToString(),
                            Type = "B",
                            Units = "lb",
                            Contributor = "Air Buoyancy"
                        };
                        totalSquare = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(totalSquare + Convert.ToDouble(Air.Square))));
                        airBuoyancy = Air;
                        #endregion
                        #region TemperatureEffect
                        var magnitudeTemp = "0.0000";
                        var divisorTemp = 1.73;
                        var quotientTemp = (double)RoundFirstSignificantDigit(Convert.ToDecimal(Convert.ToDouble(magnitudeTemp) / divisorTemp));
                        var TempEffect = new TemperatureEffect
                        {

                            Distribution = "Rectangular",
                            Divisor = divisorAir,
                            Magnitude = magnitudeAir,
                            Quotient = quotientAir,
                            Square =RoundFirstSignificantDigit(Convert.ToDecimal(Math.Pow(quotientTemp, 2))).ToString(),
                            Type = "B",
                            Units = "lb",
                            Contributor = "Temperature Effect"
                        };
                        totalSquare = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(totalSquare + Convert.ToDouble(TempEffect.Square))));
                        temperatureEffect = TempEffect;
                        #endregion
                    }


                }

                if (_repeatabilityBCR != null)
                {
                    //stdev
                    var med = _repeatabilityBCR.Sum(x => x.AsLeft) / _repeatabilityBCR.Count();
                    var distMed = _repeatabilityBCR.Sum(x => Math.Pow((x.AsLeft - med), 2));
                    var stdev = Math.Sqrt(distMed / _repeatabilityBCR.Count());


                    if (_repeatabilityBCR != null && _repeatabilityBCR.Count() > 0)
                    {
                        var rowseq = _repeatabilityBCR.FirstOrDefault();
                        var magnituderep = (double)RoundFirstSignificantDigit(Convert.ToDecimal(stdev));

                        var divisorrep = 1;
                        var quotientrep = (double)RoundFirstSignificantDigit(Convert.ToDecimal(magnituderep / divisorrep));

                        string unitAbreviation = "";
                        string weigth = "";
                        if (rowseq != null)
                        {
                            unitAbreviation = rowseq.UnitOfMeasureID.GetUoM(UoMs).Abbreviation;
                            weigth = rowseq.WeightApplied.ToString() + @"\" + unitAbreviation;
                        }
                        else
                        {
                            unitAbreviation = "NA";
                            weigth = "0";
                        }
                        var rep = new RepeatabilityOfBalance
                        {
                            Weigth = weigth,
                            Distribution = "Normal",
                            Divisor = divisorrep,
                            Magnitude = RoundFirstSignificantDigit(Convert.ToDecimal(magnituderep)).ToString(),
                            Quotient = quotientrep,
                            Square = RoundFirstSignificantDigit(Convert.ToDecimal(Math.Pow(quotientrep, 2))).ToString(),
                            Type = "A",
                            Units = unitAbreviation,
                            Contributor = "Repeatability & Reproducibility"
                        };
                        totalSquare = (double)RoundFirstSignificantDigit(Convert.ToDecimal(totalSquare + Convert.ToDouble(rep.Square)));
                        repeatabilityOfBalances.Add(rep);

                    }
                }

             


                if (_eccentricityBCR != null)
                {


                    if (_eccentricityBCR != null && _eccentricityBCR.Count() > 0)
                    {
                        var rowseq = _eccentricityBCR.FirstOrDefault();
                        var magnitudeecc = (double)RoundFirstSignificantDigit(Convert.ToDecimal(_eccentricityBCR.Max(x => x.AsLeft) - _eccentricityBCR.Min(x => x.AsLeft))); //Math.Round(_eccentricityBCR.Max(x => x.AsLeft) - _eccentricityBCR.Min(x => x.AsLeft), 8);
                        var divisorecc = 1.73;
                        var quotientecc = (double)RoundFirstSignificantDigit(Convert.ToDecimal(magnitudeecc / divisorecc)); // Math.Round(magnitudeecc / divisorecc, 8);
                        string unitAbreviation = "";
                        if (rowseq != null && rowseq.UnitOfMeasureID > 0)
                        {
                            unitAbreviation = rowseq.UnitOfMeasureID.GetUoM(UoMs).Abbreviation;
                        }
                        else
                        {
                            unitAbreviation = "NA";
                        }



                        var cornerload = new CornerloadEccentricity
                        {
                            Weigth = "0",
                            Distribution = "Rectangular",
                            Divisor = divisorecc,
                            Magnitude = RoundFirstSignificantDigit(Convert.ToDecimal(magnitudeecc)).ToString(),
                            Quotient = quotientecc,
                            Square =RoundFirstSignificantDigit(Convert.ToDecimal(Math.Pow(quotientecc, 2))).ToString()  ,
                            Type = "A",
                            Units = unitAbreviation

                        };
                        totalSquare = (double)RoundFirstSignificantDigit(Convert.ToDecimal(totalSquare + Convert.ToDouble(cornerload.Square))); 
                        cornerloadEccentricities.Add(cornerload);

                    }
                }

               

                var magnitudeEnv = "0.004";
                var divisorEnv = 1.73;
                var quotientEnv = (double)RoundFirstSignificantDigit(Convert.ToDecimal(Convert.ToDouble(magnitudeEnv) / divisorEnv)); 
                var env = new EnvironmentalFactors
                {

                    Distribution = "Rectangular",
                    Divisor = divisorEnv,
                    Magnitude = magnitudeEnv,
                    Quotient = quotientEnv,
                    Square = RoundFirstSignificantDigit(Convert.ToDecimal(Math.Pow(quotientEnv, 2))).ToString(),
                
                    Type = "B",
                    Units = "g"

                };
                totalSquare = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(totalSquare + Convert.ToDouble(env.Square)))); 


                var magnitudeRes = workOrderDetail.Resolution;
                var divisorRes = 3.46;
                var quotientRes = (double)RoundFirstSignificantDigit(Convert.ToDecimal(Convert.ToDouble(magnitudeRes) / divisorRes));
                var res = new Resolution
                {
                    Distribution = "Resolution",
                    Divisor = divisorRes,
                    Magnitude = RoundFirstSignificantDigit(Convert.ToDecimal(Convert.ToDouble(magnitudeRes))).ToString(),  /*Math.Round(magnitudeRes, 2).ToString(),*/
                    Quotient = (double)RoundFirstSignificantDigit(Convert.ToDecimal(Convert.ToDouble(quotientRes))), //Math.Round(quotientRes, 2),
                    Square = RoundFirstSignificantDigit(Convert.ToDecimal(Convert.ToDouble(Math.Pow(quotientRes, 2)))).ToString(),
                    Type = "B",
                    Units = "g",
                    Contributor = "Resolution at Load"

                };
                totalSquare =  ((double)RoundFirstSignificantDigit(Convert.ToDecimal(totalSquare + Convert.ToDouble(res.Square))));

                totales.SumOfSquares = RoundFirstSignificantDigit(Convert.ToDecimal(totalSquare)).ToString();
                totales.StdUnc = Math.Round(Math.Sqrt(Convert.ToDouble(totales.SumOfSquares)), 3);
                totales.Veff = 2;//(int)(Math.Pow(totales.StdUnc, 4) / J23)
                totales.TotalUncerainty = Convert.ToDouble(RoundFirstSignificantDigit(Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(totales.SumOfSquares)))));
                totales.ExpandedUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(totales.TotalUncerainty * totales.TotalUncerainty)));
               

                uncert.CornerloadEccentricities = cornerloadEccentricities;
                uncert.RepeatabilityOfBalances = repeatabilityOfBalances;
                uncert.EnvironmentalFactors = env;
                uncert.Resolutions = res;
                uncert.Totales = totales;
                uncert.UncertaintyOfRerences = uncertaintyOfRerence;
                if (workOrderDetail.CertificationID == 2)
                {
                    uncert.UncertaintyMPEs = uncertaintyMPE;
                    uncert.DriftStabilities = driftStability;
                    uncert.AirBuoyancies = airBuoyancy;
                    uncert.TemperatureEffects = temperatureEffect;
                }
                return uncert;
            
            

            }


            //private static void calculateCalibrationResultValues(Linearity linearity, EquipmentTemplate equipmentTemplate, PieceOfEquipment pieceOfEquipment, WorkOrderDetail workOrderDetail )
            //{
            //    BasicCalibrationResult basicCalibrationResult = linearity.BasicCalibrationResult;

            //    if (basicCalibrationResult == null)
            //    {
            //        return;
            //    }


            //    UnitOfMeasure testPointUnitOfMeasure = basicCalibrationResult.UnitOfMeasure;


            //    var calibrationResultWeights = linearity.WeightSets;

            //    double actualValue = 0;
            //    double nominalTestPointValue = 0;
            //    var test = nominalTestPointValue + 5;
            //    double calibrationUncertaintyValue = 0;
            //    int? unitOfMeasureId = null;
            //    int? calibrationUncertaintyValueUnitOfMeasureId = null;
            //    String calibrationUncertaintyType = "";
            //    String calibrationUncertaintyDistribution = "";
            //    double calibrationUncertaintyDivisor = 1;

            //    double manualNominal = linearity.BasicCalibrationResult.WeightApplied;

            //    if (calibrationResultWeights != null && calibrationResultWeights.Count > 0)
            //    {
            //        foreach (var calibrationResultWeight in calibrationResultWeights)
            //        {

            //            if (basicCalibrationResult.UnitOfMeasure != null && calibrationResultWeight.UnitOfMeasure !=null && calibrationResultWeight.UnitOfMeasureID != basicCalibrationResult.UnitOfMeasureID)
            //            {
            //                actualValue += calibrationResultWeight.UnitOfMeasure.ConversionValue.ConvertToUOM(calibrationResultWeight.WeightActualValue, basicCalibrationResult.UnitOfMeasure);
            //            }
            //            else
            //            {
            //                if (calibrationResultWeight.WeightActualValue == 0)
            //                {
            //                    throw new Exception(WODMessages.CalculateWeightSetConfiguration);
            //                }

            //                actualValue += calibrationResultWeight.WeightActualValue;
            //            }

            //            if (basicCalibrationResult.UnitOfMeasure != null &&  calibrationResultWeight.UnitOfMeasure != null && calibrationResultWeight.UnitOfMeasureID != basicCalibrationResult.UnitOfMeasureID)
            //            {
            //                nominalTestPointValue += calibrationResultWeight.UnitOfMeasure.ConversionValue.ConvertToUOM(calibrationResultWeight.WeightNominalValue, basicCalibrationResult.UnitOfMeasure);
            //            }
            //            else
            //            {
            //                nominalTestPointValue += calibrationResultWeight.WeightNominalValue;
            //            }


            //            if (testPointUnitOfMeasure != null && testPointUnitOfMeasure.UncertaintyUnitOfMeasure != null)
            //            {
            //                if (calibrationResultWeight.UncertaintyUnitOfMeasure != null && calibrationResultWeight.UncertaintyUnitOfMeasureId != testPointUnitOfMeasure.UncertaintyUnitOfMeasureID)
            //                {
            //                    calibrationUncertaintyValue += calibrationResultWeight.UncertaintyUnitOfMeasure.ConversionValue.ConvertToUOM(calibrationResultWeight.CalibrationUncertValue, testPointUnitOfMeasure.UncertaintyUnitOfMeasure);

            //                    calibrationResultWeight.CalibrationUncertValue = calibrationResultWeight.UncertaintyUnitOfMeasure.ConversionValue.ConvertToUOM(calibrationResultWeight.CalibrationUncertValue, testPointUnitOfMeasure.UncertaintyUnitOfMeasure);
            //                    calibrationResultWeight.UncertaintyUnitOfMeasureId = testPointUnitOfMeasure.UncertaintyUnitOfMeasureID;
            //                    calibrationResultWeight.UncertaintyUnitOfMeasure = testPointUnitOfMeasure.UncertaintyUnitOfMeasure;
            //                }
            //                else
            //                {
            //                    calibrationUncertaintyValue += calibrationResultWeight.CalibrationUncertValue;
            //                  }
            //            }
            //            else
            //            {

            //                if (testPointUnitOfMeasure != null && calibrationResultWeight.UncertaintyUnitOfMeasure != null)
            //                {
            //                    calibrationUncertaintyValue += calibrationResultWeight.UncertaintyUnitOfMeasure.ConversionValue.ConvertToUOM(calibrationResultWeight.CalibrationUncertValue, testPointUnitOfMeasure);
            //                    calibrationResultWeight.CalibrationUncertValue = calibrationResultWeight.UncertaintyUnitOfMeasure.ConversionValue.ConvertToUOM(calibrationResultWeight.CalibrationUncertValue, testPointUnitOfMeasure);
            //                    calibrationResultWeight.UncertaintyUnitOfMeasureId = testPointUnitOfMeasure.UnitOfMeasureID;
            //                    calibrationResultWeight.UncertaintyUnitOfMeasure = testPointUnitOfMeasure;
            //                }
            //                else
            //                {
            //                    calibrationUncertaintyValue += calibrationResultWeight.CalibrationUncertValue;

            //                }
            //            }


            //            calibrationUncertaintyType = calibrationResultWeight.Type;

            //            calibrationUncertaintyDistribution = calibrationResultWeight.Distribution;


            //            if (calibrationResultWeight.Divisor == 0)
            //            {
            //                throw new Exception(WODMessages.CalculateWeightSetConfiguration + " Used weight divisor value in 0");
            //            }


            //            calibrationUncertaintyDivisor = calibrationResultWeight.Divisor;

            //            unitOfMeasureId = basicCalibrationResult.UnitOfMeasureID;

            //            if (testPointUnitOfMeasure != null && testPointUnitOfMeasure.UncertaintyUnitOfMeasure != null)
            //            {
            //                calibrationUncertaintyValueUnitOfMeasureId = testPointUnitOfMeasure.UncertaintyUnitOfMeasureID.Value;
            //            }
            //            else
            //            {
            //                calibrationUncertaintyValueUnitOfMeasureId = basicCalibrationResult.UnitOfMeasure.UnitOfMeasureID;
            //            }
            //        }
            //    }

            //    if(nominalTestPointValue == 0 && manualNominal > 0)
            //    {

            //        nominalTestPointValue = manualNominal;
            //    }


            //    linearity.BalanceAndScaleCalibration.ResolutionUncertaintyDivisor = 1;

            //    linearity.BalanceAndScaleCalibration.EnvironmentalUncertaintyDivisor = 1;


            //    if (calibrationUncertaintyDivisor == 0)
            //    {
            //        throw new Exception(WODMessages.CalculateWeightSetConfiguration);
            //    }

            //    double quotient = calibrationUncertaintyValue / calibrationUncertaintyDivisor;
            //    double square = quotient * quotient;


            //    double eccentricitySquare = 0;

            //    if(linearity?.BalanceAndScaleCalibration?.Eccentricity != null)
            //    eccentricitySquare = linearity.BalanceAndScaleCalibration.Eccentricity.EccentricitySquare;


            //    double repeatabilitySquare = 0;

            //    if(linearity?.BalanceAndScaleCalibration?.Repeatability!= null)
            //    repeatabilitySquare = linearity.BalanceAndScaleCalibration.Repeatability.RepeatabilitySquare;


            //    double resolutionSquare = 0;
            //    resolutionSquare = linearity.BalanceAndScaleCalibration.ResolutionSquare;


            //    double environmentalSquare = 0;
            //    environmentalSquare = linearity.BalanceAndScaleCalibration.EnvironmentalSquare;

            //    double sumOfSquares = square + eccentricitySquare + repeatabilitySquare + resolutionSquare + environmentalSquare;


            //    ///Uncertainty Calculate
            //    ///Linearity li, int seq, WorkOrderDetail wod, List<PieceOfEquipment>? poes
            //   // var uncertainty = GetReportUncertaintyBudget(linearity, linearity.SequenceID, workOrderDetail, poes_);
            //    double totalUncertainty = Math.Sqrt(sumOfSquares);
            //    double expandedUncertainty = totalUncertainty * 2;



            //    basicCalibrationResult.FinalReadingStandard = nominalTestPointValue;
            //    linearity.TotalNominal = nominalTestPointValue;
            //    linearity.TotalActual = actualValue;
            //    linearity.SumUncertainty = calibrationUncertaintyValue;
            //    linearity.Quotient = quotient;
            //    linearity.Square = square;
            //    linearity.SumOfSquares = sumOfSquares;
            //    linearity.TotalUncertainty = totalUncertainty;
            //    linearity.ExpandedUncertainty = expandedUncertainty;
            //    linearity.CalibrationUncertaintyType = calibrationUncertaintyType;
            //    linearity.CalibrationUncertaintyDistribution = calibrationUncertaintyDistribution;
            //    linearity.CalibrationUncertaintyDivisor = calibrationUncertaintyDivisor;
            //    linearity.UnitOfMeasureId = unitOfMeasureId;
            //    linearity.CalibrationUncertaintyValueUnitOfMeasureId = calibrationUncertaintyValueUnitOfMeasureId;

            //    double tolerancePercentage = 0;
            //    double resolutionValue = 0;

            //    if (linearity.TestPoint != null) 
            //        nominalTestPointValue = linearity.TestPoint.NominalTestPoit;
            //    double minTolerance = nominalTestPointValue;
            //    double maxTolerance = nominalTestPointValue;


            //    if (workOrderDetail.Tolerance.ToleranceTypeID == 0)
            //    {
            //        throw new Exception("Tolerance Type Not Found");
            //    }

            //    if (workOrderDetail.Ranges == null)
            //    {
            //        workOrderDetail.Ranges = new List<RangeTolerance>();
            //    }


            //    if (workOrderDetail.Tolerance.ToleranceTypeID == 2)
            //    {

            //        tolerancePercentage = workOrderDetail.Tolerance.AccuracyPercentage;
            //        resolutionValue = workOrderDetail.Resolution;

            //        var tolerance = workOrderDetail.Ranges.Where(x => x.ToleranceTypeID == 2 && x.MinValue <= nominalTestPointValue && nominalTestPointValue >= x.MaxValue).FirstOrDefault();

            //        if (tolerance != null)
            //        {
            //            tolerancePercentage = tolerance.Percent;
            //        }

            //        var resolution = workOrderDetail.Ranges.Where(x => x.ToleranceTypeID == 1 && x.MinValue <= nominalTestPointValue && nominalTestPointValue >= x.MaxValue).FirstOrDefault();

            //        if (resolution != null)
            //        {
            //            resolutionValue = resolution.Resolution;
            //        }

            //        if ((nominalTestPointValue * ((tolerancePercentage) / 100)) > resolutionValue)
            //        {
            //            minTolerance = nominalTestPointValue - (nominalTestPointValue * ((tolerancePercentage) / 100));
            //            maxTolerance = nominalTestPointValue + (nominalTestPointValue * ((tolerancePercentage) / 100));
            //        }
            //        else
            //        {
            //            minTolerance = nominalTestPointValue - resolutionValue;
            //            maxTolerance = nominalTestPointValue + resolutionValue;
            //        }

            //        linearity.MinTolerance = minTolerance;

            //        linearity.MaxTolerance = maxTolerance;
            //    }

            //    if (workOrderDetail.Tolerance.ToleranceTypeID == 4)
            //    {

            //        tolerancePercentage = workOrderDetail.Tolerance.AccuracyPercentage;
            //        resolutionValue = workOrderDetail.Resolution;

            //        var tolerance = workOrderDetail.Ranges.Where(x => x.ToleranceTypeID == 4 && x.MinValue <= nominalTestPointValue && nominalTestPointValue >= x.MaxValue).FirstOrDefault();

            //        if (tolerance != null)
            //        {
            //            tolerancePercentage = tolerance.Percent;
            //        }

            //            minTolerance = nominalTestPointValue - (nominalTestPointValue * ((tolerancePercentage) / 100));
            //            maxTolerance = nominalTestPointValue + (nominalTestPointValue * ((tolerancePercentage) / 100));


            //        linearity.MinTolerance = minTolerance;

            //        linearity.MaxTolerance = maxTolerance;
            //    }



            //    if (workOrderDetail.Tolerance.ToleranceTypeID == 1)
            //    {
            //        resolutionValue = workOrderDetail.Resolution;

            //        var resolution = workOrderDetail.Ranges.Where(x => x.ToleranceTypeID == 1 && x.MinValue <= nominalTestPointValue && nominalTestPointValue >= x.MaxValue).FirstOrDefault();

            //        if (resolution != null)
            //        {
            //            resolutionValue = resolution.Resolution;
            //        }

            //        minTolerance = minTolerance - resolutionValue;
            //        maxTolerance = maxTolerance + resolutionValue;

            //        linearity.MinTolerance = minTolerance;

            //        linearity.MaxTolerance = maxTolerance;
            //    }


            //    if (workOrderDetail.Tolerance.ToleranceTypeID == 3)
            //    {
            //          double grads = nominalTestPointValue / workOrderDetail.Resolution;
            //        int toleranceGrads = grads.GetGradsHB44( workOrderDetail.ClassHB44);
            //        double toleranceValue = (double)toleranceGrads * workOrderDetail.Resolution;
            //        if (workOrderDetail.IsComercial)
            //        {
            //            if (toleranceValue > workOrderDetail.Resolution)
            //            {

            //                linearity.MinTolerance = minTolerance - toleranceValue;

            //                linearity.MaxTolerance = maxTolerance + toleranceValue;
            //            }
            //            else
            //            {
            //                linearity.MinTolerance = minTolerance - workOrderDetail.Resolution;
            //                linearity.MaxTolerance = maxTolerance + workOrderDetail.Resolution;
            //            }




            //            int acceptanceToleranceGrads = toleranceGrads / 2;
            //          double toleranceAcceptanceValue = (double)acceptanceToleranceGrads * workOrderDetail.Resolution;

            //            if (toleranceAcceptanceValue > workOrderDetail.Resolution)
            //            {
            //                linearity.MinToleranceAsLeft = minTolerance - toleranceAcceptanceValue;
            //                linearity.MaxToleranceAsLeft = maxTolerance + toleranceAcceptanceValue;
            //            }
            //            else
            //            {
            //                linearity.MinToleranceAsLeft = minTolerance - workOrderDetail.Resolution;
            //                linearity.MaxToleranceAsLeft = maxTolerance + workOrderDetail.Resolution;
            //            }
            //        }
            //        else
            //        {
            //            if(workOrderDetail.Multiplier == 0)
            //            {
            //                workOrderDetail.Multiplier = 1;
            //            }
            //            if ((toleranceValue * workOrderDetail.Multiplier) > workOrderDetail.Resolution)
            //            {

            //                linearity.MinTolerance = minTolerance - (toleranceValue * workOrderDetail.Multiplier);

            //                linearity.MaxTolerance = maxTolerance + (toleranceValue * workOrderDetail.Multiplier);
            //            }
            //            else
            //            {
            //                linearity.MinTolerance = minTolerance - workOrderDetail.Resolution;
            //                linearity.MaxTolerance = maxTolerance + workOrderDetail.Resolution;
            //            }

            //            ;
            //        }
            //    }                             

            //    double asFound = basicCalibrationResult.AsFound;
            //    if (asFound >= linearity.MinTolerance && asFound <= linearity.MaxTolerance)
            //    {
            //        basicCalibrationResult.InToleranceFound = "Pass";
            //    }
            //    else
            //    {
            //        basicCalibrationResult.InToleranceFound = "Fail";
            //    }

            //    double asLeft = basicCalibrationResult.AsLeft;
            //    if (workOrderDetail.Tolerance.ToleranceTypeID == 3 && workOrderDetail.IsComercial)
            //    {
            //        if (asLeft >= linearity.MinToleranceAsLeft && asLeft <= linearity.MaxToleranceAsLeft)
            //        {
            //            basicCalibrationResult.InToleranceLeft = "Pass";
            //        }
            //        else
            //        {
            //            basicCalibrationResult.InToleranceLeft = "Fail";
            //        }
            //    }
            //    else
            //    {
            //        if (asLeft >= linearity.MinTolerance && asLeft <= linearity.MaxTolerance)
            //        {
            //            basicCalibrationResult.InToleranceLeft = "Pass";
            //        }
            //        else
            //        {
            //            basicCalibrationResult.InToleranceLeft = "Fail";
            //        }
            //    }

            //    basicCalibrationResult.Linearity = null;
            //    basicCalibrationResult.UnitOfMeasure.BasicCalibrationResult = null;

            //}



            public static decimal CalculateDecimalDigitsAmmount(decimal value)
            {

                int[] bits = Decimal.GetBits(value);
                ulong lowInt = (uint)bits[0];
                ulong midInt = (uint)bits[1];
                int exponent = (bits[3] & 0x00FF0000) >> 16;
                int result = exponent;
                ulong lowDecimal = lowInt | (midInt << 32);
                while (result > 0 && (lowDecimal % 10) == 0)
                {
                    result--;
                    lowDecimal /= 10;
                }

                return result;

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

            public static double Stdev(double[] list)
            {

                if (list?.Length == 0)
                {
                    return 0;
                }

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


            public static Status GetNextStatus(WorkOrderDetail DTO,IEnumerable<Status> s)
            {

                 if (DTO.CurrentStatus != null && DTO.CurrentStatusID==0)
                {
                    DTO.CurrentStatusID = DTO.CurrentStatus.StatusId;
                }


                Status nextStatus = new Status();
                if (DTO.SelectedNewStatus == -1)
                {

                    var c = s.Where(x => x.StatusId == DTO.CurrentStatusID).FirstOrDefault();

                    if (c != null && !string.IsNullOrEmpty(c.Possibilities))
                    {
                        var a = c.Possibilities.Split(';');


                        if (a != null && a.Length > 0)
                        {
                            int next = 0;
                            foreach (var t in a)
                            {
                                if (Convert.ToInt32(t) > c.StatusId)
                                {
                                    next = Convert.ToInt32(t);
                                    break;
                                }
                            }

                            if (next > 0)
                            {
                                nextStatus = s.ToList().Where(x => x.StatusId == next).FirstOrDefault();
                            }
                            else
                            {
                                nextStatus = c;
                            }
                        }
                    }
                }

                else
                {

                    var ci = s.Where(x => x.StatusId == DTO.SelectedNewStatus).FirstOrDefault();

                    nextStatus = ci;

                }

                if (nextStatus.StatusId == 0 && DTO.CurrentStatusID !=3)
                {
                    throw new Exception("Next Status not Found");
                }

                return nextStatus;
            }

            public static List<string> ValidateWODList(Status NextStatus, ref WorkOrderDetail DTO, bool Calculate = false)
            {
                List<string> listValidation = new List<string>();
                if (NextStatus.StatusId == 2)
                {
                    


                    if (DTO.AddressID == null || DTO.AddressID == 0)
                    {

                        listValidation.Add(Querys.WOD.WODMessages.Localization);
                    }

                    //if (DTO.Tolerance.ToleranceTypeID == 0 && DTO.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject.HasTolerance )
                    //{
                    //    listValidation.Add(Querys.WOD.WODMessages.Tolerance);

                    //}


                    if (DTO.SelectSingle(Querys.ValidateAccuracyPercent()) && DTO.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject.HasTolerance)
                    {
                        listValidation.Add(Querys.WOD.WODMessages.Tolerance);

                    }

                    if (DTO.SelectSingle(Querys.ValidateToleranceAccuracy()) && DTO.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject.HasTolerance)
                    
                    {
                        listValidation.Add(Querys.WOD.WODMessages.Tolerance);

                    }

                     if (DTO.CalibrationIntervalID == 0)
                    {
                        listValidation.Add(Querys.WOD.WODMessages.Interval);
                      
                    }

                    if (!DTO.CalibrationDate.HasValue || DTO.CalibrationDate.Value.Year <= 2000)
                    {
                        listValidation.Add(Querys.WOD.WODMessages.Dates);
                      
                    }

                    if (!DTO.CalibrationDate.HasValue || DTO.CalibrationDate.Value.Year < 2000)
                    {
                        listValidation.Add(Querys.WOD.WODMessages.Dates);
                      
                    }

                     if (DTO.CalibrationTypeID==2 && !DTO.CertificationID.HasValue )
                    {
                        listValidation.Add(Querys.WOD.WODMessages.Certification);
                      
                    }


                }

                var acc = DTO.IsAccredited.HasValue && DTO.IsAccredited.Value;

                if ((NextStatus.StatusId == 3 || Calculate) )
                {

                    if (DTO.TechnicianID == null || !DTO.TechnicianID.HasValue || DTO.TechnicianID.Value == 0)
                    {
                      
                        listValidation.Add(Querys.WOD.WODMessages.Technician);
                    }

                    //if (DTO.Temperature == 0 || DTO.TemperatureUOMID == 0)
                    //{
                    //    listValidation.Add(Querys.WOD.WODMessages.Temperature);
                        
                    //}
                    //if ((DTO.Humidity == 0 || DTO.HumidityUOMID == 0) )
                    //{
                    //    listValidation.Add(Querys.WOD.WODMessages.Humidity);
                        
                    //}

                    //if (string.IsNullOrEmpty(DTO.Environment)  )
                    //{
                    //    listValidation.Add(Querys.WOD.WODMessages.Enviroment);
                       
                    //}

                   
                   
                    if (DTO.EquipmentCondition == null)
                    {
                        listValidation.Add(Querys.WOD.WODMessages.Condition);                    


                    }

                    if (!DTO.IsAccredited.HasValue)
                    {
                        listValidation.Add(Querys.WOD.WODMessages.Accredited);
                    }


                    if (DTO.IsAccredited == true
                          && (DTO.WOD_Weights == null || DTO.WOD_Weights.Count == 0)
                          && !(DTO.BalanceAndScaleCalibration.TestPointResult.FirstOrDefault()?.CalibrationSubTypeId >= 507
                               && DTO.BalanceAndScaleCalibration.TestPointResult.FirstOrDefault()?.CalibrationSubTypeId <= 518)
                          && (DTO.CalibrationType.IsNivel1 == false))
                    {
                        listValidation.Add(Querys.WOD.WODMessages.WeightSets);
                    }
                  
                    //if (DTO.IsAccredited == true && DTO.WOD_Weights == null && !(DTO.BalanceAndScaleCalibration.TestPointResult.FirstOrDefault().CalibrationSubTypeId >= 507 && DTO.BalanceAndScaleCalibration.TestPointResult.FirstOrDefault().CalibrationSubTypeId <= 518) && DTO.CalibrationType?.IsNivel1 == false)
                    //{
                    //    listValidation.Add(Querys.WOD.WODMessages.WeightSets);
                      
                    //}


                    List<Linearity> linne = new List<Linearity>();
                    Eccentricity ecce = new Eccentricity();
                    Entities.Repeatability repea = new Entities.Repeatability();

                    if (DTO?.BalanceAndScaleCalibration != null)
                    {
                        if(DTO?.BalanceAndScaleCalibration?.Linearities != null)
                        {
                            linne = DTO.BalanceAndScaleCalibration.Linearities.ToList();
                        }

                        if (DTO?.BalanceAndScaleCalibration?.Eccentricity != null)
                        {
                            ecce = DTO.BalanceAndScaleCalibration.Eccentricity;

                        }

                        if (DTO?.BalanceAndScaleCalibration?.Repeatability != null)
                        {
                            repea = DTO.BalanceAndScaleCalibration.Repeatability;

                        }


                        if (linne?.Count == 0 && ecce==null && repea == null)
                        {
                            listValidation.Add(Querys.WOD.WODMessages.OneCalibrationType);

                        }

                    }
                    else if (DTO?.BalanceAndScaleCalibration?.Linearities?.Count == 0
                        || DTO?.BalanceAndScaleCalibration.Linearities.Where(x => x.TestPoint == null).ToList().Count > 0)
                    {
                        listValidation.Add(Querys.WOD.WODMessages.CalibrationType);

                    }


                    if (linne?.Count > 0 && DTO?.BalanceAndScaleCalibration != null && DTO?.BalanceAndScaleCalibration?.Linearities != null
                        && DTO?.BalanceAndScaleCalibration?.Linearities?.Count > 0)
                    {

                        foreach (var item in DTO.BalanceAndScaleCalibration.Linearities)
                        {
                            if (item.BasicCalibrationResult.WeightApplied > 0 && (item.BasicCalibrationResult.AsFound == 0 || item.BasicCalibrationResult.AsLeft == 0))
                            {
                                listValidation.Add(Querys.WOD.WODMessages.Linearity + " in line number : " + item.BasicCalibrationResult.Position);
                                break;
                              
                            }

                            if (DTO.IsAccredited.Value && (item.BasicCalibrationResult.WeightApplied > 0 
                                && (item.CalibrationSubType_Weights == null || item.CalibrationSubType_Weights.Count == 0)))
                            {
                                listValidation.Add(Querys.WOD.WODMessages.UnassignedWeights + " Linearity"  + " in line number : " + item.BasicCalibrationResult.Position);
                                break;
                            }

                        }
                    }

                    if (repea != null  && DTO?.BalanceAndScaleCalibration != null && DTO?.BalanceAndScaleCalibration?.Repeatability != null &&
                        DTO.BalanceAndScaleCalibration.Repeatability?.TestPointResult?.Count > 0)
                    {


                        if (DTO.IsAccredited.Value && (DTO.BalanceAndScaleCalibration.Repeatability.CalibrationSubType_Weights == null
                            || DTO.BalanceAndScaleCalibration.Repeatability.CalibrationSubType_Weights.Count == 0))
                        {
                            listValidation.Add(Querys.WOD.WODMessages.UnassignedWeights + " Repeatability");
                           
                        }

                        foreach (var item in DTO?.BalanceAndScaleCalibration?.Repeatability?.TestPointResult)
                        {
                            if (item.AsFound == 0 || item.AsLeft == 0)
                            {
                                listValidation.Add(Querys.WOD.WODMessages.Repeatibility);
                               
                            }
                        }
                    }


                    if (Querys.ValidateEccentrycityTespoint( DTO))
                     {

                        if (DTO.IsAccredited.Value && (DTO.BalanceAndScaleCalibration.Eccentricity.CalibrationSubType_Weights == null 
                            || DTO.BalanceAndScaleCalibration.
                            Eccentricity.CalibrationSubType_Weights.Count == 0))
                        {
                            listValidation.Add(Querys.WOD.WODMessages.EccentricityWeights);
                         
                        }

                        foreach (var item in DTO?.BalanceAndScaleCalibration?.Eccentricity?.TestPointResult)
                        {
                            if (item.AsFound == 0 || item.AsLeft == 0)
                            {
                                listValidation.Add(Querys.WOD.WODMessages.Eccentricity);
                                break;
                               
                            }
                        }
                    }




                }
             
                if (NextStatus.StatusId == 4 )
                {
                   
                }


                //if (NextStatus.StatusId > 2 && !CalibrationSaaS.Domain.Aggregates.Shared.LTILogic.ValidateTemperature(DTO.Temperature))
                //{
                //    listValidation.Add(Querys.WOD.WODMessages.TemperatureRange);
                //}

                // if (DTO.CalibrationTypeID ==2 && NextStatus.StatusId > 2 && !CalibrationSaaS.Domain.Aggregates.Shared.LTILogic.ValidateTemperatureDifference(DTO.Temperature,DTO.TemperatureAfter))
                //{
                //    listValidation.Add(Querys.WOD.WODMessages.TemperatureRange);
                //}


                return listValidation.Distinct().ToList();
            }


            public class WODMessages
            {
                public const string Accredited = "The order detail must be credited or not ";
                public const string Technician = "There must be a technician";
                public const string Tolerance = "Review the tolerance information";
                public const string Temperature = "Enter the temperature";
                public const string AllFields = "Fill in the requested information";
                public const string Dates = "Review Dates Information";
                public const string Condition = "Review Equipment condition";
                public const string Localization = "Review Address";
                public const string WeightSets = "You must configure the standards";
                public const string CalibrationType = "You must configure the Calibrations";
                public const string Linearity = "Fill in all the linearity information";
                public const string Humidity = "Review the Humidity information";
                public const string Enviroment = "Review the Enviroment information";
                public const string Interval = "Review the Calibration Interval information";
                public const string Certification = "Review the Certification information";
                public const string WeighSets = "Configure the WeightSets";
                public const string Repeatibility = "Fill in all the Repeatibility information";
                public const string Eccentricity = "Fill in all the Eccentricity information";
                public const string EccentricityWeights = "Fill in all the Eccentricity Weights";
                public const string CalculateWeightSetConfiguration = "WeightSet Configuration Error, Actual Weight not must be zero";
                public const string OneCalibrationType = "You must configure at least one type of calibration";
                public const string UnassignedWeights = "Unassigned weights";
                public const string TestPoint = "You must configure Test Point ";
                public const string StatusComplete = "Do you want to replace the actual certificate version or create a new one?";
                public const string TemperatureRange = "Review the Temperature information";
                public const string TemperatureDiff = "Review the Temperature information2";



            }


            public static  Task<Reports.Domain.ReportViewModels.UncertaintyViewModel> GetReportUncertaintyBudget(Linearity li, int seq, WorkOrderDetail wod, List<PieceOfEquipment>? poes_)
            {


                WorkOrderDetail wo = new WorkOrderDetail();
                wo.WorkOrderDetailID = li.WorkOrderDetailId;
                WorkOrderDetail workOrderDetail1 = new WorkOrderDetail();
                IEnumerable<PieceOfEquipment> poes = new List<PieceOfEquipment>();
               
                    workOrderDetail1 = wod;
                var workOrderDetail = workOrderDetail1;//await wodRepository.GetWorkOrderDetailByID(wo.WorkOrderDetailID);

                var workOrder = workOrderDetail1.WorkOder;


                var poe = workOrderDetail1.PieceOfEquipment;


                poes = poes_; //await pieceOfEquipmentRepository.GetAllWeightSets(poe);

                var eqTemp = poe.EquipmentTemplate;
                List<WeightSet> weigthSets = new List<WeightSet>();
                if (poes != null)
                    weigthSets = poes.FirstOrDefault().WeightSets.ToList();

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


                IEnumerable<BasicCalibrationResult> _repeatabilityBCR = null;
                IEnumerable<BasicCalibrationResult> _eccentricityBCR = null;

                if (workOrderDetail.BalanceAndScaleCalibration.Repeatability != null)
                {

                    _repeatabilityBCR = workOrderDetail.BalanceAndScaleCalibration.Repeatability.TestPointResult.Where(x => x.CalibrationSubTypeId == 2);
                }



                var _balance = workOrderDetail.BalanceAndScaleCalibration;

                if (workOrderDetail.BalanceAndScaleCalibration.Eccentricity != null)
                {
                    _eccentricityBCR = workOrderDetail.BalanceAndScaleCalibration.Eccentricity.TestPointResult.Where(x => x.CalibrationSubTypeId == 3);

                }


                Reports.Domain.ReportViewModels.UncertaintyViewModel uncert = new Reports.Domain.ReportViewModels.UncertaintyViewModel()
                {
                    EquipmentTypeId = eqTemp.EquipmentTypeID.ToString(),
                    WorkOrderDetail = workOrderDetail.WorkOrderDetailID.ToString(),
                    CalibrationDate = workOrderDetail.CalibrationDate.Value.ToString("MM/dd/yyyy")
                };

                List<UncertaintyOfRerence> uncertaintyOfRerence = new List<UncertaintyOfRerence>();
                List<CornerloadEccentricity> cornerloadEccentricities = new List<CornerloadEccentricity>();
                List<RepeatabilityOfBalance> repeatabilityOfBalances = new List<RepeatabilityOfBalance>();
                EnvironmentalFactors environmentalFactors = new EnvironmentalFactors();
                Resolution resolutions = new Resolution();
                Reports.Domain.ReportViewModels.Totales totales = new Reports.Domain.ReportViewModels.Totales();

                double totalSquare = 0;


                if (_linearity != null && _linearity.Count > 0)
                {
                    var af = _linearity.Where(x => x.SequenceID == seq).FirstOrDefault();

                    var wodWeigths = workOrderDetail.WOD_Weights.ToList();

                    if (wodWeigths != null && wodWeigths.Count() > 0)
                    {
                        foreach (var wodWeight in wodWeigths)
                        {
                            //DO
                            WeightSet weightSet = null; // await pieceOfEquipmentRepository.GetWeigthSetById(wodWeight.WeightSetID);   //af.WeightSets.FirstOrDefault();



                            af.WeightSets.FirstOrDefault();
                            double magnitude = 0;
                            string weigth = "0";
                            double divisor = 0;
                            if (af.WeightSets != null && af.WeightSets.Count() > 0)
                            {
                                magnitude = (double)RoundFirstSignificantDigit(Convert.ToDecimal(weightSet.CalibrationUncertValue)); 
                                UnitOfMeasure uom = new UnitOfMeasure();
                                uom.UnitOfMeasureID = weightSet.UnitOfMeasureID;
                                // DO var UoM = await uomRepository.GetByID(uom);
                                //DO weigth = weightSet.WeightActualValue.ToString() + " " + UoM.Abbreviation;
                                divisor = weightSet.Divisor;

                            }

                            double quotient = 0;
                            if (divisor != 0)
                                quotient = (double)RoundFirstSignificantDigit(Convert.ToDecimal(magnitude / divisor)); // Math.Round(magnitude / divisor, 8);
                            AppState appState = new AppState();
                            Dictionary<int, string> distributionUncertList = appState.DistributionUncertList;
                            List<WeightType> typeUncertList = appState.WeightTypeList;
                            int t = Convert.ToInt16(weightSet.Type);
                            string type = typeUncertList.Where(x => x.WeightTypeID == t).FirstOrDefault().Name;


                            var Uncertainty = new UncertaintyOfRerence
                            {
                                Weigth = weigth,
                                Distribution = weightSet.Distribution.GetDistribution(distributionUncertList),
                                Divisor = divisor,
                                Magnitude = magnitude.ToString(),
                                Quotient = quotient,
                                Square = Math.Pow(quotient, 2).ToString(),
                                Type = type,
                                Units = af.TestPoint.UnitOfMeasurement.Abbreviation

                            };
                            totalSquare = totalSquare + Convert.ToDouble(Uncertainty.Square);
                            uncertaintyOfRerence.Add(Uncertainty);
                        }
                    }


                }


                if (_eccentricityBCR != null)
                {


                    if (_eccentricityBCR != null && _eccentricityBCR.Count() > 0)
                    {
                        var rowseq = _eccentricityBCR.Where(x => x.SequenceID == seq);
                        var magnitudeecc = (double)RoundFirstSignificantDigit(Convert.ToDecimal(_eccentricityBCR.Max(x => x.AsLeft) - _eccentricityBCR.Min(x => x.AsLeft))); //Math.Round(_eccentricityBCR.Max(x => x.AsLeft) - _eccentricityBCR.Min(x => x.AsLeft), 3);
                        var divisorecc = 1.73;
                        var quotientecc = (double)RoundFirstSignificantDigit(Convert.ToDecimal(magnitudeecc / divisorecc)); //Math.Round(magnitudeecc / divisorecc, 8);

                        var cornerload = new CornerloadEccentricity
                        {
                            Weigth = "0",
                            Distribution = "Rectangular",
                            Divisor = divisorecc,
                            Magnitude = magnitudeecc.ToString(),
                            Quotient = quotientecc,
                            Square = RoundFirstSignificantDigit(Convert.ToDecimal(Math.Pow(quotientecc, 2))).ToString(), //Math.Round(Math.Pow(quotientecc, 2), 2),
                            Type = "A",
                            Units = rowseq.FirstOrDefault().UnitOfMeasure.Abbreviation

                        };
                        totalSquare = totalSquare + Convert.ToDouble(cornerload.Square);
                        cornerloadEccentricities.Add(cornerload);

                    }
                }

                if (_repeatabilityBCR != null)
                {
                    //stdev
                    var med = _repeatabilityBCR.Sum(x => x.AsLeft) / _repeatabilityBCR.Count();
                    var distMed = _repeatabilityBCR.Sum(x => Math.Pow((x.AsLeft - med), 2));
                    var stdev = Math.Sqrt(distMed / _repeatabilityBCR.Count());


                    if (_repeatabilityBCR != null && _repeatabilityBCR.Count() > 0)
                    {
                        var rowseq = _eccentricityBCR.Where(x => x.SequenceID == seq);
                        var magnituderep = (double)RoundFirstSignificantDigit(Convert.ToDecimal(stdev)); //Math.Round(stdev, 2);

                        var divisorrep = 1;
                        var quotientrep = (double)RoundFirstSignificantDigit(Convert.ToDecimal(magnituderep / divisorrep)); //Math.Round(magnituderep / divisorrep);
                        var rep = new RepeatabilityOfBalance
                        {
                            Weigth = rowseq.FirstOrDefault().WeightApplied.ToString() + " " + rowseq.FirstOrDefault().UnitOfMeasure.Abbreviation,
                            Distribution = "Normal",
                            Divisor = divisorrep,
                            Magnitude = magnituderep.ToString(),
                            Quotient = quotientrep,
                            Square = RoundFirstSignificantDigit(Convert.ToDecimal(Math.Pow(quotientrep, 2))).ToString(), //Math.Round(Math.Pow(quotientrep, 2), 2),
                            Type = "A",
                            Units = rowseq.FirstOrDefault().UnitOfMeasure.Abbreviation

                        };
                        totalSquare = totalSquare + Convert.ToDouble(rep.Square);
                        repeatabilityOfBalances.Add(rep);

                    }
                }


                var magnitudeEnv = "0.004";
                var divisorEnv = 1.73;
                var quotientEnv = (double)RoundFirstSignificantDigit(Convert.ToDecimal(magnitudeEnv)); // Math.Round(Convert.ToDouble(magnitudeEnv) / divisorEnv, 8);
                var env = new EnvironmentalFactors
                {

                    Distribution = "Rectangular",
                    Divisor = divisorEnv,
                    Magnitude = magnitudeEnv,
                    Quotient = quotientEnv,
                    Square = RoundFirstSignificantDigit(Convert.ToDecimal(Math.Pow(quotientEnv, 2))).ToString(), //Math.Round(Math.Pow(quotientEnv, 2), 4),
                    Type = "B",
                    Units = "g"

                };
                totalSquare = totalSquare + Convert.ToDouble(env.Square);


                var magnitudeRes = workOrderDetail1.Resolution;
                var divisorRes = 3.46;
                var quotientRes = (double)RoundFirstSignificantDigit(Convert.ToDecimal(Convert.ToDouble(magnitudeRes) / divisorRes)); // Math.Round(Convert.ToDouble(magnitudeRes) / divisorRes, 8);
                var res = new Resolution
                {
                    Distribution = "Resolution",
                    Divisor = divisorRes,
                    Magnitude = magnitudeRes.ToString(),
                    Quotient = quotientRes,
                    Square = RoundFirstSignificantDigit(Convert.ToDecimal(Math.Pow(quotientRes, 2))).ToString(),  //Math.Round(Math.Pow(quotientRes, 2), 2),
                    Type = "B",
                    Units = "g"

                };
                totalSquare = totalSquare + Convert.ToDouble(res.Square);

                totales.SumOfSquares = totalSquare.ToString();
                totales.TotalUncerainty = (double)RoundFirstSignificantDigit(Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(totales.SumOfSquares)))); // Math.Round(Math.Sqrt(totales.SumOfSquares), 4);
                totales.ExpandedUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(totales.TotalUncerainty * totales.TotalUncerainty)));

                uncert.CornerloadEccentricities = cornerloadEccentricities;
                uncert.RepeatabilityOfBalances = repeatabilityOfBalances;
                uncert.EnvironmentalFactors = env;
                uncert.Resolutions = res;
                uncert.Totales = totales;
                uncert.UncertaintyOfRerences = uncertaintyOfRerence;

                return null; //DO uncert;

            }
        }

        //public static decimal RoundFirstSignificantDigit(decimal numberInit)
        //{
        //    try
        //    {
        //        if (numberInit == 0)
        //            return 0;

        //        int precision = 0;
        //        while (Math.Abs(numberInit) < 1)
        //        {
        //            numberInit *= 10;
        //            precision++;
        //        }

        //        int digits = (int)Math.Floor(Math.Log10((double)Math.Abs(numberInit)) + 1);
        //        precision = precision < 1 ? Math.Max(2, 2 + (2 - digits)) : precision + 3;

        //        return Math.Round(numberInit, precision - 2);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        return Math.Round(numberInit, 2);
        //    }
        //}
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

    }
}
