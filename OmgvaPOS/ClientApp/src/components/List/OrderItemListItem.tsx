import React, { useEffect, useState } from 'react';
import { OrderItem, updateOrderItem } from '../../services/orderItemService';
import { useAuth } from '../../contexts/AuthContext';
import { OrderStatus, updateOrder } from '../../services/orderService';
import { Item, getItem } from '../../services/itemService';

interface OrderItemListItemProps {
    orderItem: OrderItem,
    orderStatus: OrderStatus,
    orderId: string,
    onDelete: (orderItemId: string) => void,
    updateOrder: () => void
}

const OrderItemListItem: React.FC<OrderItemListItemProps> = (props: OrderItemListItemProps) => {
    const [error, setError] = useState<string | null>(null);
    const { authToken } = useAuth();
    const [item, setItem] = useState<Item>();
    const [variationString, setVariationString] = useState<string>();
    const [longVariationString, setLongVariationString] = useState<string>();
    const [expanded, setExpanded] = useState<boolean>();
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
        e.stopPropagation();
        setSliderValue(+e.currentTarget.value);
    };

    const removeOrderItem = async (e) => {
        e.stopPropagation();
        props.onDelete(props.orderItem.Id);
    }

    const saveChangedItemAmount = async (e) => {
        e.stopPropagation();
        try {
            const result = await updateOrderItem(authToken, props.orderId, props.orderItem.Id, { Quantity: sliderValue });
            if (typeof result == "string") {
                setError("An error occurred while updating order item quantity: " + result);
                return;
            }
            props.updateOrder();
            setSavedSliderValue(sliderValue);
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const expandInfo = async () => {
        setExpanded(expanded ? false : true);
    }

    useEffect(() => {
        if (!authToken) {
            setError("You have to authenticate first!");
            return;
        }

        setSliderValue(props.orderItem.Quantity);
        let newVariationString = '(';
        if (props.orderItem.Variations) {
            for (const variation of props.orderItem.Variations) {
                newVariationString += variation.ItemVariationName + ', ';
            }
        }
        let longVariationsString;
        if (newVariationString.length > 1)
            longVariationsString = newVariationString.substring(0, newVariationString.length - 2) + ')';
        else
            longVariationsString = '()';
        setLongVariationString(longVariationsString);
        if (newVariationString.length >= 32) {
            newVariationString = newVariationString.substring(0, 30);
            newVariationString += '...';
        }
        else {
            if (newVariationString.length > 1)
                newVariationString = newVariationString.substring(0, newVariationString.length - 2);
        }
        newVariationString += ')';
        setVariationString(newVariationString);
        getSelectedItem();

        window.scrollTo(0, 0);
    }, []);
    //TODO: Currency
    return (
        (error) || <>
            <div className="order-item-list-item" onClick={ expandInfo }>
                <table>
                    <tr>
                        <td><b>{props.orderItem.ItemName} {variationString}</b></td>
                        <td>Price: {props.orderItem.TotalPrice} EUR</td>
                        {(props.orderStatus === OrderStatus.Open && item) ? <>
                            <td>Quantity:</td>
                            <td>
                                <input
                                    type="range"
                                    min='1'
                                    max={Math.min(props.orderItem.MaxQuantity, 65000)}
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
                            <td className="last-order-list-item-cell">
                                <button onClick={removeOrderItem}>X</button>
                                { (sliderValue !== savedSliderValue) &&
                                <button onClick={saveChangedItemAmount}>Save changes</button>
                                }  
                            </td>
                        </>
                        : <td>Quantity: {props.orderItem.Quantity}</td>}
                    </tr>
                </table>
            </div>
            {(expanded) && <div className="expanded-item-info">
                <table>
                    <tr>
                        <td>Item id: { props.orderItem.ItemId }</td>
                        <td>Item name: { props.orderItem.ItemName }</td>
                        <td>Tax percent: { props.orderItem.TaxPercent }%</td>
                        <td>Variations: {longVariationString}</td>
                    </tr>
                </table>
            </div>
            }
        </>
    );
};

export default OrderItemListItem;
