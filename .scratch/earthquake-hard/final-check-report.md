# Earthquake Hard: final check report

Ticket: `issues/03-final-check.md`. Run on 2026-11-17, branch `earthquake-hard` at 85d854e.

## Batch runs

| Step | Result |
|---|---|
| `BuildAllBatch` | exit 0 |
| EditMode | 155 total, 155 passed, 0 failed |
| PlayMode | 10 total, 10 passed, 0 failed |
| `CaptureBatch` | exit 0, 54 Level screenshots (10 to 58, both sizes) |

## Tests added on this branch

- EditMode `ScenarioContentTests.HardLevels_AllExist`
- EditMode `ScenarioTests.CanShake_OnlyForSettingsWithAShaker`: new cases `mall_a` true, `mall_b`, `home_after`, `evacuation_area` false
- PlayMode `LevelFlowPlayTests.HardDifficulty_CompletingAllSevenLevels_CompletesTheEpisode`
- PlayMode `MenuFlowPlayTests.HardOpen_LevelSelection_StartsEveryHardLevel`
- Changed: `LevelFlowPlayTests.NormalDifficulty_CompletingAllFiveLevels_OpensHard` now expects "Hard is open! Let's play!"

## Screenshots

All screenshots 10 to 58 were read at 1920x1080 and 2400x1080.

- 50 to 56 show the right Setting for each Hard Level. No Choice label is cut off. The long labels ("Stand by the glass shop window", "Share news you heard from others") use the compact label font.
- 52 (`mall_a`): the tables, elevator, shop window, display case and hanging sign are clear of the UI. At 2400x1080 the top of the hanging sign is cropped by the screen edge, not covered by UI.
- 53 (`mall_b`): at 2400x1080 Bantay's bubble hides the top of the mall doors. The spec allows this.
- 56 (`evacuation_area`): Bantay and the Hint stand by the tent. The families, the radio and the go bag are clear.
- 57 shows the `elevator` Feedback. 58 shows "Fire is open!" and Bantay's "Fire is open! New levels are coming soon."
- 46 shows "Hard is open! Let's play!".
- 10 to 45 match the layouts on main.

Two layout quirks are already on main and are not caused by this branch:

- A two-line Goal (Easy 2, Normal 1, Normal 3, Normal 5, Hard 5) fills the 120-unit Goal banner, so the top of the first line touches the banner's top edge. The text is not cut off.
- The Normal 5 Level Completed lesson wraps to three lines, so its last line sits just above the buttons (shot 46).

## Assets

`git diff 0e4ba7f --stat` shows no PNG, `.aseprite`, audio or Scenario file changes after the design commit. Only new `.meta` files appear.

## Not built from the spec

Nothing. The code review noted two points:

- On this branch, the spec and ticket 03 were edited after the design commit to match what was built. The mall sign moved from (330, 0) to (380, 0). The lists of objects to keep clear were relaxed: the `mall_b` doors, the `home_after` photo and lamp, and the `evacuation_area` tent. The asset rule now allows comment edits under `ArtSource`. These spec edits are approved.
- The new Setting groups set only `guidanceOffsetX`. `alignArtTop` and `goalPosition` keep their defaults, and the screenshots show those defaults work.
