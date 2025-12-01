// Development Service Worker - NO FETCH INTERCEPTION
// This service worker registers but doesn't intercept any requests to avoid development issues

self.addEventListener('install', event => event.waitUntil(onInstall(event)));
self.addEventListener('activate', event => event.waitUntil(onActivate(event)));
// NOTE: No fetch event listener in development to avoid interference

const cacheNamePrefix = 'dev-cache-';

async function onInstall(event) {
    console.info('Service worker: Install (Development Mode - No Fetch Interception)');
    // Skip waiting to activate immediately
    self.skipWaiting();
}

async function onActivate(event) {
    console.info('Service worker: Activate (Development Mode - No Fetch Interception)');
    // Take control of all clients immediately
    self.clients.claim();

    // Delete all caches to ensure fresh content in development
    const cacheNames = await caches.keys();
    await Promise.all(cacheNames.map(cacheName => caches.delete(cacheName)));

    console.info('Development service worker activated - all requests will go directly to network');
}

// No onFetch function in development mode to avoid any interference with Blazor loading