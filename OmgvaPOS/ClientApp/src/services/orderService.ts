import axios from 'axios';
import { SimpleDiscount } from './discountService';
import { SimpleUser } from './userService';
import { OrderItem } from './orderItemService';

export interface Order {
    Id: string,
    Status: OrderStatus,
    Currency: string,
    Tip: number,
    RefundReason?: string,
    FinalPrice: number,
    TaxesPaid: number,
    Discount?: SimpleDiscount,
    User: SimpleUser,
    OrderItems: Array<OrderItem>
}

export interface UpdateOrderRequest {
    Status?: OrderStatus, 
    Tip?: number,
    RefundReason?: string,
    UserId?: string
}

export interface RefundOrderRequest {
    RefundReason: string
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

const getOrder = async (token: string | null, id: string): Promise<{ result?: Order, error?: string }> => {
    try {
        const response = await axios.get(`/api/order/${id}`, {
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

const createOrder = async (token: string | null): Promise<{ error?: string, result?: Order }> => {
    try {
        const response = await axios.post(`/api/order`, {}, {
            headers: { Authorization: `Bearer ${token}` }
        });
        if (response.status === 201) {
            return { result: response.data };
        } else {
            return { error: response.data.message };
        }
    } catch (error: any) {
        return { error: error.message || 'An unexpected error occurred.' };
    }
};

const updateOrder = async (token: string | null, id: string, order: UpdateOrderRequest): Promise<string | Order> => {
    try {
        const response = await axios.patch(`/api/order/${id}`, order, {
            headers: { Authorization: `Bearer ${token}` }
        });
        if (response.status === 200) {
            return response.data;
        } else {
            return response.data.message;
        }
    } catch (error: any) {
        return error.message || 'An unexpected error occurred.';
    }
};

const cancelOrder = async (token: string | null, id: string): Promise<string | Order> => {
    try {
        const response = await axios.post(`/api/order/${id}/cancel`, {}, {
            headers: { Authorization: `Bearer ${token}` }
        });
        if (response.status === 200) {
            return response.data;
        } else {
            return response.data.message;
        }
    } catch (error: any) {
        return error.message || 'An unexpected error occurred.';
    }
};

const refundOrder = async (token: string | null, id: string, request: RefundOrderRequest): Promise<string | Order> => {
    try {
        const response = await axios.post(`/api/order/${id}/refund`, request, {
            headers: { Authorization: `Bearer ${token}` }
        });
        if (response.status === 200) {
            return response.data;
        } else {
            return response.data.message;
        }
    } catch (error: any) {
        return error.message || 'An unexpected error occurred.';
    }
};

export { getAllActiveOrders, getAllOrders, getOrder, createOrder, updateOrder, cancelOrder, refundOrder };
