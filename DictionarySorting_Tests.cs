using System;
using System.Collections.Generic;
using System.Linq;
using CalibrationSaaS.Utilities;

namespace CalibrationSaaS.Tests
{
    /// <summary>
    /// Unit tests for dictionary sorting utilities
    /// </summary>
    public class DictionarySortingTests
    {
        public static void RunAllTests()
        {
//            Console.WriteLine("Dictionary Sorting Tests");
//            Console.WriteLine("========================");

            // Test 1: Basic sorting
            Test1_BasicSorting();

            // Test 2: Case sensitivity
            Test2_CaseSensitivity();

            // Test 3: Null and empty handling
            Test3_NullAndEmptyHandling();

            // Test 4: Grouped dictionary sorting
            Test4_GroupedDictionarySorting();

            // Test 5: Performance with large dataset
            Test5_PerformanceTest();

//            Console.WriteLine("\nAll tests completed!");
        }

        private static void Test1_BasicSorting()
        {
//            Console.WriteLine("\nTest 1: Basic Sorting");
//            Console.WriteLine("--------------------");

            var testDict = new Dictionary<string, string>
            {
                { "3", "Charlie" },
                { "1", "Alpha" },
                { "4", "Delta" },
                { "2", "Bravo" }
            };

            var sorted = DictionarySortingUtilities.SortByValues(testDict);
            var expectedOrder = new[] { "Alpha", "Bravo", "Charlie", "Delta" };

            bool passed = sorted.Select(kvp => kvp.Value).SequenceEqual(expectedOrder);
//            Console.WriteLine($"✓ Basic sorting: {(passed ? "PASSED" : "FAILED")}");

            if (!passed)
            {
//                Console.WriteLine("Expected: " + string.Join(", ", expectedOrder));
//                Console.WriteLine("Actual: " + string.Join(", ", sorted.Select(kvp => kvp.Value)));
            }
        }

        private static void Test2_CaseSensitivity()
        {
//            Console.WriteLine("\nTest 2: Case Sensitivity");
//            Console.WriteLine("------------------------");

            var testDict = new Dictionary<string, string>
            {
                { "1", "apple" },
                { "2", "Banana" },
                { "3", "cherry" },
                { "4", "Apple" }
            };

            // Case-sensitive sorting
            var caseSensitive = DictionarySortingUtilities.SortByValues(testDict);
            var caseSensitiveValues = caseSensitive.Select(kvp => kvp.Value).ToArray();

            // Case-insensitive sorting
            var caseInsensitive = DictionarySortingUtilities.SortByValuesIgnoreCase(testDict);
            var caseInsensitiveValues = caseInsensitive.Select(kvp => kvp.Value).ToArray();

            // In case-sensitive: "Apple", "Banana", "apple", "cherry" (uppercase first)
            // In case-insensitive: "apple", "Apple", "Banana", "cherry" (alphabetical)

            bool caseSensitiveCorrect = caseSensitiveValues[0] == "Apple"; // Uppercase comes first
            bool caseInsensitiveCorrect = caseInsensitiveValues[0] == "apple" || caseInsensitiveValues[0] == "Apple"; // Either apple variant first

//            Console.WriteLine($"✓ Case-sensitive sorting: {(caseSensitiveCorrect ? "PASSED" : "FAILED")}");
//            Console.WriteLine($"✓ Case-insensitive sorting: {(caseInsensitiveCorrect ? "PASSED" : "FAILED")}");

//            Console.WriteLine($"  Case-sensitive result: {string.Join(", ", caseSensitiveValues)}");
//            Console.WriteLine($"  Case-insensitive result: {string.Join(", ", caseInsensitiveValues)}");
        }

        private static void Test3_NullAndEmptyHandling()
        {
//            Console.WriteLine("\nTest 3: Null and Empty Handling");
//            Console.WriteLine("-------------------------------");

            // Test null dictionary
            var nullResult = DictionarySortingUtilities.SortByValues(null);
            bool nullHandled = nullResult != null && nullResult.Count == 0;
//            Console.WriteLine($"✓ Null dictionary handling: {(nullHandled ? "PASSED" : "FAILED")}");

            // Test empty dictionary
            var emptyDict = new Dictionary<string, string>();
            var emptyResult = DictionarySortingUtilities.SortByValues(emptyDict);
            bool emptyHandled = emptyResult != null && emptyResult.Count == 0;
//            Console.WriteLine($"✓ Empty dictionary handling: {(emptyHandled ? "PASSED" : "FAILED")}");

            // Test dictionary with null/empty values
            var dictWithNulls = new Dictionary<string, string>
            {
                { "1", "Alpha" },
                { "2", null },
                { "3", "" },
                { "4", "Beta" }
            };

            try
            {
                var sortedWithNulls = DictionarySortingUtilities.SortByValues(dictWithNulls);
//                Console.WriteLine($"✓ Dictionary with null/empty values: PASSED");
//                Console.WriteLine($"  Result: {string.Join(", ", sortedWithNulls.Select(kvp => $"{kvp.Key}:{kvp.Value ?? "null"}"))}");
            }
            catch (Exception ex)
            {
//                Console.WriteLine($"✗ Dictionary with null/empty values: FAILED - {ex.Message}");
            }
        }

        private static void Test4_GroupedDictionarySorting()
        {
//            Console.WriteLine("\nTest 4: Grouped Dictionary Sorting");
//            Console.WriteLine("----------------------------------");

            var groupedDict = new Dictionary<string, Dictionary<string, string>>
            {
                {
                    "GroupB", new Dictionary<string, string>
                    {
                        { "2", "Zebra" },
                        { "1", "Alpha" }
                    }
                },
                {
                    "GroupA", new Dictionary<string, string>
                    {
                        { "4", "Delta" },
                        { "3", "Charlie" }
                    }
                }
            };

            // Test inner sorting only
            var innerSorted = DictionarySortingUtilities.SortGroupedDictionaryByValues(groupedDict);
            bool innerSortCorrect = innerSorted["GroupB"].First().Value == "Alpha" && 
                                   innerSorted["GroupA"].First().Value == "Charlie";
//            Console.WriteLine($"✓ Inner dictionary sorting: {(innerSortCorrect ? "PASSED" : "FAILED")}");

            // Test complete sorting
            var completeSorted = DictionarySortingUtilities.SortGroupedDictionaryCompletely(groupedDict);
            bool completeSortCorrect = completeSorted.First().Key == "GroupA" && 
                                      completeSorted.First().Value.First().Value == "Charlie";
//            Console.WriteLine($"✓ Complete sorting: {(completeSortCorrect ? "PASSED" : "FAILED")}");
        }

        private static void Test5_PerformanceTest()
        {
//            Console.WriteLine("\nTest 5: Performance Test");
//            Console.WriteLine("------------------------");

            // Create large dictionary
            var largeDict = new Dictionary<string, string>();
            var random = new Random(42); // Fixed seed for reproducible results

            for (int i = 0; i < 1000; i++)
            {
                largeDict[$"Key{i:D4}"] = $"Value{random.Next(1000):D3}";
            }

            var startTime = DateTime.Now;
            var sorted = DictionarySortingUtilities.SortByValues(largeDict);
            var endTime = DateTime.Now;

            var duration = (endTime - startTime).TotalMilliseconds;
            bool performanceOk = duration < 100; // Should complete in less than 100ms

//            Console.WriteLine($"✓ Performance test (1000 items): {(performanceOk ? "PASSED" : "FAILED")}");
//            Console.WriteLine($"  Duration: {duration:F2} ms");
//            Console.WriteLine($"  Items sorted: {sorted.Count}");

            // Verify sorting is correct
            bool sortingCorrect = true;
            for (int i = 1; i < sorted.Count; i++)
            {
                if (string.Compare(sorted[i-1].Value, sorted[i].Value, StringComparison.Ordinal) > 0)
                {
                    sortingCorrect = false;
                    break;
                }
            }

//            Console.WriteLine($"✓ Large dataset sorting correctness: {(sortingCorrect ? "PASSED" : "FAILED")}");
        }

        /// <summary>
        /// Test the extension methods
        /// </summary>
        public static void TestExtensionMethods()
        {
//            Console.WriteLine("\nTesting Extension Methods");
//            Console.WriteLine("========================");

            var testDict = new Dictionary<string, string>
            {
                { "3", "Charlie" },
                { "1", "Alpha" },
                { "2", "Bravo" }
            };

            // Test extension method (if implemented)
            try
            {
                // This would require the extension methods to be in scope
                // var sorted = testDict.SortByValues(ignoreCase: true);
                // Console.WriteLine($"✓ Extension method: PASSED");
//                Console.WriteLine("✓ Extension methods available in DictionaryExtensions class");
            }
            catch (Exception ex)
            {
//                Console.WriteLine($"✗ Extension method: FAILED - {ex.Message}");
            }
        }

        /// <summary>
        /// Comprehensive test with real-world data patterns
        /// </summary>
        public static void TestRealWorldScenarios()
        {
//            Console.WriteLine("\nReal-World Scenarios Test");
//            Console.WriteLine("=========================");

            // Scenario 1: Units of measure with mixed case and special characters
            var unitsDict = new Dictionary<string, string>
            {
                { "101", "mm (millimeter)" },
                { "102", "CM (Centimeter)" },
                { "103", "m (meter)" },
                { "104", "in (inch)" },
                { "105", "ft (foot)" }
            };

            var sortedUnits = DictionarySortingUtilities.SortByValuesIgnoreCase(unitsDict);
//            Console.WriteLine("Sorted units of measure:");
            foreach (var unit in sortedUnits)
            {
//                Console.WriteLine($"  {unit.Key}: {unit.Value}");
            }

            // Scenario 2: Equipment with numeric values in names
            var equipmentDict = new Dictionary<string, string>
            {
                { "E001", "Equipment 10" },
                { "E002", "Equipment 2" },
                { "E003", "Equipment 1" },
                { "E004", "Equipment 20" }
            };

            var sortedEquipment = DictionarySortingUtilities.SortByValues(equipmentDict);
//            Console.WriteLine("\nSorted equipment (note: string sorting, not numeric):");
            foreach (var equipment in sortedEquipment)
            {
//                Console.WriteLine($"  {equipment.Key}: {equipment.Value}");
            }

//            Console.WriteLine("\n✓ Real-world scenarios completed");
        }
    }
}
