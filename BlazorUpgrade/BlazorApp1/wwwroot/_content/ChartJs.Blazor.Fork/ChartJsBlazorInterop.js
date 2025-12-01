// ChartJs.Blazor.Fork Interop
// JavaScript interop for Chart.js integration with Blazor

window.ChartJsBlazor = window.ChartJsBlazor || {};

// Chart management
window.ChartJsBlazor.charts = new Map();

// Initialize Chart.js Blazor interop
window.ChartJsBlazor.init = function() {
    console.log('[CHARTJS] Chart.js Blazor interop initialized');
    
    // Check if Chart.js is available
    if (typeof Chart === 'undefined') {
        console.warn('[CHARTJS] Chart.js library not found. Charts will not work.');
        return false;
    }
    
    console.log('[CHARTJS] Chart.js version:', Chart.version);
    return true;
};

// Create a new chart
window.ChartJsBlazor.createChart = function(canvasId, config) {
    try {
        const canvas = document.getElementById(canvasId);
        if (!canvas) {
            console.error('[CHARTJS] Canvas element not found:', canvasId);
            return false;
        }
        
        // Destroy existing chart if it exists
        if (window.ChartJsBlazor.charts.has(canvasId)) {
            window.ChartJsBlazor.destroyChart(canvasId);
        }
        
        const chart = new Chart(canvas, config);
        window.ChartJsBlazor.charts.set(canvasId, chart);
        
        console.log('[CHARTJS] Chart created:', canvasId);
        return true;
    } catch (error) {
        console.error('[CHARTJS] Error creating chart:', error.message);
        return false;
    }
};

// Update chart data
window.ChartJsBlazor.updateChart = function(canvasId, config) {
    try {
        const chart = window.ChartJsBlazor.charts.get(canvasId);
        if (!chart) {
            console.warn('[CHARTJS] Chart not found for update:', canvasId);
            return window.ChartJsBlazor.createChart(canvasId, config);
        }
        
        // Update chart configuration
        Object.assign(chart.config, config);
        chart.update();
        
        console.log('[CHARTJS] Chart updated:', canvasId);
        return true;
    } catch (error) {
        console.error('[CHARTJS] Error updating chart:', error.message);
        return false;
    }
};

// Destroy a chart
window.ChartJsBlazor.destroyChart = function(canvasId) {
    try {
        const chart = window.ChartJsBlazor.charts.get(canvasId);
        if (chart) {
            chart.destroy();
            window.ChartJsBlazor.charts.delete(canvasId);
            console.log('[CHARTJS] Chart destroyed:', canvasId);
            return true;
        }
        return false;
    } catch (error) {
        console.error('[CHARTJS] Error destroying chart:', error.message);
        return false;
    }
};

// Resize chart
window.ChartJsBlazor.resizeChart = function(canvasId) {
    try {
        const chart = window.ChartJsBlazor.charts.get(canvasId);
        if (chart) {
            chart.resize();
            console.log('[CHARTJS] Chart resized:', canvasId);
            return true;
        }
        return false;
    } catch (error) {
        console.error('[CHARTJS] Error resizing chart:', error.message);
        return false;
    }
};

// Get chart data as base64 image
window.ChartJsBlazor.getChartImage = function(canvasId) {
    try {
        const chart = window.ChartJsBlazor.charts.get(canvasId);
        if (chart) {
            return chart.toBase64Image();
        }
        return null;
    } catch (error) {
        console.error('[CHARTJS] Error getting chart image:', error.message);
        return null;
    }
};

// Cleanup all charts
window.ChartJsBlazor.cleanup = function() {
    try {
        window.ChartJsBlazor.charts.forEach((chart, canvasId) => {
            chart.destroy();
        });
        window.ChartJsBlazor.charts.clear();
        console.log('[CHARTJS] All charts cleaned up');
        return true;
    } catch (error) {
        console.error('[CHARTJS] Error during cleanup:', error.message);
        return false;
    }
};

// Error handling wrapper
window.ChartJsBlazor.safeExecute = function(operation, fallback) {
    try {
        return operation();
    } catch (error) {
        console.warn('[CHARTJS] Operation failed:', error.message);
        return fallback ? fallback() : null;
    }
};

// Initialize when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', function() {
        window.ChartJsBlazor.init();
    });
} else {
    window.ChartJsBlazor.init();
}

// Cleanup on page unload
window.addEventListener('beforeunload', function() {
    window.ChartJsBlazor.cleanup();
});

console.log('[CHARTJS] ChartJs.Blazor.Fork interop loaded');
