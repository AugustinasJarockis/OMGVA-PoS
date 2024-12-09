import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom'
import { getTokenBusinessId, getTokenRole } from '../../utils/tokenUtils';
import '../../index.css';
import TaxDataForm from '../../components/Forms/TaxDataForm';
import { Tax, TaxUpdateRequest, getTax, updateTax } from '../../services/taxService';
interface UpdateTaxPageProps {
    token: string | null;
}

const UpdateTaxPage: React.FC<UpdateTaxPageProps> = ({ token: authToken }) => {
    const [error, setError] = useState<string | null>(null);
    const [tax, setTax] = useState<Tax>();
    const { state } = useLocation();
    const { id } = useParams();
    const navigate = useNavigate();

    const handleSubmission = async (taxInfo: TaxUpdateRequest) => {
        try {
            if (checkId() && id) {
                const error = await updateTax(authToken, id, taxInfo);

                if (error) {
                    setError("An error occurred while updating the tax: " + error);
                    return;
                }

                navigate('/tax');
            }
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const acquireTax = async () => {
        if (checkId() && id) {
            const { result, error } = await getTax(authToken, id);
            if (!result) {
                setError('Problem acquiring tax: ' + error);
            }
            else {
                setTax(result);
            }
        }
    }

    const checkId = () => {
        if (id) {
            return true;
        }
        else {
            setError("Could not identify the tax");
            return false;
        }
    }

    const returnToList = async () => {
        navigate("/tax");
    }

    useEffect(() => {
        if (state) {
            setTax(state.object);
        }
        else {
            acquireTax();
            if (!tax) {
                return;
            }
        }

        if (authToken) {
            const role = getTokenRole(authToken);
            if (role !== "Admin") {
                navigate('/');
                return;
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
            {!error && tax ? (
            <>
                <h1>Update tax</h1>
                <TaxDataForm onSubmit={handleSubmission} tax={tax} submitText="Update tax" />
            </>
            )
            : <p className="error-message">{error}</p>
            }
        </div>
    );
};

export default UpdateTaxPage;

