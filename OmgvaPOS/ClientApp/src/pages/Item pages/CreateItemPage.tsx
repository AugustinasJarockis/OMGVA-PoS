import React, { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom'
import { ChangeableItemFields, CreateItemRequest, createItem} from '../../services/itemService';
import '../../index.css';
import ItemDataForm from '../../components/Forms/ItemDataForm';
import { useAuth } from '../../contexts/AuthContext';

const CreateItemPage: React.FC = () => {
    const [error, setError] = useState<string | null>(null);
    const [group, setGroup] = useState<string | undefined>(undefined);
    const { state } = useLocation();
    const navigate = useNavigate();
    const { authToken } = useAuth();

    const handleSubmission = async (itemInfo: ChangeableItemFields) => {
        let itemCreateRequest: CreateItemRequest;
        if (itemInfo.InventoryQuantity
            && itemInfo.Name
            && itemInfo.Price
            && itemInfo.ItemGroup
            && itemInfo.Currency
            && (!itemInfo.UserId || itemInfo.Duration)
            && itemInfo.ImgPath
        ) {
            itemCreateRequest = {
                Name: itemInfo.Name,
                InventoryQuantity: itemInfo.InventoryQuantity,
                Price: itemInfo.Price,
                Currency: itemInfo.Currency,
                ItemGroup: itemInfo.ItemGroup,
                ImgPath: itemInfo.ImgPath,
                UserId: itemInfo.UserId,
                Duration: itemInfo.Duration,
                DiscountId: itemInfo.DiscountId
            }
        }
        else {
            setError("Invalid form data");
            return;
        }

        try {
            const { result, error } = await createItem(authToken, itemCreateRequest);
            if (error || !result) {
                setError("An error occurred while creating item: " + error);
                return;
            }
            if (group)
                navigate('/item', { state: { group: group } });
            else {
                navigate('/item/group');
            }
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const returnToList = () => {
        if (group)
            navigate('/item', { state: { group: group } });
        else
            navigate('/item/group');
    }

    useEffect(() => {
        if (!authToken) {
            setError("You have to authenticate first!");
            return;
        }

        if (state && state.itemGroup) {
            setGroup(state.itemGroup);
        }

        window.scrollTo(0, 0);
    }, []);

    return (
        <div>
            {!error ? (
                <>
                    <button onClick={returnToList}>Return</button>
                    <h1>Create item</h1>
                    <ItemDataForm onSubmit={handleSubmission} required submitText="Create item" itemGroup={ group } />
                </>
            )
                : <p className="error-message">{error}</p>
            }
        </div>
    );
};

export default CreateItemPage;