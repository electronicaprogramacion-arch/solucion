// Fluxor Blazor Web Scripts
// This file provides the necessary JavaScript interop for Fluxor state management

window.Fluxor = window.Fluxor || {};

// Initialize Fluxor JavaScript interop
window.Fluxor.init = function() {
    console.log('[FLUXOR] JavaScript interop initialized');
    
    // Add error handling for Fluxor operations
    window.addEventListener('error', function(event) {
        if (event.error && event.error.message && event.error.message.includes('Fluxor')) {
            console.warn('[FLUXOR] JavaScript error caught:', event.error.message);
            // Don't let Fluxor errors crash the app
            event.preventDefault();
        }
    });
    
    return true;
};

// Redux DevTools integration (if available)
window.Fluxor.reduxDevTools = function() {
    if (typeof window !== 'undefined' && window.__REDUX_DEVTOOLS_EXTENSION__) {
        console.log('[FLUXOR] Redux DevTools detected');
        return window.__REDUX_DEVTOOLS_EXTENSION__();
    }
    console.log('[FLUXOR] Redux DevTools not available');
    return null;
};

// State management helpers
window.Fluxor.stateHelpers = {
    // Helper to safely access state
    getState: function(stateName) {
        try {
            if (window.Blazor && window.Blazor.getState) {
                return window.Blazor.getState(stateName);
            }
            return null;
        } catch (error) {
            console.warn('[FLUXOR] Error getting state:', error.message);
            return null;
        }
    },
    
    // Helper to safely dispatch actions
    dispatch: function(action) {
        try {
            if (window.Blazor && window.Blazor.dispatch) {
                return window.Blazor.dispatch(action);
            }
            console.warn('[FLUXOR] Blazor dispatch not available');
            return false;
        } catch (error) {
            console.warn('[FLUXOR] Error dispatching action:', error.message);
            return false;
        }
    }
};

// Error boundary for Fluxor operations
window.Fluxor.errorBoundary = function(operation, fallback) {
    try {
        return operation();
    } catch (error) {
        console.warn('[FLUXOR] Operation failed, using fallback:', error.message);
        return fallback ? fallback() : null;
    }
};

// Initialize when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', function() {
        window.Fluxor.init();
    });
} else {
    window.Fluxor.init();
}

console.log('[FLUXOR] Fluxor JavaScript interop loaded');
