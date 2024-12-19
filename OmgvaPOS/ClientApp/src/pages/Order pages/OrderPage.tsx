import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom';
import '../../index.css';
import { useAuth } from '../../contexts/AuthContext';
import '../../pages/Homepage.css';
import { Order, OrderStatus, getOrder } from '../../services/orderService';
import OrderItemListItem from '../../components/List/OrderItemListItem';
import { deleteOrderItem } from '../../services/orderItemService';
import PaymentModal from '../../components/Modals/PaymentModal';
import Swal from 'sweetalert2';
import { loadStripe } from '@stripe/stripe-js';
import { Elements } from '@stripe/react-stripe-js';
import axios from 'axios';

const OrderPage: React.FC = () => {
    const [listItems, setListItems] = useState<Array<JSX.Element>>();
    const [error, setError] = useState<string | null>(null);
    const [order, setOrder] = useState<Order>();
    const { state } = useLocation();
    const { id } = useParams();
    const navigate = useNavigate();
    const { authToken } = useAuth();
    const [showPayment, setShowPayment] = useState<boolean>(false);
    const [stripePromise, setStripePromise] = useState<any>(null);

    const loadStripeKey = async () => {
        try {
            const response = await axios.get('/api/payment/stripe-publish-key', {
                headers: { Authorization: `Bearer ${authToken}` },
            });
            if (response.status === 200 && response.data.publishKey) {
                setStripePromise(loadStripe(response.data.publishKey));
            } else {
                setError("Failed to load stripe publish key");
            }
        } catch (e: any) {
            setError(e.message);
        }
    }

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

    const cancelOrder = async () => {
        //TODO: Do stuff
    }

    const refundOrder = async () => {
        //TODO: Do stuff
    }

    const finishOrder = async () => {
        setShowPayment(!showPayment);
    }

    useEffect(() => {
        if (authToken) {
            loadStripeKey(); // Load stripe key as soon as we know we have auth
            if (state && state.order) {
                setOrder(state.order);
            } else {
                loadOrder();
            }
        } else {
            setError("You have to authenticate first!");
        }
    }, []);

    const calculateTotalAmount = (): number => {
        if (!order || !order.OrderItems) return 0;
        return order.OrderItems.reduce((total, item) => total + (item.TotalPrice * item.Quantity), 0);
    }

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
                    {order.Status == OrderStatus.Open && <button onClick={finishOrder}>Finish order</button>}
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    {order.Status == OrderStatus.Open && <button onClick={cancelOrder}>Cancel order</button>}
                    {order.Status == OrderStatus.Closed && <button onClick={refundOrder}>Refund order</button>}
                </>
            ) : (
                <p className="error-message">{error}</p>
            )}
            {stripePromise && (
                <Elements stripe={stripePromise}>
                    <PaymentModal
                        isOpen={showPayment}
                        onClose={() => setShowPayment(false)}
                        authToken={authToken as string}
                        orderId={order?.Id.toString() ?? ''}
                        totalAmount={calculateTotalAmount()}
                        onPaymentSuccess={() => {
                            Swal.fire('Payment successful!', '', 'success');
                            setShowPayment(false);
                        }}
                        onPaymentError={(errorMessage) => setError(errorMessage)}
                    />
                </Elements>
            )}
        </div>
    );
};

export default OrderPage;
