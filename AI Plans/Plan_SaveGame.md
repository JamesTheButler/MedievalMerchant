# Save Game Plan

## Core Principle: DTO Snapshot Pattern

Don't serialize live model objects directly. `Observable<T>`, `ModifiableVariable`, `Dictionary<Vector2Int, Town>` etc. are not serialization-friendly. Instead, define plain DTO records that mirror **mutable runtime state only**, and write explicit snapshot/restore logic.

`OngoingLevelSaveData` (currently an empty record in `Features/Levels/Serialization/`) is the existing hook. It gets filled out as the top-level `GameplaySaveData`.

---

## What Needs Saving

### PlayerSaveData
- Location (grid position, in-transit progress)
- Funds + inventory contents (goods with quantities)
- Cart levels (array of `int`, one per cart index)
- Companion levels (`CompanionType → int`)

### TownSaveData (per town, keyed by `Vector2Int`)
- Inventory (funds + goods with quantities)
- Reputation level/value
- Development tier + progress toward next tier
- Unlocked milestones
- Active missions (type, deadline, progress)
- Active producers (good + level)

### LevelStateSaveData
- Current date (`DateModel`)
- Condition progress (`LevelConditions`)
- Active events (`EventModel`)
- Stats (`StatsModel`)

---

## Structure

```
GameplaySaveData               // top-level, replaces empty OngoingLevelSaveData
  PlayerSaveData Player
  Dictionary<Vector2Int, TownSaveData> Towns
  LevelStateSaveData LevelState
```

All in `Features/Levels/Serialization/`.

A `GameplaySnapshotService` handles `GameplayModel → GameplaySaveData` and back.
Lives in `Common/Infrastructure/Gameplay/` or `Features/Levels/Serialization/`.

---

## Load Flow

1. `LevelBootstrapper` constructs the level fresh from config (normal init)
2. Checks if `OngoingLevelSaveData` has data for this level
3. If yes: `GameplaySnapshotService.Restore(saveData, GameplayModel)` overrides dynamic state

**Construction vs. restoration**: `Town` and `PlayerModel` have complex constructors that set up static config. Save data only overrides dynamic state — reconstruct from config, then overlay save data on top.

---

## Save Triggers
- Leaving a town
- Day tick
- `GameplayContext.OnDestroy` (app close / scene unload)
- Write path: `ProgressModel.UpdateOngoingLevel(levelId, saveData)`

---

## What NOT to Save
- `TileFlagMap`, `ProductionZones`, `GoodPool` — static map/config data
- `GameSpeed`, `MapModeModel` — transient UI/session state
- `ModifiableVariable` modifier stacks — these are derived; re-apply during init from restored base state (companion levels, milestones, etc.)
