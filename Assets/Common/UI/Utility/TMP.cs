using UnityEngine;

namespace Common.UI.Utility
{
    public static class TMP
    {
        public static string WithDefaultStyle(this string content)
        {
            return $"<style=\"Title\">{content}</style>";
        }

        public static string WithGoodStyle(this string content)
        {
            return $"<style=\"Color_Good\">{content}</style>";
        }

        public static string WithBadStyle(this string content)
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