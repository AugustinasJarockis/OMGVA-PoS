import axios from 'axios';

export interface Customer {
    Id?: number;
    Name?: string;
}

export interface CreateCustomerRequest {
    Name: string;
}

export interface UpdateCustomerRequest {
    Name: string;
}

/**
 * Create a new customer.
 * Returns { result?: Customer, error?: string }
 */
export const createCustomer = async (token: string | null, request: CreateCustomerRequest): Promise<{ result?: Customer, error?: string }> => {
    try {
        const response = await axios.post('/api/customers', request, {
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

/**
 * Get a customer by ID.
 * Returns { result?: Customer, error?: string }
 */
export const getCustomer = async (token: string | null, customerId: number): Promise<{ result?: Customer, error?: string }> => {
    try {
        const response = await axios.get(`/api/customers/${customerId}`, {
            headers: { Authorization: `Bearer ${token}` },
        });
        if (response.status === 200) {
            return { result: response.data };
        } else {
            return { error: response.data.message };
        }
    } catch (error: any) {
        if (error.response && error.response.status === 404) {
            return { error: `Customer with ID ${customerId} not found.` };
        }
        return { error: error.message || 'An unexpected error occurred.' };
    }
};

/**
 * Update a customer by ID.
 * Returns { result?: Customer, error?: string }
 */
export const updateCustomer = async (token: string | null, customerId: number, request: UpdateCustomerRequest): Promise<{ result?: Customer, error?: string }> => {
    try {
        const response = await axios.put(`/api/customers/${customerId}`, request, {
            headers: { Authorization: `Bearer ${token}` }
        });
        if (response.status === 200) {
            return { result: response.data };
        } else {
            return { error: response.data.message };
        }
    } catch (error: any) {
        if (error.response && error.response.status === 404) {
            return { error: `Customer with ID ${customerId} not found.` };
        }
        return { error: error.message || 'An unexpected error occurred.' };
    }
};

/**
 * Delete a customer by ID.
 * Returns { error?: string } - no result on success since it's NoContent (204)
 */
export const deleteCustomer = async (token: string | null, customerId: number): Promise<{ error?: string }> => {
    try {
        const response = await axios.delete(`/api/customers/${customerId}`, {
            headers: { Authorization: `Bearer ${token}` }
        });
        if (response.status === 204) {
            return {};
        } else {
            return { error: response.data.message };
        }
    } catch (error: any) {
        if (error.response && error.response.status === 404) {
            return { error: `Customer with ID ${customerId} not found.` };
        }
        return { error: error.message || 'An unexpected error occurred.' };
    }
};

/**
 * Get all customers for a specific business.
 * Returns { result?: Array<Customer>, error?: string }
 */
export const getBusinessCustomers = async (token: string | null, businessId: number): Promise<{ result?: Array<Customer>, error?: string }> => {
    try {
        const response = await axios.get(`/api/customers/business/${businessId}`, {
            headers: { Authorization: `Bearer ${token}` },
        });
        if (response.status === 200) {
            return { result: response.data };
        } else {
            return { error: response.data.message };
        }
    } catch (error: any) {
        if (error.response && error.response.status === 404) {
            return { error: `No customers found for business ID ${businessId}.` };
        }
        return { error: error.message || 'An unexpected error occurred.' };
    }
};
