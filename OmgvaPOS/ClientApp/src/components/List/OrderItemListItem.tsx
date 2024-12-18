import React, { useEffect, useState } from 'react';
import { OrderItem, updateOrderItem } from '../../services/orderItemService';
import { useAuth } from '../../contexts/AuthContext';
import { OrderStatus } from '../../services/orderService';
import { Item, getItem } from '../../services/itemService';

interface OrderItemListItemProps {
    orderItem: OrderItem,
    orderStatus: OrderStatus,
    orderId: string,
    onDelete: (orderItemId: string) => void
}

const OrderItemListItem: React.FC<OrderItemListItemProps> = (props: OrderItemListItemProps) => {
    const [error, setError] = useState<string | null>(null);
    const { authToken } = useAuth();
    const [item, setItem] = useState<Item>();
    const [sliderValue, setSliderValue] = useState<number>(1); 
    const [savedSliderValue, setSavedSliderValue] = useState<number>(1); 

    const getSelectedItem = async () => {
        setError(null);

        try {
            const { result, error } = await getItem(authToken, props.orderItem.ItemId);
            if (error) {
                setError(`An error occurred while fetching item with id ${props.orderItem.ItemId}: ` + error);
                return;
            }
            if (result?.Duration) {
                const splitRes = result.Duration.split(':');
                result.Duration = splitRes[0] + ':' + splitRes[1];
            }
            setItem(result);
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const updateSliderValue = (e: React.ChangeEvent<HTMLInputElement>) => {
        setSliderValue(+e.currentTarget.value);
    };

    const removeOrderItem = async () => {
        props.onDelete(props.orderItem.Id);
    }

    const saveChangedItemAmount = async () => {
        try {
            const result = await updateOrderItem(authToken, props.orderId, props.orderItem.Id, { Quantity: sliderValue });
            if (typeof result == "string") {
                setError("An error occurred while updating order item quantity: " + result);
                return;
            }
            setSavedSliderValue(sliderValue);
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    useEffect(() => {
        if (!authToken) {
            setError("You have to authenticate first!");
            return;
        }

        setSliderValue(props.orderItem.Quantity);
        getSelectedItem();

        window.scrollTo(0, 0);
    }, []);
    //TODO: Currency
    //TODO: Max value
    //TODO: fix css
    //TODO: show variations
    return (
        (error) ||
        <div className="order-item-list-item">
            <table>
                <tr>
                    <td><b>{props.orderItem.ItemName}</b></td>
                    <td>Price: {props.orderItem.TotalPrice} EUR</td>
                    <td>Quantity:</td>
                    {(props.orderStatus === OrderStatus.Open && item) && <>
                        <td>
                            <input
                                type="range"
                                min='1'
                                max={Math.min(item?.InventoryQuantity, 65000)}
                                value={sliderValue}
                                step='1'
                                onInput={updateSliderValue}
                            />
                        </td>
                        <td>
                            <input
                                type="number"
                                min='1'
                                max={Math.min(item?.InventoryQuantity, 65000)}
                                value={sliderValue}
                                step='1'
                                onInput={updateSliderValue}
                            />
                        </td>
                        { (sliderValue !== savedSliderValue) &&
                        <td>
                            <button onClick={saveChangedItemAmount}>Save changes</button>
                        </td>
                        }
                        <td>
                            <button onClick={removeOrderItem}>X</button>
                        </td>
                    </>}
                </tr>
            </table>
        </div>
    );
};

export default OrderItemListItem;
