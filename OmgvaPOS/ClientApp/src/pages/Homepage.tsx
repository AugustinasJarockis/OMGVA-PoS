import React from 'react';
import { useNavigate } from 'react-router-dom';
import { getTokenUserId, getTokenUserName } from '../utils/tokenUtils';
import './Homepage.css';
import { useAuth } from '../contexts/AuthContext';
interface HomePageProps {
    onLogout: () => void;
}

const HomePage: React.FC<HomePageProps> = ({ onLogout }) => {
    const navigate = useNavigate();
    const { authToken } = useAuth();

    const getEmployeeName = () => {
        return getTokenUserName(authToken ?? "");
    };

    const goToReservations = () => {
        const id = getTokenUserId(authToken ?? "");
        navigate(`/reservation/employee/${id}`);
    };

    const goToSchedule = () => {
        const id = getTokenUserId(authToken ?? "");
        navigate(`/schedules/${id}`);
    };

    return (
        <div>
            <nav className="nav-bar">
                <ul className="nav-list">
                    <li>
                        <button onClick={goToReservations}>My Reservations</button>
                    </li>
                    <li>
                        <button onClick={goToSchedule}>My Schedule</button>
                    </li>
                    <li>
                        <button onClick={onLogout}>Logout</button>
                    </li>
                </ul>
            </nav>

            <div className="weather-container">
                <h1>Welcome, {getEmployeeName()}!</h1>
            </div>
        </div>
    );
};

export default HomePage;
