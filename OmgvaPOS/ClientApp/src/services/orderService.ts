import axios from 'axios';
import { SimpleDiscount } from './discountService';
import { SimpleUser } from './userService';
import { OrderItem } from './orderItemService';

export interface Order {
    Id: string,
    Status: OrderStatus,
    Tip: number,
    RefundReason?: string,
    FinalPrice: number,
    TaxesPaid: number,
    Discount?: SimpleDiscount,
    User: SimpleUser,
    OrderItems: Array<OrderItem>
}

export enum OrderStatus {
    Open,
    Closed,
    Cancelled,
    Refunded
}

const getAllOrders = async (token: string | null): Promise<{ result?: Array<Order>, error?: string }> => {
    try {
        const response = await axios.get('/api/order', {
            headers: { Authorization: `Bearer ${token}` },
        });
        if (response.status === 200) {
            return { result: response.data };
        } else {
            return { error: response.data.message };
        }
    } catch (error: any) {
        return { error: error.message || 'An unexpected error occurred.' };
    }
};

const getAllActiveOrders = async (token: string | null): Promise<{ result?: Array<Order>, error?: string }> => {
    try {
        const response = await axios.get('/api/order/active', {
            headers: { Authorization: `Bearer ${token}` },
        });
        if (response.status === 200) {
            return { result: response.data };
        } else {
            return { error: response.data.message };
        }
    } catch (error: any) {
        return { error: error.message || 'An unexpected error occurred.' };
    }
};

export { getAllActiveOrders, getAllOrders };