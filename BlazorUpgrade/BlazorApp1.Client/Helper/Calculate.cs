using Blazor.IndexedDB.Framework;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp1.Blazor.Blazor.Helper
{
    public class Calculate
    {

        //[Inject] CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany AppState { get; set; }

        //private readonly IUOMService<CallContext> UOM;

        [Inject]
        IIndexedDbFactory DbFactory { get; set; }

        public Calculate()
        {
            //UOM = _UOM(DbFactory);

        }

        public Calculate(Func<IIndexedDbFactory, CalibrationSaaS.Application.Services.IUOMService<CallContext>> _UOM)
        {
            //UOM = _UOM(DbFactory);

        }


        public Calculate(IUOMService<CallContext> _UOM)
        {
            //UOM = _UOM;

        }






        //public async Task<double>   ConversionUOM(int iniID, double value, int finalID)
        //{



        //    if (UOM != null)
        //    {
        //        UOMServiceGRPC uom = new UOMServiceGRPC(UOM);

        //        UnitOfMeasure ini = new UnitOfMeasure();
        //        ini.UnitOfMeasureID = iniID;

        //        ini = await uom.GetByID(ini);

        //        UnitOfMeasure final = new UnitOfMeasure();
        //        final.UnitOfMeasureID = iniID;

        //        final = await uom.GetByID(final);

        //       var result= ConversionUOM(ini, value, final);

        //        return result;
        //    }

        //    throw new Exception("NO UOM Service present");

        //}


        //public static async Task<double> ConversionUOM(int iniID, double value, int finalID, int f=0)
        //{



        //    //if (UOM != null)
        //    //{
        //    //    UOMServiceGRPC uom = new UOMServiceGRPC(UOM);

        //    //    UnitOfMeasure ini = new UnitOfMeasure();
        //    //    ini.UnitOfMeasureID = iniID;

        //    //    ini = await uom.GetByID(ini);

        //    //    UnitOfMeasure final = new UnitOfMeasure();
        //    //    final.UnitOfMeasureID = iniID;

        //    //    final = await uom.GetByID(final);

        //    //    var result = ConversionUOM(ini, value, final);

        //    //    return result;
        //    //}

        //    throw new Exception("NO UOM Service present");

        //}


        //public static double ConversionUOM(UnitOfMeasure ini, double value, UnitOfMeasure final)
        //{



        //    if(ini == null || final == null)
        //    {
        //        ini = new UnitOfMeasure();

        //      //ini=  Calculate.Conversion<UnitOfMeasure>(ini,
        //      //    arg, AppState.UnitofMeasureList, nameof(test.UnitOfMeasurementOut.UnitOfMeasureID));


        //        return 0;
        //    }

        //    if (ini.UnitOfMeasureID ==0 || final.UnitOfMeasureID==0)
        //    {
        //        throw new Exception("Unit of Measure not Zero");

        //    }

        //    if(ini.UnitOfMeasureID == final.UnitOfMeasureID)
        //    {
        //        return value;
        //    }

        //    if((ini.ConversionValue ==0 || final.ConversionValue == 0 ))
        //    {
        //        //Logger.LogDebug("Conversion Value not zero");
        //        throw new Exception("Conversion Value not zero");

        //    }


        //    if (ini.TypeID !=final.TypeID)
        //    {

        //        //Logger.LogDebug("The units of measure " + ini.Name + " and " + final.Name + " are from different unit types, a conversion can't be done");
        //        throw new Exception("The units of measure " + ini.Name + " and " + final.Name + " are from different unit types, a conversion can't be done");             
        //    }



        //    var result= (value * ini.ConversionValue) * (1 / final.ConversionValue);

        //    return result;
        //}


        //public static T Conversion<T>(T context, ChangeEventArgs arg, IEnumerable<T> list, string propertyName) where T :new()
        //{

        //    if (string.IsNullOrEmpty(propertyName))
        //    {
        //        return context;
        //    }

        //    var resu = arg.Value.ToString();

        //    var res = list.Where(x => (x.GetType().GetProperty(propertyName).GetValue(x, null)).ToString() == resu).FirstOrDefault();

        //    if(context== null)
        //    {
        //        context = new T();
        //    }

        //    context.CopyPropertiesFrom(res);

        //    return context;
        //}

        //public static T Conversion<T>(T context, string resu, IEnumerable<T> list, string propertyName) where T : new()
        //{

        //    if (string.IsNullOrEmpty(propertyName))
        //    {
        //        return context;
        //    }

        //    //var resu = arg.Value.ToString();

        //    var res = list.Where(x => (x.GetType().GetProperty(propertyName).GetValue(x, null)).ToString() == resu).FirstOrDefault();

        //    if (context == null)
        //    {
        //        context = new T();
        //    }

        //    context.CopyPropertiesFrom(res);

        //    return context;
        //}

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task<Linearity> InlineCalculate(List<WeightSet> Weights, Linearity origin, string? customer = null)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            Linearity lin = new Linearity();

            //UOMServiceGRPC uom = new UOMServiceGRPC(UOM);

            double weightApplied = 0;
            double tolerance = 10;

           
            foreach (var Item in Weights)
            {
                double weightApplied_ = 0;
                if (Item != null && customer == "LTI")
                {
                    weightApplied_ = Item.WeightActualValue;
                }
                else
                {
                    weightApplied_ = Item.WeightNominalValue;
                }

                weightApplied += weightApplied_;

                // await   uom.Conversion(Item.UnitOfMeasure, Item.UnitOfMeasure, Item.UnitOfMeasure, new UnitOfMeasure());
            }


            lin.WeightSets = Weights;
            lin.Tolerance = tolerance;

            return lin;

        }

        public Repeatability InlineCalculateRepeatibility(List<WeightSet> Weights, Repeatability origin)
        {

            Repeatability lin = new Repeatability();



            return lin;

        }

        public Eccentricity CalculateEccentricity(List<WeightSet> Weights, Eccentricity origin)
        {

            Eccentricity lin = new Eccentricity();



            return origin;

        }

        public bool LinearityValidation(List<WeightSet> Weights, object origin)
        {
            return true;

        }


        public static decimal CalculateResolution(decimal Capacity, TestPoint _TestPoint, decimal value)
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

        //public static decimal CalculateResolution(decimal value)
        //{


        //    int[] bits = Decimal.GetBits(value);
        //    ulong lowInt = (uint)bits[0];
        //    ulong midInt = (uint)bits[1];
        //    int exponent = (bits[3] & 0x00FF0000) >> 16;
        //    int result = exponent;
        //    ulong lowDecimal = lowInt | (midInt << 32);
        //    while (result > 0 && (lowDecimal % 10) == 0)
        //    {
        //        result--;
        //        lowDecimal /= 10;
        //    }

        //    return result;


        //}

        public static TestPoint CalculateTestPoint(double value, TestPoint _TestPoint, List<RangeTolerance> range,
            double ETpercentage, double ETresolution, double DecimalNumber, double Tolerance = -1)
        {


            if (Tolerance == -1)
            {
                var n = value;



                if (range != null && range.Count > 0)
                {
                    var r = range.Where(x => (double)_TestPoint.NominalTestPoit >= x.MinValue
                    && (double)_TestPoint.NominalTestPoit <= x.MaxValue).FirstOrDefault();

                    if (r != null)
                    {
                        var per = (r.Percent * _TestPoint.NominalTestPoit / 100);

                        _TestPoint.UpperTolerance = _TestPoint.NominalTestPoit + per;

                        _TestPoint.LowerTolerance = _TestPoint.NominalTestPoit - per;
                    }
                    else
                    {

                        n = n * ETpercentage / 100;

                        _TestPoint.UpperTolerance = _TestPoint.NominalTestPoit + n;

                        _TestPoint.LowerTolerance = _TestPoint.NominalTestPoit - n;

                    }


                }
                else
                {

                    var tol = n * ETpercentage / 100;


                    _TestPoint.UpperTolerance = n + tol;

                    _TestPoint.LowerTolerance = n - tol;

                    _TestPoint.NominalTestPoit = n;

                }
                return _TestPoint;

            }
            else
            {

                return _TestPoint;

            }
#pragma warning disable CS0162 // Se detectó código inaccesible
            return _TestPoint;
#pragma warning restore CS0162 // Se detectó código inaccesible

        }



    }

    public static class ObjectExtensionMethods
    {

        //public static double ConversionMethod(this WeightSet item, TestPoint TestPoint, List<UnitOfMeasure> UnitofMeasureList, out string UOMABB)
        //{
        //    double result = 0;
        //    //foreach (var item in SelectItem)
        //    //{
        //    if (item.UnitOfMeasure != null && item.UnitOfMeasure.UnitOfMeasureID > 0
        //        && TestPoint.UnitOfMeasurementOut != null
        //        && TestPoint.UnitOfMeasurementOut.UnitOfMeasureID > 0)
        //    {
        //        UOMABB = TestPoint.UnitOfMeasurementOut.Abbreviation;
        //        result = Calculate.ConversionUOM(item.UnitOfMeasure, item.WeightNominalValue, TestPoint.UnitOfMeasurementOut);
        //    }
        //    else
        //    {


        //        item.UnitOfMeasure = Calculate.Conversion<UnitOfMeasure>(item.UnitOfMeasure,
        //          item.UnitOfMeasureID.ToString(), UnitofMeasureList, nameof(item.UnitOfMeasureID));

        //        TestPoint.UnitOfMeasurementOut = Calculate.Conversion<UnitOfMeasure>(TestPoint.UnitOfMeasurementOut,
        //         TestPoint.UnitOfMeasurementOutID.ToString(), UnitofMeasureList, nameof(TestPoint.UnitOfMeasurement.UnitOfMeasureID));

        //        UOMABB = TestPoint.UnitOfMeasurementOut.Abbreviation;
        //        result = Calculate.ConversionUOM(item.UnitOfMeasure, item.WeightNominalValue, TestPoint.UnitOfMeasurementOut);

        //    }

        //    //}
        //    return result;
        //}


        //public static void CopyPropertiesFrom(this object self, object parent)
        //{
        //    var fromProperties = parent.GetType().GetProperties();
        //    var toProperties = self.GetType().GetProperties();

        //    foreach (var fromProperty in fromProperties)
        //    {
        //        foreach (var toProperty in toProperties)
        //        {
        //            if (fromProperty.Name == toProperty.Name && fromProperty.PropertyType == toProperty.PropertyType)
        //            {
        //                toProperty.SetValue(self, fromProperty.GetValue(parent));
        //                break;
        //            }
        //        }
        //    }
        //}
    }

}
