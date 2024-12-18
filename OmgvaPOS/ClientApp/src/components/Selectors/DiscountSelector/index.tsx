import React, { useEffect, useState } from 'react';
import './DiscountSelector.css';
import { useAuth } from '../../../contexts/AuthContext';
import { DiscountType, getAllDiscounts } from '../../../services/discountService';

interface DiscountSelectorProps {
    required?: boolean;
    current?: string;
}

const DiscountSelector: React.FC<DiscountSelectorProps> = (props: DiscountSelectorProps) => {
    const [error, setError] = useState<string | null>(null);
    const [listItems, setListItems] = useState<Array<JSX.Element>>();
    const { authToken } = useAuth();

    const getDiscounts = async () => {
        setError(null);

        try {
            const { result, error } = await getAllDiscounts(authToken);

            if (!result) {
                setError('Problem acquiring discount: ' + error);
            }
            else {
                result.filter(discount => discount.Type == DiscountType.Item);
                setListItems(result.map(discount =>
                    <option key={discount.Id} value={discount.Id}>
                        {discount.Amount}% (Valid until: {discount.TimeValidUntil.replace('T', ' ')})
                    </option>));
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    useEffect(() => {
        if (!authToken) {
            setError("You have to authenticate first!");
        }
        else {
            getDiscounts();
        }
    }, []);

    return (
        <div className="selector-container">
            <select className="wider-select" name="discount" id="discount" required={props?.required} defaultValue={props.current ? props.current : '' }>
                <option hidden disabled value=''> -- select an option -- </option>
                {listItems}
            </select>
        </div>
        || error
    );
};

export default DiscountSelector;