import React from 'react';

interface DiscountUpdateDataFormProps {
    currentValidUntil?: string;
    submitText?: string;
    onSubmit: (newValidUntil: string) => void;
}

const DiscountUpdateDataForm: React.FC<DiscountUpdateDataFormProps> = (props: DiscountUpdateDataFormProps) => {
    const handleSubmission = async (e: React.SyntheticEvent<HTMLFormElement>) => {
        e.preventDefault();

        const form = e.currentTarget;
        const formElements = form.elements as typeof form.elements & {
            timeValidUntil: { value: string }
        }

        const newValidUntil = formElements.timeValidUntil.value.replace(' ', 'T') + ':00';

        props.onSubmit(newValidUntil);
    }

    return (
        <div>
            <>
                <form onSubmit={handleSubmission}>
                    <label htmlFor="timeValidUntil">Valid until</label>
                    <input type="text"
                        id="timeValidUntil"
                        name="timeValidUntil"
                        pattern="[0-9]{4}-(1[0-2]|0[1-9])-(0[1-9]|[1-2][0-9]|3[0-1]) ([0-1][0-9]|2[0-4]):[0-5][0-9]"
                        placeholder={props.currentValidUntil?.replace('T', ' ')}
                        onInvalid={e => e.currentTarget.setCustomValidity('Please enter a date and time in format YYYY-MM-DD hh:mm.')}
                        onInput={e => e.currentTarget.setCustomValidity('')}
                    /><br /><br />
                    <input type="submit" value={props.submitText ? props.submitText : "Submit"} />
                </form>
            </>
        </div>
    );
};

export default DiscountUpdateDataForm;
