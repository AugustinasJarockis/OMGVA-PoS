import React, { useState } from 'react';
import { login, loginWithNewToken } from '../../../services/authService';
import { useAuth } from '../../../contexts/AuthContext';
import { getTokenRole, getTokenUserId } from '../../../utils/tokenUtils';

interface LoginFormProps {
    onLoginSuccess: () => void;
}

const LoginForm: React.FC<LoginFormProps> = ({ onLoginSuccess }) => {
    const [username, setUsername] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [error, setError] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const { setAuthToken, authToken } = useAuth();

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);
        setIsLoading(true);

        const loginRequest = { username, password };

        try {
            const response = await login(loginRequest);
            if (response.IsSuccess && response.Token != null) {
                setAuthToken(response.Token);

                if (getTokenRole(response.Token) === "Admin") {
                    const id = getTokenUserId(authToken ?? "");
                    const { Token } = await loginWithNewToken(authToken, id);

                    if (Token) {
                        setAuthToken(Token);
                    }
                }
                onLoginSuccess();

            } else {
                setError(response.Message);
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="login-container">
            <h1>Login</h1>
            <form onSubmit={handleLogin} className="login-form">
                <div className="input-group">
                    <label htmlFor="username">Username</label>
                    <input
                        type="text"
                        id="username"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        required
                    />
                </div>
                <div className="input-group">
                    <label htmlFor="password">Password</label>
                    <input
                        type="password"
                        id="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>
                {error && <p className="error-message">{error}</p>}
                <button type="submit" disabled={isLoading}>
                    {isLoading ? 'Logging in...' : 'Login'}
                </button>
            </form>
        </div>
    );
};

export default LoginForm;
