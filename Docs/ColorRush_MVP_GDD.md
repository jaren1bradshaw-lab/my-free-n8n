# Color Rush: Energy Run — Technical Game Design Document (MVP)

## 1) Product Pillars
- **One-thumb mastery:** left/right swipes only; no jump/crouch.
- **Readable speed:** player always understands "what color am I" and "what color is next".
- **Fair pressure:** failure from decision timing, not hidden randomness.
- **Fast loop:** fail → retry in under ~2 seconds.
- **Expandable core:** vertical slice first, then content systems.

## 2) Core Fantasy & Experience Goals
The player sprints through neon lanes, aligns with lane-color gates, chains perfect matches, and survives pressure from an **Energy Beast**. They feel “locked in” by rhythm, anticipation, and clean decision clarity.

## 3) Core Gameplay Loop
1. Player auto-runs forward in 3 lanes.
2. Upcoming gate indicates valid lane-color mapping.
3. Player swipes left/right to align lane to active color requirement.
4. Correct gate = streak+score+confidence.
5. Wrong gate = stumble + strike + beast closer.
6. Active color changes on scheduled cadence with explicit warning window.
7. Power-ups create comeback or run-extension moments.
8. Third strike ends run; immediate replay available.

## 4) Input & Movement System (One Thumb)
- **Input:** swipe left/right interpreted by min distance and max swipe time.
- **Movement:** lane-index target (-1/0/+1), lateral interpolation, forward auto speed from difficulty.
- **Forgiveness:** one buffered swipe accepted while lane transition is in-progress.
- **Readability:** subtle body lean and trail direction cue during lane changes.

## 5) Color Match System
- Player holds an **ActiveColorId**.
- Gates define a required lane for each color and optional blocked lane.
- Gate resolve occurs when runner crosses gate Z threshold.
- If runner lane matches required lane for active color: success event.
- Else: mistake event.
- Progression: first ~20 sec uses 2 colors; then 3 colors unlocked.

## 6) Color Shift Warning System (Critical)
Before color switches:
- Warning starts **1.0–1.5 sec** before shift (phase dependent).
- HUD top-center: current color + smaller next color.
- Countdown ring shrinks to zero.
- Warning text pulse: “SWITCH INCOMING”.
- Audio cue beeps each warning tick.
- Trail flicker on player material.
- At shift complete: color swap SFX + haptic micro pulse.

Fairness rule:
- Warning duration can shorten late game, but never below configured minimum.

## 7) Scoring, Combo, and Multiplier
- **Distance score:** gained every second from traveled distance.
- **Gate score:** fixed base + combo scaling.
- **Multiplier:** rises by streak thresholds; decays on mistakes.
- **Optional bonuses:** near-miss and power-up bonus multipliers.
- **Anti-frustration:** multiplier floor >= 1x.

## 8) Failure / Pressure / Strikes
3-strike model:
- Strike 1: stumble feedback, combo break, beast closes in.
- Strike 2: heavier stumble + temporary speed dampen + beast surge.
- Strike 3: capture/game over.

The strike ladder is intentionally dramatic but deterministic.

## 9) Chaser (Energy Beast) System
- Invisible logic distance + visible world-space beast actor.
- Beast offset from runner reduced on mistakes, increased by perfect streak milestones.
- During Energy Rush power-up, beast forced backward.
- If offset <= capture threshold, trigger fail sequence.

## 10) Power-Ups (Legally Safe)
### A) **Surge Fuel** (energy-can fantasy, original IP-safe)
- Duration: 6 sec (configurable 5–8).
- Effects:
  - +runner speed modifier
  - +score multiplier bonus
  - 1 mistake guard
  - stronger VFX/audio layer
  - beast pushed backward
  - optional one-time near-miss correction flag

### B) **Shield Pulse**
- One-hit protection against a gate mistake.
- Clear ring VFX around player.

## 11) Track/Gate Generation
- Object pooled gates and pickups.
- Spawn ahead of runner using `nextSpawnZ` and dynamic spacing.
- Difficulty controls:
  - spacing
  - blocked-lane chance
  - color count
  - power-up spawn rarity
- Gate patterns curated by templates (safe transitions early).

## 12) Difficulty Phases (Invisible)
- **Warm-up (0–20s):** slower speed, larger spacing, 2 colors, long warning.
- **Lock-in (20–60s):** baseline speed, 3 colors begin, moderate spacing.
- **Pressure (60–120s):** faster rhythm, more blocked lanes, shorter warning.
- **Chaos Mastery (120s+):** high speed consistency test, fair but intense timings.

## 13) Accessibility
Each color has symbol pairing:
- Green → Triangle
- Purple → Circle
- Orange → Square

Display both color and icon in:
- HUD current/next indicators
- Gate lane markers
- Optional settings: high contrast and symbol scale.

## 14) UI/UX Screen Flow
1. Boot/Splash
2. Main Menu
3. Tap-to-Play
4. In-Game HUD
5. Pause
6. Game Over
7. Missions
8. Cosmetics
9. Settings

### In-game HUD (minimal)
- Top center: current + next color + countdown ring + warning text.
- Top left: score.
- Top right: streak + multiplier.

### Game Over
- Final score
- Best score
- Longest streak
- Retry (primary CTA)
- Rewarded revive placeholder

## 15) Audio/Haptics Hooks
Events exposed via `AudioEvents` and `HapticEvents`:
- lane switch
- match success
- streak-up
- warning beep
- stumble hit
- shield on
- surge fuel pickup
- overdrive layer on/off
- beast tension level
- game over impact

## 16) Technical Architecture Summary
- **Core orchestration:** `GameManager`
- **Feature modules:** Runner, Color, Gates, Score, Difficulty, Chaser, PowerUps, UI
- **Data-driven balancing:** ScriptableObjects
- **Loose coupling:** C# events / interfaces
- **Performance:** pooling + minimal allocations in gameplay loop

## 17) MVP vs Future
### MVP (this implementation)
- Single endless mode
- 3 lanes + color match gates
- warning UI + color shifts
- 2 power-ups
- strikes + beast pressure
- score + combo + multiplier

### Future upgrades
- Daily seeded run
- Meta progression + cosmetics economy
- Mission chains and live ops
- Boss chase variants

## 18) Step-by-step Build Order
1. Foundation data models + enums + ScriptableObjects.
2. Game state manager and event contracts.
3. Swipe input and runner lane movement.
4. Color state scheduling + warning UI.
5. Gate spawn/pooling and resolution.
6. Score/combo/multiplier.
7. Strike + beast pressure behavior.
8. Power-up pickups and timed effects.
9. HUD wiring and game-over flow.
10. Tune phase curves and spawn tables.
