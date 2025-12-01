# CreateGroupedDictionary Methods Documentation

## Overview
Two `CreateGroupedDictionary` methods have been added to the `GenericTestComponent` class in the `CalibrationSaaS.Infraestructure.Blazor.Shared` namespace. These methods provide generic ways to group objects by a specified property and create nested dictionaries using reflection.

## Location
- **File**: `CalibrationSaaS.Infraestructure.Blazor/Shared/GenericTestComponentBase.cs`
- **Class**: `GenericTestComponent<CalibrationItem, CalibrationItemResult, DomainEntity>`
- **Namespace**: `CalibrationSaaS.Infraestructure.Blazor.Shared`

## Method Signatures

### 1. CreateGroupedDictionaryFromAppState (New Primary Method)
```csharp
public Dictionary<string, Dictionary<string, string>> CreateGroupedDictionaryFromAppState(
    string appStateMethodName,
    string keyPropertyName,
    string valuePropertyName,
    string groupingPropertyName,
    params object[] methodParameters)
```

### 2. CreateGroupedDictionary (Original Method - Backward Compatibility)
```csharp
public Dictionary<string, Dictionary<string, string>> CreateGroupedDictionary<T>(
    string keyPropertyName,
    string valuePropertyName,
    string groupingPropertyName,
    List<T> items)
```

## Parameters

### CreateGroupedDictionaryFromAppState Parameters
- **`appStateMethodName`** (string): The name of the method in AppState that returns a list or enumerable
- **`keyPropertyName`** (string): The name of the property that will serve as the key in the inner dictionary
- **`valuePropertyName`** (string): The name of the property that will serve as the value in the inner dictionary
- **`groupingPropertyName`** (string): The name of the property that will be used for grouping (outer dictionary key)
- **`methodParameters`** (params object[]): Optional parameters to pass to the AppState method

### CreateGroupedDictionary Parameters (Original)
- **`keyPropertyName`** (string): The name of the property that will serve as the key in the inner dictionary
- **`valuePropertyName`** (string): The name of the property that will serve as the value in the inner dictionary
- **`groupingPropertyName`** (string): The name of the property that will be used for grouping (outer dictionary key)
- **`items`** (List<T>): A generic list of objects of any class type

## Return Type
Both methods return: `Dictionary<string, Dictionary<string, string>>`
- **Outer Dictionary**: Key is the value from the grouping property
- **Inner Dictionary**: Contains key-value pairs from the specified key and value properties

## Features
- **AppState Integration**: New method calls AppState methods dynamically using reflection
- **Parameter Support**: Supports methods with parameters using params array
- **Generic**: Original method works with any class type `T`
- **Reflection-based**: Uses reflection to access properties and methods by name
- **Null-safe**: Handles null items and null property values gracefully
- **Validation**: Validates that all specified properties and methods exist
- **Error handling**: Provides meaningful error messages for invalid inputs
- **Backward Compatibility**: Original method is preserved for existing code

## Usage Examples

### Example 1: Using AppState Methods (Recommended)

#### Calling AppState method without parameters
```csharp
// Call GetAllUnitOfMeasure() from AppState and group by TypeID
var groupedUnits = CreateGroupedDictionaryFromAppState(
    appStateMethodName: "GetAllUnitOfMeasure",
    keyPropertyName: "UnitOfMeasureID",
    valuePropertyName: "Name",
    groupingPropertyName: "TypeID"
);

// Result: Units of measure grouped by their type
```

#### Calling AppState method with parameters
```csharp
// Call GetUoMByTypes(int[] calibrationTypes) from AppState
var calibrationTypes = new int[] { 1, 2, 3 };
var groupedByCalType = CreateGroupedDictionaryFromAppState(
    appStateMethodName: "GetUoMByTypes",
    keyPropertyName: "UnitOfMeasureID",
    valuePropertyName: "Name",
    groupingPropertyName: "TypeID",
    methodParameters: calibrationTypes
);

// Result: Units of measure for specific calibration types, grouped by TypeID
```

#### Calling AppState method with single parameter
```csharp
// Call GetUoMByCalibrationTypeObj(CalibrationType calibrationType) from AppState
var calibrationType = new CalibrationType { CalibrationTypeID = 1 };
var groupedByCalibrationType = CreateGroupedDictionaryFromAppState(
    appStateMethodName: "GetUoMByCalibrationTypeObj",
    keyPropertyName: "UnitOfMeasureID",
    valuePropertyName: "Name",
    groupingPropertyName: "TypeID",
    methodParameters: calibrationType
);
```

### Example 2: Using Original Method (Backward Compatibility)

#### Sample Data Class
```csharp
public class Product
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
}
```

#### Sample Usage
```csharp
// Create sample data
var products = new List<Product>
{
    new Product { Id = "1", Name = "Laptop", Category = "Electronics", Price = 999.99m },
    new Product { Id = "2", Name = "Mouse", Category = "Electronics", Price = 25.99m },
    new Product { Id = "3", Name = "Desk", Category = "Furniture", Price = 299.99m },
    new Product { Id = "4", Name = "Chair", Category = "Furniture", Price = 199.99m }
};

// Use the original method
var groupedDictionary = CreateGroupedDictionary(
    keyPropertyName: "Id",
    valuePropertyName: "Name",
    groupingPropertyName: "Category",
    items: products
);

// Result structure:
// {
//   "Electronics": {
//     "1": "Laptop",
//     "2": "Mouse"
//   },
//   "Furniture": {
//     "3": "Desk",
//     "4": "Chair"
//   }
// }
```

## Error Handling

### CreateGroupedDictionaryFromAppState
- `InvalidOperationException`: When AppState is not available
- `ArgumentException`: When method name or property names are null or empty
- `ArgumentException`: When the specified method doesn't exist in AppState
- `ArgumentException`: When the method doesn't return a list or enumerable type
- `ArgumentException`: When a specified property doesn't exist on the returned object type
- `InvalidOperationException`: When there's an error calling the AppState method

### CreateGroupedDictionary (Original)
- `ArgumentNullException`: When the `items` parameter is null
- `ArgumentException`: When any property name parameter is null or empty
- `ArgumentException`: When a specified property doesn't exist on the type `T`

## Important Notes
1. **AppState Dependency**: The new method requires AppState to be properly injected and initialized
2. **Method Resolution**: Uses reflection to find methods by name; supports method overloading
3. **Parameter Matching**: Attempts to match method parameters by type and count
4. **Property Access**: Uses reflection to access properties, so property names must match exactly (case-sensitive)
5. **String Conversion**: All property values are converted to strings using `ToString()`
6. **Null Handling**: Null items in lists are skipped; null property values become empty strings
7. **Duplicate Keys**: If duplicate keys exist within a group, the last value overwrites previous ones
8. **Performance**: Uses reflection extensively, so consider performance implications for large datasets
9. **Return Type Validation**: Validates that AppState methods return enumerable types

## Available AppState Methods
Common AppState methods that can be used:
- `GetAllUnitOfMeasure()` - Returns all units of measure
- `GetUoMByTypes(int[] calibrationTypes)` - Returns units filtered by calibration types
- `GetUoMByCalibrationTypeObj(CalibrationType calibrationType)` - Returns units for specific calibration type
- `GetUoMByEquipmentType(EquipmentType equipmentType)` - Returns units for equipment type
- And other methods that return `List<T>` or `IEnumerable<T>`

## Testing
Test files have been updated to demonstrate both methods:
- `CreateGroupedDictionaryExample.cs`: Contains usage examples for both methods
- `CreateGroupedDictionaryTests.cs`: Contains unit tests for both methods

## Integration
Both methods are now available in any class that inherits from `GenericTestComponent`:
- Use `CreateGroupedDictionaryFromAppState` for dynamic AppState method calls (recommended)
- Use `CreateGroupedDictionary` for direct list processing (backward compatibility)
