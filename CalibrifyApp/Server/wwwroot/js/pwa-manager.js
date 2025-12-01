// PWA Manager for Calibrify App - Enhanced for Render Mode Auto
// This script handles PWA functionality including:
// - Service worker registration
// - Online/offline detection compatible with Blazor Server
// - Installation detection
// - Network status monitoring
// - Offline mode handling
// - Data synchronization support

(function () {
    // Store the current network status
    let isOnline = navigator.onLine;
    let isInstalled = false;
    let isOfflineMode = localStorage.getItem('calibrify_offline_mode') === 'true';
    let isRenderModeAuto = true; // Flag to handle render mode auto specifics

    // Store references to .NET components for callbacks
    let dotNetReferences = [];

    // Check if the app is installed (in standalone mode)
    function checkIfInstalled() {
        try {
            // iOS detection
            if (window.navigator.standalone) {
                return true;
            }

            // Android/Chrome detection
            if (window.matchMedia('(display-mode: standalone)').matches) {
                return true;
            }

            return false;
        } catch (error) {
            console.warn("Error checking if app is installed:", error);
            return false;
        }
    }

    // Initialize the PWA manager
    function initialize() {
        // Handle offline startup first
        window.handleOfflineStartup();
        console.log("PWA Manager: Initializing");

        // Check initial installation status
        isInstalled = checkIfInstalled();
        console.log("PWA Manager: Is installed:", isInstalled);

        // Register the service worker
        if ('serviceWorker' in navigator) {
            // Use the published service worker in production
            const serviceWorkerUrl = window.location.hostname.includes('localhost')
                ? 'service-worker.js'
                : 'service-worker.published.js';

            navigator.serviceWorker.register(serviceWorkerUrl)
                .then(registration => {
                    console.log(`PWA Manager: Service worker registered (scope: ${registration.scope})`);

                    // Handle service worker updates
                    registration.onupdatefound = () => {
                        const installingWorker = registration.installing;
                        installingWorker.onstatechange = () => {
                            if (installingWorker.state === 'installed') {
                                if (navigator.serviceWorker.controller) {
                                    console.log('PWA Manager: New content is available; please refresh.');
                                    notifyUpdateAvailable();
                                } else {
                                    console.log('PWA Manager: Content is cached for offline use.');
                                }
                            }
                        };
                    };
                })
                .catch(error => {
                    console.error('PWA Manager: Service worker registration failed:', error);
                });
        }

        // Set up online/offline event listeners
        window.addEventListener('online', handleNetworkChange);
        window.addEventListener('offline', handleNetworkChange);

        // Set up beforeinstallprompt event listener
        window.addEventListener('beforeinstallprompt', (e) => {
            console.log('PWA Manager: beforeinstallprompt fired');
            // Optionally, store the event for later use
            window.deferredPrompt = e;
        });

        // Set up appinstalled event listener
        window.addEventListener('appinstalled', (e) => {
            console.log('PWA Manager: App was installed');
            isInstalled = true;
            notifyInstallationChange(true);
        });
    }

    // Handle network status changes - Enhanced for Render Mode Auto
    function handleNetworkChange() {
        const newStatus = navigator.onLine;
        console.log(`PWA Manager: Network status changed to ${newStatus ? 'online' : 'offline'}`);

        // Only notify if the status actually changed
        if (isOnline !== newStatus) {
            isOnline = newStatus;

            // For installed PWAs, automatically enable offline mode when going offline
            if (!newStatus && isInstalled && !isOfflineMode) {
                console.log('PWA Manager: Auto-enabling offline mode for installed app');
                isOfflineMode = true;
                localStorage.setItem('calibrify_offline_mode', 'true');
            }

            // Notify components with proper error handling for Blazor Server
            notifyNetworkStatusChange();

            // Also update UI elements directly for immediate feedback
            updateNetworkUI();
        }
    }

    // Update UI elements directly for immediate visual feedback
    function updateNetworkUI() {
        try {
            // Update WiFi icon if it exists
            const wifiIcons = document.querySelectorAll('.connection-icon, [class*="wifi"], [class*="signal"]');
            wifiIcons.forEach(icon => {
                if (isOnline && !isOfflineMode) {
                    icon.classList.remove('offline', 'disconnected');
                    icon.classList.add('connected');
                } else {
                    icon.classList.remove('connected');
                    icon.classList.add('offline', 'disconnected');
                }
            });

            // Update status text if it exists
            const statusTexts = document.querySelectorAll('.status-text, .connection-status');
            statusTexts.forEach(text => {
                if (text.textContent) {
                    text.textContent = (isOnline && !isOfflineMode) ? 'Online' : 'Offline';
                }
            });
        } catch (error) {
            console.warn('PWA Manager: Error updating UI elements:', error);
        }
    }

    // Notify all registered components of network status change - Enhanced for Blazor Server
    function notifyNetworkStatusChange() {
        const effectiveStatus = isOfflineMode ? false : isOnline;

        // Notify registered .NET components
        dotNetReferences.forEach(ref => {
            try {
                if (ref && typeof ref.invokeMethodAsync === 'function') {
                    ref.invokeMethodAsync('OnNetworkStatusChanged', effectiveStatus);
                }
            } catch (error) {
                console.error('PWA Manager: Error notifying component of network change:', error);
            }
        });

        // Also try to notify through global window object for compatibility
        try {
            if (window.dotNetReference && typeof window.dotNetReference.invokeMethodAsync === 'function') {
                window.dotNetReference.invokeMethodAsync('OnNetworkStatusChanged', effectiveStatus);
            }
        } catch (error) {
            console.warn('PWA Manager: Could not notify through global dotNetReference:', error);
        }

        // Dispatch custom event for components that listen to events
        try {
            const event = new CustomEvent('networkStatusChanged', {
                detail: {
                    isOnline: effectiveStatus,
                    isOfflineMode: isOfflineMode,
                    actualNetworkStatus: isOnline
                }
            });
            window.dispatchEvent(event);
        } catch (error) {
            console.warn('PWA Manager: Could not dispatch custom event:', error);
        }
    }

    // Notify all registered components of installation status change
    function notifyInstallationChange(installed) {
        dotNetReferences.forEach(ref => {
            try {
                ref.invokeMethodAsync('OnInstallationChanged', installed);
            } catch (error) {
                console.error('PWA Manager: Error notifying component of installation change:', error);
            }
        });
    }

    // Notify all registered components of update availability
    function notifyUpdateAvailable() {
        dotNetReferences.forEach(ref => {
            try {
                ref.invokeMethodAsync('OnUpdateAvailable');
            } catch (error) {
                console.error('PWA Manager: Error notifying component of update:', error);
            }
        });
    }

    // Register a .NET component to receive notifications
    window.registerPwaComponent = function (dotNetRef) {
        if (dotNetRef && typeof dotNetRef.invokeMethodAsync === 'function') {
            // Add to our list of references if not already present
            if (!dotNetReferences.includes(dotNetRef)) {
                dotNetReferences.push(dotNetRef);
                console.log('PWA Manager: Component registered');

                // Immediately notify the component of the current status
                try {
                    dotNetRef.invokeMethodAsync('OnNetworkStatusChanged', isOnline);
                    dotNetRef.invokeMethodAsync('OnInstallationChanged', isInstalled);
                } catch (error) {
                    console.error('PWA Manager: Error in initial notification:', error);
                }
            }
        }
    };

    // Unregister a .NET component
    window.unregisterPwaComponent = function (dotNetRef) {
        const index = dotNetReferences.indexOf(dotNetRef);
        if (index !== -1) {
            dotNetReferences.splice(index, 1);
            console.log('PWA Manager: Component unregistered');
        }
    };

    // Get current browser dimensions and status
    window.getDimensions = function () {
        // Update installation status
        isInstalled = checkIfInstalled();

        // For offline mode, we need to consider both the actual network status
        // and whether the user has manually enabled offline mode
        const effectiveOnlineStatus = isOfflineMode ? false : navigator.onLine;

        return {
            width: window.innerWidth,
            height: window.innerHeight,
            online: effectiveOnlineStatus,
            scroll: window.pageYOffset || document.documentElement.scrollTop || 0,
            install: isInstalled
        };
    };

    // Toggle offline mode - Enhanced with data sync support
    window.toggleOfflineMode = function() {
        const previousMode = isOfflineMode;
        isOfflineMode = !isOfflineMode;
        console.log(`PWA Manager: Manually toggled offline mode to ${isOfflineMode ? 'enabled' : 'disabled'}`);

        // Store the offline mode preference in localStorage
        if (isOfflineMode) {
            localStorage.setItem('calibrify_offline_mode', 'true');
            console.log('PWA Manager: Offline mode enabled - data will be cached locally');
        } else {
            localStorage.removeItem('calibrify_offline_mode');
            console.log('PWA Manager: Offline mode disabled - attempting to sync data');

            // Trigger data synchronization when going back online
            if (previousMode && navigator.onLine) {
                triggerDataSync();
            }
        }

        // Update the UI to reflect the current network status
        updateNetworkUI();
        notifyNetworkStatusChange();

        // Dispatch event for data sync components
        try {
            const event = new CustomEvent('offlineModeToggled', {
                detail: {
                    isOfflineMode: isOfflineMode,
                    previousMode: previousMode,
                    shouldSync: !isOfflineMode && previousMode && navigator.onLine
                }
            });
            window.dispatchEvent(event);
        } catch (error) {
            console.warn('PWA Manager: Could not dispatch offline mode toggle event:', error);
        }
    };

    // Trigger data synchronization
    function triggerDataSync() {
        console.log('PWA Manager: Triggering data synchronization...');
        try {
            // Notify .NET components about sync requirement
            dotNetReferences.forEach(ref => {
                try {
                    if (ref && typeof ref.invokeMethodAsync === 'function') {
                        ref.invokeMethodAsync('OnDataSyncRequired');
                    }
                } catch (error) {
                    console.error('PWA Manager: Error notifying component of sync requirement:', error);
                }
            });

            // Dispatch sync event
            const event = new CustomEvent('dataSyncRequired', {
                detail: { timestamp: Date.now() }
            });
            window.dispatchEvent(event);
        } catch (error) {
            console.error('PWA Manager: Error triggering data sync:', error);
        }
    }

    // Handle initial page load in offline mode
    window.handleOfflineStartup = function() {
        // Check if we're offline
        if (!navigator.onLine) {
            console.log('PWA Manager: Device is offline on startup');

            // If we're in a PWA, automatically enable offline mode
            if (isInstalled) {
                console.log('PWA Manager: Automatically enabling offline mode for installed app');
                isOfflineMode = true;
                localStorage.setItem('calibrify_offline_mode', 'true');
            }
        }
    };

    // Initialize when the DOM is loaded
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initialize);
    } else {
        initialize();
    }
})();
