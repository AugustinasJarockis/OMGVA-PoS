import axios from "axios";

export interface EmployeeSchedule {
    Id: number;
    EmployeeId: number;
    Date: Date;
    StartTime: string;
    EndTime: string;
    IsCancelled: boolean;
}
export interface EmployeeSchedulesWithAvailability {
    EmployeeId: number;
    EmployeeName: string;
    ScheduleWithAvailability: Array<ScheduleWithAvailability>;
}
export interface ScheduleWithAvailability {
    EmployeeScheduleId: number;
    Date: string;
    StartTime: string;
    EndTime: string;
    AvailableTimeslots: Array<Timeslot>;
}
export interface Timeslot {
    StartTime: string;
    EndTime: string;
}
export interface CreateEmployeeSchedule {
    EmployeeId: number;
    Date: string;
    StartTime: string;
    EndTime: string;
}
export interface UpdateEmployeeSchedule {
    StartTime?: string;
    EndTime?: string;
}

const createEmployeeSchedule = async (token: string | null, schedule: CreateEmployeeSchedule): Promise<{ error?: string, result?: EmployeeSchedule }> => {
    try {
        const response = await axios.post(`/api/schedules`, schedule, {
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

const getEmployeeSchedule = async (token: string | null, id: string): Promise<{ result?: ScheduleWithAvailability, error?: string }> => {
    try {
        const response = await axios.get(`/api/schedules/${id}`, {
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

const UpdateEmployeeSchedule = async (token: string | null, id: string,  schedule: UpdateEmployeeSchedule): Promise<{ result?: EmployeeSchedule, error?: string }> => {
    try {
        const response = await axios.put(`/api/schedules/${id}`, schedule, {
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

const deleteEmployeeSchedule = async (token: string | null, id: string): Promise<string | undefined> => {
    try {
        const response = await axios.delete(`/api/schedules/${id}`, {
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

const getEmployeeSchedulesByItemAndDate = async (token: string | null, itemId: string, date: string): Promise<{ result?: EmployeeSchedulesWithAvailability, error?: string }> => {
    try {
        const response = await axios.get(`/api/schedules?itemId=${itemId}&date=${date}`, {
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

const getEmployeeSchedulesWithAvailability = async (token: string | null, employeeId: string, date: string): Promise<{ result?: EmployeeSchedulesWithAvailability, error?: string }> => {
    try {
        const response = await axios.get(`/api/schedules?employeeId=${employeeId}&date=${date}`, {
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

const getAllSchedulesByEmployeeId = async (token: string | null, id: string): Promise<{ result?: Array<EmployeeSchedule>, error?: string }> => {
    try {
        const response = await axios.get(`/api/schedules/employee/${id}`, {
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

export { createEmployeeSchedule, getEmployeeSchedule, UpdateEmployeeSchedule, deleteEmployeeSchedule, getEmployeeSchedulesByItemAndDate, getEmployeeSchedulesWithAvailability, getAllSchedulesByEmployeeId }