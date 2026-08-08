# Bandits and Guards

## Overview
Bandits roam the world, spawning from bandit camps scattered across the map. Left unchecked, they raid towns - stealing gold and goods, stalling the town's growth, and denting the player's reputation if the neglect drags on. Loot from a successful raid lets a bandit group grow stronger and more aggressive, so waiting too long makes the problem worse.

The player fights back with hired guards, led by a new companion - the captain - who fully reuses the existing companion system (same level cap, same upgrade-mission mechanics) to level up and make the whole squad more effective. Winning disbands the bandit group and nets some spoils; losing costs gold and leaves the player weakened for a while. The goal is for bandits to be a real, active threat the player has to manage, not just background flavor.

## Spawning
Each camp has an independent daily spawn chance. Each day that a bandit group has NOT been spawned the chance accumulates (flat, +5%/day). When a group is spawned, the chance resets to 0. Then there is a cooldown of 5 days in which no spawn attempt is made - chance stays frozen at 0 for the whole cooldown, it doesn't build up in the background.

Max group amount is one shared cap for the whole level, not per camp. While the level is at the cap, spawn chance is frozen the same way as during cooldown. Only a player victory over a group frees up a slot; once freed, any camp can roll again the very next day.

Camps are permanent, indestructible map features - the player can beat the groups a camp spawns, but never the camp itself. The shared cap is the only thing limiting bandit presence.

A newly spawned group starts at Tier I with a randomized 2-4 units and 200 gold.

### Configurable Values - Spawning
- daily spawn chance (5%)
- cooldown after successfull spawn (5 days)
- spawn tile radius (2-3 tiles)
- max group amount (set in level, shared across all camps)
- starting unit count on spawn (2-4, randomized)
- starting gold on spawn (200)

## Combat
Both the player and a group of bandits are represented as combatants. A combatant is represented by their units' base health and combat points. Combatants than can have upgrades, typically in the form of a higher commander level or in the shape of temporary buffs/debuffs. All of a combatants units have the same max health and combat score, although units have their own current health. The captain and the bandit group both have multiple tiers, where each tier increases HP and combat score for the units. Bandits are generally weaker, but there are more in a group whereas guards are generally stronger but the player will not be able to hire as many.

Combat runs automatically, without player input, and is resolved in rounds. Each round is a snapshot: every unit alive at the start of the round picks an opponent's unit at random, and all attacks resolve simultaneously off that snapshot - a unit that dies mid-round still gets its hit in. If a unit runs out of health, it dies. Rounds run until one side has no remaining units. If both sides run out of units in the same round, it's a draw: the bandit group is disbanded with no loot for the player, and the player goes into Recovering, same as a loss.
The formula for a unit's health change per attack is 
hp_new = hp_old - (unit.combat_strength * commander.total_combat_multiplier) * hit_factor
- commander.total_combat_multiplier is fully multiplicative: base combat strength * tier multiplier * any active buff/debuff multiplier.
- hit_factor is randomized per hit. Bandits and guards use separate distributions - bandits swing wider (more extreme high and low rolls), guards are steadier with a slight positive skew.

Combatants get stronger for each level (of the captain, or of the bandit group), but there can also be temporary buffs and debuffs. This allows the player to be more strategic as to when to engage and when to sit back and wait. It will hopefully create more interesting gameplay as the player might have to make a choice: do i engage the bandit now, that they are strong or do I choose to miss an upgrade mission that is soon to run out.

### Combat - Buffs and Debuffs
- The bandits have different combat buffs and debuffs depending on their current state:
  - Resting: minor debuff - 15% weaker, 10% slower (they are licking their wounds and filling their bellies.)
  - Raiding a town: major debuff - 30% weaker, combat only since they're not moving (they're fighting the townsfolk. this is a good moment to intervene)
  - Rushing to a raid: major buff - 30% stronger, 25% faster (they're fired up!) 
  - Looking for Rest: minor buff + minor movement debuff - 15% stronger, 10% slower (tired but victorious) 
- Bandit buffs and debuffs must be clearly visible on the minimap with a precise tooltip. "[Icon] Resting: The bandits are 15% weaker and 10% slower."


### Configurable Values - Combat
- base health per tier/level (Bandit Tier I and Tier IV rebalanced via simulation - see note below; II/III still the original values and NOT yet validated the same way):
              Bandit  Guard
  Level/Tier I    38      40
  Level/Tier II   35      70
  Level/Tier III  55      110
  Tier IV         84      -
- base combat strength per tier/level (Bandit Tier I rebalanced - see note below):
              Bandit  Guard
  Level/Tier I    10      10
  Level/Tier II   9       18
  Level/Tier III  14      28
  Tier IV         20      -
- Rebalance note: a Monte Carlo simulation of the round-resolution rules above showed the original stats (guard = 2x a same-tier bandit in both HP and CS) made guards overwhelmingly dominant - a same-tier, same-count fight (5 Bandit-I vs 5 Guard-1) was a 100%-guard-win, ~0-loss stomp in 3.45 rounds, and even the game's absolute largest possible fight (20 Tier-IV bandits vs 15 Tier-III guards, both at their max caps) was 96%+ guard-favored. Tier I bandit stats were raised (HP 20→38, CS 5→10, now matching Guard-1's CS) to make the 5v5 fight a real contest: 75% guard win, ~5.75 rounds, meaningful average losses on both sides. Tier IV HP was raised slightly (80→84) to bring the max-cap fight to 65% guard win, ~7.6 rounds. Guard stats, and Bandit Tiers II/III, are unchanged from before this pass - II/III still produce 100%-guard-win stomps at equal count and would need the same simulation-driven tuning if that matters, but no target scenario was given for them yet.
- max guard count per captain level (3 levels, matching the reused companion system - NOT the stale 5-level GuardData currently in CompanionConfig_Default.asset, which needs to be overwritten during implementation): Lvl 1: 5, Lvl 2: 10, Lvl 3: 15
- hit_factor distribution - bandits: uniform random, 0.5-1.5 (placeholder pending a curve-based balancing tool - see Not for v1)
- hit_factor distribution - guards: uniform random, 0.85-1.15 (placeholder; the "slight positive skew" isn't expressible with plain uniform - revisit once curve tooling exists)
- guard hire cost per unit, scales with captain level (not flat): Lvl 1: 15 gold, Lvl 2: 25 gold, Lvl 3: 35 gold
- guard daily upkeep: 0.3 gold/day per guard, flat regardless of captain level. At max (15 guards) that's 4.5 gold/day total, in line with the ~5 gold/day a level-3 companion costs.
- bandit debuff while resting (15% weaker, 10% slower)
- bandit debuff while raiding (30% weaker)
- bandit buff while rushing to raid (30% stronger, 25% faster)
- bandit buff/debuff while looking for rest (15% stronger, 10% slower)
- player "Recovering" debuff duration after a loss or draw (5 days) - movement slow + untargetable
- engagement radius (1 tile, global - the same everywhere combat can trigger, regardless of bandit state)


## Bandit Behavior
Bandit behavior is resolved as a state machine. The Rest state is the default.
 - Rest: 
   - Description: Bandits occupy a road tile for 5 days. Each day they are resting, they consume gold and goods depending on their tier and unit count. If they can't afford it, nothing happens - no debt, they just consume nothing that day.
  At the end of a rest cycle, they may start a new rest cycle or go on a raid. If they choose to rest, they can hire units, upgrade their tier (in both cases they stay where they are) or they relocate a couple of tiles away. The decision making works as follows, checked in this order, first match wins:
    - If they upgraded last cycle, they always raid this cycle - upgrading is a commitment, it always leads to a raid attempt the cycle after.
    - They will always go on a raid, if they don't have any resources to hire.
    - They will always upgrade if they are at MAX unit count and have the resources. (sets the "upgraded last cycle" flag)
    - They will always raid if they are at Max unit count and cannot upgrade.
    - They may raid if they are above a certain raid threshold of units. From thereon it scales linearly up to the max unit count which has 100%. This threshold is its own value, separate from the upgrade threshold below.
    - They will try to upgrade IF they have enough resources AND they have enough units (over the upgrade threshold). They will roll the dice. The more units they have, the higher the chance for an upgrade. Success also sets the "upgraded last cycle" flag.
    - If no upgrade happens, they will hire 1-max units, where max is the Min(MaxAffordable, MaxPredefined).
    - Independently of all of the above, they will pick randomly based on a fixed percentage if they move. That percentage stacks for every cycle they haven't moved. They will only move after a certain amount of cycles. If they don't try to move, the percentage doesn't stack. This can happen the same cycle as hiring or upgrading.
     Effects:
     - While resting, bandits are more vulnerable: Debuff to combat ability. No detection radius - they won't chase the player, but combat still triggers if the player closes to the engagement radius (1 tile).
 - Relocating:
   - Description: the bandits pick a new road tile a couple of tiles away and move there. Also the fallback if a raid decision fires but no raidable town exists (see Travelling).
   - Effects: 
     - no debuff, no buff, no detection radius. they're just moving.
 - Travelling: 
   - Description: the bandits pick a raidable town at random and travel there. A town only counts as raidable if it isn't currently Recovering (see Raiding), being raided, or being rushed at by another group. If no town qualifies, they relocate instead. As they travel, they will attack the player if he is near.
   - Effects: Fervor! They are real excited. movementspeed and combat buff, and a detection radius of 2.5 tiles - they'll come after the player. Engagement radius is unchanged (1 tile).
 - Raiding: When they reach a town, they will start a raid. A raid takes a few days. The larger a town, the longer it takes. Loot is only stolen if the raid completes: if the player loses a fight here, the raid just continues where it left off; if the player wins, the group is disbanded, the raid never completes (town keeps everything), and the player can loot whatever the group had banked before this raid. Once a raid concludes, the town becomes Recovering for a flat cooldown (can't be targeted again for a while), and the bandits pick a nearby road tile and go into Looking for Rest.
 - Looking for Rest
   - Description: bandits travel back from a raid before actually settling into Rest.
   - Effects: Small combat buff, small movement debuff (tired but in good spirits). No detection radius, same as Resting - only engages if the player closes to the engagement radius (1 tile).


### Behavior - Configurable Values
#### Basic
 - Movement Speed per bandit tier (player caravan speed ranges 7 at the start up to 24 with upgrades, for reference): Tier I: 6, Tier II: 8, Tier III: 11, Tier IV: 15

#### Rest
- days per rest cycle (5)
- relocation distance min/max (2-5 tiles)
- relocation cooldown (3 cycles)
- relocation chance per cycle (15%), stacks whenever they haven't relocated.
- hire cost per tier (I: 50, II:75, III:100, IV:125)
- upgrade cost per tier  (I:200, II:400, III:600, IV:800)
- max unit count per tier (I:5, II:10, III:15, IV: 20)
- upgrade threshold (I:3; II:6; III:8, IV:11)
- raid threshold per tier (separate from upgrade threshold, deliberately higher so upgrading is statistically preferred) (I:4, II:7, III:9, IV:12)
- min and max hire per rest cycle  per tier (I:1-2, II:1-3, III:2-3, IV:2-4)
- consumption of coin per bandit per day (probably per level => low tier bandits are cheaper than hier tier) (2,3,4,5)
- Combat Debuff (Resting)

#### Rushing to Raid
- Movespeed Buff (Rushing)
- Combat Buff (Rushing)
- Detection Radius for Player (2.5 tiles) - engagement radius is unchanged at 1 tile (see Combat Configurable Values)

#### Raiding
- Combat Debuff (Raiding)
- Raid length per town Tier per bandit Tier
              BanditI   BanditII    BanditIII   BanditIV
  TownI       7         6           5           4
  Town II     9         8           7           5
  Town III    15        10          8           6
- Stolen goods per unit per tier (I:1, II:2, III:3, IV:4 units of goods - kept lower than stolen coin since raids are primarily a gold drain)
- Stolen coin per unit per tier (3, 5, 7, 10)
- Max loot capacity per tier - NOT per unit: a flat total cap per group per tier, split into a fixed total goods count (irrespective of good type/tier) and a fixed coin amount.
  - Tier I: 25 goods, 200 coin
  - Tier II: 35 goods, 300 coin
  - Tier III: 50 goods, 450 coin
  - Tier IV: 75 goods, 700 coin
- Recovering duration (7 days, flat across all towns)

#### Looking for Rest
- Combat Buff (Looking for Rest)
- Movement Debuff (Looking for Rest)
- Detection Radius for Player: 0 tiles, same as Resting - only engages if the player closes to the engagement radius (1 tile)

## UI

### Hiring Panel
- Accessed via the campsite tent, renamed "Companions & Guards" - houses the existing Companion Panel and this new Hiring panel as separate tabs, not merged into one screen.
- Hire flow: quantity slider/stepper to pick how many guards to hire, cost preview scales with quantity (per-guard cost is flat at the player's current captain level, but that per-guard cost itself increases at higher captain levels), then confirm.
- Displays: current guard count / cap, captain level, how many more guard slots the next captain level unlocks, daily upkeep total, gold on hand.
- No dismiss/fire mechanic for v1 - the only way to lose a guard is dying in combat.
- Guards are generic - no individual stats or portraits to browse; all guard strength comes from captain level, not per-unit variance.
- Visual layout is TBD to match the existing in-game guard UI - revisit once a reference screenshot is available.

### Pre-battle Summary
- Confirmation screen shown before the player commits to a fight - engaging a bandit group does not drop straight into combat, and the player can back out from here.
- Shows both sides: tiers and troop counts.

### Combat Screen
- Round-by-round battle view: sword-throw animation per unit each round, hit/death animations, hover a unit to see its active effects and damage received.
- "Next Round" control, plus an "Auto" toggle that plays rounds back to back.

### Win Screen
- Post-battle summary: unit losses on both sides.

### Win Loot Screen
- Shown after a win - player picks which goods to keep from what the bandit group had banked, plus the gold received.

### Loss Screen
- Post-battle summary: unit losses on both sides.

### Loss Loot Screen
- Shown after a loss - displays the resources and gold the player lost.

### Draw Screen
- Bandits disbanded, no loot for the player, player enters Recovering. Same post-battle summary treatment as win/loss (unit losses shown), but no loot screen follows since there's nothing to distribute.

### Bandit Group Tooltip
- Tier
- Unit count
- Combat score and health score for a single unit (all units in a group are identical)
- Buffs/debuffs list - each entry is icon + title + description

### Player Overlay (in-world)
- Unit count, bottom-left
- Status effect icon row, top-right
- Player's only possible status effect: Recovering (post-loss/draw)

### Bandit Group Overlay (in-world)
- Unit count, bottom-left
- Status effect icon row, top-right - one icon per current behavior state, plus any other active effects, all shown in a row

### Minimap Indicator
- Each roaming bandit group shows a single state icon on its minimap dot, reflecting its current behavior state (same icon set as the in-world overlay).
- Hovering gives a precise tooltip, e.g. "[Icon] Resting: The bandits are 15% weaker and 10% slower."

### Town Recovering Indicator
- Shown as an icon overlay on the town's map icon only - not called out separately in the Town UI panel.

### Notifications
- Bandit events (spawn, raid start, etc.) do NOT go through the existing minor/major notification system.
- Exception: a spawn SFX cue plays whenever a bandit group spawns, anywhere on the map, regardless of camera position (see SFX section).

## Art
- bandit commander icon
- bandit unit icon
- bandit camp icon (permanent spawn tile, separate from the roaming group's icon - same camp motif in gloomy colors)
- HP icon
- combat strength icon
- sword for attack animation
- bandit status effects
  - resting
  - raiding
  - travelling for raid



## SFX
- battle started ()
- battle won 
- battle lost
- attack
- death
- bandit group spawned (plays globally, regardless of camera position)
  
## Not for v1
- Campsite morale boost: pay gold to boost combat effectiveness before a fight. Mechanic never got nailed down (one-time vs timed, cost, stacking) - shelved for now.
- Recently-upgraded buff: minor combat buff right after a bandit group upgrades tier. Cut to keep only one active bandit effect at a time.
- Health-based combat penalty (unit.health_factor): wounded units fighting weaker as they lose HP. Cut for v1 simplicity - units fight at full strength until they die.
- Bandit groups with lots of loot should get a movement debuff (at 75% of their max capacity).
- Curve-based hit_factor tuning: sample hit_factor from an arbitrary drawn curve (e.g. Unity AnimationCurve) instead of a flat min/max, so the shape of variance/skew can be tuned visually. Flagged as an important future balancing tool, but v1 uses plain uniform random ranges.
- In the Engagement screen, there must be a button to attempt to flee. The flee chance should be derived from the difference in movespeed. If the player is faster, the flight chance scales linearly up to 80% when the player is 50% faster than the bandits. If the player has less mvoespeed than the bandits, fleeing is impossible ("Fleeing is impossble. You are too slow"). 