using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Helpers
{
    /// <summary>
    /// Utility class for converting objects to dictionaries with various options
    /// </summary>
    public static class ObjectToDictionaryConverter
    {
        /// <summary>
        /// Converts an object to Dictionary<string, object> using reflection
        /// </summary>
        /// <param name="obj">Object to convert</param>
        /// <param name="includeNullValues">Whether to include properties with null values</param>
        /// <param name="includePrivateProperties">Whether to include private properties</param>
        /// <returns>Dictionary representation of the object</returns>
        public static Dictionary<string, object> ToDictionary(
            this object obj, 
            bool includeNullValues = true, 
            bool includePrivateProperties = false)
        {
            if (obj == null)
                return new Dictionary<string, object>();

            var dictionary = new Dictionary<string, object>();
            var bindingFlags = BindingFlags.Public | BindingFlags.Instance;
            
            if (includePrivateProperties)
                bindingFlags |= BindingFlags.NonPublic;

            var properties = obj.GetType().GetProperties(bindingFlags);

            foreach (var property in properties)
            {
                if (!property.CanRead)
                    continue;

                try
                {
                    var value = property.GetValue(obj);
                    
                    if (!includeNullValues && value == null)
                        continue;

                    dictionary[property.Name] = value;
                }
                catch (Exception ex)
                {
                    // Handle properties that might throw exceptions when accessed
                    dictionary[property.Name] = $"Error: {ex.Message}";
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Converts an object to Dictionary<string, string> with string values only
        /// </summary>
        /// <param name="obj">Object to convert</param>
        /// <param name="includeNullValues">Whether to include null values as "null"</param>
        /// <returns>Dictionary with string keys and values</returns>
        public static Dictionary<string, string> ToStringDictionary(
            this object obj, 
            bool includeNullValues = true)
        {
            if (obj == null)
                return new Dictionary<string, string>();

            var dictionary = new Dictionary<string, string>();
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (!property.CanRead)
                    continue;

                try
                {
                    var value = property.GetValue(obj);
                    
                    if (!includeNullValues && value == null)
                        continue;

                    dictionary[property.Name] = value?.ToString() ?? "null";
                }
                catch (Exception ex)
                {
                    dictionary[property.Name] = $"Error: {ex.Message}";
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Converts an object to dictionary using JSON serialization (handles complex nested objects)
        /// </summary>
        /// <param name="obj">Object to convert</param>
        /// <param name="settings">JSON serializer settings</param>
        /// <returns>Dictionary representation</returns>
        public static Dictionary<string, object> ToDictionaryViaJson(
            this object obj, 
            JsonSerializerSettings settings = null)
        {
            if (obj == null)
                return new Dictionary<string, object>();

            try
            {
                var json = JsonConvert.SerializeObject(obj, settings ?? new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                var jObject = JObject.Parse(json);
                return jObject.ToObject<Dictionary<string, object>>();
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object> { { "Error", ex.Message } };
            }
        }

        /// <summary>
        /// Converts an object to a flat dictionary (nested objects become dot-notation keys)
        /// </summary>
        /// <param name="obj">Object to convert</param>
        /// <param name="prefix">Prefix for nested properties</param>
        /// <returns>Flattened dictionary</returns>
        public static Dictionary<string, object> ToFlatDictionary(
            this object obj, 
            string prefix = "")
        {
            var result = new Dictionary<string, object>();
            
            if (obj == null)
                return result;

            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (!property.CanRead)
                    continue;

                try
                {
                    var value = property.GetValue(obj);
                    var key = string.IsNullOrEmpty(prefix) ? property.Name : $"{prefix}.{property.Name}";

                    if (value == null)
                    {
                        result[key] = null;
                    }
                    else if (IsSimpleType(property.PropertyType))
                    {
                        result[key] = value;
                    }
                    else if (value is System.Collections.IEnumerable enumerable && !(value is string))
                    {
                        // Handle collections
                        var index = 0;
                        foreach (var item in enumerable)
                        {
                            var itemDict = ToFlatDictionary(item, $"{key}[{index}]");
                            foreach (var kvp in itemDict)
                            {
                                result[kvp.Key] = kvp.Value;
                            }
                            index++;
                        }
                    }
                    else
                    {
                        // Handle nested objects
                        var nestedDict = ToFlatDictionary(value, key);
                        foreach (var kvp in nestedDict)
                        {
                            result[kvp.Key] = kvp.Value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    var key = string.IsNullOrEmpty(prefix) ? property.Name : $"{prefix}.{property.Name}";
                    result[key] = $"Error: {ex.Message}";
                }
            }

            return result;
        }

        /// <summary>
        /// Converts an object to dictionary with custom property name mapping
        /// </summary>
        /// <param name="obj">Object to convert</param>
        /// <param name="propertyNameMapper">Function to map property names</param>
        /// <returns>Dictionary with mapped property names</returns>
        public static Dictionary<string, object> ToDictionaryWithMapping(
            this object obj,
            Func<string, string> propertyNameMapper)
        {
            if (obj == null)
                return new Dictionary<string, object>();

            var dictionary = new Dictionary<string, object>();
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (!property.CanRead)
                    continue;

                try
                {
                    var value = property.GetValue(obj);
                    var mappedName = propertyNameMapper?.Invoke(property.Name) ?? property.Name;
                    dictionary[mappedName] = value;
                }
                catch (Exception ex)
                {
                    var mappedName = propertyNameMapper?.Invoke(property.Name) ?? property.Name;
                    dictionary[mappedName] = $"Error: {ex.Message}";
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Converts an object to dictionary with type information
        /// </summary>
        /// <param name="obj">Object to convert</param>
        /// <returns>Dictionary with values and their types</returns>
        public static Dictionary<string, object> ToDictionaryWithTypes(this object obj)
        {
            if (obj == null)
                return new Dictionary<string, object>();

            var dictionary = new Dictionary<string, object>();
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (!property.CanRead)
                    continue;

                try
                {
                    var value = property.GetValue(obj);
                    dictionary[property.Name] = new
                    {
                        Value = value,
                        Type = property.PropertyType.Name,
                        FullTypeName = property.PropertyType.FullName
                    };
                }
                catch (Exception ex)
                {
                    dictionary[property.Name] = new
                    {
                        Value = $"Error: {ex.Message}",
                        Type = "Error",
                        FullTypeName = "Error"
                    };
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Converts an object to dictionary excluding specified properties
        /// </summary>
        /// <param name="obj">Object to convert</param>
        /// <param name="excludeProperties">Properties to exclude</param>
        /// <returns>Dictionary without excluded properties</returns>
        public static Dictionary<string, object> ToDictionaryExcluding(
            this object obj, 
            params string[] excludeProperties)
        {
            if (obj == null)
                return new Dictionary<string, object>();

            var excludeSet = new HashSet<string>(excludeProperties ?? new string[0]);
            var dictionary = new Dictionary<string, object>();
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (!property.CanRead || excludeSet.Contains(property.Name))
                    continue;

                try
                {
                    var value = property.GetValue(obj);
                    dictionary[property.Name] = value;
                }
                catch (Exception ex)
                {
                    dictionary[property.Name] = $"Error: {ex.Message}";
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Converts an object to dictionary including only specified properties
        /// </summary>
        /// <param name="obj">Object to convert</param>
        /// <param name="includeProperties">Properties to include</param>
        /// <returns>Dictionary with only included properties</returns>
        public static Dictionary<string, object> ToDictionaryIncluding(
            this object obj, 
            params string[] includeProperties)
        {
            if (obj == null)
                return new Dictionary<string, object>();

            var includeSet = new HashSet<string>(includeProperties ?? new string[0]);
            var dictionary = new Dictionary<string, object>();
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (!property.CanRead || !includeSet.Contains(property.Name))
                    continue;

                try
                {
                    var value = property.GetValue(obj);
                    dictionary[property.Name] = value;
                }
                catch (Exception ex)
                {
                    dictionary[property.Name] = $"Error: {ex.Message}";
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Helper method to determine if a type is a simple type
        /// </summary>
        private static bool IsSimpleType(Type type)
        {
            return type.IsPrimitive ||
                   type.IsEnum ||
                   type == typeof(string) ||
                   type == typeof(decimal) ||
                   type == typeof(DateTime) ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(TimeSpan) ||
                   type == typeof(Guid) ||
                   Nullable.GetUnderlyingType(type) != null;
        }
    }

    public static class DictionaryToObject
    {
        public static void Map<T>(this Dictionary<string, object> dict, T obj)
        {
            var type = typeof(T);
            var properties = type.GetProperties().Where(p => p.CanRead && p.CanWrite && dict.ContainsKey(p.Name));

            foreach (var property in properties)
            {
                var value = Convert.ChangeType(dict[property.Name], property.PropertyType);
                property.SetValue(obj, value);
            }
        }

        public static T Map<T>(this Dictionary<string, object> dict) where T : new()
        {
            var result = new T();
            Map(dict, result);
            return result;
        }
    }


}
