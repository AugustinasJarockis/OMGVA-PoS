import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom'
import { getTokenRole } from '../../utils/tokenUtils';
import '../../index.css';
import TaxDataForm from '../../components/Forms/TaxDataForm';
import { TaxCreateRequest, TaxUpdateRequest, createTax } from '../../services/taxService';
interface CreateTaxPageProps {
    token: string | null;
}

const CreateTaxPage: React.FC<CreateTaxPageProps> = ({ token: authToken }) => {
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    const handleSubmission = async (taxInfo: TaxUpdateRequest) => {
        try {
            let taxCreateRequest: TaxCreateRequest;
            if (taxInfo.taxType && taxInfo.percent) {
                taxCreateRequest = {
                    taxType: taxInfo.taxType,
                    percent: +taxInfo.percent
                }
            }
            else {
                setError("Invalid form data");
                return;
            }
            const { error, result } = await createTax(authToken, taxCreateRequest);

            if (error || !result) {
                setError("An error occurred while creating the tax: " + error);
                return;
            }

            navigate('/tax');
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const returnToList = async () => {
        navigate("/tax");
    }

    useEffect(() => {
        if (authToken) {
            const role = getTokenRole(authToken);
            if (role !== "Admin") {
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
            <h1> Create a new tax </h1>
            {
                !error ? (
                    <>
                        <TaxDataForm onSubmit={handleSubmission} submitText="Create" required />
                    </>
                )
                    : <p className="error-message" >{error}</p>
            }
        </div>
    );
};

export default CreateTaxPage;

