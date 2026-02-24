# Plan: Remove PluralWorld and fix localization anti-pattern

## Problem

`GoodResourceData.PluralWorld` stores "much" or "many" per good, used in `TradeValidator` to compose sentences like:

- `$"{townName} does not own that {muchOrMany} {goodName}."`
- `$"You do not own that {muchOrMany} {goodName}."`

This is not localization-friendly:

- **English**: much (uncountable) vs many (countable) — works, but fragile.
- **German**: "viel/viele" depends on grammatical gender and case, not just countability. A simple flag can't cover it.
- **French**: "autant de" works universally, but sentence structure differs entirely.
- **Spanish**: "tanto/tanta/tantos/tantas" varies by gender *and* number.

The root issue is **string concatenation with grammatical fragments** — a classic localization anti-pattern. It assumes English word order and grammar.

## Solution

Replace both interpolated messages in `TradeValidator` with complete localized sentences using `{townName}` and `{goodName}` as smart string variables. Reword to use "enough" which works for both countable and uncountable nouns in English:

| Key | EN | DE | FR | ES |
|-----|----|----|----|----|
| `trade.error.town-not-enough` | {townName} does not have enough {goodName}. | {townName} hat nicht genug {goodName}. | {townName} n'a pas assez de {goodName}. | {townName} no tiene suficiente {goodName}. |
| `trade.error.player-not-enough` | You do not have enough {goodName}. | Du hast nicht genug {goodName}. | Vous n'avez pas assez de {goodName}. | No tienes suficiente {goodName}. |

## Steps

1. Add the two localized string entries to the appropriate string table.
2. In `TradeValidator`, replace the `muchOrMany` interpolation with localized string lookups using `{townName}` and `{goodName}` as variables.
3. Remove the `PluralWorld` property from `GoodResourceData.cs`.
4. Remove `<PluralWorld>k__BackingField` from all 42 `.asset` files.

## Files affected

- `Assets/Features/Goods/Config/GoodResourceData.cs` — remove `PluralWorld` property
- `Assets/Features/Trade/Logic/TradeValidator.cs` — replace string interpolation with localized strings
- `Assets/Features/Goods/Config/Goods/*.asset` (42 files) — remove `PluralWorld` serialized field
- String table CSV / shared data — add two new entries
