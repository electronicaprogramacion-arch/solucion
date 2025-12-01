using System;
using System.Collections.Generic;
using System.Linq;
using CalibrationSaaS.Utilities;
using Newtonsoft.Json;

namespace CalibrationSaaS.Examples
{
    /// <summary>
    /// Examples and test cases for ObjectToDictionaryConverter
    /// </summary>
    public class ObjectToDictionaryExamples
    {
        // Sample classes for testing
        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public string Email { get; set; }
            public DateTime BirthDate { get; set; }
            public Address Address { get; set; }
            public List<string> Hobbies { get; set; }
            public bool IsActive { get; set; }
            public decimal? Salary { get; set; }
        }

        public class Address
        {
            public string Street { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public int ZipCode { get; set; }
        }

        public class CalibrationData
        {
            public string DeviceId { get; set; }
            public DateTime CalibrationDate { get; set; }
            public double Temperature { get; set; }
            public double Pressure { get; set; }
            public string Technician { get; set; }
            public bool IsValid { get; set; }
            public List<Measurement> Measurements { get; set; }
        }

        public class Measurement
        {
            public string Parameter { get; set; }
            public double Value { get; set; }
            public string Unit { get; set; }
            public double Tolerance { get; set; }
        }

        /// <summary>
        /// Example 1: Basic object to dictionary conversion
        /// </summary>
        public static void Example1_BasicConversion()
        {
            Console.WriteLine("=== Example 1: Basic Object to Dictionary ===");

            var person = new Person
            {
                Name = "Juan Pérez",
                Age = 30,
                Email = "juan@example.com",
                BirthDate = new DateTime(1993, 5, 15),
                IsActive = true,
                Salary = 50000.50m
            };

            // Basic conversion
            var dictionary = person.ToDictionary();
            
            Console.WriteLine("Basic Dictionary:");
            foreach (var kvp in dictionary)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Example 2: String dictionary conversion
        /// </summary>
        public static void Example2_StringDictionary()
        {
            Console.WriteLine("=== Example 2: String Dictionary ===");

            var calibrationData = new CalibrationData
            {
                DeviceId = "CAL-001",
                CalibrationDate = DateTime.Now,
                Temperature = 25.5,
                Pressure = 1013.25,
                Technician = "María García",
                IsValid = true
            };

            // Convert to string dictionary
            var stringDict = calibrationData.ToStringDictionary();
            
            Console.WriteLine("String Dictionary:");
            foreach (var kvp in stringDict)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Example 3: Nested objects with JSON conversion
        /// </summary>
        public static void Example3_NestedObjectsJson()
        {
            Console.WriteLine("=== Example 3: Nested Objects via JSON ===");

            var person = new Person
            {
                Name = "Ana López",
                Age = 28,
                Email = "ana@example.com",
                Address = new Address
                {
                    Street = "Calle Principal 123",
                    City = "Madrid",
                    Country = "España",
                    ZipCode = 28001
                },
                Hobbies = new List<string> { "Lectura", "Natación", "Fotografía" }
            };

            // Convert using JSON serialization (handles nested objects)
            var jsonDict = person.ToDictionaryViaJson();
            
            Console.WriteLine("JSON Dictionary (handles nested objects):");
            foreach (var kvp in jsonDict)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Example 4: Flat dictionary (dot notation for nested properties)
        /// </summary>
        public static void Example4_FlatDictionary()
        {
            Console.WriteLine("=== Example 4: Flat Dictionary ===");

            var person = new Person
            {
                Name = "Carlos Ruiz",
                Age = 35,
                Address = new Address
                {
                    Street = "Avenida Libertad 456",
                    City = "Barcelona",
                    Country = "España",
                    ZipCode = 08001
                },
                Hobbies = new List<string> { "Fútbol", "Cocina" }
            };

            // Convert to flat dictionary with dot notation
            var flatDict = person.ToFlatDictionary();
            
            Console.WriteLine("Flat Dictionary (dot notation):");
            foreach (var kvp in flatDict.OrderBy(x => x.Key))
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Example 5: Dictionary with custom property name mapping
        /// </summary>
        public static void Example5_CustomMapping()
        {
            Console.WriteLine("=== Example 5: Custom Property Name Mapping ===");

            var calibrationData = new CalibrationData
            {
                DeviceId = "CAL-002",
                CalibrationDate = DateTime.Now,
                Temperature = 22.3,
                Pressure = 1015.8,
                Technician = "Pedro Martínez",
                IsValid = true
            };

            // Convert with custom property name mapping (camelCase)
            var mappedDict = calibrationData.ToDictionaryWithMapping(propName => 
                char.ToLowerInvariant(propName[0]) + propName.Substring(1));
            
            Console.WriteLine("Dictionary with camelCase mapping:");
            foreach (var kvp in mappedDict)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Example 6: Dictionary with type information
        /// </summary>
        public static void Example6_WithTypes()
        {
            Console.WriteLine("=== Example 6: Dictionary with Type Information ===");

            var measurement = new Measurement
            {
                Parameter = "Temperature",
                Value = 25.7,
                Unit = "°C",
                Tolerance = 0.1
            };

            // Convert with type information
            var typedDict = measurement.ToDictionaryWithTypes();
            
            Console.WriteLine("Dictionary with type information:");
            foreach (var kvp in typedDict)
            {
                Console.WriteLine($"  {kvp.Key}: {JsonConvert.SerializeObject(kvp.Value, Formatting.Indented)}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Example 7: Excluding specific properties
        /// </summary>
        public static void Example7_ExcludingProperties()
        {
            Console.WriteLine("=== Example 7: Excluding Properties ===");

            var person = new Person
            {
                Name = "Laura Fernández",
                Age = 32,
                Email = "laura@example.com",
                Salary = 60000m,
                IsActive = true
            };

            // Convert excluding sensitive information
            var filteredDict = person.ToDictionaryExcluding("Salary", "Email");
            
            Console.WriteLine("Dictionary excluding Salary and Email:");
            foreach (var kvp in filteredDict)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Example 8: Including only specific properties
        /// </summary>
        public static void Example8_IncludingProperties()
        {
            Console.WriteLine("=== Example 8: Including Only Specific Properties ===");

            var calibrationData = new CalibrationData
            {
                DeviceId = "CAL-003",
                CalibrationDate = DateTime.Now,
                Temperature = 24.1,
                Pressure = 1012.3,
                Technician = "Roberto Silva",
                IsValid = true
            };

            // Convert including only essential properties
            var essentialDict = calibrationData.ToDictionaryIncluding("DeviceId", "Temperature", "Pressure", "IsValid");
            
            Console.WriteLine("Dictionary with only essential properties:");
            foreach (var kvp in essentialDict)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Example 9: Complex calibration scenario
        /// </summary>
        public static void Example9_CalibrationScenario()
        {
            Console.WriteLine("=== Example 9: Complex Calibration Scenario ===");

            var calibrationData = new CalibrationData
            {
                DeviceId = "TEMP-SENSOR-001",
                CalibrationDate = DateTime.Now,
                Temperature = 23.5,
                Pressure = 1013.25,
                Technician = "Ing. Patricia Morales",
                IsValid = true,
                Measurements = new List<Measurement>
                {
                    new Measurement { Parameter = "Temperature", Value = 23.5, Unit = "°C", Tolerance = 0.1 },
                    new Measurement { Parameter = "Humidity", Value = 45.2, Unit = "%", Tolerance = 2.0 },
                    new Measurement { Parameter = "Pressure", Value = 1013.25, Unit = "hPa", Tolerance = 0.5 }
                }
            };

            // Different conversion approaches for calibration data
            Console.WriteLine("1. Basic Dictionary:");
            var basicDict = calibrationData.ToDictionary(includeNullValues: false);
            foreach (var kvp in basicDict.Where(x => x.Key != "Measurements"))
            {
                Console.WriteLine($"   {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine("\n2. Flat Dictionary (for database storage):");
            var flatDict = calibrationData.ToFlatDictionary();
            foreach (var kvp in flatDict.OrderBy(x => x.Key))
            {
                Console.WriteLine($"   {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine("\n3. String Dictionary (for UI display):");
            var stringDict = calibrationData.ToStringDictionary();
            foreach (var kvp in stringDict.Where(x => x.Key != "Measurements"))
            {
                Console.WriteLine($"   {kvp.Key}: {kvp.Value}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Run all examples
        /// </summary>
        public static void RunAllExamples()
        {
            Console.WriteLine("🔄 OBJECT TO DICTIONARY CONVERTER - EXAMPLES");
            Console.WriteLine("=============================================");
            Console.WriteLine();

            Example1_BasicConversion();
            Example2_StringDictionary();
            Example3_NestedObjectsJson();
            Example4_FlatDictionary();
            Example5_CustomMapping();
            Example6_WithTypes();
            Example7_ExcludingProperties();
            Example8_IncludingProperties();
            Example9_CalibrationScenario();

            Console.WriteLine("✅ All examples completed!");
        }
    }
}
