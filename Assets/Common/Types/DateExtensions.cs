using Common.Infrastructure;

namespace Common.Types
{
    public static class DateExtensions
    {
        public static int DiffInDays(Date date1, Date date2)
        {
            var yearDiff = date1.Year - date2.Year;
            var dayDiff = date1.Day - date2.Day;
            return yearDiff * DateModel.LastDayOfYear + dayDiff;
        }
    }
}