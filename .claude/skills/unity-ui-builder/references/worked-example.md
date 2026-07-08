# Worked Example: CampsiteCartUI

A real, hand-built (pre-skill) example demonstrating hierarchy, layout groups, cell reuse, and `Observable` binding together — `Assets/Features/Player/Camp/UI/CampsiteCartUI.prefab` + `Assets/Features/Player/Camp/UI/DetailedCartUI.cs` + `Assets/Common/UI/Elements/Cells/GoodCell.cs`. Read the actual files for full detail; this is the map.

Note: this project has a *second*, more feature-complete sibling attempt at the same "cart" UI — `Assets/Features/Player/Caravan/UI/CartStatsUI.cs` / `.prefab` — which additionally shows a locked/unlocked state split, a hover-preview mechanic, and affordability-based button interactivity. Both were orphaned (unwired) when found. If a task touches cart UI again, check which one (if either) has since been wired up and built upon, rather than assuming this simpler one is still the active version.

## Hierarchy

```
CampsiteCartUI (root — RectTransform + CanvasRenderer + DetailedCartUI script, no Canvas: nested under an existing one)
├── Background (Image)
└── Details Group (VerticalLayoutGroup)
    ├── Header (HorizontalLayoutGroup, no CanvasRenderer — pure layout container)
    │   ├── Waggon Text (TextMeshProUGUI + ContentSizeFitter)
    │   ├── Movespeed Group (HorizontalLayoutGroup)
    │   │   ├── Movespeed Icon (Image + LayoutElement, fixed 24x24-ish via LayoutElement not SizeDelta)
    │   │   └── Movespeed Text (TextMeshProUGUI + ContentSizeFitter)
    │   └── Upkeep Group (HorizontalLayoutGroup)
    │       ├── Upkeep Icon (Image + LayoutElement)
    │       └── Upkeep Text (TextMeshProUGUI + ContentSizeFitter)
    └── Inventory Cells (HorizontalLayoutGroup + ContentSizeFitter)
        ├── InventoryCell instance [0..3]   ← reused Assets/Common/UI/Elements/Cells/InventoryCell.prefab component set, not rebuilt from scratch
```

Notice the pattern: a "Group" GameObject that only carries a layout component (`HorizontalLayoutGroup`/`VerticalLayoutGroup`, optionally `ContentSizeFitter`) is the standard way to compose rows/columns — it has no visuals of its own, just RectTransform + layout MonoBehaviours, and its children do the rendering.

## The root script wiring

`DetailedCartUI`'s `[SerializeField]` fields map directly onto fileIDs of the objects above:

```yaml
m_EditorClassIdentifier: Assembly-CSharp::Features.Player.Camp.UI.DetailedCartUI
inventoryCells:
- {fileID: 4606592932010719048}   # → InventoryCell instance [0]
- {fileID: 5666744207795900840}   # → InventoryCell instance [1]
cartImage: {fileID: 2725854481829059834}   # → Background's Image component
waggonText: {fileID: 5022697919536362038}  # → Waggon Text's TextMeshProUGUI
moveSpeedText: {fileID: 5235873511158244512}
upkeepText: {fileID: 7166244451559095750}
cartString:
  m_TableReference:
    m_TableCollectionName: GUID:ff03fea2efc05eb4b8adc11fc8973523
  m_TableEntryReference:
    m_KeyId: 8180320621780992
tierIcon: {fileID: 347114442268797336}
```

## The C# side

`DetailedCartUI.Bind(Cart cart, int index)`:
- Sets `waggonText.text` immediately from a `LocalizedString` (`cartString.GetLocalizedString(index + 1)`).
- Tracks four `Observable<T>.Observe(...)` subscriptions in `_cartBindings` — level, slot count, upkeep, move speed — each updating one UI field when the underlying `Cart` model changes.
- Tracks one `Observe` per inventory slot in a *separate* `_slotBindings` group, so slots can be reset (`ResetSlots()`) independently of the cart-level bindings.
- `Unbind()` calls `_slotBindings.UnbindAll()` plus explicit `StopObserving` calls for the cart-level ones, then nulls `_cart` — guarded so double-unbind is a no-op.

`GoodCell` (base class for `InventoryCell`) shows the single-item-cell pattern: `SetGood(Good? good)` toggles the icon's active state and assigns a sprite looked up from `ResourceManager`/config data, and `IPointerClickHandler.OnPointerClick` raises `Clicked`/`RightClicked` events rather than taking a direct dependency on whatever uses the cell.

## What to copy from this example

- The "Group = layout-only container GameObject" pattern for every row/column in a new layout.
- Reusing `InventoryCell`/`GoodCell` for anything that displays a good/item, instead of rebuilding icon+amount+tooltip logic.
- The `Bindings`-per-logical-group shape for a `Bind()`/`Unbind()` script, especially when part of the view (like a variable-length list of slots) needs independent rebinding from the rest.
