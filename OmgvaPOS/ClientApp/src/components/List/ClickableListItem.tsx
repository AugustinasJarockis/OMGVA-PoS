import React from 'react';
import { useNavigate } from "react-router-dom";
import './ClickableListItem.css';

interface ClickableListItemProps {
    url: string;
    text?: string;
}

const ClickableListItem: React.FC<ClickableListItemProps> = (props: ClickableListItemProps) => {
    const navigate = useNavigate();

    const handleClick = async () => {
        navigate(props.url);
    };

    return (
        <div className="clickable-list-item" onClick={handleClick}>{props.text}</div>
    );
};

export default ClickableListItem;
