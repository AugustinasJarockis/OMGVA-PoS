import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom'
import { Business, getBusiness } from '../services/businessService';
import { getTokenBusinessId, getTokenRole } from '../utils/tokenUtils';
import '../index.css';

interface BusinessPageProps {
    token: string | null
}

const BusinessPage: React.FC<BusinessPageProps> = ({ token: authToken }) => {
    const [error, setError] = useState<string | null>(null);
    const [role, setRole] = useState<string>('');
    const [business, setBusiness] = useState<Business>();
    const { state } = useLocation();
    const { id } = useParams();
    const navigate = useNavigate();

    const loadBusiness = async () => {
        setError(null);

        try {
            if (!id) {
                setError('Business id not provided');
                return;
            }
            const { result, error } = await getBusiness(authToken, id);

            if (!result) {
                setError('Problem acquiring businesses: ' + error);
            }
            else {
                setBusiness(result);
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const handleUpdateBusinessOnclick = async () => {
        navigate("/business/update/" + id, { state: { business: business } });
    }

    const goToBusinessSelection = async () => {
        navigate("/business/");
    }

    useEffect(() => {
        if (state && state.business) {
            setBusiness(state.business);
        }
        else {
            loadBusiness();
        }

        if (authToken) {
            const role = getTokenRole(authToken);
            const businessId = getTokenBusinessId(authToken);
            if (!((role === "Owner" && businessId === id) || role === "Admin")) {
                navigate('/');
            }
            setRole(role);
        }
        else {
            setError("Authorization token is missing!");
        }
    }, []);

    return (
        <div>
            {   role==='Admin' &&
                (<header>
                    <button onClick={goToBusinessSelection}>Select another business</button>
                </header>)}
            {business ? (
                <>
                    <h1>{business.Name}</h1>
                    <section>
                        <p>Address: { business.Address }</p>
                        <p>Phone: { business.Phone }</p>
                        <p>Email: { business.Email }</p>
                    </section>
                    {(role === 'Owner' || role === 'Admin') && <button onClick={handleUpdateBusinessOnclick}>Update information</button>}
                </>
                )
                : <p className="error-message">{error}</p>
            }
        </div>
    );
};

export default BusinessPage;

