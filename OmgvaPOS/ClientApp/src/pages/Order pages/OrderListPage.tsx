import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import '../../index.css';
import './OrderPages.css';
import '../../components/List/ClickableListItem.css';
import { getTokenBusinessId } from '../../utils/tokenUtils';
import { useAuth } from '../../contexts/AuthContext';
import { OrderStatus, getAllActiveOrders, getAllOrders } from '../../services/orderService';
import ClickableOrderListItem from '../../components/List/ClickableOrderListItem';
import ClickableListItem from '../../components/List/ClickableListItem';

const OrderListPage: React.FC = () => {
    const [listItems, setListItems] = useState<Array<JSX.Element>>();
    const [error, setError] = useState<string | null>(null);
    const [showOnlyOpen, setShowOnlyOpen] = useState<boolean>(true);
    const navigate = useNavigate();
    const { authToken } = useAuth();

    const getActiveOrders = async () => {
        setError(null);

        try {
            const { result, error } = await getAllActiveOrders(authToken);

            if (!result) {
                setError('Problem acquiring active orders: ' + error);
            }
            else {
                setListItems(result.map(order =>
                    <ClickableOrderListItem orderType={OrderStatus.Open} key={order.Id} text={'Order  #' + order.Id} url={'/order/' + order.Id} />));
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const getOrders = async () => {
        setError(null);

        try {
            const { result, error } = await getAllOrders(authToken);

            if (!result) {
                setError('Problem acquiring all orders: ' + error);
            }
            else {
                setListItems(result.map(order =>
                    <ClickableOrderListItem orderType={order.Status} key={order.Id} text={'Order  #' + order.Id} url={'/order/' + order.Id} />));
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const changeShowOnlyOpen = async () => {
        setShowOnlyOpen(showOnlyOpen ? false : true);
        if (showOnlyOpen) {
            getOrders();
        }
        else {
            getActiveOrders();
        }
    }

    const goToBusiness = async () => {
        if (authToken)
            navigate('/business/' + getTokenBusinessId(authToken));
        else
            navigate('/');
    }

    useEffect(() => {
        if (authToken) {
            getActiveOrders();
        }
        else {
            setError("You have to authenticate first!");
        }
    }, []);

    return (
        <div>
            <header>
                <button onClick={goToBusiness}>Business</button>
            </header>
            <br/><br/>
            <button onClick={changeShowOnlyOpen}>{ showOnlyOpen ? "Show only open orders" : "Show all orders" }</button>
            <h1>Current open orders</h1>
            <div className="business-list-container">
                {listItems}
                <div className="create-button-wrapper">
                    <ClickableListItem key="create" text="Create a new order" url={'/order/create'} />
                </div>
            </div>
            {error && <p className="error-message">{error}</p>}
        </div>
    );
};

export default OrderListPage;

