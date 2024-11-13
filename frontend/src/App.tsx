// frontend/src/App.tsx

import React, { useEffect, useState } from 'react';
import { getWeatherForecast } from './services/weatherService';

interface WeatherForecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

const App: React.FC = () => {
    const [forecasts, setForecasts] = useState<WeatherForecast[]>([]);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        getWeatherForecast()
            .then((data) => setForecasts(data))
            .catch((err) => setError(err.message));
    }, []);

    return (
        <div>
            <h1>Weather Forecast</h1>
            {error && <p>Error: {error}</p>}
            <ul>
                {forecasts.map((forecast, index) => (
                    <li key={index}>
                        <strong>{forecast.date}</strong>: {forecast.summary} - {forecast.temperatureC}°C / {forecast.temperatureF}°F
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default App;
