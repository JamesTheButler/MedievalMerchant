# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.


## Project Overview

Medieval Merchant is a strategy/trading simulation game built in **Unity 6 (6000.2.x)** with **C#**. The player manages a traveling caravan, trading goods across a medieval world to help towns prosper. Released on Steam in Early Access. I am using the unity localization package to roll out localizations.


## Build & Development

- **Unity version**: 6000.2.15f1 (Unity 6)
- **Solution**: `MedievalMerchant.sln` — open in Unity or your IDE
- **Build outputs**: `Builds/` directory (versioned releases)
- **No CLI build/test commands**: This is a Unity project — build, run, and test through the Unity Editor
- **Key packages**: Unity Localization 1.5.9, URP 17.2.0, Input System 1.16.0, NaughtyAttributes, NuGetForUnity (git), SerializeReference Extensions (git)


## Aim for Claude

I want to make this project localizable and eventually localize it into different languages. I will NOT consider non-RTL languages. The game is fully written in English for now. All strings are hard-coded into .cs files, or written in scriptable objects, prefabs or scenes. We need to extract all strings into localizable string tables (as supported by the Unity localization package). For this, we can use .csv files. It is important to maintain a human-readable structure for the tables and the keys within the tables.
The project is built in a data-driven way. There is a ConfigManager and ResourceManager that provides singleton-esque access to fundamental data. Configs are data that can be tweaked to influence gameplay, such as a movementspeed variable, or a town growth speed multiplier. Resources are static, such as an icon for a good or the name of a companion. It is important to reflect this data-driven approach in the tables. Keys should be structured to reflect the organization of featuers in the folder structures.


## Architecture

### Two-Layer Context System

The game runs on two context singletons that manage lifecycle:

- **`GlobalContext`** (`Assets/Common/Infrastructure/Global/`) — persists across scenes via `DontDestroyOnLoad`. Holds `GlobalModel`, `GlobalServices`, `GlobalSystems`, and `PersistenceServices`. Initializes on `Awake()`.
- **`GameplayContext`** (`Assets/Common/Infrastructure/Gameplay/`) — lives per-level. Holds `GameplayModel`, `GameplaySystems`, `GameplayServices`, and `Selection`. Created when a level scene loads; cleaned up on destroy.

Both are accessed via static `Instance` properties.

### Initialization Flow

1. `LevelBootstrapper` loads on scene start
2. Instantiates the map from a prefab
3. Creates `PlayerModel` and towns via `TownFactory`
4. Initializes `GameplayContext` with all data
5. Calls `Initialize()` on all services, systems, and `InitializableBehavior` objects
6. Applies level game modifiers

All major systems implement `IInitializable` (with `Initialize()` and `CleanUp()` methods).

### Observable/Reactive Pattern

`Observable<T>` wraps values and fires events on change. UI and systems subscribe via `.Observe()` which returns an `IBinding` for cleanup. `ObservableEvent` variants (0-3 generic params) handle events. This is the primary mechanism for connecting model state to UI.

### Modifiable Variables

`ModifiableVariable` extends `Observable<float>` with a modifier stack. Any gameplay value (prices, speeds, growth rates) can be dynamically modified by events, companion levels, reputation, level settings, etc. Modifiers implement `IModifier` (flat, percentage, average, custom).

### Data-Driven Configuration

- **`ConfigurationManager`** — singleton with Debug/Release `Configurations` ScriptableObject profiles. Access via `ConfigurationManager.Configurations`. Configs are tweakable gameplay values.
- **`ResourceManager`** — singleton providing static data (icons, names, recipes, etc.) via 15+ `SerializedDictionary` collections. Resources are read-only reference data.

ScriptableObjects are created via standardized `CreateAssetMenu` paths under `Medieval Merchant/`.

### Service vs System Distinction

- **`IService`** — persistent feature-level logic (e.g., `NavigationService`, `NotificationService`, `CheatService`)
- **`ISystem`** — frame-based or tick-based update logic (e.g., `PlayerTickSystem`, `GameSfxSystem`)

Both are managed by their respective containers (`GameplayServices`/`GameplaySystems` or `GlobalServices`/`GlobalSystems`).


## Key Features & Organization

`Assets/Features/` contains self-contained feature folders, each typically with subfolders for:
- `Config/` or `Data/` — ScriptableObjects and data classes
- `Logic/` — systems, services, and pure logic
- `UI/` — MonoBehaviours for presenting the feature

Key features: `Towns/` (production, development, reputation, missions, flags), `Trade/` (trading logic, haggling, price calculation), `Player/` (caravan, retinue/companions), `Goods/` (good types, recipes), `Map/` (tiling, pathfinding, zones), `Levels/` (level data, conditions, game modifiers), `Inventory/`, `Ticking/`, `Localization/`, `Tutorial/`, `Notifications/`, `Audio/`, `Stats/`, `Achievements/`.

### Shared Infrastructure

`Assets/Common/` contains cross-cutting concerns:
- `Infrastructure/` — contexts, observation, modifiable variables, lifecycle interfaces
- `Types/` — core types: `Date`, `Difficulty`, `Good`, `Region`, `Tier`
- `UI/` — shared UI elements, tooltips, inventory UI, utilities
- `Utility/` — extension methods and helpers
- `Camera/` — camera management

### Namespace Convention

Namespaces mirror the folder structure:
- `Common.Infrastructure.Gameplay`, `Common.Infrastructure.Global`, `Common.Infrastructure.Observation`
- `Common.Types`, `Common.UI`, `Common.Utility`
- `Features.{FeatureName}.Config`, `Features.{FeatureName}.Logic`, `Features.{FeatureName}.UI`

### Code Conventions

- Classes are typically `sealed`
- Properties favor `readonly`/immutable patterns
- Modern C# features used: records, init-only properties, nullable reference types
- `JetBrains.Annotations` for null-safety attributes (`[CanBeNull]`, etc.)
- NaughtyAttributes for editor: `[Expandable]`, `[Required]`, `[SerializedDictionary]`, `[SubclassSelector]`


## Scenes

- `GameplayScene` — main gameplay
- `StartScene` — main menu and level selection
- `UIDevelopment` — UI testing/prototyping
- Test scenes: `TooltipTest`, `TilingTest`
