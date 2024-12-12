import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom';
import { Business, getBusiness } from '../../services/businessService';
import { getTokenBusinessId, getTokenRole, getTokenUserId } from '../../utils/tokenUtils';
import { loginWithNewToken } from '../../services/authService';
import '../../index.css';
import { useAuth } from '../../contexts/AuthContext';

interface BusinessPageProps {
    token: string | null;
}

const BusinessPage: React.FC<BusinessPageProps> = ({ token: authToken }) => {
    const [error, setError] = useState<string | null>(null);
    const [role, setRole] = useState<string>('');
    const [business, setBusiness] = useState<Business>();
    const { state } = useLocation();
    const { id } = useParams();
    const navigate = useNavigate();
    const { setAuthToken } = useAuth();

    const loadBusiness = async () => {
        setError(null);

        try {
            let newToken;
            if (!id) {
                setError('Could not identify the business');
                return;
            }

            if (authToken) {
                const { token } = await loginWithNewToken(authToken, id);
                newToken = token;
                if (token) {
                    setAuthToken(token);
                }
            }

            const { result, error } = await getBusiness(newToken ?? authToken, id);

            if (!result) {
                setError('Problem acquiring businesses: ' + error);
            } else {
                setBusiness(result);
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    };

    const handleUpdateBusinessOnclick = async () => {
        navigate("/business/update/" + id, { state: { business: business } });
    };

    const goToBusinessSelection = async () => {
        navigate("/business/");
    };

    const goToUser = async () => {
        if (authToken) {
            const userId = getTokenUserId(localStorage.getItem('authToken') ?? authToken);
            navigate(`/user/${userId}/`);
        }
    };

    const goToBusinessUsersList = async () => {
        if (authToken && getTokenRole(authToken) !== "Employee") {
            navigate('/user/business');
        }
    };

    useEffect(() => {
        if (state && state.business) {
            setBusiness(state.business);
        } else {
            loadBusiness();
        }

        if (authToken) {
            const role = getTokenRole(authToken);
            const businessId = getTokenBusinessId(authToken);
            if (!(businessId === id || role === "Admin")) {
                navigate('/');
            }
            setRole(role);
        } else {
            setError("You have to authenticate first!");
        }
    }, [authToken, id, navigate, state, setAuthToken]);

    return (
        <div>
            {role === 'Admin' && (
                <header>
                    <button onClick={goToBusinessSelection}>Select another business</button>
                    <button onClick={goToBusinessUsersList}>See all business users</button>
                    <button onClick={goToUser}>Me</button>
                </header>
            )}
            {business ? (
                <>
                    <h1>{business.Name}</h1>
                    <section>
                        <p>Address: {business.Address}</p>
                        <p>Phone: {business.Phone}</p>
                        <p>Email: {business.Email}</p>
                    </section>
                    {(role === 'Owner' || role === 'Admin') && <button onClick={handleUpdateBusinessOnclick}>Update information</button>}
                </>
            ) : (
                <p className="error-message">{error}</p>
            )}
        </div>
    );
};

export default BusinessPage;
