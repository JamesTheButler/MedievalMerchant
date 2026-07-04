# Templates

Verified real field sets, trimmed to placeholders. Use these as a starting skeleton, not a rote fill-in-the-blanks form — always sanity-check against `references/yaml-anatomy.md` and, if the result looks off, against a live example in the codebase (these snippets can go stale if the TMP/UGUI package version changes; a real prefab in the project never will).

Placeholder convention: `{ALL_CAPS_NAME}` — replace every occurrence, including in cross-references (a component's `{FILEID_RECTTRANSFORM}` must exactly match the fileID you gave that RectTransform's own document header). Comments (`#`) explain what each placeholder means; strip the comments in the final output.

Files:
- `image-element.yaml` — a GameObject with just an `Image` (icon, background, sprite).
- `text-label.yaml` — a GameObject with `TextMeshProUGUI`, sized to its content via `ContentSizeFitter`.
- `layout-container.yaml` — a pure row/column container (`HorizontalLayoutGroup` or `VerticalLayoutGroup`), no visuals of its own.
- `fixed-size-icon.yaml` — an `Image` sized by `LayoutElement` instead of `RectTransform.sizeDelta` (for icons living inside a layout group that shouldn't stretch).
- `script.cs.meta` / `prefab.meta` — minimal `.meta` skeletons for new assets.
