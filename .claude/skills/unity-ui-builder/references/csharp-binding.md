# C# Binding Conventions

## Base-class decision tree

- **Stateless helper, no lifecycle** → plain `MonoBehaviour` (e.g. `Clickable`, `Hoverable`).
- **Needs setup/teardown hooked to the GameObject's lifecycle** → `Common.UI.Elements.InitializableBehavior`:
  ```csharp
  public abstract class InitializableBehavior : MonoBehaviour, IInitializable
  {
      public abstract void Initialize();
      public virtual void CleanUp() { }
      private void OnDestroy() => CleanUp();   // CleanUp is called automatically
  }
  ```
  Implement `Initialize()`; override `CleanUp()` only if you have something to release.
- **Open/closable panel** → `Common.UI.Elements.Panels.DynamicPanel` (itself an `InitializableBehavior`). Override `OnInitialize()`, `OnOpen()`, `OnClose()` — see `TownUI.cs` for a real example.
- **Single-item display cell** (shows one good/item + optional amount) → `Common.UI.Elements.Cells.GoodCell` or `InventoryCellBase` — don't reinvent tooltip/click handling, extend these.
- **Data-bound composite view with no special lifecycle needs** (a panel section, a detail view) → plain `MonoBehaviour` with a `Bind(...)`/`Unbind()` pair, exactly like `DetailedCartUI` below. This is the most common shape for a new feature-specific UI script.

## The `Observable` / `Bindings` pattern

Every reactive UI subscription in this project follows the same shape: `Observable<T>.Observe(callback)` registers a callback and returns an `IBinding`; a `Bindings` instance collects those and releases them all at once.

```csharp
// Common.Infrastructure.Observation.Bindings
public sealed class Bindings : IBinding
{
    public void Track(IBinding binding);
    public void Track(params IBinding[] binding);
    public void Unbind();      // unsubscribes everything, then clears
}
```

Real usage, from `Assets/Features/Player/Camp/UI/DetailedCartUI.cs`:

```csharp
public sealed class DetailedCartUI : MonoBehaviour
{
    [SerializeField]
    private List<InventoryCell> inventoryCells;

    [SerializeField, Required]
    private Image cartImage;

    [SerializeField, Required]
    private TMP_Text waggonText, moveSpeedText, upkeepText;

    [SerializeField]
    private LocalizedString cartString;

    private Cart _cart;
    private readonly Bindings _cartBindings = new(), _slotBindings = new();

    public void Bind(Cart cart, int index)
    {
        _cart = cart;
        waggonText.text = cartString.GetLocalizedString(index + 1);

        _cartBindings.Track(
            _cart.Level.Observe(OnLevelChanged),
            _cart.SlotCount.Observe(OnSlotCountChanged),
            _cart.Upkeep.Observe(OnUpkeepChanged),
            _cart.MoveSpeed.Observe(OnMoveSpeedChanged)
        );

        // per-index bindings (e.g. a list of slots) get tracked in their own Bindings
        // instance so they can be torn down independently of the "whole cart" bindings
        for (var slotIndex = 0; slotIndex < cart.Slots.Length; slotIndex++)
        {
            var cellIndex = slotIndex;
            _slotBindings.Track(cart.Slots[slotIndex].Observe(entry => OnSlotChanged(cellIndex, entry)));
        }
    }

    private void OnUpkeepChanged(float upkeep) => upkeepText.text = upkeep.ToString("0.##");

    public void Unbind()
    {
        if (_cart == null) return;
        _slotBindings.Unbind();
        _cart.Level.StopObserving(OnLevelChanged);
        _cart.SlotCount.StopObserving(OnSlotCountChanged);
        _cart = null;
    }
}
```

Takeaways for a new UI script:
- One (or more) `private readonly Bindings _xBindings = new();` field per logical group of subscriptions you want to be able to tear down together.
- `Bind(TModel model, ...)` sets initial values directly, then tracks `Observe` callbacks for anything that changes over time.
- The class's own `Unbind()` calls `_xBindings.Unbind()` (and/or `StopObserving`) and nulls out the model reference — always guard against double-unbind (`if (_model == null) return;`).
- `Bindings` itself implements `IBinding`, so a group can be tracked inside another group and torn down with it. Never track a group in itself — `Unbind()` would recurse forever.
- For a *list* of dynamically-shown items (like inventory slots), track each item's binding separately so a single slot can be rebound without tearing down the whole view.

## `[SerializeField]` ↔ prefab wiring

Field names become YAML keys directly under `m_EditorClassIdentifier` in the prefab (see `yaml-anatomy.md`), each pointing at a component/GameObject fileID:

```yaml
inventoryCells:
- {fileID: 4606592932010719048}
- {fileID: 5666744207795900840}
cartImage: {fileID: 2725854481829059834}
waggonText: {fileID: 5022697919536362038}
```

`[Required]` (NaughtyAttributes) is a project convention for fields that must never be left unassigned — use it on anything the script would null-ref without, mirroring `DetailedCartUI`'s `[SerializeField, Required] private Image cartImage;`.

## Localization

Per this project's localization goal, prefer `LocalizedString` fields (`UnityEngine.Localization`) for any UI text over hardcoded strings, exactly as `cartString` is used above (`cartString.GetLocalizedString(...)`). If a new script needs one, add the field but leave the table/entry reference for the user to assign in the Editor's Localization picker unless an existing entry can be copied.
