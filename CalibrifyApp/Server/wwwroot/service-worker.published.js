// Enhanced Service Worker for Blazor Server with Render Mode Auto
// Supports offline functionality while maintaining server-side rendering capabilities
// See https://aka.ms/blazor-offline-considerations

self.importScripts('./service-worker-assets.js');
self.addEventListener('install', event => event.waitUntil(onInstall(event)));
self.addEventListener('activate', event => event.waitUntil(onActivate(event)));
self.addEventListener('fetch', event => event.respondWith(onFetch(event)));
self.addEventListener('message', event => {
    if (event.data && event.data.type === 'SKIP_WAITING') {
        self.skipWaiting();
    }
});

const cacheNamePrefix = 'calibrify-cache-';
const cacheName = `${cacheNamePrefix}${self.assetsManifest.version}`;
const dataCacheName = 'calibrify-data-cache';

// Enhanced asset patterns for Blazor Server + Auto render mode
const offlineAssetsInclude = [
    /\.dll$/, /\.pdb$/, /\.wasm/, /\.html/, /\.js$/, /\.json$/, /\.css$/,
    /\.woff$/, /\.woff2$/, /\.ttf$/, /\.eot$/, /\.svg$/,
    /\.png$/, /\.jpe?g$/, /\.gif$/, /\.ico$/, /\.webp$/,
    /\.blat$/, /\.dat$/, /\.db$/,
    /_framework\/.*\.js$/, /_framework\/.*\.wasm$/, /_framework\/.*\.dll$/
];

const offlineAssetsExclude = [/^service-worker\.js$/, /^sw-registrator\.js$/];

async function onInstall(event) {
    console.info('Service worker: Install');

    // Activate the new service worker as soon as the old one is retired.
    self.skipWaiting();

    try {
        // Define critical files that must be cached for offline support
        const criticalFiles = [
            'index.html',
            '_framework/blazor.web.js',
            '_framework/dotnet.js',
            '_framework/blazor.server.js',
            'css/sb-admin-2.css',
            'css/site.css',
            'css/radzen-custom.css',
            'js/pwa-manager.js',
            'js/site.js',
            'manifest.json'
        ];

        // Fetch and cache all matching items from the assets manifest
        const assetsRequests = self.assetsManifest.assets
            .filter(asset => offlineAssetsInclude.some(pattern => pattern.test(asset.url)))
            .filter(asset => !offlineAssetsExclude.some(pattern => pattern.test(asset.url)))
            .map(asset => new Request(asset.url, { integrity: asset.hash, cache: 'no-cache' }));

        // Open the cache
        const cache = await caches.open(cacheName);

        // First, cache the critical files individually to ensure they're cached
        for (const file of criticalFiles) {
            try {
                console.log(`Caching critical file: ${file}`);
                const response = await fetch(file, { cache: 'no-cache' });
                if (response.ok) {
                    await cache.put(file, response);
                }
            } catch (error) {
                console.warn(`Failed to cache critical file: ${file}`, error);
            }
        }

        // Then cache all the assets from the manifest
        await cache.addAll(assetsRequests);
        console.log('Service worker: All assets cached');
    }
    catch (error) {
        console.error('Service worker install error:', error);
    }
}

async function onActivate(event) {
    console.info('Service worker: Activate');

    // Claim clients immediately so the service worker is in control
    await self.clients.claim();
    console.log('Service worker has claimed all clients');

    // Delete unused caches
    const cacheKeys = await caches.keys();
    await Promise.all(cacheKeys
        .filter(key => key.startsWith(cacheNamePrefix) && key !== cacheName)
        .map(key => caches.delete(key)));
}

async function onFetch(event) {
    // Skip chrome-extension and other unsupported schemes
    if (!event.request.url.startsWith('http://') && !event.request.url.startsWith('https://')) {
        console.log('Skipping non-HTTP request:', event.request.url);
        return fetch(event.request);
    }

    // Special handling for navigation requests (most important for offline support)
    // But ONLY for actual page navigation, not for resource requests
    if (event.request.mode === 'navigate' &&
        !event.request.url.includes('.js') &&
        !event.request.url.includes('.css') &&
        !event.request.url.includes('/_framework/') &&
        !event.request.url.includes('/api/') &&
        !event.request.url.includes('.json') &&
        !event.request.url.includes('.wasm') &&
        !event.request.url.includes('.dll')) {

        // Always try to serve index.html from cache first for navigation requests
        const cache = await caches.open(cacheName);
        const cachedIndexHtml = await cache.match('index.html');

        if (cachedIndexHtml) {
            console.log('Serving cached index.html for navigation request');
            return cachedIndexHtml;
        }

        // If index.html is not in cache, try the network
        try {
            console.log('Index.html not in cache, fetching from network');
            const response = await fetch(event.request);

            // Cache the response for future use
            if (response.ok) {
                await cache.put('index.html', response.clone());
            }

            return response;
        }
        catch (error) {
            console.error('Navigation fetch failed:', error);

            // Last resort: return a simple offline message
            return new Response(
                '<html><body><h1>You are offline</h1><p>Please check your internet connection and try again.</p></body></html>',
                { status: 200, headers: { 'Content-Type': 'text/html' } }
            );
        }
    }

    // Don't serve index.html for non-navigation requests (like JS, CSS, API calls)
    if (event.request.url.includes('.js') ||
        event.request.url.includes('.css') ||
        event.request.url.includes('/api/') ||
        event.request.url.includes('/_framework/') ||
        event.request.url.includes('.json') ||
        event.request.url.includes('.wasm') ||
        event.request.url.includes('.dll')) {

        // For these resources, try cache first, then network, but don't fallback to index.html
        const cachedResponse = await caches.match(event.request);
        if (cachedResponse) {
            return cachedResponse;
        }

        // Try network for these specific resources
        try {
            const response = await fetch(event.request);

            // Cache successful responses
            if (response.ok && event.request.method === 'GET') {
                try {
                    const cache = await caches.open(cacheName);
                    await cache.put(event.request, response.clone());
                } catch (cacheError) {
                    console.warn('Failed to cache response:', cacheError);
                }
            }

            return response;
        } catch (error) {
            console.error('Failed to fetch resource:', event.request.url, error);

            // Return appropriate error responses for different resource types
            if (event.request.url.includes('/api/')) {
                return new Response(JSON.stringify({ error: 'Network error', offline: true }),
                    { status: 503, headers: { 'Content-Type': 'application/json' } });
            } else if (event.request.url.includes('.js')) {
                return new Response('// Network error - script unavailable offline',
                    { status: 503, headers: { 'Content-Type': 'application/javascript' } });
            } else if (event.request.url.includes('.css')) {
                return new Response('/* Network error - stylesheet unavailable offline */',
                    { status: 503, headers: { 'Content-Type': 'text/css' } });
            } else {
                return new Response('Network error occurred',
                    { status: 503, headers: { 'Content-Type': 'text/plain' } });
            }
        }
    }

    // For all other requests (images, fonts, etc.), try the cache first, then network
    const cachedResponse = await caches.match(event.request);
    if (cachedResponse) {
        return cachedResponse;
    }

    // If not in cache, try the network
    try {
        const response = await fetch(event.request);

        // Cache successful GET responses for HTTP/HTTPS only
        if (event.request.method === 'GET' && response.ok &&
            (event.request.url.startsWith('http://') || event.request.url.startsWith('https://'))) {
            try {
                const cache = await caches.open(cacheName);
                await cache.put(event.request, response.clone());
            } catch (cacheError) {
                console.warn('Failed to cache response:', cacheError);
                // Continue without caching
            }
        }

        return response;
    }
    catch (error) {
        console.error('Fetch failed:', error);

        // Return appropriate error responses based on request type
        if (event.request.url.includes('/api/')) {
            return new Response(JSON.stringify({ error: 'Network error', offline: true }),
                { status: 503, headers: { 'Content-Type': 'application/json' } });
        } else if (event.request.url.includes('.js')) {
            return new Response('// Network error - script unavailable offline',
                { status: 503, headers: { 'Content-Type': 'application/javascript' } });
        } else if (event.request.url.includes('.css')) {
            return new Response('/* Network error - stylesheet unavailable offline */',
                { status: 503, headers: { 'Content-Type': 'text/css' } });
        } else {
            // For other requests (images, fonts, etc.), return a generic error
            return new Response('Network error occurred',
                { status: 503, headers: { 'Content-Type': 'text/plain' } });
        }
    }
}
