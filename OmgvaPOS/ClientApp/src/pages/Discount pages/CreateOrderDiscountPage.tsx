import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import '../../index.css';
import { useAuth } from '../../contexts/AuthContext';
import { DiscountCreateRequest, DiscountType, createDiscount } from '../../services/discountService';
import DiscountCreationDataForm from '../../components/Forms/DiscountCreationDataForm';

const CreateOrderDiscountPage: React.FC = () => {
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();
    const { id } = useParams();
    const { authToken } = useAuth();

    const handleSubmission = async (discountCreateRequest: DiscountCreateRequest) => {
        try {
            const { error, result } = await createDiscount(authToken, discountCreateRequest);

            if (error || !result) {
                setError("An error occurred while creating the discount: " + error);
                return;
            }

            navigate("/order/" + id);
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const returnToOrder = async () => {
        navigate("/order/" + id);
    }

    useEffect(() => {
        if (!authToken) {
            setError("You have to authenticate first!");
        }
    }, []);

    return (
        <div>
            <header>
                <button onClick={returnToOrder}>Return</button>
            </header>
            <h1> Add a discount to order </h1>
            {
                !error ? (
                    <>
                        <DiscountCreationDataForm onSubmit={handleSubmission} submitText="Add" type={DiscountType.Order} orderId={ id } />
                    </>
                )
                    : <p className="error-message" >{error}</p>
            }
        </div>
    );
};

export default CreateOrderDiscountPage;

