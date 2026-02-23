using Microsoft.CodeAnalysis;

namespace Unity.Localization.Roslyn;

internal sealed record TableInfo(AdditionalText OriginalText, string Name);