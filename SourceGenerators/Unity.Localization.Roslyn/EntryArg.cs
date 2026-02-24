namespace Unity.Localization.Roslyn;

/// <summary>
/// Represents a single argument in a localization entry.
/// </summary>
/// <param name="Name">Clean parameter name for the generated C# method (e.g., "Argument").</param>
/// <param name="Type">C# type (e.g., "int", "string", "object").</param>
/// <param name="RawPlaceholder">Original placeholder name as it appears in the smart string (e.g., "_int_Argument"). Null for positional args.</param>
/// <param name="Formatter">Optional SmartFormat formatter suffix (e.g., "D2").</param>
internal sealed record EntryArg(string Name, string Type, string? RawPlaceholder = null, string? Formatter = null);