// Enhanced Blazor Error Handler
// Prevents malformed component errors and provides better error recovery

(function() {
    'use strict';
    
    console.log('[BLAZOR ERROR HANDLER] Initializing enhanced error handling');
    
    // Track error counts to prevent infinite loops
    const errorCounts = new Map();
    const maxErrorsPerType = 5;
    const errorResetInterval = 60000; // 1 minute
    
    // Reset error counts periodically
    setInterval(() => {
        errorCounts.clear();
        console.log('[BLAZOR ERROR HANDLER] Error counts reset');
    }, errorResetInterval);
    
    // Enhanced global error handler
    window.addEventListener('error', function(event) {
        const error = event.error;
        const message = error ? error.message : event.message;
        const source = event.filename || 'unknown';
        
        // Check if this is a Blazor-related error
        if (message && (
            message.includes('Found malformed component comment') ||
            message.includes('Blazor') ||
            message.includes('InteractiveAuto') ||
            message.includes('prerenderId')
        )) {
            console.warn('[BLAZOR ERROR HANDLER] Blazor error detected:', message);
            
            // Track error frequency
            const errorKey = message.substring(0, 100); // First 100 chars as key
            const count = errorCounts.get(errorKey) || 0;
            errorCounts.set(errorKey, count + 1);
            
            // If we've seen this error too many times, suggest page reload
            if (count >= maxErrorsPerType) {
                console.error('[BLAZOR ERROR HANDLER] Too many similar errors, suggesting reload');
                showErrorRecoveryDialog();
                return;
            }
            
            // Handle specific error types
            if (message.includes('Found malformed component comment')) {
                handleMalformedComponentError(error);
            } else if (message.includes('InteractiveAuto')) {
                handleInteractiveAutoError(error);
            }
            
            // Prevent the error from bubbling up and crashing the app
            event.preventDefault();
            return false;
        }
        
        // Log other errors normally
        console.error('[BLAZOR ERROR HANDLER] Unhandled error:', {
            message: message,
            source: source,
            line: event.lineno,
            column: event.colno,
            error: error
        });
    });
    
    // Handle unhandled promise rejections
    window.addEventListener('unhandledrejection', function(event) {
        const reason = event.reason;
        const message = reason ? reason.message || reason.toString() : 'Unknown rejection';
        
        if (message.includes('Blazor') || message.includes('component')) {
            console.warn('[BLAZOR ERROR HANDLER] Unhandled Blazor promise rejection:', message);
            
            // Try to recover gracefully
            setTimeout(() => {
                tryBlazorRecovery();
            }, 1000);
            
            // Prevent unhandled rejection
            event.preventDefault();
        }
    });
    
    // Handle malformed component errors specifically
    function handleMalformedComponentError(error) {
        console.log('[BLAZOR ERROR HANDLER] Handling malformed component error');
        
        // Try to clean up any corrupted DOM elements
        cleanupCorruptedElements();
        
        // Attempt to reinitialize Blazor components
        setTimeout(() => {
            tryComponentRecovery();
        }, 500);
    }
    
    // Handle InteractiveAuto render mode errors
    function handleInteractiveAutoError(error) {
        console.log('[BLAZOR ERROR HANDLER] Handling InteractiveAuto error');
        
        // These errors often resolve themselves, just log and continue
        setTimeout(() => {
            checkBlazorHealth();
        }, 1000);
    }
    
    // Clean up potentially corrupted DOM elements
    function cleanupCorruptedElements() {
        try {
            // Remove any malformed Blazor comment nodes
            const walker = document.createTreeWalker(
                document.body,
                NodeFilter.SHOW_COMMENT,
                {
                    acceptNode: function(node) {
                        if (node.nodeValue && node.nodeValue.includes('Blazor:')) {
                            return NodeFilter.FILTER_ACCEPT;
                        }
                        return NodeFilter.FILTER_REJECT;
                    }
                }
            );
            
            const corruptedNodes = [];
            let node;
            while (node = walker.nextNode()) {
                if (node.nodeValue.includes('malformed') || node.nodeValue.length > 10000) {
                    corruptedNodes.push(node);
                }
            }
            
            // Remove corrupted nodes
            corruptedNodes.forEach(node => {
                try {
                    node.parentNode.removeChild(node);
                    console.log('[BLAZOR ERROR HANDLER] Removed corrupted comment node');
                } catch (e) {
                    console.warn('[BLAZOR ERROR HANDLER] Failed to remove corrupted node:', e);
                }
            });
            
        } catch (e) {
            console.warn('[BLAZOR ERROR HANDLER] Error during cleanup:', e);
        }
    }
    
    // Try to recover component functionality
    function tryComponentRecovery() {
        try {
            // Check if Blazor is still available
            if (window.Blazor) {
                console.log('[BLAZOR ERROR HANDLER] Blazor still available, attempting recovery');
                
                // Try to trigger a gentle re-render
                if (window.Blazor.reconnect) {
                    window.Blazor.reconnect();
                }
            } else {
                console.log('[BLAZOR ERROR HANDLER] Blazor not available, waiting for reload');
            }
        } catch (e) {
            console.warn('[BLAZOR ERROR HANDLER] Recovery attempt failed:', e);
        }
    }
    
    // Check overall Blazor health
    function checkBlazorHealth() {
        try {
            if (window.Blazor) {
                console.log('[BLAZOR ERROR HANDLER] Blazor health check: OK');
                return true;
            } else {
                console.warn('[BLAZOR ERROR HANDLER] Blazor health check: FAILED');
                return false;
            }
        } catch (e) {
            console.error('[BLAZOR ERROR HANDLER] Health check error:', e);
            return false;
        }
    }
    
    // Try general Blazor recovery
    function tryBlazorRecovery() {
        console.log('[BLAZOR ERROR HANDLER] Attempting Blazor recovery');
        
        // Clean up first
        cleanupCorruptedElements();
        
        // Check if we can recover
        setTimeout(() => {
            if (!checkBlazorHealth()) {
                console.warn('[BLAZOR ERROR HANDLER] Recovery failed, may need page reload');
            }
        }, 2000);
    }
    
    // Show error recovery dialog
    function showErrorRecoveryDialog() {
        if (document.getElementById('blazor-error-dialog')) {
            return; // Dialog already shown
        }
        
        const dialog = document.createElement('div');
        dialog.id = 'blazor-error-dialog';
        dialog.innerHTML = `
            <div style="
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background: rgba(0,0,0,0.8);
                z-index: 10000;
                display: flex;
                align-items: center;
                justify-content: center;
                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            ">
                <div style="
                    background: white;
                    padding: 30px;
                    border-radius: 10px;
                    max-width: 500px;
                    text-align: center;
                    box-shadow: 0 4px 20px rgba(0,0,0,0.3);
                ">
                    <h3 style="color: #e74a3b; margin-bottom: 15px;">
                        <i class="fas fa-exclamation-triangle"></i>
                        Application Error
                    </h3>
                    <p style="margin-bottom: 20px; color: #6c757d;">
                        The application has encountered repeated errors and may need to be reloaded.
                    </p>
                    <div style="display: flex; gap: 10px; justify-content: center;">
                        <button onclick="window.location.reload()" style="
                            background: #4e73df;
                            color: white;
                            border: none;
                            padding: 10px 20px;
                            border-radius: 5px;
                            cursor: pointer;
                            font-size: 14px;
                        ">Reload Page</button>
                        <button onclick="document.getElementById('blazor-error-dialog').remove()" style="
                            background: #6c757d;
                            color: white;
                            border: none;
                            padding: 10px 20px;
                            border-radius: 5px;
                            cursor: pointer;
                            font-size: 14px;
                        ">Continue</button>
                    </div>
                </div>
            </div>
        `;
        
        document.body.appendChild(dialog);
        
        // Auto-remove after 30 seconds
        setTimeout(() => {
            if (document.getElementById('blazor-error-dialog')) {
                document.getElementById('blazor-error-dialog').remove();
            }
        }, 30000);
    }
    
    // Initialize when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', function() {
            console.log('[BLAZOR ERROR HANDLER] DOM ready, error handling active');
        });
    } else {
        console.log('[BLAZOR ERROR HANDLER] Error handling active');
    }
    
    // Expose recovery functions globally for debugging
    window.BlazorErrorHandler = {
        tryRecovery: tryBlazorRecovery,
        checkHealth: checkBlazorHealth,
        cleanup: cleanupCorruptedElements,
        getErrorCounts: () => new Map(errorCounts)
    };
    
})();
