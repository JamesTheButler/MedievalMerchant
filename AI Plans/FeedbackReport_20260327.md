# Feedback Report — Medieval Merchant (compiled 2026-03-27)

Source: Google Forms CSV, entries up to 2026-03-27, unhandled only.

---

## TO DO (Critical)

**[BUG] Mission completion stuck / doesn't register**
Reported by: Tyko, Kelly (Level 3), Asaphx — 3 reporters, v0.2.0–0.2.1
Mission shows all goods delivered but never completes. Also: fulfill button resets goods count to 0, mission persists after selling required goods. Causes players to abandon runs.

**[BUG] Town upgrade quest never spawns (Level 2 + Level 3)**
Reported by: Asaphx, Tyko, Anonymous (05/03) — 3 reporters, v0.2.1
After failing or in certain conditions, upgrade quests stop spawning. Town dev bar fills but nothing happens. Players get stuck and must restart. Directly blocks level completion.

**[BUG] Navigator reduces retinue upkeep instead of caravan upkeep**
Reported by: Soulhunt, v0.2.0
Straightforward modifier bug. Contributes to companions-OP balance issue.

---

## High Priority

**[BUG] First mission fulfillment didn't consume goods / gave 1k gold for free**
Reported by: Anonymous, v0.1.5. Serious economic exploit.

**[BALANCE] Companions OP / game too easy**
Reported by: Jaslaw, v0.1.3 (detailed)
Upkeep near-zero with companions, 14k gold by midgame. "Run circles" strategy dominates, only 2 T2 carts needed. T3 production not economically competitive with T1/T2. Compounded by Navigator bug.

**[UX] Tutorial doesn't cover core gameplay / doesn't restart properly**
Reported by: Matt Gambler, Anonymous (16/01), Nithin — multiple versions
Players don't understand how to move between towns, buy/sell, or that companions exist. Tutorial doesn't re-trigger after level restart. Retention risk.

**[BUG] Timer runs during tutorial popups**
Reported by: Nithin, v0.2.0. Quick fix.

**[BUG] Impossible objectives — fish demand with no fish supply (Level 2)**
Reported by: FeudalismAdvocate, v0.2.0
Mission system generates demands for goods structurally unavailable on the map. Needs validation guard at mission generation.

---

## Medium Priority

**[UX] No way to review dismissed events**
Reported by: Nithin, v0.2.0. Event log or persistent indicator needed.

**[UX] Can't change production choice after selecting**
Reported by: Tyko, v0.2.1. Suggested: allow change at gold cost.

**[UX] Secondary resource click behavior is confusing**
Reported by: Nithin, v0.2.0
Clicking an input good on a production building shows "won't buy" — but player wanted to buy it. Non-obvious interaction.

**[UX] Quick trade amount buttons (e.g. 5, 15, 30)**
Reported by: Kelly, OS, v0.2.1. Low effort, meaningful QoL.

**[UX] Show remaining inventory after trade**
Reported by: OS, v0.2.1. Basic info gap during trading.

**[UX] Font too small**
Reported by: OS (v0.2.1), Menzek (v0.1.3, 2K monitor).

---

## Low Priority / Backlog

| Item | Reporter | Notes |
|---|---|---|
| Save game mid-session | ToeLingusLicker, OS, Menzek | Already in planning |
| Endless mode (no time limit) | Rafal | Post-EA |
| Starting profiles / difficulty presets | Kelly | Post-EA |
| Female merchant avatar | Tyko | Nice to have |
| All-goods trade window (Port Royal style) | Setrik | Major UI rework |
| Mouse lock to screen | Nithin | Minor QoL |
| Typo: "And" in level 1 description | Nithin | 2-minute fix |
| Favorite goods stat tracking wrong | Hampus | Minor stat bug |
| Caravan slot limit not enforced on sea town | Kelly | Edge case inventory bug |
