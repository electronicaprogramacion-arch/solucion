# SelectFilter JSON Configuration Documentation

## Overview
The `SelectFilter` class provides a JSON-configurable way to use the `CreateGroupedDictionaryFromAppState` method. This allows for dynamic configuration of how data is retrieved from AppState and grouped into nested dictionaries.

## SelectFilter Class Structure
```csharp
public class SelectFilter 
{
    public string Filter { get; set; }    // Optional parameters for the AppState method
    public string Key { get; set; }       // Property name for dictionary keys
    public string Value { get; set; }     // Property name for dictionary values
    public string Group { get; set; }     // Property name for grouping
    public string Method { get; set; }    // AppState method name to call
}
```

## Property Descriptions

### Method
- **Purpose**: Specifies the AppState method to call
- **Type**: String
- **Examples**: `"GetAllUnitOfMeasure"`, `"GetUoMByTypes"`, `"GetUoMByEquipmentType"`
- **Required**: Yes

### Key
- **Purpose**: Property name from the returned objects to use as dictionary keys
- **Type**: String
- **Examples**: `"UnitOfMeasureID"`, `"EquipmentTypeID"`, `"StandardID"`
- **Required**: Yes

### Value
- **Purpose**: Property name from the returned objects to use as dictionary values
- **Type**: String
- **Examples**: `"Name"`, `"Abbreviation"`, `"Description"`
- **Required**: Yes

### Group
- **Purpose**: Property name from the returned objects to use for grouping (outer dictionary key)
- **Type**: String
- **Examples**: `"TypeID"`, `"CalibrationTypeID"`, `"Category"`
- **Required**: Yes

### Filter
- **Purpose**: Optional parameters to pass to the AppState method (JSON serialized)
- **Type**: String (JSON)
- **Examples**: `""` (empty), `"[1,2,3]"` (array), `"{\"EquipmentTypeID\":5}"` (object)
- **Required**: No

## JSON Examples

### 1. Basic Usage (No Parameters)
```json
{
  "Filter": "",
  "Key": "UnitOfMeasureID",
  "Value": "Name",
  "Group": "TypeID",
  "Method": "GetAllUnitOfMeasure"
}
```
**Result**: Groups all units of measure by TypeID, using UnitOfMeasureID as key and Name as value.

### 2. Array Parameters
```json
{
  "Filter": "[1,2,3]",
  "Key": "UnitOfMeasureID",
  "Value": "Abbreviation",
  "Group": "TypeID",
  "Method": "GetUoMByTypes"
}
```
**Result**: Groups units for calibration types 1, 2, and 3, using abbreviations as values.

### 3. Object Parameters
```json
{
  "Filter": "{\"EquipmentTypeID\":5,\"Name\":\"Pressure Gauge\"}",
  "Key": "UnitOfMeasureID",
  "Value": "Name",
  "Group": "TypeID",
  "Method": "GetUoMByEquipmentType"
}
```
**Result**: Groups units for a specific equipment type.

### 4. Alternative Grouping
```json
{
  "Filter": "",
  "Key": "TypeID",
  "Value": "UnitOfMeasureID",
  "Group": "Name",
  "Method": "GetAllUnitOfMeasure"
}
```
**Result**: Groups by unit name, showing TypeID → UnitOfMeasureID mappings.

## Implementation Patterns

### Basic Implementation
```csharp
// Deserialize SelectFilter from JSON
var selectFilter = JsonConvert.DeserializeObject<SelectFilter>(jsonString);

// Call the method
var result = CreateGroupedDictionaryFromAppState(
    appStateMethodName: selectFilter.Method,
    keyPropertyName: selectFilter.Key,
    valuePropertyName: selectFilter.Value,
    groupingPropertyName: selectFilter.Group
);
```

### With Parameters
```csharp
var selectFilter = JsonConvert.DeserializeObject<SelectFilter>(jsonString);

// Parse parameters from Filter property
object[] methodParameters = null;
if (!string.IsNullOrEmpty(selectFilter.Filter))
{
    // For array parameters
    if (selectFilter.Filter.StartsWith("["))
    {
        var arrayParams = JsonConvert.DeserializeObject<int[]>(selectFilter.Filter);
        methodParameters = new object[] { arrayParams };
    }
    // For object parameters
    else if (selectFilter.Filter.StartsWith("{"))
    {
        var objectParam = JsonConvert.DeserializeObject<EquipmentType>(selectFilter.Filter);
        methodParameters = new object[] { objectParam };
    }
}

var result = CreateGroupedDictionaryFromAppState(
    appStateMethodName: selectFilter.Method,
    keyPropertyName: selectFilter.Key,
    valuePropertyName: selectFilter.Value,
    groupingPropertyName: selectFilter.Group,
    methodParameters: methodParameters
);
```

## Real-World Usage Scenarios

### 1. Dynamic UI Dropdowns
Configure dropdown lists that are automatically grouped by categories:
```json
{
  "Filter": "",
  "Key": "UnitOfMeasureID",
  "Value": "Abbreviation",
  "Group": "TypeName",
  "Method": "GetAllUnitOfMeasure"
}
```

### 2. Filtered Equipment Selection
Filter and group equipment by specific criteria:
```json
{
  "Filter": "{\"CalibrationTypeID\":2}",
  "Key": "EquipmentID",
  "Value": "SerialNumber",
  "Group": "ManufacturerName",
  "Method": "GetEquipmentByType"
}
```

### 3. Standards Selection
Group available standards by classification:
```json
{
  "Filter": "",
  "Key": "StandardID",
  "Value": "Description",
  "Group": "Classification",
  "Method": "GetAvailableStandards"
}
```

## Expected Output Format
All configurations produce the same nested dictionary structure:
```json
{
  "GroupValue1": {
    "Key1": "Value1",
    "Key2": "Value2"
  },
  "GroupValue2": {
    "Key3": "Value3",
    "Key4": "Value4"
  }
}
```

## Error Handling
The implementation should handle:
- Invalid JSON in SelectFilter
- Non-existent AppState methods
- Invalid property names
- Malformed Filter parameters
- Empty or null results

## Best Practices

1. **Validate JSON**: Always validate SelectFilter JSON before processing
2. **Handle Errors**: Implement try-catch blocks for robust error handling
3. **Parameter Types**: Ensure Filter parameters match expected method signatures
4. **Property Names**: Verify property names exist on returned object types
5. **Performance**: Cache results when possible for frequently used configurations

## Integration with Existing Code
This JSON configuration integrates seamlessly with the existing GenericTestComponent implementation:
```csharp
// From GenericTestComponentBase.cs line 764
var sf = JsonConvert.DeserializeObject<SelectFilter>(dyn[yi]?.ViewPropertyBase?.SelectOptions);

var result = CreateGroupedDictionaryFromAppState(
    appStateMethodName: sf.Method,
    keyPropertyName: sf.Key,
    valuePropertyName: sf.Value,
    groupingPropertyName: sf.Group
);

var optionsJson = JsonConvert.SerializeObject(result);
dyn[yi].ViewPropertyBase.SelectOptions = optionsJson;
```

## Available AppState Methods
Common methods that can be used in the Method property:
- `GetAllUnitOfMeasure()` - No parameters
- `GetUoMByTypes(int[] calibrationTypes)` - Array parameter
- `GetUoMByCalibrationTypeObj(CalibrationType calibrationType)` - Object parameter
- `GetUoMByEquipmentType(EquipmentType equipmentType)` - Object parameter
- Any other AppState method returning `List<T>` or `IEnumerable<T>`
