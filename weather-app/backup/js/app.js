const API_URL = 'http://84.85.32.192:7086/api/Most_Recent?page=1&page_size=10';
const REFRESH_INTERVAL = 60000;
let currentLocation = null;
let currentData = null;

async function fetchData() {
    try {
        const response = await fetch(API_URL);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error('Error fetching data:', error);
        throw error;
    }
}

function getUniqueLocations(data) {
    return [...new Set(data.data.map(item => item.location))];
}

function formatValue(value, unit) {
    if (value === null || value === undefined) return 'N/A';
    return `${parseFloat(value).toFixed(1)}${unit}`;
}

function getTimeAgo(date) {
    const seconds = Math.floor((new Date() - new Date(date)) / 1000);
    
    let interval = seconds / 31536000;
    if (interval > 1) return Math.floor(interval) + ' years ago';
    
    interval = seconds / 2592000;
    if (interval > 1) return Math.floor(interval) + ' months ago';
    
    interval = seconds / 86400;
    if (interval > 1) return Math.floor(interval) + ' days ago';
    
    interval = seconds / 3600;
    if (interval > 1) return Math.floor(interval) + ' hours ago';
    
    interval = seconds / 60;
    if (interval > 1) return Math.floor(interval) + ' minutes ago';
    
    return Math.floor(seconds) + ' seconds ago';
}

function createLocationButtons(locations) {
    const container = document.getElementById('locationButtons');
    container.innerHTML = `
        <button class="location-button ${!currentLocation ? 'active' : ''}" 
                onclick="selectLocation(null)">
            All Locations
        </button>
    `;
    
    locations.forEach(location => {
        const button = document.createElement('button');
        button.className = `location-button ${currentLocation === location ? 'active' : ''}`;
        button.onclick = () => selectLocation(location);
        button.textContent = location;
        container.appendChild(button);
    });
}

function filterDataByLocation(data, location) {
    if (!location) return data.data;
    return data.data.filter(item => item.location === location);
}

function displayData(data) {
    const container = document.getElementById('dataContainer');
    const filteredData = filterDataByLocation(data, currentLocation);
    
    // Update sensor count
    document.getElementById('sensorCount').textContent = 
        `${filteredData.length} sensors ${currentLocation ? `in ${currentLocation}` : 'total'}`;

    container.innerHTML = '';

    filteredData.forEach(item => {
        const card = document.createElement('div');
        card.className = 'data-card';
        card.innerHTML = `
            <div class="card-header">
                <h3>${item.node_id}</h3>
                <span class="location-badge">${item.location}</span>
            </div>
            <div class="data-grid">
                <div class="data-item">
                    <span class="data-label">Indoor Temperature</span>
                    <span class="data-value">${formatValue(item.temperature_indoor, '°C')}</span>
                </div>
                <div class="data-item">
                    <span class="data-label">Outdoor Temperature</span>
                    <span class="data-value">${formatValue(item.temperature_outdoor, '°C')}</span>
                </div>
                <div class="data-item">
                    <span class="data-label">Humidity</span>
                    <span class="data-value">${formatValue(item.humidity, '%')}</span>
                </div>
                <div class="data-item">
                    <span class="data-label">Pressure</span>
                    <span class="data-value">${formatValue(item.pressure, ' hPa')}</span>
                </div>
                <div class="data-item">
                    <span class="data-label">Illumination</span>
                    <span class="data-value">${formatValue(item.illumination, ' lux')}</span>
                </div>
                <div class="data-item">
                    <span class="data-label">Last Updated</span>
                    <span class="data-value">${getTimeAgo(item.time)}</span>
                </div>
            </div>
        `;
        container.appendChild(card);
    });

    document.getElementById('lastUpdated').textContent = 
        `Last updated: ${new Date().toLocaleString()}`;
}

function selectLocation(location) {
    currentLocation = location;
    if (currentData) {
        createLocationButtons(getUniqueLocations(currentData));
        displayData(currentData);
    }
}

function showError(message) {
    const container = document.getElementById('dataContainer');
    container.innerHTML = `
        <div class="error-message">
            <strong>Error:</strong> ${message}
        </div>
    `;
}

function showLoading() {
    const container = document.getElementById('dataContainer');
    container.innerHTML = `
        <div class="loading">
            <div class="loading-spinner"></div>
            <span>Loading sensor data...</span>
        </div>
    `;
}

async function updateData() {
    try {
        showLoading();
        const data = await fetchData();
        currentData = data;
        const locations = getUniqueLocations(data);
        createLocationButtons(locations);
        displayData(data);
    } catch (error) {
        showError('Failed to fetch sensor data. Please try again later.');
    }
}

// Initial load
updateData();

// Set up periodic refresh
setInterval(updateData, REFRESH_INTERVAL);