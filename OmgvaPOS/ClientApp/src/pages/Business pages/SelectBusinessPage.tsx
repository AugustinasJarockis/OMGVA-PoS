import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom'
import { getAllBusinesses } from '../../services/businessService';
import ClickableListItem from '../../components/List/ClickableListItem';
import '../../index.css';
import '../../components/List/ClickableListItem.css';
import { getTokenRole } from '../../utils/tokenUtils';

interface SelectBusinessPageProps {
    token : string | null
}

const SelectBusinessPage: React.FC<SelectBusinessPageProps> = ({token: authToken}) => {
    const [listItems, setListItems] = useState<Array<JSX.Element>>();
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    const getBusinesses = async () => {
        setError(null);

        try {
            const { result, error } = await getAllBusinesses(authToken);

            if (!result) {
                setError('Problem acquiring businesses: ' + error);
            }
            else {
                setListItems(result.map(business =>
                    <ClickableListItem key={business.Id} text={business.Name} url={'/business/' + business.Id}/>));
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const goToTaxesList = async () => {
        navigate("/tax/");
    }

    useEffect(() => {
        if (authToken) {
            const role = getTokenRole(authToken);
            if (role !== "Admin") {
                navigate('/');
                return;
            }
            getBusinesses();
        }
        else {
            setError("You have to authenticate first!");
        }
    }, []);

    return (
        <div>
            <header>
                <button onClick={goToTaxesList}>Taxes</button>
            </header>
            <h1>Select the business to open</h1>
            <div className="business-list-container">
                {listItems}
                <div className="create-button-wrapper">
                    <ClickableListItem key="create" text="Create a new business" url={'/business/create'} />
                </div>
            </div>
            {error && <p className="error-message">{error}</p>}
        </div>
    );
};

export default SelectBusinessPage;

