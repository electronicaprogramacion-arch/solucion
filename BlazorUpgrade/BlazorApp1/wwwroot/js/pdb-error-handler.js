// This script handles PDB loading errors and other framework errors
window.addEventListener('error', function (e) {
    // Check if the error is related to PDB files or framework errors
    if (e && e.filename && (e.filename.endsWith('.pdb') || e.filename.includes('/_framework/'))) {
        // Prevent the error from propagating
        e.preventDefault();
        e.stopPropagation();
        console.warn('Error suppressed:', e.filename);
        return true;
    }
    return false;
}, true);

// Override fetch to handle problematic requests
const originalFetch = window.fetch;
window.fetch = function (resource, init) {
    if (typeof resource === 'string') {
        // Handle PDB files
        if (resource.endsWith('.pdb')) {
            console.warn('PDB fetch request suppressed:', resource);
            return Promise.resolve(new Response(null, { status: 404, statusText: 'Not Found' }));
        }

        // Handle other problematic resources
        if (resource.includes('fj7ijvg861.pdb') || resource.includes('.pdb')) {
            console.warn('Problematic resource fetch suppressed:', resource);
            return Promise.resolve(new Response(null, { status: 404, statusText: 'Not Found' }));
        }
    }

    // Continue with the original fetch for other resources
    return originalFetch.apply(this, arguments)
        .catch(error => {
            console.warn('Fetch error caught and suppressed:', error);
            return Promise.resolve(new Response(null, { status: 404, statusText: 'Not Found' }));
        });
};

// Add getDimensions function if it doesn't exist
if (typeof window.getDimensions !== 'function') {
    window.getDimensions = function() {
        return {
            width: window.innerWidth,
            height: window.innerHeight,
            online: navigator.onLine,
            scroll: window.scrollY || document.documentElement.scrollTop,
            install: false
        };
    };
}

// Add setLog function if it doesn't exist
if (typeof window.setLog !== 'function') {
    window.setLog = function() {
        console.log('Log enabled');
    };
}

// Suppress unhandled promise rejections
window.addEventListener('unhandledrejection', function(event) {
    console.warn('Unhandled promise rejection suppressed:', event.reason);
    event.preventDefault();
    event.stopPropagation();
    return true;
}, true);
