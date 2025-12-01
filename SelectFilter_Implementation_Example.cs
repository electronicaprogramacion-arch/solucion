using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using CalibrationSaaS.Infraestructure.Blazor.Shared;
using Helpers.Models;
using CalibrationSaaS.Domain.Aggregates.Entities;

namespace CalibrationSaaS.Examples
{
    /// <summary>
    /// Demonstrates how to use SelectFilter JSON configurations with CreateGroupedDictionaryFromAppState
    /// </summary>
    public class SelectFilterImplementationExample
    {
        // Example component that inherits from GenericTestComponent
        public class SelectFilterComponent : GenericTestComponent<object, object, object>
        {
            /// <summary>
            /// Example 1: Basic usage without parameters
            /// </summary>
            public void UseBasicSelectFilter()
            {
                // JSON configuration for basic units of measure grouping
                string selectFilterJson = @"{
                    ""Filter"": """",
                    ""Key"": ""UnitOfMeasureID"",
                    ""Value"": ""Name"",
                    ""Group"": ""TypeID"",
                    ""Method"": ""GetAllUnitOfMeasure""
                }";

                try
                {
                    // Deserialize the SelectFilter configuration
                    var selectFilter = JsonConvert.DeserializeObject<SelectFilter>(selectFilterJson);

                    // Call the method using the configuration
                    var result = CreateGroupedDictionaryFromAppState(
                        appStateMethodName: selectFilter.Method,
                        keyPropertyName: selectFilter.Key,
                        valuePropertyName: selectFilter.Value,
                        groupingPropertyName: selectFilter.Group
                    );

                    // Display results
//                    Console.WriteLine("Basic SelectFilter Results:");
//                    Console.WriteLine("==========================");
                    DisplayResults(result);
                }
                catch (Exception ex)
                {
//                    Console.WriteLine($"Error in basic SelectFilter: {ex.Message}");
                }
            }

            /// <summary>
            /// Example 2: Using SelectFilter with array parameters
            /// </summary>
            public void UseSelectFilterWithArrayParameters()
            {
                // JSON configuration with array parameters in Filter
                string selectFilterJson = @"{
                    ""Filter"": ""[1,2,3]"",
                    ""Key"": ""UnitOfMeasureID"",
                    ""Value"": ""Abbreviation"",
                    ""Group"": ""TypeID"",
                    ""Method"": ""GetUoMByTypes""
                }";

                try
                {
                    var selectFilter = JsonConvert.DeserializeObject<SelectFilter>(selectFilterJson);

                    // Parse the Filter property to get method parameters
                    object[] methodParameters = null;
                    if (!string.IsNullOrEmpty(selectFilter.Filter))
                    {
                        // Parse array parameters from Filter
                        var calibrationTypes = JsonConvert.DeserializeObject<int[]>(selectFilter.Filter);
                        methodParameters = new object[] { calibrationTypes };
                    }

                    var result = CreateGroupedDictionaryFromAppState(
                        appStateMethodName: selectFilter.Method,
                        keyPropertyName: selectFilter.Key,
                        valuePropertyName: selectFilter.Value,
                        groupingPropertyName: selectFilter.Group,
                        methodParameters: methodParameters
                    );

//                    Console.WriteLine("\nSelectFilter with Array Parameters:");
//                    Console.WriteLine("===================================");
                    DisplayResults(result);
                }
                catch (Exception ex)
                {
//                    Console.WriteLine($"Error in SelectFilter with parameters: {ex.Message}");
                }
            }

            /// <summary>
            /// Example 3: Using SelectFilter with object parameters
            /// </summary>
            public void UseSelectFilterWithObjectParameters()
            {
                // JSON configuration with object parameters in Filter
                string selectFilterJson = @"{
                    ""Filter"": ""{\"\"EquipmentTypeID\"\":5,\"\"Name\"\":\"\"Pressure Gauge\"\"}"",
                    ""Key"": ""UnitOfMeasureID"",
                    ""Value"": ""Name"",
                    ""Group"": ""TypeID"",
                    ""Method"": ""GetUoMByEquipmentType""
                }";

                try
                {
                    var selectFilter = JsonConvert.DeserializeObject<SelectFilter>(selectFilterJson);

                    // Parse the Filter property to get object parameters
                    object[] methodParameters = null;
                    if (!string.IsNullOrEmpty(selectFilter.Filter))
                    {
                        // Parse object parameters from Filter
                        var equipmentType = JsonConvert.DeserializeObject<EquipmentType>(selectFilter.Filter);
                        methodParameters = new object[] { equipmentType };
                    }

                    var result = CreateGroupedDictionaryFromAppState(
                        appStateMethodName: selectFilter.Method,
                        keyPropertyName: selectFilter.Key,
                        valuePropertyName: selectFilter.Value,
                        groupingPropertyName: selectFilter.Group,
                        methodParameters: methodParameters
                    );

//                    Console.WriteLine("\nSelectFilter with Object Parameters:");
//                    Console.WriteLine("====================================");
                    DisplayResults(result);
                }
                catch (Exception ex)
                {
//                    Console.WriteLine($"Error in SelectFilter with object parameters: {ex.Message}");
                }
            }

            /// <summary>
            /// Example 4: Generic method to process any SelectFilter JSON
            /// </summary>
            public Dictionary<string, Dictionary<string, string>> ProcessSelectFilter(string selectFilterJson)
            {
                try
                {
                    var selectFilter = JsonConvert.DeserializeObject<SelectFilter>(selectFilterJson);

                    // Determine if method parameters are needed
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
//                    Console.WriteLine($"Error processing SelectFilter: {ex.Message}");
                    return new Dictionary<string, Dictionary<string, string>>();
                }
            }

            /// <summary>
            /// Helper method to parse Filter parameters based on method type
            /// </summary>
            private object[] ParseFilterParameters(string filter, string methodName)
            {
                if (string.IsNullOrEmpty(filter)) return null;

                try
                {
                    switch (methodName)
                    {
                        case "GetUoMByTypes":
                            // Expecting int array
                            var intArray = JsonConvert.DeserializeObject<int[]>(filter);
                            return new object[] { intArray };

                        case "GetUoMByEquipmentType":
                            // Expecting EquipmentType object
                            var equipmentType = JsonConvert.DeserializeObject<EquipmentType>(filter);
                            return new object[] { equipmentType };

                        case "GetUoMByCalibrationTypeObj":
                            // Expecting CalibrationType object
                            var calibrationType = JsonConvert.DeserializeObject<CalibrationType>(filter);
                            return new object[] { calibrationType };

                        default:
                            // Try to parse as generic object
                            var genericObject = JsonConvert.DeserializeObject(filter);
                            return new object[] { genericObject };
                    }
                }
                catch (Exception ex)
                {
//                    Console.WriteLine($"Error parsing filter parameters: {ex.Message}");
                    return null;
                }
            }

            /// <summary>
            /// Helper method to display results in a readable format
            /// </summary>
            private void DisplayResults(Dictionary<string, Dictionary<string, string>> results)
            {
                if (results == null || results.Count == 0)
                {
//                    Console.WriteLine("No results found.");
                    return;
                }

                foreach (var group in results)
                {
//                    Console.WriteLine($"\nGroup: {group.Key}");
                    foreach (var item in group.Value)
                    {
//                        Console.WriteLine($"  {item.Key}: {item.Value}");
                    }
                }
            }

            /// <summary>
            /// Example 5: Batch processing multiple SelectFilter configurations
            /// </summary>
            public void ProcessMultipleSelectFilters()
            {
                var configurations = new[]
                {
                    @"{""Filter"":"""",""Key"":""UnitOfMeasureID"",""Value"":""Name"",""Group"":""TypeID"",""Method"":""GetAllUnitOfMeasure""}",
                    @"{""Filter"":""[1,2]"",""Key"":""UnitOfMeasureID"",""Value"":""Abbreviation"",""Group"":""TypeID"",""Method"":""GetUoMByTypes""}",
                    @"{""Filter"":"""",""Key"":""TypeID"",""Value"":""UnitOfMeasureID"",""Group"":""Name"",""Method"":""GetAllUnitOfMeasure""}"
                };

//                Console.WriteLine("\nBatch Processing Multiple SelectFilters:");
//                Console.WriteLine("========================================");

                for (int i = 0; i < configurations.Length; i++)
                {
//                    Console.WriteLine($"\nConfiguration {i + 1}:");
                    var result = ProcessSelectFilter(configurations[i]);
                    DisplayResults(result);
                }
            }

            /// <summary>
            /// Example 6: Real-world usage - mimicking the existing implementation
            /// </summary>
            public void RealWorldUsage(string selectOptionsJson)
            {
                // This mimics how it's used in GenericTestComponentBase.cs line 764
                try
                {
                    var sf = JsonConvert.DeserializeObject<SelectFilter>(selectOptionsJson);

                    var result = CreateGroupedDictionaryFromAppState(
                        appStateMethodName: sf.Method,
                        keyPropertyName: sf.Key,
                        valuePropertyName: sf.Value,
                        groupingPropertyName: sf.Group
                    );

                    // Serialize result back to JSON (as done in the original code)
                    var optionsJson = JsonConvert.SerializeObject(result);

//                    Console.WriteLine("\nReal-world Usage Result:");
//                    Console.WriteLine("========================");
//                    Console.WriteLine($"Serialized Options: {optionsJson}");
                }
                catch (Exception ex)
                {
//                    Console.WriteLine($"Error in real-world usage: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Static method to run all examples
        /// </summary>
        public static void RunAllExamples()
        {
            var component = new SelectFilterComponent();

//            Console.WriteLine("SelectFilter Implementation Examples");
//            Console.WriteLine("===================================");

            component.UseBasicSelectFilter();
            component.UseSelectFilterWithArrayParameters();
            component.UseSelectFilterWithObjectParameters();
            component.ProcessMultipleSelectFilters();

            // Example of real-world usage
            string realWorldJson = @"{""Filter"":"""",""Key"":""UnitOfMeasureID"",""Value"":""Name"",""Group"":""TypeID"",""Method"":""GetAllUnitOfMeasure""}";
            component.RealWorldUsage(realWorldJson);

//            Console.WriteLine("\nAll SelectFilter examples completed!");
        }
    }
}
