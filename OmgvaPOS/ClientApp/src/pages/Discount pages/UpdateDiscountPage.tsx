import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom'
import { getTokenRole } from '../../utils/tokenUtils';
import '../../index.css';
import DiscountUpdateDataForm from '../../components/Forms/DiscountUpdateDataForm';
import { useAuth } from '../../contexts/AuthContext';
import { Discount, getDiscount, updateDiscountValidUntilTime } from '../../services/discountService';

const UpdateTaxPage: React.FC = () => {
    const [error, setError] = useState<string | null>(null);
    const [discount, setDiscount] = useState<Discount>();
    const { state } = useLocation();
    const { id } = useParams();
    const navigate = useNavigate();
    const { authToken } = useAuth();

    const handleSubmission = async (newValidUntil: string) => {
        try {
            if (checkId() && id) {
                const error = await updateDiscountValidUntilTime(authToken, id, newValidUntil);

                if (error) {
                    setError("An error occurred while updating the discount: " + error);
                    return;
                }

                navigate('/discount');
            }
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const acquireDiscount = async () => {
        if (checkId() && id) {
            const { result, error } = await getDiscount(authToken, id);
            if (!result) {
                setError('Problem acquiring discount: ' + error);
            }
            else {
                setDiscount(result);
            }
        }
    }

    const checkId = () => {
        if (id) {
            return true;
        }
        else {
            setError("Could not identify the discount");
            return false;
        }
    }

    const returnToList = async () => {
        navigate("/discount");
    }

    useEffect(() => {
        if (state) {
            setDiscount(state.object);
        }
        else {
            acquireDiscount();
            if (!discount) {
                return;
            }
        }

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
            {!error && discount ? (
            <>
                <h1>Update tax</h1>
                <DiscountUpdateDataForm onSubmit={handleSubmission} currentValidUntil={discount.TimeValidUntil} submitText="Update tax" />
            </>
            )
            : <p className="error-message">{error}</p>
            }
        </div>
    );
};

export default UpdateTaxPage;

