using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Helpers
{
    public static class DynamicHelper
    {
        public static string StripHTML(this string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return "";

            // could be stored in static variable
            var regex = new Regex("<[^>]+>|\\s{2}", RegexOptions.IgnoreCase);
            return System.Web.HttpUtility.HtmlDecode(regex.Replace(html, ""));
        }


        public static bool PropertyExists(dynamic obj, string name,out dynamic resutl)
        {
            resutl = null;

            if (obj == null) return false;
            if (obj is IDictionary<string, object> dict)
            {
                dict.TryGetValue(name,out resutl);

                return dict.ContainsKey(name);
            }

             

            return resutl != null;
        }
        public static void SetPropertyValue(this object p_object, string p_propertyName, object value)
        {
            PropertyInfo property = p_object.GetType().GetProperty(p_propertyName);
            Type t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            object safeValue = (value == null) ? null : Convert.ChangeType(value, t);

            property.SetValue(p_object, safeValue, null);


        }

        public static void SetProperty(string compoundProperty, object target, object value)
        {
            var bits = compoundProperty.Split('.');
            if (bits == null) bits = new string[1] { compoundProperty };
            for (var i = 0; i < bits.Length - 1; i++)
            {
                var propertyToGet = target.GetType().GetProperty(bits[i]);
                var propertyValue = propertyToGet.GetValue(target, null);
                if (propertyValue == null)
                {
                    propertyValue = Activator.CreateInstance(propertyToGet.PropertyType);
                    propertyToGet.SetValue(target, propertyValue);
                }
                target = propertyToGet.GetValue(target, null);
            }
            var propertyToSet = target.GetType().GetProperty(bits.Last());
            propertyToSet.SetValue(target, value, null);
        }


        public static object GetPropertyValue(object src, string propName)
        {
            if (src == null)
            {

                return new object();
                // throw new ArgumentException("Value cannot be null.", "src");
            }

            if (propName == null)
            {

                throw new ArgumentException("Value cannot be null.", "propName");
            }


            if (propName.Contains("."))//complex type nested
            {
                var temp = propName.Split(new char[] { '.' }, 2);

                var og = GetPropertyValue(src, temp[0]);

                if (og == null)
                {
                    og = src;
                }

                return GetPropertyValue(og, temp[1]);
            }
            else
            {
                var prop = src.GetType().GetProperty(propName);
                return prop != null ? prop.GetValue(src, null) : null;
            }
        }


        public static Object GetPropValue(this Object obj, String name)
        {
            //foreach (String part in name.Split('.'))
            //{
            //    if (obj == null) { return null; }

            //    Type type = obj.GetType();
            //    PropertyInfo info = type.GetProperty(part);
            //    if (info == null) { return null; }

            //    obj = info.GetValue(obj, null);
            //}
            ////return obj;
            return GetPropertyValue(obj, name);
        }

        public static T GetPropValue<T>(this Object obj, String name)
        {
            Object retval = GetPropValue(obj, name);
            if (retval == null) { return default(T); }

            // throws InvalidCastException if types are incompatible
            return (T)retval;
        }

        public static object GetInstance(string strFullyQualifiedName)
        {
            Type type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return Activator.CreateInstance(type);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type);
            }
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types = null;
                try
                {
                    types = asm.GetTypes();
                }
                finally
                {

                }

                if (types != null & types?.Count() > 0)
                {
                    foreach (var item in types)
                    {
                        if (item.Name == strFullyQualifiedName)
                        {
                            return Activator.CreateInstance(item);
                        }


                    }
                }
               
            }





            return null;
        }

        public static T convertJSON<T>(T instancia,string JSON)
        {
            var obj =(T) Newtonsoft.Json.JsonConvert.DeserializeObject(JSON,instancia.GetType());

            return obj;

        }

        /// <summary>
        /// dynamic expandoObj = dynamicHelper.convertToExpando(myObject);
        //Add Custom Properties dynamicHelper.AddProperty(expandoObj, "dynamicKey", "Some Value");
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ExpandoObject convertToExpando(object obj)
        {
            //Get Properties Using Reflections
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            PropertyInfo[] properties = obj.GetType().GetProperties(flags);

            //Add Them to a new Expando
            ExpandoObject expando = new ExpandoObject();

            foreach (PropertyInfo property in properties)
            {
                AddProperty(expando, property.Name, property.GetValue(obj));
            }

            return expando;
        }

        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            //Take use of the IDictionary implementation
            var expandoDict = expando as IDictionary<String, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        public static ExpandoObject ToExpando(this object obj)
        {
            return JsonConvert.DeserializeObject<ExpandoObject>(JsonConvert.SerializeObject(obj));
        }
        public static dynamic ToDynamic(this object obj)
        {
            return JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(obj));
        }

        public static dynamic ToDynamic(this string obj)
        {
            return JsonConvert.DeserializeObject<dynamic>(obj);
        }

        /// <summary>
        /// Extension method that turns a dictionary of string and object to an ExpandoObject
        /// </summary>
        public static ExpandoObject ToExpando(this IDictionary<string, object> dictionary)
        {
            var expando = new ExpandoObject();
            var expandoDic = (IDictionary<string, object>)expando;

            // go through the items in the dictionary and copy over the key value pairs)
            foreach (var kvp in dictionary)
            {
                // if the value can also be turned into an ExpandoObject, then do it!
                if (kvp.Value is IDictionary<string, object>)
                {
                    var expandoValue = ((IDictionary<string, object>)kvp.Value).ToExpando();
                    expandoDic.Add(kvp.Key, expandoValue);
                }
                else if (kvp.Value is ICollection)
                {
                    // iterate through the collection and convert any strin-object dictionaries
                    // along the way into expando objects
                    var itemList = new List<object>();
                    foreach (var item in (ICollection)kvp.Value)
                    {
                        if (item is IDictionary<string, object>)
                        {
                            var expandoItem = ((IDictionary<string, object>)item).ToExpando();
                            itemList.Add(expandoItem);
                        }
                        else
                        {
                            itemList.Add(item);
                        }
                    }

                    expandoDic.Add(kvp.Key, itemList);
                }
                else
                {
                    expandoDic.Add(kvp);
                }
            }

            return expando;
        }
    }


    //short helper class to ignore some properties from serialization
    public class IgnorePropertiesResolver : DefaultContractResolver
    {
        private readonly HashSet<string> ignoreProps;
        public IgnorePropertiesResolver(IEnumerable<string> propNamesToIgnore)
        {
            this.ignoreProps = new HashSet<string>(propNamesToIgnore);
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            if (this.ignoreProps.Contains(property.PropertyName))
            {
                property.ShouldSerialize = _ => false;
            }
            return property;
        }
    }

}
