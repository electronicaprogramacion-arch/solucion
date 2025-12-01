using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProtoBuf.Grpc.Server;
using CalibrationSaaS.Infraestructure.GrpcServices.Services;
using CalibrationSaaS.Infraestructure.EntityFramework.DataAccess;
using CalibrationSaaS.Application.UseCases;
using CalibrationSaaS.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using CalibrationSaaS.Domain.Repositories;
using Microsoft.Extensions.Logging;
//using CalibrationSaaS.Infraestructure.GrpcServices.ServicesDummies;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using CalibrationSaaS.Infraestructure.Grpc.Helpers;
using CalibrationSaaS.Infraestructure.Grpc.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Rewrite;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text.Json.Serialization;
using CalibrationSaaS.Infraestructure.GrpcServices.Configuration;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Infraestructure.EntityFramework.DataAccess;
using CalibrationSaaS.Application.UseCases;
using CalibrationSaaS.Infraestructure.GrpcServices.Services;
using CalibrationSaaS.Application.Services;
using static System.Net.Mime.MediaTypeNames;

namespace CalibrationSaaS.Service
{
    public class Startup
    {
        //https://stackoverflow.com/questions/46010003/asp-net-core-2-0-value-cannot-be-null-parameter-name-connectionstring
        //public Microsoft.Extensions.Configuration.IConfigurationRoot Configuration { get; }
        public Microsoft.Extensions.Configuration.IConfiguration Configuration;

        public static bool test;

        public Startup(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            Configuration = configuration;
            
        }
        

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        //Protobuf-net.
        //https://stackoverflow.com/questions/60869533/protobuf-net-grpc-client-and-net-cores-grpc-client-factory-integration
        //https://github.com/protobuf-net/protobuf-net.Grpc/blob/master/examples/pb-net-grpc/Server_CS/Startup.cs
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddHttpClient();
            var a = Configuration.GetSection("Security").GetValue<string>("Url");

            if (Configuration.GetSection("Mode").Exists())
            {
                test = Configuration.GetSection("Mode").GetValue<bool>("Test");
            }
            else
            {
                test = false;
            }

            
            
            
            //services.AddAuthorization();
            //services.AddAuthorizationPolicyEvaluator();

            //services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
            //    .AddAzureADBearer(options => Configuration.Bind("AzureAd", options));
            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
            }));
            //services.AddAuthentication("Bearer")
            //   .AddJwtBearer("Bearer", opt =>
            //   {
            //       opt.RequireHttpsMetadata = false;
            //       opt.Authority = "https://localhost:5005";
            //       opt.Audience = "GRPC";

            //   });

            

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.RequireHttpsMetadata = false;
                   options.Authority = Configuration.GetSection("Security").GetValue<string>("Url");//"https://localhost:5005";
                   options.Audience = "GRPC";
                   options.TokenValidationParameters = new TokenValidationParameters()
                   {
                       NameClaimType =  "name",
                       RoleClaimType = "role"
                   };
               });


            services.AddAuthorization();



            services.AddHttpClient();
            
            services.AddGrpc(options =>
            {
                options.Interceptors.Add<LoggerInterceptor>();
                options.Interceptors.Add<CalibrationSaaS.Infraestructure.GrpcServices.Interceptors.PerformanceInterceptor>();
                options.Interceptors.Add<CalibrationSaaS.Infraestructure.GrpcServices.Interceptors.AuditUserContextInterceptor>();
                options.EnableDetailedErrors = true;
                options.MaxReceiveMessageSize = 10 * 1024 * 1024; // 2 MB
                options.MaxSendMessageSize = 10 * 1024 * 1024; // 5 MB



            });

            services.AddGrpcReflection();

            //https://docs.microsoft.com/en-us/aspnet/core/grpc/browser?view=aspnetcore-5.0
            //services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            //{
            //    builder.AllowAnyOrigin()
            //           .AllowAnyMethod()
            //           .AllowAnyHeader()
            //           .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
            //}));



            //https://www.youtube.com/watch?v=ZM0XeSjuwbc&t=4530s
            services.AddCodeFirstGrpc(config =>
            {
                config.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
               
            });




            //var options = new DbContextOptionsBuilder<CalibrationSaaSDBContext>()
            //   .UseInMemoryDatabase(databaseName: "Test")
            //   .Options;

            //var options2 = new DbContextOptionsBuilder<CalibrationSaaSDBContext>()
            //   .UseSqlServer(Microsoft.Extensions.Configuration
            //     .ConfigurationExtensions.GetConnectionString(this.Configuration, "DefaultConnection"))
            //   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options
            //   ;

            if (test)
            {
                services.AddDbContext<ApplicationDbContext>(options =>
               //options.UseInMemoryDatabase("identity")    
               //options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"))
               options.UseInMemoryDatabase(databaseName: "Test"));

                services.AddDbContext<CalibrationSaaSDBContext>(options => options.UseInMemoryDatabase(databaseName: "Test"), ServiceLifetime.Transient);
            }
            else
            {
                //services.AddDbContext<CalibrationSaaSDBContext>(options =>
                //options.UseSqlServer(Microsoft.Extensions.Configuration
                //.ConfigurationExtensions.GetConnectionString(this.Configuration, "DefaultConnection"))
                //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                //, ServiceLifetime.Transient);

                services.AddDbContext<ApplicationDbContext>(options =>
              
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

                services.AddDbContextFactory<CalibrationSaaSDBContext>(options =>
                options.UseSqlServer(Microsoft.Extensions.Configuration
                .ConfigurationExtensions.GetConnectionString(this.Configuration, "DefaultConnection")
                , providerOptions => providerOptions.EnableRetryOnFailure().MigrationsAssembly("CalibrationSaaS.Infraestructure.GrpcServices"))
                // AUDIT FIX: Enable change tracking for audit logging to work properly
                // Note: This may impact performance but is required for audit functionality
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
                .EnableDetailedErrors(true)
                .EnableSensitiveDataLogging(true)
                // AUDIT FIX: Add custom interceptor to capture original values before EF updates change tracker
                .AddInterceptors(new CalibrationSaaS.Infraestructure.GrpcServices.Interceptors.AuditInterceptor())
                , ServiceLifetime.Transient);



               

                //services.AddDbContextFactory<TestDbContext>(options =>
                //options.UseSqlServer(Microsoft.Extensions.Configuration
                //.ConfigurationExtensions.GetConnectionString(this.Configuration, "DefaultConnection")
                //, providerOptions => providerOptions.EnableRetryOnFailure().MigrationsAssembly("CalibrationSaaS.Infraestructure.GrpcServices"))
                //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                //.EnableDetailedErrors(true)
                //.EnableSensitiveDataLogging(true)
                //, ServiceLifetime.Transient);
                
                //services.AddScoped<UserGenerator>();
                

                //services.AddEntityFrameworkNpgsql().AddDbContext<Test3.TestDbContext>();




            }


            services.AddLogging(config =>
            {
                config.AddDebug();
                config.AddConsole();
                //etc
            });

            services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.MaxDepth = 32; 
            });

            //Dependecy injection
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1#service-lifetimes
            //
            services.AddTransient<ICustomerRepository, CustomerRepositoryEF<CalibrationSaaSDBContext>>();
            services.AddTransient<CustomerUseCases, CustomerUseCases>();
            services.AddTransient<SampleUseCases, SampleUseCases>();
            
            //services.AddTransient<CustomerAddressUseCases, CustomerAddressUseCases>();
            //services.AddTransient<ICustomerAddressRepository, CustomerAddressRepositoryEF>();
            services.AddTransient<IPieceOfEquipmentRepository, PieceOfEquipmentRepositoryEF<CalibrationSaaSDBContext>>();
            services.AddTransient<PieceOfEquipmentUseCases, PieceOfEquipmentUseCases>();
            services.AddTransient<BasicsUseCases, BasicsUseCases>();
            services.AddTransient<IBasicsRepository, BasicsRepositoryEF<CalibrationSaaSDBContext>>();
            services.AddTransient<AssetsUseCases, AssetsUseCases>();
            services.AddTransient<IAssetsRepository, AssetsRepositoryEF<CalibrationSaaSDBContext>>();
            services.AddTransient<IUOMRepository, UOMRepositoryEF<CalibrationSaaSDBContext>>();
            services.AddTransient<UOMUseCases, UOMUseCases>();
            services.AddTransient<WorkOrderDetailUseCase, WorkOrderDetailUseCase>();
            services.AddTransient<IWorkOrderDetailRepository, WODRepositoryEF<CalibrationSaaSDBContext>>();
            services.AddTransient<IIdentityRepository, IdentityClient.IdentityClient>();


            services.AddSingleton<ValidatorHelper, ValidatorHelper>();
            //services.AddBlazoredModal();

            // Configure Audit.NET
            services.ConfigureAudit(Configuration);

            // Register Audit Log services
            services.AddScoped<IAuditLogRepository, AuditLogRepositoryEF<CalibrationSaaSDBContext>>();
            services.AddScoped<AuditLogUseCases>();
            services.AddScoped<AuditLogService>();

            // Register User Context Provider for audit logging
            services.AddHttpContextAccessor(); // Required for UserContextProvider
            services.AddScoped<IUserContextProvider, UserContextProvider>();

            // Register Performance Monitoring services
            services.AddSingleton<CalibrationSaaS.Infraestructure.GrpcServices.Interceptors.IPerformanceMonitoringService,
                CalibrationSaaS.Infraestructure.GrpcServices.Interceptors.PerformanceMonitoringService>();

            // Quote services
            services.AddTransient<IWorkOrderRepository, WorkOrderRepositoryEF>();
            services.AddTransient<WorkOrderUseCases, WorkOrderUseCases>();
            services.AddTransient<QuoteUseCases, QuoteUseCases>();
            services.AddTransient<IQuoteRepository, QuoteRepositoryEF<CalibrationSaaSDBContext>>();
            services.AddTransient<CalibrationSaaS.Infraestructure.GrpcServices.Services.QuoteService, CalibrationSaaS.Infraestructure.GrpcServices.Services.QuoteService>();

            // Price Type services
            services.AddTransient<IPriceTypeRepository, PriceTypeRepositoryEF<CalibrationSaaSDBContext>>();
            services.AddTransient<PriceTypeUseCases, PriceTypeUseCases>();
            services.AddTransient<CalibrationSaaS.Service.Services.PriceTypeService, CalibrationSaaS.Service.Services.PriceTypeService>();

            // Equipment Recall services
            services.AddTransient<IEquipmentRecallRepository, EquipmentRecallRepositoryEF<CalibrationSaaSDBContext>>();
            services.AddTransient<EquipmentRecallUseCases, EquipmentRecallUseCases>();
            services.AddTransient<CalibrationSaaS.Infraestructure.GrpcServices.Services.EquipmentRecallService, CalibrationSaaS.Infraestructure.GrpcServices.Services.EquipmentRecallService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Initialize the service provider for audit configuration
            CalibrationSaaS.Infraestructure.GrpcServices.Configuration.AuditConfiguration.SetServiceProvider(app.ApplicationServices);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            //app.UseRouting();
            //        app.UseRouting(routes => {
            //            routes.MapGrpcService<ContentStoreImpl>()
            //              .RequireAuthorization();
            //        }
            //);

            //https://blog.sanderaernouts.com/grpc-aspnetcore-azure-ad-authentication
            //app.UseAuthentication(); // UseAuthentication must come before UseAuthorization
            //app.UseAuthorization();


            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
           
            app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });     
            
            
            
            //TODO: change to config file to handle things
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGrpcService<GreeterService>().EnableGrpcWeb().RequireCors("AllowAll");
                //       //.RequireCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                //           //.WithOrigins("http://localhost:54070", "https://localhost:44351", "https://localhost:44355",, "https://calibrationsaas.azurewebsites.net", "https://localhost:44352"));

                //endpoints.MapGrpcService<CalculatorService>().EnableGrpcWeb().RequireCors("AllowAll");
                //       //.RequireCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                //           //.WithOrigins("http://localhost:54070", "https://localhost:44351", "https://localhost:44355",, "https://calibrationsaas.azurewebsites.net", "https://localhost:44352"));

                //endpoints.MapGrpcService<ModalService>().EnableGrpcWeb().RequireCors("AllowAll");
                //       //.RequireCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                //           //.WithOrigins("http://localhost:54070", "https://localhost:44351", "https://localhost:44355",, "https://calibrationsaas.azurewebsites.net", "https://localhost:44352"));


                endpoints.MapGrpcService<UOMServices>().EnableGrpcWeb().RequireCors("AllowAll");
                //.RequireCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                //.WithOrigins("http://localhost:54070", "https://localhost:44351", "https://localhost:44355", "https://calibrationsaas.azurewebsites.net", "https://localhost:44352"));

                endpoints.MapGrpcService<ReportService>().EnableGrpcWeb().RequireCors("AllowAll");
                //.RequireCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                //.WithOrigins("http://localhost:54070", "https://localhost:44351", "https://localhost:44355", "https://calibrationsaas.azurewebsites.net", "https://localhost:44352"));

                endpoints.MapGrpcService<WorkOrderDetailServices>().EnableGrpcWeb().RequireCors("AllowAll");
                     //.RequireCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                         //.WithOrigins("http://localhost:54070", "https://localhost:44351", "https://localhost:44355", "https://calibrationsaas-staging.azurewebsites.net", "https://calibrationsaas-proproduction.azurewebsites.net", "https://calibrationsaas.azurewebsites.net", "https://localhost:44352"));


                endpoints.MapGrpcService<CustomerService>().EnableGrpcWeb().RequireCors("AllowAll");
                       //.RequireCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                           //.WithOrigins("http://localhost:54070", "https://localhost:44351", "https://localhost:44355", "https://calibrationsaas-staging.azurewebsites.net", "https://calibrationsaas-proproduction.azurewebsites.net", "https://calibrationsaas.azurewebsites.net", "https://localhost:44352"));

                //endpoints.MapGrpcService<CustomerService>().RequireAuthorization().EnableGrpcWeb().RequireCors("AllowAll");
                //       //.RequireCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                //           //.WithOrigins("http://localhost:54070", "https://localhost:44351", "https://localhost:44355", "https://calibrationsaas.azurewebsites.net"));

                endpoints.MapGrpcService<SampleService>().EnableGrpcWeb().RequireCors("AllowAll");
                       //.RequireCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                           //.WithOrigins("http://localhost:54070", "https://localhost:44351", "https://localhost:44355", "https://calibrationsaas.azurewebsites.net"));


                endpoints.MapGrpcService<BasicsServices>().EnableGrpcWeb().RequireCors("AllowAll");
                       //.RequireCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                           //.WithOrigins("http://localhost:54070", "https://localhost:44351", "https://localhost:44355", "https://calibrationsaas-staging.azurewebsites.net", "https://calibrationsaas-proproduction.azurewebsites.net", "https://calibrationsaas.azurewebsites.net", "https://localhost:44352"));


                endpoints.MapGrpcService<AddressServices>().EnableGrpcWeb().RequireCors("AllowAll");
                      //.RequireCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                          //.WithOrigins("http://localhost:54070", "https://localhost:44351", "https://localhost:44355", "https://calibrationsaas-staging.azurewebsites.net", "https://calibrationsaas-proproduction.azurewebsites.net", "https://calibrationsaas.azurewebsites.net", "https://localhost:44352"));


               
                endpoints.MapGrpcService<CustomerAddressService>().EnableGrpcWeb().RequireCors("AllowAll");
                       //.RequireCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                           //.WithOrigins("http://localhost:54070", "https://localhost:44351", "https://localhost:44355", "https://calibrationsaas-staging.azurewebsites.net", "https://calibrationsaas-proproduction.azurewebsites.net", "https://calibrationsaas.azurewebsites.net", "https://localhost:44352"));

                endpoints.MapGrpcService<PieceOfEquipmentService>().EnableGrpcWeb().RequireCors("AllowAll");
                       //.RequireCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                           //.WithOrigins("http://localhost:54070", "https://localhost:44351", "https://localhost:44355", "https://calibrationsaas-staging.azurewebsites.net", "https://calibrationsaas-proproduction.azurewebsites.net", "https://calibrationsaas.azurewebsites.net", "https://localhost:44352"));

                endpoints.MapGrpcService<WorkOrderService>().EnableGrpcWeb().RequireCors("AllowAll");
                       //.RequireCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                           //.WithOrigins("http://localhost:54070", "https://localhost:44351", "https://localhost:44355", "https://calibrationsaas-staging.azurewebsites.net", "https://calibrationsaas-proproduction.azurewebsites.net", "https://calibrationsaas.azurewebsites.net", "https://localhost:44352"));

                endpoints.MapGrpcService<AssetsServices>().EnableGrpcWeb().RequireCors("AllowAll");
                       //.RequireCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                           //.WithOrigins("http://localhost:54070", "https://localhost:44351", "https://localhost:44355", "https://calibrationsaas-staging.azurewebsites.net", "https://calibrationsaas-proproduction.azurewebsites.net", "https://calibrationsaas.azurewebsites.net", "https://localhost:44352"));

                endpoints.MapGrpcService<FileUpload>().EnableGrpcWeb().RequireCors("AllowAll");
                       //.RequireCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                           //.WithOrigins("http://localhost:54070", "https://localhost:44351", "https://calibrationsaas.azurewebsites.net", "https://localhost:44352"));


                 endpoints.MapGrpcService<ComponentServices>().EnableGrpcWeb().RequireCors("AllowAll");
                       //.RequireCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                           //.WithOrigins("http://localhost:54070", "https://localhost:44351", "https://localhost:44355", "https://calibrationsaas-staging.azurewebsites.net", "https://calibrationsaas-proproduction.azurewebsites.net", "https://calibrationsaas.azurewebsites.net", "https://localhost:44352"));

                endpoints.MapGrpcService<AuditLogService>().EnableGrpcWeb().RequireCors("AllowAll");
                       //.RequireCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                           //.WithOrigins("http://localhost:54070", "https://localhost:44351", "https://localhost:44355", "https://calibrationsaas-staging.azurewebsites.net", "https://calibrationsaas-proproduction.azurewebsites.net", "https://calibrationsaas.azurewebsites.net", "https://localhost:44352"));

                endpoints.MapGrpcService<CalibrationSaaS.Infraestructure.GrpcServices.Services.QuoteService>().EnableGrpcWeb().RequireCors("AllowAll");
                       //.RequireCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                           //.WithOrigins("http://localhost:54070", "https://localhost:44351", "https://localhost:44355", "https://calibrationsaas-staging.azurewebsites.net", "https://calibrationsaas-proproduction.azurewebsites.net", "https://calibrationsaas.azurewebsites.net", "https://localhost:44352"));

                endpoints.MapGrpcService<CalibrationSaaS.Service.Services.PriceTypeService>().EnableGrpcWeb().RequireCors("AllowAll");
                       //.RequireCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                           //.WithOrigins("http://localhost:54070", "https://localhost:44351", "https://localhost:44355", "https://calibrationsaas-staging.azurewebsites.net", "https://calibrationsaas-proproduction.azurewebsites.net", "https://calibrationsaas.azurewebsites.net", "https://localhost:44352"));

                endpoints.MapGrpcService<CalibrationSaaS.Infraestructure.GrpcServices.Services.EquipmentRecallService>().EnableGrpcWeb().RequireCors("AllowAll");
                       //.RequireCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                           //.WithOrigins("http://localhost:54070", "https://localhost:44351", "https://localhost:44355", "https://calibrationsaas-staging.azurewebsites.net", "https://calibrationsaas-proproduction.azurewebsites.net", "https://calibrationsaas.azurewebsites.net", "https://localhost:44352"));

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });

                if (env.IsDevelopment())
                {
                    endpoints.MapGrpcReflectionService();
                }

            });
            //app.UseRouting();
            //app.UseAuthentication();
            //app.UseAuthorization();
            //var options = new RewriteOptions().AddRedirectToHttps(302, 5334);
            // app.UseRewriter(options);
            //app.UseHttpsRedirection();  

            var httpsSection = Configuration.GetSection("HttpServer:Endpoints:Https");
            if (httpsSection.Exists())
            {
            var httpsEndpoint = new EndpointConfiguration();
            httpsSection.Bind(httpsEndpoint);
            app.UseRewriter(new RewriteOptions().AddRedirectToHttps(
            statusCode: env.IsDevelopment() ? StatusCodes.Status302Found : StatusCodes.Status301MovedPermanently,
            sslPort: httpsEndpoint.Port));
            }
         
           //using (var scope = app.ApplicationServices.CreateScope())
           
           // {
           // var services = scope.ServiceProvider;
        

           // var context = services.GetService<CalibrationSaaSDBContext>();
            
           //var contextSecurity = services.GetService<ApplicationDbContext>();

           //     if (!test)
           //         CalibrationSaaS.Infraestructure.GrpcServices.Helpers.DataGenerator.RecollectData(context, contextSecurity);

           //     if (test)
           //         CalibrationSaaS.Infraestructure.GrpcServices.Helpers.DataGenerator.Initialize(context, contextSecurity);

           // }

            app.UseStaticFiles();


    //          app.UseStaticFiles(new StaticFileOptions()
    //{
    //    FileProvider = new PhysicalFileProvider(
    //                        Path.Combine(Directory.GetCurrentDirectory(), @"Certificate")),
    //                        RequestPath = new PathString("/Certificate")
    //});

        }
    }

    public class MyClass
    {

        private readonly ILogger<MyClass> _logger;


        public MyClass(ILogger<MyClass> logger)
        {
            _logger = logger;
        }
        public void MyFunc()
        {
            _logger.Log(LogLevel.Error, "My Message");
        }
    }

}
