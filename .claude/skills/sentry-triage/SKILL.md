---
name: sentry-triage
description: Investigate a Sentry error issue for the Medieval Merchant Unity project by fetching it straight from the Sentry API (issue metadata + latest event stack trace) and cross-referencing the implicated code against this repo's source and git history, then proposing a root-cause diagnosis and a concrete fix. Use this whenever the user gives a Sentry short ID (a pattern like "MED-123", "MM-45"), pastes a Sentry issue link, or asks to "investigate", "look into", "triage", or "figure out why" a crash/error/exception from Sentry, the error board, or a live build. Don't use this for errors the user pastes manually as raw stack-trace text with no Sentry ID - just debug that stack trace directly instead of invoking this workflow.
---

# Sentry issue triage

Pulls a real issue and its stack trace from Sentry, then does the same
detective work a developer would: find the implicated code, check what
changed around when it started, and work out what's actually going wrong.

This is diagnosis, not autopilot: **always end with a chat report, never
silently edit files.** If the user likes the proposed fix and wants it
applied, that's a separate follow-up ask.

## 0. Prerequisite

The fetch script needs `SENTRY_AUTH_TOKEN` in the environment (optionally
`SENTRY_ORG` if the user belongs to more than one Sentry org, and
`SENTRY_BASE_URL` for self-hosted Sentry). If running it fails with a
"SENTRY_AUTH_TOKEN is not set" error, or any 401/403 from the Sentry API,
stop and tell the user: **the `SENTRY_AUTH_TOKEN` environment variable
doesn't look like it's set (or is invalid/expired) in this session** - they
need to set it themselves, then start a fresh Claude Code session for the
new environment variable to be visible (an already-running session won't
pick it up). **Never ask the user to paste a Sentry token into chat**, and
never write one into a file yourself - this is on them to fix in their own
environment.

## 1. Get the short ID

Pull it from whatever the user gave you - a bare ID like `MED-123`, a pasted
Sentry issue URL (the short ID is usually in the page title or the URL's
`?query=` isn't it, look at the breadcrumb/header instead), or explicit
instruction. If genuinely ambiguous, ask.

## 2. Fetch the issue

```bash
python ".claude/skills/sentry-triage/scripts/fetch_sentry_issue.py" <SHORT-ID>
```

This does three Sentry API calls (resolve short ID → issue detail → latest
event) and prints one JSON object: issue metadata (title, culprit, level,
count, userCount, firstSeen/lastSeen, firstRelease/lastRelease, tags,
permalink) plus `exception_type`, `exception_value`, and `frames` (crash
frame first, each with filename/module/function/lineNo/inApp/context).

This script only ever does read-only GETs against Sentry. Don't extend it
(or improvise curl calls) to mutate issue status, add comments, or resolve
anything - that's out of scope for triage and this skill has no write
scopes anyway.

## 3. Find the implicated code

Work from the frames, **innermost (crash) frame first**, preferring frames
with `"inApp": true` - those are this project's code rather than Unity/engine
internals.

Sentry Unity stack traces vary in quality depending on the build:

- Best case: `filename` and `lineNo` point straight at a `.cs` file under
  `Assets/` - read it directly.
- Degraded case (common for IL2CPP release builds without uploaded debug
  symbols): filename/line may be missing, generic, or refer to generated
  IL2CPP C++ rather than the original C#. In that case fall back to
  `function`/`module`/the exception's `culprit` field - grep the codebase for
  that class/method name instead of trusting the path.

Use Grep/Glob to locate the real source, then Read the surrounding method.
Don't assume the top frame is the actual bug site - for exceptions thrown
deep in shared infrastructure (`Common/Infrastructure`, `Observable<T>`,
`ModifiableVariable`), the real mistake is often one or two `inApp` frames up
the stack, at the call site that handed in a bad value or skipped
initialization.

## 4. Correlate with git history

Use the issue's `firstSeen` timestamp and `firstRelease` version to narrow
down what shipped the bug:

```bash
git log --since="<firstSeen minus a few days>" --until="<firstSeen>" -- <implicated file>
```

If `firstRelease` looks like a git tag or you can otherwise map it to a
commit, `git log <that-commit>..HEAD -- <file>` or `git log -S"<method
name>" -- <file>` are also useful for finding when a suspect line was
introduced. Treat this as a lead, not proof - plenty of bugs are old code
that a new *trigger condition* (data, save file, player action) finally hit.

## 5. Form the diagnosis

Read the actual code, don't just pattern-match on the exception type. That
said, exception type is a strong prior worth checking first given this
project's patterns (see the repo's `CLAUDE.md`):

- **NullReferenceException** - check for: a service/system accessed before
  `Initialize()` ran (see `IInitializable` lifecycle, `GlobalContext` vs
  `GameplayContext` ordering); an `Observable<T>`/`ModifiableVariable`
  read before its owning model finished construction; a `[CanBeNull]`
  (JetBrains annotation) value used without a null check.
- **IndexOutOfRangeException / ArgumentOutOfRangeException** - collection
  mutated during iteration, or an index computed from data that changed
  shape (e.g. inventory/retinue size) without the index being re-clamped.
- **InvalidOperationException / NotSupportedException** - often a
  `SerializedDictionary`/config lookup for a key that doesn't exist in one
  environment's `ResourceManager`/`ConfigurationManager` data.

Cross-check the issue's `tags` (platform, release, environment) and
`userCount`/`count` - a huge `count` with a single build/tag strongly
suggests a systemic bug in a common path; a low, scattered count suggests an
edge-case data condition.

**Don't just assert a mechanism narratively - check it.** A plausible-sounding
story for how a bug triggers is not the same as a verified one, and this
codebase's reactive/observable wiring makes it easy to construct several
different plausible-sounding stories that all fit the same stack trace. Where
the logic is pure/deterministic (score thresholds, dictionary state machines,
anything not touching Unity APIs), trace it by hand or - better - write a
small throwaway script (Python's fine, this doesn't need to be C#) that
mirrors the actual algorithm and feeds it candidate input sequences to see
whether your hypothesized trigger actually reproduces the crash. If it
doesn't, that's a real finding: it means the defect must originate somewhere
your first theory didn't cover (e.g. a piece of *shared* state being touched
by two independent instances, rather than one instance misbehaving on its
own) - don't paper over a failed check by writing the report as if the
first theory held.

State your confidence honestly, **and keep that reasoning out of the final
report.** Do the verification work, but the user wants the conclusion, not
the investigation log - see the report format below.

## 6. Report back in chat

No files get written. The user wants the issue and the fix, not a
play-by-play of how you found it - keep prose minimal and skip straight to
what's wrong and what changes. Reply with:

```
## MED-123 - <title>

### Issue
<2-4 sentences: what's broken and the mechanism, stated as fact if verified,
or clearly flagged as unconfirmed if it isn't - see below>

### Fix
<the concrete code change(s) - diff-shaped snippets with file:line, ready to
apply>

### Test case
<the most direct reproduction you can construct - see below>
```

**On confidence:** separate what you verified from what you're inferring.
It's fine - common, even - to be fully confident in a defect and its fix
(e.g. "this dictionary Add throws on any duplicate key, TryAdd doesn't")
while being unsure of the *exact* gameplay sequence that first produces the
duplicate. Say so plainly in one line rather than presenting a guess as the
mechanism or padding the report with the chain of theories that didn't pan
out.

**On the test case:** prefer the smallest thing that deterministically
proves the defect over a speculative "click these buttons in this order"
gameplay recipe you haven't actually verified - especially since you can't
run the Unity Editor yourself in this session. If the buggy code is
reachable in isolation (a system class, a pure function), sketch a minimal
repro that calls straight into it (an EditMode-test-shaped snippet is fine
even if this project has no test assembly yet - the user can adapt it, or
just run it as a scratch script). If you truly can't isolate it, say so and
give the closest safe manual repro instead, flagged as unverified.
