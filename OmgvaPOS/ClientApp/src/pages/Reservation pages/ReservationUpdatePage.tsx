import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getReservation, updateReservation } from '../../services/reservationService';
import { useAuth } from '../../contexts/AuthContext';

const ReservationUpdatePage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const { authToken } = useAuth();
    const navigate = useNavigate();
    const [reservation, setReservation] = useState<any>(null);
    const [error, setError] = useState<string | null>(null);
    const [formData, setFormData] = useState<any>({
        timeReserved: '',
        status: ''
    });

    const fetchReservation = async () => {
        setError(null);
        try {
            const { result, error } = await getReservation(authToken, id!);
            if (error) {
                setError('Error fetching reservation: ' + error);
            } else {
                if (result != null) {
                    setReservation(result);
                    setFormData({
                        employeeId: result.EmployeeId,
                        customerId: result.CustomerId,
                        timeReserved: result.TimeReserved,
                        status: result.Status
                    });
                }
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value } = e.target;
        setFormData((prevState: any) => ({
            ...prevState,
            [name]: value
        }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const { employeeId, customerId, timeReserved, status } = formData;
        const updatedReservation = {
            EmployeeId: employeeId,
            CustomerId: customerId,
            TimeReserved: timeReserved,
            Status: status
        };
        const { error } = await updateReservation(authToken, id!, updatedReservation);
        if (!error) {
            navigate(`/reservation/${id}`);
        } else {
            setError(error);
        }
    };

    const returnToList = async () => {
        navigate(`/reservation/employee/${formData.employeeId}`);
    };

    useEffect(() => {
        if (authToken) fetchReservation();
    }, [authToken]);

    return (
        <div>
            <h1>Update Reservation</h1>
            {reservation ? (
                <form onSubmit={handleSubmit}>
                    <div>
                        <label htmlFor="timeReserved">Time Reserved:</label>
                        <input
                            type="datetime-local"
                            id="timeReserved"
                            name="timeReserved"
                            value={formData.timeReserved}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div>
                        <label htmlFor="status">Status:</label>
                        <select
                            id="status"
                            name="status"
                            value={formData.status}
                            onChange={handleChange}
                            required
                        >
                            <option value="0">Open</option>
                            <option value="1">Cancelled</option>
                            <option value="2">Done</option>
                        </select>
                        <br></br>
                        <br></br>
                    </div>
                    <div>
                        <button type="submit">Update Reservation</button>
                    </div>
                    <br></br>
                </form>
            ) : (
                <p>Loading reservation...</p>
            )}
            {error && <p className="error-message">{error}</p>}
            <button onClick={returnToList}>Return to list</button>
        </div>
    );
};

export default ReservationUpdatePage;
