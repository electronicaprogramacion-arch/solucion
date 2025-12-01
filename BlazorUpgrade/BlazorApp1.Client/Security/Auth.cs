using Bogus;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Security.Claims;

namespace BlazorApp1.Client.Security
{
    public class CustomAuthStateProvider : AuthenticationStateProvider, IAccessTokenProvider
    {
        private AuthenticationState authenticationState;
        private readonly AuthenticationStateProvider _authStateProvider;
        public CustomAuthStateProvider(ClaimsPrincipal user)
        {
            authenticationState = new AuthenticationState(user);


            NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
            //_authStateProvider = authStateProvider;
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

        public override async Task<AuthenticationState> GetAuthenticationStateAsync() {


            return await Task.FromResult(authenticationState);

            try
            {
                var authState = await _authStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                bool isAuthenticated = user?.Identity?.IsAuthenticated == true;



                return authState; // (isAuthenticated, user);
            }
            catch (Exception ex)
            {

                return await Task.FromResult(authenticationState);
            }

            

        }
            

        public ValueTask<AccessTokenResult> RequestAccessToken()
        {
            return new(); // customize the result
        }

        public ValueTask<AccessTokenResult> RequestAccessToken(AccessTokenRequestOptions options)
        {
            return new(); // customize the result
        }
    }

    public class CustomAuthenticationService
    {
        readonly AppSecurity appSecurity ;

        public CustomAuthenticationService(AppSecurity _appSecurity)
        {
            appSecurity= _appSecurity;
            if (_appSecurity != null && _appSecurity.Principal != null)
            {
                currentUser=_appSecurity.Principal;
            }
        }


        public event Action<ClaimsPrincipal>? UserChanged;
        private ClaimsPrincipal? currentUser;

        public ClaimsPrincipal CurrentUser
        {
            get {

                if (appSecurity.IsOffline) {

                    var identity = new ClaimsIdentity(
                    [
                        new Claim(ClaimTypes.Name, "yuliana"),
                        new Claim(ClaimTypes.Role, "admin"),
                    ], "WebAssembly");

                    var user = new ClaimsPrincipal(identity);

                    currentUser = user;
                }

                return currentUser ?? new(); 
            
            
            }
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

}
