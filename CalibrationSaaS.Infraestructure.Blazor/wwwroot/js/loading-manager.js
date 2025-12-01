// Professional Loading Manager for Blazor WebAssembly
window.CalibrationLoadingManager = (() => {
    let loadingElement = null;
    let currentPhase = 'initializing';
    let isVisible = true;
    let hideTimeout = null;
    let messageQueue = [];
    let isProcessingQueue = false;

    // Loading phases with default messages
    const phases = {
        initializing: 'Inicializando aplicación',
        authentication: 'Verificando autenticación',
        loading_data: 'Cargando datos del sistema',
        preparing_ui: 'Preparando interfaz de usuario',
        connecting: 'Estableciendo conexión',
        offline_mode: 'Modo sin conexión activado',
        finalizing: 'Finalizando carga'
    };

    // Initialize loading indicator
    function init() {
        loadingElement = document.getElementById('app-loading-indicator');
        if (!loadingElement) {
            createLoadingElement();
        }
        
        console.log('CalibrationLoadingManager initialized');
        return true;
    }

    // Create loading element if it doesn't exist
    function createLoadingElement() {
        const loadingHTML = `
            <div id="app-loading-indicator">
                <div class="loading-brand">
                    <img src="favicon.png" alt="Calibration System" class="loading-logo" />
                    <div class="loading-title">Sistema de Calibración</div>
                    <div class="loading-subtitle">Phoenix Calibration Management</div>
                </div>
                
                <div class="loading-spinner-container">
                    <div class="loading-spinner"></div>
                    <div class="loading-spinner-inner"></div>
                </div>
                
                <div class="loading-progress-container">
                    <div class="loading-progress-bar">
                        <div class="loading-progress-fill"></div>
                    </div>
                </div>
                
                <div class="loading-message" id="loading-message">Inicializando aplicación</div>
                <div class="loading-phase" id="loading-phase">
                    <span class="loading-dots">Cargando</span>
                </div>
            </div>
        `;
        
        document.body.insertAdjacentHTML('afterbegin', loadingHTML);
        loadingElement = document.getElementById('app-loading-indicator');
    }

    // Show loading indicator
    function show(message = null, phase = 'initializing') {
        try {
            if (!loadingElement) {
                init();
            }

            clearTimeout(hideTimeout);
            
            loadingElement.classList.remove('loaded', 'hidden');
            loadingElement.style.display = 'flex';
            isVisible = true;
            
            if (message || phase !== currentPhase) {
                updateMessage(message || phases[phase] || phases.initializing, phase);
            }
            
            console.log(`Loading indicator shown - Phase: ${phase}`);
            return true;
        } catch (error) {
            console.error('Error showing loading indicator:', error);
            return false;
        }
    }

    // Hide loading indicator with smooth transition
    function hide(delay = 800) {
        try {
            if (!loadingElement || !isVisible) {
                return true;
            }

            clearTimeout(hideTimeout);
            
            hideTimeout = setTimeout(() => {
                if (loadingElement) {
                    loadingElement.classList.add('loaded');
                    isVisible = false;
                    
                    // Completely hide after transition
                    setTimeout(() => {
                        if (loadingElement && !isVisible) {
                            loadingElement.classList.add('hidden');
                            loadingElement.style.display = 'none';
                        }
                    }, 600);
                }
                console.log('Loading indicator hidden');
            }, delay);
            
            return true;
        } catch (error) {
            console.error('Error hiding loading indicator:', error);
            return false;
        }
    }

    // Update loading message and phase
    function updateMessage(message, phase = null) {
        try {
            if (!loadingElement) {
                return false;
            }

            const messageElement = document.getElementById('loading-message');
            const phaseElement = document.getElementById('loading-phase');
            
            if (messageElement && message) {
                // Add to queue for smooth transitions
                messageQueue.push({ message, phase });
                processMessageQueue();
            }
            
            if (phase) {
                currentPhase = phase;
            }
            
            return true;
        } catch (error) {
            console.error('Error updating loading message:', error);
            return false;
        }
    }

    // Process message queue for smooth transitions
    async function processMessageQueue() {
        if (isProcessingQueue || messageQueue.length === 0) {
            return;
        }

        isProcessingQueue = true;
        
        while (messageQueue.length > 0) {
            const { message, phase } = messageQueue.shift();
            
            const messageElement = document.getElementById('loading-message');
            const phaseElement = document.getElementById('loading-phase');
            
            if (messageElement) {
                // Fade out
                messageElement.style.opacity = '0';
                
                await new Promise(resolve => setTimeout(resolve, 150));
                
                // Update content
                messageElement.textContent = message;
                
                // Fade in
                messageElement.style.opacity = '1';
                
                if (phaseElement && phase) {
                    phaseElement.innerHTML = `<span class="loading-dots">${phase}</span>`;
                }
                
                await new Promise(resolve => setTimeout(resolve, 300));
            }
        }
        
        isProcessingQueue = false;
    }

    // Set error state
    function setError(errorMessage = 'Error al cargar la aplicación') {
        try {
            if (!loadingElement) {
                return false;
            }

            loadingElement.classList.add('loading-error');
            updateMessage(errorMessage, 'error');
            
            // Auto-hide after 5 seconds on error
            setTimeout(() => hide(0), 5000);
            
            return true;
        } catch (error) {
            console.error('Error setting error state:', error);
            return false;
        }
    }

    // Get current state
    function getState() {
        return {
            isVisible,
            currentPhase,
            hasElement: !!loadingElement
        };
    }

    // Force hide (emergency)
    function forceHide() {
        try {
            if (loadingElement) {
                clearTimeout(hideTimeout);
                loadingElement.style.display = 'none';
                loadingElement.classList.add('loaded', 'hidden');
                isVisible = false;
            }
            return true;
        } catch (error) {
            console.error('Error force hiding loading indicator:', error);
            return false;
        }
    }

    // Auto-initialize when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

    // Fallback timeout to prevent infinite loading
    setTimeout(() => {
        if (isVisible) {
            console.warn('Loading indicator timeout reached, force hiding');
            forceHide();
        }
    }, 30000); // 30 seconds timeout

    // Public API
    return {
        show,
        hide,
        updateMessage,
        setError,
        getState,
        forceHide,
        phases
    };
})();

// Blazor integration events
document.addEventListener('DOMContentLoaded', () => {
    console.log('DOM loaded, loading indicator ready');
});

// Listen for Blazor events
if (window.Blazor) {
    Blazor.addEventListener('enhancedload', () => {
        console.log('Blazor enhanced load complete');
        window.CalibrationLoadingManager.updateMessage('Aplicación lista', 'finalizing');
        setTimeout(() => window.CalibrationLoadingManager.hide(), 1000);
    });
}