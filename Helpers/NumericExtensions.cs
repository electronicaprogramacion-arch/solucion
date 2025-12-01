using Helpers.Controls;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using Helpers;
using Helpers.Models;
using System.Reflection.Metadata.Ecma335;

namespace Helpers
{
    public enum RoundType
    {
        RoundToResolution,
        Normal,
        Ceiling,
        Floor,
        Without
    }


  




        public class JavaScriptFunctions
    {
        
        public bool IsFullScale;

        public bool warning(dynamic tolerance, params double[] values)
        {

            double toleranceLow = ToleranceLow(tolerance, values);
            double toleranceMax = ToleranceMax(tolerance, values);
            double testPoint = values[1];

            if (testPoint >= toleranceLow && testPoint <= toleranceMax)
            {
                return true;
            }
            else
            {
                return false;
            }


        }
        public double ToleranceLow(dynamic tolerance, params double[] values)
        {
            // Validar si el parámetro tolerance es un string JSON y convertirlo al objeto Tolerance
            if (tolerance is string toleranceJson)
            {
                try
                {
                    // Intentar deserializar el JSON como un objeto dinámico primero
                    var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(toleranceJson);
                    if (jsonObject != null)
                    {
                        tolerance = jsonObject;
                    }
                }
                catch (Newtonsoft.Json.JsonException)
                {
                    // Si no es un JSON válido, mantener el valor original
                }
            }

            if (tolerance.FullScale == null)
                IsFullScale = false;
            else
                IsFullScale = tolerance.FullScale;

            switch (tolerance.ToleranceTypeID)
            {
                case 100:
                    return Accuracy1(tolerance, 1, values);
                case 101:
                    return Accuracy2(tolerance, 1, values);
                case 102:
                    return Accuracy3(tolerance, 1, values);
                case 103:
                    return Accuracy4(tolerance, 1, values);
                case 1:
                    return Accuracy9(tolerance, 1, values);
                case 200:
                    return Accuracy10(tolerance, 1, values);
                case 201:
                    return Accuracy11(tolerance, 1, values);
                case 3:
                    return Accuracy11(tolerance, 1, values);
            }

            return 0;
        }
        public double ToleranceLow(dynamic tolerance, params string[] values)
        {

            if (tolerance.FullScale == null)
                IsFullScale = false;
            else
                IsFullScale = tolerance.FullScale;

           

            return 0;
        }

        public string OutOfTolerance(params object[] values)
        {
            try
            {

                if (values == null || values.Length < 4)
                {
                    return "*";
                }


                double back = Math.Round(Convert.ToDouble(values[0]), 5);
                double nominal = Math.Round(Convert.ToDouble(values[1]), 5);
                double minTolerance = Math.Round(Convert.ToDouble(values[2]), 5);
                double maxTolerance = Math.Round(Convert.ToDouble(values[3]), 5);

                // El objeto en la posición 5 (índice 4) puede ser cualquier tipo, incluyendo JSON
                object additionalObject = values.Length > 4 ? values[4] : null;


                double lowerLimit = Math.Round(nominal - minTolerance, 5);
                double upperLimit = Math.Round(nominal + maxTolerance, 5);


                if (back >= lowerLimit && back <= upperLimit)
                {
                    return " ";
                }
                else
                {
                    return "*";
                }
            }
            catch (Exception ex)
            {
                // En caso de error, devolver asterisco
                return "*";
            }
        }
        public double ToleranceMax(dynamic tolerance, params double[] values)
        {
            // Validar si el parámetro tolerance es un string JSON y convertirlo al objeto Tolerance
            if (tolerance is string toleranceJson)
            {
                try
                {
                    // Intentar deserializar el JSON como un objeto dinámico primero
                    var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(toleranceJson);
                    if (jsonObject != null)
                    {
                        tolerance = jsonObject;
                    }
                }
                catch (Newtonsoft.Json.JsonException)
                {
                    // Si no es un JSON válido, mantener el valor original
                }
            }

            if (tolerance.FullScale == null)
                IsFullScale = false;
            else
                IsFullScale = tolerance.FullScale;

            switch (tolerance.ToleranceTypeID)
            {
                case 100:
                    return Accuracy1(tolerance, 2, values);
                case 101:
                    return Accuracy2(tolerance, 2, values);
                case 102:
                    return Accuracy3(tolerance, 2, values);
                case 103:
                    return Accuracy4(tolerance, 2, values);
                case 1:
                    return Accuracy9(tolerance, 2, values);
                case 200:
                    return Accuracy10(tolerance, 2, values);
                case 201:
                    return Accuracy11(tolerance, 2, values);

            }

            return 0;
        }

        public string ToleranceRange(dynamic tolerance, params double[] values)
        {
           
            if (tolerance is string toleranceJson )
            {
                try
                {
                   
                    var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(toleranceJson);
                    if (jsonObject != null)
                    {
                        tolerance = jsonObject;
                        tolerance.FullScale = false;
                    }
                }
                catch (Newtonsoft.Json.JsonException)
                {
                   
                }
            }
            if (tolerance.FullScale == null)
                IsFullScale = false;
            else
                IsFullScale = tolerance.FullScale;

            double minTolerance = 0;
            double maxTolerance = 0;
            double resolution = tolerance.Resolution;
            int toleranceType = tolerance.ToleranceTypeID;
            switch (toleranceType)
            {
                case 100:
                    minTolerance = Accuracy1(tolerance, 1, values);
                    maxTolerance = Accuracy1(tolerance, 2, values);
                    break;
                case 101:
                    minTolerance = Accuracy2(tolerance, 1, values);
                    maxTolerance = Accuracy2(tolerance, 2, values);
                    break;
                case 102:
                    minTolerance = Accuracy3(tolerance, 1, values);
                    maxTolerance = Accuracy3(tolerance, 2, values);
                    break;
                case 103:
                    minTolerance = Accuracy4(tolerance, 1, values);
                    maxTolerance = Accuracy4(tolerance, 2, values);
                    break;
                case 1:
                    minTolerance = Accuracy9(tolerance, 1, values);
                    maxTolerance = Accuracy9(tolerance, 2, values);
                    break;
                case 200:
                    minTolerance = Accuracy10(tolerance, 1, values);
                    maxTolerance = Accuracy10(tolerance, 2, values);
                    break;
                case 201:
                    minTolerance = Accuracy11(tolerance, 1, values);
                    maxTolerance = Accuracy11(tolerance, 2, values);
                    break;
             
                default:
                    return "Invalid ToleranceTypeID";
            }

            int decimalPlaces = GetDecimalPlaces(resolution);

            // Redondear
            minTolerance = Math.Round(minTolerance, decimalPlaces);
            maxTolerance = Math.Round(maxTolerance, decimalPlaces);


            if (values.Length > 1)
            {
               
                var valEnter = values[1];
                if (valEnter >= minTolerance && valEnter <= maxTolerance)
                {
                    return " ";

                }
                else
                {
                    return "X";
                }
            }
            else
            {
                return $"{minTolerance} to {maxTolerance}";

            }
            

            
        }
        private int GetDecimalPlaces(double number)
        {
            string numberString = number.ToString(System.Globalization.CultureInfo.InvariantCulture);
            int index = numberString.IndexOf(".");
            if (index < 0)
                return 0;
            return numberString.Length - index - 1;
        }
        public double ToleranceHB44(dynamic tolerance, params double?[] values)
        {
            try
            {
                double nominalTestPointValue = 0;

                if (values[0].HasValue)
                {
                    nominalTestPointValue = values[0].Value;
                }

                double resolution = 0;
                if (values[1].HasValue)
                {
                    resolution =values[1].Value;
                }
               



                bool isComercial = Convert.ToBoolean(values[2]);
                int ClassHB44 = 0;

                if (values[3].HasValue)
                {
                    ClassHB44=(int)values[3].Value;
                }




                int op = 0;
                if (values[4].HasValue)
                {
                  op=  (int)values[4].Value;
                }


                double multiplier = 0;
                if (values[5].HasValue)
                {
                    multiplier = values[5].Value;
                }


                double tolerance_ = 0;
                double grads = nominalTestPointValue / resolution;

                if (double.IsNaN(grads))
                    grads = 0;
                int toleranceGrads = grads.GetGradsHB44(ClassHB44);
                double toleranceValue = (double)toleranceGrads * resolution;

                double ToleranceMin = 0;
                double ToleranceMax = 0;
                double ToleranceMinAsLeft = 0;
                double ToleranceMaxAsLeft = 0;
                if (isComercial)
                {
                    if (toleranceValue > resolution)
                    {

                        ToleranceMin = nominalTestPointValue - toleranceValue;

                        ToleranceMax = nominalTestPointValue + toleranceValue;
                    }
                    else
                    {
                        ToleranceMin = nominalTestPointValue - resolution; // minTolerance - workOrderDetail.Resolution;
                        ToleranceMax = nominalTestPointValue + resolution;//maxTolerance + workOrderDetail.Resolution;
                    }




                    int acceptanceToleranceGrads = toleranceGrads / 2;
                    double toleranceAcceptanceValue = (double)acceptanceToleranceGrads * resolution;

                    if (toleranceAcceptanceValue > resolution)
                    {

                        ToleranceMinAsLeft = nominalTestPointValue - toleranceAcceptanceValue;
                        ToleranceMaxAsLeft = nominalTestPointValue + toleranceAcceptanceValue;
                    }
                    else
                    {
                        ToleranceMinAsLeft = nominalTestPointValue - resolution;
                        ToleranceMaxAsLeft = nominalTestPointValue + resolution;
                    }
                }
                else
                {
                    if (multiplier == 0)
                    {
                        multiplier = 1;
                    }
                    if ((toleranceValue * multiplier) > resolution)
                    {

                        ToleranceMin = nominalTestPointValue - (toleranceValue * multiplier);
                        ToleranceMinAsLeft = nominalTestPointValue - (toleranceValue * multiplier);

                        ToleranceMax = nominalTestPointValue + (toleranceValue * multiplier);
                        ToleranceMaxAsLeft = nominalTestPointValue + (toleranceValue * multiplier);
                    }
                    else
                    {
                        ToleranceMin = nominalTestPointValue - resolution;
                        ToleranceMinAsLeft = nominalTestPointValue - resolution;
                        ToleranceMax = nominalTestPointValue + resolution;
                        ToleranceMaxAsLeft = nominalTestPointValue + resolution;
                    }

                   ;
                }

                //Random rnd = new Random();
                //var r = rnd.Next(1, 1000);

                //return r;// op;

                switch (op)
                {
                    case 1:
                        return ToleranceMin;
                    case 2:
                        return ToleranceMax;
                    case 3:
                        return ToleranceMinAsLeft;
                    case 4:
                        return ToleranceMaxAsLeft;

                }

                return 0;
            }
            catch (Exception ex)
            {
//                Console.WriteLine("ToleranceHB44 " + ex.Message);

                return 0;
            }
           

        }
            public double ToleranceResult(dynamic tolerance, params double[] values)
        {
            try
            { 


            double tolLow = values[0];
            double tolMax = values[1];
                 
                double Tolerance;
                
                Tolerance = values[2];
                if (tolLow <= Tolerance && tolMax >= Tolerance)
                    return 1;
                else
                    return 0;
           }
            catch (Exception ex)
                {
                return 0;
            }


        }
      
        public string ToleranceResultHB44(params double[] values)
        {
            try
            {


                double tolLow = values[0];
                double tolMax = values[1];
                
                double nominal = values[2];


                if (tolLow <= nominal && tolMax >= nominal)
                    return "Pass";
                else
                    return "Fail";
            }
            catch (Exception ex)
            {
                return "Fail";
            }


        }

        //Acuracy %

        private double Accuracy1(dynamic tolerance, int ToleranceRange, params double[] values)
        {
            
            double Accuracy = tolerance.AccuracyPercentage;

            double toleranceLow = 0;
            if (!IsFullScale)
            {

                double TestPoint = values[0];
                if (ToleranceRange == 1)
                    toleranceLow = TestPoint - (TestPoint * Accuracy) / 100;
                else
                    toleranceLow = TestPoint + (TestPoint * Accuracy) / 100;

            }
            else
            {
               
                double TestPointMax = tolerance.MaxValue;
                double TestPoint = values[0];
                if (ToleranceRange == 1)
                    toleranceLow = TestPoint - ((TestPointMax * Accuracy) / 100);
                else
                    toleranceLow = TestPoint + ((TestPointMax * Accuracy) / 100);
            }

            return toleranceLow;
        }

        private double Accuracy2(dynamic tolerance, int ToleranceRange, params double[] values)
        {
            double toleranceLow = 0;
            double Accuracy = tolerance.AccuracyPercentage;
            double TestPointMax = 0;
            double TestPoint = values[0];
            double valFijo = tolerance.ToleranceFixedValue;
            if (!IsFullScale)
            {


                if (ToleranceRange == 1)
                    toleranceLow = TestPoint - (((TestPoint * Accuracy) / 100) + valFijo);
                else
                    toleranceLow = TestPoint + (((TestPoint * Accuracy) / 100) + valFijo);

            }
            else
            {

                 TestPointMax = tolerance.MaxValue;

                if (ToleranceRange == 1)
                    toleranceLow = TestPoint - (((TestPointMax * Accuracy) / 100) + valFijo);
                else
                    toleranceLow = TestPoint + (((TestPointMax * Accuracy) / 100) + valFijo);
            }


            return toleranceLow;
        }

        private double Accuracy3(dynamic tolerance, int ToleranceRange, params double[] values)
        {
            double toleranceLow = 0;
            double Accuracy = tolerance.AccuracyPercentage;
            
            double TestPoint = values[0];
            double valFijo = tolerance.ToleranceFixedValue;
            if (!IsFullScale)
            {


                if (ToleranceRange == 1)
                    toleranceLow = TestPoint - (((TestPoint * Accuracy) / 100) * valFijo);
                else
                    toleranceLow = TestPoint + (((TestPoint * Accuracy) / 100) * valFijo);

            }
            else
            {

                double TestPointMax = tolerance.MaxValue;
                if (ToleranceRange == 1)
                    toleranceLow = TestPoint - (((TestPointMax * Accuracy) / 100) * valFijo);
                else
                    toleranceLow = TestPoint + (((TestPointMax * Accuracy) / 100) * valFijo);
            }


            return toleranceLow;
        }
        private double Accuracy4(dynamic tolerance, int ToleranceRange, params double[] values)
        {
            double toleranceLow;
            double Accuracy = tolerance.AccuracyPercentage;
            double resolution = tolerance.Resolution;
           
            double TestPoint = values[0];
         
            if (!IsFullScale)
            {


                if (ToleranceRange == 1)
                    toleranceLow = TestPoint - (((TestPoint * Accuracy) / 100) + resolution);
                else
                    toleranceLow = TestPoint + (((TestPoint * Accuracy) / 100) + resolution);

            }
            else
            {
                double TestPointMax = tolerance.MaxValue;

                if (ToleranceRange == 1)
                    toleranceLow = TestPoint - (((TestPointMax * Accuracy) / 100) + resolution);
                else
                    toleranceLow = TestPoint + (((TestPointMax * Accuracy) / 100) + resolution);
            }


           

            return toleranceLow;
        }

        //Resolution
        private double Accuracy9(dynamic tolerance, int ToleranceRange, params double[] values)
        {
            double resolution = tolerance.Resolution;
            double toleranceLow;


            double TestPoint = values.FirstOrDefault();

            if (ToleranceRange == 1)
                toleranceLow = TestPoint - resolution;
            else
                toleranceLow = TestPoint + resolution;
            return toleranceLow;
        }
        
        private double Generic(dynamic tolerance, int ToleranceRange, params double[] values)
        {
            double resolution = tolerance.Resolution;
            double toleranceLow;


            double TestPoint = values.FirstOrDefault();
            if (ToleranceRange == 1)
                toleranceLow = TestPoint - resolution;
            else
                toleranceLow = TestPoint + resolution;
            return toleranceLow;
        }
        private double Accuracy10(dynamic tolerance, int ToleranceRange, params double[] values)
        {
            double resolution = tolerance.Resolution;
            double toleranceLow;
            double fixedValue = tolerance.ToleranceFixedValue;

            double TestPoint = values.FirstOrDefault();

            if (ToleranceRange == 1)
                toleranceLow = TestPoint - (resolution + fixedValue);
            else
                toleranceLow = TestPoint + (resolution + fixedValue);

            return toleranceLow;
        }
        private double Accuracy11(dynamic tolerance, int ToleranceRange, params double[] values)
        {
            double resolution = tolerance.Resolution;
            double toleranceLow;
            double fixedValue = tolerance.ToleranceFixedValue;

            double TestPoint = values.FirstOrDefault();
            if (ToleranceRange == 1)
                toleranceLow = TestPoint - (resolution * fixedValue);
            else
                toleranceLow = TestPoint + (resolution * fixedValue);

            return toleranceLow;
        }


        public double Error(params double[] values)
        {
            var sd = Average(values);

            double res = 0;

            res = Math.Abs(sd - values.Max());

            return res.ValidDouble();
        }

        public double ErrorAvg(double Value, double Value2)
        {


            var error = Math.Abs(Value - Value2);

            return error.ValidDouble();
        }


        public double Repetability(params double[] values)
        {
            var sd = StandardDeviation(values);

            double res = 0;

            res = sd / Math.Sqrt(values.Count());

            return res.ValidDouble();
        }

        public double MIN(params double[] values)
        {
            if (values == null || values.Count() == 0) return 0;


            double min = 0;

            if (values.Any())
            {
                min = values.Min();
            }



            return min;

        }


        public double MAX(params double[] values)
        {
            if (values == null || values.Count() == 0) return 0;


            double max = 0;

            if (values.Any())
            {
                max= values.Max();  
            }



            return max;

        }

        public double StandardDeviation(params double[] values)
        {
            if (values == null || values.Count() == 0) return 0;


            double standardDeviation = 0;

            if (values.Any())
            {
                // Compute the average.     
                double avg = values.Average();

                // Perform the Sum of (value-avg)_2_2.      
                double sum = values.Sum(d => Math.Pow(d - avg, 2));

                // Put it all together.      
                standardDeviation = Math.Sqrt((sum) / (values.Count() - 1));
            }

            return standardDeviation;
        }

        public double StandardDeviationAbs(params double[] values)
        {
            

            return Math.Abs(StandardDeviation(values));
        }


        public double AverageHBW(double Load, double Diam, double BallDiam)
        {

            if (Diam == 0 || Load == 0)
            {
                return 0;
            }

            var HBW = (2 * Load) / (Diam * Math.PI * (Diam - Math.Sqrt(((Diam * Diam) - (BallDiam * BallDiam)))));

            return HBW.ValidDouble();


        }

        public double Average(params double[] val)
        {


            if (val == null || val.Count() == 0) return 0;

            //double sum = 0; 
            //foreach (var item in val)
            //{
            //  sum=sum + item;
            //}

            //var res=sum/(val.Count());

            return val.Average();

        }


        public string Coordinates(double valorDecimal)
        {
            
            // Validar el valor decimal
            //if (isNaN(valorDecimal))
            //{
            //    gradosInput.value = 0;
            //    minutosInput.value = 0;
            //    segundosInput.value = 0;
            //    return;
            //}


            // Calcular grados, minutos y segundos
            var grados = Math.Floor(valorDecimal.ValidDouble());
            var minutosDecimal = (valorDecimal.ValidDouble() - grados) * 60;
            var minutos = Math.Floor(minutosDecimal);

            var segundos = (minutosDecimal - minutos) * 60;
            string resul= grados.ToString() + "'" + minutos.ToString() + "''" + segundos.ToString("N4"); 
            return resul;   
            // Mostrar las coordenadas en los campos de entrada
            //gradosInput.value = grados;
            //minutosInput.value = minutos;
            //segundosInput.value = segundos.toFixed(4); // Limitar los segundos a 4 decimales



        }
        


        public bool ValidTolerance(double Nominal, double run1, int ToleranceTypeID, double Resolution, double AccuracyPercentage, string? pass)
        {
            //if (run1 == -1)
            //{ 
            //    var message = "In";
            //}

            double nominal = Nominal;
            double run = Math.Abs(run1);
            int? toleranceType = ToleranceTypeID;
            double toleranceLow = 0;
            double toleranceMax = 0;
            if ((pass == "0.5" || pass == "1") && (pass != null))
            {
                pass = "PASS";
            }
            else if ((pass == "1" || pass == "2" || pass == "3" || pass == "NA") && (pass != null))
            {
                pass = "FAIL";
            }

            

                if (pass != null && pass?.ToUpper() == "PASS" && nominal == 0 )
                {
                    return true;
                }
                else if (pass != null && pass?.ToUpper() == "FAIL" && nominal == 0  )
                {
                    return false;
                }
            
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


            if (run >= toleranceLow && run <= toleranceMax)
            {
                return true;
            }
            else
            {
                return false;
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


    }


        public static class NumericExtensions
    {


        public static string LabelString(string Key,string Default="", string Lang = "")
        {

            return Key;
        
        }


        public static string ColumnWidth(string column)
        {

            if (string.IsNullOrEmpty(column))
            {
                return "";
            }

            string widthcolumn = "";


            string patron = @"(?:- *)?\d+(?:\.\d+)?";
                string operacion = column;
                Regex regex = new Regex(patron);


                var nume = regex.Matches(operacion).FirstOrDefault();

                string nume2 = "";

                foreach (Match m in regex.Matches(operacion))
                {
                    if (m.Success)
                    {
                        nume2 = nume2 + m.Value;
                    }

                }

                if (!string.IsNullOrEmpty(nume2))
                {

                    int.TryParse(nume2, out int number);
                    number = Math.Abs(number);

                    if (number > 0)
                    {
                        widthcolumn = (number * 9).ToString() + "%";
                    }
                    else
                    {
                        widthcolumn = "10%";
                    }
                }

            if (!string.IsNullOrEmpty(widthcolumn))
            {
                return "width: " + widthcolumn;
            }
            return "";
            
        }




        public static bool TryParseDouble(this object value, out double? parsed)
        {
            parsed = null;
            try
            {
                if (value == null)
                    return true;

                double parsedValue;

                parsed = double.TryParse(value.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out  parsedValue) ? (double?)parsedValue : null;
                //parsed = double.TryParse(value.ToString(), out parsedValue) ? (double?)parsedValue : null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static int GetInts(string text)
        {
            char[] chars = text.ToCharArray();
            bool prevIsNum = false;
            int startI = 0;
            int lenI = 0;
            List<char> ints = new List<char>();
            string res = "0";
            for (int i = 0; i < chars.Length; i++)
            {
                if (Char.IsNumber(chars[i]))
                {
                    if (prevIsNum)
                    {
                        lenI++;
                    }
                    else
                    {
                        startI = i;
                        lenI++;
                    }
                    prevIsNum = true;
                    ints.Add(chars[i]);
                }
                else 
                {
                    prevIsNum = false;
                    
                }
            }

            foreach (var item in ints)
            {
                res = res + item;
            }

            return Convert.ToInt32(res);
        }

        static Random ar = new Random(100);
        static Random br = new Random(1000);
        static Random cr = new Random(1000);
        static int IDCont = 1;
        public static int GetUniqueID()
        {


            var now = DateTime.Now;
            var zeroDate = DateTime.MinValue.AddHours(now.Hour).AddMinutes(now.Minute).AddSeconds(now.Second).AddMilliseconds(now.Millisecond);
            int uniqueId = Math.Abs((int)((zeroDate.Ticks + ar.Next()) - br.Next() / 10000));
            //int res = (uniqueId + id) / cr.Next();
            return uniqueId;

        }
        public static int? GetUniqueID(int? id)
        {

            if (id > 0 && id.HasValue)
            {
                return id.Value;
            }
            else if(id.HasValue)
            {
                return GetUniqueID(id);
            }
            return GetUniqueID();

        }
        public static int GetUniqueID(int id)
        {

            if(id > 0)
            {
                return id;
            }
            else
            {
                return GetUniqueID();
            }
        }

        public static int GetUniqueID(string id1)
        {
            var id = GetInts(id1);

            if (id > 0)
            {
                return id;
            }
            else
            {
                return GetUniqueID();
            }

        }


            public static string RoundedFunction(this string currentString, string StepResol, int DecimalNumbers, RoundType DecimalRoundType, string TypeValue, bool EnableToastMessage=true)
        {

            string _currentValueTemp = "";
            bool WasRounded = false ;

            bool ToastLunch=false;

            string CssClassDecimal= "";

            bool ChangeBackground=false;

            
            try
            {
                //currentString = currentString.Replace(',', '.');

                if (StepResol.Contains(","))
                {
                    StepResol = StepResol.Replace(",", ".");
                }


                if (StepResol == "any" || StepResol == "0" || DecimalNumbers < 0)
                {
                    return convertToDecimal(currentString);
                }

                if (DecimalNumbers == 0  && StepResol != "any" && StepResol != "0")
                {
                    DecimalNumbers = NumericExtensions.CalculateDecimalNumber(Convert.ToDecimal(StepResol));
//                    Console.WriteLine("Calculate decimanumber");
                }


                if (double.TryParse(currentString, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedValue))
                {



                    if (DecimalRoundType == RoundType.Without)
                    {
                        parsedValue = parsedValue.ToStringNoTruncate(DecimalNumbers);
                        if (TypeValue == typeof(int).ToString() && parsedValue == 0 && EnableToastMessage
                            
                            && _currentValueTemp == currentString)
                        {
                            //ShowToast(ToastMessage, Blazed.Controls.Toast.ToastLevel.Warning).ConfigureAwait(false).GetAwaiter().GetResult();
                            _currentValueTemp = currentString;
                            WasRounded = true;
                        }


                    }



                    var mult = 1;
                    for (int iii = 0; iii < DecimalNumbers; iii++)
                    {
                        mult = mult * 10;
                    }

                    if (DecimalRoundType == RoundType.Ceiling)
                    {
                        //int vall = (int)parsedValue * mult;


                        parsedValue = Math.Ceiling(parsedValue);
                        parsedValue = parsedValue.ToStringNoTruncate(DecimalNumbers);
                    }



                    if (DecimalRoundType == RoundType.Normal)
                    {


                        parsedValue = Math.Round(parsedValue, DecimalNumbers);


                    }

                    if (DecimalRoundType == RoundType.Floor)
                    {


                        parsedValue = Math.Floor(parsedValue);
                        parsedValue = parsedValue.ToStringNoTruncate(DecimalNumbers);
                    }

                    if (!string.IsNullOrEmpty(StepResol) && StepResol != "any" && StepResol != "0" && double.TryParse(StepResol, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedValue3))
                    {

                        if (DecimalRoundType == RoundType.RoundToResolution)
                        {
                            parsedValue = (Math.Round(parsedValue / parsedValue3, MidpointRounding.AwayFromZero)) * parsedValue3; // 1.25
                            parsedValue = parsedValue.ToStringNoTruncate(DecimalNumbers);
                        }

                        if (EnableToastMessage  && (Convert.ToDouble(currentString) != parsedValue) && _currentValueTemp != currentString)
                        {
                            

                            ToastLunch = true;

                            _currentValueTemp = currentString;

                            WasRounded = true;

//                            Console.WriteLine("showtoast");
                        }

                        var cssinvalid = parsedValue.GetInvalidClass(parsedValue3, DecimalNumbers);

                        if (ChangeBackground && !string.IsNullOrEmpty(cssinvalid))
                        {
                            CssClassDecimal = cssinvalid;
                            //in Testpoint (weight) the typed value XX was rounded to XX



                        }
                        else
                        {
                            CssClassDecimal = "";
                        }

                    }


                    var parsedValue2 = parsedValue.ToString();

                    var position = parsedValue2.IndexOf(".");

                    var lengt = 0;

                    if (position > 0)
                        lengt = parsedValue2.Substring(position + 1).Length;

                    var dif = DecimalNumbers - lengt;




                    //if (parsedValue.IsInt())
                    if (dif > 0)
                    {
                        if (DecimalRoundType == RoundType.RoundToResolution)
                        {

                        }
                        var zeros = "";
                        for (int i = 0; i < dif; i++)
                        {
                            zeros = zeros + "0";
                        }
                        if (!string.IsNullOrEmpty(zeros) && position == -1)
                        {
                            parsedValue2 = parsedValue2 + "." + zeros;
                        }
                        else if (!string.IsNullOrEmpty(zeros) && position > 0)
                        {
                            parsedValue2 = parsedValue2 + zeros;

                        }

                    }

                    //var a = parsedValue2; //String.Format("{0:0.00}", parsedValue2);
                    if (WasRounded && ChangeBackground)
                    {
                        CssClassDecimal = CssClassDecimal + " warningcolor";
                    }

                    //currentString= parsedValue2;

                    return convertToDecimal(parsedValue2);


                }
                else
                {
                    return convertToDecimal(currentString);
                }


            }
            catch (Exception ex)
            {
//                Console.WriteLine("error textinput");
                return convertToDecimal(currentString);
            }



        }


        public static string convertToDecimal(this string item)
        {
            if (item != null && item.ToString().ToLower().Contains("e-"))
            {
                //var result1 = string.Format("{0:F5}", item); // CurrentValueAsString.ToString("N30");// String.Format("{0:N30}", CurrentValueAsString);//String.Format("{0:N30}", result1);

                decimal h2 = Decimal.Parse(item, System.Globalization.NumberStyles.Any);

                return h2.ToString();
            }

            return item;
        }


        public static bool IsNumeric(this object obj)
        {
            var negativeString = obj.ToString();
            double number;
            if (double.TryParse(negativeString, out number))
            {
                if (Math.Abs(number) > 0)
                {
                    return true;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsNumeric(this string obj)
        {
            var negativeString = obj;
            double number;
            if (double.TryParse(negativeString, out number))
            {
                if (Math.Abs(number) > 0)
                {
                    return true;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }


        public static bool IsInt(this string obj)
        {
            var negativeString = obj;
            int number;
            if (int.TryParse(negativeString, out number))
            {
                if (Math.Abs(number) > 0)
                {
                    return true;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        public static double standardDeviation(this IEnumerable<double> sequence)
        {
            double average = sequence.Average();
            double sum = sequence.Sum(d => Math.Pow(d - average, 2));
            return Math.Sqrt((sum) / sequence.Count());
        }
        /// <summary>
        /// Table 6 Algorithm
        /// </summary>
        /// <param name="grads"></param>
        /// <param name="classValue"></param>
        /// <returns></returns>
        public static int GetGradsHB44(this double grads, int classValue)
        {
            switch (classValue)
            {
                // "|"
                case 1:
                    if (grads >= 0 && grads <= 50000)
                        return 1;
                    if (grads > 50000 && grads <= 200000)
                        return 2;
                    if (grads > 200000)
                        return 3;
                    break;
                // "||"
                case 2:
                    if (grads >= 0 && grads <= 5000)
                        return 1;
                    if (grads > 5000 && grads <= 20000)
                        return 2;
                    if (grads > 20000)
                        return 3;
                    break;
                // "|||"
                case 3:
                    if (grads >= 0 && grads <= 500)
                        return 1;
                    if (grads > 500 && grads <= 2000)
                        return 2;
                    if (grads > 2000 && grads <= 4000)
                        return 3;
                    if (grads > 4000)
                        return 5;
                    break;
                // "||||"
                case 4:
                    if (grads >= 0 && grads <= 50)
                        return 1;
                    if (grads > 50 && grads <= 200)
                        return 2;
                    if (grads > 200 && grads <= 400)
                        return 3;
                    if (grads > 400)
                        return 5;
                    break;
                // "||| L"
                case 5:
                    if (grads >= 0 && grads <= 500)
                        return 1;
                    if (grads > 500 && grads <= 1000)
                        return 2;
                    // Adds one per each 500 or fraction
                    if (grads > 1000)
                        return (int.Parse(Math.Ceiling(grads / 500d).ToString()));
                    break;
                default:
                    throw new Exception("Invalid Class for HB44");
            }
            throw new Exception("Invalid range or class for HB44");
        }
    




         public  static bool requiresUncertantyConversion(this int? UncertaintyUnitOfMeasureID,int UnitOfMeasureID)
        {
            return (UncertaintyUnitOfMeasureID != UnitOfMeasureID);
        }



       

        public static double ValidDouble(this double value)
        { 
             if(!Double.IsNaN(value) && !Double.IsInfinity(value))
            {
                return value;
            }
            else
            {
                return 0;
            }
        
        }


        public static double ValidDouble(this object value2)
        {

            if (value2==null)
            {
//                Console.WriteLine("-----------------Value is Null ");
                return 0;
            }

            if (!value2.IsNumeric())
            {
//                Console.WriteLine("------------Value is Not numeric: " + value2.ToString());
                return 0;
            }


            double value = Convert.ToDouble(value2);

            if (!Double.IsNaN(value) && !Double.IsInfinity(value))
            {
                return value;
            }
            else
            {
                return 0;
            }

        }


        public static string GetSHA1Has(this string str)
        {
            MD5 md5 = MD5.Create();
            byte[] b = Encoding.ASCII.GetBytes(str);
            byte[] hash = md5.ComputeHash(b);

            StringBuilder sb = new StringBuilder();
            foreach (var a in hash)
                sb.Append(a.ToString("X2"));
            return sb.ToString();
        }
         public static int CalculateDecimalNumber(decimal value)
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

        public static T CreateObject<T>(string typeName, string assembly)
        {

            Type typeSource = GetType(typeName, assembly);
            object objTarget = Activator.CreateInstance(typeSource);

            return (T)objTarget;

        }

        public static object CreateObject(string typeName, string assembly)
        {

            Type typeSource = GetType(typeName, assembly);
            object objTarget = Activator.CreateInstance(typeSource);

            return objTarget;

        }



        public static Type GetType(string typeName,string assembly)
        {
            var type = Type.GetType(typeName + "," + assembly);
    if (type != null) return type;
    foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
    {
        type = a.GetType(typeName);
        if (type != null)
            return type;
    }
    return null;
        }

        // public static double Ceiling10(this double len)
        //{
        //    double number = Math.Round((len + 5) / 10.0) * 10.0;

        //    return number;

        //}

        public static int Ceiling10(this int len)
        {
            if((len % 10) == 0)
            {
                return len;
            }

            double number = Math.Round((len + 5) / 10.0) * 10.0;

            return Convert.ToInt32(number);

        }


        public static double TruncateDecimal(this double value, int precision)
        {
            double step = (double)Math.Pow(10, precision);
            double tmp = Math.Round(step * value);
            return tmp / step;
        }
        public static double ToStringNoTruncate(this double me, int decimalplaces = 1)
        {
            string result = me.ToString();

            if (decimalplaces == 0)
            {
                return me;
            }

            if(result.LastIndexOf('.') ==-1)
            {
                return me;
            }

            char dec = '.';//System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];

            var lengt = result.Substring(result.LastIndexOf('.') + 1).Length;

            if(lengt <= decimalplaces)
            {
                return me;
            }
            var resultRound = Math.Round(Convert.ToDecimal(result), decimalplaces).ToString();
            var a= resultRound.Substring(0, result.LastIndexOf(dec) + decimalplaces + 1);
//            Console.WriteLine("ToStringNoTruncate");
//            Console.WriteLine(a);
            if (string.IsNullOrEmpty(a))
            {
                a = "0.00";
            }
            return Convert.ToDouble(a);
        }



                public static string GetInvalidClass(this double a, double b, int decimalnumber)
        {
            var valid = 0d;
            if (b != 0)
            {
                valid = (a / b);
            }
            else
            {
                return "";
            }
            var c = true;
            if (Math.Abs(valid) > 0)
            {
                c = valid.IsInt(decimalnumber);//Math.Abs(valid % 1) <= (Double.Epsilon * 200);
            }

           


            var cssinvalid = "";
            if (!c)
            {
                cssinvalid = "inputinvalid";
            }
            return cssinvalid;
        }

        static int times = 0;
        public static bool IsInt(this double n1,int decimalnumber)
        {

            var n = n1.ToStringNoTruncate(decimalnumber);

            //var  c = Math.Abs(n % 1) <= (Double.Epsilon * 10000);

            //return c;
            var ha = n - (Int64)n == 0;

            if (!ha && times < 4)
            {
                times = times + 1;
                //ha= (n1 + 0.00000000000050d) - (Int64)n1 == 0;
               ha = IsInt((n1 + 0.00000000000150d), decimalnumber);
            }



            return ha;
        }


          public static string GetFields(this object cs, string value)
        {
            try
            {
               


                var result = ConvertObjectToDictionary(new ObjectConvertInfo
                {
                    ConvertObject = cs,
                    IgnoreProperties = new List<string> { "Ticks", "TotalMilliseconds", "TotalMinutes", "TotalSeconds" , "TotalHours", "Millisecond", "Milliseconds", "Minutes","Seconds" },
                    IgnoreTypes = new List<Type> { typeof(IntPtr), typeof(Delegate), typeof(Type) },
                    MaxDeep = 3
                });

                var myKeys = result.Where(x => x.Value != null && x.Value.ToString().ToLower().Contains(value.ToLower())).ToList();

                List<string> lst = new List<string>();
                foreach (var s in myKeys)
                {
                    var v = s.Key.LastIndexOf(".");
                    var enc = s.Value; //.SplitCamelCase() + ": ";
                    if (v > 0) {
                        var ss = s.Key.Substring(v + 1, (s.Key.Length - v) - 1);
                        lst.Add( ss.SplitCamelCase() + ": "+ enc);
                    }
                    else
                    {
                        lst.Add( s.Key.SplitCamelCase() + ": "+ enc );
                    }
                }

                string result2 = string.Join(",", lst);




                return result2;
            }
            catch (Exception ex)
            {
                return "";
            }
           
        }

        public static IEnumerable<T> DistinctBy2<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
        {
            return enumerable.GroupBy(keySelector).Select(grp => grp.First());
        }

       

        public static Dictionary<string, string> ConvertObjectToDictionary(ObjectConvertInfo objectConvertInfo)
        {
            try
            {
                var dictionary = new Dictionary<string, string>();
                MapToDictionaryInternal(dictionary, objectConvertInfo, objectConvertInfo.ConvertObject.GetType().Name, 0);
                return dictionary;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        
       public  static void MapToDictionaryInternal(IDictionary<string, string> dictionary, ObjectConvertInfo objectConvertInfo, string name, int deep)
        {
            try
            {
                if (deep > objectConvertInfo.MaxDeep)
                    return;

                var properties = objectConvertInfo.ConvertObject.GetType().GetProperties();
                foreach (var propertyInfo in properties)
                {
                    if (objectConvertInfo.IgnoreProperties.Contains(propertyInfo.Name))
                        continue;

                    var key = name + "." + propertyInfo.Name;
                    var value = propertyInfo.GetValue(objectConvertInfo.ConvertObject, null);
                    if (value == null)
                        continue;

                    var valueType = value.GetType();

                    if (objectConvertInfo.IgnoreTypes.Contains(valueType))
                        continue;

                    if (valueType.IsPrimitive || valueType == typeof(String))
                    {
                        dictionary[key] = value.ToString();
                    }
                    else if (value is IEnumerable)
                    {
                        var i = 0;
                        foreach (var data in (IEnumerable)value)
                        {
                            MapToDictionaryInternal(dictionary, new ObjectConvertInfo
                            {
                                ConvertObject = data,
                                IgnoreTypes = objectConvertInfo.IgnoreTypes,
                                IgnoreProperties = objectConvertInfo.IgnoreProperties,
                                MaxDeep = objectConvertInfo.MaxDeep
                            }, key + "[" + i + "]", deep + 1);
                            i++;
                        }
                    }
                    else
                    {
                        MapToDictionaryInternal(dictionary, new ObjectConvertInfo
                        {
                            ConvertObject = value,
                            IgnoreTypes = objectConvertInfo.IgnoreTypes,
                            IgnoreProperties = objectConvertInfo.IgnoreProperties,
                            MaxDeep = objectConvertInfo.MaxDeep
                        }, key, deep + 1);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        



        public static T Conversion<T>(T context, object obj, IEnumerable<T> list, string propertyName) where T : new()
        {

            if (string.IsNullOrEmpty(propertyName))
            {
                return context;
            }
            if(obj == null)
            {
                return context;
            }

            var resu = obj.ToString();

            var res = list.Where(x => (x.GetType().GetProperty(propertyName).GetValue(x, null)).ToString() == resu).FirstOrDefault();

            if (context == null)
            {
                context = new T();
            }

            context.CopyPropertiesFrom(res);

            return context;
        }

        public static ICollection<KeyValue> ConvertListStringinKeyValue(this ICollection<string> list)
        {

            if (list == null)
            {
                return null;
            }
            ICollection<KeyValue> list2 = new List<KeyValue>();
            int cont = 0;
            foreach (var item in list)
            {
                list2.Add(new KeyValue() { Value = cont, Key = item }); ;
            }
            return list2;

        }


        public static void CopyPropertiesFrom(this object self, object parent)
        {
            var fromProperties = parent.GetType().GetProperties();
            var toProperties = self.GetType().GetProperties();

            foreach (var fromProperty in fromProperties)
            {
                foreach (var toProperty in toProperties)
                {
                    if (fromProperty.Name == toProperty.Name && fromProperty.PropertyType == toProperty.PropertyType)
                    {
                        toProperty.SetValue(self, fromProperty.GetValue(parent));
                        break;
                    }
                }
            }
        }

        public static void CombineBoolPropertiesFrom(this object self, object parent)
        {
            var fromProperties = parent.GetType().GetProperties();
            var toProperties = self.GetType().GetProperties();

            ///
            ///
                var toPropertiesDict = toProperties
                .Where(p => p.PropertyType == typeof(bool))
                .ToDictionary(p => p.Name);
            ///


            foreach (var fromProperty in fromProperties)
            {
                //foreach (var toProperty in toProperties)
                //{
                //    if (fromProperty.Name == toProperty.Name && fromProperty.PropertyType == toProperty.PropertyType 
                //        && toProperty.PropertyType.Name== "System.Boolean")
                //    {
                //        var parentbool = fromProperty.GetValue(parent);

                //        var targetbool = fromProperty.GetValue(toProperty);

                //        var resull = Convert.ToBoolean(parentbool) || Convert.ToBoolean(targetbool);
                //        //if (parentbool != null)
                //        //{

                //        //}

                //        toProperty.SetValue(self, resull);
                //        break;
                //    }
                //}

                if (fromProperty.PropertyType == typeof(bool))
                {
                    // Intentar obtener la propiedad correspondiente en el objeto self
                    if (toPropertiesDict.TryGetValue(fromProperty.Name, out var toProperty))
                    {
                        var parentBool = fromProperty.GetValue(parent);
                        var targetBool = toProperty.GetValue(self);

                        var result = Convert.ToBoolean(parentBool) || Convert.ToBoolean(targetBool);

                        toProperty.SetValue(self, result);
                    }
                }
            }
        }


        public static object CloneObject(this object objSource)
        {
            if (objSource is null)
            {
                return null;
            }
            //Get the type of source object and create a new instance of that type
            Type typeSource = objSource.GetType();
            object objTarget = Activator.CreateInstance(typeSource);
            //Get all the properties of source object type
            PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            //Assign all source property to taget object 's properties
            foreach (PropertyInfo property in propertyInfo)
            {
                //Check whether property can be written to
                try
                {
                    if (property.CanWrite)
                    {
                        //check whether property type is value type, enum or string type
                        if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(typeof(System.String)))
                        {
                            property.SetValue(objTarget, property.GetValue(objSource, null), null);
                        }
                        //else property type is object/complex types, so need to recursively call this method until the end of the tree is reached
                        else
                        {
                            object objPropertyValue = property.GetValue(objSource, null);
                            if (objPropertyValue == null)
                            {
                                property.SetValue(objTarget, null, null);
                            }
                            else
                            {
                                property.SetValue(objTarget, objPropertyValue.CloneObject(), null);
                            }
                        }
                    }
                }
                catch
                {

                }
               
            }
            return objTarget;
        }

        public static object CloneObjectTestPointGroup(this object objSource)
        {
            if (objSource is null)
            {
                return null;
            }
            //Get the type of source object and create a new instance of that type
            Type typeSource = objSource.GetType();
            object objTarget = Activator.CreateInstance(typeSource);
            //Get all the properties of source object type
            PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            //Assign all source property to taget object 's properties
            foreach (PropertyInfo property in propertyInfo)
            {
                //Check whether property can be written to
                try
                {
                    if (property.CanWrite)
                    {
                        //check whether property type is value type, enum or string type
                        if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(typeof(System.String)))
                        {
                            property.SetValue(objTarget, property.GetValue(objSource, null), null);
                        }
                        //else property type is object/complex types, so need to recursively call this method until the end of the tree is reached
                        else
                        {
                            object objPropertyValue = property.GetValue(objSource, null);
                            if (objPropertyValue == null)
                            {
                                property.SetValue(objTarget, null, null);
                            }
                            else
                            {
                                property.SetValue(objTarget, objPropertyValue.CloneObject(), null);
                            }
                        }
                    }
                }
                catch
                {

                }

            }

            return objTarget;
        }



        public static object CloneObjectWithFormat(this object objSource)
        {
            //Get the type of source object and create a new instance of that type
            Type typeSource = objSource.GetType();
            object objTarget = Activator.CreateInstance(typeSource);
            //Get all the properties of source object type
            PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            //Assign all source property to taget object 's properties
            foreach (PropertyInfo property in propertyInfo)
            {
                //Check whether property can be written to
                try
                {
                    if (property.CanWrite)
                    {
                        //check whether property type is value type, enum or string type
                        if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(typeof(System.String)))
                        {
                            property.SetValue(objTarget, property.GetValue(objSource, null), null);
                        }
                        //else property type is object/complex types, so need to recursively call this method until the end of the tree is reached
                        else
                        {
                            object objPropertyValue = property.GetValue(objSource, null);
                            if (objPropertyValue == null)
                            {
                                property.SetValue(objTarget, null, null);
                            }
                            else
                            {
                                property.SetValue(objTarget, objPropertyValue.CloneObject(), null);
                            }
                        }
                    }
                }
                catch
                {

                }

            }
            return objTarget;
        }



       

        public static object CopyPropertiesFrom(this object parent)
        {
            object self = new object();
            CopyPropertiesFrom(self,parent);

            return self;
        
        }

                    public static string SplitCamelCase(this string input)
            {
                if (input == null) return null;
                if (string.IsNullOrWhiteSpace(input)) return "";

                var separated = input;

                separated = SplitCamelCaseRegex.Replace(separated, @" $1").Trim();

                //Set ALL CAPS words
                if (_SplitCamelCase_AllCapsWords.Any())
                    foreach (var word in _SplitCamelCase_AllCapsWords)
                        separated = SplitCamelCase_AllCapsWords_Regexes[word].Replace(separated, word.ToUpper());

                //Capitalize first letter
                var firstChar = separated.First(); //NullOrWhiteSpace handled earlier
                if (char.IsLower(firstChar))
                    separated = char.ToUpper(firstChar) + separated.Substring(1);

                return separated;
            }

            private static readonly Regex SplitCamelCaseRegex = new Regex(@"
            (
                (?<=[a-z])[A-Z0-9] (?# lower-to-other boundaries )
                |
                (?<=[0-9])[a-zA-Z] (?# number-to-other boundaries )
                |
                (?<=[A-Z])[0-9] (?# cap-to-number boundaries; handles a specific issue with the next condition )
                |
                (?<=[A-Z])[A-Z](?=[a-z]) (?# handles longer strings of caps like ID or CMS by splitting off the last capital )
            )"
                , RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace
            );

        //WebConfigurationManager.AppSettings["SplitCamelCase_AllCapsWords"] ?? 
        private static readonly string[] _SplitCamelCase_AllCapsWords =
                ("")
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(a => a.ToLowerInvariant().Trim())
                    .ToArray()
                    ;

            private static Dictionary<string, Regex> _SplitCamelCase_AllCapsWords_Regexes;
            private static Dictionary<string, Regex> SplitCamelCase_AllCapsWords_Regexes
            {
                get
                {
                    if (_SplitCamelCase_AllCapsWords_Regexes == null)
                    {
                        _SplitCamelCase_AllCapsWords_Regexes = new Dictionary<string, Regex>();
                        foreach (var word in _SplitCamelCase_AllCapsWords)
                            _SplitCamelCase_AllCapsWords_Regexes.Add(word, new Regex(@"\b" + word + @"\b", RegexOptions.Compiled | RegexOptions.IgnoreCase));
                    }

                    return _SplitCamelCase_AllCapsWords_Regexes;
                }
            }




    }
}