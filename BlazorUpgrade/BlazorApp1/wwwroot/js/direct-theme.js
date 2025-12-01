// This file is kept for backward compatibility
// All theme functionality has been moved to theme.js

// Available themes
const availableThemes = [
    "default",
    "dark",
    "software",
    "software-dark",
    "humanistic",
    "humanistic-dark",
    "standard",
    "standard-dark",
    "material",
    "material-dark"
];

// Theme display names
const themeNames = {
    "default": "Precision Pro",
    "dark": "CalibraSphere",
    "software": "Exactitude",
    "software-dark": "Exactitude Dark",
    "humanistic-dark": "TrueTune Dark",
    "humanistic": "TrueTune",
    "standard": "Alignify",
    "standard-dark": "Alignify Dark",
    "material": "AxisPoint",
    "material-dark": "AxisPoint Dark"
};

// Provide compatibility with existing code
window.directTheme = {
    apply: function(theme) {
        console.log("Redirecting directTheme.apply to applyTheme");
        return window.applyTheme(theme);
    },
    initialize: function() {
        console.log("Redirecting directTheme.initialize to initializeTheme");
        return window.initializeTheme();
    },
    availableThemes: availableThemes,
    themeNames: themeNames
};
