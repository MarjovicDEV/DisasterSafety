# Landslide Episode

Started in the Landslide planning interview on 2026-11-27. The user settled L1 to L10 on 2026-11-27, choosing a far slide that moves in Hard 2 only (L2), and approved the Level list below with its GAP rows kept on their weaker sources (L9). Landslide is the fifth and last Episode. Art is drawn by Codex in batches, and each batch is approved in its Setting before import; gameplay waits for the art and for the Fire, Typhoon and Flood tickets it builds on. Terms follow CONTEXT.md. Safety facts and sources are in docs/research/landslide-safety.md (LS-01 to LS-29), with EQ, TY and FL IDs from the other research files. The Level flow, Act-out rules, v2 art bar, Stars and Attempts from docs/design/gameplay-redesign.md hold unchanged unless this file says otherwise. ADR 0005 holds for Landslide, and ADR 0006 records the landslide rules.

## Decisions

| # | Question | Decision |
|---|---|---|
| L1 | What Landslide owns | Settled 2026-11-27. The Slope and the moving ground: Slide Signs, digging into a Slope, posted landslide zones, staying off and away from Slopes in heavy rain, the Barangay Tanod's evacuation call, the evacuation route, waiting for the Tanod to say it is safe, the Fallen Soil, the cleanup and cracks in the house. Rainfall colour warnings and Floodwater stay with Typhoon and Flood. An earthquake-triggered landslide appears once, in Hard 7: the shaking has already stopped, the Caption says the Student did "Duck, Cover, and Hold", and nothing shakes (LS-29, EQ-82). |
| L2 | How a landslide is shown | Settled 2026-11-27. Real enough to teach, never frightening. Everywhere except Hard 2 the game shows only Slide Signs (a crack, a leaning tree, a muddy spring, a crack in the wall) and Fallen Soil at rest. Hard 2 shows one **far slide**: on a far bare hill across the valley, in the back plane of `hill_road`, at most 48x40 art pixels, sliding over 6 frames at 150 ms and then still. No house, tree, road or person is on it or below it; no bouncing rocks, no dust cloud and no sound. Nobody is ever buried, hit, trapped or hurt; nothing falls toward a person; no house collapses. Every actor start, stop and ending is on flat ground at least 24 art pixels from the Slope's foot, any crack, Muddy Runoff, the spring, Fallen Soil and the rope (the Flood `feet_gap` check). No rumble sound. |
| L3 | What "during" looks like | Settled 2026-11-27. No shake (`Shakes` stays Earthquake only). Rain and Dark Sky are reused from Typhoon. New per-Level layers: the muddy spring (4 frames), Muddy Runoff over the road (4 frames), the leaning tree, Fallen Soil (still) and the far slide (Hard 2). In a "during" Scenario the layers fade in over about 1.5 seconds, the warning plays if the Level has one, the Student plays surprised, and the Action Bar slides up, as in Typhoon. "Before" and "after" Scenarios start still and show the Action Bar after 1 second. Pause stops the layers and the rain sound; Resume continues them. |
| L4 | Warnings | Settled 2026-11-27. Only the Barangay Tanod (megaphone, speech bubble; Hard 1, Hard 2, Hard 3) and the Teacher (speech bubble; Normal 3). No TV and no radio. Every warning is a speech bubble with the voice blip, never a recorded voice (LS-16, TY-19, TY-20). |
| L5 | Settings | Settled 2026-11-27. Two new Settings: `hillside` (a home at the foot of a steep grassy Slope; Easy 1 to 3, Normal 1, 2 and 5, Hard 1, 4, 5 and 7) and `hill_road` (a road cut into a hill, with a far valley and a far bare hill; Normal 4, Hard 2). Reused: `classroom` (Normal 3), `evacuation_area` (Hard 3) and `home` (Hard 6). |
| L6 | Grown-ups | Settled 2026-11-27. Mom, Dad, the Teacher and the Barangay Tanod are reused. One new pose: `dad_shovel` (Dad shovelling mud off the path, 2 frames; Hard 5). No rescuer, no neighbour drawn, nobody in danger. |
| L7 | The end of the game | Settled 2026-11-27. The first time the Learner completes every Landslide Hard Level, Level Completed shows a "Game finished" ribbon and a Finish button in the Next position. Finish opens a one-page **Safety Reminder**: the Student with the Display Name, the five Episode emblems (`Assets/Art/Episodes/episode_*.png`), one cited Tip per Episode, and a Main Menu button. Later completions of Landslide Hard Levels show no Next Episode and no Finish. The Safety Reminder needs its own art (Batch 6), and its Tips are drafted with the text. |
| L8 | Rule sources | Settled 2026-11-27. ADR 0005 holds (flashlight never a candle, 911, the 3-day Go Bag). New ADR 0006 records the landslide rules (docs/adr/0006-landslide-safety-rules.md). |
| L9 | What each Difficulty teaches | Settled 2026-11-27. Easy: Slide Signs and Slope habits. Normal: heavy rain at home, on the road and at school. Hard: from the Tanod's call to after the landslide, ending with the earthquake Level. The 15 Levels below, with the GAP rows kept. |
| L10 | Words | Settled 2026-11-27. CONTEXT.md gains Slope, Slide Sign, Fallen Soil and Muddy Runoff, and the Setting keys `hillside` and `hill_road`. |

Landslide already plays music Set A (`EpisodeMusic`) and opens after Flood by the Episode order (`Curriculum`). Once Landslide has Playable Levels, Flood Hard 7's Next Episode leads into Landslide.

## Levels

The Safe Choice is listed first here only. Choice counts follow ADR 0002: Easy 2, Normal 3, Hard 4. "GAP" marks a row whose child action has no Philippine source (see docs/research/landslide-safety.md, "Unsourced or uncertain").

### Easy: Slide Signs and Slope habits

| # | Setting + layers | Phase | Scenario | Safe Choice | Unsafe Choices | Sources |
|---|---|---|---|---|---|---|
| E1 | `hillside` + crack | before | A new crack shows in the ground near the foot of the Slope. Dad is by the house. | Tell Dad about the crack | Jump over the crack | LS-04, LS-10; GAP (what a child does) |
| E2 | `hillside` + garden box | before | The Student wants soil for a plant pot. | Take soil from the garden box | Dig into the Slope | LS-12; GAP (child digging) |
| E3 | `hillside` + LANDSLIDE ZONE sign and rope | before | The ball rolled past the "LANDSLIDE ZONE" sign. | Leave the ball there | Go under the rope | LS-14; GAP (sign from a PHIVOLCS photo; barricade rule from UNICEF Viet Nam) |

### Normal: heavy rain at home, on the road and at school

| # | Setting + layers | Phase | Scenario | Safe Choice | Unsafe Choices | Sources |
|---|---|---|---|---|---|---|
| N1 | `hillside` + Rain + Dark Sky + muddy spring | during | Muddy water starts coming out of the Slope by the path. | Go in and tell Mom | Splash in the muddy water; Go up to look | LS-06, LS-10 |
| N2 | `hillside` + Rain + Dark Sky + leaning tree | during | Walking home in the rain, the coconut tree on the Slope leans more than before. | Walk to the house, away from the Slope | Wait under the tree; Climb up for a coconut | LS-05, LS-21 |
| N3 | `classroom` + Rain + Dark Sky | during | The Teacher says classes stop, and the hill road may slide. | Wait with the Teacher for Mom | Walk home by the hill road; Wait outside at the gate | LS-18 (TY-13, TY-14), LS-19 |
| N4 | `hill_road` + Rain + Dark Sky + Muddy Runoff + Fallen Soil at the road edge | during | Walking home with Dad, muddy water runs over the road ahead. | Go back the long way with Dad | Cross the mud; Climb over the soil | LS-19, TY-52; GAP (mud over the road is a Ready.gov rule only) |
| N5 | `hillside` + wet Slope | after | The rain stopped. Friends call: "Let's slide down the Slope!" | Play on the flat yard | Slide down on a banana leaf; Climb the wet Slope | LS-23, EQ-82; GAP (no source on children playing on Slopes) |

### Hard: from the Tanod's call to after the landslide

| # | Setting + layers | Phase | Scenario | Safe Choice | Unsafe Choices | Sources |
|---|---|---|---|---|---|---|
| H1 | `hillside` + Rain + Dark Sky | before | The Barangay Tanod calls through a megaphone: "Families by the hill, leave now!" Mom has the Go Bag. | Leave now with Mom and Dad | Wait for the rain to stop; Go back for your toy; Go look at the Slope | LS-17, LS-16, TY-19, TY-51 |
| H2 | `hill_road` + light Rain + Dark Sky + far slide | during | On the evacuation route behind the Tanod, soil slides down a far hill. | Keep walking with the Tanod | Take the Slope shortcut; Stop to watch the hill; Run ahead alone | LS-11, LS-19, TY-52, LS-02 |
| H3 | `evacuation_area` + Dark Sky clearing | after | At the Evacuation Center the rain stops. A friend says, "Let's go home now." | Wait for the Tanod to say it is safe | Go home with your friend; Go check your house; Run out to play | LS-24, LS-23, TY-55 |
| H4 | `hillside` + Fallen Soil + rope | after | Soil came down across the path, far from the house. A toy lies in it. | Stay back with Mom | Climb on the soil; Get the toy from the soil; Go close to look | LS-25, LS-14, LS-23 |
| H5 | `hillside` + Fallen Soil | after | The Tanod says the path may be cleared. Dad shovels mud off it. | Stay with Mom by the house | Grab a shovel and help; Carry a pail of mud; Play in the mud | LS-28, TY-59; GAP (no Philippine source) |
| H6 | `home` + wall crack + hill window view | after | Back home, there is a new crack in the wall, and the door sticks. | Tell Dad about the crack | Hide it with a poster; Pick at the crack; Push the stuck door | LS-07, LS-27 |
| H7 | `hillside` | after | Another day, the shaking stopped. The Student did Duck, Cover, and Hold inside. Mom says, "Let's go outside." | Go to the open yard, away from the Slope | Wait by the Slope; Sit under the coconut tree; Go up the path to look | LS-29, EQ-82 |

H3 teaches the same rule as Typhoon Hard 5 (TY-55) with the landslide reason: the ground stays wet and can still slide after the rain (LS-23). The friends in N5 and the friend in H3 are named in the Caption only and are not drawn.

### Act-outs in short form (for the art layout)

"Then" separates steps. **Stop** marks where an Unsafe Choice halts; the Student then plays `wait` and says "Hmm, wait...". Every Safe Choice ends with `happy` and the Safe Reaction. No stop is on or below a Slope, on the path up the Slope, near a crack, the spring, Muddy Runoff or Fallen Soil, under the leaning tree, or past the rope: each is on flat ground at least 24 art pixels from all of them, and each Unsafe stop of a Level is at a different x. Grown-ups keep the same 24-pixel gap. The art batches record the coordinates; the spec may still adjust them.

| Level | Safe Act-out | Unsafe stops |
|---|---|---|
| E1 | Turn from the crack, walk to Dad by the house, `call` "Dad, a crack!"; Dad turns toward the crack and stays well back from it. | Jump: run-start toward the crack, stop on the near side. |
| E2 | Walk to the garden box, `crouch`, fill the pot; stand with the pot. | Dig: walk toward the foot of the Slope, `crouch`, stop on the flat. |
| E3 | Turn from the rope, walk to Mom at the door, `happy`. | Rope: walk toward the rope, `crouch`, stop before it. |
| N1 | Walk to the house door, `call` "Mom, muddy water!"; Mom comes to the door. | Splash: run-start toward the muddy water, stop. Look: walk toward the spring, stop at a different x. |
| N2 | Walk along the right side of the yard to the house door. | Tree: walk toward the leaning tree, stop short of the Slope's foot. Coconut: walk toward the tree, `reach-up`, stop on the flat at a different x. |
| N3 | Walk to the Teacher, `sit` at a desk; Mom walks in with the umbrella (`mom_umbrella`), the Student walks to her. | Hill road: walk toward the door, stop inside. Gate: walk toward the door, stop before the threshold, facing out. |
| N4 | Turn, walk back the way they came with Dad (`dad_walk`). | Mud: walk toward the Muddy Runoff, stop before it. Soil: walk toward the Fallen Soil, stop at a different x. |
| N5 | Walk to the middle of the flat yard, `happy`. | Slide: walk toward the foot of the Slope, stop short. Climb: walk toward the path up the Slope, stop before its start. |
| H1 | Walk to Mom (`mom_walk_gobag`) and Dad at the house; the family walks off right behind the Tanod (`tanod_walk`). | Rain: `sit` on the house step. Toy: walk toward the open door, stop. Look: walk toward the Slope, stop on the flat. |
| H2 | The far slide plays once while the Student plays surprised; then walk with Mom behind the Tanod to the EVACUATION ROUTE sign. | Shortcut: walk toward the path up the cut Slope, stop at its start. Watch: stop and turn to face the far hill. Ahead: run-start past the Tanod, stop. |
| H3 | `sit` on the mat by Mom; the Tanod walks in, megaphone: "It is safe to go home now." | Friend: walk toward the court edge, stop. House: walk off between the tents, stop. Play: run-start toward the open side, stop. |
| H4 | Walk back to Mom at the house door and stand with her. | Climb: walk toward the soil, stop before the rope. Toy: walk toward the toy, `crouch`, stop before the rope at a different x. Look: walk toward the rope, stop at a third x. |
| H5 | Walk to Mom by the house and stand with her; Dad (`dad_shovel`) keeps clearing the near edge of the soil. | Shovel: walk toward the spare shovel, stop. Pail: walk toward the pail, `crouch`, stop. Mud: walk toward the mud, stop before it. |
| H6 | Walk to Dad, `call` "Dad, a new crack!"; Dad turns toward the wall. | Poster: `carry` the poster toward the wall, stop. Pick: walk to the wall, `reach`, stop short. Door: walk toward the front door, `reach`, stop short. |
| H7 | Walk with Mom from the house door to the middle of the flat yard, away from the Slope, the tree and the house. | Slope: walk toward the Slope, stop on the flat. Tree: walk toward the coconut tree, stop well back from the Slope's foot. Path: walk toward the path up the Slope, stop before its start. |

H5: Dad shovels only the near edge of the Fallen Soil, after the Tanod's word (Caption), and stays at least 24 art pixels from the Slope's foot. Placement is settled in Batch 2.

## Art list

- **Settings (new):** `bg_hillside`, `bg_hill_road`, each 480x270 and bottom-anchored, with a Dark Sky tint each (`landslide_dark_sky_hillside`, `landslide_dark_sky_hill_road`). Neither shakes.
- **Layers:** the crack (E1), the muddy spring (N1, 4 frames: clear seep, then muddy, then a small muddy pool), the leaning coconut tree overlay (N2), the wet Slope (N5, darker sheen on the grass and mud), Fallen Soil on `hillside` (H4, H5) and at the `hill_road` edge (N4), Muddy Runoff over the road (N4, 4 frames), the far slide (H2, 6 frames). Rain (`typhoon_rain_light`, `typhoon_rain_heavy`) is reused.
- **Props:** garden box and plant pot (E2), "LANDSLIDE ZONE" sign and the rope on short posts (E3, H4, H5), the toy in the soil (H4), shovel and pail (H5), banana leaf (N5, optional), the wall crack and a rolled poster (H6), `home_window_hill` (H6: the Slope seen through the home window, grey sky, no rain), the EVACUATION ROUTE sign and the shortcut path on `hill_road` (H2). Reused: `yard_ball`, `home_toy`, the Go Bag, `classroom_window_storm`, `typhoon_dark_sky_evacuation_area`, `evacuation_area_*`.
- **Grown-ups:** reused `mom_*`, `dad_*`, `teacher_*`, `tanod_*`. New `dad_shovel` (2 frames).
- **Student poses:** all reused (`walk`, run-start, `call`, `crouch`, `reach`, `reach-up`, `carry`, `sit`, surprised, `wait`, `happy`).
- **Menus:** a Landslide Episode background and three Difficulty backgrounds (not yet designed; Batch 5).
- **Safety Reminder:** one 480x270 background for the end-of-game page (not yet designed; Batch 6).
- **Icons:** about 45 `icon_landslide_*`, reusing existing icons where they fit.
- **Sound:** nothing new. The Typhoon rain loops are reused. No rumble.

## Batch 1: `hillside` and its signs (Easy 1, Normal 1, Hard 4)

The batch list is in `.scratch/landslide-episode/art-batches.md` and the Codex prompt in `.scratch/landslide-episode/codex-art-prompt.md`. Batch 1 is drawn to `D:/GameDEV/landslide-episode-scratch/art/01-hillside/` and waits for approval.

`bg_hillside`: a steep grassy Slope on the left (x 0 to about 200), its top below row 54, with a dry gully, banana plants and a coconut tree on it, and its foot line near y 212; a dirt path along the foot; a narrow path up the Slope starting near x 190; a flat packed-earth yard from about x 210 to the right edge; the Student's house on the right (door near x 390, a low step). Walking line y 236. Layers: the crack on the flat ground near the foot (about x 176 to 208, y 222; E1); the muddy spring at the foot (about (140,206)) spreading to a small pool that ends near x 186 (N1); Fallen Soil at rest over the foot of the Slope and the path (about x 0 to 188), with the rope on short posts near x 204 and a toy in the soil (H4); Rain and the Dark Sky tint (N1).

Draft Act-out points (Codex records the final ones): Student start (300,236). E1: Dad at (392,236); Jump stop near (236,236); Safe walk to Dad. N1: Mom comes to the door (392,236); Splash stop near (224,236), Look stop near (244,236). H4: Mom at the door; Climb, Toy and Look stops near x 232, 244 and 256.

The preview sheet (480x270 at 4x, a 2400x1080 crop, `phone-size.png` and `motion.gif` for the spring and the rain) shows:

- The Slope reads as a steep hill, and its foot line is clear, at phone size.
- The crack reads as a crack, not a path, a shadow or a twig.
- The muddy spring reads as muddy water coming out of the ground, not as Floodwater, a shadow or a puddle.
- Fallen Soil reads as soil that came down and stopped, not as a dirt path or a shadow; it is calm, with no rocks in the air and nothing broken.
- Scale: the Student (32x50) beside Mom and Dad (36x62), against the Slope and the house.
- Hand poses: `call` faces the Grown-up; `crouch` at the rope stays behind it; nothing touches the crack, the water or the soil.
- The walk line: the start, every stop at a different x, and the smallest gap (at least 24 art pixels) to the Slope's foot, the crack, the spring, the soil and the rope, from `review.py`.
- Clearance for the Caption, Hint and Pause, and nothing the Learner must see in rows 0 to 53.
- Tone: green, calm, grey sky in N1; never scary.

## Open questions

- The Landslide Episode and Difficulty backgrounds (Batch 5) and the Safety Reminder art (Batch 6): proposals go to the user before drawing.
- The wording of every Caption, Hint, Feedback, Choice label, Reaction, Tanod and Teacher line, and the five Safety Reminder Tips, drafted per Difficulty and approved by the user before each slice is committed.
The test seams were confirmed on 2026-11-27 (the five Flood seams plus a Safety Reminder seam, the game-finished rule as a core flag, the far slide held still on Try Again). The build plan is in docs/specs/landslide-episode.md and the nine tickets in `.scratch/landslide-episode/issues/`.
