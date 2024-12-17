import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import '../../index.css';
import { useAuth } from '../../contexts/AuthContext';
import { ChangeableItemVariationFields, CreateItemVariationRequest, createItemVariation } from '../../services/itemVariationService';
import ItemVariationDataForm from '../../components/Forms/ItemVariationForm';

const CreateItemVariationPage: React.FC = () => {
    const [error, setError] = useState<string | null>(null);
    const { id } = useParams();
    const navigate = useNavigate();
    const { authToken } = useAuth();

    const handleSubmission = async (itemVariationInfo: ChangeableItemVariationFields) => {
        try {
            let itemVariationCreateRequest: CreateItemVariationRequest;
            if (itemVariationInfo.InventoryQuantity
                && itemVariationInfo.Name
                && itemVariationInfo.PriceChange
                && itemVariationInfo.ItemVariationGroup) {
                itemVariationCreateRequest = {
                    Name: itemVariationInfo.Name,
                    InventoryQuantity: itemVariationInfo.InventoryQuantity,
                    PriceChange: itemVariationInfo.PriceChange,
                    ItemVariationGroup: itemVariationInfo.ItemVariationGroup
                }
            }
            else {
                setError("Invalid form data");
                return;
            }
            if (!id) {
                setError("Item id is missing");
                return;
            }
            const { error, result } = await createItemVariation(authToken, itemVariationCreateRequest, id);

            if (error || !result) {
                setError("An error occurred while creating the item variation: " + error);
                return;
            }

            navigate('/item/' + id);
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const returnToItem = async () => {
        navigate(-1);
    }

    useEffect(() => {
        if (!authToken) {
            setError("You have to authenticate first!");
        }
    }, []);

    return (
        <div>
            <header>
                <button onClick={returnToItem}>Return</button>
            </header>
            <h1> Create a new item variation </h1>
            {
                !error ? (
                    <>
                        <ItemVariationDataForm onSubmit={handleSubmission} submitText="Create" required />
                    </>
                )
                    : <p className="error-message" >{error}</p>
            }
        </div>
    );
};

export default CreateItemVariationPage;

