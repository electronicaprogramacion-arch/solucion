// Cache Warmer Script
// This script ensures critical resources are cached for offline use

(function() {
    // List of critical resources to cache
    const criticalResources = [
        'index.html',
        '_framework/blazor.web.js',
        '_framework/dotnet.js',
        'css/sb-admin-2.css',
        'css/site.css',
        'js/offline-detector.js',
        'lib/fontawesome-free/css/all.min.css',
        'img/logo/Transparent Logo.svg',
        'img/logo/favicon.svg'
    ];
    
    // Function to warm up the cache
    async function warmCache() {
        console.log('Cache warmer: Starting cache warm-up');
        
        if (!('caches' in window)) {
            console.warn('Cache API not supported');
            return;
        }
        
        try {
            // Open the cache
            const cache = await caches.open('offline-app-cache');
            
            // Cache each resource
            for (const resource of criticalResources) {
                try {
                    console.log(`Cache warmer: Caching ${resource}`);
                    const response = await fetch(resource, { cache: 'reload' });
                    if (response.ok) {
                        await cache.put(resource, response);
                    }
                } catch (error) {
                    console.warn(`Cache warmer: Failed to cache ${resource}`, error);
                }
            }
            
            console.log('Cache warmer: Cache warm-up complete');
            
            // Cache the current page
            const currentPageUrl = window.location.href;
            try {
                const response = await fetch(currentPageUrl, { cache: 'reload' });
                if (response.ok) {
                    await cache.put(currentPageUrl, response);
                    console.log(`Cache warmer: Cached current page: ${currentPageUrl}`);
                }
            } catch (error) {
                console.warn(`Cache warmer: Failed to cache current page`, error);
            }
        } catch (error) {
            console.error('Cache warmer: Error warming cache', error);
        }
    }
    
    // Run the cache warmer when the page is loaded and idle
    if ('requestIdleCallback' in window) {
        requestIdleCallback(() => {
            warmCache();
        });
    } else {
        // Fallback for browsers that don't support requestIdleCallback
        setTimeout(() => {
            warmCache();
        }, 1000);
    }
})();
