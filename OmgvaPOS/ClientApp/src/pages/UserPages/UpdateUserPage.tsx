import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { UpdateUser, UserResponse, getUser, updateUser } from '../../services/userService';
import '../../index.css';
import { useAuth } from '../../contexts/AuthContext';
import { UserDataForm } from '../../components/Forms/UserDataForm';
import { getTokenUserId } from '../../utils/tokenUtils';

const UpdateUserPage: React.FC = () => {
    const [error, setError] = useState<string | null>(null);
    const [user, setUser] = useState<UserResponse>();
    const { id } = useParams();
    const navigate = useNavigate();
    const { authToken } = useAuth();

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
        if (id) {
            const { result, error } = await getUser(authToken, id);
            if (!result) {
                setError('Problem acquiring user: ' + error);
            }
            else {
                setUser(result);
            }
        }
    }

    const returnToUserDetailsOrBusiness = async () => {
        if (authToken) {
            if (getTokenUserId(authToken) === user?.Id) {
                navigate(`user/${id}`);
            } else {
                navigate(`/user/business`);
            }
        }
    }

    useEffect(() => {
        if (authToken && id) {
            acquireUser();
        }
    }, [authToken, id, error]);


    return (
        <div>
            <header>
                <button onClick={returnToUserDetailsOrBusiness}>Return</button>
            </header>
            {!error && user ? (
                <>
                    <h1>Update user</h1>
                    <UserDataForm onSubmitUpdate={handleSubmission} user={user} token={authToken ?? ''} submitText="Update user" />
                </>
            )
                : <p className="error-message">{error}</p>
            }
        </div>
    );
};

export default UpdateUserPage;
