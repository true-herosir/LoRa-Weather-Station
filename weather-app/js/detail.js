import { CONFIG } from './config.js';
import { fetchWeatherData } from './api.js';
import { WeatherChart } from './charts.js';

const debounce = (fn, delay) => {
    let timeoutId;
    return (...args) => {
        clearTimeout(timeoutId);
        timeoutId = setTimeout(() => fn(...args), delay);
    };
};

class WeatherDashboard {
    constructor() {
        this.currentLocation = CONFIG.DEFAULT_LOCATION;
        this.charts = {
            humidity: new WeatherChart('chart1', 'Humidity (%)', 'rgba(26, 115, 232, 1)')
        };
        this.setupEventListeners();
        this.startAutoRefresh();
        this.fetchData();
    }

    async fetchData() {
        try {
            this.setLoadingState(true);
            const data = await fetchWeatherData(this.currentLocation);
            this.updateCharts(data.data || data);
            this.updateLastRefreshTime();
        } catch (error) {
            this.handleError(error);
        } finally {
            this.setLoadingState(false);
        }
    }

    updateCharts(data) {
        if (!Array.isArray(data)) {
            throw new Error('Invalid data format');
        }
        this.charts.humidity.update(data, 'humidity');
    }

    setupEventListeners() {
        document.querySelectorAll('.location-button').forEach(button => {
            button.addEventListener('click', () => this.handleLocationChange(button));
        });

        document.querySelector('.refresh-button').addEventListener(
            'click', 
            debounce(() => this.fetchData(), 500)
        );
    }

    handleLocationChange(button) {
        document.querySelectorAll('.location-button')
            .forEach(btn => btn.classList.remove('active'));
        button.classList.add('active');
        this.currentLocation = button.dataset.location;
        document.querySelector('.selected-location').textContent = 
            `Selected Location: ${this.currentLocation}`;
        this.fetchData();
    }

    setLoadingState(isLoading) {
        document.querySelector('.loading-indicator')
            .classList.toggle('hidden', !isLoading);
    }

    updateLastRefreshTime() {
        document.querySelector('.last-update').textContent = 
            `Last updated: ${new Date().toLocaleTimeString()}`;
    }

    handleError(error) {
        console.error('Error:', error);
        document.querySelector('.error-message').textContent = 
            `Error: ${error.message}`;
    }
}