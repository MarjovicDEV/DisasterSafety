# 01 Level 2 plays in the corridor

Blocked by: none. Blocks: 02.

Spec: `docs/specs/earthquake-easy-2-3.md` (behaviours 1, 2, 3, 4; the Core, Game and Editor parts named below). Rules: `AGENTS.md`.

## Build

- Core: `ScenarioSettings.cs` (all three keys), `ScenarioPhases.cs`, `Scenario.Shakes`, and the three new `ScenarioRules` Problems.
- Game: `UI/LevelSetting.cs`. In `LevelController`, replace `shaker` with `settings` and add `ActiveSetting`. Guard the shake and the rumble with `Shakes`.
- Editor:
  - In `ProjectBuilder.Level.cs`, add the setting groups `classroom` (today's art, moved into a `LevelSetting`) and `corridor`, which has pots at (116, 12) and (236, 12) as wobblers at 4 fps with `randomStart`.
  - Add all 7 new Choice pictures to `ChoicePictureNames`.
  - Add the screenshots `20-level2` and `21-level2-feedback-safe` to `LevelScreenshots.cs`.
- Tests:
  - EditMode: the new `ScenarioTests` cases, plus `EasyLevels_AllExist`, `EveryChoicePicture_HasArt` and `EverySetting_HasArt` in `ScenarioContentTests`.
  - PlayMode: in `LevelFlowPlayTests` and `MenuFlowPlayTests`, update the two assertions that said Level 2 is not playable.

Level 3 has no art group until ticket 02. If `scenario.setting` matches no group, log an error and keep the first group active.

## Done when

- `BuildAllBatch` succeeds, and EditMode and PlayMode are green.
- Screenshot `20-level2` at both sizes shows the corridor, with the pots visible and not covered by the Choices panel.
- Level 1 screenshots 10-15 look the same as before.
