// Service Worker Debug Utilities
// This script provides utilities for debugging service worker issues

(function() {
    'use strict';

    // Add debug utilities to window object
    window.swDebug = {
        // Clear all caches
        clearAllCaches: async function() {
            try {
                const cacheNames = await caches.keys();
                console.log('Found caches:', cacheNames);
                
                const deletePromises = cacheNames.map(cacheName => {
                    console.log('Deleting cache:', cacheName);
                    return caches.delete(cacheName);
                });
                
                await Promise.all(deletePromises);
                console.log('All caches cleared successfully');
                return true;
            } catch (error) {
                console.error('Error clearing caches:', error);
                return false;
            }
        },

        // Unregister all service workers
        unregisterAllServiceWorkers: async function() {
            try {
                if ('serviceWorker' in navigator) {
                    const registrations = await navigator.serviceWorker.getRegistrations();
                    console.log('Found service worker registrations:', registrations.length);
                    
                    const unregisterPromises = registrations.map(registration => {
                        console.log('Unregistering service worker:', registration.scope);
                        return registration.unregister();
                    });
                    
                    await Promise.all(unregisterPromises);
                    console.log('All service workers unregistered successfully');
                    return true;
                } else {
                    console.log('Service workers not supported');
                    return false;
                }
            } catch (error) {
                console.error('Error unregistering service workers:', error);
                return false;
            }
        },

        // Full reset: clear caches and unregister service workers
        fullReset: async function() {
            console.log('Starting full service worker reset...');
            
            const cacheResult = await this.clearAllCaches();
            const swResult = await this.unregisterAllServiceWorkers();
            
            if (cacheResult && swResult) {
                console.log('Full reset completed successfully. Please refresh the page.');
                return true;
            } else {
                console.log('Full reset completed with some errors. Check console for details.');
                return false;
            }
        },

        // Get service worker status
        getStatus: async function() {
            const status = {
                serviceWorkerSupported: 'serviceWorker' in navigator,
                registrations: [],
                caches: [],
                controller: null
            };

            if (status.serviceWorkerSupported) {
                try {
                    // Get registrations
                    const registrations = await navigator.serviceWorker.getRegistrations();
                    status.registrations = registrations.map(reg => ({
                        scope: reg.scope,
                        state: reg.active ? reg.active.state : 'no active worker'
                    }));

                    // Get controller
                    if (navigator.serviceWorker.controller) {
                        status.controller = {
                            scriptURL: navigator.serviceWorker.controller.scriptURL,
                            state: navigator.serviceWorker.controller.state
                        };
                    }

                    // Get caches
                    const cacheNames = await caches.keys();
                    status.caches = cacheNames;

                } catch (error) {
                    status.error = error.message;
                }
            }

            console.log('Service Worker Status:', status);
            return status;
        },

        // Force update service worker
        forceUpdate: async function() {
            try {
                if ('serviceWorker' in navigator) {
                    const registrations = await navigator.serviceWorker.getRegistrations();
                    
                    for (const registration of registrations) {
                        console.log('Forcing update for:', registration.scope);
                        await registration.update();
                    }
                    
                    console.log('Service worker update forced');
                    return true;
                } else {
                    console.log('Service workers not supported');
                    return false;
                }
            } catch (error) {
                console.error('Error forcing service worker update:', error);
                return false;
            }
        }
    };

    // Add console commands info
    console.log('Service Worker Debug Utilities loaded. Available commands:');
    console.log('- swDebug.getStatus() - Get current service worker status');
    console.log('- swDebug.clearAllCaches() - Clear all caches');
    console.log('- swDebug.unregisterAllServiceWorkers() - Unregister all service workers');
    console.log('- swDebug.fullReset() - Clear caches and unregister service workers');
    console.log('- swDebug.forceUpdate() - Force service worker update');

})();
