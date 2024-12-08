import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom'
import { Business, updateBusiness } from '../../services/businessService';
import { getTokenBusinessId, getTokenRole } from '../../utils/tokenUtils';
import '../../index.css';
import BusinessDataForm from '../../components/Forms/BusinessDataForm';

interface UpdateBusinessPageProps {
    token: string | null;
}

const UpdateBusinessPage: React.FC<UpdateBusinessPageProps> = ({ token: authToken }) => {
    const [error, setError] = useState<string | null>(null);
    const [business, setBusiness] = useState<Business>();
    const { state } = useLocation();
    const { id } = useParams();
    const navigate = useNavigate();

    const handleSubmission = async (businessInfo: Business) => {
        try {
            if (id) {
                const error = await updateBusiness(authToken, id, businessInfo);
                if (error) {
                    setError("An error occurred while updating business: " + error);
                    return;
                }
                navigate('/business/' + id);
            }
            else {
                setError("Could not identify the business");
            }
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    useEffect(() => {
        setBusiness(state.business);

        if (authToken) {
            const role = getTokenRole(authToken);
            const businessId = getTokenBusinessId(authToken);
            if (!((role === "Owner" && businessId === id) || role === "Admin")) {
                navigate('/');
            }
        }
        else {
            setError("You have to authenticate first!");
        }
    }, []);

    return (
        <div>
            {!error && business ? (
                <>
                    <h1>Update {business.Name} information</h1>
                    <BusinessDataForm onSubmit={ handleSubmission } business={business} submitText="Update information" />
                </>
            )
                : <p className="error-message">{error}</p>
            }
        </div>
    );
};

export default UpdateBusinessPage;

