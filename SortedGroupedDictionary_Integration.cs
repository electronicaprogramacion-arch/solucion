using System;
using System.Collections.Generic;
using CalibrationSaaS.Infraestructure.Blazor.Shared;
using CalibrationSaaS.Utilities;
using Newtonsoft.Json;
using Helpers.Models;

namespace CalibrationSaaS.Integration
{
    /// <summary>
    /// Integration example showing how to use dictionary sorting with CreateGroupedDictionaryFromAppState
    /// </summary>
    public class SortedGroupedDictionaryIntegration
    {
        // Example component that extends GenericTestComponent with sorting capabilities
        public class SortedGenericTestComponent : GenericTestComponent<object, object, object>
        {
            /// <summary>
            /// Enhanced version of CreateGroupedDictionaryFromAppState that returns sorted results
            /// </summary>
            /// <param name="appStateMethodName">AppState method name</param>
            /// <param name="keyPropertyName">Property for keys</param>
            /// <param name="valuePropertyName">Property for values</param>
            /// <param name="groupingPropertyName">Property for grouping</param>
            /// <param name="sortInnerDictionaries">Whether to sort inner dictionaries by values</param>
            /// <param name="sortOuterDictionary">Whether to sort outer dictionary by keys</param>
            /// <param name="ignoreCase">Whether to ignore case when sorting</param>
            /// <param name="methodParameters">Optional method parameters</param>
            /// <returns>Sorted grouped dictionary</returns>
            public Dictionary<string, Dictionary<string, string>> CreateSortedGroupedDictionaryFromAppState(
                string appStateMethodName,
                string keyPropertyName,
                string valuePropertyName,
                string groupingPropertyName,
                bool sortInnerDictionaries = true,
                bool sortOuterDictionary = false,
                bool ignoreCase = true,
                params object[] methodParameters)
            {
                // Get the original grouped dictionary
                var originalResult = CreateGroupedDictionaryFromAppState(
                    appStateMethodName,
                    keyPropertyName,
                    valuePropertyName,
                    groupingPropertyName,
                    methodParameters
                );

                // Apply sorting based on parameters
                if (sortInnerDictionaries && sortOuterDictionary)
                {
                    return DictionarySortingUtilities.SortGroupedDictionaryCompletely(originalResult, ignoreCase);
                }
                else if (sortInnerDictionaries)
                {
                    return DictionarySortingUtilities.SortGroupedDictionaryByValues(originalResult, ignoreCase);
                }
                else if (sortOuterDictionary)
                {
                    // Sort only outer dictionary by keys
                    var comparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
                    var result = new Dictionary<string, Dictionary<string, string>>();
                    
                    foreach (var group in originalResult.OrderBy(kvp => kvp.Key, comparer))
                    {
                        result[group.Key] = group.Value;
                    }
                    
                    return result;
                }

                return originalResult;
            }

            /// <summary>
            /// Enhanced SelectFilter processing with sorting options
            /// </summary>
            /// <param name="selectFilterJson">SelectFilter JSON configuration</param>
            /// <param name="sortOptions">Sorting options</param>
            /// <returns>Sorted grouped dictionary</returns>
            public Dictionary<string, Dictionary<string, string>> ProcessSelectFilterWithSorting(
                string selectFilterJson,
                SortingOptions sortOptions = null)
            {
                sortOptions = sortOptions ?? new SortingOptions();

                try
                {
                    var selectFilter = JsonConvert.DeserializeObject<SelectFilter>(selectFilterJson);

                    // Parse parameters if needed
                    object[] methodParameters = null;
                    if (!string.IsNullOrEmpty(selectFilter.Filter))
                    {
                        methodParameters = ParseFilterParameters(selectFilter.Filter, selectFilter.Method);
                    }

                    return CreateSortedGroupedDictionaryFromAppState(
                        appStateMethodName: selectFilter.Method,
                        keyPropertyName: selectFilter.Key,
                        valuePropertyName: selectFilter.Value,
                        groupingPropertyName: selectFilter.Group,
                        sortInnerDictionaries: sortOptions.SortInnerDictionaries,
                        sortOuterDictionary: sortOptions.SortOuterDictionary,
                        ignoreCase: sortOptions.IgnoreCase,
                        methodParameters: methodParameters
                    );
                }
                catch (Exception ex)
                {
//                    Console.WriteLine($"Error processing SelectFilter with sorting: {ex.Message}");
                    return new Dictionary<string, Dictionary<string, string>>();
                }
            }

            /// <summary>
            /// Helper method to parse filter parameters
            /// </summary>
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
            /// Real-world example: Get sorted units of measure for UI dropdowns
            /// </summary>
            /// <returns>Sorted units grouped by type</returns>
            public Dictionary<string, Dictionary<string, string>> GetSortedUnitsForUI()
            {
                return CreateSortedGroupedDictionaryFromAppState(
                    appStateMethodName: "GetAllUnitOfMeasure",
                    keyPropertyName: "UnitOfMeasureID",
                    valuePropertyName: "Name",
                    groupingPropertyName: "TypeID",
                    sortInnerDictionaries: true,
                    sortOuterDictionary: true,
                    ignoreCase: true
                );
            }

            /// <summary>
            /// Get sorted units for specific calibration types
            /// </summary>
            /// <param name="calibrationTypes">Array of calibration type IDs</param>
            /// <returns>Sorted units for specified types</returns>
            public Dictionary<string, Dictionary<string, string>> GetSortedUnitsByTypes(int[] calibrationTypes)
            {
                return CreateSortedGroupedDictionaryFromAppState(
                    appStateMethodName: "GetUoMByTypes",
                    keyPropertyName: "UnitOfMeasureID",
                    valuePropertyName: "Abbreviation",
                    groupingPropertyName: "TypeID",
                    sortInnerDictionaries: true,
                    sortOuterDictionary: true,
                    ignoreCase: true,
                    methodParameters: calibrationTypes
                );
            }
        }

        /// <summary>
        /// Configuration class for sorting options
        /// </summary>
        public class SortingOptions
        {
            public bool SortInnerDictionaries { get; set; } = true;
            public bool SortOuterDictionary { get; set; } = false;
            public bool IgnoreCase { get; set; } = true;
        }

        /// <summary>
        /// Enhanced SelectFilter with sorting configuration
        /// </summary>
        public class SelectFilterWithSorting : SelectFilter
        {
            public SortingOptions SortingOptions { get; set; } = new SortingOptions();
        }

        /// <summary>
        /// Demonstration of the integration
        /// </summary>
        public static void RunIntegrationExamples()
        {
//            Console.WriteLine("Sorted Grouped Dictionary Integration Examples");
//            Console.WriteLine("==============================================");

            var component = new SortedGenericTestComponent();

            // Example 1: Basic sorted units
            Example1_BasicSortedUnits(component);

            // Example 2: SelectFilter with sorting
            Example2_SelectFilterWithSorting(component);

            // Example 3: Different sorting configurations
            Example3_DifferentSortingConfigurations(component);

            // Example 4: JSON configuration with sorting
            Example4_JsonConfigurationWithSorting(component);

//            Console.WriteLine("\nAll integration examples completed!");
        }

        private static void Example1_BasicSortedUnits(SortedGenericTestComponent component)
        {
//            Console.WriteLine("\nExample 1: Basic Sorted Units");
//            Console.WriteLine("=============================");

            try
            {
                var sortedUnits = component.GetSortedUnitsForUI();
                
//                Console.WriteLine("Sorted Units of Measure for UI:");
                PrintGroupedDictionary(sortedUnits);
            }
            catch (Exception ex)
            {
//                Console.WriteLine($"Note: {ex.Message} (Expected in test environment)");
            }
        }

        private static void Example2_SelectFilterWithSorting(SortedGenericTestComponent component)
        {
//            Console.WriteLine("\nExample 2: SelectFilter with Sorting");
//            Console.WriteLine("====================================");

            string selectFilterJson = @"{
                ""Filter"": """",
                ""Key"": ""UnitOfMeasureID"",
                ""Value"": ""Name"",
                ""Group"": ""TypeID"",
                ""Method"": ""GetAllUnitOfMeasure""
            }";

            var sortingOptions = new SortingOptions
            {
                SortInnerDictionaries = true,
                SortOuterDictionary = true,
                IgnoreCase = true
            };

            try
            {
                var result = component.ProcessSelectFilterWithSorting(selectFilterJson, sortingOptions);
//                Console.WriteLine("SelectFilter result with sorting applied:");
                PrintGroupedDictionary(result);
            }
            catch (Exception ex)
            {
//                Console.WriteLine($"Note: {ex.Message} (Expected in test environment)");
            }
        }

        private static void Example3_DifferentSortingConfigurations(SortedGenericTestComponent component)
        {
//            Console.WriteLine("\nExample 3: Different Sorting Configurations");
//            Console.WriteLine("===========================================");

            // Simulate some test data since AppState might not be available
            var testData = new Dictionary<string, Dictionary<string, string>>
            {
                {
                    "2", new Dictionary<string, string>
                    {
                        { "201", "Kilogram" },
                        { "202", "Gram" },
                        { "203", "Pound" }
                    }
                },
                {
                    "1", new Dictionary<string, string>
                    {
                        { "101", "Meter" },
                        { "102", "Centimeter" },
                        { "103", "Millimeter" }
                    }
                }
            };

//            Console.WriteLine("Original test data:");
            PrintGroupedDictionary(testData);

            // Sort only inner dictionaries
            var sortedInner = DictionarySortingUtilities.SortGroupedDictionaryByValues(testData, ignoreCase: true);
//            Console.WriteLine("\nSorted inner dictionaries only:");
            PrintGroupedDictionary(sortedInner);

            // Sort completely
            var sortedComplete = DictionarySortingUtilities.SortGroupedDictionaryCompletely(testData, ignoreCase: true);
//            Console.WriteLine("\nSorted completely (outer by keys, inner by values):");
            PrintGroupedDictionary(sortedComplete);
        }

        private static void Example4_JsonConfigurationWithSorting(SortedGenericTestComponent component)
        {
//            Console.WriteLine("\nExample 4: JSON Configuration with Sorting");
//            Console.WriteLine("==========================================");

            // Example of extended JSON configuration that could include sorting options
            var extendedConfig = new
            {
                SelectFilter = new
                {
                    Filter = "",
                    Key = "UnitOfMeasureID",
                    Value = "Name",
                    Group = "TypeID",
                    Method = "GetAllUnitOfMeasure"
                },
                SortingOptions = new
                {
                    SortInnerDictionaries = true,
                    SortOuterDictionary = true,
                    IgnoreCase = true
                }
            };

            string configJson = JsonConvert.SerializeObject(extendedConfig, Formatting.Indented);
//            Console.WriteLine("Extended JSON configuration with sorting:");
//            Console.WriteLine(configJson);

            // This could be used to configure both data retrieval and sorting in one JSON object
        }

        // Helper method for displaying results
        private static void PrintGroupedDictionary(Dictionary<string, Dictionary<string, string>> groupedDict)
        {
            if (groupedDict == null || groupedDict.Count == 0)
            {
//                Console.WriteLine("  (No data)");
                return;
            }

            foreach (var group in groupedDict)
            {
//                Console.WriteLine($"  Group {group.Key}:");
                foreach (var item in group.Value)
                {
//                    Console.WriteLine($"    {item.Key}: {item.Value}");
                }
            }
        }
    }
}
