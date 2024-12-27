export const CONFIG = {
    API_BASE_URL: 'http://84.85.32.192:7086/api',
    REFRESH_INTERVAL: 300000,
    DEFAULT_LOCATION: 'Gronau',
    CACHE_DURATION: 60000
};

export const CHART_CONFIG = {
    responsive: true,
    interaction: { intersect: false, mode: 'index' },
    plugins: {
        tooltip: {
            enabled: true,
            callbacks: {
                label: (context) => `${context.dataset.label}: ${context.parsed.y.toFixed(1)}`
            }
        }
    },
    scales: {
        x: {
            type: 'time',
            time: { unit: 'hour', displayFormats: { hour: 'HH:mm' } },
            ticks: { maxRotation: 45 }
        },
        y: { beginAtZero: true }
    }
};