import React, { useState } from 'react';
import { OrderItem } from '../../../services/orderItemService';

interface OrderItemSelectorProps {
    currency: string;
  orderItems: Array<OrderItem>;
  onSelectionChange: (selectedItems: Array<string>) => void;
}

const OrderItemSelector: React.FC<OrderItemSelectorProps> = ({ currency, orderItems, onSelectionChange }) => {
  const [selectedItems, setSelectedItems] = useState<Array<string>>([]);

  const handleCheckboxChange = (itemId: string, isChecked: boolean) => {
    const updatedSelectedItems = isChecked
      ? [...selectedItems, itemId]
      : selectedItems.filter(id => id !== itemId);

    setSelectedItems(updatedSelectedItems);
    onSelectionChange(updatedSelectedItems);
  };

  return (
    <div className="order-item-selector">
      {orderItems.map(item => (
        <div key={item.Id} className="order-item">
          <label>
            <input
              type="checkbox"
              checked={selectedItems.includes(item.Id)}
              onChange={(e) => handleCheckboxChange(item.Id, e.target.checked)}
                  />
                  {item.ItemName} - {item.TotalPrice} {currency}
          </label>
        </div>
      ))}
    </div>
  );
};

export default OrderItemSelector;
