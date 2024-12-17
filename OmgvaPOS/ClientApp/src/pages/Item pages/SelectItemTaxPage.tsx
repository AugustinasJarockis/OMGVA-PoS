import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import '../../index.css';
import { useAuth } from '../../contexts/AuthContext';
import { changeItemTaxes, getItemTaxes } from '../../services/itemService';
import { getAllTaxes } from '../../services/taxService';
import './ItemPages.css';

const SelectItemTaxPage: React.FC = () => {
    const [listItems, setListItems] = useState<Array<JSX.Element>>();
    const [originalValues, setOriginalValues] = useState<Array<{id: string, applied: boolean }>>();
    const [error, setError] = useState<string | null>(null)
    const { id } = useParams();
    const navigate = useNavigate();
    const { authToken } = useAuth();

    const loadTaxes = async () => {
        setError(null);

        try {
            if (!id) {
                setError('Could not identify the business');
                return;
            }

            const { result, error } = await getItemTaxes(authToken, id);
            const { result: allTaxResult, error: allTaxError } = await getAllTaxes(authToken);

            if (!result || !allTaxResult) {
                if (!result)
                    setError('Problem acquiring item taxes: ' + error);
                if (!allTaxResult)
                    setError('Problem acquiring item taxes: ' + allTaxError);
            } else {
                setListItems(allTaxResult.map(tax =>
                    <div className="check-box-list-item" key={tax.Id}>
                        <input type="checkbox" name={tax.Id} id={tax.Id} defaultChecked={result.map(t => t.Id).includes(tax.Id)} />
                        <label htmlFor={tax.Id}>{tax.TaxType}: {tax.Percent}%</label>
                        <br />
                    </div>));

                setOriginalValues(allTaxResult.map(tax => { return { id: tax.Id, applied: result.map(t => t.Id).includes(tax.Id) } }));
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    };

    const returnToItem = () => {
        navigate('/item/' + id);
    }

    const handleSubmittion = async (e: React.SyntheticEvent<HTMLFormElement>) => {
        e.preventDefault();

        const formData = new FormData(e.currentTarget);

        const taxToAdd = [];
        const taxToRemove = [];

        if (originalValues) {
            for (const originalValue of originalValues) {
        
                if ((formData.get(originalValue.id) as string === 'on') !== originalValue.applied) {
                    if (originalValue.applied) {
                        taxToRemove.push(originalValue.id);
                    }
                    else {
                        taxToAdd.push(originalValue.id);
                    }
                }
            }
        }

        if (id) {
            const { result, error } = await changeItemTaxes(authToken, id, { TaxesToAddIds: taxToAdd, TaxesToRemoveIds: taxToRemove });

            if (!result) {
                setError('Problem changing item taxes: ' + error);
            }
            else {
                navigate('/item/' + result.Id);
            }
        }
        else {
            setError("Id of the item is missing");
        }
    }

    useEffect(() => {
        loadTaxes();

        if (!authToken) {
            setError("You have to authenticate first!");
        }
    }, [authToken, id, navigate]);

    return (
        <div>
            <h1>Taxes</h1>
            <form onSubmit={handleSubmittion}>
                {listItems}
                <br/>
                <input type="submit" value="Save changes" />
                <br/>
                <br/>
                <button onClick={returnToItem}>Return</button>
            </form>
            {error}
        </div>
    );
};

export default SelectItemTaxPage;
