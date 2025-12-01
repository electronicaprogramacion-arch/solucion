using Blazed.Controls;
using Blazed.Controls.Route.Services;
using BlazorApp1.Client.Security;
using Blazored.Modal;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Security;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite;
using CalibrationSaaS.Infraestructure.Blazor.Shared;
using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Helpers.Controls;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Client;
using Radzen;
using Security;
using SqliteWasmHelper;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;
using PrerenderRouteHelper = BlazorApp1.Client.Security.PrerenderRouteHelper;
namespace BlazorApp1.Client;

public class Program
{
    public static CalibrationSaaS.Domain.Aggregates.Shared.AppSecurity AppSecurity = new CalibrationSaaS.Domain.Aggregates.Shared.AppSecurity();
    public static IJSInProcessRuntime JSInProcRuntime;

    public static string _token;

    public static string roles = "";


    public static async Task<CalibrationSaaS.Domain.Aggregates.ValueObjects.RolResultSet> Roles(string endpoint)
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
        var a = channel.CreateGrpcService<CalibrationSaaS.Application.Services.IBasicsServices<CallContext>>();




        try
        {
            if (BlazorApp1.Client.Program.AppSecurity.IsReport || BlazorApp1.Client.Program.AppSecurity.IsNotGrpc)
            {
                return GetRoles();
            }

            var b = await a.GetRoles(CallContext.Default);
            return b;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex?.InnerException?.Message);
            Console.WriteLine(ex?.StackTrace);

            BlazorApp1.Client.Program.AppSecurity.IsNotGrpc = true;


            return GetRoles();


            //var c= new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.CustomerServiceOffline<CalibrationSaaSDBContextOff2>(database);
        }

    }


    public static bool HasConexion = true;

    public static bool IsOffline = false;
    public static async Task Main(string[] args)
    {

        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        string[] startupParams = null;

        var js = (IJSInProcessRuntime)builder.Services.BuildServiceProvider().GetRequiredService<IJSRuntime>();
        try
        {
            startupParams = js.Invoke<string[]>("startupParams");
        }
        finally
        {

        }

        try
        {
            var _IsOffline = js.Invoke<string>("isOffline.get");

            if (_IsOffline=="true")
            {
                IsOffline = true;
                HasConexion = false;
            }
            else
            {
                IsOffline = false;
                HasConexion = true;
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex?.InnerException?.Message);
            Console.WriteLine(ex?.StackTrace);
        }
        finally
        {

        }



        Console.WriteLine("program");
        Console.WriteLine(startupParams?.ElementAtOrDefault(0));
        Console.WriteLine(startupParams?.ElementAtOrDefault(1));
        Console.WriteLine(startupParams?.ElementAtOrDefault(2));


       
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddAuthenticationStateDeserialization(options =>
        {

            options.DeserializationCallback = (state) =>
            {

                List<Claim> claims = new List<Claim>();
                if (state.Claims != null)
                {
                    foreach (var claim in state.Claims)
                    {
                        claims.Add(new Claim(claim.Type, claim.Value));
                    }
                }
                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "WebAssembly"));

                var auth = new BlazorApp1.Client.Security.CustomAuthStateProvider(principal);
                if (claims.Count > 0)
                {
                    // Deserialize the state to a ClaimsPrincipal

                    
                    AppSecurity.Principal = principal;

                    //var app =Task.FromResult(auth.GetAuthenticationStateAsync());

                }

                var app1 = auth.GetAuthenticationStateAsync();

                return app1;
            };


        }
                       
        );



        builder.Services.AddRadzenComponents();
        builder.Services.AddRadzenCookieThemeService(options =>
        {
            options.Name = "CalibrifyTheme";
            options.Duration = TimeSpan.FromDays(365);
        });


        builder.Services.AddScoped<BlazorApp1.CacheStorage.Utils.CacheStorageAccessor>();


        // Configure HttpClient for server-side components
        builder.Services.AddHttpClient();

        //Add router services
        builder.Services.AddScoped<RouterSessionService>();


        builder.Services.AddSingleton<CalibrationSaaS.Domain.Aggregates.Shared.AppSecurity>(AppSecurity);


      

        builder.Services.AddScoped<IModalHandler, BlazorApp1.Client.Security.ModalHandler>();
        builder.Services.AddScoped<IDynamicModalHandler, BlazorApp1.Blazor.Blazor.Shared.DynamicModalHandler>();

        builder.Services.AddSingleton<CalibrationSaaS.Domain.Aggregates.Shared.Basic.AppState>();
        builder.Services.AddSingleton<CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany>();





        builder.Services.AddScoped<Blazed.Controls.Toast.IToastService, Blazed.Controls.Toast.ToastService>();

        builder.Services.AddBlazoredModal();






        string AddressEnnpoint = builder.Configuration["Service:Url"];

        BlazorApp1.Client.Program.AppSecurity.AssemblyName = builder.Configuration["VersionApp:Assembly"];

        if (BlazorApp1.Client.Program.AppSecurity.RolesList == null)
        {
            if (BlazorApp1.Client.Program.AppSecurity.IsNotGrpc)
            {

                BlazorApp1.Client.Program.AppSecurity.RolesList = GetRoles().Roles; ;
            }
            else
            {
                var a = await Roles(AddressEnnpoint);
                BlazorApp1.Client.Program.AppSecurity.RolesList = a.Roles;
            }


            var s = "";

            if (BlazorApp1.Client.Program.AppSecurity.RolesList != null)
            {
                foreach (var item in BlazorApp1.Client.Program.AppSecurity.RolesList)
                {
                    s = s + item.Name + ",";
                }
            }

            if (!string.IsNullOrEmpty(s))
            {
                s = s.Substring(0, s.LastIndexOf(","));
            }

            BlazorApp1.Client.Program.roles = s;

        }


        if(BlazorApp1.Client.Program.AppSecurity.IsNotGrpc)
        {


           HasConexion = false;
            IsOffline = true;

        }

        builder.Services.AddTransient<dynamic>();


        builder.Services.AddTransient<Func<dynamic, CalibrationSaaS.Application.Services.IWorkOrderDetailServices<CallContext>>>(WODServiceFactorySqlite);

        CalibrationSaaS.Infraestructure.Blazor.Services.ConnectionStatusService.grpcUrl = AddressEnnpoint;//builder.Configuration["Kestrel:Endpoints:Http2:Url"];



        builder.Services.AddTransient(services =>
        {
            //// Create a gRPC-Web channel pointing to the backend server
            //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
            //var channel = Grpc.Net.Client.GrpcChannel.ForAddress(builder.Configuration["Kestrel:Endpoints:Http2:Url"], new GrpcChannelOptions { HttpClient = httpClient });
            ////var channel = Grpc.Net.Client.GrpcChannel.ForAddress("https://calsaasgrpc.azurewebsites.net", new GrpcChannelOptions { HttpClient = httpClient });

            //// Now we can instantiate gRPC clients for this channel
            //return channel.CreateGrpcService<Application.Services.IFileUpload>();

            return CreateService<CalibrationSaaS.Application.Services.IFileUpload>(AddressEnnpoint);
        });


        if (!BlazorApp1.Client.Program.AppSecurity.IsReport)
        {
            builder.Services.AddTransient(services =>
            {
                return CreateService<CalibrationSaaS.Application.Services.IWorkOrderDetailServices<CallContext>>(AddressEnnpoint);

            });

            builder.Services.AddTransient(services =>
            {


                return CreateService<CalibrationSaaS.Application.Services.IUOMService<CallContext>>(AddressEnnpoint);


            });

            builder.Services.AddTransient(services =>
            {

                return CreateService<CalibrationSaaS.Application.Services.ICustomerService<CallContext>>(AddressEnnpoint);

            });




            builder.Services.AddTransient<Func<dynamic, CalibrationSaaS.Application.Services.ISampleService2<CallContext>>>(SampleService2FactorySqlite);



            builder.Services.AddTransient<Func<dynamic, CalibrationSaaS.Application.Services.ICustomerService<CallContext>>>(CustomerServiceFactorySqlite);

            builder.Services.AddTransient<Func<dynamic, CalibrationSaaS.Application.Services.IBasicsServices<CallContext>>>(BasicsServiceFactorySqlite);

            builder.Services.AddTransient<Func<dynamic, CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext>>>(POEServiceFactorySqlite);

            builder.Services.AddTransient<Func<dynamic, CalibrationSaaS.Application.Services.IUOMService<CallContext>>>(UOMServiceFactorySqlite);


            builder.Services.AddTransient<Func<dynamic, CalibrationSaaS.Application.Services.IAssetsServices<CallContext>>>(AssetsServiceFactorySqlite);


            builder.Services.AddTransient<Func<dynamic, CalibrationSaaS.Application.Services.IAddressServices>>(AddressServiceFactorySqlite);


            builder.Services.AddTransient<Func<dynamic, CalibrationSaaS.Application.Services.IQuoteService<CallContext>>>(QuoteServiceFactorySqlite);

            builder.Services.AddTransient<Func<dynamic, CalibrationSaaS.Application.Services.IPriceTypeService<CallContext>>>(PriceTypeServiceFactorySqlite);


            builder.Services.AddTransient(services =>
            {
                //// Create a gRPC-Web channel pointing to the backend server
                //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                //var channel = Grpc.Net.Client.GrpcChannel.ForAddress(builder.Configuration["Kestrel:Endpoints:Http2:Url"], new GrpcChannelOptions { HttpClient = httpClient });

                //// Now we can instantiate gRPC clients for this channel
                //return channel.CreateGrpcService<Application.Services.IReportService<CallContext>>();

                return CreateService<CalibrationSaaS.Application.Services.IReportService<CallContext>>(AddressEnnpoint);
            });



            builder.Services.AddTransient(services =>
            {
                //// Create a gRPC-Web channel pointing to the backend server
                //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                //var channel = Grpc.Net.Client.GrpcChannel.ForAddress(builder.Configuration["Kestrel:Endpoints:Http2:Url"], new GrpcChannelOptions { HttpClient = httpClient });
                ////var channel = Grpc.Net.Client.GrpcChannel.ForAddress("https://calsaasgrpc.azurewebsites.net", new GrpcChannelOptions { HttpClient = httpClient });

                //// Now we can instantiate gRPC clients for this channel
                //return channel.CreateGrpcService<Application.Services.IAddressServices>();


                return CreateService<CalibrationSaaS.Application.Services.IAddressServices>(AddressEnnpoint);
            });

            builder.Services.AddTransient(services =>
            {


                return CreateService<CalibrationSaaS.Application.Services.IBasicsServices<CallContext>>(AddressEnnpoint);

            });

            builder.Services.AddTransient(services =>
            {

                return CreateService<CalibrationSaaS.Application.Services.ICustomerAddressService>(AddressEnnpoint);

            });

            builder.Services.AddTransient(services =>
            {


                return CreateService<CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext>>(AddressEnnpoint);

            });
            builder.Services.AddTransient(services =>
            {
                //// Create a gRPC-Web channel pointing to the backend server
                //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                //var channel = Grpc.Net.Client.GrpcChannel.ForAddress(builder.Configuration["Kestrel:Endpoints:Http2:Url"], new GrpcChannelOptions { HttpClient = httpClient });
                ////var channel = Grpc.Net.Client.GrpcChannel.ForAddress("https://calsaasgrpc.azurewebsites.net", new GrpcChannelOptions { HttpClient = httpClient });

                //// Now we can instantiate gRPC clients for this channel
                //return channel.CreateGrpcService<Application.Services.IWorkOrderService<CallContext>>();

                return CreateService<CalibrationSaaS.Application.Services.IWorkOrderService<CallContext>>(AddressEnnpoint);

            });

            builder.Services.AddTransient(services =>
            {
                //// Create a gRPC-Web channel pointing to the backend server
                //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                //var channel = Grpc.Net.Client.GrpcChannel.ForAddress(builder.Configuration["Kestrel:Endpoints:Http2:Url"], new GrpcChannelOptions { HttpClient = httpClient });
                ////var channel = Grpc.Net.Client.GrpcChannel.ForAddress("https://calsaasgrpc.azurewebsites.net", new GrpcChannelOptions { HttpClient = httpClient });

                //// Now we can instantiate gRPC clients for this channel
                //return channel.CreateGrpcService<Application.Services.IAssetsServices<CallContext>>();

                return CreateService<CalibrationSaaS.Application.Services.IAssetsServices<CallContext>>(AddressEnnpoint);


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
            builder.Services.AddSingleton<CalibrationSaaS.Domain.Aggregates.ValueObjects.EquipmentTypeResultSet>();
            builder.Services.AddSingleton<CalibrationSaaS.Domain.Aggregates.ValueObjects.PieceOfEquipmentResultSet>();
            builder.Services.AddSingleton<CalibrationSaaS.Domain.Aggregates.ValueObjects.AddressResultSet>();
            builder.Services.AddSingleton<CalibrationSaaS.Domain.Aggregates.ValueObjects.UserResultSet>();



        }

        builder.Services.AddScoped<BrowserService>();


        if (!HasConexion)
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            builder.Services.AddAuthorizationCore();

            builder.Services.AddTransient<BlazorApp1.Client.Security.CustomAuthenticationService>();

            builder.Services.AddScoped<AuthenticationStateProvider, BlazorApp1.Client.Security.CustomAuthStateProvider>();

            

            builder.Services.AddTransient<IClaimsTransformation, BlazorApp1.Services.AddRolesClaimsTransformation>();

            builder.Services.AddScoped<BlazorApp1.Client.Security.CustomAuthorizationMessageHandler>();
            ////////////////////////////////////////////////////////////////////////////////////////////////
            ///
            ///////offline
            builder.Services.AddAuthentication(options =>
            {
                //options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                //options.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
                ////options.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
                ///options.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
                ////options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                ///

                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;


            })
            .AddCookie(options =>
            {
                options.Cookie.Name = "__Host-blazorwebapp";
                options.Cookie.SameSite = SameSiteMode.Lax;
                // can be strict if same-site
                //options.Cookie.SameSite = SameSiteMode.Strict;
            });
            ////////////////////////////

        }
        else
        {

           



            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "OpenIdConnect";
                options.DefaultSignOutScheme = "OpenIdConnect";
                //options.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
                // options.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;


                //options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;


            })
          .AddCookie(options =>
          {
              options.Cookie.Name = "__Host-blazorwebapp";
              options.Cookie.SameSite = SameSiteMode.Lax;
              // can be strict if same-site
              //options.Cookie.SameSite = SameSiteMode.Strict;
          });
            //.AddOpenIdConnect(options =>
            //{
            //    builder.Configuration.GetSection("OpenIDConnectSettings").Bind(options);

            //    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.ResponseType = OpenIdConnectResponseType.Code;

            //    options.SaveTokens = true;
            //    options.GetClaimsFromUserInfoEndpoint = true;
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        NameClaimType = ClaimTypes.Name,
            //        RoleClaimType = ClaimTypes.Role,


            //    };
            //    options.RequireHttpsMetadata = true;
            //    options.Scope.Add("profile");
            //    options.Scope.Add("email");
            //    options.Scope.Add("roles");
            //    options.Scope.Add("dataEventRecords");
            //    options.Scope.Add(OpenIdConnectScope.OfflineAccess);
            //    options.ClaimActions.MapUniqueJsonKey("role",
            //                                "role");



            //});
        }



        builder.Services.AddAuthorizationCore(o =>
        {
            o.AddPolicy("HasAccess", policy =>
            {  //policy.RequireAuthenticatedUser();
               //policy.Requirements.Add(new AccesRequirement2(AppSecurity.RolesList));
                policy.Requirements.Add(new AccesRequirement(roles, AppSecurity.Components));

            });

        });
        builder.Services.AddTransient<IAuthorizationHandler, AccesHandler>();

        //builder.Services.AddOidcAuthentication(options =>
        //{
        //    builder.Configuration.Bind("Local", options.ProviderOptions);
        //    options.ProviderOptions.DefaultScopes.Add("openid");
        //    options.ProviderOptions.DefaultScopes.Add("profile");
        //    options.ProviderOptions.DefaultScopes.Add("email");
        //    options.ProviderOptions.DefaultScopes.Add("roles");
        //    options.ProviderOptions.DefaultScopes.Add("offline_access");
        //    options.ProviderOptions.DefaultScopes.Add("name");
        //    options.ProviderOptions.ResponseType = "code";


        //});

        //builder.Services.AddOidcAuthentication(options =>
        //{

        //    builder.Configuration.Bind("oidc", options.ProviderOptions);
        //    options.UserOptions.ScopeClaim = "scope";
        //    options.UserOptions.RoleClaim = "role";
        //    options.UserOptions.NameClaim = "name";
        //    options.ProviderOptions.DefaultScopes.Add("offline_access");

        //})
        //.AddAccountClaimsPrincipalFactory<MultipleRoleClaimsPrincipalFactory<RemoteUserAccount>, CustomUserAccount, CustomAccountFactory>();
        //.AddAccountClaimsPrincipalFactory<BlazorApp1.Client.Security.MultipleRoleClaimsPrincipalFactory<RemoteUserAccount>>();

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
        BlazorApp1.Client.Program.ThreadProc();
        // Add TodosState for KavokuComponentBase
        builder.Services.AddScoped<IState<Helpers.Controls.TodosState>>(provider =>
        {
            var state = new Helpers.Controls.TodosState(false, null, null, null, null, null);
            return new CalibrationSaaS.Infraestructure.Blazor.WrappedState<Helpers.Controls.TodosState>(state);
        });
        builder.Services.AddFluxor(options =>
        {
            options.ScanAssemblies(Assembly.GetExecutingAssembly());
            options.UseRouting();
            options.UseReduxDevTools();
        });


        builder.Services.AddScoped<CalibrationSaaS.Infraestructure.Blazor.StateFacade>();


        builder.Services.AddTransient<Blazed.Controls.IStateFacade, CalibrationSaaS.Infraestructure.Blazor.StateFacade>();

        builder.Services.AddTransient<IResponsiveTableService, Blazed.Controls.ResponsiveTableService>();

        builder.Services.AddScoped<LazyAssemblyLoader>();

        builder.Services.AddLocalization();

        builder.Services.AddTransient<IClaimsTransformation, BlazorApp1.Services.AddRolesClaimsTransformation>();

        builder.Services.AddTransient<ISampleService2<CallContext>, SampleService2Offline<CalibrationSaaSDBContextOff2>>();
        /////Offline
        ///
        builder.Services.AddSingleton<ISqliteWasmDbContextFactory<CalibrationSaaSDBContextOff2>, SqliteWasmDbContextFactory<CalibrationSaaSDBContextOff2>>();
        builder.Services.AddSingleton<IBrowserCache, BrowserCache>();
        builder.Services.AddSingleton<ISqliteSwap, SqliteSwap>();

        string path = $"Data Source={DatabaseService2<CalibrationSaaSDBContextOff2>.FileName};Foreign Keys=False";

        builder.Services.AddSqliteWasmDbContextFactory<CalibrationSaaSDBContextOff2>(
        opts => opts.UseSqlite(path));

        builder.Services.AddSingleton<DatabaseService2<CalibrationSaaSDBContextOff2>>();


        builder.Services.AddRouter();


        builder.Services.AddScoped(typeof(Blazed.Controls.DragDropService<>));

        // Add MockAccessTokenProvider for components that require IAccessTokenProvider
        //builder.Services.AddScoped<Microsoft.AspNetCore.Components.WebAssembly.Authentication.IAccessTokenProvider, CalibrifyApp.Server.Services.MockAccessTokenProvider>();


        //builder.Services.AddApiAuthorization();
        AppSecurity.IsOffline = IsOffline;

        if (IsOffline)
        {
            //builder.RootComponents.Add<BlazorApp.Client.Blazor.App>("#app");
            
            builder.RootComponents.Add<Routes>("#app");
            
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }



        

        //app.MapBlazorHub(
        //  // Long-polling produces such a bad user experience that we can't support it
        //  configureOptions: options => { options.Transports = HttpTransportType.WebSockets; }
        //);





        await builder.Build().RunAsync();



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


    public static List<CustomSequence> CustomSequences { get; set; }



    public static async Task<ICollection<Helpers.Controls.Component>> GetComponents(string endpoint, string AddressEnnpoint)
    {

        var aa = PrerenderRouteHelper.GetRoutesToRender(typeof(BlazorApp1.Client._Imports).Assembly); // Pass in the WebAssembly app's Assembly
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
        var channel = Grpc.Net.Client.GrpcChannel.ForAddress(endpoint, new GrpcChannelOptions
        {
            HttpClient = httpClient
            //,Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)


        });
        //var channel = Grpc.Net.Client.GrpcChannel.ForAddress("https://calsaasgrpc.azurewebsites.net", new GrpcChannelOptions { HttpClient = httpClient });

        // Now we can instantiate gRPC clients for this channel
        var a = channel.CreateGrpcService<CalibrationSaaS.Application.Services.IComponentServices<CallContext>>();



        var bb = aa;
        //
        var c = await a.GetAllComponents(new CallContext());


        List<Helpers.Controls.Component> cnts = new List<Helpers.Controls.Component>();
        foreach (var item in aa)
        {
            Helpers.Controls.Component cc = new Helpers.Controls.Component();

            var c2 = c?.List.Where(x => x.Name == item).FirstOrDefault();
            if (c2 != null)
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
    public static void DisplayValues()
    {
        // Create new thread and display three random numbers.
        Console.WriteLine("Some currency values:");
        for (int ctr = 0; ctr <= 3; ctr++)
            Console.WriteLine("   {0:C2}", rnd.NextDouble() * 10);
    }
    public static void DisplayThreadInfo()
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

    public static void ThreadProc()
    {
        DisplayThreadInfo();
        DisplayValues();
    }


    public static T CreateService<T>(string AddresEndPoint) where T : class
    {

        return CreateChanel(AddresEndPoint).CreateGrpcService<T>();

    }

    public static GrpcChannel CreateChanel(string AddressEndPoint)
    {
        //builder.Configuration["Kestrel:Endpoints:Http2:Url"]
        var credentials = CallCredentials.FromInterceptor((context, metadata) =>
        {
            if (!string.IsNullOrEmpty(BlazorApp1.Client.Program.AppSecurity.Token))
            {


                metadata.Add("Authorization", $"Bearer {BlazorApp1.Client.Program.AppSecurity.Token}");
            }
            return Task.CompletedTask;
        });

        // Create a gRPC-Web channel pointing to the backend server
        var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));

        httpClient.Timeout = TimeSpan.FromSeconds(180);
        //var channel = Grpc.Net.Client.GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions { HttpClient = httpClient });

        //var cre = credentials;


        var channel = Grpc.Net.Client.GrpcChannel.ForAddress(AddressEndPoint, new GrpcChannelOptions
        {

            HttpClient = httpClient,
            //Credentials = ChannelCredentials.Insecure //ChannelCredentials.Create(ChannelCredentials.Insecure, credentials)
            Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)



        });

        return channel;

    }


    public static Func<IServiceProvider, Func<dynamic, CalibrationSaaS.Application.Services.IAddressServices>> AddressServiceFactorySqlite =>
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

             return BlazorApp1.Client.Program.CreateService<CalibrationSaaS.Application.Services.IAddressServices>(ConnectionStatusService.grpcUrl);


         }
         else
         {
             //ILocalStorageService localStorageService = new LocalStorageService();

             return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.AddressServiceOffline<CalibrationSaaSDBContextOff2>(database);
         }

         throw new ArgumentException("Could not determine location");
     };
 };



    public static Func<IServiceProvider, Func<dynamic
       , CalibrationSaaS.Application.Services.IAssetsServices<CallContext>>> AssetsServiceFactorySqlite =>
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

            return BlazorApp1.Client.Program.CreateService<CalibrationSaaS.Application.Services.IAssetsServices<CallContext>>(ConnectionStatusService.grpcUrl);


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


    public static Func<IServiceProvider, Func<dynamic, CalibrationSaaS.Application.Services.ICustomerService<CallContext>>> CustomerServiceFactorySqlite =>
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

            return BlazorApp1.Client.Program.CreateService<CalibrationSaaS.Application.Services.ICustomerService<CallContext>>(ConnectionStatusService.grpcUrl);

        }
        else
        {
            //ILocalStorageService localStorageService = new LocalStorageService();

            return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.CustomerServiceOffline<CalibrationSaaSDBContextOff2>(database);
        }

        throw new ArgumentException("Could not determine location");
    };
};
    public static Func<IServiceProvider, Func<dynamic, CalibrationSaaS.Application.Services.ISampleService2<CallContext>>> SampleService2FactorySqlite =>
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
            return channel.CreateGrpcService<CalibrationSaaS.Application.Services.ISampleService2<CallContext>>();
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


    public static Func<IServiceProvider, Func<dynamic, CalibrationSaaS.Application.Services.IWorkOrderDetailServices<CallContext>>> WODServiceFactorySqlite =>
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
            return BlazorApp1.Client.Program.CreateService<CalibrationSaaS.Application.Services.IWorkOrderDetailServices<CallContext>>(ConnectionStatusService.grpcUrl);


        }
        else
        {
            //ILocalStorageService localStorageService = new LocalStorageService();

            return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.WODServiceOffline<CalibrationSaaSDBContextOff2>(database);
        }

        throw new ArgumentException("Could not determine location");
    };
};


    public static Func<IServiceProvider, Func<dynamic, CalibrationSaaS.Application.Services.IBasicsServices<CallContext>>> BasicsServiceFactorySqlite =>
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
            return CreateService<CalibrationSaaS.Application.Services.IBasicsServices<CallContext>>(ConnectionStatusService.grpcUrl);


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

    private static Func<IServiceProvider, Func<dynamic, CalibrationSaaS.Application.Services.IQuoteService<CallContext>>> QuoteServiceFactorySqlite =>
service =>
{
   return database =>
   {
       Console.WriteLine("QuoteServiceFactorySqlite");
       Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
       Console.WriteLine(ConnectionStatusService.grpcUrl);

       if (ConnectionStatusService.GetCurrentStatus())//online
       {
           ////Create a gRPC - Web channel pointing to the backend server
           //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
           //var channel = Grpc.Net.Client.GrpcChannel.ForAddress(ConnectionStatusService.grpcUrl, new GrpcChannelOptions { HttpClient = httpClient });

           ////Now we can instantiate gRPC clients for this channel
           //return channel.CreateGrpcService<Application.Services.IQuoteService<CallContext>>();
           return CreateService<CalibrationSaaS.Application.Services.IQuoteService<CallContext>>(ConnectionStatusService.grpcUrl);
       }
       else
       {
           //ILocalStorageService localStorageService = new LocalStorageService();

           return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.QuoteServiceOffline<CalibrationSaaSDBContextOff2>(database);
       }

       throw new ArgumentException("Could not determine location");
   };
};

    private static Func<IServiceProvider, Func<dynamic, CalibrationSaaS.Application.Services.IPriceTypeService<CallContext>>> PriceTypeServiceFactorySqlite =>
service =>
{
return database =>
{
    Console.WriteLine("PriceTypeServiceFactorySqlite");
    Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
    Console.WriteLine(ConnectionStatusService.grpcUrl);

    if (ConnectionStatusService.GetCurrentStatus())//online
    {
        ////Create a gRPC - Web channel pointing to the backend server
        //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
        //var channel = Grpc.Net.Client.GrpcChannel.ForAddress(ConnectionStatusService.grpcUrl, new GrpcChannelOptions { HttpClient = httpClient });

        ////Now we can instantiate gRPC clients for this channel
        //return channel.CreateGrpcService<Application.Services.IPriceTypeService<CallContext>>();
        return CreateService<CalibrationSaaS.Application.Services.IPriceTypeService<CallContext>>(ConnectionStatusService.grpcUrl);
    }
    else
    {
        //ILocalStorageService localStorageService = new LocalStorageService();

        return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.PriceTypeServiceOffline<CalibrationSaaSDBContextOff2>(database);
    }

    throw new ArgumentException("Could not determine location");
};
};

    public static Func<IServiceProvider, Func<dynamic, CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext>>> POEServiceFactorySqlite =>
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
            return CreateService<CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext>>(ConnectionStatusService.grpcUrl);


        }
        else
        {
            //ILocalStorageService localStorageService = new LocalStorageService();

            return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.POEServiceOffline<CalibrationSaaSDBContextOff2>(database);
        }

        throw new ArgumentException("Could not determine location");
    };
};


    public static Func<IServiceProvider, Func<dynamic, CalibrationSaaS.Application.Services.IUOMService<CallContext>>> UOMServiceFactorySqlite =>
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
            return CreateService<CalibrationSaaS.Application.Services.IUOMService<CallContext>>(ConnectionStatusService.grpcUrl);
        }
        else
        {
            //ILocalStorageService localStorageService = new LocalStorageService();

            return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.UOMServiceOffline<CalibrationSaaSDBContextOff2>(database);
        }

        throw new ArgumentException("Could not determine location");
    };
};






}

