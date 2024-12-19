import axios from 'axios';

export interface Payment {
    Id?: string,
    Method?: string,
    CustomerId?: number,
    OrderId?: number,
    Amount?: number,
    GiftCardId?: number
}

export const getPayments = async (token: string | null): Promise<{ result?: Array<Payment>, error?: string} > => {
    try {
        const response = await axios.get('/api/payment', {
            headers: { Authorization: `Bearer ${token}`},
        });
        if (response.status === 200) {
            return { result: response.data };
        } else {
            return { error: response.data.message };
        }
    } catch (error: any) {
        return { error: error.message || 'An unexpected error occurred.' };
    }
}

export const createPayment = async (token: string | null, payment: Payment): Promise<{ result?: Payment, error?: string }> => {
    switch (payment.Method) {
        case 'cash':
            return createCashPayment(token, payment);
        case 'giftcard':
            return createGiftcardPayment(token, payment);
        case 'card':
            // Card payment will need a PaymentMethodId from Stripe.js front-end
            // We'll handle calling createCardPayment in PaymentModal after we get the PaymentMethodId.
            // For now, just return an error if called directly.
            return { error: 'Card payment requires a payment method id.' };
        default:
            return { error: 'Invalid payment method.' };
    }
}

export const createCardPayment = async (token: string | null, payment: Payment, paymentMethodId: string): Promise<{ result?: Payment, error?: string }> => {
    try {
        const response = await axios.post('/api/payment/process-card', {
            PaymentMethodId: paymentMethodId,
            OrderId: payment.OrderId,
            Amount: payment.Amount,
            CustomerId: payment.CustomerId
        }, {
            headers: { Authorization: `Bearer ${token}` },
        });
        if (response.status === 200) {
            return { result: response.data };
        } else {
            return { error: response.data.error || 'An error occurred' };
        }
    } catch (error: any) {
        return { error: error.message || 'An unexpected error occurred.' };
    }
}

export const createCashPayment = async (token: string | null, payment: Payment): Promise<{ result?: Payment, error?: string }> => {
    try {
        const response = await axios.post('/api/payment/process-cash', payment, {
            headers: { Authorization: `Bearer ${token}` },
        });
        if (response.status === 200) {
            return { result: response.data.payment };
        } else {
            return { error: response.data.message };
        }
    } catch (error: any) {
        return { error: error.message || 'An unexpected error occurred.' };
    }
}

export const createGiftcardPayment = async (token: string | null, payment: Payment): Promise<{ result?: Payment, error?: string }> => {
    try {
        const response = await axios.post('/api/payment/process-giftcard', payment, {
            headers: { Authorization: `Bearer ${token}` },
        });
        if (response.status === 200) {
            return { result: response.data.payment };
        } else {
            return { error: response.data.message };
        }
    } catch (error: any) {
        if (error.response && error.response.status === 404) {
            // Giftcard not found or invalid
            return { error: "Giftcard not found or invalid" };
        }
        return { error: error.message || 'An unexpected error occurred.' };
    }
}
