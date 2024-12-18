import React, { useEffect, useState } from 'react';
import { ChangeableItemFields, Item } from '../../../services/itemService';
import '../../../pages/Item pages/ItemPages.css';
import UserSelector from '../../Selectors/UserSelector';
import DiscountSelector from '../../Selectors/DiscountSelector';

interface ItemDataFormProps {
    item?: Item;
    itemGroup?: string;
    submitText?: string;
    onSubmit: (item: ChangeableItemFields) => void;
    required?: boolean;
}

const ItemDataForm: React.FC<ItemDataFormProps> = (props: ItemDataFormProps) => {
    const [isService, setIsService] = useState<boolean>(false);

    const handleSubmission = async (e: React.SyntheticEvent<HTMLFormElement>) => {
        e.preventDefault();

        const form = e.currentTarget;
        const formElements = form.elements as typeof form.elements & {
            name: { value: string }
            inventoryQuantity: { value: string }
            price: { value: string }
            currency: { value: string }
            itemGroup: { value: string }
            imgPath: { value: string }
            discount: { value: string }
            duration: { value: string }
            userId: { value: string }
        }

        const itemInfo: ChangeableItemFields = {
            Name: formElements.name.value === '' ? undefined : formElements.name.value,
            InventoryQuantity: formElements.inventoryQuantity.value === '' ? undefined : +formElements.inventoryQuantity.value,
            Price: formElements.price.value === '' ? undefined : +formElements.price.value,
            Currency: formElements.currency.value === '' ? undefined : formElements.currency.value,
            ItemGroup: props.itemGroup ? props.itemGroup : (formElements.itemGroup.value === '' ? undefined : formElements.itemGroup.value),
            ImgPath: formElements.imgPath.value === '' ? undefined : formElements.imgPath.value,
            DiscountId: formElements.discount.value === '' ? undefined : formElements.discount.value.replace(' ', 'T'),
            Duration: !formElements.duration || formElements.duration.value === '' ? undefined : ('0' + formElements.duration.value + ':00'),
            UserId: !formElements.userId || formElements.userId.value === '' ? undefined : formElements.userId.value,
        };

        props.onSubmit(itemInfo);
    }

    const changeItemType = () => {
        if (isService) {
            setIsService(false);
        }
        else {
            setIsService(true);
        }
        
    }

    useEffect(() => {
        if (props.item?.UserId) {
            setIsService(true);
        }
    }, []);

    return (
        <div>
            <>
                <form onSubmit={handleSubmission}>
                    <label htmlFor="name">Name</label>
                    <input type="text" id="name" name="name" placeholder={props.item?.Name} required={props.required} /><br /><br />
                    <label htmlFor="inventoryQuantity">Inventory quantity</label>
                    <input
                        type="number"
                        id="inventoryQuantity"
                        name="inventoryQuantity"
                        placeholder={props.item?.InventoryQuantity ? String(props.item?.InventoryQuantity) : undefined}
                        required={props.required}
                        min='0'
                    /><br /><br />
                    <label htmlFor="price">Price</label>
                    <input
                        type="text"
                        id="price"
                        name="price"
                        placeholder={props.item?.Price ? String(props.item?.Price) : undefined}
                        pattern="[0-9]+(.[0-9][0-9]?)?"
                        required={props.required}
                        onInvalid={e => e.currentTarget.setCustomValidity('Please enter a price.')}
                        onInput={e => e.currentTarget.setCustomValidity('')}
                    /><br /><br />
                    <label htmlFor="currency">Currency</label>
                    <input
                        type="text"
                        id="currency"
                        name="currency"
                        pattern="[a-zA-z]{3}"
                        placeholder={props.item?.Currency}
                        required={props.required}
                        onInvalid={e => e.currentTarget.setCustomValidity('Please enter a currency type. The type has to consist of three letters.')}
                        onInput={e => e.currentTarget.setCustomValidity('')}
                    /><br /><br />
                    {!props.itemGroup && (
                        <>
                            <label htmlFor="itemGroup">Item group</label>
                            <input
                                type="text"
                                id="itemGroup"
                                name="itemGroup"
                                placeholder={props.item?.ItemGroup}
                                required={props.required}
                            /><br /><br />
                        </>
                        )
                    }
                    <label htmlFor="imgPath">Path to image</label>
                    <input
                        type="text"
                        id="imgPath"
                        name="imgPath"
                        placeholder={props.item?.ImgPath}
                        required={props.required}
                    /><br /><br />
                    <label>Discount</label>
                    <DiscountSelector current={ props.item?.DiscountId } />
                    <br /><br />
                    <div className="mid-form-input">
                        Is service:
                        <input type="checkbox" onClick={changeItemType} checked={ isService } readOnly />
                    </div><br />
                    {isService &&
                    <>
                        <label htmlFor="duration">Duration</label>
                        <input
                            type="text"
                            id="duration"
                            name="duration"
                            pattern="[0-7]{1}:[0-9]{2}"
                            placeholder={props.item?.Duration}
                            required={props.required}
                            onInvalid={e => e.currentTarget.setCustomValidity('Please enter the duration in format H:mm.')}
                            onInput={e => e.currentTarget.setCustomValidity('')}
                        /><br /><br />
                        <label>Service provider</label>
                        <UserSelector current={props.item?.UserId} required={props.required} /><br/><br/>
                    </>
                    }
                    <input type="submit" value={props.submitText ? props.submitText : "Submit"} />
                </form>
            </>
        </div>
    );
};

export default ItemDataForm;
