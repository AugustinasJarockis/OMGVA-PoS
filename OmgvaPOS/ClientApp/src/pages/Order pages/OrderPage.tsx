import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom';
import '../../index.css';
import { useAuth } from '../../contexts/AuthContext';
import '../../pages/Homepage.css';
import { Order, OrderStatus, getOrder } from '../../services/orderService';

const OrderPage: React.FC = () => {
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
                console.log(result);
            }
            else {
                setError("Could not identify the order");
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    };

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

    const cancelOrder = async () => {
        //TODO: Do stuff
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
                    <section>
                        {/*<p>Address: {order.Address}</p>*/}
                        {/*<p>Phone: {order.Phone}</p>*/}
                        {/*<p>Email: {order.Email}</p>*/}
                    </section>
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
