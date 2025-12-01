// Custom JavaScript interop functions

window.appInterop = {
    // Function to safely invoke a Blazor method
    safeInvoke: function (dotNetReference, methodName, ...args) {
        try {
            return dotNetReference.invokeMethodAsync(methodName, ...args);
        } catch (error) {
            console.warn(`Error invoking ${methodName}:`, error);
            return Promise.resolve(null);
        }
    },

    // Function to initialize Radzen components safely
    initializeRadzen: function () {
        try {
            // Add any Radzen initialization code here
            console.log("Radzen components initialized safely");
        } catch (error) {
            console.warn("Error initializing Radzen components:", error);
        }
    },

    // Handle renderer errors
    handleRendererError: function() {
        console.log("Handling potential renderer issues");
        // Store active renderers to help with debugging
        window._activeRenderers = window._activeRenderers || {};

        // Check if Blazor is defined
        if (typeof Blazor !== 'undefined') {
            // Add a hook for renderer registration if possible
            const originalRegister = Blazor._internal?.registerCustomEventType;
            if (originalRegister) {
                Blazor._internal.registerCustomEventType = function(eventName, renderer) {
                    console.log(`Registering event ${eventName} for renderer ${renderer?.id}`);
                    if (renderer) {
                        window._activeRenderers[renderer.id] = true;
                    }
                    return originalRegister(eventName, renderer);
                };
            }
        }
    }
};
