using Blazed.Controls;
using Blazed.Controls.Toast;
using Blazor.Extensions.Logging;
using Blazor.IndexedDB.Framework;
using Blazor.Sqlite.Client.Features.Contributions;
using Blazored.Modal;
using Blazored.Modal.Services;
using CacheStorage.Utils;
//using Microsoft.FluentUI.AspNetCore.Components;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Security;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite;
using CalibrationSaaS.Infraestructure.Blazor.Shared;
using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;
using Grpc.Net.Client.Web;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Client;
using Security;
using SqliteWasmHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Component = Helpers.Controls.Component;

namespace CalibrationSaaS.Infraestructure.Blazor
{
    public class Program
    {

        private const string Address = "localhost";
        private static async Task<string> Authenticate()
        {
            Console.WriteLine($"Authenticating as {Environment.UserName}...");
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://{Address}/generateJwtToken?name={HttpUtility.UrlEncode(Environment.UserName)}"),
                Method = HttpMethod.Get,
                Version = new Version(2, 0)
            };
            var tokenResponse = await httpClient.SendAsync(request);
            tokenResponse.EnsureSuccessStatusCode();

            var token = await tokenResponse.Content.ReadAsStringAsync();
            Console.WriteLine("Successfully authenticated.");

            return token;
        }


        public static Microsoft.Extensions.Configuration.IConfiguration config;

        //internal static string Scope = "dbe7c3bb-7aa8-406b-9fe5-6657a22032a0/Bitterman.Access";
        public static string roles = "";

        public static Domain.Aggregates.Shared.AppSecurity AppSecurity = new Domain.Aggregates.Shared.AppSecurity();


        public static async Task<Domain.Aggregates.ValueObjects.RolResultSet> Roles(WebAssemblyHostBuilder builder)
        {
            

            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(_token))
                {
                    metadata.Add("Authorization", $"Bearer {_token}");
                }
                return Task.CompletedTask;
            });
            // Create a gRPC-Web channel pointing to the backend server
            var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
            var channel = Grpc.Net.Client.GrpcChannel.ForAddress(builder.Configuration["Kestrel:Endpoints:Http3:Url"], new GrpcChannelOptions
            {
                HttpClient = httpClient
                ,
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)


            });
            //var channel = Grpc.Net.Client.GrpcChannel.ForAddress("https://calsaasgrpc.azurewebsites.net", new GrpcChannelOptions { HttpClient = httpClient });

            // Now we can instantiate gRPC clients for this channel
            var a = channel.CreateGrpcService<Application.Services.IBasicsServices<CallContext>>();




            try
            {
                if (AppSecurity.IsReport || AppSecurity.IsNotGrpc)
                {
                    return GetRoles();
                }

                var b = await a.GetRoles(CallContext.Default);
                return b;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex?.InnerException?.Message);
                Console.WriteLine(ex?.StackTrace);

                AppSecurity.IsNotGrpc = true;

               
                return GetRoles();


                //var c= new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.CustomerServiceOffline<CalibrationSaaSDBContextOff2>(database);
            }

           

            
        }

        public static RolResultSet GetRoles()
        {
            RolResultSet roles = new RolResultSet();

            roles.Roles = new List<Rol>();

            Rol r = new Rol();
            r.Name = "tech";
            roles.Roles.Add(r);
            r = new Rol();
            r.Name = "admin";
            roles.Roles.Add(r);
            r = new Rol();
            r.Name = "job";
            roles.Roles.Add(r);
            return roles;
        }
        public static async Task<User> GetUserById(string name,string endpoint)
        {
            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(_token))
                {
                    metadata.Add("Authorization", $"Bearer {_token}");
                }
                return Task.CompletedTask;
            });
            // Create a gRPC-Web channel pointing to the backend server
            var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
            var channel = Grpc.Net.Client.GrpcChannel.ForAddress(endpoint, new GrpcChannelOptions
            {
                HttpClient = httpClient
                ,
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)


            });
            //var channel = Grpc.Net.Client.GrpcChannel.ForAddress("https://calsaasgrpc.azurewebsites.net", new GrpcChannelOptions { HttpClient = httpClient });

            // Now we can instantiate gRPC clients for this channel
            var a = channel.CreateGrpcService<Application.Services.IBasicsServices<CallContext>>();

            User user = new User();

            user.Name = name;
            user.UserName = name;
            var b = await a.GetUserById(user, CallContext.Default);

            return b;
        }

        public static List<CustomSequence> CustomSequences { get; set; }

        public static async Task<ICollection<Helpers.Controls.Component>> GetComponents(WebAssemblyHostBuilder builder, string AddressEnnpoint)
        {

            var aa = PrerenderRouteHelper.GetRoutesToRender(typeof(Program).Assembly); // Pass in the WebAssembly app's Assembly
                                                                                                                          //.Select(config => new object[] { config });

            // Create a gRPC-Web channel pointing to the backend server
            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(_token))
                {
                    metadata.Add("Authorization", $"Bearer {_token}");
                }
                return Task.CompletedTask;
            });
            // Create a gRPC-Web channel pointing to the backend server
            var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
            var channel = Grpc.Net.Client.GrpcChannel.ForAddress(builder.Configuration["Kestrel:Endpoints:Http3:Url"], new GrpcChannelOptions
            { HttpClient = httpClient
                //,Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)


            });
            //var channel = Grpc.Net.Client.GrpcChannel.ForAddress("https://calsaasgrpc.azurewebsites.net", new GrpcChannelOptions { HttpClient = httpClient });

            // Now we can instantiate gRPC clients for this channel
            var a = channel.CreateGrpcService<Application.Services.IComponentServices<CallContext>>();



            var bb = aa;
            //
            var c = await a.GetAllComponents(new CallContext());


            List<Component> cnts = new List<Component>();
            foreach (var item in aa)
            {
               Component cc = new Component();

                var c2 = c?.List.Where(x => x.Name == item).FirstOrDefault();
                if ( c2 != null)
                {
                    cc = c2;
                }
                else
                {
                    cc.Name = item.ToString();
                    cc.Route = item.ToString();
                }
               

                cnts.Add(cc);
            }

           

            var b = await a.CreateComponents(cnts, new CallContext());

            var cs = await a.CustomSequences(new CallContext());

            CustomSequences = cs.List;

            return b.List;

        }

        static Random rnd = new Random();
        private static void DisplayValues()
        {
            // Create new thread and display three random numbers.
            Console.WriteLine("Some currency values:");
            for (int ctr = 0; ctr <= 3; ctr++)
                Console.WriteLine("   {0:C2}", rnd.NextDouble() * 10);
        }
        private static void DisplayThreadInfo()
        {

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator = ".";

            Console.WriteLine("\nCurrent Thread Name: '{0}'",
                              Thread.CurrentThread.Name);
            Console.WriteLine("Current Thread Culture/UI Culture: {0}/{1}",
                              Thread.CurrentThread.CurrentCulture.Name,
                              Thread.CurrentThread.CurrentUICulture.Name);
        }

        private static void ThreadProc()
        {
            DisplayThreadInfo();
            DisplayValues();
        }

        private static string _token;


        static GrpcChannel CreateChanel(string AddressEndPoint)
        {
            //builder.Configuration["Kestrel:Endpoints:Http2:Url"]
            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(_token))
                {
                    metadata.Add("Authorization", $"Bearer {_token}");
                }
                return Task.CompletedTask;
            });

            // Create a gRPC-Web channel pointing to the backend server
            var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));

            httpClient.Timeout = TimeSpan.FromSeconds(180);
            //var channel = Grpc.Net.Client.GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions { HttpClient = httpClient });
            var channel = Grpc.Net.Client.GrpcChannel.ForAddress(AddressEndPoint, new GrpcChannelOptions
            {

                HttpClient = httpClient,
                //Credentials = ChannelCredentials.Insecure //ChannelCredentials.Create(ChannelCredentials.Insecure, credentials)
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials),


            });

            return channel;

        }

        static T CreateService<T>(string AddresEndPoint) where T : class
        {

            return CreateChanel(AddresEndPoint).CreateGrpcService<T>();

        }

        public static IJSInProcessRuntime JSInProcRuntime;
        public static async Task Main(string[] args)
        {

            var watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here
            
           

            string[] startupParams;

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            //builder.RootComponents.Add<App>("app");

            var js = (IJSInProcessRuntime)builder.Services.BuildServiceProvider().GetRequiredService<IJSRuntime>();
            try
            {
                 startupParams = js.Invoke<string[]>("startupParams");
            }
            finally
            {

            }
            Console.WriteLine("program");
            Console.WriteLine(startupParams?.ElementAtOrDefault(0));
            Console.WriteLine(startupParams?.ElementAtOrDefault(1));
            Console.WriteLine(startupParams?.ElementAtOrDefault(2));

            if (startupParams != null && startupParams.Count() > 0 && startupParams?.ElementAtOrDefault(0).ToLower().Contains("dynamicreport") == true)
            {
                AppSecurity.IsReport = true;
                Console.WriteLine("IS REPORT");
            }

            if(startupParams?.ElementAtOrDefault(1) != null)
            {
                var ison = Convert.ToBoolean(startupParams?.ElementAtOrDefault(1));

                AppSecurity.IsNotGrpc = !ison;
                Console.WriteLine("IS NOT GRPC");
                Console.WriteLine(AppSecurity.IsNotGrpc);
            }

            //builder.Services.Configure<CalibrationSaaS.Domain.Aggregates.Shared.VersionApp>(config.GetSection(CalibrationSaaS.Domain.Aggregates.Shared.VersionApp.Version));

            //builder.Configuration["Kestrel:Endpoints:Http2:Url"]

            var loglevel = LogLevel.Information;
            if (builder.HostEnvironment.Environment == "Production" || builder.HostEnvironment.Environment == "Bitterman")
            {
                loglevel = LogLevel.Error;
            }

            //Log
            //https://docs.microsoft.com/en-us/aspnet/core/blazor/fundamentals/logging?view=aspnetcore-3.1
            //https://github.com/BlazorExtensions/Logging
            // Add Blazor.Extensions.Logging.BrowserConsoleLogger
            builder.Services.AddLogging(builder => builder
                .AddBrowserConsole()
                .SetMinimumLevel(loglevel)
            );




            builder.Services.AddOidcAuthentication(options =>
            {

                builder.Configuration.Bind("oidc", options.ProviderOptions);
                options.UserOptions.ScopeClaim = "scope";
                options.UserOptions.RoleClaim = "role";
                options.UserOptions.NameClaim = "name";




            })
              //.AddAccountClaimsPrincipalFactory<MultipleRoleClaimsPrincipalFactory<RemoteUserAccount>, CustomUserAccount, CustomAccountFactory>();
              .AddAccountClaimsPrincipalFactory<MultipleRoleClaimsPrincipalFactory<RemoteUserAccount>>();

            //MultipleRoleClaimsPrincipalFactory<RemoteUserAccount> p = new MultipleRoleClaimsPrincipalFactory<RemoteUserAccount>();

           
            builder.Services.AddOptions();
            //builder.Services.AddAuthorizationCore();

             string AddressEnnpoint = builder.Configuration["Kestrel:Endpoints:Http2:Url"];


            AppSecurity.AssemblyName =  builder.Configuration["VersionApp:Assembly"]; 

            //https://github.com/StefH/BlazorWasmGrpcWithAADAuth/blob/master/BlazorWasmGrpcWithAADAuth/Client/Program.cs
            //builder.Services.AddMsalAuthentication(options =>
            //{
            //    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
            //    // Add Scope https://github.com/StefH/BlazorWasmGrpcWithAADAuth
            //    options.ProviderOptions.DefaultAccessTokenScopes.Add(Scope);
            //});

            //builder.Services.AddGrpcBearerTokenProvider();

            //https://visualstudiomagazine.com/articles/2020/09/08/blazor-pwa-local-storage.aspx
            //builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }).AddBlazoredLocalStorage();
            //builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            //https://blog.stevensanderson.com/2019/08/03/blazor-indexeddb/
            //builder.Services.AddTransient<IIndexedDbFactory, IndexedDbFactory>();

            builder.Services.AddTransient<dynamic>();


            builder.Services.AddTransient<Func<dynamic, Application.Services.IWorkOrderDetailServices<CallContext>>>(WODServiceFactorySqlite);

            CalibrationSaaS.Infraestructure.Blazor.Services.ConnectionStatusService.grpcUrl = AddressEnnpoint;//builder.Configuration["Kestrel:Endpoints:Http2:Url"];


            builder.Services.AddTransient(services =>
            {
                //// Create a gRPC-Web channel pointing to the backend server
                //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                //var channel = Grpc.Net.Client.GrpcChannel.ForAddress(builder.Configuration["Kestrel:Endpoints:Http2:Url"], new GrpcChannelOptions { HttpClient = httpClient });
                ////var channel = Grpc.Net.Client.GrpcChannel.ForAddress("https://calsaasgrpc.azurewebsites.net", new GrpcChannelOptions { HttpClient = httpClient });

                //// Now we can instantiate gRPC clients for this channel
                //return channel.CreateGrpcService<Application.Services.IFileUpload>();

                return CreateService<Application.Services.IFileUpload>(AddressEnnpoint);
            });


            if (!AppSecurity.IsReport)
            {
                builder.Services.AddTransient(services =>
                {
                    return CreateService<Application.Services.IWorkOrderDetailServices<CallContext>>(AddressEnnpoint);

                });

                builder.Services.AddTransient(services =>
                {


                    return CreateService<Application.Services.IUOMService<CallContext>>(AddressEnnpoint);


                });

                builder.Services.AddTransient(services =>
                {

                    return CreateService<Application.Services.ICustomerService<CallContext>>(AddressEnnpoint);

                });





                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //builder.Services.AddTransient<Func<IIndexedDbFactory, Application.Services.ISampleService<CallContext>>>(SampleServiceFactory);

                //builder.Services.AddTransient<Func<IIndexedDbFactory, Application.Services.ISampleService2<CallContext>>>(SampleService2Factory);

                //builder.Services.AddTransient<Func<IIndexedDbFactory, Application.Services.IWorkOrderDetailServices<CallContext>>>(WODServiceFactory);

                //builder.Services.AddTransient<Func<IIndexedDbFactory, Application.Services.IBasicsServices<CallContext>>>(BasicsServiceFactory);

                //builder.Services.AddTransient<Func<IIndexedDbFactory, Application.Services.ISampleService<CallContext>>>(SampleServiceFactory);

                //builder.Services.AddTransient<Func<IIndexedDbFactory, Application.Services.IWorkOrderService<CallContext>>>(WorkOrderServiceFactory);
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                builder.Services.AddTransient<ISampleService2<CallContext>, Services.Offline.Sqlite.SampleService2Offline<Services.Offline.Sqlite.CalibrationSaaSDBContextOff2>>();


                builder.Services.AddTransient<Func<dynamic, Application.Services.ISampleService2<CallContext>>>(SampleService2FactorySqlite);



                builder.Services.AddTransient<Func<dynamic, Application.Services.ICustomerService<CallContext>>>(CustomerServiceFactorySqlite);

                builder.Services.AddTransient<Func<dynamic, Application.Services.IBasicsServices<CallContext>>>(BasicsServiceFactorySqlite);

                builder.Services.AddTransient<Func<dynamic, Application.Services.IPieceOfEquipmentService<CallContext>>>(POEServiceFactorySqlite);

                builder.Services.AddTransient<Func<dynamic, Application.Services.IUOMService<CallContext>>>(UOMServiceFactorySqlite);


                builder.Services.AddTransient<Func<dynamic, Application.Services.IAssetsServices<CallContext>>>(AssetsServiceFactorySqlite);


                builder.Services.AddTransient<Func<dynamic, Application.Services.IAddressServices>>(AddressServiceFactorySqlite);



                builder.Services.AddTransient(services =>
                {
                    //// Create a gRPC-Web channel pointing to the backend server
                    //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                    //var channel = Grpc.Net.Client.GrpcChannel.ForAddress(builder.Configuration["Kestrel:Endpoints:Http2:Url"], new GrpcChannelOptions { HttpClient = httpClient });

                    //// Now we can instantiate gRPC clients for this channel
                    //return channel.CreateGrpcService<Application.Services.IReportService<CallContext>>();

                    return CreateService<Application.Services.IReportService<CallContext>>(AddressEnnpoint);
                });



                builder.Services.AddTransient(services =>
                {
                    //// Create a gRPC-Web channel pointing to the backend server
                    //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                    //var channel = Grpc.Net.Client.GrpcChannel.ForAddress(builder.Configuration["Kestrel:Endpoints:Http2:Url"], new GrpcChannelOptions { HttpClient = httpClient });
                    ////var channel = Grpc.Net.Client.GrpcChannel.ForAddress("https://calsaasgrpc.azurewebsites.net", new GrpcChannelOptions { HttpClient = httpClient });

                    //// Now we can instantiate gRPC clients for this channel
                    //return channel.CreateGrpcService<Application.Services.IAddressServices>();


                    return CreateService<Application.Services.IAddressServices>(AddressEnnpoint);
                });

                builder.Services.AddTransient(services =>
                {


                    return CreateService<Application.Services.IBasicsServices<CallContext>>(AddressEnnpoint);

                });

                builder.Services.AddTransient(services =>
                {

                    return CreateService<Application.Services.ICustomerAddressService>(AddressEnnpoint);

                });

                builder.Services.AddTransient(services =>
                {


                    return CreateService<Application.Services.IPieceOfEquipmentService<CallContext>>(AddressEnnpoint);

                });
                builder.Services.AddTransient(services =>
                {
                    //// Create a gRPC-Web channel pointing to the backend server
                    //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                    //var channel = Grpc.Net.Client.GrpcChannel.ForAddress(builder.Configuration["Kestrel:Endpoints:Http2:Url"], new GrpcChannelOptions { HttpClient = httpClient });
                    ////var channel = Grpc.Net.Client.GrpcChannel.ForAddress("https://calsaasgrpc.azurewebsites.net", new GrpcChannelOptions { HttpClient = httpClient });

                    //// Now we can instantiate gRPC clients for this channel
                    //return channel.CreateGrpcService<Application.Services.IWorkOrderService<CallContext>>();

                    return CreateService<Application.Services.IWorkOrderService<CallContext>>(AddressEnnpoint);

                });

                builder.Services.AddTransient(services =>
                {
                    //// Create a gRPC-Web channel pointing to the backend server
                    //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                    //var channel = Grpc.Net.Client.GrpcChannel.ForAddress(builder.Configuration["Kestrel:Endpoints:Http2:Url"], new GrpcChannelOptions { HttpClient = httpClient });
                    ////var channel = Grpc.Net.Client.GrpcChannel.ForAddress("https://calsaasgrpc.azurewebsites.net", new GrpcChannelOptions { HttpClient = httpClient });

                    //// Now we can instantiate gRPC clients for this channel
                    //return channel.CreateGrpcService<Application.Services.IAssetsServices<CallContext>>();

                    return CreateService<Application.Services.IAssetsServices<CallContext>>(AddressEnnpoint);


                });
                


                //services between pages
                //https://wellsb.com/csharp/aspnet/blazor-singleton-pass-data-between-pages/#:~:text=Blazor%20Singleton%20Pass%20Data%20between%20Pages%20The%20best,onto%20the%20pages%20or%20components%20that%20need%20it.
                builder.Services.AddSingleton<Reports.Domain.ReportViewModels.Repeatability>();
                builder.Services.AddSingleton<Reports.Domain.ReportViewModels.Header>();
                builder.Services.AddSingleton<Reports.Domain.ReportViewModels.PointDecresingNoCorner>();
                builder.Services.AddSingleton<Reports.Domain.ReportViewModels.AsFound>();
                builder.Services.AddSingleton<Reports.Domain.ReportViewModels.AsLeft>();
                builder.Services.AddSingleton<Reports.Domain.ReportViewModels.Excentricity>();
                builder.Services.AddSingleton<Reports.Domain.ReportViewModels.ExcentricityDet>();
                builder.Services.AddSingleton<Reports.Domain.ReportViewModels.Repeteab>();
                builder.Services.AddSingleton<Reports.Domain.ReportViewModels.CornerloadUncert>();
                builder.Services.AddSingleton<Reports.Domain.ReportViewModels.CornerloadUncertComp>();
                builder.Services.AddSingleton<Reports.Domain.ReportViewModels.EnvironmentalUncert>();
                builder.Services.AddSingleton<Reports.Domain.ReportViewModels.EnvironmentalUncertComp>();
                builder.Services.AddSingleton<Reports.Domain.ReportViewModels.RepeteabilityUncert>();
                builder.Services.AddSingleton<Reports.Domain.ReportViewModels.RepeteabilityUncertComp>();
                builder.Services.AddSingleton<Reports.Domain.ReportViewModels.ResolutionUncert>();
                builder.Services.AddSingleton<Reports.Domain.ReportViewModels.ResolutionUncertComp>();
                builder.Services.AddSingleton<Reports.Domain.ReportViewModels.UcertaintyUncert>();
                builder.Services.AddSingleton<Reports.Domain.ReportViewModels.UcertaintyUncertComp>();
                builder.Services.AddSingleton<Reports.Domain.ReportViewModels.Sticker>();
                builder.Services.AddSingleton<Domain.Aggregates.ValueObjects.EquipmentTypeResultSet>();
                builder.Services.AddSingleton<Domain.Aggregates.ValueObjects.PieceOfEquipmentResultSet>();
                builder.Services.AddSingleton<Domain.Aggregates.ValueObjects.AddressResultSet>();
                builder.Services.AddSingleton<Domain.Aggregates.ValueObjects.UserResultSet>();
                


            }


            builder.Services.AddSingleton<Domain.Aggregates.Shared.Basic.AppState>();
            builder.Services.AddSingleton<Domain.Aggregates.Shared.AppStateCompany>();





            builder.Services.AddScoped<Blazed.Controls.Toast.IToastService, Blazed.Controls.Toast.ToastService>();

            builder.Services.AddBlazoredModal();

            //builder.Services.AddScoped<IModalHandler, ModalHandler>();
            //builder.Services.AddScoped<IDynamicModalHandler, DynamicModalHandler>();
            builder.Services.AddScoped<BrowserService>();
            

            //builder.Services.AddCECRouting();

            builder.Services.AddBlazorDragDrop();

            builder.Services.AddRouter();

           



            if (AppSecurity.RolesList == null)
            {
                if (AppSecurity.IsNotGrpc)
                {

                    AppSecurity.RolesList = GetRoles().Roles; ;
                }
                else
                {
                    var a = await Roles(builder);
                    AppSecurity.RolesList = a.Roles;
                }
              

                var s = "";

                if (AppSecurity.RolesList != null)
                {
                    foreach (var item in AppSecurity.RolesList)
                    {
                        s = s + item.Name + ",";
                    }
                }

                if (!string.IsNullOrEmpty(s))
                {
                    s = s.Substring(0, s.LastIndexOf(","));
                }

                roles = s;

            }

            if (AppSecurity.Components == null && !AppSecurity.IsReport && !AppSecurity.IsNotGrpc)
            {
                try
                {
                    AppSecurity.Components = await GetComponents(builder, AddressEnnpoint);
                    if(CustomSequences != null)
                    {
                        AppSecurity.CustomSequences = CustomSequences;


                    }
                   

                   
                }
                catch
                {

                }
                

            }

            //builder.Services.AddAuthorizationCore(config =>
            //{

            //config.AddPolicy("HasAccess", policy =>

            //policy.Requirements.Add(new AccesRequirement(roles))

            //    ) ;

            //    //config.AddPolicy("OnlyView", policy =>
            //    //policy.Requirements.Add(new TechRequirement("tech")));
            //});
            if (AppSecurity.IsNotGrpc)
            {
                ////////////////////////////////////////////////////////////////////////////////////////
                builder.Services.AddAuthorizationCore();
                builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
                ////////////////////////////////////////////////////////////////////////////////////////////////

            }

            builder.Services.AddCascadingAuthenticationState();

            builder.Services.AddScoped<CustomAuthenticationService>();

            builder.Services.AddAuthorizationCore(o =>
            {
                o.AddPolicy("HasAccess", policy =>
                {  //policy.RequireAuthenticatedUser();
                    //policy.Requirements.Add(new AccesRequirement2(AppSecurity.RolesList));
                    policy.Requirements.Add(new AccesRequirement(roles,null));
                });
            });

            //builder.Services.AddScoped<AuthenticationStateProvider, TestAuthStateProvider>();

            builder.Services.AddTransient<IAuthorizationHandler, AccesHandler>();



            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();


            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Transient<IPostConfigureOptions<RemoteAuthenticationOptions<ApiAuthorizationProviderOptions>>,
                    ApiAuthorizationOptionsConfiguration>());

            builder.Services.AddSingleton<Domain.Aggregates.Shared.AppSecurity>(AppSecurity);



            if (Thread.CurrentThread.CurrentCulture.Name != "en-US")
            {
                // If current culture is not fr-FR, set culture to fr-FR.
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");
            }
            else
            {
                // Set culture to en-US.
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");
            }
            ThreadProc();




            
            //Add custom application services
            //builder.Services.AddScoped<StateFacade>();


            //builder.Services.AddTransient<Blazed.Controls.IStateFacade, StateFacade>();

           //builder.Services.AddTransient<IResponsiveTableService, ResponsiveTableService>();


            // Add Fluxor
            builder.Services.AddFluxor(options =>
            {
                options.ScanAssemblies(Assembly.GetExecutingAssembly());
                options.UseRouting();
                options.UseReduxDevTools();
            });



            builder.Services.AddScoped<LazyAssemblyLoader>();

            builder.Services.AddLocalization();

            builder.Services.AddTransient<IClaimsTransformation, AddRolesClaimsTransformation>();

            builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

            //builder.Services.AddScoped<ISqliteWasmDbContextFactory<CalibrationSaaSDBContextOff2>, SqliteWasmDbContextFactory<CalibrationSaaSDBContextOff2>>();

            

            

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            /////Offline
            ///

            string path = $"Data Source={DatabaseService2<CalibrationSaaSDBContextOff2>.FileName};Foreign Keys=False";

            builder.Services.AddSqliteWasmDbContextFactory<CalibrationSaaSDBContextOff2>(
            opts => opts.UseSqlite(path));

            builder.Services.AddSingleton<DatabaseService2<CalibrationSaaSDBContextOff2>>();

            //JSInProcRuntime = (IJSInProcessRuntime)builder.Services.GetRequiredService<IJSRuntime>();

            //JSInProcRuntime = (IJSInProcessRuntime)builder.Services.BuildServiceProvider().GetRequiredService<IJSRuntime>();

            //builder.Services.AddSingleton<IJSInProcessRuntime>(JSInProcRuntime);

            //

            if (1 == 2)
            {
                builder.Services.AddContributionsFeature();
            }
            builder.Services.AddScoped<CacheStorageAccessor>();

            builder.Services.AddScoped<MyLoggerFactory>();
            ////////////////////////////////////////
            var b= builder.Build();


            JSInProcRuntime = (IJSInProcessRuntime)b.Services.GetRequiredService<IJSRuntime>();
            ////offline
            if (1 == 2)
            {
                await b.InitializeContributionsFeature();
            }



            //////////////////////////////////////////////////////////////
            //using (var scope = b.Services.CreateScope())

            //{
            //    var services = scope.ServiceProvider;

            //    var ca = services.GetService<AccesRequirement>();

            //    ca.Requirement = "test";


            //}
            //builder.Services.AddAuthorizationCore(config =>
            //{

            //    config.AddPolicy("HasAccess", policy =>
            //    policy.Requirements.Add(new AccesRequirement(roles)));

            //    //config.AddPolicy("OnlyView", policy =>
            //    //policy.Requirements.Add(new TechRequirement("tech")));
            //});
            //builder.Services.AddMultiTenant<SekureTenantInfo>().WithConfigurationStore().WithHostStrategy();
            //builder.Services.AddMultiTenant<TenantInfo>();
            //.WithBasePathStrategy(options => options.RebaseAspNetCorePathBase = true)

            //await b.SetDefaultCulture();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;

            Console.WriteLine("Timer " + elapsedMs);

            await b.RunAsync();

        }


  //      private static Func<IServiceProvider, Func<IIndexedDbFactory, Application.Services.IWorkOrderService<CallContext>>> WorkOrderServiceFactory =>
  //service =>
  //{
  //    return database =>
  //    {
  //        Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
  //        if (1 == 2)//online
  //        {
  //            //Create a gRPC - Web channel pointing to the backend server

  //            var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));

  //            var channel = Grpc.Net.Client.GrpcChannel.ForAddress(ConnectionStatusService.grpcUrl, new GrpcChannelOptions { HttpClient = httpClient });

  //            //Now we can instantiate gRPC clients for this channel
  //            return channel.CreateGrpcService<Application.Services.IWorkOrderService<CallContext>>();
  //        }
  //        else
  //        {
  //            //ILocalStorageService localStorageService = new LocalStorageService();

  //            return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.WorkOrderServiceOffline(database);
  //        }

  //        throw new ArgumentException("Could not determine location");
  //    };
  //};



    //    private static Func<IServiceProvider, Func<IIndexedDbFactory, Application.Services.IAssetsServices<CallContext>>> AssetsServiceFactory =>
    //service =>
    //{
    //    return database =>
    //    {
    //        Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
    //        if (ConnectionStatusService.GetCurrentStatus())//online
    //        {
    //            //Create a gRPC - Web channel pointing to the backend server
    //            var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                
                
    //            var channel = Grpc.Net.Client.GrpcChannel.ForAddress(ConnectionStatusService.grpcUrl, new GrpcChannelOptions { HttpClient = httpClient, });

    //            //Now we can instantiate gRPC clients for this channel
    //            return channel.CreateGrpcService<Application.Services.IAssetsServices<CallContext>>();
    //        }
    //        else
    //        {
    //            //ILocalStorageService localStorageService = new LocalStorageService();

    //            return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.AssetsServiceOffline(database);
    //        }

    //        throw new ArgumentException("Could not determine location");
    //    };
    //};


        private static Func<IServiceProvider, Func<dynamic, Application.Services.IAddressServices>> AddressServiceFactorySqlite =>
 service =>
 {
     return database =>
     {
         Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
         if (ConnectionStatusService.GetCurrentStatus())//online
         {
             ////Create a gRPC - Web channel pointing to the backend server
             //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
             //var channel = Grpc.Net.Client.GrpcChannel.ForAddress(ConnectionStatusService.grpcUrl, new GrpcChannelOptions { HttpClient = httpClient });

             ////Now we can instantiate gRPC clients for this channel
             //return channel.CreateGrpcService<Application.Services.ICustomerService<CallContext>>();

             return CreateService<Application.Services.IAddressServices>(ConnectionStatusService.grpcUrl);

            
         }
         else
         {
             //ILocalStorageService localStorageService = new LocalStorageService();

             return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.AddressServiceOffline<CalibrationSaaSDBContextOff2>(database);
         }

         throw new ArgumentException("Could not determine location");
     };
 };



        private static Func<IServiceProvider, Func<dynamic
           , Application.Services.IAssetsServices<CallContext>>> AssetsServiceFactorySqlite =>
  service =>
  {
      return database =>
      {
          Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
          if (ConnectionStatusService.GetCurrentStatus())//online
          {
              ////Create a gRPC - Web channel pointing to the backend server
              //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
              //var channel = Grpc.Net.Client.GrpcChannel.ForAddress(ConnectionStatusService.grpcUrl, new GrpcChannelOptions { HttpClient = httpClient });

              ////Now we can instantiate gRPC clients for this channel
              //return channel.CreateGrpcService<Application.Services.IAssetsServices<CallContext>>();

              return CreateService<Application.Services.IAssetsServices<CallContext>>(ConnectionStatusService.grpcUrl);


          }
          else
          {
              //ILocalStorageService localStorageService = new LocalStorageService();

              //ISqliteWasmDbContextFactory<CalibrationSaaSDBContextOff2> Factory=new SqliteWasmDbContextFactory(database,)


              return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.AssetsServiceOffline<CalibrationSaaSDBContextOff2>(database);
          }

          throw new ArgumentException("Could not determine location");
      };
  };


        private static Func<IServiceProvider, Func<dynamic, Application.Services.ICustomerService<CallContext>>> CustomerServiceFactorySqlite =>
service =>
{
    return database =>
    {
        Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
        if (ConnectionStatusService.GetCurrentStatus())//online
        {
            ////Create a gRPC - Web channel pointing to the backend server
            //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
            //var channel = Grpc.Net.Client.GrpcChannel.ForAddress(ConnectionStatusService.grpcUrl, new GrpcChannelOptions { HttpClient = httpClient });

            ////Now we can instantiate gRPC clients for this channel
            //return channel.CreateGrpcService<Application.Services.ICustomerService<CallContext>>();

            return CreateService<Application.Services.ICustomerService<CallContext>>(ConnectionStatusService.grpcUrl);

        }
        else
        {
            //ILocalStorageService localStorageService = new LocalStorageService();

            return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.CustomerServiceOffline<CalibrationSaaSDBContextOff2>(database);
        }

        throw new ArgumentException("Could not determine location");
    };
};
        private static Func<IServiceProvider, Func<dynamic, Application.Services.ISampleService2<CallContext>>> SampleService2FactorySqlite =>
    service =>
    {
        return database =>
        {
            Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
            if (1 == 2)//online
            {
                //Create a gRPC - Web channel pointing to the backend server
                var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                httpClient.Timeout = TimeSpan.FromSeconds(180);
                var channel = Grpc.Net.Client.GrpcChannel.ForAddress(ConnectionStatusService.grpcUrl, new GrpcChannelOptions { HttpClient = httpClient });

                //Now we can instantiate gRPC clients for this channel
                return channel.CreateGrpcService<Application.Services.ISampleService2<CallContext>>();
            }
            else
            {
                //ILocalStorageService localStorageService = new LocalStorageService();

                //var db= new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.SampleService2Offline();


                return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.SampleService2Offline<CalibrationSaaSDBContextOff2>(database);
            }

            throw new ArgumentException("Could not determine location");
        };
    };


        private static Func<IServiceProvider, Func<dynamic, Application.Services.IWorkOrderDetailServices<CallContext>>> WODServiceFactorySqlite =>
  service =>
  {
      return database =>
      {
          Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
          if (ConnectionStatusService.GetCurrentStatus())//online
          {
              ////Create a gRPC - Web channel pointing to the backend server
              //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
              //var channel = Grpc.Net.Client.GrpcChannel.ForAddress(ConnectionStatusService.grpcUrl, new GrpcChannelOptions { HttpClient = httpClient });

              ////Now we can instantiate gRPC clients for this channel
              //return channel.CreateGrpcService<Application.Services.IWorkOrderDetailServices<CallContext>>();
              return CreateService<Application.Services.IWorkOrderDetailServices<CallContext>>(ConnectionStatusService.grpcUrl);


          }
          else
          {
              //ILocalStorageService localStorageService = new LocalStorageService();

              return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.WODServiceOffline<CalibrationSaaSDBContextOff2>(database);
          }

          throw new ArgumentException("Could not determine location");
      };
  };


        private static Func<IServiceProvider, Func<dynamic, Application.Services.IBasicsServices<CallContext>>> BasicsServiceFactorySqlite =>
service =>
{
    return database =>
    {
        Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
        if (ConnectionStatusService.GetCurrentStatus())//online
        {
            ////Create a gRPC - Web channel pointing to the backend server
            //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
            //var channel = Grpc.Net.Client.GrpcChannel.ForAddress(ConnectionStatusService.grpcUrl, new GrpcChannelOptions { HttpClient = httpClient });

            ////Now we can instantiate gRPC clients for this channel
            //return channel.CreateGrpcService<Application.Services.IBasicsServices<CallContext>>();
            return CreateService<Application.Services.IBasicsServices<CallContext>>(ConnectionStatusService.grpcUrl);


        }
        else
        {
            //ILocalStorageService localStorageService = new LocalStorageService();

            //var db= new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.BasicsServiceOffline();

            return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.BasicsServiceOffline<CalibrationSaaSDBContextOff2>(database);
        }

        throw new ArgumentException("Could not determine location");
    };
};


        private static Func<IServiceProvider, Func<dynamic, Application.Services.IPieceOfEquipmentService<CallContext>>> POEServiceFactorySqlite =>
service =>
{
    return database =>
    {
        Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
        if (ConnectionStatusService.GetCurrentStatus())//online
        {
            ////Create a gRPC - Web channel pointing to the backend server
            //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
            //var channel = Grpc.Net.Client.GrpcChannel.ForAddress(ConnectionStatusService.grpcUrl, new GrpcChannelOptions { HttpClient = httpClient });

            ////Now we can instantiate gRPC clients for this channel
            //return channel.CreateGrpcService<Application.Services.IPieceOfEquipmentService<CallContext>>();
            return CreateService<Application.Services.IPieceOfEquipmentService<CallContext>>(ConnectionStatusService.grpcUrl);


        }
        else
        {
            //ILocalStorageService localStorageService = new LocalStorageService();

            return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.POEServiceOffline<CalibrationSaaSDBContextOff2>(database);
        }

        throw new ArgumentException("Could not determine location");
    };
};


        private static Func<IServiceProvider, Func<dynamic, Application.Services.IUOMService<CallContext>>> UOMServiceFactorySqlite =>
service =>
{
    return database =>
    {
        Console.WriteLine("UOMServiceFactorySqlite");
        Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
        Console.WriteLine(ConnectionStatusService.grpcUrl);

        if (ConnectionStatusService.GetCurrentStatus())//online
        {
            ////Create a gRPC - Web channel pointing to the backend server
            //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
            //var channel = Grpc.Net.Client.GrpcChannel.ForAddress(ConnectionStatusService.grpcUrl, new GrpcChannelOptions { HttpClient = httpClient });

            ////Now we can instantiate gRPC clients for this channel
            //return channel.CreateGrpcService<Application.Services.IUOMService<CallContext>>();
            return CreateService<Application.Services.IUOMService<CallContext>>(ConnectionStatusService.grpcUrl);
        }
        else
        {
            //ILocalStorageService localStorageService = new LocalStorageService();

            return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.UOMServiceOffline<CalibrationSaaSDBContextOff2>(database);
        }

        throw new ArgumentException("Could not determine location");
    };
};



        //        private static Func<IServiceProvider, Func<IIndexedDbFactory, Application.Services.IWorkOrderService<CallContext>>> WorkOrderServiceFactory =>
        //  service =>
        //  {
        //      return database =>
        //      {
        //          Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
        //          if (1 == 2)//online
        //          {
        //              //Create a gRPC - Web channel pointing to the backend server

        //              var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));

        //              var channel = Grpc.Net.Client.GrpcChannel.ForAddress(ConnectionStatusService.grpcUrl, new GrpcChannelOptions { HttpClient = httpClient });

        //              //Now we can instantiate gRPC clients for this channel
        //              return channel.CreateGrpcService<Application.Services.IWorkOrderService<CallContext>>();
        //          }
        //          else
        //          {
        //              //ILocalStorageService localStorageService = new LocalStorageService();

        //              return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.WorkOrderServiceOffline(database);
        //          }

        //          throw new ArgumentException("Could not determine location");
        //      };
        //  };



        //        private static Func<IServiceProvider, Func<IIndexedDbFactory, Application.Services.IAssetsServices<CallContext>>> AssetsServiceFactory =>
        //    service =>
        //    {
        //        return database =>
        //        {
        //            Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
        //            if (ConnectionStatusService.GetCurrentStatus())//online
        //            {
        //                //Create a gRPC - Web channel pointing to the backend server
        //                var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));


        //                var channel = Grpc.Net.Client.GrpcChannel.ForAddress(ConnectionStatusService.grpcUrl, new GrpcChannelOptions { HttpClient = httpClient, });

        //                //Now we can instantiate gRPC clients for this channel
        //                return channel.CreateGrpcService<Application.Services.IAssetsServices<CallContext>>();
        //            }
        //            else
        //            {
        //                //ILocalStorageService localStorageService = new LocalStorageService();

        //                return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.AssetsServiceOffline(database);
        //            }

        //            throw new ArgumentException("Could not determine location");
        //        };
        //    };


        //        private static Func<IServiceProvider, Func<IIndexedDbFactory, Application.Services.ICustomerService<CallContext>>> CustomerServiceFactory =>
        // service =>
        // {
        //     return database =>
        //     {
        //         Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
        //         if (ConnectionStatusService.GetCurrentStatus())//online
        //         {
        //             //Create a gRPC - Web channel pointing to the backend server
        //             var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
        //             var channel = Grpc.Net.Client.GrpcChannel.ForAddress(ConnectionStatusService.grpcUrl, new GrpcChannelOptions { HttpClient = httpClient });

        //             //Now we can instantiate gRPC clients for this channel
        //             return channel.CreateGrpcService<Application.Services.ICustomerService<CallContext>>();
        //         }
        //         else
        //         {
        //             //ILocalStorageService localStorageService = new LocalStorageService();

        //             return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.CustomerServiceOffline(database);
        //         }

        //         throw new ArgumentException("Could not determine location");
        //     };
        // };



        //        private static Func<IServiceProvider, Func<IIndexedDbFactory, Application.Services.ISampleService2<CallContext>>> SampleService2Factory =>
        //     service =>
        //     {
        //         return database =>
        //         {
        //             Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
        //             if (1 == 2)//online
        //             {
        //                 //Create a gRPC - Web channel pointing to the backend server
        //#pragma warning disable CS0162 // Se detectó código inaccesible
        //                 var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
        //#pragma warning restore CS0162 // Se detectó código inaccesible
        //                 var channel = Grpc.Net.Client.GrpcChannel.ForAddress(ConnectionStatusService.grpcUrl, new GrpcChannelOptions { HttpClient = httpClient });

        //                 //Now we can instantiate gRPC clients for this channel
        //                 return channel.CreateGrpcService<Application.Services.ISampleService2<CallContext>>();
        //             }
        //             else
        //             {
        //                 //ILocalStorageService localStorageService = new LocalStorageService();

        //                 return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.SampleService2Offline(database);
        //             }

        //             throw new ArgumentException("Could not determine location");
        //         };
        //     };

        //        private static Func<IServiceProvider, Func<IIndexedDbFactory, Application.Services.ISampleService<CallContext>>> SampleServiceFactory =>
        //      service =>
        //      {
        //          return database =>
        //          {
        //              Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
        //              if (ConnectionStatusService.GetCurrentStatus())//online
        //              {
        //                  //Create a gRPC - Web channel pointing to the backend server
        //                  var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
        //                  var channel = Grpc.Net.Client.GrpcChannel.ForAddress(ConnectionStatusService.grpcUrl, new GrpcChannelOptions { HttpClient = httpClient });

        //                  //Now we can instantiate gRPC clients for this channel
        //                  return channel.CreateGrpcService<Application.Services.ISampleService<CallContext>>();
        //              }
        //              else
        //              {
        //                  //ILocalStorageService localStorageService = new LocalStorageService();

        //                  return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.SampleServiceOffline(database);
        //              }

        //              throw new ArgumentException("Could not determine location");
        //          };
        //      };

        //        private static Func<IServiceProvider, Func<IIndexedDbFactory, Application.Services.IWorkOrderDetailServices<CallContext>>> WODServiceFactory =>
        //    service =>
        //    {
        //        return database =>
        //        {
        //            Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
        //            if (ConnectionStatusService.GetCurrentStatus())//online
        //            {
        //                //Create a gRPC - Web channel pointing to the backend server
        //                var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
        //                httpClient.Timeout = TimeSpan.FromSeconds(180);
        //                var channel = Grpc.Net.Client.GrpcChannel.ForAddress(ConnectionStatusService.grpcUrl, new GrpcChannelOptions { HttpClient = httpClient });

        //                //Now we can instantiate gRPC clients for this channel
        //                return channel.CreateGrpcService<Application.Services.IWorkOrderDetailServices<CallContext>>();
        //            }
        //            else
        //            {
        //                //ILocalStorageService localStorageService = new LocalStorageService();

        //                return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.WODServiceOffline(database);
        //            }

        //            throw new ArgumentException("Could not determine location");
        //        };
        //    };


        //        private static Func<IServiceProvider, Func<IIndexedDbFactory, Application.Services.IBasicsServices<CallContext>>> BasicsServiceFactory =>
        //service =>
        //{
        //    return database =>
        //    {
        //        Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
        //        if (ConnectionStatusService.GetCurrentStatus())//online
        //        {
        //            //Create a gRPC - Web channel pointing to the backend server
        //            var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
        //            var channel = Grpc.Net.Client.GrpcChannel.ForAddress(ConnectionStatusService.grpcUrl, new GrpcChannelOptions { HttpClient = httpClient });

        //            //Now we can instantiate gRPC clients for this channel
        //            return channel.CreateGrpcService<Application.Services.IBasicsServices<CallContext>>();
        //        }
        //        else
        //        {
        //            //ILocalStorageService localStorageService = new LocalStorageService();

        //            return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.BasicsServiceOffline(database);
        //        }

        //        throw new ArgumentException("Could not determine location");
        //    };
        //};


        //        private static Func<IServiceProvider, Func<IIndexedDbFactory, Application.Services.IPieceOfEquipmentService<CallContext>>> POEServiceFactory =>
        //service =>
        //{
        //    return database =>
        //    {
        //        Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
        //        if (ConnectionStatusService.GetCurrentStatus())//online
        //        {
        //            //Create a gRPC - Web channel pointing to the backend server
        //            var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
        //            httpClient.Timeout = TimeSpan.FromSeconds(180);
        //            var channel = Grpc.Net.Client.GrpcChannel.ForAddress(ConnectionStatusService.grpcUrl, new GrpcChannelOptions { HttpClient = httpClient });

        //            //Now we can instantiate gRPC clients for this channel
        //            return channel.CreateGrpcService<Application.Services.IPieceOfEquipmentService<CallContext>>();
        //        }
        //        else
        //        {
        //            //ILocalStorageService localStorageService = new LocalStorageService();

        //            return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.POEServiceOffline(database);
        //        }

        //        throw new ArgumentException("Could not determine location");
        //    };
        //};


        //        private static Func<IServiceProvider, Func<IIndexedDbFactory, Application.Services.IUOMService<CallContext>>> UOMServiceFactory =>
        //service =>
        //{
        //    return database =>
        //    {
        //        Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
        //        if (ConnectionStatusService.GetCurrentStatus())//online
        //        {
        //            //Create a gRPC - Web channel pointing to the backend server
        //            var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
        //            var channel = Grpc.Net.Client.GrpcChannel.ForAddress(ConnectionStatusService.grpcUrl, new GrpcChannelOptions { HttpClient = httpClient });

        //            //Now we can instantiate gRPC clients for this channel
        //            return channel.CreateGrpcService<Application.Services.IUOMService<CallContext>>();
        //        }
        //        else
        //        {
        //            //ILocalStorageService localStorageService = new LocalStorageService();

        //            return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.UOMServiceOffline(database);
        //        }

        //        throw new ArgumentException("Could not determine location");
        //    };
        //};


    }

    //    public async Task<string> CallApi()
    //{
    //    var accessToken = await HttpContext.GetTokenAsync("access_token");

    //    var client = new HttpClient();
    //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    //    var content = await client.GetStringAsync("https://localhost:6001/identity");

    //        //ViewBag.Json = JArray.Parse(content).ToString();
    //        //return View("json");
    //        return null;
    //}




}

public static class WebAssemblyHostExtension
{
    public async static Task SetDefaultCulture(this WebAssemblyHost host)
    {
        var jsInterop = host.Services.GetRequiredService<IJSRuntime>();

        //var js = (IJSInProcessRuntime)JSRuntime;
        //       js.InvokeVoid("blazorCulture.set", value.Name);


        CultureInfo culture;

        await jsInterop.InvokeVoidAsync("blazorCulture.set", "en-US"); ;

        var result = await jsInterop.InvokeAsync<string>("blazorCulture.get");
        if (result != null)
            //culture = new CultureInfo(result);
            culture = new CultureInfo("en-US");
        else
            culture = new CultureInfo("en-US");

        culture = new CultureInfo("en-US");

        DateTimeFormatInfo dateformat = new DateTimeFormatInfo();


        dateformat.ShortDatePattern = "MM/dd/yyyy";
        dateformat.FullDateTimePattern = "dddd, mmmm dd, yyyy h:mm:ss tt";// Date format of en-us

        culture.NumberFormat.NumberDecimalSeparator = ".";

        culture.DateTimeFormat = dateformat;



        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}


public class OfflineAccountClaimsPrincipalFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
{
    private readonly IServiceProvider services;


    public OfflineAccountClaimsPrincipalFactory(IServiceProvider services, IAccessTokenProviderAccessor accessor) : base(accessor)
    {
        this.services = services;
    }


    public override async ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
    {
        //var localVehiclesStore = services.GetRequiredService<LocalVehiclesStore>();


        var result = await base.CreateUserAsync(account, options);
        if (result.Identity.IsAuthenticated)
        {
            //await localVehiclesStore.SaveUserAccountAsync(result);
        }
        else
        {
            //result = await localVehiclesStore.LoadUserAccountAsync();
        }

        return result;
    }
}

public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public CustomAuthorizationMessageHandler(IAccessTokenProvider provider,
        NavigationManager navigationManager)
        : base(provider, navigationManager)
    {
        //ConfigureHandler(
        //    authorizedUrls: new[] { "http://localhost:7071" },
        //    scopes: new[] { "access_as_user" });
    }
}

//public class TestAuthStateProvider : AuthenticationStateProvider
//{
//    public override Task<AuthenticationState> GetAuthenticationStateAsync()
//    {
//        var identity = new ClaimsIdentity();
//        var user = new ClaimsPrincipal(identity);

//        return Task.FromResult(new AuthenticationState(user));
//    }
//    public void AuthenticateUser(string userIdentifier)
//    {
//        var identity = new ClaimsIdentity(new[]
//        {
//            new Claim(ClaimTypes.Name, userIdentifier),
//        }, "Custom Authentication");

//        var user = new ClaimsPrincipal(identity);

//        NotifyAuthenticationStateChanged(
//            Task.FromResult(new AuthenticationState(user)));
//    }
//}


public class CustomAuthenticationService
{
    public event Action<ClaimsPrincipal>? UserChanged;
    private ClaimsPrincipal? currentUser;

    public ClaimsPrincipal CurrentUser
    {
        get { return currentUser ?? new(); }
        set
        {
            currentUser = value;

            if (UserChanged is not null)
            {
                UserChanged(currentUser);
            }
        }
    }
}
public class CustomAuthStateProvider : AuthenticationStateProvider, IAccessTokenProvider
{
    private AuthenticationState authenticationState;

    public CustomAuthStateProvider(ClaimsPrincipal user)
    {
        authenticationState = new AuthenticationState(user);

        
        NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
       
    }

    public CustomAuthStateProvider(CustomAuthenticationService service)
    {
        authenticationState = new AuthenticationState(service.CurrentUser);

        service.UserChanged += (newUser) =>
        {
            authenticationState = new AuthenticationState(newUser);
            NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
        };
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
        Task.FromResult(authenticationState);

    public ValueTask<AccessTokenResult> RequestAccessToken()
    {
        return new(); // customize the result
    }

    public ValueTask<AccessTokenResult> RequestAccessToken(AccessTokenRequestOptions options)
    {
        return new(); // customize the result
    }
}


