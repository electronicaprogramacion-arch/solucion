using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace CalibrationSaaS.Diagnostics
{
    /// <summary>
    /// Herramienta de diagnóstico específica para verificar el DbSet de ProcedureEquipment
    /// </summary>
    public static class DiagnosticoProcedureEquipmentDbSet
    {
        /// <summary>
        /// Ejecuta un diagnóstico completo del DbSet de ProcedureEquipment
        /// </summary>
        public static void EjecutarDiagnostico()
        {
            Console.WriteLine("🔍 DIAGNÓSTICO ESPECÍFICO: DbSet ProcedureEquipment");
            Console.WriteLine("==================================================");

            try
            {
                // 1. Verificar que la clase ProcedureEquipment existe
                Console.WriteLine("\n✅ Paso 1: Verificando existencia de la clase ProcedureEquipment...");
                
                var assembly = Assembly.LoadFrom("CalibrationSaaS.Models/bin/Debug/net9.0/CalibrationSaaS.Domain.Aggregates.dll");
                var procedureEquipmentType = assembly.GetTypes()
                    .FirstOrDefault(t => t.Name == "ProcedureEquipment");

                if (procedureEquipmentType != null)
                {
                    Console.WriteLine($"   ✓ Clase ProcedureEquipment encontrada: {procedureEquipmentType.FullName}");
                    Console.WriteLine($"   ✓ Namespace: {procedureEquipmentType.Namespace}");
                    Console.WriteLine($"   ✓ Assembly: {procedureEquipmentType.Assembly.GetName().Name}");
                    
                    // Listar propiedades
                    var properties = procedureEquipmentType.GetProperties();
                    Console.WriteLine($"   ✓ Propiedades encontradas: {properties.Length}");
                    foreach (var prop in properties.Take(10)) // Solo las primeras 10
                    {
                        Console.WriteLine($"     - {prop.Name}: {prop.PropertyType.Name}");
                    }
                }
                else
                {
                    Console.WriteLine("   ❌ Clase ProcedureEquipment NO encontrada");
                    Console.WriteLine("   💡 Listando todas las clases que contienen 'Procedure':");
                    
                    var procedureTypes = assembly.GetTypes()
                        .Where(t => t.Name.Contains("Procedure"))
                        .ToList();
                    
                    foreach (var type in procedureTypes)
                    {
                        Console.WriteLine($"     - {type.FullName}");
                    }
                    return;
                }

                // 2. Verificar el DbContext
                Console.WriteLine("\n✅ Paso 2: Verificando DbContext...");
                
                var contextAssembly = Assembly.LoadFrom("ServerContext/bin/Debug/net9.0/ServerContext.dll");
                var contextTypes = contextAssembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(DbContext)))
                    .ToList();

                Console.WriteLine($"   ✓ DbContext encontrados: {contextTypes.Count}");
                foreach (var contextType in contextTypes)
                {
                    Console.WriteLine($"     - {contextType.Name}");
                    
                    // Verificar DbSets
                    var dbSetProperties = contextType.GetProperties()
                        .Where(p => p.PropertyType.IsGenericType && 
                                   p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                        .ToList();
                    
                    Console.WriteLine($"       DbSets encontrados: {dbSetProperties.Count}");
                    
                    var procedureEquipmentDbSet = dbSetProperties
                        .FirstOrDefault(p => p.PropertyType.GetGenericArguments()[0].Name == "ProcedureEquipment");
                    
                    if (procedureEquipmentDbSet != null)
                    {
                        Console.WriteLine($"       ✓ DbSet<ProcedureEquipment> encontrado: {procedureEquipmentDbSet.Name}");
                        Console.WriteLine($"       ✓ Tipo: {procedureEquipmentDbSet.PropertyType}");
                        Console.WriteLine($"       ✓ Tipo de entidad: {procedureEquipmentDbSet.PropertyType.GetGenericArguments()[0].FullName}");
                    }
                    else
                    {
                        Console.WriteLine("       ❌ DbSet<ProcedureEquipment> NO encontrado");
                        Console.WriteLine("       💡 DbSets disponibles:");
                        foreach (var dbSet in dbSetProperties.Take(10))
                        {
                            var entityType = dbSet.PropertyType.GetGenericArguments()[0];
                            Console.WriteLine($"         - {dbSet.Name}: DbSet<{entityType.Name}>");
                        }
                    }
                }

                // 3. Verificar compatibilidad de tipos
                Console.WriteLine("\n✅ Paso 3: Verificando compatibilidad de tipos...");
                
                if (procedureEquipmentType != null)
                {
                    // Verificar que implementa las interfaces necesarias
                    var interfaces = procedureEquipmentType.GetInterfaces();
                    Console.WriteLine($"   ✓ Interfaces implementadas: {interfaces.Length}");
                    foreach (var iface in interfaces)
                    {
                        Console.WriteLine($"     - {iface.Name}");
                    }
                    
                    // Verificar atributos de Entity Framework
                    var attributes = procedureEquipmentType.GetCustomAttributes();
                    Console.WriteLine($"   ✓ Atributos de clase: {attributes.Count()}");
                    foreach (var attr in attributes)
                    {
                        Console.WriteLine($"     - {attr.GetType().Name}");
                    }
                    
                    // Verificar propiedades clave
                    var keyProperties = procedureEquipmentType.GetProperties()
                        .Where(p => p.GetCustomAttributes().Any(a => a.GetType().Name == "KeyAttribute"))
                        .ToList();
                    
                    Console.WriteLine($"   ✓ Propiedades clave encontradas: {keyProperties.Count}");
                    foreach (var keyProp in keyProperties)
                    {
                        Console.WriteLine($"     - {keyProp.Name}: {keyProp.PropertyType.Name}");
                    }
                }

                Console.WriteLine("\n🎉 DIAGNÓSTICO COMPLETADO");
                Console.WriteLine("\n📋 RESUMEN:");
                Console.WriteLine("   ✅ Verificación de clase: COMPLETADA");
                Console.WriteLine("   ✅ Verificación de DbContext: COMPLETADA");
                Console.WriteLine("   ✅ Verificación de compatibilidad: COMPLETADA");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ ERROR DURANTE EL DIAGNÓSTICO: {ex.Message}");
                Console.WriteLine($"❌ Tipo de excepción: {ex.GetType().Name}");
                Console.WriteLine($"❌ Stack Trace: {ex.StackTrace}");
                
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"❌ Excepción interna: {ex.InnerException.Message}");
                }

                Console.WriteLine("\n🔧 POSIBLES CAUSAS:");
                Console.WriteLine("1. Los assemblies no están compilados correctamente");
                Console.WriteLine("2. Faltan dependencias en el proyecto");
                Console.WriteLine("3. Hay conflictos de versiones de Entity Framework");
                Console.WriteLine("4. La configuración del DbContext es incorrecta");
            }
        }

        /// <summary>
        /// Verifica específicamente si hay errores de compilación relacionados con ProcedureEquipment
        /// </summary>
        public static void VerificarErroresCompilacion()
        {
            Console.WriteLine("\n🔍 VERIFICACIÓN DE ERRORES DE COMPILACIÓN");
            Console.WriteLine("==========================================");

            try
            {
                // Intentar cargar el assembly y capturar errores específicos
                Console.WriteLine("Intentando cargar CalibrationSaaS.Domain.Aggregates...");
                
                var modelsAssembly = Assembly.LoadFrom("CalibrationSaaS.Models/bin/Debug/net9.0/CalibrationSaaS.Domain.Aggregates.dll");
                Console.WriteLine("✓ Assembly de modelos cargado exitosamente");

                Console.WriteLine("Intentando cargar ServerContext...");
                var contextAssembly = Assembly.LoadFrom("ServerContext/bin/Debug/net9.0/ServerContext.dll");
                Console.WriteLine("✓ Assembly de contexto cargado exitosamente");

                // Verificar tipos específicos
                var procedureEquipmentType = modelsAssembly.GetType("CalibrationSaaS.Domain.Aggregates.Entities.ProcedureEquipment");
                if (procedureEquipmentType != null)
                {
                    Console.WriteLine("✓ Tipo ProcedureEquipment encontrado y accesible");
                    
                    // Intentar crear una instancia
                    var instance = Activator.CreateInstance(procedureEquipmentType);
                    Console.WriteLine("✓ Instancia de ProcedureEquipment creada exitosamente");
                }
                else
                {
                    Console.WriteLine("❌ Tipo ProcedureEquipment no encontrado");
                }

            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"❌ Assembly no encontrado: {ex.FileName}");
                Console.WriteLine("💡 Asegúrate de que el proyecto esté compilado correctamente");
            }
            catch (ReflectionTypeLoadException ex)
            {
                Console.WriteLine("❌ Error al cargar tipos del assembly:");
                foreach (var loaderEx in ex.LoaderExceptions)
                {
                    Console.WriteLine($"   - {loaderEx?.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error inesperado: {ex.Message}");
            }
        }
    }
}

// Programa principal para ejecutar el diagnóstico
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("🚀 INICIANDO DIAGNÓSTICO DE ProcedureEquipment DbSet");
        Console.WriteLine("====================================================");

        CalibrationSaaS.Diagnostics.DiagnosticoProcedureEquipmentDbSet.EjecutarDiagnostico();
        
        Console.WriteLine("\n" + new string('=', 60));
        
        CalibrationSaaS.Diagnostics.DiagnosticoProcedureEquipmentDbSet.VerificarErroresCompilacion();

        Console.WriteLine("\n🏁 DIAGNÓSTICO FINALIZADO");
        Console.WriteLine("Presiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}
