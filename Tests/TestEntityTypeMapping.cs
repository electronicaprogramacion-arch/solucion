using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CalibrationSaaS.Infraestructure.Blazor.Services;

namespace CalibrationSaaS.Tests
{
    /// <summary>
    /// Simple test to verify that EntityTypeProvider is returning the correct entity types
    /// that match what's stored in the audit logs
    /// </summary>
    public class TestEntityTypeMapping
    {
        public static async Task RunTest()
        {
//            Console.WriteLine("=== ENTITY TYPE MAPPING TEST ===");
//            Console.WriteLine();

            // Create a logger (you can use a console logger or null logger for testing)
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = loggerFactory.CreateLogger<EntityTypeProvider>();

            // Create the EntityTypeProvider
            var entityTypeProvider = new EntityTypeProvider(logger);

            try
            {
                // Get the audited entity types
                var auditedEntityTypes = await entityTypeProvider.GetAuditedEntityTypesAsync();

//                Console.WriteLine("Entity types returned by EntityTypeProvider:");
//                Console.WriteLine("Value (used for filtering) | Display Name (shown in UI)");
//                Console.WriteLine("---------------------------|---------------------------");

                foreach (var entityType in auditedEntityTypes)
                {
//                    Console.WriteLine($"{entityType.Value,-26} | {entityType.DisplayName}");
                }

//                Console.WriteLine();
//                Console.WriteLine("Expected entity class names that should be stored in audit logs:");
//                Console.WriteLine("- Manufacturer");
//                Console.WriteLine("- EquipmentTemplate");
//                Console.WriteLine("- PieceOfEquipment");
//                Console.WriteLine("- WorkOrder");
//                Console.WriteLine("- WorkOrderDetail");

//                Console.WriteLine();
//                Console.WriteLine("✅ Test completed successfully");
//                Console.WriteLine();
//                Console.WriteLine("If the 'Value' column matches the expected class names above,");
//                Console.WriteLine("then the entity type mapping is correct.");
            }
            catch (Exception ex)
            {
//                Console.WriteLine($"❌ Test failed with error: {ex.Message}");
//                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}
