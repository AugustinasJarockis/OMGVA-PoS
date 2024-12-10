import axios from 'axios';

export interface Tax {
    id: string,
    taxType: string,
    percent: string,
}
export interface TaxUpdateRequest {
    taxType?: string,
    percent?: string,
}
export interface TaxCreateRequest {
    taxType: string,
    percent: number
}

const getAllTaxes = async (token: string | null): Promise<{ result?: Array<Tax>, error?: string }> => {
    try {
        const response = await axios.get('/api/tax', {
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

const getTax = async (token: string | null, id: string): Promise<{ result?: Tax, error?: string }> => {
    try {
        const response = await axios.get(`/api/tax/${id}`, {
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

const createTax = async (token: string | null, tax: TaxCreateRequest): Promise<{ error?: string, result?: Tax }> => {
    try {
        const response = await axios.post(`/api/tax`, tax, {
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

const updateTax = async (token: string | null, id: string, tax: TaxUpdateRequest): Promise<string | undefined> => {
    try {
        const response = await axios.patch(`/api/tax/${id}`, tax, {
            headers: { Authorization: `Bearer ${token}` }
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

const deleteTax = async (token: string | null, id: string): Promise<string | undefined> => {
    try {
        const response = await axios.delete(`/api/tax/${id}`, {
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

export { getAllTaxes, getTax, createTax, updateTax, deleteTax };
