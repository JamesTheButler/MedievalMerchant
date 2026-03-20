# Localization: Towns
Strinngs from the Towns table are shown in the Town UI on the right of the screen, when selecting a town.

## Key Organization
The keys are organized roughly along the different sections of the UI.
- Towns.Header.*: Strings in the top-most section of the town UI containing the towns flag, name, reputation and funds among others.
- Towns.Development.*: Strings shown in the "Development" section towards the top of the town UI. Some strings are shown when hovering the different elements.
- Towns.Milestones.*: Milestones are the square UI elements. They indicate milestones in the development of a town. Strings here are shown when hovering the various milestones.
- Towns.Missions.*: Shown in "Missions" section of town.
    - Towns.Missions.Details.*: Strings shown when hovering missions, mostly in the "On Success"/"On Failure" boxes
- Towns.Production.*: Shown in the bottom-most section

## Testing and Cheats
- Open cheat console via [F9].
- Testing Towns.Milestones.*: select town, open console [F9] and type `town.upgrade`. This will upgrade the town by one tier, showing you new milestones. You can do this for tier 1, 2 and 3 of a town.
- Testing Towns.Missions.*: 
    - Trade missions cannot be cheated but should popup regularly. All trade missions have the same strings.
    - Upgrade missions can be cheated: select town, open console [F9] and type `town.grow`. This will grow the town to development 100, triggering an upgrade mission