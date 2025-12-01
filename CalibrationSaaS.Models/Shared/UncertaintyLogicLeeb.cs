using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Dynamic;
using Microsoft.VisualBasic;
using Helpers;
using static CalibrationSaaS.Domain.Aggregates.Querys.Querys;
using System.Linq;

namespace CalibrationSaaS.Domain.Aggregates.Shared
{
    public  class UncertaintyLogicLeeb: IFormula

    {

      

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
            FormulaResult formulaResult = new FormulaResult();
            JObject jObject = new JObject();
            if (gcrObject.Object != null)
            {
                var extObj = gcrObject.Object;

                JObject obj1 = JObject.Parse(extObj);


                List<Contributor> contributors = new List<Contributor>();
                Contributor contributor = new Contributor();
                //1
                var load = Convert.ToDouble((string)obj1["Scale"]);
                var uncertainty = Convert.ToDouble((string)obj1["UncertantyBias"]);
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
                contributor.Units = unit;
                contributor.Type = "A";
                contributor.SerialNumber = "";
                contributor.CalibrationRole = "";
                contributor.Comment = "";
                contributor.Contributors = "";

                contributors.Add(contributor);

                //2                
                //Repeatability
                var divRep = Math.Sqrt(2);
                var repeat = Convert.ToDouble((string)obj1["Repeat"]);
                magnitude = repeat;
                quotient = magnitude / divRep;
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
                contributor.Divisor = divRep.ToString();
                contributors.Add(contributor);


                //3 Resolution

                var divRes = 3.46;
                magnitude = resolution;
                quotient = magnitude / divRes;
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
                contributor.Divisor = divRes.ToString();
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
                totales.ExpandedUncertainty = Math.Round(expandedUncertainty, 3).ToString();

                uncertaintyView.Contributors = contributors;
                uncertaintyView.totales = totales;

                expUncertainty = expandedUncertainty;

                formulaResult.uncertainty = expUncertainty;

                ///Uncertainty CMC Values
                ///

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

                            formulaResult.uncertainty = item.CMC.ValidDouble();
                            formulaResult.uncertaintyCMC = expUncertainty;

                        }
                        if (replace == false)
                        {
                            formulaResult.uncertaintyCMC = null;
                        }

                    }
                }

                //string json = JsonConvert.SerializeObject(uncertaintyView);

                //// Convertimos el JSON a un objeto JObject
                //jObject = JObject.Parse(json);
            }

            return formulaResult; //expUncertainty.ValidDouble();


        }
        public async Task<object> FormulaMethodToReport(dynamic gcrObject, WorkOrderDetail wod, CalibrationType caltype)
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
                var load = Convert.ToDouble((string)obj1["Scale"]);
                var uncertainty = Convert.ToDouble((string)obj1["UncertantyBias"]);
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
                contributor.SerialNumber = wod.PieceOfEquipment.SerialNumber;
                contributor.CalibrationRole = "Normal";
                contributor.Comment = "";
                contributor.Contributors = "";
                contributor.ControlNumber = wod.PieceOfEquipment.PieceOfEquipmentID;
                contributors.Add(contributor);

                //2                
                //Repeatability
                var divRep = Math.Sqrt(2);
                var repeat = Convert.ToDouble((string)obj1["Repeat"]);
                magnitude = repeat;
                quotient = magnitude / divRep;
                square = Math.Round(quotient * quotient, 3);

                contributor = new Contributor();
                contributor.Magnitude = Math.Round(magnitude, 3).ToString();
                contributor.Distribution = "Normal";
                contributor.Quotient = Math.Round(quotient, 3).ToString();
                contributor.Square = Math.Round(square, 3).ToString();
                contributor.Units = unit.ToString();
                contributor.Type = "B";
                contributor.SerialNumber = wod.PieceOfEquipment.SerialNumber;
                contributor.CalibrationRole = "Normal";
                contributor.Comment = "";
                contributor.Contributors = "";
                contributor.Divisor = divRep.ToString();
                contributor.ControlNumber = wod.PieceOfEquipment.PieceOfEquipmentID;
                contributors.Add(contributor);


                //3 Resolution

                var divRes = 3.46;
                magnitude = resolution;
                quotient = magnitude / divRes;
                square = Math.Round(quotient * quotient, 3);
                contributor = new Contributor();
                contributor.Magnitude = Math.Round(magnitude, 3).ToString();
                contributor.Distribution = "Resolution";
                contributor.Quotient = Math.Round(quotient, 3).ToString();
                contributor.Square = Math.Round(square, 3).ToString();
                contributor.Units = unit.ToString();
                contributor.Type = "B";
                contributor.SerialNumber = wod.PieceOfEquipment.SerialNumber;
                contributor.CalibrationRole = "Resolution";
                contributor.Comment = "";
                contributor.Contributors = "";
                contributor.Divisor = divRes.ToString();
                contributor.ControlNumber = wod.PieceOfEquipment.PieceOfEquipmentID;
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
                totales.ExpandedUncertainty = Math.Round(expandedUncertainty, 3).ToString();

                uncertaintyView.Contributors = contributors;
                uncertaintyView.totales = totales;

                expUncertainty = expandedUncertainty;

                

                ///Uncertainty CMC Values
                ///

                string uomAbreviation = null;
                if (wod.PieceOfEquipment != null && wod.PieceOfEquipment.UnitOfMeasure != null)
                {
                    uomAbreviation = wod.PieceOfEquipment.UnitOfMeasure.Abbreviation;
                }
                if (caltype != null && caltype.CMCValues != null && caltype.CMCValues.Count > 0)
                {

                    bool replace = false;
                    var nominalValue = load;

                    foreach (var item in caltype.CMCValues)
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
    }
}
