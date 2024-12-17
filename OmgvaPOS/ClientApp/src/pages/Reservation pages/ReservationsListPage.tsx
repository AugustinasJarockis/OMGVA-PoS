import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { deleteReservation, getEmployeeReservations } from '../../services/reservationService';
import { useAuth } from '../../contexts/AuthContext';
import { getTokenRole } from '../../utils/tokenUtils';
import DeletableUpdatableReservationListItem from '../../components/List/DeletableUpdatableReservationListItem';

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
                    <DeletableUpdatableReservationListItem
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
        try {
            const error = await deleteReservation(authToken, id);

            if (error) {
                setError("An error occurred while deleting reservation: " + error);
                return;
            }

            if (!reservations) {
                fetchReservations();
            }

            if (reservations) {
                const newList = reservations.filter((reservation) => reservation.key != id);
                setReservations(newList);
            }
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    };

    const returnToHome = async () => {
        const role = getTokenRole(authToken ?? "");
        if (role == "Admin")
            navigate(`/user/${id}`);

        navigate(`/home`);
    };

    const goToCreate = async () => {
        if(authToken == null)
            navigate(`/`);
        
        navigate(`/schedules/create/${id}`);
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
            <h1>Employee Schedules</h1>
            {reservations}
            <button onClick={goToCreate}>Add new schedule</button>
            <button onClick={returnToHome}>Return</button>
            {error && <p className="error-message">{error}</p>}
        </div>
    );
};

export default EmployeeReservationsPage;
