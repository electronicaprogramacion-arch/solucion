using Microsoft.JSInterop;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop.Infrastructure;

namespace BlazorApp1.Services
{
    public class ThemeService
    {
        private readonly IJSRuntime _jsRuntime;
        public event Action<string> ThemeChanged;

        private string _theme = "default";

        public string Theme
        {
            get => _theme;
            set
            {
                if (_theme != value)
                {
                    Console.WriteLine($"ThemeService: Theme property changing from {_theme} to {value}");
                    _theme = value;

                    // Only invoke the event if there are subscribers
                    if (ThemeChanged != null)
                    {
                        Console.WriteLine($"ThemeService: Invoking ThemeChanged event with {_theme}");
                        ThemeChanged.Invoke(_theme);
                    }
                }
            }
        }

        private static ThemeService _instance;

        public ThemeService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            _instance = this;
        }

        [JSInvokable("NotifyThemeChanged")]
        public static Task NotifyThemeChanged(string theme)
        {
            if (_instance != null && !string.IsNullOrEmpty(theme))
            {
                Console.WriteLine($"ThemeService.NotifyThemeChanged: Theme changed to {theme} via JS interop");
                return _instance.SetTheme(theme);
            }
            return Task.CompletedTask;
        }

        public virtual async Task InitializeAsync()
        {
            try
            {
                // First try to get the theme from the custom event
                var hasCustomEvent = await _jsRuntime.InvokeAsync<bool>("eval",
                    "typeof window.directTheme !== 'undefined' && typeof window.directTheme.availableThemes !== 'undefined'");

                if (hasCustomEvent)
                {
                    // Get the theme directly from the directTheme object
                    var currentTheme = await _jsRuntime.InvokeAsync<string>("eval",
                        "localStorage.getItem('calibration_theme') || document.cookie.split('; ').find(row => row.startsWith('CalibrifyTheme='))?.split('=')[1] || 'material'");

                    if (!string.IsNullOrEmpty(currentTheme))
                    {
                        Theme = currentTheme;
                        return;
                    }
                }

                // Fallback to localStorage
                var storedTheme = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "calibration_theme");
                if (!string.IsNullOrEmpty(storedTheme))
                {
                    Theme = storedTheme;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing theme: {ex.Message}");
                // Default to material theme if there's an error
                Theme = "material";
            }
        }

        // Add a debounce mechanism to prevent rapid theme changes
        private string _pendingTheme = null;
        private bool _isApplyingTheme = false;
        private readonly SemaphoreSlim _themeSemaphore = new SemaphoreSlim(1, 1);

        public virtual async Task SetTheme(string theme)
        {
            if (string.IsNullOrEmpty(theme))
                return;

            // Don't do anything if the theme is already set
            if (_theme == theme)
            {
                Console.WriteLine($"ThemeService.SetTheme: Theme {theme} is already set, skipping");
                return;
            }

            Console.WriteLine($"ThemeService.SetTheme: Setting theme to {theme}");

            // Update the theme property (this will trigger the ThemeChanged event)
            Theme = theme;

            // Use a semaphore to prevent concurrent theme changes
            await _themeSemaphore.WaitAsync();

            try
            {
                // If we're already applying a theme, just update the pending theme
                if (_isApplyingTheme)
                {
                    _pendingTheme = theme;
                    Console.WriteLine($"ThemeService.SetTheme: Already applying a theme, setting pending theme to {theme}");
                    return;
                }

                _isApplyingTheme = true;

                // Apply the theme and any pending themes
                await ApplyThemeWithStorage(theme);

                // Check if there's a pending theme that needs to be applied
                while (_pendingTheme != null && _pendingTheme != theme)
                {
                    theme = _pendingTheme;
                    _pendingTheme = null;

                    // Update the theme property again if needed
                    if (_theme != theme)
                    {
                        Theme = theme;
                    }

                    await ApplyThemeWithStorage(theme);
                }
            }
            finally
            {
                _isApplyingTheme = false;
                _themeSemaphore.Release();
            }
        }

        private async Task ApplyThemeWithStorage(string theme)
        {
            try
            {
                // Store the theme in localStorage
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "calibration_theme", theme);

                // Store the theme in a cookie for server-side rendering
                await _jsRuntime.InvokeVoidAsync("eval", $"document.cookie = 'CalibrifyTheme={theme}; path=/; max-age={365*24*60*60}'");

                // Apply the theme using JavaScript
                await ApplyTheme(theme);

                Console.WriteLine($"ThemeService.ApplyThemeWithStorage: Theme set to {theme} with JS interop");
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("JavaScript interop calls cannot be issued at this time"))
            {
                // This happens during prerendering - we'll just set the theme property
                // and let the JavaScript handle it after rendering
                Console.WriteLine($"ThemeService.ApplyThemeWithStorage: Cannot use JS interop during prerendering for theme {theme}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ThemeService.ApplyThemeWithStorage: Error setting theme: {ex.Message}");
            }
        }

        protected virtual async Task ApplyTheme(string theme)
        {
            try
            {
                // Try to use the direct theme implementation first
                try
                {
                    // Check if directTheme.apply is available
                    var directThemeAvailable = await _jsRuntime.InvokeAsync<bool>("eval",
                        "typeof window.directTheme !== 'undefined' && typeof window.directTheme.apply === 'function'");

                    if (directThemeAvailable)
                    {
                        Console.WriteLine("Using direct theme implementation");
                        var result = await _jsRuntime.InvokeAsync<bool>("directTheme.apply", theme);

                        if (result)
                        {
                            Console.WriteLine($"Theme applied successfully using direct theme: {theme}");
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Direct theme application returned false, falling back to standard implementation");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error using direct theme implementation: {ex.Message}. Falling back to standard implementation.");
                }

                // Fall back to the original theme implementation
                await _jsRuntime.InvokeVoidAsync("applyTheme", theme);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying theme: {ex.Message}");
            }
        }
    }
}