# Combat System Design

## Overview

Bandits are introduced in levels 4 and 5. Combat is a meaningful obstacle that rewards preparation — not a game-ender, but a real cost if you're underprepared. The core loop stays trading; combat is a pressure system layered on top.

---

## Bandits on the Map

- Bandits appear in two forms: **roaming** (move between towns on a tick schedule) and **camped** (sit on a tile as a route blocker)
- Each bandit group has a visible **threat rating** surfaced via tooltip
- Tooltip text is flavour-based, not mechanical. Examples:
  - *"A weak band of bandits. Not much of a threat and probably not very rich either."*
  - *"A scary looking gang that seem to have recently gotten the better of one of your competitors."*
- Bandit groups have a **pre-generated tier** (Weak / Mid / Strong) determined at spawn via weighted probability

### Bandit Strength Values

| Tier | Strength |
|---|---|
| Weak | 3–5 |
| Mid | 8–14 |
| Strong | 18–25 |

---

## Bandit Spawn Weights (Maturity System)

Bandit tier is determined by a single **maturity score** (0–1) blended from development progress and game time.

### Formula

```
devCeiling = townCount * 300 * 0.8
devMaturity = clamp(globalDevScore / devCeiling, 0, 1)

timeCeiling = level deadline in days
timeMaturity = clamp(currentDay / timeCeiling, 0, 1)

maturity = devMaturity * 0.8 + timeMaturity * 0.2
```

### Global Development Score

Each town contributes: `(tier - 1) * 100 + currentDev`

- Tier 1 town at 50 dev = 50
- Tier 2 town at 25 dev = 125
- Tier 3 town at 100 dev = 300 (max per town)

### Spawn Weight Table

| Maturity | Weak | Mid | Strong |
|---|---|---|---|
| 0.0–0.3 | 70% | 25% | 5% |
| 0.3–0.5 | 40% | 45% | 15% |
| 0.5–0.7 | 20% | 45% | 35% |
| 0.7–1.0 | 5% | 25% | 70% |

> All values configurable from a ScriptableObject.

---

## Encounter Options

When the player meets a bandit group, three options are presented:

| Option | Requirement | Notes |
|---|---|---|
| **Fight** | None | Full combat resolution |
| **Bribe** | Negotiator companion | Cost scales with bandit strength. Better negotiator tier = cheaper bribe |
| **Flee** | Navigator companion | Caravan move speed affects success chance. Failure may cost cargo |

---

## Companions

### Commander
- Never dies
- Has 3 tiers with increasing combat power (Atk value)
- Determines maximum number of hireable guards:
  - Tier 1 → 2 guard slots
  - Tier 2 → 3 guard slots
  - Tier 3 → 4 guard slots
- Guards receive a slight combat strength bonus based on commander tier

### Guards
- Anonymous — no names, no individual health
- Displayed as a segmented progress bar (max 4 segments)
- All guards share the same tier as the commander upgrade
- Can die in combat; need to be rehired after losses
- Travelling without guards is valid but risky

### Guard Tier Stats

| Tier | Atk |
|---|---|
| 1 | 3 |
| 2 | 6 |
| 3 | 10 |

> Def stat was considered for reducing guard deaths but replaced by the SO-configured death table.

---

## Combat Resolution

### Roll Formula

```
playerRoll = guardCount * tierAtk * Random(0.75, 1.25)
banditRoll = banditStrength * Random(0.75, 1.25)
margin = playerRoll - banditRoll
```

### Outcome Categories

Margin thresholds are expressed as a percentage of `banditStrength` for natural scaling across the full progression curve. Exact thresholds are configurable in a ScriptableObject.

| Outcome | Condition |
|---|---|
| Clean Win | margin > highThreshold |
| Costly Win | margin > lowThreshold |
| Narrow Win | margin > 0 |
| Loss | margin <= 0 |

### Mid-Fight Choices

The player makes one choice per fight:

| Choice | Effect |
|---|---|
| **Press the attack** | Multiply player roll by 1.3. Guard injury worse if this round is lost |
| **Hold the line** | No modifier. Guard injury reduced if this round is lost |

---

## Guard Deaths

Guard deaths are looked up from a ScriptableObject-configured table. No math at runtime.

| Guards | Clean Win | Costly Win | Narrow Win | Loss |
|---|---|---|---|---|
| 1 | 0 | 0 | 1 | 1 |
| 2 | 0 | 1 | 1 | 2 |
| 3 | 0 | 1 | 2 | 3 |
| 4 | 0 | 1 | 2 | 3 |

- Commander tier gates which rows are reachable (Tier 1 → rows 1–2 only, etc.)
- Deaths are random — any guard can die, no priority order

---

## Loot System

### On a Loss — Bandits Loot You

**Gold stolen** — fixed amount per bandit tier with randomness, configurable in SO.

**Goods stolen** — capacity-based algorithm:

```
maxLoot = totalPlayerGoods * 0.8  // cap to avoid total wipeout
remainingCapacity = min(baseLootAmount * Random(0.9, 1.1), maxLoot)

while remainingCapacity > 0:
    pick a random cargo type from player inventory
    take min(randomAmountOfThatGood, remainingCapacity)
    remainingCapacity -= amountTaken
```

- 80% cap is configurable in SO
- Randomness window (±10%) is configurable in SO

### On a Win — You Loot Bandits

**No gold gained.**

**Bandit inventory** is generated at combat resolution from a tier-based table:

| Bandit Tier | Inventory |
|---|---|
| Weak | 3 T1 goods |
| Mid | 4 T1 + 1–2 T2 goods |
| Strong | 2–3 T1 + 1–2 T2 + 1–3 T3 goods |

- Specific goods within each tier slot are randomised
- A **post-combat loot screen** lets the player browse the bandit inventory and choose what to take
- Limited by player carry capacity
- Player can dump existing cargo slots on this screen to make room — no confirmation required, the cost of dumping is inherent

---

## ScriptableObject Configuration Surface

The following values should be externally configurable:

- Guard tier Atk values
- Guard death table (2D: guardCount × outcomeCategory)
- Outcome margin thresholds (as % of banditStrength)
- Bandit strength ranges per tier
- Spawn weight table per maturity band
- Maturity blend weights (dev % vs time %)
- Gold stolen per bandit tier (base + variance)
- Goods loot capacity per bandit tier (base + variance)
- Goods loot randomness window (default ±10%)
- Player goods cap for bandit theft (default 80%)
- Bandit inventory table per tier
- Bribe cost formula inputs
- Flee success chance inputs
