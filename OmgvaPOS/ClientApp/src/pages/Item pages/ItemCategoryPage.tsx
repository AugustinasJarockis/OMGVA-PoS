import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom'
import { getAllItems } from '../../services/itemService';
import SquareGridItem from '../../components/Grid/SquareGridItem';
import './ItemPages.css';
import { getTokenBusinessId } from '../../utils/tokenUtils';
import { useAuth } from '../../contexts/AuthContext';

const ItemCategoryListPage: React.FC = () => {
    const [gridItems, setGridItems] = useState<Array<JSX.Element>>();
    const [error, setError] = useState<string | null>(null);
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
                    <SquareGridItem key={ itemGroup } onclick={() => { openItemGroup(itemGroup); }}>
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
        navigate('/item', { state: {group: groupString} });
    }

    const createNewItem = () => {
        navigate('/item/create');
    }

    const returnToBusiness = () => {
        if (authToken)
            navigate('/business/' + getTokenBusinessId(authToken));
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
                <button onClick={returnToBusiness}>Return to business</button>
            </header>
            <h1>Products and services</h1>
            <div className="square-grid-container">
                {gridItems}
                <div className="create-item-button-wrapper">
                    <SquareGridItem key="create" onclick={createNewItem}>
                        +
                    </SquareGridItem>
                </div>
            </div>
            {error && <p className="error-message">{error}</p>}
        </div>
    );
};

export default ItemCategoryListPage;

