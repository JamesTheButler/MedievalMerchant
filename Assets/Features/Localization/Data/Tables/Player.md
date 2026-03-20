# Localization: Player
Strings shown in various places around the players inventory, retinue/companions and caravan.

## Key Organization
The keys are organized roughly along the different panels of the UI.
- Player.Attributes.*: Strings relating to the different stats of the player and their caravan. A lot of these are visible when hovering different stats in the top and left UIs.
- Player.Caravan.*: Strings about the caravan, mostly shown in the caravan/inventory panel in the top-left (shortcut: [Q])
- Player.Companions.*: Strings about companions. Each companion has a title, description and a list of effects. These can most easily be seen buy upgrading a companion to level 1 and then hovering them in the retinue UI.

## Testing and Cheats
- Open cheat console via [F9].
- Testing Towns.Milestones.*: select town, open console [F9] and type `town.upgrade`. This will upgrade the town by one tier, showing you new milestones. You can do this for tier 1, 2 and 3 of a town.
- Testing Towns.Missions.*: 
    - Trade missions cannot be cheated but should popup regularly. All trade missions have the same strings.
    - Upgrade missions can be cheated: select town, open console [F9] and type `town.grow`. This will grow the town to development 100, triggering an upgrade mission.