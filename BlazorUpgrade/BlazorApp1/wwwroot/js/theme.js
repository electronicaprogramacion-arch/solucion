// Theme management functions
window.setCookie = function (name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
    console.log(`Cookie set: ${name}=${value}`);
};

window.getCookie = function (name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
};

window.eraseCookie = function (name) {
    document.cookie = name + '=; Max-Age=-99999999;';
};

// Track the current theme to avoid unnecessary changes
window.currentAppliedTheme = null;

// Debounce function to prevent rapid theme changes
window.themeChangeTimeout = null;
window.themeChangeDelay = 100; // ms

window.applyTheme = function (theme) {
    // Don't do anything if the theme is already applied
    if (window.currentAppliedTheme === theme) {
        console.log("Theme already applied:", theme);
        return true;
    }

    // Clear any pending theme changes
    if (window.themeChangeTimeout) {
        clearTimeout(window.themeChangeTimeout);
    }

    console.log("Applying theme:", theme);
    try {
        // Remove any existing theme links
        var existingLinks = document.querySelectorAll('link[data-theme]');
        existingLinks.forEach(function (link) {
            link.disabled = true;
        });

        // Enable the selected theme
        var themeLink = document.querySelector('link[data-theme="' + theme + '"]');
        if (themeLink) {
            themeLink.disabled = false;
            console.log("Theme applied:", theme);
        } else {
            console.error("Theme not found:", theme);
            return false;
        }

        // Set theme class on body
        document.body.className = document.body.className.replace(/theme-\S+/g, '').trim();
        document.body.classList.add('theme-' + theme);

        // Store theme in localStorage for persistence
        localStorage.setItem('calibration_theme', theme);

        // Also store in cookie for server-side rendering
        setCookie('CalibrifyTheme', theme, 365);

        // Set data-theme attribute on html element for CSS variables
        document.documentElement.setAttribute('data-theme', theme);

        // Remember the current theme
        window.currentAppliedTheme = theme;

        // Only notify Blazor if this is not the initial load
        if (window.DotNet && typeof DotNet.invokeMethodAsync === 'function' && window.themeInitialized) {
            try {
                // Check if we have a renderer error before trying to invoke
                if (window.lastRendererError) {
                    console.warn("Skipping Blazor notification due to previous renderer error");
                    // Try to recover by initializing renderer error handling
                    if (typeof window.appInterop !== 'undefined' && typeof window.appInterop.handleRendererError === 'function') {
                        window.appInterop.handleRendererError();
                        window.lastRendererError = false;
                    }
                } else {
                    DotNet.invokeMethodAsync('CalibrationSaaS.Infraestructure.Blazor', 'NotifyThemeChanged', theme)
                        .then(function() {
                            console.log("Theme notification sent to Blazor components");
                        })
                        .catch(function(error) {
                            console.warn("Error notifying Blazor components about theme change:", error);
                            // Check if this is a renderer error
                            if (error && error.message && error.message.includes('No interop methods are registered for renderer')) {
                                window.lastRendererError = true;
                                // Try to recover
                                if (typeof window.appInterop !== 'undefined' && typeof window.appInterop.handleRendererError === 'function') {
                                    window.appInterop.handleRendererError();
                                }
                            }
                        });
                }
            } catch (e) {
                console.warn("Error using DotNet.invokeMethodAsync:", e);
                // Check if this is a renderer error
                if (e && e.message && e.message.includes('No interop methods are registered for renderer')) {
                    window.lastRendererError = true;
                    // Try to recover
                    if (typeof window.appInterop !== 'undefined' && typeof window.appInterop.handleRendererError === 'function') {
                        window.appInterop.handleRendererError();
                    }
                }
            }
        }

        // Force a page refresh if all else fails
        if (window.forceRefreshOnThemeChange === true) {
            console.log("Forcing page refresh to apply theme");
            window.location.reload();
        }

        return true;
    } catch (error) {
        console.error("Error applying theme:", error);
        console.error(error.stack);
        return false;
    }
};

// Check if theme is applied correctly
window.isThemeApplied = function(theme) {
    try {
        // Check if the theme link is enabled
        var themeLink = document.querySelector('link[data-theme="' + theme + '"]');
        if (!themeLink || themeLink.disabled) {
            console.warn(`Theme ${theme} is not properly applied (link is disabled or missing)`);
            return false;
        }

        // Check if body has the theme class
        if (!document.body.classList.contains('theme-' + theme)) {
            console.warn(`Theme ${theme} is not properly applied (body class missing)`);
            return false;
        }

        return true;
    } catch (error) {
        console.error("Error checking if theme is applied:", error);
        return false;
    }
};

// Initialize theme from localStorage or cookie on page load
window.initializeTheme = function() {
    try {
        // Reset renderer error flag
        window.lastRendererError = false;

        // Use the same cookie name as in the Blazor project
        var theme = localStorage.getItem('calibration_theme') || getCookie('CalibrifyTheme') || 'humanistic';
        console.log("Initializing theme:", theme);

        // Apply the theme without notifying Blazor (to avoid loops)
        window.applyTheme(theme);

        // Mark that initialization is complete
        window.themeInitialized = true;

        // Initialize renderer error handling if available
        if (typeof window.appInterop !== 'undefined' && typeof window.appInterop.handleRendererError === 'function') {
            console.log("Initializing renderer error handling");
            window.appInterop.handleRendererError();
        }

        return theme;
    } catch (error) {
        console.error("Error initializing theme:", error);
        return 'humanistic';
    }
};