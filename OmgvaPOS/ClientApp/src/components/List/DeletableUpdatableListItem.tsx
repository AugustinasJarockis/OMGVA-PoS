import React from 'react';
import { useNavigate } from "react-router-dom";
import './DeletableUpdatableListItem.css';

interface DeletableUpdatableListItemProps {
    id: string
    text: string
    updateUrl: string
    deleteFunction: (id: string) => void
    disableDelete?: boolean
    object?: any
}

const DeletableUpdatableListItem: React.FC<DeletableUpdatableListItemProps> = (props: DeletableUpdatableListItemProps) => {
    const navigate = useNavigate();

    const goToUpdate = () => {
        navigate(props.updateUrl, { state: { object: props.object } });
    };

    const deleteItem = async () => {
        props.deleteFunction(props.id);
    }

    return (
        <div className="deletable-updatable-list-item">
            {props.text}
            <button onClick={deleteItem} disabled={props.disableDelete ?? false}>Delete</button>
            <button onClick={goToUpdate}>Update</button>
        </div>
    );
};

export default DeletableUpdatableListItem;