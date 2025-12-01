using BlazorApp1.Components.Account;
using BlazorApp1.Components.Account.Pages;
using BlazorApp1.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Encodings.Web;

namespace Blazor.BFF.OpenIddict.Server.Controllers;

// orig src https://github.com/berhir/BlazorWebAssemblyCookieAuth
[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
public class AccountController : ControllerBase
{
    UserManager<ApplicationUser> UserManager;
    IUserStore<ApplicationUser> UserStore;
    SignInManager<ApplicationUser> SignInManager;
    IEmailSender<ApplicationUser> EmailSender;
    ILogger<Register> Logger;
    NavigationManager NavigationManager;
    private IdentityRedirectManager RedirectManager;

    
    //public AccountController(UserManager<ApplicationUser> _UserManager, IUserStore<ApplicationUser> _UserStore
    //    , SignInManager<ApplicationUser> _SignInManager, IEmailSender<ApplicationUser> _EmailSender,ILogger<Register> _Logger
    //    , NavigationManager _NavigationManager)
    //{
    //    UserManager = _UserManager;
    //    UserStore= _UserStore;
    //    SignInManager= _SignInManager;
    //    EmailSender= _EmailSender;
    //    Logger= _Logger;
    //    NavigationManager= _NavigationManager;
    //    RedirectManager = new IdentityRedirectManager(NavigationManager);

    //}


    [HttpGet("Login")]
    public ActionResult Login(string returnUrl) => Challenge(new AuthenticationProperties
    {
        RedirectUri = !string.IsNullOrEmpty(returnUrl) ? returnUrl : "/"
    });

    //[ValidateAntiForgeryToken]
    //[Authorize]
    [HttpGet("Logout")]
    public async Task<IActionResult> Logout() {

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        // This line does not work

        try
        {
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = "/",
            });
        }
        catch
        {

        }
       

        return SignOut();
       //return SignOut(new AuthenticationProperties
       // {
       //     RedirectUri = "/"
       // },
       // CookieAuthenticationDefaults.AuthenticationScheme,
       // OpenIdConnectDefaults.AuthenticationScheme);

    } 

    [HttpGet("Status")]
    public ActionResult GetAuthenticationStatus()
    {
        return Ok(new { 
            IsAuthenticated = User.Identity?.IsAuthenticated ?? false,
            UserName = User.Identity?.Name 
        });
    }


    private ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor.");
        }
    }

    private string? ReturnUrl { get; set; }

    private IEnumerable<IdentityError>? identityErrors;


    [HttpGet("Register")]
    public async Task RegisterUser(string Email,string Password)
    {
        var user = CreateUser();

        await UserStore.SetUserNameAsync(user, Email, CancellationToken.None);
        var emailStore = GetEmailStore();
        await emailStore.SetEmailAsync(user, Email, CancellationToken.None);
        var result = await UserManager.CreateAsync(user, Password);

        if (!result.Succeeded)
        {
            identityErrors = result.Errors;
            return;
        }

        Logger.LogInformation("User created a new account with password.");

        var userId = await UserManager.GetUserIdAsync(user);
        var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUrl = NavigationManager.GetUriWithQueryParameters(
            NavigationManager.ToAbsoluteUri("Account/ConfirmEmail").AbsoluteUri,
            new Dictionary<string, object?> { ["userId"] = userId, ["code"] = code, ["returnUrl"] = ReturnUrl });

        await EmailSender.SendConfirmationLinkAsync(user, Email, HtmlEncoder.Default.Encode(callbackUrl));

        if (UserManager.Options.SignIn.RequireConfirmedAccount)
        {
            RedirectManager.RedirectTo(
                "Account/RegisterConfirmation",
                new() { ["email"] = Email, ["returnUrl"] = ReturnUrl });
        }

        await SignInManager.SignInAsync(user, isPersistent: false);
        RedirectManager.RedirectTo(ReturnUrl);
    }
    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!UserManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)UserStore;
    }
}
