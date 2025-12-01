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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static CalibrationSaaS.Domain.Aggregates.Querys.Querys;
using System.Dynamic;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using Helpers;
using Newtonsoft.Json;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using Reports.Domain.ReportViewModels;
using CalibrationSaaS.Domain.Aggregates.Querys;
using Microsoft.VisualBasic;
using System.Xml.Schema;
using Totales = CalibrationSaaS.Domain.Aggregates.ValueObjects.Totales;
using UncertaintyViewModel = CalibrationSaaS.Domain.Aggregates.ValueObjects.UncertaintyViewModel;
using System.Diagnostics.CodeAnalysis;

namespace CalibrationSaaS.Domain.Aggregates.Shared
{
    public partial class UncertaintyLogicDynamic : IFormula
    {

        
        private AppState appState { get; set; }= new AppState();
        
        public async Task<object> FormulaMethod(dynamic gcrObject, PieceOfEquipment poe, CalibrationType calibrationType)
        {

           
            FormulaResult formulaResult = new FormulaResult();
           
            return formulaResult; //expUncertainty.ValidDouble();


        }
        public async Task<object> FormulaMethodToReportDynamic(dynamic gcrObjectTestPoint,  dynamic gcrObject, WorkOrderDetail wod, CalibrationType caltype)
        {
            Dictionary<int, string> distributionUncertList = appState.DistributionUncertList;
            List<WeightType> typeUncertList = appState.WeightTypeList;
            double accumContributorsSquare = 0;
             
            //1. Poe
            List<ContributorDynamic> resultContributors = new List<ContributorDynamic>();
            string uomWod = wod?.PieceOfEquipment?.UnitOfMeasure?.Abbreviation;

            double nominal = 0;
            if (gcrObjectTestPoint?.Object != null)
            {
                var extObj = gcrObjectTestPoint.Object;

                JObject obj1 = JObject.Parse(extObj);

                //1
                nominal = Convert.ToDouble((string)obj1["Nominal"]);
            }

            if (wod?.PieceOfEquipment?.Uncertainty?.Count() > 0)
            {

                List<Uncertainty> uncs = wod?.PieceOfEquipment?.Uncertainty.ToList();
                
                foreach (var unc in uncs)
                {
                    if (unc.RangeMin <= nominal && unc.RangeMax >= nominal && unc.UnitOfMeasure?.Abbreviation == uomWod)
                    {
                        ContributorDynamic resultContributor = new ContributorDynamic();
                        resultContributor.Contributor = wod?.PieceOfEquipment?.SerialNumber;
                        resultContributor.Magnitude = Math.Round((double)unc.Value, 3).ToString();
                        resultContributor.Divisor = (int)unc.Divisor;
                        resultContributor.Quotient = Math.Round((Convert.ToDouble(resultContributor.Magnitude) / (int)unc.Divisor), (int)wod.Resolution);
                        resultContributor.Units = unc.UnitOfMeasure.Abbreviation;
                        resultContributor.Square = unc.Square.ToString();
                        resultContributor.Type = unc.Type == 1 ? "A" :
                                                                 unc.Type == 2 ? "B" :
                                                                 unc.Type.ToString();
                        resultContributor.Distribution = unc.Distribution.GetDistribution(distributionUncertList);
                        resultContributor.Square = Math.Pow(Convert.ToDouble(resultContributor.Quotient), 2).ToString();
                        resultContributors.Add(resultContributor);
                        break;
                    }
                }
            }



            // Standard
            List<WOD_Weight> standards = new List<WOD_Weight>();
            if (wod?.WOD_Weights?.Count() > 0 )
            {
                standards = wod?.WOD_Weights?.DistinctBy(x=>x.WeightSet.PieceOfEquipmentID).ToList();
                if (standards.Count > 0 && standards != null)
                {



                    foreach (var standard in standards)
                    {


                        var uncs = standard?.WeightSet?.PieceOfEquipment?.Uncertainty?.ToList();

                        foreach (var unc in uncs)
                        {

                            if (unc != null && unc.RangeMin <= nominal && unc.RangeMax >= nominal && unc?.UnitOfMeasureID == standard?.WeightSet?.PieceOfEquipment?.UnitOfMeasureID)
                            {
                                var type = GetTypeUncert(unc.Type, typeUncertList);

                                ContributorDynamic resultContributor = new ContributorDynamic();
                                resultContributor.Contributor = standard?.WeightSet?.PieceOfEquipment?.SerialNumber.ToString();
                                resultContributor.Magnitude = unc.Value.ToString();
                                resultContributor.Divisor = unc.Divisor;
                                resultContributor.Quotient = Math.Round((Convert.ToDouble(resultContributor.Magnitude) / unc.Divisor), (int)wod.Resolution);
                                resultContributor.Units = standard?.WeightSet?.PieceOfEquipment?.UnitOfMeasure?.Abbreviation.ToString();

                                resultContributor.Square = Math.Pow(Convert.ToDouble(resultContributor.Quotient), 2).ToString();
                                resultContributor.Type = type.ToString();
                                resultContributor.Distribution = unc.Distribution.GetDistribution(distributionUncertList);

                                resultContributors.Add(resultContributor);
                            }
                        }
                    }


                }

            }

            if (gcrObject != null)
            {
                var json = (string)gcrObject;
                var result = new Dictionary<string, Dictionary<string, object>>();

                var jObject = JObject.Parse(json);
                var typeUncertainty = jObject["TypeUncertainty"] as JObject;


                if (typeUncertainty != null)
                {



                    foreach (var property in typeUncertainty.Properties())
                    {

                        ContributorDynamic contributor = new ContributorDynamic();

                        var typeName = property.Name; // "Constant", "Resolution", "TestPoint"

                        if (typeName != "Standard")
                        {
                            var values = property.Value as JObject;

                            var fieldData = new Dictionary<string, object>();

                            foreach (var field in values.Properties())
                            {
                                fieldData[field.Name] = field.Value;
                            }

                            result[typeName] = fieldData;

                            ///2. Constant Uncertainty Attribute:
                            ///
                            double magnitude = 0;
                            var divisor = 0;

                            if (typeName == "Constant")
                            {
                                magnitude = Convert.ToDouble(Convert.ToDouble(fieldData["Magnitude"]));

                            }
                            else if (typeName == "Resolution")
                            {
                                magnitude = wod.Resolution;
                            }

                            else if (typeName == "TestPoint")
                            {
                                var operation = fieldData["Operation"].ToString();
                                var datasource = fieldData["DataSource"].ToString();

                                var calibrationSubtypeId = (double)values["CalibrationSubtypeId"];
                                List<string> objects = wod.BalanceAndScaleCalibration.TestPointResult
                                    .Where(x => x.ComponentID == wod.WorkOrderDetailID.ToString() &&
                                                x.Position != -1 &&
                                                x.CalibrationSubTypeId == calibrationSubtypeId)
                                    .Select(x => x.ExtendedObject)
                                    .ToList();

                                var numericValues = new List<double>();

                                if (objects != null && objects.Any())
                                {
                                    foreach (var obj in objects)
                                    {
                                        try
                                        {
                                            var jObj = JObject.Parse(obj);

                                            if (jObj.ContainsKey(datasource))
                                            {
                                                var arr = jObj[datasource] as JArray;
                                                if (arr != null && arr.Count >= 3)
                                                {
                                                    var valueStr = arr[2]?.ToString();
                                                    if (double.TryParse(valueStr, out double value))
                                                    {
                                                        numericValues.Add(value);
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            // Manejar parseos fallidos si el JSON no es válido
                                            //                                        Console.WriteLine($"Error parsing ExtendedObject: {ex.Message}");
                                        }
                                    }
                                }

                                if (numericValues.Any())
                                {
                                    double resultValue = 0;

                                    if (operation == "AVG")
                                    {
                                        resultValue = numericValues.Average();
                                    }
                                    else if (operation == "STDEV")
                                    {
                                        var avg = numericValues.Average();
                                        var sumSquares = numericValues.Sum(val => Math.Pow(val - avg, 2));
                                        resultValue = Math.Sqrt(sumSquares / numericValues.Count);
                                    }

                                    magnitude = resultValue;

                                }
                            }



                            contributor.Contributor = values["Name"].ToString();
                            magnitude = magnitude;
                            contributor.Divisor = Convert.ToInt16(fieldData["Divisor"]);
                            contributor.Quotient = Math.Round(magnitude / Convert.ToInt16(contributor.Divisor), 3);
                            contributor.Magnitude = Math.Round(magnitude, 3).ToString();
                            contributor.Distribution = values["Distribution"].ToString();
                            contributor.Square = Math.Round(Convert.ToDouble(contributor.Quotient) * Convert.ToDouble(contributor.Quotient), 3).ToString();
                            contributor.Units = fieldData["UoM"].ToString();
                            contributor.Type = fieldData["Type"].ToString();

                            resultContributors.Add(contributor);

                        }
                        else if (typeName == "Standard")
                        {
                            double magnitude = 0;
                            List<WeightSet> weigthsets = new List<WeightSet>();
                            GenericCalibrationResult2 gcr2 = gcrObjectTestPoint;
                            var testpointResults = wod.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == gcr2.CalibrationSubTypeId && x.SequenceID == gcr2.SequenceID);   //   TestPointResults-- GenericCalibration2-- WeigthSets



                            foreach (var item in testpointResults)
                            {


                                weigthsets = item.GenericCalibration2.WeightSets.ToList();

                                foreach (var item1 in weigthsets)
                                {

                                    var uncerts = item1.PieceOfEquipment?.Uncertainty?.ToList();

                                    foreach (var unc in uncerts)
                                    {
                                        if (unc.RangeMin <= nominal && unc.RangeMax >= nominal && unc.UnitOfMeasure?.Abbreviation == uomWod)
                                        {
                                            ContributorDynamic resultContributor = new ContributorDynamic();
                                            resultContributor.Contributor = unc.PieceOfEquipmentID;
                                            resultContributor.Magnitude = Math.Round((double)unc.Value, 3).ToString();
                                            resultContributor.Divisor = unc.Divisor;
                                            resultContributor.Quotient = Math.Round((Convert.ToDouble(resultContributor.Magnitude) / unc.Divisor), 3);
                                            resultContributor.Units = unc.UnitOfMeasure.Abbreviation;
                                            resultContributor.Square = unc.Square.ToString();
                                            resultContributor.Type = unc.Type == 1 ? "A" :
                                                                     unc.Type == 2 ? "B" :
                                                                     unc.Type.ToString();
                                            resultContributor.Distribution = unc.Distribution.GetDistribution(distributionUncertList);
                                            resultContributor.Square = Math.Pow(Convert.ToDouble(resultContributor.Quotient), 2).ToString();
                                            resultContributors.Add(resultContributor);
                                            break;
                                        }
                                    }
                                }

                            }


                        }
                    }
                }
            }

            return resultContributors;


        }

        public async Task<object> GetContributtors(WorkOrderDetail wod, params string[] properties)
        {
                       
            Dictionary<int, string> distributionUncertList = appState.DistributionUncertList;
            List<WeightType> typeUncertList = appState.WeightTypeList;
            double accumContributorsSquare = 0;

            var testPointsResult = wod?.BalanceAndScaleCalibration?.TestPointResult?
                                 .Where(tp =>
                                 {
                                     if (string.IsNullOrEmpty(tp.ExtendedObject)) return false;

                                     try
                                     {
                                         var jObj = JObject.Parse(tp.ExtendedObject);
                                         return jObj.ContainsKey("Nominal");
                                     }
                                     catch
                                     {
                                         return false;
                                     }
                                 })
                                 .ToList();
            object resultUncertainty = null;
          
           // var testpointResults = testPointsResult.Where(x => x.CalibrationSubTypeId == gcr2.CalibrationSubTypeId && x.SequenceID == gcr2.SequenceID);
            //1. Poe
            
            string uomWod = wod?.PieceOfEquipment?.UnitOfMeasure?.Abbreviation;

            double nominal = 0;

            CalibrationType calType = new CalibrationType()
            {
                CalibrationTypeId = (int)wod.CalibrationTypeID
            };
            calType = wod?.CalibrationType;
            var gcrObject = calType?.JsonUncertaintyConfiguration;

        
            if (testPointsResult != null)
            {
                
                foreach (var gcrObjectTestPoint in testPointsResult)
                {
                    List<ContributorDynamic> resultContributors = new List<ContributorDynamic>();
                    if (gcrObjectTestPoint?.Object != null)
                    {
                        var extObj = gcrObjectTestPoint.Object;

                        JObject obj1 = JObject.Parse(extObj);

                        //1
                        nominal = Convert.ToDouble((string)obj1["Nominal"]);
                    }

                    /////////////////////////////////
                    ///
                    // When Standards are general
                    List<WOD_Weight> standards = new List<WOD_Weight>();
                    if (wod?.WOD_Weights?.Count() > 0)
                    {
                        standards = wod?.WOD_Weights?.DistinctBy(x => x.WeightSet.PieceOfEquipmentID).ToList();
                        if (standards.Count > 0 && standards != null)
                        {



                            foreach (var standard in standards)
                            {


                                var uncs = standard?.WeightSet?.PieceOfEquipment?.Uncertainty?.ToList();

                                foreach (var unc in uncs)
                                {

                                    if (unc != null && unc.RangeMin <= nominal && unc.RangeMax >= nominal && unc?.UnitOfMeasureID == standard?.WeightSet?.PieceOfEquipment?.UnitOfMeasureID)
                                    {
                                        var type = GetTypeUncert(unc.Type, typeUncertList);

                                        ContributorDynamic resultContributor = new ContributorDynamic();
                                        resultContributor.Contributor = standard?.WeightSet?.PieceOfEquipment?.SerialNumber.ToString();
                                        resultContributor.Magnitude = unc.Value.ToString();
                                        resultContributor.Divisor = unc.Divisor;
                                        resultContributor.Quotient = Math.Round((Convert.ToDouble(resultContributor.Magnitude) / unc.Divisor), (int)wod.Resolution);
                                        resultContributor.Units = standard?.WeightSet?.PieceOfEquipment?.UnitOfMeasure?.Abbreviation.ToString();

                                        resultContributor.Square = Math.Pow(Convert.ToDouble(resultContributor.Quotient), 2).ToString();
                                        resultContributor.Type = type.ToString();
                                        resultContributor.Distribution = unc.Distribution.GetDistribution(distributionUncertList);

                                        resultContributors.Add(resultContributor);
                                    }
                                }
                            }


                        }

                    }
                    ////////////////////////////////////////




                    // Usar el repositorio para obtener las incertidumbres por PieceOfEquipment ID
                    var uncertaintiesFromRepo = wod?.PieceOfEquipment?.Uncertainty;
                    
                    
                    if (uncertaintiesFromRepo != null && uncertaintiesFromRepo.Count() > 0)
                    {
                        List<Uncertainty> uncs = uncertaintiesFromRepo.ToList();

                        foreach (var unc1 in uncs)
                        {
                            if (unc1.RangeMin <= nominal && unc1.RangeMax >= nominal && unc1.UnitOfMeasure?.Abbreviation == uomWod)
                            {
                                ContributorDynamic resultContributor = new ContributorDynamic();
                                resultContributor.Contributor = wod?.PieceOfEquipment?.SerialNumber;
                                resultContributor.Magnitude = Math.Round((double)unc1.Value, 3).ToString();
                                resultContributor.Divisor = (int)unc1.Divisor;
                                resultContributor.Quotient = Math.Round((Convert.ToDouble(resultContributor.Magnitude) / (int)unc1.Divisor), (int)wod.Resolution);
                                resultContributor.Units = unc1.UnitOfMeasure.Abbreviation;
                                resultContributor.Square = unc1.Square.ToString();
                                resultContributor.Type = unc1.Type.ToString();
                                resultContributor.Distribution = unc1.Distribution.GetDistribution(distributionUncertList);
                                resultContributor.Square = Math.Pow(Convert.ToDouble(resultContributor.Quotient), 2).ToString();
                                resultContributors.Add(resultContributor);
                                break;
                            }
                        }
                    }

                    var result = new Dictionary<string, Dictionary<string, object>>();
                    var json = (string)gcrObject;

                    if (json != null)
                    {
                        var jObject = JObject.Parse(json);
                        var typeUncertainty = jObject["TypeUncertainty"] as JObject;


                        if (typeUncertainty != null)
                        {



                            foreach (var property in typeUncertainty.Properties())
                            {

                                ContributorDynamic contributor = new ContributorDynamic();

                                var typeName = property.Name; // "Constant", "Resolution", "TestPoint"

                                if (typeName != "Standard")
                                {
                                    var values = property.Value as JObject;

                                    var fieldData = new Dictionary<string, object>();

                                    foreach (var field in values.Properties())
                                    {
                                        fieldData[field.Name] = field.Value;
                                    }

                                    result[typeName] = fieldData;

                                    ///2. Constant Uncertainty Attribute:
                                    ///
                                    double magnitude = 0;
                                    var divisor = 0;

                                    if (typeName == "Constant")
                                    {
                                        magnitude = Convert.ToDouble(Convert.ToDouble(fieldData["Magnitude"]));

                                    }
                                    else if (typeName == "Resolution")
                                    {
                                        magnitude = wod.Resolution;
                                    }

                                    else if (typeName == "TestPoint")
                                    {
                                        var operation = fieldData["Operation"].ToString();
                                        var datasource = fieldData["DataSource"].ToString();

                                        var calibrationSubtypeId = (double)values["CalibrationSubtypeId"];
                                        List<string> objects = wod.BalanceAndScaleCalibration.TestPointResult
                                            .Where(x => x.ComponentID == wod.WorkOrderDetailID.ToString() &&
                                                        x.Position != -1 &&
                                                        x.CalibrationSubTypeId == calibrationSubtypeId)
                                            .Select(x => x.ExtendedObject)
                                            .ToList();

                                        var numericValues = new List<double>();

                                        if (objects != null && objects.Any())
                                        {
                                            foreach (var obj in objects)
                                            {
                                                try
                                                {
                                                    var jObj = JObject.Parse(obj);

                                                    if (jObj.ContainsKey(datasource))
                                                    {
                                                        var arr = jObj[datasource] as JArray;
                                                        if (arr != null && arr.Count >= 3)
                                                        {
                                                            var valueStr = arr[2]?.ToString();
                                                            if (double.TryParse(valueStr, out double value))
                                                            {
                                                                numericValues.Add(value);
                                                            }
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    // Manejar parseos fallidos si el JSON no es válido
                                                    Console.WriteLine($"Error parsing ExtendedObject: {ex.Message}");
                                                }
                                            }
                                        }

                                        if (numericValues.Any())
                                        {
                                            double resultValue = 0;

                                            if (operation == "AVG")
                                            {
                                                resultValue = numericValues.Average();
                                            }
                                            else if (operation == "STDEV")
                                            {
                                                var avg = numericValues.Average();
                                                var sumSquares = numericValues.Sum(val => Math.Pow(val - avg, 2));
                                                resultValue = Math.Sqrt(sumSquares / numericValues.Count);
                                            }

                                            magnitude = resultValue;

                                        }
                                    }



                                    contributor.Contributor = values["Name"].ToString();
                                    magnitude = magnitude;
                                    contributor.Divisor = Convert.ToInt16(fieldData["Divisor"]);
                                    contributor.Quotient = Math.Round(magnitude / Convert.ToInt16(contributor.Divisor), 3);
                                    contributor.Magnitude = Math.Round(magnitude, 3).ToString();
                                    contributor.Distribution = values["Distribution"].ToString();
                                    contributor.Square = Math.Round(Convert.ToDouble(contributor.Quotient) * Convert.ToDouble(contributor.Quotient), 3).ToString();
                                    contributor.Units = fieldData["UoM"].ToString();
                                    contributor.Type = fieldData["Type"].ToString();

                                    resultContributors.Add(contributor);

                                }
                                else if (typeName == "Standard" && nominal != 0)
                                {
                                    double magnitude = 0;
                                    List<WeightSet> weigthsets = new List<WeightSet>();
                                    GenericCalibrationResult2 gcr2 = gcrObjectTestPoint;
                                    var testpointResults = testPointsResult.Where(x => x.CalibrationSubTypeId == gcr2.CalibrationSubTypeId && x.SequenceID == gcr2.SequenceID);   //   TestPointResults-- GenericCalibration2-- WeigthSets

                                    var item = gcrObjectTestPoint;
                                    var weigthsets_ = item.GenericCalibration2?.WeightSets;
                                    if (weigthsets_ != null && weigthsets_.Count() > 0)
                                    {
                                        weigthsets = weigthsets_.ToList();
                                        foreach (var item1 in weigthsets)
                                        {

                                            List<Uncertainty> uncertaintybyPoes = new List<Uncertainty>();

                                            var uncerts = item1.PieceOfEquipment.Uncertainty;
                                            ///

                                            uncertaintybyPoes = uncerts.ToList();

                                            var poe = item1.PieceOfEquipment;

                                            foreach (var unc1 in uncerts)
                                            {
                                                if (unc1.RangeMin <= nominal && unc1.RangeMax >= nominal && unc1.UnitOfMeasure?.Abbreviation == uomWod)
                                                {
                                                    ContributorDynamic resultContributor = new ContributorDynamic();
                                                    resultContributor.Contributor = unc1.PieceOfEquipmentID;
                                                    resultContributor.Magnitude = Math.Round((double)unc1.Value, 3).ToString();
                                                    resultContributor.Divisor = (double)unc1.Divisor;
                                                    resultContributor.Quotient = Math.Round((Convert.ToDouble(resultContributor.Magnitude) / unc1.Divisor), 3);
                                                    resultContributor.Units = unc1.UnitOfMeasure.Abbreviation;
                                                    resultContributor.Square = unc1.Square.ToString();
                                                    resultContributor.Type = unc1.Type == 1 ? "A" :
                                                                             unc1.Type == 2 ? "B" :
                                                                             unc1.Type.ToString();
                                                    resultContributor.Distribution = unc1.Distribution.GetDistribution(distributionUncertList);
                                                    resultContributor.Square = Math.Pow(Convert.ToDouble(resultContributor.Quotient), 2).ToString();
                                                    resultContributors.Add(resultContributor);
                                                    break;
                                                }
                                            }
                                        }
                                    }



                                }
                            }
                        }
                    }
                    var sumOfSquares = resultContributors
                                .Where(x => !string.IsNullOrEmpty(x.Square.ToString()))
                                .Sum(x => double.TryParse(x.Square.ToString(), out var val) ? val : 0);
                    
                    var totalUncertainty = Math.Sqrt(sumOfSquares);
                    var tot = new Reports.Domain.ReportViewModels.Totales()
                    {
                        SumOfSquares = Math.Round(sumOfSquares, 3).ToString(),
                        TotalUncerainty = Math.Round(totalUncertainty, 3),
                        ExpandedUncertainty = Math.Round(totalUncertainty * 2, 3)

                    };
                    Reports.Domain.ReportViewModels.UncertaintyViewModel model = new Reports.Domain.ReportViewModels.UncertaintyViewModel();
                    model = new Reports.Domain.ReportViewModels.UncertaintyViewModel
                    {
                        Totales = tot,
                        ContributorsDynamic = resultContributors
                    };


                   

                    gcrObjectTestPoint.UncertaintyJSON = model.Totales.ExpandedUncertainty.ToString();
                   
                    


                }
            }

       

            
            return wod;
        }

        //public async Task<object> GetContributtors1(WorkOrderDetail wod, params string[] properties)
        //{


        //    Reports.Domain.ReportViewModels.UncertaintyViewModel unc = new Reports.Domain.ReportViewModels.UncertaintyViewModel();
        //    Reports.Domain.ReportViewModels.Totales totales = new Reports.Domain.ReportViewModels.Totales();

        //    //unc.totales = totales;

        //    try
        //    {
        //        List<Uncertainty> uncertainties = new List<Uncertainty>();

        //        var testpoints = wod.TestPointResult;

        //        double? magnitude = null;
        //        double? resolutionvar = null;

        //        double? uncertanintyvalue = null;

        //        if (properties != null && properties.Count() > 0)
        //        {
        //            magnitude = Convert.ToDouble(properties[0]);
        //        }
        //        if (properties != null && properties.Count() > 1)
        //        {
        //            resolutionvar = Convert.ToDouble(properties[1]);
        //        }
        //        if (properties != null && properties.Count() > 2)
        //        {
        //            uncertanintyvalue = Convert.ToDouble(properties[2]);
        //        }

        //        foreach (var testpoint in testpoints)
        //        {

        //            if (testpoint?.GenericCalibration2?.WeightSets?.Count > 0)
        //            {
        //                foreach (var standars in testpoint.GenericCalibration2.WeightSets)
        //                {
        //                    // Usar el repositorio para obtener las incertidumbres por PieceOfEquipment ID
        //                    var standardUncertainties = _uncertaintyRepository != null && !string.IsNullOrEmpty(standars.PieceOfEquipment?.PieceOfEquipmentID)
        //                        ? await _uncertaintyRepository.GetUncertaintyByPoe(standars.PieceOfEquipment.PieceOfEquipmentID)
        //                        : standars?.PieceOfEquipment?.Uncertainty?.ToList() ?? new List<Uncertainty>();

        //                    if (standars.PieceOfEquipment != null && standardUncertainties.Any())
        //                    {
        //                        foreach (var standar in standardUncertainties)
        //                        {
        //                            standar.Source = "WeightSets";
        //                            uncertainties.Add(standar);
        //                        }
        //                    }
        //                }

        //            }
        //        }

        //        // Usar el repositorio para obtener las incertidumbres por PieceOfEquipment ID
        //        var pieceUncertainties = _uncertaintyRepository != null && !string.IsNullOrEmpty(wod?.PieceOfEquipment?.PieceOfEquipmentID)
        //            ? await _uncertaintyRepository.GetUncertaintyByPoe(wod.PieceOfEquipment.PieceOfEquipmentID)
        //            : wod?.PieceOfEquipment?.Uncertainty?.ToList() ?? new List<Uncertainty>();

        //        if (pieceUncertainties.Any())
        //        {
        //            foreach (var testpoint in pieceUncertainties)
        //            {
        //                testpoint.Source = "PieceOfEquipment";
        //                uncertainties.Add(testpoint);
        //            }
        //        }

        //        if (wod?.PieceOfEquipment?.EquipmentTemplate?.Uncertainty?.Count > 0)
        //        {
        //            foreach (var testpoint in wod.PieceOfEquipment.EquipmentTemplate.Uncertainty)
        //            {

        //                testpoint.Source = "EquipmentTemplate";

        //                uncertainties.Add(testpoint);
        //            }
        //        }


        //        List<Uncertainty> uncertainties1 = new List<Uncertainty>();

        //        List<Uncertainty> uncValue = new List<Uncertainty>();

        //        List<Uncertainty> uncRange = new List<Uncertainty>();

        //        List<Uncertainty> uncConst = new List<Uncertainty>();

        //        List<Uncertainty> uncResolution = new List<Uncertainty>();

        //        List<Uncertainty> uncCalculation = new List<Uncertainty>();

        //        //UncertaintyViewModel view = new UncertaintyViewModel();

        //        foreach (var item in uncertainties)
        //        {


        //            //var divRes = 3.46;
        //            //magnitude = resolution;
        //            //quotient = magnitude / divRes;
        //            //square = Math.Round(quotient * quotient, 3);


        //            if (item.RangeMax.HasValue && item.RangeMin.HasValue && item.Value.HasValue)
        //            {


        //                item.ContributorType = "Range";

        //                uncRange.Add(item);



        //            }
        //            if (item.Value.HasValue && !item.RangeMax.HasValue && !item.RangeMin.HasValue)
        //            {
        //                item.ContributorType = "Single";
        //                uncValue.Add(item);
        //            }

        //            if (!item.RangeMax.HasValue && !item.RangeMin.HasValue && !item.Value.HasValue)
        //            {
        //                item.ContributorType = "Constant";
        //                uncConst.Add(item);
        //            }

        //            ///Resolution
        //            if (item.Distribution == "4")
        //            {
        //                item.ContributorType = "Resolution";
        //                uncResolution.Add(item);
        //            }

        //            if (!string.IsNullOrEmpty(item.UncertantyOperation))
        //            {
        //                item.ContributorType = "Calculation";
        //                uncCalculation.Add(item);
        //            }


        //        }

        //        List<Contributor> uncContributors = new List<Contributor>();

        //        ///Create Contributors
        //        ///

        //        //AppState appState = new AppState();

        //        if (uncValue.Count > 0)
        //        {
        //            uncertainties1.AddRange(uncValue);
        //        }

        //        if (uncRange.Count > 0)
        //        {
        //            uncertainties1.AddRange(uncRange);
        //        }
        //        if (uncConst.Count > 0)
        //        {
        //            uncertainties1.AddRange(uncConst);
        //        }

        //        if (uncResolution.Count > 0)
        //        {
        //            uncertainties1.AddRange(uncResolution);
        //        }
        //        if (uncCalculation.Count > 0)
        //        {
        //            uncertainties1.AddRange(uncCalculation);
        //        }





        //        uncContributors.AddRange(Contributor.ConvertUncertainties(uncertainties1, appState.UnitofMeasureList,
        //            appState.WeightDistributionList,
        //            appState.WeightTypeList, magnitude, resolutionvar, uncertanintyvalue));






        //        double sumOfSquares = 0;
        //        double totalUncertinty = 0;
        //        double expandedUncertainty = 0;
        //        foreach (var item in uncContributors)
        //        {
        //            sumOfSquares = sumOfSquares + Convert.ToDouble(item.Square);
        //        }

        //        totalUncertinty = Math.Sqrt(sumOfSquares);
        //        expandedUncertainty = totalUncertinty * totalUncertinty;


        //        totales.SumOfSquares = Math.Round(sumOfSquares, 3).ToString();
        //        //totales.TotalUncerainty = Math.Round(totalUncertinty, 3).ToString();
        //        //totales.ExpandedUncertainty = Math.Round(expandedUncertainty, 3).ToString();



        //        //unc.Contributors = uncContributors;

        //        //unc.totales = totales;

        //        //unc.Uncertainties = uncertainties1;

        //        //unc.ExpUncertainty=
        //        return unc;

        //    }
        //    catch (Exception ex)
        //    {

        //        //unc.totales.ExpandedUncertainty = "0";
        //        //unc.totales.SumOfSquares = "0";
        //        //unc.totales.TotalUncerainty = "0";
        //        //unc.Message = ex.Message;
        //        return unc;

        //    }


        //}


        public async Task<double> GetUncertantyValue(WorkOrderDetail wod)
        {


            return 0;

        }

        private string GetTypeUncert(int typeId, List<WeightType> typeUncertList)
        {
            var type = typeUncertList.FirstOrDefault(t => t.WeightTypeID == typeId);
            return type != null ? type.Name : "Unknown";
        }



    }
}
