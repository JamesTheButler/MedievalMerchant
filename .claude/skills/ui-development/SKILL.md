---
name: ui-development
description: Build Unity uGUI screens for Medieval Merchant end to end - from a prose description, through a wireframe design and an element/component breakdown, to generated .prefab files and their C# code-behind. Use this whenever the user asks to design, build, mock up, add, or extend a UI screen, panel, popup, dialog, header, row, tile, card, or cell for the game - even when they only describe the layout in prose ("a header row with the town name, then three columns of stats") and never say "prefab" or "Unity". Also use it when asked to wire a UI prefab up to model data, or to add an element into a screen that is already being built this way. Do not use it for UI Toolkit / UXML work - this project's UI is uGUI (Canvas Renderer) only. Do not use it for gameplay logic or ScriptableObject config changes that merely happen to sit in a feature that also has UI.
---

# UI Development

Prefabs are never hand-authored here. You write a **builder method** in
`Assets/Editor/UIGenerator.cs`; the user runs it from Unity's
menu; Unity's own API writes the `.prefab`. That is the whole mechanism, and it
is why prefab YAML never appears in this workflow.

You cannot run the Unity Editor in this session. Everything you produce is a
proposal the user compiles and runs, and a mistake surfaces as a red line in
their console minutes later, not as a failed tool call. So the workflow is built
around getting agreement before writing C#, and getting C# compiling before
writing the builder that references it.

## Reference file

`references/ui-reference.md` - annotated, real code from this repo for every
shape you will need to write: dumb elements, bound components, list population,
localized strings with and without arguments, base classes, and a full builder
method. **Read it before writing any C#.** Don't work from memory of "typical
Unity code" - this project has its own idioms and they are not the common ones.

Design tokens (TMP style hashes, icon sizes, paddings) are **not** duplicated
there. They live as constants at the top of
`Assets/Editor/UIGenerationHelper.cs`; read that file when you need their values.

The generator is **two files**, and the split matters:

- `Assets/Editor/UIGenerator.cs` - class `UIGenerator`. The menu item, the output
  folder, and the builder methods for the screen being generated. **This is the
  file you rewrite.** Its builders are single-use; delete leftovers from previous
  runs outright.
- `Assets/Editor/UIGenerationHelper.cs` - class `UIGenerationHelper`. Design
  tokens, layout, panels, the shared button prefabs, localization, wiring, save.
  **Persistent.** Add to it when a layout need has no helper; never put anything
  screen-specific here.

`UIGenerator` opens with `using static Editor.UIGenerationHelper;`, so the helper
vocabulary reads unqualified inside builder methods.

## 1. Vocabulary

Three tiers. Which tier a thing belongs to is decided by what it is *allowed to
know*, not by how big it is:

- **Element** - atomic, reusable, dumb. Its public API is setters: `SetUp(...)`,
  `SetCount(...)`, `SetCompleted(bool)`. It takes primitives and strings. It has
  **no model dependency, no `GameplayContext`, no `ResourceManager`, and no
  bindings.** Whoever owns it feeds it.
- **Component** - composes elements and binds to a model. Public API is a
  `Bind(TModel ...)` / `Unbind()` pair. May reach `GameplayContext`,
  `ConfigurationManager` and `ResourceManager`.
- **Screen** - the root of a view. Owns lifecycle; usually extends
  `InitializableBehavior` so `LevelBootstrapper` drives `Initialize()` and
  `CleanUp()`.

Every element and component is exactly two files, identically named, in the same
folder: `OutcomeTile.cs` and `OutcomeTile.prefab`. The generator enforces this -
the prefab filename is the root GameObject's name is the class name.

## 2. Phase 1 - design

Turn the user's prose into a **neutral wireframe** artifact: grey boxes, labels,
no game art, no colour. Its job is to agree structure and hierarchy, not to
preview the final look. Iterate on it through their comments until they are
happy.

Alongside the wireframe, produce two tables - **UI Elements** and **UI
Components** - with these columns:

| Column | What goes in it |
|---|---|
| Name | The class/prefab name |
| Public API | The exact method signatures you intend to write |
| Composes | Which elements this is built from (empty for elements) |
| Strings | Every piece of text, marked `static` or `code-set` |
| Description | One line on what it is for |

*Composes* is what makes the hierarchy checkable at review time. *Strings* is
what Phase 2 needs in order to create the localization entries.

**Icons are not part of a public API.** An icon that is the same every time a
given prefab instance is used - the health icon on a stat row, the coin on a
price row - is a `[SerializeField] Sprite` on the element, assigned by hand in
the editor on that instance. Do not add a `Sprite` parameter to a setter, and do
not add a `Sprite` field to the owner just to pass one down; that only moves the
same decision one level up and puts it in code instead of the inspector.

A `Sprite` belongs in a signature only when it genuinely varies with the model at
runtime and one prefab serves every case - `UnitToken.SetUnit(Sprite characterIcon,
CombatUnit unit)`, where the same token is a guard or a bandit depending on the
combatant, or a tier icon looked up from `ResourceManager` by level. If the answer
to "could the user just drag the right sprite onto this instance?" is yes, it is
a serialized field.

**Do not go searching the codebase for existing elements to reuse.** If
something pre-existing should be used, the user will name it. Ask if you are
unsure whether a piece of the design is meant to be new.

When the tables are agreed, write them to `AI Plans/UI/<ScreenName>.md` before
moving on, so the spec survives a context reset.

**Stop here and get explicit approval.** Do not write C# until the tables are
signed off.

## 3. Phase 2 - code-behind first, then models

Write the `.cs` files **before** touching the generator. The builder method
calls `AddComponent<YourClass>()`, so the type has to exist and Unity has to
have compiled it before the menu item can run.

Element code-behind stays trivial - `[SerializeField, Required]` fields and
setter methods, nothing else. Icons are serialized fields on the element, never
setter parameters (see Phase 1). Component code-behind may bind; follow the
`Bindings` shape in the reference file, and always tear down in `OnDestroy` as
well as in `Unbind()`.

If a component needs a **model class that does not exist yet**, generate it as a
placeholder: the public API only - properties and method signatures the UI
actually consumes - with bodies that throw
`new NotImplementedException("Placeholder from the ui-development skill.")`.
No gameplay logic, no fields beyond what the signatures need. The user fills it
in.

**Stop here and get explicit approval**, then tell the user to focus the Unity
Editor so it compiles. Only then move to Phase 3.

## 4. Phase 3 - the builder method

Rewrite `Assets/Editor/UIGenerator.cs`. Point its `OutputFolder` at the feature's
UI folder, and register each builder method in `Generate()`.

**The constants at the top of `UIGenerationHelper.cs` must not be changed** - the
TMP style hashes, the icon sizes, the padding values. The rest of that file is
extensible: add helpers freely, but don't delete ones you aren't using and don't
move screen-specific code into it.

Builder methods in `UIGenerator.cs` left over from previous runs are **deleted
outright** without checking whether anything used them. Each builder is written
to be run once.

Use the existing helper vocabulary (`NewUI`, `NewRow`, `NewColumn`, `NewIcon`,
`NewText`, `NewLocalizedText`, `NewPanel`, `NewButton`, `NewElement`, `Fit`,
`SetSize`, `SetTextStyle`, `Assign`, `Save`) and add to `UIGenerationHelper.cs`
when a layout needs something it doesn't have.

**Add the code-behind with `AddBehaviour<T>(root)`, not `AddComponent<T>().`** It
does the same thing and then walks the component up to sit directly under the
Transform, so opening the prefab shows the script first rather than an Image, a
layout group and a fitter. The uGUI plumbing stays below it.

**Buttons come from the shared prefabs, never from a panel plus a `Button`
component** - the prefabs carry the project's hover, layout and text conventions:

| Need | Helper | Prefab |
|---|---|---|
| Label, optionally with an icon | `NewButton(name, parent, table, key, english, comment, icon, iconAfterText)` | `Assets/Common/UI/Elements/Button.prefab` |
| Icon only | `NewIconButton(name, parent, icon)` | `Assets/Common/UI/Elements/ButtonWithIcon.prefab` |
| Close (X), top-right of a panel | `NewCloseButton(name, parent)` | `Assets/Common/UI/Elements/XButton.prefab` |

`NewButton` creates and wires the label's entry itself, so don't add a
`NewLocalizedText` child to a button. `iconAfterText: true` flips the row's
Reverse Arrangement so the icon trails the label.

Two things that bite:

- **`Assign()` binds by field-name string.** A name that doesn't match the C#
  field produces a `Debug.LogError` and a silently unassigned field - not a
  compile error. Copy the field names out of the `.cs` you just wrote; don't
  retype them from memory.
- **One panel per visual block.** A `NewPanel(...)` sprite already carries its
  own border, drawn **tiled** from a 9-slice so it behaves like a plain rectangle
  at any size, matching Panel UI Template. That prefab has a single Image, and so
  do this project's elements - none of them wrap their contents in an inner panel.
  Nest a second `NewPanel` only where the design actually shows a frame inside a
  frame, and then it is usually a real element prefab (a tile, a cell) rather than
  a wrapper. A reflexive `Inner` panel under every block is a GameObject, an Image
  and a draw call producing a border nobody asked for, which the user then has to
  delete.

## 5. Localization

**Every string is localized.** The only exceptions are strings whose non-numeric
content is purely mathematical or symbolic - `$"{a}/{b}"`, `$"{v:0.##}"`,
`$" {sign} {abs * 100:0.##}% ="`. If a word appears, it is localized.

Three places a string can live. Pick by where it is set:

1. **Static text, known at design time** goes in a `LocalizedText` prefab
   instance (`Assets/Features/Localization/UI/LocalizedText.prefab`), created
   via `NewLocalizedText(...)` - never a bare `TMP_Text`. The generator assigns
   its table and key, so the editor shows real English.
2. **Text set from code, specific to this one element or component** goes in a
   `[SerializeField] private LocalizedString` on the code-behind, resolved with
   `.GetLocalizedString(args)`. **This is the default for code-set text.**
3. **A `<Feature>LocalizationResources` class** - only when the string is
   consumed by the *logic* layer, which has no easy path to a `[SerializeField]`,
   or when it is reused across so many elements that assigning it per instance
   would be a drag. See `PlayerLocalizationResources` in the reference file.

Placeholder text on a plain `TMP_Text` should be a realistic example of what
will appear there ("1,240", "Tier III"), so the prefab reads correctly in the
editor. This does **not** apply to `LocalizedText` - it overwrites its own text
on `OnEnable` and `OnValidate`, and shows `<none>` until a key is assigned,
which is why the generator assigns keys.

### Creating entries

The generator's `EnsureEntry(...)` helper writes the key, the English text and a
translator comment through Unity's localization API.

- **Ask the user which table collection to use.** Do not guess.
- **Never create a table collection.** If the right one doesn't exist, stop and
  tell the user to create it in the Localization Tables window.
- **Key naming**: `<Table>.<Area>.<Element>.<Slot>`, mirroring the feature folder
  structure - `Towns.Header.Funds.Title`, `Common.Map.Overlays.Zones.Description`.
- **Always write a comment.** It is what a translator sees as the *Description*
  column. Say where the string appears and what its parameters mean; don't
  restate the English.
- **Parametrized strings are Smart Strings with named placeholders**:
  `{TownName}`, `{_int_Amount:0.##}`. Flag the entry smart, and have the consumer
  pass a matching anonymous object:

  ```csharp
  var args = new { _int_Level = level, _int_Index = cartIndex };
  levelText.text = levelString.GetLocalizedString(args);
  ```

  The `_int_` and `_float_` prefixes are an existing project convention -
  preserve them exactly; `/mm-localize` treats them as literal. **Never use
  positional arguments** (`GetLocalizedString(index + 1)`) - a translator cannot
  tell what `{0}` means.

## 6. Styling

Text style is a TMP style-sheet hash, set with `SetTextStyle`:

- Titles and default body text use `StyleTitle`
- Anything italic or muted in the design uses `StyleSubtitle`
- A number that is good or bad, and known to be so at design time, uses
  `StyleGood` or `StyleBad`

When good/bad depends on a runtime value, leave the field on `StyleTitle` and
style it in code with the extensions in
`Assets/Common/UI/Utility/StyleExtensions.cs`:

```csharp
var style = unitsLost.GetNumberStyle(isPositiveGood: false);
lostCountText.text = unitsLost.ToString().WithStyle(style);
```

Numeric formats in use across the project: `{v:0.##}`, `{v:0.#}`,
`{v:+0.0;-0.0;0.0}`, `.ToString("N0")`. Match a neighbour rather than inventing
one.

## 7. Layout

Rows and columns are **layout-only container GameObjects** - a RectTransform
plus a `HorizontalLayoutGroup` or `VerticalLayoutGroup`, no `Image`, no visuals
of their own. Their children do the rendering. Icons inside a layout group are
sized by `LayoutElement`, not by `sizeDelta`.

Before laying out N fixed-size items in a container, do the arithmetic: sum of
item widths + spacing + padding, against the container's actual width. A layout
group with `childControlWidth`/`childControlHeight` off **will not shrink
children to fit** - it just overflows. If it doesn't fit, restructure (a grid
instead of one row) rather than quietly widening the container.

## 8. Running and verifying

Tell the user to:

1. Focus the Unity Editor and let it compile.
2. Run `Tools/AI/Generate UI Prefabs`.
3. **Check the console.** Success is zero `Debug.LogError` lines - every
   field-name mismatch, missing sub-sprite and missing table reports there.
4. Open the prefab in Prefab Mode: static labels show real English (not
   `<none>`), and panel borders are crisp rather than stretched.
5. Drop it into the `UIDevelopment` scene to check bindings fire and tear down.

If a field came out unassigned, the cause is almost always an `Assign()` key
that doesn't match the C# field name. Fix the dictionary, don't rename the
field.
