namespace Data
{
    public static class DateExtensions
    {
        public static void IncrementDay(this Date date)
        {
            date.SetDay(date.Day.Value + 1);
        }
    }
}