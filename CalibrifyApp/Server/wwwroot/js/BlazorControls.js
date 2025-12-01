window.BlazorControls = {
    assemblyname: 'Blazor.Controls',
    setFocus: (element) => {
        if (!element)
            return;

        element.focus();
    },
    preventEnter: (element, disabled) => {
        if (!element) {
            console.log("Error: preventEnter() element not found");
            return;
        }

        if (disabled)
            element.addEventListener('keydown', BlazorControls.preventEnterHandler);
        else
            element.removeEventListener('keydown', BlazorControls.preventEnterHandler);
    },
    preventEnterHandler: (event) => {
        const key = event.key;

        if (key === "Enter") {
            event.preventDefault();
        }
    },
    showAppLoading: () => {
        const loadingIndicator = document.getElementById('app-loading-indicator');
        if (loadingIndicator) {
            loadingIndicator.style.display = 'block';
        }
    },
    hideAppLoading: () => {
        const loadingIndicator = document.getElementById('app-loading-indicator');
        if (loadingIndicator) {
            loadingIndicator.style.display = 'none';
        }
    }
};





