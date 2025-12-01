using Fido2Identity;
using Fido2NetLib;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using OpenIddict.Abstractions;
using OpenIddict.Client;
using OpenIddict.Validation.AspNetCore;
using OpeniddictServer.Data;
using Quartz;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IdentityModel.Tokens.Jwt;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace OpeniddictServer;

internal static class HostingExtensions
{
    private static IWebHostEnvironment _env;
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        _env = builder.Environment;

        services.AddControllersWithViews();
        services.AddRazorPages();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            // Configure the context to use Microsoft SQL Server.
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            // Register the entity sets needed by OpenIddict.
            // Note: use the generic overload if you need
            // to replace the default OpenIddict entities.
            options.UseOpenIddict();
        });

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddIdentity<ApplicationUser, IdentityRole>()
          .AddEntityFrameworkStores<ApplicationDbContext>()
          .AddDefaultTokenProviders()
          .AddDefaultUI()
          .AddTokenProvider<Fido2UserTwoFactorTokenProvider>("FIDO2");


    //    services.AddIdentity<User, Role>()
    //.AddSignInManager()
    ////.AddUserStore<UserStore>()
    ////.AddRoleStore<RoleStore>()
    //.AddUserManager<UserManager<User>>();


        services.Configure<Fido2Configuration>(configuration.GetSection("fido2"));
        services.AddScoped<Fido2Store>();

        services.AddDistributedMemoryCache();

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(2);
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        services.Configure<IdentityOptions>(options =>
        {
            // Configure Identity to use the same JWT claims as OpenIddict instead
            // of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            
            options.ClaimsIdentity.UserNameClaimType = Claims.Name;
            options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
            options.ClaimsIdentity.RoleClaimType = Claims.Role;
            options.ClaimsIdentity.EmailClaimType = Claims.Email;
            
            // Note: to require account confirmation before login,
            // register an email sender service (IEmailSender) and
            // set options.SignIn.RequireConfirmedAccount to true.
            //
            // For more information, visit https://aka.ms/aspaccountconf.
            options.SignIn.RequireConfirmedAccount = false;
            

        });

        // OpenIddict offers native integration with Quartz.NET to perform scheduled tasks
        // (like pruning orphaned authorizations/tokens from the database) at regular intervals.
        services.AddQuartz(options =>
        {
            options.UseSimpleTypeLoader();
            options.UseInMemoryStore();
        });

        // Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);


        //services.AddOpenIddict()

        //   // Register the OpenIddict core components.
        //   .AddCore(options =>
        //   {
        //       // Configure OpenIddict to use the Entity Framework Core stores and models.
        //       // Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
        //       options.UseEntityFrameworkCore()
        //              .UseDbContext<ApplicationDbContext>();

        //       // Enable Quartz.NET integration.
        //       options.UseQuartz();
        //   })

        //   // Register the OpenIddict server components.
        //   .AddServer(options =>
        //   {
        //       // Enable the token endpoint.
        //       options.SetTokenEndpointUris("connect/token");

        //       // Enable the client credentials flow.
        //       options.AllowClientCredentialsFlow();

        //       // Register the signing and encryption credentials.
        //       options.AddDevelopmentEncryptionCertificate()
        //              .AddDevelopmentSigningCertificate();

        //       // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
        //       options.UseAspNetCore()
        //              .EnableTokenEndpointPassthrough();
        //   })

        //   // Register the OpenIddict validation components.
        //   .AddValidation(options =>
        //   {
        //       // Import the configuration from the local OpenIddict server instance.
        //       options.UseLocalServer();

        //       // Register the ASP.NET Core host.
        //       options.UseAspNetCore();
        //   });




        services.AddOpenIddict()

            // Register the OpenIddict core components.
            .AddCore(options =>
            {
                // Configure OpenIddict to use the Entity Framework Core stores and models.
                // Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
                options.UseEntityFrameworkCore()
                       .UseDbContext<ApplicationDbContext>();

                // Enable Quartz.NET integration.
                options.UseQuartz();
            })

            // Register the OpenIddict server components.
            .AddServer(options =>
            {
                // Enable the authorization, logout, token and userinfo endpoints.
                options
                //.SetIssuer(new Uri("https://localhost:44318/"))
                .SetAuthorizationEndpointUris("/connect/authorize")
                    .SetEndSessionEndpointUris("/connect/logout")
                    .SetIntrospectionEndpointUris("/connect/introspect")
                    .SetTokenEndpointUris("/connect/token")
                    .SetUserInfoEndpointUris("/connect/userinfo")
                    .SetEndUserVerificationEndpointUris("/connect/verify");


                options.AllowPasswordFlow();
                options.AllowRefreshTokenFlow();

                // Note: this sample uses the code, device code, password and refresh token flows, but you
                // can enable the other flows if you need to support implicit or client credentials.
                options.AllowAuthorizationCodeFlow()
                       .AllowClientCredentialsFlow()
                       .AllowHybridFlow()
                       .AllowRefreshTokenFlow().AllowImplicitFlow();

                // Mark the "email", "profile", "roles" and "dataEventRecords" scopes as supported scopes.
                options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles, "dataEventRecords");

                // remove this with introspection, added security
                options.DisableAccessTokenEncryption();

                // Register the signing and encryption credentials.
                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();

                // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                options.UseAspNetCore()
                       .EnableAuthorizationEndpointPassthrough()
                       .EnableEndSessionEndpointPassthrough()
                       .EnableTokenEndpointPassthrough()
                       .EnableUserInfoEndpointPassthrough()
                       .EnableStatusCodePagesIntegration();


                options.RegisterClaims(Claims.Name, Claims.GivenName, Claims.Role);

                options.UseReferenceAccessTokens();
                options.UseReferenceRefreshTokens();


                options.AllowClientCredentialsFlow();

                options.AllowPasswordFlow();

                //options.DisableHttpsRequirement();
                //options.js();
                //options.AddSigningKey(signingKey);

                options.AcceptAnonymousClients();



            })
            /////////////////

        //    // Register the OpenIddict validation components.
        //    .AddValidation(options =>
        //    {
        //        // Configure the validation handler to use introspection and register the client
        //        // credentials used when communicating with the remote introspection endpoint.
        //        //options.UseIntrospection()
        //        //        .SetClientId("rs_dataEventRecordsApi")
        //        //        .SetClientSecret("dataEventRecordsSecret");
        //        //// Note: the validation handler uses OpenID Connect discovery
        //        //// to retrieve the address of the introspection endpoint.
        //        //options.SetIssuer("https://localhost:5001/");
        //        //options.AddAudiences("rs_dataEventRecordsApi");





        //        // Register the System.Net.Http integration.
        //        options.UseSystemNetHttp();


        //        // Import the configuration from the local OpenIddict server instance.
        //        options.UseLocalServer();

        //        // Register the ASP.NET Core host.
        //        options.UseAspNetCore();
        //    })

        // Register the OpenIddict client components.


        //.AddClient(options =>
        //{
        //    // Allow grant_type=password and grant_type=refresh_token to be negotiated.
        //    options.AllowPasswordFlow()
        //           .AllowRefreshTokenFlow();

        //    // Disable token storage, which is not necessary for non-interactive flows like
        //    // grant_type=password, grant_type=client_credentials or grant_type=refresh_token.
        //    options.DisableTokenStorage();

        //    // Register the System.Net.Http integration and use the identity of the current
        //    // assembly as a more specific user agent, which can be useful when dealing with
        //    // providers that use the user agent as a way to throttle requests (e.g Reddit).
        //    options.UseSystemNetHttp()
        //           .SetProductInformation(typeof(Program).Assembly);

        //    // Add a client registration without a client identifier/secret attached.
        //    //options.AddRegistration(new OpenIddictClientRegistration
        //    //{
        //    //    Issuer = new Uri("https://localhost:44382/", UriKind.Absolute)
        //    //});
        //    options.AddRegistration(new OpenIddictClientRegistration
        //    {
        //        Issuer = new Uri("https://localhost:44318/", UriKind.Absolute),

        //        ClientId = "oidc-pkce-confidential",
        //        ClientSecret = "oidc-pkce-confidential_secret"
        //    });
        //})


        ;

        // Register the worker responsible of seeding the database.
        // Note: in a real world application, this step should be part of a setup script.
        services.AddHostedService<Worker>();

        //services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = OpenIddictConstants.Schemes.Bearer;
            options.DefaultChallengeScheme = OpenIddictConstants.Schemes.Bearer;
        });

        //services.AddAuthorization(options =>
        //{
        //    options.AddPolicy("dataEventRecordsPolicy", policyUser =>
        //    {
        //        policyUser.RequireClaim("scope", "dataEventRecords");
        //    });
        //});

        //services.AddGrpc();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        IdentityModelEventSource.ShowPII = true;
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        app.UseSerilogRequestLogging();

        if (_env!.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseStatusCodePagesWithReExecute("~/error");
            //app.UseExceptionHandler("~/error");
            //app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSession();

        app.MapControllers();
        app.MapDefaultControllerRoute();
        app.MapRazorPages();

        return app;
    }


}




[Table(nameof(Role))]
public class Role : IdentityRole<long>
{
    public List<UserRole> UserRoles { get; set; }
}

//public class RoleStore : IRoleStore<Role>
//{
//    public Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }

//    public Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }

//    public void Dispose()
//    {
//    }

//    public Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }

//    public Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }

//    public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }

//    public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }

//    public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }

//    public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }

//    public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }

//    public Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }
//}


[Table(nameof(User))]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    public List<UserRole> UserRoles { get; set; }
}


//public class UserRole
//{
   
//    public Guid UserId { get; set; }
//    public User User { get; set; }

  
//    public long RoleId { get; set; }
//    public Role Role { get; set; }
//}

//public class UserStore : IUserStore<User>, IUserPasswordStore<User>, IUserRoleStore<User>
//{
   

//    public Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }

//    public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
//    {
       
//        return IdentityResult.Success;
//    }

//    public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }

//    public void Dispose()
//    {
//    }

//    public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }

//    public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
//    {
//        //return await _context.Users
//        //    .Include(u => u.UserRoles)
//        //    .ThenInclude(ur => ur.Role)
//        //    .SingleOrDefaultAsync(u => u.Username == normalizedUserName, cancellationToken);
//        throw new System.NotImplementedException();
//    }

//    public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }

//    public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
//    {
//        return Task.FromResult(user.PasswordHash);
//    }

//    public Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }

//    public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }

//    public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
//    {
//        return Task.FromResult(user.Username);
//    }

//    public Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }

//    public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }

//    public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }

//    public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }

//    public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
//    {
//        user.Username = normalizedName;
//        return Task.CompletedTask;
//    }

//    public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }

//    public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }

//    public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
//    {
//        throw new System.NotImplementedException();
//    }
//}