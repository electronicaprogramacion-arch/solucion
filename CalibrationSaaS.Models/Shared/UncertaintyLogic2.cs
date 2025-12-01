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
using LinqKit;
using System.Data;

namespace CalibrationSaaS.Domain.Aggregates.Shared
{
    public  class UncertaintyLogic2: IFormula

    {

        //public async Task<object> FormulaMethod(dynamic gcrObject2)
        //{

        //    var gcrObject = gcrObject2.BasicCalibrationResult;
        //    List<PieceOfEquipment> poeResult = new List<PieceOfEquipment>();

        //    List<WeightSet> standards = new List<WeightSet>();
        //    var id = gcrObject.WorkOrderDetailId;
        //    CalibrationResultContributor calibrationResultContributors = new CalibrationResultContributor();
        //    JObject newObjOrder = new JObject();
        //    //  var uncertainty = poe.Uncertainty;
        //    double expUncertainty = 0;
        //    Dictionary<string, JToken> newObj1 = new Dictionary<string, JToken>();
        //    string titledJson = "";
        //    double expandedUncertainty = 0;
        //    UncertaintyViewModel uncertaintyView = new UncertaintyViewModel();
        //    dynamic UnceDynamic = new ExpandoObject();

        //    JObject jObject = new JObject();
        //    if (gcrObject.Object != null)
        //    {
        //        var extObj = gcrObject.Object;

        //        JObject obj1 = JObject.Parse(extObj);


        //        List<Contributor> contributors = new List<Contributor>();
        //        Contributor contributor = new Contributor();
        //        //1
        //        var load = Convert.ToDouble((string)obj1["Load_KFG"]);
        //        var uncertainty = Convert.ToDouble((string)obj1["UncertantyBlock"]);
        //        double resolution = Convert.ToDouble((string)obj1["StepResol"]);
        //        string unit = "";

        //        double magnitude = 0;
        //        if ((gcrObject.CalibrationSubTypeId == 14 || gcrObject.CalibrationSubTypeId == 15))
        //        {
        //            if (uncertainty != 0)
        //                magnitude = 1854.4 * ((load * 1000) / (uncertainty * uncertainty));
        //            else
        //                magnitude = 0;
        //            unit = "HV";
        //        }
        //        else if (gcrObject.CalibrationSubTypeId == 16 || gcrObject.CalibrationSubTypeId == 17)
        //        {

        //            if (uncertainty != 0)
        //                magnitude = 14229 * ((load * 1000) / (uncertainty * uncertainty));
        //            else
        //                magnitude = 0;
        //            unit = "HK";
        //        }


        //        double divisor = 2;
        //        var quotient = magnitude / divisor;
        //        var square = Math.Round(quotient * quotient, 3);

        //        contributor.Magnitude = Math.Round(magnitude, 3).ToString();
        //        contributor.Distribution = "Expanded";
        //        contributor.Quotient = Math.Round(quotient, 3).ToString();
        //        contributor.Square = Math.Round(square, 3).ToString();
        //        contributor.Units = unit.ToString();
        //        contributor.Type = "A";
        //        contributor.SerialNumber = "";
        //        contributor.CalibrationRole = "";
        //        contributor.Comment = "";
        //        contributor.Contributors = "";

        //        contributors.Add(contributor);

        //        //2                
        //        //Repeatability
        //        var Test1Avg = Convert.ToDouble((string)obj1["Test1Avg"]);
        //        var Test2Avg = Convert.ToDouble((string)obj1["Test2Avg"]);
        //        var Test3Avg = Convert.ToDouble((string)obj1["Test3Avg"]);
        //        var Test4Avg = Convert.ToDouble((string)obj1["Test4Avg"]);
        //        var Test5Avg = Convert.ToDouble((string)obj1["Test5Avg"]);
        //        var AverageUM = Convert.ToDouble((string)obj1["AverageUM"]);


        //        double repeat = 0;
        //        if (gcrObject.CalibrationSubTypeId == 16 || gcrObject.CalibrationSubTypeId == 14) ////As Found
        //        {
        //            repeat = (Math.Sqrt(((Math.Pow((Test1Avg - AverageUM), 2) + Math.Pow((Test2Avg - AverageUM), 2) + Math.Pow((Test3Avg - AverageUM), 2)) / 3))) / 3;
        //            divisor = 2.236;
        //        }
        //        else if (gcrObject.CalibrationSubTypeId == 15 || gcrObject.CalibrationSubTypeId == 17)
        //        {
        //            repeat = (Math.Sqrt(((Math.Pow((Test1Avg - AverageUM), 2) + Math.Pow((Test2Avg - AverageUM), 2) + Math.Pow((Test3Avg - AverageUM), 2) + Math.Pow((Test5Avg - AverageUM), 2) + Math.Pow((Test4Avg - AverageUM), 2)) / 5))) / 5;

        //            divisor = 1.732;
        //        }


        //        if (gcrObject.CalibrationSubTypeId == 14 || gcrObject.CalibrationSubTypeId == 15)
        //        {
        //            if (repeat != 0)
        //                magnitude = 1854.4 * ((load * 1000) / (repeat * repeat));
        //            else
        //                magnitude = 0;

        //            unit = "HV";
        //        }

        //        else if (gcrObject.CalibrationSubTypeId == 16 || gcrObject.CalibrationSubTypeId == 17)
        //        {
        //            if (repeat != 0)
        //                magnitude = 14229 * ((load * 1000) / (repeat * repeat));
        //            else
        //                magnitude = 0;

        //            unit = "HK";
        //        }



        //        quotient = magnitude / divisor;
        //        square = Math.Round(quotient * quotient, 3);

        //        contributor = new Contributor();
        //        contributor.Magnitude = Math.Round(magnitude, 3).ToString();
        //        contributor.Distribution = "Normal";
        //        contributor.Quotient = Math.Round(quotient, 3).ToString();
        //        contributor.Square = Math.Round(square, 3).ToString();
        //        contributor.Units = unit.ToString();
        //        contributor.Type = "B";
        //        contributor.SerialNumber = "";
        //        contributor.CalibrationRole = "";
        //        contributor.Comment = "";
        //        contributor.Contributors = "";
        //        contributor.Divisor = divisor.ToString();
        //        contributors.Add(contributor);


        //        //3 Resolution

        //        if (gcrObject.CalibrationSubTypeId == 14 || gcrObject.CalibrationSubTypeId == 15)
        //        {
        //            if (resolution != 0)
        //                magnitude = 1854.4 * ((load * 1000) / (resolution * resolution));
        //            else
        //                magnitude = 0;

        //            unit = "HV";
        //        }

        //        else if (gcrObject.CalibrationSubTypeId == 16 || gcrObject.CalibrationSubTypeId == 17)
        //        {
        //            if (resolution != 0)
        //                magnitude = 14229 * ((load * 1000) / (resolution * resolution));
        //            else
        //                magnitude = 0;


        //            unit = "HK";
        //        }



        //        quotient = magnitude / divisor;
        //        square = Math.Round(quotient * quotient, 3);
        //        contributor = new Contributor();
        //        contributor.Magnitude = Math.Round(magnitude, 3).ToString();
        //        contributor.Distribution = "Resolution";
        //        contributor.Quotient = Math.Round(quotient, 3).ToString();
        //        contributor.Square = Math.Round(square, 3).ToString();
        //        contributor.Units = unit.ToString();
        //        contributor.Type = "B";
        //        contributor.SerialNumber = "";
        //        contributor.CalibrationRole = "";
        //        contributor.Comment = "";
        //        contributor.Contributors = "";
        //        contributors.Add(contributor);

        //        //					Debug.logInfo("(exception) Diff CMC min: " + cmcMinStandardValue.value + " max: " + cmcMaxStandardValue.value + " value: " + cmcStandardValue.value + " UOM: " + cmcUnitOfMeasure.description + " Test Point Value: " + testPointStandardValue.value, "calculateExpandedUncertainty");
        //        //					Debug.logInfo("Diff CMC Converted min: " + normalizedMinCmcStandardValue.value + " max: " + normalizedMaxCmcStandardValue.value + " value: " + normalizedCmcStandardValue.value + " UOM: " + testPointUnitOfMeasure.description + " Test Point Value: " + testPointStandardValue.value, "calculateExpandedUncertainty");
        //        //				}

        //        //Totales
        //        double sumOfSquares = 0;
        //        double totalUncertinty = 0;
        //        foreach (var item in contributors)
        //        {
        //            sumOfSquares = sumOfSquares + Convert.ToDouble(item.Square);
        //        }

        //        totalUncertinty = Math.Sqrt(sumOfSquares);
        //        expandedUncertainty = totalUncertinty * totalUncertinty;

        //        Totales totales = new Totales();
        //        totales.SumOfSquares = Math.Round(sumOfSquares, 3).ToString();
        //        totales.TotalUncerainty = Math.Round(totalUncertinty, 3).ToString();
        //        totales.ExpandedUncertainty = Math.Round(expandedUncertainty, 3).ToString();

        //        uncertaintyView.Contributors = contributors;
        //        uncertaintyView.totales = totales;

        //        expUncertainty = expandedUncertainty;


        //    }

        //    return expUncertainty.ValidDouble();


        //}



        public async Task<object> FormulaMethod(dynamic gcrObject, PieceOfEquipment poe, CalibrationType calibrationType)
        {

            List<PieceOfEquipment> poeResult = new List<PieceOfEquipment>();

            List<WeightSet> standards = new List<WeightSet>();
            var id = gcrObject.WorkOrderDetailId;
            CalibrationResultContributor calibrationResultContributors = new CalibrationResultContributor();
            JObject newObjOrder = new JObject();
            //  var uncertainty = poe.Uncertainty;
            double expUncertainty = 0;
            Dictionary<string, JToken> newObj1 = new Dictionary<string, JToken>();
            string titledJson = "";
            double expandedUncertainty = 0;
            UncertaintyViewModel uncertaintyView = new UncertaintyViewModel();
            dynamic UnceDynamic = new ExpandoObject();



            JObject jObject = new JObject();
            if (gcrObject.Object != null)
            {
                var extObj = gcrObject.Object;

                JObject obj1 = JObject.Parse(extObj);


                List<Contributor> contributors = new List<Contributor>();
                Contributor contributor = new Contributor();
                //1
                var load = Convert.ToDouble((string)obj1["Load_KFG"]);
                var uncertainty = Convert.ToDouble((string)obj1["UncertantyBlock"]);
                double resolution = Convert.ToDouble((string)obj1["StepResol"]);
                string unit = "";

                double magnitude = 0;
                if ((gcrObject.CalibrationSubTypeId == 14 || gcrObject.CalibrationSubTypeId == 15))
                {
                    if (uncertainty != 0)
                        magnitude = 1854.4 * ((load * 1000) / (uncertainty * uncertainty));
                    else
                        magnitude = 0;
                    unit = "HV";
                }
                else if (gcrObject.CalibrationSubTypeId == 16 || gcrObject.CalibrationSubTypeId == 17)
                {

                    if (uncertainty != 0)
                        magnitude = 14229 * ((load * 1000) / (uncertainty * uncertainty));
                    else
                        magnitude = 0;
                    unit = "HK";
                }


                double divisor = 2;
                var quotient = magnitude / divisor;
                var square = Math.Round(quotient * quotient, 3);

                contributor.Magnitude = Math.Round(magnitude, 3).ToString();
                contributor.Distribution = "Expanded";
                contributor.Quotient = Math.Round(quotient, 3).ToString();
                contributor.Square = Math.Round(square, 3).ToString();
                contributor.Units = unit.ToString();
                contributor.Type = "A";
                contributor.SerialNumber = poe.SerialNumber;
                contributor.CalibrationRole = "";
                contributor.Comment = "";
                contributor.Contributors = "PieceOfEquipment";
                contributor.ControlNumber = poe.PieceOfEquipmentID;
                contributor.Divisor = divisor.ToString();
                contributors.Add(contributor);

                //2                
                //Repeatability
                var Test1Avg = Convert.ToDouble((string)obj1["Test1Avg"]);
                var Test2Avg = Convert.ToDouble((string)obj1["Test2Avg"]);
                var Test3Avg = Convert.ToDouble((string)obj1["Test3Avg"]);
                var Test4Avg = Convert.ToDouble((string)obj1["Test4Avg"]);
                var Test5Avg = Convert.ToDouble((string)obj1["Test5Avg"]);
                var AverageUM = Convert.ToDouble((string)obj1["AverageUM"]);


                double repeat = 0;
                if (gcrObject.CalibrationSubTypeId == 16 || gcrObject.CalibrationSubTypeId == 14) ////As Found
                {
                    repeat = (Math.Sqrt(((Math.Pow((Test1Avg - AverageUM), 2) + Math.Pow((Test2Avg - AverageUM), 2) + Math.Pow((Test3Avg - AverageUM), 2)) / 3))) / 3;
                    divisor = 2.236;
                }
                else if (gcrObject.CalibrationSubTypeId == 15 || gcrObject.CalibrationSubTypeId == 17)
                {
                    repeat = (Math.Sqrt(((Math.Pow((Test1Avg - AverageUM), 2) + Math.Pow((Test2Avg - AverageUM), 2) + Math.Pow((Test3Avg - AverageUM), 2) + Math.Pow((Test5Avg - AverageUM), 2) + Math.Pow((Test4Avg - AverageUM), 2)) / 5))) / 5;

                    divisor = 1.732;
                }


                if (gcrObject.CalibrationSubTypeId == 14 || gcrObject.CalibrationSubTypeId == 15)
                {
                    if (repeat != 0)
                        magnitude = 1854.4 * ((load * 1000) / (repeat * repeat));
                    else
                        magnitude = 0;

                    unit = "HV";
                }

                else if (gcrObject.CalibrationSubTypeId == 16 || gcrObject.CalibrationSubTypeId == 17)
                {
                    if (repeat != 0)
                        magnitude = 14229 * ((load * 1000) / (repeat * repeat));
                    else
                        magnitude = 0;

                    unit = "HK";
                }



                quotient = magnitude / divisor;
                square = Math.Round(quotient * quotient, 3);

                contributor = new Contributor();
                contributor.Magnitude = Math.Round(magnitude, 3).ToString();
                contributor.Distribution = "Normal";
                contributor.Quotient = Math.Round(quotient, 3).ToString();
                contributor.Square = Math.Round(square, 3).ToString();
                contributor.Units = unit.ToString();
                contributor.Type = "B";
                contributor.SerialNumber = poe.SerialNumber;
                contributor.CalibrationRole = "";
                contributor.Comment = "";
                contributor.Contributors = "PieceOfEquipment";
                contributor.ControlNumber = poe.PieceOfEquipmentID;
               
                contributors.Add(contributor);


                //3 Resolution

                if (gcrObject.CalibrationSubTypeId == 14 || gcrObject.CalibrationSubTypeId == 15)
                {
                    if (resolution != 0)
                        magnitude = 1854.4 * ((load * 1000) / (resolution * resolution));
                    else
                        magnitude = 0;

                    unit = "HV";
                }

                else if (gcrObject.CalibrationSubTypeId == 16 || gcrObject.CalibrationSubTypeId == 17)
                {
                    if (resolution != 0)
                        magnitude = 14229 * ((load * 1000) / (resolution * resolution));
                    else
                        magnitude = 0;


                    unit = "HK";
                }



                quotient = magnitude / divisor;
                square = Math.Round(quotient * quotient, 3);
                contributor = new Contributor();
                contributor.Magnitude = Math.Round(magnitude, 3).ToString();
                contributor.Distribution = "Resolution";
                contributor.Quotient = Math.Round(quotient, 3).ToString();
                contributor.Square = Math.Round(square, 3).ToString();
                contributor.Units = unit.ToString();
                contributor.Type = "B";
                contributor.SerialNumber = poe.SerialNumber;
                contributor.CalibrationRole = "";
                contributor.Comment = "";
                contributor.Contributors = "PieceOfEquipment";
                contributor.ControlNumber = poe.PieceOfEquipmentID;
                contributors.Add(contributor);

                //					Debug.logInfo("(exception) Diff CMC min: " + cmcMinStandardValue.value + " max: " + cmcMaxStandardValue.value + " value: " + cmcStandardValue.value + " UOM: " + cmcUnitOfMeasure.description + " Test Point Value: " + testPointStandardValue.value, "calculateExpandedUncertainty");
                //					Debug.logInfo("Diff CMC Converted min: " + normalizedMinCmcStandardValue.value + " max: " + normalizedMaxCmcStandardValue.value + " value: " + normalizedCmcStandardValue.value + " UOM: " + testPointUnitOfMeasure.description + " Test Point Value: " + testPointStandardValue.value, "calculateExpandedUncertainty");
                //				}

                //Totales
                double sumOfSquares = 0;
                double totalUncertinty = 0;
                foreach (var item in contributors)
                {
                    sumOfSquares = sumOfSquares + Convert.ToDouble(item.Square);
                }

                totalUncertinty = Math.Sqrt(sumOfSquares);
                expandedUncertainty = totalUncertinty * totalUncertinty;

                Totales totales = new Totales();
                totales.SumOfSquares = Math.Round(sumOfSquares, 3).ToString();
                totales.TotalUncerainty = Math.Round(totalUncertinty, 3).ToString();
                totales.ExpandedUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(expandedUncertainty))).ToString();

                uncertaintyView.Contributors = contributors;
                uncertaintyView.totales = totales;

                expUncertainty = expandedUncertainty;

                ///Uncertainty CMC Values
                ///

                string uomAbreviation = null;
                if (poe!=null && poe.UnitOfMeasure != null && poe.UnitOfMeasure.UnitOfMeasureID != null)
                {
                    uomAbreviation = poe.UnitOfMeasure.Abbreviation;
                }
                if (calibrationType != null && calibrationType.CMCValues != null && calibrationType.CMCValues.Count() > 0)
                {

                    bool replace = false;
                    var nominalValue = load;

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

                        if (replace && expUncertainty < item.CMC)
                        {
                            var uom = unit;

                            var uncertaintyOld = expUncertainty;
                            totales.ExpandedUncertainty = item.CMC.ToString() + " *";
                            uncertaintyView.cmcValuesReplace = "The original value for this uncertainty was: " +
                                    uncertaintyOld + "-" + uomAbreviation +
                                    " but it was replaced by the CMC value of " + item.CMC.ToString() +
                                    " according to the range limit of " + item.MinRange + " " + uomAbreviation + "-" + item.MaxRange + " " + uomAbreviation;

                        }
                       

                    }
                }



                string json = JsonConvert.SerializeObject(uncertaintyView);


                // Convertimos el JSON a un objeto JObject
                jObject = JObject.Parse(json);
            }

            return jObject;


        }

        public static async Task<JObject> CalculateUncertaintyCertBrinell(GenericCalibrationResult2 gcrObject)
        {

            List<PieceOfEquipment> poeResult = new List<PieceOfEquipment>();

            List<WeightSet> standards = new List<WeightSet>();
            var id = gcrObject.WorkOrderDetailId;
            CalibrationResultContributor calibrationResultContributors = new CalibrationResultContributor();
            JObject newObjOrder = new JObject();
            //  var uncertainty = poe.Uncertainty;
            double expUncertainty = 0;
            Dictionary<string, JToken> newObj1 = new Dictionary<string, JToken>();
            string titledJson = "";
            double expandedUncertainty = 0;
            UncertaintyViewModel uncertaintyView = new UncertaintyViewModel();
            dynamic UnceDynamic = new ExpandoObject();

            JObject jObject = new JObject();
            if (gcrObject.Object != null)
            {
                var extObj = gcrObject.Object;

                JObject obj1 = JObject.Parse(extObj);


                List<Contributor> contributors = new List<Contributor>();
                Contributor contributor = new Contributor();
                //1
                var load = Convert.ToDouble((string)obj1["Load"]);
                var uncertainty = Convert.ToDouble((string)obj1["Uncertanty"]);
                double resolution = Convert.ToDouble((string)obj1["StepResol"]);
                string unit = "";

                double magnitude = uncertainty;

                double divisor = 2;
                var quotient = magnitude / divisor;
                var square = Math.Round(quotient * quotient, 3);

                contributor.Magnitude = Math.Round(magnitude, 3).ToString();
                contributor.Distribution = "Expanded";
                contributor.Quotient = Math.Round(quotient, 3).ToString();
                contributor.Square = Math.Round(square, 3).ToString();
                contributor.Units = unit.ToString();
                contributor.Type = "A";
                contributor.SerialNumber = "";
                contributor.CalibrationRole = "";
                contributor.Comment = "";
                contributor.Contributors = "";

                contributors.Add(contributor);

                //2                
                //Repeatability

                var repeat = Convert.ToDouble((string)obj1["Repeat"]);
                magnitude = repeat;
                quotient = magnitude / divisor;
                square = Math.Round(quotient * quotient, 3);

                contributor = new Contributor();
                contributor.Magnitude = Math.Round(magnitude, 3).ToString();
                contributor.Distribution = "Normal";
                contributor.Quotient = Math.Round(quotient, 3).ToString();
                contributor.Square = Math.Round(square, 3).ToString();
                contributor.Units = unit.ToString();
                contributor.Type = "B";
                contributor.SerialNumber = "";
                contributor.CalibrationRole = "";
                contributor.Comment = "";
                contributor.Contributors = "";
                contributor.Divisor = divisor.ToString();
                contributors.Add(contributor);


                //3 Resolution


                magnitude = resolution;
                quotient = magnitude / divisor;
                square = Math.Round(quotient * quotient, 3);
                contributor = new Contributor();
                contributor.Magnitude = Math.Round(magnitude, 3).ToString();
                contributor.Distribution = "Resolution";
                contributor.Quotient = Math.Round(quotient, 3).ToString();
                contributor.Square = Math.Round(square, 3).ToString();
                contributor.Units = unit.ToString();
                contributor.Type = "B";
                contributor.SerialNumber = "";
                contributor.CalibrationRole = "";
                contributor.Comment = "";
                contributor.Contributors = "";
                contributors.Add(contributor);

                //					Debug.logInfo("(exception) Diff CMC min: " + cmcMinStandardValue.value + " max: " + cmcMaxStandardValue.value + " value: " + cmcStandardValue.value + " UOM: " + cmcUnitOfMeasure.description + " Test Point Value: " + testPointStandardValue.value, "calculateExpandedUncertainty");
                //					Debug.logInfo("Diff CMC Converted min: " + normalizedMinCmcStandardValue.value + " max: " + normalizedMaxCmcStandardValue.value + " value: " + normalizedCmcStandardValue.value + " UOM: " + testPointUnitOfMeasure.description + " Test Point Value: " + testPointStandardValue.value, "calculateExpandedUncertainty");
                //				}

                //Totales
                double sumOfSquares = 0;
                double totalUncertinty = 0;
                foreach (var item in contributors)
                {
                    sumOfSquares = sumOfSquares + Convert.ToDouble(item.Square);
                }

                totalUncertinty = Math.Sqrt(sumOfSquares);
                expandedUncertainty = totalUncertinty * totalUncertinty;

                Totales totales = new Totales();
                totales.SumOfSquares = Math.Round(sumOfSquares, 3).ToString();
                totales.TotalUncerainty = Math.Round(totalUncertinty, 3).ToString();
                totales.ExpandedUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(expandedUncertainty))).ToString();

                uncertaintyView.Contributors = contributors;
                uncertaintyView.totales = totales;

                expUncertainty = expandedUncertainty;



                string json = JsonConvert.SerializeObject(uncertaintyView);

                // Convertimos el JSON a un objeto JObject
                jObject = JObject.Parse(json);
            }

            return jObject;


        }
    }
}
