import React from 'react';
import { RefundOrderRequest } from '../../../services/orderService';
import "./OrderRefundForm.css";

interface OrderRefundDataFormProps {
    submitText?: string;
    onSubmit: (refundReason: RefundOrderRequest) => void;
}

const OrderRefundDataForm: React.FC<OrderRefundDataFormProps> = (props: OrderRefundDataFormProps) => {
    const handleSubmission = async (e: React.SyntheticEvent<HTMLFormElement>) => {
        e.preventDefault();

        const form = e.currentTarget;
        const formElements = form.elements as typeof form.elements & {
            refundReason: { value: string }
        }

        const refundReason = formElements.refundReason.value;

        props.onSubmit({ RefundReason: refundReason });
    }

    return (
        <div>
            <>
                <form className="order-refund-form" onSubmit={handleSubmission}>
                    <label htmlFor="refundReason">Refund reason</label>
                    <input type="text-area"
                        id="refundReason"
                        name="refundReason"
                        placeholder="Please write the reason of refund here"
                    /><br /><br />
                    <input type="submit" value={props.submitText ? props.submitText : "Submit"} />
                </form>
            </>
        </div>
    );
};

export default OrderRefundDataForm;
