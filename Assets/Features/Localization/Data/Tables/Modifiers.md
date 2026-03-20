# Localization: Modifiers
Modifiers are shown in 2 places:
- when selecting a level in the start screen
- in the top-right menu "Level Modifiers"

These modifiers impact gameplay buy changing some game mechanics and tweaking some parameters like movespeed, prices or prodcution speeds.

## Key Organization
The keys are organized roughly along the different panels of the UI.
- Modifiers.Event.*: Modifiers from events are shown in the event popup and in the top-right "Level Modifiers" menu.
- Modifiers.Level*.*: Level modifiers can be seen in the start menu when selecting a level before pressing "Play" and in the top-right "Level Modifiers" menu. 
- Modifiers.Effect.*: Effects are the actual changes to game mechancs and parameters. They usually change some value and use placeholders (e.g. {0} to dynamically display numbers, for example "+25% to sale prices")

## Testing and Cheats
- Open cheat console via [F9].
- Testing Towns.Milestones.*: select town, open console [F9] and type `town.upgrade`. This will upgrade the town by one tier, showing you new milestones. You can do this for tier 1, 2 and 3 of a town.
- Testing Towns.Missions.*: 
    - Trade missions cannot be cheated but should popup regularly. All trade missions have the same strings.
    - Upgrade missions can be cheated: select town, open console [F9] and type `town.grow`. This will grow the town to development 100, triggering an upgrade mission.