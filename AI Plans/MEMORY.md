# Memory Index

## Project: Medieval Merchant
- Unity 6 (6000.2.15f1), C#, Steam Early Access
- Owner: Tom Ille
- Solution: `MedievalMerchant.sln`

## Key Files
- [unity-localization.md](unity-localization.md) - How Unity Localization package works (String Tables, CSV, Smart Strings, LocalizedString API)
- [architecture.md](architecture.md) - Project architecture patterns and conventions
- [localization-plan.md](localization-plan.md) - Plan for extracting strings (to be created)
- [Plan_SaveGame.md](Plan_SaveGame.md) - Plan for save game serialization (DTO snapshot pattern)
- [FeedbackReport_20260327.md](FeedbackReport_20260327.md) - Compiled player feedback report (critical bugs, priorities)

## Important Conventions
- Feature folders: `Assets/Features/{Name}/{Config,Logic,UI,Data}/`
- Namespaces mirror folders: `Common.*`, `Features.{Name}.*`
- Classes are typically `sealed`; uses modern C# (records, init-only, nullable refs)
- Two context singletons: `GlobalContext` (persists) and `GameplayContext` (per-level)
- Observable/reactive pattern: `Observable<T>`, `IBinding`, `ObservableEvent`
- Data-driven: `ConfigurationManager` (tweakable) vs `ResourceManager` (static data)
- `IInitializable` interface with `Initialize()` / `CleanUp()` on all major systems

## Localization Status
- Package installed: Unity Localization 1.5.9
- Current state: Strings are hardcoded in .cs files, ScriptableObjects, prefabs, scenes
- Goal: Extract to CSV-based string tables, LTR languages only
- Some `LocalizedString` usage already exists (LevelInfo, difficulty names)
