namespace Data
{
    public static class DateExtensions
    {
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