export const CONFIG = {
    // API Configuration
    API_URL: 'http://84.85.32.192:7086/api',
    API_TIMEOUT: 5000,
    API_TABLES: [
        "most_recent",
        "Nodes/node_location",
        "Hours_AVG",
        "Max_Min",
        "Node_location"
    ],

    // Time Ranges (in days)
    TIME_RANGES: {
        SHORT: 3,
        MEDIUM: 14
    },

    // Page Sizes
    PAGE_SIZE: {
        LHT: 350,
        DEFAULT: 900
    },

    // Date Formats
    DATE_FORMAT: {
        WITH_TIME: true,
        WITHOUT_TIME: false
    },

    // Error Messages
    ERRORS: {
        NETWORK: 'Network response was not ok',
        TIMEOUT: 'Request timeout',
        DATE_INVALID: 'End date cannot be before start date',
        FETCH_FAILED: 'Failed to fetch weather data'
    },

    // // Chart Configuration
    // CHART: {
    //     COLORS: ['#1a73e8', '#34a853', '#fbbc04', '#ea4335'],
    //     LINE_TENSION: 0.4,
    //     POINT_RADIUS: 2
    // }
};

export default CONFIG;