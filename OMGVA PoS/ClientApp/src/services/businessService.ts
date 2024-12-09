import axios from 'axios';

export interface Business {
    Id?: string,
    Name?: string,
    Address?: string,
    Phone?: string,
    Email?: string
}

const getAllBusinesses = async (token: string | null): Promise<{ result?: Array<Business>, error?: string} > => {
    try {
        const response = await axios.get('/api/business', {
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

const getBusiness = async (token: string | null, id: string): Promise<{ result?: Business, error?: string }> => {
    try {
        const response = await axios.get(`/api/business/${id}`, {
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

const createBusiness = async (token: string | null, business: Business): Promise<{ error?: string, result?: Business }> => {
    try {
        const response = await axios.post(`/api/business`, business, {
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

const updateBusiness = async (token: string | null, id: string, business: Business): Promise<string | undefined > => {
    try {
        const response = await axios.patch(`/api/business/${id}`, business, {
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

export { getAllBusinesses, getBusiness, createBusiness, updateBusiness };
