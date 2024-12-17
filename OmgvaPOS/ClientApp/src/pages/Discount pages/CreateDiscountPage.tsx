import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom'
import { getTokenRole } from '../../utils/tokenUtils';
import '../../index.css';
import { useAuth } from '../../contexts/AuthContext';
import { DiscountCreateRequest, DiscountType, createDiscount } from '../../services/discountService';
import DiscountCreationDataForm from '../../components/Forms/DiscountCreationDataForm';

const CreateDiscountPage: React.FC = () => {
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();
    const { authToken } = useAuth();

    const handleSubmission = async (discountCreateRequest: DiscountCreateRequest) => {
        try {
            const { error, result } = await createDiscount(authToken, discountCreateRequest);

            if (error || !result) {
                setError("An error occurred while creating the discount: " + error);
                return;
            }

            navigate('/discount');
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const returnToList = async () => {
        navigate("/discount");
    }

    useEffect(() => {
        if (authToken) {
            const role = getTokenRole(authToken);
            if (!(role === "Owner" || role === "Admin")) {
                navigate('/');
            }
        }
        else {
            setError("You have to authenticate first!");
        }
    }, []);

    return (
        <div>
            <header>
                <button onClick={returnToList}>Return</button>
            </header>
            <h1> Create a new discount </h1>
            {
                !error ? (
                    <>
                        <DiscountCreationDataForm onSubmit={handleSubmission} submitText="Create" type={DiscountType.Item} />
                    </>
                )
                    : <p className="error-message" >{error}</p>
            }
        </div>
    );
};

export default CreateDiscountPage;

