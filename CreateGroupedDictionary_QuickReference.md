# CreateGroupedDictionary - Quick Reference Card

## 🚀 **Quick Start**

### **Flat Dictionary (No Grouping)**
```csharp
var result = CreateGroupedDictionary(
    keyPropertyName: "Id",
    valuePropertyName: "Name",
    groupingPropertyName: null,  // ← NULL for flat dictionary
    items: myList
);
var dict = (Dictionary<string, string>)result;
```

### **Nested Dictionary (With Grouping)**
```csharp
var result = CreateGroupedDictionary(
    keyPropertyName: "Id",
    valuePropertyName: "Name",
    groupingPropertyName: "Category",  // ← Property name for grouping
    items: myList
);
var dict = (Dictionary<string, Dictionary<string, string>>)result;
```

---

## 📋 **Method Signatures**

### **CreateGroupedDictionary**
```csharp
public object CreateGroupedDictionary<T>(
    string keyPropertyName,      // Property for dictionary key
    string valuePropertyName,    // Property for dictionary value
    string groupingPropertyName, // Property for grouping (null = flat)
    List<T> items)               // List of items to process
```

### **CreateGroupedDictionaryFromAppState**
```csharp
public object CreateGroupedDictionaryFromAppState(
    string appStateMethodName,   // AppState method to call
    string keyPropertyName,      // Property for dictionary key
    string valuePropertyName,    // Property for dictionary value
    string groupingPropertyName, // Property for grouping (null = flat)
    params object[] methodParameters) // Optional method parameters
```

---

## 🎯 **Return Types**

| groupingPropertyName | Return Type |
|---------------------|-------------|
| `null` or `""` | `Dictionary<string, string>` |
| Property name | `Dictionary<string, Dictionary<string, string>>` |

---

## 💡 **Common Use Cases**

### **1. Dropdown List (Flat)**
```csharp
// Get all units: ID -> Name
var units = CreateGroupedDictionary(
    "UnitOfMeasureID", "Name", null, unitList
);
var dropdown = (Dictionary<string, string>)units;
```

### **2. Cascading Dropdowns (Nested)**
```csharp
// Get units grouped by type: TypeID -> (ID -> Name)
var unitsByType = CreateGroupedDictionary(
    "UnitOfMeasureID", "Name", "TypeID", unitList
);
var cascading = (Dictionary<string, Dictionary<string, string>>)unitsByType;
```

### **3. Lookup Table (Flat)**
```csharp
// Get equipment: EquipmentID -> EquipmentName
var equipment = CreateGroupedDictionary(
    "EquipmentID", "EquipmentName", null, equipmentList
);
var lookup = (Dictionary<string, string>)equipment;
```

### **4. Grouped Data (Nested)**
```csharp
// Get products by category: Category -> (ProductID -> ProductName)
var productsByCategory = CreateGroupedDictionary(
    "ProductID", "ProductName", "Category", productList
);
var grouped = (Dictionary<string, Dictionary<string, string>>)productsByCategory;
```

---

## 🔍 **Pattern Matching (Recommended)**

```csharp
var result = CreateGroupedDictionary(key, value, groupBy, items);

if (result is Dictionary<string, string> flat)
{
    // Use flat dictionary
    foreach (var kvp in flat)
        Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}
else if (result is Dictionary<string, Dictionary<string, string>> nested)
{
    // Use nested dictionary
    foreach (var group in nested)
    {
        Console.WriteLine($"Group: {group.Key}");
        foreach (var kvp in group.Value)
            Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
    }
}
```

---

## ⚡ **Quick Examples**

### **Example 1: Simple Lookup**
```csharp
// Input: List<Product> with Id, Name, Category
// Output: Dictionary<string, string> { "1" => "Laptop", "2" => "Mouse" }

var lookup = (Dictionary<string, string>)CreateGroupedDictionary(
    "Id", "Name", null, products
);
```

### **Example 2: Grouped by Category**
```csharp
// Input: List<Product> with Id, Name, Category
// Output: Dictionary<string, Dictionary<string, string>>
//   { "Electronics" => { "1" => "Laptop", "2" => "Mouse" },
//     "Furniture" => { "3" => "Desk" } }

var grouped = (Dictionary<string, Dictionary<string, string>>)CreateGroupedDictionary(
    "Id", "Name", "Category", products
);
```

### **Example 3: From AppState**
```csharp
// Get all units from AppState as flat dictionary
var units = (Dictionary<string, string>)CreateGroupedDictionaryFromAppState(
    "GetAllUnitOfMeasure", "UnitOfMeasureID", "Name", null
);
```

### **Example 4: From AppState with Grouping**
```csharp
// Get units from AppState grouped by type
var unitsByType = (Dictionary<string, Dictionary<string, string>>)
    CreateGroupedDictionaryFromAppState(
        "GetAllUnitOfMeasure", "UnitOfMeasureID", "Name", "TypeID"
    );
```

---

## ⚠️ **Important Rules**

1. **Always cast the result** to the appropriate dictionary type
2. **`null` or `""` for groupingPropertyName** = flat dictionary
3. **Property name for groupingPropertyName** = nested dictionary
4. **Duplicate keys**: Last value wins
5. **Null items**: Automatically skipped
6. **Property names**: Case-sensitive

---

## 🎨 **Decision Tree**

```
Do you need grouping?
│
├─ NO  → Use groupingPropertyName: null
│        Cast to: Dictionary<string, string>
│        Use for: Dropdowns, lookups, simple key-value pairs
│
└─ YES → Use groupingPropertyName: "PropertyName"
         Cast to: Dictionary<string, Dictionary<string, string>>
         Use for: Cascading dropdowns, grouped data, hierarchical structures
```

---

## 📊 **Comparison Table**

| Feature | Flat Dictionary | Nested Dictionary |
|---------|----------------|-------------------|
| **groupingPropertyName** | `null` or `""` | Property name |
| **Return Type** | `Dictionary<string, string>` | `Dictionary<string, Dictionary<string, string>>` |
| **Use Case** | Simple lookups | Grouped/hierarchical data |
| **Example** | ID → Name | Category → (ID → Name) |
| **Complexity** | Simple | More complex |

---

## 🛠️ **Helper Methods**

```csharp
// Check if result is flat dictionary
public bool IsFlatDictionary(object result)
{
    return result is Dictionary<string, string>;
}

// Check if result is nested dictionary
public bool IsNestedDictionary(object result)
{
    return result is Dictionary<string, Dictionary<string, string>>;
}

// Safe cast to flat dictionary
public Dictionary<string, string> ToFlatDictionary(object result)
{
    return result as Dictionary<string, string> 
        ?? throw new InvalidCastException("Result is not a flat dictionary");
}

// Safe cast to nested dictionary
public Dictionary<string, Dictionary<string, string>> ToNestedDictionary(object result)
{
    return result as Dictionary<string, Dictionary<string, string>> 
        ?? throw new InvalidCastException("Result is not a nested dictionary");
}
```

---

## 🎯 **Best Practices**

1. **Use pattern matching** for type-safe handling
2. **Cache results** when possible (reflection is expensive)
3. **Validate property names** before calling
4. **Handle empty results** gracefully
5. **Document the expected return type** in your code comments

---

## 📝 **Code Template**

```csharp
// Template for using CreateGroupedDictionary
public void MyMethod()
{
    // 1. Prepare your data
    var items = GetMyItems();
    
    // 2. Decide if you need grouping
    string groupBy = needsGrouping ? "CategoryProperty" : null;
    
    // 3. Call the method
    var result = CreateGroupedDictionary(
        keyPropertyName: "IdProperty",
        valuePropertyName: "NameProperty",
        groupingPropertyName: groupBy,
        items: items
    );
    
    // 4. Use pattern matching to handle the result
    if (result is Dictionary<string, string> flatDict)
    {
        // Handle flat dictionary
        ProcessFlatDictionary(flatDict);
    }
    else if (result is Dictionary<string, Dictionary<string, string>> nestedDict)
    {
        // Handle nested dictionary
        ProcessNestedDictionary(nestedDict);
    }
}
```

---

## 🔗 **Related Documentation**

- **Full Documentation**: `CreateGroupedDictionary_Updated_Documentation.md`
- **Working Examples**: `CreateGroupedDictionary_Examples.cs`
- **Unit Tests**: `CreateGroupedDictionary_Tests.cs`
- **Summary**: `CreateGroupedDictionary_SUMMARY.md`

---

## ✅ **Checklist**

Before using the method, make sure:
- [ ] You know which properties to use for key, value, and grouping
- [ ] You've decided if you need grouping (null vs property name)
- [ ] You're ready to cast the result to the appropriate type
- [ ] You have error handling in place
- [ ] You understand that duplicate keys will be overwritten

---

**Quick Tip**: When in doubt, use pattern matching! It's the safest way to handle the dynamic return type.

