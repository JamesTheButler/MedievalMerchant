# Changelog - v0.2.2

## Language Support

The game now supports multiple languages. All in-game text has been localized and can be displayed in English, French, and German.

- Added language settings menu accessible from the start screen
- The game automatically detects your system language on first launch; if your language isn't supported, it defaults to English
- Changing language reloads the start screen to apply all translations
- Your language preference is saved and remembered across sessions
- Added support for extended Latin character sets (accented characters, etc.)
  - _a63f760 add localization and addressables packages_
  - _846a1ef add localization resources_
  - _85097cb move Localization to Features_
  - _fe900ad add language settings_
  - _d5bf590 allow locale changes in start menu_
  - _cf8bc9c add localization persistence_
  - _14cc362 support extended latin character set in font_
  - _and ~80 localization commits covering all UI: menus, trade, towns, companions, tutorials, tooltips, notifications, game over, onboarding, missions, goods, conditions, modifiers, credits_

## Trading Improvements

- Added quick-trade buttons for 15 and 30 goods, making bulk trades faster
  - _6c82fc5 add quick trade buttons for 15, 30 goods_
- Added safeguards to prevent completing invalid trades
  - _3485cfa add error proofing to trade ui to avoid illegal trades_
  - _4304466 trade completely uses completed trade_

## Audio

- Added win and loss sound effects for level completion
  - _a86660f add win and loss audio_
- Added fireworks sound effect when a town upgrades
  - _5ea4ac4 add fireworks sfx to town upgrade_
- Normalized volume levels across all music tracks and sound effects for a more consistent audio experience
  - _6c367c9 normalize music_
  - _fa7c924 normalize SFX_

## Visual Improvements

- Added smooth slide-in and fade animations to UI panels
  - _488a1f4 add DOTween_
  - _5e75fa8 faded background animations_
  - _5642606 panel slide in animations_
- Added coin glint animation
  - _5b9e9ef add coin glint animation_
- Added lights to town upgrade fireworks
  - _75c5809 add lights to town fireworks_
- Improved forest and field map tiles
  - _ccef071 new forest tiles_
  - _ebd62b3 field tile fixes_

## UI Improvements

- Added major notification popup for important game events (e.g. loss condition warnings)
  - _018c36d major notification popup_
  - _b06ca42 fix loss con warnings and add notification for them_
- Player funds change indicator now hides when the value is zero
  - _a9156cb hide player funds change text if its 0_
- Unified close-button styling across all panels
  - _3658682 unified X-Button prefab_
- Various pixel-perfect fixes and layout improvements across UI elements
  - _6080e67 improve town ui sub-header sizes_
  - _6fa2545 fix pixel errors in ui elements_
  - _d29ae76 fix pixel errors in base asset_
  - _e4e876a pixel tweaks_
  - _31891c1 fixes to icon buttons, TitleDescription tooltip and caravan mini ui_

## Bug Fixes

- Fixed a bug where the game over screen could crash when displaying the player's favorite good
  - _16a5b82 fix bug in game over ui for favorite good_
- Fixed a bug where town inventory slots were not working correctly
  - _7ed8705 fix bug about town inventory slots_
- Fixed a bug where the caravan panel could not be opened in certain situations
  - _e71275c fix open caravan bug_
- Fixed incorrect level difficulty display
  - _f50efe3 fix level difficulties_
- Fixed date gauge tooltip
  - _37b0090 fix date gauge tooltip_
- Fixed win/loss condition panels not displaying correctly in-game
  - _1b5bbc4 fix ingame win/loss conditions and level/event conditions panels_
- Fixed a startup error related to editor initialization
  - _605bff2 fix NRE on editor startup_
