# uGUI Prefab YAML Anatomy

Verified against `Assets/Features/Player/Camp/UI/CampsiteCartUI.prefab` and sibling prefabs under `Assets/Common/UI/Elements/`.

## Document shape

A `.prefab` file is a stream of YAML documents, one per Unity Object, separated by `--- !u!<classID> &<fileID>`:

```yaml
%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!1 &45338918473926658
GameObject:
  ...
--- !u!224 &4839843604137614650
RectTransform:
  ...
```

`<classID>` is Unity's built-in numeric type ID (`1` = GameObject, `224` = RectTransform, `222` = CanvasRenderer, `223` = Canvas, `114` = MonoBehaviour — the container for *every* scripted component, built-in package or custom). `<fileID>` is a locally-unique 64-bit-ish integer identifying this object *within this file*. Every cross-reference elsewhere in the file (parent/child, component ownership, SerializeField wiring) is just `{fileID: <that number>}`.

## GameObject block

```yaml
--- !u!1 &45338918473926658
GameObject:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  serializedVersion: 6
  m_Component:
  - component: {fileID: 4839843604137614650}   # RectTransform
  - component: {fileID: 82378168786829628}     # CanvasRenderer
  - component: {fileID: 1789255770100535497}   # Image
  - component: {fileID: 4178717393972408310}   # LayoutElement
  m_Layer: 5                                    # 5 = UI layer, always this for canvas UI
  m_Name: Movespeed Icon
  m_TagString: Untagged
  m_Icon: {fileID: 0}
  m_NavMeshLayer: 0
  m_StaticEditorFlags: 0
  m_IsActive: 1
```

`m_Component` lists every component's fileID in the order they appear in the Inspector. The GameObject itself has no transform/position fields — those live on the paired `RectTransform`.

## RectTransform — the hierarchy backbone

Every UI GameObject has exactly one `RectTransform (!u!224)`. This is where parent/child hierarchy lives:

```yaml
--- !u!224 &4839843604137614650
RectTransform:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 45338918473926658}     # owning GameObject
  m_LocalRotation: {x: -0, y: -0, z: -0, w: 1}
  m_LocalPosition: {x: 0, y: 0, z: 0}
  m_LocalScale: {x: 1, y: 1, z: 1}
  m_ConstrainProportionsScale: 0
  m_Children: []                                 # child RectTransform fileIDs, in visual/sibling order
  m_Father: {fileID: 6946516611767217219}        # parent RectTransform fileID (0 = prefab root)
  m_LocalEulerAnglesHint: {x: 0, y: 0, z: 0}
  m_AnchorMin: {x: 0, y: 0}
  m_AnchorMax: {x: 0, y: 0}
  m_AnchoredPosition: {x: 0, y: 0}
  m_SizeDelta: {x: 24, y: 0}
  m_Pivot: {x: 0.5, y: 0.5}
```

**`m_Children` order matters** — it's the sibling order, which is also the visual order inside a `HorizontalLayoutGroup`/`VerticalLayoutGroup`. When you add a child, append (or insert) its fileID into the parent's `m_Children` array at the position you want it to appear.

**Anchor/pivot cheat sheet** (values seen in this project):
- `m_AnchorMin`/`m_AnchorMax` both `{0,0}` to `{0,0}` → anchored to a fixed point (bottom-left here), size is exactly `m_SizeDelta`. This is how the prefab *root* is typically anchored.
- `m_AnchorMin: {0,1}`, `m_AnchorMax: {0,1}` → anchored to top-left corner, common for a label/icon inside a layout group.
- Anchors matching on min/max (not stretched) mean this RectTransform is **not** using anchor-stretch sizing — its size comes from `m_SizeDelta` or from a `LayoutElement`/layout group instead. Elements inside a `HorizontalLayoutGroup`/`VerticalLayoutGroup` with `m_ChildControlWidth/Height: 0` keep their own `m_SizeDelta`/`LayoutElement` sizing; the layout group only positions them.
- `m_Pivot` is the point within the rect that `m_AnchoredPosition` refers to — `{0.5, 0.5}` = center, `{0, 1}` = top-left.

A prefab root with `m_Father: {fileID: 0}` is not parented to anything inside the file — when instanced into a scene/parent prefab, Unity reparents it there.

## CanvasRenderer

Any GameObject with a `Graphic` (Image, TextMeshProUGUI) needs a paired `CanvasRenderer (!u!222)` — it has almost no fields:

```yaml
--- !u!222 &82378168786829628
CanvasRenderer:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 45338918473926658}
  m_CullTransparentMesh: 1
```

Pure layout containers (a GameObject that only holds a `HorizontalLayoutGroup`/`VerticalLayoutGroup`/`ContentSizeFitter` and no visible Image/Text) do **not** get a `CanvasRenderer` — confirmed by the "Header"/"Inventory Cells" container objects in `CampsiteCartUI.prefab`, which have only `RectTransform` + the layout MonoBehaviours.

A prefab is only a top-level `Canvas` if it's meant to be a standalone screen; most sub-panels (like `CampsiteCartUI`) have no `Canvas` component at all because they're instanced under an existing Canvas elsewhere.

## MonoBehaviour block (any scripted component)

Every non-native component — whether a built-in UGUI/TMP class or a custom project script — is `!u!114 MonoBehaviour`:

```yaml
--- !u!114 &1789255770100535497
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 45338918473926658}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {fileID: 11500000, guid: fe87c0e1cc204ed48ad3b37840f39efc, type: 3}
  m_Name: 
  m_EditorClassIdentifier: UnityEngine.UI::UnityEngine.UI.Image
  m_Material: {fileID: 0}
  m_Color: {r: 1, g: 1, b: 1, a: 1}
  ...
```

`m_Script.guid` identifies the class (see `component-guids.md`). `m_EditorClassIdentifier` is a free bonus: when present it spells out the exact class (`Assembly::Namespace.ClassName`), which is the fastest way to confirm you have the right GUID or to identify an unknown one — grep the codebase for `m_EditorClassIdentifier:.*ClassName` to find another instance and copy its field layout.

For a custom project script's `[SerializeField]` fields, they appear as plain YAML keys directly below `m_EditorClassIdentifier`, keyed by field name, e.g.:

```yaml
  m_EditorClassIdentifier: Assembly-CSharp::Features.Player.Camp.UI.DetailedCartUI
  inventoryCells:
  - {fileID: 4606592932010719048}
  - {fileID: 5666744207795900840}
  cartImage: {fileID: 2725854481829059834}
  waggonText: {fileID: 5022697919536362038}
```

A `LocalizedString` field serializes as a nested `m_TableReference`/`m_TableEntryReference` block (see the worked example) — don't hand-write a `GUID:...` table reference from scratch, copy it from an existing field that already points at the correct string table, or leave it empty and tell the user it needs to be assigned in the Editor's Localization picker.

## Minting fileIDs

Local fileIDs just need to be unique **within the file being written**. Generate large pseudo-random integers (the project's existing IDs are 16–19 digits) and never reuse one already present in the file — grep the target file for existing fileIDs before picking new ones if you're editing rather than creating fresh.

## `.meta` files

Every asset (`.prefab`, `.cs`, ...) needs a sibling `.meta` file or Unity won't recognize it. Format is minimal and hand-writable:

**Script** (`SomeScript.cs.meta`) — just two lines:
```yaml
fileFormatVersion: 2
guid: d9a92a15e380f9b44bcda1ae1f69f0e0
```

**Prefab** (`SomePrefab.prefab.meta`):
```yaml
fileFormatVersion: 2
guid: d9b7520b47fed824c975466b6219e892
labels:
- Text
PrefabImporter:
  externalObjects: {}
  userData: 
  assetBundleName: 
  assetBundleVariant: 
```
(the `labels: - Text` block is an optional project convention seen on prefabs, not required by Unity — safe to include or omit.)

The `guid` must be a fresh 32-character lowercase hex string, globally unique across the project (astronomically unlikely to collide if generated randomly — no need to cross-check against every existing `.meta`). When creating a **new script**, mint its `.meta` guid *first*, then reuse that exact value for the `m_Script.guid` field wherever the prefab attaches that script.
