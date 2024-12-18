import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { deleteEmployeeSchedule, getAllSchedulesByEmployeeId } from '../../services/scheduleService';
import { useAuth } from '../../contexts/AuthContext';
import { getTokenRole } from '../../utils/tokenUtils';
import DeletableUpdatableSchedulesListItem from '../../components/List/DeletableUpdatableScheduleListItem';

const EmployeeSchedulesPage: React.FC = () => {
    const [schedules, setSchedules] = useState<Array<JSX.Element>>([]);
    const [error, setError] = useState<string | null>(null);
    const { authToken } = useAuth();
    const { id } = useParams();
    const navigate = useNavigate();

    const fetchSchedules = async () => {
        setError(null);
        try {
            const { result, error } = await getAllSchedulesByEmployeeId(authToken, id ?? "");
            if (error) {
                setError('Error fetching schedules: ' + error);
            } else if (result) {
                setSchedules(result.map(schedule => (
                    <DeletableUpdatableSchedulesListItem
                        key={schedule.Id}
                        id={schedule.Id.toString()}
                        date={`${schedule.Date}`}
                        startTime={schedule.StartTime}
                        endTime={schedule.EndTime}
                        updateUrl={`/schedules/update/${schedule.Id}`}
                        deleteFunction={handleDelete}
                        object={schedule}
                    />
                )));
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    };

    const handleDelete = async (id: string) => {
         try {
             const error = await deleteEmployeeSchedule (authToken, id);

            if (error) {
                setError("An error occurred while deleting schedule: " + error);
                return;
            }

            if (!schedules) {
                fetchSchedules();
            }

            if (schedules) {
                const newList = schedules.filter((schedule) => schedule.key != id);
                setSchedules(newList);
            }
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    };

    const returnToHome = () => {
        const role = getTokenRole(authToken ?? "");
        if (role === "Admin") {
            navigate(`/user/${id}`);
        } else {
            navigate(`/home`);
        }
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
            fetchSchedules();
        }
    }, [authToken]);

    return (
        <div>
            <h1>Employee Schedules</h1>
            <div>{schedules}</div>
            <button onClick={goToCreate}>Add new schedule</button>
            <button onClick={returnToHome}>Return</button>
            {error && <p className="error-message">{error}</p>}
        </div>
    );
};

export default EmployeeSchedulesPage;
