namespace Common.UI
{
    public static class TMP
    {
        public static string ColorGood(string content)
        {
            return $"<style=\"Color_Good\">{content}</style>";
        }

        public static string ColorBad(string content)
        {
            return $"<style=\"Color_Bad\">{content}</style>";
        }
    }
}