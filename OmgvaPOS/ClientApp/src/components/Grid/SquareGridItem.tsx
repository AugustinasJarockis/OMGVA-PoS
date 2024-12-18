import React from 'react';
import './SquareGridItem.css';

interface SquareGridItemProps {
    children: any;
    onclick: () => void;
}

const SquareGridItem: React.FC<SquareGridItemProps> = (props: SquareGridItemProps) => {
    return (
        <div className="square-grid-item" onClick={ props.onclick }>{props.children}</div>
    );
};

export default SquareGridItem;