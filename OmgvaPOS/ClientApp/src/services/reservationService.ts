import axios from "axios";

export interface Reservation {
  Id: number;
  TimeCreated: Date;
  TimeReserved: string;
  Status: number;
  EmployeeId: number;
  CustomerId: number;
  EmployeeName?: string;
  CustomerName?: string;
}
export interface CreateReservation {
    TimeReserved: Date;
    EmployeeId: number;
    CustomerId: number;
    ItemId: number;
}
export interface UpdateReservation {
    TimeReserved?: Date;
    EmployeeId?: number;
    CustomerId?: number;
    Status?: number;
}

 export const statusMap: { [key: string]: string } = {
        2: 'Done',
        1: 'Cancelled',
        0: 'Open',
 };

const createReservation = async (token: string | null, reservation: CreateReservation): Promise<{ error?: string, result?: Reservation }> => {
    try {
        const response = await axios.post(`/api/reservation`, reservation, {
            headers: { Authorization: `Bearer ${token}` }
        });
        if (response.status === 201) {
            return { result: response.data };
        } else {
            return { error: response.data.message };
        }
    } catch (error: any) {
        return { error: error.message || 'An unexpected error occurred.' };
    }
};

const getReservation = async (token: string | null, id: string): Promise<{ result?: Reservation, error?: string }> => {
    try {
        const response = await axios.get(`/api/reservation/${id}`, {
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
};

const updateReservation = async (token: string | null, id: string,  reservation: UpdateReservation): Promise<{ result?: Reservation, error?: string }> => {
    try {
        const response = await axios.put(`/api/reservation/${id}`, reservation, {
            headers: { Authorization: `Bearer ${token}` },
        });
        if (response.status === 200) {
            return { result: response.data };
        } else {
            return response.data.message;
        }
    } catch (error: any) {
        return error.message || 'An unexpected error occurred.';
    }
};

const deleteReservation = async (token: string | null, id: string): Promise<string | undefined> => {
    try {
        const response = await axios.delete(`/api/reservation/${id}`, {
            headers: { Authorization: `Bearer ${token}` },
        });
        if (response.status === 200) {
            return undefined;
        } else {
            return response.data.message;
        }
    } catch (error: any) {
        return error.message || 'An unexpected error occurred.';
    }
};

const getBusinessReservations = async (token: string | null, id: string): Promise<{ result?: Array<Reservation>, error?: string }> => {
    try {
        const response = await axios.get(`/api/reservation/business/${id}`, {
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
};

const getEmployeeReservations = async (token: string | null, id: string): Promise<{ result?: Array<Reservation>, error?: string }> => {
    try {
        const response = await axios.get(`/api/reservation/employee/${id}`, {
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
};

export { createReservation, getReservation, updateReservation, deleteReservation, getBusinessReservations, getEmployeeReservations }