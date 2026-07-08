---
name: unity-ui-builder
description: Build or extend Unity canvas-based (uGUI) UI for Medieval Merchant by directly authoring/editing prefab YAML plus the connecting C# MonoBehaviour, instead of hand-laying-out in the Unity Editor. Use this whenever the user asks to create, build, design, mock up, or add a UI panel, screen, popup, button, cell, row, header, or any other visual UI element for the game — even if they only describe the layout in prose ("two rows, the first split into two containers...") without using Unity terminology. Also use it when asked to wire a UI prefab up to game/model data, add a new element into an *existing* UI prefab, or when a UI prefab/script needs to be built quickly instead of through the Editor. Do not use for UI Toolkit / UXML work — this project's UI is uGUI (Canvas Renderer) only.
---

# Unity Canvas UI Builder

This project serializes prefabs and scenes as **Force-Text YAML** (`ProjectSettings/EditorSettings.asset` → `m_SerializationMode: 2`). That means a `.prefab` file is just readable text — you can write or edit one directly instead of clicking through the Unity Editor's Inspector and Scene view. This skill captures how this project's UI prefabs are actually structured so you can do that reliably.

You cannot run the Unity Editor yourself in this session. Treat generated YAML/C# as a proposal the user will reimport and verify — see "Verifying your work" below.

## Reference files

Load these as needed — don't try to hold it all in your head:

| File | Read this when |
|---|---|
| `references/yaml-anatomy.md` | You need to understand or write raw prefab YAML: GameObject/Component blocks, RectTransform anchor/pivot semantics, hierarchy, fileID minting. |
| `references/component-guids.md` | You need the GUID for a component (Image, TextMeshProUGUI, layout groups, Button, Slider, or a custom project script) or need to look one up. |
| `references/csharp-binding.md` | You're writing the MonoBehaviour that binds the prefab to game data — base class choice, `Observable`/`Bindings` pattern, `[SerializeField]` wiring. |
| `references/editing-existing-prefabs.md` | The task is inserting/modifying elements in a prefab that already exists, not building one from scratch. Higher risk — read this first. |
| `references/worked-example.md` | You want a concrete, annotated example tying YAML and C# together (`CampsiteCartUI.prefab` / `DetailedCartUI.cs` / `GoodCell.cs`). |
| `templates/` | Small verified YAML skeletons (panel, layout container, text label, icon) to start from instead of writing a block from memory. |

## Workflow

### 1. Parse the description into a hierarchy sketch

Users will mostly describe UIs in prose, e.g.:

> "Two rows. The first row is split in half, containing two containers, each showing the inventory of the player or the town. The second row has 3 buttons, equally spaced and centered. I want each inventory to have a title with the name of the player/town."

That's a perfectly good input — don't ask the user to learn a syntax. Turn it into an indented outline of rows/containers/elements, noting for each: rough sizing/spacing, what data it shows (if any), and what it does on interaction (if any). Prose that already states things top-down (rows → contents), with explicit sizing/spacing words ("split evenly", "fixed 200px", "centered", "8px apart") and explicit data bindings ("shows X"), needs the least clarification — but don't require the user to write that way, just prefer to interpret it that way when parsing.

### 2. Clarify — but only genuine gaps

Check the sketch against this list. Ask about an item only if it's actually ambiguous or missing; don't interrogate over things a reasonable default answers:

- **Exact sizing/spacing** where "equal split" / "centered" / "spaced out" doesn't pin down a number and there's no similar existing element to copy proportions from.
- **Data source**: which model/`Observable` field does a described element actually read? (e.g. does "inventory of the player" mean an `Observable` that already exists on `PlayerModel`, or does it need to be added? Grep the codebase before asking — the answer may already be obvious.)
- **Interaction behavior**: what does each button/click concretely call in code?
- **Reuse vs. build**: does a described element map to something that already exists (`InventoryCell`, `GoodCell`, `Button.prefab`, `DynamicPanel`) rather than needing new YAML? Prefer reuse — see the checklist below.
- **Standalone vs. insertion**: is this a new prefab, or does it slot into an existing one? (See `references/editing-existing-prefabs.md` if the latter.)
- **Styling gaps** (font, color, icon): default to whatever the nearest existing similar UI element already uses, and say so explicitly rather than silently inventing a style.
- **Fits the container?** Before laying out several fixed-size elements (e.g. reusing an existing card prefab N times) inside an existing panel, do the arithmetic: sum of element widths/heights + spacing + the container's padding, against that container's actual available size. A `HorizontalLayoutGroup`/`VerticalLayoutGroup` with `ChildControlWidth/Height: 0` will not shrink children to fit — it'll just overflow. If it doesn't fit, restructure (e.g. a grid instead of a single row) rather than resizing shared dialog chrome, unless the user asked for a bigger panel.

### 3. Confirm before writing anything

Restate the resolved hierarchy + bindings + behaviors as a short outline and get a nod before generating YAML or code — regenerating a large prefab file after a misunderstanding is expensive, a quick correction up front is cheap.

### 4. Reuse-first checklist

Before hand-authoring raw component YAML, check whether `Assets/Common/UI/Elements/` already has what's needed:

- `Cells/GoodCell.prefab`, `Cells/InventoryCell.prefab` — single-item display cells
- `Button.prefab` — buttons (has `LocalizedText`, `Hoverable`, styling already wired — don't rebuild this from scratch; compose the same component set it uses, see `references/component-guids.md`)
- `Panels/Panel UI Template.prefab`, base class `DynamicPanel` — open/closable panels
- `Slider.prefab` — sliders

Reusing these means matching their existing component structure, not necessarily true nested-prefab-instance YAML (which is more complex — see the note in `references/component-guids.md`).

### 5. Write the C# MonoBehaviour first

Decide `SerializeField` slots and the `Bind()`/`Unbind()` shape before writing YAML, so the prefab's fileIDs have somewhere to point. See `references/csharp-binding.md` for the base-class decision tree and the `Observable`/`Bindings` pattern this project uses everywhere.

### 6. Generate the prefab YAML

Wire `[SerializeField]` fields to real fileIDs of the GameObjects/components you create. See `references/yaml-anatomy.md` for structure and `references/component-guids.md` for GUIDs. When a component type isn't covered by a template, find the most similar existing instance of it in the codebase (e.g. `grep -r "guid: <known-guid>"` across `Assets/`) and copy its exact field set — this is more reliable than reconstructing a component's full field list from memory, since these blocks have many fields (Unity/TMP internals) that are easy to omit incorrectly.

### 7. Write/update the `.meta` file for anything new

A new `.prefab` or `.cs` file needs a matching `.meta` with a freshly-minted, collision-free 32-hex-char GUID. See `references/yaml-anatomy.md` for the exact format and generation rule. If you're creating a **new custom script**, mint its `.meta` GUID *first*, then use that same GUID when wiring `m_Script` in the prefab.

### 8. Verifying your work

You cannot open Unity from this session. Tell the user to focus/reopen the Unity Editor so it reimports the new/changed files, then:
- Check the Console for import or serialization errors first.
- Open the prefab in Prefab Mode or enter Play mode to confirm it looks and behaves as described.
- `git diff` is a good sanity check after editing an *existing* prefab — it should only touch the blocks you intended to touch.

If something breaks on import, the most common causes are: a duplicate fileID within the file, a `m_Script` guid that doesn't match any `.meta` in the project, or a missing required field on a built-in component (fixed by copying from a real nearby example instead of writing from memory).
