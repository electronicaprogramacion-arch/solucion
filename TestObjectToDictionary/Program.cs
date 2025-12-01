using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CalibrationSaaS.TestApp
{
    // Sample classes for testing
    public class Person
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public Address? Address { get; set; }
        public List<string>? Hobbies { get; set; }
        public bool IsActive { get; set; }
        public decimal? Salary { get; set; }
    }

    public class Address
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int ZipCode { get; set; }
    }

    public class CalibrationData
    {
        public string DeviceId { get; set; } = string.Empty;
        public DateTime CalibrationDate { get; set; }
        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public string Technician { get; set; } = string.Empty;
        public bool IsValid { get; set; }
    }

    /// <summary>
    /// Utility class for converting objects to dictionaries
    /// </summary>
    public static class ObjectToDictionaryConverter
    {
        /// <summary>
        /// Converts an object to Dictionary<string, object> using reflection
        /// </summary>
        public static Dictionary<string, object> ToObjectDictionary(this object obj)
        {
            if (obj == null)
                return new Dictionary<string, object>();

            var dictionary = new Dictionary<string, object>();
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (!property.CanRead)
                    continue;

                try
                {
                    var value = property.GetValue(obj);
                    dictionary[property.Name] = value;
                }
                catch (Exception ex)
                {
                    dictionary[property.Name] = $"Error: {ex.Message}";
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Converts an object to Dictionary<string, string>
        /// </summary>
        public static Dictionary<string, string> ToStringDictionary(this object obj)
        {
            if (obj == null)
                return new Dictionary<string, string>();

            var dictionary = new Dictionary<string, string>();
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (!property.CanRead)
                    continue;

                try
                {
                    var value = property.GetValue(obj);
                    dictionary[property.Name] = value?.ToString() ?? "null";
                }
                catch (Exception ex)
                {
                    dictionary[property.Name] = $"Error: {ex.Message}";
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Converts an object to dictionary using JSON serialization
        /// </summary>
        public static Dictionary<string, object?> ToDictionaryViaJson(this object obj)
        {
            if (obj == null)
                return new Dictionary<string, object?>();

            try
            {
                var json = JsonConvert.SerializeObject(obj);
                var jObject = JObject.Parse(json);
                return jObject.ToObject<Dictionary<string, object?>>() ?? new Dictionary<string, object?>();
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object?> { { "Error", ex.Message } };
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("🔄 OBJECT TO DICTIONARY CONVERTER - TEST APPLICATION");
            Console.WriteLine("====================================================");
            Console.WriteLine();

            // Test 1: Basic Person object
            TestBasicConversion();

            // Test 2: Calibration data
            TestCalibrationData();

            // Test 3: Nested objects
            TestNestedObjects();

            // Test 4: JSON conversion
            TestJsonConversion();

            Console.WriteLine("✅ All tests completed!");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void TestBasicConversion()
        {
            Console.WriteLine("=== Test 1: Basic Object Conversion ===");

            var person = new Person
            {
                Name = "Juan Pérez",
                Age = 30,
                Email = "juan@example.com",
                BirthDate = new DateTime(1993, 5, 15),
                IsActive = true,
                Salary = 50000.50m
            };

            // Convert to dictionary
            var dictionary = person.ToObjectDictionary();

            Console.WriteLine("Object to Dictionary:");
            foreach (var kvp in dictionary)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value} ({kvp.Value?.GetType().Name})");
            }

            // Convert to string dictionary
            var stringDict = person.ToStringDictionary();
            
            Console.WriteLine("\nObject to String Dictionary:");
            foreach (var kvp in stringDict)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            Console.WriteLine();
        }

        static void TestCalibrationData()
        {
            Console.WriteLine("=== Test 2: Calibration Data ===");

            var calibrationData = new CalibrationData
            {
                DeviceId = "TEMP-SENSOR-001",
                CalibrationDate = DateTime.Now,
                Temperature = 23.5,
                Pressure = 1013.25,
                Technician = "Ing. María García",
                IsValid = true
            };

            var dictionary = calibrationData.ToObjectDictionary();
            
            Console.WriteLine("Calibration Data Dictionary:");
            foreach (var kvp in dictionary)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            Console.WriteLine();
        }

        static void TestNestedObjects()
        {
            Console.WriteLine("=== Test 3: Nested Objects ===");

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

            var dictionary = person.ToObjectDictionary();
            
            Console.WriteLine("Nested Objects Dictionary:");
            foreach (var kvp in dictionary)
            {
                if (kvp.Value is Address address)
                {
                    Console.WriteLine($"  {kvp.Key}: Address Object");
                    Console.WriteLine($"    Street: {address.Street}");
                    Console.WriteLine($"    City: {address.City}");
                    Console.WriteLine($"    Country: {address.Country}");
                    Console.WriteLine($"    ZipCode: {address.ZipCode}");
                }
                else if (kvp.Value is List<string> hobbies)
                {
                    Console.WriteLine($"  {kvp.Key}: [{string.Join(", ", hobbies)}]");
                }
                else
                {
                    Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
                }
            }
            Console.WriteLine();
        }

        static void TestJsonConversion()
        {
            Console.WriteLine("=== Test 4: JSON Conversion (Handles Nested Objects) ===");

            var person = new Person
            {
                Name = "Carlos Ruiz",
                Age = 35,
                Email = "carlos@example.com",
                Address = new Address
                {
                    Street = "Avenida Libertad 456",
                    City = "Barcelona",
                    Country = "España",
                    ZipCode = 08001
                },
                Hobbies = new List<string> { "Fútbol", "Cocina", "Viajes" }
            };

            var jsonDict = person.ToDictionaryViaJson();
            
            Console.WriteLine("JSON Dictionary (flattens nested objects):");
            foreach (var kvp in jsonDict)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            Console.WriteLine();
        }
    }
}
