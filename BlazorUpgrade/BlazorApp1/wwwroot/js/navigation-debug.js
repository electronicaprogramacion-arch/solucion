// Navigation Debug Script - ENHANCED VERSION
// This script tracks ALL possible navigation events to help identify what's causing the login page to disappear

(function() {
    console.log('='.repeat(80));
    console.log('[NAV DEBUG] ENHANCED Navigation debug script loaded at', new Date().toLocaleTimeString());
    console.log('[NAV DEBUG] User Agent:', navigator.userAgent);
    console.log('[NAV DEBUG] Page loaded from:', document.referrer || 'direct navigation');

    // Track the original URL
    let originalUrl = window.location.href;
    console.log('[NAV DEBUG] Original URL:', originalUrl);
    console.log('[NAV DEBUG] Document ready state:', document.readyState);

    // Track all navigation events
    let navigationCount = 0;

    // Override window.location.href setter
    let originalLocationHref = Object.getOwnPropertyDescriptor(window.location, 'href') ||
                              Object.getOwnPropertyDescriptor(Location.prototype, 'href');

    // Track history API calls
    const originalPushState = history.pushState;
    const originalReplaceState = history.replaceState;

    history.pushState = function(...args) {
        navigationCount++;
        console.log(`[NAV DEBUG] ${navigationCount}. history.pushState called at`, new Date().toLocaleTimeString());
        console.log('[NAV DEBUG] pushState args:', args);
        console.log('[NAV DEBUG] Stack trace:', new Error().stack);
        return originalPushState.apply(this, args);
    };

    history.replaceState = function(...args) {
        navigationCount++;
        console.log(`[NAV DEBUG] ${navigationCount}. history.replaceState called at`, new Date().toLocaleTimeString());
        console.log('[NAV DEBUG] replaceState args:', args);
        console.log('[NAV DEBUG] Stack trace:', new Error().stack);
        return originalReplaceState.apply(this, args);
    };

    // Track popstate events
    window.addEventListener('popstate', function(event) {
        navigationCount++;
        console.log(`[NAV DEBUG] ${navigationCount}. popstate event at`, new Date().toLocaleTimeString());
        console.log('[NAV DEBUG] popstate event:', event);
        console.log('[NAV DEBUG] Current URL:', window.location.href);
    });

    // Track beforeunload events
    window.addEventListener('beforeunload', function(event) {
        console.log('[NAV DEBUG] beforeunload event at', new Date().toLocaleTimeString());
        console.log('[NAV DEBUG] Leaving URL:', window.location.href);
    });

    // Track hashchange events
    window.addEventListener('hashchange', function(event) {
        navigationCount++;
        console.log(`[NAV DEBUG] ${navigationCount}. hashchange event at`, new Date().toLocaleTimeString());
        console.log('[NAV DEBUG] hashchange from:', event.oldURL, 'to:', event.newURL);
    });

    // Monitor URL changes with polling
    let lastUrl = window.location.href;
    setInterval(function() {
        if (window.location.href !== lastUrl) {
            navigationCount++;
            console.log(`[NAV DEBUG] ${navigationCount}. URL changed detected at`, new Date().toLocaleTimeString());
            console.log('[NAV DEBUG] From:', lastUrl);
            console.log('[NAV DEBUG] To:', window.location.href);
            lastUrl = window.location.href;
        }
    }, 100);

    // Track Blazor navigation events if available
    if (window.Blazor) {
        console.log('[NAV DEBUG] Blazor detected, setting up Blazor navigation tracking');

        // Try to hook into Blazor's navigation
        const originalNavigateTo = window.Blazor.navigateTo;
        if (originalNavigateTo) {
            window.Blazor.navigateTo = function(uri, forceLoad, replace) {
                navigationCount++;
                console.log(`[NAV DEBUG] ${navigationCount}. Blazor.navigateTo called at`, new Date().toLocaleTimeString());
                console.log('[NAV DEBUG] Blazor navigateTo URI:', uri);
                console.log('[NAV DEBUG] Blazor navigateTo forceLoad:', forceLoad);
                console.log('[NAV DEBUG] Blazor navigateTo replace:', replace);
                console.log('[NAV DEBUG] Stack trace:', new Error().stack);
                return originalNavigateTo.call(this, uri, forceLoad, replace);
            };
        }
    } else {
        console.log('[NAV DEBUG] Blazor not yet available, will check again...');

        // Check for Blazor periodically
        let blazorCheckCount = 0;
        const blazorChecker = setInterval(function() {
            blazorCheckCount++;
            if (window.Blazor) {
                console.log('[NAV DEBUG] Blazor detected after', blazorCheckCount, 'checks');
                clearInterval(blazorChecker);

                const originalNavigateTo = window.Blazor.navigateTo;
                if (originalNavigateTo) {
                    window.Blazor.navigateTo = function(uri, forceLoad, replace) {
                        navigationCount++;
                        console.log(`[NAV DEBUG] ${navigationCount}. Blazor.navigateTo called at`, new Date().toLocaleTimeString());
                        console.log('[NAV DEBUG] Blazor navigateTo URI:', uri);
                        console.log('[NAV DEBUG] Blazor navigateTo forceLoad:', forceLoad);
                        console.log('[NAV DEBUG] Blazor navigateTo replace:', replace);
                        console.log('[NAV DEBUG] Stack trace:', new Error().stack);
                        return originalNavigateTo.call(this, uri, forceLoad, replace);
                    };
                }
            } else if (blazorCheckCount > 50) {
                console.log('[NAV DEBUG] Blazor not found after 50 checks, stopping');
                clearInterval(blazorChecker);
            }
        }, 100);
    }

    // Track any setTimeout/setInterval calls that might cause delayed navigation
    const originalSetTimeout = window.setTimeout;
    const originalSetInterval = window.setInterval;

    window.setTimeout = function(callback, delay, ...args) {
        if (delay > 1000 && delay < 5000) { // Focus on 1-5 second delays
            console.log(`[NAV DEBUG] setTimeout with ${delay}ms delay registered at`, new Date().toLocaleTimeString());
            console.log('[NAV DEBUG] setTimeout stack trace:', new Error().stack);
        }
        return originalSetTimeout.call(this, callback, delay, ...args);
    };

    window.setInterval = function(callback, delay, ...args) {
        if (delay > 1000 && delay < 5000) { // Focus on 1-5 second intervals
            console.log(`[NAV DEBUG] setInterval with ${delay}ms delay registered at`, new Date().toLocaleTimeString());
            console.log('[NAV DEBUG] setInterval stack trace:', new Error().stack);
        }
        return originalSetInterval.call(this, callback, delay, ...args);
    };

    // Track all errors
    window.addEventListener('error', function(event) {
        console.error('[NAV DEBUG] JavaScript Error:', event.error);
        console.error('[NAV DEBUG] Error source:', event.filename, 'line:', event.lineno);
        console.error('[NAV DEBUG] Error message:', event.message);
    });

    window.addEventListener('unhandledrejection', function(event) {
        console.error('[NAV DEBUG] Unhandled Promise Rejection:', event.reason);
    });

    // Track DOM mutations that might affect the login form
    if (window.MutationObserver) {
        const observer = new MutationObserver(function(mutations) {
            mutations.forEach(function(mutation) {
                if (mutation.type === 'childList') {
                    mutation.removedNodes.forEach(function(node) {
                        if (node.nodeType === 1) { // Element node
                            if (node.querySelector && (
                                node.querySelector('form') ||
                                node.querySelector('[type="password"]') ||
                                node.querySelector('.login') ||
                                node.id === 'login-form' ||
                                node.className.includes('login')
                            )) {
                                console.warn('[NAV DEBUG] LOGIN FORM REMOVED FROM DOM at', new Date().toLocaleTimeString());
                                console.warn('[NAV DEBUG] Removed element:', node);
                                console.warn('[NAV DEBUG] Stack trace:', new Error().stack);
                            }
                        }
                    });

                    mutation.addedNodes.forEach(function(node) {
                        if (node.nodeType === 1) { // Element node
                            if (node.querySelector && node.querySelector('.text-danger, .alert-danger, [class*="error"], [class*="not-found"]')) {
                                console.warn('[NAV DEBUG] ERROR CONTENT ADDED TO DOM at', new Date().toLocaleTimeString());
                                console.warn('[NAV DEBUG] Error element:', node);
                                console.warn('[NAV DEBUG] Error text:', node.textContent);
                            }
                        }
                    });
                }
            });
        });

        observer.observe(document.body, {
            childList: true,
            subtree: true
        });

        console.log('[NAV DEBUG] DOM mutation observer installed');
    }

    // Track fetch requests that might cause redirects
    const originalFetch = window.fetch;
    window.fetch = function(...args) {
        console.log('[NAV DEBUG] Fetch request:', args[0]);
        return originalFetch.apply(this, args).then(response => {
            if (response.redirected) {
                console.log('[NAV DEBUG] Fetch redirected to:', response.url);
            }
            if (!response.ok) {
                console.warn('[NAV DEBUG] Fetch failed:', response.status, response.statusText);
            }
            return response;
        });
    };

    console.log('[NAV DEBUG] All navigation tracking hooks installed');
    console.log('='.repeat(80));
})();
