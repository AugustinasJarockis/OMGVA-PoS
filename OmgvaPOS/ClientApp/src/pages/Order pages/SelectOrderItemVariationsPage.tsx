import React, { useState, useEffect } from 'react';
import { useLocation, useNavigate, useParams } from 'react-router-dom'
import '../Item pages/ItemPages.css';
import { useAuth } from '../../contexts/AuthContext';
import { getAllItemVariations } from '../../services/itemVariationService';
import ItemVariationSelector from '../../components/Selectors/ItemVariationsSelector';
import { CreateOrderItemRequest, createOrderItem } from '../../services/orderItemService';

const SelectOrderItemVariationsPage: React.FC = () => {
    const [listItems, setListItems] = useState<Array<JSX.Element>>();
    const [groupNames, setGroupNames] = useState<Array<string>>();
    const [error, setError] = useState<string | null>(null);
    const { id, itemId } = useParams();
    const { state } = useLocation();
    const navigate = useNavigate();
    const { authToken } = useAuth();

    const getItemVariations = async () => {
        setError(null);

        try {
            if (!itemId) {
                setError("Could not identify the item");
                return;
            }
            const { result, error } = await getAllItemVariations(authToken, itemId);

            if (!result) {
                setError('Problem acquiring item variations: ' + error);
            }
            else {
                result.sort((a, b) =>
                    (a.Name < b.Name)
                        ? -1
                        : (a.Name > b.Name)
                            ? 1 : 0);

                const groupNamesWithRepeat = result.map(variation => variation.ItemVariationGroup);
                const groupNamesWithoutRepeat = ([...new Set(groupNamesWithRepeat)]).sort();
                setGroupNames(groupNamesWithoutRepeat);

                const pageElements = groupNamesWithoutRepeat.map(variationGroup => {
                    const groupVariations = result.filter(variation => variation.ItemVariationGroup == variationGroup);
                    return (
                        <div key = { variationGroup } id = { variationGroup } >
                            <h2>{variationGroup} variations:</h2>
                            <ItemVariationSelector potentialVariations={groupVariations} groupName={variationGroup} />
                        </div >                        
                    );
                });
                setListItems(pageElements);
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const handleSubmission = async (e: React.SyntheticEvent<HTMLFormElement>) => {
        e.preventDefault();

        const form = e.currentTarget;
        const formData = new FormData(form);

        const itemVariationIds = [];
        if (!groupNames)
            return [];

        for (const group of groupNames) {
            const chosenValue = formData.get(group);
            if (chosenValue !== '' && chosenValue !== null && typeof chosenValue !== 'File')
                itemVariationIds.push( chosenValue );
        }

        //TODO: check if item quantity is not 0
        if (!itemId) {
            setError('Could not identify the item');
            return;
        }
        const orderItemCreationRequest: CreateOrderItemRequest = {
            Quantity: 1,
            ItemId: itemId,
            ItemVariationIds: itemVariationIds
        }

        try {
            let error;
            if (id)
                error = await createOrderItem(authToken, orderItemCreationRequest, id);
            else
                setError("Could not identify the order while creating order item");

            if (error) {
                setError("An error occurred while creating order item: " + error);
                return;
            }
            if (state.group)
                navigate(`/order/${id}/add-items`, { state: { group: state.group } });
            else {
                navigate(`/order/${id}/add-items`);
            }
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const returnToList = () => {
        navigate(`/order/${id}/add-items`, { state: { group: state.group } });
    }

    const returnToOrder = () => {
        if (authToken)
            navigate('/order/' + id);
        else
            navigate('/');
    }

    useEffect(() => {
        if (authToken) {
            getItemVariations();
        }
        else {
            setError("You have to authenticate first!");
        }
    }, []);
    //TODO: item name
    return (
        <div>
            <header>
                <button onClick={returnToOrder}>Return to order</button>
                <button onClick={returnToList}>Return</button>
            </header>
            <h1>Item variations</h1>
            <form onSubmit={handleSubmission}>
                {listItems}
                <br/><br/>
                <input type="submit" value="Finish selection" />
            </form>
            {error && <p className="error-message">{error}</p>}
        </div>
    );
};

export default SelectOrderItemVariationsPage;