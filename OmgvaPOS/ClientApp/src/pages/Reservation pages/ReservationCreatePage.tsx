﻿import React, { useState, useEffect } from 'react';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import { CreateReservation, createReservation } from '../../services/reservationService';
import { useAuth } from '../../contexts/AuthContext';
import { getEmployeeSchedulesByItemAndDate } from '../../services/scheduleService';
import ScheduleViewer from '../../components/Schedules';

interface ReservationCreatePageParams extends Record<string, string | undefined> {
  itemId: string;
    employeeId: string;
    orderId: string;
}

const ReservationCreatePage: React.FC = () => {
  const { itemId, employeeId, orderId } = useParams<ReservationCreatePageParams>();
  const [timeReserved, setTimeReserved] = useState<string>('');
  const [customerId, setCustomerId] = useState<number>(0);
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const { state } = useLocation();
  const [scheduleData, setScheduleData] = useState<any>(null);
  const navigate = useNavigate();
  const { authToken } = useAuth();


    useEffect(() => {
    if (timeReserved) {
        const fetchSchedule = async () => {
            const date = new Date(timeReserved).toISOString().split('T')[0];
        const { result, error } = await getEmployeeSchedulesByItemAndDate(authToken, itemId ?? "", date);
        
        if (error) {
            setError(error);
            setScheduleData(null);
        } else {
            setScheduleData(result);
            setError(null);
        }
        };

        fetchSchedule();
    }
  }, [timeReserved, itemId, authToken]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const reservationData: CreateReservation = {
      TimeReserved: new Date(timeReserved),
      EmployeeId: parseInt(employeeId ?? ""),
      CustomerId: customerId,
      ItemId: parseInt(itemId ?? ""),
    };

    const { result, error } = await createReservation(authToken, reservationData);

    if (error) {
      setError(error);
      setSuccessMessage(null);
    } else {
        setSuccessMessage('Reservation created successfully!');
        if (state.group)
            setTimeout(() => navigate(`/order/${orderId}/add-items`, { state: { group: state.group } }), 2000);
        else {
           setTimeout(() =>  navigate(`/order/${orderId}/add-items`), 2000);
        }
      setError(null);
    }
   };

    const returnToVariations = () => {
        navigate(`/order/${orderId}/add-items/${itemId}/variations`, { state: { group: state.group } });
    }

  return (
      <div>
        <header>
            <button onClick={returnToVariations}>Return</button>
        </header>
      <h2>Create a Reservation</h2>
      
      {successMessage && <p style={{ color: 'green' }}>{successMessage}</p>}
      {error && <p style={{ color: 'red' }}>{error}</p>}

      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="timeReserved">Reservation Time:</label>
          <input
            type="datetime-local"
            id="timeReserved"
            value={timeReserved}
            onChange={(e) => setTimeReserved(e.target.value)}
            required
          />
        </div>
        
        <div>
          <label htmlFor="customerId">Customer ID:</label>
          <input
            type="number"
            id="customerId"
            value={customerId}
            onChange={(e) => setCustomerId(Number(e.target.value))}
            required
          />
        </div>

        <button type="submit">Create Reservation and Finish Selection</button>
      </form>

      {scheduleData && (
        <ScheduleViewer
          employeeSchedulesWithAvailability={scheduleData}
        />
      )}
    </div>
  );
};

export default ReservationCreatePage;