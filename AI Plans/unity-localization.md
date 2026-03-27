# Unity Localization Package (1.5.9) - Reference

## Core Concepts
- **StringTableCollection**: Groups SharedTableData + per-locale StringTable assets
- **SharedTableData**: Stores key names and IDs (shared across locales)
- **StringTable**: One per locale, maps key IDs to translated strings
- **LocalizedString**: Serializable reference to a table entry (works in MonoBehaviours and ScriptableObjects)

## CSV Format
```
Key,Id,English(en),German(de),French(fr)
GREETING,1,Hello,Hallo,Bonjour
```
- Leave Id blank/0 for new entries (auto-assigned)
- Import modes: CSV (Replace) or CSV (Merge)
- Export via String Table window or programmatically via `UnityEditor.Localization.Plugins.CSV.Csv`

## Using in C# Code
```csharp
// Field (serializable, shows dropdown in Inspector)
[SerializeField] LocalizedString myString;

// Subscribe to changes
myString.StringChanged += (s) => label.text = s;

// Synchronous
string text = myString.GetLocalizedString();

// With arguments
string text = myString.GetLocalizedString(arg1, arg2);

// Construct in code
var ls = new LocalizedString("TableName", "KEY_NAME");
```

## Using in ScriptableObjects
`LocalizedString` is fully serializable - use as field, Inspector shows table/entry picker.
```csharp
[SerializeField] LocalizedString questTitle;
public string GetTitle() => questTitle.GetLocalizedString();
```

## Using in UI (Prefabs)
- Add `LocalizeStringEvent` component alongside TextMeshPro
- Set String Reference to table + entry
- Wire UpdateString event to TMP.text
- Shortcut: Right-click TMP component > Localize

## Smart Strings (dynamic values)
- Enable per-entry with Smart checkbox
- Syntax: `{0}` indexed, `{variable-name}` named
- Pluralization: `{count:plural:an apple|{} apples}`
- Choose: `{0:choose(0|1|2):none|one|two}`
- Variables: `StringVariable`, `IntVariable`, `FloatVariable`, etc.
- Global variables via `PersistentVariablesSource`

## Project Setup Steps
1. Create LocalizationSettings (Edit > Project Settings > Localization)
2. Create Locales (Locale Generator)
3. Create String Table Collections (Window > Asset Management > Localization Tables)
4. Populate entries
5. Build requires Addressables build step

## Key Namespaces
- Runtime: `UnityEngine.Localization`, `UnityEngine.Localization.Tables`, `UnityEngine.Localization.Settings`
- Editor: `UnityEditor.Localization`, `UnityEditor.Localization.Plugins.CSV`
- Smart: `UnityEngine.Localization.SmartFormat.PersistentVariables`
