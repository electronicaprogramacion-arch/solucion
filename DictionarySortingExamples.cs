using System;
using System.Collections.Generic;
using System.Linq;
using CalibrationSaaS.Utilities;

namespace CalibrationSaaS.Examples
{
    /// <summary>
    /// Examples and tests for dictionary sorting utilities
    /// </summary>
    public class DictionarySortingExamples
    {
        /// <summary>
        /// Demonstrates all dictionary sorting methods with sample data
        /// </summary>
        public static void RunAllExamples()
        {
//            Console.WriteLine("Dictionary Sorting Examples");
//            Console.WriteLine("===========================");

            // Create sample data
            var sampleDictionary = CreateSampleDictionary();
            var groupedDictionary = CreateSampleGroupedDictionary();

            // Example 1: Basic sorting by values
            Example1_BasicSorting(sampleDictionary);

            // Example 2: Case-insensitive sorting
            Example2_CaseInsensitiveSorting(sampleDictionary);

            // Example 3: Descending order sorting
            Example3_DescendingSorting(sampleDictionary);

            // Example 4: Different return types
            Example4_DifferentReturnTypes(sampleDictionary);

            // Example 5: Sorting grouped dictionaries
            Example5_GroupedDictionarySorting(groupedDictionary);

            // Example 6: Real-world CalibrationSaaS scenario
            Example6_CalibrationSaaSScenario();

//            Console.WriteLine("\nAll examples completed!");
        }

        private static Dictionary<string, string> CreateSampleDictionary()
        {
            return new Dictionary<string, string>
            {
                { "101", "Zebra" },
                { "102", "apple" },
                { "103", "Banana" },
                { "104", "cherry" },
                { "105", "Apple" },
                { "106", "banana" }
            };
        }

        private static Dictionary<string, Dictionary<string, string>> CreateSampleGroupedDictionary()
        {
            return new Dictionary<string, Dictionary<string, string>>
            {
                {
                    "TypeA", new Dictionary<string, string>
                    {
                        { "1", "Zebra" },
                        { "2", "Apple" },
                        { "3", "banana" }
                    }
                },
                {
                    "TypeB", new Dictionary<string, string>
                    {
                        { "4", "cherry" },
                        { "5", "Date" },
                        { "6", "apple" }
                    }
                }
            };
        }

        private static void Example1_BasicSorting(Dictionary<string, string> dictionary)
        {
//            Console.WriteLine("\nExample 1: Basic Sorting by Values (Case-Sensitive)");
//            Console.WriteLine("===================================================");

//            Console.WriteLine("Original Dictionary:");
            PrintDictionary(dictionary);

            var sortedList = DictionarySortingUtilities.SortByValues(dictionary);

//            Console.WriteLine("\nSorted by Values (Ascending, Case-Sensitive):");
            PrintKeyValuePairs(sortedList);
        }

        private static void Example2_CaseInsensitiveSorting(Dictionary<string, string> dictionary)
        {
//            Console.WriteLine("\nExample 2: Case-Insensitive Sorting");
//            Console.WriteLine("===================================");

            var sortedList = DictionarySortingUtilities.SortByValuesIgnoreCase(dictionary);

//            Console.WriteLine("Sorted by Values (Ascending, Case-Insensitive):");
            PrintKeyValuePairs(sortedList);
        }

        private static void Example3_DescendingSorting(Dictionary<string, string> dictionary)
        {
//            Console.WriteLine("\nExample 3: Descending Order Sorting");
//            Console.WriteLine("===================================");

            var sortedDescending = DictionarySortingUtilities.SortByValuesDescending(dictionary, ignoreCase: true);

//            Console.WriteLine("Sorted by Values (Descending, Case-Insensitive):");
            PrintKeyValuePairs(sortedDescending);
        }

        private static void Example4_DifferentReturnTypes(Dictionary<string, string> dictionary)
        {
//            Console.WriteLine("\nExample 4: Different Return Types");
//            Console.WriteLine("=================================");

            // As IEnumerable
//            Console.WriteLine("As IEnumerable<KeyValuePair<string, string>>:");
            var enumerable = DictionarySortingUtilities.SortByValuesAsEnumerable(dictionary, ignoreCase: true);
            foreach (var kvp in enumerable.Take(3)) // Take first 3 for demo
            {
//                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            // As new Dictionary
//            Console.WriteLine("\nAs new Dictionary<string, string>:");
            var newDict = DictionarySortingUtilities.SortByValuesAsDictionary(dictionary, ignoreCase: true);
            PrintDictionary(newDict);

            // Sorted by values then by keys
//            Console.WriteLine("\nSorted by Values then by Keys:");
            var sortedByValuesThenKeys = DictionarySortingUtilities.SortByValuesThenByKeys(dictionary, ignoreCase: true);
            PrintDictionary(sortedByValuesThenKeys);
        }

        private static void Example5_GroupedDictionarySorting(Dictionary<string, Dictionary<string, string>> groupedDict)
        {
//            Console.WriteLine("\nExample 5: Grouped Dictionary Sorting");
//            Console.WriteLine("=====================================");

//            Console.WriteLine("Original Grouped Dictionary:");
            PrintGroupedDictionary(groupedDict);

            // Sort only inner dictionaries
            var sortedInner = DictionarySortingUtilities.SortGroupedDictionaryByValues(groupedDict, ignoreCase: true);
//            Console.WriteLine("\nWith Inner Dictionaries Sorted by Values:");
            PrintGroupedDictionary(sortedInner);

            // Sort both outer and inner dictionaries
            var sortedComplete = DictionarySortingUtilities.SortGroupedDictionaryCompletely(groupedDict, ignoreCase: true);
//            Console.WriteLine("\nCompletely Sorted (Outer by Keys, Inner by Values):");
            PrintGroupedDictionary(sortedComplete);
        }

        private static void Example6_CalibrationSaaSScenario()
        {
//            Console.WriteLine("\nExample 6: Real CalibrationSaaS Scenario");
//            Console.WriteLine("========================================");

            // Simulate data from CreateGroupedDictionaryFromAppState
            var unitsOfMeasure = new Dictionary<string, Dictionary<string, string>>
            {
                {
                    "1", new Dictionary<string, string> // Length units
                    {
                        { "101", "Meter" },
                        { "102", "Centimeter" },
                        { "103", "Millimeter" },
                        { "104", "Inch" },
                        { "105", "Foot" }
                    }
                },
                {
                    "2", new Dictionary<string, string> // Weight units
                    {
                        { "201", "Kilogram" },
                        { "202", "Gram" },
                        { "203", "Pound" },
                        { "204", "Ounce" }
                    }
                },
                {
                    "3", new Dictionary<string, string> // Temperature units
                    {
                        { "301", "Kelvin" },
                        { "302", "Celsius" },
                        { "303", "Fahrenheit" }
                    }
                }
            };

//            Console.WriteLine("Units of Measure (Original):");
            PrintGroupedDictionary(unitsOfMeasure);

            // Sort for better UI presentation
            var sortedForUI = DictionarySortingUtilities.SortGroupedDictionaryByValues(unitsOfMeasure, ignoreCase: true);

//            Console.WriteLine("\nUnits of Measure (Sorted for UI):");
            PrintGroupedDictionary(sortedForUI);

            // Example of sorting a single inner dictionary
//            Console.WriteLine("\nSorting Single Type (Length Units Only):");
            if (unitsOfMeasure.ContainsKey("1"))
            {
                var lengthUnits = unitsOfMeasure["1"];
                var sortedLengthUnits = DictionarySortingUtilities.SortByValuesIgnoreCase(lengthUnits);
                PrintKeyValuePairs(sortedLengthUnits);
            }
        }

        // Helper methods for displaying results
        private static void PrintDictionary(Dictionary<string, string> dictionary)
        {
            foreach (var kvp in dictionary)
            {
//                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
        }

        private static void PrintKeyValuePairs(IEnumerable<KeyValuePair<string, string>> pairs)
        {
            foreach (var kvp in pairs)
            {
//                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
        }

        private static void PrintGroupedDictionary(Dictionary<string, Dictionary<string, string>> groupedDict)
        {
            foreach (var group in groupedDict)
            {
//                Console.WriteLine($"  Group {group.Key}:");
                foreach (var item in group.Value)
                {
//                    Console.WriteLine($"    {item.Key}: {item.Value}");
                }
            }
        }

        /// <summary>
        /// Extension method example for easy integration with existing code
        /// </summary>
        public static class DictionaryExtensions
        {
            /// <summary>
            /// Extension method to sort a Dictionary<string, string> by values
            /// </summary>
            public static List<KeyValuePair<string, string>> SortByValues(this Dictionary<string, string> dictionary, bool ignoreCase = false)
            {
                return ignoreCase 
                    ? DictionarySortingUtilities.SortByValuesIgnoreCase(dictionary)
                    : DictionarySortingUtilities.SortByValues(dictionary);
            }

            /// <summary>
            /// Extension method to sort grouped dictionaries by values
            /// </summary>
            public static Dictionary<string, Dictionary<string, string>> SortInnerDictionariesByValues(
                this Dictionary<string, Dictionary<string, string>> groupedDictionary, 
                bool ignoreCase = false)
            {
                return DictionarySortingUtilities.SortGroupedDictionaryByValues(groupedDictionary, ignoreCase);
            }
        }

        /// <summary>
        /// Performance test for large dictionaries
        /// </summary>
        public static void PerformanceTest()
        {
//            Console.WriteLine("\nPerformance Test");
//            Console.WriteLine("================");

            // Create large dictionary
            var largeDictionary = new Dictionary<string, string>();
            var random = new Random();
            
            for (int i = 0; i < 10000; i++)
            {
                largeDictionary[$"Key{i}"] = $"Value{random.Next(1000)}";
            }

            var startTime = DateTime.Now;
            var sorted = DictionarySortingUtilities.SortByValues(largeDictionary);
            var endTime = DateTime.Now;

//            Console.WriteLine($"Sorted {largeDictionary.Count} items in {(endTime - startTime).TotalMilliseconds:F2} ms");
//            Console.WriteLine($"First 5 sorted items:");
            
            foreach (var kvp in sorted.Take(5))
            {
//                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
        }
    }
}
