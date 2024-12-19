import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom'
import '../../index.css';
import { useAuth } from '../../contexts/AuthContext';
import OrderRefundDataForm from '../../components/Forms/OrderRefundDataForm';
import { RefundOrderRequest, refundOrder } from '../../services/orderService';

const OrderRefundPage: React.FC = () => {
    const [error, setError] = useState<string | null>(null);
    const { id } = useParams();
    const navigate = useNavigate();
    const { authToken } = useAuth();

    const handleSubmission = async (refundOrderRequest: RefundOrderRequest) => {
        try {
            if (checkId() && id) {
                const result = await refundOrder(authToken, id, refundOrderRequest);

                if (typeof result === 'string') {
                    setError("An error occurred while refunding the order: " + result);
                    return;
                }

                navigate('/order/' + id);
            }
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const checkId = () => {
        if (id) {
            return true;
        }
        else {
            setError("Could not identify the order");
            return false;
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
            {!error ? (
                <>
                    <h1>Refund</h1>
                    <OrderRefundDataForm onSubmit={handleSubmission} submitText="Refund" />
                </>
            )
                : <p className="error-message">{error}</p>
            }
        </div>
    );
};

export default OrderRefundPage;