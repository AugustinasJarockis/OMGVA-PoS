import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom'
import { deleteTax, getAllTaxes } from '../../services/taxService';
import DeletableUpdatableListItem from '../../components/List/DeletableUpdatableListItem';
import '../../index.css';
import '../../components/List/ClickableListItem.css';
import { getTokenRole } from '../../utils/tokenUtils';
import ClickableListItem from '../../components/List/ClickableListItem';

interface TaxListPageProps {
    token: string | null
}

const TaxListPage: React.FC<TaxListPageProps> = ({ token: authToken }) => {
    const [listItems, setListItems] = useState<Array<JSX.Element>>();
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    const onDelete = async (id: string) => {
        try {
            const error = await deleteTax(authToken, id);

            if (error) {
                setError("An error occurred while deleting tax: " + error);
                return;
            }

            if (!listItems) {
                getTaxes();
            }

            if (listItems) {
                const newList = listItems.filter((item) => item.key != id);
                setListItems(newList);
            }
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const getTaxes = async () => {
        setError(null);

        try {
            const { result, error } = await getAllTaxes(authToken);

            if (!result) {
                setError('Problem acquiring taxes: ' + error);
            }
            else {
                setListItems(result.map(tax =>
                    <DeletableUpdatableListItem
                        key={tax.id}
                        id={tax.id}
                        text={tax.taxType + ": " + (tax.percent ? (tax.percent + "%") : "NaN")}
                        updateUrl={"/tax/update/" + tax.id}
                        deleteFunction={onDelete}
                        object={ tax }
                    />));
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const goToBusinessSelection = async () => {
        navigate("/business/");
    }

    useEffect(() => {
        if (authToken) {
            const role = getTokenRole(authToken);
            if (role !== "Admin") {
                navigate('/');
                return;
            }
            getTaxes();
        }
        else {
            setError("You have to authenticate first!");
        }
    }, []);

    return (
        <div>
            <header>
                <button onClick={goToBusinessSelection}>Businesses</button>
            </header>
            <h1>Available taxes</h1>
            <div className="tax-list-container">
                {listItems}
                <div className="create-button-wrapper">
                    <ClickableListItem key="create" text="Create a new tax" url={'/tax/create'} />
                </div>
            </div>
            {error && <p className="error-message">{error}</p>}
        </div>
    );
};

export default TaxListPage;

