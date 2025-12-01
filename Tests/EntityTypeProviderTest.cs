using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CalibrationSaaS.Infraestructure.Blazor.Services;

namespace CalibrationSaaS.Tests
{
    /// <summary>
    /// Simple test to verify EntityTypeProvider is working correctly
    /// </summary>
    public class EntityTypeProviderTest
    {
        public static async Task RunTest()
        {
//            Console.WriteLine("=== ENTITY TYPE PROVIDER TEST ===");
//            Console.WriteLine();

            // Create a console logger
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = loggerFactory.CreateLogger<EntityTypeProvider>();

            // Create the EntityTypeProvider
            var entityTypeProvider = new EntityTypeProvider(logger);

            try
            {
//                Console.WriteLine("Testing EntityTypeProvider.GetAuditedEntityTypesAsync()...");
                
                // Get the audited entity types
                var auditedEntityTypes = await entityTypeProvider.GetAuditedEntityTypesAsync();

//                Console.WriteLine($"✅ SUCCESS: EntityTypeProvider returned {auditedEntityTypes?.Count() ?? 0} entity types");
//                Console.WriteLine();
                
//                Console.WriteLine("Entity types returned:");
//                Console.WriteLine("Value                  | Display Name");
//                Console.WriteLine("----------------------|---------------------------");

                if (auditedEntityTypes != null)
                {
                    foreach (var entityType in auditedEntityTypes)
                    {
//                        Console.WriteLine($"{entityType.Value,-21} | {entityType.DisplayName}");
                    }
                }
                
//                Console.WriteLine();
                
                // Test specific entity type checks
//                Console.WriteLine("Testing specific entity type checks:");
                var testEntities = new[] { "Manufacturer", "WorkOrderDetail", "NonExistent" };
                
                foreach (var testEntity in testEntities)
                {
                    var isAudited = entityTypeProvider.IsEntityTypeAudited(testEntity);
                    var displayName = entityTypeProvider.GetEntityTypeDisplayName(testEntity);
//                    Console.WriteLine($"  {testEntity}: IsAudited={isAudited}, DisplayName='{displayName}'");
                }
                
//                Console.WriteLine();
//                Console.WriteLine("✅ EntityTypeProvider test completed successfully!");
                
                // Check if we have exactly 5 entity types
                var count = auditedEntityTypes?.Count() ?? 0;
                if (count == 5)
                {
//                    Console.WriteLine("✅ CORRECT: Found exactly 5 entity types as expected");
                }
                else
                {
//                    Console.WriteLine($"❌ ERROR: Expected 5 entity types but found {count}");
                }
                
                // Check if WorkOrderDetail is present
                var hasWorkOrderDetail = auditedEntityTypes?.Any(e => e.Value == "WorkOrderDetail") ?? false;
                if (hasWorkOrderDetail)
                {
//                    Console.WriteLine("✅ CORRECT: WorkOrderDetail is present in the list");
                }
                else
                {
//                    Console.WriteLine("❌ ERROR: WorkOrderDetail is missing from the list");
                }
            }
            catch (Exception ex)
            {
//                Console.WriteLine($"❌ ERROR: EntityTypeProvider test failed with exception: {ex.Message}");
//                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
            
//            Console.WriteLine();
//            Console.WriteLine("=== TEST COMPLETED ===");
        }
    }
}
