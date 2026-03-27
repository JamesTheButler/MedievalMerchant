# Localization Plan Summary

## Already Localized
- `LevelInfo.LevelName`, `LevelInfo.Description` (LocalizedString)
- `LocalizationResources.Difficulties` (SerializedDictionary<Difficulty, LocalizedString>)
- Helper exists: `LocalizationExtensions.cs` (SetText extension, Update extension)

## 7 Proposed String Tables

1. **UI** (~15 keys) — Shared labels: Funds, Reputation, Cost, /day, Sells for, Buys for, etc.
2. **GameOver** (~20 keys) — Win/loss screens, condition descriptions (bankruptcy, timeout, funds, reputation, town tier)
3. **Trade** (~15 keys) — Trade UI, price modifier descriptions, validation messages, haggling
4. **Towns** (~25 keys) — Missions (Trade/Upgrade Mission, reward/penalty), milestones, development, producer names
5. **Notifications** (~10 keys) — Mission started/failed, event started/expired, loss imminent
6. **Player** (~20 keys) — Companion names/descriptions/level descriptions, caravan UI (Waggon, Cost)
7. **Goods** (~30+ keys) — Good names, plural forms, availability, selectors, tier display, region names

Optional 8th: **Tutorial** — TutorialTopicData.Title, TutorialChapterData.Title/Description

## ScriptableObject Fields to Convert (string → LocalizedString)
- `GoodResourceData`: GoodName, PluralWorld
- `AvailabilityResourceData`: DisplayString, Description
- `RegionResourceData`: Name
- `CompanionConfigData`: Name, Description
- `GameModifierData`: Title, Description
- `TutorialTopicData`: Title
- `TutorialChapterData`: Title, Description

## Key Source Files with Hardcoded Strings
- `Assets/Common/UI/GameOverUI.cs` — win/loss messages, statistics
- `Assets/Features/Trade/UI/TradeUI.cs` — funds, profit/loss, reputation text
- `Assets/Features/Trade/Logic/Price/*.cs` — all price modifier descriptions
- `Assets/Features/Towns/Missions/UI/TownUIMissionSectionItem.cs` — mission UI
- `Assets/Features/Towns/Missions/UI/MissionTooltip.cs` — mission reward/penalty
- `Assets/Features/Towns/Missions/Mission*Notification.cs` — notification strings
- `Assets/Features/Towns/UI/TownUIHeaderSection.cs` — town header labels
- `Assets/Features/Towns/UI/TownUIDevelopmentSection.cs` — development text
- `Assets/Features/Towns/Development/Config/Milestones/*.cs` — milestone descriptions
- `Assets/Features/Levels/Conditions/Model/*.cs` — condition messages
- `Assets/Features/Levels/Conditions/Data/*.cs` — condition descriptions
- `Assets/Features/Levels/GameModifiers/Effects/Data/*.cs` — effect descriptions
- `Assets/Features/Levels/GameModifiers/Events/*Notification.cs` — event notifications
- `Assets/Features/Player/Retinue/Config/LevelDatas/*.cs` — companion descriptions
- `Assets/Features/Player/Caravan/UI/*.cs` — cart/waggon UI
- `Assets/Features/Goods/UI/GoodTooltip.cs` — good tooltip labels
- `Assets/Features/Goods/Selector/*.cs` — display strings
- `Assets/Common/Types/TierExtensions.cs` — "Tier I/II/III"
