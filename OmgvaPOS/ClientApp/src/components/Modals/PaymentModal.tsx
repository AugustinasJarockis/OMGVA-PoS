import { useState } from 'react';
import './PaymentModal.css';

interface PaymentModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSubmit: (paymentMethod: string, customerId: number) => void;
    totalAmount: number;
}

const PaymentModal: React.FC<PaymentModalProps> = ({ isOpen, onClose, onSubmit, totalAmount }) => {
    const [tipsField, setTipsField] = useState<string>('');
    const [discount, setDiscount] = useState<number>(0);
    const [customerId, setCustomerId] = useState<number>(0);

    if (!isOpen) return null;

    const handlePayment = (method: string, customerId: number) => {
        onSubmit(method, customerId);
        onClose();
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
                            disabled // Currently unused
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
                    <button onClick={() => handlePayment('cash', customerId)}>Pay with Cash</button>
                    <button onClick={() => handlePayment('giftcard', customerId)}>Pay with Giftcard</button>
                    <button onClick={() => handlePayment('card', customerId)}>Pay with Card</button>
                </div>
                <div className="modal-actions">
                    <button onClick={onClose}>Cancel</button>
                </div>
            </div>
        </div>
    );
};

export default PaymentModal;