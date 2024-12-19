import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom';
import '../../index.css';
import { useAuth } from '../../contexts/AuthContext';
import '../../pages/Homepage.css';
import { Order, OrderStatus, UpdateOrderRequest, cancelOrder, getOrder, updateOrder } from '../../services/orderService';
import OrderItemListItem from '../../components/List/OrderItemListItem';
import { deleteOrderItem } from '../../services/orderItemService';
import PaymentModal from '../../components/Modals/PaymentModal';
import Swal from 'sweetalert2';
import { loadStripe } from '@stripe/stripe-js';
import { Elements } from '@stripe/react-stripe-js';
import axios from 'axios';
import SplitPaymentModal from "../../components/Modals/SplitPaymentModal.tsx";

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
    const [showSplitPayment, setShowSplitPayment] = useState<boolean>(false);

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
                        <OrderItemListItem key={item.Id} orderCurrency={result?.Currency} orderItem={item} orderId={String(id)} updateOrder={ loadOrder } orderStatus={result?.Status} onDelete={onDeleteOrderItem} />
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

            loadOrder();
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
            navigate(`/order/${id}/add-items/group`, { state: { currency: order?.Currency } });
        }
        else {
            navigate('/');
        }
    }

    const addOrderDiscount = () => {
        navigate(`/order/${id}/discount`);
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
                await loadOrder();
            }
            else {
                setError("Could not identify the order");
            }
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    };

    const cancelCurrentOrder = async () => {
        try {
            if (id) {
                const result = await cancelOrder(authToken, id);
                if (typeof result == "string") {
                    setError("An error occurred while cancelling order: " + result);
                    return;
                }
                setOrder(result);
                navigate('/order');
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
        navigate(`/order/${id}/refund`);
    }

    const finishOrder = async () => {
        setShowPayment(!showPayment);
    }

    const orderItemsForSplit = order?.OrderItems?.map(item => ({
        Id: item.Id,
        Name: item.ItemName,
        Quantity: item.Quantity,
        TotalPrice: item.TotalPrice
    })) ?? [];

    const onSplitSuccess = async (simpleOrders: any) => {
        // simpleOrders is the list of SimpleOrderDTO returned by the server
        Swal.fire('Order split successfully!', '', 'success');
        navigate('/order');
    };

    const splitOrder = async () => {
        setShowSplitPayment(true);
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
                    {(order.Status === OrderStatus.Open || order.Discount !== null) &&
                    <div className="tip-total-container">
                        <div className='discount-box'>
                            {(order.Status === OrderStatus.Open && order.Discount === null) ?
                            <button onClick={addOrderDiscount}>Add discount</button>
                            :   ((order.Status == OrderStatus.Open) 
                                ? <p>Discount: {order.Discount?.DiscountAmount}%</p> 
                                : (order.Discount && <p>Discount: {order.Discount?.DiscountAmount}%</p>)
                                )
                            }
                        </div>
                    </div>
                    }
                    <div className="tip-total-container">
                        <div className="tip-box">
                            <p>Tip</p>
                            {order.Status == OrderStatus.Open ? 
                                <input
                                    type="number"
                                    min="0"
                                    value={order.Tip}
                                    onInput={updateTip}
                                />  
                                : <p>{order.Tip}</p>
                            }
                            &nbsp;{order.Currency}
                        </div>
                        <div className="total-box">
                            <p>Total</p>
                            <span>{order.FinalPrice.toFixed(2)} {order.Currency}</span>
                        </div>
                    </div>
                    <br/><br/>
                    {order.Status == OrderStatus.Open && <button onClick={finishOrder}>Finish order</button>}
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    {order.Status == OrderStatus.Open && <button onClick={splitOrder}>Split payment</button>}
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    {order.Status == OrderStatus.Open && <button onClick={cancelCurrentOrder}>Cancel order</button>}
                    {order.Status == OrderStatus.Closed && <button onClick={refundOrder}>Refund order</button>}
                    {order.Status == OrderStatus.Refunded && <p className="nice-order-text">Order refunded. Reason: {order.RefundReason}</p>}
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
                        totalAmount={order?.FinalPrice ?? 0}
                        onPaymentSuccess={async () => {
                            Swal.fire('Payment successful!', '', 'success');
                            const result = await updateOrder(authToken as string, order?.Id.toString() ?? '', { Status: OrderStatus.Closed });
                            if (typeof result == "string") {
                                setError("An error occurred while finishing order payment: " + result);
                                return;
                            }
                            if (result)
                                setOrder(result);
                            loadOrder();
                            setShowPayment(false);
                        }}
                        onPaymentError={(errorMessage) => {
                            Swal.fire('Error', errorMessage, 'error');
                        }}
                    />
                </Elements>
            )}
            <SplitPaymentModal
                isOpen={showSplitPayment}
                onClose={() => setShowSplitPayment(false)}
                authToken={authToken as string}
                orderId={order?.Id.toString() ?? ''}
                orderItems={orderItemsForSplit}
                onSplitSuccess={onSplitSuccess}
            />
        </div>
    );
};

export default OrderPage;
