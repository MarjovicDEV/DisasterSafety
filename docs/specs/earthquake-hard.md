# Spec: Earthquake / Hard (Levels 1 to 7)

Build the code that makes Earthquake / Hard / 1 to 7 playable and finishes the Earthquake Episode. Read `AGENTS.md` first, then `docs/design/earthquake-hard.md` for what was decided and why. `docs/specs/earthquake-normal.md` and `docs/specs/earthquake-easy-2-3.md` describe the Setting system this builds on.

Already done, so use these as they are:

- Scenario files `Assets/Resources/Scenarios/Earthquake/Hard/1.json` to `7.json`. Their text is final. Until behaviour 1 is built, `ScenarioContentTests` fail on them, because their new Setting keys are unknown.
- Art in `Assets/Art/Level/` (sizes in art pixels; 1 art pixel = 4 UI units; positions from the top left of the 480x270 scene, the convention `ArtImage(parent, name, sprite, artX, artY)` uses), drawn by `ArtSource/scripts/hard.lua`:

  | File | Size | Use |
  |---|---|---|
  | `bg_mall_a.png` | 480x270 | Setting `mall_a` (mall food court) |
  | `mall_sign_0.png`, `mall_sign_1.png` | 60x50 | hanging shop sign at (380, 0); the sprite sits right of where `hard.lua` drew it, to keep it clear of the Goal banner |
  | `shop_display_0.png`, `shop_display_1.png` | 40x64 | glass display case at (66, 112) |
  | `bg_mall_b.png` | 480x270 | Setting `mall_b` (outside the mall) |
  | `bg_home_after.png` | 480x270 | Setting `home_after` (the living room after the shaking; lamp and cabinet are drawn in) |
  | `bg_evacuation_area.png` | 480x270 | Setting `evacuation_area` |
  | 28 Choice pictures | 72x52 | `choice_meeting_place`, `choice_own_way`, `choice_meet_home`, `choice_phone_numbers`, `choice_route_field`, `choice_route_post`, `choice_route_tree`, `choice_route_later`, `choice_food_court_table`, `choice_run_exit`, `choice_elevator_during`, `choice_shop_window`, `choice_mall_stairs`, `choice_elevator_after`, `choice_run_push`, `choice_mall_wall`, `choice_call_adult`, `choice_pull_up`, `choice_leave_hurt`, `choice_fix_pipe`, `choice_go_bag_out`, `choice_back_for_toys`, `choice_no_go_bag`, `choice_no_note`, `choice_radio`, `choice_go_home`, `choice_rumor`, `choice_phone_all_day` |

| Level | Setting | Phase | Safe Choice id |
|---|---|---|---|
| Hard 1 | `home` (existing) | before | `meeting_place` |
| Hard 2 | `schoolyard` (existing) | before | `route_field` |
| Hard 3 | `mall_a` | during | `table` |
| Hard 4 | `mall_b` | after | `stairs` |
| Hard 5 | `home_after` | after | `call_adult` |
| Hard 6 | `home_after` | after | `go_bag` |
| Hard 7 | `evacuation_area` | after | `radio` |

## Behaviour

1. **Four new Settings.** `ScenarioSettings` lists `mall_a`, `mall_b`, `home_after` and `evacuation_area`. Only `mall_a` can shake. The Level scene gets one group for each, built like the Normal ones (a still "Backing" copy behind the foreground):

   | Setting | Shakes | Moving parts |
   |---|---|---|
   | `mall_a` | yes | wobblers: `Level/mall_sign` at (380, 0), 4 fps, and `Level/shop_display` at (66, 112), 5 fps; both `randomStart` |
   | `mall_b` | no (no `SceneShaker`) | none |
   | `home_after` | no | none |
   | `evacuation_area` | no | none |

   Set each group's `alignArtTop`, `goalPosition` and `guidanceOffsetX` so that the Goal banner, Bantay, the Hint button and the Choices panel cover none of: the food court tables, the elevator, the shop window, the display case and the hanging sign (`mall_a`); the outdoor stair, the parking area and the waiting family (`mall_b`; Bantay's bubble may hide the top of the mall doors); the glass cabinet, the door, the go bag and the fallen books (`home_after`; the Goal may sit over the still lamp, and on wide screens Bantay's bubble may hide the crooked photo); the families on the mats, the radio and the go bag (`evacuation_area`; Bantay and the Hint stand by the tent). Check this in the screenshots at 1920x1080 and 2400x1080.
2. **Phase** works as today: Hard 3 shakes; Hard 1, 2 and 4 to 7 start still.
3. **Choice pictures**: all 28 new names are added to `ChoicePictureNames`.
4. **Next and Open** follow the existing rules, with no new code: Next runs from Hard 1 to Hard 7, and Hard 7 hides Next.
   - Completing all of Normal now says "Hard is open! Let's play!" (`UIStrings.BantayOpened`), because Hard has Playable Levels. The ribbon still reads "Hard is open!".
   - Completing all of Hard sets `EpisodeCompleted` and opens Fire / Easy. The ribbon reads "Fire is open!", and Bantay says "Fire is open! New levels are coming soon.", because Fire has no Playable Level yet. There is no Safety Reminder screen in this milestone.
5. **The Learner's path to Hard** is unchanged: once Normal is complete, Level Selection for Hard shows 7 Levels, and Start is on for each.

## File-by-file

- `Assets/Scripts/Core/ScenarioSettings.cs`: add the four keys to `All`; `CanShake` adds `mall_a` only.
- `Assets/Scripts/Editor/ProjectBuilder.Level.cs`:
  - add `BuildMallA`, `BuildMallB`, `BuildHomeAfter` and `BuildEvacuationArea`, using the existing `BuildSetting` and `BuildLevelAnimation`;
  - add them to `c.settings` (inactive except the first);
  - add the 28 picture names to `ChoicePictureNames`.
- `Assets/Scripts/Editor/LevelScreenshots.cs`:
  - `46-level-completed-hard-open` now shows `UIStrings.BantayOpened(hardName)`;
  - add these shots at both sizes, each with its Setting active:
    - `50-hard1` to `56-hard7`: the Choices with Bantay's intro;
    - `57-hard3-feedback-unsafe`: the `elevator` Choice of Hard 3;
    - `58-level-completed-fire-open`: Hard 7 Level Completed with the "Fire is open!" ribbon and `UIStrings.BantayOpenedSoon(UIStrings.EpisodeName(Episode.Fire))`.
- No change in `LevelController`, `UIStrings`, `LevelCompletion`, the audio code, the art or the Scenario files.

## Tests

- EditMode `ScenarioContentTests.cs`: add `HardLevels_AllExist`, which checks `Earthquake/Hard/1` to `/7`. `EverySetting_HasArt` and `EveryChoicePicture_HasArt` already cover the new art.
- EditMode `ScenarioTests.cs`: extend `CanShake_OnlyForSettingsWithAShaker` with `mall_a` true and `mall_b`, `home_after`, `evacuation_area` false.
- PlayMode `LevelFlowPlayTests.cs`:
  - in `NormalDifficulty_CompletingAllFiveLevels_OpensHard`, expect "Hard is open! Let's play!" in place of "Hard is open! New levels are coming soon.";
  - add `HardDifficulty_CompletingAllSevenLevels_CompletesTheEpisode`. Start from a fresh save with the Display Name "Ana" and Earthquake / Hard Open (`GameServices.Progress.Open(Episode.Earthquake, Difficulty.Hard)`, then save). For each Hard Level in order, assert `ActiveSetting.key` (`home`, `schoolyard`, `mall_a`, `mall_b`, `home_after`, `home_after`, `evacuation_area`) and whether the shaker is shaking (only Level 3). Pick the Safe Choice by `SafeChoiceIndex`, press Finish, then press Next.
  - Still in that test, after Level 7 assert all of these:
    - `LastResult.EpisodeCompleted` is true and `LastResult.Opened` contains "Fire/Easy";
    - `openedText.text` is "Fire is open!";
    - Next is hidden;
    - the save has Fire / Easy Open;
    - within `OpenedLineDelay` plus 2 s, Bantay says "Fire is open! New levels are coming soon.".
- PlayMode `MenuFlowPlayTests.cs`: add `HardOpen_LevelSelection_StartsEveryHardLevel`, the Hard twin of `NormalOpen_LevelSelection_StartsEveryNormalLevel` (7 slots, Start on for each).

## Acceptance criteria

- `BuildAllBatch` succeeds, and EditMode and PlayMode report zero failures, with every test listed above present.
- Screenshots 10-45 look as they do on main. Shot 46 shows Bantay's "Hard is open! Let's play!" line. Screenshots 50-58 show the right Setting, no Choice label or lesson is cut off, and nothing covers the objects listed in behaviour 1 at either size.
- After the first commit on the `earthquake-hard` branch (the Hard design, art and Scenarios), no PNG, `.aseprite`, audio or Scenario file changes, except new `.meta` files Unity creates. Comments and docs under `ArtSource` may change to match where the builder places sprites.

## Tickets

Tracer bullets, in order: `.scratch/earthquake-hard/issues/`.

1. `01-hard-settings.md`: the four Settings, their scene groups and the 28 pictures, so every Hard Level is playable and the EditMode tests pass again.
2. `02-hard-flow.md` (blocked by 1): the Hard and Normal PlayMode changes, the Level Selection test and screenshots 46 and 50-58.
3. `03-final-check.md` (blocked by 2): full build, both test runs, read screenshots 10-58 at both sizes, confirm the asset folders have no changes, and report the totals.
