using BlazorApp1.Services;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorApp1.Blazor.Services
{
    public class CookieThemeService : ThemeService
    {
        private readonly IJSRuntime _jsRuntime;
        private const string COOKIE_NAME = "CalibrifyTheme";
        private const int COOKIE_DAYS = 365; // Store for a year

        public CookieThemeService(IJSRuntime jsRuntime) : base(jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public override async Task InitializeAsync()
        {
            var storedTheme = await _jsRuntime.InvokeAsync<string>("getCookie", COOKIE_NAME);
            if (!string.IsNullOrEmpty(storedTheme))
            {
                await base.SetTheme(storedTheme);
            }
        }

        public override async Task SetTheme(string theme)
        {
            await base.SetTheme(theme);

            // Also store in a cookie for server-side usage
            await _jsRuntime.InvokeVoidAsync("setCookie", COOKIE_NAME, theme, COOKIE_DAYS);
        }
    }
}