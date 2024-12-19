import React from 'react';
import { useAuth } from '../../contexts/AuthContext';
import { CreateUserFrom } from '../../components/Forms/UserDataForm'; // Ensure this is the correct component name
import { CreateUser, createUser } from '../../services/userService';
import { getTokenBusinessId } from '../../utils/tokenUtils';
import { useNavigate } from 'react-router-dom';
import Swal from 'sweetalert2';

const CreateUserPage: React.FC = () => {
    const navigate = useNavigate();
    const { authToken } = useAuth();

    /**
     * Handles the submission of the user creation form.
     * Displays appropriate Swal alerts based on the outcome and redirects on success.
     *
     * @param user - The user data to be created.
     */
    const handleSubmission = async (user: CreateUser) => {
        try {
            // Attempt to create the user
            await createUser(authToken, user);

            // If successful, display a success alert
            await Swal.fire({
                icon: 'success',
                title: 'Success',
                text: 'User created successfully!',
                confirmButtonText: 'OK'
            });

            // Redirect to the business page after successful creation
            goToBusiness();
        }
        catch (err: any) {
            // Handle errors by displaying an error alert
            await Swal.fire({
                icon: 'error',
                title: 'Error',
                text: err.message || 'An unexpected error occurred.',
                confirmButtonText: 'OK'
            });
        }
    };

    /**
     * Redirects the user to their business page based on the business ID extracted from the token.
     */
    const goToBusiness = () => {
        if (authToken) {
            const businessId = getTokenBusinessId(authToken);
            navigate('/business/' + businessId);
        } else {
            // Handle the case where authToken is not available
            Swal.fire({
                icon: 'error',
                title: 'Authentication Error',
                text: 'Authentication token is missing. Please log in again.',
                confirmButtonText: 'OK'
            }).then(() => {
                navigate('/login'); // Redirect to login or another appropriate page
            });
        }
    };

    return (
        <div className="create-user-page">
            <header>
                <button onClick={goToBusiness} className="return-button">
                    Return to Business
                </button>
            </header>
            <main>
                <h1>Create User</h1>
                <CreateUserFrom
                    onSubmitCreate={handleSubmission}
                    token={authToken ?? ''}
                    submitText="Create User"
                />
            </main>
        </div>
    );
};

export default CreateUserPage;
