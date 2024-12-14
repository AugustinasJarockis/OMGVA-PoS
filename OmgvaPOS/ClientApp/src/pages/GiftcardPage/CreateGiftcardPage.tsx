import React, { useEffect, useState } from 'react';
import { useAuth } from '../../contexts/AuthContext';
import { useNavigate } from 'react-router-dom';
import { CreateGiftcard, createGiftcard } from '../../services/giftcardService';
import CreateGiftcardForm from '../../components/Forms/GiftcardDataForm';

const CreateGiftcardPage: React.FC = () => {
    const [error, setError] = useState<string | null>(null);
    const [successMessage, setSuccessMessage] = useState<string | null>(null);
    const navigate = useNavigate();
    const { authToken } = useAuth();

    const handleSubmission = async (giftcard: CreateGiftcard) => {
        try {
            const { error, result } = await createGiftcard(authToken, giftcard);

            if (error) {
                setError("An error occurred while creating a giftcard: " + error);
                setSuccessMessage(null);
                return;
            }
            setSuccessMessage("Giftcard created successfully!");
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    };

    const goToGiftcards = async () => {
        if (authToken) {
            navigate('/giftcard/');
        }
    };

    return (
        <div>
            <header>
                <button onClick={goToGiftcards}>Return to giftcards</button>
            </header>
            <h1>Create Giftcard</h1>
            <CreateGiftcardForm onSubmitCreate={handleSubmission} token={authToken ?? ''} submitText="Create giftcard"/>

            {error && <p className="error-message">{error}</p>}
            {successMessage && <p>{successMessage}</p>}
        </div>
    );
};

export default CreateGiftcardPage;
