import React from 'react';
import { Discount, DiscountCreateRequest, DiscountType } from '../../../services/discountService';

interface DiscountCreationDataFormProps {
    discount?: Discount;
    orderId?: string;
    submitText?: string;
    onSubmit: (discount: DiscountCreateRequest) => void;
    type: DiscountType;
}

const DiscountCreationDataForm: React.FC<DiscountCreationDataFormProps> = (props: DiscountCreationDataFormProps) => {
    const handleSubmission = async (e: React.SyntheticEvent<HTMLFormElement>) => {
        e.preventDefault();

        const form = e.currentTarget;
        const formElements = form.elements as typeof form.elements & {
            amount: { value: string }
            timeValidUntil: { value: string }
        }

        const discountInfo: DiscountCreateRequest = {
            Amount: +formElements.amount.value,
            TimeValidUntil: formElements.timeValidUntil.value.replace(' ', 'T') + ':00',
            Type: props.type,
            OrderId: props.orderId
        };

        props.onSubmit(discountInfo);
    }

    return (
        <div>
            <>
                <form onSubmit={handleSubmission}>
                    <label htmlFor="amount">Amount</label>
                    <input type="text" id="amount" name="amount" placeholder={props.discount?.Amount} required /><br /><br />
                    <label htmlFor="timeValidUntil">Valid until</label>
                    <input type="text"
                        id="timeValidUntil"
                        name="timeValidUntil"
                        pattern="[0-9]{4}-(1[0-2]|0[1-9])-(0[1-9]|[1-2][0-9]|3[0-1]) ([0-1][0-9]|2[0-4]):[0-5][0-9]"
                        placeholder={props.discount?.TimeValidUntil}
                        required
                        onInvalid={e => e.currentTarget.setCustomValidity('Please enter a date and time in format YYYY-MM-DD hh:mm.')}
                        onInput={e => e.currentTarget.setCustomValidity('')}
                    /><br /><br />
                    <input type="submit" value={props.submitText ? props.submitText : "Submit"} />
                </form>
            </>
        </div>
    );
};

export default DiscountCreationDataForm;
