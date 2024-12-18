using OmgvaPOS.Exceptions;

namespace OmgvaPOS.ScheduleManagement.Validators
{
    public static class EmployeeScheduleValidator
    {
        public static void AreStartToEndDatesValid(TimeSpan start, TimeSpan end)
        {
            if (start >= end)
                throw new ValidationException("Start time cannot be later than the end time.");
        }
        public static void IsValidStartTime(TimeSpan reservationStart, TimeSpan timeToUpdate)
        {
            if (timeToUpdate > reservationStart)
                throw new ValidationException("Start time cannot be later than the first reservation's start time.");
        }
        public static void IsValidEndTime(TimeSpan reservationEnd, TimeSpan timeToUpdate)
        {
            if (reservationEnd > timeToUpdate)
                throw new ValidationException("End time cannot be earlier than the last reservation's end time.");
        }
    }
}
