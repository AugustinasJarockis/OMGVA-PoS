import React, { useState, useEffect } from 'react';
import { loadStripe, Stripe } from '@stripe/stripe-js';
import { Elements, CardElement, useStripe, useElements, CardElementComponent } from '@stripe/react-stripe-js';

interface PublishableKeyResponse {
    publishableKey: string;
}

interface ProcessPaymentResponse {
    requiresAction?: boolean;
    paymentIntentClientSecret?: string;
    success?: boolean;
    error?: string;
}

const CheckoutForm: React.FC = () => {
    const stripe = useStripe();
    const elements = useElements();
    const [error, setError] = useState<string | null>(null);

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        if (!stripe || !elements) return;

        const cardElement = elements.getElement(CardElement);
        if (!cardElement) return;

        const { error: pmError, paymentMethod } = await stripe.createPaymentMethod({
            type: 'card',
            card: cardElement,
        });

        if (pmError) {
            setError(pmError.message || 'Unknown error creating payment method');
            return;
        }

        const response = await fetch('http://localhost:9900/checkout', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ paymentMethodId: paymentMethod?.id, amount: 2000 })
        });

        const data: ProcessPaymentResponse = await response.json();

        if (data.error) {
            setError(data.error);
        } else if (data.requiresAction && data.paymentIntentClientSecret) {
            const { error: confirmError, paymentIntent } = await stripe.confirmCardPayment(data.paymentIntentClientSecret);
            if (confirmError) {
                setError(confirmError.message || 'Unknown error confirming payment');
            } else if (paymentIntent && paymentIntent.status === 'succeeded') {
                alert("Payment succeeded!");
            }
        } else if (data.success) {
            alert("Payment succeeded!");
        }
    };

    return (
        <form onSubmit={handleSubmit}>
        <CardElement />
        <button type="submit" disabled={!stripe}>Pay $20.00</button>
    {error && <div style={{color:'red'}}>{error}</div>}
    </form>
    );
    };

    const PaymentForm: React.FC = () => {
        const [stripePromise, setStripePromise] = useState<Promise<Stripe | null> | null>(null);

        useEffect(() => {
            async function fetchPublishableKey() {
                const res = await fetch('http://localhost:9900/stripekeys/publishableKey');
                const data: PublishableKeyResponse = await res.json();
                setStripePromise(loadStripe(data.publishableKey));
            }
            fetchPublishableKey();
        }, []);

        if (!stripePromise) {
            return <div>Loading...</div>;
        }

        return (
            <Elements stripe={stripePromise}>
                <CheckoutForm />
                </Elements>
        );
    }

    export default PaymentForm;
