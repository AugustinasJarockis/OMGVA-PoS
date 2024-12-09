import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import BusinessPage from './pages/BusinessPage';
import SelectBusinessPage from './pages/SelectBusinessPage';
import UpdateBusinessPage from './pages/UpdateBusinessPage';
import WeatherPage from './pages/WeatherPage';
import './App.css';
import { getTokenRole, isTokenValid } from './utils/tokenUtils';
import CreateBusinessPage from './pages/CreateBusinessPage';

const App: React.FC = () => {
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);

    const checkTokenValidity = () => {
        const token = localStorage.getItem('authToken');
        if (token && isTokenValid(token)) {
            setIsAuthenticated(true);
        } else {
            localStorage.removeItem('authToken');
            setIsAuthenticated(false);
        }
    };

    useEffect(() => {
        checkTokenValidity();

        const handleUnload = () => {
            localStorage.removeItem('authToken');
        };

        window.addEventListener('beforeunload', handleUnload);

        return () => {
            window.removeEventListener('beforeunload', handleUnload);
        };
    }, []);

    const handleLoginSuccess = () => {
        checkTokenValidity();
    };

    const handleLogout = () => {
        localStorage.removeItem('authToken');
        setIsAuthenticated(false);
    };

    const getRole = () => {
        const token = localStorage.getItem('authToken');
        if (token) {
            return getTokenRole(token);
        }
        return null;
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
                            <Route path="/business/:id" element={<BusinessPage token={localStorage.getItem('authToken')} />} />
                            <Route path="/business/create" element={<CreateBusinessPage token={localStorage.getItem('authToken')} />} />
                            <Route path="/business/update/:id" element={<UpdateBusinessPage token={localStorage.getItem('authToken')} />} />
                            <Route path="/weather" element={<WeatherPage onLogout={handleLogout} />} />
                            {localStorage.getItem('authToken') !== null && getRole() === "Admin"
                                ? (<Route path="*" element={<Navigate to="/business" />} />)
                                : (<Route path="*" element={<Navigate to="/weather" />} />)}
                        </>
                    )}
                </Routes>
            </div>
        </Router>
    );
};

export default App;
