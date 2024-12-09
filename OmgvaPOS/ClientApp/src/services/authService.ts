import axios from 'axios';

export interface LoginRequest {
    username: string;
    password: string;
}

export interface LoginResponse {
    isSuccess: boolean;
    message: string;
    token?: string;
}

const login = async (loginRequest: LoginRequest): Promise<LoginResponse> => {
    try {
        const response = await axios.post('/api/user/login', loginRequest);
        if (response.status === 200) {
            return { isSuccess: true, message: response.data.message, token: response.data.token };
        } else {
            return { isSuccess: false, message: response.data.message };
        }
    } catch (error: any) {
        return { isSuccess: false, message: error.message || 'An unexpected error occurred.' };
    }
};

export { login };
