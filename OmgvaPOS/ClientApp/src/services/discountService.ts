import axios from 'axios';

export interface Discount {
    Id: string,
    Amount: string,
    TimeValidUntil: string,
    Type: DiscountType
}
export interface DiscountCreateRequest {
    Amount: number,
    TimeValidUntil: string,
    Type: DiscountType,
    OrderId?: string
}

export enum DiscountType {
    Item,
    Order
}

const getAllDiscounts = async (token: string | null): Promise<{ result?: Array<Discount>, error?: string }> => {
    try {
        const response = await axios.get('/api/discount', {
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

const getDiscount = async (token: string | null, id: string): Promise<{ result?: Discount, error?: string }> => {
    try {
        const response = await axios.get(`/api/discount/${id}`, {
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

const createDiscount = async (token: string | null, discount: DiscountCreateRequest): Promise<{ error?: string, result?: Discount }> => {
    try {
        const response = await axios.post(`/api/discount`, discount, {
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

const updateDiscountValidUntilTime = async (token: string | null, id: string, validUntil: string): Promise<string | undefined> => {
    try {
        console.log(validUntil);
        const response = await axios.patch(`/api/discount/${id}`, '"' + validUntil + '"', {
            headers: { Authorization: `Bearer ${token}`, "Content-Type": "application/json" }
        });
        if (response.status === 200) {
            return undefined;
        } else {
            return response.data.message;
        }
    } catch (error: any) {
        return error.message || 'An unexpected error occurred.';
    }
};

const deleteDiscount = async (token: string | null, id: string): Promise<string | undefined> => {
    try {
        const response = await axios.delete(`/api/discount/${id}`, {
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

export { getAllDiscounts, getDiscount, createDiscount, updateDiscountValidUntilTime, deleteDiscount };
