# Spec: gameplay redesign pilot (Earthquake / Easy / 1)

Build the new Level flow for one Level, Earthquake / Easy / 1 in the classroom, so it can be judged on a phone before every Level moves over. Read `AGENTS.md`, then `docs/design/gameplay-redesign.md` (what was decided) and `docs/adr/0001-v2-art-and-no-bantay.md` (why). Terms follow `CONTEXT.md`: Choice, Action Bar, Caption, Student, Reaction.

Branch: `gameplay-redesign`. All batch commands in `AGENTS.md` run against `D:/GameDEV/DisasterSafety`, so the work happens in that checkout with `gameplay-redesign` checked out.

## Scope

In the pilot:

- v2 art for the Student (boy and girl), the classroom, its moving parts, 4 Choice icons and the new UI pieces.
- The Learner picks a boy or a girl Student with the Display Name.
- Earthquake / Easy / 1 plays with the Caption, the Action Bar and the Student acting out each Choice.
- An Android test build (APK).

Not in the pilot (later specs, after the phone check):

- Every other Level keeps today's flow: Choice cards and Bantay. Both flows must work side by side until the rollout ends.
- The Main Menu and How to Play keep Bantay for now.
- Chains of 2 or 3 steps (Normal, Hard) and their scoring.
- Deleting Bantay and the Choice pictures.

## Behaviour

1. **Which flow a Level uses.** A Scenario uses the new flow when every Choice has an `action`. Easy 1 gets `icon` and `action`; every other Scenario file stays as it is and uses today's flow.
2. **Student pick.** After the Display Name is accepted, a picker shows the big boy and the big girl Student; the Learner taps one. The pick is saved as `student` ("boy" or "girl") and is changeable later only by a new Display Name entry. A save with a Display Name but no `student` shows the picker once at the next Main Menu. Until a pick exists, the Level uses the boy.
3. **Level start.** The classroom shows, with the Student standing at the start spot, idle (breathing loop). The Caption at the top shows the Goal and, under it, the Scenario intro.
4. **The earthquake** ("during" only). The scene shakes for 2.5 seconds with the rumble, the fan and cabinet wobble, and the Student plays "surprised" and says "Whoa!". Then the shake drops to the gentle wobble and the Action Bar slides up. A "before" or "after" Scenario in the new flow shows the Action Bar after 1 second, without the shake.
5. **The Action Bar** shows the Scenario's Choices in file order as buttons: a 16x16 icon and the Choice `label`. A Choice already picked in this Attempt stays visible but marked tried, as cards are today.
6. **A Safe Choice.** The Action Bar hides. The Student turns and walks to the Choice's action spot, then plays the end pose (for Easy 1: Duck, Cover, and Hold under the desk; the desk's front is drawn over the Student). The shaking calms, the Student says "I'm safe!", and after 1 second the Feedback panel shows with Finish. Finish leads to Level Completed as today.
7. **An Unsafe Choice.** The Action Bar hides. The Student walks part of the way to the action spot (`stopAt`, a fraction of the distance), stops, plays "Hmm, wait..." and says "Hmm, wait...". After 1 second the Feedback panel shows with Try Again. Try Again hides Feedback, the Student walks back to the start spot, and the Action Bar shows again.
8. **Hint.** The Hint button writes the Hint into the Caption, in place of the intro, and costs a Star as today.
9. **Feedback panel.** Same heading and text as today, with no picture. The new-flow layout centres the text.
10. **Level Completed.** The Safe Choice picture is replaced by the Student's happy pose, and the cheering Bantay by the Student's speech bubble with the existing cheer line. Stars, the lesson, the Opened ribbon and the buttons are unchanged.
11. **Pause, Restart and Quit** work as today. While paused, the Student and every animation stop (they follow `Time.timeScale`).
12. **Reactions** never judge and never mention Stars: only "Whoa!", "Hmm, wait..." and "I'm safe!" in the Level, from `UIStrings`.

## Data

- `Scenario.cs`, `ScenarioChoice`: add `icon` (art key of a 16x16 icon, e.g. `icon_desk`) and `action` (key of an action spot in the Setting, e.g. `desk`). Keep `picture`; a new-flow Choice may leave it empty.
- `ScenarioRules`: a Scenario where some Choices have `action` and some do not is invalid. In a new-flow Scenario every `icon` must have art and every `action` must exist in that Setting.
- `Earthquake/Easy/1.json`: give each Choice `icon` and `action`: `run` uses `icon_door` and `door`; `desk` uses `icon_desk` and `desk`; `window` uses `icon_window` and `window`; `cabinet` uses `icon_cabinet` and `cabinet`. No text changes.
- `SaveData`: add `student` (string, empty by default). `saveVersion` stays 1, because an empty `student` reads as "not picked yet".
- `ProgressState`: `Student` (a `StudentKind` enum: `None`, `Boy`, `Girl`) and `SetStudent(StudentKind)`, which saves.

## Art (ticket 01)

Drawn by a new `ArtSource/scripts/student.lua` and a v2 classroom in `ArtSource/scripts/classroom_v2.lua`, both run from `build.lua`, in the v2 style of ADR 0001. Each saves an `.aseprite` file and the PNGs, so the user can hand-polish any of them. The style samples and their half maps are in `D:\GameDEV\DisasterSafety - Source\StyleSample\` (`student2.py`, `v2_*.png`).

| File in `Assets/Art/` | Size | Frames |
|---|---|---|
| `Student/boy_idle_*`, `girl_idle_*` | 32x50 | 2 |
| `Student/*_surprised_*` | 32x50 | 2 |
| `Student/*_walk_*` (side view facing right; the game mirrors it for left) | 32x50 | 4 |
| `Student/*_duck_*` (Duck, Cover, and Hold, front, sized to fit under the v2 desk) | 32x50 canvas | 2 |
| `Student/*_wait_*` ("Hmm, wait...", front, hand on chin) | 32x50 | 2 |
| `Student/*_happy_*` (arms up) | 32x50 | 2 |
| `Student/*_portrait` (the big front idle for the picker and Level Completed) | 32x50 | 1 |
| `Level/v2/bg_classroom` | 480x270 | 1 |
| `Level/v2/desk_front` (the part of the desk drawn over the ducking Student) | as drawn | 1 |
| `Level/v2/fan_*`, `Level/v2/cabinet_*` | as drawn | 2 each |
| `UI/icon_door`, `icon_desk`, `icon_window`, `icon_cabinet` | 16x16 | 1 |
| `UI/caption_banner`, `UI/action_button`, `UI/speech_bubble` | 9-slice | 1 |

The classroom is laid out for a 50-pixel Student: the floor line is at y = 230 (the Student's feet), the desk is about 40 pixels tall, and there is room to walk between the start spot and every action spot. `ArtSource/README.md` records the placements, the start spot and the four action spots (door, desk, window, cabinet), in art pixels.

The current art stays untouched: no file under `Assets/Art` that exists today changes.

## Code

- `Assets/Scripts/Game/UI/StudentActor.cs` (new MonoBehaviour): holds the pose frames for one Student, plays a pose (`Idle`, `Surprised`, `Walk`, `Duck`, `Wait`, `Happy`), faces left or right by mirroring, walks to an x in art pixels at a set speed, and shows a Reaction in its speech bubble. It follows `Time.timeScale`.
- `Assets/Scripts/Game/UI/SpeechBubble.cs` (new MonoBehaviour), if `StudentActor` needs it as its own component.
- `Assets/Scripts/Game/UI/ActionBar.cs` and `ActionButton.cs` (new MonoBehaviours): the slide-up bar and its buttons.
- `LevelSetting`: add `startX` and `actionSpots` (`key`, `x`, `stopAt`, `endPose`, and an optional overlay such as `desk_front`).
- `LevelController`: branch on the flow from behaviour 1. The new flow uses the Caption, the Action Bar and `StudentActor`; the old flow stays exactly as it is. `Core` rules (`Attempt`, Stars) drive both.
- `ProjectBuilder.Level.cs`: build the v2 classroom as a new Setting group `classroom_v2` used only by the new flow; build the Caption, the Action Bar and the Student; keep every existing group and the old UI.
- `ScenarioSettings`: no new key. The new flow picks the `classroom_v2` art group for `setting: "classroom"`; the old flow keeps `classroom`.
- Main Menu: the Student picker after Display Name entry (`MainMenuController`, `ProjectBuilder.cs`).
- `UIStrings`: "Whoa!", "Hmm, wait...", "I'm safe!", the picker title "Who are you?" and the two button names "Boy" and "Girl".
- `ProjectBuilder`: `BuildAndroidBatch`, which writes `Builds/DisasterSafety-pilot.apk` (development build off, `Builds/` ignored by git).

## Tests

- EditMode:
  - `ScenarioTests`: a mixed Scenario (some Choices with `action`, some without) is rejected; `Earthquake/Easy/1` uses the new flow; every other Scenario uses the old flow.
  - `ScenarioContentTests`: every new-flow `icon` has art.
  - `ProgressStateTests`: a new save has `StudentKind.None`; `SetStudent` saves and reloads.
- PlayMode `LevelFlowPlayTests`:
  - `EasyLevel1_NewFlow_SafeChoice_StudentDucksAndCompletes`: the Caption shows the Goal; the Action Bar is hidden during the first shake and then shows 4 buttons; pressing `desk` makes the Student walk to the desk spot and play `Duck`, then Feedback shows with Finish; Finish shows Level Completed with 3 Stars.
  - `EasyLevel1_NewFlow_UnsafeChoice_StudentStopsAndTriesAgain`: pressing `window` makes the Student stop short of the window spot with `Wait`; Feedback shows Try Again; Try Again returns the Student to the start spot and shows the Action Bar with `window` marked tried.
  - `EasyLevel1_NewFlow_Hint_ShowsInCaption`.
  - The existing tests for Easy 2 and 3, Normal and Hard still pass unchanged (old flow).
- PlayMode `MenuFlowPlayTests`: after a new Display Name, the picker shows; picking the girl saves `Girl` and Easy 1 then uses the girl frames.
- Screenshots, at both sizes: `60-pilot-start`, `61-pilot-shaking`, `62-pilot-actionbar`, `63-pilot-duck`, `64-pilot-wait`, `65-pilot-completed`, `66-student-picker`. Screenshots 10-57 stay as they are.

## Acceptance criteria

- `BuildAllBatch` succeeds; EditMode and PlayMode report zero failures with every test above present.
- Easy 1 plays the new flow end to end; every other Level plays exactly as on `main`.
- No existing file under `Assets/Art` or `Assets/Audio` changes, and no Scenario file except `Earthquake/Easy/1.json`.
- `Builds/DisasterSafety-pilot.apk` installs and runs on the user's Android phone, and the user has judged the pilot there.

## Tickets

Tracer bullets in `.scratch/gameplay-redesign/issues/`, in order:

1. `01-v2-art.md`: the Student, the v2 classroom, icons and UI pieces.
2. `02-student-pick.md` (blocked by 1): save field, picker, tests.
3. `03-pilot-level-flow.md` (blocked by 1 and 2): the new flow for Easy 1, tests and screenshots.
4. `04-android-pilot-build.md` (blocked by 3): the APK and the phone check.
