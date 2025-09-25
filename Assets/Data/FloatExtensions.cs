namespace Data
{
    public static class FloatExtensions
    {
        public static string Sign(this float value)
        {
            return value switch
            {
                > 0.0001f => "+",
                < -0.0001f => "",
                _ => "+/-",
            };
        }
    }
}