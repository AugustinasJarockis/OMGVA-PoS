import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import '../../index.css';
import { useAuth } from '../../contexts/AuthContext';
import { ChangeableItemVariationFields, ItemVariation, getItemVariation, updateItemVariation } from '../../services/itemVariationService';
import ItemVariationDataForm from '../../components/Forms/ItemVariationForm';

const UpdateItemVariationPage: React.FC = () => {
    const [error, setError] = useState<string | null>(null);
    const { itemId, id } = useParams();
    const [itemVariation, setItemVariation] = useState<ItemVariation>();
    const navigate = useNavigate();
    const { authToken } = useAuth();

    const handleSubmission = async (itemVariationInfo: ChangeableItemVariationFields) => {
        try {
            if (!id) {
                setError("Item id is missing");
                return;
            }
            const result = await updateItemVariation(authToken, id, itemVariationInfo);

            if (typeof result === 'string') {
                setError("An error occurred while updating the item variation: " + error);
                return;
            }

            navigate('/item/' + itemId);
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const loadItemVariation = async () => {
        setError(null);

        try {
            if (!id) {
                setError('Could not identify the item variation');
                return;
            }

            const { result, error } = await getItemVariation(authToken, id);

            if (!result) {
                setError('Problem acquiring item variation: ' + error);
            } else {
                setItemVariation(result);
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    };

    const returnToItem = async () => {
        navigate(-1);
    }

    useEffect(() => {
        if (!authToken) {
            setError("You have to authenticate first!");
        }
        else {
            loadItemVariation();
        }
    }, []);

    return (
        <div>
            <header>
                <button onClick={returnToItem}>Return</button>
            </header>
            <h1> Update item variation </h1>
            {
                !error ? (
                    <>
                        <ItemVariationDataForm onSubmit={handleSubmission} submitText="Update variation" itemVariation={ itemVariation } />
                    </>
                )
                    : <p className="error-message" >{error}</p>
            }
        </div>
    );
};

export default UpdateItemVariationPage;

