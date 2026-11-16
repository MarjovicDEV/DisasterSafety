# Spec: Earthquake / Easy / Levels 2 and 3

Build the code that makes Earthquake / Easy / 2 and 3 playable, and the moment when Normal opens after the last Easy Level. Read `AGENTS.md` first. Read `docs/design/earthquake-easy-2-3.md` for what was decided and why.

Already done and committed, so use these as they are:

- Scenario files `Assets/Resources/Scenarios/Earthquake/Easy/2.json` (`phase` "during", `setting` "corridor") and `3.json` (`phase` "after", `setting` "classroom_after"). Their text is final and checked by `ScenarioContentTests`.
- Art in `Assets/Art/Level/` (sizes in art pixels; 1 art pixel = 4 UI units):

  | File | Size | Use |
  |---|---|---|
  | `bg_corridor.png` | 480x270 | Setting `corridor` background |
  | `pots_0.png`, `pots_1.png` | 20x44 | hanging flower pot, 2 sway frames, placed at (116, 12) and (236, 12) |
  | `bg_classroom_after.png` | 480x270 | Setting `classroom_after` background; the fan and cabinet are painted in |
  | `teacher_0.png`, `teacher_1.png` | 30x50 | the teacher waving in the doorway, placed at (418, 126) |
  | `choice_cover`, `choice_stairs`, `choice_hall_window`, `choice_plants`, `choice_walk`, `choice_push`, `choice_stay` | 72x52 | Choice pictures |

  Positions are art pixels from the top left of the scene, the same convention `ArtImage(parent, name, sprite, artX, artY)` uses.
- Sound `Assets/Audio/Sfx/sfx_opened.wav`.

## Behaviour

1. **Setting selects the art.** The Level scene holds one art group per Setting. Only the group named by `Scenario.setting` is active.

   | Setting | Background | Moving parts |
   |---|---|---|
   | `classroom` | `Level/bg_classroom` | `Level/fan` at (204, 0), `Level/cabinet` at (364, 64); wobblers (as today) |
   | `corridor` | `Level/bg_corridor` | `Level/pots` at (116, 12) and at (236, 12); wobblers, 4 fps, `randomStart` so they sway out of step |
   | `classroom_after` | `Level/bg_classroom_after` | `Level/teacher` at (418, 126); always animating at 3 fps; it is not a wobbler |

   Each group keeps today's structure: a still "Backing" copy of the background behind a "Shaking" copy that the `SceneShaker` moves.
2. **Phase decides shaking.** For `phase` "during", keep today's flow: `StartShake`, the rumble, a 0.4 s wait, Bantay's intro, the Choices, `Calm` on a Choice and `ResumeWobble` on Try Again. For "after" and "before" there is no shake, no rumble and no wobble: Bantay says the intro at once, and the Choices appear 1.1 s later.
3. **Choice pictures.** All 11 Choice pictures are in `LevelController.pictures`: the 4 Level 1 pictures plus the 7 new ones.
4. **Next** works between Easy Levels with the existing rule. Level 1 now shows Next to Level 2, and Level 2 shows Next to Level 3. Level 3 hides Next, because Normal 1 is not a Playable Level.
5. **"Normal is open!"** When the save succeeded and `CompletionResult.Opened` is not empty, Level Completed also shows:
   - **An Opened ribbon.** It sits under the Stars: a gold `UI/button_yellow` sliced chip with `Icons/icon_hardhat` and the label `UIStrings.Opened(OpenedName(Opened[0]))`, which gives "Normal is open!". Anchor it to the panel's top centre at (-370, -428), size 520x80, with the label in `f4` Ink. It must not overlap the Stars (they end near y -380) or the Safe picture (it starts at y -476). Hide it on every other Level Completed.
   - **`sfx_opened`**, played once `stars.Done` is true, plus 0.2 s, after the confetti.
   - **Bantay's line.** After Bantay's cheer, about 2.5 s later, Bantay says `UIStrings.BantayOpenedSoon(name)` when the opened Difficulty has no Playable Level yet, and `UIStrings.BantayOpened(name)` otherwise. Tapping Bantay still repeats the cheer.

   `OpenedName(key)` turns a `Curriculum` key into a name: use `Curriculum.TryParseKey`. A key with Difficulty Easy means a new Episode opened, so the name is `UIStrings.EpisodeName`; any other key gives `UIStrings.DifficultyName`. A replay of an Easy Level opens nothing, so no ribbon, sound or line appears.

## File-by-file

### Core (`Assets/Scripts/Core`, EditMode-tested, no MonoBehaviour)

- `ScenarioSettings.cs` (new) holds `public static class ScenarioSettings`: the constants `Classroom = "classroom"`, `Corridor = "corridor"`, `ClassroomAfter = "classroom_after"`, a read-only list `All`, and `bool IsKnown(string)`.
- `ScenarioPhases.cs` (new) holds `public static class ScenarioPhases` with `Before`, `During` and `After`, and `bool IsKnown(string)`.
- `Scenario.cs`: add the property `public bool Shakes => phase == ScenarioPhases.During;`.
- `ScenarioRules.cs`: add three Problems.
  - The setting is unknown: "Setting '<x>' has no scene art."
  - The phase is unknown: "Phase '<x>' must be before, during or after."
  - A Choice has an empty `picture`: "<name> needs a picture."

### Game (`Assets/Scripts/Game`)

- `UI/LevelSetting.cs` is a new MonoBehaviour. It has `public string key; public SceneShaker shaker;` and sits on the root of each Setting group.
- `UI/LevelController.cs`
  - Replace the field `shaker` with `public LevelSetting[] settings;`. In `Start`, activate the setting whose `key` equals `scenario.setting` and deactivate the others.
  - Expose `public LevelSetting ActiveSetting { get; private set; }` for tests.
  - Apply behaviour 2: guard every `StartShake`, `Calm` and `ResumeWobble` call and the rumble with `scenario.Shakes`.
  - Add `public GameObject openedRibbon; public Text openedText;`. Apply behaviour 5 in `ShowCompleted`.
- `UIStrings.cs`: add these, word for word.
  - `public static string Opened(string what) => what + " is open!";`
  - `public static string BantayOpenedSoon(string what) => what + " is open! New levels are coming soon.";`
  - `public static string BantayOpened(string what) => what + " is open! Let's play!";`
- `Audio/AudioManager.cs`: add `Opened` at the end of `enum Sfx`.
- `Audio/AudioLibrary.cs`: add the field `public AudioClip opened;` and `case Sfx.Opened: return opened;`.

### Editor (`Assets/Scripts/Editor`)

- `ProjectBuilder.cs`: in the audio library build, add `lib.opened = Clip("Sfx/sfx_opened");`.
- `ProjectBuilder.Level.cs`
  - Replace `BuildClassroom` with one builder per Setting: `BuildClassroom`, `BuildCorridor` and `BuildClassroomAfter`. All three go under one "World" `RectTransform` with the `EnvelopeScaler`, and each returns its `LevelSetting`. Assign them to `c.settings`. Keep the layer order: World first, then SafeArea.
  - Extend `ChoicePictureNames` with the 7 new pictures.
  - Build the Opened ribbon inside `BuildLevelCompleted` (behaviour 5), inactive by default.
- `LevelScreenshots.cs`: keep shots 10-15 for Level 1. Add:
  - `20-level2` (Level 2 Choices with the intro);
  - `21-level2-feedback-safe`;
  - `30-level3`;
  - `31-level3-feedback-unsafe`, with the "Time to go" heading on `stay`;
  - `32-level-completed-normal-open` (Level 3 Level Completed with the ribbon and the `BantayOpenedSoon` line).

  Activate the matching Setting for each shot.

### Tests

- EditMode `ScenarioTests.cs`:
  - each new ScenarioRules Problem is reported;
  - `TestScenarios.Classroom()` still has no Problems;
  - `Shakes` is true only for "during".
- EditMode `ScenarioContentTests.cs`:
  - rename `FirstPlayableLevel_Exists` to `EasyLevels_AllExist`, checking `Earthquake/Easy/1`, `/2` and `/3`;
  - add `EveryChoicePicture_HasArt`: for every Choice, the file `Assets/Art/Level/<picture>.png` exists;
  - add `EverySetting_HasArt`: for every Scenario, the background `Assets/Art/Level/bg_<setting>.png` exists.
- PlayMode `LevelFlowPlayTests.cs`:
  - change the assertion with the message "Level 2 is not playable yet" so it expects Next to be shown.
  - Add `EasyDifficulty_CompletingAllThreeLevels_OpensNormal`. Start from a fresh save with the Display Name "Ana". Play Level 1 (the desk Choice). Press Next. Assert Level 2 is loaded, `ActiveSetting.key == "corridor"` and the shaker is shaking. Pick `stairs`, check the heading "Not safe yet", press Try Again, pick `cover`, then Finish.
  - In that test, press Next again. Assert Level 3 is loaded, `ActiveSetting.key == "classroom_after"` and nothing is shaking. Pick `walk`, then Finish.
  - Then assert all of these:
    - `LastResult.Opened` contains "Earthquake/Normal";
    - `openedRibbon` is active and `openedText.text` is "Normal is open!";
    - Next is hidden;
    - the save on disk has Earthquake/Normal Open;
    - within 5 s Bantay's bubble shows "Normal is open! New levels are coming soon.".
  - Finally press Replay, play Level 3 again, and assert that `openedRibbon` is inactive.
- PlayMode `MenuFlowPlayTests.cs`: Level Selection Start is on for Easy 1, 2 and 3. Replace the assertion with the message "Level 2 is not a Playable Level yet".

## Acceptance criteria

- `BuildAllBatch` succeeds, and EditMode and PlayMode report zero failures, with every test listed above present.
- Screenshots 10-15 look as before. Screenshots 20-32 show the right Setting, nothing overlaps at 1920x1080 or 2400x1080, and the ribbon sits between the Stars and the Safe picture.
- No new Learner sentence exists outside `UIStrings.cs` and the Scenario files. No file under `Assets/Art`, `Assets/Audio`, `ArtSource` or `Assets/Resources/Scenarios` changed.
- Each new MonoBehaviour is in its own file named after the class. `DisasterSafety.Core` has no MonoBehaviour.

## Tickets

Tracer bullets, in order. Each ticket ends playable, tested and green before the next one starts. They live in `.scratch/earthquake-easy-2-3/issues/`:

1. `01-level-2-corridor.md`
2. `02-level-3-after-the-shaking.md` (blocked by 1)
3. `03-normal-is-open.md` (blocked by 2)
4. `04-final-check.md` (blocked by 3)
