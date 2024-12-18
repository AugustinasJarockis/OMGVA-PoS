import axios from 'axios';
import { SimpleDiscount } from './discountService';

export interface OrderItem {
    Id: string,
    TotalPrice: number,
    UnitPriceNoDiscount: number,
    TaxPercent: string,
    ItemId: string,
    ItemName: string,
    Quantity: number,
    Discount?: SimpleDiscount,
    Variations?: Array<OrderItemVariation>
}

export interface OrderItemVariation {
    Id: string,
    ItemVariationId: string,
    ItemVariationName: string,
    ItemVariationGroup: string,
    PriceChange: number
}

export interface CreateOrderItemRequest {
    Quantity: number,
    ItemId: string,
    ItemVariationIds: Array<string>
}

export interface UpdateOrderItemRequest {
    Quantity: number
}

const createOrderItem = async (token: string | null, orderItem: CreateOrderItemRequest, orderId: string): Promise<string | undefined> => {
    try {
        const response = await axios.post(`/api/order/${orderId}/item`, orderItem, {
            headers: { Authorization: `Bearer ${token}` }
        });
        if (response.status === 204) {
            return undefined;
        } else {
            return response.data.message;
        }
    } catch (error: any) {
        return error.message || 'An unexpected error occurred.';
    }
};

export { createOrderItem };
