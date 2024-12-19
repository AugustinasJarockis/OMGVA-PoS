import React, { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import '../../index.css';
import './OrderPages.css';
import '../../components/List/ClickableListItem.css';
import { getTokenBusinessId } from '../../utils/tokenUtils';
import { useAuth } from '../../contexts/AuthContext';
import { OrderStatus, createOrder, getAllActiveOrders, getAllOrders } from '../../services/orderService';
import ClickableOrderListItem from '../../components/List/ClickableOrderListItem';
import CallbackListItem from '../../components/List/CallbackListItem';

const OrderListPage: React.FC = () => {
    const [listItems, setListItems] = useState<Array<JSX.Element>>();
    const [splitPaymentItems, setSplitPaymentItems] = useState<string | undefined>(undefined);
    const [shouldSetSplit, setShouldSetSplit] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [showOnlyOpen, setShowOnlyOpen] = useState<boolean>(true);
    const { state } = useLocation();
    const navigate = useNavigate();
    const { authToken } = useAuth();

    const getActiveOrders = async () => {
        setError(null);
        if (splitPaymentItems) {
            await getOrders();
            return;
        }

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
            let { result, error } = await getAllOrders(authToken);

            if (!result) {
                setError('Problem acquiring all orders: ' + error);
            }
            else {
                if (splitPaymentItems) {
                    result = result.filter(o => splitPaymentItems.includes(o.Id));
                }

                setListItems(result.map(order =>
                    <ClickableOrderListItem orderType={order.Status} key={order.Id} stateContent={{ splitOrders: splitPaymentItems }} text={'Order  #' + order.Id} url={'/order/' + order.Id} />));
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const createNewOrder = async () => {
        setError(null);

        try {
            const { result, error } = await createOrder(authToken);

            if (!result) {
                setError('Problem creating order: ' + error);
            }
            else {
                navigate('/order/' + result.Id);
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

    const clearSplitOrder = () => {
        setSplitPaymentItems(undefined);
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
            if (state && state.splitOrders && shouldSetSplit) {
                setSplitPaymentItems(state.splitOrders);
                setShouldSetSplit(false);
            }
            getActiveOrders();
        }
        else {
            setError("You have to authenticate first!");
        }
    }, [splitPaymentItems]);

    return (
        <div>
            <header>
                <button onClick={goToBusiness}>Business</button>
            </header>
            <br/><br/>
            {(splitPaymentItems)
              ? <><button onClick={clearSplitOrder}>Exit split order view</button> <br/> <br/> </>
              : <>
                <button onClick={changeShowOnlyOpen}>{showOnlyOpen ? "Show all orders" : "Show only open orders"}</button>
                <h1>{showOnlyOpen ? "Current open orders" : "All orders"}</h1>
              </>
            }
                <div className="order-list-container">
                    {listItems}
                    { (!splitPaymentItems) &&
                    <div className="create-button-wrapper">
                        <CallbackListItem key="create" text="Create a new order" onClickHandle={createNewOrder} />
                    </div>
                    }
                </div>
            {error && <p className="error-message">{error}</p>}
        </div>
    );
};

export default OrderListPage;

