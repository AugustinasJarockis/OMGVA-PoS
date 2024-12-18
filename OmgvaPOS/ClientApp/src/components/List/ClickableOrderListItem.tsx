import React from 'react';
import './ClickableListItem.css';
import './ClickableOrderListItem.css';
import { OrderStatus } from '../../services/orderService';
import ClickableListItem from './ClickableListItem';

interface ClickableOrderListItemProps {
    url: string;
    text?: string;
    orderType: OrderStatus;
}

const ClickableOrderListItem: React.FC<ClickableOrderListItemProps> = (props: ClickableOrderListItemProps) => {
    return (
        <>
            {props.orderType === OrderStatus.Open &&
            <ClickableListItem url={props.url} text={props.text} />}
            {props.orderType === OrderStatus.Closed && 
            <div className='closed-order-clickable'>
                <ClickableListItem url={props.url} text={props.text} />
            </div>}
            {props.orderType === OrderStatus.Cancelled &&
            <div className='cancelled-order-clickable'>
                <ClickableListItem url={props.url} text={props.text} />
            </div>}
            {props.orderType === OrderStatus.Refunded &&
            <div className='refunded-order-clickable'>
                <ClickableListItem url={props.url} text={props.text} />
            </div>}
        </>
    );
};

export default ClickableOrderListItem;
