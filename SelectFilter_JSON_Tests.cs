using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using CalibrationSaaS.Infraestructure.Blazor.Shared;
using Helpers.Models;

namespace CalibrationSaaS.Tests
{
    /// <summary>
    /// Tests for SelectFilter JSON configurations with CreateGroupedDictionaryFromAppState
    /// </summary>
    public class SelectFilterJsonTests
    {
        // Test component
        public class TestSelectFilterComponent : GenericTestComponent<object, object, object>
        {
            public TestSelectFilterComponent()
            {
                // Initialize AppState for testing
                AppState = new CalibrationSaaS.Domain.Aggregates.Shared.Basic.AppState();
            }

            public Dictionary<string, Dictionary<string, string>> TestProcessSelectFilter(string selectFilterJson)
            {
                return ProcessSelectFilter(selectFilterJson);
            }

            public Dictionary<string, Dictionary<string, string>> ProcessSelectFilter(string selectFilterJson)
            {
                try
                {
                    var selectFilter = JsonConvert.DeserializeObject<SelectFilter>(selectFilterJson);

                    // Basic validation
                    if (string.IsNullOrEmpty(selectFilter.Method))
                        throw new ArgumentException("Method cannot be empty");
                    if (string.IsNullOrEmpty(selectFilter.Key))
                        throw new ArgumentException("Key cannot be empty");
                    if (string.IsNullOrEmpty(selectFilter.Value))
                        throw new ArgumentException("Value cannot be empty");
                    if (string.IsNullOrEmpty(selectFilter.Group))
                        throw new ArgumentException("Group cannot be empty");

                    // Parse parameters if needed
                    object[] methodParameters = null;
                    if (!string.IsNullOrEmpty(selectFilter.Filter))
                    {
                        methodParameters = ParseFilterParameters(selectFilter.Filter, selectFilter.Method);
                    }

                    return CreateGroupedDictionaryFromAppState(
                        appStateMethodName: selectFilter.Method,
                        keyPropertyName: selectFilter.Key,
                        valuePropertyName: selectFilter.Value,
                        groupingPropertyName: selectFilter.Group,
                        methodParameters: methodParameters
                    );
                }
                catch (Exception ex)
                {
                    //Console.WriteLine($"Error processing SelectFilter: {ex.Message}");
                    return new Dictionary<string, Dictionary<string, string>>();
                }
            }

            private object[] ParseFilterParameters(string filter, string methodName)
            {
                if (string.IsNullOrEmpty(filter)) return null;

                try
                {
                    switch (methodName)
                    {
                        case "GetUoMByTypes":
                            var intArray = JsonConvert.DeserializeObject<int[]>(filter);
                            return new object[] { intArray };

                        default:
                            // Try to parse as generic object
                            var genericObject = JsonConvert.DeserializeObject(filter);
                            return new object[] { genericObject };
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine($"Error parsing filter parameters: {ex.Message}");
                    return null;
                }
            }
        }

        public static void RunTests()
        {
            var testComponent = new TestSelectFilterComponent();

            //Console.WriteLine("Running SelectFilter JSON Tests...");
            //Console.WriteLine("==================================");

            // Test 1: Valid basic JSON
            Test1_ValidBasicJson(testComponent);

            // Test 2: Valid JSON with array parameters
            Test2_ValidJsonWithArrayParameters(testComponent);

            // Test 3: Invalid JSON structure
            Test3_InvalidJsonStructure(testComponent);

            // Test 4: Missing required properties
            Test4_MissingRequiredProperties(testComponent);

            // Test 5: JSON deserialization
            Test5_JsonDeserialization();

            //Console.WriteLine("\nAll SelectFilter JSON tests completed!");
        }

        private static void Test1_ValidBasicJson(TestSelectFilterComponent testComponent)
        {
            //Console.WriteLine("\nTest 1: Valid Basic JSON");
            //Console.WriteLine("------------------------");

            string validJson = @"{
                ""Filter"": """",
                ""Key"": ""UnitOfMeasureID"",
                ""Value"": ""Name"",
                ""Group"": ""TypeID"",
                ""Method"": ""GetAllUnitOfMeasure""
            }";

            try
            {
                var selectFilter = JsonConvert.DeserializeObject<SelectFilter>(validJson);
                
                if (selectFilter != null &&
                    selectFilter.Method == "GetAllUnitOfMeasure" &&
                    selectFilter.Key == "UnitOfMeasureID" &&
                    selectFilter.Value == "Name" &&
                    selectFilter.Group == "TypeID" &&
                    selectFilter.Filter == "")
                {
                    //Console.WriteLine("✓ PASSED: Valid basic JSON deserialized correctly");
                }
                else
                {
                    //Console.WriteLine("✗ FAILED: JSON deserialization incorrect");
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"✗ FAILED: Exception during deserialization: {ex.Message}");
            }
        }

        private static void Test2_ValidJsonWithArrayParameters(TestSelectFilterComponent testComponent)
        {
            //Console.WriteLine("\nTest 2: Valid JSON with Array Parameters");
            //Console.WriteLine("----------------------------------------");

            string validJsonWithParams = @"{
                ""Filter"": ""[1,2,3]"",
                ""Key"": ""UnitOfMeasureID"",
                ""Value"": ""Abbreviation"",
                ""Group"": ""TypeID"",
                ""Method"": ""GetUoMByTypes""
            }";

            try
            {
                var selectFilter = JsonConvert.DeserializeObject<SelectFilter>(validJsonWithParams);
                
                if (selectFilter != null &&
                    selectFilter.Method == "GetUoMByTypes" &&
                    selectFilter.Filter == "[1,2,3]")
                {
                    // Test parameter parsing
                    var parameters = JsonConvert.DeserializeObject<int[]>(selectFilter.Filter);
                    if (parameters.Length == 3 && parameters[0] == 1 && parameters[1] == 2 && parameters[2] == 3)
                    {
                        //Console.WriteLine("✓ PASSED: JSON with array parameters parsed correctly");
                    }
                    else
                    {
                        //Console.WriteLine("✗ FAILED: Array parameters not parsed correctly");
                    }
                }
                else
                {
                    //Console.WriteLine("✗ FAILED: JSON with parameters deserialization incorrect");
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"✗ FAILED: Exception during parameter parsing: {ex.Message}");
            }
        }

        private static void Test3_InvalidJsonStructure(TestSelectFilterComponent testComponent)
        {
            //Console.WriteLine("\nTest 3: Invalid JSON Structure");
            //Console.WriteLine("------------------------------");

            string invalidJson = @"{
                ""Filter"": """",
                ""Key"": ""UnitOfMeasureID"",
                ""Value"": ""Name"",
                // Missing closing brace and Method property
            ";

            try
            {
                var selectFilter = JsonConvert.DeserializeObject<SelectFilter>(invalidJson);
                //Console.WriteLine("✗ FAILED: Should have thrown exception for invalid JSON");
            }
            catch (JsonException)
            {
                //Console.WriteLine("✓ PASSED: Correctly threw exception for invalid JSON structure");
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"✗ FAILED: Unexpected exception type: {ex.GetType().Name}");
            }
        }

        private static void Test4_MissingRequiredProperties(TestSelectFilterComponent testComponent)
        {
            //Console.WriteLine("\nTest 4: Missing Required Properties");
            //Console.WriteLine("-----------------------------------");

            string jsonMissingMethod = @"{
                ""Filter"": """",
                ""Key"": ""UnitOfMeasureID"",
                ""Value"": ""Name"",
                ""Group"": ""TypeID""
            }";

            try
            {
                var selectFilter = JsonConvert.DeserializeObject<SelectFilter>(jsonMissingMethod);
                
                if (string.IsNullOrEmpty(selectFilter.Method))
                {
                    //Console.WriteLine("✓ PASSED: Missing Method property detected");
                }
                else
                {
                    //Console.WriteLine("✗ FAILED: Missing Method property not detected");
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"✗ FAILED: Unexpected exception: {ex.Message}");
            }
        }

        private static void Test5_JsonDeserialization()
        {
            //Console.WriteLine("\nTest 5: JSON Serialization/Deserialization Round Trip");
            //Console.WriteLine("-----------------------------------------------------");

            var originalSelectFilter = new SelectFilter
            {
                Filter = "[1,2,3]",
                Key = "UnitOfMeasureID",
                Value = "Name",
                Group = "TypeID",
                Method = "GetUoMByTypes"
            };

            try
            {
                // Serialize to JSON
                string json = JsonConvert.SerializeObject(originalSelectFilter, Formatting.Indented);
                //Console.WriteLine($"Serialized JSON:\n{json}");

                // Deserialize back
                var deserializedSelectFilter = JsonConvert.DeserializeObject<SelectFilter>(json);

                // Compare
                if (deserializedSelectFilter.Filter == originalSelectFilter.Filter &&
                    deserializedSelectFilter.Key == originalSelectFilter.Key &&
                    deserializedSelectFilter.Value == originalSelectFilter.Value &&
                    deserializedSelectFilter.Group == originalSelectFilter.Group &&
                    deserializedSelectFilter.Method == originalSelectFilter.Method)
                {
                    //Console.WriteLine("✓ PASSED: Round trip serialization/deserialization successful");
                }
                else
                {
                    //Console.WriteLine("✗ FAILED: Round trip serialization/deserialization failed");
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"✗ FAILED: Exception during round trip: {ex.Message}");
            }
        }
    }
}
