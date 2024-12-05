import React, { useState } from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import WeatherPage from './pages/WeatherPage';
import './App.css';
import SelectBusinessPage from './pages/SelectBusinessPage';

const App: React.FC = () => {
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(!!localStorage.getItem('authToken'));

    const handleLoginSuccess = () => {
        setIsAuthenticated(true);
    };

    const handleLogout = () => {
        localStorage.removeItem('authToken');
        setIsAuthenticated(false);
    };

    return (
        <Router>
            <div className="app-container">
                <Routes>
                    {!isAuthenticated ? (
                        <Route path="/" element={<LoginPage onLoginSuccess={handleLoginSuccess} />} />
                    ) : (
                        <>
                                <Route path="/business" element={<SelectBusinessPage token={localStorage.getItem('authToken')} />} />
                            <Route path="/weather" element={<WeatherPage onLogout={handleLogout} />} />
                            <Route path="*" element={<Navigate to="/weather" />} />
                        </>
                    )}
                </Routes>
            </div>
        </Router>
    );
};

export default App;
