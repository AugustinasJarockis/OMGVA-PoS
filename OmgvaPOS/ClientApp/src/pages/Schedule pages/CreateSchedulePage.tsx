import React, { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { createEmployeeSchedule } from '../../services/scheduleService';
import { useAuth } from '../../contexts/AuthContext';

const CreateSchedulePage: React.FC = () => {
    const [date, setDate] = useState<string>('');
    const [startTime, setStartTime] = useState<string>('');
    const [endTime, setEndTime] = useState<string>('');
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState<string | null>(null);
    const { authToken } = useAuth();
    const { id } = useParams();
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);
        setSuccess(null);

        if (!date || !startTime || !endTime) {
            setError("All fields are required!");
            return;
        }

        try {
            const { error, result } = await createEmployeeSchedule(authToken, {
                EmployeeId: parseInt(id ?? '0'),
                Date: new Date(date).toISOString().split('T')[0],
                StartTime: startTime+':00',
                EndTime: endTime+':00',
            });

            if (error) {
                setError(error);
            } else {
                setSuccess("Schedule created successfully!");
                setTimeout(() => navigate(`/schedules/${id}`), 2000);
            }
        } catch (err: any) {
            setError(err.message || "An unexpected error occurred.");
        }
    };

    const handleCancel = () => {
        navigate(`/schedules/${id}`);
    };

    return (
        <div>
            <h1>Create Schedule</h1>
            {error && <p className="error-message">{error}</p>}
            {success && <p className="success-message">{success}</p>}
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="date">Date:</label>
                    <input
                        type="date"
                        id="date"
                        value={date}
                        onChange={(e) => setDate(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="start-time">Start Time:</label>
                    <input
                        type="time"
                        id="start-time"
                        value={startTime}
                        onChange={(e) => setStartTime(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="end-time">End Time:</label>
                    <input
                        type="time"
                        id="end-time"
                        value={endTime}
                        onChange={(e) => setEndTime(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <button type="submit">Create</button>
                    <button type="button" onClick={handleCancel}>
                        Cancel
                    </button>
                </div>
            </form>
        </div>
    );
};

export default CreateSchedulePage;
