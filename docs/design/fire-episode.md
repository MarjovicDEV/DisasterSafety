# Fire Episode

Settled in the Fire design interview on 2026-11-26. The user accepted every recommendation and asked for the best option wherever one was open. Terms follow CONTEXT.md. Safety facts and their sources are in docs/research/fire-safety.md (FI-01 to FI-83). The Level flow, Act-out rules, v2 art bar, Stars and Attempts from docs/design/gameplay-redesign.md hold unchanged unless this file says otherwise. The rules for conflicting sources are recorded in docs/adr/0004-fire-safety-rules.md.

## Decisions

| # | Question | Decision |
|---|---|---|
| F1 | Rules where sources disagree | As in ADR 0004: the Student never uses a fire extinguisher; take the stairs, never the elevator; close doors behind you; never a wet cloth on a fire, a handkerchief over the nose only while walking out; Stop, Drop, and Roll with "cover your face with your hands"; "Cool the burn with cool water" with no time. |
| F2 | What "during" looks like | Grey Smoke drifts in, the Fire Bell rings or someone shouts "Fire!", and small Flames burn at one side of the scene, never on a person. No screen shake and no red tint. Nobody is ever shown burning ("fun, never scary"). |
| F3 | What each Difficulty teaches | Easy: before a fire, safe habits (prevention). Normal: escaping, in the school fire drill and in home kitchen and electrical fires. Hard: real fires in harder places (bedroom at night, trapped, clothes on fire, mall, neighbor's house, firefighter). |
| F4 | Backgrounds | Reuse the Earthquake Setting art that fits (`classroom`, `corridor`, `home`, `bedroom`, `schoolyard`, `mall_a`, `mall_b`) and put Smoke, Flames and a Night tint on top as per-Level layers. New Settings only where no picture fits: `kitchen`, `home_night` (brownout, candle light), `yard` (siga), `street` (barangay street) and `street_after`. Every new picture keeps the v2 art bar. |
| F5 | What starts a home fire | At home: a Grown-up shouts "Fire!", or the Student smells or sees Smoke. No smoke alarm in home Levels (how common they are in Philippine homes is not confirmed). School and the mall use the Fire Bell. |
| F6 | Wording and hotline | English Captions, as in Earthquake. The shout in the speech bubble is "Fire!". The hotline is always 911. "S.A.F.E." appears at most once, in Feedback, and its "Fight the small fire" step is never shown. |
| F7 | Who approves | The researcher reviews the Level list below (each Safe Choice, Unsafe Choices and FI sources) before `/to-spec`. Nothing ships that the researcher has not approved. |
| F8 | The 15 Levels | As listed below. |
| F9 | Fire Difficulty backgrounds | Easy: the `home` living room. Normal: the school corridor with the red Fire Bell box. Hard: the barangay street at dusk with the fire truck parked. Level selection for Fire uses these in place of the station noticeboard stand-in. |
| F10 | Build order | Shared work first: the Smoke, Flames and Night layers and the Fire Bell; the new poses (`crawl`, `stop_drop_roll`, `feel_door`, `signal`); the Firefighter Grown-up; the `kitchen`, `home_night`, `yard`, `street` and `street_after` Settings; the Fire Difficulty backgrounds and the Set B music. Then one slice per Setting group in play order (Easy, Normal, Hard), each passing the build, both test runs and its screenshots. The user plays each Difficulty in the Unity Editor; one Android APK at the end. All work is committed on `main`. |
| F11 | Crawling under Smoke | A side-view `crawl` on hands and knees, 4 frames, mirrored for left and right. The Smoke band fills the top 40% of the scene so the Student is visibly under it. The handkerchief is not used in the crawl; it appears only in Normal 2, held to the nose while the Student walks out low. |

## Levels

The Safe Choice is listed first here only. Choice counts follow ADR 0002: Easy 2, Normal 3, Hard 4. Phase is "before", "during" or "after".

### Easy: before a fire

| # | Setting | Phase | Scenario | Safe Choice | Unsafe Choices | FI |
|---|---|---|---|---|---|---|
| E1 | `home` | before | A lighter lies on the floor. | Give it to Mom | Try to light it | 56, 57, 59 |
| E2 | `home_night` | before | Brownout. A candle burns near the curtain. | Tell Mom to move it | Play with the candle | 58, 60, 61, 62 |
| E3 | `kitchen` | before | A pot boils over on the stove. Mom is outside. | Call Mom | Move the pot yourself | 49, 50 |

### Normal: escape

| # | Setting | Phase | Scenario | Safe Choice | Unsafe Choices | FI |
|---|---|---|---|---|---|---|
| N1 | `classroom` | during | The Fire Bell rings. | Line up and walk out | Duck under the desk; Get your bag | 42, 43, 44, 45, 48, 10 |
| N2 | `corridor` + Smoke | during | Smoke in the hallway during the drill. | Stay low and follow the Teacher | Run through the smoke; Hide in the comfort room | 17, 18, 20, 71 |
| N3 | `schoolyard` | after | The class reaches the Evacuation Area. | Stay with your class | Go home; Go back for your bag | 42, 45, 10 |
| N4 | `kitchen` + Flames | during | Oil in Mom's pan catches fire. | Shout "Fire!" and go out with Mom | Throw water on it; Carry the pan outside | 12, 51 |
| N5 | `home` + Flames | during | The octopus wiring sparks. | Tell Dad and stay away | Pour water on it; Pull the plugs | 54, 55 |

In N4, Mom covers the pan with the lid and turns off the stove as she leaves (an adult action, FI-51); the Student never touches the pan. In N5, Dad turns off the main switch.

### Hard: real fires

| # | Setting | Phase | Scenario | Safe Choice | Unsafe Choices | FI |
|---|---|---|---|---|---|---|
| H1 | `bedroom` + Night + Smoke | during | Smoke comes into the bedroom at night. | Crawl low and get out | Hide under the bed; Run standing up; Get your toy | 17, 18, 71, 08 |
| H2 | `bedroom` + Night + Smoke | during | The way out is through a closed door. | Feel the door first | Open the door; Hide in the closet; Wait in bed | 21, 22, 71 |
| H3 | `bedroom` + Night + Smoke | during | The door is hot and there is no other way out. | Block the gap and signal at the window | Jump from the window; Open the door; Take a video | 24, 75, 78 |
| H4 | `yard` | during | The Student's sleeve catches fire near the siga. | Stop, Drop, and Roll | Run for help; Beat it with your hands; Wave your arms | 25, 26, 28, 29, 30 |
| H5 | `mall_a` / `mall_b` | during | The Fire Bell rings in the mall. | Follow the EXIT sign to the stairs with Mom | Take the elevator; Run and push; Go back for the toy | 43, 66, 67, 68, 69 |
| H6 | `street` + Flames | during | A neighbor's house is on fire. | Tell an adult to call 911 | Go near to watch; Film it; Run inside to help | 11, 16, 75, 77, 78 |
| H7 | `street_after` | after | A Firefighter in full gear walks over. | Go to the Firefighter | Hide from the Firefighter; Go back in for the cat; Run back home | 08, 09, 71, 72, 73 |

- H2: the Safe Act-out is: crawl to the door, feel it with the back of the hand, find it hot, crawl to the second way out (the window to the yard of a one-storey house), and go to the Meeting Place.
- H3: the Safe Act-out is: push a blanket into the gap under the door, go to the window, wave a bright cloth. Mom outside sees and calls 911; a Firefighter reaches the window. The Student never jumps.
- H4: the Safe Act-out is: stop, drop, cover the face with the hands, roll until the flame is out. Its Feedback adds "Cool the burn with cool water and tell an adult" (FI-28, FI-29).
- H6 and H7: the Student stays at a safe distance on the street. The fire is a neighbor's house in H6, and the Student's own house, already out, in H7.

## Grown-ups and characters

- The Teacher (N1, N2, N3), Mom (E1, E2, E3, N4, H3, H5, H7) and Dad (N5, H6) reuse the Earthquake Grown-ups.
- **Firefighter**: a new Grown-up in the v2 style, about 62 pixels tall, in full gear (helmet, coat, mask hanging at the neck so the face shows). Poses: idle, walk, wave, kneel to the Student's height (H3, H7). Never scary: the face is visible and friendly (FI-72, FI-73).

## New poses

`crawl` (side, 4 frames), `stop_drop_roll` (drop, cover face, roll: 6 frames), `feel_door` (side, back of hand to the door: 2 frames), `signal` (front, waving a cloth at the window: 2 frames). Walking low with the handkerchief reuses the walk cycle with a lower head and the handkerchief prop.

## Open questions

- The wording of every Caption, Hint, Feedback and Reaction is drafted in the spec and checked by the researcher (F7).
- Whether the researcher wants the Filipino shout "Sunog!" next to "Fire!" (F6 keeps English only).
