import React, { useState } from 'react';
import './SplitPaymentModal.css';
import axios from 'axios';
import Swal from 'sweetalert2';

interface OrderItem {
    Id: number;
    Name: string;
    Quantity: number;
    TotalPrice: number;
}

interface SplitOrderItem {
    OrderItemId: number;
    Quantity: number;
}

interface SplitPaymentModalProps {
    isOpen: boolean;
    onClose: () => void;
    authToken: string;
    orderId: string;
    orderItems: OrderItem[];
    onSplitSuccess: (simpleOrders: any) => void;
}

const SplitPaymentModal: React.FC<SplitPaymentModalProps> = ({
                                                                 isOpen,
                                                                 onClose,
                                                                 authToken,
                                                                 orderId,
                                                                 orderItems,
                                                                 onSplitSuccess
                                                             }) => {
    const [selectedItems, setSelectedItems] = useState<{
        [itemId: number]: number;
    }>({});

    if (!isOpen) return null;

    const handleQuantityChange = (itemId: number, quantityString: string) => {
        const quantity = parseInt(quantityString, 10);

        if (isNaN(quantity) || quantity < 0) {
            // Invalid input: reset to 0
            setSelectedItems(prev => ({ ...prev, [itemId]: 0 }));
            return;
        }

        const item = orderItems.find(item => item.Id === itemId);
        if (!item) return;

        const maxAllowed = item.Quantity;

        if (quantity > maxAllowed) {
            Swal.fire(
                "Invalid Quantity",
                `You can only split up to ${maxAllowed} unit(s) for "${item.Name}".`,
                "warning"
            );
            setSelectedItems(prev => ({ ...prev, [itemId]: maxAllowed }));
            return;
        }

        if (quantity > 0) {
            setSelectedItems(prev => ({ ...prev, [itemId]: quantity }));
        } else {
            // If quantity is 0, remove the item from selection
            setSelectedItems(prev => {
                const updated = { ...prev };
                delete updated[itemId];
                return updated;
            });
        }
    };

    const handleSplitSubmit = async () => {
        // Build the splitOrderItems array
        const splitOrderItems: SplitOrderItem[] = Object.entries(selectedItems)
            .filter(([_, qty]) => qty > 0)
            .map(([itemIdString, qty]) => ({
                OrderItemId: parseInt(itemIdString, 10),
                Quantity: qty
            }));

        if (splitOrderItems.length === 0) {
            Swal.fire(
                "No Items Selected",
                "Please select at least one item and specify the quantity to split.",
                "warning"
            );
            return;
        }

        // Validate that all split quantities are less than the available quantities
        for (const splitItem of splitOrderItems) {
            const originalItem = orderItems.find(item => item.Id === splitItem.OrderItemId);
            if (!originalItem) {
                Swal.fire("Error", "Selected item not found.", "error");
                return;
            }
        }

        const payload = {
            SplitOrderItems: splitOrderItems
        };

        try {
            const response = await axios.post(`/api/order/${orderId}/split`, payload, {
                headers: { Authorization: `Bearer ${authToken}` }
            });

            if (response.status === 200) {
                onSplitSuccess(response.data);
                Swal.fire('Order Split Successfully!', '', 'success');
                onClose();
            } else {
                Swal.fire("Error", "An error occurred while splitting the order.", "error");
            }
        } catch (error: any) {
            Swal.fire(
                "Error",
                "An error occurred while splitting the order: " + (error.message || "Unknown error"),
                "error"
            );
        }
    };

    return (
        <div className="modal-overlay">
            <div className="modal-content split-payment-modal">
                <h2>Split Payment</h2>
                <p>Select the items and specify the quantity you want to split into a separate order:</p>
                <ul className="split-item-list">
                    {orderItems.map(item => (
                        <li key={item.Id}>
                            <span>{item.Name} (Available: {item.Quantity})</span>
                            <input
                                type="number"
                                min="1"
                                max={item.Quantity}
                                value={selectedItems[item.Id] || ''}
                                onChange={(e) => handleQuantityChange(item.Id, e.target.value)}
                                placeholder="0"
                            />
                        </li>
                    ))}
                </ul>
                <div className="modal-actions">
                    <button onClick={handleSplitSubmit}>Confirm Split</button>
                    <button onClick={onClose}>Cancel</button>
                </div>
            </div>
        </div>
    );
};

export default SplitPaymentModal;
