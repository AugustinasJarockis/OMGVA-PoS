import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom'
import DeletableUpdatableListItem from '../../components/List/DeletableUpdatableListItem';
import '../../index.css';
import '../../components/List/ClickableListItem.css';
import ClickableListItem from '../../components/List/ClickableListItem';
import { useAuth } from '../../contexts/AuthContext';
import { DiscountType, deleteDiscount, getAllDiscounts } from '../../services/discountService';
import { getTokenBusinessId, getTokenRole } from '../../utils/tokenUtils';

const DiscountListPage: React.FC = () => {
    const [listItems, setListItems] = useState<Array<JSX.Element>>();
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();
    const { authToken } = useAuth();

    const onDelete = async (id: string) => {
        try {
            const error = await deleteDiscount(authToken, id);

            if (error) {
                setError("An error occurred while deleting discount: " + error);
                return;
            }

            if (!listItems) {
                getDiscounts();
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

    const getDiscounts = async () => {
        setError(null);

        try {
            const { result, error } = await getAllDiscounts(authToken);

            if (!result) {
                setError('Problem acquiring discounts: ' + error);
            }
            else {
                result.filter(d => d.Type == DiscountType.Item);

                setListItems(result.map(discount =>
                    <DeletableUpdatableListItem
                        key={discount.Id}
                        id={discount.Id}
                        text={"Valid until: " + discount.TimeValidUntil.replace('T', ' ').slice(0 , 16) + " Amount: " + (discount.Amount ? (discount.Amount + "%") : "NaN")}
                        updateUrl={"/discount/update/" + discount.Id}
                        deleteFunction={onDelete}
                        object={ discount }
                    />));
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const goToBusinessSelection = async () => {
        if (authToken)
            navigate("/business/" + getTokenBusinessId(authToken));
    }

    useEffect(() => {
        if (authToken) {
            const role = getTokenRole(authToken);
            if (!(role === "Owner" || role === "Admin")) {
                navigate('/');
            }
            getDiscounts();
        }
        else {
            setError("You have to authenticate first!");
        }
    }, []);

    return (
        <div>
            <header>
                <button onClick={goToBusinessSelection}>Business</button>
            </header>
            <h1>Available discounts</h1>
            <div className="tax-list-container">
                {listItems}
                <div className="create-button-wrapper">
                    <ClickableListItem key="create" text="Create a new discount" url={'/discount/create'} />
                </div>
            </div>
            {error && <p className="error-message">{error}</p>}
        </div>
    );
};

export default DiscountListPage;