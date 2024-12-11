﻿import React, { useState, useEffect } from 'react';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import { UpdateUser, UserResponse, getUser, updateUser } from '../../services/userService';
import { getTokenRole } from '../../utils/tokenUtils';
import UserDataForm from '../../components/Forms/UserDataForm';

interface UpdateUserPageProps {
    token: string | null;
}

const UpdateTaxPage: React.FC<UpdateUserPageProps> = ({ token: authToken }) => {
    const [error, setError] = useState<string | null>(null);
    const [user, setUser] = useState<UserResponse>();
    const { state } = useLocation();
    const { id } = useParams();
    const navigate = useNavigate();

    const handleSubmission = async (user: UpdateUser) => {
        try {
            if (id) {
                const error = await updateUser(authToken, id, user);

                if (error) {
                    setError("An error occurred while updating the user: " + error);
                    return;
                }

                navigate(`/user/${id}`);
            }
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const acquireUser = async () => {
        if (checkId() && id) {
            const { result, error } = await getUser(authToken, id);
            if (!result) {
                setError('Problem acquiring tax: ' + error);
            }
            else {
                setUser(result);
            }
        }
    }

    const returnToUserDetails = async () => {
        navigate("/user");
    }

    const userRole = (): string | undefined => {
        if (authToken) {
            const role = getTokenRole(authToken);
            return role;
        }
    }

    useEffect(() => {
        if (state) {
            setUser(state.object);
        }
        else {
            acquireUser();
            if (!user) {
                return;
            }
        }
    }, []);

    return (
        <div>
            <header>
                <button onClick={returnToUserDetails}>Return</button>
            </header>
            {!error && user ? (
                <>
                    <h1>Update tax</h1>
                    <UserDataForm onSubmit={handleSubmission} user={user} roleFromToken={userRole()} submitText="Update user" />
                </>
            )
                : <p className="error-message">{error}</p>
            }
        </div>
    );
};

export default UpdateTaxPage;