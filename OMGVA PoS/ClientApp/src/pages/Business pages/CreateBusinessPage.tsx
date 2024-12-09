import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom'
import { Business, createBusiness } from '../../services/businessService';
import { getTokenRole } from '../../utils/tokenUtils';
import '../../index.css';
import BusinessDataForm from '../../components/Forms/BusinessDataForm';
interface CreateBusinessPageProps
{
    token: string | null;
}

const CreateBusinessPage: React.FC <CreateBusinessPageProps> = ({ token: authToken }) => {
    const [error, setError] = useState <string | null> (null);
    const navigate = useNavigate();

    const handleSubmission = async(businessInfo: Business) => {
        try {
            const { error, result } = await createBusiness(authToken, businessInfo);

            if (error || !result) {
                setError("An error occurred while creating the business: " + error);
                return;
            }

            // Workaround because typescript treats variable name cases weirdly
            const anyResult: any = result;
            result.Id = anyResult.id;
            result.Name = anyResult.name;
            result.Address = anyResult.address;
            result.Phone = anyResult.phone;
            result.Email = anyResult.email;
            // Workaround end

            navigate('/business/' + result.Id, { state: { business: result } });
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const returnToList = async () => {
        navigate("/business/");
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
            <h1> Create a new business </h1>
        {
            !error ? (
                <>
                    <BusinessDataForm onSubmit={ handleSubmission } submitText="Create" required/>
                </>
            )
                : <p className = "error-message" >{ error}</p>
        }
        </div>
    );
};

export default CreateBusinessPage;

