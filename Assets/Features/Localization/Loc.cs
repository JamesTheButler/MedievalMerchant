using UnityEngine.Localization.Settings;

namespace Features.Localization
{
    public static partial class Loc
    {
        private static string Get(string table, long id)
            => LocalizationSettings.StringDatabase.GetLocalizedString(table, id);

        private static string Get(string table, long id, params object[] args)
            => LocalizationSettings.StringDatabase.GetLocalizedString(table, id, args);
    }
}
