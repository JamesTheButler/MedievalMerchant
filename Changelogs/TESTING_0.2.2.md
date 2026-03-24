# Testing Checklist - v0.2.2

## Localization — Core System

- [ ] First launch (no save file): game detects system language and applies it
- [ ] First launch with unsupported system language: defaults to English
- [ ] Saved locale persists across game restarts (check `LocaleSettings.save` file)
- [ ] Language settings UI shows correct toggle for current locale on open
- [ ] Switching language saves preference, reloads start scene, and displays new language
- [ ] Double-clicking language toggle doesn't cause errors or double-reload
- [ ] Extended Latin characters (accented: e, o, u, etc.) render correctly in all fonts
  - _14cc362 support extended latin character set in font_

## Localization — Start Screen & Menus

- [ ] All static text on start screen is localized
- [ ] Level selection screen: level names, descriptions, difficulty labels
- [ ] Escape menu: all buttons localized
- [ ] Audio settings: header and labels localized
- [ ] Credits UI: static labels localized (names stay as-is)
- [ ] Feedback UI: static labels localized
- [ ] Language settings panel: labels and toggle text localized
- [ ] Version text displays correctly
  - _ae470a4, 08959c1, 719a132, 79bb619, f057693, 5700169_

## Localization — Gameplay UI

- [ ] Town UI: name, tier descriptor, reputation, funds, development section — all localized
- [ ] Town UI: dynamic sizing works (long translated strings don't overflow)
  - _5c9ac04 fix up town translations and dynamic sizing_
- [ ] Trade UI: buy/sell button, funds summary, reputation summary, profit/loss text
- [ ] Trade UI: haggle level names, coin effect, reputation effect strings
- [ ] Trade UI: trade validation error messages (not enough coin, etc.)
  - _cb68f03 implement LOC for trade validation strings_
- [ ] Good tooltips: name, base price label, current price label with town name prefix
- [ ] Good selectors (recipe panel, etc.) display localized good names
- [ ] Mission tooltips: mission type, details, reward/penalty descriptions
- [ ] Companion UI: names, descriptions, level tooltips, upkeep strings
- [ ] Companion tooltips: effect descriptions per level
- [ ] Cart tooltips: tier label, upgrade details, upkeep per-day string
- [ ] New cart tooltip: localized
- [ ] Modifiable tooltips: modifier descriptions (flat, percentage, base value)
  - _1e73ef1, fa6adc1, 30e923b, 990c7f1_
- [ ] Modifiable tooltip: dynamic sizing doesn't clip content
  - _340d871 fix dynamic sizing in modifiable tooltip_
- [ ] Notification system: minor + major notification text localized
- [ ] Event list header ("Ongoing Events (N)") uses LocalizedText with args
- [ ] Timed game modifier: "X days left" uses LocalizedText with args
- [ ] Building names localized
  - _2a9b18d localize building names_
- [ ] Region names localized
  - _339a30d localize region names_
- [ ] Availability strings localized
  - _5cd2b2e localize availability_

## Localization — Conditions

- [ ] Win conditions progress: funds ("X/Y coin"), town tier, local rep, global rep
- [ ] Loss conditions progress: timeout ("X days left"), bankruptcy ("X days left in bankruptcy")
- [ ] Condition descriptions (from ScriptableObjects) use GetLocalizedString
- [ ] Warning messages and game over messages localized
- [ ] Conditions button shows "X/Y" format
  - _776a631, f5708a5_

## Localization — Game Over

- [ ] Win screen: title, stats labels (finish date, etc.), dynamic stats
- [ ] Loss screen: title, failure reason text
- [ ] Favorite good stat displays correctly (was a bug)
  - _16a5b82 fix bug in game over ui for favorite good_
  - _183924a, 73de51d_

## Localization — Tutorial & Onboarding

- [ ] Tutorial topics: titles and descriptions localized
- [ ] Tutorial chapter counter displays correctly
- [ ] Onboarding explainer text: smart strings render with correct args
- [ ] Onboarding task list: "Tasks" header localized
  - _0e149cc, 9ada0ba, 9e9f9e9, e84d97b_

## Localization — Level Modifiers & Events

- [ ] Level modifier names and descriptions (from ScriptableObjects)
- [ ] Event effect descriptions: production, reputation, price, movement speed, development
- [ ] EffectPercentModifier description format (e.g. "Event: Heavy Rain") localized
- [ ] Event-started notification shows description + effects
  - _9819886, 87a3edf, 6883a5c_

## Trading

- [ ] Quick trade buttons (15, 30, Max, Mission) all work
- [ ] Quick trade button for mission amount only visible when mission exists for that good
- [ ] Trade error proofing: can't complete trade with 0 amount or unaffordable amount
- [ ] Trade uses CompletedTrade flow correctly
  - _6c82fc5, 3485cfa, 4304466_

## Audio

- [ ] Win audio plays on level completion
- [ ] Loss audio plays on level failure
- [ ] Town upgrade fireworks have sound effect
- [ ] Music volume is consistent across all tracks (no sudden loud/quiet)
- [ ] SFX volume is consistent across all effects
  - _a86660f, 5ea4ac4, 6c367c9, fa7c924_

## UI Animations

- [ ] Panels slide in smoothly when opened
- [ ] Background fades in when panel opens
- [ ] Animations don't break panel open/close state
- [ ] DOTween doesn't cause issues on scene transitions
  - _488a1f4, 5e75fa8, 5642606_

## Visual / Art

- [ ] New forest tiles look correct on maps
- [ ] Field tiles have no visual gaps or artifacts
- [ ] Coin glint animation plays correctly
- [ ] Town fireworks have light effects
- [ ] No pixel errors in UI elements
  - _ccef071, ebd62b3, 5b9e9ef, 75c5809_

## UI Fixes

- [ ] Unified X-button works correctly on all panels
- [ ] Player funds change text hidden when value is 0
- [ ] Major notification popup displays and dismisses correctly
- [ ] Loss condition warnings trigger major notification
- [ ] Town UI sub-header sizes look correct
- [ ] Campsite panel layout is correct
- [ ] Caravan panel can be opened from all valid states
- [ ] Date gauge tooltip shows correct info
- [ ] Win/loss conditions panel displays correctly in-game
  - _3658682, a9156cb, 018c36d, b06ca42, 6080e67, 2a102f3, e71275c, 37b0090, 1b5bbc4_

## Companion System (WIP — verify no regressions)

- [ ] CompanionModel refactor didn't break existing companion behavior
- [ ] Companion upkeep modifiers still apply correctly
- [ ] Companion upgrade (if accessible) doesn't crash
- [ ] Campsite UI displays companions correctly
  - _5092892, 267210c, 267bb90, e30d9e0, 361364a, 942dfb2, e3ba58e_

## Regression Checks

- [ ] Town inventory slots work correctly
  - _7ed8705_
- [ ] Level difficulties display correctly in level selection
  - _f50efe3_
- [ ] Editor startup: no NRE from initializable singletons
  - _605bff2_
- [ ] Ally selection UI works correctly
  - _3fd1d8f_
- [ ] Add cart button works and is localized
  - _743c554_
