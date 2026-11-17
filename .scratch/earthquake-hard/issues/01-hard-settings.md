# 01 Hard Settings: every Hard Level playable

Blocked by: none. Blocks: 02.

Spec: `docs/specs/earthquake-hard.md` (behaviours 1 to 3). Design: `docs/design/earthquake-hard.md`. Rules: `AGENTS.md`.

## Build

- `ScenarioSettings.cs`: add `mall_a`, `mall_b`, `home_after` and `evacuation_area` to `All`; `CanShake` adds `mall_a` only.
- `ProjectBuilder.Level.cs`:
  - add `BuildMallA` (hanging sign and glass display wobblers), `BuildMallB`, `BuildHomeAfter` and `BuildEvacuationArea` (all three still) to `c.settings`;
  - set each new group's `alignArtTop`, `goalPosition` and `guidanceOffsetX` so nothing covers the objects listed in spec behaviour 1;
  - add all 28 Hard Choice pictures to `ChoicePictureNames`.
- `LevelScreenshots.cs`: add `50-hard1` to `56-hard7` and `57-hard3-feedback-unsafe`.
- Tests:
  - EditMode: `HardLevels_AllExist`, and the four new cases in `CanShake_OnlyForSettingsWithAShaker`.

The Hard design, art and Scenario files are the first commit on the `earthquake-hard` branch, so the EditMode content tests fail there until this ticket lands. That is expected.

## Done when

- `BuildAllBatch` succeeds, and EditMode is green with the new tests present.
- PlayMode is green except `NormalDifficulty_CompletingAllFiveLevels_OpensHard`, which now hears "Hard is open! Let's play!"; ticket 02 updates it. Note any other failure in the report.
- Screenshots 50 to 57 at both sizes show the right Setting, no label is cut off, and the objects listed in spec behaviour 1 are uncovered.
- Screenshots 10 to 45 look as they do on `main`.
