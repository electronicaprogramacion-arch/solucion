using System;
using System.Collections.Generic;
using System.Linq;
using CalibrationSaaS.Utilities;
using Newtonsoft.Json;

namespace CalibrationSaaS.Examples
{
    /// <summary>
    /// Examples and test cases for JsonToDictionaryDeserializer
    /// </summary>
    public class JsonToDictionaryExamples
    {
        /// <summary>
        /// Example 1: Basic JSON object deserialization
        /// </summary>
        public static void Example1_BasicJsonDeserialization()
        {
            Console.WriteLine("=== Example 1: Basic JSON Deserialization ===");

            var json = @"{
                ""name"": ""Juan Pérez"",
                ""age"": 30,
                ""email"": ""juan@example.com"",
                ""isActive"": true,
                ""salary"": 50000.50,
                ""birthDate"": ""1993-05-15T00:00:00""
            }";

            // Using Newtonsoft.Json
            var dictNewtonsoft = JsonToDictionaryDeserializer.DeserializeWithNewtonsoft(json);
            Console.WriteLine("Newtonsoft.Json Result:");
            foreach (var kvp in dictNewtonsoft)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value} ({kvp.Value?.GetType().Name})");
            }

            Console.WriteLine();

            // Using System.Text.Json
            var dictSystemText = JsonToDictionaryDeserializer.DeserializeWithSystemTextJson(json);
            Console.WriteLine("System.Text.Json Result:");
            foreach (var kvp in dictSystemText)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value} ({kvp.Value?.GetType().Name})");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Example 2: Nested JSON objects
        /// </summary>
        public static void Example2_NestedJsonObjects()
        {
            Console.WriteLine("=== Example 2: Nested JSON Objects ===");

            var json = @"{
                ""deviceId"": ""TEMP-SENSOR-001"",
                ""calibrationDate"": ""2025-09-16T10:30:00"",
                ""measurements"": {
                    ""temperature"": 25.5,
                    ""pressure"": 1013.25,
                    ""humidity"": 45.2
                },
                ""technician"": {
                    ""name"": ""María García"",
                    ""id"": ""TECH-001"",
                    ""certification"": ""Level-3""
                },
                ""isValid"": true
            }";

            var dictionary = JsonToDictionaryDeserializer.DeserializeWithNewtonsoft(json);
            
            Console.WriteLine("Nested JSON Dictionary:");
            foreach (var kvp in dictionary)
            {
                if (kvp.Value is Dictionary<string, object> nestedDict)
                {
                    Console.WriteLine($"  {kvp.Key}: [Nested Object]");
                    foreach (var nestedKvp in nestedDict)
                    {
                        Console.WriteLine($"    {nestedKvp.Key}: {nestedKvp.Value}");
                    }
                }
                else
                {
                    Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
                }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Example 3: Flat dictionary from nested JSON
        /// </summary>
        public static void Example3_FlatDictionary()
        {
            Console.WriteLine("=== Example 3: Flat Dictionary from Nested JSON ===");

            var json = @"{
                ""device"": {
                    ""id"": ""CAL-001"",
                    ""type"": ""Temperature Sensor"",
                    ""location"": {
                        ""building"": ""Lab A"",
                        ""room"": ""101"",
                        ""coordinates"": {
                            ""x"": 10.5,
                            ""y"": 20.3
                        }
                    }
                },
                ""calibration"": {
                    ""date"": ""2025-09-16"",
                    ""technician"": ""Pedro Martínez"",
                    ""results"": [25.1, 25.2, 25.0]
                }
            }";

            var flatDict = JsonToDictionaryDeserializer.DeserializeToFlatDictionary(json);
            
            Console.WriteLine("Flat Dictionary (dot notation):");
            foreach (var kvp in flatDict.OrderBy(x => x.Key))
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Example 4: JSON array deserialization
        /// </summary>
        public static void Example4_JsonArrayDeserialization()
        {
            Console.WriteLine("=== Example 4: JSON Array Deserialization ===");

            var json = @"[
                {
                    ""deviceId"": ""TEMP-001"",
                    ""temperature"": 25.5,
                    ""timestamp"": ""2025-09-16T10:00:00""
                },
                {
                    ""deviceId"": ""TEMP-002"",
                    ""temperature"": 26.1,
                    ""timestamp"": ""2025-09-16T10:01:00""
                },
                {
                    ""deviceId"": ""TEMP-003"",
                    ""temperature"": 24.8,
                    ""timestamp"": ""2025-09-16T10:02:00""
                }
            ]";

            var arrayOfDictionaries = JsonToDictionaryDeserializer.DeserializeJsonArray(json);
            
            Console.WriteLine("JSON Array as List of Dictionaries:");
            for (int i = 0; i < arrayOfDictionaries.Count; i++)
            {
                Console.WriteLine($"  Item {i + 1}:");
                foreach (var kvp in arrayOfDictionaries[i])
                {
                    Console.WriteLine($"    {kvp.Key}: {kvp.Value}");
                }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Example 5: String dictionary conversion
        /// </summary>
        public static void Example5_StringDictionary()
        {
            Console.WriteLine("=== Example 5: String Dictionary Conversion ===");

            var json = @"{
                ""deviceId"": ""PRESSURE-001"",
                ""pressure"": 1013.25,
                ""temperature"": 22.3,
                ""isCalibrated"": true,
                ""lastCalibration"": ""2025-09-15T14:30:00"",
                ""nullValue"": null,
                ""arrayValue"": [1, 2, 3]
            }";

            var stringDict = JsonToDictionaryDeserializer.DeserializeToStringDictionary(json);
            
            Console.WriteLine("String Dictionary (all values as strings):");
            foreach (var kvp in stringDict)
            {
                Console.WriteLine($"  {kvp.Key}: \"{kvp.Value}\"");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Example 6: Property name mapping
        /// </summary>
        public static void Example6_PropertyNameMapping()
        {
            Console.WriteLine("=== Example 6: Property Name Mapping ===");

            var json = @"{
                ""DeviceId"": ""FLOW-001"",
                ""CalibrationDate"": ""2025-09-16T15:00:00"",
                ""FlowRate"": 125.7,
                ""IsValid"": true,
                ""TechnicianName"": ""Ana López""
            }";

            // Convert PascalCase to camelCase
            var camelCaseDict = JsonToDictionaryDeserializer.DeserializeWithPropertyMapping(
                json, 
                propName => char.ToLowerInvariant(propName[0]) + propName.Substring(1)
            );
            
            Console.WriteLine("Property Name Mapping (PascalCase → camelCase):");
            foreach (var kvp in camelCaseDict)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Example 7: JSON validation and deserialization
        /// </summary>
        public static void Example7_JsonValidation()
        {
            Console.WriteLine("=== Example 7: JSON Validation ===");

            var validJson = @"{""temperature"": 25.5, ""isValid"": true}";
            var invalidJson = @"{""temperature"": 25.5, ""isValid"": true"; // Missing closing brace

            Console.WriteLine("Valid JSON:");
            var validResult = JsonToDictionaryDeserializer.DeserializeWithValidation(validJson);
            foreach (var kvp in validResult)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine("\nInvalid JSON:");
            var invalidResult = JsonToDictionaryDeserializer.DeserializeWithValidation(invalidJson);
            foreach (var kvp in invalidResult)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Example 8: Type preservation
        /// </summary>
        public static void Example8_TypePreservation()
        {
            Console.WriteLine("=== Example 8: Type Preservation ===");

            var json = @"{
                ""intValue"": 42,
                ""doubleValue"": 3.14159,
                ""stringValue"": ""Hello World"",
                ""boolValue"": true,
                ""dateValue"": ""2025-09-16T10:30:00"",
                ""nullValue"": null
            }";

            var typedDict = JsonToDictionaryDeserializer.DeserializeWithTypePreservation(json);
            
            Console.WriteLine("Type Preservation Dictionary:");
            foreach (var kvp in typedDict)
            {
                Console.WriteLine($"  {kvp.Key}: {JsonConvert.SerializeObject(kvp.Value, Formatting.None)}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Example 9: CalibrationSaaS specific scenarios
        /// </summary>
        public static void Example9_CalibrationScenarios()
        {
            Console.WriteLine("=== Example 9: CalibrationSaaS Scenarios ===");

            // Scenario 1: Device configuration JSON
            var deviceConfigJson = @"{
                ""deviceConfiguration"": {
                    ""deviceId"": ""MULTI-SENSOR-001"",
                    ""deviceType"": ""Multi-Parameter Sensor"",
                    ""parameters"": {
                        ""temperature"": {
                            ""range"": ""-40 to 85°C"",
                            ""accuracy"": ""±0.1°C"",
                            ""resolution"": ""0.01°C""
                        },
                        ""pressure"": {
                            ""range"": ""0 to 2000 hPa"",
                            ""accuracy"": ""±0.5 hPa"",
                            ""resolution"": ""0.1 hPa""
                        }
                    },
                    ""calibrationSchedule"": {
                        ""frequency"": ""Monthly"",
                        ""nextDue"": ""2025-10-16T09:00:00"",
                        ""responsible"": ""Quality Team""
                    }
                }
            }";

            Console.WriteLine("1. Device Configuration (Nested):");
            var deviceDict = JsonToDictionaryDeserializer.DeserializeWithNewtonsoft(deviceConfigJson);
            PrintNestedDictionary(deviceDict, "  ");

            Console.WriteLine("\n2. Device Configuration (Flat for Database):");
            var deviceFlatDict = JsonToDictionaryDeserializer.DeserializeToFlatDictionary(deviceConfigJson);
            foreach (var kvp in deviceFlatDict.OrderBy(x => x.Key))
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            // Scenario 2: Calibration results JSON array
            var calibrationResultsJson = @"[
                {
                    ""measurementId"": ""M001"",
                    ""parameter"": ""Temperature"",
                    ""referenceValue"": 25.0,
                    ""measuredValue"": 25.1,
                    ""deviation"": 0.1,
                    ""withinTolerance"": true
                },
                {
                    ""measurementId"": ""M002"",
                    ""parameter"": ""Pressure"",
                    ""referenceValue"": 1013.25,
                    ""measuredValue"": 1013.30,
                    ""deviation"": 0.05,
                    ""withinTolerance"": true
                }
            ]";

            Console.WriteLine("\n3. Calibration Results Array:");
            var resultsArray = JsonToDictionaryDeserializer.DeserializeJsonArray(calibrationResultsJson);
            for (int i = 0; i < resultsArray.Count; i++)
            {
                Console.WriteLine($"  Measurement {i + 1}:");
                foreach (var kvp in resultsArray[i])
                {
                    Console.WriteLine($"    {kvp.Key}: {kvp.Value}");
                }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Helper method to print nested dictionaries
        /// </summary>
        private static void PrintNestedDictionary(Dictionary<string, object> dict, string indent)
        {
            foreach (var kvp in dict)
            {
                if (kvp.Value is Dictionary<string, object> nestedDict)
                {
                    Console.WriteLine($"{indent}{kvp.Key}: [Nested Object]");
                    PrintNestedDictionary(nestedDict, indent + "  ");
                }
                else
                {
                    Console.WriteLine($"{indent}{kvp.Key}: {kvp.Value}");
                }
            }
        }

        /// <summary>
        /// Run all examples
        /// </summary>
        public static void RunAllExamples()
        {
            Console.WriteLine("🔄 JSON TO DICTIONARY DESERIALIZER - EXAMPLES");
            Console.WriteLine("==============================================");
            Console.WriteLine();

            Example1_BasicJsonDeserialization();
            Example2_NestedJsonObjects();
            Example3_FlatDictionary();
            Example4_JsonArrayDeserialization();
            Example5_StringDictionary();
            Example6_PropertyNameMapping();
            Example7_JsonValidation();
            Example8_TypePreservation();
            Example9_CalibrationScenarios();

            Console.WriteLine("✅ All JSON deserialization examples completed!");
        }
    }
}
