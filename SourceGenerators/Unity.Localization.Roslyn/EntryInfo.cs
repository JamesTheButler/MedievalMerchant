namespace Unity.Localization.Roslyn
{
    internal sealed record EntryInfo(
        long Id,
        string Key,
        string EnglishText,
        EntryArg[] Args);
}