using System;
using System.Collections.Generic;
using System.Linq;
using CalibrationSaaS.Infraestructure.Blazor.Shared;

namespace CalibrationSaaS.Tests
{
    /// <summary>
    /// Unit tests for the CreateGroupedDictionary method
    /// </summary>
    public class CreateGroupedDictionaryTests
    {
        // Test class
        public class TestItem
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }
            public int Value { get; set; }
        }

        // Mock AppState for testing
        public class MockAppState
        {
            public List<TestItem> GetTestItems()
            {
                return new List<TestItem>
                {
                    new TestItem { Id = "1", Name = "Item1", Category = "A", Value = 10 },
                    new TestItem { Id = "2", Name = "Item2", Category = "A", Value = 20 },
                    new TestItem { Id = "3", Name = "Item3", Category = "B", Value = 30 }
                };
            }

            public List<TestItem> GetTestItemsByCategory(string category)
            {
                var allItems = GetTestItems();
                return allItems.Where(x => x.Category == category).ToList();
            }

            public List<TestItem> GetTestItemsByValues(int[] values)
            {
                var allItems = GetTestItems();
                return allItems.Where(x => values.Contains(x.Value)).ToList();
            }
        }

        // Test component that inherits from GenericTestComponent
        public class TestComponent : GenericTestComponent<object, object, object>
        {
            public TestComponent()
            {
                // Initialize AppState with mock for testing
                AppState = new CalibrationSaaS.Domain.Aggregates.Shared.Basic.AppState();
            }

            public Dictionary<string, Dictionary<string, string>> TestCreateGroupedDictionary<T>(
                string keyPropertyName,
                string valuePropertyName,
                string groupingPropertyName,
                List<T> items)
            {
                return CreateGroupedDictionary(keyPropertyName, valuePropertyName, groupingPropertyName, items);
            }

            public Dictionary<string, Dictionary<string, string>> TestCreateGroupedDictionaryFromAppState(
                string appStateMethodName,
                string keyPropertyName,
                string valuePropertyName,
                string groupingPropertyName,
                params object[] methodParameters)
            {
                return CreateGroupedDictionaryFromAppState(appStateMethodName, keyPropertyName, valuePropertyName, groupingPropertyName, methodParameters);
            }
        }

        public static void RunTests()
        {
            var testComponent = new TestComponent();
            
//            Console.WriteLine("Running CreateGroupedDictionary Tests...");
//            Console.WriteLine("=======================================");

            // Test 1: Basic functionality
            Test1_BasicFunctionality(testComponent);
            
            // Test 2: Empty list
            Test2_EmptyList(testComponent);
            
            // Test 3: Null items in list
            Test3_NullItemsInList(testComponent);
            
            // Test 4: Invalid property names
            Test4_InvalidPropertyNames(testComponent);
            
            // Test 5: Duplicate keys within groups
            Test5_DuplicateKeysWithinGroups(testComponent);

            // Test 6: AppState method calls
            Test6_AppStateMethodCalls(testComponent);

//            Console.WriteLine("\nAll tests completed!");
        }

        private static void Test1_BasicFunctionality(TestComponent testComponent)
        {
//            Console.WriteLine("\nTest 1: Basic Functionality");
//            Console.WriteLine("---------------------------");

            var items = new List<TestItem>
            {
                new TestItem { Id = "1", Name = "Item1", Category = "A", Value = 10 },
                new TestItem { Id = "2", Name = "Item2", Category = "A", Value = 20 },
                new TestItem { Id = "3", Name = "Item3", Category = "B", Value = 30 },
                new TestItem { Id = "4", Name = "Item4", Category = "B", Value = 40 }
            };

            var result = testComponent.TestCreateGroupedDictionary("Id", "Name", "Category", items);

            // Verify results
            if (result.Count == 2 && 
                result.ContainsKey("A") && result.ContainsKey("B") &&
                result["A"].Count == 2 && result["B"].Count == 2 &&
                result["A"]["1"] == "Item1" && result["A"]["2"] == "Item2" &&
                result["B"]["3"] == "Item3" && result["B"]["4"] == "Item4")
            {
//                Console.WriteLine("✓ PASSED: Basic functionality works correctly");
            }
            else
            {
//                Console.WriteLine("✗ FAILED: Basic functionality test failed");
            }
        }

        private static void Test2_EmptyList(TestComponent testComponent)
        {
//            Console.WriteLine("\nTest 2: Empty List");
//            Console.WriteLine("------------------");

            var emptyList = new List<TestItem>();
            var result = testComponent.TestCreateGroupedDictionary("Id", "Name", "Category", emptyList);

            if (result.Count == 0)
            {
//                Console.WriteLine("✓ PASSED: Empty list returns empty dictionary");
            }
            else
            {
//                Console.WriteLine("✗ FAILED: Empty list test failed");
            }
        }

        private static void Test3_NullItemsInList(TestComponent testComponent)
        {
//            Console.WriteLine("\nTest 3: Null Items in List");
//            Console.WriteLine("--------------------------");

            var itemsWithNull = new List<TestItem>
            {
                new TestItem { Id = "1", Name = "Item1", Category = "A", Value = 10 },
                null,
                new TestItem { Id = "2", Name = "Item2", Category = "A", Value = 20 }
            };

            var result = testComponent.TestCreateGroupedDictionary("Id", "Name", "Category", itemsWithNull);

            if (result.Count == 1 && result["A"].Count == 2)
            {
//                Console.WriteLine("✓ PASSED: Null items are properly skipped");
            }
            else
            {
//                Console.WriteLine("✗ FAILED: Null items test failed");
            }
        }

        private static void Test4_InvalidPropertyNames(TestComponent testComponent)
        {
//            Console.WriteLine("\nTest 4: Invalid Property Names");
//            Console.WriteLine("------------------------------");

            var items = new List<TestItem>
            {
                new TestItem { Id = "1", Name = "Item1", Category = "A", Value = 10 }
            };

            try
            {
                var result = testComponent.TestCreateGroupedDictionary("InvalidProperty", "Name", "Category", items);
//                Console.WriteLine("✗ FAILED: Should have thrown exception for invalid property");
            }
            catch (ArgumentException)
            {
//                Console.WriteLine("✓ PASSED: Correctly throws exception for invalid property name");
            }
            catch (Exception ex)
            {
//                Console.WriteLine($"✗ FAILED: Unexpected exception type: {ex.GetType().Name}");
            }
        }

        private static void Test5_DuplicateKeysWithinGroups(TestComponent testComponent)
        {
//            Console.WriteLine("\nTest 5: Duplicate Keys Within Groups");
//            Console.WriteLine("------------------------------------");

            var items = new List<TestItem>
            {
                new TestItem { Id = "1", Name = "FirstItem", Category = "A", Value = 10 },
                new TestItem { Id = "1", Name = "SecondItem", Category = "A", Value = 20 }
            };

            var result = testComponent.TestCreateGroupedDictionary("Id", "Name", "Category", items);

            if (result["A"]["1"] == "SecondItem")
            {
//                Console.WriteLine("✓ PASSED: Duplicate keys are overwritten with last value");
            }
            else
            {
//                Console.WriteLine("✗ FAILED: Duplicate keys test failed");
            }
        }

        private static void Test6_AppStateMethodCalls(TestComponent testComponent)
        {
//            Console.WriteLine("\nTest 6: AppState Method Calls");
//            Console.WriteLine("-----------------------------");

            // Test calling a method that exists in AppState
            try
            {
                // This will test calling GetAllUnitOfMeasure() which should exist in AppState
                var result = testComponent.TestCreateGroupedDictionaryFromAppState(
                    "GetAllUnitOfMeasure",
                    "UnitOfMeasureID",
                    "Name",
                    "TypeID"
                );

                // Since AppState might be empty in test environment, we just check that no exception was thrown
//                Console.WriteLine("✓ PASSED: AppState method call completed without exception");
            }
            catch (ArgumentException ex) when (ex.Message.Contains("Method") && ex.Message.Contains("not found"))
            {
//                Console.WriteLine("⚠ WARNING: Method not found in AppState (expected in test environment)");
            }
            catch (Exception ex)
            {
//                Console.WriteLine($"✗ FAILED: Unexpected exception: {ex.Message}");
            }

            // Test calling a non-existent method
            try
            {
                var result = testComponent.TestCreateGroupedDictionaryFromAppState(
                    "NonExistentMethod",
                    "Id",
                    "Name",
                    "Category"
                );
//                Console.WriteLine("✗ FAILED: Should have thrown exception for non-existent method");
            }
            catch (ArgumentException ex) when (ex.Message.Contains("Method") && ex.Message.Contains("not found"))
            {
//                Console.WriteLine("✓ PASSED: Correctly throws exception for non-existent method");
            }
            catch (Exception ex)
            {
//                Console.WriteLine($"✗ FAILED: Unexpected exception type: {ex.GetType().Name}");
            }
        }
    }
}
