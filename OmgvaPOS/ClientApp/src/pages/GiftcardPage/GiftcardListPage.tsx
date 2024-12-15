import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { getBusinessGiftcards } from '../../services/giftcardService';
import { getTokenBusinessId, getTokenRole } from '../../utils/tokenUtils';
import GiftcardListItem from '../../components/List/GiftcardListItem';

const GiftcardListPage: React.FC = () => {
    const [listItems, setListItems] = useState<Array<JSX.Element>>();
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();
    const { authToken } = useAuth();

    const getGiftcards = async () => {
        setError(null);

        try {
            const { result, error } = await getBusinessGiftcards(authToken);

            if (!result) {
                setError('Problem acquiring giftcards: ' + error);
            } else {
                setListItems(result.map(giftcard => 
                        <GiftcardListItem
                            key={giftcard.Id}
                            text={giftcard.Code}
                            value={giftcard.Value}
                            balance={giftcard.Balance}
                        />));
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    };


    const goToBusiness = async () => {
        if (authToken) {
            const businessId = getTokenBusinessId(authToken);
            navigate('/business/' + businessId);
        }
    };

    const goToCreate = async () => {
        navigate('/giftcard/create');
    };

    useEffect(() => {
        if (!authToken) {
            setError("You have to authenticate first!");
        } else {
            const role = getTokenRole(authToken);
            if (role === "Employee") {
                navigate('/');
                return;
            }
            getGiftcards();
        }
    }, [authToken]);

    return (
        <div>
            <header>
                <button onClick={goToBusiness}>Return to business</button>
            </header>
            <h1>Business giftcards</h1>
            <div className="tax-list-container">
                {listItems}
                <br></br>
                <div className="create-button-wrapper" >
                    <button key="create" onClick={goToCreate}>Create a new giftcard</button>
                </div>
            </div>
            {error && <p className="error-message">{error}</p>}
        </div>
    );
};

export default GiftcardListPage;