using System.Collections.Generic;
using Common.Utility;
using UnityEngine;

namespace Common.UI.Utility
{
    public static class StyleExtensions
    {
        private static readonly Dictionary<Style, string> StyleStrings = new()
        {
            { Style.Default, "Title" },
            { Style.Subtitle, "Subtitle" },
            { Style.Link, "Link" },
            { Style.Good, "Color_Good" },
            { Style.Bad, "Color_Bad" },
            { Style.TutorialHighlight, "TutorialHighlight" },
        };

        public static string WithStyle(this string content, Style style)
        {
            return $"<style=\"{StyleStrings[style]}\">{content}</style>";
        }

        public static string WithColor(this string content, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{content}</color>";
        }

        public static Style GetNumberStyle(this float value, bool isPositiveGood = true)
        {
            if (value.IsApproximately(0))
                return Style.Default;

            return value > 0 == isPositiveGood ? Style.Good : Style.Bad;
        }
    }
}