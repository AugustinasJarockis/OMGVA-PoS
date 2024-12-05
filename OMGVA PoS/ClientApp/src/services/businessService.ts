import axios from 'axios';

export interface Business {
    id: string,
    name: string,
    address: string,
    phone: string,
    email: string
}

const getAllBusinesses = async (): Promise<{ result?: Array<Business>, error?: string} > => {
    try {
        const response = await axios.get('/api/business');
        if (response.status === 200) {
            return { result: response.data };
        } else {
            return { error: response.data.message };
        }
    } catch (error: any) {
        return { error: error.message || 'An unexpected error occurred.' };
    }
};

export { getAllBusinesses };
