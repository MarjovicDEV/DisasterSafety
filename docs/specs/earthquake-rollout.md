# Spec: the Earthquake rollout (14 Levels to the Student flow)

Move the 14 remaining Earthquake Levels (Easy 2 and 3, Normal 1 to 5, Hard 1 to 7) to the Student flow that the pilot built for Easy 1, with a realistic Act-out for every Choice. Read `AGENTS.md`, then `docs/design/gameplay-redesign.md` (the Level flow, Q1 to Q18, "Pilot result" and "Rollout decisions" R1 to R11 with the Act-out tables) and `docs/adr/0001-v2-art-and-no-bantay.md`. Terms follow `CONTEXT.md`: Choice, Action Bar, Caption, Student, Reaction, Act-out, Grown-up.

The Scenario text, safety lines and EQ sources in `docs/design/first-playable-level.md`, `earthquake-easy-2-3.md`, `earthquake-normal.md` and `earthquake-hard.md` still hold. No Scenario text changes in this rollout.

Branch: `earthquake-rollout`, created from `gameplay-redesign` at `7bdd53e` (the pilot), in `D:\GameDEV\DisasterSafety-redesign`. `main` does not have the pilot yet, because its checkout `D:\GameDEV\DisasterSafety` has uncommitted changes and must not be touched; the user merges it later. Batch commands run against `D:/GameDEV/DisasterSafety-redesign` with Unity 6000.6.9f1 and the Editor closed.

## Scope

In the rollout:

- The Act-out Steps that describe every Choice, with Easy 1 moved onto them.
- Nine new shared Student poses, three Grown-ups (Teacher, Mom, Dad), props, about 35 icons and the Reaction voice blip.
- Ten Settings redrawn in the v2 style, and the 14 Levels moved to the Student flow with an `icon` and an `action` per Choice.
- The Student in place of Bantay on the Main Menu and in How to Play.
- One Android APK at the end, then the Q17 cleanup.

Not in the rollout:

- Chains of steps in Normal and Hard (dropped, R1).
- Any change to `DisasterSafety.Core` rules: Stars, Attempts, Open and Locked, and Saved Progress stay as they are.
- Fire, Typhoon, Flood and Landslide.

## Hard rules

1. **Realistic Act-outs.** Every Choice, Safe and Unsafe, is acted out step by step, as in the tables in the design doc. A jump cut to a final pose is not enough. An Unsafe Choice starts the action, stops, plays `wait` and says "Hmm, wait...". Nothing falls and nobody gets hurt.
2. **The art bar.** Every new Setting, pose, Grown-up, prop and icon matches the pilot: the v2 outline, three shades with light from the left, the same palette and pixel scale (one art pixel is four UI units at 1920x1080), and furniture sized for the 50-pixel Student. Each art ticket renders a preview sheet that puts the new piece beside the pilot classroom and Student, checks it, and sends it to the user.
3. **New files only** under `Assets/Art`, `Assets/Audio` and `ArtSource`. No existing file there is edited or regenerated until the cleanup deletes what R7 names. The generator scripts may be edited; run them with a filter so they write only the new files, and check that `git status` shows only new files under those folders.
4. **Before and after Levels** show the Action Bar after 1 second without a shake; the Student still acts the Choice out.
5. **Core keeps its rules** (see "Not in the rollout").

## Behaviour

1. **Which flow a Level uses.** Unchanged: a Scenario uses the Student flow when every Choice has an `action`. Each slice gives its Levels an `icon` and an `action` per Choice. A Level not yet moved keeps the card flow, so both flows work side by side until the cleanup.
2. **Level start.** The Setting shows with the Student at its start spot in its start pose (`idle`, except Easy 3, which starts in `hold` under the desk). Grown-ups and props stand at their start places. The Caption shows the Goal and the intro.
3. **The earthquake** ("during" only), the Action Bar and the Hint work as in the pilot. The music ducks while the ground shakes.
4. **A Safe Choice.** The Action Bar hides and the Choice's Act-out plays every step. In a "during" Level the shaking calms at the step the Act-out names, once the Student is in cover. The Student plays `happy` and says the Safe Reaction: "I'm safe!" in "during" and "after" Levels, "All ready!" in "before" Levels. Then the sparkles, the Safe sound and Feedback with Finish, then Level Completed, as today.
5. **An Unsafe Choice.** The Action Bar hides and the Act-out plays up to its stop step. The Student plays `wait` and says "Hmm, wait...". Feedback shows with Try Again. Try Again hides Feedback; the Student walks back to the start spot while every moved prop returns to its place (fading back, or sliding back for furniture) and every Grown-up returns to its start pose. Then the Action Bar shows again with the Choice marked tried.
6. **Grown-ups** act only as their Act-out steps say (wave, walk, turn, sit, kneel, happy). They never judge a Choice; any line they say is a short line from `UIStrings`.
7. **Act-out lines.** A few Act-outs carry a short line in the Student's speech bubble ("Stay still, Mom.", "Help!", "Did you hear...?"). They come from `UIStrings`, follow the Reaction rule (never judge, never mention Stars), and are not part of the Scenario file.
8. **The voice blip.** Each Reaction and Act-out line plays `sfx_blip` once, pitched up for the girl, quietly under the Safe and Unsafe sounds. It follows the sound switch in Settings.
9. **Pause, Restart and Quit** work as today. While paused, the Student, the Grown-ups, the props and every Act-out stop.
10. **Main Menu and How to Play.** The Learner's Student replaces Bantay on the Main Menu (greetings and Tips in its speech bubble) and in How to Play, and the Tutorial Pages show the Action Bar flow. Level Completed already shows the Student.

## Data

- **Scenario files.** In each moved Level, every Choice gets `icon` (a 16x16 icon key, such as `icon_stairs`) and `action` (the key of its Act-out in that Setting: the Choice `id`, unless the Setting already uses that key for another Level). No `label`, `safe`, `heading`, `feedback` or `sources` change. `picture` stays until the cleanup.
- **`ScenarioSettings.ActionSpots(setting)`** lists the valid `action` keys of every Setting, for `ScenarioRules`. A shared Setting lists the keys of all its Levels; within one Setting, a key names one Act-out.
- **`ScenarioRules`.** Unchanged rules, now applied to every Setting: no mixed Scenario, every `icon` has art, every `action` exists in the Setting.

## Code

- **Act-out Steps** (`Assets/Scripts/Game/UI/ActOut.cs`): an `ActOut` has a `key`, a list of `ActOutStep`s, and `stopAfter`, the index of the last step an Unsafe Choice plays. It replaces the pilot's `ActionSpot` fields (`x`, `stopAt`, `endPose`, `overlay`, `crouchFromX`). A step is one of:
  - `Walk` or `Run` to a point (x, and y for a slope or stairs), at walk, fast-walk or run speed; `Crouch` to an x (the crouch shuffle);
  - `Pose` (play a Student pose and hold it for a time), `Say` (a `UIStrings` key), `Wait` (seconds), `Calm` (calm the shake);
  - `PropShow`, `PropHide`, `PropPickUp` (the prop follows the Student's hands), `PropPlace` (put it at a named place), `PropSlide` (move furniture a few pixels), `Overlay` (show a front overlay such as `desk_front`);
  - `GrownUp` (a Grown-up walks to a point, plays a pose or turns).

  `LevelController.ActOut` plays the steps in order and, for an Unsafe Choice, stops after `stopAfter`. A failed step (a missing prop or Grown-up) logs an error and is skipped, so the Level never locks up.
- **`PoseActor`**: the part of `StudentActor` that plays pose frames, mirrors, walks along a path at a speed and shows a speech bubble, so that `StudentActor` and the new `GrownUpActor` share it. Walking may change y as well as x.
- **`StudentPose`** gains `Carry`, `Reach`, `ReachUp`, `Push`, `Call`, `Cover`, `Pack`, `Phone` and `Sit`; `StudentFrames` gains their frames. `GrownUpPose` covers `Idle`, `Wave`, `Walk`, `Happy`, `Sit`, `HurtSit` and `Kneel`.
- **`LevelProp`**: a prop with its start place; it resets on Try Again and on Restart.
- **`LevelSetting`** gains `startPose`, `grownUps` and `props`, and holds `actOuts` in place of `actionSpots`.
- **`ProjectBuilder.Level.cs`**: one `Build<Setting>V2` per v2 Setting, in the pattern of `BuildClassroomV2`, with the Act-outs as data. Easy 1's classroom moves onto Act-out Steps with the same timing and positions.
- **`AudioManager.PlayBlip(StudentKind)`** and `Sfx.Blip`; `make_audio.py` writes only the new `Assets/Audio/Sfx/sfx_blip.wav`.
- **`UIStrings`**: "All ready!" and the Act-out lines.
- **Art scripts**: `student.lua` gains the nine poses behind a `poses=` filter, so a run writes only the named poses. New scripts, each wired into `build.lua` behind its own `only=` part: `grownups_v2.lua`, `icons_v2.lua`, `corridor_v2.lua`, `classroom_after_v2.lua`, `bedroom_v2.lua`, `home_v2.lua` (both `home` and `home_after`), `schoolyard_v2.lua`, `beach_v2.lua`, `mall_v2.lua` (both `mall_a` and `mall_b`) and `evacuation_area_v2.lua`. `ArtSource/README.md` records every new placement, start spot, Act-out point and floor line in art pixels.
- **`AGENTS.md`**: the Unity version in the batch commands becomes 6000.6.9f1, and the project path is the checkout in use.

## Art

| File in `Assets/Art/` | Size | Frames |
|---|---|---|
| `Student/{boy,girl}_carry_*` | 32x50 | 4 |
| `Student/{boy,girl}_{reach,reachup,push,call,cover,pack,phone,sit}_*` | 32x50 | 2 each |
| `GrownUp/teacher_{idle,wave}_*`, `GrownUp/teacher_walk_*` | about 36x62 | 2 each, 4 |
| `GrownUp/mom_{idle,happy,sit,hurtsit}_*`, `GrownUp/mom_walk_*` | about 36x62 | 2 each, 4 |
| `GrownUp/dad_kneel_*`, `GrownUp/dad_walk_*` | about 36x62 | 2, 4 |
| `Level/v2/bg_<setting>` for the ten Settings | 480x270 | 1 |
| `Level/v2/<setting>_<part>_*` (moving parts, front overlays such as `table_front` and `foodtable_front`, props) | as drawn | 1 or 2 |
| `UI/icon_<name>` (about 35 new) | 16x16 | 1 |

Sound: `Assets/Audio/Sfx/sfx_blip.wav`, a short soft syllable.

Each background is laid out for the 50-pixel Student and the 62-pixel Grown-ups: a floor line, room to walk between the start spot and every Act-out point, and clear space under the Caption and above the Action Bar.

## Tests

- **EditMode**
  - `ScenarioTests`: after each slice, its Levels use the Student flow and the Levels not yet moved still use the card flow; at the end, all 15 do.
  - `ScenarioContentTests`: every `icon` has art; every `action` is listed for its Setting; no Scenario text changed (the test holds the `label`, `heading`, `feedback` and `sources` of every Choice as they are on the pilot commit).
- **PlayMode `LevelFlowPlayTests`**, for every moved Level:
  - `<Level>_SafeChoice_ActsOutAndCompletes`: every step of the Safe Act-out is reached in order (the poses and prop states the table names), then Feedback with Finish, then Level Completed with 3 Stars.
  - `<Level>_UnsafeChoice_StopsAndResets`, for one Unsafe Choice per Level: the Act-out stops at `stopAfter` with `wait`; Try Again returns the Student, every prop and every Grown-up to the start, and marks the Choice tried.
  - `ActOuts_AreComplete`: every Choice of every moved Level has an Act-out in its Setting whose steps are valid and whose `stopAfter` is inside the list.
  - The existing pilot tests for Easy 1 still pass after its move onto Act-out Steps.
- **PlayMode `MenuFlowPlayTests`**: the Main Menu and How to Play show the Learner's Student, not Bantay.
- **Screenshots**, at 1920x1080 and 2400x1080, for every moved Level: `<l>-actionbar`, `<l>-safe` (the last Safe step) and `<l>-stop` (an Unsafe stop), where `<l>` is `e2`, `e3`, `n1` to `n5` and `h1` to `h7`. Each slice reads its own shots and fixes overlaps: the Caption, the Hint and Pause buttons, the Action Bar, the Student, the Grown-ups and the props must not cover each other or the parts the Act-outs use.

## Checks by the user

- After the last Easy slice, the last Normal slice and the last Hard slice, the user plays that Difficulty in the Unity Editor. The answers go into the design doc under "Rollout checks", with any change the user asks for.
- At the end, `BuildAndroidBatch` builds the APK and the user plays all 15 Earthquake Levels on the phone. The cleanup starts only after the user approves.

## Acceptance criteria

- `BuildAllBatch` succeeds; EditMode and PlayMode report zero failures with every test above present.
- All 15 Earthquake Levels play the Student flow end to end, and every Choice plays its Act-out as the design doc's tables say.
- Before the cleanup, no existing file under `Assets/Art`, `Assets/Audio` or `ArtSource` has changed, and no Scenario text has changed.
- The user has approved each Difficulty in the Editor and the APK on the phone.
- After the cleanup, no Bantay, no Choice picture, no `picture` field, no card flow and no unused v1 background remains, and every test still passes.

## Tickets

Tracer bullets in `.scratch/earthquake-rollout/issues/`, each with its blocking edges:

1. `01-act-out-steps.md`: Act-out Steps, `PoseActor`, props and y movement; Easy 1 moved onto them; the `AGENTS.md` version fix.
2. `02-shared-poses-and-blip.md`: the nine Student poses, the icon script and the voice blip.
3. `03-grown-ups.md` (blocked by 1): the Teacher, Mom and Dad, and `GrownUpActor`.
4. `04-corridor-easy-2.md` (blocked by 1, 2).
5. `05-classroom-after-easy-3.md` (blocked by 3, 4), with the Easy check.
6. `06-bedroom-normal-1.md` (blocked by 1, 2).
7. `07-home-normal-2-3-hard-1.md` (blocked by 2, 3).
8. `08-schoolyard-normal-4-hard-2.md` (blocked by 2, 3).
9. `09-beach-normal-5.md` (blocked by 6, 7, 8), with the Normal check.
10. `10-mall-hard-3-4.md` (blocked by 1, 2).
11. `11-home-after-hard-5-6.md` (blocked by 7).
12. `12-evacuation-area-hard-7.md` (blocked by 2, 3).
13. `13-menus-without-bantay.md` (blocked by 1).
14. `14-final-check-and-apk.md` (blocked by 5, 9, 10, 11, 12, 13), with the Hard check and the phone check.
15. `15-cleanup.md` (blocked by 14).
