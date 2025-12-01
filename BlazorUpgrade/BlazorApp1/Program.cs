using Blazed.Controls;
using Blazed.Controls.Route.Services;
using Blazor.BFF.OpenIddict.Server;
using Blazor.BFF.OpenIddict.Server.Services;
using Blazor.Extensions.Logging;
using BlazorApp1.Blazor.Blazor.Shared;
using BlazorApp1.Client.Pages;
using BlazorApp1.Client.Security;
using BlazorApp1.Components;
using BlazorApp1.Components.Account;
using BlazorApp1.Data;
using BlazorApp1.Server.Middleware;
using Blazored.Modal;
using BlazorWebApp;
using CacheStorage.Utils;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Security;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite;
using CalibrationSaaS.Infraestructure.Blazor.Shared;

using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using GrpcClient;
using Helpers.Controls;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Client;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Client;
using Radzen;
using Security;
using SqliteWasmHelper;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using static OpenIddict.Abstractions.OpenIddictConstants.Permissions;
using static System.Net.Mime.MediaTypeNames;
using CallContext = ProtoBuf.Grpc.CallContext;

namespace BlazorApp1
{
    //public class Program
    //{
    //    //public static void Main(string[] args)
    //    //{
    //    //    var builder = WebApplication.CreateBuilder(args);

    //    //    // Add services to the container.
    //    //    builder.Services.AddRazorComponents()
    //    //        .AddInteractiveServerComponents()
    //    //        .AddInteractiveWebAssemblyComponents()
    //    //        .AddAuthenticationStateSerialization();

    //    //    builder.Services.AddCascadingAuthenticationState();
    //    //    builder.Services.AddScoped<IdentityUserAccessor>();
    //    //    builder.Services.AddScoped<IdentityRedirectManager>();
    //    //    builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

    //    //    builder.Services.AddAuthentication(options =>
    //    //    {
    //    //        options.DefaultScheme = IdentityConstants.ApplicationScheme;
    //    //        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    //    //    })
    //    //        .AddIdentityCookies();

    //    //    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    //    //    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    //    //        options.UseSqlServer(connectionString));
    //    //    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    //    //    builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    //    //        .AddEntityFrameworkStores<ApplicationDbContext>()
    //    //        .AddSignInManager()
    //    //        .AddDefaultTokenProviders();

    //    //    builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

    //    //    var app = builder.Build();

    //    //    // Configure the HTTP request pipeline.
    //    //    if (app.Environment.IsDevelopment())
    //    //    {
    //    //        app.UseWebAssemblyDebugging();
    //    //        app.UseMigrationsEndPoint();
    //    //    }
    //    //    else
    //    //    {
    //    //        app.UseExceptionHandler("/Error", createScopeForErrors: true);
    //    //        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //    //        app.UseHsts();
    //    //    }

    //    //    app.UseHttpsRedirection();


    //    //    app.UseAntiforgery();

    //    //    app.MapStaticAssets();
    //    //    app.MapRazorComponents<App>()
    //    //        .AddInteractiveServerRenderMode()
    //    //        .AddInteractiveWebAssemblyRenderMode()
    //    //        .AddAdditionalAssemblies(typeof(BlazorApp1.Client._Imports).Assembly);

    //    //    // Add additional endpoints required by the Identity /Account Razor components.
    //    //    app.MapAdditionalIdentityEndpoints();

    //    //    app.Run();


    //    //}


    //}

    public class Program
    {



        private static string _token;


        public static bool HasConexion=true;
        public static async Task Main(string[] args)
        {

            var watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here

            var builder = WebApplication.CreateBuilder(args);

            string[] startupParams = null;

           

            var loglevel = LogLevel.Debug;
          
            string AddressEnnpoint = builder.Configuration["Service:Url"];


            AppSecurity.AssemblyName = builder.Configuration["VersionApp:Assembly"];

            // Add services to the container.
            //// Add services to the container.
            //builder.Services.AddRazorComponents()

            //builder.Services.AddRazorComponents().AddInteractiveServerComponents(options =>
            //{
            //    options.DetailedErrors = true;
            //    options.DisconnectedCircuitMaxRetained = 1000;
            //    options.JSInteropDefaultCallTimeout = new TimeSpan(0, 3, 0)
            //    ;
            //})



            //.AddInteractiveWebAssemblyComponents().AddAuthenticationStateSerialization(options =>
            //{
            //    options.SerializeAllClaims = true;
            //    options.SerializationCallback = (claims) =>
            //    {
            //        // Customize the serialization of claims if needed

            //        // For example, you can filter out certain claims or modify them
            //        return claims;
            //    };
            //}   
            //); 

            builder.Services.AddRazorComponents().AddInteractiveServerComponents(options =>
            {
                options.DetailedErrors = true;
                options.DisconnectedCircuitMaxRetained = 1000;
                options.JSInteropDefaultCallTimeout = new TimeSpan(0, 3, 0);
            })
                   .AddInteractiveWebAssemblyComponents().AddAuthenticationStateSerialization(options =>
                   {
                       options.SerializeAllClaims = true;
                       options.SerializationCallback = (authState) =>
                       {

                           var app = AppSecurity.IsNotGrpc;

                           var claims= authState.User.Claims;
                           List<ClaimData> claimsList = new List<ClaimData>();
                           foreach (var claim in claims)
                           {
                               claimsList.Add(new ClaimData(claim.Type, claim.Value));
                           }
                           if (authState?.User?.Identity?.IsAuthenticated==true)
                           {
                               AppSecurity.Principal = authState.User;
                           }
                           

                           var ClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims,"Server"));

                           var au = new AuthenticationState(ClaimsPrincipal);
                           AuthenticationStateData ausd = null;
                           if (claims.Count() > 0) {
                                ausd = new AuthenticationStateData()
                               {
                                   Claims = claimsList,
                                   NameClaimType = ClaimTypes.Name,
                                   RoleClaimType = ClaimTypes.Role
                               };

                           }
                          
                           // Fix for CS1662: Ensure the lambda returns a compatible type
                           return new ValueTask<AuthenticationStateData?>(ausd);
                          
                       };
                   });


            builder.Services.AddHttpContextAccessor();

            var oidcConfig = builder.Configuration.GetSection("OpenIDConnectSettings");



         





            //       builder.Services.AddControllersWithViews(options =>
            //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

            builder.Services.AddRazorPages().AddMvcOptions(options =>
            {
                //var policy = new AuthorizationPolicyBuilder()
                //    .RequireAuthenticatedUser()
                //    .Build();
                //options.Filters.Add(new AuthorizeFilter(policy));
            });



            /////////////////////////////////////////////7
            ///
            //builder.Services.AddRazorPages().AddMvcOptions(options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));
            //});

            //builder.Services.AddControllersWithViews(options =>
            //    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));


            //////////////////////////////////////////


            builder.Services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
                options.Cookie.Name = "__Host-core-X-XSRF-TOKEN";
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.SuppressXFrameOptionsHeader = true;
            });

            //builder.Services.AddSecurityHeaderPolicies()
            //    .SetDefaultPolicy(SecurityHeadersDefinitions
            //    .GetHeaderPolicyCollection("OpenIDConnectSettings:Authority"]],
            //        builder.Environment.IsDevelopment()));


            //builder.Services.AddAuthorization();
            builder.Services.AddCascadingAuthenticationState();



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


            if (!AppSecurity.IsReport)
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


            builder.Services.AddSingleton<CalibrationSaaS.Domain.Aggregates.Shared.Basic.AppState>();
            builder.Services.AddSingleton<CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany>();





            builder.Services.AddScoped<Blazed.Controls.Toast.IToastService, Blazed.Controls.Toast.ToastService>();

            builder.Services.AddBlazoredModal();

           

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
                    var a = await Roles(AddressEnnpoint);
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
                    AppSecurity.Components = await GetComponents(AddressEnnpoint, AddressEnnpoint);
                    if (CustomSequences != null)
                    {
                        AppSecurity.CustomSequences = CustomSequences;


                    }



                }
                catch
                {

                }


            }

           
            //builder.Services.AddScoped<AuthenticationStateProvider, Microsoft.AspNetCore.Components.Server.ServerAuthenticationStateProvider>();
            if (!HasConexion)
            {
                ////////////////////////////////////////////////////////////////////////////////////////
                builder.Services.AddAuthorizationCore();

                builder.Services.AddScoped<AuthenticationStateProvider, BlazorApp1.Client.Security.CustomAuthStateProvider>();

                builder.Services.AddSingleton<BlazorApp1.Client.Security.CustomAuthenticationService>();

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
                //builder.Services.AddSingleton<BlazorApp1.Client.Security.CustomAuthenticationService>();

                //builder.Services.AddScoped<AuthenticationStateProvider, BlazorApp1.Client.Security.CustomAuthStateProvider>();

               

                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    options.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
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
              })
                .AddOpenIdConnect(options =>
                {
                    builder.Configuration.GetSection("OpenIDConnectSettings").Bind(options);

                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.ResponseType = OpenIdConnectResponseType.Code;

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.Name,
                        RoleClaimType = ClaimTypes.Role,


                    };
                    options.RequireHttpsMetadata = true;
                    options.Scope.Add("profile");
                    options.Scope.Add("email");
                    options.Scope.Add("roles");
                    options.Scope.Add("dataEventRecords");
                    options.Scope.Add(OpenIdConnectScope.OfflineAccess);
                    options.ClaimActions.MapUniqueJsonKey("role",
                                                "role");



                });
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

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Local", options.ProviderOptions);
                options.ProviderOptions.DefaultScopes.Add("openid");
                options.ProviderOptions.DefaultScopes.Add("profile");
                options.ProviderOptions.DefaultScopes.Add("email");
                options.ProviderOptions.DefaultScopes.Add("roles");
                options.ProviderOptions.DefaultScopes.Add("offline_access");
                options.ProviderOptions.DefaultScopes.Add("name");
                options.ProviderOptions.ResponseType = "code";


            });



            

           

           



            //builder.Services.AddScoped<AuthenticationStateProvider, TestAuthStateProvider>();

           



            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();


            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Transient<IPostConfigureOptions<RemoteAuthenticationOptions<ApiAuthorizationProviderOptions>>,
                    BlazorApp1.Services.ApiAuthorizationOptionsConfiguration>());

            builder.Services.AddSingleton<CalibrationSaaS.Domain.Aggregates.Shared.AppSecurity>(AppSecurity);




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

            // Add TodosState for KavokuComponentBase
            builder.Services.AddScoped<IState<Helpers.Controls.TodosState>>(provider =>
            {
                var state = new Helpers.Controls.TodosState(false, null, null, null, null, null);
                return new CalibrationSaaS.Infraestructure.Blazor.WrappedState<Helpers.Controls.TodosState>(state);
            });

            //builder.Services.AddScoped<IState<Helpers.Controls.TodosState>>();
            builder.Services.AddFluxor(options =>
            {
                options.ScanAssemblies(Assembly.GetExecutingAssembly());
                options.UseRouting();
                options.UseReduxDevTools();
            });


            // Add custom application services
            builder.Services.AddScoped<StateFacade>();


            builder.Services.AddTransient<Blazed.Controls.IStateFacade, StateFacade>();

            builder.Services.AddScoped(typeof(Blazed.Controls.DragDropService<>));

            builder.Services.AddTransient<IResponsiveTableService, ResponsiveTableService>();

            builder.Services.AddScoped<LazyAssemblyLoader>();

            builder.Services.AddLocalization();

            var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()))
            {
                Timeout = TimeSpan.FromSeconds(60) // Reduced from 180 seconds to 60 seconds
            };


            /////Offline
            ///
            builder.Services.AddScoped<BlazorApp1.CacheStorage.Utils.CacheStorageAccessor>();


            builder.Services.AddHttpClient();

            builder.Services.AddRadzenComponents();
            builder.Services.AddRadzenCookieThemeService(options =>
            {
                options.Name = "CalibrifyTheme";
                options.Duration = TimeSpan.FromDays(365);
            });




            // Add localization services
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
        new CultureInfo("en-US"),
        new CultureInfo("es-ES")
    };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });


            //Add router services
            builder.Services.AddScoped<RouterSessionService>();


            // Add custom circuit handler to handle JSDisconnectedException
            builder.Services.AddSingleton<Microsoft.AspNetCore.Components.Server.Circuits.CircuitHandler, BlazorApp1.Server.Services.CustomCircuitHandler>();
            builder.Services.AddServerSideBlazor()
            .AddHubOptions(options =>
            {
                options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
                options.HandshakeTimeout = TimeSpan.FromSeconds(60);
                options.DisableImplicitFromServicesParameters = true;
                // Establecer el modo de renderizado predeterminado.  
                
            })
            .AddCircuitOptions(o =>
            {

                o.DetailedErrors = true;

            })
            ;


            // Add component initialization service for better error handling and retry logic
            builder.Services.AddScoped<BlazorApp1.Server.Services.ComponentInitializationService>();

            // Add component lifecycle optimizer to prevent double-loading issues
            builder.Services.AddScoped<BlazorApp1.Server.Services.ComponentLifecycleOptimizer>();

            // Add background service for component cleanup
            builder.Services.AddScoped<BlazorApp1.Server.Services.ComponentCleanupService>();

           

            builder.Services.AddScoped<IModalHandler, ModalHandler>();
            builder.Services.AddScoped<IDynamicModalHandler, BlazorApp1.Blazor.Blazor.Shared.DynamicModalHandler>();


            // Add MockAccessTokenProvider for components that require IAccessTokenProvider
            //builder.Services.AddScoped<IAccessTokenProvider, CalibrifyApp.Server.Services.MockAccessTokenProvider>();


            builder.Services.AddCors();

            var app = builder.Build();

            app.UseCors(x => x.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());

            app.MapRazorPages();
           
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();

                IdentityModelEventSource.ShowPII = true;
                IdentityModelEventSource.LogCompleteSecurityArtifact = true;
            }
            else
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


          



            app.MapRazorComponents<BlazorApp.Blazor.App>()
                
                .AddInteractiveServerRenderMode(o => o.DisableWebSocketCompression = true)
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(BlazorApp1.Client._Imports).Assembly);


          


            app.MapLoginLogoutEndpoints();


            app.UseSecurityHeaders();

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            //app.UseNoUnauthorizedRedirect("/api");

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAntiforgery();
            app.MapRazorPages();
            app.MapControllers();
            //app.MapNotFound("/api/{**segment}");

            // Use custom error handling middleware
            app.UseErrorHandling();

            //app.UsePdbBlocker();

            //app.Use(async (context, next) =>
            //{
            //    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            //    context.Response.Headers["Pragma"] = "no-cache";
            //    context.Response.Headers["Expires"] = "0";
            //    await next.Invoke();
            //});


            await app.RunAsync();



        }

        public static async Task<User> GetUserById(string name, string endpoint)
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

            User user = new User();

            user.Name = name;
            user.UserName = name;
            var b = await a.GetUserById(user, CallContext.Default);

            return b;
        }

        private const string Address = "localhost";





        public static Microsoft.Extensions.Configuration.IConfiguration config;

        //internal static string Scope = "dbe7c3bb-7aa8-406b-9fe5-6657a22032a0/Bitterman.Access";
        public static string roles = "";

        public static CalibrationSaaS.Domain.Aggregates.Shared.AppSecurity AppSecurity = new CalibrationSaaS.Domain.Aggregates.Shared.AppSecurity();


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
                if (AppSecurity.IsReport || AppSecurity.IsNotGrpc)
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


        public static List<CustomSequence> CustomSequences { get; set; }



        public static async Task<ICollection<Helpers.Controls.Component>> GetComponents(string endpoint, string AddressEnnpoint)
        {

            var aa = BlazorApp1.Client.Security.PrerenderRouteHelper.GetRoutesToRender(typeof(BlazorApp1.Client._Imports).Assembly); // Pass in the WebAssembly app's Assembly
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




        static GrpcChannel CreateChanel(string AddressEndPoint)
        {
            //builder.Configuration["Kestrel:Endpoints:Http2:Url"]
            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(AppSecurity.Token))
                {


                    metadata.Add("Authorization", $"Bearer {AppSecurity.Token}");
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

        static T CreateService<T>(string AddresEndPoint) where T : class
        {

            return CreateChanel(AddresEndPoint).CreateGrpcService<T>();

        }

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



        private static Func<IServiceProvider, Func<dynamic, CalibrationSaaS.Application.Services.IAddressServices>> AddressServiceFactorySqlite =>
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

             return CreateService<CalibrationSaaS.Application.Services.IAddressServices>(ConnectionStatusService.grpcUrl);


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

              return CreateService<CalibrationSaaS.Application.Services.IAssetsServices<CallContext>>(ConnectionStatusService.grpcUrl);


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


        private static Func<IServiceProvider, Func<dynamic, CalibrationSaaS.Application.Services.ICustomerService<CallContext>>> CustomerServiceFactorySqlite =>
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

            return CreateService<CalibrationSaaS.Application.Services.ICustomerService<CallContext>>(ConnectionStatusService.grpcUrl);

        }
        else
        {
            //ILocalStorageService localStorageService = new LocalStorageService();

            return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.CustomerServiceOffline<CalibrationSaaSDBContextOff2>(database);
        }

        throw new ArgumentException("Could not determine location");
    };
};
        private static Func<IServiceProvider, Func<dynamic, CalibrationSaaS.Application.Services.ISampleService2<CallContext>>> SampleService2FactorySqlite =>
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


        private static Func<IServiceProvider, Func<dynamic, CalibrationSaaS.Application.Services.IWorkOrderDetailServices<CallContext>>> WODServiceFactorySqlite =>
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
              return CreateService<CalibrationSaaS.Application.Services.IWorkOrderDetailServices<CallContext>>(ConnectionStatusService.grpcUrl);


          }
          else
          {
              //ILocalStorageService localStorageService = new LocalStorageService();

              return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.WODServiceOffline<CalibrationSaaSDBContextOff2>(database);
          }

          throw new ArgumentException("Could not determine location");
      };
  };


        private static Func<IServiceProvider, Func<dynamic, CalibrationSaaS.Application.Services.IBasicsServices<CallContext>>> BasicsServiceFactorySqlite =>
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


        private static Func<IServiceProvider, Func<dynamic, CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext>>> POEServiceFactorySqlite =>
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


        private static Func<IServiceProvider, Func<dynamic, CalibrationSaaS.Application.Services.IUOMService<CallContext>>> UOMServiceFactorySqlite =>
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

        //public class testj : IJSRuntime
        //{



        //    public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, object?[]? args)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

    }


}






