import React from 'react';
import { EmployeeSchedulesWithAvailability, ScheduleWithAvailabilities, Timeslot } from '../../services/scheduleService';
import './ScheduleViewer.css';

interface ScheduleViewerProps {
  scheduleWithAvailability?: ScheduleWithAvailabilities;
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
        <p><strong>Date:</strong> {scheduleWithAvailability.Date}</p>
        <p><strong>Start Time:</strong> {scheduleWithAvailability.StartTime}</p>
        <p><strong>End Time:</strong> {scheduleWithAvailability.EndTime}</p>
        {scheduleWithAvailability.AvailableTimeslots && scheduleWithAvailability.AvailableTimeslots.length > 0 ? (
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
            {employeeSchedulesWithAvailability.ScheduleWithAvailabilities && employeeSchedulesWithAvailability.ScheduleWithAvailabilities.length > 0 ? (
            employeeSchedulesWithAvailability.ScheduleWithAvailabilities.map((schedule) => (
                <div key={schedule.EmployeeScheduleId} className="schedule-item">
                <p><strong>Date:</strong> {schedule.Date}</p>
                <p><strong>Start Time:</strong> {schedule.StartTime}</p>
                <p><strong>End Time:</strong> {schedule.EndTime}</p>
                {schedule.AvailableTimeslots && schedule.AvailableTimeslots.length > 0 ? (
                    <div>
                    <h4 className="timeslot-header">Available Timeslots:</h4>
                    <ul className="timeslot-list">
                        {schedule.AvailableTimeslots.map((timeslot, index) => (
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
            ))
            ) : (
            <p>No schedules found for this employee.</p>
            )}
        </div>
        </div>
    );
  }

  return <p className="no-data">No data available to display.</p>;
};

export default ScheduleViewer;
