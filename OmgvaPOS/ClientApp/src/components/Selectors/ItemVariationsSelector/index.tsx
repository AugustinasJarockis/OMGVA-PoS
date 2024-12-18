import React, { useEffect, useState } from 'react';
import './ItemVariationSelector.css';
import { useAuth } from '../../../contexts/AuthContext';
import { ItemVariation } from '../../../services/itemVariationService';

interface ItemVariationSelectorProps {
    current?: string;
    potentialVariations: Array<ItemVariation>;
    groupName: string;
}

const ItemVariationSelector: React.FC<ItemVariationSelectorProps> = (props: ItemVariationSelectorProps) => {
    const [error, setError] = useState<string | null>(null);
    const [listItems, setListItems] = useState<Array<JSX.Element>>();
    const { authToken } = useAuth();

    const setVariations = async () => {
        setError(null);

        try {
            setListItems(props.potentialVariations.map(variation =>
                <option key={variation.Id} value={variation.Id}>
                    {variation.Name} | Price: { variation.PriceChange }
                </option>));
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    useEffect(() => {
        if (!authToken) {
            setError("You have to authenticate first!");
        }
        else {
            setVariations();
        }
    }, []);

    return (
        <div className="selector-container">
            <select className="wider-select" name={props.groupName} id={props.groupName} defaultValue={props.current ? props.current : '' }>
                <option hidden disabled value=''> None </option>
                {listItems}
            </select>
        </div>
        || error
    );
};

export default ItemVariationSelector;