
using CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using CalibrationSaaS.Application.Services;
using ProtoBuf.Grpc;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;
namespace Blazor.Sqlite.Client.Features.Contributions
{
    public static class ContributionServiceCollectionExtensions 
    {

        public static void AddContributionsFeature(this IServiceCollection services)
        {

            SqliteWasmHelper.Extensions.AddSqliteWasmDbContextFactory<CalibrationSaaSDBContextOff2>(
            services,opts => opts.UseSqlite($"Filename={DatabaseService<CalibrationSaaSDBContextOff2>.FileName}"));


            //services.AddDbContextFactory<CalibrationSaaSDBContextOff>(
            //        options => options.UseSqlite($"Filename={DatabaseService<CalibrationSaaSDBContextOff>.FileName}")
            //        );


            //services.AddScoped<CalibrationSaaSDBContextOff>();
            
        }

        public static async Task InitializeContributionsFeature(this WebAssemblyHost host)
        {
            // Initialize DatabaseContext and sync with IndexedDb Files
            var dbService = host.Services.GetRequiredService<DatabaseService2<CalibrationSaaSDBContextOff2>>();
            
            await dbService.InitDatabaseAsync();

           
           
            var offService = host.Services.GetRequiredService<ISampleService2<CallContext>>();

            await offService.InitializeAsync();



        }
    }
}
