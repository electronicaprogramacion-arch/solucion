using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CalibrationSaaS.Data.EntityFramework;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Application.UseCases;
using CalibrationSaaS.Infraestructure.EntityFramework.DataAccess;
using CalibrationSaaS.Domain.Repositories;
using Helpers.Controls.ValueObjects;
using Xunit;
using Microsoft.Extensions.Configuration;

namespace CalibrationSaaS.Tests
{
    /// <summary>
    /// Integration test for Audit.NET implementation
    /// This test validates the complete audit logging flow
    /// </summary>
    public class AuditLogIntegrationTest : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly CalibrationSaaSDBContext _context;

        public AuditLogIntegrationTest()
        {
            // Setup test services
            var services = new ServiceCollection();
            
            // Add configuration
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("ConnectionStrings:DefaultConnection", 
                        "Server=(localdb)\\mssqllocaldb;Database=CalibrationSaaSTest;Trusted_Connection=true;MultipleActiveResultSets=true")
                })
                .Build();
            
            services.AddSingleton<IConfiguration>(configuration);
            
            // Add Entity Framework
            services.AddDbContext<CalibrationSaaSDBContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));
            
            // Add logging
            services.AddLogging(builder => builder.AddConsole());
            
            // Add audit services
            services.AddScoped<IAuditLogRepository, AuditLogRepositoryEF<CalibrationSaaSDBContext>>();
            services.AddScoped<AuditLogUseCases>();
            
            _serviceProvider = services.BuildServiceProvider();
            _context = _serviceProvider.GetRequiredService<CalibrationSaaSDBContext>();
            
            // Ensure database is created
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task Should_Create_Audit_Log_When_Entity_Is_Modified()
        {
            // Arrange
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = "Test Customer",
                Email = "test@example.com",
                CreatedDate = DateTime.UtcNow
            };

            // Act - Create customer (should trigger audit log)
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Assert - Check if audit log was created
            var auditLogs = await _context.AuditLogs
                .Where(a => a.EntityType == "Customer" && a.EntityId == customer.Id.ToString())
                .ToListAsync();

            Assert.NotEmpty(auditLogs);
            Assert.Equal("Insert", auditLogs.First().ActionType);
        }

        [Fact]
        public async Task Should_Retrieve_Audit_Logs_Through_Service()
        {
            // Arrange
            var auditLogUseCases = _serviceProvider.GetRequiredService<AuditLogUseCases>();
            
            // Create some test audit logs
            var auditLog1 = new AuditLog
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow.AddMinutes(-10),
                UserName = "test.user@example.com",
                EntityType = "WorkOrder",
                EntityId = "WO-001",
                ActionType = "Create",
                CurrentState = "{\"Id\":\"WO-001\",\"Status\":\"New\"}",
                CreatedDate = DateTime.UtcNow
            };

            var auditLog2 = new AuditLog
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow.AddMinutes(-5),
                UserName = "test.user@example.com",
                EntityType = "WorkOrder",
                EntityId = "WO-001",
                ActionType = "Update",
                PreviousState = "{\"Id\":\"WO-001\",\"Status\":\"New\"}",
                CurrentState = "{\"Id\":\"WO-001\",\"Status\":\"In Progress\"}",
                CreatedDate = DateTime.UtcNow
            };

            _context.AuditLogs.AddRange(auditLog1, auditLog2);
            await _context.SaveChangesAsync();

            // Act
            var pagination = new Pagination<AuditLogEntry>
            {
                Page = 1,
                PageSize = 10,
                ColumnName = "Timestamp",
                SortingAscending = false
            };

            var result = await auditLogUseCases.GetAuditLogs(pagination);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Count >= 2);
            Assert.NotEmpty(result.List);
            
            var firstEntry = result.List.First();
            Assert.Equal("test.user@example.com", firstEntry.UserName);
            Assert.Equal("WorkOrder", firstEntry.EntityType);
        }

        [Fact]
        public async Task Should_Filter_Audit_Logs_By_Entity_Type()
        {
            // Arrange
            var auditLogUseCases = _serviceProvider.GetRequiredService<AuditLogUseCases>();
            
            // Create audit logs for different entity types
            var customerAuditLog = new AuditLog
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                UserName = "test.user@example.com",
                EntityType = "Customer",
                EntityId = "CUST-001",
                ActionType = "Create",
                CurrentState = "{\"Id\":\"CUST-001\",\"Name\":\"Test Customer\"}",
                CreatedDate = DateTime.UtcNow
            };

            var workOrderAuditLog = new AuditLog
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                UserName = "test.user@example.com",
                EntityType = "WorkOrder",
                EntityId = "WO-002",
                ActionType = "Create",
                CurrentState = "{\"Id\":\"WO-002\",\"Status\":\"New\"}",
                CreatedDate = DateTime.UtcNow
            };

            _context.AuditLogs.AddRange(customerAuditLog, workOrderAuditLog);
            await _context.SaveChangesAsync();

            // Act
            var searchRequest = new AuditLogSearchRequest
            {
                EntityType = "Customer",
                Page = 1,
                PageSize = 10,
                SortColumn = "Timestamp",
                SortDescending = true
            };

            var result = await auditLogUseCases.SearchAuditLogs(searchRequest);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Count >= 1);
            Assert.All(result.List, entry => Assert.Equal("Customer", entry.EntityType));
        }

        [Fact]
        public async Task Should_Filter_Audit_Logs_By_Date_Range()
        {
            // Arrange
            var auditLogUseCases = _serviceProvider.GetRequiredService<AuditLogUseCases>();
            
            var oldAuditLog = new AuditLog
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow.AddDays(-10),
                UserName = "test.user@example.com",
                EntityType = "Customer",
                EntityId = "CUST-OLD",
                ActionType = "Create",
                CurrentState = "{\"Id\":\"CUST-OLD\"}",
                CreatedDate = DateTime.UtcNow
            };

            var recentAuditLog = new AuditLog
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow.AddHours(-1),
                UserName = "test.user@example.com",
                EntityType = "Customer",
                EntityId = "CUST-RECENT",
                ActionType = "Create",
                CurrentState = "{\"Id\":\"CUST-RECENT\"}",
                CreatedDate = DateTime.UtcNow
            };

            _context.AuditLogs.AddRange(oldAuditLog, recentAuditLog);
            await _context.SaveChangesAsync();

            // Act
            var searchRequest = new AuditLogSearchRequest
            {
                FromDate = DateTime.UtcNow.AddDays(-1),
                ToDate = DateTime.UtcNow,
                Page = 1,
                PageSize = 10,
                SortColumn = "Timestamp",
                SortDescending = true
            };

            var result = await auditLogUseCases.SearchAuditLogs(searchRequest);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.List.Any(entry => entry.EntityId == "CUST-RECENT"));
            Assert.False(result.List.Any(entry => entry.EntityId == "CUST-OLD"));
        }

        public void Dispose()
        {
            _context?.Dispose();
            _serviceProvider?.Dispose();
        }
    }
}
