using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CalibrationSaaS.Reports.Infraestructure.APIRep.Extensions;
using CalibrationSaaS.Reports.Infraestructure.APIRep.Services;
using CalibrationSaaS.Reports.Infraestructure.APIRep.Services.Meta;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using CalibrationSaaS.Infraestructure.EntityFramework.DataAccess;

namespace CalibrationSaaS.Reports.Infraestructure.APIRep
{
    public static class Startup
    {
        public static Microsoft.Extensions.Configuration.IConfiguration Configuration;


        
        public static WebApplication startApp(string[] args)
        {
            

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

           
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ITemplateService, RazorViewsTemplateService>();

            builder.Services.AddControllers();
            builder.WebHost.UseIISIntegration();
            builder.Services.AddEndpointsApiExplorer();
           
            builder.Services.AddHttpClient();
            ConfigureServices(builder.Services, builder);

            var app = builder.Build();
            Configure(app, app.Environment);
            
            return app;
        }

        //public IConfiguration Configuration { get; }

        public static void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
        {
            // Add CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            services.AddDbContext<CalibrationSaaSDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                       .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking),
                ServiceLifetime.Scoped);

            // Register IDbContextFactory with a scoped lifetime
            services.AddDbContextFactory<CalibrationSaaSDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Scoped);

            builder.Services.AddScoped<IWorkOrderDetailRepository, WODRepositoryEF<CalibrationSaaSDBContext>>();

            builder.Services.AddScoped<IBasicsRepository, BasicsRepositoryEF<CalibrationSaaSDBContext>>();

            builder.Services.AddScoped<IIdentityRepository, IdentityClient.IdentityClient>();

            builder.Services.AddScoped<IPieceOfEquipmentRepository, PieceOfEquipmentRepositoryEF<CalibrationSaaSDBContext>>();

            builder.Services.AddScoped<IUOMRepository, UOMRepositoryEF<CalibrationSaaSDBContext>>();


            builder.Services.AddScoped<IAssetsRepository, AssetsRepositoryEF<CalibrationSaaSDBContext>>();
            builder.Services.AddScoped<IQuoteRepository, QuoteRepositoryEF<CalibrationSaaSDBContext>>();

            // Quote enhancement repositories
            builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepositoryEF>();
            builder.Services.AddScoped<IPriceTypeRepository, PriceTypeRepositoryEF<CalibrationSaaSDBContext>>();

            builder.Services.AddScoped<CalibrationSaaS.Application.UseCases.WorkOrderDetailUseCase, CalibrationSaaS.Application.UseCases.WorkOrderDetailUseCase>();
            builder.Services.AddScoped<CalibrationSaaS.Application.UseCases.PieceOfEquipmentUseCases, CalibrationSaaS.Application.UseCases.PieceOfEquipmentUseCases>();
            builder.Services.AddScoped<CalibrationSaaS.Application.UseCases.UOMUseCases, CalibrationSaaS.Application.UseCases.UOMUseCases>();
            builder.Services.AddScoped<CalibrationSaaS.Application.UseCases.BasicsUseCases, CalibrationSaaS.Application.UseCases.BasicsUseCases>();
            builder.Services.AddScoped<CalibrationSaaS.Application.UseCases.QuoteUseCases, CalibrationSaaS.Application.UseCases.QuoteUseCases>();

        }

        public static void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            // Enable CORS - must be before UseAuthorization
            app.UseCors("AllowAll");

            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthorization();

            app.MapControllers();
//app.PreparePuppeteerAsync(env).GetAwaiter().GetResult();
            //app.UseEndpoints(endpoints => { endpoints.MapControllers(); });


        }
    }
}