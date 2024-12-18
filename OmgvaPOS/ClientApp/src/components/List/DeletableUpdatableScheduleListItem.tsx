import { useNavigate } from 'react-router-dom';
import './DeletableUpdatableSchedulesListItem.css';

interface DeletableUpdatableSchedulesListItemProps {
    id: string;
    date: string;
    startTime: string;
    endTime: string;
    updateUrl: string;
    deleteFunction: (id: string) => void;
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
            <p>Date: {props.date}</p>
            <p>Start time: {props.startTime}</p>
            <p>End time: {props.endTime}</p>
            <button onClick={deleteItem} >Cancel schedule</button>
            <button onClick={goToUpdate} >Update</button>
        </div>
    );
};

export default DeletableUpdatableSchedulesListItem ;