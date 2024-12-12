import axios from 'axios';

export interface UserResponse {
    Id?: string;
    BusinessId?: string;
    Username?: string;
    Password?: string;
    Name?: string;
    Email?: string;
    Role?: number;
    HasLeft?: boolean;
}

export interface CreateUser {
    BusinessId?: string;
    Name: string;
    Username: string;
    Email: string;
    Role: string;
    Password: string;
}

export interface UpdateUser {
    Name?: string;
    Password?: string;
    Username?: string;
    Email?: string;
    Role?: string;
}

const createUser = async (token: string | null, user: CreateUser): Promise<{ error?: string, result?: UserResponse }> => {
    try {
        const response = await axios.post(`/api/user`, user, {
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

const getAllUsers = async (token: string | null): Promise<{ result?: Array<UserResponse>, error?: string }> => {
    try {
        const response = await axios.get('/api/user', {
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

const getUser = async (token: string | null, id: string): Promise<{ result?: UserResponse, error?: string }> => {
    try {
        const response = await axios.get(`/api/user/${id}`, {
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

const updateUser = async (token: string | null, id: string, user: UpdateUser): Promise<string | undefined> => {
    try {
        const response = await axios.patch(`/api/user/${id}`, user, {
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

const deleteUser = async (token: string | null, id: string): Promise<string | undefined> => {
    try {
        const response = await axios.delete(`/api/user/${id}`, {
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

const getBusinessUsers = async (token: string | null, businessId: string): Promise<{ result?: Array<UserResponse>, error?: string }> => {
    try {
        const response = await axios.get(`/api/user/business/${businessId}`, {
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

//no schedules or orders by now, will add later with reservations and orders

export { createUser, getAllUsers, getUser, updateUser, deleteUser, getBusinessUsers };
