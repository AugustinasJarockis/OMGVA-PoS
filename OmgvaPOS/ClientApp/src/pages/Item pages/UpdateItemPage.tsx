import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom'
import { ChangeableItemFields, Item, getItem, updateItem, deleteItem } from '../../services/itemService';
import '../../index.css';
import ItemDataForm from '../../components/Forms/ItemDataForm';
import { useAuth } from '../../contexts/AuthContext';
import ItemVariationManagementComponent from '../../components/ItemVariationManagementList';

const UpdateItemPage: React.FC = () => {
    const [error, setError] = useState<string | null>(null);
    const [item, setItem] = useState<Item>();
    const { id } = useParams();
    const navigate = useNavigate();
    const { authToken } = useAuth();

    const getSelectedItem = async () => {
        setError(null);

        try {
            if (id) {
                const { result, error } = await getItem(authToken, id);
                if (error) {
                    setError(`An error occurred while fetching item with id ${id}: ` + error);
                    return;
                }
                if (result?.Duration) {
                    const splitRes = result.Duration.split(':');
                    result.Duration = splitRes[0] + ':' + splitRes[1];
                }
                setItem(result);
            }
            else {
                setError("Could not identify the item");
            }
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const handleSubmission = async (itemInfo: ChangeableItemFields) => {
        try {
            if (id) {
                const result = await updateItem(authToken, id, itemInfo);
                if (typeof result == "string") {
                    setError("An error occurred while updating item: " + result);
                    return;
                }
                const group = itemInfo.ItemGroup ? itemInfo.ItemGroup : item?.ItemGroup;
                navigate('/item', { state: { group: group }});
            }
            else {
                setError("Could not identify the item");
            }
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const returnToList = () => {
        navigate('/item', { state: { group: item?.ItemGroup } });
    }

    const goToTaxesManagement = () => {
        navigate('/item/' + id + '/taxes');
    }

    const onDeleteItem = async () => {
        try {
            let error;
            if (id) {
                error = await deleteItem(authToken, id);
            }
            else {
                setError("Missing item id");
                return;
            }

            if (error) {
                setError("An error occurred while deleting item: " + error);
                return;
            }

            navigate('/item', { state: { group: item?.ItemGroup } });
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    useEffect(() => {
        getSelectedItem();

        if (!authToken) {
            setError("You have to authenticate first!");
        }

        window.scrollTo(0, 0);
    }, []);

    return (
        <div>
            {!error && item ? (
                <>
                    <button onClick={returnToList}>Return</button>
                    <h1>Update "{item.Name}" information</h1>
                    <ItemDataForm onSubmit={handleSubmission} item={item} submitText="Update item" />
                </>
            )
                : <p className="error-message">{error}</p>
            }

            <br /><br />
            <h2>Item variations</h2>
            {id && <ItemVariationManagementComponent itemId={id} />}
            <br/><br/>

            <button onClick={ goToTaxesManagement }>Manage item taxes</button>
            <button onClick={ onDeleteItem }>Delete item</button>
        </div>
    );
};

export default UpdateItemPage;