# Spec: the Typhoon Episode (15 Playable Levels)

Make the Typhoon Episode playable: fifteen Levels in the Student flow, with realistic Act-outs, Rain, Wind, Dark Sky and Wind Signal warnings, as settled in the Typhoon design interview on 2026-11-27 and approved by the researcher on the same day. Read `AGENTS.md`, then [the Typhoon design](../design/typhoon-episode.md) (T1 to T12, the Level tables, the Act-outs in short form and "Researcher review"), [ADR 0005](../adr/0005-typhoon-safety-rules.md), [ADR 0001](../adr/0001-v2-art-and-no-bantay.md), [ADR 0002](../adr/0002-choice-counts-per-difficulty.md), [ADR 0003](../adr/0003-licensed-music.md) and [the Typhoon research](../research/typhoon-safety.md) (TY-01 to TY-65). The Fire planning is the model: [the Fire specification](fire-episode.md) and `.scratch/fire-episode/issues/`. Terms follow [CONTEXT.md](../../CONTEXT.md): Episode, Difficulty, Level, Setting, Phase, Choice, Safe Choice, Unsafe Choice, Act-out, Caption, Action Bar, Reaction, Hint, Feedback, Grown-up, Student, Episode background, Difficulty background, Wind Signal, Rain, Wind, Dark Sky, Storm Surge, Evacuation Center, Go Bag and Barangay Tanod.

Work happens on `main` in `D:\GameDEV\DisasterSafety`, with no branches or worktrees. Batch commands run with Unity 6000.6.9f1 and the Editor closed (see "Batch commands" under Further Notes; `AGENTS.md` still names the old `DisasterSafety-redesign` folder). Leave `ProjectSettings/ProjectSettings.asset` (Android symbols from batch runs), `.vscode/settings.json` and untracked screenshots out of commits.

Typhoon builds on Fire. The Fire tickets were not built when this spec was written, so each Typhoon ticket also names the Fire tickets it waits for. Before writing code, read how those Fire tickets actually named their seams and copy that pattern; this spec names the seams by what they do.

## Problem Statement

A Learner who finishes Fire Hard sees Typhoon become Open, but Typhoon has no Playable Levels: its Difficulty selection and Level selection show the plain noticeboard, and Fire Hard 7's Next Episode would lead only to a coming-soon message. Grade 3 to 6 Learners at Baluan Elementary School get no practice in what to do before, during and after a typhoon, although typhoons reach Mindanao, classes are suspended by Wind Signal, and power outages and fallen wires follow most storms.

## Solution

Typhoon gets fifteen Playable Levels (Easy 3, Normal 5, Hard 7) that play exactly like Earthquake and Fire: the Learner picks a Choice in the Action Bar and the Student acts it out step by step. Instead of a shake, a Typhoon "during" Scenario starts with Rain, Wind and Dark Sky fading in, a Wind Signal warning from the TV, the radio, the Teacher or the Barangay Tanod, and the sound of rain and wind. Easy teaches getting ready before the typhoon, Normal teaches staying safe during it at school and at home, and Hard covers harder cases from evacuating before landfall to the damaged yard after the storm. Every safety line cites a Typhoon research finding (TY-xx), and every rule follows ADR 0005. Typhoon's menus show their own Episode and Difficulty backgrounds, and music Set A plays as it already does.

## User Stories

1. As a Learner, I want Typhoon's Level selection to list Playable Levels once Typhoon is Open, so that I can start a Typhoon Level.
2. As a Learner, I want Typhoon Easy, Normal and Hard to open in the same order as Earthquake's and Fire's, so that the progression feels familiar.
3. As a Learner, I want Next Episode on Fire Hard 7's Level Completed to take me into Typhoon, so that I am not sent to a coming-soon message.
4. As a Learner, I want Typhoon's Difficulty selection to show the barangay hall under a grey sky with a "SIGNAL NO. 2" board and a flag bending in the wind, so that I know which Episode I am in.
5. As a Learner, I want Typhoon Easy's Level selection to show a home doorway with the packed Go Bag and the radio, Normal's the classroom windows with rain, and Hard's a school gym Evacuation Center at night in the rain, so that each Difficulty has its own place.
6. As a Learner, I want Typhoon to play music Set A (Cherry Blossom, Happy Alley, Prelude and Action, Light Sting), as it already does.
7. As a Learner, I want each Easy Level to show two Choices, each Normal Level three and each Hard Level four, so that the Choices get harder with the Difficulty.
8. As a Learner, I want a Typhoon "during" Level to start with Rain, Wind and a Dark Sky fading in, then a Wind Signal warning, so that I see and hear the storm without being scared.
9. As a Learner, I want the scene never to shake in a Typhoon Level and never to show lightning, so that I do not mix up a typhoon and an earthquake and am not frightened.
10. As a Learner, I want the Action Bar to slide up once the warning has been given and the Student looks surprised, so that I choose while the danger is clear.
11. As a Learner, I want "before" and "after" Typhoon Levels to start still and show the Action Bar after 1 second, even when the sky is already grey or the trees sway a little.
12. As a Learner, I want nothing heavier than a leaf to fly through the air, and nothing to pass close to a person, so that the storm looks real but safe.
13. As a Learner, I want Pause to stop the rain, the wind and their sounds, and Resume to continue them.
14. As a Learner, I want to hear light rain, heavy rain on a tin roof and gusts of wind, with the music quieter under heavy rain, so that the storm sounds like the ones I know.
15. As a Learner, I want the Student to act out every Choice step by step, so that I see what the action really looks like.
16. As a Learner, I want an Unsafe Choice to stop partway with "Hmm, wait..." before any wire, wave, flame, open doorway to the storm or broken thing, so that I learn without being frightened.
17. As a Learner, in Easy 1 I want to tell Mom when the TV shows "SIGNAL NO. 1", so that I learn the family needs the news to get ready.
18. As a Learner, in Easy 2 I want to help Dad bring the pail and the chair inside, so that I learn the wind can throw light things left outside.
19. As a Learner, in Easy 3 I want to put the flashlight in the Go Bag, so that I learn it is needed when the power goes out.
20. As a Learner, in Normal 1 I want to stay with the Teacher until Mom comes when classes are suspended, so that I learn the school keeps me safe until travel is safe.
21. As a Learner, in Normal 2 I want to stay home and do my module when the TV shows "SIGNAL NO. 2", so that I learn there are no face-to-face classes for my grade at Signal No. 2.
22. As a Learner, in Normal 3 I want to leave the rattling bedroom window and go to the living room with Mom, so that I learn to stay away from windows.
23. As a Learner, in Normal 4 I want to stay inside with Mom while mangoes lie in the windy yard, so that I learn strong wind makes things fly and branches fall.
24. As a Learner, in Normal 5 I want to tell Mom and bring the Go Bag when the Barangay Tanod calls through the megaphone, so that I learn the barangay's warning is for my family.
25. As a Learner, in Hard 1 I want to walk with Mom on the evacuation route when the Tanod says to go, so that I learn to go right away on the set route.
26. As a Learner, in Hard 2 I want to go up to the Evacuation Center with Dad when the radio warns of a Storm Surge, so that I learn to move away from the coast.
27. As a Learner, in Hard 3 I want to turn on the flashlight and stay with Mom when the power is out, so that I learn never to use a candle.
28. As a Learner, in Hard 4 I want to stay inside and listen to the radio when the wind suddenly stops, and see the storm come back, so that I learn the eye of the storm is not the end.
29. As a Learner, in Hard 5 I want to wait at the Evacuation Center until the Tanod says it is safe to go home, so that I learn to wait for officials.
30. As a Learner, in Hard 6 I want to stay away from the fallen wire and tell Dad, who calls 911, so that I learn a fallen wire and the water near it can carry electricity.
31. As a Learner, in Hard 7 I want to stay with Mom while Dad checks the broken shed, so that I learn children do not do cleanup work.
32. As a Learner, I want every Feedback to explain kindly why a Choice is safe or unsafe, so that I understand the reason.
33. As a Learner, I want the Hint to point toward the Safe Choice without naming it, as in Earthquake and Fire.
34. As a Learner, I want Stars, Attempts, Try Again, Pause, Restart and Quit to work exactly as in Earthquake and Fire.
35. As a Learner, I want the Student's Safe Reaction to be "All ready!" in "before" Levels and "I'm safe!" in "during" and "after" Levels.
36. As a Learner, I want the Student, the Grown-ups and the Barangay Tanod to match the Earthquake and Fire art, so that the game looks like one game.
37. As a Learner, I want the Barangay Tanod's face to be visible and friendly, so that I trust the barangay's warning.
38. As a Learner, I want props the Student moved, the Grown-ups and the storm layers to return to their start on Try Again.
39. As a Learner whose Earthquake and Fire progress is saved, I want my save to keep working, so that I lose nothing.
40. As the researcher, I want every safety line in a Typhoon Scenario and every warning bubble to cite a TY finding, so that the content is traceable to PAGASA, DepEd, DILG, UNICEF Philippines and the international sources.
41. As the researcher, I want the game never to show the Student holding a candle or matches, taping, covering or opening a window as a Safe Choice, going outside before an all-clear, or Wind Signal No. 4 or 5, so that it follows ADR 0005.
42. As the researcher, I want to approve every Typhoon Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line before it ships, so that the wording is right for Grade 3 to 6.
43. As the developer, I want a Typhoon Scenario to fail the content tests if its Choice count, Safe Choice, icon, action or TY citation is wrong, so that mistakes never reach a Learner.
44. As the developer, I want Earthquake and Fire Levels to keep playing exactly as before, with byte-identical screenshots, so that Typhoon breaks nothing.
45. As the developer, I want each new Setting, pose, Grown-up and background to come with a preview sheet next to the pilot classroom and Student, so that the art bar can be checked.
46. As the user, I want to play each Typhoon Difficulty in the Unity Editor and then one Android APK, so that I can judge Typhoon on the phone.

## Implementation Decisions

### Scenario data

- Fifteen Scenario files in the existing format under the Typhoon Episode's Resources folder (`Assets/Resources/Scenarios/Typhoon/<Difficulty>/<n>.json`), one per Level, with `level` as `Typhoon/<Difficulty>/<n>`. The fields stay the ones `Scenario` already reads: `level`, `phase`, `setting`, `goal`, `intro`, `hint`, `lesson` and `choices`, and every Choice has `id`, `label`, `icon`, `action`, `safe`, an optional `heading`, `feedback` and `sources`. Every `sources` entry is a TY ID from the Typhoon research.
- The Choices, Settings and Phases are the ones in the design's Level tables, approved by the researcher. The Safe Choice is listed first only in the design; across each Difficulty it moves between slots.
- Text is drafted in the first slice of each Difficulty (Normal in ticket 01, Easy in ticket 03, Hard in ticket 08) and approved by the researcher before any Scenario file of that Difficulty is committed. The text keeps to the existing content tests: at most 12 words in a sentence, only characters the pixel font has, and no word "drop" (the Earthquake slogan test rejects it; write "rain" rather than "raindrop").

### Phase and the "during" effect

- `Scenario.Shakes` stays true only for an Earthquake "during" Scenario (Fire ticket 01 makes that change). A Typhoon "during" Scenario does not shake and starts the Typhoon effect instead, through a new Scenario property beside the one Fire ticket 01 adds for the Fire effect. Both derive from the `level` Episode and the `phase`; there is no new JSON field. Name the Typhoon property after the Fire one.
- The Typhoon effect, in order: the Level's Rain, Wind and Dark Sky layers fade in over about 1.5 seconds; then the warning plays as a speech bubble with the voice blip (the TV screen prop, the radio, the Teacher or the Barangay Tanod's megaphone); then the Student plays the surprised pose; then the Action Bar slides up. No lightning and no thunder. Where the design names no warning (Normal 3, Normal 4, Hard 3 and Hard 4), the warning step is left out and the Caption tells the situation.
- Where the design's Level table says only "Rain", the Level uses heavy Rain; "light Rain" is named where it applies (Hard 1 and Hard 2).
- The rain and wind loops play under the music for as long as their layers show. Light Rain plays the light rain loop, heavy Rain plays the heavy rain on a tin roof loop, and Wind plays the wind with gusts loop. The music ducks under heavy rain as it does under the rumble.
- Pause stops the layers and the loops; Resume continues them. Quit and Restart stop them.
- **Hard 4 calm step:** in Hard 4 the Rain and Wind fade out after the Action Bar shows, the Dark Sky lightens, and the loops fade with them. The Learner chooses in the calm, and every Unsafe Act-out plays in the calm. In the Safe Act-out, after the radio says "Stay inside.", the Rain and Wind fade back in with their loops. Try Again returns the scene to the calm, not to the storm, because the Action Bar is already showing.
- **"Before" and "after" Levels:** a Typhoon "before" or "after" Scenario may show Dark Sky, light Wind, light Rain or storm damage from the first frame. It starts still: no fade-in, no warning bubble, no surprised pose, and the Action Bar shows after 1 second as in Earthquake. Its light Wind and light Rain loops play quietly from the start. What the Tanod or the radio says in Hard 1 and Hard 2 is told in the Caption; the Tanod may stand in the megaphone pose, but no warning bubble plays before the Action Bar.
- Warning bubbles and Act-out lines are `UIStrings` lines. A line that states safety carries a comment with its TY ID (for example "Signal No. 2. Classes are suspended." cites TY-12), and the researcher approves it with the Scenario text.

### Settings and layers

- `ScenarioSettings` gains `street_storm` and `coast`, each with its action keys. Neither is in `CanShake`; `CanShake` is unchanged.
- Typhoon Levels that reuse Settings (`home`, `bedroom`, `yard`, `classroom`, `home_night` and `evacuation_area`) add their own action keys to that Setting's list. Within one Setting a key still names one Act-out, so Typhoon reuses an existing key only where the Act-out is the same; every other Typhoon Act-out gets a new key.
- Rain (light or heavy), Wind and Dark Sky are per-Level layers chosen by the Level's start entry on its Setting (the per-Level `LevelStart` in `LevelSetting`, which Fire ticket 03 extends with the Smoke and Flames layers). Typhoon adds its layers the same way, not as new Setting art. Night is reused from Fire ticket 08.
  - **Rain:** diagonal streaks, light (sparse, thin) or heavy (dense). Outdoors (`yard`, `street_storm`, `coast`) the streaks cover the whole scene. Indoors (`classroom`, `home`, `bedroom`, `home_night`) rain never falls in the room: it shows only in the window views, as streaks and a grey wash on the glass. It never hides the Student, the Grown-ups or a prop the Learner must see.
  - **Wind:** trees, curtains, clothes on a line and loose things sway on their own frames; small leaves blow across the top band of the scene only, never within 40 art pixels of a person. Nothing heavier than a leaf moves through the air.
  - **Dark Sky:** a grey tint on the sky and on window panes.
- Storm damage (fallen branch, fallen wire, puddle, roof sheet, broken shed, broken glass) is per-Level props on the start entry, not new Setting art.
- `street_storm` is the Fire barangay `street` redrawn: the shutters closed, no food vendor, an evacuation route sign and a path toward the creek. `coast` is a seaside road with high, rough waves behind a sea wall, steps down to the sand, a house, and a path up to an Evacuation Center sign. Both meet the v2 bar (480 x 270, bottom-anchored, with nothing the Learner must see in the top 54 rows). Their placements go into `ArtSource/README-rollout.md` beside the reused Settings.

### Characters, poses and props

- **Barangay Tanod:** a new Grown-up in the v2 style, about 36 x 62 pixels: navy shirt, cap, yellow reflective vest reading "TANOD", boots, a friendly visible face and a megaphone. Poses: idle, walk (side, 4 frames, mirrored), megaphone (front, 2 frames) and wave. Used in Normal 5, Hard 1, Hard 5 and, if room allows, Hard 6.
- **Mom:** a walk holding an umbrella (Normal 1). **Dad:** `carry` (Easy 2) and `phone` (Hard 6). The design calls Dad's `phone` a Fire pose, but no Fire ticket adds it; its source `ArtSource/GrownUp/dad_phone.aseprite` exists, so ticket 12 imports it unless a Fire ticket has. The Teacher is reused.
- **Student:** a new `flashlight` pose (front, holding a lit flashlight with a short beam, 2 frames) for the boy and the girl. Every other Act-out reuses the existing poses.
- **Props:** the TV "SIGNAL NO. 1" and "SIGNAL NO. 2" screens, the pail, the ball, the plastic chair, the school bag (Fire), the module booklet, mangoes on the grass, clothes on the line, the candle and matches on a shelf (the Fire candle, never held), the flashlight with its beam, the phone, the bike, the stick, the fallen branch, the fallen wire, the puddle, the roof sheet, the broken shed, the broken glass and the broom, the shells, the toy, Mom's umbrella, the Tanod's megaphone and the evacuation route sign. The Go Bag (open and closed), the radio (2 frames) and the flashlight are reused from Earthquake.
- **Art the tickets need beyond the design's list:** Dad's `dad_idle` (Dad has no standing pose yet), Mom's walk with the Go Bag on her back (Hard 1), a neighbour family walking out of the Evacuation Center (Hard 5), an unlit candle and a matchbox for the shelf (Hard 3), the window views for indoor Settings (storm, grey, calm, with the mangoes, the clothes and the bike seen through the `home` window), a `home` window mask for the Tanod walking past, and Dark Sky overlays per outdoor Setting.
- **Art prompt:** every new sprite is drawn first, in batches, from [the Typhoon Codex art prompt](../../.scratch/typhoon-episode/codex-art-prompt.md), and approved by the researcher before import. Each ticket then wires in the art its Levels need.
- **Icons:** one 16 x 16 icon per new Choice, about 40, named `icon_typhoon_<choice>` after the Fire icons' pattern. An existing icon is reused only where its picture matches the Choice (`icon_go_bag`, `icon_radio`, `icon_door`, `icon_watch_waves`, `icon_high_ground`, `icon_go_home`, `icon_toy_car`). There is no `icon_window` yet; the window Choices get new icons.

### Act-outs

- Every Choice is an Act-out of steps, as in the design's Act-out table. A Safe Choice plays in full and ends with `happy` and the Safe Reaction ("All ready!" in "before" Levels, "I'm safe!" in "during" and "after" Levels). An Unsafe Choice stops after its stop step; the Student plays `wait` and says "Hmm, wait...".
- Unsafe stops come before any contact with, or reach of, a wire, a wave, the sea wall, a flame, the candle or the matches, an open doorway to the storm, the window, the broken shed or the glass. They stop on clear indoor floor or clear ground, apart from the Safe ending.
- Act-out lines are short `UIStrings` lines that follow the Reaction rule (no judging words, no Stars), such as "Mom, a typhoon!", "Mom, the Tanod!" and "Dad, a wire!". The radio's "Stay inside." and the Tanod's "It is safe to go home now." are warnings and cite their findings.
- A Grown-up's step (Mom turning the radio on, Mom walking in with the umbrella, Dad carrying the chair, Dad's 911 call, the Tanod walking in) is a Grown-up Act-out step, as in Earthquake and Fire.
- Normal 4's mangoes lie on the grass under the swaying tree and are seen through the window; none falls through the air, and the door stays closed. The Caption says the mangoes are falling.

### Menus and music

- `MenuBackground` gains the Typhoon Episode background (`BarangayHall`: the barangay hall under a grey sky, a "SIGNAL NO. 2" board and a flag bending in the wind) for Typhoon's Difficulty selection, and three Typhoon Difficulty backgrounds for its Level selection: `GoBagDoorway` (Easy: a home doorway with the packed Go Bag and the radio, grey sky), `RainyWindows` (Normal: the classroom windows with rain streaming down) and `EvacuationGym` (Hard: a school gym used as an Evacuation Center at night in the rain, lights on, an "EVACUATION CENTER" sign). They are menu illustrations, distinct from the playable Settings and from Earthquake Hard's covered court.
- `EpisodeMusic` already maps Typhoon to Set A; nothing changes there.
- **Next Episode:** today `LevelCompletion` offers Next Episode only after Earthquake Hard. It becomes one rule for every Episode: after the last Hard Level, Next Episode shows when the next Episode is Open and has a Playable Level. Earthquake Hard 7 still leads into Fire, Fire Hard 7 leads into Typhoon once Typhoon has Playable Levels, and Typhoon Hard 7 shows no Next Episode while Flood has none.

### Sounds

- Three recorded, human-made CC0 loops: light rain, heavy rain on a tin roof, and wind with gusts. Candidates come from Freesound.org with the CC0 licence filter, OpenGameArt.org CC0 and Wikimedia Commons CC0 field recordings.
- The agent lists two or three candidates per loop (page URL, author, licence, length, file size) and asks the user before downloading any file. Each chosen file is checked as a real field recording (the page describes the recording; no synthesized or AI audio) and is trimmed to a seamless loop.
- The loops go under `Assets/Audio/Sfx/` and are credited like the music: in `ArtSource/audio/recorded/SOURCES.md`, the licence list and the credits view. No thunder, no siren, no recorded voice and no child voice cheers. If no candidate passes, the ticket stops and asks the user for a sound.

### Build order

- The shared slice first (ticket 01): the Typhoon effect (Rain, Wind, Dark Sky layers and the three loops), the Typhoon content rules and the ADR 0005 rule test, and Normal 1 in the `classroom` end to end. The Typhoon menus (ticket 02) can run beside it.
- Then the Setting-group slices in play order from design T12: Easy (`home`, `yard`), Normal home (`home`, `bedroom`) with the Barangay Tanod, Hard outside before (`street_storm`, `coast`), Hard home (`home_night`, `home`), Hard after (`evacuation_area`, `street_storm`, `yard`).
- The user plays each Difficulty in the Editor after its last slice. One Android APK at the end.

## Testing Decisions

- Good tests check what a Learner or the content rules can observe: which Choices a Scenario offers, whether it completes, what plays, what is saved. They do not check private fields or step timings.
- **Content seam (EditMode):** `ScenarioContentTests` loads every Scenario file and runs `ScenarioRules`. They must pass for all Typhoon files: the exact Choice count per Difficulty (ADR 0002), one Safe Choice, every `icon` has art, every `action` exists in its Setting, every source is a TY finding that exists in the research notes, every character is in the pixel font and every sentence has at most 12 words. Add a Level-exists test for the fifteen Typhoon Levels and a test per Level that its Choice ids and Hint match the approved text, as the Earthquake tests do.
- **ADR 0005 rule test (EditMode):** one test fails if a Typhoon Safe Choice lights or holds a candle or matches, tapes, covers or opens a window, or goes outside before an all-clear; and if any Typhoon text names Signal No. 4 or Signal No. 5.
- **Phase seam (EditMode):** `ScenarioTests` checks that a Typhoon "during" Scenario does not shake and starts the Typhoon effect, that an Earthquake "during" Scenario still shakes, that a Fire "during" Scenario still starts the Fire effect and not the Typhoon one, and that `street_storm` and `coast` are known Settings that cannot shake.
- **Menu seam (EditMode):** `MenuNavigatorTests` checks the Typhoon Episode background on Difficulty selection and the three Typhoon Difficulty backgrounds on Level selection, with Earthquake's and Fire's unchanged, and that Typhoon Levels are listed as Playable. `LevelCompletionTests` checks that Fire Hard 7 offers Next Episode into Typhoon once Typhoon has a Playable Level, that Typhoon Hard 7 offers none while Flood has none, and that Earthquake Hard 7 still offers Fire.
- **Level flow seam (PlayMode):** `LevelFlowPlayTests` (with `ActOutPlayTests` and `GrownUpPlayTests` where they fit): per Typhoon Level, the Safe Choice completes it; one Unsafe Choice shows Feedback, and Try Again restores the Student, the props and the Grown-ups. The Typhoon effect starts in a Typhoon "during" Level, its layers and loops stop on Pause and continue on Resume. Hard 4's Rain and Wind fade out after the Action Bar shows and return at the end of the Safe Act-out.
- **Screenshots:** each slice renders its Levels at 1920 x 1080 and 2400 x 1080 through the existing Level screenshot tool (`LevelScreenshots`, run by the screenshot batch), with every Safe ending and Unsafe stop, and checks they are clear of the Caption, Hint and Pause. Every Earthquake and Fire screenshot stays byte-identical.
- Prior art: `ScenarioContentTests`, `ScenarioTests`, `MenuNavigatorTests`, `LevelCompletionTests` and `LevelFlowPlayTests`, and the Fire tests that Fire tickets 01 and 03 add for the Fire effect and its layers.

## Out of Scope

- Flood and Landslide. Floodwater, rivers and canals, rainfall colour warnings, wading, rising water at home and drinking water belong to Flood (design T2); a Typhoon Level names the creek only as a place not to go.
- Wind Signals No. 4 and No. 5; lightning, thunder and sirens; phone alerts.
- Filipino Captions or warnings ("Bagyo", "Signal No." read aloud); the researcher kept English only.
- A rescuer Grown-up, raincoats and real General Santos evacuation centers or hotlines.
- Any change to Stars, Attempts, Hints, Open and Locked rules, or Saved Progress.
- Changes to Earthquake and Fire Levels, except adding Typhoon action keys to shared Settings and the one Next Episode rule above.

## Further Notes

- The researcher approved the Level list on 2026-11-27 with no changes (design "Researcher review"). The researcher must still approve every Typhoon text before the Scenario files of a slice are committed.
- At 2400 x 1080 Level art is bottom-anchored and the top 54 rows are cropped. The Wind leaves, the TV screen text and the Tanod's bubble must stay readable at both sizes.
- Rain and Dark Sky must keep the Caption, the Reaction, the Student and every prop the Learner must see readable. Check the heavy Rain Levels at both sizes before the screenshots are approved.
- TY-38 (windows) is written as `- TY-38 (windows):` in the research notes, so the content test's finding pattern (`- TY-xx:`) does not find it. Ticket 01 rewrites that one line's label to `- TY-38: (windows)` without changing its wording, so a Scenario can cite TY-38.
- Open questions still standing from the design: the wording of every text (drafted per Difficulty); Hard 5 rests on weak sources (TY-54, TY-55) and its text review may still change "Look for your friend alone"; whether the nearby coast is a Storm Surge area is unconfirmed (Hard 2 stays a visit to Lola).
- Fire seams this spec builds on, by Fire ticket: 01 (the Episode-specific "during" effect and `Shakes` for Earthquake only), 02 (Fire's `MenuBackground` entries), 03 (per-Level layers on `LevelStart`), 05 (`home_night`), 08 (Night layer), 09 (the bedroom door), 10 (`yard`), 12 (`street` and Fire Hard 7).

### Batch commands

Close the Unity Editor first. The project folder is `D:/GameDEV/DisasterSafety`.

```
"C:\Program Files\Unity\Hub\Editor\6000.6.9f1\Editor\Unity.exe" -batchmode -quit -projectPath D:/GameDEV/DisasterSafety -executeMethod DisasterSafety.EditorTools.ProjectBuilder.BuildAllBatch -logFile build.log
"C:\Program Files\Unity\Hub\Editor\6000.6.9f1\Editor\Unity.exe" -batchmode -projectPath D:/GameDEV/DisasterSafety -runTests -testPlatform EditMode -testResults editmode.xml -logFile editmode.log
"C:\Program Files\Unity\Hub\Editor\6000.6.9f1\Editor\Unity.exe" -batchmode -projectPath D:/GameDEV/DisasterSafety -runTests -testPlatform PlayMode -testResults playmode.xml -logFile playmode.log
"C:\Program Files\Unity\Hub\Editor\6000.6.9f1\Editor\Unity.exe" -batchmode -projectPath D:/GameDEV/DisasterSafety -executeMethod DisasterSafety.EditorTools.MenuScreenshots.CaptureBatch -shotsDir D:/GameDEV/DisasterSafety/Screenshots -logFile shots.log
"C:\Program Files\Unity\Hub\Editor\6000.6.9f1\Editor\Unity.exe" -batchmode -projectPath D:/GameDEV/DisasterSafety -buildTarget Android -executeMethod DisasterSafety.EditorTools.ProjectBuilder.BuildAndroidBatch -logFile android.log
```
