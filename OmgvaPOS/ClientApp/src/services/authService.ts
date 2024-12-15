import axios from 'axios';

export interface LoginRequest {
    Username: string;
    Password: string;
}

export interface LoginResponse {
    IsSuccess: boolean;
    Message: string;
    Token?: string;
}

const login = async (loginRequest: LoginRequest): Promise<LoginResponse> => {
    try {
        const response = await axios.post('/api/auth/login', loginRequest);
        if (response.status === 200) {
            return { IsSuccess: true, Message: response.data.Message, Token: response.data.Token };
        } else {
            return { IsSuccess: false, Message: response.data.Message };
        }
    } catch (error: any) {
        return { IsSuccess: false, Message: error.message || 'An unexpected error occurred.' };
    }
};


const loginWithNewToken = async (token: string | null, businessId: string): Promise<LoginResponse> => {
    try {
        const response = await axios.get(`/api/auth/login/${businessId}`, {
            headers: { Authorization: `Bearer ${token}` }
        });
        if (response.status === 200) {
            return { IsSuccess: true, Message: response.data.message, Token: response.data.Token };
        } else {
            return { IsSuccess: false, Message: response.data.message };
        }
    } catch (error: any) {
        return { IsSuccess: false, Message: error.message || 'An unexpected error occurred.' };
    }
};

export { login, loginWithNewToken };
