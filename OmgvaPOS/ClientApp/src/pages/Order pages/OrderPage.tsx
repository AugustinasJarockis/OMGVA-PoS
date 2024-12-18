import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom';
import '../../index.css';
import { useAuth } from '../../contexts/AuthContext';
import '../../pages/Homepage.css';
import { Order, OrderStatus, UpdateOrderRequest, getOrder, updateOrder } from '../../services/orderService';
import OrderItemListItem from '../../components/List/OrderItemListItem';
import { deleteOrderItem } from '../../services/orderItemService';

const OrderPage: React.FC = () => {
    const [listItems, setListItems] = useState<Array<JSX.Element>>();
    const [error, setError] = useState<string | null>(null);
    const [order, setOrder] = useState<Order>();
    const { state } = useLocation();
    const { id } = useParams();
    const navigate = useNavigate();
    const { authToken } = useAuth();

    const loadOrder = async () => {
        setError(null);

        try {
            if (id) {
                const { result, error } = await getOrder(authToken, id);
                if (error) {
                    setError(`An error occurred while fetching order with id ${id}: ` + error);
                    return;
                }
                setOrder(result);

                if (result?.OrderItems) {
                    setListItems(result?.OrderItems.map(item =>
                        <OrderItemListItem key={item.Id} orderItem={item} orderId={String(id)} orderStatus={result?.Status} onDelete={onDeleteOrderItem} />
                    ));
                }
            }
            else {
                setError("Could not identify the order");
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    };

    const onDeleteOrderItem = async (orderItemId: string) => {
        try {
            if (!id) {
                setError("Could not identify the order");
                return;
            }

            const error = await deleteOrderItem(authToken, id, orderItemId);

            if (error) {
                setError("An error occurred while deleting order item: " + error);
                return;
            }

            if (!listItems) {
                loadOrder();
            }

            if (listItems) {
                const newList = listItems.filter((item) => item.key != orderItemId);
                setListItems(newList);
            }
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const goToBusinessOrders = async () => {
        if (authToken) {
            navigate('/order');
        }
        else {
            navigate('/');
        }
    }

    const goToAddItems = async () => {
        if (authToken) {
            navigate(`/order/${id}/add-items/group`);
        }
        else {
            navigate('/');
        }
    }

    const updateTip = async (e: React.ChangeEvent<HTMLInputElement>) => {
        const request: UpdateOrderRequest = {
            Tip: +e.currentTarget.value
        }

        try {
            if (id) {
                const result = await updateOrder(authToken, id, request);
                if (typeof result == "string") {
                    setError("An error occurred while updating order: " + result);
                    return;
                }
                console.log(result);
                setOrder(result);
            }
            else {
                setError("Could not identify the order");
            }
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    };

    const cancelOrder = async () => {
        const request: UpdateOrderRequest = {
            Status: OrderStatus.Cancelled
        }

        console.log("working");

        try {
            if (id) {
                const result = await updateOrder(authToken, id, request);
                if (typeof result == "string") {
                    setError("An error occurred while updating order status: " + result);
                    return;
                }
            }
            else {
                setError("Could not identify the order");
            }
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const refundOrder = async () => {
        //TODO: Do stuff
    }

    const finishOrder = async () => {
        //TODO: Do stuff
    }

    useEffect(() => {
        if (authToken) {
            if (state && state.order) {
                setOrder(state.order);
            } else {
                loadOrder();
            }
        } else {
            setError("You have to authenticate first!");
        }
    }, []);

    return (
        error ||
        <div>
            <header>
                <nav className="nav-bar">
                    <ul className="nav-list">
                        <li>
                            <button onClick={goToBusinessOrders}>Business orders</button>
                        </li>
                    </ul>
                </nav>
            </header>

            {order ? (
                <>
                    <h1>{'Order #' + order.Id}</h1>
                    {order.Status == OrderStatus.Open && <button onClick={goToAddItems}>Add items</button>}
                    <br/><br/>
                    <section>
                        {listItems}
                    </section>
                    <br/><br/>
                    <div className="tip-total-container">
                        <div className="tip-box">
                            <p>Tip</p>
                            <input
                                type="number"
                                min="0"
                                value={order.Tip}
                                onInput={updateTip}
                            />
                        </div>
                        <div className="total-box">
                            <p>Total</p>
                            <span>{order.FinalPrice.toFixed(2)}</span>
                        </div>
                    </div>
                    <br/><br/>
                    {order.Status == OrderStatus.Open && <button onClick={finishOrder}>Finish order</button>}
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    {order.Status == OrderStatus.Open && <button onClick={cancelOrder}>Cancel order</button>}
                    {order.Status == OrderStatus.Closed && <button onClick={refundOrder}>Refund order</button>}
                </>
            ) : (
                <p className="error-message">{error}</p>
            )}
        </div>
    );
};

export default OrderPage;
