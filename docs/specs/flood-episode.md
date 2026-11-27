# Spec: the Flood Episode (15 Playable Levels)

Make the Flood Episode playable: fifteen Levels in the Student flow, with realistic Act-outs, Floodwater, Rain and Dark Sky, as settled in the Flood planning interview on 2026-11-27. Read `AGENTS.md`, then [the Flood design](../design/flood-episode.md) (F1 to F7, the Level tables, the Act-outs in short form and Batch 1), [ADR 0005](../adr/0005-typhoon-safety-rules.md) (it holds for Flood too), [ADR 0001](../adr/0001-v2-art-and-no-bantay.md), [ADR 0002](../adr/0002-choice-counts-per-difficulty.md), [ADR 0003](../adr/0003-licensed-music.md), [the Flood research](../research/flood-safety.md) (FL-01 to FL-20) and [the Typhoon research](../research/typhoon-safety.md) (the TY IDs Flood also cites). The Typhoon planning is the model: [the Typhoon specification](typhoon-episode.md) and `.scratch/typhoon-episode/issues/`. Terms follow [CONTEXT.md](../../CONTEXT.md): Episode, Difficulty, Level, Setting, Phase, Choice, Safe Choice, Unsafe Choice, Act-out, Caption, Action Bar, Reaction, Hint, Feedback, Grown-up, Student, Episode background, Difficulty background, Floodwater, Rain, Dark Sky, Evacuation Center and Go Bag.

Work happens on `main` in `D:\GameDEV\DisasterSafety`, with no branches or worktrees. Batch commands run with Unity 6000.6.9f1 and the Editor closed (see "Batch commands" under Further Notes; `AGENTS.md` still names the old `DisasterSafety-redesign` folder). Leave `ProjectSettings/ProjectSettings.asset` (Android symbols from batch runs), `.vscode/settings.json` and untracked screenshots out of commits.

Flood builds on Fire and Typhoon. When this spec was written, only Earthquake had Scenario files, so each Flood ticket also names the Fire and Typhoon tickets it waits for. Before writing code, read how those tickets actually named their seams (the Episode effect property, the per-Level layer fields, the content rule per Episode, the menu backgrounds) and copy that pattern; this spec names the seams by what they do.

The art is drawn by Codex in batches from [the Flood Codex art prompt](../../.scratch/flood-episode/codex-art-prompt.md) and approved by the user in its Setting before import ([art batches](../../.scratch/flood-episode/art-batches.md)). A ticket starts only after the art batch its Levels need is approved and imported.

## Problem Statement

A Learner who finishes Typhoon Hard sees Flood become Open, but Flood has no Playable Levels: its Difficulty selection and Level selection show the plain noticeboard, and Typhoon Hard 7 offers no Next Episode. Grade 3 to 6 Learners at Baluan Elementary School get no practice in what to do before, during and after a flood, although heavy rain floods low roads, yards and creeks in their area, children are drawn to play in floodwater, and dirty water, wet food and wet wiring follow every flood.

## Solution

Flood gets fifteen Playable Levels (Easy 3, Normal 5, Hard 7) that play exactly like Earthquake, Fire and Typhoon: the Learner picks a Choice in the Action Bar and the Student acts it out step by step. Instead of a shake, a Flood "during" Scenario starts with its Rain, Dark Sky and Floodwater fading in, the Teacher's warning where the design gives one, and the sound of rain. Indoors, Floodwater only seeps in under the closed front door; outdoors it covers the yard, the road or the path, still or moving. Nobody is ever drawn standing in water. Easy teaches rainy-day habits, Normal teaches staying safe during the flood at home and at school, and Hard runs from the Red Rainfall Warning to the days after the flood. Every safety line cites a finding (FL-xx, or a TY ID where the design uses one). Flood's menus show their own Episode and Difficulty backgrounds, and music Set B plays as it already does.

## User Stories

1. As a Learner, I want Flood's Level selection to list Playable Levels once Flood is Open, so that I can start a Flood Level.
2. As a Learner, I want Flood Easy, Normal and Hard to open in the same order as the other Episodes, so that the progression feels familiar.
3. As a Learner, I want Next Episode on Typhoon Hard 7's Level Completed to take me into Flood, so that I am not sent to a coming-soon message.
4. As a Learner, I want Flood's Difficulty selection and each Flood Level selection to show their own backgrounds, so that I know which Episode and Difficulty I am in.
5. As a Learner, I want Flood to play music Set B, as it already does.
6. As a Learner, I want each Easy Level to show two Choices, each Normal Level three and each Hard Level four, so that the Choices get harder with the Difficulty.
7. As a Learner, I want a Flood "during" Level to start with the rain and the Floodwater fading in, so that I see the flood come without being scared.
8. As a Learner, I want the scene never to shake in a Flood Level and never to show lightning, so that I do not mix up a flood and an earthquake and am not frightened.
9. As a Learner, I want the Action Bar to slide up once the Student looks surprised, so that I choose while the danger is clear.
10. As a Learner, I want "before" and "after" Flood Levels to start still and show the Action Bar after 1 second, even when the sky is grey, the yard is wet or mud is on the floor.
11. As a Learner, I want Floodwater to look like muddy brown-grey water with a pale sheen, so that I can tell it from a shadow, a rug or mud.
12. As a Learner, I want indoor Floodwater to show only as water seeping in under the closed front door, so that the home never looks full of water.
13. As a Learner, I want moving Floodwater to show ripple lines that run with the current, so that I can see the water is moving.
14. As a Learner, I want never to see a person standing in the water, so that the flood looks real but safe.
15. As a Learner, I want Pause to stop the rain, the Floodwater motion and the rain sound, and Resume to continue them.
16. As a Learner, I want to hear light or heavy rain under the music, with the music quieter under heavy rain, as in Typhoon.
17. As a Learner, I want the Student to act out every Choice step by step, so that I see what the action really looks like.
18. As a Learner, I want an Unsafe Choice to stop partway with "Hmm, wait..." on dry ground, before the water, a plug, the tap or the switch box, so that I learn without being frightened.
19. As a Learner, in Easy 1 I want to carry my school bag to the high shelf while Mom puts a box up, so that I learn to put things above the water.
20. As a Learner, in Easy 2 I want to put my snack wrapper in the trash bin, not the canal, so that I learn garbage blocks the water's way.
21. As a Learner, in Easy 3 I want to tip the rainwater out of the can while Dad tips the tire, so that I learn mosquitoes breed in standing water.
22. As a Learner, in Normal 1 I want to leave my ball in the floodwater and call Mom, so that I learn children stay out of floodwater.
23. As a Learner, in Normal 2 I want to step back from the water under the door and call Dad, who turns off the main switch, so that I learn power and floodwater are a Grown-up's job.
24. As a Learner, in Normal 3 I want to drink from the boiled-water jug, not the rain drum or the tap, so that I learn to drink only clean or boiled water during a flood.
25. As a Learner, in Normal 4 I want to wait with the Teacher under the roof after the Orange warning when the road past the gate is under moving water, so that I learn not to cross moving water.
26. As a Learner, in Normal 5 I want to stay home with Mom when my friends call me to see the big river, so that I learn to stay away from rivers that flood fast.
27. As a Learner, in Hard 1 I want to leave right away with Mom, Dad and the Go Bag when the TV shows a Red Rainfall Warning, so that I learn to leave before the water cuts the way.
28. As a Learner, in Hard 2 I want to take the high road with Dad when the low road is under moving water, so that I learn not to cross flooded roads or bridges.
29. As a Learner, in Hard 3 I want to walk home away from the high, fast creek even though the rain has stopped, so that I learn the creek stays dangerous.
30. As a Learner, in Hard 4 I want to wash my feet with clean water and tell Mom after they got wet in floodwater, so that I learn to wash skin that touched floodwater.
31. As a Learner, in Hard 5 I want to give the wet bread to Mom, so that I learn not to eat food that floodwater touched.
32. As a Learner, in Hard 6 I want to sit with Mom and wait for the electrician's check before the TV goes on, so that I learn power stays off until it is checked.
33. As a Learner, in Hard 7 I want to stay home with Mom when my friends want to see the flooded part of town, so that I learn not to go to flooded places.
34. As a Learner, I want every Feedback to explain kindly why a Choice is safe or unsafe, so that I understand the reason.
35. As a Learner, I want the Hint to point toward the Safe Choice without naming it, as in the other Episodes.
36. As a Learner, I want Stars, Attempts, Try Again, Pause, Restart and Quit to work exactly as in the other Episodes.
37. As a Learner, I want the Student's Safe Reaction to be "All ready!" in "before" Levels and "I'm safe!" in "during" and "after" Levels.
38. As a Learner, I want the Student, Mom, Dad and the Teacher to match the art of the other Episodes, so that the game looks like one game.
39. As a Learner, I want props the Student moved, the Grown-ups and the Floodwater to return to how they were when the Action Bar showed on Try Again.
40. As a Learner whose Earthquake, Fire and Typhoon progress is saved, I want my save to keep working, so that I lose nothing.
41. As the researcher, I want every safety line in a Flood Scenario and the Teacher's warning to cite a finding, so that the content traces back to PAGASA, DOH and the other sources.
42. As the researcher, I want the four GAP rows (Normal 2, Normal 3, Hard 3, Hard 4) to say no more than their sources support, so that weak sources are not stretched.
43. As the researcher, I want the game never to show a Safe Choice that wades, swims, crosses water, unplugs, plugs in or touches the switch box, so that the Flood rules hold.
44. As the user, I want to approve every Flood Caption, Hint, Feedback, lesson, Choice label, warning and Act-out line before it ships, so that the wording is right for Grade 3 to 6.
45. As the developer, I want a Flood Scenario to fail the content tests if its Choice count, Safe Choice, icon, action or citation is wrong, so that mistakes never reach a Learner.
46. As the developer, I want Earthquake, Fire and Typhoon Levels to keep playing exactly as before, with byte-identical screenshots, so that Flood breaks nothing.
47. As the user, I want to play each Flood Difficulty in the Unity Editor and then one Android APK, so that I can judge Flood on the phone.

## Implementation Decisions

### Scenario data

- Fifteen Scenario files in the existing format under the Flood Episode's Resources folder (`Flood/<Difficulty>/<n>`), one per Level, with `level` as `Flood/<Difficulty>/<n>`. The fields stay the ones `Scenario` already reads: `level`, `phase`, `setting`, `goal`, `intro`, `hint`, `lesson` and `choices`, and every Choice has `id`, `label`, `icon`, `action`, `safe`, an optional `heading`, `feedback` and `sources`. Every `sources` entry is an FL ID from the Flood research or a TY ID from the Typhoon research.
- The Choices, Settings and Phases are the ones in the design's Level tables, approved by the user on 2026-11-27 with the GAP rows kept. The Safe Choice is listed first only in the design; across each Difficulty it moves between slots.
- Text is drafted in the first slice of each Difficulty (Normal in ticket 01, Easy and Hard in their first tickets) and approved by the user before any Scenario file of that Difficulty is committed. The text keeps to the existing content tests: at most 12 words in a sentence, only characters the pixel font has, and no word "drop".
- GAP rows: Normal 2's Feedback says the Student steps back and tells a Grown-up, citing FL-09 for the power and TY-43 for the child's part; Normal 3 does not say tap water is unsafe, only that boiled water is safe (FL-13, TY-58); Hard 3 does not say the creek keeps rising after the rain, only to stay away from it (FL-12); Hard 4 says to wash with clean water and tell Mom (FL-18), with no soap or doctor line.

### Phase and the "during" effect

- `Scenario.Shakes` stays true only for an Earthquake "during" Scenario. A Flood "during" Scenario does not shake and starts the Flood effect instead, through a new Scenario property beside the Fire and Typhoon ones. It derives from the `level` Episode and the `phase`; there is no new JSON field. Name it after the Typhoon property.
- The Flood effect, in order: the Level's Rain, Dark Sky and Floodwater layers fade in over about 1.5 seconds; then, in Normal 4 only, the Teacher's Orange warning plays as a speech bubble with the voice blip; then the Student plays the surprised pose; then the Action Bar slides up. No lightning and no thunder. Every other "during" Level has no warning step, and the Caption tells the situation (F4).
- In Normal 2 the door seep spreads through its three steps during the fade-in and stays at step 3. The approved step 3 edge (about x 103 at the wall) is where the water stops.
- The light and heavy rain loops from Typhoon play for as long as their Rain layer shows; the music ducks under heavy rain as in Typhoon. Floodwater has no sound of its own.
- Pause stops the layers' motion and the loops; Resume continues them. Quit and Restart stop them. Try Again returns the scene to how it was when the Action Bar showed: Floodwater at its full extent, not faded out.
- **"Before" and "after" Levels:** a Flood "before" or "after" Scenario may show Rain, Dark Sky, wet ground, still Floodwater or mud from the first frame. It starts still: no fade-in, no warning bubble, no surprised pose, and the Action Bar shows after 1 second. In Hard 1 the TV shows the Red Rainfall Warning screen from the first frame and the Caption tells what it says; no warning bubble plays.
- Warning bubbles and Act-out lines are `UIStrings` lines. A line that states safety carries a comment with its finding ID, and the user approves it with the Scenario text.

### Settings and layers

- `ScenarioSettings` gains `creek`, with its action keys. It is not in `CanShake`; `CanShake` is unchanged.
- Flood Levels that reuse Settings (`home`, `kitchen`, `yard`, `street`, `schoolyard`, `street_storm`, `home_after`, `street_after` and `evacuation_area`) add their own action keys to that Setting's list. Within one Setting a key still names one Act-out, so Flood reuses an existing key only where the Act-out is the same; every other Flood Act-out gets a new key.
- Floodwater is a per-Level layer chosen by the Level's start entry on its Setting (the per-Level `LevelStart` that Fire and Typhoon extend), beside Rain and Dark Sky, which are reused from Typhoon. It is not new Setting art. Its forms:
  - **Door seep** (`home`, Normal 2): the three seep steps under the closed front door, each with two shimmer frames. It never fills a room and never reaches the Student's walk line.
  - **Window flood view** (`home`): the Typhoon storm view with low water over the yard (Normal 2) or high water (Hard 1).
  - **Still water** (`yard` in Normal 1, `street_after` in Hard 7): a shallow brown-grey sheet with a slow sheen.
  - **Moving water** (`schoolyard` gate road in Normal 4, `street_storm` road and bridge in Hard 2): ripple lines that run with the current on their own frames.
- Mud is a per-Level layer for `home_after` (Hard 5, Hard 6) and `street_after` (Hard 7).
- The `creek` Setting (Hard 3) is new art: a high, fast creek with moving water, a grassy bank, stones and a path home away from it, at the v2 bar (480 x 270, bottom-anchored, nothing the Learner must see in the top 54 rows).
- Every actor stop is on dry ground at least 24 art pixels from the nearest water pixel, as checked by each art batch's review. The placements go into `ArtSource/README-rollout.md` with each approved batch.

### Characters, poses and props

- **Grown-ups:** Mom, Dad and the Teacher, reused. No new Grown-up, no Barangay Tanod and no rescuer (F7). Poses used: Mom's `mom_reach`, `mom_happy`, `mom_sit`, `mom_walk` and `mom_walk_gobag`; Dad's `dad_idle`, `dad_walk`, `dad_switch` and `dad_kneel`; the Teacher's idle and walk.
- **Student:** a new `wash` pose (H4) for the boy and the girl. A `tip` pose (E3) only if `crouch` cannot show the can tipping. Every other Act-out reuses the existing poses.
- **Props:** the seep, the stand fan and its low outlet (N2); the bag on the high shelf and the workbook (E1, N5, H1); the TV Red Rainfall Warning screen (H1); the boiled-water jug and cup, the rain drum and the kitchen window with rain (N3); the canal grate, the trash bin and the wrapper (E2); the can and the tire with rainwater, upright and tipped (E3); the floating ball (N1); the flooded bridge (H2); the creek stones (H3); the basin and dipper (H4); the wet bread and the low cupboard (H5); the switch box states and the TV off (H6). Reused: the Go Bag, the toy, the phone, `yard_ball`, the water drum, the switch box from Fire, and the cat (`street_cat`, if it fits indoors in H5; otherwise a Batch 6 cat).
- **Icons:** one 16 x 16 icon per new Choice, about 40, named `icon_flood_<choice>` after the Typhoon icons' pattern. An existing icon is reused only where its picture matches the Choice (the Go Bag, the door, the toy and the tell-a-Grown-up icons).

### Act-outs

- Every Choice is an Act-out of steps, as in the design's Act-out table. A Safe Choice plays in full and ends with `happy` and the Safe Reaction. An Unsafe Choice stops after its stop step; the Student plays `wait` and says "Hmm, wait...".
- Unsafe stops come before the water's edge, a plug, the tap, the TV, the switch box, the canal, the creek bank or the mud, on dry ground, and away from the Safe ending. No hand touches a plug, the tap, the TV or the switch box; a `reach` toward one stops short. Hard 5's "Eat it" stops with the bread in hand, and no eating is shown.
- Act-out lines are short `UIStrings` lines that follow the Reaction rule (no judging words, no Stars), such as "Mom, my ball!", "Dad, water!" and "Mom, my feet got wet." The Teacher's Orange warning is a warning and cites its finding.
- A Grown-up's step (Mom putting a box up, Mom walking in, Dad walking to the switch box and turning off the main switch, Dad tipping the tire, Mom taking the bread) is a Grown-up Act-out step, as in the other Episodes. In Normal 2 the fan stops when Dad turns off the main switch.

### Menus and music

- `MenuBackground` gains the Flood Episode background for Flood's Difficulty selection and three Flood Difficulty backgrounds for its Level selection. Their design is chosen by the user at art Batch 7; the menus ticket names the entries after that choice.
- `EpisodeMusic` already maps Flood to Set B; nothing changes there.
- **Next Episode:** the one rule from the Typhoon spec holds: after the last Hard Level, Next Episode shows when the next Episode is Open and has a Playable Level. Typhoon Hard 7 leads into Flood once Flood has Playable Levels, and Flood Hard 7 shows no Next Episode while Landslide has none.

### Content rules

- The content test reads the Flood research notes beside the others, so an FL ID that exists there is a known finding. Every FL finding already uses the `- FL-xx:` label the finding pattern needs.
- A Flood rule test fails if a Flood Safe Choice wades, swims, crosses water, splashes, unplugs, plugs in, flips a switch or turns on the TV; and if any Flood text names the Barangay Tanod, a radio warning, a Wind Signal or Storm Surge (F1, F4, F7).

### Build order

- The shared slice first (ticket 01, after art Batch 1): the Flood effect, the Floodwater layer with the door seep and the window flood view, the Flood content rules and the Flood rule test, and Normal 2 in `home` end to end. It drafts the text of all five Normal Levels.
- Then one slice per approved art batch: home and kitchen (Easy 1, Normal 3, Normal 5, Hard 1), outdoor Floodwater (Normal 1, Normal 4, Hard 2), the creek (Hard 3), the Easy props (Easy 2, Easy 3), after the flood (Hard 4 to Hard 7), and the menus. The icons arrive with each batch.
- The user plays each Difficulty in the Editor after its last slice. One Android APK at the end.

## Testing Decisions

- Good tests check what a Learner or the content rules can observe: which Choices a Scenario offers, whether it completes, what plays, what is saved. They do not check private fields or step timings.
- **Content seam (EditMode):** `ScenarioContentTests` loads every Scenario file and runs `ScenarioRules`. They must pass for all Flood files: the exact Choice count per Difficulty (ADR 0002), one Safe Choice, every `icon` has art, every `action` exists in its Setting, every source is an FL or TY finding that exists in the research notes, every character is in the pixel font and every sentence has at most 12 words. Add a Level-exists test for the fifteen Flood Levels and a test per Level that its Choice ids and Hint match the approved text, as the Earthquake tests do. The Flood rule test lives here too.
- **Phase seam (EditMode):** `ScenarioTests` checks that a Flood "during" Scenario does not shake and starts the Flood effect, that Earthquake, Fire and Typhoon "during" Scenarios keep their own effects, and that `creek` is a known Setting that cannot shake.
- **Menu seam (EditMode):** `MenuNavigatorTests` checks the Flood Episode background on Difficulty selection and the three Flood Difficulty backgrounds on Level selection, with the other Episodes' unchanged, and that Flood Levels are listed as Playable. `LevelCompletionTests` checks that Typhoon Hard 7 offers Next Episode into Flood once Flood has a Playable Level and that Flood Hard 7 offers none while Landslide has none.
- **Level flow seam (PlayMode):** `LevelFlowPlayTests` (with `ActOutPlayTests` and `GrownUpPlayTests` where they fit): per Flood Level, the Safe Choice completes it; one Unsafe Choice shows Feedback, and Try Again restores the Student, the props, the Grown-ups and the Floodwater. The Flood effect starts in a Flood "during" Level, and its layers and loops stop on Pause and continue on Resume. In Normal 2 the seep ends at step 3 and the fan stops after Dad's switch step.
- **Screenshots:** each slice renders its Levels at 1920 x 1080 and 2400 x 1080 through the existing Level screenshot tool (`LevelScreenshots`, run by the screenshot batch), with every Safe ending and Unsafe stop, and checks they are clear of the Caption, Hint and Pause and that no actor stands in or within 24 art pixels of water. Every Earthquake, Fire and Typhoon screenshot stays byte-identical.
- Prior art: `ScenarioContentTests`, `ScenarioTests`, `MenuNavigatorTests`, `LevelCompletionTests` and `LevelFlowPlayTests`, and the tests the Fire and Typhoon tickets add for their effects, layers and Episode rules.

## Out of Scope

- Landslide. A Flood Level names landslides nowhere.
- Wind, Wind Signals, Storm Surge, the Barangay Tanod and radio warnings (F1, F4, F7); lightning, thunder and sirens; phone alerts.
- Floodwater inside a room beyond the door seep, any person in water, rescues, boats and anyone in danger.
- A flowing-water sound or any new sound; Flood reuses the Typhoon rain loops.
- Filipino Captions or warnings ("baha"); English only, as in Typhoon.
- Real General Santos flood zones, evacuation centers or hotlines.
- Any change to Stars, Attempts, Hints, Open and Locked rules, or Saved Progress.
- Changes to Earthquake, Fire and Typhoon Levels, except adding Flood action keys to shared Settings.

## Further Notes

- The user approved the Level list on 2026-11-27 with the GAP rows kept on their weaker sources, and settled F1 to F7 the same day. The user must still approve every Flood text before the Scenario files of a slice are committed.
- At 2400 x 1080 Level art is bottom-anchored and the top 54 rows are cropped. The TV screen text, the Teacher's bubble and the Floodwater edge must stay readable at both sizes.
- Floodwater must read as water at phone size, not as a stain. The Batch 1 draft read as a dark stain and its fan blades were pale; Codex fixes both before approval.
- The national DOH site was blocked by a bot check during research, and no way around it was tried. Hard 4's soap and doctor advice stays out until a source is read.
- Open questions still standing: the wording of every text (drafted per Difficulty); the Flood menu backgrounds (art Batch 7); whether `street_cat` fits indoors for Hard 5.
- Fire and Typhoon seams this spec builds on, by ticket: Fire 01 (the Episode-specific "during" effect and `Shakes` for Earthquake only), Fire 03 (per-Level layers on `LevelStart`), Fire 06 (`kitchen`), Fire 10 (`yard`), Fire 12 (`street` and `street_after`); Typhoon 01 (Rain, Dark Sky and the rain loops), Typhoon 02 (the one Next Episode rule), Typhoon 03 and 05 (the `home` window views), Typhoon 08 (`street_storm`).

### Batch commands

Close the Unity Editor first. The project folder is `D:/GameDEV/DisasterSafety`.

```
"C:\Program Files\Unity\Hub\Editor\6000.6.9f1\Editor\Unity.exe" -batchmode -quit -projectPath D:/GameDEV/DisasterSafety -executeMethod DisasterSafety.EditorTools.ProjectBuilder.BuildAllBatch -logFile build.log
"C:\Program Files\Unity\Hub\Editor\6000.6.9f1\Editor\Unity.exe" -batchmode -projectPath D:/GameDEV/DisasterSafety -runTests -testPlatform EditMode -testResults editmode.xml -logFile editmode.log
"C:\Program Files\Unity\Hub\Editor\6000.6.9f1\Editor\Unity.exe" -batchmode -projectPath D:/GameDEV/DisasterSafety -runTests -testPlatform PlayMode -testResults playmode.xml -logFile playmode.log
"C:\Program Files\Unity\Hub\Editor\6000.6.9f1\Editor\Unity.exe" -batchmode -projectPath D:/GameDEV/DisasterSafety -executeMethod DisasterSafety.EditorTools.MenuScreenshots.CaptureBatch -shotsDir D:/GameDEV/DisasterSafety/Screenshots -logFile shots.log
"C:\Program Files\Unity\Hub\Editor\6000.6.9f1\Editor\Unity.exe" -batchmode -projectPath D:/GameDEV/DisasterSafety -buildTarget Android -executeMethod DisasterSafety.EditorTools.ProjectBuilder.BuildAndroidBatch -logFile android.log
```
