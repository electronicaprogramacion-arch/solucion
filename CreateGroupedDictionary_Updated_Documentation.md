# CreateGroupedDictionary Methods - Updated Documentation

## Overview
The `CreateGroupedDictionary` methods in the `GenericTestComponent` class have been updated to support both **flat** and **nested** dictionary structures based on whether grouping is required.

## Location
- **File**: `CalibrationSaaS.Infraestructure.Blazor/Shared/GenericTestComponentBase.cs`
- **Class**: `GenericTestComponent<CalibrationItem, CalibrationItemResult, DomainEntity>`
- **Namespace**: `CalibrationSaaS.Infraestructure.Blazor.Shared`

## What's New? 🎉

### Key Changes:
1. **Return Type Changed**: Both methods now return `object` instead of `Dictionary<string, Dictionary<string, string>>`
2. **Null Grouping Support**: When `groupingPropertyName` is `null`, the methods return a flat `Dictionary<string, string>`
3. **Backward Compatible**: When `groupingPropertyName` is provided, the methods return the original nested `Dictionary<string, Dictionary<string, string>>`

## Method Signatures

### 1. CreateGroupedDictionary (Direct List)

```csharp
public object CreateGroupedDictionary<T>(
    string keyPropertyName,
    string valuePropertyName,
    string groupingPropertyName,  // Can be null!
    List<T> items)
```

**Returns:**
- `Dictionary<string, string>` when `groupingPropertyName` is `null`
- `Dictionary<string, Dictionary<string, string>>` when `groupingPropertyName` is provided

### 2. CreateGroupedDictionaryFromAppState

```csharp
public object CreateGroupedDictionaryFromAppState(
    string appStateMethodName,
    string keyPropertyName,
    string valuePropertyName,
    string groupingPropertyName,  // Can be null!
    params object[] methodParameters)
```

**Returns:**
- `Dictionary<string, string>` when `groupingPropertyName` is `null`
- `Dictionary<string, Dictionary<string, string>>` when `groupingPropertyName` is provided

## Usage Examples

### Example 1: Flat Dictionary (No Grouping)

```csharp
public class Product
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
}

var products = new List<Product>
{
    new Product { Id = "1", Name = "Laptop", Category = "Electronics" },
    new Product { Id = "2", Name = "Mouse", Category = "Electronics" },
    new Product { Id = "3", Name = "Desk", Category = "Furniture" }
};

// Create a flat dictionary: Id -> Name (no grouping)
var result = CreateGroupedDictionary(
    keyPropertyName: "Id",
    valuePropertyName: "Name",
    groupingPropertyName: null,  // NULL = flat dictionary
    items: products
);

// Cast to the appropriate type
var flatDict = (Dictionary<string, string>)result;

// Result:
// {
//     "1" => "Laptop",
//     "2" => "Mouse",
//     "3" => "Desk"
// }
```

### Example 2: Nested Dictionary (With Grouping)

```csharp
// Create a nested dictionary grouped by Category
var result = CreateGroupedDictionary(
    keyPropertyName: "Id",
    valuePropertyName: "Name",
    groupingPropertyName: "Category",  // Group by Category
    items: products
);

// Cast to the appropriate type
var nestedDict = (Dictionary<string, Dictionary<string, string>>)result;

// Result:
// {
//     "Electronics" => {
//         "1" => "Laptop",
//         "2" => "Mouse"
//     },
//     "Furniture" => {
//         "3" => "Desk"
//     }
// }
```

### Example 3: Using AppState Method (Flat Dictionary)

```csharp
// Get all units of measure as a flat dictionary: UnitOfMeasureID -> Name
var result = CreateGroupedDictionaryFromAppState(
    appStateMethodName: "GetAllUnitOfMeasure",
    keyPropertyName: "UnitOfMeasureID",
    valuePropertyName: "Name",
    groupingPropertyName: null  // NULL = flat dictionary
);

var flatDict = (Dictionary<string, string>)result;

// Result:
// {
//     "mm" => "Millimeter",
//     "cm" => "Centimeter",
//     "m" => "Meter",
//     "kg" => "Kilogram"
// }
```

### Example 4: Using AppState Method (Nested Dictionary)

```csharp
// Get all units of measure grouped by TypeID
var result = CreateGroupedDictionaryFromAppState(
    appStateMethodName: "GetAllUnitOfMeasure",
    keyPropertyName: "UnitOfMeasureID",
    valuePropertyName: "Name",
    groupingPropertyName: "TypeID"  // Group by TypeID
);

var nestedDict = (Dictionary<string, Dictionary<string, string>>)result;

// Result:
// {
//     "1" => {  // Length measurements
//         "mm" => "Millimeter",
//         "cm" => "Centimeter",
//         "m" => "Meter"
//     },
//     "2" => {  // Weight measurements
//         "kg" => "Kilogram",
//         "g" => "Gram"
//     }
// }
```

### Example 5: Using AppState Method with Parameters

```csharp
// Get units of measure for specific calibration types (flat dictionary)
var calibrationTypes = new int[] { 1, 2, 3 };

var result = CreateGroupedDictionaryFromAppState(
    appStateMethodName: "GetUoMByTypes",
    keyPropertyName: "UnitOfMeasureID",
    valuePropertyName: "Name",
    groupingPropertyName: null,  // NULL = flat dictionary
    methodParameters: calibrationTypes
);

var flatDict = (Dictionary<string, string>)result;
```

## Best Practices

### 1. Type Casting

Since the return type is `object`, you need to cast to the appropriate dictionary type:

```csharp
// For flat dictionary
var flatDict = (Dictionary<string, string>)CreateGroupedDictionary(...);

// For nested dictionary
var nestedDict = (Dictionary<string, Dictionary<string, string>>)CreateGroupedDictionary(...);
```

### 2. Safe Casting with Pattern Matching

```csharp
var result = CreateGroupedDictionary(
    keyPropertyName: "Id",
    valuePropertyName: "Name",
    groupingPropertyName: groupBy,  // Could be null or a property name
    items: products
);

// Use pattern matching for safe casting
if (result is Dictionary<string, string> flatDict)
{
    // Handle flat dictionary
    foreach (var kvp in flatDict)
    {
        Console.WriteLine($"{kvp.Key}: {kvp.Value}");
    }
}
else if (result is Dictionary<string, Dictionary<string, string>> nestedDict)
{
    // Handle nested dictionary
    foreach (var group in nestedDict)
    {
        Console.WriteLine($"Group: {group.Key}");
        foreach (var kvp in group.Value)
        {
            Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
        }
    }
}
```

### 3. Helper Method for Type Detection

```csharp
public bool IsGroupedDictionary(object result)
{
    return result is Dictionary<string, Dictionary<string, string>>;
}

public bool IsFlatDictionary(object result)
{
    return result is Dictionary<string, string>;
}
```

## When to Use Each Approach

### Use Flat Dictionary (groupingPropertyName = null) when:
- ✅ You need a simple key-value lookup
- ✅ All keys are unique across the entire dataset
- ✅ No grouping or categorization is needed
- ✅ You're populating dropdown lists or select options
- ✅ You're creating simple lookup tables

### Use Nested Dictionary (groupingPropertyName provided) when:
- ✅ You need to group items by a category or type
- ✅ You want to organize data hierarchically
- ✅ You need to process items within each group separately
- ✅ You're creating cascading dropdowns (e.g., Category -> Products)
- ✅ You're building grouped reports or summaries

## Error Handling

Both methods throw exceptions for invalid inputs:

```csharp
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
    // Property 'InvalidProperty' not found on type 'Product'
    Console.WriteLine(ex.Message);
}
catch (ArgumentNullException ex)
{
    // Items list is null
    Console.WriteLine(ex.Message);
}
```

## Migration Guide

### Before (Old Code):
```csharp
// Old code always returned nested dictionary
var result = CreateGroupedDictionary(
    keyPropertyName: "Id",
    valuePropertyName: "Name",
    groupingPropertyName: "Category",
    items: products
);
// result was Dictionary<string, Dictionary<string, string>>
```

### After (New Code):
```csharp
// New code returns object, needs casting
var result = CreateGroupedDictionary(
    keyPropertyName: "Id",
    valuePropertyName: "Name",
    groupingPropertyName: "Category",
    items: products
);
var nestedDict = (Dictionary<string, Dictionary<string, string>>)result;

// OR use the new flat dictionary feature
var flatResult = CreateGroupedDictionary(
    keyPropertyName: "Id",
    valuePropertyName: "Name",
    groupingPropertyName: null,  // NEW: null for flat dictionary
    items: products
);
var flatDict = (Dictionary<string, string>)flatResult;
```

## Notes

1. **Duplicate Keys**: If duplicate keys exist, the last value will overwrite previous ones
2. **Null Items**: Null items in the list are automatically skipped
3. **Empty Results**: Empty lists return empty dictionaries of the appropriate type
4. **Case Sensitivity**: Property names are case-sensitive
5. **Performance**: Both methods use reflection, so cache results when possible for frequently accessed data

## Complete Working Example

See `CreateGroupedDictionary_Examples.cs` for a complete working example with all scenarios.

