using System.Runtime.CompilerServices;

namespace OmgvaPOS.src.Validators
{
    public static class DateValidators
    {
        public static bool IsDateDayInPast(this DateTime date) {
            return date.Date < DateTime.Today;
        }

        public static bool IsDateDayToday(this DateTime date) {
            return date.Date == DateTime.Today;
        }

        public static bool IsDateDayInFuture(this DateTime date) {
            return date.Date > DateTime.Today;
        }
    }
}
