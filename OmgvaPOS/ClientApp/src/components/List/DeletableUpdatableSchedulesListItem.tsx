import React from 'react';
import { useNavigate } from 'react-router-dom';
import './DeletableUpdatableSchedulesListItem.css';
import { statusMap } from '../../services/reservationService';

interface DeletableUpdatableSchedulesListItemProps {
    id: string;
    customerName: string;
    timeReserved: string;
    status: number;
    updateUrl: string;
    deleteFunction: (id: string) => void;
    disableDelete?: boolean;
    object?: any;
}

const DeletableUpdatableSchedulesListItem: React.FC<DeletableUpdatableSchedulesListItemProps> = (props) => {
    const navigate = useNavigate();

    const goToUpdate = () => {
        navigate(props.updateUrl, { state: { object: props.object } });
    };

    const deleteItem = async () => {
        props.deleteFunction(props.id);
    };

    return (
        <div className="deletable-updatable-schedule-list-item">
            <p>Customer: {props.customerName}</p>
            <p>Time Reserved: {props.timeReserved.split('T')[0]} {props.timeReserved.split('T')[1]}</p>
            <p>Status: {statusMap[props.status]}</p>
            <button onClick={deleteItem} disabled={props.disableDelete ?? (statusMap[props.status] == 'Cancelled' || statusMap[props.status] == 'Done')}>Delete</button>
            <button onClick={goToUpdate} disabled={statusMap[props.status] == 'Cancelled' || statusMap[props.status] == 'Done'}>Update</button>
        </div>
    );
};

export default DeletableUpdatableSchedulesListItem;
