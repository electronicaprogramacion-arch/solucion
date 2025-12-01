using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CalibrationSaaS.Utilities
{
    /// <summary>
    /// Utility class for deserializing JSON to Dictionary<string, object>
    /// </summary>
    public static class JsonToDictionaryDeserializer
    {
        /// <summary>
        /// Deserializes JSON string to Dictionary<string, object> using Newtonsoft.Json
        /// </summary>
        public static Dictionary<string, object> DeserializeWithNewtonsoft(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new Dictionary<string, object>();

            try
            {
                var jObject = JsonConvert.DeserializeObject<JObject>(json);
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
        public static Dictionary<string, object> DeserializeWithSystemTextJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new Dictionary<string, object>();

            try
            {
                var jsonElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(json);
                return ConvertJsonElementToDictionary(jsonElement);
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object> { { "Error", $"JSON Parse Error: {ex.Message}" } };
            }
        }

        /// <summary>
        /// Deserializes JSON to Dictionary<string, string> (all values as strings)
        /// </summary>
        public static Dictionary<string, string> DeserializeToStringDictionary(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new Dictionary<string, string>();

            try
            {
                var objectDict = DeserializeWithNewtonsoft(json);
                var stringDict = new Dictionary<string, string>();

                foreach (var kvp in objectDict)
                {
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
        /// Deserializes JSON array to List of dictionaries
        /// </summary>
        public static List<Dictionary<string, object>> DeserializeJsonArray(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<Dictionary<string, object>>();

            try
            {
                var jArray = JsonConvert.DeserializeObject<JArray>(json);
                var result = new List<Dictionary<string, object>>();

                if (jArray != null)
                {
                    foreach (var item in jArray)
                    {
                        if (item is JObject jObj)
                        {
                            result.Add(ConvertJObjectToDictionary(jObj));
                        }
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
        /// Validates and deserializes JSON with validation info
        /// </summary>
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
                var jToken = JToken.Parse(json);
                
                result["IsValid"] = true;
                result["JsonType"] = jToken.Type.ToString();
                
                if (jToken is JObject jsonObject)
                {
                    result["PropertyCount"] = jsonObject.Properties().Count();
                    result["Data"] = ConvertJObjectToDictionary(jsonObject);
                }
                else if (jToken is JArray jsonArray)
                {
                    result["PropertyCount"] = jsonArray.Count;
                    result["Data"] = jsonArray.ToObject<object[]>();
                }
                else
                {
                    result["PropertyCount"] = 1;
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

            if (value is Array || (value.GetType().IsGenericType && value.GetType().GetGenericTypeDefinition() == typeof(List<>)))
                return JsonConvert.SerializeObject(value);

            return value.ToString() ?? "null";
        }

        #endregion
    }
}

namespace CalibrationSaaS.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("🔄 JSON TO DICTIONARY DESERIALIZER - TEST APPLICATION");
            Console.WriteLine("======================================================");
            Console.WriteLine();

            // Test 1: Basic JSON deserialization
            TestBasicJsonDeserialization();

            // Test 2: Nested JSON objects
            TestNestedJsonObjects();

            // Test 3: JSON array deserialization
            TestJsonArrayDeserialization();

            // Test 4: String dictionary conversion
            TestStringDictionaryConversion();

            // Test 5: JSON validation
            TestJsonValidation();

            Console.WriteLine("✅ All JSON deserialization tests completed!");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void TestBasicJsonDeserialization()
        {
            Console.WriteLine("=== Test 1: Basic JSON Deserialization ===");

            var json = @"{
                ""deviceId"": ""TEMP-SENSOR-001"",
                ""temperature"": 25.5,
                ""pressure"": 1013.25,
                ""isActive"": true,
                ""lastCalibration"": ""2025-09-16T10:30:00"",
                ""nullValue"": null
            }";

            Console.WriteLine("Original JSON:");
            Console.WriteLine(json);
            Console.WriteLine();

            // Using Newtonsoft.Json
            var dictNewtonsoft = JsonToDictionaryDeserializer.DeserializeWithNewtonsoft(json);
            Console.WriteLine("Newtonsoft.Json Result:");
            foreach (var kvp in dictNewtonsoft)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value} ({kvp.Value?.GetType().Name ?? "null"})");
            }

            Console.WriteLine();

            // Using System.Text.Json
            var dictSystemText = JsonToDictionaryDeserializer.DeserializeWithSystemTextJson(json);
            Console.WriteLine("System.Text.Json Result:");
            foreach (var kvp in dictSystemText)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value} ({kvp.Value?.GetType().Name ?? "null"})");
            }

            Console.WriteLine();
        }

        static void TestNestedJsonObjects()
        {
            Console.WriteLine("=== Test 2: Nested JSON Objects ===");

            var json = @"{
                ""calibration"": {
                    ""deviceId"": ""MULTI-SENSOR-001"",
                    ""date"": ""2025-09-16T15:00:00"",
                    ""technician"": {
                        ""name"": ""María García"",
                        ""id"": ""TECH-001"",
                        ""level"": ""Senior""
                    },
                    ""measurements"": {
                        ""temperature"": 23.5,
                        ""pressure"": 1012.8,
                        ""humidity"": 45.2
                    }
                },
                ""status"": ""Completed""
            }";

            var dictionary = JsonToDictionaryDeserializer.DeserializeWithNewtonsoft(json);
            
            Console.WriteLine("Nested JSON Dictionary:");
            PrintNestedDictionary(dictionary, "  ");

            Console.WriteLine();
        }

        static void TestJsonArrayDeserialization()
        {
            Console.WriteLine("=== Test 3: JSON Array Deserialization ===");

            var json = @"[
                {
                    ""measurementId"": ""M001"",
                    ""parameter"": ""Temperature"",
                    ""value"": 25.1,
                    ""unit"": ""°C""
                },
                {
                    ""measurementId"": ""M002"",
                    ""parameter"": ""Pressure"",
                    ""value"": 1013.25,
                    ""unit"": ""hPa""
                },
                {
                    ""measurementId"": ""M003"",
                    ""parameter"": ""Humidity"",
                    ""value"": 45.2,
                    ""unit"": ""%""
                }
            ]";

            var arrayOfDictionaries = JsonToDictionaryDeserializer.DeserializeJsonArray(json);
            
            Console.WriteLine("JSON Array as List of Dictionaries:");
            for (int i = 0; i < arrayOfDictionaries.Count; i++)
            {
                Console.WriteLine($"  Measurement {i + 1}:");
                foreach (var kvp in arrayOfDictionaries[i])
                {
                    Console.WriteLine($"    {kvp.Key}: {kvp.Value}");
                }
            }

            Console.WriteLine();
        }

        static void TestStringDictionaryConversion()
        {
            Console.WriteLine("=== Test 4: String Dictionary Conversion ===");

            var json = @"{
                ""deviceId"": ""FLOW-METER-001"",
                ""flowRate"": 125.7,
                ""temperature"": 22.3,
                ""isCalibrated"": true,
                ""lastMaintenance"": ""2025-09-15T14:30:00"",
                ""arrayValue"": [1, 2, 3, 4, 5],
                ""nullValue"": null
            }";

            var stringDict = JsonToDictionaryDeserializer.DeserializeToStringDictionary(json);
            
            Console.WriteLine("String Dictionary (all values as strings):");
            foreach (var kvp in stringDict)
            {
                Console.WriteLine($"  {kvp.Key}: \"{kvp.Value}\"");
            }

            Console.WriteLine();
        }

        static void TestJsonValidation()
        {
            Console.WriteLine("=== Test 5: JSON Validation ===");

            var validJson = @"{""temperature"": 25.5, ""isValid"": true, ""deviceId"": ""TEMP-001""}";
            var invalidJson = @"{""temperature"": 25.5, ""isValid"": true"; // Missing closing brace

            Console.WriteLine("Valid JSON Test:");
            var validResult = JsonToDictionaryDeserializer.DeserializeWithValidation(validJson);
            foreach (var kvp in validResult)
            {
                if (kvp.Key == "Data" && kvp.Value is Dictionary<string, object> dataDict)
                {
                    Console.WriteLine($"  {kvp.Key}:");
                    foreach (var dataKvp in dataDict)
                    {
                        Console.WriteLine($"    {dataKvp.Key}: {dataKvp.Value}");
                    }
                }
                else
                {
                    Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
                }
            }

            Console.WriteLine("\nInvalid JSON Test:");
            var invalidResult = JsonToDictionaryDeserializer.DeserializeWithValidation(invalidJson);
            foreach (var kvp in invalidResult)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Helper method to print nested dictionaries
        /// </summary>
        static void PrintNestedDictionary(Dictionary<string, object> dict, string indent)
        {
            foreach (var kvp in dict)
            {
                if (kvp.Value is Dictionary<string, object> nestedDict)
                {
                    Console.WriteLine($"{indent}{kvp.Key}: [Nested Object]");
                    PrintNestedDictionary(nestedDict, indent + "  ");
                }
                else if (kvp.Value is object[] array)
                {
                    Console.WriteLine($"{indent}{kvp.Key}: [Array with {array.Length} items]");
                    for (int i = 0; i < array.Length; i++)
                    {
                        Console.WriteLine($"{indent}  [{i}]: {array[i]}");
                    }
                }
                else
                {
                    Console.WriteLine($"{indent}{kvp.Key}: {kvp.Value} ({kvp.Value?.GetType().Name ?? "null"})");
                }
            }
        }
    }
}
