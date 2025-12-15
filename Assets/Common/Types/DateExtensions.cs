namespace Common.Types
{
    public static class DateExtensions
    {
        public static void AddDays(this Date date, int days)
        {
            date.SetDay(date.Day.Value + days);
        }

        public static int DiffInDays(Date date1, Date date2)
        {
            var yearDiff = date1.Year.Value - date2.Year.Value;
            var dayDiff = date1.Day.Value - date2.Day.Value;
            return yearDiff * Date.LastDayOfYear + dayDiff;
        }

        public static void IncrementDay(this Date date)
        {
            date.SetDay(date.Day.Value + 1);
        }

        public static int AsDays(this Date date)
        {
            return date.Day.Value + Date.LastDayOfYear * (date.Year.Value - 1);
        }
    }
}