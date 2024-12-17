import React, { useEffect, useState } from 'react';
import '../List/ClickableListItem.css';
import './ItemVariationManagement.css';
import { deleteItemVariation, getAllItemVariations } from '../../services/itemVariationService';
import ClickableListItem from '../List/ClickableListItem';
import { useAuth } from '../../contexts/AuthContext';
import DeletableUpdatableListItem from '../List/DeletableUpdatableListItem';

interface ItemVariationManagementComponentProps {
    itemId: string;
}

const ItemVariationManagementComponent: React.FC<ItemVariationManagementComponentProps> = (props: ItemVariationManagementComponentProps) => {
    const [listItems, setListItems] = useState<Array<JSX.Element>>();
    const [error, setError] = useState<string | null>(null);
    const { authToken } = useAuth();

    const getItemVariations = async () => {
        setError(null);

        try {
            const { result, error } = await getAllItemVariations(authToken, props.itemId);

            if (!result) {
                setError('Problem acquiring item variations: ' + error);
            }
            else {
                result.sort((a, b) =>
                    (a.ItemVariationGroup < b.ItemVariationGroup || (a.ItemVariationGroup === b.ItemVariationGroup && a.Name < b.Name))
                        ? -1
                        : ((a.ItemVariationGroup > b.ItemVariationGroup) || (a.ItemVariationGroup === b.ItemVariationGroup && a.Name > b.Name))
                            ? 1 : 0);

                setListItems(result.map(variation =>
                    <DeletableUpdatableListItem
                        key={variation.Id}
                        id={variation.Id}
                        text={variation.ItemVariationGroup + '/' + variation.Name + ": " + variation.PriceChange + ' (Remaining: ' + variation.InventoryQuantity + ')'}
                        updateUrl={`/item/${props.itemId}/item-variation/update/${variation.Id}`}
                        deleteFunction={onDelete}
                        object={variation}
                    />));
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const onDelete = async (id: string) => {
        try {
            const error = await deleteItemVariation(authToken, id);

            if (error) {
                setError("An error occurred while deleting tax: " + error);
                return;
            }

            if (!listItems) {
                getItemVariations();
            }

            if (listItems) {
                const newList = listItems.filter(item => item.key != id);
                setListItems(newList);
            }
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    useEffect(() => {
        if (!authToken) {
            setError("You have to authenticate first!");
        }
        else {
            getItemVariations();
        }
    }, []);

    return (
        <div className="item-variation-management-container">
            {listItems}
            <div className="create-button-wrapper">
                <ClickableListItem key="create" text="Create a new item variation" url={`/item/${props.itemId}/item-variation/create`} />
            </div>
            {error && <p className="error-message">{error}</p>}
        </div>
    );
};

export default ItemVariationManagementComponent;