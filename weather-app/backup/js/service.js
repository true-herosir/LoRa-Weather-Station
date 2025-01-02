    class WeatherThingyService {
        constructor() {
            this.API_IP = 'http://84.85.32.192';
            this.API_PORT = ':7086/api/';
            this.API_TABLES = {
                MOST_RECENT: 'most_recent',
                NODE_LOCATION: 'Nodes/node_location',
                HOURS_AVG: 'Hours_AVG',
                MAX_MIN: 'Max_Min',
                NODE_LOCATION_DATA: 'Node_location'
            };
        }

        async callAPI(url) {
            try {
                const response = await fetch(url);
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                const jsonData = await response.json();
                
                return {
                    total_items: Number(jsonData.total_items),
                    total_pages: Number(jsonData.total_pages),
                    current_page: Number(jsonData.current_page),
                    page_size: Number(jsonData.page_size),
                    data: this.parseData(jsonData.data)
                };
            } catch (error) {
                console.error('API call failed:', error);
                throw error;
            }
        }

        parseData(data) {
            return data.map(item => ({
                time: item.time ? new Date(item.time) : null,
                the_day: this.parseDateTime(item.the_day, item.the_hour),
                node_id: item.node_id || null,
                pressure: this.parseNumber(item.pressure),
                illumination: this.parseNumber(item.illumination),
                humidity: this.parseNumber(item.humidity),
                temperature_indoor: this.parseNumber(item.temperature_indoor),
                temperature_outdoor: this.parseNumber(item.temperature_outdoor),
                gateway_Location: item.gateway_Location || null,
                location: item.location || null,
                battery_status: item.battery_status?.toString() || null,
                min_pressure: this.parseNumber(item.min_pressure),
                min_illumination: this.parseNumber(item.min_illumination),
                min_humidity: this.parseNumber(item.min_humidity),
                min_temperature_indoor: this.parseNumber(item.min_temperature_indoor),
                min_temperature_outdoor: this.parseNumber(item.min_temperature_outdoor),
                max_pressure: this.parseNumber(item.max_pressure),
                max_illumination: this.parseNumber(item.max_illumination),
                max_humidity: this.parseNumber(item.max_humidity),
                max_temperature_indoor: this.parseNumber(item.max_temperature_indoor),
                max_temperature_outdoor: this.parseNumber(item.max_temperature_outdoor)
            }));
        }

        parseDateTime(date, hour) {
            if (!date) return null;
            const parsedDate = new Date(date);
            if (hour !== undefined && !isNaN(hour)) {
                parsedDate.setHours(hour);
            }
            return parsedDate;
        }

        parseNumber(value) {
            const parsed = parseFloat(value);
            return isNaN(parsed) ? null : parsed;
        }

        async getNodeData() {
            const params = new URLSearchParams({
                page: '1',
                page_size: '10'
            });
            
            const url = `${this.API_IP}${this.API_PORT}${this.API_TABLES.MOST_RECENT}?${params}`;
            return this.callAPI(url);
        }

        async getBattData() {
            const params = new URLSearchParams({
                page: '1',
                page_size: '10'
            });
            
            const url = `${this.API_IP}${this.API_PORT}${this.API_TABLES.NODE_LOCATION_DATA}?${params}`;
            return this.callAPI(url);
        }

        async getNodeDataWithParams(location, start, end, page) {
            // Validate dates
            const timeDiff = end.getTime() - start.getTime();
            const daysDifference = Math.floor(timeDiff / (1000 * 60 * 60 * 24));
            
            if (daysDifference < 0) {
                throw new Error('End date cannot be before start date');
            }

            // Determine location parameter format
            const isDevice = location.includes('lht') || location.includes('mkr') || location.includes('thingy');
            const locationParam = isDevice ? `id=${location}` : `location=${location}`;
            
            // Determine page size based on device type
            const pageSize = isDevice ? 350 : 900;

            // Determine which table and date format to use based on date range
            let table, startParam, endParam;
            
            if (daysDifference <= 3) {
                table = this.API_TABLES.NODE_LOCATION;
                startParam = `start_time=${this.formatDateTime(start)}`;
                endParam = `end_time=${this.formatDateTime(end)}`;
            } else if (daysDifference <= 14) {
                table = this.API_TABLES.HOURS_AVG;
                startParam = `start_date=${this.formatDate(start)}`;
                endParam = `end_date=${this.formatDate(end)}`;
            } else {
                table = this.API_TABLES.MAX_MIN;
                startParam = `start_date=${this.formatDate(start)}`;
                endParam = `end_date=${this.formatDate(end)}`;
            }

            const url = `${this.API_IP}${this.API_PORT}${table}?${locationParam}&${startParam}&${endParam}&page=${page}&page_size=${pageSize}`;
            return this.callAPI(url);
        }

        formatDateTime(date) {
            return encodeURIComponent(
                date.toISOString()
                    .replace('T', ' ')
                    .replace('Z', '')
                    .split('.')[0]
            );
        }

        formatDate(date) {
            return date.toISOString().split('T')[0];
        }
    }

    // Export the service
    export default WeatherThingyService;