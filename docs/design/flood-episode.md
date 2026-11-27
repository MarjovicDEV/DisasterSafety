# Flood Episode

Started in the Flood planning interview on 2026-11-27. The user settled F2, then approved the Level list below with its GAP rows kept on their weaker sources (2026-11-27). The user confirmed F3 to F7 on 2026-11-27, with F4 narrowed to the TV and the Teacher and F6 gaining `kitchen`. Art is drawn in batches and each batch is approved in its Setting before import; gameplay waits for the art. Terms follow CONTEXT.md. Safety facts and sources are in docs/research/flood-safety.md (FL-01 to FL-20) and docs/research/typhoon-safety.md (TY IDs). The Level flow, Act-out rules, v2 art bar, Stars and Attempts from docs/design/gameplay-redesign.md hold unchanged unless this file says otherwise. ADR 0005 (flashlight never a candle, 911, boiled water with no time, 3-day Go Bag) also holds for Flood.

## Decisions

| # | Question | Decision |
|---|---|---|
| F1 | Typhoon or Flood | As Typhoon T2: Flood keeps the water. Rainfall colour warnings and PAGASA flood warnings, rivers and canals, wading or playing in floodwater, rising water at home, dirty water and boiling it, food the water touched, power after the flood, mud and mosquitoes. No Flood Choice is about wind, Wind Signals or Storm Surge. |
| F2 | Where Floodwater may show | Settled by the user on 2026-11-27. Indoors, Floodwater shows only as a seep under the closed front door, spreading over the floor band. It never fills a room. Outdoors it may cover the yard, the road or the schoolyard path. The Student always stands, walks and stops on dry ground; no person is ever drawn standing in water. |
| F3 | What "during" looks like | No shake. Per-Level layers: Rain and Dark Sky (reused from Typhoon) and a new **Floodwater** layer: a brown-grey band with a soft ripple. Three forms: the door seep (indoors, three spreading steps), still shallow water (yard, road), and moving water (road, schoolyard path, creek) with ripple lines showing the current. In a "during" Scenario the layers fade in, the warning plays, the Student plays surprised, and the Action Bar slides up, as in Typhoon. |
| F4 | Warnings in the game | Only two warnings. The TV gives the PAGASA Red Rainfall Warning at home (Hard 1), and the Teacher gives an Orange warning at school (Normal 4). No radio and no Barangay Tanod. Every warning is a TV screen or a speech bubble, never a recorded voice. |
| F5 | What each Difficulty teaches | Easy: rainy-day habits (things up high, no garbage in the canal, tip out rainwater). Normal: during the flood at home and school (floodwater, power, drinking water, moving water at the gate, the river). Hard: from the warning to after the flood (leave early, the flooded road, the high creek, washing after floodwater, food the water touched, power after the flood, staying away from flooded places). |
| F6 | Settings | Reuse `home`, `kitchen` (Normal 3), `yard`, `street`, `schoolyard`, `street_storm`, `home_after`, `street_after` and `evacuation_area` with the Floodwater, Rain, Dark Sky and mud layers. One new Setting, `creek` (Hard 3). |
| F7 | Grown-ups | Mom, Dad and the Teacher are reused. No new Grown-up, no Barangay Tanod and no rescuer. |

## Levels (draft)

The Safe Choice is listed first. Choice counts follow ADR 0002. "GAP" marks a row whose source is weak or missing (see docs/research/flood-safety.md, "Unsourced or uncertain").

### Easy: rainy-day habits

| # | Setting + layers | Phase | Scenario | Safe Choice | Unsafe Choices | Sources |
|---|---|---|---|---|---|---|
| E1 | `home` + Rain in the window | before | It has rained all day. Mom is moving things up high. The school bag is on the floor. | Put your bag up high | Leave it on the floor | FL-04 |
| E2 | `street` + canal grate | before | The Student has a snack wrapper beside the canal. | Put it in the trash bin | Drop it in the canal | FL-05 |
| E3 | `yard` | after | The rain stopped. A can and an old tire hold rainwater. | Tip out the water | Leave it | FL-19, TY-60 |

### Normal: during the flood, at home and at school

| # | Setting + layers | Phase | Scenario | Safe Choice | Unsafe Choices | Sources |
|---|---|---|---|---|---|---|
| N1 | `yard` + Rain + Floodwater (still) | during | The ball floated into the floodwater. | Leave it and tell Mom | Wade in for the ball; Splash in the water | FL-10 |
| N2 | `home` + Rain in the window + Floodwater (door seep) | during | Water seeps in under the door. The fan is plugged in low. | Step back and tell Dad | Unplug the fan; Wipe up the water | FL-09; GAP (child rule only in TY-43, US) |
| N3 | `kitchen` + Rain in the window | during | The Student is thirsty. | Drink from the boiled-water jug | Drink from the rain drum; Drink from the tap | FL-13, TY-58; GAP (tap during the flood) |
| N4 | `schoolyard` + Rain + Floodwater (moving) at the gate | during | The Teacher gives an Orange warning in class. The road past the gate is under moving water. | Wait with the Teacher under the roof | Cross the water; Take off your shoes and wade | TY-14, FL-11 |
| N5 | `home` + Rain in the window | during | Friends call from outside: "Let's see the big river!" | Stay home | Go to the riverbank; Go to the bridge | FL-12, FL-11 |

### Hard: from the warning to after the flood

| # | Setting + layers | Phase | Scenario | Safe Choice | Unsafe Choices | Sources |
|---|---|---|---|---|---|---|
| H1 | `home` + Rain + window flood view | before | The TV shows a Red Rainfall Warning: evacuate. The water outside is still low. | Leave now with Mom, Dad and the Go Bag | Wait until the water comes in; Go back for your toy; Finish your game | FL-08, TY-09, TY-51 |
| H2 | `street_storm` + Rain + Floodwater (moving) on the road | during | On the evacuation route with Dad, the low road ahead is under moving water. | Take the high road with Dad | Cross the water; Walk on the flooded bridge; Wade barefoot | FL-11, TY-52 |
| H3 | `creek` (new) | during | The rain stopped, but the creek is high and fast. | Walk home, away from the creek | Swim in the creek; Fish at the edge; Throw stones from the bank | FL-12; GAP (rising after the rain stops) |
| H4 | `evacuation_area` | during | The Student's feet got wet in floodwater on the way. | Wash with clean water and tell Mom | Wipe them on your shirt; Keep playing; Let them dry | FL-18; GAP (soap and seeing a doctor: national DOH blocked) |
| H5 | `home_after` + mud | after | The bread in the low cupboard got wet. | Give it to Mom | Eat it; Wipe it and eat it; Feed it to the cat | FL-14 |
| H6 | `home_after` | after | The power is off and the floor is wet. The Student wants to watch TV. | Wait for the electrician's check | Turn on the TV; Plug in the fan; Flip the switch box | FL-15 |
| H7 | `street_after` + mud | after | The water went down. Friends want to see the flooded part of town. | Stay home with Mom | Go see the flooded street; Play in the mud; Pick things out of the water | FL-17, FL-10 |

N3 uses the Fire `kitchen`, because it has the tap at the sink; `home` has none.

### Act-outs in short form (for the art layout)

"Then" separates steps. **Stop** marks where an Unsafe Choice halts; the Student then plays `wait` and says "Hmm, wait...". Every Safe Choice ends with `happy` and the Safe Reaction. No stop is in water, within 24 art pixels of water, or within reach of a plug, the tap or the switch box. The art batches record the coordinates; the spec may still adjust them.

| Level | Safe Act-out | Unsafe stops |
|---|---|---|
| E1 | Walk to the bag on the floor, `crouch`, pick it up, `carry` it to the high wall shelf, `reach-up` and set it there; Mom (`mom_reach`, putting a box up) plays `mom_happy`. | Leave it: walk past the bag toward the sofa, stop. |
| E2 | Walk to the trash bin with the wrapper, `reach`, the wrapper drops in. | Canal: walk to the canal grate, `crouch`, stop with the wrapper still in hand. |
| E3 | Walk to the can, `crouch`, the can tips and the water runs out; Dad (`dad_kneel`) tips the tire. | Leave it: walk past the can, stop. |
| N1 | Turn away from the water, `call` "Mom, my ball!"; Mom walks in from the left and stands with the Student. | Wade: walk toward the water, stop before its edge. Splash: run-start toward the water, stop at a different x before its edge. |
| N2 | Walk right, away from the water, `call` "Dad, water!"; Dad walks in from the right, plays `dad_switch` at the switch box, and the fan stops. | Unplug: walk toward the fan, `reach`, stop far from the plug. Wipe: walk toward the water, `crouch`, stop on dry floor. |
| N3 | Walk to the counter, `reach` for the jug and cup. | Drum: walk toward the rain drum by the screen door, stop before it. Tap: walk to the sink, `reach`, stop short of the tap. |
| N4 | Walk to the Teacher at the doorway under the roof and stand with the Teacher. | Cross: walk toward the moving water, stop before its edge. Wade: walk toward it, `crouch` (shoes), stop at a different x. |
| N5 | Walk to Mom, `sit` beside her. | Riverbank: walk toward the door, stop. Bridge: walk toward the door, stop at a different x. |
| H1 | Walk to Mom (`mom_walk_gobag`) and Dad by the front door and stand with them. | Wait: `sit` on the floor by the sofa, stop. Toy: walk toward the toy, stop. Game: `phone`, stop. |
| H2 | Walk with Dad along the near pavement to the EVACUATION ROUTE sign. | Cross: walk toward the moving water, stop before its edge. Bridge: walk toward the flooded bridge, stop at the path start. Barefoot: `crouch`, stop. |
| H3 | Walk along the path to Mom, who waits on the path away from the creek. | Swim: walk toward the bank, stop well back. Fish: `crouch` at the top of the bank, stop. Stones: `reach`, stop. |
| H4 | Walk to the basin, `wash` (pour clean water over the feet), walk to Mom, `call` "Mom, my feet got wet." | Shirt: `crouch`, stop. Play: run-start toward the open side, stop. Dry: `sit` alone on the mat edge, stop. |
| H5 | `crouch`, pick up the wet bread, walk to Mom, `reach`; Mom takes it. | Eat: pick up the bread, stop (no eating shown). Wipe: pick it up, stop. Cat: walk toward the cat, stop. |
| H6 | Walk to Mom and `sit` beside her. | TV: walk toward the TV, `reach`, stop short. Fan: walk toward the outlet, stop far from it. Switch box: walk toward it, `reach-up`, stop short. |
| H7 | Walk to Mom at the front door and stand with her. | See: walk toward the far side of the street, stop. Mud: walk toward the mud, stop before it. Water: walk toward the leftover floodwater, stop before its edge. |

## Art list (draft)

- **Layers:** Floodwater (door seep in 3 steps, still shallow, moving), mud, and window flood views for `home` (low and high water). Rain, Dark Sky and the leaf are reused from Typhoon.
- **Settings:** new `creek` (H3). Reused: `home`, `kitchen`, `yard`, `street`, `schoolyard`, `street_storm`, `home_after`, `street_after`, `evacuation_area`.
- **Menus:** a Flood Episode background and three Difficulty backgrounds (not yet designed).
- **Grown-ups:** reused. `dad_switch` and `home_switch_box` (148,161) come from Fire.
- **Student poses:** reused `reach`, `reach-up`, `carry`, `crouch`, `call`, `walk`, `sit`, `wait`, `happy`. Possibly new: `wash` (H4) and `tip` (E3).
- **Props:** canal grate, trash bin and wrapper (E2); can and tire with rainwater, upright and tipped (E3); low outlet with the fan plug (N2, unless Fire's `home_outlet` at (271,149) fits); boiled-water jug and cup (N3); a TV rainfall-warning screen (H1); wet bread (H5); basin and dipper (H4); a flooded bridge (H2). Reused: Go Bag, radio, toy, `yard_ball`, the water drum at (408,148), `classroom_bag`.
- **Icons:** about 40 new `icon_flood_*`, reusing the Go Bag, door, toy and tell-Dad icons where they fit.

## Batch 1: Floodwater at home, Normal 2

The batch list is in `.scratch/flood-episode/art-batches.md`. Batch 1 is drawn to `D:/GameDEV/flood-episode-scratch/art/01-home-seep/` and waits for approval.

`bg_home` with the flooded window view at (304,72) (rain, brown water low in the yard); the Floodwater seep under the closed front door at (18,107), spreading in three steps over the floor band and reaching about x 103 at the wall; the stand fan at (89,173) plugged into a low outlet at (72,188); Mom on the sofa; Dad walking in from the right edge to the switch box at (148,161) and playing `dad_switch`. Act-out points: Student start (214,232); "Unplug the fan" stops in `reach` at (162,232); "Wipe up the water" stops in `crouch` at (140,232); Safe: walk right to (402,232), `call`, Dad walks in and turns off the main switch, the fan stops, the Student plays `happy`.

The preview sheet (480x270 at 4x, a 2400x1080 crop and `motion.gif`) shows:

- The waterline: it reads as water at phone size, not as a shadow, a rug or mud; the outlet stays clearly above it.
- Scale: the Student (32x50) beside Mom and Dad (36x62), against the door and the outlet.
- Hand poses: `reach` toward the plug stops short and never touches it; `call` faces Dad; Dad's hand is on the lever.
- The walk line: the Student's start on y 230, the "Unplug the fan" and "Wipe up the water" stops at different x, all on dry floor at least 24 art pixels from the water's edge, and the Safe walk to Dad.
- Clearance for the Caption, Hint and Pause buttons, and nothing the Learner must see in rows 0 to 53.
- Tone: calm and grey-brown, not scary; nobody in the water.

## Open questions

The Level rows and the GAP rows were approved on 2026-11-27, and the build plan is in docs/specs/flood-episode.md. Still open:

- The Flood Episode and Difficulty backgrounds (art Batch 7).
- The wording of every text, drafted per Difficulty and approved by the user before each slice is committed.
