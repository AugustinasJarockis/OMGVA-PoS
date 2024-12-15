import React, { useState } from 'react';
import { CreateGiftcard } from '../../../services/giftcardService';
import { getTokenBusinessId } from '../../../utils/tokenUtils';

interface CreateGiftcardFormProps {
    onSubmitCreate: (giftcard: CreateGiftcard) => Promise<void>;
    token: string;
    submitText: string;
}

const CreateGiftcardForm: React.FC<CreateGiftcardFormProps> = (props: CreateGiftcardFormProps) => {
    const [errors, setErrors] = useState<any>({});

    const handleSubmission = async (e: React.SyntheticEvent<HTMLFormElement>) => {
        e.preventDefault();

        const form = e.currentTarget;
        const formElements = form.elements as typeof form.elements & {
            value: { value: number }
        };

        const giftcard: CreateGiftcard = {
            BusinessId: getTokenBusinessId(props.token),
            Value: formElements.value.value
        };

        const newErrors: any = {};
        if (Object.keys(newErrors).length > 0) {
            setErrors(newErrors);
        } else {
            setErrors({});
            if (props.onSubmitCreate) {
                props.onSubmitCreate(giftcard);
            }
        }
    };

    return (
        <div>
            <form onSubmit={handleSubmission}>
                <label htmlFor="giftcardValue">Giftcard value</label>
                <input type="number" id="value" name="value" min="0" max="1000" required />
                <br /><br />
                <input type="submit" value={props.submitText} />
            </form>
        </div>
    );
};

export default CreateGiftcardForm;
