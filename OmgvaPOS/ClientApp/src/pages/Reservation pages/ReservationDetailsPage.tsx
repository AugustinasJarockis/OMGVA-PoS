import React, { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getReservation, deleteReservation, statusMap } from '../../services/reservationService';
import { useAuth } from '../../contexts/AuthContext';

const ReservationDetailsPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const { authToken } = useAuth();
    const navigate = useNavigate();
    const [reservation, setReservation] = useState<any>(null);
    const [error, setError] = useState<string | null>(null);

    const fetchReservation = async () => {
        setError(null);
        try {
            const { result, error } = await getReservation(authToken, id!);
            if (error) {
                setError('Error fetching reservation: ' + error);
            } else {
                setReservation(result);
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    };

    const handleUpdate = async () => {
        navigate(`/reservation/update/${reservation.Id}`);
    };

    const handleDelete = async () => {
        const error = await deleteReservation(authToken, id!);
        if (!error) navigate(`/reservation/employee/${id}`);
    };

    const returnToList = async () => {
        navigate(`/reservation/employee/${id}`);
    };

    React.useEffect(() => {
        if (authToken) fetchReservation();
    }, [authToken]);

    return (
        <div>
            <h1>Reservation Details</h1>
            {reservation ? (
                <div>
                    <p>Customer: {reservation.CustomerName}</p>
                    <p>Time Reserved: {reservation.TimeReserved.split('T')[0]} {reservation.TimeReserved.split('T')[1]}</p>
                    <p>Status: {statusMap[reservation.Status]}</p>
                    <button onClick={handleUpdate} disabled={ statusMap[reservation.Status] == 'Cancelled' || statusMap[reservation.Status] == 'Done' } >Update</button>
                    <button onClick={handleDelete} disabled={ statusMap[reservation.Status] == 'Cancelled' || statusMap[reservation.Status] == 'Done' } >Delete</button>
                </div>
            ) : (
                <p>Loading reservation...</p>
            )}
            {error && <p className="error-message">{error}</p>}
            <button onClick={returnToList}>Return to list</button>
        </div>
    );
};

export default ReservationDetailsPage;
