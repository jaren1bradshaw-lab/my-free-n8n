# Color Rush: Energy Run — Unity MVP Architecture Plan

## 1. Recommended Folder Structure
```
Assets/
  Art/
  Audio/
  Materials/
  Prefabs/
    Gameplay/
    UI/
    PowerUps/
  Scenes/
    Boot.unity
    MainMenu.unity
    Game.unity
  ScriptableObjects/
    Colors/
    Difficulty/
    Gates/
    PowerUps/
  Scripts/
    Core/
    Data/
    Input/
    Runner/
    Color/
    Gates/
    Scoring/
    Chaser/
    PowerUps/
    UI/
    Difficulty/
    Audio/
    Infrastructure/
```

## 2. Scene List
- Boot: splash + async preload.
- MainMenu: entry, missions/cosmetics/settings entry points.
- Game: tap-to-play, runner, HUD, pause, game-over overlays.

## 3. Script List and Responsibilities
- `GameManager`: game state, run start/end, strikes, retry.
- `RunnerController`: lane movement, forward motion, lean, buffered swipes.
- `SwipeInputController`: one-thumb swipe detection.
- `ColorStateController`: active/next color logic + warning schedule.
- `ColorShiftWarningUI`: warning ring + text pulse + color previews.
- `GateController`: gate resolution success/fail by color and lane.
- `GateSpawner`: procedural pooled gate spawning by phase.
- `ScoreManager`: distance score + streak + multiplier.
- `ChaserController`: Energy Beast pressure distance.
- `PowerUpController`: timed buffs + mistake protection charges.
- `EnergyRushPowerUp`: Surge Fuel pickup trigger.
- `ShieldPowerUp`: Shield Pulse pickup trigger.
- `HUDController`: top HUD updates.
- `GameOverUI`: final stats and replay flow.
- `DifficultyDirector`: runtime phase resolution and speed scaling.

## 4. ScriptableObject Usage
- `ColorConfigSO`: color + symbol definitions.
- `DifficultyConfigSO`: phase tuning table.
- `GateConfigSO`: curated gate templates.
- `PowerUpConfigSO`: timings and scalar values.

## 5. Data Models
- `ColorDefinition`
- `DifficultyPhaseConfig`
- `GatePattern`
- Enums: `GameState`, `LaneId`, `ColorId`, `SymbolId`, `DifficultyPhaseId`, `PowerUpType`

## 6. Game State Machine Flow
`Boot -> MainMenu -> TapToPlay -> Playing -> Paused -> Playing -> GameOver -> (Retry->Playing | Menu->MainMenu)`

Gameplay event flow:
- Gate resolve success => score/streak/chaser retreat.
- Gate resolve fail => consume shield if present else strike and pressure.
- Strike 3 => game over.

## 7. UI Prefab Structure (MVP)
- `HUDRoot`
  - `ScoreCluster`
  - `StreakCluster`
  - `ColorShiftCluster`
    - `CurrentColor`
    - `NextColor`
    - `CountdownRing`
    - `WarningText`
- `PausePanel`
- `GameOverPanel`
- `TapToPlayPanel`

## 8. Object Pooling Plan
- Prewarm 16+ gate instances.
- Recycle gates after runner passes beyond despawn distance.
- Extend same pooling pattern for pickups and optional blocked-lane props.

## 9. Expandability Roadmap
- Cosmetics: character skin ScriptableObjects.
- Missions: mission definition SO + mission runtime tracker.
- Daily runs: seed-based procedural pattern selection.
- LiveOps: remote config patching of difficulty/power-up values.

## 10. MVP Feature Checklist
- [x] 3-lane swipe runner
- [x] Active color + next color
- [x] Shift warning ring/text
- [x] Gate success/failure resolution
- [x] Score + streak + multiplier
- [x] 3-strike fail loop
- [x] Chaser pressure system
- [x] Surge Fuel + Shield Pulse
- [x] Phase-based speed/gate spacing
- [x] HUD + Game Over screens

## 11. Prototype Build Order
1) Add ScriptableObjects and tuning defaults.
2) Wire `GameManager` and initial run flow.
3) Add swipe + runner lane movement.
4) Add color state + warning UI.
5) Add gate prefab and spawner.
6) Hook scoring and strike logic.
7) Hook chaser reaction.
8) Add power-up pickups/effects.
9) Finalize HUD and game-over UX.
10) Tune difficulty and polish audio/haptics hooks.
