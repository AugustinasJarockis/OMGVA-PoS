import React from 'react';
import './ScheduleViewer.css';
import { EmployeeSchedulesWithAvailability, ScheduleWithAvailability, Timeslot } from '../../services/scheduleService';

interface ScheduleViewerProps {
    scheduleWithAvailability?: ScheduleWithAvailability;
    employeeSchedulesWithAvailability?: EmployeeSchedulesWithAvailability;
}

const ScheduleViewer: React.FC<ScheduleViewerProps> = ({
    scheduleWithAvailability,
    employeeSchedulesWithAvailability,
}) => {
    if (scheduleWithAvailability) {
        return (
            <div className="schedule-viewer">
                <h2 className="schedule-header">Schedule with Availability</h2>
                <p><strong>Date:</strong> {new Date(scheduleWithAvailability.Date).toLocaleDateString('en-US', { 
                    weekday: 'short', 
                    year: 'numeric', 
                    month: 'short', 
                    day: 'numeric' 
                })}</p>                <p><strong>Start Time:</strong> {scheduleWithAvailability.StartTime}</p>
                <p><strong>End Time:</strong> {scheduleWithAvailability.EndTime}</p>
                {scheduleWithAvailability.AvailableTimeslots?.length ? (
                    <div>
                        <h3 className="timeslot-header">Available Timeslots:</h3>
                        <ul className="timeslot-list">
                            {scheduleWithAvailability.AvailableTimeslots.map((timeslot: Timeslot, index: number) => (
                                <li key={index} className="timeslot-item">
                                    {timeslot.StartTime} - {timeslot.EndTime}
                                </li>
                            ))}
                        </ul>
                    </div>
                ) : (
                    <p className="no-timeslots">No available timeslots.</p>
                )}
            </div>
        );
    }

    if (employeeSchedulesWithAvailability) {
        return (
            <div className="schedule-viewer">
                <h2 className="schedule-header">Employee Schedules with Availability</h2>
                <p><strong>Employee Name:</strong> {employeeSchedulesWithAvailability.EmployeeName}</p>
                <div>
                    <h3 className="schedule-header">Schedules:</h3>
                    {employeeSchedulesWithAvailability.ScheduleWithAvailability.map((schedule, index) => (
                        <div key={index} className="schedule-item">
                        <p><strong>Date:</strong> {new Date(schedule.Date).toLocaleDateString('en-US', { 
                            weekday: 'short', 
                            year: 'numeric', 
                            month: 'short', 
                            day: 'numeric' 
                        })}</p>
                            <p><strong>Start Time:</strong> {schedule.StartTime}</p>
                            <p><strong>End Time:</strong> {schedule.EndTime}</p>
                            {schedule.AvailableTimeslots?.length ? (
                                <div>
                                    <h4 className="timeslot-header">Available Timeslots:</h4>
                                    <ul className="timeslot-list">
                                        {schedule.AvailableTimeslots.map((timeslot: Timeslot, slotIndex: number) => (
                                            <li key={slotIndex} className="timeslot-item">
                                                {timeslot.StartTime} - {timeslot.EndTime}
                                            </li>
                                        ))}
                                    </ul>
                                </div>
                            ) : (
                                <p className="no-timeslots">No available timeslots.</p>
                            )}
                        </div>
                    ))}
                </div>
            </div>
        );
    }

    return <p className="no-data">No data available to display.</p>;
};

export default ScheduleViewer;
