using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MedievalMerchant.Generators
{
    /// <summary>
    /// Parses a Unity Localization "*_en.asset" YAML file to extract
    /// the localized English text for each entry ID.
    /// </summary>
    public static class LocaleParser
    {
        private static readonly Regex ArgPattern = new Regex(@"\{(\d+)(?::[^}]*)?\}", RegexOptions.Compiled);

        /// <summary>
        /// Parses the English locale file and returns a dictionary mapping entry ID to localized text.
        /// </summary>
        public static Dictionary<long, string> Parse(string content)
        {
            var result = new Dictionary<long, string>();
            var lines = content.Split('\n');

            long currentId = 0;
            bool inTableData = false;

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].TrimEnd('\r');
                var trimmed = line.TrimStart();

                if (trimmed.StartsWith("m_TableData:"))
                {
                    inTableData = true;
                    continue;
                }

                if (!inTableData)
                    continue;

                // Detect end of table data
                if (!line.StartsWith("  ") && !line.StartsWith("\t") && trimmed.Length > 0 && !trimmed.StartsWith("-"))
                {
                    if (trimmed.StartsWith("references:") || trimmed.StartsWith("m_"))
                        break;
                }

                // Strip YAML list item marker "- " prefix
                var field = trimmed;
                if (field.StartsWith("- "))
                    field = field.Substring(2);

                if (field.StartsWith("m_Id:"))
                {
                    var idStr = ExtractValue(field);
                    if (long.TryParse(idStr, out var id))
                        currentId = id;
                    continue;
                }

                if (field.StartsWith("m_Localized:"))
                {
                    var text = ExtractLocalizedValue(field);

                    // Handle multi-line YAML strings (continuation lines indented further)
                    while (i + 1 < lines.Length)
                    {
                        var nextLine = lines[i + 1].TrimEnd('\r');
                        var nextTrimmed = nextLine.TrimStart();
                        // Continuation lines are indented and not a new field
                        if (nextLine.Length > 0 && (nextLine.StartsWith("      ") || nextLine.StartsWith("\t\t\t"))
                            && !nextTrimmed.StartsWith("m_") && !nextTrimmed.StartsWith("-"))
                        {
                            text += " " + nextTrimmed;
                            i++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    result[currentId] = text;
                    continue;
                }
            }

            return result;
        }

        /// <summary>
        /// Counts the number of Smart String arguments in the text.
        /// Returns the highest argument index + 1 (e.g., "{0} {2}" returns 3).
        /// </summary>
        public static int CountArguments(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            var matches = ArgPattern.Matches(text);
            if (matches.Count == 0)
                return 0;

            int maxIndex = -1;
            foreach (Match match in matches)
            {
                if (int.TryParse(match.Groups[1].Value, out var idx) && idx > maxIndex)
                    maxIndex = idx;
            }

            return maxIndex + 1;
        }

        private static string ExtractValue(string line)
        {
            var colonIdx = line.IndexOf(':');
            if (colonIdx < 0 || colonIdx + 1 >= line.Length)
                return "";
            return line.Substring(colonIdx + 1).Trim();
        }

        private static string ExtractLocalizedValue(string line)
        {
            var colonIdx = line.IndexOf(':');
            if (colonIdx < 0 || colonIdx + 1 >= line.Length)
                return "";

            var value = line.Substring(colonIdx + 1).Trim();

            // Remove YAML string quoting
            if (value.Length >= 2)
            {
                if ((value[0] == '\'' && value[value.Length - 1] == '\'') ||
                    (value[0] == '"' && value[value.Length - 1] == '"'))
                {
                    value = value.Substring(1, value.Length - 2);
                }
            }

            // Unescape YAML single-quote escaping ('' → ')
            value = value.Replace("''", "'");

            return value;
        }
    }
}
