using UnityEngine;

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

        public static string WithStyle(this string content, string style)
        {
            return $"<style=\"{style}\">{content}</style>";
        }

        public static string WithColor(this string content, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{content}</color>";
        }
    }
}