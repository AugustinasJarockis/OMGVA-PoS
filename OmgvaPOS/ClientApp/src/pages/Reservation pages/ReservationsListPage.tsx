import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { getEmployeeReservations } from '../../services/reservationService';
import { useAuth } from '../../contexts/AuthContext';
import DeletableUpdatableSchedulesListItem from '../../components/List/DeletableUpdatableSchedulesListItem';
import { getTokenRole } from '../../utils/tokenUtils';

const EmployeeReservationsPage: React.FC = () => {
    const [reservations, setReservations] = useState<Array<JSX.Element>>([]);
    const [error, setError] = useState<string | null>(null);
    const { authToken } = useAuth();
    const { id } = useParams();
    const navigate = useNavigate();

    const fetchReservations = async () => {
        setError(null);
        try {
            console.log(id);
            const { result, error } = await getEmployeeReservations(authToken, id ?? "");
            if (error) {
                setError('Error fetching reservations: ' + error);
            } else if (result) {
                setReservations(result.map(reservation => (
                    <DeletableUpdatableSchedulesListItem
                        key={reservation.Id}
                        id={reservation.Id.toString()}
                        customerName={reservation.CustomerName ?? ""}
                        timeReserved={reservation.TimeReserved}
                        status={reservation.Status}
                        updateUrl={`/reservation/update/${reservation.Id}`}
                        deleteFunction={handleDelete}
                        object={reservation}
                    />
                )));
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    };

    const handleDelete = async (id: string) => {
        // Implement the delete logic here (call the API or update state)
        console.log(`Delete reservation with id: ${id}`);
    };

    const returnToHome = async () => {
        const role = getTokenRole(authToken ?? "");
        if (role == "Admin")
            navigate(`/user/${id}`);

        navigate(`/home`);
    };

    useEffect(() => {
        if (!authToken) {
            setError("You have to authenticate first!");
        } else {
            fetchReservations();
        }
    }, [authToken]);

    return (
        <div>
            <h1>Employee Reservations</h1>
            <div>{reservations}</div>
            <button onClick={returnToHome}>Return</button>
            {error && <p className="error-message">{error}</p>}
        </div>
    );
};

export default EmployeeReservationsPage;
