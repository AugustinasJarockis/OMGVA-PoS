import axios from "axios";

export interface Giftcard {
    Id: string;
    BusinessId: string;
    Code: string;
    Value: number;
    Balance: number;
    GiftcardPayments: null; //keep if needed
}

export interface CreateGiftcard {
    BusinessId: string;
    Value: number;
}
export interface UpdateGiftcard {
    Code: string;
    Amount: number;
}

const createGiftcard = async (token: string | null, giftcard: CreateGiftcard): Promise<{ error?: string, result?: Giftcard }> => {
    try {
        const response = await axios.post(`/api/giftcard`, giftcard, {
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

const getGiftcard = async (token: string | null, id: string): Promise<{ result?: Giftcard, error?: string }> => {
    try {
        const response = await axios.get(`/api/giftcard/${id}`, {
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

const updateUser = async (token: string | null, giftcard: UpdateGiftcard): Promise<string | undefined> => {
    try {
        const response = await axios.patch(`/api/giftcard`, giftcard, {
            headers: { Authorization: `Bearer ${token}` },
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

const getBusinessGiftcards = async (token: string | null): Promise<{ result?: Array<Giftcard>, error?: string }> => {
    try {
        const response = await axios.get(`/api/giftcard`, {
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

export { createGiftcard, getGiftcard, updateUser, getBusinessGiftcards }