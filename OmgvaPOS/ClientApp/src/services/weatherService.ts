import axios from 'axios';

export const getWeatherForecast = async () => {
    try {
        const response = await axios.get('/api/WeatherForecast');
        return response.data;
    } catch (error) {
        console.error('Error fetching weather forecast:', error);
        throw error;
    }
};