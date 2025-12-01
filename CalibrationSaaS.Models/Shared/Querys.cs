using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.Aggregates.Views;
using CalibrationSaaS.Models.ViewModels;
using DynamicExpressions;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Customer = CalibrationSaaS.Domain.Aggregates.Entities.Customer;
using WorkOrderDetailByCustomer = CalibrationSaaS.Domain.Aggregates.Views.WorkOrderDetailByCustomer;

namespace CalibrationSaaS.Domain.Aggregates.Querys
{


    /// <summary>
    /// Extension methods for the string data type
    /// </summary>
    public static class ConventionBasedFormattingExtensions
        {
            /// <summary>
            /// Turn CamelCaseText into Camel Case Text.
            /// </summary>
            /// <param name="input"></param>
            /// <returns></returns>
            /// <remarks>Use AppSettings["SplitCamelCase_AllCapsWords"] to specify a comma-delimited list of words that should be ALL CAPS after split</remarks>
            /// <example>
            /// wordWordIDWord1WordWORDWord32Word2
            /// Word Word ID Word 1 Word WORD Word 32 Word 2
            /// 
            /// wordWordIDWord1WordWORDWord32WordID2ID
            /// Word Word ID Word 1 Word WORD Word 32 Word ID 2 ID
            /// 
            /// WordWordIDWord1WordWORDWord32Word2Aa
            /// Word Word ID Word 1 Word WORD Word 32 Word 2 Aa
            /// 
            /// wordWordIDWord1WordWORDWord32Word2A
            /// Word Word ID Word 1 Word WORD Word 32 Word 2 A
            /// </example>

        }
 
    public static class QueryableExtensions2
    {
        

    

        public static string Completezero(string parsedValue2, int DecimalNumbers)
        {
          
            var position = parsedValue2.IndexOf(".");

            var lengt = 0;

            if (position > 0)
                lengt = parsedValue2.Substring(position + 1).Length;

            var dif = DecimalNumbers - lengt;

            if (dif > 0)
            {

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

            return parsedValue2;

        }

        public  static T Clone<T>(T source)
    {
       var EquipmentTemplateJSon = JsonSerializer.Serialize(source);

            var a = JsonSerializer.Deserialize<T>(EquipmentTemplateJSon);

            return a;

    }



        public static bool IsVisible(this object obj)
        {
            var value = obj.GetType().GetProperty("IsVisible").GetValue(obj,null);

            return (bool)value; ;

        }

         public static bool IsEnable(this object obj)
        {
            var value = obj.GetType().GetProperty("IsEnable").GetValue(obj,null);

            return (bool)value; ;

        }


         public static bool Valid<T>(this T model)
        {
            StringBuilder strValidationResults = new StringBuilder();
            ValidationContext context = new ValidationContext(model, null, null);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(model, context, validationResults, true);
            if (!valid)
            {
                foreach (ValidationResult validationResult in validationResults)
                {
                    strValidationResults.AppendLine(validationResult.ErrorMessage);
                }
                throw new InvalidCalSaaSModel(strValidationResults.ToString());
            }
            return valid;
        }

        public static bool PropertyExists(string path, Type t)
        {



            if (string.IsNullOrEmpty(path))
                return false;

            var pp = path.Split('.');

            foreach (var prop in pp)
            {
                if (int.TryParse(prop, out var result))
                {
                    /* skip array accessors */
                    continue;
                }

                var propInfo = t.GetMember(prop)
                    .Where(p => p is PropertyInfo)
                    .Cast<PropertyInfo>()
                    .FirstOrDefault();

                if (propInfo != null)
                {
                    t = propInfo.PropertyType;

                    if (t.GetInterfaces().Contains(typeof(IEnumerable)) && t != typeof(string))
                    {
                        t = t.IsGenericType
                            ? t.GetGenericArguments()[0]
                            : t.GetElementType();

                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public static T Validate<T>(this T clase, string message)
        {
            return clase;
        }


            public static int GetTotalPages(int rows, int PageSize)
        {
            int total = 0;
            if (rows > 0)
            {
                total = ((int)(rows / PageSize)) + 1;
            }
            else
            {
                total = 1;
            }

            return total;
        }

        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, Pagination<T> pagination)
        {
            if (pagination.Show == -1)
            {
                pagination.Show = 1000;
            }

            return queryable
                .Skip((pagination.Page - 1) * pagination.Show)
                .Take(pagination.Show);
        }

        //public static Expression<Func<T, object>> ToMemberOf<T>(this string name) where T : class
        //{


        //    var parameter = Expression.Parameter(typeof(T), "e");

        //    var propertyOrField = name.Split('.')
        //    .Aggregate((Expression)parameter, Expression.PropertyOrField);
        //    var unaryExpression = Expression.MakeUnary(ExpressionType.Convert, propertyOrField, typeof(object));

        //    return Expression.Lambda<Func<T, object>>(unaryExpression, parameter);
        //}

        public static Expression<Func<T, object>> ToMemberOf<T>(this string name) where T : class
        {
            try
            {
                var parameter = Expression.Parameter(typeof(T), "e");

                var propertyOrField = name.Split('.')
                    .Aggregate((Expression)parameter, (current, memberName) =>
                        GetCaseSensitiveMemberExpression(current, memberName));

                var unaryExpression = Expression.MakeUnary(ExpressionType.Convert, propertyOrField, typeof(object));

                return Expression.Lambda<Func<T, object>>(unaryExpression, parameter);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ToMemberOf " + ex.Message);
                return null;
            }
        }

        private static Expression GetCaseSensitiveMemberExpression(Expression instance, string memberName)
        {
            var instanceType = instance.Type;

            // First try to find a property with exact case match
            var property = instanceType.GetProperty(memberName, BindingFlags.Public | BindingFlags.Instance);
            if (property != null)
            {
                return Expression.Property(instance, property);
            }

            // If no property found, try to find a field with exact case match
            var field = instanceType.GetField(memberName, BindingFlags.Public | BindingFlags.Instance);
            if (field != null)
            {
                return Expression.Field(instance, field);
            }

            // If neither property nor field found, throw an exception
            throw new ArgumentException($"Member '{memberName}' not found on type '{instanceType.Name}' (case-sensitive search)");
        }

        public static async Task<ResultSet<T>> PaginationAndFilterQueryOff<T>(this IQueryable<T> MainQuery, Pagination<T> pagination,
            IQueryable<T> SimpleQuery, Expression<Func<T, bool>> Filter) where T : class
        {
            string filter = pagination.Filter;
            List<T> result = null;
            int rows = 0;

            var softdelete = QueryableExtensions2.PropertyExists("IsDelete", typeof(T));

            Expression<Func<T, bool>> predicate = null;

            if (softdelete)
            {
                predicate = DynamicExpressions.DynamicExpressions.GetPredicate<T>("IsDelete", FilterOperator.Equals, false);
            }


            if (pagination.Page == 1)
            {


                if (Filter == null || ((string.IsNullOrEmpty(filter) && pagination.Entity == null) && pagination.Object == null))
                {
                    if (softdelete)
                    {

                        rows = SimpleQuery
                        .Where(predicate)
                        .Count();
                    }
                    else
                    {
                        var count = SimpleQuery.Count();
                        rows = count;
                    }

                }
                else
                {
                    try
                    {
                        if (softdelete)
                        {

                            rows = SimpleQuery
                           .Where(Filter).Where(predicate)
                           .Count();
                        }
                        else
                        {
                            rows = SimpleQuery
                            .Where(Filter)
                            .Count();

                        }

                    }
                    catch (Exception ex)
                    {
                        throw new Exception("--------------------------------Error in filter implementation, please review--------------------------------------------------------");
                    }
                }
            }


            if (Filter == null || ((string.IsNullOrEmpty(filter) && pagination.Entity == null) && pagination.Object == null))
            {
                var queryable = MainQuery; 
                if (!string.IsNullOrEmpty(pagination.ColumnName))
                {
                    var order2 = pagination.ColumnName.ToMemberOf<T>();
                    if (pagination.SortingAscending)
                    {
                        if (softdelete)
                        {
                            queryable = queryable.Where(predicate).OrderBy(order2);
                          
                        }
                        else
                        {
                            queryable = queryable.OrderBy(order2);
                            
                        }

                    }
                    else
                    {
                        if (softdelete)
                        {
                            queryable = queryable.Where(predicate).OrderByDescending(order2);
                        }
                        else
                        {
                            queryable = queryable.OrderByDescending(order2);
                        }

                        
                    }
                }
                result = await Task.FromResult(queryable.Paginar(pagination).ToList());
            }
            else
            {
                IQueryable<T> queryable = null;

                if (softdelete)
                {
                    queryable = MainQuery
                    .Where(Filter).Where(predicate)
                    .AsQueryable();
                }
                else
                {
                    queryable = MainQuery
                    .Where(Filter)
                    .AsQueryable();
                }

                if (!string.IsNullOrEmpty(pagination.ColumnName))
                {
                    var order2 = pagination.ColumnName.ToMemberOf<T>();
                    if (pagination.SortingAscending)
                    {
                        queryable = queryable.OrderBy(order2);
                    }
                    else
                    {
                        queryable = queryable.OrderByDescending(order2);
                    }
                }
                result = await Task.FromResult(queryable.Paginar(pagination).ToList());
            }

            int total = QueryableExtensions2.GetTotalPages(rows, pagination.Show);

            return new ResultSet<T>
            {
                CurrentPage = pagination.Page,
                List = result,
                Count = rows,
                PageTotal = total,
                ColumnName = pagination.ColumnName,
                SortingAscending = pagination.SortingAscending
            };
        }



        public static TOutput SelectSingle<TInput, TOutput>(this TInput obj,Expression<Func<TInput, TOutput>> expression)
        {
            return expression.Compile()(obj);
        }


        public static double ConversionMethod(this WeightSet item, TestPoint TestPoint, List<UnitOfMeasure> UnitofMeasureList, out string UOMABB, string? customer = null)
        {
            double weightApplied = 0; 
            if (item != null && customer == "LTI")
            {
                weightApplied = item.WeightActualValue;
            }
            else
            {
                weightApplied = item.WeightNominalValue;
            }

            try
            {
                double result = 0;

                if (TestPoint == null)
                {
                    UOMABB = item.UnitOfMeasureID.GetUoM(UnitofMeasureList).Abbreviation;

                    return Convert.ToDouble(weightApplied);
                }

                if (item.UnitOfMeasure != null && item.UnitOfMeasure.UnitOfMeasureID > 0
                    && TestPoint.UnitOfMeasurementOut != null
                    && TestPoint.UnitOfMeasurementOut.UnitOfMeasureID > 0)
                {
                    UOMABB = TestPoint.UnitOfMeasurementOut.Abbreviation;
                    result = ConversionUOM(item.UnitOfMeasure, weightApplied, TestPoint.UnitOfMeasurementOut);
                }
                else
                {


                    item.UnitOfMeasure = Helpers.NumericExtensions.Conversion<UnitOfMeasure>(item.UnitOfMeasure,
                      item.UnitOfMeasureID.ToString(), UnitofMeasureList, nameof(item.UnitOfMeasureID));

                    TestPoint.UnitOfMeasurementOut = Helpers.NumericExtensions.Conversion<UnitOfMeasure>(TestPoint.UnitOfMeasurementOut,
                     TestPoint.UnitOfMeasurementOutID.ToString(), UnitofMeasureList, nameof(TestPoint.UnitOfMeasurement.UnitOfMeasureID));

                    UOMABB = TestPoint.UnitOfMeasurementOut.Abbreviation;
                    result = ConversionUOM(item.UnitOfMeasure, weightApplied, TestPoint.UnitOfMeasurementOut);

                }

                return result;

            }
            catch(Exception ex) 
            {
                throw new Exception("ConversionMethod " + ex.Message);

            } 
           
        }

         public static double ConvertToUOM(this double ConversionValue, double value, UnitOfMeasure final)
        {
         
            if(final == null)
            {
                return 0;
            }
            
            if (ConversionValue == 0 || final.ConversionValue == 0)
            {
                throw new Exception("Unit of meeasure Conversion Value not zero");
            }

            var result = (value * ConversionValue) * (1 / final.ConversionValue);

            return result;
        }

        public static int CalculateResolution(decimal value)
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


        public static double ConversionUOM(UnitOfMeasure ini, double value, UnitOfMeasure final)
        {
            int? iniuombase = null;

            int? finalbase = null;

            if (ini.UnitOfMeasureBaseID.HasValue)
            {
                iniuombase = ini.UnitOfMeasureBaseID.Value;
            }
            if (ini.UnitOfMeasureBaseID.HasValue==false && ini.UnitOfMeasureBase != null)
            {
                iniuombase = ini.UnitOfMeasureBase.UnitOfMeasureID;
            }


            if (final.UnitOfMeasureBaseID.HasValue)
            {
                finalbase = final.UnitOfMeasureBaseID.Value;
            }
            if (final.UnitOfMeasureBaseID.HasValue == false && final.UnitOfMeasureBase != null)
            {
                finalbase = final.UnitOfMeasureBase.UnitOfMeasureID;
            }


            if (ini == null || final == null)
            {
                ini = new UnitOfMeasure();

                return 0;
            }

            if (ini.UnitOfMeasureID == 0 || final.UnitOfMeasureID == 0)
            {
                throw new Exception("Unit of Measure not Zero");

            }

            if (ini.UnitOfMeasureID == final.UnitOfMeasureID)
            {
                return value;
            }

            if ((ini.ConversionValue == 0 || final.ConversionValue == 0))
            {

                throw new Exception("Conversion Value not zero");

            }


            if (ini.TypeID != final.TypeID && (iniuombase != finalbase || (iniuombase.HasValue==false || finalbase.HasValue==false)))
            {

 
                throw new Exception("The units of measure " + ini.Name + " and " + final.Name + " are from different unit types, a conversion can't be done");
            }

           


            var result = (value * ini.ConversionValue) * (1 / final.ConversionValue);

            return result;
        }

          public static double ConversionUOM(int? iniint, double value, int finalint,IEnumerable<UnitOfMeasure> uomlst)
        {

            if (!iniint.HasValue)
            {
                throw new Exception("Unit of Measure ini not null");
            }


            var ini = iniint.Value.GetUoM(uomlst);

            var final = finalint.GetUoM(uomlst);

            if (ini == null || final == null)
            {
                ini = new UnitOfMeasure();

//               Console.WriteLine("conversion 0");

                return 0;
            }

            if (ini.UnitOfMeasureID == 0 || final.UnitOfMeasureID == 0)
            {
                throw new Exception("Unit of Measure not Zero");

            }

            if (ini.UnitOfMeasureID == final.UnitOfMeasureID)
            {
                return value;
            }

            if ((ini.ConversionValue == 0 || final.ConversionValue == 0))
            {
                throw new Exception("Conversion Value not zero");

            }


            if (ini.TypeID != final.TypeID)
            {

                throw new Exception("The units of measure " + ini.Name + " and " + final.Name + " are from different unit types, a conversion can't be done");
            }



            var result = (value * ini.ConversionValue) * (1 / final.ConversionValue);

            return result;
        }
       


        public static UnitOfMeasure GetUoMFromAbb(this string Abbr, IEnumerable<UnitOfMeasure> list)
        {
            if (list == null)
            {
                return null;
            }

            var res = list.Where(x => x.Abbreviation.ToLower() == Abbr.ToLower()).FirstOrDefault();

            return res;
        }



        public static UnitOfMeasure  GetUoM(this int ID, IEnumerable<UnitOfMeasure> list, double? Capacity=null,double? Capacity2=null)
        {
//            Console.WriteLine("uom");

            if (ID == 0)
            {
                return new UnitOfMeasure() { Abbreviation="N/A" };
            }

            if(list == null)
            {
                return null;
            }

            var res = list.Where(x => x.UnitOfMeasureID==ID).FirstOrDefault();



            if (res != null && Capacity2.HasValue)
            {
                res.CapacityFull = Capacity.ToString() + " - " + Capacity2.Value.ToString() + " " + res.Abbreviation;
            }
            else if(res != null && Capacity.HasValue)
            {
                res.CapacityFull = Capacity.ToString() + " " + res.Abbreviation;
            }
            

            return res;
        }


        public static UnitOfMeasure GetUoMUncertainty(this int ID, IEnumerable<UnitOfMeasure> list)
        {
            if (list != null)
            {

                var res = list.Where(x => x.UnitOfMeasureID == ID).FirstOrDefault();

                

                return res;
            }
            else
            {
                return null;
            }
        }


        public static UnitOfMeasure GetUoM(this int? ID, IEnumerable<UnitOfMeasure> list, double Capacity,double? Capacity2=null)
        {
            if (ID.HasValue && list !=null) 
            { 

                   var res = list.Where(x => x.UnitOfMeasureID == ID).FirstOrDefault();
                
                if (res != null && Capacity2.HasValue)
                {
                    res.CapacityFull = Capacity.ToString() + " - " + Capacity2.Value.ToString() + " " + res.Abbreviation;
                }
                else if(res != null  )
                {
                    res.CapacityFull = Capacity.ToString() + " " + res.Abbreviation;
                }
                
            
                return res;
            }
            else
            {
                return null;
            }
        }

        public static UnitOfMeasure GetUoM(this int? ID, IEnumerable<UnitOfMeasure> list)
        {
            if (ID.HasValue && list !=null) 
            { 

            var res = list.Where(x => x.UnitOfMeasureID == ID).FirstOrDefault();

                if(res != null)
                {
                    res.CapacityFull =  res.Abbreviation;
                }
                
            
                return res;
            }
            else
            {
                return null;
            }
        }


        public static UnitOfMeasureType GetUoMType(this int ID, IEnumerable<UnitOfMeasureType> list)
        {


            var res = list.Where(x => x.Value == ID).FirstOrDefault();

            return res;
        }


        public static string GetStatus(this int ID, Dictionary<string,string> list)
        {
            try
            {
                var res = list.Where(x => x.Key == ID.ToString()).FirstOrDefault();

                return res.Value;
            }
            catch(Exception ex)
            {
                return "";
            }

           
        }
        public static string GetStatus(this int ID, IEnumerable<Status> list)
        {
            try
            {
                var res = list.Where(x => x.StatusId == ID).FirstOrDefault();

                return res.Name;
            }
            catch (Exception ex)
            {
                return "";
            }


        }


        public static string GetStatus(this string ID, Dictionary<string, string> list)
        {
            if (list ==null)
            {
                return "";
            }

            var res = list.Where(x => x.Key == ID.ToString()).FirstOrDefault();

            return res.Value;
        }

        public static string GetTypePhone(this string ID, Dictionary<string, string> list)
        {
            if (string.IsNullOrEmpty(ID))
            {
                return "";
            }

            var res = list.Where(x => x.Key == ID).FirstOrDefault();

            return res.Value;
        }


        public static string GetState(this string ID, IReadOnlyCollection<StateLocation> list)
        {
           

            var res = list.Where(x => x.Value == ID.ToString().Trim()).FirstOrDefault();
            if(res != null)
            {
                return res.Name;
            }
            return "";
            
        }

        public static string GetDistribution(this string ID, Dictionary<int, string> list)
        {
            if (list == null ||  string.IsNullOrEmpty(ID))
            {
                return "";
            }

            var res = list.Where(x => x.Key == Convert.ToInt32(ID.ToString())).FirstOrDefault();

            return res.Value;
        }

        public static string GetTypeUncert(this int ID, IEnumerable<WeightType> list)
        {
            if (list == null)
            {
                return "";
            }

            var res = list.Where(x => x.WeightTypeID == ID).FirstOrDefault();

            return res.Name;
        }

        public static string GetCertificateType(this string ID, IReadOnlyCollection<Certification> list)
        {


            var res = list.Where(x => x.CertificationID.ToString() == ID).FirstOrDefault();

            return res.Name;
        }


        public static bool GetIsBalance(this int ID, List<EquipmentType> list)
        {
            try
            {
                var res = list.Where(x => x.EquipmentTypeID == ID).FirstOrDefault();
                if(res != null)
                {
                    return res.IsBalance;
                }
                else
                {
                    return false;
                }

               
            }
            catch(Exception ex)
            {
                return false;
            }
           
        }


    }


    public partial class Querys
    {


        public static Expression<Func<PieceOfEquipment, bool>> POEIndicator(Pagination<PieceOfEquipment> pagination)
        {
            var poe = pagination.Entity.CustomerId;

            if (string.IsNullOrEmpty(pagination.Filter))
            {
                return null;
            }


            Expression<Func<PieceOfEquipment, bool>> exprTree = x =>
                   (x.SerialNumber != null && x.SerialNumber.ToLower().Contains(pagination.Filter.ToLower()))
                   || (x.EquipmentTemplate.Model != null && x.EquipmentTemplate.Model.ToLower().Contains(pagination.Filter.ToLower())
                   || (x.Customer != null && x.Customer.Name != null && x.Customer.Name.ToLower().Contains(pagination.Filter.ToLower()))
                   //|| (x.EquipmentTemplate.EquipmentTypeObject != null && x.EquipmentTemplate.EquipmentTypeObject.Name.ToLower().Contains(pagination.Filter.ToLower()))
                   ) ;

            return exprTree;

        }


        public static Expression<Func<PieceOfEquipment, bool>> POEBalanceByIndicator(string poe)
        {
            Expression<Func<PieceOfEquipment, bool>> exprTree = x =>  x.EquipmentTemplate.EquipmentTypeID == 3 && x.IndicatorPieceOfEquipmentID==poe;

            return exprTree;

        }


         public static Expression<Func<PieceOfEquipment, bool>> POEBalanceByPer(string poe)
        {
            Expression<Func<PieceOfEquipment, bool>> exprTree = x =>  1== 1 ;

            return exprTree;

        }




        public static Expression<Func<PieceOfEquipment, bool>> POEFilter(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                Expression<Func<PieceOfEquipment, bool>> exprTree = x => 1 == 1; 
                return exprTree;

            }
            else
            {
                Expression<Func<PieceOfEquipment, bool>> exprTree = x =>

                 //(!string.IsNullOrEmpty(x.SerialNumber ) && x.SerialNumber.ToLower().Contains(filter.ToLower()))
                 //|| (x.EquipmentTemplate != null && x.EquipmentTemplate.Model != null && x.EquipmentTemplate.Model.ToLower().Contains(filter.ToLower())
                 //|| (x.Customer != null && x.Customer.Name != null && x.Customer.Name.ToLower().Contains(filter.ToLower()))
                 //|| (x.EquipmentTemplate != null && x.EquipmentTemplate.EquipmentTypeObject != null && x.EquipmentTemplate.EquipmentTypeObject.Name.ToLower().Contains(filter.ToLower()))
                 //|| (!string.IsNullOrEmpty(x.PieceOfEquipmentID)  && x.PieceOfEquipmentID.ToLower().Contains(filter.ToLower()))
                 //|| (!string.IsNullOrEmpty(x.InstallLocation)  && x.InstallLocation.ToLower().Contains(filter.ToLower()))
                 //);

                 (
                   (!string.IsNullOrEmpty(x.PieceOfEquipmentID) && x.PieceOfEquipmentID.ToLower().Contains(filter.ToLower()))
                   || !string.IsNullOrEmpty(x.SerialNumber) && x.SerialNumber.ToLower().Contains(filter.ToLower()))
                   || (x.EquipmentTemplate != null && x.EquipmentTemplate.Model != null && x.EquipmentTemplate.Model.ToLower().Contains(filter.ToLower()))
                   || (x.Customer != null && x.Customer.Name != null && x.Customer.Name.ToLower().Contains(filter.ToLower()))
                   || (x.EquipmentTemplate != null && x.EquipmentTemplate.EquipmentTemplateID > 0 && x.EquipmentTemplate.EquipmentTemplateID.ToString().ToLower().Contains(filter.ToLower()))
                   //|| (x.EquipmentTemplate != null && x.EquipmentTemplate.EquipmentTypeObject != null && x.EquipmentTemplate.EquipmentTypeObject.Name.ToLower().Contains(filter.ToLower()))
                  
                   || (!string.IsNullOrEmpty(x.InstallLocation) && x.InstallLocation.ToLower().Contains(filter.ToLower()))
                   || (!string.IsNullOrEmpty(x.CustomerToolId) && x.CustomerToolId.ToLower().Contains(filter.ToLower()))
                   ;


                return exprTree;
            }

           


        }

        public static Expression<Func<PieceOfEquipment, bool>> POEFilterNew(Pagination<PieceOfEquipment> filter)
        {
            //if (string.IsNullOrEmpty(filter))
            //{
            //    return null;
            //}
            //filter = filter.ToLower();
            if (filter == null)
            {
                throw new Exception("No filter implemented" );
            }

            if (  filter.Object != null && filter.Object.Advanced && filter.Object.EntityFilter != null  && (filter.Filter == null || filter.Filter == ""))
            {
//                Console.WriteLine("filter object");
                if (filter.Object.EntityFilter.SerialNumber == null)
                    filter.Object.EntityFilter.SerialNumber = "NULL";
                else
                    filter.Object.EntityFilter.SerialNumber = filter.Object.EntityFilter.SerialNumber.ToLower();

                if (filter.Object.EntityFilter.Customer.Name == null)
                    filter.Object.EntityFilter.Customer.Name = "NULL";
                else
                    filter.Object.EntityFilter.Customer.Name = filter.Object.EntityFilter.Customer.Name.ToLower();



                Expression<Func<PieceOfEquipment, bool>> exprTree = x =>
                       //(x.Name != null && x.Name.ToLower().StartsWith(filter.ToLower()))
                       (filter.Object.EntityFilter.SerialNumber == "NULL" || x.SerialNumber.ToLower().Trim().Contains(filter.Object.EntityFilter.SerialNumber.Trim()))
                        && (filter.Object.EntityFilter.Customer.Name == "NULL" || x.Customer.Name.ToLower().Trim().Contains(filter.Object.EntityFilter.Customer.Name.Trim()))
                        && ((filter.Object.InitDate == null && filter.Object.EndDate == null) || x.DueDate >= filter.Object.InitDate && x.DueDate <= filter.Object.EndDate)
                       //&& (filter.Object.EntityFilter.EquipmentTemplate.EquipmentTypeID == 0 || x.EquipmentTemplate.EquipmentTypeID == filter.Object.EntityFilter.EquipmentTemplate.EquipmentTypeID);
                       && ((filter.Object.EntityFilter.EquipmentTemplate.EquipmentTypeGroupID.HasValue == true && x.EquipmentTemplate.EquipmentTypeGroupID == filter.Object.EntityFilter.EquipmentTemplate.EquipmentTypeGroupID) || filter.Object.EntityFilter.EquipmentTemplate.EquipmentTypeGroupID.HasValue==false)
                       ;
                


                return exprTree;
            }
            else
            {
//                 Console.WriteLine("filter string");
                return Querys.POEFilter(filter.Filter);


            }


        }

          public static Expression<Func<Uncertainty, bool>> UncertaintyFilter(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return null;
            }


            Expression<Func<Uncertainty, bool>> exprTree = x => 1 == 1;
            //(x.Name != null && x.Name.ToLower().StartsWith(filter.ToLower())) ||
                   

           return exprTree;

        }


        public static Expression<Func<T, bool>> POENullfilter<T>(string filter)
        {
            
                return null;
            
        }
            public static Expression<Func<PieceOfEquipment, bool>> POEWOFilter(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return null;
            }


            Expression<Func<PieceOfEquipment, bool>> exprTree = x => 
            //(x.Name != null && x.Name.ToLower().StartsWith(filter.ToLower())) ||
                    (x.SerialNumber != null && x.SerialNumber.ToLower().Contains(filter.ToLower()))
                    || (x.EquipmentTemplate.Model != null && x.EquipmentTemplate.Model.ToLower().Contains(filter.ToLower())
                    || (x.PieceOfEquipmentID != null && x.PieceOfEquipmentID.ToLower().Contains(filter.ToLower()))
                    || (x.Customer != null && x.Customer.Name != null && x.Customer.Name.ToLower().Contains(filter.ToLower()))
                    );

           return exprTree;

        }


       

        public static Expression<Func<EquipmentTemplate, bool>> EquipmentTemplateFilterAdvance(Pagination<EquipmentTemplate> filter)
        {

            if (filter.Object != null && filter.Object.EntityFilter != null && (filter.Filter == null || filter.Filter == ""))
            {

                if (filter.Object.EntityFilter.Model == null)
                    filter.Object.EntityFilter.Model = "NULL";
                else
                    filter.Object.EntityFilter.Model = filter.Object.EntityFilter.Model.ToLower();

                if (filter.Object.EntityFilter.Manufacturer1.Name == null)
                    filter.Object.EntityFilter.Manufacturer1.Name = "NULL";
                else
                    filter.Object.EntityFilter.Manufacturer1.Name = filter.Object.EntityFilter.Manufacturer1.Name.ToLower();



                Expression<Func<EquipmentTemplate, bool>> exprTree = x =>
                       //(x.Name != null && x.Name.ToLower().StartsWith(filter.ToLower()))
                       (filter.Object.EntityFilter.Model == "NULL" || x.Model.ToLower().Contains(filter.Object.EntityFilter.Model))
                        && (filter.Object.EntityFilter.Manufacturer1.Name == "NULL" || x.Manufacturer1.Name.ToLower().Contains(filter.Object.EntityFilter.Manufacturer1.Name))

                       //&& (filter.Object.EntityFilter.EquipmentTypeID == 0 || x.EquipmentTypeID == filter.Object.EntityFilter.EquipmentTypeID)
                       && ((filter.Object.EntityFilter.EquipmentTypeGroupID.HasValue == true && x.EquipmentTypeGroupID == filter.Object.EntityFilter.EquipmentTypeGroupID) || filter.Object.EntityFilter.EquipmentTypeGroupID.HasValue==false)
                       ;






                //Expression<Func<EquipmentTemplate, bool>> exprTree = x =>
                //       //(x.Name != null && x.Name.ToLower().StartsWith(filter.ToLower()))
                //       (filter.Object.EntityFilter.Model != null && x.Model.ToLower().Contains(filter.Object.EntityFilter.Model))
                //       || (filter.Object.EntityFilter.Manufacturer1.Name != null && x.Manufacturer1.Name.ToLower().Contains(filter.Object.EntityFilter.Manufacturer1.Name))

                //       || (filter.Object.EntityFilter.EquipmentTypeID > 0 && x.EquipmentTypeID == filter.Object.EntityFilter.EquipmentTypeID)
                //       || (filter.Object.EntityFilter.EquipmentTypeGroupID.HasValue == true && x.EquipmentTypeGroupID == filter.Object.EntityFilter.EquipmentTypeGroupID)
                //       ;


                return exprTree;
            }
            else
            {
                return EquipmentTemplateFilter(filter.Filter);

            }




            
        
        }



            public static Expression<Func<EquipmentTemplate, bool>> EquipmentTemplateFilter(string filter)
        {

            if (!string.IsNullOrEmpty(filter))
            {

                Expression<Func<EquipmentTemplate, bool>> exprTree = x => (x.EquipmentTemplateID.ToString().Contains(filter.ToLower())
                || (x.Name != null && x.Name.ToLower().StartsWith(filter.ToLower()))
                    || (x.Model != null && x.Model.ToLower().Contains(filter.ToLower()))
                    //|| (x.Status != null && x.Status.ToLower().Contains(filter.ToLower()))
                    || (x.Manufacturer != null && x.Manufacturer.ToLower().Contains(filter.ToLower()))
                    || (x.Manufacturer1 != null && x.Manufacturer1.Name.ToLower().StartsWith(filter.ToLower()))
                    || (x.Capacity.ToString().ToLower().Contains(filter.ToLower()))
                    || (x.EquipmentTypeObject != null && x.EquipmentTypeObject.Name != null && x.EquipmentTypeObject.ETCalculatedAlgorithm != "Parent" && x.EquipmentTypeObject.Name.ToLower().Contains(filter.ToLower()))
                    )
                    && x.EquipmentTypeObject.ETCalculatedAlgorithm != "Parent"
                    ;

                return exprTree;
            }
            else
            {
               Expression<Func<EquipmentTemplate, bool>> exprTree = x => 1==1;
                return exprTree;
            }

        }

        public static Expression<Func<EquipmentType, bool>> EquipmentTypeFilter(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return null;
            }
            Expression<Func<EquipmentType, bool>> exprTree = i => i.Name.ToLower().Contains(filter.ToLower());
            //    || i.Description.ToLower().Contains(filter.ToLower());
            //Expression<Func<EquipmentType, bool>> exprTree = i =>string.IsNullOrEmpty(filter) ||  i.Name.ToLower().Contains(filter.ToLower())
            //|| i.EquipmentTypeID.ToString() == filter
            //;

            return exprTree;
        }
        public static Expression<Func<CalibrationType, bool>> CalibrationTypeFilter(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return null;
            }
            Expression<Func<CalibrationType, bool>> exprTree = i => i.Name.ToLower().Contains(filter.ToLower());
          

            return exprTree;
        }

        public static Expression<Func<Customer, bool>> CustomerFilter(string filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                Expression<Func<Customer, bool>> exprTree = i => i.Name.ToLower().Contains(filter.ToLower())
                 || i.CustomerID.ToString().ToLower().Contains(filter.ToLower())
                 || i.CustomID.ToString().ToLower().Contains(filter.ToLower())
                 || i.Description.ToLower().Contains(filter.ToLower());
                return exprTree;
            }
            else
            {

                return null;
            }
        }

        public static Expression<Func<Address, bool>> AddressFilter(string filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                Expression<Func<Address, bool>> exprTree = i => i.StreetAddress1.ToLower().Contains(filter.ToLower())
                 || i.StreetAddress2.ToLower().Contains(filter.ToLower())
                 || i.StreetAddress3.ToLower().Contains(filter.ToLower())
                 || i.City.ToLower().Contains(filter.ToLower())
                 || i.State.ToLower().Contains(filter.ToLower())
                 || i.ZipCode.ToLower().Contains(filter.ToLower())
                 || i.Country.ToLower().Contains(filter.ToLower())
                 || i.Description.ToLower().Contains(filter.ToLower())
                 || i.AddressId.ToString().ToLower().Contains(filter.ToLower());
                return exprTree;
            }
            else
            {
                return null;
            }
        }

        public static Expression<Func<AddressCustomerViewModel, bool>> AddressCustomerFilter(string filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                Expression<Func<AddressCustomerViewModel, bool>> exprTree = i =>
                    i.StreetAddress1.ToLower().Contains(filter.ToLower())
                    || i.StreetAddress2.ToLower().Contains(filter.ToLower())
                    || i.StreetAddress3.ToLower().Contains(filter.ToLower())
                    || i.City.ToLower().Contains(filter.ToLower())
                    || i.State.ToLower().Contains(filter.ToLower())
                    || i.ZipCode.ToLower().Contains(filter.ToLower())
                    || i.Country.ToLower().Contains(filter.ToLower())
                    || i.Description.ToLower().Contains(filter.ToLower())
                    || i.AddressId.ToString().ToLower().Contains(filter.ToLower())
                    || i.CustomerName.ToLower().Contains(filter.ToLower())
                    || i.CustomID.ToLower().Contains(filter.ToLower())
                    || i.CustomerID.ToString().ToLower().Contains(filter.ToLower())
                    || i.CustomerDescription.ToLower().Contains(filter.ToLower());
                return exprTree;
            }
            else
            {
                return null;
            }
        }

        public static Expression<Func<AddressCustomerViewModel, bool>> AddressCustomerFilterOptimized(string filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                // Optimized filter with null checks and reduced ToLower() calls
                var filterLower = filter.ToLower();
                Expression<Func<AddressCustomerViewModel, bool>> exprTree = i =>
                    (i.StreetAddress1 != null && i.StreetAddress1.ToLower().Contains(filterLower))
                    || (i.City != null && i.City.ToLower().Contains(filterLower))
                    || (i.State != null && i.State.ToLower().Contains(filterLower))
                    || (i.ZipCode != null && i.ZipCode.ToLower().Contains(filterLower))
                    || (i.CustomerName != null && i.CustomerName.ToLower().Contains(filterLower))
                    || (i.CustomID != null && i.CustomID.ToLower().Contains(filterLower))
                    || i.AddressId.ToString().Contains(filterLower)
                    || i.CustomerID.ToString().Contains(filterLower);
                return exprTree;
            }
            else
            {
                return null;
            }
        }

        public static Expression<Func<CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder, bool>> WorkOrderFilter(string filter)
        {
            

            if (!string.IsNullOrEmpty(filter))
            {
                Expression<Func<CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder, bool>> exprTree = i => i.Name.ToLower().Contains(filter.ToLower())
             || i.WorkOrderId.ToString().ToLower().Contains(filter)
             || i.Description.ToLower().Contains(filter)
             || i.Invoice.ToLower().Contains(filter)
             || i.Customer.Name != null && i.Customer.Name.ToLower().Contains(filter.ToLower())
             ;
                return exprTree;
            }
            else
            {
                return null;
            }
        }

            public static Expression<Func<TestCode, bool>> TestCodeFilter(string filter)
        {
           
                if (!string.IsNullOrEmpty(filter))
            {
                Expression<Func<TestCode, bool>> exprTree = i => i.Code.ToLower().StartsWith(filter.ToLower())
             || i.Description.ToLower().Contains(filter)
             || i.TestCodeID.ToString().ToLower().Contains(filter)
             || (i.ProcedureID.HasValue && i.Procedure != null && i.Procedure.Name.ToLower().Contains(filter.ToLower()))
             ;
                return exprTree;
            }
            else
            {
                return null;
            }
           
        }

        public static Expression<Func<UnitOfMeasure, bool>> UOMFilter(string filter)
        {

            
            Expression<Func<UnitOfMeasure, bool>> exprTree = i => i.Name.ToLower().Contains(filter.ToLower())
                 || i.Abbreviation.ToLower().Contains(filter.ToLower())
                 || i.Type != null && i.Type.Name.ToLower().Contains(filter.ToLower())
                ;
            return exprTree;
        }


        public static Expression<Func<EquipmentTemplate, bool>> EquipmentTemplateExists(EquipmentTemplate DTO)
        {
            //|| x.Name.ToLower() == DTO.Name.ToLower()
            Expression<Func<EquipmentTemplate, bool>> exprTree = x => x.EquipmentTemplateID ==
           DTO.EquipmentTemplateID ;
            return exprTree;
        }


        public static Expression<Func<EquipmentTemplate, bool>> EquipmentTemplateByID(EquipmentTemplate DTO)
        {

            Expression<Func<EquipmentTemplate, bool>> exprTree = x=> x.EquipmentTemplateID == DTO.EquipmentTemplateID
             ;
            return exprTree;
        }

        public static Expression<Func<EquipmentTemplate, bool>> EquipmentTemplateByID(int ID)
        {

            Expression<Func<EquipmentTemplate, bool>> exprTree = x => x.EquipmentTemplateID == ID
             ;
            return exprTree;
        }



        public static Expression<Func<WorkOrderDetail, bool>> WorkOrderDetailByID(WorkOrderDetail DTO)
        {
            Expression<Func<WorkOrderDetail, bool>> exprTree = h => h.WorkOrderDetailID == DTO.WorkOrderDetailID;
            return exprTree;
        }


        public static Expression<Func<WorkOrderDetail, bool>> WorkOrderDetailByID(int ID)
        {
            Expression<Func<WorkOrderDetail, bool>> exprTree = h => h.WorkOrderDetailID == ID;
            return exprTree;
        }

        public static Expression<Func<Manufacturer, bool>> ManufacturerFilter(string filter)
        {

            if (filter !=null )
            { 
            Expression<Func<Manufacturer, bool>> exprTree = i => (i.Name.ToLower().Contains(filter.ToLower())
                 || i.Description != null && i.Description.ToLower().Contains(filter.ToLower()
                 ));
                return exprTree;
            }
            else
            {
                
                return null;
            }

        }

        public static Expression<Func<Procedure, bool>> ProcedureFilter(string filter)
        {

            if (filter != null)
            {
                Expression<Func<Procedure, bool>> exprTree = i => (i.Name.ToLower().Contains(filter.ToLower()));
                return exprTree;
            }
            else
            {

                return null;
            }

        }

        //public static Expression<Func<Customer, bool>> Customer(int ID)
        //{
        //    Expression<Func<Customer, bool>> exprTree = (i => i.Name.ToLower().Contains(filter.ToLower())
        //     || i.CustomerID.ToString().ToLower().Contains(filter)
        //     || i.Description.ToString().ToLower().Contains(filter))


        //    return exprTree;
        //}





        public static Expression<Func<BalanceAndScaleCalibration, bool>> BalanceAndScaleCalibrationByWorkOrderDetail(WorkOrderDetail DTO)
        {
            Expression<Func<BalanceAndScaleCalibration, bool>> exprTree = h => h.WorkOrderDetailId == DTO.WorkOrderDetailID;

            return exprTree;
        }


        public static Expression<Func<CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder, bool>> WorkOrderFilter(int ID)
        {
            Expression<Func<CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder, bool>> exprTree = sc => sc.WorkOrderId == ID;
            return null;
        }


        public static Expression<Func<CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder, bool>> WorkOrderByID(int ID)
        {
            Expression<Func<CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder, bool>> exprTree = sc => sc.WorkOrderId == ID;
            return exprTree;
        }

        public static Expression<Func<WorkOrderDetail, bool>> WorkOrderDetailByWorkOrderID(int ID)
        {
            Expression<Func<WorkOrderDetail, bool>> exprTree = x => x.WorkOrderID == ID;
            return exprTree;
        }

        public static Expression<Func<TechnicianCode, bool>> TechnianCodeByUserFilter(string filter,User user)
        {
           
            Expression<Func<TechnicianCode, bool>> exprTree = x => x.UserID == user.UserID; //&& x.Code.ToLower().Contains(filter.ToLower());
            
          

            return exprTree;
        }

        public static Expression<Func<User, bool>> UserFilter(string filter)
        {
            Expression<Func<User, bool>> exprTree = x => x.Name.ToLower().Contains(filter.ToLower())
                || x.LastName.ToLower().Contains(filter.ToLower())
                || x.Email.ToLower().Contains(filter.ToLower())
                ;


            return exprTree;
        }

        public static Expression<Func<Contact, bool>> ContactFilter(string filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                Expression<Func<Contact, bool>> exprTree = x => x.Name.ToLower().Contains(filter.ToLower())
                    || (x.LastName != null && x.LastName.ToLower().Contains(filter.ToLower()))
                    || (x.Email != null && x.Email.ToLower().Contains(filter.ToLower()))
                    || (x.PhoneNumber != null && x.PhoneNumber.ToLower().Contains(filter.ToLower()))
                    || (x.CellPhoneNumber != null && x.CellPhoneNumber.ToLower().Contains(filter.ToLower()))
                    || (x.UserName != null && x.UserName.ToLower().Contains(filter.ToLower()))
                    || (x.Occupation != null && x.Occupation.ToLower().Contains(filter.ToLower()))
                    || x.ContactID.ToString().Contains(filter);
                return exprTree;
            }
            else
            {
                return null;
            }
        }




      

        public static Expression<Func<WorkOrderDetail, bool>> ValidateToleranceAccuracy()
        {
            Expression<Func<WorkOrderDetail, bool>> exprTree = DTO => (DTO.Tolerance.ToleranceTypeID == 0 ) || DTO.Tolerance.ToleranceTypeID == 1 && DTO.Resolution == 0;
            return exprTree;
        }

        public static Expression<Func<WorkOrderDetail, bool>> ValidateAccuracyPercent()
        {
            Expression<Func<WorkOrderDetail, bool>> exprTree = DTO => (DTO.Tolerance.ToleranceTypeID == 0) || DTO.Tolerance.ToleranceTypeID == 2 && (DTO.Tolerance.AccuracyPercentage == 0 || DTO.Resolution  ==0 ) 
            
            ;
            return exprTree;
        }

        public static Expression<Func<EquipmentTemplate, bool>> ValidateToleranceAccuracyET()
        {
            Expression<Func<EquipmentTemplate, bool>> exprTree = DTO => (DTO.Tolerance.ToleranceTypeID > 0 && DTO.Tolerance.ToleranceTypeID == 1) && DTO.Resolution > 0;
            return exprTree;
        }

        public static Expression<Func<EquipmentTemplate, bool>> ValidateAccuracyPercentET()
        {
            Expression<Func<EquipmentTemplate, bool>> exprTree = DTO => (DTO.Tolerance.AccuracyPercentage > 0  && DTO.Resolution > 0 && DTO.Tolerance.ToleranceTypeID == 2)
            && DTO.Tolerance.AccuracyPercentage == 0
            && DTO.Resolution == 0
            ;
            return exprTree;
        }


         




        public static bool ValidateEccentrycityTespoint(WorkOrderDetail DTO)
        {
            if ( DTO?.BalanceAndScaleCalibration != null && DTO?.BalanceAndScaleCalibration?.Eccentricity != null &&
                  DTO.BalanceAndScaleCalibration.Eccentricity?.TestPointResult?.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static class WODRules
        {
            public static Expression<Func<WorkOrderDetail, bool>> RuleAllStatus()
            {
                Expression<Func<WorkOrderDetail, bool>> exprTree = x => x.CurrentStatusID != -1;
                return exprTree;
            }


            public static Expression<Func<WorkOrderDetail, bool>> RuleDownLoadCertificateStatus()
            {
                Expression<Func<WorkOrderDetail, bool>> exprTree = x => x.CurrentStatusID >= 2;
                return exprTree;
            }
            public static Expression<Func<WorkOrderDetail, bool>> RuleIsAccreditedStatus()
            {
                Expression<Func<WorkOrderDetail, bool>> exprTree = x => x.CurrentStatusID <= 2;
                return exprTree;
            }


            public static Expression<Func<WorkOrderDetail, bool>> RuleContractReviewStatus()
            {
                Expression<Func<WorkOrderDetail, bool>> exprTree = x => x.CurrentStatusID == 1;
                return exprTree;
            }

            public static Expression<Func<WorkOrderDetail, bool>> RuleAccreditedWod()
            {
                Expression<Func<WorkOrderDetail, bool>> exprTree = x => x.IsAccredited == true;
                return exprTree;
            }


            public static Expression<Func<WorkOrderDetail, bool>> RuleContractReviewCustomDateStatus(bool custom)
            {
                Expression<Func<WorkOrderDetail, bool>> exprTree = x => x.CurrentStatusID == 1 && custom==true;
                return exprTree;
            }


            public static Expression<Func<WorkOrderDetail, bool>> RuleReadyForCalibrationStatus(bool? currentstatus=null)
            {

                if (currentstatus.HasValue && currentstatus.Value==true)
                {
                    Expression<Func<WorkOrderDetail, bool>> exprTree2 = x => 1 == 1;
                    return exprTree2;
                }
                if (currentstatus.HasValue && currentstatus.Value == false)
                {
                    Expression<Func<WorkOrderDetail, bool>> exprTree2 = x => 1 == 2;
                    return exprTree2;
                }

                Expression<Func<WorkOrderDetail, bool>> exprTree = x => x.CurrentStatusID == 2;
                return exprTree;
            }

            public static Expression<Func<WorkOrderDetail, bool>> RuleReadyForCalibrationAndContractReviewStatus()
            {
                Expression<Func<WorkOrderDetail, bool>> exprTree = x => x.CurrentStatusID == 2 || x.CurrentStatusID == 1;
                return exprTree;
            }

            public static string UsersAcces()
            {
                return "admin";
            }

           


        }

     

        public static Expression<Func<PieceOfEquipment, bool>> POEGridDueDateCondition()
        {
            Expression<Func<PieceOfEquipment, bool>> exprTree = x =>   (DateTime.Now > x.DueDate && x.EquipmentTemplate !=null && x.EquipmentTemplate.EquipmentTypeID==3) ;
            return exprTree;
        }

        public static Expression<Func<PieceOfEquipment, bool>> POEGridDalmostueDateCondition()
        {
            var a = DateTime.Now.AddDays(30);
            //Console.WriteLine(a.ToShortDateString());
            Expression<Func<PieceOfEquipment, bool>> exprTree = x => x.DueDate.Date <= a 
            && x.DueDate.Date >= DateTime.Now.Date && x.EquipmentTemplate.EquipmentTypeID==3;
            return exprTree;
        }


        public static void AssingAlgoritm(List<WeightSet> source, IEnumerable<ICalibrationSubType> target, List<UnitOfMeasure> unitOfMeasures, string? customer = null)
        {

            if(source ==null || source?.Count == 0)
            {
                return;
            }

            source = source.DistinctBy(x => x.WeightSetID).ToList();


            foreach (var item in target)
            {
                AssingAlgoritm(source, item, unitOfMeasures);
                // Note: Weights are NOT removed from source after assignment
                // because in calibration, the same weights can be reused for different test points
                // (each test point is measured at a different moment in time)
            }

            foreach (var item in target)
            {
                double result = 0;
                double result2 = 0;
                if (item.WeightSets != null && item?.WeightSets?.Count > 0)
                {
                    List<WeightSet> lst = new List<WeightSet>();
                    foreach (var item2 in item.WeightSets)
                    {
                        string j = string.Empty;

                        var valueconv= item2.ConversionMethod(item.TestPoint, unitOfMeasures, out j, customer);

                        var restemp = result2 + valueconv;

                        //9969 YP
                        double weightApplied = 0;
                        if (customer == "LTI")
                        {
                            weightApplied = result + item2.WeightActualValue;
                        }
                        else
                        {

                            weightApplied = result + item2.WeightNominalValue;
                        }
                        if(restemp <= (item.TestPoint.NominalTestPoit + (0.1* item.TestPoint.NominalTestPoit)))
                        {
                            //TODO code for minus values
                            result2 = result2 + valueconv;
                            lst.Add(item2);
                            result = weightApplied; /// result + item2.WeightNominalValue;
                        }
                    }


                    var argument = Convert.ToDecimal(item.TestPoint.NominalTestPoit);
                    int count = BitConverter.GetBytes(decimal.GetBits(argument)[3])[2];

                    var resulttext = Math.Round(result2, count);

                    if (resulttext == item.TestPoint.NominalTestPoit)
                    {
                        item.CalculateWeightValue = 0;
                        item.CalculateWeightValue = result2;
                        item.CalculateWeightValue = result2;
                        item.WeightSets = lst;
                    }
                    else
                    {
                        item.CalculateWeightValue = 0;
                        item.WeightSets = null;
                    }
                }
               

            }


        }

        public static void AssingAlgoritm(List<WeightSet> source, ICalibrationSubType target, List<UnitOfMeasure> unitOfMeasures)
        {
            
            var value = target.TestPoint.NominalTestPoit;
            List<WeightSet> resultado = new List<WeightSet>();
            List<WeightSet> source2 = new List<WeightSet>(source);
            CalculateAssing(source2, value, target.TestPoint, unitOfMeasures, ref resultado);
            target.WeightSets = resultado;

        }



        public static void CalculateAssing(List<WeightSet> _source, double number, TestPoint TestPoint,List<UnitOfMeasure> UnitOfMeasureList, ref List<WeightSet> resultado)
        {
            Console.WriteLine($"[AUTO-ASSIGN] Starting for test point: {number} {TestPoint?.UnitOfMeasurementOut?.Abbreviation ?? TestPoint?.UnitOfMeasurementOutID.ToString()}");

            if (_source == null || _source?.Count == 0 || number == 0)
            {
                Console.WriteLine($"[AUTO-ASSIGN] Early exit - source null/empty or number is 0");
                return;
            }

            // Get the target unit of measure
            // First try to use the navigation property, if null then look it up by ID
            var targetUOM = TestPoint.UnitOfMeasurementOut;
            if (targetUOM == null)
            {
                targetUOM = UnitOfMeasureList?.FirstOrDefault(u => u.UnitOfMeasureID == TestPoint.UnitOfMeasurementOutID);
                Console.WriteLine($"[AUTO-ASSIGN] Target UOM looked up by ID: {targetUOM?.Abbreviation ?? "NULL"}");
            }
            else
            {
                Console.WriteLine($"[AUTO-ASSIGN] Target UOM from navigation property: {targetUOM.Abbreviation}");
            }

            if (targetUOM == null)
            {
                Console.WriteLine($"[AUTO-ASSIGN] ERROR: Target UOM is null, cannot proceed");
                return;
            }

            // Convert all available weights to the target UOM and work with decimal precision
            var weightValues = new List<(WeightSet weight, decimal convertedValue)>();

            // Determine precision for rounding based on target value
            // This handles floating-point conversion errors while maintaining accuracy
            int decimalPlaces = GetDecimalPlaces(number);
            Console.WriteLine($"[AUTO-ASSIGN] Using {decimalPlaces} decimal places for precision");

            foreach (var weight in _source)
            {
                if (weight.UnitOfMeasure == null)
                {
                    continue;
                }

                try
                {
                    // Convert weight nominal value to target UOM
                    double convertedDouble = QueryableExtensions2.ConversionUOM(weight.UnitOfMeasure, weight.WeightNominalValue, targetUOM);

                    // Use decimal for exact arithmetic to avoid floating-point errors
                    // Round to appropriate decimal places to handle conversion precision
                    decimal convertedValue = Math.Round(Convert.ToDecimal(convertedDouble), decimalPlaces);

                    weightValues.Add((weight, convertedValue));
                }
                catch (Exception ex)
                {
                    // Skip weights that can't be converted (incompatible unit types)
                    Console.WriteLine($"[AUTO-ASSIGN] Skipping weight {weight.WeightNominalValue} {weight.UnitOfMeasure?.Abbreviation} - conversion error: {ex.Message}");
                    continue;
                }
            }

            Console.WriteLine($"[AUTO-ASSIGN] Converted {weightValues.Count} weights to target UOM");

            if (weightValues.Count == 0)
            {
                Console.WriteLine($"[AUTO-ASSIGN] No convertible weights found");
                return;
            }

            // Convert target to decimal for exact comparison
            // Round to same precision as weight values
            decimal targetDecimal = Math.Round(Convert.ToDecimal(number), decimalPlaces);
            Console.WriteLine($"[AUTO-ASSIGN] Target value: {targetDecimal} {targetUOM.Abbreviation}");

            // Sort by converted value (descending) for better performance
            weightValues = weightValues.OrderByDescending(x => x.convertedValue).ToList();

            // Debug: Show ALL available weights to diagnose the issue
            Console.WriteLine($"[AUTO-ASSIGN] Available weights (sorted descending) - ALL {weightValues.Count} weights:");
            foreach (var (weight, convertedValue) in weightValues)
            {
                Console.WriteLine($"  - {weight.WeightNominalValue} {weight.UnitOfMeasure.Abbreviation} = {convertedValue} {targetUOM.Abbreviation}");
            }

            // OPTIMIZATION: Try greedy approach first for simple cases
            // This handles common cases like exact single weight match or simple combinations
            var result = new List<WeightSet>();

            // Check for exact single weight match
            var exactMatch = weightValues.FirstOrDefault(w => w.convertedValue == targetDecimal);
            if (exactMatch.weight != null)
            {
                Console.WriteLine($"[AUTO-ASSIGN] ✓ Found exact single weight match: {exactMatch.weight.WeightNominalValue} {exactMatch.weight.UnitOfMeasure.Abbreviation}");
                resultado.Add(exactMatch.weight);
                return;
            }

            Console.WriteLine($"[AUTO-ASSIGN] No exact single match, trying recursive search with top 30 weights, max depth 20");

            // For larger targets, try a limited recursive search with strict limits
            // Only use first 30 weights to avoid performance issues
            var limitedWeights = weightValues.Take(30).ToList();

            if (FindExactCombination(limitedWeights, targetDecimal, 0, result, maxDepth: 20))
            {
                Console.WriteLine($"[AUTO-ASSIGN] ✓ Found exact combination with {result.Count} weights:");
                foreach (var w in result)
                {
                    Console.WriteLine($"  - {w.WeightNominalValue} {w.UnitOfMeasure.Abbreviation}");
                }
                resultado.AddRange(result);
                return;
            }

            // If no exact match found, don't assign any weights
            // This prevents approximate combinations
            Console.WriteLine($"[AUTO-ASSIGN] ✗ No exact combination found for {targetDecimal} {targetUOM.Abbreviation}");
            return;
        }

        /// <summary>
        /// Recursive backtracking to find exact weight combination
        /// Uses decimal arithmetic to ensure exact matches only
        /// Optimized to handle large weight sets efficiently
        /// </summary>
        private static bool FindExactCombination(
            List<(WeightSet weight, decimal convertedValue)> weights,
            decimal remainingTarget,
            int startIndex,
            List<WeightSet> currentCombination,
            int maxDepth = 5)
        {
            // Check if we've reached the target EXACTLY
            if (remainingTarget == 0)
            {
                return true;
            }

            // If remaining target is negative, we've exhausted all weights, or exceeded max depth, backtrack
            if (remainingTarget < 0 || startIndex >= weights.Count || currentCombination.Count >= maxDepth)
            {
                return false;
            }

            // Try including each weight from startIndex onwards
            // Limit the search to avoid performance issues with large weight sets
            int searchLimit = Math.Min(startIndex + 20, weights.Count); // Only look at next 20 weights

            for (int i = startIndex; i < searchLimit; i++)
            {
                var (weight, convertedValue) = weights[i];

                // Skip if this weight is larger than remaining target
                if (convertedValue > remainingTarget)
                {
                    continue;
                }

                // Include this weight
                currentCombination.Add(weight);

                // Recursively try to complete the combination
                if (FindExactCombination(weights, remainingTarget - convertedValue, i + 1, currentCombination, maxDepth))
                {
                    return true;
                }

                // Backtrack - remove this weight and try next
                currentCombination.RemoveAt(currentCombination.Count - 1);
            }

            return false;
        }

        /// <summary>
        /// Determines the number of decimal places in a double value
        /// Used for rounding to appropriate precision
        /// </summary>
        private static int GetDecimalPlaces(double value)
        {
            // Convert to decimal to get accurate decimal places
            decimal decimalValue = Convert.ToDecimal(value);

            // Remove trailing zeros
            decimalValue = decimalValue / 1.000000000000000000000000000000000m;

            // Get the number of decimal places
            int decimalPlaces = BitConverter.GetBytes(decimal.GetBits(decimalValue)[3])[2];

            // Limit to reasonable precision (max 6 decimal places for weight measurements)
            // This prevents floating-point precision issues while maintaining accuracy
            return Math.Min(decimalPlaces, 6);
        }

        public static Expression<Func<WorkOrderDetailByStatus, bool>> WorkOrderDetailByStatusFilter(WorkOrderDetailByStatus wod)
        {
            DateTime date = Convert.ToDateTime("01/01/1999 12:00:00 AM");

            if (wod.WorkOrderReceiveDate.ToString() == "1/1/0001 12:00:00 AM")
            {
                wod.WorkOrderReceiveDate = Convert.ToDateTime("01/01/1999 12:00:00 AM");

            }

            if (wod.StatusDate.ToString() == "1/1/0001 12:00:00 AM")
            {
                wod.StatusDate = Convert.ToDateTime("01/01/1999 12:00:00 AM");

            }

            if (wod.Model == null)
            {
                wod.Model = "null";
            }

            if (wod.Company == null)
            {
                wod.Company = "null";
            }

            if (wod.SerialNumber == null)
            {
                wod.SerialNumber = "null";
            }




            //Expression<Func<WorkOrderDetailByStatus, bool>> exprTree = x => (x.StatusId > 0 && x.StatusId == wod.StatusId || wod.StatusId==0)
            //&& ((wod.WorkOrderReceiveDate != null && ( x.WorkOrderReceiveDate == wod.WorkOrderReceiveDate ))
            //|| (wod.StatusDate != null && (x.StatusDate == wod.StatusDate ))             
            //||  (x.Company.ToUpper().Contains(wod.Company.ToUpper()) || wod.Company == "null"))
            //&& (x.EquipmentTypeID > 0 && x.EquipmentTypeID == wod.EquipmentTypeID || wod.EquipmentTypeID == 0)
            //&& (wod.Model !=null  && x.Model.ToUpper().Contains(wod.Model.ToUpper()) || wod.Model == null);
            //return exprTree;


            Expression<Func<WorkOrderDetailByStatus, bool>> exprTree = x => (wod.StatusId > 0 && x.StatusId == wod.StatusId || wod.StatusId == 0)
         && (wod.Model == "null" || x.Model.ToUpper().Contains(wod.Model.ToUpper()) )
         && ((wod.SerialNumber == "null" || x.SerialNumber.ToUpper().Contains(wod.SerialNumber.ToUpper()) ))
         //|| (x.WorkOrderDetailID == wod.WorkOrderDetailID || wod.WorkOrderDetailID == 0)
         && (wod.WorkOrderReceiveDate.HasValue == false || wod.WorkOrderReceiveDate == date 
         || x.WorkOrderReceiveDate.HasValue && wod.WorkOrderReceiveDate.HasValue 
         && x.WorkOrderReceiveDate.Value.Date == wod.WorkOrderReceiveDate.Value.Date )
         && (wod.StatusDate.HasValue == false || wod.StatusDate == date || x.StatusDate.HasValue && wod.StatusDate.HasValue  && x.StatusDate.Value.Date == wod.StatusDate.Value.Date )
         && (wod.Company == "null" || x.Company.ToUpper().Contains(wod.Company.ToUpper()) );
            return exprTree;


        }

        public static Expression<Func<WorkOrderDetailByStatus, bool>> WorkOrderDetailByEquipmentFilter(WorkOrderDetailByStatus wod)
        {

            DateTime date = Convert.ToDateTime("01/01/1999 12:00:00 AM");

            if (wod.WorkOrderReceiveDate.ToString() == "1/1/0001 12:00:00 AM")
            {
                wod.WorkOrderReceiveDate = Convert.ToDateTime("01/01/1999 12:00:00 AM");

            }

            if (wod.StatusDate.ToString() == "1/1/0001 12:00:00 AM")
            {
                wod.StatusDate = Convert.ToDateTime("01/01/1999 12:00:00 AM");

            }

            if (wod.Model == null)
            {
                wod.Model = "null";
            }

            if (wod.Company == null)
            {
                wod.Company = "null";
            }

            if (wod.SerialNumber == null)
            {
                wod.SerialNumber = "null";
            }




            Expression<Func<WorkOrderDetailByStatus, bool>> exprTree = x => (x.EquipmentTypeID == wod.EquipmentTypeID || wod.EquipmentTypeID == 0)
           || (x.Model.ToUpper().Contains(wod.Model.ToUpper()) || wod.Model == "null")
           || ((x.SerialNumber.ToUpper().Contains(wod.SerialNumber.ToUpper()) || wod.SerialNumber == "null"))
           || (x.WorkOrderDetailID == wod.WorkOrderDetailID || wod.WorkOrderDetailID == 0)
           || (x.WorkOrderReceiveDate == wod.WorkOrderReceiveDate || wod.WorkOrderReceiveDate == date)
           || (x.StatusDate == wod.StatusDate || wod.StatusDate == date)
           || (x.Company.ToUpper().Contains(wod.Company.ToUpper()) || wod.Company == "null");



            return exprTree;



        }
        public static Expression<Func<WorkOrderDetail, bool>> WODbyTech2(int ID)
        {
            Expression<Func<WorkOrderDetail, bool>> exprTree = x => x.TechnicianID.HasValue && ID > 0 && x.TechnicianID == ID || x.TechnicianID.HasValue;


            return null;

            //return null;

        }


        public static Expression<Func<T, bool>> WorkOrderDetailFilterQuery<T>(FilterObject<T> wodFilter)
        {

            
                return null;
            
        }

            public static Expression<Func<WorkOrderDetail, bool>> WorkOrderDetailFilter(FilterObject<WorkOrderDetail> wodFilter)
        {

            if(wodFilter==null || (wodFilter.EntityFilter == null) || !wodFilter.Advanced)
            {
                return null;
            }

            //DateTime date = Convert.ToDateTime("01/01/1999 12:00:00 AM");

            //if (wod.WorkOrderReceiveDate.ToString() == "1/1/0001 12:00:00 AM")
            //{
            //    wod.WorkOrderReceiveDate = Convert.ToDateTime("01/01/1999 12:00:00 AM");

            //}

            //if (wod.StatusDate.ToString() == "1/1/0001 12:00:00 AM")
            //{
            //    wod.StatusDate = Convert.ToDateTime("01/01/1999 12:00:00 AM");

            //}

            //if (wod.Model == null)
            //{
            //    wod.Model = "null";
            //}

            //if (wod.Company == null)
            //{
            //    wod.Company = "null";
            //}

            //if (wod.SerialNumber == null)
            //{
            //    wod.SerialNumber = "null";
            //}

            WorkOrderDetail wod = wodFilter.EntityFilter;

           



            Expression<Func<WorkOrderDetail, bool>> exprTree = x => 
          
          
            
            ((wod.PieceOfEquipment != null && wod.PieceOfEquipment.EquipmentTemplate != null 
           && !string.IsNullOrEmpty(wod.PieceOfEquipment.EquipmentTemplate.Model) 
           && x.PieceOfEquipment.EquipmentTemplate.Model.Trim().ToUpper().Contains(wod.PieceOfEquipment.EquipmentTemplate.Model.ToUpper()))
           //|| (wod.PieceOfEquipment != null && wod.PieceOfEquipment.EquipmentTemplate != null 
           //&& (string.IsNullOrEmpty(wod.PieceOfEquipment.EquipmentTemplate.Model) && 1==1)
           //&& (string.IsNullOrEmpty(x.PieceOfEquipment.EquipmentTemplate.Model) 
           //|| !string.IsNullOrEmpty(x.PieceOfEquipment.EquipmentTemplate.Model))
           )
           
           ||

            ((wod.PieceOfEquipment != null 
           && !string.IsNullOrEmpty(wod.PieceOfEquipment.SerialNumber)
           && x.PieceOfEquipment.SerialNumber.Trim().ToUpper().Contains(wod.PieceOfEquipment.SerialNumber.Trim().ToUpper()))
           //|| (wod.PieceOfEquipment != null 
           //&& (string.IsNullOrEmpty(wod.PieceOfEquipment.SerialNumber))
           //&& (string.IsNullOrEmpty(x.PieceOfEquipment.SerialNumber)
           //|| !string.IsNullOrEmpty(x.PieceOfEquipment.SerialNumber)))
           )

           ||

            ((wod.WorkOder != null
           && !string.IsNullOrEmpty(wod.WorkOder.PurchaseOrder)
           && x.WorkOder.PurchaseOrder.Trim().ToUpper().Contains(wod.WorkOder.PurchaseOrder.ToUpper()))
           //|| (wod.WorkOder != null
           //&& string.IsNullOrEmpty(wod.WorkOder.PurchaseOrder) && (string.IsNullOrEmpty(x.WorkOder.PurchaseOrder)
           //|| !string.IsNullOrEmpty(x.WorkOder.PurchaseOrder)))
           )

            ||

            ((wod.WorkOder != null
           && wod.WorkOder.WorkOrderId > 0
           && x.WorkOder.WorkOrderId.ToString().Contains(wod.WorkOder.WorkOrderId.ToString()))
           //|| (wod.WorkOder != null
           //&& wod.WorkOder.WorkOrderId == 0
           //&& x.WorkOder.WorkOrderId > 0
           //|| x.WorkOder.WorkOrderId == 0
           //)
           )

            //&&

            // ((wod != null
            //&& wod.WorkOrderDetailID > 0
            //&& x.WorkOrderDetailID.ToString().Contains(wod.WorkOrderDetailID.ToString()))
            //|| (wod != null
            //&& wod.WorkOrderDetailID == 0
            //&& x.WorkOrderDetailID > 0
            //|| x.WorkOrderDetailID==0
            //)
            //)

            ||

            ((wod != null && wod.TechnicianID.HasValue == true
           && x.TechnicianID.HasValue == true
           && x.TechnicianID.Value.ToString().Contains(wod.TechnicianID.ToString()))
           //|| (wod != null
           //&& wod.TechnicianID.HasValue == false
           //&& x.TechnicianID.HasValue == false
           //|| x.TechnicianID.HasValue == true
           //)
           )


            ||

            ((wod != null
           && !string.IsNullOrEmpty(wod.WorkOrderDetailUserID)
           && x.WorkOrderDetailUserID.Trim().ToUpper().Contains(wod.WorkOrderDetailUserID.ToUpper()))
           //|| (wod != null
           //&& string.IsNullOrEmpty(wod.WorkOrderDetailUserID) 
           //&& (string.IsNullOrEmpty(x.WorkOrderDetailUserID)
           //|| !string.IsNullOrEmpty(x.WorkOrderDetailUserID)))
           )


            ||

            ((wod.PieceOfEquipment != null
           && !string.IsNullOrEmpty(wod.PieceOfEquipment.PieceOfEquipmentID)
           && x.PieceOfEquipment.PieceOfEquipmentID.Trim().ToUpper().Contains(wod.PieceOfEquipment.PieceOfEquipmentID.ToUpper()))
           //|| (wod.PieceOfEquipment != null
           //&& string.IsNullOrEmpty(wod.PieceOfEquipment.PieceOfEquipmentID) && (string.IsNullOrEmpty(x.PieceOfEquipment.PieceOfEquipmentID)
           //|| !string.IsNullOrEmpty(x.PieceOfEquipment.PieceOfEquipmentID)))
           )


           ||

           ((wod.PieceOfEquipment != null && wod.PieceOfEquipment.Customer != null
           && !string.IsNullOrEmpty(wod.PieceOfEquipment.Customer.Name)
           && x.PieceOfEquipment.Customer.Name.Trim().ToUpper().Contains(wod.PieceOfEquipment.Customer.Name.Trim().ToUpper()))
           //|| (wod.PieceOfEquipment != null && wod.PieceOfEquipment.Customer != null
           //&& string.IsNullOrEmpty(wod.PieceOfEquipment.Customer.Name) 
           //&& (string.IsNullOrEmpty(x.PieceOfEquipment.Customer.Name)
           //|| !string.IsNullOrEmpty(x.PieceOfEquipment.Customer.Name)))
           )
           ||

            ((wod.PieceOfEquipment != null && wod.PieceOfEquipment.EquipmentTemplate != null
           && wod.PieceOfEquipment.EquipmentTemplate.EquipmentTypeGroupID.HasValue == true
           && x.PieceOfEquipment.EquipmentTemplate.EquipmentTypeGroupID.Value.Equals(wod.PieceOfEquipment.EquipmentTemplate.EquipmentTypeGroupID.Value))
           //|| (wod.PieceOfEquipment != null && wod.PieceOfEquipment.EquipmentTemplate != null
           //&& wod.PieceOfEquipment.EquipmentTemplate.EquipmentTypeGroupID.HasValue == false
           //&& (x.PieceOfEquipment.EquipmentTemplate.EquipmentTypeGroupID.HasValue == true)
           //|| x.PieceOfEquipment.EquipmentTemplate.EquipmentTypeGroupID.HasValue == false)
           )


            ||

            ((wod.WorkOder != null && wodFilter.InitDate.HasValue == true && wodFilter.EndDate.HasValue && x.WorkOder.ScheduledDate.HasValue == true

           && x.WorkOder.ScheduledDate.Value.Date >= wodFilter.InitDate.Value.Date
           && x.WorkOder.ScheduledDate.Value.Date <= wodFilter.EndDate.Value.Date))

           ||

            ((wod.WorkOder != null && wodFilter.InitDate.HasValue == true && x.WorkOder.ScheduledDate.HasValue == true

           && x.WorkOder.ScheduledDate.Value.Date >= wodFilter.InitDate.Value.Date
           ))





           //|| (x.WorkOrderReceiveDate == wod.WorkOrderReceiveDate || wod.WorkOrderReceiveDate == date)
           //|| (x.StatusDate == wod.StatusDate || wod.StatusDate == date)
           //|| (x.Company.ToUpper().Contains(wod.Company.ToUpper()) || wod.Company == "null")
           ;



            return exprTree;



        }


        public static Expression<Func<WorkOrderDetail, bool>> WODbyTech(int ID)
        {
            Expression<Func<WorkOrderDetail, bool>> exprTree = x =>x.TechnicianID.HasValue &&  x.TechnicianID == ID ;


            return exprTree;

        }




        public static Expression<Func<WorkOrderDetailByCustomer, bool>> 
            WorkOrderDetailByCustomerFilter(WorkOrderDetailByCustomer wod)
        {
            DateTime date = Convert.ToDateTime("01/01/1999 12:00:00 AM");

            if (wod.WorkOrderReceiveDate.ToString() == "1/1/0001 12:00:00 AM")
            {
                wod.WorkOrderReceiveDate = Convert.ToDateTime("01/01/1999 12:00:00 AM");

            }

            if (wod.StatusDate.ToString() == "1/1/0001 12:00:00 AM")
            {
                wod.StatusDate = Convert.ToDateTime("01/01/1999 12:00:00 AM");

            }

            Expression<Func<WorkOrderDetailByCustomer, bool>> exprTree = x => 
            (x.DueDate == wod.DueDate || wod.DueDate == date)
             && x.StatusId==wod.StatusId;
        
            return exprTree;

        }


        public static Expression<Func<PieceOfEquipment, bool>> FilterWeightSets(Pagination<PieceOfEquipment> pagination)
        {


            //var poe = pagination.Entity.CustomerId;

            var DTO = pagination.Entity;

            Expression<Func<PieceOfEquipment, bool>> exprTree = null;
            //if (string.IsNullOrEmpty(pagination.Filter))
            //{
            //    exprTree = x =>
            //        //(x.Name != null && x.Name.ToLower().StartsWith(filter.ToLower()))
            //        //x.CustomerId == poe  &&
            //        //x.IsWeigthSet && x.WeightSets.Count > 0
            //        //&& (x.IsForAccreditedCal == DTO.IsForAccreditedCal) || (DTO.IsForAccreditedCal == false && x.IsForAccreditedCal == true)
            //        //&&
            //        ( x.DueDate >= DateTime.Now
            //        //|| (x.Customer != null && x.Customer.Name != null && x.Customer.Name.ToLower().Contains(pagination.Filter.ToLower()))
            //        //|| (x.EquipmentTemplate.EquipmentTypeObject != null && x.EquipmentTemplate.EquipmentTypeObject.Name.ToLower().Contains(pagination.Filter.ToLower()))
            //        );

            //    //return null;
            //}
            //else
            //{ 

            if (string.IsNullOrEmpty(pagination.Filter))
            {
                return null;
            }
            else
            {

                exprTree = x =>
                   //(x.Name != null && x.Name.ToLower().StartsWith(filter.ToLower()))
                   //x.CustomerId == poe  &&
                   //x.IsWeigthSet && x.WeightSets.Count > 0
                   //&& (x.IsForAccreditedCal == DTO.IsForAccreditedCal) || (DTO.IsForAccreditedCal == false && x.IsForAccreditedCal == true)
                   //&&
                   ((x.SerialNumber != null && x.SerialNumber.ToLower().Contains(pagination.Filter.ToLower()))
                   || (x.EquipmentTemplate.Model != null && x.EquipmentTemplate.Model.ToLower().Contains(pagination.Filter.ToLower())
                   || (x.Capacity > 0 && x.Capacity.ToString().ToLower().Contains(pagination.Filter.ToLower())
                   || (x.PieceOfEquipmentID != null && x.PieceOfEquipmentID.ToLower().Contains(pagination.Filter.ToLower()))
                   ) && x.DueDate >= DateTime.Now
                   //|| (x.Customer != null && x.Customer.Name != null && x.Customer.Name.ToLower().Contains(pagination.Filter.ToLower()))
                   //|| (x.EquipmentTemplate.EquipmentTypeObject != null && x.EquipmentTemplate.EquipmentTypeObject.Name.ToLower().Contains(pagination.Filter.ToLower()))
                   ));

                }
                return exprTree;





                //Expression<Func<PieceOfEquipment, bool>> exprTree = x => x.IsWeigthSet
                //     && (x.IsForAccreditedCal == DTO.IsForAccreditedCal) || (DTO.IsForAccreditedCal == false && x.IsForAccreditedCal == true)
                //     && x.WeightSets.Count > 0  ;
                //return exprTree;
            }


        public static Expression<Func<PieceOfEquipment, bool>> FilterPeripherical(Pagination<PieceOfEquipment> pagination)
        {

            if (string.IsNullOrEmpty(pagination.Filter))
            {
                return null;
            }

            Expression<Func<PieceOfEquipment, bool>> exprTree = x => x.EquipmentTemplate.EquipmentTypeID == 4 ;
                
            return exprTree;
        }

    }

    [Serializable]
    public class InvalidCalSaaSModel : Exception
    {
        public InvalidCalSaaSModel() { }
        public InvalidCalSaaSModel(string message) : base(message)
        {

        }
        public InvalidCalSaaSModel(string message, Exception inner) : base(message, inner)
        {

        }
    }

    //public static class PredicateBuilder
    //{
    //    public static Expression<Func<T, bool>> True<T>() { return f => true; }
    //    public static Expression<Func<T, bool>> False<T>() { return f => false; }

    //    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
    //                                                        Expression<Func<T, bool>> expr2)
    //    {
    //        var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
    //        return Expression.Lambda<Func<T, bool>>
    //              (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
    //    }

    //    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
    //                                                         Expression<Func<T, bool>> expr2)
    //    {
    //        var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
    //        return Expression.Lambda<Func<T, bool>>
    //              (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
    //    }
    //}


}
