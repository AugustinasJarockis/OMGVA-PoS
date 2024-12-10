import React from 'react';

interface WeatherPageProps {
    onLogout: () => void;
}

const WeatherPage: React.FC<WeatherPageProps> = ({ onLogout }) => {
    return (
        <div className="weather-container">
            <h1>Weather Forecast</h1>
            <button onClick={onLogout}>Logout</button>
        </div>
    );
};

export default WeatherPage;
