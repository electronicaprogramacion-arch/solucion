# 📚 JSON to Dictionary Deserializer - Documentación Completa

## 🎯 **Descripción General**

La clase `JsonToDictionaryDeserializer` proporciona múltiples métodos para deserializar JSON en `Dictionary<string, object>` con diferentes opciones y configuraciones. Es especialmente útil para:

- **Procesamiento de APIs REST** que devuelven JSON
- **Configuración dinámica** desde archivos JSON
- **Almacenamiento flexible** en bases de datos NoSQL
- **Transformación de datos** para diferentes sistemas
- **Validación y análisis** de estructuras JSON

## 🔧 **Métodos Disponibles**

### 1. **`DeserializeWithNewtonsoft()`** - Deserialización con Newtonsoft.Json
```csharp
Dictionary<string, object> DeserializeWithNewtonsoft(
    string json, 
    JsonSerializerSettings? settings = null)
```

**Descripción**: Deserializa JSON usando la librería Newtonsoft.Json (Json.NET).

**Ventajas**:
- Manejo robusto de tipos complejos
- Configuración flexible de serialización
- Mejor manejo de fechas y tipos especiales

**Ejemplo**:
```csharp
var json = @"{""name"": ""Juan"", ""age"": 30, ""isActive"": true}";
var dict = JsonToDictionaryDeserializer.DeserializeWithNewtonsoft(json);
// Resultado: { "name": "Juan", "age": 30, "isActive": true }
```

### 2. **`DeserializeWithSystemTextJson()`** - Deserialización con System.Text.Json
```csharp
Dictionary<string, object> DeserializeWithSystemTextJson(
    string json, 
    JsonSerializerOptions? options = null)
```

**Descripción**: Deserializa JSON usando la librería nativa System.Text.Json de .NET.

**Ventajas**:
- Mayor rendimiento
- Menor uso de memoria
- Nativo en .NET Core/5+

**Ejemplo**:
```csharp
var json = @"{""deviceId"": ""TEMP-001"", ""temperature"": 25.5}";
var dict = JsonToDictionaryDeserializer.DeserializeWithSystemTextJson(json);
// Resultado: { "deviceId": "TEMP-001", "temperature": 25.5 }
```

### 3. **`DeserializeToFlatDictionary()`** - Diccionario Plano
```csharp
Dictionary<string, object> DeserializeToFlatDictionary(
    string json, 
    string prefix = "")
```

**Descripción**: Deserializa JSON anidado a un diccionario plano usando notación de puntos.

**Uso ideal**: Para almacenamiento en bases de datos relacionales o sistemas que no soportan objetos anidados.

**Ejemplo**:
```csharp
var json = @"{
    ""device"": {
        ""id"": ""TEMP-001"",
        ""location"": {
            ""building"": ""Lab A"",
            ""room"": ""101""
        }
    }
}";
var flatDict = JsonToDictionaryDeserializer.DeserializeToFlatDictionary(json);
// Resultado: 
// { 
//   "device.id": "TEMP-001", 
//   "device.location.building": "Lab A", 
//   "device.location.room": "101" 
// }
```

### 4. **`DeserializeToStringDictionary()`** - Diccionario de Strings
```csharp
Dictionary<string, string> DeserializeToStringDictionary(
    string json, 
    bool includeNullValues = true)
```

**Descripción**: Deserializa JSON a `Dictionary<string, string>` con todos los valores como strings.

**Uso ideal**: Para mostrar datos en UI, logging, o cuando necesitas todos los valores como texto.

**Ejemplo**:
```csharp
var json = @"{""temperature"": 25.5, ""isValid"": true, ""date"": ""2025-09-16""}";
var stringDict = JsonToDictionaryDeserializer.DeserializeToStringDictionary(json);
// Resultado: { "temperature": "25.5", "isValid": "true", "date": "2025-09-16" }
```

### 5. **`DeserializeWithTypePreservation()`** - Preservación de Tipos
```csharp
Dictionary<string, object> DeserializeWithTypePreservation(string json)
```

**Descripción**: Deserializa JSON intentando preservar los tipos originales de datos.

**Uso ideal**: Cuando necesitas mantener la información de tipos para procesamiento posterior.

**Ejemplo**:
```csharp
var json = @"{""intValue"": 42, ""doubleValue"": 3.14, ""stringValue"": ""Hello""}";
var typedDict = JsonToDictionaryDeserializer.DeserializeWithTypePreservation(json);
// Preserva int, double, string como tipos específicos
```

### 6. **`DeserializeJsonArray()`** - Arrays JSON
```csharp
List<Dictionary<string, object>> DeserializeJsonArray(string json)
```

**Descripción**: Deserializa un array JSON a una lista de diccionarios.

**Uso ideal**: Para procesar listas de objetos JSON como datos tabulares.

**Ejemplo**:
```csharp
var json = @"[
    {""id"": 1, ""name"": ""Device A""},
    {""id"": 2, ""name"": ""Device B""}
]";
var arrayOfDicts = JsonToDictionaryDeserializer.DeserializeJsonArray(json);
// Resultado: Lista con 2 diccionarios
```

### 7. **`DeserializeWithPropertyMapping()`** - Mapeo de Propiedades
```csharp
Dictionary<string, object> DeserializeWithPropertyMapping(
    string json,
    Func<string, string> propertyNameMapper)
```

**Descripción**: Deserializa JSON con mapeo personalizado de nombres de propiedades.

**Uso ideal**: Para convertir entre diferentes convenciones de nomenclatura.

**Ejemplo**:
```csharp
var json = @"{""DeviceId"": ""TEMP-001"", ""CalibrationDate"": ""2025-09-16""}";
var camelCaseDict = JsonToDictionaryDeserializer.DeserializeWithPropertyMapping(
    json, 
    name => char.ToLowerInvariant(name[0]) + name.Substring(1)
);
// Resultado: { "deviceId": "TEMP-001", "calibrationDate": "2025-09-16" }
```

### 8. **`DeserializeWithValidation()`** - Validación y Deserialización
```csharp
Dictionary<string, object> DeserializeWithValidation(string json)
```

**Descripción**: Valida y deserializa JSON incluyendo información de validación.

**Uso ideal**: Cuando necesitas verificar la validez del JSON antes de procesarlo.

**Ejemplo**:
```csharp
var json = @"{""temperature"": 25.5}";
var result = JsonToDictionaryDeserializer.DeserializeWithValidation(json);
// Resultado: 
// { 
//   "IsValid": true, 
//   "JsonType": "Object", 
//   "PropertyCount": 1,
//   "Data": { "temperature": 25.5 }
// }
```

## 🎯 **Casos de Uso Específicos para CalibrationSaaS**

### **1. Configuración de Dispositivos desde JSON**
```csharp
// JSON de configuración de dispositivo
var deviceConfigJson = GetDeviceConfigFromApi();
var config = JsonToDictionaryDeserializer.DeserializeWithNewtonsoft(deviceConfigJson);

// Acceder a configuraciones específicas
var deviceId = config["deviceId"].ToString();
var parameters = (Dictionary<string, object>)config["parameters"];
```

### **2. Resultados de Calibración desde API**
```csharp
// JSON array de resultados de calibración
var calibrationResultsJson = GetCalibrationResultsFromApi();
var results = JsonToDictionaryDeserializer.DeserializeJsonArray(calibrationResultsJson);

// Procesar cada resultado
foreach (var result in results)
{
    var measurementId = result["measurementId"].ToString();
    var value = Convert.ToDouble(result["value"]);
    var withinTolerance = Convert.ToBoolean(result["withinTolerance"]);
}
```

### **3. Almacenamiento Plano en Base de Datos**
```csharp
// JSON complejo para almacenamiento en tabla SQL
var complexJson = GetComplexCalibrationData();
var flatData = JsonToDictionaryDeserializer.DeserializeToFlatDictionary(complexJson);

// Insertar en tabla con columnas dinámicas
foreach (var kvp in flatData)
{
    InsertKeyValuePair(kvp.Key, kvp.Value);
}
```

### **4. Validación de Datos de Entrada**
```csharp
// Validar JSON antes de procesamiento
var inputJson = GetUserInputJson();
var validation = JsonToDictionaryDeserializer.DeserializeWithValidation(inputJson);

if ((bool)validation["IsValid"])
{
    var data = (Dictionary<string, object>)validation["Data"];
    ProcessValidData(data);
}
else
{
    LogError($"Invalid JSON: {validation["Error"]}");
}
```

### **5. Conversión para UI**
```csharp
// Convertir datos JSON para mostrar en interfaz
var deviceStatusJson = GetDeviceStatusJson();
var displayData = JsonToDictionaryDeserializer.DeserializeToStringDictionary(deviceStatusJson);

// Mostrar en UI como texto
foreach (var kvp in displayData)
{
    AddToUIGrid(kvp.Key, kvp.Value);
}
```

## ⚡ **Características Especiales**

### **✅ Manejo de Errores Robusto**
- Captura excepciones de parsing JSON
- Devuelve diccionarios con información de error
- Continúa funcionando con JSON malformado

### **✅ Tipos Soportados**
- **Primitivos**: int, double, string, bool
- **Fechas**: DateTime con diferentes formatos
- **Objetos anidados**: Conversión recursiva
- **Arrays**: Conversión a object[] o List<>
- **Valores null**: Manejo explícito de nulls

### **✅ Flexibilidad de Configuración**
- Configuraciones personalizadas de serialización
- Mapeo de nombres de propiedades
- Inclusión/exclusión de valores null
- Preservación o conversión de tipos

### **✅ Rendimiento Optimizado**
- Soporte para System.Text.Json (más rápido)
- Soporte para Newtonsoft.Json (más flexible)
- Manejo eficiente de memoria
- Procesamiento de arrays grandes

## 🚀 **Instalación y Uso**

1. **Agregar el archivo** `JsonToDictionaryDeserializer.cs` a tu proyecto
2. **Instalar dependencias**:
   ```bash
   # Para Newtonsoft.Json
   dotnet add package Newtonsoft.Json
   
   # System.Text.Json viene incluido en .NET Core 3.0+
   ```
3. **Usar los métodos**:
   ```csharp
   using CalibrationSaaS.Utilities;
   
   var json = GetJsonFromSomewhere();
   var dictionary = JsonToDictionaryDeserializer.DeserializeWithNewtonsoft(json);
   ```

## 📊 **Comparación de Métodos**

| Método | Objetos Anidados | Rendimiento | Flexibilidad | Uso Recomendado |
|--------|------------------|-------------|--------------|-----------------|
| `DeserializeWithNewtonsoft()` | ✅ | ⭐⭐ | ⭐⭐⭐ | General, APIs complejas |
| `DeserializeWithSystemTextJson()` | ✅ | ⭐⭐⭐ | ⭐⭐ | Alto rendimiento |
| `DeserializeToFlatDictionary()` | ✅→Plano | ⭐⭐ | ⭐⭐ | BD relacionales |
| `DeserializeToStringDictionary()` | ❌ | ⭐⭐⭐ | ⭐ | UI, logging |
| `DeserializeWithTypePreservation()` | ✅ | ⭐⭐ | ⭐⭐⭐ | Análisis de tipos |
| `DeserializeJsonArray()` | ✅ | ⭐⭐ | ⭐⭐ | Listas de objetos |
| `DeserializeWithPropertyMapping()` | ✅ | ⭐⭐ | ⭐⭐⭐ | Mapeo de nombres |
| `DeserializeWithValidation()` | ✅ | ⭐⭐ | ⭐⭐⭐ | Validación de entrada |

## 🔍 **Ejemplos de JSON Soportados**

### **JSON Simple**
```json
{
  "deviceId": "TEMP-001",
  "temperature": 25.5,
  "isActive": true,
  "lastCalibration": "2025-09-16T10:30:00"
}
```

### **JSON Anidado**
```json
{
  "device": {
    "id": "MULTI-001",
    "parameters": {
      "temperature": {"min": -40, "max": 85},
      "pressure": {"min": 0, "max": 2000}
    }
  }
}
```

### **JSON Array**
```json
[
  {"id": 1, "value": 25.1},
  {"id": 2, "value": 25.2},
  {"id": 3, "value": 25.0}
]
```

**¡El sistema está listo para deserializar cualquier JSON en diccionarios según tus necesidades específicas!** 🎉
