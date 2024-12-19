import { useState } from 'react';
import './PaymentModal.css';
import { createPayment, Payment } from '../../services/paymentService';
import Swal from 'sweetalert2';

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

    if (!isOpen) return null;

    const handlePayment = async (method: string, code?: string) => {
        try {
            const payment: Payment = {
                Method: method,
                OrderId: orderId,
                Amount: totalAmount * 100, // Convert to cents if needed by your API
                CustomerId: customerId,
            };

            // If you need to send the giftcard code along with the payment,
            // you can add it to the payment object if your API supports it.
            if (code) {
                (payment as any).GiftcardCode = code;
            }

            const { Payment: paymentResult, error } = await createPayment(authToken, payment);

            if (error) {
                onPaymentError("Payment failed: " + error);
            } else {
                onPaymentSuccess();
            }
        } catch (err: any) {
            onPaymentError(err.message || 'An unexpected error occurred during payment.');
        } finally {
            onClose();
        }
    };

    const handleGiftcardPayment = async () => {
        // Show Swal prompt for giftcard code
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
                return inputCode; // returning the input code if valid
            },
            allowOutsideClick: () => !Swal.isLoading()
        });

        if (code) {
            // Set the giftcard code and then call the handlePayment with giftcard
            setGiftcardCode(code);
            handlePayment('giftcard', code);
        }
    };

    return (
        <div className="modal-overlay">
            <div className="modal-content">
                <h2>Complete Payment</h2>
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
                    <button onClick={() => handlePayment('card')}>Pay with Card</button>
                </div>
                <div className="modal-actions">
                    <button onClick={onClose}>Cancel</button>
                </div>
            </div>
        </div>
    );
};

export default PaymentModal;
