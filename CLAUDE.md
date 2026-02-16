# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Medieval Merchant is a strategy/trading simulation game built in **Unity 6 (6000.2.x)** with **C#**. The player manages a traveling caravan, trading goods across a medieval world to help towns prosper. Released on Steam in Early Access.

## Build & Run

This is a Unity project — there is no CLI build command. Open the project in Unity Editor (version 6000.2.15f1). The solution file `MedievalMerchant.sln` is auto-generated and gitignored; open it via Unity or Rider/VS for C# editing.

Key scenes in `Assets/Scenes/`:
- `StartScene.unity` — main menu / level select
- `GameplayScene.unity` — the core gameplay scene
- `UIDevelopment.unity` — UI testing sandbox

There are no automated unit tests in the project code; `Test/` directories contain in-editor testing MonoBehaviours (e.g., tooltip tester, tick tester).

## Architecture

### Two-Tier Context System

The game uses singleton context objects that own all models, services, and systems:

- **`GlobalContext`** (`Assets/Common/Infrastructure/Global/`) — persists across scenes (`DontDestroyOnLoad`). Owns `GlobalModel`, `GlobalServices`, `GlobalSystems`, and `PersistenceServices`.
- **`GameplayContext`** (`Assets/Common/Infrastructure/Gameplay/`) — exists only during a level. Owns `GameplayModel`, `GameplayServices`, `GameplaySystems`, and `Selection`. Access via `GameplayContext.Instance`.

### Model / System / Service Pattern

- **Models** — plain C# classes holding game state via `Observable<T>` and `ModifiableVariable`. No Unity dependencies where possible.
- **Systems** (`ISystem`) — implement game logic by reacting to model changes. Created per-level in `GameplaySystems` (global systems, player systems, and per-town systems). Not accessed outside init/teardown.
- **Services** (`IService`) — provide utility operations that other code actively calls (e.g., `TradeService`, `TickingService`, `NavigationService`).

All implement `IInitializable` (with `Initialize()` and `CleanUp()` lifecycle methods).

### Observable & Modifiable Infrastructure

`Assets/Common/Infrastructure/Observation/`:
- `Observable<T>` — value wrapper that notifies subscribers on change. Supports `Observe(callback)` which returns an `IBinding` for unsubscription.
- `ObservableEvent` / `ObservableEvent<T>` — event broadcasting.

`Assets/Common/Infrastructure/Modifiable/`:
- `ModifiableVariable` — an `Observable<float>` whose value is computed from a `BaseValueModifier` plus stacked `FlatModifier` and `BasePercentageModifier` instances. Formula: `(base + flatSum) * (1 + percentSum)`. This is how all gameplay values (prices, speeds, rates) support dynamic modification from events, upgrades, town levels, etc.
- `IModifier` — has `Value`, `FormattedValue`, and `Description` observables.

### Data-Driven Configuration

- **`ConfigurationManager`** — singleton MonoBehaviour that selects between debug and release `Configurations` ScriptableObjects. Access configs via `ConfigurationManager.Configurations.TownConfig`, etc.
- **`ResourceManager`** — singleton MonoBehaviour holding references to all resource ScriptableObjects (art lookups, recipe data, localization, etc.). Access via `ResourceManager.Instance`.
- **`Configurations`** (`Assets/Common/Config/`) — aggregates all gameplay config SOs (CaravanConfig, TownConfig, GoodConfig, TickConfig, etc.).

### Level Bootstrapping

`LevelBootstrapper` in `Assets/Features/Levels/` orchestrates level startup: instantiates the map prefab, scans the tilemap for town positions, creates `Town` instances via `TownFactory`, initializes `GameplayContext.Model`, and applies level-specific gameplay modifiers.

### Feature Organization

`Assets/Features/` contains self-contained feature folders, each typically with subfolders for:
- `Config/` or `Data/` — ScriptableObjects and data classes
- `Logic/` — systems, services, and pure logic
- `UI/` — MonoBehaviours for presenting the feature

Key features: `Towns/` (production, development, reputation, missions, flags), `Trade/` (trading logic, haggling, price calculation), `Player/` (caravan, retinue/companions), `Goods/` (good types, recipes), `Map/` (tiling, pathfinding, zones), `Levels/` (level data, conditions, game modifiers), `Inventory/`, `Ticking/`, `Localization/`, `Tutorial/`, `Notifications/`, `Audio/`, `Stats/`, `Achievements/`.

### Common Utilities

`Assets/Common/` contains shared code:
- `Infrastructure/` — core framework (contexts, observable, modifiable, serialization)
- `UI/` — shared UI components, tooltip system (base classes in `Tooltips/`), inventory UI
- `Types/` — domain enums and value types (`Good`, `Region`, `Tier`, `Date`, `Availability`, `Difficulty`)
- `Utility/` — extension methods and helpers
- `Camera/` — camera management

### Key Third-Party Dependencies

- **NaughtyAttributes** — inspector enhancements (`[Required]`, `[Expandable]`, etc.)
- **SerializedCollections** — serializable dictionary support
- **DOTween** — animation tweening
- **Unity Localization** (com.unity.localization) — i18n
- **NuGetForUnity** — NuGet package management
- **Universal Render Pipeline (URP)** — rendering
- **SerializeReference Extensions** — polymorphic serialization support

## Conventions

- Namespaces mirror folder structure (e.g., `Features.Towns.Development.Logic`, `Common.Infrastructure.Modifiable`)
- ScriptableObjects are used for all configuration and balancing data — create them via `[CreateAssetMenu]`
- UI components observe models via `Observable.Observe()` and clean up bindings in `CleanUp()`/`OnDestroy()`
- The `Configs.prefab` in `Assets/Common/Infrastructure/` holds the `ConfigurationManager` singleton
- The `Resources.prefab` in `Assets/Common/Config/` holds the `ResourceManager` singleton
