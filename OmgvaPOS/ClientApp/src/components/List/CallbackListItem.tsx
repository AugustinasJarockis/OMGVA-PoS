import React from 'react';
import './ClickableListItem.css';

interface ClickableListItemProps {
    onClickHandle: () => void;
    text?: string;
}

const ClickableListItem: React.FC<ClickableListItemProps> = (props: ClickableListItemProps) => {
    return (
        <div className="clickable-list-item" onClick={props.onClickHandle}>{props.text}</div>
    );
};

export default ClickableListItem;
