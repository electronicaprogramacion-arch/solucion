// ChartJsInterop.js - Bridge between Chart.js and Blazor

window.ChartJsInterop = {
    setupChart: function (canvas, config) {
        // This function will be called from Blazor to set up a chart
        if (!canvas || !canvas.getContext) {
            console.error("Canvas element not found or invalid");
            return null;
        }

        // Convert the config if it's a string (JSON serialized object)
        if (typeof config === "string") {
            config = JSON.parse(config);
        }

        // Create and return the chart instance
        try {
            return new Chart(canvas.getContext('2d'), config);
        } catch (error) {
            console.error("Error creating chart:", error);
            return null;
        }
    },
    
    updateChart: function (chart, config) {
        // Update an existing chart with new configuration
        if (!chart) {
            console.error("Chart instance not found");
            return;
        }

        // Convert the config if it's a string
        if (typeof config === "string") {
            config = JSON.parse(config);
        }

        // Update the chart data and options
        try {
            chart.data = config.data || chart.data;
            chart.options = config.options || chart.options;
            chart.update();
        } catch (error) {
            console.error("Error updating chart:", error);
        }
    },

    destroyChart: function (chart) {
        // Clean up chart resources
        if (chart) {
            try {
                chart.destroy();
            } catch (error) {
                console.error("Error destroying chart:", error);
            }
        }
    }
};