using System;
using System.Collections.Generic;
using CalibrationSaaS.Infraestructure.Blazor.Shared;
using CalibrationSaaS.Domain.Aggregates.Entities;

namespace CalibrationSaaS.Examples
{
    /// <summary>
    /// Real-world examples of using CreateGroupedDictionaryFromAppState with actual AppState methods
    /// </summary>
    public class AppStateMethodsExample
    {
        // Example component that inherits from GenericTestComponent
        public class RealWorldComponent : GenericTestComponent<object, object, object>
        {
            /// <summary>
            /// Example 1: Group all units of measure by their type
            /// Uses: GetAllUnitOfMeasure() method from AppState
            /// </summary>
            public void GroupUnitsByType()
            {
                try
                {
                    var groupedUnits = CreateGroupedDictionaryFromAppState(
                        appStateMethodName: "GetAllUnitOfMeasure",
                        keyPropertyName: "UnitOfMeasureID",
                        valuePropertyName: "Name",
                        groupingPropertyName: "TypeID"
                    );

//                    Console.WriteLine("Units of Measure grouped by Type ID:");
//                    Console.WriteLine("===================================");
                    
                    foreach (var typeGroup in groupedUnits)
                    {
//                        Console.WriteLine($"\nType ID: {typeGroup.Key}");
                        foreach (var unit in typeGroup.Value)
                        {
//                            Console.WriteLine($"  Unit ID {unit.Key}: {unit.Value}");
                        }
                    }
                }
                catch (Exception ex)
                {
//                    Console.WriteLine($"Error grouping units by type: {ex.Message}");
                }
            }

            /// <summary>
            /// Example 2: Group units of measure by calibration types (with parameters)
            /// Uses: GetUoMByTypes(int[] calibrationTypes) method from AppState
            /// </summary>
            public void GroupUnitsByCalibrationTypes()
            {
                try
                {
                    // Specify which calibration types we want
                    var calibrationTypes = new int[] { 1, 2, 3, 4, 5 };
                    
                    var groupedUnits = CreateGroupedDictionaryFromAppState(
                        appStateMethodName: "GetUoMByTypes",
                        keyPropertyName: "UnitOfMeasureID",
                        valuePropertyName: "Name",
                        groupingPropertyName: "TypeID",
                        methodParameters: calibrationTypes
                    );

//                    Console.WriteLine("\nUnits for specific calibration types, grouped by Type ID:");
//                    Console.WriteLine("========================================================");
                    
                    foreach (var typeGroup in groupedUnits)
                    {
//                        Console.WriteLine($"\nType ID: {typeGroup.Key}");
                        foreach (var unit in typeGroup.Value)
                        {
//                            Console.WriteLine($"  Unit ID {unit.Key}: {unit.Value}");
                        }
                    }
                }
                catch (Exception ex)
                {
//                    Console.WriteLine($"Error grouping units by calibration types: {ex.Message}");
                }
            }

            /// <summary>
            /// Example 3: Group units by equipment type (with object parameter)
            /// Uses: GetUoMByEquipmentType(EquipmentType equipmentType) method from AppState
            /// </summary>
            public void GroupUnitsByEquipmentType(EquipmentType equipmentType)
            {
                try
                {
                    var groupedUnits = CreateGroupedDictionaryFromAppState(
                        appStateMethodName: "GetUoMByEquipmentType",
                        keyPropertyName: "UnitOfMeasureID",
                        valuePropertyName: "Name",
                        groupingPropertyName: "TypeID",
                        methodParameters: equipmentType
                    );

//                    Console.WriteLine($"\nUnits for Equipment Type '{equipmentType?.Name}', grouped by Type ID:");
//                    Console.WriteLine("=====================================================================");
                    
                    foreach (var typeGroup in groupedUnits)
                    {
//                        Console.WriteLine($"\nType ID: {typeGroup.Key}");
                        foreach (var unit in typeGroup.Value)
                        {
//                            Console.WriteLine($"  Unit ID {unit.Key}: {unit.Value}");
                        }
                    }
                }
                catch (Exception ex)
                {
//                    Console.WriteLine($"Error grouping units by equipment type: {ex.Message}");
                }
            }

            /// <summary>
            /// Example 4: Group equipment types by their properties
            /// Uses: EquipmentTypes property from AppState (if it has a getter method)
            /// </summary>
            public void GroupEquipmentTypes()
            {
                try
                {
                    // Note: This assumes there's a method to get equipment types
                    // You might need to adjust the method name based on actual AppState implementation
                    var groupedEquipment = CreateGroupedDictionaryFromAppState(
                        appStateMethodName: "GetEquipmentTypes", // Adjust method name as needed
                        keyPropertyName: "EquipmentTypeID",
                        valuePropertyName: "Name",
                        groupingPropertyName: "CalibrationTypeID"
                    );

//                    Console.WriteLine("\nEquipment Types grouped by Calibration Type ID:");
//                    Console.WriteLine("===============================================");
                    
                    foreach (var calTypeGroup in groupedEquipment)
                    {
//                        Console.WriteLine($"\nCalibration Type ID: {calTypeGroup.Key}");
                        foreach (var equipment in calTypeGroup.Value)
                        {
//                            Console.WriteLine($"  Equipment ID {equipment.Key}: {equipment.Value}");
                        }
                    }
                }
                catch (Exception ex)
                {
//                    Console.WriteLine($"Error grouping equipment types: {ex.Message}");
                }
            }

            /// <summary>
            /// Example 5: Alternative grouping - Group units by name and show IDs
            /// Demonstrates different property combinations
            /// </summary>
            public void AlternativeGrouping()
            {
                try
                {
                    var groupedByName = CreateGroupedDictionaryFromAppState(
                        appStateMethodName: "GetAllUnitOfMeasure",
                        keyPropertyName: "TypeID",
                        valuePropertyName: "UnitOfMeasureID",
                        groupingPropertyName: "Name"
                    );

//                    Console.WriteLine("\nUnits grouped by Name (showing Type ID -> Unit ID):");
//                    Console.WriteLine("===================================================");
                    
                    foreach (var nameGroup in groupedByName)
                    {
//                        Console.WriteLine($"\nUnit Name: {nameGroup.Key}");
                        foreach (var typeToId in nameGroup.Value)
                        {
//                            Console.WriteLine($"  Type ID {typeToId.Key}: Unit ID {typeToId.Value}");
                        }
                    }
                }
                catch (Exception ex)
                {
//                    Console.WriteLine($"Error in alternative grouping: {ex.Message}");
                }
            }

            /// <summary>
            /// Example 6: Error handling demonstration
            /// Shows how to handle various error scenarios
            /// </summary>
            public void DemonstrateErrorHandling()
            {
//                Console.WriteLine("\nError Handling Examples:");
//                Console.WriteLine("========================");

                // Test 1: Non-existent method
                try
                {
                    var result = CreateGroupedDictionaryFromAppState(
                        "NonExistentMethod",
                        "Id",
                        "Name",
                        "Category"
                    );
                }
                catch (ArgumentException ex)
                {
//                    Console.WriteLine($"✓ Caught expected error for non-existent method: {ex.Message}");
                }

                // Test 2: Invalid property name
                try
                {
                    var result = CreateGroupedDictionaryFromAppState(
                        "GetAllUnitOfMeasure",
                        "InvalidProperty",
                        "Name",
                        "TypeID"
                    );
                }
                catch (ArgumentException ex)
                {
//                    Console.WriteLine($"✓ Caught expected error for invalid property: {ex.Message}");
                }

                // Test 3: Empty method name
                try
                {
                    var result = CreateGroupedDictionaryFromAppState(
                        "",
                        "UnitOfMeasureID",
                        "Name",
                        "TypeID"
                    );
                }
                catch (ArgumentException ex)
                {
//                    Console.WriteLine($"✓ Caught expected error for empty method name: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Static method to run all examples
        /// </summary>
        public static void RunAllExamples()
        {
            var component = new RealWorldComponent();
            
//            Console.WriteLine("AppState Methods Examples");
//            Console.WriteLine("========================");
            
            // Run examples
            component.GroupUnitsByType();
            component.GroupUnitsByCalibrationTypes();
            
            // Example with equipment type (you'd need to provide a real EquipmentType object)
            // component.GroupUnitsByEquipmentType(new EquipmentType { Name = "Test Equipment" });
            
            component.GroupEquipmentTypes();
            component.AlternativeGrouping();
            component.DemonstrateErrorHandling();
            
//            Console.WriteLine("\nAll examples completed!");
        }
    }
}
