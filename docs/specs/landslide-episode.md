# Spec: the Landslide Episode (15 Playable Levels) and the end of the game

Make the Landslide Episode playable: fifteen Levels in the Student flow, with realistic Act-outs, Slide Signs, Fallen Soil, Muddy Runoff, Rain and Dark Sky, and the Safety Reminder that ends the game, as settled in the Landslide planning interview on 2026-11-27. Read `AGENTS.md`, then [the Landslide design](../design/landslide-episode.md) (L1 to L10, the Level tables, the Act-outs in short form and Batch 1), [ADR 0006](../adr/0006-landslide-safety-rules.md) (how a landslide is shown), [ADR 0005](../adr/0005-typhoon-safety-rules.md) (it holds for Landslide too), [ADR 0001](../adr/0001-v2-art-and-no-bantay.md), [ADR 0002](../adr/0002-choice-counts-per-difficulty.md), [ADR 0003](../adr/0003-licensed-music.md), [the Landslide research](../research/landslide-safety.md) (LS-01 to LS-29), and the Earthquake, Typhoon and Flood research for the EQ, TY and FL IDs Landslide also cites. The Flood planning is the model: [the Flood specification](flood-episode.md) and `.scratch/flood-episode/issues/`. Terms follow [CONTEXT.md](../../CONTEXT.md): Episode, Difficulty, Level, Setting, Phase, Choice, Safe Choice, Unsafe Choice, Act-out, Caption, Action Bar, Reaction, Hint, Feedback, Grown-up, Student, Barangay Tanod, Episode background, Difficulty background, Slope, Slide Sign, Fallen Soil, Muddy Runoff, Rain, Dark Sky, Evacuation Center, Go Bag and Safety Reminder.

Work happens on `main` in `D:\GameDEV\DisasterSafety`, with no branches or worktrees. Batch commands run with Unity 6000.6.9f1 and the Editor closed (see "Batch commands" under Further Notes; `AGENTS.md` still names the old `DisasterSafety-redesign` folder). Leave `ProjectSettings/ProjectSettings.asset` (Android symbols from batch runs), `.vscode/settings.json` and untracked screenshots out of commits.

Landslide builds on Fire, Typhoon and Flood. When this spec was written, only Earthquake had Scenario files, so each Landslide ticket also names what it waits for. Before writing code, read how the Fire, Typhoon and Flood tickets actually named their seams (the Episode effect property, the per-Level layer fields, the content rule per Episode, the menu backgrounds, the one Next Episode rule) and copy that pattern; this spec names the seams by what they do.

The art is drawn by Codex in batches from [the Landslide Codex art prompt](../../.scratch/landslide-episode/codex-art-prompt.md) and approved by the user in its Setting before import ([art batches](../../.scratch/landslide-episode/art-batches.md)). A ticket starts only after the art batch its Levels need is approved and imported.

## Problem Statement

A Learner who finishes Flood Hard sees Landslide become Open, but Landslide has no Playable Levels: its Difficulty selection and Level selection show the plain noticeboard, and Flood Hard 7 offers no Next Episode. Grade 3 to 6 Learners at Baluan Elementary School get no practice in the signs of a landslide or in what to do before, during and after one, although General Santos has landslide-prone upland barangays, heavy rain and the June 2026 earthquake loosen slopes, and children play on and dig into slopes. And when a Learner finishes the last Episode, the game simply stops: there is no ending and no reminder of what all five Episodes taught.

## Solution

Landslide gets fifteen Playable Levels (Easy 3, Normal 5, Hard 7) that play exactly like the other Episodes: the Learner picks a Choice in the Action Bar and the Student acts it out step by step. Instead of a shake, a Landslide "during" Scenario starts with Rain and Dark Sky fading in with a Slide Sign or Muddy Runoff, the Tanod's or the Teacher's warning where the design gives one, and the sound of rain. The landslide is real enough to teach and never frightening: the game shows Slide Signs and Fallen Soil at rest, and a slide moves only once, far away across the valley in Hard 2. Nobody stands on or below a Slope. Easy teaches the Slide Signs and Slope habits, Normal teaches staying safe in heavy rain at home, on the road and at school, and Hard runs from the Tanod's evacuation call to after the landslide, ending with moving away from the Slope after an earthquake. Every safety line cites a finding (LS-xx, or an EQ, TY or FL ID where the design uses one). Landslide's menus show their own backgrounds, and music Set A plays as it already does. The first time the Learner completes Landslide Hard, Level Completed says the game is finished and opens a one-page Safety Reminder with one cited Tip from each Episode.

## User Stories

1. As a Learner, I want Landslide's Level selection to list Playable Levels once Landslide is Open, so that I can start a Landslide Level.
2. As a Learner, I want Landslide Easy, Normal and Hard to open in the same order as the other Episodes, so that the progression feels familiar.
3. As a Learner, I want Next Episode on Flood Hard 7's Level Completed to take me into Landslide, so that I am not sent to a coming-soon message.
4. As a Learner, I want Landslide's Difficulty selection and each Landslide Level selection to show their own backgrounds, so that I know which Episode and Difficulty I am in.
5. As a Learner, I want Landslide to play music Set A, as it already does.
6. As a Learner, I want each Easy Level to show two Choices, each Normal Level three and each Hard Level four, so that the Choices get harder with the Difficulty.
7. As a Learner, I want a Landslide "during" Level to start with the rain and the sign of danger fading in, so that I see the danger come without being scared.
8. As a Learner, I want the scene never to shake in a Landslide Level, even in the earthquake Level, so that I am not frightened and do not mix up the shaking and the slide.
9. As a Learner, I want the Action Bar to slide up once the Student looks surprised, so that I choose while the danger is clear.
10. As a Learner, I want "before" and "after" Landslide Levels to start still and show the Action Bar after 1 second, even when the sky is grey or Fallen Soil lies across the path.
11. As a Learner, I want a crack, a leaning tree and muddy water from the ground to look like what they are, so that I can spot Slide Signs in real life.
12. As a Learner, I want Fallen Soil to look like soil that came down and stopped, not like a path or a shadow, so that I understand what a landslide leaves.
13. As a Learner, I want to see a landslide only far away, so that I learn what the signs warn about without being frightened.
14. As a Learner, I want never to see a person on, below or near a Slope, in Fallen Soil or in Muddy Runoff, so that the landslide looks real but safe.
15. As a Learner, I want Pause to stop the rain, the spring, the runoff and the rain sound, and Resume to continue them.
16. As a Learner, I want to hear light or heavy rain under the music, with the music quieter under heavy rain, as in Typhoon and Flood.
17. As a Learner, I want the Student to act out every Choice step by step, so that I see what the action really looks like.
18. As a Learner, I want an Unsafe Choice to stop partway with "Hmm, wait..." on flat ground, before the Slope, the crack, the water, the soil or the rope, so that I learn without being frightened.
19. As a Learner, in Easy 1 I want to walk to Dad and tell him about the new crack in the ground, so that I learn to tell a Grown-up about a Slide Sign.
20. As a Learner, in Easy 2 I want to take soil from the garden box, not dig into the Slope, so that I learn digging weakens a Slope.
21. As a Learner, in Easy 3 I want to leave my ball past the LANDSLIDE ZONE sign and go to Mom, so that I learn to stay out of a roped-off landslide zone.
22. As a Learner, in Normal 1 I want to go in and tell Mom when muddy water comes out of the Slope, so that I learn a muddy spring is a Slide Sign.
23. As a Learner, in Normal 2 I want to walk to the house, away from the coconut tree that leans more in the rain, so that I learn leaning trees are a Slide Sign.
24. As a Learner, in Normal 3 I want to wait with the Teacher for Mom when classes stop and the hill road may slide, so that I learn the school keeps me safe when the way home is not.
25. As a Learner, in Normal 4 I want to go back the long way with Dad when muddy water runs over the road below the hill, so that I learn not to cross mud on a road.
26. As a Learner, in Normal 5 I want to play on the flat yard when my friends want to slide down the wet Slope, so that I learn the ground stays dangerous after the rain stops.
27. As a Learner, in Hard 1 I want to leave right away with Mom, Dad and the Go Bag when the Tanod calls through the megaphone, so that I learn to evacuate when the barangay says.
28. As a Learner, in Hard 2 I want to keep walking with the Tanod on the evacuation route when soil slides down a far hill, so that I learn to stay on the route and not stop or take a shortcut.
29. As a Learner, in Hard 3 I want to wait at the Evacuation Center until the Tanod says it is safe, even though the rain stopped, so that I learn to go home only when officials say.
30. As a Learner, in Hard 4 I want to stay back with Mom and leave my toy in the Fallen Soil, so that I learn to keep away from where a landslide happened.
31. As a Learner, in Hard 5 I want to stay with Mom while Dad clears the mud, so that I learn children stay out of the cleanup.
32. As a Learner, in Hard 6 I want to tell Dad about the new crack in the wall, so that I learn a crack in the house is a Slide Sign too.
33. As a Learner, in Hard 7 I want to go to the open yard with Mom, away from the Slope, after the shaking stops, so that I learn earthquakes can loosen Slopes.
34. As a Learner, I want every Feedback to explain kindly why a Choice is safe or unsafe, so that I understand the reason.
35. As a Learner, I want the Hint to point toward the Safe Choice without naming it, as in the other Episodes.
36. As a Learner, I want Stars, Attempts, Try Again, Pause, Restart and Quit to work exactly as in the other Episodes.
37. As a Learner, I want the Student's Safe Reaction to be "All ready!" in "before" Levels and "I'm safe!" in "during" and "after" Levels.
38. As a Learner, I want the Student, Mom, Dad, the Teacher and the Barangay Tanod to match the art of the other Episodes, so that the game looks like one game.
39. As a Learner, I want props the Student moved and the Grown-ups to return to how they were when the Action Bar showed on Try Again, and the far hill in Hard 2 to keep its still scar.
40. As a Learner, the first time I complete every Landslide Hard Level, I want Level Completed to say the game is finished and offer Finish, so that the game has an ending.
41. As a Learner, I want Finish to open a Safety Reminder with my Display Name, my Student, the five Episode emblems and one Tip from each Episode, so that I remember what the whole game taught.
42. As a Learner, I want Main Menu on the Safety Reminder to take me back to the Main Menu, so that I can keep playing any Level.
43. As a Learner, after the first time, I want Landslide Hard Levels to end with Replay, Levels and Main Menu and no Next Episode, so that I am not sent nowhere.
44. As a Learner whose progress in the other Episodes is saved, I want my save to keep working, so that I lose nothing.
45. As the researcher, I want every safety line in a Landslide Scenario, every warning and every Safety Reminder Tip to cite a finding, so that the content traces back to PHIVOLCS, MGB, DILG and the other sources.
46. As the researcher, I want the six GAP rows (Easy 1, Easy 2, Easy 3, Normal 4, Normal 5, Hard 5) to say no more than their sources support, so that weak sources are not stretched.
47. As the researcher, I want the earthquake action in Hard 7 to stay "Duck, Cover, and Hold" and the "get under a sturdy table" advice for slides never to appear, so that ADR 0006 holds.
48. As the user, I want to approve every Landslide Caption, Hint, Feedback, lesson, Choice label, warning, Act-out line and Safety Reminder Tip before it ships, so that the wording is right for Grade 3 to 6.
49. As the developer, I want a Landslide Scenario to fail the content tests if its Choice count, Safe Choice, icon, action or citation is wrong, so that mistakes never reach a Learner.
50. As the developer, I want the game-finished rule in the gameplay core with its own tests, so that the ending cannot show at the wrong time.
51. As the developer, I want Earthquake, Fire, Typhoon and Flood Levels to keep playing exactly as before, with byte-identical screenshots, so that Landslide breaks nothing.
52. As the user, I want to play each Landslide Difficulty in the Unity Editor and then one Android APK, so that I can judge Landslide and the ending on the phone.

## Implementation Decisions

### Scenario data

- Fifteen Scenario files in the existing format under the Landslide Episode's Resources folder (`Landslide/<Difficulty>/<n>`), one per Level, with `level` as `Landslide/<Difficulty>/<n>`. The fields stay the ones `Scenario` already reads: `level`, `phase`, `setting`, `goal`, `intro`, `hint`, `lesson` and `choices`, and every Choice has `id`, `label`, `icon`, `action`, `safe`, an optional `heading`, `feedback` and `sources`. Every `sources` entry is an LS ID from the Landslide research or an EQ, TY or FL ID from the other research notes.
- The Choices, Settings and Phases are the ones in the design's Level tables, approved by the user on 2026-11-27 with the GAP rows kept. The Safe Choice is listed first only in the design; across each Difficulty it moves between slots.
- Text is drafted in the first slice of each Difficulty (Normal in ticket 01, Easy and Hard in ticket 02) and approved by the user before any Scenario file of that Difficulty is committed. The text keeps to the existing content tests: at most 12 words in a sentence, only characters the pixel font has, and no word "drop".
- GAP rows: the child's action is worded as telling a Grown-up or staying back, citing the sourced rule (LS-10, LS-12, LS-14, LS-19, LS-23, LS-28) and never claiming a source says what children must do. Normal 4 cites LS-19 for keeping away from damaged roads and TY-52; Hard 5 cites LS-28 and TY-59.
- Hard 7's Caption says the shaking stopped and the Student did "Duck, Cover, and Hold"; it is an "after" Scenario.

### Phase and the "during" effect

- `Scenario.Shakes` stays true only for an Earthquake "during" Scenario. A Landslide "during" Scenario does not shake and starts the Landslide effect instead, through a new Scenario property beside the Fire, Typhoon and Flood ones. It derives from the `level` Episode and the `phase`; there is no new JSON field. Name it after the Flood property.
- The Landslide effect, in order: the Level's Rain, Dark Sky and Landslide layers fade in over about 1.5 seconds; in Hard 2 the far slide then plays once (6 frames at 150 ms) and holds its last frame; then, in Normal 3 only, the Teacher's warning plays, and in Hard 2 the Tanod's line plays, as speech bubbles with the voice blip; then the Student plays the surprised pose; then the Action Bar slides up. No lightning, no thunder, no rumble.
- In Normal 1 the spring runs from clear to muddy during the fade-in and keeps its muddy loop.
- The light and heavy rain loops from Typhoon play for as long as their Rain layer shows; the music ducks under heavy rain as in Typhoon. Slide Signs, Fallen Soil and Muddy Runoff have no sound.
- Pause stops the layers' motion and the loops; Resume continues them. Quit and Restart stop them. Try Again returns the scene to how it was when the Action Bar showed: layers at their full extent and the far slide on its last frame (it does not replay within an Attempt).
- **"Before" and "after" Levels:** a Landslide "before" or "after" Scenario may show Rain, Dark Sky, the wet Slope, a crack or Fallen Soil from the first frame. It starts still and the Action Bar shows after 1 second. In Hard 1 the Tanod's megaphone line plays as a speech bubble once the scene is still, before the Action Bar; in Hard 3 the Tanod's "safe to go home" line belongs to the Safe Act-out.
- Warning bubbles and Act-out lines are `UIStrings` lines. A line that states safety carries a comment with its finding ID, and the user approves it with the Scenario text.

### Settings and layers

- `ScenarioSettings` gains `hillside` and `hill_road`, with their action keys. Neither is in `CanShake`; `CanShake` is unchanged.
- Landslide Levels that reuse Settings (`classroom`, `evacuation_area` and `home`) add their own action keys to that Setting's list. Within one Setting a key still names one Act-out, so Landslide reuses an existing key only where the Act-out is the same; every other Landslide Act-out gets a new key.
- The Landslide layers are per-Level layers chosen by the Level's start entry on its Setting, beside Rain and Dark Sky, which are reused from Typhoon with a new Dark Sky tint per new Setting. They are not new Setting art:
  - **Slide Signs** (`hillside`): the crack (Easy 1), the muddy spring (Normal 1, 4 frames), the leaning tree (Normal 2), the wet Slope (Normal 5).
  - **Fallen Soil** (`hillside` in Hard 4 and Hard 5; `hill_road` in Normal 4): still.
  - **Muddy Runoff** (`hill_road`, Normal 4): 4 frames of current lines.
  - **Far slide** (`hill_road`, Hard 2): 6 frames, played once, then held.
  - **Home** (Hard 6): the wall crack and the hill window view.
- The rope and the LANDSLIDE ZONE sign (Easy 3, Hard 4, Hard 5) are props on `hillside`.
- `hillside` and `hill_road` are new art at the v2 bar (480 x 270, bottom-anchored, nothing the Learner must see in the top 54 rows).
- Every actor start, stop and ending is on flat ground at least 24 art pixels from the nearest hazard pixel (the Slope and its foot, the path up it, a crack, the spring, Muddy Runoff, Fallen Soil, the rope), as checked by each art batch's review. The placements go into `ArtSource/README-rollout.md` with each approved batch.

### Characters, poses and props

- **Grown-ups:** Mom, Dad, the Teacher and the Barangay Tanod, reused, plus the new `dad_shovel` pose (Hard 5). No rescuer, no neighbour drawn. Poses used include `mom_idle`, `mom_walk`, `mom_walk_gobag`, `mom_umbrella`, `mom_sit`, `dad_idle`, `dad_walk`, `teacher_idle`, `tanod_megaphone`, `tanod_walk` and `tanod_idle`.
- **Student:** every Act-out reuses the existing poses. No new Student pose.
- **Props:** the garden box and pot (Easy 2), the rope and the LANDSLIDE ZONE sign (Easy 3, Hard 4, Hard 5), the toy in the soil (Hard 4), the shovel and pail (Hard 5), the banana leaf (Normal 5, if drawn), the rolled poster (Hard 6). Reused: `yard_ball`, `home_toy`, the Go Bag.
- **Icons:** one 16 x 16 icon per new Choice, about 45, named `icon_landslide_<choice>`. An existing icon is reused only where its picture matches the Choice.

### Act-outs

- Every Choice is an Act-out of steps, as in the design's Act-out table. A Safe Choice plays in full and ends with `happy` and the Safe Reaction. An Unsafe Choice stops after its stop step; the Student plays `wait` and says "Hmm, wait...".
- Unsafe stops come before the Slope's foot, the path up the Slope, a crack, the spring, Muddy Runoff, Fallen Soil, the rope, the wall crack or the door, on flat ground, each at its own x, and away from the Safe ending. No hand touches a crack, the soil or the water; a `reach` stops short. No Act-out shows a Student holding a shovel in the mud.
- Act-out lines are short `UIStrings` lines that follow the Reaction rule (no judging words, no Stars), such as "Dad, a crack!", "Mom, muddy water!" and "Dad, a new crack!". The Tanod's and the Teacher's warnings are warnings and cite their findings.
- A Grown-up's step (Mom coming to the door, the family walking off behind the Tanod, Dad shovelling, Mom walking in with the umbrella, the Tanod saying it is safe) is a Grown-up Act-out step, as in the other Episodes.

### Menus, music and Next Episode

- `MenuBackground` gains the Landslide Episode background for Landslide's Difficulty selection and three Landslide Difficulty backgrounds for its Level selection. Their design is chosen by the user at art Batch 5; the menus ticket names the entries after that choice.
- `EpisodeMusic` already maps Landslide to Set A; nothing changes there.
- **Next Episode:** the one rule from the Typhoon spec holds: after the last Hard Level, Next Episode shows when the next Episode is Open and has a Playable Level. Flood Hard 7 leads into Landslide once Landslide has Playable Levels. Landslide is the last Episode, so a Landslide Hard Level never offers Next Episode.

### The end of the game

- **Core rule:** `LevelCompletion` sets a new `CompletionResult` flag (named in the ticket, for example `GameFinished`) when a completion makes every Landslide Hard Level a Completed Level for the first time, which is the same moment `EpisodeCompleted` is set for Landslide. Later completions of Landslide Hard Levels leave it false. No other Episode sets it. It is plain C# in the gameplay core with EditMode tests, beside the existing unlock rules; nothing new is saved, because the rule reads the Completed Levels.
- **Level Completed:** when the flag is set, Level Completed shows a "Game finished" ribbon and a Finish button in the Next position. Otherwise a Landslide Hard Level shows Replay, Levels and Main Menu, as the last Level of a Difficulty with no next Difficulty does.
- **Safety Reminder:** Finish opens a one-page Safety Reminder screen: its background (art Batch 6), the Student with the Display Name, the five Episode emblems in Episode order, one short Tip per Episode under its emblem, and a Main Menu button. The five Tips are `UIStrings` lines, each with a finding ID comment (an EQ, FI, TY, FL or LS ID), at most 12 words, pixel-font characters only, no word "drop", and approved by the user. The Student's line on the page follows the Reaction rule. Pause and Hint do not appear on it.
- The screen is built by the project builder like the other menus; the `.unity` files stay build output.

### Content rules

- The content test reads the Landslide research notes beside the others, so an LS ID that exists there is a known finding. Every LS finding already uses the `- LS-xx:` label the finding pattern needs.
- A Landslide rule test fails if a Landslide Safe Choice climbs, digs, slides, crosses mud, goes under the rope, goes up the path, picks something out of the soil or takes a shovel; if any Landslide text names a TV or radio warning, a Wind Signal, Storm Surge or Floodwater; if any text says to get under a table or curl into a ball in a landslide; and if an earthquake action other than "Duck, Cover, and Hold" appears (ADR 0006).

### Build order

- The shared slice first (ticket 01, after art Batch 1): the Landslide effect, the `hillside` Setting, the Landslide layers with the muddy spring, the Landslide content rules and the Landslide rule test, and Normal 1 end to end. It drafts the text of all five Normal Levels.
- Then one slice per approved art batch: the rest of Batch 1 (Easy 1 and Hard 4, drafting the Easy and Hard text), Batch 2 in two slices (Easy 2, Easy 3, Normal 2, Normal 5; then Hard 1, Hard 5, Hard 7 with `dad_shovel`), Batch 3 (`hill_road`: Normal 4 and Hard 2 with the far slide), Batch 4 (Normal 3, Hard 3, Hard 6), Batch 5 (menus and Next Episode), Batch 6 (the end of the game and the Safety Reminder). The icons arrive with each batch.
- The user plays each Difficulty in the Editor after its last slice. One Android APK at the end.

## Testing Decisions

- Good tests check what a Learner or the content rules can observe: which Choices a Scenario offers, whether it completes, what plays, what is saved, which buttons Level Completed shows. They do not check private fields or step timings.
- **Content seam (EditMode):** `ScenarioContentTests` loads every Scenario file and runs `ScenarioRules`. They must pass for all Landslide files: the exact Choice count per Difficulty (ADR 0002), one Safe Choice, every `icon` has art, every `action` exists in its Setting, every source is an LS, EQ, TY or FL finding that exists in the research notes, every character is in the pixel font, every sentence has at most 12 words, and no word "drop". Add a Level-exists test for the fifteen Landslide Levels and a test per Level that its Choice ids and Hint match the approved text, as the Earthquake tests do. The Landslide rule test lives here too.
- **Phase seam (EditMode):** `ScenarioTests` checks that a Landslide "during" Scenario does not shake and starts the Landslide effect, that Hard 7 is an "after" Scenario that does not shake, that Earthquake, Fire, Typhoon and Flood "during" Scenarios keep their own effects, and that `hillside` and `hill_road` are known Settings that cannot shake.
- **Menu and completion seam (EditMode):** `MenuNavigatorTests` checks the Landslide Episode background on Difficulty selection and the three Landslide Difficulty backgrounds on Level selection, with the other Episodes' unchanged, and that Landslide Levels are listed as Playable. `LevelCompletionTests` checks that Flood Hard 7 offers Next Episode into Landslide once Landslide has a Playable Level, and that no Landslide Level ever offers Next Episode.
- **Safety Reminder seam (EditMode and PlayMode):** `LevelCompletionTests` checks the game-finished flag: set on the completion that first finishes every Landslide Hard Level (in any order), not set on earlier Landslide Hard completions, not set on any later completion, and never set by another Episode. An EditMode test checks the five Tips: one per Episode in Episode order, each citing a finding that exists, at most 12 words, pixel-font characters only. A PlayMode test checks that Level Completed shows the ribbon and Finish when the flag is set and not otherwise, that Finish opens the Safety Reminder with the Display Name and five emblems, and that Main Menu returns to the Main Menu.
- **Level flow seam (PlayMode):** `LevelFlowPlayTests` (with `ActOutPlayTests` and `GrownUpPlayTests` where they fit): per Landslide Level, the Safe Choice completes it; one Unsafe Choice shows Feedback, and Try Again restores the Student, the props, the Grown-ups and the layers. The Landslide effect starts in a Landslide "during" Level, and its layers and loops stop on Pause and continue on Resume. In Hard 2 the far slide plays once and stays on its last frame after Try Again.
- **Screenshots:** each slice renders its Levels at 1920 x 1080 and 2400 x 1080 through the existing Level screenshot tool (`LevelScreenshots`, run by the screenshot batch), with every Safe ending and Unsafe stop, and checks they are clear of the Caption, Hint and Pause and that no actor stands within 24 art pixels of a hazard pixel. The Safety Reminder and the Level Completed ending are captured too. Every Earthquake, Fire, Typhoon and Flood screenshot stays byte-identical.
- Prior art: `ScenarioContentTests`, `ScenarioTests`, `MenuNavigatorTests`, `LevelCompletionTests` and `LevelFlowPlayTests`, and the tests the Fire, Typhoon and Flood tickets add for their effects, layers and Episode rules.

## Out of Scope

- Rainfall colour warnings on a TV or radio, Floodwater, Wind, Wind Signals and Storm Surge (L1, L4); lightning, thunder, sirens and a rumble sound; phone alerts.
- A slide near a person or a home, a person on or below a Slope, a broken or buried house, rocks in the air, rescues, rescuers and anyone in danger (ADR 0006).
- Heavy rain at night (LS-22) and the "get under a sturdy table" or "curl into a ball" advice for landslides (ADR 0006).
- Any new sound; Landslide reuses the Typhoon rain loops.
- Filipino Captions or warnings ("pagguho"); English only.
- Real General Santos hazard zones, roads, evacuation centers or hotlines; the game uses 911 (TY-65) and a fictional Evacuation Center.
- Credits, a certificate or any ending beyond the one-page Safety Reminder; any change to Stars, Attempts, Hints, Open and Locked rules, or Saved Progress.
- Changes to Earthquake, Fire, Typhoon and Flood Levels, except adding Landslide action keys to shared Settings.

## Further Notes

- The user settled L1 to L10 and approved the Level list on 2026-11-27 with the GAP rows kept on their weaker sources, and confirmed the test seams the same day: the five Flood seams plus the Safety Reminder seam, the game-finished rule as a core flag, and the far slide held still on Try Again. The user must still approve every Landslide text and the five Safety Reminder Tips before the files of a slice are committed.
- At 2400 x 1080 Level art is bottom-anchored and the top 54 rows are cropped. The top of the Slope, the coconut tree, the LANDSLIDE ZONE sign and the far slide must stay visible at both sizes.
- During research, mgb.gov.ph, denr.gov.ph and mindanews.com showed bot checks and one PHIVOLCS page was blocked by its firewall; no way around them was tried. The Slide Signs rest on MGB Region 7, the DILG-CAR L!STO Landslide manual and the Quezon City DRRMO guide.
- Open questions still standing: the wording of every text (drafted per Difficulty) and the five Safety Reminder Tips; the Landslide menu backgrounds (art Batch 5); the Safety Reminder background (art Batch 6).
- Seams this spec builds on, by what they do: the Episode-specific "during" effect with `Shakes` for Earthquake only, and per-Level layers on `LevelStart` (Fire); Rain, Dark Sky, the rain loops, the Barangay Tanod, the classroom storm windows and the Evacuation Center Hard 5 layout (Typhoon); the one Next Episode rule (Typhoon); the `home` window views, the `WATER` shades and the 24-pixel clearance check (Flood).

### Batch commands

Close the Unity Editor first. The project folder is `D:/GameDEV/DisasterSafety`.

```
"C:\Program Files\Unity\Hub\Editor\6000.6.9f1\Editor\Unity.exe" -batchmode -quit -projectPath D:/GameDEV/DisasterSafety -executeMethod DisasterSafety.EditorTools.ProjectBuilder.BuildAllBatch -logFile build.log
"C:\Program Files\Unity\Hub\Editor\6000.6.9f1\Editor\Unity.exe" -batchmode -projectPath D:/GameDEV/DisasterSafety -runTests -testPlatform EditMode -testResults editmode.xml -logFile editmode.log
"C:\Program Files\Unity\Hub\Editor\6000.6.9f1\Editor\Unity.exe" -batchmode -projectPath D:/GameDEV/DisasterSafety -runTests -testPlatform PlayMode -testResults playmode.xml -logFile playmode.log
"C:\Program Files\Unity\Hub\Editor\6000.6.9f1\Editor\Unity.exe" -batchmode -projectPath D:/GameDEV/DisasterSafety -executeMethod DisasterSafety.EditorTools.MenuScreenshots.CaptureBatch -shotsDir D:/GameDEV/DisasterSafety/Screenshots -logFile shots.log
"C:\Program Files\Unity\Hub\Editor\6000.6.9f1\Editor\Unity.exe" -batchmode -projectPath D:/GameDEV/DisasterSafety -buildTarget Android -executeMethod DisasterSafety.EditorTools.ProjectBuilder.BuildAndroidBatch -logFile android.log
```
