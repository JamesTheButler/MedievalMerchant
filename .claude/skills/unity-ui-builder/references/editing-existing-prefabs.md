# Editing an Existing Prefab

This is the higher-risk path — a mistake here corrupts a real, already-working prefab, unlike creating a fresh file. Follow this sequence:

0. **If the script you're editing looks unfinished or orphaned (nothing instantiates/binds it), audit it for the two bug classes that produce that state before adding new behavior on top:**
   - **Field-name/serialized-key mismatch.** Unity serializes a `[SerializeField]` by the C# field's *name*, not by declaration order or type. If the field was renamed after the prefab was authored, the prefab's YAML still has the *old* key (e.g. `inventoryCells:` in the file) while the C# now declares `slots` — Unity silently deserializes this as an empty/default value, with no import error. Confirm every field name in the script actually appears as a YAML key in the prefab's `MonoBehaviour` block for that component; if one is missing where you'd expect it (and a similarly-named key exists instead), that's almost certainly a stale rename, not a field that's supposed to be empty. Fixing the C# name back to match existing prefab data is usually far lower-risk than hand-editing the serialized array.
   - **Uninitialized model reference.** A `Bind(Model model, ...)` method that reads `_model.Something` without a line assigning `_model = model;` somewhere in it will null-ref the instant it runs. Skim every `Bind`/`Init` method for this before trusting it works.
   - Also expect **stale cosmetic strings** like `m_EditorClassIdentifier` or a `UnityEvent` call's `m_TargetAssemblyTypeName` to still name an old class after a rename (e.g. `CartUI` where the script is now `CartStatsUI`) — these are editor-convenience caches, not functional wiring (the real link is the `m_Script` guid / `m_Target` fileID), so leave them alone rather than "fixing" them.

1. **Re-read the full current file immediately before editing.** Never rely on a version read earlier in the conversation or on the worked example's numbers — fileIDs are unique per file and unrelated to any other prefab, and the file may have changed since you last looked at it.

2. **Locate the target parent's `RectTransform` block** and its `m_Children` array. This is where you'll insert the new subtree's root fileID, at whatever index gives the sibling order you want (see `yaml-anatomy.md` — `m_Children` order = visual order inside layout groups).

3. **Mint fresh fileIDs** for every new GameObject/component you add, checked against the *entire current file* (not just nearby blocks) to avoid collisions.

4. **Append the new document blocks** (GameObject + its components) to the file — position in the file doesn't matter to Unity, only the `m_Component`/`m_Children`/`m_Father` cross-references do. Appending at the end keeps the diff clean and easy to review.

5. **Update exactly two things on existing objects**: the parent's `m_Children` array (to include the new subtree's root) and, if you're inserting a new component onto an *existing* GameObject rather than a new one, that GameObject's `m_Component` list. Leave every other existing block byte-for-byte untouched.

6. **Diff before trusting it.** After writing, `git diff` the file — the change should read as "N new document blocks + a one-line addition to `m_Children`/`m_Component`", nothing else. If unrelated lines shifted or changed, something went wrong (e.g. a fileID collision, or you accidentally rewrote a block you meant to leave alone) — stop and re-examine rather than pushing forward.

7. **Tell the user to reimport and check the Console** before relying on the result — see the "Verifying your work" section in `SKILL.md`.

If the requested change is large relative to the existing prefab (e.g. restructuring several rows, not just adding one element), consider whether it's actually simpler and safer to treat it as a series of small, individually-diffable edits rather than one big rewrite — easier for the user to review, and easier to recover from if one step goes wrong.
