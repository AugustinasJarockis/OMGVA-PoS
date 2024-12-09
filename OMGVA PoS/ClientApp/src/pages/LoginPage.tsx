import React from 'react';
import LoginForm from '../components/Forms/LoginForm/index';

interface LoginPageProps {
    onLoginSuccess: () => void;
}

const LoginPage: React.FC<LoginPageProps> = ({ onLoginSuccess }) => {
    return (
        <div>
            <LoginForm onLoginSuccess={onLoginSuccess} />
        </div>
    );
};

export default LoginPage;
