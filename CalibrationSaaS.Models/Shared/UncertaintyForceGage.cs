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
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace CalibrationSaaS.Domain.Aggregates.Shared
{
    public class MyLoggerFactory
    {
        //public ILogger CreateLogger(string categoryName)
        //{
        //    var loggerFactory = LoggerFactory.Create(static builder =>
        //    {
        //        builder
        //            .AddFilter("Microsoft", LogLevel.Warning)
        //            .AddFilter("System", LogLevel.Warning)
        //            .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug);
                    
        //    });

        //    return loggerFactory.CreateLogger(categoryName);
        //}
    }

    public  class UncertaintyForceGage: IFormula

    {
        private readonly ILoggerFactory log;
        public UncertaintyForceGage(ILoggerFactory _log)
        {

            log = _log;

        }

        public UncertaintyForceGage()
        {

           

        }

        public async Task<object> FormulaMethod(dynamic gcrObject, PieceOfEquipment poe, CalibrationType calibrationType)
        {
                    
            double expUncertainty = 0;
            
            double expandedUncertainty = 0;
            UncertaintyViewModel uncertaintyView = new UncertaintyViewModel();
         
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<dynamic, GenericCalibration>();
            }, log);

            IMapper mapper = config.CreateMapper();
            GenericCalibration genericCalibration = mapper.Map<GenericCalibration>(gcrObject);

            try
            {
                JObject jObject = new JObject();
                if (gcrObject.WeightSets != null)
                {
                    double resolution = gcrObject.BasicCalibrationResult.Resolution;
                    genericCalibration.WeightSets = new List<WeightSet>();
                    foreach (WeightSet dynamicWeight in gcrObject.WeightSets)
                    {
                        //var config2 = new MapperConfiguration(cfg =>
                        //{
                        //    cfg.CreateMap<dynamic, WeightSet>();
                        //});

                        var config2 = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<dynamic, WeightSet>()
                               .ForMember(dest => dest.WeightValue, opt => opt.MapFrom(src => dynamicWeight.WeightValue))
                               .ForMember(dest => dest.UnitOfMeasureID, opt => opt.MapFrom(src => dynamicWeight.UnitOfMeasureID))
                               .ForMember(dest => dest.CalibrationUncertValue, opt => opt.MapFrom(src => dynamicWeight.CalibrationUncertValue))
                               .ForMember(dest => dest.Resolution, opt => opt.MapFrom(src => dynamicWeight.Resolution))
                               .ForMember(dest => dest.UncertaintyUnitOfMeasureId, opt => opt.MapFrom(src => dynamicWeight.UncertaintyUnitOfMeasureId))
                               .ForMember(dest => dest.UnitOfMeasureID, opt => opt.MapFrom(src => dynamicWeight.UnitOfMeasureID))
                               .ForMember(dest => dest.WeightActualValue, opt => opt.MapFrom(src => dynamicWeight.WeightActualValue))
                               .ForMember(dest => dest.WeightNominalValue, opt => opt.MapFrom(src => dynamicWeight.WeightNominalValue))
                               .ForMember(dest => dest.WeightNominalValue2, opt => opt.MapFrom(src => dynamicWeight.WeightNominalValue2))
                               .ForMember(dest => dest.WeightSetID, opt => opt.MapFrom(src => dynamicWeight.WeightSetID))
                               .ForMember(dest => dest.PieceOfEquipmentID, opt => opt.MapFrom(src => dynamicWeight.PieceOfEquipmentID))
                               // Agregar más propiedades aquí...
                               ;
                        },log);

                        IMapper mapper2 = config2.CreateMapper();
                        WeightSet weight = mapper2.Map<WeightSet>(dynamicWeight);

                        
                        UnitOfMeasure unitOfMeasure = new UnitOfMeasure();
var config3 = new MapperConfiguration(cfg => {
                            cfg.CreateMap<dynamic, UnitOfMeasure>();
                        },log);

                        IMapper mapper3 = config3.CreateMapper();
                         unitOfMeasure = mapper.Map<UnitOfMeasure>(dynamicWeight.UnitOfMeasure);


                        //UnitOfMeasure unitOfMeasureUncert = new UnitOfMeasure();
                        //var config4 = new MapperConfiguration(cfg => {
                        //    cfg.CreateMap<dynamic, UnitOfMeasure>();
                        //});

                        //IMapper mapper4 = config4.CreateMapper();
                        //unitOfMeasureUncert = mapper.Map<UnitOfMeasure>(dynamicWeight.UnitOfMeasure.UncertaintyUnitOfMeasure);


                        weight.UnitOfMeasure = new UnitOfMeasure();
                        weight.UnitOfMeasure = unitOfMeasure;


                        genericCalibration.WeightSets.Add(weight);
                    }



                    //genericCalibration.WeightSets.Add();

                    List<Contributor> contributors = new List<Contributor>();
                    Contributor contributor = new Contributor();
                    //1
                    //var load = Convert.ToDouble((string)obj1["Load"]);
                    var uncertainty = genericCalibration.WeightSets.FirstOrDefault().CalibrationUncertValue; //Convert.ToDouble((string)obj1["Uncertanty"]);
                    var uomUncertainty = genericCalibration.WeightSets.FirstOrDefault().UnitOfMeasure.UncertaintyUnitOfMeasure.Abbreviation;
                    //double resolution = genericCalibration.WeightSets.FirstOrDefault().Resolution;//Convert.ToDouble((string)obj1["StepResol"]);
                    string unit = "";

                    switch (uomUncertainty)
                    {
                        case "lb":
                            uomUncertainty = "lbf";
                            break;
                        case "kg":
                            uomUncertainty = "kgf";
                            break;
                        case "oz":
                            uomUncertainty = "ozf";
                            break;
                        case "g":
                            uomUncertainty = "gf";
                            break;
                    }


                    double magnitude = uncertainty;

                    double divisor = 2;
                    var quotient = magnitude / divisor;
                    var square = Math.Round(quotient * quotient, 3);

                    contributor.Magnitude = Math.Round(magnitude, 3).ToString();
                    contributor.Distribution = "Expanded";
                    contributor.Quotient = Math.Round(quotient, 3).ToString();
                    contributor.Square = Math.Round(square, 3).ToString();
                    contributor.Units = uomUncertainty;
                    contributor.Type = "A";
                    contributor.SerialNumber = "";
                    contributor.CalibrationRole = "";
                    contributor.Comment = "";
                    contributor.Contributors = "Uncertainty";
                    contributor.Divisor = divisor.ToString();
                    contributor.ControlNumber = genericCalibration.WeightSets.FirstOrDefault().PieceOfEquipmentID;
                    contributors.Add(contributor);


                    //Deviation
                    double magnitudeDev = Math.Abs(genericCalibration.WeightSets.FirstOrDefault().WeightNominalValue - genericCalibration.WeightSets.FirstOrDefault().WeightActualValue);

                    double divisorDev = 1.73;
                    var quotientDev = magnitudeDev / divisorDev;
                    var squareDev = Math.Round(quotientDev * quotientDev, 3);
                    contributor = new Contributor();
                    contributor.Magnitude = Math.Round(magnitudeDev, 3).ToString();
                    contributor.Distribution = "Rectangular";
                    contributor.Quotient = Math.Round(quotientDev, 3).ToString();
                    contributor.Square = Math.Round(squareDev, 3).ToString();
                    contributor.Units = uomUncertainty;
                    contributor.Type = "B";
                    contributor.SerialNumber = "";
                    contributor.CalibrationRole = "";
                    contributor.Comment = "";
                    contributor.Contributors = "Deviation";
                    contributor.Divisor = divisorDev.ToString();
                    contributor.ControlNumber = genericCalibration.WeightSets.FirstOrDefault().PieceOfEquipmentID;
                    contributors.Add(contributor);

                    ////2                
                    ////Repeatability
                    //var divRep = Math.Sqrt(2);
                    //var repeat = 0;//Convert.ToDouble((string)obj1["Repeat"]);
                    //magnitude = repeat;
                    //quotient = magnitude / divRep;
                    //square = Math.Round(quotient * quotient, 3);

                    //contributor = new Contributor();
                    //contributor.Magnitude = Math.Round(magnitude, 3).ToString();
                    //contributor.Distribution = "Normal";
                    //contributor.Quotient = Math.Round(quotient, 3).ToString();
                    //contributor.Square = Math.Round(square, 3).ToString();
                    //contributor.Units = unit.ToString();
                    //contributor.Type = "B";
                    //contributor.SerialNumber = "";
                    //contributor.CalibrationRole = "";
                    //contributor.Comment = "";
                    //contributor.Contributors = "";
                    //contributor.Divisor = divRep.ToString();
                    //contributors.Add(contributor);


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
                    contributor.Units = uomUncertainty;
                    contributor.Type = "B";
                    contributor.SerialNumber = "";
                    contributor.CalibrationRole = "";
                    contributor.Comment = "";
                    contributor.Contributors = "Resolution";
                    contributor.Divisor = divRes.ToString();
                    contributor.ControlNumber = genericCalibration.WeightSets.FirstOrDefault().PieceOfEquipmentID;
                    contributors.Add(contributor);

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

                    expUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(expandedUncertainty))); 



                    string json = JsonConvert.SerializeObject(uncertaintyView);

                    // Convertimos el JSON a un objeto JObject
                    jObject = JObject.Parse(json);
                }


            }
            catch (Exception ex)
            {
                return 0;
            }
            return expUncertainty.ValidDouble();

        }

        public async Task<object> FormulaMethodToReport(dynamic gcrObject)
        {

            double expUncertainty = 0;

            double expandedUncertainty = 0;
            UncertaintyViewModel uncertaintyView = new UncertaintyViewModel();

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<dynamic, GenericCalibration>();
            }, null);

                    IMapper mapper = config.CreateMapper();
            GenericCalibration genericCalibration = mapper.Map<GenericCalibration>(gcrObject);
            JObject jObject = new JObject();
            try
            {
                UnitOfMeasure unitOfMeasure = new UnitOfMeasure();

                if (gcrObject.WeightSets != null)
                {
                    double resolution = gcrObject.BasicCalibrationResult.Resolution;
                    genericCalibration.WeightSets = new List<WeightSet>();
                    foreach (WeightSet dynamicWeight in gcrObject.WeightSets)
                    {
                        //var config2 = new MapperConfiguration(cfg =>
                        //{
                        //    cfg.CreateMap<dynamic, WeightSet>();
                        //});

                        var config2 = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<dynamic, WeightSet>()
                               .ForMember(dest => dest.WeightValue, opt => opt.MapFrom(src => dynamicWeight.WeightValue))
                               .ForMember(dest => dest.UnitOfMeasureID, opt => opt.MapFrom(src => dynamicWeight.UnitOfMeasureID))
                               .ForMember(dest => dest.CalibrationUncertValue, opt => opt.MapFrom(src => dynamicWeight.CalibrationUncertValue))
                               .ForMember(dest => dest.Resolution, opt => opt.MapFrom(src => dynamicWeight.Resolution))
                               .ForMember(dest => dest.UncertaintyUnitOfMeasureId, opt => opt.MapFrom(src => dynamicWeight.UncertaintyUnitOfMeasureId))
                               .ForMember(dest => dest.UnitOfMeasureID, opt => opt.MapFrom(src => dynamicWeight.UnitOfMeasureID))
                               .ForMember(dest => dest.WeightActualValue, opt => opt.MapFrom(src => dynamicWeight.WeightActualValue))
                               .ForMember(dest => dest.WeightNominalValue, opt => opt.MapFrom(src => dynamicWeight.WeightNominalValue))
                               .ForMember(dest => dest.WeightNominalValue2, opt => opt.MapFrom(src => dynamicWeight.WeightNominalValue2))
                               .ForMember(dest => dest.WeightSetID, opt => opt.MapFrom(src => dynamicWeight.WeightSetID))
                               .ForMember(dest => dest.PieceOfEquipmentID, opt => opt.MapFrom(src => dynamicWeight.PieceOfEquipmentID))
                               // Agregar más propiedades aquí...
                               ;
                        },log);

                        IMapper mapper2 = config2.CreateMapper();
                        WeightSet weight = mapper2.Map<WeightSet>(dynamicWeight);


                        
                        var config3 = new MapperConfiguration(cfg => {
                            cfg.CreateMap<dynamic, UnitOfMeasure>();
                        },log);

                        IMapper mapper3 = config3.CreateMapper();
                        unitOfMeasure = mapper.Map<UnitOfMeasure>(dynamicWeight.UnitOfMeasure);


                        //UnitOfMeasure unitOfMeasureUncert = new UnitOfMeasure();
                        //var config4 = new MapperConfiguration(cfg => {
                        //    cfg.CreateMap<dynamic, UnitOfMeasure>();
                        //});

                        //IMapper mapper4 = config4.CreateMapper();
                        //unitOfMeasureUncert = mapper.Map<UnitOfMeasure>(dynamicWeight.UnitOfMeasure.UncertaintyUnitOfMeasure);


                        weight.UnitOfMeasure = new UnitOfMeasure();
                        weight.UnitOfMeasure = unitOfMeasure;


                        genericCalibration.WeightSets.Add(weight);
                    }



                    //genericCalibration.WeightSets.Add();




                    List<Contributor> contributors = new List<Contributor>();
                    Contributor contributor = new Contributor();
                    //1
                    //var load = Convert.ToDouble((string)obj1["Load"]);
                    var uncertainty = genericCalibration.WeightSets.FirstOrDefault().CalibrationUncertValue; //Convert.ToDouble((string)obj1["Uncertanty"]);
                    string uomUncertainty = "";
                    if (unitOfMeasure != null && unitOfMeasure.Abbreviation != null)
                     uomUncertainty = unitOfMeasure.Abbreviation;//genericCalibration.WeightSets.FirstOrDefault().UnitOfMeasure.UncertaintyUnitOfMeasure.Abbreviation;
                    //double resolution = genericCalibration.WeightSets.FirstOrDefault().Resolution;//Convert.ToDouble((string)obj1["StepResol"]);
                    string unit = "";

                    switch (uomUncertainty)
                        {
                        case "lb":
                            uomUncertainty = "lbf";
                            break;
                        case "kg":
                            uomUncertainty = "kgf";
                            break;
                        case "oz":
                            uomUncertainty = "ozf";
                            break;
                        case "g":
                            uomUncertainty = "gf";
                            break;
                    }
                        

                    double magnitude = uncertainty;

                    double divisor = 2;
                    var quotient = magnitude / divisor;
                    var square = Math.Round(quotient * quotient, 3);

                    contributor.Magnitude = Math.Round(magnitude, 3).ToString();
                    contributor.Distribution = "Expanded";
                    contributor.Quotient = Math.Round(quotient, 3).ToString();
                    contributor.Square = Math.Round(square, 3).ToString();
                    contributor.Units = uomUncertainty;
                    contributor.Type = "A";
                    contributor.SerialNumber = "";
                    contributor.CalibrationRole = "";
                    contributor.Comment = "";
                    contributor.Contributors = "Uncertainty";
                    contributor.Divisor = divisor.ToString();
                    contributor.ControlNumber = genericCalibration.WeightSets.FirstOrDefault().PieceOfEquipmentID;
                    contributors.Add(contributor);


                    //Deviation
                    double magnitudeDev = Math.Abs(genericCalibration.WeightSets.FirstOrDefault().WeightNominalValue - genericCalibration.WeightSets.FirstOrDefault().WeightActualValue);

                    double divisorDev = 1.73;
                    var quotientDev = magnitudeDev / divisorDev;
                    var squareDev = Math.Round(quotientDev * quotientDev, 3);
                    contributor = new Contributor();
                    contributor.Magnitude = Math.Round(magnitudeDev, 3).ToString();
                    contributor.Distribution = "Rectangular";
                    contributor.Quotient = Math.Round(quotientDev, 3).ToString();
                    contributor.Square = Math.Round(squareDev, 3).ToString();
                    contributor.Units = uomUncertainty;
                    contributor.Type = "B";
                    contributor.SerialNumber = "";
                    contributor.CalibrationRole = "";
                    contributor.Comment = "";
                    contributor.Contributors = "Deviation";
                    contributor.Divisor = divisorDev.ToString();
                    contributor.ControlNumber = genericCalibration.WeightSets.FirstOrDefault().PieceOfEquipmentID;
                    contributors.Add(contributor);

                    ////2                
                    ////Repeatability
                    //var divRep = Math.Sqrt(2);
                    //var repeat = 0;//Convert.ToDouble((string)obj1["Repeat"]);
                    //magnitude = repeat;
                    //quotient = magnitude / divRep;
                    //square = Math.Round(quotient * quotient, 3);

                    //contributor = new Contributor();
                    //contributor.Magnitude = Math.Round(magnitude, 3).ToString();
                    //contributor.Distribution = "Normal";
                    //contributor.Quotient = Math.Round(quotient, 3).ToString();
                    //contributor.Square = Math.Round(square, 3).ToString();
                    //contributor.Units = unit.ToString();
                    //contributor.Type = "B";
                    //contributor.SerialNumber = "";
                    //contributor.CalibrationRole = "";
                    //contributor.Comment = "";
                    //contributor.Contributors = "";
                    //contributor.Divisor = divRep.ToString();
                    //contributors.Add(contributor);


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
                    contributor.Units = uomUncertainty;
                    contributor.Type = "B";
                    contributor.SerialNumber = "";
                    contributor.CalibrationRole = "";
                    contributor.Comment = "";
                    contributor.Contributors = "Resolution";
                    contributor.Divisor = divRes.ToString();
                    contributor.ControlNumber = genericCalibration.WeightSets.FirstOrDefault().PieceOfEquipmentID;
                    contributors.Add(contributor);

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

                    expUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(expandedUncertainty))); ;



                    string json = JsonConvert.SerializeObject(uncertaintyView);

                    // Convertimos el JSON a un objeto JObject
                    jObject = JObject.Parse(json);

                    
                }




            }
            catch (Exception ex)
            {
                return 0;
            }
            return jObject;

        }

    }
}
