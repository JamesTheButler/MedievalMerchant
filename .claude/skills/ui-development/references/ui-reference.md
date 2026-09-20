# UI Reference

Real code from this repo, one canonical example per shape you will need to write.
Copy the idiom, not just the intent — this project has its own conventions and
they are not the common Unity ones.

Design tokens are **not** repeated here. TMP style hashes, icon sizes and
paddings are constants at the top of `Assets/Editor/UIGenerationHelper.cs`; read
that file for their values. Builder methods go in `Assets/Editor/UIGenerator.cs`,
which imports the helper with `using static Editor.UIGenerationHelper;`.

## Conventions visible in every example below

- Classes are `sealed`. Namespaces mirror the folder path (`Features.{Feature}.UI`,
  `Common.UI.Elements`), written as a block namespace.
- `[SerializeField, Required]` on its own line above the field. `Required` is from
  NaughtyAttributes and goes on object references — not on `bool`/`float`/`Sprite`
  defaults, and not on `LocalizedString`.
- Same-typed fields are comma-grouped:
  `private TMP_Text nameText, levelText, upkeepValueText;`
- Private state is `_camelCase`. Bindings groups are
  `private readonly Bindings _bindings = new();`

---

## Element — pure setters

The whole element tier looks like this. No model, no context, no bindings; the
owner feeds it.

<!-- source: Assets/Common/UI/Elements/TextWithIconElement.cs -->
```csharp
namespace Common.UI.Elements
{
    public sealed class TextWithIconElement : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text text;

        [SerializeField, Required]
        private Image icon;

        public void SetUp(string newText, Sprite newIcon)
        {
            text.text = newText;
            icon.sprite = newIcon;
        }
    }
}
```

## Element — icons live in fields, not in the signature

`TextWithIconElement` above takes a `Sprite` because it is generic: the caller
decides what the icon is, and it differs call to call. That is the exception.

The rule is the other way round. An icon that is the same every time a given
prefab instance is used — the health icon on a stat row, the coin on a price row —
is a serialized field, assigned by hand in the editor on that instance. It never
travels through the API, and the owner does not hold a `Sprite` field to pass
down; that only moves the same decision one level up and out of the inspector.

<!-- source: Assets/Features/Combat/UI/TotalStatRow.cs (abridged) -->
```csharp
public sealed class TotalStatRow : MonoBehaviour
{
    // The health row gets the health icon, the strength row the strength icon.
    // Set once, in the editor.
    [SerializeField, Required]
    private Image icon;

    [SerializeField, Required]
    private TMP_Text value, delta;

    public void SetTotal(IReadOnlyObservable<float> total)
    {
        _bindings.Unbind();
        _bindings.Track(total.Observe(OnTotalChanged));
        SetDelta(0f);
    }
}
```

A `Sprite` earns a place in a signature only when the art varies with the model
and one prefab serves every case — `UnitToken.SetUnit(Sprite characterIcon,
CombatUnit unit)`, where the same token renders a guard or a bandit depending on
the combatant. Ask "could the user just drag the right sprite onto this
instance?" If yes, it is a field.

## Element — state swap

Sprite variants are serialized fields on the element, not looked up from
`ResourceManager`. That is what keeps the element dumb.

<!-- source: Assets/Features/Player/Retinue/UI/RetinueMiniProgressElement.cs -->
```csharp
namespace Features.Player.Retinue.UI
{
    public sealed class RetinueMiniProgressElement : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        [SerializeField]
        private Sprite completedSprite, incompleteSprite;

        public void SetCompleted(bool isCompleted)
        {
            image.sprite = isCompleted ? completedSprite : incompleteSprite;
        }
    }
}
```

---

## Component — symmetric Bind / Unbind

The minimal component shape. Two `Bindings` groups so the per-slot subscriptions
can be torn down independently of the whole-model ones. `Unbind()` guards against
being called twice.

<!-- source: Assets/Features/Player/Caravan/UI/CartInventoryUI.cs (abridged) -->
```csharp
public sealed class CartInventoryUI : MonoBehaviour
{
    [SerializeField]
    private List<InventoryCell> inventoryCells;

    [SerializeField, Required]
    private Image cartImage;

    [SerializeField]
    private LocalizedString cartString;

    private Cart _cart;
    private readonly Bindings _cartBindings = new(), _slotBindings = new();

    public void Bind(Cart cart, int index)
    {
        _cart = cart;
        ResetSlots();

        _cartBindings.Track(
            _cart.Level.Observe(OnLevelChanged),
            _cart.SlotCount.Observe(OnSlotCountChanged)
        );

        for (var slotIndex = 0; slotIndex < cart.Slots.Length; slotIndex++)
        {
            var cellIndex = slotIndex;   // captured per iteration, not by reference
            var binding = cart.Slots[slotIndex].Observe(entry => OnSlotChanged(cellIndex, entry));
            _slotBindings.Track(binding);
        }
    }

    public void Unbind()
    {
        if (_cart == null)
            return;

        _slotBindings.Unbind();

        _cart.Level.StopObserving(OnLevelChanged);
        _cart.SlotCount.StopObserving(OnSlotCountChanged);
        _cart = null;
    }
}
```

Note: this file resolves its label with `cartString.GetLocalizedString(index + 1)`
— a **positional** argument. Don't copy that. See *Localization* below.

`Observe` returns an `IBinding` and fires immediately by default, so `Bind()`
does not need to push initial values by hand. `Bindings.Unbind()` releases
everything it tracked and clears itself, so it is safe to call repeatedly.

## Component — composes, binds, localizes

The richest realistic example: a mission list rebuilt from scratch whenever the
model changes, with its per-item bindings in their own group.

<!-- source: Assets/Features/Player/Camp/UI/CampsiteCompanionPanelUiItem.cs (abridged) -->
```csharp
public sealed class CampsiteCompanionPanelUiItem : MonoBehaviour
{
    [SerializeField, Required]
    private TMP_Text nameText, levelText, upkeepValueText;

    [SerializeField]
    private LocalizedString levelString, deliveryString, hireString;

    [SerializeField, Required]
    private RectTransform missionItemContainer;

    [SerializeField, Required]
    private InventoryCell goodItemPrefab;      // typed prefab reference, never GameObject

    private readonly Bindings _bindings = new(), _missionBindings = new();

    private void Awake()
    {
        // Context and resource lookups happen once, at the component tier.
        _companionResource = ResourceManager.Instance.CompanionResources.Get(companionType);
        _companionModel = GameplayContext.Instance.Model.Player.RetinueModel.Companions[companionType];

        nameText.text = _companionResource.Name;
        missionItemContainer.DestroyChildren();
    }

    public void Bind()
    {
        Unbind();

        _bindings.Track(
            _companionModel.Level.Observe(OnLevelChanged),
            _companionModel.Upkeep.Observe(OnUpkeepChanged),
            _companionModel.ActiveMission.Observe(OnActiveMissionChanged)
        );
    }

    public void Unbind()
    {
        _bindings.Unbind();
        _missionBindings.Unbind();
        missionItemContainer.DestroyChildren();
    }

    private void SetLevelInfo(int level)
    {
        // Named smart-string argument. This is the shape to copy.
        levelText.text = levelString.GetLocalizedString(new { _int_Level = level });
    }

    private void OnUpkeepChanged(float upkeep)
    {
        upkeepValueText.text = upkeep.ToString("0.#");
    }

    private void OnActiveMissionChanged(CompanionMission mission)
    {
        missionItemContainer.DestroyChildren();
        _missionBindings.Unbind();

        if (mission == null)
            return;

        foreach (var (good, item) in mission.MissionItems)
        {
            var cell = Instantiate(goodItemPrefab, missionItemContainer);
            cell.SetGood(good);

            _missionBindings.Track(item.RemainingAmount.Observe(cell.SetAmount));

            var capturedGood = good;
            cell.Clicked += () => OnGoodCellClicked(cell, capturedGood);
        }
    }
}
```

---

## List population

`DestroyChildren()` then `Instantiate` a **typed** prefab reference. Per-item
bindings all go into one `Bindings` so a rebuild releases them together.

**There is no UI object pool in this project.** Don't invent one.

<!-- source: Assets/Features/Levels/Conditions/UI/InGameConditionListUI.cs -->
```csharp
public sealed class InGameConditionListUI : InitializableBehavior
{
    [SerializeField, Required]
    private InGameConditionListItem listItemPrefab;

    [SerializeField, Required]
    private GameObject listContainer;

    [SerializeField, Required]
    private Sprite incompleteIcon, warningIcon, completeIcon;

    private readonly Dictionary<ICondition, InGameConditionListItem> _listItems = new();
    private readonly Bindings _bindings = new();

    public override void Initialize()
    {
        _conditionResources = ResourceManager.Instance.ConditionResources;
    }

    public override void CleanUp()
    {
        base.CleanUp();
        _bindings.Unbind();
    }

    public void Setup(IEnumerable<ICondition> conditions)
    {
        Initialize();
        listContainer.DestroyChildren();

        foreach (var condition in conditions)
        {
            var listItem = Instantiate(listItemPrefab, listContainer.transform);
            listItem.Setup(condition.Description, _conditionResources.Conditions[condition.Type].Icon);
            _listItems.Add(condition, listItem);

            _bindings.Track(
                condition.Progress.CurrentValueText.Observe(listItem.SetProgressText),
                condition.Progress.IsCompleted.Observe(isCompleted =>
                    listItem.SetProgressIcon(isCompleted ? completeIcon : incompleteIcon))
            );
        }
    }
}
```

`DestroyChildren()` is an extension in `Assets/Common/Utility/GameObjectExtensions.cs`,
available on both `Transform` and `GameObject`, with a `<TComponent>` overload
that destroys only children carrying that component.

For a **bounded** list (a fixed number of slots), prefer pre-placing the cells in
the prefab as a `List<T>` field and toggling `gameObject.SetActive` — see
`CartInventoryUI.ResetSlots()` — over instantiating.

---

## Localization

### Static label — `LocalizedText`

`Assets/Features/Localization/UI/LocalizedText.prefab` carries its own
`LocalizedString` (the serialized field is called `staticString`) and pushes it
into the sibling `TMP_Text` on `OnEnable`/`OnValidate`. That means it **overwrites
any placeholder text you set**, and renders `<none>` until a key is assigned —
which is why the generator's `NewLocalizedText(...)` creates the entry and wires
the key.

`SetArgs(object)` feeds a smart string from code while keeping the label static:

<!-- source: Assets/Common/UI/DateGauge.cs -->
```csharp
public sealed class DateGauge : InitializableBehavior
{
    [SerializeField, Required]
    private LocalizedText dateText;

    private DateModel _gameDate;

    public override void Initialize()
    {
        _gameDate = GameplayContext.Instance.Model.DateModel;
        _gameDate.GameDate.Observe(OnDateChanged);
    }

    public override void CleanUp()
    {
        base.CleanUp();
        _gameDate.GameDate.StopObserving(OnDateChanged);
    }

    private void OnDateChanged(Date date)
    {
        var args = new
        {
            _int_Day = date.Day,
            _int_Year = date.Year,
        };
        dateText.SetArgs(args);
    }
}
```

### Code-set string — `[SerializeField] LocalizedString`

The default for text this element or component sets itself. Declared without
`[Required]`, resolved with `.GetLocalizedString(args)`. See
`CampsiteCompanionPanelUiItem` above.

### Shared string — `<Feature>LocalizationResources`

Only for strings the *logic* layer needs, or ones reused so widely that assigning
them per instance is a drag. These are `[Serializable]` classes aggregated onto
the `LocalizationResources` ScriptableObject and reached through
`ResourceManager.Instance.LocalizationResources.<Feature>`.

<!-- source: Assets/Features/Localization/Data/PlayerLocalizationResources.cs -->
```csharp
[Serializable]
public sealed class PlayerLocalizationResources
{
    [SerializeField]
    private LocalizedString fundsChangeModifier,
        movementSpeed,
        upgradeCostBase,
        cartUpkeep;

    // No arguments: expose as a property.
    public string FundsChangeModifier => fundsChangeModifier.GetLocalizedString();
    public string MovementSpeed => movementSpeed.GetLocalizedString();

    // ANTI-PATTERN. Positional {0} - a translator cannot tell what it means,
    // and cannot reorder it. Do not write new code like this.
    public string UpgradeCostBase(int level) => upgradeCostBase.GetLocalizedString(level);

    // CORRECT. Named smart-string arguments in an anonymous object.
    public string CartUpkeep(int cartIndex, int level)
    {
        var args = new
        {
            _int_Level = level,
            _int_Index = cartIndex,
        };
        return cartUpkeep.GetLocalizedString(args);
    }
}
```

The `_int_` / `_float_` prefixes are a project-wide convention that
`/mm-localize` treats as literal — preserve them exactly.

Helpers in `Assets/Common/Utility/LocalizationExtensions.cs`:
`text.SetLocalizedText(localizedString)` and
`localizedString.GetLocalizedStringOptional()` (empty string instead of throwing
when unassigned).

---

## Base classes

Pick by lifecycle need:

| Need | Base |
|---|---|
| Element, or a component driven entirely by its owner | plain `MonoBehaviour` |
| Setup/teardown tied to level bootstrap | `Common.UI.Elements.InitializableBehavior` |
| Hover tooltip on an element | `Common.UI.Tooltips.TooltipHandlerBase<TData>` on the hoverable object, `TooltipBase<TData>` on the tooltip prefab |

<!-- source: Assets/Common/UI/Elements/InitializableBehavior.cs -->
```csharp
public abstract class InitializableBehavior : MonoBehaviour, IInitializable
{
    public abstract void Initialize();

    public virtual void CleanUp() { }

    private void OnDestroy()
    {
        CleanUp();
    }
}
```

`CleanUp()` is called automatically on destroy, so an override must call
`base.CleanUp()`.

A tooltip handler is usually a one-liner — the base does the hovering,
instantiating, positioning and destroying:

<!-- source: Assets/Features/Player/Caravan/UI/CartTooltipHandler.cs -->
```csharp
public sealed class CartTooltipHandler : TooltipHandlerBase<Cart> { }
```

The owner then calls `handler.SetData(model)`. The handler needs its
`toolTipPrefab` field assigned — wire it in the builder method.

---

## A builder method

One panel for the visual block, a layout-only row inside it, an icon, a
code-driven number, and a static localized label. A real builder ends by adding
its code-behind component with `AddBehaviour` and calling `Assign(...)` with that
component's exact `[SerializeField]` field names.

<!-- source: Assets/Editor/UIGenerator.cs -->
```csharp
private static string BuildExampleElement()
{
    // One panel. The sprite carries its own border - don't wrap the contents in a
    // second "Inner" panel unless the design really shows a frame inside a frame.
    var root = NewPanel("ExampleElement", null, PanelKind.Background);
    SetSize(root, 220, 48);

    var row = NewRow("Row", PaddingMedium);
    row.transform.SetParent(root.transform, false);
    SetPadding(row, PaddingSmall);

    // Fixed for this element, so the icon is a serialized field wired below rather
    // than a parameter on its setter.
    var icon = NewIcon("Icon", row, DefaultIconSize);

    // Code-driven text: a plain TMP field, with a realistic placeholder so the
    // prefab reads correctly in the editor.
    var amount = NewText("Amount", row, 24f);
    amount.text = "1,240";
    SetTextStyle(amount, StyleTitle);

    // Static text: never a bare TMP field. The entry is created and wired here, so
    // the label shows real English in the editor instead of "<none>".
    NewLocalizedText(
        "Label", row,
        table: "Common",
        key: "Common.Example.Label",
        english: "in stock",
        comment: "Suffix after a goods count, e.g. \"1,240 in stock\".",
        styleHashCode: StyleSubtitle);

    // AddBehaviour, not AddComponent: it puts the code-behind directly under the
    // Transform so the prefab opens on the script rather than on uGUI plumbing.
    var component = AddBehaviour<ExampleElement>(root);

    Assign(component, new Dictionary<string, Object>
    {
        { "icon", icon },
        { "amountText", amount },
    });

    return Save(root);
}
```

A `[SerializeField] LocalizedString` on the code-behind is created and pointed at
its entry in the same pass, right after the `Assign(...)` above:

```csharp
AssignLocalizedString(
    component, "emptyString",
    table: "Common",
    key: "Common.Example.Empty",
    english: "Nothing in stock",
    comment: "Shown in place of the goods count when the store is empty.");
```

### Helper vocabulary

| Helper | Produces |
|---|---|
| `NewPanel(name, parent, PanelKind)` | An `Image` with a 9-slice panel sprite, drawn Tiled |
| `NewRow` / `NewColumn(name, spacing)` | A layout-only container — no `Image`, no `CanvasRenderer` |
| `NewIcon(name, parent, size)` | An `Image` sized by both `sizeDelta` and a `LayoutElement` |
| `NewText(name, parent, fontSize)` | A plain `TextMeshProUGUI` for code-driven text |
| `NewLocalizedText(...)` | A `LocalizedText` prefab instance with its entry created and key wired |
| `NewButton(name, parent, table, key, english, comment, icon, iconAfterText)` | `Button.prefab` instance; creates and wires its label entry |
| `NewIconButton(name, parent, icon)` | `ButtonWithIcon.prefab` instance, icon only |
| `NewCloseButton(name, parent)` | `XButton.prefab` instance, for a panel's top-right corner |
| `NewElement<T>(name, parent)` / `LoadElement<T>()` | An existing layer-1 element prefab from the output folder, found by class name |
| `AddBehaviour<T>(go)` | `AddComponent<T>` then moves it directly under the Transform |
| `NewFillImage(name, parent)` | Stretched `Image` in Filled/Horizontal mode, for bars |
| `FixedWidth(go, width)` | Releases the horizontal `ContentSizeFitter` so a width can be pinned |
| `Stretch(go, inset)` | Anchors the rect to fill its parent, inset on every side |
| `Fit(go)` / `SetPadding(go, p)` / `SetSize(go, w, h)` | `ContentSizeFitter`, layout padding, explicit size |
| `SetTextStyle(text, hash)` | Writes TMP's `m_TextStyleHashCode` |
| `Assign(component, fields)` | Wires `[SerializeField]` object references by field name |
| `EnsureEntry` / `AssignLocalizedString` | Creates a string table entry and points a field at it |
| `Save(root)` | Writes `<TargetFolder>/<root.name>.prefab` and returns the path |

A new layout need is a reason to add a helper, not to inline the same six lines
into three builders.
