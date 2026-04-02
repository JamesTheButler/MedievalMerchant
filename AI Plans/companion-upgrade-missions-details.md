# Companion Upgrade Missions — Backend Implementation Plan

## Context

Replace hardcoded goods-per-companion-per-level with tier-based random sampling from the map's `GoodPool`. Each good slot can be substituted with gold. The current system uses `CompanionMissionConfigData` with a fixed `Cost` + `List<CompanionMissionItemConfigData>` (specific Good + Amount per level). The new system defines tier requirements (e.g. "2 Tier1 goods, 1 Tier2 good") and samples randomly at runtime from the map's available goods.

**Scope**: Backend logic only. UI will be adapted later in a separate pass.

---

## Current State of All Files

All paths relative to `Assets/Features/Player/Retinue/`.

### DONE — Already implemented and working

#### 1. `Logic/CompanionMissionItem.cs` — made abstract
Was `sealed`, now `abstract`. Shared base for coin and good items.
```csharp
public abstract class CompanionMissionItem
{
    public int TargetAmount { get; }
    public IReadOnlyObservable<int> RemainingAmount => _remainingAmount;
    public Observable<bool> IsCompleted { get; } = new();
    private readonly Observable<int> _remainingAmount;

    public CompanionMissionItem(int targetAmount) { ... }
    public void Deliver(int amount) { ... } // decrements remaining, sets IsCompleted when 0
}
```

#### 2. `Logic/CompanionMissionCoinItem.cs` — NEW
```csharp
public sealed class CompanionMissionCoinItem : CompanionMissionItem
{
    public CompanionMissionCoinItem(int targetAmount) : base(targetAmount) { }
}
```

#### 3. `Logic/CompanionMissionGoodItem.cs` — NEW
Computes `SubstituteCostSingle` from `GoodConfig.BasePriceData[tier] * CompanionConfig.GoodMissionSubstituteFactor`.
```csharp
public sealed class CompanionMissionGoodItem : CompanionMissionItem
{
    public Good Good { get; }
    public float SubstituteCostSingle { get; }

    public CompanionMissionGoodItem(Good good, int targetAmount) : base(targetAmount)
    {
        Good = good;
        var tier = ResourceManager.Instance.GoodResources.ResourceData[good].Tier;
        SubstituteCostSingle = ConfigurationManager.Configurations.GoodConfig.BasePriceData[tier]
            * ConfigurationManager.Configurations.CompanionConfig.GoodMissionSubstituteFactor;
    }
}
```

#### 4. `Config/CompanionConfig.cs` — added substitute factor
Added field (line 32):
```csharp
[field: SerializeField]
public float GoodMissionSubstituteFactor { get; private set; } = 3f;
```

#### 5. `Logic/CompanionMission.cs` — uses new item types
`CoinCost` now creates `CompanionMissionCoinItem`. `MissionItems` values are now `CompanionMissionGoodItem` (created in constructor). Public API unchanged: `Dictionary<Good, CompanionMissionItem> MissionItems`, `Deliver(Good, int)`, `DeliverCoin(int)`, `Completed` event via `_incompleteItemCount` tracking.

#### 6. `Logic/CompanionDeliveryService.cs` — rewritten
Now takes `CompanionMissionItem` directly (no more companion type + good lookup). Two methods:
- `Deliver(CompanionMissionItem missionItem, int amount)` — switches on type to remove good/coin from inventory, then calls `missionItem.Deliver(amount)`
- `Substitute(CompanionMissionGoodItem goodMissionItem, int goodAmount)` — computes coin cost from `goodAmount * SubstituteCostSingle`, removes funds, calls `Deliver(goodAmount)`

Removed `_retinueModel` field — no longer looks up missions internally.

#### 7. UI files — adapted to new API (working, but will be redone later)
- `CompanionCampPanelUiItem.cs`: passes `item` directly to `deliveryPanel.SetUp(companionType, item)` instead of `good`
- `CompanionDeliveryPanel.cs`: `SetUp(CompanionType, CompanionMissionItem)` stores the item. Uses `is CompanionMissionGoodItem` checks for coin vs good display. Has substitute button logic.

---

### TODO — Still needs implementation

#### 8. `Config/CompanionMissionConfigData.cs` — replace fields

**Current** (still old):
```csharp
[Serializable]
public sealed class CompanionMissionConfigData
{
    [field: SerializeField] public int Cost { get; private set; }
    [field: SerializeField] public List<CompanionMissionItemConfigData> Items { get; private set; }
}
```

**Target**:
```csharp
[Serializable]
public sealed class CompanionMissionConfigData
{
    [field: SerializeField] public int BaseGoldCost { get; private set; }
    [field: SerializeField] public List<CompanionMissionTierRequirement> TierRequirements { get; private set; }
}
```

#### 9. `Config/CompanionMissionTierRequirement.cs` — NEW

```csharp
using System;
using Common.Types;
using UnityEngine;

namespace Features.Player.Retinue.Config
{
    [Serializable]
    public sealed class CompanionMissionTierRequirement
    {
        [field: SerializeField] public Tier Tier { get; private set; }
        [field: SerializeField] public int GoodCount { get; private set; }
        [field: SerializeField] public int AmountPerGood { get; private set; }
    }
}
```

#### 10. `Config/CompanionMissionItemConfigData.cs` — DELETE
No longer needed. Goods are sampled at runtime, not configured per-item.

#### 11. `Logic/CompanionMissionSystem.cs` — rewrite `OnLevelChanged`

**Current**: Reads `nextMissionConfig.Cost` + iterates `nextMissionConfig.Items` to build `Dictionary<Good, int>`.

**Target**: Read `BaseGoldCost` + `TierRequirements`. For each requirement, sample `GoodCount` unique goods from `GoodPool` by tier. Pass sampled goods + amounts to `StartMission`.

New fields needed at `Initialize()`:
```csharp
_goodPool = GameplayContext.Instance.Model.GoodPool;  // via GameplayModel.GoodPool
```

New `OnLevelChanged(int level)`:
```csharp
private void OnLevelChanged(int level)
{
    _companionModel.ActiveMission.Value = null;

    var missionConfig = _companionConfig.Get(_companionModel.CompanionType).MissionConfig;
    var nextMissionConfig = missionConfig.ConfigsPerLevel.ElementAtOrDefault(level);
    if (nextMissionConfig == null) return;

    var baseGoldCost = ApplyNegotiatorDiscount(nextMissionConfig.BaseGoldCost);
    var missionTargets = SampleGoods(nextMissionConfig.TierRequirements);

    _companionModel.StartMission(baseGoldCost, missionTargets);
    _companionModel.ActiveMission.Value!.Completed.Observe(OnMissionCompleted);
}

private Dictionary<Good, int> SampleGoods(List<CompanionMissionTierRequirement> requirements)
{
    var result = new Dictionary<Good, int>();
    var usedGoods = new HashSet<Good>();

    foreach (var req in requirements)
    {
        var pool = GetGoodsForTier(req.Tier);
        var available = new List<Good>(pool.Where(g => !usedGoods.Contains(g)));
        var count = Mathf.Min(req.GoodCount, available.Count);

        for (var i = 0; i < count; i++)
        {
            var good = available.GetRandom();  // from CollectionExtensions
            available.Remove(good);
            usedGoods.Add(good);
            result.Add(good, req.AmountPerGood);
        }
    }
    return result;
}

private IReadOnlyCollection<Good> GetGoodsForTier(Tier tier) => tier switch
{
    Tier.Tier1 => _goodPool.Tier1Goods,
    Tier.Tier2 => _goodPool.Tier2Goods,
    Tier.Tier3 => _goodPool.Tier3Goods,
    _ => _goodPool.Tier1Goods
};
```

**Note**: `CompanionModel.StartMission(int, IReadOnlyDictionary<Good, int>)` signature stays the same — no change needed there. The `CompanionMission` constructor already creates `CompanionMissionGoodItem` instances from the dictionary.

#### 12. `Logic/CompanionUpgradeService.cs` — simplify

**Current**: Builds a `ModifiableVariable` with base cost from remaining `CoinCost`, applies Negotiator discount, checks funds, deducts funds, levels up.

**Target**: All costs are now delivered progressively (coin via `CoinCost`, goods via `MissionItems`). Negotiator discount is applied at mission creation in `CompanionMissionSystem.ApplyNegotiatorDiscount`. The upgrade service just checks that the mission is complete and levels up:

```csharp
public void LevelUpgradeRequested(CompanionType companionType, int newLevel)
{
    var companionModel = _player.RetinueModel.Companions[companionType];
    var companionConfigData = _companionConfig.Get(companionType);

    if (newLevel > companionConfigData.Levels.Count)
    {
        Debug.LogError($"Upgrade of {companionType} failed. Level {newLevel} > max {companionConfigData.Levels.Count}");
        return;
    }

    var mission = companionModel.ActiveMission.Value;
    if (mission == null)
    {
        Debug.LogWarning($"Cannot upgrade {companionType} — no active mission.");
        return;
    }

    // Mission auto-completes and auto-levels-up via CompanionMissionSystem.OnMissionCompleted.
    // This method exists for manual/UI-driven upgrade requests.
    // Check: all items fulfilled?
    var allComplete = mission.CoinCost.IsCompleted.Value;
    foreach (var (_, item) in mission.MissionItems)
    {
        if (!item.IsCompleted.Value) { allComplete = false; break; }
    }

    if (!allComplete)
    {
        Debug.LogWarning($"Cannot upgrade {companionType} — mission not yet complete.");
        return;
    }

    Debug.Log($"Upgrading {companionType} to {newLevel}.");
    _player.RetinueModel.SetLevel(companionType, newLevel);
}
```

Remove: `_loc` field, `ModifiableVariable` cost calculation, `CompanionUpgradeBaseCostModifier`, `GenericBasePercentageModifier`, fund deduction. Can also remove unused `using` statements for `Modifiable`, `Localization`, `Modifiers`.

**Note**: The `Completed` event on `CompanionMission` already fires and `CompanionMissionSystem.OnMissionCompleted` already auto-levels-up. So `LevelUpgradeRequested` may become a no-op guard in practice. Keep it for UI-driven explicit upgrade confirmation if needed.

#### 13. `CompanionDeliveryPanel.cs` — minor refactor (optional)

Cache the type check result in `SetUp` to reduce 4x repeated `is CompanionMissionGoodItem` casts:
```csharp
private CompanionMissionGoodItem _goodItem;
private bool IsGoodDelivery => _goodItem != null;

public void SetUp(CompanionType companionType, CompanionMissionItem missionItem)
{
    _companionType = companionType;
    _missionItem = missionItem;
    _goodItem = missionItem as CompanionMissionGoodItem;
}
```
Then replace all `_missionItem is CompanionMissionGoodItem goodMissionItem` with `IsGoodDelivery` / `_goodItem`.

---

## Key Utility References

| What | Where | Usage |
|------|-------|-------|
| `GoodPool` | `Features.Goods.GoodPool` | `.Tier1Goods`, `.Tier2Goods`, `.Tier3Goods` — `IReadOnlyCollection<Good>` |
| Access GoodPool | `GameplayContext.Instance.Model.GoodPool` | Set in `GameplayModel.Initialize()` |
| Good's tier | `ResourceManager.Instance.GoodResources.ResourceData[good].Tier` | Returns `Common.Types.Tier` enum |
| Base price by tier | `ConfigurationManager.Configurations.GoodConfig.BasePriceData[tier]` | `SerializedDictionary<Tier, float>` |
| Substitute factor | `ConfigurationManager.Configurations.CompanionConfig.GoodMissionSubstituteFactor` | Default 3f |
| Random from collection | `available.GetRandom()` | `Common.Utility.CollectionExtensions.GetRandom<T>(IList<T>)` |
| `Tier` enum | `Common.Types.Tier` | `Tier1 = 1, Tier2 = 2, Tier3 = 3` |

---

## Execution Order

0. **Copy this plan** to `AI Plans/Plan_CompanionMissions.md` (so it travels with git)
1. Create `Config/CompanionMissionTierRequirement.cs` (new file)
2. Rewrite `Config/CompanionMissionConfigData.cs` (BaseGoldCost + TierRequirements)
3. Delete `Config/CompanionMissionItemConfigData.cs`
4. Rewrite `Logic/CompanionMissionSystem.OnLevelChanged` to sample from GoodPool
5. Simplify `Logic/CompanionUpgradeService.LevelUpgradeRequested`
6. (Optional) Refactor `CompanionDeliveryPanel.cs` — cache `_goodItem`

**No changes needed** for `CompanionModel.StartMission` — its existing `(int, IReadOnlyDictionary<Good, int>)` signature works with the new sampling approach.

---

## Config Asset Impact

After code changes, the `CompanionMissionConfig` ScriptableObject instances in Unity will lose their serialized data (old `Cost` + `Items` fields removed). They need to be **reconfigured in the Unity Editor** with:
- `BaseGoldCost` per level
- `TierRequirements` list per level (e.g. level 0→1: [{Tier1, 2, 1}], level 1→2: [{Tier1, 1, 2}, {Tier2, 1, 1}])

---

## Verification

1. **Compile** — open in Unity, no errors
2. **Inspector** — check companion config SO → MissionConfig shows `BaseGoldCost` + `TierRequirements` per level
3. **Runtime** — start a level → Debug.Log shows mission generated with random goods from map pool
4. **Progressive delivery** — deliver goods/coin incrementally → remaining amounts track correctly
5. **Gold substitution** — substitute a good with gold → item marked complete, funds deducted
6. **Upgrade** — complete all requirements → auto-level-up fires via `Completed` event
7. **Edge case** — map with few T3 goods → gracefully samples as many as available
