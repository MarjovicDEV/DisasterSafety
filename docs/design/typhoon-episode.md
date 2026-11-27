# Typhoon Episode

Settled in the Typhoon design interview on 2026-11-27. The user accepted every recommendation. Terms follow CONTEXT.md. Safety facts and their sources are in docs/research/typhoon-safety.md (TY-01 to TY-65). The Level flow, Act-out rules, v2 art bar, Stars and Attempts from docs/design/gameplay-redesign.md hold unchanged unless this file says otherwise. The rules for conflicting sources are recorded in docs/adr/0005-typhoon-safety-rules.md.

The spec and tickets were to wait until two things were done: the researcher approves the Level list below, and Fire tickets 01 and 03 are built. The researcher approved the Level list on 2026-11-27 (see "Researcher review"). Fire tickets 01 and 03 were not built yet; the user chose to write the spec and tickets anyway, so every Typhoon ticket also waits for the Fire tickets it builds on. Typhoon reuses the Episode-specific "during" effect that Fire ticket 01 adds (a "during" Scenario that does not shake) and the per-Level layers that Fire ticket 03 adds (layers chosen by the Level's start entry on its Setting).

## Decisions

| # | Question | Decision |
|---|---|---|
| T1 | Rules where sources disagree | As in ADR 0005: class suspension follows DepEd DO 22, s. 2024 (Grades 3 to 6 have no face-to-face classes at Wind Signal No. 2 or higher); a flashlight, never a candle, and no Student ever holds a candle or matches; windows are never taped or covered as a Safe Choice, stay closed during the storm, and the Student stays away from them; evacuate with the Grown-ups when the barangay or officials say; go out or go home only when officials say it is safe; the Go Bag lasts 3 days; "boiled water" with no time; only the current PAGASA 5-level signals; the hotline is 911. |
| T2 | Typhoon or Flood | Typhoon keeps the wind and the warnings: Wind Signals, class suspension, preparing, staying indoors and away from windows, power outages, flying things, the eye of the storm, the barangay warning, evacuating before landfall, Storm Surge, and after the storm fallen wires, damaged houses and cleanup. Flood (Episode 4) keeps the water: rainfall colour warnings, rivers and canals, wading or playing in floodwater, rising water at home, dirty water and boiling it, mud and mosquitoes. A Typhoon Level may name the creek only as a place not to go; no Typhoon Choice is about floodwater itself. |
| T3 | What "during" looks like | No screen shake (`Scenario.Shakes` stays Earthquake only). Per-Level layers on the Setting art: **Rain** (light or heavy diagonal streaks), **Wind** (trees, curtains, clothes and loose things sway; only small leaves blow, in the top band of the scene, never within 40 art pixels of a person), **Dark Sky** (a grey tint on the sky and on windows) and Night (reused from Fire). In a Typhoon "during" Scenario the layers fade in over about 1.5 seconds, then the warning plays (T4), the Student plays the surprised pose, and the Action Bar slides up. No lightning flash and no thunder. Pause stops the layers and the rain and wind sounds; Resume continues them. "Before" and "after" Scenarios may show Dark Sky, light Wind or wet ground from the start, but start still and show the Action Bar after 1 second, as in Earthquake. Nothing heavier than a leaf ever moves through the air. |
| T4 | Warnings in the game | At home: a TV screen prop reading "SIGNAL NO. 1" or "SIGNAL NO. 2", or a speech bubble from the radio. At school: the Teacher says "Signal No. 2. Classes are suspended." In the barangay: the Barangay Tanod speaks through a megaphone in a speech bubble. Every warning is a speech bubble with the voice blip, never a recorded voice. Captions say "Wind Signal No. 2 is up." Only Signals 1, 2 and 3 appear, never 4 or 5 (too frightening, and the strongest wind on record at the General Santos station is in the Signal 3 range, TY-63). No phone alerts (no finding covers them). |
| T5 | What each Difficulty teaches | Easy: before the typhoon (listen to the news, bring loose things in, put the flashlight in the Go Bag). Normal: during the typhoon, at school and at home (a Signal raised in class, no classes at Signal 2, windows, falling mangoes, the barangay warning). Hard: harder cases, in play order from before to after (evacuating before landfall, Storm Surge at the coast, the power out at night, the eye of the storm, waiting at the Evacuation Center, a fallen wire, the damaged yard). |
| T6 | Settings | Reuse `home` (TV, window, door), `bedroom`, `yard`, `classroom`, `home_night` and `evacuation_area` with Rain, Wind, Dark Sky and Night layers and per-Level props. Two new Settings: `street_storm` (the Fire barangay street redrawn with the shutters closed, no food vendor, an evacuation route sign and a path toward the creek; Fire's `street` shows a vendor outdoors and `street_after` a burnt house) and `coast` (a seaside road with high, rough waves, a house and a path up to the Evacuation Center; the Earthquake `beach` shows the sea pulled back for a tsunami, the opposite of a Storm Surge). Both meet the v2 art bar. Neither shakes. |
| T7 | Episode and Difficulty backgrounds | Episode background (Difficulty selection): the barangay hall under a grey sky, with a "SIGNAL NO. 2" board and a flag bending in the wind. Easy: a home doorway with the packed Go Bag and the radio, grey sky. Normal: the classroom windows with rain streaming down. Hard: a school gym used as an Evacuation Center at night in the rain, its lights on and an "EVACUATION CENTER" sign (distinct from Earthquake Hard's open covered court). All four are menu illustrations, distinct from the playable Settings. |
| T8 | Grown-ups | Mom, Dad and the Teacher are reused. One new Grown-up, the **Barangay Tanod** (TY-18, TY-19, TY-20). No rescuer: no Level needs one without showing someone in danger. |
| T9 | The 15 Levels | As listed below. |
| T10 | New poses and props | As listed under "New poses" and "Props and icons" below. |
| T11 | Sounds | Three recorded, human-made CC0 loops: light rain, heavy rain on a tin roof, and wind with gusts. Candidate sources: Freesound.org with the CC0 licence filter, OpenGameArt.org CC0 and Wikimedia Commons CC0 field recordings. Each file is checked as a real recording (no synthesized or AI audio) and credited like the music. No thunder, no siren and no recorded voices. The music ducks under heavy rain as it does under the rumble. Nothing is downloaded until the spec. |
| T12 | Who approves and build order | The researcher reviews the Level list (`.scratch/typhoon-episode/level-list-for-review.md`) and later every text, before the spec. Nothing ships that the researcher has not approved. The build starts after Fire tickets 01 and 03. Shared work first: the Typhoon effect (Rain, Wind, Dark Sky and their sounds), the Barangay Tanod, the `flashlight` pose, the `street_storm` and `coast` Settings, the Typhoon Episode and Difficulty backgrounds. Then one slice per Setting group in play order: Easy (`home`, `yard`), Normal school (`classroom`), Normal home (`home`, `bedroom`), Hard outside before (`street_storm`, `coast`), Hard home (`home_night`, `home`), Hard after (`evacuation_area`, `street_storm`, `yard`). Each slice passes the build, both test runs and its screenshots. The user plays each Difficulty in the Unity Editor; one Android APK at the end. All work is on `main`. |

Typhoon already plays music Set A (`EpisodeMusic`), and Typhoon opens after Fire by the existing Episode order (`Curriculum`). Once Typhoon has Playable Levels, Fire Hard 7's Next Episode leads into Typhoon.

## Levels

The Safe Choice is listed first here only; in the Scenario files it moves between slots. Choice counts follow ADR 0002: Easy 2, Normal 3, Hard 4. Phase is "before", "during" or "after". Layers are the per-Level layers of T3.

### Easy: before the typhoon

| # | Setting + layers | Phase | Scenario | Safe Choice | Unsafe Choices | TY |
|---|---|---|---|---|---|---|
| E1 | `home` + Dark Sky | before | The TV says "SIGNAL NO. 1". A typhoon is coming. | Tell Mom the news | Change to cartoons | 22, 01, 31, 35 |
| E2 | `yard` + Dark Sky + light Wind | before | Dad ties things down. A pail, a ball and a plastic chair are on the grass. | Help bring light things inside | Keep playing with the ball | 28, 03, 29 |
| E3 | `home` + Dark Sky | before | Mom is packing the Go Bag. The flashlight is on the floor. | Put the flashlight in the Go Bag | Play with the flashlight | 23, 24, 27 |

Unsafe reasons: E1, the family needs the news to get ready (TY-22). E2, Signal 2 winds can throw light things (TY-03). E3, the flashlight and its batteries are needed when the power goes out (TY-27).

### Normal: during the typhoon, at school and at home

| # | Setting + layers | Phase | Scenario | Safe Choice | Unsafe Choices | TY |
|---|---|---|---|---|---|---|
| N1 | `classroom` + Rain + Wind + Dark Sky | during | The Teacher says "Signal No. 2. Classes are suspended." | Stay with the Teacher until Mom comes | Walk home alone; Wait outside at the gate | 12, 13, 36, 32 |
| N2 | `home` + Rain + Wind + Dark Sky | during | The TV says "SIGNAL NO. 2". The Student is in uniform with the school bag. | Stay home and do your module | Walk to school anyway; Go out to play in the rain | 12, 15, 16, 32 |
| N3 | `bedroom` + Rain + Wind + Dark Sky | during | The wind is loud and the window rattles. | Go to the living room with Mom | Watch the storm at the window; Open the window | 33, 04, 05, 32 |
| N4 | `home` + Rain + Wind + Dark Sky | during | Mangoes are falling in the yard, and the clothes are still on the line. | Stay inside with Mom | Pick up the mangoes; Bring in the clothes | 32, 46, 03 |
| N5 | `home` + Rain + Wind + Dark Sky | during | A Barangay Tanod calls through a megaphone: "Families near the creek, get ready!" | Tell Mom and bring the Go Bag | Keep playing; Go out to see the Tanod | 18, 19, 20, 24, 51 |

Unsafe reasons: N1, the school keeps learners safe until travel is safe (TY-13). N2, at Signal 2 there are no face-to-face classes for Grades 3 to 6 (TY-12); going out in the storm is unsafe (TY-32). N3, windows can break in strong wind (TY-04, TY-05, TY-33). N4, strong wind makes things fly and branches fall (TY-46). N5, the megaphone is the barangay's early warning; tell a Grown-up at once (TY-19, TY-20).

### Hard: harder cases

| # | Setting + layers | Phase | Scenario | Safe Choice | Unsafe Choices | TY |
|---|---|---|---|---|---|---|
| H1 | `street_storm` + light Rain + Dark Sky | before | The Barangay Tanod says families near the creek must go to the Evacuation Center now. Mom has the Go Bag. | Walk with Mom on the evacuation route | Run back for your toy; Take the shortcut by the creek; Stay home, our house is strong | 21, 51, 52, 53, 47 |
| H2 | `coast` + light Rain + Wind + Dark Sky | before | Visiting Lola by the sea, the radio warns of a Storm Surge. | Go up to the Evacuation Center with Dad | Watch the big waves; Swim before the storm; Pick up shells on the sand | 10, 50, 51, 34 |
| H3 | `home_night` + Rain + Wind + Night | during | The power is out during the storm. A flashlight and a candle are on the shelf. | Turn on the flashlight and stay with Mom | Light a candle; Go out to check the lights; Play games on the phone | 39, 27, 32, 44, 25, 40 |
| H4 | `home` + Rain + Wind + Dark Sky, then calm | during | The wind suddenly stops and the sky looks lighter: the eye of the storm. | Stay inside and listen to the radio | Go out to play; Get your bike from the yard; Visit your friend next door | 37, 35, 46, 32 |
| H5 | `evacuation_area` + Dark Sky | after | At the Evacuation Center, the rain stops. Some neighbors start to leave. | Wait for the Tanod to say it is safe | Go home with the neighbors; Run out to play; Look for your friend alone | 55, 54, 18 |
| H6 | `street_storm` + storm damage props | after | After the typhoon, a wire lies across the road with a branch on it, near a puddle. Dad is nearby. | Stay away and tell Dad | Poke it with a stick; Move the branch off the wire; Walk through the puddle by it | 44, 45, 56, 65 |
| H7 | `yard` + storm damage props | after | The shed is broken. A roof sheet and broken glass lie on the grass. Dad is checking. | Stay with Mom while Dad checks | Pick up the roof sheet; Go into the broken shed; Sweep up the broken glass | 57, 59, 55 |

Unsafe reasons: H1, go right away with the family on the set route, and evacuation is meant to finish before landfall (TY-21, TY-51, TY-52, TY-53). H2, Storm Surge floods low coasts, and people stay off the shore (TY-10, TY-34, TY-50). H3, open flames start fires; the phone battery is needed while the power is out; wires may be down outside (TY-39, TY-25, TY-40, TY-44). H4, the other side of the storm can come back in minutes (TY-37). H5, go home only when officials say it is safe (TY-55). H6, a fallen wire, and water near it, can carry electricity (TY-44, TY-45). H7, damaged buildings are checked first, and children do not do cleanup work (TY-57, TY-59).

### Act-outs in short form

"Then" separates steps. **Stop** marks where an Unsafe Choice halts; the Student then plays `wait` and says "Hmm, wait...". Every Safe Choice ends with `happy` and the Safe Reaction ("All ready!" in "before" Levels, "I'm safe!" in "during" and "after" Levels). No Unsafe stop is outdoors in the storm, in a doorway open to the storm, at the water, or within reach of a wire, a flame, the shed or the glass.

| Level | Safe Act-out | Unsafe stops |
|---|---|---|
| E1 | Walk to Mom, `call` "Mom, a typhoon!", Mom plays `happy` and turns the radio on. | Cartoons: walk to the TV, `reach` toward it, stop. |
| E2 | Pick up the pail, `carry` it to the house, set it down; Dad carries the chair in. | Ball: walk to the ball, stop beside it. |
| E3 | Pick up the flashlight, walk to the bag, `pack` it in; the bag closes. | Play: pick up the flashlight, `flashlight` pose, stop. |
| N1 | Walk to the Teacher, `sit` at the desk; Mom walks in with an umbrella, the Student walks to her. | Walk home: walk toward the door, stop inside. Gate: walk toward the door, stop before the threshold, facing out. |
| N2 | Put the bag down, `sit` at the table with the module, `pack` (writing). | School: `carry` the bag toward the door, stop. Play: walk toward the door, stop. |
| N3 | Walk to the bedroom door, the door opens, walk out; Mom waits in the doorway. | Watch: walk toward the window, stop at the bed's end. Open: walk to the window, `reach-up` toward the latch, stop. |
| N4 | Walk away from the door to Mom, `sit` beside her. | Mangoes: walk toward the door, stop. Clothes: walk toward the door, `reach` for the knob, stop. |
| N5 | Walk to Mom, `call` "Mom, the Tanod!", pick up the Go Bag, `carry` it to Mom at the door. | Keep playing: `sit` with the toy, stop. See: walk toward the window, stop. |
| H1 | Walk with Mom behind the Tanod to the evacuation route sign. | Toy: turn and walk back toward the house, stop. Shortcut: walk toward the creek path, stop at its start. Stay: walk back toward the door, stop. |
| H2 | Turn, walk with Dad up the path to the Evacuation Center sign. | Waves: walk toward the sea wall, stop well back from it. Swim: walk toward the steps to the sand, stop at the top. Shells: walk toward the steps, `crouch`, stop at the top. |
| H3 | Walk to the shelf, pick up the flashlight, `flashlight` (beam on), walk to Mom and `sit` beside her. | Candle: walk to the shelf, stop before reaching; no hand ever touches the candle or the matches. Lights: walk toward the door, stop. Phone: `phone`, stop. |
| H4 | Walk to Mom, `sit` by the radio; the radio says "Stay inside." The Rain and Wind return. | Play: run-start toward the door, stop. Bike: walk toward the door, stop. Friend: walk toward the door, stop and face the window. |
| H5 | `sit` on the mat by Mom; the Tanod walks in, megaphone: "It is safe to go home now." | Neighbors: walk toward the court edge after them, stop. Play: run-start toward the open side, stop. Friend: walk off between the tents, stop. |
| H6 | Step back from the wire, walk to Dad, `call` "Dad, a wire!"; Dad uses `phone` (911). | Stick: walk toward the stick, `crouch`, stop before the wire. Branch: walk toward the branch, stop far from it. Puddle: walk toward the puddle, stop before its edge. |
| H7 | Walk to Mom by the house, stand with her; Dad looks over the shed. | Roof sheet: walk toward it, stop. Shed: walk toward the shed door, stop. Glass: walk toward the broom, stop. |

## Grown-ups and characters

- The Teacher (N1), Mom (E1, E3, N1, N3, N4, N5, H1, H3, H4, H5, H7) and Dad (E2, H2, H6, H7) reuse the existing Grown-ups. Mom gets a walk with an umbrella for N1. Dad gets a `carry` for E2. Dad's existing `phone` pose (Fire) serves H6.
- **Barangay Tanod**: a new Grown-up in the v2 style, about 62 pixels tall: a navy shirt, a cap, a yellow reflective vest reading "TANOD", boots, a friendly, visible face, and a megaphone in hand. Poses: idle, walk (side, 4 frames, mirrored), megaphone (front, 2 frames), wave. Used in N5 (heard and seen at the window), H1, H5 and, if room allows, H6 (TY-18, TY-19, TY-20).
- Lola (H2) is named in the Caption only and is not drawn; the Level shows Dad.

## New poses

`flashlight` (front, holding a lit flashlight with a short beam, 2 frames) for the boy and the girl. Every other Act-out reuses the existing poses: walk, run-start, `carry`, `pack`, `reach`, `reach-up`, `call`, `phone`, `sit`, `crouch`, surprised, `wait` and `happy`.

## Props and icons

- Props: the TV "SIGNAL NO. 1" and "SIGNAL NO. 2" screens, the pail, the ball, the plastic chair, the school bag (Fire), the module booklet, mangoes, clothes on the line, the candle and matches on a shelf (Fire candle reused, never held), the flashlight with its beam, the phone, the bike, the stick, the fallen branch, the fallen wire, the puddle, the roof sheet, the broken shed, the broken glass and broom, the shells, the toy, Mom's umbrella, the Tanod's megaphone and the evacuation route sign. Reused: the Go Bag (open and closed), the radio (2 frames) and the flashlight from Earthquake.
- Icons: one 16x16 icon per new Choice, about 40, reusing `icon_go_bag`, `icon_window`, `icon_door` and the existing toy and phone icons where a Choice matches.

## Researcher review

### Level list review

- **Scope:** The researcher reviewed `.scratch/typhoon-episode/level-list-for-review.md`. The review covered every Safe Choice, every Unsafe Choice with its reason and sources, the rules from ADR 0005 and the six "Points to check".
- **Result:** On 2026-11-27 the researcher approved the Level list with no changes. The Level tables, the Act-out table, the Grown-ups, the props and ADR 0005 stay as written.
- **Points to check:** All six plans stand as written in the list:
  1. Hard 5 keeps "Look for your friend alone" and rests on TY-54 and TY-55.
  2. Hard 1 keeps "Run back for your toy" and rests on TY-51.
  3. Hard 2 stays a visit to Lola by the sea, apart from Earthquake Normal 5.
  4. Nobody wears a raincoat in Hard 1 or Hard 2.
  5. Captions and warnings stay in English only.
  6. The game uses 911 and a fictional Evacuation Center.
- **Next:** The researcher still approves the wording of every Caption, Hint, Feedback, Choice label, lesson and Act-out line before each slice's Scenario files are committed (T12).

## Open questions

- The wording of every Caption, Hint, Feedback, Choice label and Reaction is drafted in the first slice of each Difficulty and checked by the researcher (T12).
- H5 stays weakly sourced: no primary source gives rules for children inside an Evacuation Center (TY-54). The researcher accepted it in the Level list review; its text review may still change "Look for your friend alone".
- H2: whether Baluan or nearby General Santos coasts are Storm Surge areas is not confirmed. Lola's house keeps it a visit (accepted in the Level list review).
- The General Santos CDRRMO hotline and the city's real evacuation centers were not found; the game uses 911 and a fictional Evacuation Center (accepted in the Level list review).
