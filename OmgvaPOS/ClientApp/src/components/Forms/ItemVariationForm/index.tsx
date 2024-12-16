import React from 'react';
import '../../../pages/Item pages/ItemPages.css';
import { ChangeableItemVariationFields, ItemVariation } from '../../../services/itemVariationService';

interface ItemVariationDataFormProps {
    itemVariation?: ItemVariation;
    submitText?: string;
    onSubmit: (item: ChangeableItemVariationFields) => void;
    required?: boolean;
}

const ItemVariationDataForm: React.FC<ItemVariationDataFormProps> = (props: ItemVariationDataFormProps) => {

    const handleSubmission = async (e: React.SyntheticEvent<HTMLFormElement>) => {
        e.preventDefault();

        const form = e.currentTarget;
        const formElements = form.elements as typeof form.elements & {
            name: { value: string }
            inventoryQuantity: { value: string }
            priceChange: { value: string }
            itemVariationGroup: { value: string }
        }

        const itemVariationInfo: ChangeableItemVariationFields = {
            Name: formElements.name.value === '' ? undefined : formElements.name.value,
            InventoryQuantity: formElements.inventoryQuantity.value === '' ? undefined : +formElements.inventoryQuantity.value,
            PriceChange: formElements.priceChange.value === '' ? undefined : +formElements.priceChange.value,
            ItemVariationGroup: formElements.itemVariationGroup.value === '' ? undefined : formElements.itemVariationGroup.value
        };

        props.onSubmit(itemVariationInfo);
    }

    return (
        <div>
            <>
                <form onSubmit={handleSubmission}>
                    <label htmlFor="name">Name</label>
                    <input type="text" id="name" name="name" placeholder={props.itemVariation?.Name} required={props.required} /><br /><br />
                    <label htmlFor="inventoryQuantity">Inventory quantity</label>
                    <input
                        type="number"
                        id="inventoryQuantity"
                        name="inventoryQuantity"
                        placeholder={props.itemVariation?.InventoryQuantity ? String(props.itemVariation?.InventoryQuantity) : undefined}
                        required={props.required}
                        min='0'
                    /><br /><br />
                    <label htmlFor="priceChange">Price change</label>
                    <input
                        type="text"
                        id="priceChange"
                        name="priceChange"
                        placeholder={props.itemVariation?.PriceChange ? String(props.itemVariation?.PriceChange) : undefined}
                        pattern="-?[0-9]+(.[0-9][0-9]?)?"
                        required={props.required}
                        onInvalid={e => e.currentTarget.setCustomValidity('Please enter a price change.')}
                        onInput={e => e.currentTarget.setCustomValidity('')}
                    /><br /><br />
                    <label htmlFor="itemVariationGroup">Item variation group</label>
                    <input
                        type="text"
                        id="itemVariationGroup"
                        name="itemVariationGroup"
                        placeholder={props.itemVariation?.ItemVariationGroup}
                        required={props.required}
                    /><br /><br />
                    
                    <input type="submit" value={props.submitText ? props.submitText : "Submit"} />
                </form>
            </>
        </div>
    );
};

export default ItemVariationDataForm;
