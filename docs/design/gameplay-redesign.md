# Gameplay redesign: the Student acts it out

Settled in the design interview on 2026-11-17. The user found the gameplay flat and the art weak, especially the characters and the Choice pictures, and asked for a game where the Learner chooses what to do and then watches it happen. The user accepted every recommendation below. Terms follow CONTEXT.md. This file replaces the Choice-card and Bantay parts of docs/design/first-playable-level.md, earthquake-easy-2-3.md, earthquake-normal.md and earthquake-hard.md; their Scenario text, safety lines and EQ sources still hold.

The art direction and the removal of Bantay are recorded in docs/adr/0001-v2-art-and-no-bantay.md.

## Principle: lots of animation

The game is animated throughout. The Student, the scene and the reactions all move, and every Choice the Learner picks is acted out on screen. This needs real animation support, not only the two-frame wobble Settings use today: frame sets per pose and a small animator per character (see "Animation" below).

## The Level flow

1. **Scene.** The Learner's Student stands in the Setting. The Caption at the top shows the Scenario intro and the Goal.
2. **The earthquake** (a "during" Scenario only). The scene shakes for 2 to 3 seconds with the rumble, the moving parts sway, and the Student reacts ("Whoa!"). A "before" or "after" Scenario skips this step and starts still.
3. **The Action Bar** slides up at the bottom while the shaking goes on more gently. It shows the Scenario's 3 or 4 Choices as short commands, each with a 16x16 icon, such as "Duck under the desk" or "Run to the door". There are no Choice pictures.
4. **The Student acts it out.** The Learner taps a Choice and the Student does it in the scene.
   - **Safe Choice:** the full action plays, for example walking to the desk and doing Duck, Cover, and Hold. The shaking stops, the Student says "I'm safe!", then Feedback shows, then Level Completed.
   - **Unsafe Choice:** the Student starts the action, stops, and says "Hmm, wait...". Feedback explains why, and the Learner chooses again in the same Attempt. Nothing falls and nobody gets hurt ("fun, never scary" still holds).
5. **Hint.** The Hint button shows the Hint in the Caption.

Stars, Attempts, Open/Locked and Saved Progress do not change. `DisasterSafety.Core` keeps its rules; only how a Choice is shown and played changes.

## Decisions

| # | Question | Decision |
|---|---|---|
| Q1 | How the Learner chooses | An Action Bar of short commands at the bottom; the scene is where the Choice plays out. |
| Q1a | One Choice or a chain | One Choice per Level in every Difficulty. The chain for Normal and Hard was dropped after the pilot (R1 below). |
| Q1b | The earthquake moment | A 2 to 3 second shake first, then the Action Bar slides up while a gentler shake continues. |
| Q1c | "Before" and "after" Levels | The same Action Bar, without the shake. The Student still acts out the Choice. |
| Q2 | The Learner's character | The Learner picks a boy or a girl Student once, with the Display Name. |
| Q3 | What an Unsafe Choice shows | The Student starts, stops, says "Hmm, wait...", then Feedback. A Safe Choice plays in full. |
| Q4 | How the art is made | The v2 style (see ADR 0001). Claude drafts every sprite in code; the user may polish any of them in Aseprite, and a hand-edited `.aseprite` file then becomes the source. |
| Q5 | Scope and order | Pilot Easy 1 (classroom) first, judged on a phone, then roll out to every Level. |
| Q6 | Earthquake Hard | Hard was merged into main before this interview ended, so the rollout covers all 15 Earthquake Levels. |
| Q7 | Who tells the story | The Caption tells the Scenario and shows the Hint. The Student's speech bubble carries short reactions. |
| Q8 | Who reacts to an Unsafe Choice | The Student: "Hmm, wait...". |
| Q9 | Main Menu and How to Play | The Learner's chosen Student replaces Bantay there. |
| Q10 | An Action Bar button | A short command plus a 16x16 icon. |
| Q11 | The Student's size | The 50-pixel Student acts in the scene. The 22-pixel chibi is kept for map-style screens. |
| Q12 | Backgrounds | Redrawn in the v2 style, classroom first, with furniture sized for the 50-pixel Student (a desk about 40 pixels tall). |
| Q13 | Facing | Front view for idle, reactions and Duck, Cover, and Hold; a side view, mirrored for left and right, for walking. |
| Q14 | Stars and Attempts | Unchanged. |
| Q15 | Chain scoring | No chain, so no chain scoring (R1 below). |
| Q16 | How the pilot is judged | An Android test build (APK) installed on the user's phone. |
| Q17 | Old art | Bantay and the Choice pictures stay in the repo until every Level is moved over, then go in one cleanup. |
| Q18 | Where the work happens | Branch `gameplay-redesign` from main. |

## Characters

- **Student** (boy and girl): the v2 sprites, 32x50 big and 16x22 chibi, in the school uniform: a white polo or blouse with the red school patch, navy pants with a belt or a navy pleated skirt with white socks, and black shoes. The samples are in `StyleSample/` in the source folder (`v2_big_boy`, `v2_big_girl`, `v2_chibi_boy`, `v2_chibi_girl`, `v2_chibi_duck`) with the script `student2.py`.
- **Bantay** is removed from the Levels, the Main Menu and How to Play.

## Animation for the pilot (Easy 1, classroom)

The big Student, boy and girl:

| Pose | Facing | Frames |
|---|---|---|
| Idle (breathing) | front | 2 |
| Surprised ("Whoa!") | front | 2 |
| Walk | side, mirrored | 4 |
| Duck, Cover, and Hold under the desk | front | 2 (hold loop) |
| Start of a run (Unsafe) | side, mirrored | 2 |
| "Hmm, wait..." | front | 2 |
| Safe and happy ("I'm safe!") | front | 2 |

The scene: the classroom redrawn in v2 style, its ceiling fan and cabinet wobble as today, the Student's speech bubble, the Caption banner and the Action Bar with its slide-up.

## Pilot result (2026-11-18, Unity Editor)

The user played Easy 1 in the Unity Editor (not on the phone) and answered the checklist as a whole: "that's exactly the gameplay that I want".

- **Gameplay: go on with the rollout.** The Student flow is the target for every Level.
- **Duck, changed after the first play.** The first front-facing duck did not look real. The Student now walks to the desk, crouches and shuffles in under it (`crouch`, side view), then holds the desk leg with one hand and covers the head with the other arm (`hold`). This is the drill as taught. The design rule: actions are acted out realistically, step by step, not as a jump cut.
- **Art: keep the bar.** The user is happy with the v2 art. Every new Level, scene, pose and prop must match it: the v2 outline and three-shade style, light from the left, the same palette and pixel scale.
- **Music, changed after the first play.** One song played in the menus and in the Level, which the user did not like. The Level now has its own song, `bgm_level` (calm and focused: 88 BPM, soft triangle lead, no drums), and fades from the menu song when a Level opens and back when it closes. The music drops to about a third while the ground shakes, so the rumble stands out.

## Rollout decisions (2026-11-21)

Settled in the rollout interview for the 14 remaining Earthquake Levels. The user accepted every recommendation and asked for the choices that make the best game. The spec is `docs/specs/earthquake-rollout.md`; the tickets are in `.scratch/earthquake-rollout/issues/`.

| # | Question | Decision |
|---|---|---|
| R1 | Chain of steps in Normal and Hard (Q15) | Dropped. Every Level keeps one Scenario with 3 or 4 Choices. A chain would need about 24 new Scenario steps with new text, Feedback and EQ sources, and a new Star rule across steps, which would change `DisasterSafety.Core`. The depth comes from the realistic act-out instead. |
| R2 | Sound for Reactions | One soft, very short voice blip (`sfx_blip`, generated by `make_audio.py`), pitched a little higher for the girl, played with each Reaction under the existing Safe and Unsafe sounds. Never words. |
| R3 | How a Choice's act-out is described | A list of Act-out Steps per Choice in the Setting (walk, crouch, pose, prop, grown-up, wait). An Unsafe Choice stops after a set step, then plays "Hmm, wait...". Easy 1 moves onto the same steps with no visible change, so there is one path. |
| R4 | Other people in the scene | Three Grown-ups in the v2 style, about 62 pixels tall: the Teacher (Easy 3, Hard 2), Mom (Normal 2, Hard 1, Hard 5, Hard 6, Hard 7) and Dad (Hard 5). In Hard 5 the hurt family member is Mom, sitting and holding her leg, with no blood. Crowds are 2 or 3 still or two-frame people drawn into the mall and evacuation area backgrounds. |
| R5 | Shared backgrounds | `home` is one living room for Normal 2, Normal 3 and Hard 1. `home_after` is the same room after the earthquake, for Hard 5 and Hard 6. `schoolyard` serves Normal 4 and Hard 2. `classroom_after` is a variant of the pilot classroom drawn by a new script. `evacuation_area`, `bedroom`, `corridor`, `beach`, `mall_a` and `mall_b` each get their own picture. Every background is a new file; no pilot file is edited. |
| R6 | Order and checks | The shared work first (Act-out Steps, Grown-ups, shared poses, the blip), then one slice per Setting group in play order. Each slice passes the build, both test runs and its screenshots, and its art preview is compared with the pilot classroom and Student. The user plays in the Unity Editor after the last Easy, Normal and Hard slice. One Android APK is built at the end, before the cleanup. |
| R7 | Q17 cleanup | One last ticket, after the user approves the final APK: remove Bantay (sprites, code, strings), the Choice pictures, the `picture` field of every Scenario, the old card flow in `LevelController` and the v1 Setting backgrounds no longer used. |
| R8 | The act-out of every Choice | As listed under "Act-outs" below. |
| R9 | New Student poses | Nine shared poses for the boy and the girl, listed under "New poses" below. Climbing a slope or stairs reuses the walk cycle with a change of height; lifting and kneeling reuse `crouch`. |
| R10 | Grown-ups, props and retrying | Grown-up poses, props and icons as listed below. After an Unsafe Choice, while the Student walks back to the start, every prop the Student moved returns to its place (fading back, or sliding back for furniture). |
| R11 | Main Menu, How to Play and Level Completed (Q9) | The Student replaces Bantay on the Main Menu and in How to Play, and the Tutorial Pages show the Action Bar flow, before the cleanup removes Bantay. |

### Act-outs

"→" means "then". **Stop** marks where an Unsafe Choice halts; the Student then plays `wait` and says "Hmm, wait...". Every Safe Choice ends with `happy`, "I'm safe!" (or the Level's own Safe Reaction), the sparkles and Feedback. Pose names in `code` are new art.

**Easy 2, corridor (during).** Stairs on the right, a window, hanging plants and a plain inner wall in the middle.

| Choice | Act-out |
|---|---|
| Safe: Duck and cover your head | Walk to the inner wall, away from the glass and pots → crouch → `cover` (kneel low, both arms over the head and neck) and hold until the shaking calms |
| Run down the stairs | Run-start toward the stairs → stop at the top step |
| Stand by the window | Walk toward the window → stop |
| Stand under the hanging plants | Walk toward the spot under the pots → stop just before it |

**Easy 3, classroom_after (after).** The Student starts under the desk in `hold`, continuing from Easy 1. The Teacher waves at the door.

| Choice | Act-out |
|---|---|
| Safe: Walk out with your teacher | Crouch-shuffle out from under the desk → stand → walk to the Teacher → the Teacher turns and both walk out of the door |
| Stay under the desk | Keep holding and peek at the Teacher → stop |
| Run and push to the door | Crouch-shuffle out → stand → run-start at the door → stop behind the Teacher |

**Normal 1, bedroom (before).** A heavy box and a big picture on the floor; a bed, a window, a tall cabinet and a low shelf.

| Choice | Act-out |
|---|---|
| Safe: Put heavy things on a low shelf | Walk to the box → crouch and lift it → `carry` it to the low shelf → crouch and set it down (the box shows on the shelf) |
| Hang a big picture over the bed | Pick up the picture → `carry` it to the bed → `reach-up` toward the wall → stop |
| Move the bed under the window | Walk to the bed → `push` (the bed slides a few pixels) → stop |
| Put books on top of the tall cabinet | Pick up the books → `carry` them to the cabinet → `reach-up` on tiptoe → stop |

**Normal 2, home (before).** Mom by the table with an open go bag; water, a can, a flashlight, a whistle, snacks and toys around it; boxes in the corner.

| Choice | Act-out |
|---|---|
| Safe: Pack water, food, a flashlight and a whistle | `pack` four times, each item popping into the bag → the bag closes → `carry` the bag to its spot by the door → Mom plays `happy` |
| Pack it later, when it shakes | Turn away and walk toward the TV → stop |
| Hide it under the boxes | `carry` the bag to the boxes → crouch to push it under → stop |
| Pack only snacks and toys | Pick up a toy → `pack` → stop over the bag |

**Normal 3, home (during).** The living room with a strong table, a sofa, a glass cabinet and the door.

| Choice | Act-out |
|---|---|
| Safe: Duck under the strong table | The pilot Duck: walk → crouch-shuffle under → `hold` the table leg, with a `table_front` overlay drawn over the Student |
| Run out the door | Run-start → stop |
| Stand by the glass cabinet | Walk toward it → stop |
| Lie down beside the sofa | Walk to the sofa → crouch, starting to lie down → stop |

**Normal 4, schoolyard (during).** A big tree, an electric post, the building and an open field.

| Choice | Act-out |
|---|---|
| Safe: Go to the open field | Walk to the middle of the field → crouch → `cover` until the shaking calms |
| Hide under the big tree | Walk toward the tree → stop at the edge of its shade |
| Run back into the building | Run-start at the door → stop |
| Stand by the electric post | Walk toward the post → stop |

**Normal 5, beach (after).** The sea has pulled back and fish lie on the wet sand. A path climbs on the right to a hilltop with a tsunami evacuation sign.

| Choice | Act-out |
|---|---|
| Safe: Go to high ground right away | Turn → fast walk up the path (the Student moves up as well as across) → stand on the hilltop by the sign |
| Pick up the fish on the sand | Walk down onto the wet sand → crouch to reach → stop |
| Stay and watch the waves | Walk to the edge of the water and look out → stop |
| Wait on the beach for news | Walk to the beach mat → `sit` → stop |

**Hard 1, home (before).** Mom at the table with a "Meet here" board on the wall and two cards on the table: a park and a house.

| Choice | Act-out |
|---|---|
| Safe: Agree on a meeting place | Pick up the park card → walk to the board → `reach-up` and pin it → Mom plays `happy` |
| Each of us finds our own way | Turn and walk off alone toward the door → stop |
| Plan to meet back at home | Pick up the house card → walk to the board → `reach-up` → stop |
| Keep numbers only in the phone | `phone` (take the phone out and start typing) → stop |

**Hard 2, schoolyard (before).** The Teacher at the building door with the class route; three paths lead away.

| Choice | Act-out |
|---|---|
| Safe: Take the route across the field | The Student and the Teacher walk the field path to the Evacuation Area sign |
| Take the path by the post | Walk toward the post path → stop |
| Walk the path under the tree | Walk toward the tree path → stop |
| Find the way when it happens | Turn and walk back toward the classroom → stop |

**Hard 3, mall_a (during).** The food court with a table, the exit, an elevator and a glass shop window.

| Choice | Act-out |
|---|---|
| Safe: Duck under the food court table | The pilot Duck, with a `foodtable_front` overlay |
| Run to the mall exit | Run-start → stop |
| Ride the elevator down | Walk to the elevator → `reach` toward the button → stop |
| Stand by the glass shop window | Walk toward the glass → stop |

**Hard 4, mall_b (after).** The upper floor on the left, stairs down to the exit, a crowd at the exit, the mall wall outside and the open parking on the right.

| Choice | Act-out |
|---|---|
| Safe: Walk down the stairs and out | Walk down the stairs (moving down as well as across) → out of the exit → to the parking sign |
| Take the elevator down | Walk to the elevator → `reach` → stop |
| Run and push to get out | Run-start into the crowd → stop behind the people |
| Wait by the mall wall outside | Down the stairs and out of the exit → walk toward the wall → stop |

**Hard 5, home_after (after).** Mom sits on the floor holding her leg; a broken pipe drips.

| Choice | Act-out |
|---|---|
| Safe: Call a grown-up for help | Crouch by Mom ("Stay still, Mom.") → stand → walk to the door → `call` ("Help!") → Dad walks in and kneels by Mom |
| Pull them up to walk | Walk to Mom → `reach` for her hand → stop |
| Fix the broken pipe yourself | Walk to the pipe → `reach` → stop |
| Leave them and go outside | Walk toward the door → stop |

**Hard 6, home_after (after).** The go bag by the door, where Normal 2 put it; paper and a pen on the table; Mom at the door.

| Choice | Act-out |
|---|---|
| Safe: Grab the go bag and walk out | `pack` at the table, writing the note (the note then shows on the table) → pick up the bag → `carry` it out of the door with Mom |
| Leave without the go bag | Walk to the door with empty hands → stop |
| Run back in for your toys | Run-start toward the inner room → stop |
| Leave without a note | Pick up the bag → `carry` it to the door → stop |

**Hard 7, evacuation_area (after).** The family's mat with a radio, neighbours chatting, and a path back to the houses.

| Choice | Act-out |
|---|---|
| Safe: Listen to the radio for news | Walk to the mat → `sit` beside Mom → Mom turns the radio on (two-frame sound waves) |
| Go home now to get things | Walk toward the path to the houses → stop |
| Share news you heard from others | Walk to the neighbours → `call`, hand by the mouth, whispering → stop |
| Call friends on the phone all day | `phone` → stop |

### New poses

For the boy and the girl, 32x50, in the v2 style, drawn by `student.lua` as new files only.

| Pose | Facing | Frames | Used in |
|---|---|---|---|
| `carry` (walking with something held in both arms) | side, mirrored | 4 | Normal 1, Normal 2, Hard 6 |
| `reach` (one arm out forward) | side, mirrored | 2 | Hard 3, Hard 4, Hard 5 |
| `reach-up` (on tiptoe, arms up) | side, mirrored | 2 | Normal 1, Hard 1 |
| `push` | side, mirrored | 2 | Normal 1 |
| `call` (hands at the mouth) | side, mirrored | 2 | Hard 5, Hard 7 |
| `cover` (kneeling low, arms over the head and neck) | side, mirrored | 2 | Easy 2, Normal 4 |
| `pack` (hands at table height; also writing) | front | 2 | Normal 2, Hard 6 |
| `phone` | front | 2 | Hard 1, Hard 7 |
| `sit` | front | 2 | Normal 5, Hard 7 |

### Grown-ups, props and icons

- **Teacher:** idle, wave, walk (side, 4 frames). **Mom:** idle, happy, walk, sit, hurt-sit (sitting and holding her leg). **Dad:** walk, kneel. All about 62 pixels tall, front view plus a mirrored side walk.
- **Props**, small sprites that show, hide or move with the Student's hands: box, books, picture, go bag (open and closed), water, can, flashlight, whistle, toy, snack, the park card, the house card, the note, the radio (2 frames) and fish (2-frame flop). Furniture that moves (the bed) is a prop too.
- **Icons:** one 16x16 icon per Choice, about 35 new ones, reusing `icon_door`, `icon_window`, `icon_desk` and `icon_cabinet` where a Choice matches.

## Rollout checks

### Earthquake Easy Editor check

- **Easy 2:** The researcher said the corridor floor looked like blocks and asked for more realistic stairs without the exposed bar. The revision uses a continuous concrete floor lit by the window, readable treads and risers, and a solid stairwell wall. The first tread at x 404 and the Act-out stop at x 394 stay in place. The build and all 174 EditMode and 35 PlayMode tests passed. The six updated Easy 2 screenshots were checked at both target sizes, and the other 93 matched byte for byte.
- **Easy 2 follow-up:** The researcher asked to correct the angle of the stairs. The flight now descends more gently from the same approach. The backs of the treads recede toward the centre of the corridor. The build, all 174 EditMode and 35 PlayMode tests, and the screenshot comparison passed again. Only the six Easy 2 screenshots changed.
- **Easy art follow-up:** The researcher asked to remove the block pattern from the floors in Easy 1 and 3 and improve the Student's Cover pose in Easy 2. Both classrooms now share a continuous concrete floor with window light. Both Students tuck their chins and cover the head and neck in the redrawn two-frame pose. The Scenario text, Act-out points and timing stay the same. The Unity build and all 174 EditMode and 35 PlayMode tests passed. All 24 changed screenshots were reviewed at both target sizes, and the other 75 matched byte for byte.
- **Pending:** Researcher review of the revised Easy art and any remaining playthrough observations.

### Earthquake Normal 1 art check

- **Action poses:** The researcher asked to improve the Student's action art before gameplay. `carry` now holds the load on level forearms and leans back, `reachup` raises both arms in front of the face on tiptoe, and `push` leans forward with flat palms. The frame counts stay the same.
- **Bedroom:** The researcher approved the bedroom after a final pass. The start spot is uncluttered, every prop stands on the back of the floor, no prop overlaps another, and the red ramp matches the shared palette. In the push stop, the Student steps with the sliding bed so the palms stay on the headboard. Placements and Act-out points are in `ArtSource/README-rollout.md` (Bedroom).
- **Checks:** The build, all 175 EditMode and 41 PlayMode tests passed. The six new Normal 1 screenshots were reviewed at both target sizes, and the other 99 matched byte for byte.

## Open questions

None. Q15 and the voice blip were settled in the rollout interview (R1, R2).

## Not verified

- No phone test of the new flow yet: the pilot was played in the Unity Editor only.
- The 50-pixel Student's readability on a 6-inch phone is assumed from the 4-units-per-art-pixel scale and still needs checking on the device.
