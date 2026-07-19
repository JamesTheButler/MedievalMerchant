# Testing Checklist - v0.3.0

> Supersedes `TESTING_0.2.2.md` — that release was drafted but never tagged/shipped, so its
> items are folded in here alongside everything new since. Scope: all commits since the last
> tagged release `0.2.1` (2026-02-05), refreshed through `a59a955` (2026-07-18).

## Localization — Core System

- [ ] First launch (no save file): game detects system language and applies it
- [ ] First launch with unsupported system language: defaults to English
- [ ] Saved locale persists across game restarts (check `LocaleSettings.save` file)
- [ ] Language settings UI shows correct toggle for current locale on open
- [ ] Switching language saves preference, reloads start scene, and displays new language
- [ ] Switching language does **not** cause a major lag spike/freeze
  - _294a5d7 fix major lag when changing language_
- [ ] Double-clicking language toggle doesn't cause errors or double-reload
- [ ] Extended Latin characters (accented: é, ö, ü, etc.) render correctly in all fonts
  - _14cc362 support extended latin character set in font_
- [ ] Font outlines render correctly on all localized text
  - _eea2137 fix font outlines, 8b26394 font fixes_
- [ ] Language Settings now lists German as a third selectable option (rewritten as a generic list of toggles, not hardcoded EN/FR) — each toggle correctly reflects the active locale on open, and selecting it applies German
  - _e6f0f13 enable German + localization fixes_
- [ ] Language settings header text fits without clipping/overflow after the shortened title
  - _c22176f shorten language settings header_

## Localization — Translation Coverage (DE / FR)

- [ ] German translations are complete and correct across all tables (Common, Modifiers, Trade, Towns, Player, Campsite, Goods, Conditions, Levels, Tutorial, Start Menu)
  - _b23d8d1, a1c4cbd, db29274, 434a09c and ~15 other German-translation commits, plus e6f0f13 enable German_
- [ ] French translations are complete and correct across all tables (same list as above)
  - _cce7a50, 26ed976, 353ffa3, 886a62c, 9676c0d, f393cf7, f4ff8de, 7b31981, d4016f9, a5ba0ef, cb817d7, b974f50, bb7bdb7, 3e0684f, f718100 and other French-pass commits_
- [ ] No untranslated/fallback-key strings visible anywhere in DE or FR (spot-check every screen, not just ones listed above)
- [ ] Previously-missing translation is now present and correct (verify the specific string, not just that it's non-empty)
  - _0db6718 fix missing translation_
- [ ] Longer DE/FR strings don't clip or overflow their containers (many dynamic-layout fixes below relate to this)
- [ ] Translator/credit links work correctly
  - _f1e007e add translator links_

## Localization — Dynamic Layout & Sizing

- [ ] Good/milestone tooltips resize correctly for long localized text
  - _b2571c8 fix dynamic layout in good and milestone tooltips_
- [ ] Start menu layout doesn't break when text length changes (language switch, refresh)
  - _2de8145, b6a304e_
- [ ] Tutorial UI layout holds up with longer translated strings
  - _1fe638b fix tutorial ui layout_
- [ ] General dynamic-text layouting fixes hold across screens
  - _09664e7, f1c37f5, fc52b11_
- [ ] Modifiable tooltips: dynamic sizing doesn't clip content
  - _340d871 fix dynamic sizing in modifiable tooltip_
- [ ] Town UI: dynamic sizing works (long translated strings don't overflow)
  - _5c9ac04 fix up town translations and dynamic sizing_

## Localization — Start Screen & Menus

- [ ] All static text on start screen is localized
- [ ] Level selection screen: level names, descriptions, difficulty labels
- [ ] Escape menu: all buttons localized
- [ ] Audio settings: header and labels localized
- [ ] Credits UI: static labels localized (names stay as-is)
- [ ] Feedback UI: static labels localized
- [ ] Language settings panel: labels and toggle text localized
- [ ] "Press Any Key" text displays and behaves correctly
  - _e553e5f improvements of Press Any Key text in start page_
- [ ] Duplicate start menu settings entries removed — each setting appears once
  - _30f8aab remove duplicate start menu settings_
- [ ] Version text displays correctly
  - _ae470a4, 08959c1, 719a132, 79bb619, f057693, 5700169_

## Localization — Gameplay UI

- [ ] Town UI: name, tier descriptor, reputation, funds, development section — all localized
- [ ] Trade UI: buy/sell button, funds summary, reputation summary, profit/loss text
- [ ] Trade UI: haggle level names, coin effect, reputation effect strings
- [ ] Trade UI: trade validation error messages (not enough coin, etc.)
  - _cb68f03 implement LOC for trade validation strings_
- [ ] Good tooltips: name, base price label, current price label with town name prefix
- [ ] Good selectors (recipe panel, etc.) display localized good names
- [ ] Mission tooltips: mission type, details, reward/penalty descriptions
- [ ] Companion UI: names, descriptions, level tooltips, upkeep strings
- [ ] Companion tooltips: effect descriptions per level
- [ ] Cart tooltips: tier label, upgrade details, upkeep per-day string; cart upgrade cost localized
  - _e1a1f43 add cart upgrade cost localization_
- [ ] Modifiable tooltips: modifier descriptions (flat, percentage, base value)
  - _1e73ef1, fa6adc1, 30e923b, 990c7f1_
- [ ] Notification system: minor + major notification text localized
- [ ] Event list header ("Ongoing Events (N)") uses LocalizedText with args
- [ ] Timed game modifier: "X days left" uses LocalizedText with args
- [ ] Building names localized
- [ ] Region names localized
- [ ] Availability strings localized and display correct names
  - _5cd2b2e, bc4ef78 improved availability names_
- [ ] Game date is localized correctly in all date displays
  - _404b2e4 localize game date_
- [ ] Tooltips never fall back to raw/untranslated text
  - _262e2a0 force localization in TitleDescriptionTooltip, 65ab289 enforce localization in simpletooltiphandler_

## Localization — Conditions, Game Over, Tutorial

- [ ] Win/loss conditions progress (funds, town tier, local/global rep, timeout, bankruptcy) localized
- [ ] Condition descriptions (from ScriptableObjects) localized; conditions list doesn't error at runtime
  - _32d36b4 fix runtime issues in conditions lists_
- [ ] Win screen: title, stats labels, dynamic stats
- [ ] Loss screen: title, failure reason text
- [ ] Favorite good stat displays correctly on game over (was a crash bug)
  - _16a5b82_
- [ ] Tutorial topics: titles, descriptions, "Tasks" header localized
- [ ] Onboarding explainer text (smart strings) renders with correct args
- [ ] Level modifier / event names, descriptions, and effect text localized (production, reputation, price, movement speed, development)

## Campsite (new feature)

- [ ] New game starts the player in the campsite rather than directly on the map
  - _a5eb440 player starts game in campsite_
- [ ] Camp tile appears correctly on the map and camp navigation works (arriving/leaving)
  - _c5301bc, 993787a_
- [ ] Camp is present and functional in the intro level
  - _67f35de add camp to intro level_
- [ ] Campsite storage: deposit/withdraw goods; storage UI reflects current state correctly
  - _c6f8f8e, 47f5503, 9530cd4, 5d8ba43 fix campsite storage_
- [ ] Campsite Cart Panel shows correct cart contents/state
  - _a00e3bd implement Campsite Cart Panel_
- [ ] Caravan tent placeholder UI displays correctly where final art is pending
  - _011bcf7_
- [ ] Campsite Companion Panel only opens/is interactive while actually in camp
  - _b2e249b_
- [ ] Retinue mini UI opens the Campsite Companion Panel (old RetinuePanel is fully gone, no dangling references)
  - _5c21cd8 retinue mini ui now opens campsite companion panel (deleted RetinuePanel)_
- [ ] Companion upkeep values shown in campsite panel are correct
  - _8115af9 correctly show upkeep in campsite companion page_
- [ ] Hovering a companion level in the campsite panel shows correct info
  - _3ddf81f level hovering in campsite companion page_
- [ ] Producer Tooltip displays correct info in camp
  - _5f9faba implement Producer Tooltip_
- [ ] Camp signage/art displays correctly (new art assets)
  - _5bd465b, 27e5972, 714455e, a713488 (placeholder AI art removed)_
- [ ] Campsite companion button visibility respects its feature flag
  - _39e02a6 hide campsite companion button based on feature flag_
- [ ] Campsite panel layout is correct at various resolutions
  - _fc52b11, aeef92b, 2a102f3_
- [ ] All escapable dynamic panels (including campsite ones) block gameplay input while open, and un-block on close
  - _2518df6 all escapable dynamic panels block gameplay inputs_
- [ ] All camp panels are disabled/inaccessible when the player is not actually in camp (broader fix beyond just the companion panel)
  - _a59a955 disabled camp panels when not in camp._
- [ ] Producer Tooltip is hidden (not shown empty) for producer groups with no producers
  - _c7ab2ec disabled producer tooltip for empty producer group UIs_
- [ ] Producer popup no longer throws an error in any producer state
  - _3c16c2e fix producer popup error_
- [ ] Camp storage inventory no longer shows an unrelated "log" popup when interacted with
  - _a54bd85 camp storage inventory no longer shows log popup_
- [ ] Hovering the *next* (not-yet-reached) companion level shows the correct upkeep value
  - _c2032b7 hover next companion level shows correct upkeep_
- [ ] Production Building tooltip displays correctly
  - _f040f0f fix Production Buliding tooltip_
- [ ] Campsite panel tooltips read clearly after simplification (no missing info)
  - _e6cd892 simplify tooltip in campsite panel_
- [ ] Campsite Cart Panel only shows carts that are actually unlockable
  - _c8fc61e campsite cart panel - hide carts that aren't unlockable_
- [ ] Cart upgrade modifier text clearly and correctly describes the upgrade's effect
  - _cdc5567 improve cart ugprade modifier text_
- [ ] Cart upgrade step in onboarding/progression works correctly end-to-end
  - _2bacc2c fix cart upgrade step and missing onboarding task to wait for 30 berries_
- [ ] Caravan panel UI refreshes immediately after unlocking a new cart (no stale display requiring reopen)
  - _1f9a665 refresh caravan panel ui when unlocking new carts_

## Onboarding & Tutorial (new/updated)

- [ ] Onboarding includes a campsite step that triggers at the right point and highlights the correct element
  - _79c5a92 add campsite step to onboarding_
- [ ] Tutorial content referencing the campsite is accurate and up to date
  - _a8e3a2b update tutorial with campsite_
- [ ] Onboarding blinker/highlight is positioned correctly on every step, not just the new campsite one
  - _1b37c58 fix onboarding blinker position bug_
- [ ] Onboarding task that waits for "30 berries" now appears/completes correctly (was previously missing)
  - _2bacc2c fix cart upgrade step and missing onboarding task to wait for 30 berries_
- [ ] Tutorial-related art assets import with correct settings (no blurry/wrong-format sprites)
  - _4584bf4 small import settings fix for tutorial assets_

## Feedback & Crash Reporting (new)

- [ ] Feedback form now submits via Sentry (`SentrySdk.CaptureFeedback`) instead of the old Google Form — confirm submitted feedback actually shows up in the Sentry dashboard
  - _82a565f add sentry and disable google-based feedback_
- [ ] Feedback form still shows a confirmation/closes correctly after submitting (event is now `ObservableEvent` instead of a C# `Action`)
- [ ] Escape menu's feedback entry point still opens and submits correctly
  - _82a565f (EscapeMenu.cs)_
- [ ] Sentry only activates in actual builds, not in the Editor — verify no Sentry network calls fire during editor play
  - _d0e648e configure sentry for builds only_
- [ ] Sentry breadcrumb trail (100 configured) doesn't cause a noticeable performance hit
  - _f809174 100 sentry breadcrumbs_
- [ ] Sentry sampling/options behave as configured with no error dialogs or blocking network calls surfaced to the player
  - _5208a19 sentry option tweaks_
- [ ] Force a crash/exception in a built (non-editor) player and confirm it's captured in Sentry

## Companion Missions & Upgrades (new feature)

- [ ] Companion upgrade mission requirements are enforced correctly (rule-based)
  - _b3bd8b2 rule based companion mission requirements_
- [ ] Mission delivery panel shows an info tooltip when delivery is currently impossible
  - _189584f_
- [ ] "Or Pay" button in the delivery panel works and applies the correct cost
  - _0992f9c improve Or Pay button in delivery panel_
- [ ] Substitute payments are accepted for companion upgrade missions when the exact good is unavailable
  - _eae0c8d implement substitute payments for companion upgrade missions_
- [ ] Companion mission delivery flow completes end-to-end without errors
  - _133d2b8 implement companion mission delivery_
- [ ] Companion frames render correctly for every companion tier (including any without final art)
  - _3395bf3, 05be83c_
- [ ] Coin cell in companion UI shows hover outline
  - _0b85784_
- [ ] No regressions vs. pre-refactor companion behavior (old behavior revert was intentional — verify nothing broke)
  - _7b3b3dc revert old companion behavior_
- [ ] CompanionModel refactor + upkeep-modifier move didn't break existing companion stats/behavior
  - _5092892, 267210c, 267bb90, e30d9e0, 361364a_

## Slot-Based Inventory (new feature)

- [ ] CaravanSlotService correctly manages cart slot allocation (add/remove/move goods between slots)
  - _93f48bc add CaravanSlotService_
- [ ] No errors/exceptions surfaced from the slot-based inventory rework
  - _2b8aa34 fix errors_
- [ ] Caravan cart panel UI still correctly reflects slot contents after the rework

## Trade & Price

- [ ] Buy/sell prices calculate correctly in all towns (Trade/Logic/Price had multiple changes this cycle — verify no regression)
- [ ] Quick trade buttons (15, 30, Max, Mission) all work
- [ ] Quick trade button for mission amount only visible when a mission exists for that good
- [ ] Trade error-proofing: can't complete a trade with 0 amount or an unaffordable amount
- [ ] Trade uses the CompletedTrade flow correctly end-to-end

## Player / Movement

- [ ] Player avatar doesn't jitter from rapid/spam clicking
  - _03e1397, 13b71ed stop click-spam jitter on player avatar_
- [ ] Game speed / movement speed changes apply immediately, without delay
  - _43d0dfb fix delayed application of game and movespeed in RoadTraveler_
- [ ] Road-traveler abstraction (prep for future bandit encounters) hasn't changed normal town-to-town travel behavior
  - _52da7b1 abstract logic from road traveler to prepare for bandits — feature itself is not yet playable, this is a regression check only_
- [ ] IMapLocation abstraction: town-based win/loss/other conditions still evaluate correctly now that Camp is also a map location
  - _79f62ca abstract IMapLocation from Town_

## Audio

- [ ] Win audio plays on level completion
- [ ] Loss audio plays on level failure
- [ ] Town upgrade fireworks have a sound effect
- [ ] Music volume is consistent across all tracks (no sudden loud/quiet)
- [ ] SFX volume is consistent across all effects

## UI Animations & Visual

- [ ] Panels slide in smoothly when opened; background fades in
- [ ] Animations don't break panel open/close state; DOTween doesn't cause issues on scene transitions
- [ ] Popup close animations don't cause a tiling/visual glitch
  - _42791c4 fix tiling issue in popup close animations_
- [ ] New forest tiles and field tiles look correct with no visual gaps or artifacts
- [ ] Coin glint animation plays correctly
- [ ] Town fireworks have light effects
- [ ] No pixel errors in UI elements

## UI Fixes & Tooltips

- [ ] Unified X-button works correctly on all panels
- [ ] Player funds change text hidden when value is 0
- [ ] Major notification popup displays and dismisses correctly
- [ ] Loss condition notification correctly opens the win/loss UI
  - _8122329 loss condition notificication now opens win/loss ui_
- [ ] Simple error tooltips blink correctly and clean up (no leaked instances)
  - _5ff950b clean-up and add blinking to simple error tooltips_
- [ ] General tooltip fix holds up (no regressions in tooltip positioning/content)
  - _fbb67ad fix tooltip_
- [ ] Town UI sub-header sizes look correct
- [ ] Caravan panel can be opened from all valid states
- [ ] Date gauge tooltip shows correct info
- [ ] Win/loss conditions panel displays correctly in-game

## Feature Flags

- [ ] FeatureFlagObjectToggler correctly shows/hides all flagged objects (campsite companion button, etc.), including after the recent fix
  - _d8c775d miprove FeatureFlagObjectToggler_

## Regression Checks

- [ ] Town inventory slots work correctly
- [ ] Level difficulties display correctly in level selection
- [ ] Editor startup: no NRE from initializable singletons
- [ ] Ally selection UI works correctly
- [ ] Add cart button works and is localized
- [ ] Observable equality checks didn't change change-detection behavior anywhere relying on `Observable<T>`
  - _c5185b6 improve equality checks for observable_
