// CalibrationSaaS Customer Portal - JavaScript Functions

// Global utilities
window.customerPortal = {
    // Show loading overlay
    showLoading: function() {
        const overlay = document.getElementById('loading-overlay');
        if (overlay) {
            overlay.style.display = 'flex';
        }
    },

    // Hide loading overlay
    hideLoading: function() {
        const overlay = document.getElementById('loading-overlay');
        if (overlay) {
            overlay.style.display = 'none';
        }
    },

    // Focus element by ID
    focusElement: function(elementId) {
        setTimeout(() => {
            const element = document.getElementById(elementId);
            if (element) {
                element.focus();
            }
        }, 100);
    },

    // Get client IP address (best effort)
    getClientIP: function() {
        // This is a placeholder - in production, you'd typically get this from the server
        return 'Unknown';
    },

    // Get user agent
    getUserAgent: function() {
        return navigator.userAgent || 'Unknown';
    },

    // Show toast notification
    showToast: function(title, message, type = 'info', duration = 5000) {
        const toastContainer = document.querySelector('.toast-container');
        if (!toastContainer) return;

        const toastId = 'toast-' + Date.now();
        const iconClass = this.getToastIcon(type);
        const bgClass = this.getToastBgClass(type);

        const toastHtml = `
            <div id="${toastId}" class="toast ${bgClass}" role="alert" aria-live="assertive" aria-atomic="true">
                <div class="toast-header">
                    <i class="${iconClass} me-2"></i>
                    <strong class="me-auto">${title}</strong>
                    <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
                <div class="toast-body">
                    ${message}
                </div>
            </div>
        `;

        toastContainer.insertAdjacentHTML('beforeend', toastHtml);

        const toastElement = document.getElementById(toastId);
        const toast = new bootstrap.Toast(toastElement, {
            autohide: true,
            delay: duration
        });

        toast.show();

        // Remove toast element after it's hidden
        toastElement.addEventListener('hidden.bs.toast', () => {
            toastElement.remove();
        });
    },

    // Get toast icon based on type
    getToastIcon: function(type) {
        switch (type) {
            case 'success': return 'fas fa-check-circle text-success';
            case 'error': return 'fas fa-exclamation-circle text-danger';
            case 'warning': return 'fas fa-exclamation-triangle text-warning';
            case 'info': return 'fas fa-info-circle text-info';
            default: return 'fas fa-info-circle text-info';
        }
    },

    // Get toast background class based on type
    getToastBgClass: function(type) {
        switch (type) {
            case 'success': return 'toast-success';
            case 'error': return 'toast-error';
            case 'warning': return 'toast-warning';
            case 'info': return 'toast-info';
            default: return '';
        }
    },

    // Download file from byte array
    downloadFile: function(fileName, contentType, byteArray) {
        const blob = new Blob([new Uint8Array(byteArray)], { type: contentType });
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = fileName;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
    },

    // Copy text to clipboard
    copyToClipboard: function(text) {
        if (navigator.clipboard && window.isSecureContext) {
            return navigator.clipboard.writeText(text).then(() => {
                this.showToast('Copied', 'Text copied to clipboard', 'success', 2000);
                return true;
            }).catch(() => {
                return this.fallbackCopyToClipboard(text);
            });
        } else {
            return this.fallbackCopyToClipboard(text);
        }
    },

    // Fallback copy to clipboard for older browsers
    fallbackCopyToClipboard: function(text) {
        const textArea = document.createElement('textarea');
        textArea.value = text;
        textArea.style.position = 'fixed';
        textArea.style.left = '-999999px';
        textArea.style.top = '-999999px';
        document.body.appendChild(textArea);
        textArea.focus();
        textArea.select();
        
        try {
            const successful = document.execCommand('copy');
            if (successful) {
                this.showToast('Copied', 'Text copied to clipboard', 'success', 2000);
            } else {
                this.showToast('Error', 'Failed to copy text', 'error', 3000);
            }
            return successful;
        } catch (err) {
            this.showToast('Error', 'Failed to copy text', 'error', 3000);
            return false;
        } finally {
            document.body.removeChild(textArea);
        }
    },

    // Format date for display
    formatDate: function(dateString, format = 'short') {
        if (!dateString) return '';
        
        const date = new Date(dateString);
        if (isNaN(date.getTime())) return '';

        const options = {
            short: { year: 'numeric', month: 'short', day: 'numeric' },
            long: { year: 'numeric', month: 'long', day: 'numeric' },
            datetime: { 
                year: 'numeric', 
                month: 'short', 
                day: 'numeric', 
                hour: '2-digit', 
                minute: '2-digit' 
            }
        };

        return date.toLocaleDateString('en-US', options[format] || options.short);
    },

    // Format number with commas
    formatNumber: function(number, decimals = 0) {
        if (typeof number !== 'number') return '';
        return number.toLocaleString('en-US', {
            minimumFractionDigits: decimals,
            maximumFractionDigits: decimals
        });
    },

    // Debounce function for search inputs
    debounce: function(func, wait, immediate) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                timeout = null;
                if (!immediate) func(...args);
            };
            const callNow = immediate && !timeout;
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
            if (callNow) func(...args);
        };
    },

    // Initialize tooltips
    initializeTooltips: function() {
        const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });
    },

    // Initialize popovers
    initializePopovers: function() {
        const popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
        popoverTriggerList.map(function (popoverTriggerEl) {
            return new bootstrap.Popover(popoverTriggerEl);
        });
    },

    // Validate email format
    isValidEmail: function(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    },

    // Get query parameter value
    getQueryParam: function(name) {
        const urlParams = new URLSearchParams(window.location.search);
        return urlParams.get(name);
    },

    // Set page title
    setPageTitle: function(title) {
        document.title = title + ' - CalibrationSaaS Customer Portal';
    },

    // Print current page
    printPage: function() {
        window.print();
    },

    // Go back in browser history
    goBack: function() {
        if (window.history.length > 1) {
            window.history.back();
        } else {
            window.location.href = '/';
        }
    }
};

// Global function aliases for Blazor interop
window.showLoading = window.customerPortal.showLoading;
window.hideLoading = window.customerPortal.hideLoading;
window.focusElement = window.customerPortal.focusElement;
window.getClientIP = window.customerPortal.getClientIP;
window.getUserAgent = window.customerPortal.getUserAgent;
window.showToast = window.customerPortal.showToast;
window.downloadFile = window.customerPortal.downloadFile;
window.copyToClipboard = window.customerPortal.copyToClipboard;

// Initialize on DOM content loaded
document.addEventListener('DOMContentLoaded', function() {
    // Initialize Bootstrap components
    window.customerPortal.initializeTooltips();
    window.customerPortal.initializePopovers();

    // Handle mobile navigation toggle
    const navToggle = document.querySelector('.navbar-toggler');
    const sidebar = document.querySelector('.sidebar');
    
    if (navToggle && sidebar) {
        navToggle.addEventListener('click', function() {
            sidebar.classList.toggle('show');
        });
    }

    // Close mobile navigation when clicking outside
    document.addEventListener('click', function(event) {
        const sidebar = document.querySelector('.sidebar');
        const navToggle = document.querySelector('.navbar-toggler');
        
        if (sidebar && navToggle && 
            !sidebar.contains(event.target) && 
            !navToggle.contains(event.target) &&
            sidebar.classList.contains('show')) {
            sidebar.classList.remove('show');
        }
    });

    // Handle form validation styling
    const forms = document.querySelectorAll('.needs-validation');
    forms.forEach(function(form) {
        form.addEventListener('submit', function(event) {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        });
    });
});

// Handle Blazor reconnection
window.addEventListener('beforeunload', function() {
    window.customerPortal.showLoading();
});

// Blazor error handling
window.addEventListener('error', function(event) {
    console.error('Global error:', event.error);
    window.customerPortal.showToast(
        'Error', 
        'An unexpected error occurred. Please refresh the page if the problem persists.', 
        'error', 
        10000
    );
});

// Service worker registration (if available)
if ('serviceWorker' in navigator) {
    window.addEventListener('load', function() {
        navigator.serviceWorker.register('/service-worker.js')
            .then(function(registration) {
                console.log('ServiceWorker registration successful');
            })
            .catch(function(err) {
                console.log('ServiceWorker registration failed');
            });
    });
}
