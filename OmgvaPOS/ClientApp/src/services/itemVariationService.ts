import axios from 'axios';

export interface ItemVariation {
    Id: string,
    ItemId: string,
    Name: string,
    InventoryQuantity: number,
    PriceChange: number,
    ItemVariationGroup: string
}

export interface CreateItemVariationRequest {
    Name: string,
    InventoryQuantity: number,
    PriceChange: number,
    ItemVariationGroup: string
}

export interface ChangeableItemVariationFields {
    Name?: string,
    InventoryQuantity?: number,
    PriceChange?: number,
    ItemVariationGroup?: string
}

const getAllItemVariations = async (token: string | null, itemId: string): Promise<{ result?: Array<ItemVariation>, error?: string }> => {
    try {
        const response = await axios.get(`/api/item-variation/item/${itemId}`, {
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

const getItemVariation = async (token: string | null, id: string): Promise<{ result?: ItemVariation, error?: string }> => {
    try {
        const response = await axios.get(`/api/item-variation/${id}`, {
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

const createItemVariation = async (token: string | null, request: CreateItemVariationRequest, itemId: string): Promise<{ error?: string, result?: ItemVariation }> => {
    try {
        const response = await axios.post(`/api/item-variation/${itemId}`, request, {
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

const updateItemVariation = async (token: string | null, id: string, item: ChangeableItemVariationFields): Promise<string | ItemVariation> => {
    try {
        const response = await axios.patch(`/api/item-variation/${id}`, item, {
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

const deleteItemVariation = async (token: string | null, id: string): Promise<string | undefined> => {
    try {
        const response = await axios.delete(`/api/item-variation/${id}`, {
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

export { getAllItemVariations, getItemVariation, createItemVariation, updateItemVariation, deleteItemVariation };
