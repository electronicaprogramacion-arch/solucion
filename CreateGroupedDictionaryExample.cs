using System;
using System.Collections.Generic;
using CalibrationSaaS.Infraestructure.Blazor.Shared;

namespace CalibrationSaaS.Examples
{
    /// <summary>
    /// Example demonstrating the usage of the CreateGroupedDictionary method
    /// </summary>
    public class CreateGroupedDictionaryExample
    {
        // Example class to demonstrate the functionality
        public class Product
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }
            public decimal Price { get; set; }
        }

        // Example class that inherits from GenericTestComponent to access the method
        public class ExampleComponent : GenericTestComponent<object, object, object>
        {
            public void DemonstrateUsageFromAppState()
            {
                // Example 1: Using AppState method without parameters
                // This calls a method like GetAllUnitOfMeasure() from AppState
                try
                {
                    var groupedByType = CreateGroupedDictionaryFromAppState(
                        appStateMethodName: "GetAllUnitOfMeasure",
                        keyPropertyName: "UnitOfMeasureID",
                        valuePropertyName: "Name",
                        groupingPropertyName: "TypeID"
                    );

//                    Console.WriteLine("Units of Measure grouped by Type:");
//                    Console.WriteLine("=================================");

                    foreach (var group in groupedByType)
                    {
//                        Console.WriteLine($"\nType ID: {group.Key}");
//                        Console.WriteLine("Units:");

                        foreach (var item in group.Value)
                        {
//                            Console.WriteLine($"  ID: {item.Key} -> Name: {item.Value}");
                        }
                    }
                }
                catch (Exception ex)
                {
//                    Console.WriteLine($"Error calling AppState method: {ex.Message}");
                }
            }

            public void DemonstrateUsageWithParameters()
            {
                // Example 2: Using AppState method with parameters
                // This calls a method like GetUoMByTypes(int[] calibrationTypes) from AppState
                try
                {
                    var calibrationTypes = new int[] { 1, 2, 3 };

                    var groupedByCalType = CreateGroupedDictionaryFromAppState(
                        appStateMethodName: "GetUoMByTypes",
                        keyPropertyName: "UnitOfMeasureID",
                        valuePropertyName: "Name",
                        groupingPropertyName: "TypeID",
                        methodParameters: calibrationTypes
                    );

//                    Console.WriteLine("\nUnits of Measure for specific calibration types:");
//                    Console.WriteLine("===============================================");

                    foreach (var group in groupedByCalType)
                    {
//                        Console.WriteLine($"\nType ID: {group.Key}");
//                        Console.WriteLine("Units:");

                        foreach (var item in group.Value)
                        {
//                            Console.WriteLine($"  ID: {item.Key} -> Name: {item.Value}");
                        }
                    }
                }
                catch (Exception ex)
                {
//                    Console.WriteLine($"Error calling AppState method with parameters: {ex.Message}");
                }
            }

            public void DemonstrateOriginalUsage()
            {
                // Example 3: Using the original method with a direct list
                var products = new List<Product>
                {
                    new Product { Id = "1", Name = "Laptop", Category = "Electronics", Price = 999.99m },
                    new Product { Id = "2", Name = "Mouse", Category = "Electronics", Price = 25.99m },
                    new Product { Id = "3", Name = "Keyboard", Category = "Electronics", Price = 79.99m },
                    new Product { Id = "4", Name = "Desk", Category = "Furniture", Price = 299.99m },
                    new Product { Id = "5", Name = "Chair", Category = "Furniture", Price = 199.99m }
                };

                // Use the original CreateGroupedDictionary method
                var groupedDictionary = CreateGroupedDictionary(
                    keyPropertyName: "Id",
                    valuePropertyName: "Name",
                    groupingPropertyName: "Category",
                    items: products
                );

//                Console.WriteLine("\nOriginal method - Products grouped by Category:");
//                Console.WriteLine("==============================================");

                foreach (var group in groupedDictionary)
                {
//                    Console.WriteLine($"\nCategory: {group.Key}");
//                    Console.WriteLine("Products:");

                    foreach (var item in group.Value)
                    {
//                        Console.WriteLine($"  ID: {item.Key} -> Name: {item.Value}");
                    }
                }
            }

        }

        // Static method to run the examples
        public static void RunExamples()
        {
            var example = new ExampleComponent();

            // Note: These examples require a properly initialized AppState
            // In a real application, AppState would be injected and populated
            example.DemonstrateUsageFromAppState();
            example.DemonstrateUsageWithParameters();
            example.DemonstrateOriginalUsage();
        }
    }
}
