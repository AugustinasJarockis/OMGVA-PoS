import React from 'react';
import { Tax, TaxUpdateRequest } from '../../../services/taxService';

interface TaxDataFormProps {
    tax?: Tax;
    submitText?: string;
    onSubmit: (tax: TaxUpdateRequest) => void;
    required?: boolean;
}

const TaxDataForm: React.FC<TaxDataFormProps> = (props: TaxDataFormProps) => {
    const handleSubmission = async (e: React.SyntheticEvent<HTMLFormElement>) => {
        e.preventDefault();

        const form = e.currentTarget;
        const formElements = form.elements as typeof form.elements & {
            taxType: { value: string }
            percent: { value: string }
        }

        const taxInfo: TaxUpdateRequest = {
            taxType: formElements.taxType.value === '' ? undefined : formElements.taxType.value,
            percent: formElements.percent.value === '' ? undefined : formElements.percent.value,
        };

        props.onSubmit(taxInfo);
    }

    return (
        <div>
            <>
                <form onSubmit={handleSubmission}>
                    <label htmlFor="taxType">Tax type</label>
                    <input type="text" id="taxType" name="taxType" placeholder={props.tax?.taxType} required={props.required} /><br /><br />
                    <label htmlFor="percent">Percentage</label>
                    <input type="text"
                        id="percent"
                        name="percent"
                        pattern="[1-9][0-9]{0,3}"
                        placeholder={props.tax?.percent}
                        required={props.required}
                        onInvalid={e => e.currentTarget.setCustomValidity('Please enter a percentage.')}
                        onInput={e => e.currentTarget.setCustomValidity('')}
                    /><br /><br />
                    <input type="submit" value={props.submitText ? props.submitText : "Submit"} />
                </form>
            </>
        </div>
    );
};

export default TaxDataForm;
