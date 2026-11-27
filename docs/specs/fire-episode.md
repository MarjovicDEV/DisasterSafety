# Spec: the Fire Episode (15 Playable Levels)

Make the Fire Episode playable: fifteen Levels in the Student flow, with realistic Act-outs, Smoke, Flames and the Fire Bell, as settled in the Fire design interview on 2026-11-26. Read `AGENTS.md`, then [the Fire design](../design/fire-episode.md) (F1 to F11 and the Level tables), [ADR 0004](../adr/0004-fire-safety-rules.md), [ADR 0002](../adr/0002-choice-counts-per-difficulty.md), [ADR 0003](../adr/0003-licensed-music.md) and [the Fire research](../research/fire-safety.md). Terms follow [CONTEXT.md](../../CONTEXT.md): Episode, Difficulty, Level, Setting, Phase, Choice, Act-out, Caption, Reaction, Grown-up, Smoke, Flames, Fire Bell, Crawl Low, Stop, Drop, and Roll, Meeting Place.

Work happens on `main` in `D:\GameDEV\DisasterSafety`, with no branches or worktrees. Batch commands run with Unity 6000.6.9f1 and the Editor closed. Leave `ProjectSettings/ProjectSettings.asset` (Android symbols from batch runs), `.vscode/settings.json` and untracked screenshots out of commits.

## Problem Statement

A Learner who finishes Earthquake Hard 1 sees Fire become Open, but Fire has no Playable Levels: its Level selection is an empty station noticeboard and Level Completed's Next Episode leads to a coming-soon message. Grade 3 to 6 Learners in Baluan get no practice in what to do before, during and after a fire at home, at school and in public places, although DepEd orders fire drills twice a month and fires in homes are common.

## Solution

Fire gets fifteen Playable Levels (Easy 3, Normal 5, Hard 7) that play exactly like Earthquake: the Learner picks a Choice in the Action Bar and the Student acts it out step by step. Instead of a shake, a Fire "during" Scenario starts with Smoke drifting in, the Fire Bell or a shout of "Fire!", and small Flames at one side. Easy teaches safe habits before a fire, Normal teaches escaping in the school fire drill and in home kitchen and electrical fires, and Hard covers real fires in harder places, ending with the Firefighter. Every safety line cites a Fire research finding (FI-xx), and every rule follows ADR 0004. Fire's Level selection shows its own Difficulty backgrounds and the Fire music (Set B) plays.

## User Stories

1. As a Learner, I want Fire's Level selection to list Playable Levels once Fire is Open, so that I can start a Fire Level.
2. As a Learner, I want Fire Easy, Normal and Hard to open in the same order as Earthquake's, so that the progression feels familiar.
3. As a Learner, I want Next Episode on Level Completed to take me into Fire and its Levels, so that I am not sent to a coming-soon message.
4. As a Learner, I want Fire's Difficulty selection to show the fire station, so that I know which Episode I am in.
5. As a Learner, I want Fire Easy's Level selection to show the home living room, Normal's the school corridor with the red Fire Bell box, and Hard's the barangay street at dusk with the fire truck, so that each Difficulty has its own place.
6. As a Learner, I want Fire to play its own music (Cheery Monday on Easy, B-Roll on Normal, Machinations on Hard, Piano Between on Level Completed), so that Fire sounds different from Earthquake.
7. As a Learner, I want each Easy Level to show two Choices, each Normal Level three and each Hard Level four, so that the Choices get harder with the Difficulty.
8. As a Learner, I want a Fire "during" Level to start with Smoke drifting in, the Fire Bell or a shout of "Fire!", and small Flames at one side, so that I see the danger without being scared.
9. As a Learner, I want the scene never to shake in a Fire Level, so that I do not mix up fire and earthquake.
10. As a Learner, I want the Action Bar to slide up once the Fire Bell or the shout has started, so that I choose while the danger is clear.
11. As a Learner, I want "before" and "after" Fire Levels to start still and show the Action Bar after 1 second, as in Earthquake.
12. As a Learner, I want the Student to act out every Choice step by step, so that I see what the action really looks like.
13. As a Learner, I want an Unsafe Choice to stop partway with "Hmm, wait..." and never show anyone burning or hurt, so that I learn without being frightened.
14. As a Learner, I want the Smoke to fill only the top part of the scene, so that I can see the Student is safe when staying low.
15. As a Learner, in Easy 1 I want to give the lighter on the floor to Mom, so that I learn to tell an adult about lighters and matches.
16. As a Learner, in Easy 2 I want to tell Mom to move the candle away from the curtain during a brownout, so that I learn candles need a safe place.
17. As a Learner, in Easy 3 I want to call Mom when the pot boils over, so that I learn not to handle the stove myself.
18. As a Learner, in Normal 1 I want to line up and walk out when the Fire Bell rings, and to see that ducking under the desk is for earthquakes, not fires.
19. As a Learner, in Normal 2 I want to stay low with a handkerchief to my nose and follow the Teacher through the smoky hallway, so that I learn to keep under the Smoke.
20. As a Learner, in Normal 3 I want to stay with my class at the Evacuation Area, so that the Teacher can count everyone.
21. As a Learner, in Normal 4 I want to shout "Fire!" and go out with Mom when the oil catches fire, while Mom covers the pan with its lid, so that I learn water never goes on an oil fire.
22. As a Learner, in Normal 5 I want to tell Dad and stay away from the sparking octopus wiring, so that I learn never to touch or wet an electrical fire.
23. As a Learner, in Hard 1 I want to Crawl Low out of the smoky bedroom at night, so that I learn not to hide or stand up in smoke.
24. As a Learner, in Hard 2 I want to feel the door with the back of my hand, find it hot, and use the window to the yard, so that I learn to check doors before opening them.
25. As a Learner, in Hard 3 I want to block the door gap with a blanket and wave a bright cloth at the window until Mom and the Firefighter come, so that I learn what to do when trapped.
26. As a Learner, in Hard 4 I want to Stop, Drop, and Roll with my face covered when my sleeve catches fire near the siga, so that I learn not to run.
27. As a Learner, after Hard 4 I want Feedback to tell me to cool the burn with cool water and tell an adult, so that I know what comes next.
28. As a Learner, in Hard 5 I want to follow the EXIT sign to the stairs with Mom when the mall's Fire Bell rings, so that I learn never to take the elevator.
29. As a Learner, in Hard 6 I want to tell an adult to call 911 when a neighbor's house burns, so that I learn to stay away and get help.
30. As a Learner, in Hard 7 I want to go to the friendly Firefighter, so that I learn not to hide from firefighters.
31. As a Learner, I want every Feedback to explain kindly why a Choice is safe or unsafe, so that I understand the reason.
32. As a Learner, I want the Hint to point toward the Safe Choice without naming it, as in Earthquake.
33. As a Learner, I want Stars, Attempts, Try Again, Pause, Restart and Quit to work exactly as in Earthquake.
34. As a Learner, I want the Student's Safe Reaction to be "I'm safe!" in "during" and "after" Levels and "All ready!" in "before" Levels, as in Earthquake.
35. As a Learner, I want the Student, the Grown-ups and the Firefighter to match the Earthquake art, so that the game looks like one game.
36. As a Learner, I want the Firefighter's face to be visible and friendly, so that I am not afraid of firefighters.
37. As a Learner, I want props the Student moved and Grown-ups to return to their places on Try Again, as in Earthquake.
38. As a Learner whose Earthquake progress is saved, I want my save to keep working, so that I lose nothing.
39. As a Learner, I want the Fire Bell to sound like a real school bell, so that I recognise it in a real drill.
40. As the researcher, I want every safety line in a Fire Scenario to cite an FI finding, so that the content is traceable to BFP, DepEd and NFPA sources.
41. As the researcher, I want the game never to show the Student using a fire extinguisher, taking an elevator, or putting water or a wet cloth on a fire, so that it follows ADR 0004.
42. As the researcher, I want to approve every Fire Caption, Hint, Feedback and Choice label before it ships, so that the wording is right for Grade 3 to 6.
43. As the developer, I want a Fire Scenario to fail the content tests if its Choice count, Safe Choice, icon, action or FI citation is wrong, so that mistakes never reach a Learner.
44. As the developer, I want Earthquake Levels to keep playing exactly as before, so that Fire breaks nothing.
45. As the developer, I want each new Setting and pose to come with a preview sheet next to the pilot classroom and Student, so that the art bar can be checked.
46. As the user, I want to play each Fire Difficulty in the Unity Editor and then one Android APK, so that I can judge Fire on the phone.

## Implementation Decisions

### Scenario data

- Fifteen Scenario files in the existing format under the Fire Episode's Resources folder, one per Level, with `level` as `Fire/<Difficulty>/<n>`. Every Choice has `id`, `label`, `icon`, `action`, `safe`, `feedback` and `sources`; every `sources` entry is an FI ID from the Fire research. The Choices, Settings and Phases are the ones in the design's Level tables. Text is drafted in the first slice of each Difficulty and approved by the researcher before it is committed.
- The Safe Choice is not always first in a file; across each Difficulty it moves between slots.

### Phase and the "during" effect

- `Scenario.Shakes` becomes true only for an Earthquake "during" Scenario. A new Scenario property, true for a Fire "during" Scenario, starts the Fire effect instead. Both derive from the `level` Episode and the `phase`; there is no new JSON field.
- The Fire effect, in order: the per-Level Smoke and Flames layers fade in over about 1.5 seconds, then the Fire Bell rings (school and mall Settings) or a Grown-up's or the Student's speech bubble says "Fire!" (home, yard and street Settings), the Student plays the surprised pose, and the Action Bar slides up. The Fire Bell keeps ringing quietly under the music until the Safe Act-out reaches its out-of-danger step. Music ducks under the Fire Bell as it does under the rumble.
- Pause stops the Smoke drift, the Flame frames and the Fire Bell; Resume continues them.

### Settings and layers

- `ScenarioSettings` gains `kitchen`, `home_night`, `yard`, `street` and `street_after`, each with its action keys. Fire Levels that reuse Earthquake Settings (`classroom`, `corridor`, `home`, `bedroom`, `schoolyard`, `mall_a`, `mall_b`) add their own action keys to that Setting's list. Within one Setting a key still names one Act-out, so Fire reuses an Earthquake Act-out only where the action is the same (Normal 1's "Duck under the desk" reuses the classroom `desk` Act-out); every other Fire Act-out gets a new key. `CanShake` is unchanged; the new Settings never shake.
- Smoke, Flames and Night are per-Level layers, chosen by the Level's start entry on the Setting (the per-Level `LevelStart` that the Earthquake home introduced), not by new Setting art. Smoke is a soft grey band that drifts across the top 40% of the scene in two or three loose layers. Flames are a small loop of two to four frames placed at a named point. Night is a dark blue tint with the window and lamp light left bright.
- Hard 1, 2 and 3 share the `bedroom` at night with Smoke. Hard 6 and 7 use `street` and `street_after`. Hard 5 uses `mall_a` and `mall_b` the way Earthquake Hard 3 and 4 do.

### Characters, poses and props

- A Firefighter Grown-up in the v2 style, about 62 pixels tall: helmet, turnout coat with reflective stripes, boots, mask hanging at the neck so the face shows. Poses: idle, walk, wave and kneel.
- New Student poses for the boy and the girl: `crawl` (side, 4 frames, mirrored), `stop_drop_roll` (drop, cover the face, roll: 6 frames), `feel_door` (side, back of the hand to the door: 2 frames) and `signal` (front, waving a cloth: 2 frames). Walking low with the handkerchief reuses the walk cycle with a lower head and a handkerchief prop.
- New props include the lighter, the candle, the curtain, the pot, the pan and its lid, the octopus wiring, the bag, the toy, the blanket, the bright cloth, the siga, the elevator door and the fire truck, plus a new 16x16 icon for every new Choice. Mom's lid-and-stove action in Normal 4, Dad's main-switch action in Normal 5 and the Firefighter's arrival in Hard 3 and Hard 7 are Grown-up Act-out steps.
- No step ever shows Flames on a person. In Hard 4 the sleeve shows a small flame only until the Student drops; the roll puts it out.

### Act-outs

- Every Choice is an Act-out of steps, as in Earthquake. A Safe Choice plays in full and ends with `happy` and the Safe Reaction. An Unsafe Choice stops after its stop step and plays "Hmm, wait...". Unsafe stops come before any contact with Smoke, Flames, heat, the window drop, the elevator door or the burning house, and on clear ground.
- Act-out lines are short `UIStrings` lines that follow the Reaction rule, such as "Fire!", "Mom, a lighter!", "Mom, the pot!", "Dad, sparks!", "Hot!", "Help! I'm here!" and "Please call 911!".

### Menus and music

- `MenuBackground` gains three Fire Difficulty backgrounds (home living room, school corridor with the Fire Bell box, barangay street at dusk with the fire truck) and uses them for Fire's Level selection in place of the station noticeboard stand-in.
- `EpisodeMusic` already maps Fire to Set B. The Set B recordings prepared earlier are imported and credited under ADR 0003, and the credits view lists them.
- Level Completed's Next Episode keeps its current rule; once Fire has Playable Levels the coming-soon message no longer appears. Fire Hard 7 shows no Next Episode while Typhoon has no Playable Levels.

### Sounds

- The Fire Bell is a recorded, human-made CC0 school bell, credited like the music. No synthesized alarm and no generated audio. The shout "Fire!" is a speech bubble with the voice blip, never a recorded voice.

### Build order

- One shared slice first: the Fire effect (Smoke, Flames, Night, Fire Bell), the new poses, the Firefighter, the new Settings' art and Act-out support, the Fire Difficulty backgrounds and the Set B music.
- Then one slice per Setting group in play order: Easy (home, home_night, kitchen), Normal school (classroom, corridor, schoolyard), Normal home (kitchen, home), Hard bedroom (Hard 1 to 3), Hard yard (Hard 4), Hard mall (Hard 5), Hard street (Hard 6 and 7).
- The user plays each Difficulty in the Editor after its last slice. One Android APK at the end.

## Testing Decisions

- Good tests check what a Learner or the content rules can observe: which Choices a Scenario offers, whether it completes, what plays, what is saved. They do not check private fields or step timings.
- **Content seam (EditMode):** the existing Scenario content tests load every Scenario file and run `ScenarioRules`. They must pass for all Fire files: the exact Choice count per Difficulty (ADR 0002), one Safe Choice, every `icon` has art, every `action` exists in its Setting, and every source cites an FI finding that exists in the research notes. Add one rule test for ADR 0004: no Fire Safe Choice offers an extinguisher, an elevator or water.
- **Phase seam (EditMode):** Scenario tests check that a Fire "during" Scenario does not shake and starts the Fire effect, and that an Earthquake "during" Scenario still shakes.
- **Menu seam (EditMode):** the menu navigator and menu background tests check Fire's Level selection backgrounds per Difficulty and that Fire Levels are listed as Playable.
- **Level flow seam (PlayMode):** the existing Level flow play tests start a Level, pick a Choice through the Action Bar and check the result. Add, per Fire Level, a test that the Safe Choice completes the Level, and a test that one Unsafe Choice shows Feedback and returns the Student and props on Try Again. Add tests that the Fire effect starts in a Fire "during" Level and that Pause stops it.
- **Screenshots:** each slice renders its Levels at 1920x1080 and 2400x1080 through the existing Level screenshot tool, with every Safe ending and Unsafe stop, and checks that they are clear of the Caption, Hint and Pause.
- Prior art: `ScenarioContentTests`, `ScenarioTests`, `MenuNavigatorTests`, `LevelCompletionTests` and `LevelFlowPlayTests`.

## Out of Scope

- Typhoon, Flood and Landslide.
- Any change to Stars, Attempts, Hints, Open and Locked rules, or Saved Progress.
- Smoke alarms in home Levels, a fire extinguisher used by the Student, Filipino-language Captions, and the "Sunog!" shout (pending the researcher).
- Changes to Earthquake Levels, except adding Fire's action keys to the shared Settings' lists.

## Further Notes

- The researcher must approve the Level list and then every Fire text (design F7) before the Scenario files of a slice are committed.
- At 2400x1080 Level art is bottom-anchored and the top 54 rows are cropped. The Smoke band and the Fire Bell box must stay readable at both sizes.
- The Set B WAVs are in the scratch `prepared/` folder from the music ticket. Check they are still there before the shared slice, or prepare them again from the sources in `tracks.md`.
