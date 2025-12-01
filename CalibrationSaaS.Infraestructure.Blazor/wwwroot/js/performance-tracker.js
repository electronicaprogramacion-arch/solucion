// CalibrationSaaS Performance Tracking System
// This script provides comprehensive performance monitoring for the Blazor application

window.PerformanceTracker = {
    // Store performance data
    performanceData: [],
    pageLoadStart: null,
    navigationStart: null,
    
    // Initialize performance tracking
    init: function() {
//        console.log('🚀 Performance Tracker initialized');
        this.navigationStart = performance.now();
        this.pageLoadStart = Date.now();
        
        // Track page load performance
        this.trackPageLoad();
        
        // Track navigation performance
        this.trackNavigation();
        
        // Track resource loading
        this.trackResourceLoading();
        
        // Track user interactions
        this.trackUserInteractions();
        
        // Set up periodic reporting
        this.setupPeriodicReporting();
    },
    
    // Track page load performance
    trackPageLoad: function() {
        window.addEventListener('load', () => {
            const loadTime = performance.now() - this.navigationStart;
            this.logPerformance('PAGE_LOAD', 'Full page load', loadTime);
            
            // Track specific performance metrics
            if (performance.getEntriesByType) {
                const navigation = performance.getEntriesByType('navigation')[0];
                if (navigation) {
                    this.logPerformance('DNS_LOOKUP', 'DNS lookup time', navigation.domainLookupEnd - navigation.domainLookupStart);
                    this.logPerformance('TCP_CONNECT', 'TCP connection time', navigation.connectEnd - navigation.connectStart);
                    this.logPerformance('SERVER_RESPONSE', 'Server response time', navigation.responseEnd - navigation.requestStart);
                    this.logPerformance('DOM_PROCESSING', 'DOM processing time', navigation.domComplete - navigation.responseEnd);
                }
            }
        });
    },
    
    // Track Blazor navigation performance
    trackNavigation: function() {
        // Override Blazor navigation to track page transitions
        const originalPushState = history.pushState;
        const originalReplaceState = history.replaceState;
        
        history.pushState = function(...args) {
            window.PerformanceTracker.startPageTransition('NAVIGATION_PUSH', args[2]);
            return originalPushState.apply(this, args);
        };
        
        history.replaceState = function(...args) {
            window.PerformanceTracker.startPageTransition('NAVIGATION_REPLACE', args[2]);
            return originalReplaceState.apply(this, args);
        };
        
        // Track popstate events (back/forward)
        window.addEventListener('popstate', (event) => {
            this.startPageTransition('NAVIGATION_POPSTATE', window.location.pathname);
        });
    },
    
    // Track resource loading performance
    trackResourceLoading: function() {
        // Monitor fetch requests
        const originalFetch = window.fetch;
        window.fetch = function(...args) {
            const startTime = performance.now();
            const url = args[0];
            
            return originalFetch.apply(this, args).then(response => {
                const endTime = performance.now();
                const duration = endTime - startTime;
                
                window.PerformanceTracker.logPerformance('FETCH_REQUEST', `Fetch: ${url}`, duration, {
                    url: url,
                    status: response.status,
                    ok: response.ok
                });
                
                return response;
            }).catch(error => {
                const endTime = performance.now();
                const duration = endTime - startTime;
                
                window.PerformanceTracker.logPerformance('FETCH_ERROR', `Fetch Error: ${url}`, duration, {
                    url: url,
                    error: error.message
                });
                
                throw error;
            });
        };
    },
    
    // Track user interactions
    trackUserInteractions: function() {
        // Track button clicks
        document.addEventListener('click', (event) => {
            if (event.target.tagName === 'BUTTON' || event.target.closest('button')) {
                const button = event.target.tagName === 'BUTTON' ? event.target : event.target.closest('button');
                const buttonText = button.textContent.trim().substring(0, 50);
                this.startUserAction('BUTTON_CLICK', `Button: ${buttonText}`);
            }
        });
        
        // Track form submissions
        document.addEventListener('submit', (event) => {
            const form = event.target;
            const formId = form.id || form.className || 'unknown';
            this.startUserAction('FORM_SUBMIT', `Form: ${formId}`);
        });
        
        // Track input changes (debounced)
        let inputTimeout;
        document.addEventListener('input', (event) => {
            if (event.target.tagName === 'INPUT' || event.target.tagName === 'SELECT' || event.target.tagName === 'TEXTAREA') {
                clearTimeout(inputTimeout);
                inputTimeout = setTimeout(() => {
                    const inputType = event.target.type || event.target.tagName.toLowerCase();
                    this.logPerformance('USER_INPUT', `Input: ${inputType}`, 0);
                }, 500);
            }
        });
    },
    
    // Start tracking a page transition
    startPageTransition: function(type, url) {
        this.currentTransition = {
            type: type,
            url: url,
            startTime: performance.now()
        };
        
        // Set a timeout to catch if the transition takes too long
        setTimeout(() => {
            if (this.currentTransition && this.currentTransition.url === url) {
                this.logPerformance('PAGE_TRANSITION_TIMEOUT', `Slow transition to: ${url}`, performance.now() - this.currentTransition.startTime);
                this.currentTransition = null;
            }
        }, 10000); // 10 second timeout
    },
    
    // End tracking a page transition
    endPageTransition: function() {
        if (this.currentTransition) {
            const duration = performance.now() - this.currentTransition.startTime;
            this.logPerformance('PAGE_TRANSITION', `${this.currentTransition.type}: ${this.currentTransition.url}`, duration);
            this.currentTransition = null;
        }
    },
    
    // Start tracking a user action
    startUserAction: function(type, description) {
        this.currentAction = {
            type: type,
            description: description,
            startTime: performance.now()
        };
        
        // Set a timeout to catch if the action takes too long
        setTimeout(() => {
            if (this.currentAction && this.currentAction.description === description) {
                this.logPerformance('USER_ACTION_TIMEOUT', `Slow action: ${description}`, performance.now() - this.currentAction.startTime);
                this.currentAction = null;
            }
        }, 5000); // 5 second timeout
    },
    
    // End tracking a user action
    endUserAction: function() {
        if (this.currentAction) {
            const duration = performance.now() - this.currentAction.startTime;
            this.logPerformance('USER_ACTION', `${this.currentAction.type}: ${this.currentAction.description}`, duration);
            this.currentAction = null;
        }
    },
    
    // Log performance data
    logPerformance: function(type, description, duration, metadata = {}) {
        const entry = {
            timestamp: Date.now(),
            type: type,
            description: description,
            duration: Math.round(duration * 100) / 100, // Round to 2 decimal places
            url: window.location.pathname,
            userAgent: navigator.userAgent,
            metadata: metadata
        };
        
        this.performanceData.push(entry);
        
        // Color-coded console logging
        const color = this.getLogColor(duration);
//        console.log(`%c⏱️ ${type}: ${description} - ${entry.duration}ms`, `color: ${color}; font-weight: bold;`, entry);
        
        // Keep only last 100 entries to prevent memory issues
        if (this.performanceData.length > 100) {
            this.performanceData = this.performanceData.slice(-100);
        }
        
        // Send to server if duration is concerning
        if (duration > 2000) { // More than 2 seconds
            this.sendPerformanceAlert(entry);
        }
    },
    
    // Get color for log based on duration
    getLogColor: function(duration) {
        if (duration < 100) return '#28a745'; // Green - Fast
        if (duration < 500) return '#ffc107'; // Yellow - Moderate
        if (duration < 1000) return '#fd7e14'; // Orange - Slow
        return '#dc3545'; // Red - Very slow
    },
    
    // Send performance alert to server
    sendPerformanceAlert: function(entry) {
        try {
            fetch('/api/performance/alert', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(entry)
            }).catch(error => {
//                console.warn('Failed to send performance alert:', error);
            });
        } catch (error) {
//            console.warn('Failed to send performance alert:', error);
        }
    },
    
    // Set up periodic reporting
    setupPeriodicReporting: function() {
        // Report performance data every 30 seconds
        setInterval(() => {
            this.generatePerformanceReport();
        }, 30000);
        
        // Report on page unload
        window.addEventListener('beforeunload', () => {
            this.generatePerformanceReport();
        });
    },
    
    // Generate performance report
    generatePerformanceReport: function() {
        if (this.performanceData.length === 0) return;
        
        const report = {
            sessionId: this.getSessionId(),
            url: window.location.pathname,
            timestamp: Date.now(),
            entries: [...this.performanceData],
            summary: this.generateSummary()
        };
        
//        console.group('📊 Performance Report');
//        console.table(report.summary);
//        console.log('Full data:', report);
//        console.groupEnd();
        
        // Clear reported data
        this.performanceData = [];
        
        return report;
    },
    
    // Generate performance summary
    generateSummary: function() {
        const summary = {};
        
        this.performanceData.forEach(entry => {
            if (!summary[entry.type]) {
                summary[entry.type] = {
                    count: 0,
                    totalDuration: 0,
                    avgDuration: 0,
                    maxDuration: 0,
                    minDuration: Infinity
                };
            }
            
            const s = summary[entry.type];
            s.count++;
            s.totalDuration += entry.duration;
            s.maxDuration = Math.max(s.maxDuration, entry.duration);
            s.minDuration = Math.min(s.minDuration, entry.duration);
            s.avgDuration = s.totalDuration / s.count;
        });
        
        // Round averages
        Object.keys(summary).forEach(key => {
            summary[key].avgDuration = Math.round(summary[key].avgDuration * 100) / 100;
            if (summary[key].minDuration === Infinity) summary[key].minDuration = 0;
        });
        
        return summary;
    },
    
    // Get or create session ID
    getSessionId: function() {
        let sessionId = sessionStorage.getItem('performanceSessionId');
        if (!sessionId) {
            sessionId = 'session_' + Date.now() + '_' + Math.random().toString(36).substr(2, 9);
            sessionStorage.setItem('performanceSessionId', sessionId);
        }
        return sessionId;
    },
    
    // Manual tracking methods for Blazor components
    startTracking: function(name) {
        const trackingId = 'track_' + Date.now() + '_' + Math.random().toString(36).substr(2, 9);
        window[trackingId] = performance.now();
        return trackingId;
    },
    
    endTracking: function(trackingId, description) {
        if (window[trackingId]) {
            const duration = performance.now() - window[trackingId];
            this.logPerformance('MANUAL_TRACKING', description, duration);
            delete window[trackingId];
            return duration;
        }
        return 0;
    }
};

// Auto-initialize when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => window.PerformanceTracker.init());
} else {
    window.PerformanceTracker.init();
}

// Blazor integration - track when Blazor finishes rendering
window.addEventListener('blazor:enhanced:load', () => {
    window.PerformanceTracker.endPageTransition();
    window.PerformanceTracker.endUserAction();
});

// Export for use in Blazor components
window.trackPerformance = function(type, description, duration) {
    window.PerformanceTracker.logPerformance(type, description, duration);
};

window.startPerformanceTracking = function(name) {
    return window.PerformanceTracker.startTracking(name);
};

window.endPerformanceTracking = function(trackingId, description) {
    return window.PerformanceTracker.endTracking(trackingId, description);
};
