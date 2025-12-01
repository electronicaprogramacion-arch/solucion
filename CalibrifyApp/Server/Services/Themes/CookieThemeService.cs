using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Services
{
    public class CookieThemeService
    {
        private readonly IJSRuntime _jsRuntime;
        private const string ThemeKey = "theme";

        public CookieThemeService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<string> GetThemeAsync()
        {
            try
            {
                var theme = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", ThemeKey);
                return string.IsNullOrEmpty(theme) ? "default" : theme;
            }
            catch
            {
                return "default";
            }
        }

        public async Task SetThemeAsync(string theme)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", ThemeKey, theme);
            await _jsRuntime.InvokeVoidAsync("eval", $"document.documentElement.setAttribute('data-theme', '{theme}')");
        }
    }
}
