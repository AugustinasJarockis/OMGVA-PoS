import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom'
import { getAllItems } from '../../services/itemService';
import SquareGridItem from '../../components/Grid/SquareGridItem';
import '../Item pages/ItemPages.css';
import { useAuth } from '../../contexts/AuthContext';

const OrderItemSelectCategoryListPage: React.FC = () => {
    const [gridItems, setGridItems] = useState<Array<JSX.Element>>();
    const [error, setError] = useState<string | null>(null);
    const { id } = useParams();
    const navigate = useNavigate();
    const { authToken } = useAuth();

    const getItems = async () => {
        setError(null);

        try {
            const { result, error } = await getAllItems(authToken);

            if (!result) {
                setError('Problem acquiring items: ' + error);
            }
            else {
                const groupNamesWithRepeat = result.map(item => item.ItemGroup);
                const groupNamesWithoutRepeat = ([...new Set(groupNamesWithRepeat)]).sort();

                setGridItems(groupNamesWithoutRepeat.map(itemGroup =>
                    <SquareGridItem key={itemGroup} onclick={() => { openItemGroup(itemGroup); }}>
                        <div className="gridItem">
                            <h2>{itemGroup}</h2>
                        </div>
                    </SquareGridItem>));
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const openItemGroup = (groupString: string) => {
        navigate(`/order/${id}/add-items`, { state: { group: groupString } });
    }

    const returnToOrder = () => {
        if (authToken)
            navigate('/order/' + id);
        else
            navigate('/');
    }

    useEffect(() => {
        if (authToken) {
            getItems();
        }
        else {
            setError("You have to authenticate first!");
        }
    }, []);

    return (
        <div>
            <header>
                <button onClick={returnToOrder}>Return to order</button>
            </header>
            <h1>Products and services</h1>
            <div className="square-grid-container">
                {gridItems}
            </div>
            {error && <p className="error-message">{error}</p>}
        </div>
    );
};

export default OrderItemSelectCategoryListPage;