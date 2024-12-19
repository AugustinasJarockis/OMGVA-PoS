import React from 'react';
import { Business } from '../../../services/businessService';

interface BusinessDataFormProps {
    business?: Business;
    submitText?: string;
    onSubmit: (business: Business) => void;
    required?: boolean;
}

const BusinessDataForm: React.FC<BusinessDataFormProps> = (props: BusinessDataFormProps) => {
    const handleSubmission = async (e: React.SyntheticEvent<HTMLFormElement>) => {
        e.preventDefault();

        const form = e.currentTarget;
        const formElements = form.elements as typeof form.elements & {
            name: { value: string }
            stripeSecretKey: { value: string }
            stripePublishKey: { value: string }
            address: { value: string }
            phone: { value: string }
            email: { value: string }
        }

        const businessInfo: Business = {
            Name: formElements.name.value === '' ? undefined : formElements.name.value,
            StripeSecretKey: formElements.stripeSecretKey.value === '' ? undefined : formElements.stripeSecretKey.value,
            StripePublishKey: formElements.stripePublishKey.value === '' ? undefined : formElements.stripePublishKey.value,
            Address: formElements.address.value === '' ? undefined : formElements.address.value,
            Phone: formElements.phone.value === '' ? undefined : formElements.phone.value,
            Email: formElements.email.value === '' ? undefined : formElements.email.value
        };

        props.onSubmit(businessInfo);
    }

    return (
        <div>
            <>
                <form onSubmit={handleSubmission}>
                    <label htmlFor="name">Name</label>
                    <input type="text" id="name" name="name" placeholder={props.business?.Name} required={props.required} /><br /><br />
                    <label htmlFor="stripeSecretKey">Stripe secret key</label>
                    <input type="text" id="stripeSecretKey" name="stripeSecretKey" placeholder={props.business?.StripeSecretKey} required={props.required} /><br /><br />
                    <label htmlFor="stripePublishKey">Stripe publish key</label>
                    <input type="text" id="stripePublishKey" name="stripePublishKey" placeholder={props.business?.StripePublishKey} required={props.required} /><br /><br />
                    <label htmlFor="address">Address</label>
                    <input type="text" id="address" name="address" placeholder={props.business?.Address} required={props.required} /><br /><br />
                    <label htmlFor="phone">Phone</label>
                    <input
                        type="tel"
                        id="phone"
                        name="phone"
                        placeholder={props.business?.Phone}
                        pattern='\+?[0-9 \-]+'
                        maxLength={40}
                        required={props.required}
                        onInvalid={e => e.currentTarget.setCustomValidity('Please enter a phone number.')}
                        onInput={e => e.currentTarget.setCustomValidity('')}
                    /><br /><br />
                    <label htmlFor="email">Email</label>
                    <input type="email" id="email" name="email" placeholder={props.business?.Email} required={props.required} /><br /><br />
                    <input type="submit" value={props.submitText ? props.submitText : "Submit"} />
                </form>
            </>
        </div>
    );
};

export default BusinessDataForm;
