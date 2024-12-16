import axios from 'axios';
import { Tax } from './taxService';

export interface Item {
    Id: string,
    Name: string,
    InventoryQuantity: number,
    Price: number,
    Currency: string,
    ItemGroup: string,
    Duration?: string,
    ImgPath: string,
    DiscountId?: string,
    UserId?: string
}

export interface CreateItemRequest {
    Name: string
    InventoryQuantity: number
    Price: number
    Currency: string
    ItemGroup: string
    Duration?: string
    ImgPath: string
    DiscountId?: string
    UserId?: string
}

export interface ChangeableItemFields {
    Name?: string,
    InventoryQuantity?: number,
    Price?: number,
    Currency?: string,
    ItemGroup?: string,
    Duration?: string,
    ImgPath?: string,
    DiscountId?: string,
    UserId?: string
}

export interface ChangeItemTaxesRequest {
    TaxesToAddIds: Array<string>;
    TaxesToRemoveIds: Array<string>;
}

const getAllItems = async (token: string | null): Promise<{ result?: Array<Item>, error?: string }> => {
    try {
        const response = await axios.get('/api/item', {
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

const getItem = async (token: string | null, id: string): Promise<{ result?: Item, error?: string }> => {
    try {
        const response = await axios.get(`/api/item/${id}`, {
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

const createItem = async (token: string | null, item: CreateItemRequest): Promise<{ error?: string, result?: Item }> => {
    try {
        const response = await axios.post(`/api/item`, item, {
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

const updateItem = async (token: string | null, id: string, item: ChangeableItemFields): Promise<string | Item> => {
    try {
        const response = await axios.patch(`/api/item/${id}`, item, {
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

const deleteItem = async (token: string | null, id: string): Promise<string | undefined> => {
    try {
        const response = await axios.delete(`/api/item/${id}`, {
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

const getItemTaxes = async (token: string | null, id: string): Promise<{ error?: string, result?: Array<Tax> }> => {
    try {
        const response = await axios.get(`/api/item/${id}/taxes`, {
            headers: { Authorization: `Bearer ${token}` }
        });
        if (response.status === 200) {
            return { result: response.data };
        } else {
            return { error: response.data.message };
        }
    } catch (error: any) {
        return { error: error.message || 'An unexpected error occurred.' };
    }
}

const changeItemTaxes = async (token: string | null, id: string, changes: ChangeItemTaxesRequest): Promise<{ error?: string, result?: Item }> => {
    try {
        const response = await axios.post(`/api/item/${id}/taxes`, changes, {
            headers: { Authorization: `Bearer ${token}` }
        });
        if (response.status === 200) {
            return { result: response.data };
        } else {
            return { error: response.data.message };
        }
    } catch (error: any) {
        return { error: error.message || 'An unexpected error occurred.' };
    }
}

export { getAllItems, getItem, createItem, updateItem, deleteItem, getItemTaxes, changeItemTaxes };
