using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Helpers
{
    /// <summary>
    /// Utility class for deserializing JSON to Dictionary<string, object>
    /// </summary>
    public static class JsonToDictionaryDeserializer
    {
        /// <summary>
        /// Deserializes JSON string to Dictionary<string, object> using Newtonsoft.Json
        /// </summary>
        /// <param name="json">JSON string to deserialize</param>
        /// <param name="settings">Optional JsonSerializerSettings</param>
        /// <returns>Dictionary representation of JSON</returns>
        public static Dictionary<string, object> DeserializeWithNewtonsoft(
            string json, 
            JsonSerializerSettings? settings = null)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new Dictionary<string, object>();

            try
            {
                var defaultSettings = new JsonSerializerSettings
                {
                    DateParseHandling = DateParseHandling.None,
                    FloatParseHandling = FloatParseHandling.Double,
                    NullValueHandling = NullValueHandling.Include
                };

                var jObject = JsonConvert.DeserializeObject<JObject>(json, settings ?? defaultSettings);
                return ConvertJObjectToDictionary(jObject);
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object> { { "Error", $"JSON Parse Error: {ex.Message}" } };
            }
        }

        /// <summary>
        /// Deserializes JSON string to Dictionary<string, object> using System.Text.Json
        /// </summary>
        /// <param name="json">JSON string to deserialize</param>
        /// <param name="options">Optional JsonSerializerOptions</param>
        /// <returns>Dictionary representation of JSON</returns>
        public static Dictionary<string, object> DeserializeWithSystemTextJson(
            string json, 
            JsonSerializerOptions? options = null)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new Dictionary<string, object>();

            try
            {
                var defaultOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    NumberHandling = JsonNumberHandling.AllowReadingFromString,
                    AllowTrailingCommas = true
                };

                var jsonElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(json, options ?? defaultOptions);
                return ConvertJsonElementToDictionary(jsonElement);
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object> { { "Error", $"JSON Parse Error: {ex.Message}" } };
            }
        }

        /// <summary>
        /// Deserializes JSON to flat dictionary (nested objects become dot-notation keys)
        /// </summary>
        /// <param name="json">JSON string to deserialize</param>
        /// <param name="prefix">Prefix for nested properties</param>
        /// <returns>Flattened dictionary</returns>
        public static Dictionary<string, object> DeserializeToFlatDictionary(
            string json, 
            string prefix = "")
        {
            if (string.IsNullOrWhiteSpace(json))
                return new Dictionary<string, object>();

            try
            {
                var jObject = JsonConvert.DeserializeObject<JObject>(json);
                return FlattenJObject(jObject, prefix);
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object> { { "Error", $"JSON Parse Error: {ex.Message}" } };
            }
        }

        /// <summary>
        /// Deserializes JSON to Dictionary<string, string> (all values as strings)
        /// </summary>
        /// <param name="json">JSON string to deserialize</param>
        /// <param name="includeNullValues">Whether to include null values</param>
        /// <returns>String dictionary</returns>
        public static Dictionary<string, string> DeserializeToStringDictionary(
            string json, 
            bool includeNullValues = true)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new Dictionary<string, string>();

            try
            {
                var objectDict = DeserializeWithNewtonsoft(json);
                var stringDict = new Dictionary<string, string>();

                foreach (var kvp in objectDict)
                {
                    if (!includeNullValues && kvp.Value == null)
                        continue;

                    stringDict[kvp.Key] = ConvertValueToString(kvp.Value);
                }

                return stringDict;
            }
            catch (Exception ex)
            {
                return new Dictionary<string, string> { { "Error", $"JSON Parse Error: {ex.Message}" } };
            }
        }

        /// <summary>
        /// Deserializes JSON with type preservation (attempts to maintain original types)
        /// </summary>
        /// <param name="json">JSON string to deserialize</param>
        /// <returns>Dictionary with preserved types</returns>
        public static Dictionary<string, object> DeserializeWithTypePreservation(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new Dictionary<string, object>();

            try
            {
                var settings = new JsonSerializerSettings
                {
                    DateParseHandling = DateParseHandling.DateTime,
                    FloatParseHandling = FloatParseHandling.Double,
                    TypeNameHandling = TypeNameHandling.Auto
                };

                var jObject = JsonConvert.DeserializeObject<JObject>(json, settings);
                return ConvertJObjectToDictionaryWithTypes(jObject);
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object> { { "Error", $"JSON Parse Error: {ex.Message}" } };
            }
        }

        /// <summary>
        /// Deserializes JSON array to List of dictionaries
        /// </summary>
        /// <param name="json">JSON array string</param>
        /// <returns>List of dictionaries</returns>
        public static List<Dictionary<string, object>> DeserializeJsonArray(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<Dictionary<string, object>>();

            try
            {
                var jArray = JsonConvert.DeserializeObject<JArray>(json);
                var result = new List<Dictionary<string, object>>();

                foreach (var item in jArray)
                {
                    if (item is JObject jObj)
                    {
                        result.Add(ConvertJObjectToDictionary(jObj));
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return new List<Dictionary<string, object>> 
                { 
                    new Dictionary<string, object> { { "Error", $"JSON Array Parse Error: {ex.Message}" } } 
                };
            }
        }

        /// <summary>
        /// Deserializes JSON with custom property name mapping
        /// </summary>
        /// <param name="json">JSON string to deserialize</param>
        /// <param name="propertyNameMapper">Function to map property names</param>
        /// <returns>Dictionary with mapped property names</returns>
        public static Dictionary<string, object> DeserializeWithPropertyMapping(
            string json,
            Func<string, string> propertyNameMapper)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new Dictionary<string, object>();

            try
            {
                var originalDict = DeserializeWithNewtonsoft(json);
                var mappedDict = new Dictionary<string, object>();

                foreach (var kvp in originalDict)
                {
                    var mappedKey = propertyNameMapper?.Invoke(kvp.Key) ?? kvp.Key;
                    mappedDict[mappedKey] = kvp.Value;
                }

                return mappedDict;
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object> { { "Error", $"JSON Parse Error: {ex.Message}" } };
            }
        }

        /// <summary>
        /// Validates and deserializes JSON with validation info
        /// </summary>
        /// <param name="json">JSON string to deserialize</param>
        /// <returns>Dictionary with validation info and data</returns>
        public static Dictionary<string, object> DeserializeWithValidation(string json)
        {
            var result = new Dictionary<string, object>();

            if (string.IsNullOrWhiteSpace(json))
            {
                result["IsValid"] = false;
                result["Error"] = "JSON string is null or empty";
                result["Data"] = new Dictionary<string, object>();
                return result;
            }

            try
            {
                // Validate JSON structure
                var jToken = JToken.Parse(json);
                
                result["IsValid"] = true;
                result["JsonType"] = jToken.Type.ToString();
                result["PropertyCount"] = jToken is JObject jObj ? jObj.Properties().Count() : 0;
                
                if (jToken is JObject jsonObject)
                {
                    result["Data"] = ConvertJObjectToDictionary(jsonObject);
                }
                else if (jToken is JArray jsonArray)
                {
                    result["Data"] = jsonArray.ToObject<object[]>();
                }
                else
                {
                    result["Data"] = jToken.ToObject<object>();
                }
            }
            catch (Exception ex)
            {
                result["IsValid"] = false;
                result["Error"] = ex.Message;
                result["Data"] = new Dictionary<string, object>();
            }

            return result;
        }

        #region Helper Methods

        /// <summary>
        /// Converts JObject to Dictionary<string, object>
        /// </summary>
        private static Dictionary<string, object> ConvertJObjectToDictionary(JObject? jObject)
        {
            if (jObject == null)
                return new Dictionary<string, object>();

            var dictionary = new Dictionary<string, object>();

            foreach (var property in jObject.Properties())
            {
                dictionary[property.Name] = ConvertJTokenToObject(property.Value);
            }

            return dictionary;
        }

        /// <summary>
        /// Converts JObject to Dictionary with type information
        /// </summary>
        private static Dictionary<string, object> ConvertJObjectToDictionaryWithTypes(JObject? jObject)
        {
            if (jObject == null)
                return new Dictionary<string, object>();

            var dictionary = new Dictionary<string, object>();

            foreach (var property in jObject.Properties())
            {
                var value = ConvertJTokenToObject(property.Value);
                dictionary[property.Name] = new
                {
                    Value = value,
                    Type = value?.GetType().Name ?? "null",
                    JsonType = property.Value.Type.ToString()
                };
            }

            return dictionary;
        }

        /// <summary>
        /// Converts JsonElement to Dictionary<string, object>
        /// </summary>
        private static Dictionary<string, object> ConvertJsonElementToDictionary(JsonElement element)
        {
            var dictionary = new Dictionary<string, object>();

            if (element.ValueKind == JsonValueKind.Object)
            {
                foreach (var property in element.EnumerateObject())
                {
                    dictionary[property.Name] = ConvertJsonElementToObject(property.Value);
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Converts JToken to appropriate .NET object
        /// </summary>
        private static object? ConvertJTokenToObject(JToken token)
        {
            return token.Type switch
            {
                JTokenType.Object => ConvertJObjectToDictionary((JObject)token),
                JTokenType.Array => token.ToObject<object[]>(),
                JTokenType.Integer => token.ToObject<long>(),
                JTokenType.Float => token.ToObject<double>(),
                JTokenType.String => token.ToObject<string>(),
                JTokenType.Boolean => token.ToObject<bool>(),
                JTokenType.Date => token.ToObject<DateTime>(),
                JTokenType.Null => null,
                _ => token.ToString()
            };
        }

        /// <summary>
        /// Converts JsonElement to appropriate .NET object
        /// </summary>
        private static object? ConvertJsonElementToObject(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.Object => ConvertJsonElementToDictionary(element),
                JsonValueKind.Array => element.EnumerateArray().Select(ConvertJsonElementToObject).ToArray(),
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => element.TryGetInt64(out var longVal) ? longVal : element.GetDouble(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => element.ToString()
            };
        }

        /// <summary>
        /// Flattens JObject to dot-notation dictionary
        /// </summary>
        private static Dictionary<string, object> FlattenJObject(JObject? jObject, string prefix = "")
        {
            var result = new Dictionary<string, object>();

            if (jObject == null)
                return result;

            foreach (var property in jObject.Properties())
            {
                var key = string.IsNullOrEmpty(prefix) ? property.Name : $"{prefix}.{property.Name}";

                if (property.Value.Type == JTokenType.Object)
                {
                    var nestedDict = FlattenJObject((JObject)property.Value, key);
                    foreach (var kvp in nestedDict)
                    {
                        result[kvp.Key] = kvp.Value;
                    }
                }
                else if (property.Value.Type == JTokenType.Array)
                {
                    var array = (JArray)property.Value;
                    for (int i = 0; i < array.Count; i++)
                    {
                        if (array[i].Type == JTokenType.Object)
                        {
                            var arrayItemDict = FlattenJObject((JObject)array[i], $"{key}[{i}]");
                            foreach (var kvp in arrayItemDict)
                            {
                                result[kvp.Key] = kvp.Value;
                            }
                        }
                        else
                        {
                            result[$"{key}[{i}]"] = ConvertJTokenToObject(array[i]);
                        }
                    }
                }
                else
                {
                    result[key] = ConvertJTokenToObject(property.Value);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts any value to string representation
        /// </summary>
        private static string ConvertValueToString(object? value)
        {
            if (value == null)
                return "null";

            if (value is string str)
                return str;

            if (value is DateTime dt)
                return dt.ToString("yyyy-MM-dd HH:mm:ss");

            if (value is bool boolean)
                return boolean.ToString().ToLower();

            if (value is Array || value.GetType().IsGenericType)
                return JsonConvert.SerializeObject(value);

            return value.ToString() ?? "null";
        }

        #endregion
    }
}
