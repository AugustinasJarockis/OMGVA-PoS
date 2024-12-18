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
        // case 'giftcard':
        //     return createGiftcardPayment(token, payment);
        // case 'card':
        //     return createCardPayment(token, payment);
        default:
            return { error: 'Invalid payment method.' };
    }
}

export const createCashPayment = async (token: string | null, payment: Payment): Promise<{ result?: Payment, error?: string }> => {
    try {
        const response = await axios.post('/api/payment/process-cash', payment, {
            headers: { Authorization: `Bearer ${token}` },
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