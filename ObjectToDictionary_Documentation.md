# 📚 Object to Dictionary Converter - Documentación Completa

## 🎯 **Descripción General**

La clase `ObjectToDictionaryConverter` proporciona múltiples métodos para convertir objetos C# en diccionarios con diferentes opciones y configuraciones. Es especialmente útil para:

- **Serialización de datos** para APIs
- **Almacenamiento en bases de datos** NoSQL
- **Logging y debugging** de objetos
- **Transformación de datos** para UI
- **Integración con sistemas externos**

## 🔧 **Métodos Disponibles**

### 1. **`ToDictionary()`** - Conversión Básica
```csharp
Dictionary<string, object> ToDictionary(
    this object obj, 
    bool includeNullValues = true, 
    bool includePrivateProperties = false)
```

**Descripción**: Convierte un objeto a `Dictionary<string, object>` usando reflexión.

**Parámetros**:
- `includeNullValues`: Incluir propiedades con valores null
- `includePrivateProperties`: Incluir propiedades privadas

**Ejemplo**:
```csharp
var person = new Person { Name = "Juan", Age = 30 };
var dict = person.ToDictionary();
// Resultado: { "Name": "Juan", "Age": 30 }
```

### 2. **`ToStringDictionary()`** - Diccionario de Strings
```csharp
Dictionary<string, string> ToStringDictionary(
    this object obj, 
    bool includeNullValues = true)
```

**Descripción**: Convierte un objeto a `Dictionary<string, string>` con todos los valores como strings.

**Uso ideal**: Para mostrar datos en UI, logging, o cuando necesitas todos los valores como texto.

**Ejemplo**:
```csharp
var calibration = new CalibrationData { Temperature = 25.5, IsValid = true };
var stringDict = calibration.ToStringDictionary();
// Resultado: { "Temperature": "25.5", "IsValid": "True" }
```

### 3. **`ToDictionaryViaJson()`** - Conversión via JSON
```csharp
Dictionary<string, object> ToDictionaryViaJson(
    this object obj, 
    JsonSerializerSettings settings = null)
```

**Descripción**: Convierte usando serialización JSON. Maneja objetos anidados complejos.

**Ventajas**: 
- Maneja objetos anidados automáticamente
- Respeta atributos de serialización JSON
- Maneja colecciones y arrays

**Ejemplo**:
```csharp
var person = new Person { 
    Name = "Ana", 
    Address = new Address { City = "Madrid" } 
};
var jsonDict = person.ToDictionaryViaJson();
// Maneja automáticamente el objeto Address anidado
```

### 4. **`ToFlatDictionary()`** - Diccionario Plano
```csharp
Dictionary<string, object> ToFlatDictionary(
    this object obj, 
    string prefix = "")
```

**Descripción**: Convierte objetos anidados a un diccionario plano usando notación de puntos.

**Uso ideal**: Para almacenamiento en bases de datos relacionales o sistemas que no soportan objetos anidados.

**Ejemplo**:
```csharp
var person = new Person { 
    Name = "Carlos", 
    Address = new Address { City = "Barcelona", ZipCode = 08001 } 
};
var flatDict = person.ToFlatDictionary();
// Resultado: 
// { 
//   "Name": "Carlos", 
//   "Address.City": "Barcelona", 
//   "Address.ZipCode": 08001 
// }
```

### 5. **`ToDictionaryWithMapping()`** - Mapeo Personalizado
```csharp
Dictionary<string, object> ToDictionaryWithMapping(
    this object obj,
    Func<string, string> propertyNameMapper)
```

**Descripción**: Convierte con mapeo personalizado de nombres de propiedades.

**Uso ideal**: Para convertir entre diferentes convenciones de nomenclatura (PascalCase ↔ camelCase).

**Ejemplo**:
```csharp
var obj = new { DeviceId = "DEV-001", CalibrationDate = DateTime.Now };
var camelCaseDict = obj.ToDictionaryWithMapping(name => 
    char.ToLowerInvariant(name[0]) + name.Substring(1));
// Resultado: { "deviceId": "DEV-001", "calibrationDate": "..." }
```

### 6. **`ToDictionaryWithTypes()`** - Con Información de Tipos
```csharp
Dictionary<string, object> ToDictionaryWithTypes(this object obj)
```

**Descripción**: Incluye información del tipo de cada propiedad junto con su valor.

**Uso ideal**: Para debugging, análisis de datos, o cuando necesitas metadatos de tipos.

**Ejemplo**:
```csharp
var measurement = new Measurement { Value = 25.7, Unit = "°C" };
var typedDict = measurement.ToDictionaryWithTypes();
// Resultado: 
// { 
//   "Value": { "Value": 25.7, "Type": "Double", "FullTypeName": "System.Double" },
//   "Unit": { "Value": "°C", "Type": "String", "FullTypeName": "System.String" }
// }
```

### 7. **`ToDictionaryExcluding()`** - Excluyendo Propiedades
```csharp
Dictionary<string, object> ToDictionaryExcluding(
    this object obj, 
    params string[] excludeProperties)
```

**Descripción**: Convierte excluyendo propiedades específicas.

**Uso ideal**: Para omitir información sensible o innecesaria.

**Ejemplo**:
```csharp
var person = new Person { Name = "Laura", Email = "laura@example.com", Salary = 60000 };
var publicDict = person.ToDictionaryExcluding("Email", "Salary");
// Resultado: { "Name": "Laura", "Age": 32, "IsActive": true }
```

### 8. **`ToDictionaryIncluding()`** - Solo Propiedades Específicas
```csharp
Dictionary<string, object> ToDictionaryIncluding(
    this object obj, 
    params string[] includeProperties)
```

**Descripción**: Convierte incluyendo solo las propiedades especificadas.

**Uso ideal**: Para crear vistas específicas de datos o DTOs.

**Ejemplo**:
```csharp
var calibration = new CalibrationData { 
    DeviceId = "DEV-001", 
    Temperature = 25.5, 
    Technician = "María" 
};
var essentialDict = calibration.ToDictionaryIncluding("DeviceId", "Temperature");
// Resultado: { "DeviceId": "DEV-001", "Temperature": 25.5 }
```

## 🎯 **Casos de Uso Específicos para CalibrationSaaS**

### **1. Para APIs REST**
```csharp
// Convertir datos de calibración para respuesta API
var calibrationData = GetCalibrationData();
var apiResponse = calibrationData.ToDictionaryExcluding("InternalNotes", "TechnicianId");
return Json(apiResponse);
```

### **2. Para Logging**
```csharp
// Log de objetos complejos
var measurement = GetMeasurement();
var logData = measurement.ToStringDictionary();
logger.LogInformation("Measurement data: {@Data}", logData);
```

### **3. Para Base de Datos NoSQL**
```csharp
// Almacenar en MongoDB o similar
var device = GetDeviceInfo();
var document = device.ToDictionaryViaJson();
await collection.InsertOneAsync(document);
```

### **4. Para Base de Datos Relacional (Flat)**
```csharp
// Almacenar objetos complejos en tabla plana
var calibration = GetCalibrationWithNested();
var flatData = calibration.ToFlatDictionary();
// Insertar flatData en tabla SQL
```

### **5. Para UI (Frontend)**
```csharp
// Datos para mostrar en interfaz
var deviceStatus = GetDeviceStatus();
var displayData = deviceStatus.ToStringDictionary(includeNullValues: false);
return Json(displayData);
```

## ⚡ **Características Especiales**

### **✅ Manejo de Errores**
- Captura excepciones al acceder propiedades
- Continúa procesando otras propiedades
- Incluye mensajes de error en el resultado

### **✅ Tipos Soportados**
- Tipos primitivos (int, string, bool, etc.)
- Tipos nullable (int?, DateTime?, etc.)
- Objetos anidados
- Colecciones y arrays
- Enums
- DateTime, Guid, Decimal

### **✅ Configuraciones Flexibles**
- Incluir/excluir valores null
- Incluir propiedades privadas
- Mapeo personalizado de nombres
- Filtrado de propiedades

### **✅ Rendimiento**
- Usa reflexión optimizada
- Caché de metadatos de tipos
- Manejo eficiente de memoria

## 🚀 **Instalación y Uso**

1. **Agregar el archivo** `ObjectToDictionaryConverter.cs` a tu proyecto
2. **Instalar Newtonsoft.Json** (para métodos JSON):
   ```bash
   dotnet add package Newtonsoft.Json
   ```
3. **Usar como extension methods**:
   ```csharp
   using CalibrationSaaS.Utilities;
   
   var myObject = new MyClass();
   var dictionary = myObject.ToDictionary();
   ```

## 📊 **Comparación de Métodos**

| Método | Objetos Anidados | Tipos de Salida | Rendimiento | Uso Recomendado |
|--------|------------------|-----------------|-------------|-----------------|
| `ToDictionary()` | ❌ | object | ⭐⭐⭐ | General, APIs |
| `ToStringDictionary()` | ❌ | string | ⭐⭐⭐ | UI, Logging |
| `ToDictionaryViaJson()` | ✅ | object | ⭐⭐ | Objetos complejos |
| `ToFlatDictionary()` | ✅ | object | ⭐⭐ | BD relacionales |
| `ToDictionaryWithMapping()` | ❌ | object | ⭐⭐⭐ | Mapeo de nombres |
| `ToDictionaryWithTypes()` | ❌ | object | ⭐⭐ | Debugging, análisis |
| `ToDictionaryExcluding()` | ❌ | object | ⭐⭐⭐ | Filtrado de datos |
| `ToDictionaryIncluding()` | ❌ | object | ⭐⭐⭐ | DTOs, vistas |

**¡El sistema está listo para convertir cualquier objeto en diccionario según tus necesidades específicas!** 🎉
