import React, { useEffect, useState } from 'react';
import { useAuth } from '../../contexts/AuthContext';
import { CreateUserFrom } from '../../components/Forms/UserDataForm';
import { CreateUser, createUser } from '../../services/userService';
import { getTokenBusinessId } from '../../utils/tokenUtils';
import { useNavigate } from 'react-router-dom';

const CreateUserPage: React.FC = () => {
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();
    const { authToken } = useAuth();

    const handleSubmission = async (user: CreateUser) => {
        try {
            const error = await createUser(authToken, user);

            if (error) {
                setError("An error occurred while creating the user: " + error);
                return;
            }
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const goToBusiness = async () => {
        if (authToken) {
            const businessId = getTokenBusinessId(authToken);
            navigate('/business/' + businessId);
        }
    };

    return (
        <div>
            <header>
                <button onClick={goToBusiness}>Return to business</button>
            </header>
            <h1>Create User</h1>
            <CreateUserFrom onSubmitCreate={handleSubmission} token={authToken ?? ''} submitText="Create user" />
        </div>
    );
};

export default CreateUserPage;
