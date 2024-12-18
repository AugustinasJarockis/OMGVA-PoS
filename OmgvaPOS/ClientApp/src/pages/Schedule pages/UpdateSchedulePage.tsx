import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { getEmployeeSchedule, UpdateEmployeeSchedule } from '../../services/scheduleService';
import { useAuth } from '../../contexts/AuthContext';

const UpdateSchedulePage: React.FC = () => {
    const [date, setDate] = useState<string>('');
    const [startTime, setStartTime] = useState<string>('');
    const [endTime, setEndTime] = useState<string>('');
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState<string | null>(null);
    const { authToken } = useAuth();
    const { id } = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        const fetchSchedule = async () => {
            setError(null);

            try {
                const { result, error } = await getEmployeeSchedule(authToken, id ?? '');
                if (error) {
                    setError('Failed to fetch schedule: ' + error);
                } else if (result) {
                    setDate(new Date(result.Date).toISOString().split('T')[0]);
                    setStartTime(result.StartTime);
                    setEndTime(result.EndTime);
                }
            } catch (err: any) {
                setError(err.message || 'An unexpected error occurred.');
            }
        };

        if (authToken) {
            fetchSchedule();
        } else {
            setError('You must authenticate first!');
        }
    }, [authToken, id]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);
        setSuccess(null);

        if (!date || !startTime || !endTime) {
            setError('All fields are required!');
            return;
        }

        if (startTime >= endTime) {
            setError('Start time cannot be later than or equal to end time!');
            return;
        }

        try {
            const { error, result } = await UpdateEmployeeSchedule(authToken, id ?? '', {
                StartTime: startTime,
                EndTime: endTime,
            });

            if (error) {
                setError('Failed to update schedule: ' + error);
            } else if (result) {
                setSuccess('Schedule updated successfully!');
                setTimeout(() => navigate(`/employee/${id}/schedules`), 2000);
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    };

    const handleCancel = () => {
        navigate(`/employee/${id}/schedules`);
    };

    return (
        <div>
            <h1>Update Schedule</h1>
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
                        disabled
                    />
                </div>
                <div>
                    <label htmlFor="start-time">Start Time:</label>
                    <input
                        type="time"
                        id="start-time"
                        value={startTime}
                        onChange={(e) => setStartTime(e.target.value+':00')}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="end-time">End Time:</label>
                    <input
                        type="time"
                        id="end-time"
                        value={endTime}
                        onChange={(e) => setEndTime(e.target.value+':00')}
                        required
                    />
                </div>
                <div>
                    <button type="submit">Update</button>
                    <button type="button" onClick={handleCancel}>
                        Return
                    </button>
                </div>
            </form>
        </div>
    );
};

export default UpdateSchedulePage;
