import React, { useState, useEffect } from 'react';
import { useLocation, useNavigate, useParams } from 'react-router-dom'
import { getAllItems } from '../../services/itemService';
import SquareGridItem from '../../components/Grid/SquareGridItem';
import '../Item pages/ItemPages.css';
import { useAuth } from '../../contexts/AuthContext';

interface ItemListPageProps {
    itemGroup?: string
}

const OrderItemSelectionListPage: React.FC<ItemListPageProps> = ({ itemGroup: itemGroup }) => {
    const [gridItems, setGridItems] = useState<Array<JSX.Element>>();
    const [error, setError] = useState<string | null>(null);
    const [group, setGroup] = useState<string | undefined>(undefined);
    const { state } = useLocation();
    const { id } = useParams();
    const navigate = useNavigate();
    const { authToken } = useAuth();

    const getItems = async () => {
        setError(null);

        try {
            let { result, error } = await getAllItems(authToken);

            if (!result) {
                setError('Problem acquiring items: ' + error);
            }
            else {
                if (group) {
                    result = result.filter(i => i.ItemGroup === group);
                }
                else if (state && state.group) {
                    result = result.filter(i => i.ItemGroup === state.group && (!state.currency || i.Currency === state.currency));
                }

                result.sort((a, b) => a.Name < b.Name ? -1 : (a.Name > b.Name ? 1 : 0));

                setGridItems(result.map(item =>
                    <SquareGridItem key={item.Id} onclick={() => { selectItem(item.Id); }}>
                        <div className="gridOrderItem">
                            <img className="grid-item-img" src={item.ImgPath} alt="img" />
                            <h2>{item.Name}</h2>
                            <ul>
                                <li><b>Quantity:</b> {item.InventoryQuantity}</li>
                                <li><b>Price:</b> {item.Price} {item.Currency} </li>
                            </ul>
                        </div>
                    </SquareGridItem>));
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const selectItem = (itemId: string) => {
        navigate(`/order/${id}/add-items/${itemId}/variations`, { state: { group: state.group, currency: state.currency } });
    }

    const returnToGroups = () => {
        navigate(`/order/${id}/add-items/group`, { state: { currency: state.currency }});
    }

    useEffect(() => {
        if (authToken) {
            setGroup(itemGroup);
            getItems();
        }
        else {
            setError("You have to authenticate first!");
        }
    }, []);

    return (
        <div>
            <header>
                <button onClick={returnToGroups}>Return</button>
            </header>
            <h1>Products and services</h1>
            <div className="square-grid-container">
                {gridItems}
            </div>
            {error && <p className="error-message">{error}</p>}
        </div>
    );
};

export default OrderItemSelectionListPage;