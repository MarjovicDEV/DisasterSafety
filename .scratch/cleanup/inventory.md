# Cleanup inventory (Phase 1, read-only)

Date: 2026-11-22. Checkout: `D:\GameDEV\DisasterSafety-redesign`, branch `earthquake-rollout` at `5edfc8e`.

## Checkout state

- The Unity Editor has this project closed. The only open Editor is on `D:\LandGrand\LandGrand`.
- The checkout differs from what the prompt expected. Besides `.vscode/settings.json`, `editmode.xml`, `playmode.xml` and `Screenshots/*`, it also has uncommitted rebuild changes in `Assets/Scenes/Boot.unity`, `Level.unity`, `MainMenu.unity` and `ProjectSettings/ProjectSettings.asset` (21234 lines added and 21234 removed: build output). This cleanup leaves them alone.
- 16 files under `Screenshots/` are tracked on this branch. The other screenshots are untracked.

## How each asset was traced

- Every sprite in the game reaches a scene through `ProjectBuilder.S(rel)` and `Frames(rel, n)` (`ProjectBuilder.cs:164-176`). No game code calls `Resources.Load` for art. The only `Resources` loads are `AudioLibrary` (`AudioManager.cs:88`) and the Scenario JSON files (`GameServices.cs:89`).
- Literal paths: every `S("...")` and `Frames("...", n)` call in `Assets/Scripts`.
- Paths built at runtime:
  - `"UI/button_" + color` and `+ "_pressed"` in `MakeButton` (`ProjectBuilder.cs:883-890`). The colours come from every `MakeButton`, `DialogButton`, `RowButton` and `PauseButton` call, plus `EpisodeColors` and `DifficultyColors`. `button_gray` is the disabled sprite.
  - `"Episodes/episode_" + EpisodeArt[i]` (`ProjectBuilder.cs:378`).
  - `"Tutorial/tutorial_" + pages[i].Art + "_0/_1"`, from `UIStrings.TutorialPages` (`ProjectBuilder.cs:704-705`).
  - `"Level/bg_" + key` for all 11 `ScenarioSettings` keys (`ProjectBuilder.Level.cs:252`).
  - `"Student/" + kind + "_" + pose` for boy and girl, 17 poses and the portrait (`ProjectBuilder.Level.cs:379-399`).
  - `"Icons/" + icon` in `RowButton` and `PauseButton`.
  - The `UI/icon_*.png` folder scan (`ProjectBuilder.Level.cs:553`).
  - `ChoicePictureNames` (`ProjectBuilder.Level.cs:22-39`).
  - `Font/pixel_font` (`PixelFontBuilder.cs:16`).
- Scene GUIDs: each PNG's GUID was taken from its `.meta` and searched in `Assets/Scenes/*.unity`, `*.prefab` and `*.asset`.
- Tickets: `.scratch/earthquake-rollout/issues/01-15`, `docs/specs/earthquake-rollout.md` and `docs/design/` were read for planned uses.
- The trace was a script: 277 of the 308 tracked PNGs are reached. Every PNG that code reaches also appears in a scene by GUID.

## Counts

| Group | Items |
|---|---|
| A: dead now, not planned (proposed) | 3 art sets, 4 C# members, 1 Core method (flagged) |
| A?: dead, needs a decision | 1 Bantay line, 2 third-party asset packs |
| B: unused now, a coming ticket needs it | 26 Grown-up PNGs (10 sources), the 9 shared poses, `sfx_blip`, `GrownUpActor`, `LevelProp`, all Act-out step kinds |
| C: card flow and Bantay, keep until ticket 15 | Bantay (4 PNGs, 1 source, `BantayGuide`, `sfx_bark`, 16 lines), 59 Choice pictures (59 sources), `picture`, `ChoiceCard`, `LevelChoicesLayout`, 11 v1 backgrounds and 22 moving-part frames |
| Packages (report only) | about 12 candidates, see below |

## Group A: dead now and not planned (proposed for deletion)

### Art

| # | Asset | Parts to delete | Evidence | Generator change |
|---|---|---|---|---|
| A1 | `Icons/icon_cross` | `Assets/Art/Icons/icon_cross.png` and `.meta`, `ArtSource/Icons/icon_cross.aseprite` | No `S()` call, not in the `UI/icon_*` scan (it lives in `Icons/`), its GUID `058fcc3e...` is in no scene, and no doc or ticket names it. Only build logs mention it. | Remove the `cross` block from `M.icons()` in `ArtSource/scripts/ui.lua:234-237`. |
| A2 | `Icons/icon_pencil` | `Assets/Art/Icons/icon_pencil.png` and `.meta`, `ArtSource/Icons/icon_pencil.aseprite` | Same evidence as A1. GUID `ccae8411...` is in no scene. | Remove the `pencil` block from `ui.lua:254-259`. |
| A3 | `UI/button_gray_pressed` | `Assets/Art/UI/button_gray_pressed.png` and `.meta`, `ArtSource/UI/button_gray_pressed.aseprite` | `MakeButton` is never called with `"gray"`. `button_gray` is used only as `disabledSprite` (`ProjectBuilder.cs:890`) and as the Tried chip, and neither has a pressed state. GUID `2a956c43...` is in no scene. | `build.lua:122-125`: skip the pressed save when the name is `gray`. `button_gray` stays. |

### Code

| # | Item | Evidence |
|---|---|---|
| A4 | `ProjectBuilder.Paper` (`ProjectBuilder.cs:32`), a private colour | It appears only in its own declaration. |
| A5 | `SpeechBubble.IsShowing` (`SpeechBubble.cs:16`) | Nothing calls it in the game, the editor tools or the tests. |
| A6 | `UIStrings.DifficultyTitle` = "Choose a Level" (`UIStrings.cs:66`) | No caller. It has not been used since `2c777f3`: the Difficulty header uses `"Earthquake"` and then `EpisodeName`. |
| A7 | `UIStrings.ComingSoon` = "Coming soon!" (`UIStrings.cs:67`) | No caller. The Level Soon message uses `BantayLevelSoon`. |
| A8 | `ProgressState.ClearDisplayName()` (`Core/ProgressState.cs:33`) | No caller and no test. **Flagged** because it is in `DisasterSafety.Core`. Removing it changes no rule (`SaveService` clears a bad name in another way), but it is public Core API. |

No test covers only A1-A8, so no test would be removed.

## Group A?: dead, but needs the researcher's decision

| # | Item | Evidence | Why it needs a decision |
|---|---|---|---|
| A9 | `UIStrings.BantayAskName` = "Let me know your name!" (`UIStrings.cs:99`) | No caller today. | It is a Bantay line, so by the rules it is group C. Ticket 13 renames the `Bantay*` keys to Student keys "without changing their text", and it may wire this line back in. Suggestion: keep it, and let ticket 13 or ticket 15 decide. **Decision 2026-11-22: deleted at the researcher's request.** |
| A10 | `Assets/8K Skybox Pack Free/` (105 tracked files plus the folder `.meta`, 263 MB): 12 skybox PNGs, 12 materials, 12 example scenes, 12 lighting files, a readme `.txt` and `Example Scenes/Script/RotateCamera.cs` | No GUID from the pack is used outside the pack. The game scenes have `m_SkyboxMaterial: {fileID: 0}`. No code or doc names it. `RotateCamera.cs` compiles into `Assembly-CSharp`, and no game scene uses it. Added in `a9d7479 Add new game assets` (2026-10-20). | It is a third-party pack, not one of Claude's assets under `Assets/Art`, and it was imported before the game existed. Deleting it is the largest win (263 MB). Also confirm that nothing outside this repo needs it. |
| A11 | `Assets/Clean Vector Icons/` (377 tracked files plus the folder `.meta`, 5.5 MB): 187 PNGs and one `.unity` scene | No GUID from the pack is used outside the pack. No code or doc names it. Added in `a9d7479`. | Same reason as A10. |

## Group B: unused now, a coming ticket needs it (keep)

- `Assets/Art/GrownUp/*`: 26 PNGs and 10 `.aseprite` sources (teacher idle, walk, wave; mom idle, walk, happy, sit, hurtsit; dad walk, kneel). No builder call yet. Tickets 05-12 place the Grown-ups.
- The nine shared Student poses (`carry`, `reach`, `reachup`, `push`, `call`, `cover`, `pack`, `phone`, `sit`), boy and girl. `StudentSet` loads them already. Only `cover` is acted out so far (Easy 2). Tickets 05-12 act out the others.
- `sfx_blip` / `Sfx.Blip`: already played for Reactions. Coming tickets use it more.
- `GrownUpActor`, `GrownUpFrames`, `LevelProp` and every `ActOutStep` kind (Run, Slope, Prop, GrownUp ...). The PlayMode tests use them. No Setting uses them yet.
- `ArtSource/scripts/grownups_v2.lua` and `icons_v2.lua`: their outputs are B, so they stay.

## Group C: used only by the card flow or Bantay (keep until ticket 15)

- **Bantay:** `Characters/bantay_0..3` (`ProjectBuilder.cs:553, 722, 780`), `ArtSource/Characters/bantay.aseprite`, `BantayGuide.cs`, `Sfx.Bark` and `Audio/Sfx/sfx_bark.wav` (only `BantayGuide.cs:55` plays it), and the `UIStrings` Bantay lines (`UIStrings.cs:95-...`, A9 included). Also `MenuController.bantay`, `LevelController.bantay` and `cheerBantay`, and their uses in `MenuScreenshots.cs:66-83` and `LevelScreenshots.cs:31-43`. `CH.bantay` in `characters.lua` stays with them.
- **Choice pictures:** 59 `Assets/Art/Level/choice_*.png` and 59 `ArtSource/Level/choice_*.aseprite`, `ChoicePictureNames`, `LV/NM/HD.choicePictures*` in `level.lua`, `normal.lua` and `hard.lua`, and the Scenario `picture` field. Easy 1 and Easy 2 still name pictures in `picture` until the cleanup (spec line 48). `ScenarioContentTests.EveryChoicePicture_HasArt` and `ScenarioTests.MissingChoicePicture_IsAProblem` cover them.
- **Card flow:** `ChoiceCard`, `LevelChoicesLayout`, `BuildChoices`, the Feedback and Level Completed pictures (`feedbackPicture`, `safePicture`), and the card branch of `LevelController`.
- **v1 Settings that a card-flow Level shows:** `bg_classroom_after` and `teacher_*` (Easy 3); `bg_bedroom` (Normal 1); `bg_home`, `lamp_*` and `glass_cabinet_*` (Normal 2-3, Hard 1); `bg_schoolyard`, `tree_*` and `post_*` (Normal 4, Hard 2); `bg_beach` and `waves_*` (Normal 5); `bg_mall_a`, `mall_sign_*` and `shop_display_*` (Hard 3); `bg_mall_b` (Hard 4); `bg_home_after` (Hard 5-6); `bg_evacuation_area` (Hard 7).
- **v1 Settings that no Level shows already:** `bg_classroom`, `fan_*` and `cabinet_*`, and `bg_corridor` and `pots_*`. Easy 1 and Easy 2 play the v2 art now. The builder still builds them (`BuildClassroom`, `BuildCorridor`) and `ScenarioContentTests.EverySetting_HasArt` checks `Level/bg_<setting>`. Ticket 15 removes them ("v1 backgrounds that no Level uses any more").
- **Shared with Bantay and the card flow, not dead:** `UI/bubble` and `bubble_tail` (Bantay's bubble and the How to Play Tip), the `Tutorial/tutorial_*` frames (How to Play uses all 10 today; ticket 13 adds new pages without rewriting these), and `Episodes/episode_*` (all 5 are used on Episode Selection).

## Checked and found used (the false positives from the 2026-11-22 name search)

- `Episodes/episode_*` (5): built from `EpisodeArt` and shown on Episode Selection. `episode_earthquake` is also the Difficulty emblem.
- `Tutorial/tutorial_*` (10): built from `TutorialPages[i].Art`.
- `UI/button_*` (19 of 20): every colour and its pressed state is reached through `MakeButton` (green, blue, orange, teal, red, white, yellow, purple and brown for the Episodes). `button_gray` is the disabled sprite and the Tried chip. Only `button_gray_pressed` is dead (A3).
- `Icons/icon_home` and `icon_levels`: built as `"Icons/" + icon` in `RowButton` and `PauseButton`, and present in the scenes.
- `Student/*`, `Level/v2/*`, `UI/icon_*`, `Background/*`, `Characters/student_*` and `Font/pixel_font`: all reached.

## Audio

All 19 files are reached. The `AudioLibrary` is built in `ProjectBuilder.cs:135-153` (2 music tracks and 17 SFX), and every `Sfx` value is played at least once. `sfx_bark` is C and `sfx_blip` is B. None is in group A.

## Art sources and generators

- 214 `.aseprite` files: each has its PNG, and each PNG has its source. There are no orphans either way.
- `ArtSource/scripts/*.lua`: every function is called, and every saved output is a tracked PNG. Only the `cross` and `pencil` blocks in `ui.lua` and the `gray` pressed save in `build.lua` draw dead art (A1-A3).

## Code

A search for names referenced only in their own declaration, confirmed by reading each one. Unity callbacks, `[MenuItem]` and `-executeMethod` entry points, Unity API members and test methods were excluded. A4-A9 are what is left. No type, file or test is dead.

## Other files

- No tracked logs, builds, `.tmp` or `.orig` files. `*.log` is ignored (`.gitignore:24`). `Builds/` is untracked.
- No empty folders, no `.meta` without its asset, and no asset without its `.meta` under `Assets/`.
- `Assets/Resources`: `AudioLibrary.asset` and the 15 Scenario files. All are used.
- Tracked, but outside this cleanup: `.vscode/` (3 files), `Screenshots/` (16 files) and `.scratch/` (the tickets).

## Packages (report only, nothing removed without a separate yes)

Nothing in `Assets/Scripts` or `Assets/Tests` uses any namespace beyond `System*`, `UnityEngine`, `UnityEngine.UI`, `EventSystems`, `Events`, `SceneManagement`, `InputSystem`, `UnityEditor` and NUnit and the test tools. No `.aseprite`, `.psb`, tilemap, sprite shape, timeline or visual-scripting asset exists under `Assets/`.

| Package | Evidence |
|---|---|
| `com.unity.visualscripting` 1.9.13 | No graph assets and no `Unity.VisualScripting` use. |
| `com.unity.ai.inference` 2.6.1 | No `Unity.InferenceEngine` or `Sentis` use. It is probably the source of `SENTIS_ANALYTICS_ENABLED` in the Android defines (`ProjectSettings.asset:625`). |
| `com.unity.ai.assistant` 2.21.0-pre.1 | An Editor assistant. Nothing in the game uses it. |
| `com.unity.learn.iet-framework` 6.0.7 | A tutorial framework. Unused. |
| `com.unity.collab-proxy` 2.14.0 | Unity Version Control. The project uses Git. |
| `com.unity.pipeline` 0.8.0-exp.1 | Experimental. Unused. |
| `com.unity.2d.animation`, `2d.psdimporter`, `2d.spriteshape`, `2d.tilemap.extras`, `com.unity.timeline` | No matching assets or code. |
| `com.unity.2d.aseprite` 6.0.0 | No `.aseprite` under `Assets/`: the sources live in `ArtSource/` and only PNGs are imported. |
| `com.unity.ide.rider` | An IDE helper. Harmless. |

Needed and kept: `inputsystem`, `ugui`, `render-pipelines.universal`, `test-framework`, `2d.sprite`, and the built-in modules (`audio`, `ui`, `imageconversion`, `jsonserialize`, `screencapture` for the screenshots, `androidjni`).
