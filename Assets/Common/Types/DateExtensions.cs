namespace Common.Types
{
    public static class DateExtensions
    {
        public static void AddDays(this Date date, int days)
        {
            date.SetDay(date.Day.Value + days);
        }
    }
}