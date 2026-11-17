# 01 Normal 1 to 3 play at home

Blocked by: none. Blocks: 02.

Spec: `docs/specs/earthquake-normal.md` (behaviours 1 to 5 for `bedroom` and `home`). Rules: `AGENTS.md`.

## Build

- `ProjectBuilder.Level.cs`:
  - add `BuildBedroom` (still) and `BuildHome` (lamp and glass cabinet wobblers) to `c.settings`;
  - add all 20 Normal Choice pictures to `ChoicePictureNames`.
- `LevelScreenshots.cs`: add `40-normal1`, `41-normal2` and `42-normal3`.
- Tests:
  - EditMode: `NormalLevels_AllExist` and `CanShake_OnlyForSettingsWithAShaker`;
  - PlayMode: update the Easy test's opened line to "Normal is open! Let's play!", and add the MenuFlow check that Normal 1 to 5 can be started once Normal is Open.

Until ticket 02 lands, Normal 4 and 5 have no art group; the existing fallback logs an error and shows the first group. That is expected.

## Done when

- `BuildAllBatch` succeeds, and EditMode and PlayMode are green.
- Screenshots 40 to 42 at both sizes show the bedroom and the living room, with the low shelf, bookcase, lamp, table, glass cabinet and door uncovered.
- Screenshots 10 to 32 look as they do on `main`.
