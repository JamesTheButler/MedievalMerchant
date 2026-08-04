# Bandits and Guards

## Overview
Bandits roam the world. Maps can contain bandit camp tiles. These bandit camps spawn groups of bandit on a random road tile within their radius. Bandits then loiter, roam the streets, attack the player and raid towns. When they successfully raid a town, they steal gold and goods from that town. The town will be slowed down significantly in their development and have lowered production. Towns rely on the player for help. If a town feels neglected, this can impact their player reputation. Bands of bandits use the raided goods and coin to upgrade to the next level, making them harder to defeat and more aggressive in their raids.
Players can fight bandits by moving close to them or by entering a town that is being raided. In order to fight, the player will have to hire guards. Guards can die in battle and will need to be rehired. The player has access to a new companion: the captain. The captain can be upgraded like other companions. The level of the captain will affect the players guards' combat abilities. To upgrade the captain and to hire new guards, the player will use the companion tent at the campsite.
During battle, bandits and guards can die. Whoevers runs out of units first, will lose the battle.
Should the player win the battle, the bandit group is disbanded. The player gets some of the gold and goods that they had in their inventory. The player should be able to choose which goods to keep, in the case that their inventory is full.
Should the bandits win the battle, the player loses some of their gold, which gets added to the bandits' inventory. The player gets a debuff to their movement speed and becomes untargetable to bandits.


## Combat
Both the player and a group of bandits are represented as combatants. A combatant is represented by their units' base health and combat points. Combatants than can have upgrades, typically in the form of a higher commander level or in the shape of temporary buffs/debuffs. All of a combatants units have the same max health and combat score, although units have their own current health. The captain and the bandit group both have multiple tiers, where each tier increases HP and combat score for the units. Bandits are generally weaker, but there are more in a group whereas guards are generally stronger but the player will not be able to hire as many.

Combat runs automatically, without player input and is resolved in rounds. Each round, every unit selects an opponents unit at random. The units combat score is multiplied by a randomized multiplier and subtracted from the selected units health. If that unit runs out of health, it dies. Rounds run until one side has no remaining units.
The formula for a units health change per attack would be 
hp_new = hp_old - (unit.combat_strength * commander.total_combat_multiplier * unit.health_factor) * hit_factor
- unit.health_factor is a fixed debuff that each unit gets to their combat strength, based on their helath. maybe each 25% health missing means 2.5% weaker combat strength. I wonder how adding this would change the dynamics of the fight
- the hit_factor is randomized for each hit and ranges from 0% to 200% of the resolved combat strength

Combatants get stronger for each level (of the captain, or of the bandit group), but there can also be temporary buffs and debuffs. This allows the player to be more strategic as to when to engage and when to sit back and wait. It will hopefully create more interesting gameplay as the player might have to make a choice: do i engage the bandit now, that they are strong or do I choose to miss an upgrade mission that is soon to run out.

### Combat - Buffs and Debuffs
- The player can pay at the campsite to boost morale. The price should be quite steep, but the resulting combat boost should be significant.
- The bandits have different combat buffs and debuffs depending on their current state:
  - Resting: minor debuff (they are licking their wounds and filling their bellies.)
  - Raiding a town: major debuff (they're fighting the townsfolk. this is a good moment to intervene)
  - Traveling to a raid: major buff (they're fired up!) 
  - Traveling from a raid: minor buff (they're tired but victorious) 
  - Recently upgraded their level: minor buff (we could leave this out, to make code simpler for v.1. Without this, there can only be one active effect and they can be cleanly mapped to behavior states)
- Bandit buffs and debuffs must be clearly visible on the minimap with a precise tooltip. "[Icon] Resting: The bandits are 15% weaker and 10% slower."


### Configurable Values - Combat
- base health per level (bandits and guards)
- base combat strength per level (bandits and guards)
- max unit count per level (bandits and guards)
- randomized damage multiplier (bandits and guards)
  - I can play with this to make combat more engaging. Perhaps bandits have higher changes for really low and really high hits, whereas guards are more reliable with a slight skew towards a positive multiplier.
  ![alt text](image.png)
- guard morale boost effect
- bandit debuff while resting
- bandit debuff while raiding
- bandit buff while traveling to town
- bandit buff while recently upgraded


## Bandit Behavior
Bandit behavior is resolved as a state machine. The Rest state is the default.
 - Rest: 
   - Description: Bandits occupy a road tile for X days. Each day they are resting, they consume gold and goods depending on their tier and unit count. 
  At the end of a rest cycle, they may start a new rest cycle or go on a raid. If they choose to rest, they can hire units, upgrade their tier (in both cases they stay where they are) or they relocate a couple of tiles away.
    - hire a unit (costs x coin and goods)
    - upgrade tier (chance scales with the )
   
   migh higher new ones, if they have the resources. They may also upgrade their tier. The longer they have rested and the fewer goods they have, the more likely they are to seek out a town to raid.
   - Effects: 
     - While resting, bandits are more vulnerable: Debuff to combat ability.
     - Should they attack the player at the end of a cycle, they will get a debuff to movement speed.
 - Relocating:
   - Description: the bandits have decided to move their camp.
 - Travelling: the bandits pick a raidable town at random and travel there. As they travel, they will attack the player if he is near.
 - Attacking player: They have increased movement speed and move towards the player. If they are near enough, a fight occurs. More on the fight later.
 - Raiding: When they reach a town, they will start a raid. A raid takes a few days. The larger a town, the longer it takes. More on raids later. Once a raid concludes, the bandits will pick a nearby road tile and go into Rest.
 - Looking for Rest


### Behavior - Configurable Values
- days per rest cycle
- consumption of coin per bandit per day (probably per level => low tier bandits are cheaper than hier tier)
- upgrade cost per tier  




## UI
- pre-battle summary (both side, their tiers, their troup counts)
- post-battle summary (who won, who lost, their unit losses)
- win screen: pick resources to steal and show gold that you got
- loss screen: show the resources and the gold you los
- commander + guard UI

## Art
- bandit commander icon
- bandit unit icon
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