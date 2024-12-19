import { useState, useEffect } from 'react';
import './PaymentModal.css';
import { createPayment, Payment, createCardPayment } from '../../services/paymentService';
import { createCustomer } from '../../services/customerService'; // Import createCustomer function
import Swal from 'sweetalert2';
import { useStripe, useElements, CardElement } from '@stripe/react-stripe-js';

interface PaymentModalProps {
    isOpen: boolean;
    onClose: () => void;
    authToken: string;
    orderId: string;
    totalAmount: number;
    onPaymentSuccess: () => void;
    onPaymentError: (message: string) => void;
}

const PaymentModal: React.FC<PaymentModalProps> = ({
                                                       isOpen,
                                                       onClose,
                                                       authToken,
                                                       orderId,
                                                       totalAmount,
                                                       onPaymentSuccess,
                                                       onPaymentError
                                                   }) => {
    const [tipsField, setTipsField] = useState<string>('');
    const [discount, setDiscount] = useState<number>(0);
    const [customerName, setCustomerName] = useState<string>(''); // No longer using customerId state
    const [giftcardCode, setGiftcardCode] = useState<string>('');
    const [isCardMode, setIsCardMode] = useState<boolean>(false);

    const stripe = useStripe();
    const elements = useElements();

    useEffect(() => {
        if (isOpen) {
            setIsCardMode(false);
        }
    }, [isOpen]);

    if (!isOpen) return null;

    const showErrorSwal = (message: string) => {
        Swal.fire('Error', message, 'error');
    };

    const handlePaymentWithCustomer = async (method: string, customerId: number, code?: string) => {
        try {
            const payment: Payment = {
                Method: method,
                OrderId: parseInt(orderId),
                Amount: Math.round(totalAmount * 100),
                CustomerId: customerId, // Use the created customerId directly
            };

            if (code) {
                (payment as any).GiftcardCode = code;
            }

            if (method === 'cash' || method === 'giftcard') {
                const { result, error } = await createPayment(authToken, payment);
                if (error) {
                    onPaymentError("Payment failed: " + error);
                    showErrorSwal("Payment failed: " + error);
                } else {
                    onPaymentSuccess();
                }
                onClose();
            } else if (method === 'card') {
                if (!stripe || !elements) {
                    const errMsg = 'Stripe not initialized.';
                    onPaymentError(errMsg);
                    showErrorSwal(errMsg);
                    return;
                }

                const cardElement = elements.getElement(CardElement);
                if (!cardElement) {
                    const errMsg = 'Card element not found.';
                    onPaymentError(errMsg);
                    showErrorSwal(errMsg);
                    return;
                }

                const { paymentMethod, error } = await stripe.createPaymentMethod({
                    type: 'card',
                    card: cardElement,
                });

                if (error) {
                    const errMsg = `Payment failed: ${error.message}`;
                    onPaymentError(errMsg);
                    showErrorSwal(errMsg);
                    return;
                }

                if (!paymentMethod) {
                    const errMsg = 'No payment method returned.';
                    onPaymentError(errMsg);
                    showErrorSwal(errMsg);
                    return;
                }

                const { result, error: cardError } = await createCardPayment(authToken, payment, paymentMethod.id);
                if (cardError) {
                    const errMsg = "Payment failed: " + cardError;
                    onPaymentError(errMsg);
                    showErrorSwal(errMsg);
                } else {
                    onPaymentSuccess();
                }
                onClose();
            }

        } catch (err: any) {
            const errMsg = err.message || 'An unexpected error occurred during payment.';
            onPaymentError(errMsg);
            showErrorSwal(errMsg);
            onClose();
        }
    };

    const handlePayment = async (method: string, code?: string) => {
        // Ensure customer name is provided
        if (!customerName.trim()) {
            const errMsg = 'Customer name is required.';
            onPaymentError(errMsg);
            showErrorSwal(errMsg);
            return;
        }

        // Create customer
        const { result: customerResult, error: customerError } = await createCustomer(authToken, { Name: customerName });
        if (customerError) {
            const errMsg = "Failed to create customer: " + customerError;
            onPaymentError(errMsg);
            showErrorSwal(errMsg);
            return;
        }

        if (!customerResult || !customerResult.Id) {
            const errMsg = 'Failed to retrieve created customer ID.';
            onPaymentError(errMsg);
            showErrorSwal(errMsg);
            return;
        }

        const createdCustomerId = customerResult.Id;

        // Proceed with payment now that we have a customer
        await handlePaymentWithCustomer(method, createdCustomerId, code);
    };

    const handleGiftcardPayment = async () => {
        const { value: code } = await Swal.fire({
            title: "Enter your Giftcard Code",
            input: "text",
            inputAttributes: {
                autocapitalize: "off"
            },
            showCancelButton: true,
            confirmButtonText: "Apply",
            showLoaderOnConfirm: true,
            preConfirm: (inputCode) => {
                if (!inputCode) {
                    Swal.showValidationMessage("Please enter a valid giftcard code.");
                }
                return inputCode;
            },
            allowOutsideClick: () => !Swal.isLoading()
        });

        if (code) {
            setGiftcardCode(code);
            handlePayment('giftcard', code);
        }
    };

    const startCardPayment = () => {
        setIsCardMode(true);
    };

    const confirmCardPayment = async () => {
        await handlePayment('card');
    };

    return (
        <div className="modal-overlay">
            <div className="modal-content">
                <h2>Complete Payment</h2>
                {!isCardMode && (
                    <>
                        <div className="form-group">
                            <label>
                                Customer Name:
                                <input
                                    type="text"
                                    value={customerName}
                                    onChange={(e) => setCustomerName(e.target.value)}
                                    placeholder="Enter customer name"
                                />
                            </label>
                        </div>
                        <div className="form-group">
                            <label>
                                Tips to add:
                                <input
                                    type="text"
                                    value={tipsField}
                                    onChange={(e) => setTipsField(e.target.value)}
                                    placeholder="Unused"
                                    disabled
                                />
                            </label>
                        </div>
                        <div className="form-group">
                            <label>
                                Discount:
                                <input
                                    type="number"
                                    value={discount}
                                    onChange={(e) => setDiscount(parseFloat(e.target.value))}
                                    placeholder="Unused"
                                    disabled
                                />
                            </label>
                        </div>
                        <div className="form-group">
                            <label>
                                Total Amount: {totalAmount.toFixed(2)}
                            </label>
                        </div>
                        <div className="payment-buttons">
                            <button onClick={() => handlePayment('cash')}>Pay with Cash</button>
                            <button onClick={handleGiftcardPayment}>Pay with Giftcard</button>
                            <button onClick={startCardPayment}>Pay with Card</button>
                        </div>
                        <div className="modal-actions">
                            <button onClick={onClose}>Cancel</button>
                        </div>
                    </>
                )}
                {isCardMode && (
                    <>
                        <h3>Enter Card Details</h3>
                        <div className="card-element-container">
                            <CardElement />
                        </div>
                        <div className="modal-actions">
                            <button onClick={confirmCardPayment}>Confirm Card Payment</button>
                            <button onClick={onClose}>Cancel</button>
                        </div>
                    </>
                )}
            </div>
        </div>
    );
};

export default PaymentModal;
