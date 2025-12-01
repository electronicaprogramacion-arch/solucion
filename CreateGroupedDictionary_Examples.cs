using System;
using System.Collections.Generic;
using CalibrationSaaS.Infraestructure.Blazor.Shared;

namespace CalibrationSaaS.Examples
{
    /// <summary>
    /// Comprehensive examples demonstrating the updated CreateGroupedDictionary methods
    /// that support both flat and nested dictionary structures.
    /// </summary>
    public class CreateGroupedDictionaryExamples
    {
        // Sample data classes
        public class Product
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }
            public decimal Price { get; set; }
        }

        public class UnitOfMeasure
        {
            public string UnitOfMeasureID { get; set; }
            public string Name { get; set; }
            public string TypeID { get; set; }
            public string Symbol { get; set; }
        }

        // Example component that inherits from GenericTestComponent to access the methods
        public class ExampleComponent : GenericTestComponent<object, object, object>
        {
            /// <summary>
            /// Example 1: Create a flat dictionary without grouping
            /// Use case: Simple key-value lookup for dropdown lists
            /// </summary>
            public void Example1_FlatDictionary()
            {
                Console.WriteLine("=== Example 1: Flat Dictionary (No Grouping) ===\n");

                var products = new List<Product>
                {
                    new Product { Id = "1", Name = "Laptop", Category = "Electronics", Price = 999.99m },
                    new Product { Id = "2", Name = "Mouse", Category = "Electronics", Price = 25.99m },
                    new Product { Id = "3", Name = "Keyboard", Category = "Electronics", Price = 79.99m },
                    new Product { Id = "4", Name = "Desk", Category = "Furniture", Price = 299.99m },
                    new Product { Id = "5", Name = "Chair", Category = "Furniture", Price = 199.99m }
                };

                // Create a flat dictionary: Id -> Name (no grouping)
                var result = CreateGroupedDictionary(
                    keyPropertyName: "Id",
                    valuePropertyName: "Name",
                    groupingPropertyName: null,  // NULL = flat dictionary
                    items: products
                );

                // Cast to Dictionary<string, string>
                var flatDict = (Dictionary<string, string>)result;

                Console.WriteLine("Flat Dictionary Result:");
                foreach (var kvp in flatDict)
                {
                    Console.WriteLine($"  {kvp.Key} => {kvp.Value}");
                }

                // Output:
                // 1 => Laptop
                // 2 => Mouse
                // 3 => Keyboard
                // 4 => Desk
                // 5 => Chair

                Console.WriteLine();
            }

            /// <summary>
            /// Example 2: Create a nested dictionary with grouping
            /// Use case: Organize products by category
            /// </summary>
            public void Example2_NestedDictionary()
            {
                Console.WriteLine("=== Example 2: Nested Dictionary (With Grouping) ===\n");

                var products = new List<Product>
                {
                    new Product { Id = "1", Name = "Laptop", Category = "Electronics", Price = 999.99m },
                    new Product { Id = "2", Name = "Mouse", Category = "Electronics", Price = 25.99m },
                    new Product { Id = "3", Name = "Keyboard", Category = "Electronics", Price = 79.99m },
                    new Product { Id = "4", Name = "Desk", Category = "Furniture", Price = 299.99m },
                    new Product { Id = "5", Name = "Chair", Category = "Furniture", Price = 199.99m }
                };

                // Create a nested dictionary grouped by Category
                var result = CreateGroupedDictionary(
                    keyPropertyName: "Id",
                    valuePropertyName: "Name",
                    groupingPropertyName: "Category",  // Group by Category
                    items: products
                );

                // Cast to Dictionary<string, Dictionary<string, string>>
                var nestedDict = (Dictionary<string, Dictionary<string, string>>)result;

                Console.WriteLine("Nested Dictionary Result:");
                foreach (var group in nestedDict)
                {
                    Console.WriteLine($"Category: {group.Key}");
                    foreach (var kvp in group.Value)
                    {
                        Console.WriteLine($"  {kvp.Key} => {kvp.Value}");
                    }
                }

                // Output:
                // Category: Electronics
                //   1 => Laptop
                //   2 => Mouse
                //   3 => Keyboard
                // Category: Furniture
                //   4 => Desk
                //   5 => Chair

                Console.WriteLine();
            }

            /// <summary>
            /// Example 3: Using pattern matching for safe type handling
            /// Use case: When grouping parameter is dynamic/conditional
            /// </summary>
            public void Example3_PatternMatching()
            {
                Console.WriteLine("=== Example 3: Pattern Matching for Safe Type Handling ===\n");

                var products = new List<Product>
                {
                    new Product { Id = "1", Name = "Laptop", Category = "Electronics", Price = 999.99m },
                    new Product { Id = "2", Name = "Mouse", Category = "Electronics", Price = 25.99m }
                };

                // Simulate dynamic grouping parameter
                string groupBy = null; // Could be "Category" or null based on user input

                var result = CreateGroupedDictionary(
                    keyPropertyName: "Id",
                    valuePropertyName: "Name",
                    groupingPropertyName: groupBy,
                    items: products
                );

                // Use pattern matching to handle both types safely
                if (result is Dictionary<string, string> flatDict)
                {
                    Console.WriteLine("Result is a flat dictionary:");
                    foreach (var kvp in flatDict)
                    {
                        Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
                    }
                }
                else if (result is Dictionary<string, Dictionary<string, string>> nestedDict)
                {
                    Console.WriteLine("Result is a nested dictionary:");
                    foreach (var group in nestedDict)
                    {
                        Console.WriteLine($"  Group: {group.Key}");
                        foreach (var kvp in group.Value)
                        {
                            Console.WriteLine($"    {kvp.Key}: {kvp.Value}");
                        }
                    }
                }

                Console.WriteLine();
            }

            /// <summary>
            /// Example 4: Using AppState method to get flat dictionary
            /// Use case: Populate dropdown with units of measure
            /// </summary>
            public void Example4_AppStateFlatDictionary()
            {
                Console.WriteLine("=== Example 4: AppState Method - Flat Dictionary ===\n");

                try
                {
                    // Get all units of measure as a flat dictionary: UnitOfMeasureID -> Name
                    var result = CreateGroupedDictionaryFromAppState(
                        appStateMethodName: "GetAllUnitOfMeasure",
                        keyPropertyName: "UnitOfMeasureID",
                        valuePropertyName: "Name",
                        groupingPropertyName: null  // NULL = flat dictionary
                    );

                    var flatDict = (Dictionary<string, string>)result;

                    Console.WriteLine("Units of Measure (Flat):");
                    foreach (var kvp in flatDict)
                    {
                        Console.WriteLine($"  {kvp.Key} => {kvp.Value}");
                    }

                    // Example output:
                    // mm => Millimeter
                    // cm => Centimeter
                    // m => Meter
                    // kg => Kilogram
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine();
            }

            /// <summary>
            /// Example 5: Using AppState method to get nested dictionary
            /// Use case: Cascading dropdowns (Type -> Units)
            /// </summary>
            public void Example5_AppStateNestedDictionary()
            {
                Console.WriteLine("=== Example 5: AppState Method - Nested Dictionary ===\n");

                try
                {
                    // Get all units of measure grouped by TypeID
                    var result = CreateGroupedDictionaryFromAppState(
                        appStateMethodName: "GetAllUnitOfMeasure",
                        keyPropertyName: "UnitOfMeasureID",
                        valuePropertyName: "Name",
                        groupingPropertyName: "TypeID"  // Group by TypeID
                    );

                    var nestedDict = (Dictionary<string, Dictionary<string, string>>)result;

                    Console.WriteLine("Units of Measure (Grouped by Type):");
                    foreach (var group in nestedDict)
                    {
                        Console.WriteLine($"Type {group.Key}:");
                        foreach (var kvp in group.Value)
                        {
                            Console.WriteLine($"  {kvp.Key} => {kvp.Value}");
                        }
                    }

                    // Example output:
                    // Type 1:
                    //   mm => Millimeter
                    //   cm => Centimeter
                    //   m => Meter
                    // Type 2:
                    //   kg => Kilogram
                    //   g => Gram
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine();
            }

            /// <summary>
            /// Example 6: Using AppState method with parameters
            /// Use case: Get filtered data based on criteria
            /// </summary>
            public void Example6_AppStateWithParameters()
            {
                Console.WriteLine("=== Example 6: AppState Method with Parameters ===\n");

                try
                {
                    var calibrationTypes = new int[] { 1, 2, 3 };

                    // Get units of measure for specific calibration types (flat dictionary)
                    var result = CreateGroupedDictionaryFromAppState(
                        appStateMethodName: "GetUoMByTypes",
                        keyPropertyName: "UnitOfMeasureID",
                        valuePropertyName: "Symbol",
                        groupingPropertyName: null,  // NULL = flat dictionary
                        methodParameters: calibrationTypes
                    );

                    var flatDict = (Dictionary<string, string>)result;

                    Console.WriteLine("Filtered Units of Measure:");
                    foreach (var kvp in flatDict)
                    {
                        Console.WriteLine($"  {kvp.Key} => {kvp.Value}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine();
            }

            /// <summary>
            /// Example 7: Error handling
            /// Use case: Handling invalid property names or null data
            /// </summary>
            public void Example7_ErrorHandling()
            {
                Console.WriteLine("=== Example 7: Error Handling ===\n");

                var products = new List<Product>
                {
                    new Product { Id = "1", Name = "Laptop", Category = "Electronics" }
                };

                // Test 1: Invalid property name
                try
                {
                    var result = CreateGroupedDictionary(
                        keyPropertyName: "InvalidProperty",
                        valuePropertyName: "Name",
                        groupingPropertyName: null,
                        items: products
                    );
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Expected error: {ex.Message}");
                }

                // Test 2: Null items list
                try
                {
                    var result = CreateGroupedDictionary<Product>(
                        keyPropertyName: "Id",
                        valuePropertyName: "Name",
                        groupingPropertyName: null,
                        items: null
                    );
                }
                catch (ArgumentNullException ex)
                {
                    Console.WriteLine($"Expected error: {ex.Message}");
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Main method to run all examples
        /// </summary>
        public static void Main(string[] args)
        {
            var examples = new ExampleComponent();

            examples.Example1_FlatDictionary();
            examples.Example2_NestedDictionary();
            examples.Example3_PatternMatching();
            examples.Example4_AppStateFlatDictionary();
            examples.Example5_AppStateNestedDictionary();
            examples.Example6_AppStateWithParameters();
            examples.Example7_ErrorHandling();

            Console.WriteLine("All examples completed!");
        }
    }
}

