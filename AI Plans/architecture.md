# Medieval Merchant - Architecture Notes

## Two-Layer Context System
- **GlobalContext** (DontDestroyOnLoad): GlobalModel, GlobalServices, GlobalSystems, PersistenceServices
- **GameplayContext** (per-level): GameplayModel, GameplaySystems, GameplayServices, Selection
- Both accessed via static `.Instance`

## Initialization Flow
1. LevelBootstrapper loads on scene start
2. Instantiates map from prefab
3. Creates PlayerModel and towns via TownFactory
4. Initializes GameplayContext
5. Initialize() on all services, systems, InitializableBehavior objects
6. Applies level game modifiers

## Key Singletons
- `ConfigurationManager` - Debug/Release Configurations ScriptableObject profiles
- `ResourceManager` - 15+ SerializedDictionary collections for static data (icons, names, recipes, etc.)

## Observable Pattern
- `Observable<T>` wraps values, fires events on change
- `.Observe()` returns `IBinding` for cleanup
- `ObservableEvent` variants (0-3 type params)
- `ModifiableVariable` extends Observable<float> with modifier stack

## Feature Folder Pattern
```
Features/{Name}/
  Config/  - ScriptableObjects
  Logic/   - Models, Systems, Services, Managers
  UI/      - MonoBehaviours, Prefabs
  Data/    - Serialization classes
```

## Scenes
- GameplayScene, StartScene, UIDevelopment
- Test: TooltipTest, TilingTest
