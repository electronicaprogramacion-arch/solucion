(() => {
    const maximumRetryCount = 8;
    const retryIntervalMilliseconds = 2000;
    const reconnectModal = document.getElementById('reconnect-modal');

    // Resource caching strategy for faster WASM downloads
    const criticalResources = new Set([
        'dotnet.wasm', 'dotnet.js', 'blazor.boot.json',
        'System.Private.CoreLib.dll', 'Microsoft.AspNetCore.Components.WebAssembly.dll'
    ]);

    const startReconnectionProcess = () => {
        reconnectModal.style.display = 'block';
        let isCanceled = false;

        (async () => {
            for (let i = 0; i < maximumRetryCount; i++) {
                reconnectModal.innerText = `Reconnecting... ${i + 1}/${maximumRetryCount}`;
                
                // Progressive backoff: start fast, then slow down
                const delay = i < 3 ? retryIntervalMilliseconds : retryIntervalMilliseconds * (i - 1);
                await new Promise(resolve => setTimeout(resolve, delay));

                if (isCanceled) return;

                try {
                    const result = await Blazor.reconnect();
                    if (!result) {
                        location.reload();
                        return;
                    }
                    return;
                } catch (error) {
                    console.warn(`Reconnection attempt ${i + 1} failed:`, error);
                }
            }
            
            // Final attempt with page reload
            console.log('Max reconnection attempts reached, reloading page');
            location.reload();
        })();

        return {
            cancel: () => {
                isCanceled = true;
                reconnectModal.style.display = 'none';
            }
        };
    };

    let currentReconnectionProcess = null;

    // Network status monitoring
    let isOnline = navigator.onLine;
    window.addEventListener('online', () => {
        isOnline = true;
        console.log('Network connection restored');
    });
    
    window.addEventListener('offline', () => {
        isOnline = false;
        console.log('Network connection lost');
    });

    Blazor.start({
        // OPTIMIZATION 1: Enhanced loadBootResource for faster WASM downloads
        loadBootResource: function (type, name, defaultUri, integrity) {
            console.log(`Loading resource: ${type} - ${name}`);
            
            // Skip dotnetjs for faster startup (handled by runtime)
            if (type === 'dotnetjs') {
                return null;
            }

            // Determine caching strategy based on resource type and criticality
            let cacheStrategy = 'default';
            let customHeaders = { 'Custom-Header': 'Blazor-WASM' };

            if (criticalResources.has(name) || type === 'dotnetwasm') {
                // Critical resources: aggressive caching with fallback
                cacheStrategy = 'force-cache';
                customHeaders['Cache-Control'] = 'public, max-age=31536000, immutable';
            } else if (type === 'assembly') {
                // Assemblies: cache but allow revalidation
                cacheStrategy = 'default';
                customHeaders['Cache-Control'] = 'public, max-age=3600, must-revalidate';
            } else {
                // Other resources: no-cache for development, cache for production
                cacheStrategy = 'no-cache';
            }

            // Enhanced fetch with retry logic and performance optimizations
            return fetchWithRetry(defaultUri, {
                cache: cacheStrategy,
                integrity: integrity,
                headers: customHeaders,
                mode: 'cors',
                credentials: 'same-origin'
            }, 3);
        },

        // OPTIMIZATION 2: WebAssembly runtime optimizations
        webAssembly: {
            // Enable streaming compilation for faster startup
            streamingCompilation: true,
            // Optimize memory usage
            initialMemorySize: 16777216, // 16MB
            maximumMemorySize: 134217728  // 128MB
        },

        // OPTIMIZATION 3: Enhanced SignalR configuration for robust reconnection
        configureSignalR: function (builder) {
            console.log('Configuring SignalR connection');
            
            builder
                .withUrl("/signalr", {
                    transport: Microsoft.AspNetCore.SignalR.HttpTransportType.WebSockets |
                              Microsoft.AspNetCore.SignalR.HttpTransportType.ServerSentEvents |
                              Microsoft.AspNetCore.SignalR.HttpTransportType.LongPolling,
                    skipNegotiation: false
                })
                .withServerTimeout(45000)        // 45 seconds server timeout
                .withKeepAliveInterval(15000)    // 15 seconds keep-alive
                .withStatefulReconnect()         // Enable stateful reconnect if available
                .configureLogging(Microsoft.AspNetCore.SignalR.LogLevel.Information);

            // Override build to apply additional connection settings
            const originalBuild = builder.build;
            builder.build = () => {
                const connection = originalBuild.call(builder);
                
                // Enhanced connection event handlers
                connection.onclose((error) => {
                    console.warn('SignalR connection closed:', error);
                    if (!isOnline) {
                        console.log('Device is offline, will retry when online');
                    }
                });

                connection.onreconnecting((error) => {
                    console.log('SignalR reconnecting:', error);
                });

                connection.onreconnected((connectionId) => {
                    console.log('SignalR reconnected:', connectionId);
                });

                return connection;
            };
        },

        // OPTIMIZATION 4: Advanced reconnection options
        reconnectionOptions: {
            maxRetries: 15,                    // Increased retry attempts
            retryIntervalMilliseconds: [       // Progressive backoff intervals
                1000,   // 1s
                2000,   // 2s  
                3000,   // 3s
                5000,   // 5s
                8000,   // 8s
                10000,  // 10s
                15000,  // 15s
                20000,  // 20s
                30000   // 30s (then repeat last interval)
            ]
        },

        // OPTIMIZATION 5: Custom reconnection handler with offline awareness
        reconnectionHandler: {
            onConnectionDown: () => {
                console.log('Blazor connection down');
                
                // Don't start reconnection if we're offline
                if (!isOnline) {
                    console.log('Device offline, waiting for network...');
                    
                    // Wait for network to come back online
                    const waitForOnline = () => {
                        if (navigator.onLine) {
                            console.log('Network restored, starting reconnection');
                            currentReconnectionProcess ??= startReconnectionProcess();
                        } else {
                            setTimeout(waitForOnline, 1000);
                        }
                    };
                    waitForOnline();
                } else {
                    currentReconnectionProcess ??= startReconnectionProcess();
                }
            },
            
            onConnectionUp: () => {
                console.log('Blazor connection restored');
                currentReconnectionProcess?.cancel();
                currentReconnectionProcess = null;
            }
        },

        // OPTIMIZATION 6: Circuit options for better performance
        circuit: {
            reconnectionOptions: {
                maxRetries: 10,
                retryIntervalMilliseconds: 3000
            },
            detailedErrors: true
        }
    });

    // Helper function for fetch with retry logic
    async function fetchWithRetry(url, options, maxRetries = 3) {
        for (let attempt = 1; attempt <= maxRetries; attempt++) {
            try {
                const response = await fetch(url, options);
                
                if (response.ok) {
                    return response;
                }
                
                if (attempt === maxRetries) {
                    throw new Error(`Failed to fetch ${url} after ${maxRetries} attempts`);
                }
                
                console.warn(`Fetch attempt ${attempt} failed for ${url}, retrying...`);
                await new Promise(resolve => setTimeout(resolve, 1000 * attempt));
                
            } catch (error) {
                if (attempt === maxRetries) {
                    console.error(`Final fetch attempt failed for ${url}:`, error);
                    throw error;
                }
                
                console.warn(`Fetch attempt ${attempt} failed for ${url}:`, error.message);
                await new Promise(resolve => setTimeout(resolve, 1000 * attempt));
            }
        }
    }

    // Performance monitoring
    console.log('Blazor.start() configuration loaded with optimizations');
    
    // Monitor initial load performance
    window.addEventListener('load', () => {
        if (performance.mark) {
            performance.mark('blazor-load-complete');
            console.log('Blazor application load completed');
        }
    });
})();
