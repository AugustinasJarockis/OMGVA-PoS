import { useState, useEffect } from 'react';
import './PaymentModal.css';
import { createPayment, Payment, createCardPayment } from '../../services/paymentService';
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
    const [customerId, setCustomerId] = useState<number>(0);
    const [giftcardCode, setGiftcardCode] = useState<string>('');
    const [isCardMode, setIsCardMode] = useState<boolean>(false);

    const stripe = useStripe();
    const elements = useElements();

    // Reset isCardMode whenever the modal opens
    useEffect(() => {
        if (isOpen) {
            setIsCardMode(false);
        }
    }, [isOpen]);

    if (!isOpen) return null;

    const handlePayment = async (method: string, code?: string) => {
        try {
            const payment: Payment = {
                Method: method,
                OrderId: parseInt(orderId),
                Amount: Math.round(totalAmount * 100),
                CustomerId: customerId,
            };

            if (code) {
                (payment as any).GiftcardCode = code;
            }

            if (method === 'cash' || method === 'giftcard') {
                const { result, error } = await createPayment(authToken, payment);
                if (error) {
                    onPaymentError("Payment failed: " + error);
                } else {
                    onPaymentSuccess();
                }
                onClose();
            } else if (method === 'card') {
                if (!stripe || !elements) {
                    onPaymentError('Stripe not initialized.');
                    return;
                }

                const cardElement = elements.getElement(CardElement);
                if (!cardElement) {
                    onPaymentError('Card element not found.');
                    return;
                }

                const { paymentMethod, error } = await stripe.createPaymentMethod({
                    type: 'card',
                    card: cardElement,
                });

                if (error) {
                    onPaymentError(`Payment failed: ${error.message}`);
                    return;
                }

                if (!paymentMethod) {
                    onPaymentError('No payment method returned.');
                    return;
                }

                const { result, error: cardError } = await createCardPayment(authToken, payment, paymentMethod.id);
                if (cardError) {
                    onPaymentError("Payment failed: " + cardError);
                } else {
                    onPaymentSuccess();
                }
                onClose();
            }

        } catch (err: any) {
            onPaymentError(err.message || 'An unexpected error occurred during payment.');
            onClose();
        }
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
                                Customer ID:
                                <input
                                    type="number"
                                    value={customerId}
                                    onChange={(e) => setCustomerId(parseFloat(e.target.value))}
                                    placeholder="Enter customer ID"
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
