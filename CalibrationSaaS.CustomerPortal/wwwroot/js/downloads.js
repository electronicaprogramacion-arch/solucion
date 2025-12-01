// Download functionality for Customer Portal

/**
 * Downloads a file from base64 data
 * @param {string} fileName - The name of the file to download
 * @param {string} contentType - The MIME type of the file
 * @param {string} base64Data - The base64 encoded file data
 */
window.downloadFile = function (fileName, contentType, base64Data) {
    try {
        // Convert base64 to blob
        const byteCharacters = atob(base64Data);
        const byteNumbers = new Array(byteCharacters.length);
        
        for (let i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        
        const byteArray = new Uint8Array(byteNumbers);
        const blob = new Blob([byteArray], { type: contentType });
        
        // Create download link
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = fileName;
        
        // Trigger download
        document.body.appendChild(link);
        link.click();
        
        // Cleanup
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
        
        return true;
    } catch (error) {
        console.error('Error downloading file:', error);
        return false;
    }
};

/**
 * Downloads multiple files as a ZIP archive
 * @param {string} zipFileName - The name of the ZIP file
 * @param {string} base64Data - The base64 encoded ZIP data
 */
window.downloadZipFile = function (zipFileName, base64Data) {
    return window.downloadFile(zipFileName, 'application/zip', base64Data);
};

/**
 * Opens a file in a new tab/window
 * @param {string} base64Data - The base64 encoded file data
 * @param {string} contentType - The MIME type of the file
 */
window.openFileInNewTab = function (base64Data, contentType) {
    try {
        const byteCharacters = atob(base64Data);
        const byteNumbers = new Array(byteCharacters.length);
        
        for (let i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        
        const byteArray = new Uint8Array(byteNumbers);
        const blob = new Blob([byteArray], { type: contentType });
        
        const url = window.URL.createObjectURL(blob);
        window.open(url, '_blank');
        
        // Cleanup after a delay to allow the browser to load the file
        setTimeout(() => {
            window.URL.revokeObjectURL(url);
        }, 1000);
        
        return true;
    } catch (error) {
        console.error('Error opening file:', error);
        return false;
    }
};

/**
 * Shows a download progress indicator
 * @param {string} elementId - The ID of the element to show progress in
 * @param {number} progress - Progress percentage (0-100)
 */
window.showDownloadProgress = function (elementId, progress) {
    const element = document.getElementById(elementId);
    if (element) {
        element.style.display = 'block';
        const progressBar = element.querySelector('.progress-bar');
        if (progressBar) {
            progressBar.style.width = progress + '%';
            progressBar.setAttribute('aria-valuenow', progress);
        }
        
        const progressText = element.querySelector('.progress-text');
        if (progressText) {
            progressText.textContent = Math.round(progress) + '%';
        }
    }
};

/**
 * Hides the download progress indicator
 * @param {string} elementId - The ID of the element to hide
 */
window.hideDownloadProgress = function (elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        element.style.display = 'none';
    }
};

/**
 * Shows a download notification
 * @param {string} message - The notification message
 * @param {string} type - The notification type (success, error, info, warning)
 */
window.showDownloadNotification = function (message, type = 'info') {
    // Create notification element
    const notification = document.createElement('div');
    notification.className = `alert alert-${type} download-notification`;
    notification.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        z-index: 9999;
        min-width: 300px;
        max-width: 500px;
        padding: 15px;
        border-radius: 4px;
        box-shadow: 0 4px 12px rgba(0,0,0,0.15);
        animation: slideInRight 0.3s ease-out;
    `;
    
    // Add icon based on type
    let icon = '';
    switch (type) {
        case 'success':
            icon = '✓';
            notification.style.backgroundColor = '#d4edda';
            notification.style.borderColor = '#c3e6cb';
            notification.style.color = '#155724';
            break;
        case 'error':
            icon = '✗';
            notification.style.backgroundColor = '#f8d7da';
            notification.style.borderColor = '#f5c6cb';
            notification.style.color = '#721c24';
            break;
        case 'warning':
            icon = '⚠';
            notification.style.backgroundColor = '#fff3cd';
            notification.style.borderColor = '#ffeaa7';
            notification.style.color = '#856404';
            break;
        default:
            icon = 'ℹ';
            notification.style.backgroundColor = '#d1ecf1';
            notification.style.borderColor = '#bee5eb';
            notification.style.color = '#0c5460';
    }
    
    notification.innerHTML = `
        <div style="display: flex; align-items: center; gap: 10px;">
            <span style="font-size: 18px; font-weight: bold;">${icon}</span>
            <span>${message}</span>
            <button onclick="this.parentElement.parentElement.remove()" 
                    style="margin-left: auto; background: none; border: none; font-size: 18px; cursor: pointer; opacity: 0.7;">
                ×
            </button>
        </div>
    `;
    
    // Add CSS animation
    if (!document.querySelector('#download-notification-styles')) {
        const style = document.createElement('style');
        style.id = 'download-notification-styles';
        style.textContent = `
            @keyframes slideInRight {
                from {
                    transform: translateX(100%);
                    opacity: 0;
                }
                to {
                    transform: translateX(0);
                    opacity: 1;
                }
            }
            
            @keyframes slideOutRight {
                from {
                    transform: translateX(0);
                    opacity: 1;
                }
                to {
                    transform: translateX(100%);
                    opacity: 0;
                }
            }
            
            .download-notification {
                border: 1px solid;
            }
        `;
        document.head.appendChild(style);
    }
    
    // Add to page
    document.body.appendChild(notification);
    
    // Auto-remove after 5 seconds
    setTimeout(() => {
        notification.style.animation = 'slideOutRight 0.3s ease-in';
        setTimeout(() => {
            if (notification.parentElement) {
                notification.remove();
            }
        }, 300);
    }, 5000);
};

/**
 * Tracks download analytics
 * @param {string} certificateId - The certificate ID
 * @param {string} fileName - The downloaded file name
 * @param {string} downloadType - The type of download (single, bulk, etc.)
 */
window.trackDownload = function (certificateId, fileName, downloadType = 'single') {
    try {
        // Send analytics data to server
        fetch('/api/analytics/download', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                certificateId: certificateId,
                fileName: fileName,
                downloadType: downloadType,
                timestamp: new Date().toISOString(),
                userAgent: navigator.userAgent
            })
        }).catch(error => {
            console.warn('Failed to track download:', error);
        });
    } catch (error) {
        console.warn('Error tracking download:', error);
    }
};

/**
 * Validates file size before download
 * @param {number} fileSizeBytes - The file size in bytes
 * @param {number} maxSizeBytes - The maximum allowed size in bytes (default: 50MB)
 * @returns {boolean} True if file size is acceptable
 */
window.validateFileSize = function (fileSizeBytes, maxSizeBytes = 50 * 1024 * 1024) {
    if (fileSizeBytes > maxSizeBytes) {
        const maxSizeMB = Math.round(maxSizeBytes / (1024 * 1024));
        const fileSizeMB = Math.round(fileSizeBytes / (1024 * 1024));
        
        window.showDownloadNotification(
            `File size (${fileSizeMB}MB) exceeds maximum allowed size (${maxSizeMB}MB). Please contact support for assistance.`,
            'warning'
        );
        return false;
    }
    return true;
};

/**
 * Formats file size for display
 * @param {number} bytes - The file size in bytes
 * @returns {string} Formatted file size string
 */
window.formatFileSize = function (bytes) {
    if (bytes === 0) return '0 Bytes';
    
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
};
