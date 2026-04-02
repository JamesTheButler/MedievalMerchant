# Companion Upgrade Missions — Implementation Plan

## 1. Config / Data

- Create a `CompanionUpgradeConfig` ScriptableObject with 3 tiers, each specifying:
  - T1 good count
  - T2 good count
  - T3 good count
  - Baseline gold cost
- Per-companion ScriptableObject gets an optional gold cost override per tier
- Gold substitute cost per good slot = `GoodConfig.BasePriceData * 3`

## 2. Mission Generation

- Triggered when the player initiates an upgrade at the camp
- Sample required goods from `GoodPool` by tier, filtered to goods available on the current map
- No duplicate goods within a single mission (e.g. 2x T1 = two *different* T1 goods)
- Store the generated mission as a stable instance for the duration of the level

### Mission Instance Structure

Each required good is represented as a slot containing:
- `GoodConfig` reference
- Fulfilled flag
- Paid-with-gold flag
- Computed gold substitute cost (`BasePriceData * 3`)

## 3. Fulfillment Logic

- Two fulfillment paths per slot:
  - **Deliver the good** from the player's inventory
  - **Pay the gold substitute** (`BasePriceData * 3`)
- Upgrade unlocks when all slots are fulfilled by either method
- Total gold cost = base companion gold cost + sum of any gold substitutes chosen

## 4. Camp UI

- Display each required good slot with:
  - Good name and quantity
  - Gold substitute cost
  - Current fulfillment state (unfulfilled / delivered / paid)
- Player can choose delivery method per slot before confirming
- All costs are visible upfront before the player commits

## 5. Upgrade Feedback

- **Immediate moment**: animated level-up on companion portrait — scale punch, flash, particle burst
- **Reveal moment**: new bonuses appear with a short stagger, not all at once; highlight changed stats
- **Persistent**: companion portrait frame or badge changes visually per tier (plain → decorated → gilded)
- **Audio**: distinct ascending SFX sting on upgrade confirm
