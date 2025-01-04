class WeatherService {
    constructor(config) {
        this.config = config;
    }

    async fetchData(startDate, endDate, location) {
        const params = this._buildRequestParams(startDate, endDate, location);
        const url = this._buildUrl(params);
        return await this._makeRequest(url);
    }

    _buildRequestParams(startDate, endDate, location) {
        const timeDiff = (endDate - startDate) / (1000 * 60 * 60 * 24);
        const tableIndex = this._getTableIndex(timeDiff);
        return {
            table: this.config.API_TABLES[tableIndex],
            location,
            startDate,
            endDate,
            timeDiff,
            pageSize: location?.includes("lht") ? 350 : 900
        };
    }

    _getTableIndex(timeDiff) {
        if (timeDiff <= 3) return 1;
        if (timeDiff <= 14) return 2;
        return 3;
    }

    _buildUrl(params) {
        const url = new URL(this.config.API_URL);
        const dateFormat = params.timeDiff <= 3 ? 'time' : 'date';
        
        url.searchParams.set('table', params.table);
        if (params.location) url.searchParams.set('location', params.location);
        url.searchParams.set(`start_${dateFormat}`, this._formatDate(params.startDate, dateFormat === 'time'));
        url.searchParams.set(`end_${dateFormat}`, this._formatDate(params.endDate, dateFormat === 'time'));
        url.searchParams.set('page', '1');
        url.searchParams.set('page_size', params.pageSize.toString());
        
        return url.toString();
    }

    _formatDate(date, includeTime) {
        return includeTime 
            ? date.toISOString().replace('T', '%20').replace(/:/g, '%3A').slice(0, -5)
            : date.toISOString().split('T')[0];
    }

    async _makeRequest(url) {
        const controller = new AbortController();
        const timeoutId = setTimeout(() => controller.abort(), 5000);

        try {
            const response = await fetch(url, { signal: controller.signal });
            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
            return await response.json();
        } finally {
            clearTimeout(timeoutId);
        }
    }
}