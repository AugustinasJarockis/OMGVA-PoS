import React from 'react';
import { useNavigate } from "react-router-dom";
import './DeletableUpdatableListItem.css';

interface DeletableUpdatableWithScheduleListItemProps {
    id: string
    text: string
    updateUrl: string
    scheduleUrl: string;
    deleteFunction: (id: string) => void
    disableDelete?: boolean
    object?: any
}

const DeletableUpdatableWithScheduleListItem: React.FC<DeletableUpdatableWithScheduleListItemProps> = (props: DeletableUpdatableWithScheduleListItemProps) => {
    const navigate = useNavigate();

    const goToUpdate = () => {
        navigate(props.updateUrl, { state: { object: props.object } });
    };

    const deleteItem = async () => {
        props.deleteFunction(props.id);
    }

    const goToSchedule = () => {
        navigate(props.scheduleUrl, { state: { object: props.object } });
    };

    return (
        <div className="deletable-updatable-list-item">
            {props.text}
            <button onClick={deleteItem} disabled={props.disableDelete ?? false}>Delete</button>
            <button onClick={goToUpdate}>Update</button>
            <button onClick={goToSchedule}>See schedule</button>
        </div>
    );
};

export default DeletableUpdatableWithScheduleListItem;