# Spec: Earthquake / Normal (Levels 1 to 5)

Build the code that makes Earthquake / Normal / 1 to 5 playable. Read `AGENTS.md` first, then `docs/design/earthquake-normal.md` for what was decided and why. `docs/specs/earthquake-easy-2-3.md` describes the Setting system this builds on.

Already done and committed, so use these as they are:

- Scenario files `Assets/Resources/Scenarios/Earthquake/Normal/1.json` to `5.json`. Their text is final and checked by `ScenarioContentTests`.
- `ScenarioSettings` already lists `bedroom`, `home`, `schoolyard` and `beach`. `home` and `schoolyard` can shake; `bedroom` and `beach` cannot.
- Art in `Assets/Art/Level/` (sizes in art pixels; 1 art pixel = 4 UI units; positions from the top left of the 480x270 scene, the convention `ArtImage(parent, name, sprite, artX, artY)` uses):

  | File | Size | Use |
  |---|---|---|
  | `bg_bedroom.png` | 480x270 | Setting `bedroom` |
  | `bg_home.png` | 480x270 | Setting `home` |
  | `lamp_0.png`, `lamp_1.png` | 24x44 | hanging lamp at (232, 0) |
  | `glass_cabinet_0.png`, `glass_cabinet_1.png` | 46x112 | glass cabinet at (330, 64) |
  | `bg_schoolyard.png` | 480x270 | Setting `schoolyard` |
  | `tree_0.png`, `tree_1.png` | 76x128 | big tree at (260, 34) |
  | `post_0.png`, `post_1.png` | 60x150 | electric post at (360, 14) |
  | `bg_beach.png` | 480x270 | Setting `beach` |
  | `waves_0.png`, `waves_1.png` | 340x8 | wave line at (140, 128) |
  | 20 Choice pictures | 72x52 | `choice_low_shelf`, `choice_picture_bed`, `choice_bed_window`, `choice_books_top`, `choice_go_bag`, `choice_pack_later`, `choice_bag_hidden`, `choice_bag_toys`, `choice_table`, `choice_run_home`, `choice_glass_cabinet`, `choice_sofa`, `choice_open_field`, `choice_tree`, `choice_post`, `choice_building`, `choice_high_ground`, `choice_fish`, `choice_watch_waves`, `choice_wait_beach` |

## Behaviour

1. **Four new Setting groups** in the Level scene, built like the Easy ones (a still "Backing" copy behind the foreground):

   | Setting | Shakes | Moving parts |
   |---|---|---|
   | `bedroom` | no (no `SceneShaker`) | none |
   | `home` | yes | wobblers: `Level/lamp` at (232, 0), 4 fps, and `Level/glass_cabinet` at (330, 64), 5 fps; both `randomStart` |
   | `schoolyard` | yes | wobblers: `Level/tree` at (260, 34), 3 fps, and `Level/post` at (360, 14), 4 fps; both `randomStart` |
   | `beach` | no | `Level/waves` at (140, 128), always animating at 2.5 fps; not a wobbler |

   Set each group's `alignArtTop`, `goalPosition` and `guidanceOffsetX` so that the Goal banner, Bantay, the Hint button and the Choices panel cover none of: the low shelf and bookcase (bedroom); the lamp, table, glass cabinet and door (home); the tree, post and open field (schoolyard); the hill path and the fish (beach). Check this in the screenshots at 1920x1080 and 2400x1080.
2. **Phase** works as today: "before" and "after" start still; "during" shakes. Normal 1, 2 and 5 are still; Normal 3 and 4 shake.
3. **Choice pictures**: all 20 new names are added to `ChoicePictureNames`.
4. **Next and Open** work with the existing rules, with no new code: Next runs from Normal 1 to Normal 5, and Normal 5 hides Next. Completing all five opens Hard, so the Opened ribbon reads "Hard is open!", and Bantay says "Hard is open! New levels are coming soon.", because Hard has no Playable Level yet.
5. **Easy 3 changes too**: Normal now has Playable Levels, so completing Easy shows Bantay's line "Normal is open! Let's play!" (`UIStrings.BantayOpened`), no longer the "coming soon" line.

## File-by-file

- `Assets/Scripts/Editor/ProjectBuilder.Level.cs`:
  - add `BuildBedroom`, `BuildHome`, `BuildSchoolyard` and `BuildBeach`, using the existing `BuildSetting` and `BuildLevelAnimation`;
  - add them to `c.settings` (inactive except the first);
  - add the 20 picture names to `ChoicePictureNames`.
- `Assets/Scripts/Editor/LevelScreenshots.cs`: add these shots at both sizes, each with its Setting active:
  - `40-normal1`, `41-normal2`, `42-normal3`, `43-normal4`, `44-normal5`: the Choices with Bantay's intro;
  - `45-normal5-feedback-unsafe`: the `fish` Choice;
  - `46-level-completed-hard-open`: Normal 5 Level Completed with the "Hard is open!" ribbon and the `BantayOpenedSoon` line.
- No change in `LevelController`, `UIStrings`, the audio code or Core, except the tests below.

## Tests

- EditMode `ScenarioContentTests.cs`: add `NormalLevels_AllExist`, which checks `Earthquake/Normal/1` to `/5`. `EverySetting_HasArt` and `EveryChoicePicture_HasArt` already cover the new art.
- EditMode `ScenarioTests.cs`: add `CanShake_OnlyForSettingsWithAShaker`: true for `classroom`, `corridor`, `home` and `schoolyard`; false for `classroom_after`, `bedroom` and `beach`.
- PlayMode `LevelFlowPlayTests.cs`:
  - in `EasyDifficulty_CompletingAllThreeLevels_OpensNormal`, expect "Normal is open! Let's play!" in place of "Normal is open! New levels are coming soon.";
  - add `NormalDifficulty_CompletingAllFiveLevels_OpensHard`. Start from a fresh save with the Display Name "Ana" and Earthquake / Normal Open (`GameServices.Progress.Open(Episode.Earthquake, Difficulty.Normal)`, then save). For each Normal Level in order, assert `ActiveSetting.key` (`bedroom`, `home`, `home`, `schoolyard`, `beach`) and whether the shaker is shaking (only Levels 3 and 4). Pick the Safe Choice by `scenario.SafeChoiceIndex` (expose the loaded Scenario on `LevelController` read-only if needed), press Finish, then press Next.
  - Still in that test, after Level 5 assert all of these:
    - `LastResult.Opened` contains "Earthquake/Hard";
    - `openedText.text` is "Hard is open!";
    - Next is hidden;
    - the save has Earthquake / Hard Open;
    - within `OpenedLineDelay` plus 2 s, Bantay says "Hard is open! New levels are coming soon.".
- PlayMode `MenuFlowPlayTests.cs`: once Normal is Open, Level Selection Start is on for Normal 1 to 5.

## Acceptance criteria

- `BuildAllBatch` succeeds, and EditMode and PlayMode report zero failures, with every test listed above present.
- Screenshots 10-32 look as they do on `main`. Screenshots 40-46 show the right Setting, and nothing covers the objects listed in behaviour 1 at either size.
- No file under `Assets/Art`, `Assets/Audio`, `ArtSource` or `Assets/Resources/Scenarios` changed, apart from `.meta` files Unity creates.

## Tickets

Tracer bullets, in order: `.scratch/earthquake-normal/issues/`.

1. `01-normal-at-home.md`: `bedroom` and `home`, so Normal 1 to 3 are playable.
2. `02-normal-outside.md` (blocked by 1): `schoolyard` and `beach`, Normal 4 and 5, and the full Normal PlayMode test.
3. `03-final-check.md` (blocked by 2).
