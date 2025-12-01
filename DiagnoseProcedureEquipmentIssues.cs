using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CalibrationSaaS.Data.EntityFramework;
using CalibrationSaaS.Domain.Aggregates.Entities;

namespace CalibrationSaaS.Diagnostics
{
    /// <summary>
    /// Diagnostic utility to identify and fix ProcedureEquipment DbSet issues
    /// </summary>
    public class ProcedureEquipmentDiagnostics
    {
        private readonly ILogger<ProcedureEquipmentDiagnostics> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ProcedureEquipmentDiagnostics(ILogger<ProcedureEquipmentDiagnostics> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Runs comprehensive diagnostics on ProcedureEquipment entity
        /// </summary>
        public async Task<DiagnosticResult> RunDiagnosticsAsync()
        {
            var result = new DiagnosticResult();
            
            try
            {
                _logger.LogInformation("Starting ProcedureEquipment diagnostics...");

                // Test 1: Check if entity is properly configured
                await CheckEntityConfiguration(result);

                // Test 2: Check if table exists in database
                await CheckTableExists(result);

                // Test 3: Check if DbSet can be accessed
                await CheckDbSetAccess(result);

                // Test 4: Check foreign key relationships
                await CheckForeignKeyRelationships(result);

                // Test 5: Test basic CRUD operations
                await TestBasicOperations(result);

                _logger.LogInformation("ProcedureEquipment diagnostics completed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during ProcedureEquipment diagnostics");
                result.AddError($"Diagnostic failed: {ex.Message}");
            }

            return result;
        }

        private async Task CheckEntityConfiguration(DiagnosticResult result)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<CalibrationSaaSDBContext>();

                var entityType = context.Model.FindEntityType(typeof(ProcedureEquipment));
                
                if (entityType == null)
                {
                    result.AddError("ProcedureEquipment entity is not configured in the model");
                    return;
                }

                result.AddSuccess("ProcedureEquipment entity is properly configured");

                // Check properties
                var properties = entityType.GetProperties();
                var expectedProperties = new[] { "Id", "ProcedureID", "PieceOfEquipmentID", "CreatedDate", "CreatedBy" };
                
                foreach (var prop in expectedProperties)
                {
                    if (properties.Any(p => p.Name == prop))
                    {
                        result.AddSuccess($"Property '{prop}' is configured");
                    }
                    else
                    {
                        result.AddError($"Property '{prop}' is missing from configuration");
                    }
                }

                // Check foreign keys
                var foreignKeys = entityType.GetForeignKeys();
                if (foreignKeys.Any(fk => fk.PrincipalEntityType.ClrType == typeof(Procedure)))
                {
                    result.AddSuccess("Foreign key to Procedure is configured");
                }
                else
                {
                    result.AddError("Foreign key to Procedure is missing");
                }

                if (foreignKeys.Any(fk => fk.PrincipalEntityType.ClrType == typeof(PieceOfEquipment)))
                {
                    result.AddSuccess("Foreign key to PieceOfEquipment is configured");
                }
                else
                {
                    result.AddError("Foreign key to PieceOfEquipment is missing");
                }
            }
            catch (Exception ex)
            {
                result.AddError($"Entity configuration check failed: {ex.Message}");
            }
        }

        private async Task CheckTableExists(DiagnosticResult result)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<CalibrationSaaSDBContext>();

                var tableExists = await context.Database.SqlQueryRaw<int>(
                    "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ProcedureEquipment'")
                    .FirstOrDefaultAsync();

                if (tableExists > 0)
                {
                    result.AddSuccess("ProcedureEquipment table exists in database");
                }
                else
                {
                    result.AddError("ProcedureEquipment table does not exist in database");
                    result.AddRecommendation("Run the CreateProcedureEquipmentMigration.sql script to create the table");
                }
            }
            catch (Exception ex)
            {
                result.AddError($"Table existence check failed: {ex.Message}");
            }
        }

        private async Task CheckDbSetAccess(DiagnosticResult result)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<CalibrationSaaSDBContext>();

                // Try to access the DbSet
                var dbSet = context.ProcedureEquipment;
                if (dbSet != null)
                {
                    result.AddSuccess("ProcedureEquipment DbSet is accessible");
                    
                    // Try to query (this will fail if table doesn't exist)
                    var count = await dbSet.CountAsync();
                    result.AddSuccess($"ProcedureEquipment DbSet query successful. Current count: {count}");
                }
                else
                {
                    result.AddError("ProcedureEquipment DbSet is null");
                }
            }
            catch (Exception ex)
            {
                result.AddError($"DbSet access failed: {ex.Message}");
                if (ex.Message.Contains("Invalid object name"))
                {
                    result.AddRecommendation("The table doesn't exist. Run the migration script.");
                }
            }
        }

        private async Task CheckForeignKeyRelationships(DiagnosticResult result)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<CalibrationSaaSDBContext>();

                // Check if Procedure table exists and has data
                var procedureCount = await context.Procedure.CountAsync();
                result.AddInfo($"Procedure table has {procedureCount} records");

                // Check if PieceOfEquipment table exists and has data
                var equipmentCount = await context.PieceOfEquipment.CountAsync();
                result.AddInfo($"PieceOfEquipment table has {equipmentCount} records");

                if (procedureCount > 0 && equipmentCount > 0)
                {
                    result.AddSuccess("Both parent tables have data for testing relationships");
                }
                else
                {
                    result.AddWarning("One or both parent tables are empty - cannot test relationships");
                }
            }
            catch (Exception ex)
            {
                result.AddError($"Foreign key relationship check failed: {ex.Message}");
            }
        }

        private async Task TestBasicOperations(DiagnosticResult result)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<CalibrationSaaSDBContext>();

                // Try to create a test record (if parent data exists)
                var firstProcedure = await context.Procedure.FirstOrDefaultAsync();
                var firstEquipment = await context.PieceOfEquipment.FirstOrDefaultAsync();

                if (firstProcedure != null && firstEquipment != null)
                {
                    var testRecord = new ProcedureEquipment
                    {
                        ProcedureID = firstProcedure.ProcedureID,
                        PieceOfEquipmentID = firstEquipment.PieceOfEquipmentID,
                        CreatedBy = "DiagnosticTest"
                    };

                    // Test add (but don't save to avoid side effects)
                    context.ProcedureEquipment.Add(testRecord);
                    result.AddSuccess("Test record can be added to DbSet");

                    // Remove the test record
                    context.ProcedureEquipment.Remove(testRecord);
                    result.AddSuccess("Test record can be removed from DbSet");
                }
                else
                {
                    result.AddWarning("Cannot test CRUD operations - missing parent data");
                }
            }
            catch (Exception ex)
            {
                result.AddError($"Basic operations test failed: {ex.Message}");
            }
        }
    }

    public class DiagnosticResult
    {
        public List<string> Successes { get; } = new List<string>();
        public List<string> Errors { get; } = new List<string>();
        public List<string> Warnings { get; } = new List<string>();
        public List<string> Info { get; } = new List<string>();
        public List<string> Recommendations { get; } = new List<string>();

        public bool HasErrors => Errors.Any();
        public bool IsSuccessful => !HasErrors;

        public void AddSuccess(string message) => Successes.Add(message);
        public void AddError(string message) => Errors.Add(message);
        public void AddWarning(string message) => Warnings.Add(message);
        public void AddInfo(string message) => Info.Add(message);
        public void AddRecommendation(string message) => Recommendations.Add(message);

        public void PrintResults()
        {
            Console.WriteLine("=== ProcedureEquipment Diagnostic Results ===");
            
            if (Successes.Any())
            {
                Console.WriteLine("\n✅ SUCCESSES:");
                Successes.ForEach(s => Console.WriteLine($"  • {s}"));
            }

            if (Errors.Any())
            {
                Console.WriteLine("\n❌ ERRORS:");
                Errors.ForEach(e => Console.WriteLine($"  • {e}"));
            }

            if (Warnings.Any())
            {
                Console.WriteLine("\n⚠️ WARNINGS:");
                Warnings.ForEach(w => Console.WriteLine($"  • {w}"));
            }

            if (Info.Any())
            {
                Console.WriteLine("\n📋 INFO:");
                Info.ForEach(i => Console.WriteLine($"  • {i}"));
            }

            if (Recommendations.Any())
            {
                Console.WriteLine("\n💡 RECOMMENDATIONS:");
                Recommendations.ForEach(r => Console.WriteLine($"  • {r}"));
            }

            Console.WriteLine($"\n🎯 OVERALL RESULT: {(IsSuccessful ? "SUCCESS" : "FAILED")}");
        }
    }
}
