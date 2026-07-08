# Component GUIDs

**GUIDs only apply to scripted components.** Verified directly against `CampsiteCartUI.prefab`: native engine types — `GameObject (!u!1)`, `Transform`, `RectTransform (!u!224)`, `Canvas (!u!223)`, `CanvasRenderer (!u!222)` — have **no `m_Script`/guid field at all**, they're identified purely by the `!u!<classID>` number in the document header. Everything else (built-in UGUI/TMP components *and* custom project scripts) is `MonoBehaviour (!u!114)` with an `m_Script: {fileID: 11500000, guid: <guid>, type: 3}` field.

There are two very different kinds of script GUID — treat them differently:

## Built-in package components — stable, hardcode these

Confirmed by the same GUID recurring identically across many different objects in this project (multiple prefabs, multiple instances per file):

| Component | GUID |
|---|---|
| `UnityEngine.UI.Image` | `fe87c0e1cc204ed48ad3b37840f39efc` |
| `TMPro.TextMeshProUGUI` | `f4688fdb7df04437aeb418b961361dc5` |
| `UnityEngine.UI.HorizontalLayoutGroup` | `30649d3a9faa99c48a7b1166b86bf2a0` |
| `UnityEngine.UI.VerticalLayoutGroup` | `59f8146938fff824cb5fd77236b75775` |
| `UnityEngine.UI.ContentSizeFitter` | `3245ec927659c4140ac4f8d17403cc18` |
| `UnityEngine.UI.LayoutElement` | `306cc8c2b49d7114eaa3623786fc2126` |
| `UnityEngine.UI.Button` | `4e29b1a8efbd4b44bb3f3716e73f07ff` |
| `UnityEngine.UI.Slider` | `67db9e8f0e2ae9c40bc1e2b64352a6b4` |

Not yet confirmed in this repo (no prefab sampled uses them): `ScrollRect`, `Mask`/`RectMask2D`, `GridLayoutGroup`, `Toggle`, `InputField`/`TMP_InputField`. **Don't guess these** — grep for an existing prefab that uses the component (Unity ships these in the same package as Image/Button, so if one turns up anywhere in `Assets/`, its guid is safe to reuse project-wide), or fall back to building the same visual effect from primitives you do have confirmed GUIDs for.

## Custom project scripts — per-file, always look these up

Each custom script's GUID lives in its own `.cs.meta` and is specific to *this project's copy* of that file — never hardcode one from memory, and never reuse a guid you saw for a different script. Confirmed examples (for reference/sanity-checking, not for reuse elsewhere):

| Script | GUID |
|---|---|
| `Features.Player.Camp.UI.DetailedCartUI` | `d9a92a15e380f9b44bcda1ae1f69f0e0` |
| `Common.UI.Elements.Cells.GoodCell` | `5dfdd97214b14cedb102210926049a2f` |
| `Common.UI.Elements.Cells.InventoryCellBase` | `e8137adcdb874957a6f5721e463780bb` |
| `Common.UI.Elements.InitializableBehavior` | `7326200c963c43a49304beb6c5a9c6f1` |
| `Common.UI.Elements.Panels.DynamicPanel` | `2a9237cf432141729e257acf73b96841` |
| `Common.UI.Elements.Hoverable` | `50ce80d58f075e545a25259e541ad5ea` |
| `Features.Localization.UI.LocalizedText` | `0aae9ebf1e7204649b1b8854b852182f` |

To find any script's real current GUID: read its `.cs.meta` file directly (fastest and authoritative), or grep for `m_EditorClassIdentifier: Assembly-CSharp::<Namespace.ClassName>` in an existing prefab that already uses it. For a **brand-new** script you're authoring, mint the `.cs.meta` GUID yourself (see `yaml-anatomy.md`) and use that same value when wiring `m_Script` in the prefab.

## Reusing `Button.prefab` instead of hand-rolling a button

`Assets/Common/UI/Elements/Button.prefab` is not a single Image+Button — the clickable GameObject carries **four** components: `Image` (background), `Button`, `Hoverable` (`Common.UI.Elements`), and `LocalizedText` (`Features.Localization.UI`, wired to a child `TextMeshProUGUI` via its `textfield` field and a `LocalizedString` table reference). `Button`'s own fields, confirmed from the prefab:

```yaml
m_Navigation:
  m_Mode: 3
  ...
m_Transition: 1
m_Colors:
  ...
m_Interactable: 1
m_TargetGraphic: {fileID: <the Image on the same GameObject>}
m_OnClick:
  m_PersistentCalls:
    m_Calls: []
```

True nested-prefab-instance YAML (a `PrefabInstance` document with `m_Modification` overrides pointing at `Button.prefab` via `m_SourcePrefab`) is possible in Force-Text mode but nontrivial to hand-author correctly. For now, when a description calls for a button, **compose the same component set `Button.prefab` uses** (Image + Button + Hoverable, plus TextMeshProUGUI child, plus `LocalizedText` if the label needs localization) rather than attempting a raw nested `PrefabInstance` — copy the real field values from `Button.prefab` itself as your source of truth.

## Instancing a custom-authored prefab N times (e.g. repeating a card/cell component)

When you need several copies of a *custom* prefab you (or the project) already built as its own standalone `.prefab` file — not `Button.prefab`, something project-specific like a card component — a true nested `PrefabInstance` is the right tool and is much less risky than it sounds, especially if the file you're editing already contains other nested instances to copy the shape from (very common in this project's dialog templates). The minimal recipe per copy:

```yaml
--- !u!1001 &<FRESH_FILEID>
PrefabInstance:
  m_ObjectHideFlags: 0
  serializedVersion: 2
  m_Modification:
    serializedVersion: 3
    m_TransformParent: {fileID: <PARENT_RECTTRANSFORM_FILEID>}   # where it's placed in the outer prefab
    m_Modifications:
    - target: {fileID: <SOURCE_ROOT_GAMEOBJECT_FILEID>, guid: <SOURCE_PREFAB_GUID>, type: 3}
      propertyPath: m_Name
      value: My Instance Name        # only override what actually differs per copy
      objectReference: {fileID: 0}
    m_RemovedComponents: []
    m_RemovedGameObjects: []
    m_AddedGameObjects: []
    m_AddedComponents: []
  m_SourcePrefab: {fileID: 100100000, guid: <SOURCE_PREFAB_GUID>, type: 3}
```

If anything elsewhere in the outer file (a layout group's `m_Children`, or your panel script's `[SerializeField]` list) needs to reference a specific object *inside* that instance, declare a **stripped** object for it — same fileID space as everything else in this file, but only identifying fields, no data:

```yaml
--- !u!224 &<FRESH_FILEID> stripped
RectTransform:
  m_CorrespondingSourceObject: {fileID: <SOURCE_OBJECT_FILEID_IN_THE_SOURCE_PREFAB>, guid: <SOURCE_PREFAB_GUID>, type: 3}
  m_PrefabInstance: {fileID: <THE_PREFABINSTANCE_FILEID_ABOVE>}
  m_PrefabAsset: {fileID: 0}
```

Find `<SOURCE_..._FILEID>` values by reading the source `.prefab` directly (e.g. its root GameObject's own fileID for the root RectTransform, or a specific MonoBehaviour's fileID to reference that component). You need one stripped block per object you must reference from outside, not one per object in the whole source tree — most of the source prefab's internals don't need a stripped counterpart at all. Repeat the `PrefabInstance` + stripped blocks per copy (fresh fileIDs each time, same `m_SourcePrefab` guid), and append the resulting stripped RectTransform fileIDs into whatever parent's `m_Children` array positions them.
